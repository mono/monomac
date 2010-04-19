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
		private static MethodInfo buildarray = typeof (NSArray).GetMethod ("FromNSObjects", BindingFlags.Static | BindingFlags.Public);
#endif

		private MethodInfo minfo;
		private Type rettype;
				
		internal NativeMethodBuilder (MethodInfo minfo) {
			ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (minfo.GetBaseDefinition (), typeof (ExportAttribute));
			ParameterInfo [] parms = minfo.GetParameters ();

			if (ea == null)
				throw new ArgumentException ("MethodInfo does not have a export attribute");

			rettype = ConvertReturnType (minfo.ReturnType);
			// FIXME: We should detect if this is in a bound assembly or not and only alloc if needed
			Selector = new Selector (ea.Selector ?? minfo.Name, true).Handle;
			Signature = string.Format ("{0}@:", TypeConverter.ToNative (rettype));
			ParameterTypes = new Type [2 + parms.Length];

			ParameterTypes [0] = typeof (NSObject);
			ParameterTypes [1] = typeof (Selector);

			for (int i = 0; i < parms.Length; i++) {
				ParameterTypes [i + 2] = parms [i].ParameterType;
				Signature += TypeConverter.ToNative (parms [i].ParameterType);
			}
			
			DelegateType = CreateDelegateType (rettype, ParameterTypes);

			this.minfo = minfo;
		}

		internal override Delegate CreateDelegate () {
			DynamicMethod method = new DynamicMethod (Guid.NewGuid ().ToString (), rettype, ParameterTypes, module, true);
			ILGenerator il = method.GetILGenerator ();
			
			if (!minfo.IsStatic)
				il.Emit (OpCodes.Ldarg_0);

			for (int i = 2; i < ParameterTypes.Length; i++) {
				il.Emit (OpCodes.Ldarg, i);
			}
	
			il.Emit (OpCodes.Call, minfo);

			if (rettype == typeof (string)) {
				il.Emit (OpCodes.Newobj, newnsstring);
#if !MONOMAC_BOOTSTRAP
			} else if (rettype.IsArray && (rettype.GetElementType () == typeof (NSObject) || rettype.GetElementType ().IsSubclassOf (typeof (NSObject)))) {
				il.Emit (OpCodes.Call, buildarray);
#endif
			}
			il.Emit (OpCodes.Ret);

			return method.CreateDelegate (DelegateType);
		}

		private Type ConvertReturnType (Type type) {
			if (type == typeof (string))
				return typeof (NSString);
#if !MONOMAC_BOOTSTRAP
			if (type.IsArray && (type.GetElementType () == typeof (NSObject) || type.GetElementType ().IsSubclassOf (typeof (NSObject))))
				return typeof (NSArray);
#endif

			return type;
		}
	}
}
