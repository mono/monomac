//
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.Foundation;

namespace MonoMac.ObjCRuntime {
	internal class NativeMethodBuilder : NativeImplementationBuilder {
		private static ConstructorInfo newnsstring = typeof (NSString).GetConstructor (new Type [] { typeof (string) });
#if !MONOMAC_BOOTSTRAP
		private static MethodInfo convertstruct = typeof (Marshal).GetMethod ("StructureToPtr", new Type [] { typeof (object), typeof (IntPtr), typeof (bool) });
		private static MethodInfo convertarray = typeof (NSArray).GetMethod ("ArrayFromHandle", new Type [] { typeof (IntPtr) });
		private static MethodInfo convertsarray = typeof (NSArray).GetMethod ("StringArrayFromHandle", new Type [] { typeof (IntPtr) });
		private static MethodInfo buildarray = typeof (NSArray).GetMethod ("FromNSObjects", BindingFlags.Static | BindingFlags.Public);
		private static MethodInfo buildsarray = typeof (NSArray).GetMethod ("FromStrings", BindingFlags.Static | BindingFlags.Public);
		private static MethodInfo getobject = typeof (Runtime).GetMethod ("GetNSObject", BindingFlags.Static | BindingFlags.Public);
		private static MethodInfo gethandle = typeof (NSObject).GetMethod ("get_Handle", BindingFlags.Instance | BindingFlags.Public);
#endif

		private MethodInfo minfo;
		private Type type;
		private Type rettype;
		private ParameterInfo [] parms;
		private bool isstret;
		private int argoffset;
				
		internal NativeMethodBuilder (MethodInfo minfo) : this (minfo, minfo.DeclaringType, (ExportAttribute) Attribute.GetCustomAttribute (minfo.GetBaseDefinition (), typeof (ExportAttribute))) {}

		internal NativeMethodBuilder (MethodInfo minfo, Type type, ExportAttribute ea) {
			if (ea == null)
				throw new ArgumentException ("MethodInfo does not have a export attribute");

			if (minfo.DeclaringType.IsGenericType)
				throw new ArgumentException ("MethodInfo cannot be in a generic type");

			parms = minfo.GetParameters ();

			rettype = ConvertReturnType (minfo.ReturnType);

			// FIXME: We should detect if this is in a bound assembly or not and only alloc if needed
			Selector = new Selector (ea.Selector ?? minfo.Name, true).Handle;
			Signature = string.Format ("{0}@:", TypeConverter.ToNative (rettype));

			if (isstret) {
				argoffset = 3;
				ParameterTypes = new Type [argoffset + parms.Length];
				ParameterTypes [0] = typeof (IntPtr);
				ParameterTypes [1] = minfo.IsStatic ? typeof (IntPtr) : typeof (NSObject);
				ParameterTypes [2] = typeof (Selector);
			} else {
				argoffset = 2;
				ParameterTypes = new Type [argoffset + parms.Length];
				ParameterTypes [0] = minfo.IsStatic ? typeof (IntPtr) : typeof (NSObject);
				ParameterTypes [1] = typeof (Selector);
			}

			for (int i = 0; i < parms.Length; i++) {
				if (parms [i].ParameterType.IsByRef && IsWrappedType (parms [i].ParameterType.GetElementType ()))
					ParameterTypes [i + argoffset] = typeof (IntPtr).MakeByRefType ();
				else if (parms [i].ParameterType.IsArray && IsWrappedType (parms [i].ParameterType.GetElementType ()))
					ParameterTypes [i + argoffset] = typeof (IntPtr);
				else if (typeof (INativeObject).IsAssignableFrom (parms [i].ParameterType) && !IsWrappedType (parms [i].ParameterType))
					ParameterTypes [i + argoffset] = typeof (IntPtr);
				else
					ParameterTypes [i + argoffset] = parms [i].ParameterType;
				// The TypeConverter will emit a ^@ for a byref type that is a NSObject or NSObject subclass in this case
				// If we passed the ParameterTypes [i+argoffset] as would be more logical we would emit a ^^v for that case, which
				// while currently acceptible isn't representative of what obj-c wants.
				Signature += TypeConverter.ToNative (parms [i].ParameterType);
			}
			
			DelegateType = CreateDelegateType (rettype, ParameterTypes);

			this.minfo = minfo;
			this.type = type;
		}

