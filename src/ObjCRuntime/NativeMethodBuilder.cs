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
		private static MethodInfo creatensstring = typeof (NSString).GetMethod ("op_Explicit", new Type [] { typeof (string) });
#if !MONOMAC_BOOTSTRAP
		private static MethodInfo convertstruct = typeof (Marshal).GetMethod ("StructureToPtr", new Type [] { typeof (object), typeof (IntPtr), typeof (bool) });
		private static MethodInfo buildarray = typeof (NSArray).GetMethod ("FromNSObjects", new Type [] { typeof (NSObject []) });
		private static MethodInfo buildsarray = typeof (NSArray).GetMethod ("FromStrings", new Type [] { typeof (string[]) });
#endif

		private MethodInfo minfo;
		private Type type;
		private Type rettype;
		private bool isstret;
				
		internal NativeMethodBuilder (MethodInfo minfo) : this (minfo, minfo.DeclaringType, (ExportAttribute) Attribute.GetCustomAttribute (minfo.GetBaseDefinition (), typeof (ExportAttribute))) {}

		internal NativeMethodBuilder (MethodInfo minfo, Type type, ExportAttribute ea) {
			if (ea == null)
				throw new ArgumentException ("MethodInfo does not have a export attribute");

			if (minfo.DeclaringType.IsGenericType)
				throw new ArgumentException ("MethodInfo cannot be in a generic type");

			Parameters = minfo.GetParameters ();

			rettype = ConvertReturnType (minfo.ReturnType);

			// FIXME: We should detect if this is in a bound assembly or not and only alloc if needed
			Selector = new Selector (ea.Selector ?? minfo.Name, true).Handle;
			Signature = string.Format ("{0}@:", TypeConverter.ToNative (minfo.ReturnType));

			ConvertParameters (Parameters, minfo.IsStatic, isstret);
			
			DelegateType = CreateDelegateType (rettype, ParameterTypes);

			this.minfo = minfo;
			this.type = type;
		}

		internal override Delegate CreateDelegate () {
			DynamicMethod method = new DynamicMethod (string.Format ("[{0}:{1}]", minfo.DeclaringType, minfo), rettype, ParameterTypes, module, true);
			ILGenerator il = method.GetILGenerator ();

			DeclareLocals (il);
			ConvertArguments (il, 0);
#if DUMP_CALLS
			il.Emit (OpCodes.Ldstr, string.Format ("Invoking {0} on a {1}", minfo.ToString (), type.ToString ()));
			il.Emit (OpCodes.Call, typeof (Console).GetMethod ("WriteLine", new Type [] { typeof (string) }));
#endif

			if (!minfo.IsStatic) {
				il.Emit (OpCodes.Ldarg, (isstret ? 1 : 0));
				il.Emit (OpCodes.Castclass, type);
			}

			LoadArguments (il, 0);

			if (minfo.IsVirtual)
				il.Emit (OpCodes.Callvirt, minfo);
			else
				il.Emit (OpCodes.Call, minfo);

			UpdateByRefArguments (il, 0);

			if (minfo.ReturnType == typeof (string)) {
				il.Emit (OpCodes.Call, creatensstring);
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
