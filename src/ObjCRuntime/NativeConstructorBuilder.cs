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
		static FieldInfo handlefld;
		static FieldInfo valuefld;
		static IntPtr selInit = MonoMac.ObjCRuntime.Selector.GetHandle ("init");

		private ConstructorInfo cinfo;
				
		static NativeConstructorBuilder () {
			trygetnsobject = typeof (Runtime).GetMethod ("TryGetNSObject", BindingFlags.Public | BindingFlags.Static);
			newobject = typeof (System.Runtime.Serialization.FormatterServices).GetMethod ("GetUninitializedObject", BindingFlags.Public | BindingFlags.Static);
			gettype = typeof (System.Type).GetMethod ("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static);
			handlefld = typeof (NSObject).GetField ("handle", BindingFlags.NonPublic | BindingFlags.Instance);
			valuefld = typeof (RuntimeTypeHandle).GetField ("value", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		internal NativeConstructorBuilder (ConstructorInfo cinfo) {
			ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (cinfo, typeof (ExportAttribute));
			ParameterInfo [] parms = cinfo.GetParameters ();

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

			il.Emit (OpCodes.Ldloc_0);
			for (int i = 2; i < ParameterTypes.Length; i++) {
				il.Emit (OpCodes.Ldarg, i);
			}
			il.Emit (OpCodes.Call, cinfo);

			il.MarkLabel (done);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ret);

			return method.CreateDelegate (DelegateType);
		}
	}
}
