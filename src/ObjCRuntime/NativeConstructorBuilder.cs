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
	internal class NativeConstructorBuilder : NativeImplementationBuilder {
		static MethodInfo trygetnsobject;
		static MethodInfo newobject;
		static MethodInfo gettype;
#if !MONOMAC_BOOTSTRAP
		static MethodInfo getobject;
		static MethodInfo convertarray;
#endif
		static FieldInfo handlefld;
		static FieldInfo valuefld;
		static IntPtr selInit = MonoMac.ObjCRuntime.Selector.GetHandle ("init");
		private ParameterInfo [] parms;

		private ConstructorInfo cinfo;
				
		static NativeConstructorBuilder () {
			trygetnsobject = typeof (Runtime).GetMethod ("TryGetNSObject", BindingFlags.Public | BindingFlags.Static);
			newobject = typeof (System.Runtime.Serialization.FormatterServices).GetMethod ("GetUninitializedObject", BindingFlags.Public | BindingFlags.Static);
			gettype = typeof (System.Type).GetMethod ("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static);
			handlefld = typeof (NSObject).GetField ("handle", BindingFlags.NonPublic | BindingFlags.Instance);
			valuefld = typeof (RuntimeTypeHandle).GetField ("value", BindingFlags.NonPublic | BindingFlags.Instance);
#if !MONOMAC_BOOTSTRAP
			getobject = typeof (Runtime).GetMethod ("GetNSObject", BindingFlags.Static | BindingFlags.Public);
			convertarray = typeof (NSArray).GetMethod ("ArrayFromHandle", new Type [] { typeof (IntPtr) });
#endif
		}

		internal NativeConstructorBuilder (ConstructorInfo cinfo) {
			ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (cinfo, typeof (ExportAttribute));

			parms = cinfo.GetParameters ();

			if (ea == null && parms.Length > 0)
				throw new ArgumentException ("ConstructorInfo does not have a export attribute");

			if (ea == null)
				Selector = selInit;
			else
				Selector = new Selector (ea.Selector, true).Handle;

			Signature = "@@:";
			ParameterTypes = new Type [2 + parms.Length];

			ParameterTypes [0] = typeof (IntPtr);
			ParameterTypes [1] = typeof (Selector);

			for (int i = 0; i < parms.Length; i++) {
				if (parms [i].ParameterType.IsByRef && (parms[i].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject)) || parms[i].ParameterType.GetElementType () == typeof (NSObject)))
					ParameterTypes [i + 2] = typeof (IntPtr).MakeByRefType ();
				else if (parms [i].ParameterType.IsArray && (parms [i].ParameterType.GetElementType () == typeof (NSObject) || parms [i].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject))))
					ParameterTypes [i + 2] = typeof (IntPtr);
				else
					ParameterTypes [i + 2] = parms [i].ParameterType;
				Signature += TypeConverter.ToNative (parms [i].ParameterType);
			}
			
			DelegateType = CreateDelegateType (typeof (IntPtr), ParameterTypes);

			this.cinfo = cinfo;
		}

		internal override Delegate CreateDelegate () {
			DynamicMethod method = new DynamicMethod (Guid.NewGuid ().ToString (), typeof (IntPtr), ParameterTypes, module, true);
			ILGenerator il = method.GetILGenerator ();
			Label done = il.DefineLabel ();
			
			il.DeclareLocal (typeof (object));

			for (int i = 0; i < parms.Length; i++) {
				if (parms [i].ParameterType.IsByRef && (parms[i].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject)) || parms[i].ParameterType.GetElementType () == typeof (NSObject))) {
					il.DeclareLocal (parms [i].ParameterType.GetElementType ());
				}
			}

			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Call, trygetnsobject);
			il.Emit (OpCodes.Brtrue, done);

			il.Emit (OpCodes.Ldtoken, cinfo.DeclaringType);
			il.Emit (OpCodes.Call, gettype);
			il.Emit (OpCodes.Call, newobject);
			il.Emit (OpCodes.Stloc_0);
			il.Emit (OpCodes.Ldloc_0);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Stfld, handlefld);

#if !MONOMAC_BOOTSTRAP
			for (int i = 2, j = 0; i < ParameterTypes.Length; i++) {
				if (parms [i-2].ParameterType.IsByRef && (parms[i-2].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject)) || parms[i-2].ParameterType.GetElementType () == typeof (NSObject))) {
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Ldind_I);
					il.Emit (OpCodes.Call, getobject);
					il.Emit (OpCodes.Stloc, j+1);
					j++;
				} else if (parms [i-2].ParameterType.IsArray && (parms [i-2].ParameterType.GetElementType () == typeof (NSObject) || parms [i-2].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject)))) {
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Call, convertarray.MakeGenericMethod (parms [i-2].ParameterType.GetElementType ()));
				}
			}
#endif
			il.Emit (OpCodes.Ldloc_0);
			il.Emit (OpCodes.Castclass, cinfo.DeclaringType);

			for (int i = 2, j = 0; i < ParameterTypes.Length; i++) {
				if (parms [i-2].ParameterType.IsByRef && (parms[i-2].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject)) || parms[i-2].ParameterType.GetElementType () == typeof (NSObject))) {
					il.Emit (OpCodes.Ldloca_S, j+1);
					j++;
				} else {
					il.Emit (OpCodes.Ldarg, i);
				}
			}

			il.Emit (OpCodes.Call, cinfo);

			il.MarkLabel (done);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ret);

			return method.CreateDelegate (DelegateType);
		}
	}
}
