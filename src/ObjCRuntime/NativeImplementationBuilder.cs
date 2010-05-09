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
	internal abstract class NativeImplementationBuilder {
		internal static AssemblyBuilder builder;
		internal static ModuleBuilder module;

		private Delegate del;
				
		static NativeImplementationBuilder () {
			builder = AppDomain.CurrentDomain.DefineDynamicAssembly (new AssemblyName {Name = "ObjCImplementations"}, AssemblyBuilderAccess.Run, null, null, null,  null, null, true);
			module = builder.DefineDynamicModule ("Implementations", true);
		}

		internal abstract Delegate CreateDelegate ();

		internal IntPtr Selector {
			get; set;
		}

		internal Type [] ParameterTypes {
			get; set;
		}

		internal Delegate Delegate {
			get {
				if (del == null)
					del = CreateDelegate ();

				return del;
			}
		}
		
		internal Type DelegateType {
			get; set;
		}

		internal string Signature {
			get; set;
		}

		protected Type CreateDelegateType (Type return_type, Type [] argument_types) {
			TypeBuilder type = module.DefineType (Guid.NewGuid ().ToString (), TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeof (MulticastDelegate));
			type.SetCustomAttribute (new CustomAttributeBuilder (typeof (MarshalAsAttribute).GetConstructor (new Type [] { typeof (UnmanagedType) }), new object [] { UnmanagedType.FunctionPtr }));

			ConstructorBuilder constructor = type.DefineConstructor (MethodAttributes.Public, CallingConventions.Standard, new Type [] { typeof (object), typeof (int) });

			constructor.SetImplementationFlags (MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

			MethodBuilder method = null;

			method = type.DefineMethod ("Invoke", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, return_type, argument_types);

			if (NeedsCustomMarshaler (return_type))
				SetupParameter (method, 0, return_type);

			for (int i = 1; i <= argument_types.Length; i++)
				if (NeedsCustomMarshaler (argument_types [i - 1]))
					SetupParameter (method, i, argument_types [i - 1]);
			
			method.SetImplementationFlags (MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

			return type.CreateType ();
		}

		private bool NeedsCustomMarshaler (Type t) {
			if (t == typeof (NSObject) || t.IsSubclassOf (typeof (NSObject)))
				return true;
			if (t == typeof (Selector))
				return true;

			return false;
		}

		private Type MarshalerForType (Type t) {
			if (t == typeof (NSObject) || t.IsSubclassOf (typeof (NSObject)))
				return typeof (NSObjectMarshaler);
			if (t == typeof (Selector))
				return typeof (SelectorMarshaler);

			throw new ArgumentException ("Cannot determine marshaler type for: " + t);
		}

		private void SetupParameter (MethodBuilder builder, int index, Type t) {
			ParameterBuilder pb = builder.DefineParameter (index, ParameterAttributes.HasFieldMarshal, string.Format ("arg{0}", index));
			ConstructorInfo cinfo = typeof (MarshalAsAttribute).GetConstructor (new Type[] { typeof (UnmanagedType) });
			FieldInfo mtrfld = typeof (MarshalAsAttribute).GetField ("MarshalTypeRef");
			CustomAttributeBuilder cabuilder = new CustomAttributeBuilder (cinfo, new object [] { UnmanagedType.CustomMarshaler }, new FieldInfo [] { mtrfld }, new object [] { MarshalerForType (t) });

			pb.SetCustomAttribute (cabuilder);
		}
	}
}