		internal override Delegate CreateDelegate () {
			DynamicMethod method = new DynamicMethod (string.Format ("[{0}:{1}]", minfo.DeclaringType, minfo), rettype, ParameterTypes, module, true);
			ILGenerator il = method.GetILGenerator ();
			
			for (int i = 0; i < parms.Length; i++) {
				if (parms [i].ParameterType.IsByRef && IsWrappedType (parms [i].ParameterType.GetElementType ())) {
					il.DeclareLocal (parms [i].ParameterType.GetElementType ());
				} else if (parms [i].ParameterType.IsArray && IsWrappedType (parms [i].ParameterType.GetElementType ())) {
					il.DeclareLocal (parms [i].ParameterType);
				}
			}

#if !MONOMAC_BOOTSTRAP
			for (int i = argoffset, j = 0; i < ParameterTypes.Length; i++) {
				if (parms [i-argoffset].ParameterType.IsByRef && IsWrappedType (parms[i-argoffset].ParameterType.GetElementType ())) {
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Ldind_I);
					il.Emit (OpCodes.Call, getobject);
					il.Emit (OpCodes.Stloc, j++);
				} else if (parms [i-argoffset].ParameterType.IsArray && IsWrappedType (parms [i-argoffset].ParameterType.GetElementType ())) {
					il.Emit (OpCodes.Ldarg, i);
					if (parms [i-argoffset].ParameterType.GetElementType () == typeof (string))
						il.Emit (OpCodes.Call, convertsarray);
					else
						il.Emit (OpCodes.Call, convertarray.MakeGenericMethod (parms [i-argoffset].ParameterType.GetElementType ()));
					il.Emit (OpCodes.Stloc, j++);
				}
			}
#endif

#if DUMP_CALLS
			il.Emit (OpCodes.Ldstr, string.Format ("Invoking {0} on a {1}", minfo.ToString (), type.ToString ()));
			il.Emit (OpCodes.Call, typeof (Console).GetMethod ("WriteLine", new Type [] { typeof (string) }));
#endif

			if (!minfo.IsStatic) {
				il.Emit (OpCodes.Ldarg, (isstret ? 1 : 0));
				il.Emit (OpCodes.Castclass, type);
			}

			for (int i = argoffset, j = 0; i < ParameterTypes.Length; i++) {
				if (parms [i-argoffset].ParameterType.IsByRef && IsWrappedType (parms[i-argoffset].ParameterType.GetElementType ()))
					il.Emit (OpCodes.Ldloca_S, j++);
				else if (parms [i-argoffset].ParameterType.IsArray && IsWrappedType (parms [i-argoffset].ParameterType.GetElementType ()))
					il.Emit (OpCodes.Ldloc, j++);
				else if (typeof (INativeObject).IsAssignableFrom (parms [i-argoffset].ParameterType) && !IsWrappedType (parms [i-argoffset].ParameterType)) {
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Newobj, parms [i-argoffset].ParameterType.GetConstructor (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type [] { typeof (IntPtr) }, null));
				} else
					il.Emit (OpCodes.Ldarg, i);
			}

			if (minfo.IsVirtual)
				il.Emit (OpCodes.Callvirt, minfo);
			else
				il.Emit (OpCodes.Call, minfo);

#if !MONOMAC_BOOTSTRAP
			for (int i = argoffset, j = 0; i < ParameterTypes.Length; i++) {
				if (parms [i-argoffset].ParameterType.IsByRef && IsWrappedType (parms[i-argoffset].ParameterType.GetElementType ())) {
					Label done = il.DefineLabel ();
					il.Emit (OpCodes.Ldloc, j);
					il.Emit (OpCodes.Brfalse, done);
					il.Emit (OpCodes.Ldloc, j++);
					il.Emit (OpCodes.Call, gethandle);
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Stind_I);
					il.MarkLabel (done);
				}
			}
#endif
			if (minfo.ReturnType == typeof (string)) {
				il.Emit (OpCodes.Newobj, newnsstring);
#if !MONOMAC_BOOTSTRAP
			} else if (minfo.ReturnType.IsArray && IsWrappedType (minfo.ReturnType.GetElementType ())) {
				if (minfo.ReturnType.GetElementType () == typeof (string))
					il.Emit (OpCodes.Call, buildsarray);
				else
					il.Emit (OpCodes.Call, buildarray);
			} else if (typeof (INativeObject).IsAssignableFrom (minfo.ReturnType) && !IsWrappedType (minfo.ReturnType)) {
				il.Emit (OpCodes.Call, minfo.ReturnType.GetProperty ("Handle").GetGetMethod ());
			} else if (isstret) {
				il.Emit (OpCodes.Box, minfo.ReturnType);
				il.Emit (OpCodes.Ldarg, 0);
				il.Emit (OpCodes.Ldc_I4, 0);
				il.Emit (OpCodes.Call, convertstruct);
#endif
			}
			il.Emit (OpCodes.Ret);

			return method.CreateDelegate (DelegateType);
		}

		private bool IsWrappedType (Type type) {
			if (type == typeof (NSObject) || type.IsSubclassOf (typeof (NSObject)) || type == typeof (string))
				return true;

			return false;
		}

		private Type ConvertReturnType (Type type) {
			if (type.IsValueType && !type.IsEnum && type.Assembly != typeof (object).Assembly && Marshal.SizeOf (type) > 8) {
				isstret = true;
				return typeof (void);
			}

			if (type == typeof (string))
				return typeof (NSString);
#if !MONOMAC_BOOTSTRAP
			if (type.IsArray && IsWrappedType (type.GetElementType ()))
				return typeof (NSArray);
#endif
			if (typeof (INativeObject).IsAssignableFrom (type) && !IsWrappedType (type))
				return typeof (IntPtr);

			return type;
		}
	}
}
