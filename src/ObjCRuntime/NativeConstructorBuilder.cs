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
		private static MethodInfo trygetnsobject = typeof (Runtime).GetMethod ("TryGetNSObject", BindingFlags.Public | BindingFlags.Static);
		private static MethodInfo newobject = typeof (System.Runtime.Serialization.FormatterServices).GetMethod ("GetUninitializedObject", BindingFlags.Public | BindingFlags.Static);
		private static MethodInfo gettype = typeof (System.Type).GetMethod ("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static);
		private static MethodInfo retain = typeof (NSObject).GetMethod ("Retain", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		private static FieldInfo handlefld = typeof (NSObject).GetField ("handle", BindingFlags.NonPublic | BindingFlags.Instance);
		private static FieldInfo valuefld = typeof (RuntimeTypeHandle).GetField ("value", BindingFlags.NonPublic | BindingFlags.Instance);
		static IntPtr selInit = MonoMac.ObjCRuntime.Selector.GetHandle ("init");

		private ConstructorInfo cinfo;
				
		internal NativeConstructorBuilder (ConstructorInfo cinfo) {
			ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (cinfo, typeof (ExportAttribute));

			Parameters = cinfo.GetParameters ();

			if (ea == null && Parameters.Length > 0)
				throw new ArgumentException ("ConstructorInfo does not have a export attribute");

			if (ea == null)
				Selector = selInit;
			else
				Selector = new Selector (ea.Selector, true).Handle;

			Signature = "@@:";
			ConvertParameters (Parameters, true, false);
			DelegateType = CreateDelegateType (typeof (IntPtr), ParameterTypes);

			this.cinfo = cinfo;
		}

		internal override Delegate CreateDelegate () {
			DynamicMethod method = new DynamicMethod (Guid.NewGuid ().ToString (), typeof (IntPtr), ParameterTypes, module, true);
			ILGenerator il = method.GetILGenerator ();
			Label done = il.DefineLabel ();
			
			il.DeclareLocal (typeof (object));
			DeclareLocals (il);

			for (int i = 0; i < Parameters.Length; i++) {
				if (Parameters [i].ParameterType.IsByRef && (Parameters[i].ParameterType.GetElementType ().IsSubclassOf (typeof (NSObject)) || Parameters[i].ParameterType.GetElementType () == typeof (NSObject))) {
					il.DeclareLocal (Parameters [i].ParameterType.GetElementType ());
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

			ConvertArguments (il, 1);

			il.Emit (OpCodes.Ldloc_0);
			il.Emit (OpCodes.Castclass, cinfo.DeclaringType);

			LoadArguments (il, 1);

			il.Emit (OpCodes.Call, cinfo);

			UpdateByRefArguments (il, 1);

			il.Emit (OpCodes.Ldloc_0);
			il.Emit (OpCodes.Call, retain);
			il.Emit (OpCodes.Pop);

			il.MarkLabel (done);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ret);

			return method.CreateDelegate (DelegateType);
		}
	}
}
