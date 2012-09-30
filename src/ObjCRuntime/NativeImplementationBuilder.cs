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

#if !MONOMAC_BOOTSTRAP
		private static MethodInfo convertarray = typeof (NSArray).GetMethod ("ArrayFromHandle", new Type [] { typeof (IntPtr) });
		private static MethodInfo convertsarray = typeof (NSArray).GetMethod ("StringArrayFromHandle", new Type [] { typeof (IntPtr) });
		private static MethodInfo convertstring = typeof (NSString).GetMethod ("ToString", Type.EmptyTypes);
		private static MethodInfo getobject = typeof (Runtime).GetMethod ("GetNSObject", BindingFlags.Static | BindingFlags.Public);
		private static MethodInfo gethandle = typeof (NSObject).GetMethod ("get_Handle", BindingFlags.Instance | BindingFlags.Public);
		private static FieldInfo intptrzero = typeof (IntPtr).GetField ("Zero", BindingFlags.Static | BindingFlags.Public);
#endif

		private Delegate del;
				
		static NativeImplementationBuilder () {
			builder = AppDomain.CurrentDomain.DefineDynamicAssembly (new AssemblyName {Name = "ObjCImplementations"}, AssemblyBuilderAccess.Run, null, null, null,  null, null, true);
			module = builder.DefineDynamicModule ("Implementations", false);
		}

		internal abstract Delegate CreateDelegate ();

		internal int ArgumentOffset {
			get; set;
		}

		internal IntPtr Selector {
			get; set;
		}

		internal Type [] ParameterTypes {
			get; set;
		}

		internal ParameterInfo [] Parameters {
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
				return typeof (NSObjectMarshaler<>).MakeGenericType (t);
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

		protected bool IsWrappedType (Type type) {
			if (type == typeof (NSObject) || type.IsSubclassOf (typeof (NSObject)) || type == typeof (string))
				return true;

			return false;
		}

		protected void ConvertParameters (ParameterInfo [] parms, bool isstatic, bool isstret) {
			if (isstret) {
				ArgumentOffset = 3;
				ParameterTypes = new Type [ArgumentOffset + parms.Length];
				ParameterTypes [0] = typeof (IntPtr);
				ParameterTypes [1] = isstatic ? typeof (IntPtr) : typeof (NSObject);
				ParameterTypes [2] = typeof (Selector);
			} else {
				ArgumentOffset = 2;
				ParameterTypes = new Type [ArgumentOffset + parms.Length];
				ParameterTypes [0] = isstatic ? typeof (IntPtr) : typeof (NSObject);
				ParameterTypes [1] = typeof (Selector);
			}

			for (int i = 0; i < Parameters.Length; i++) {
				if (Parameters [i].ParameterType.IsByRef && IsWrappedType (Parameters [i].ParameterType.GetElementType ()))
					ParameterTypes [i + ArgumentOffset] = typeof (IntPtr).MakeByRefType ();
				else if (Parameters [i].ParameterType.IsArray && IsWrappedType (Parameters [i].ParameterType.GetElementType ()))
					ParameterTypes [i + ArgumentOffset] = typeof (IntPtr);
				else if (typeof (INativeObject).IsAssignableFrom (Parameters [i].ParameterType) && !IsWrappedType (Parameters [i].ParameterType))
					ParameterTypes [i + ArgumentOffset] = typeof (IntPtr);
				else if (Parameters [i].ParameterType == typeof (string))
					ParameterTypes [i + ArgumentOffset] = typeof (NSString);
				else
					ParameterTypes [i + ArgumentOffset] = Parameters [i].ParameterType;
				// The TypeConverter will emit a ^@ for a byref type that is a NSObject or NSObject subclass in this case
				// If we passed the ParameterTypes [i+ArgumentOffset] as would be more logical we would emit a ^^v for that case, which
				// while currently acceptible isn't representative of what obj-c wants.
				Signature += TypeConverter.ToNative (Parameters [i].ParameterType);
			}
		}

		protected void DeclareLocals (ILGenerator il) {
			// Keep in sync with UpdateByRefArguments()
			for (int i = 0; i < Parameters.Length; i++) {
				if (Parameters [i].ParameterType.IsByRef && IsWrappedType (Parameters [i].ParameterType.GetElementType ())) {
					il.DeclareLocal (Parameters [i].ParameterType.GetElementType ());
				} else if (Parameters [i].ParameterType.IsArray && IsWrappedType (Parameters [i].ParameterType.GetElementType ())) {
					il.DeclareLocal (Parameters [i].ParameterType);
				} else if (Parameters [i].ParameterType == typeof (string)) {
					il.DeclareLocal (typeof (string));
				}
			}
		}


		protected void ConvertArguments (ILGenerator il, int locoffset) {
#if !MONOMAC_BOOTSTRAP
			for (int i = ArgumentOffset, j = 0; i < ParameterTypes.Length; i++) {
				if (Parameters [i-ArgumentOffset].ParameterType.IsByRef && (Attribute.GetCustomAttribute (Parameters [i-ArgumentOffset], typeof (OutAttribute)) == null) && IsWrappedType (Parameters[i-ArgumentOffset].ParameterType.GetElementType ())) {
					var nullout = il.DefineLabel ();
					var done = il.DefineLabel ();
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Brfalse, nullout);
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Ldind_I);
					il.Emit (OpCodes.Call, getobject);
					il.Emit (OpCodes.Br, done);
					il.MarkLabel (nullout);
					il.Emit (OpCodes.Ldnull);
					il.MarkLabel (done);
					il.Emit (OpCodes.Stloc, j+locoffset);
					j++;
				} else if (Parameters [i-ArgumentOffset].ParameterType.IsArray && IsWrappedType (Parameters [i-ArgumentOffset].ParameterType.GetElementType ())) {
					var nullout = il.DefineLabel ();
					var done = il.DefineLabel ();
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Brfalse, nullout);
					il.Emit (OpCodes.Ldarg, i);
					if (Parameters [i-ArgumentOffset].ParameterType.GetElementType () == typeof (string))
						il.Emit (OpCodes.Call, convertsarray);
					else
						il.Emit (OpCodes.Call, convertarray.MakeGenericMethod (Parameters [i-ArgumentOffset].ParameterType.GetElementType ()));
					il.Emit (OpCodes.Br, done);
					il.MarkLabel (nullout);
					il.Emit (OpCodes.Ldnull);
					il.MarkLabel (done);
					il.Emit (OpCodes.Stloc, j+locoffset);
					j++;
				} else if (Parameters [i-ArgumentOffset].ParameterType == typeof (string)) {
					var nullout = il.DefineLabel ();
					var done = il.DefineLabel ();
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Brfalse, nullout);
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Call, convertstring);
					il.Emit (OpCodes.Br, done);
					il.MarkLabel (nullout);
					il.Emit (OpCodes.Ldnull);
					il.MarkLabel (done);
					il.Emit (OpCodes.Stloc, j+locoffset);
					j++;
				}
			}
#endif
		}

		protected void LoadArguments (ILGenerator il, int locoffset) {
			for (int i = ArgumentOffset, j = 0; i < ParameterTypes.Length; i++) {
				if (Parameters [i-ArgumentOffset].ParameterType.IsByRef && IsWrappedType (Parameters[i-ArgumentOffset].ParameterType.GetElementType ())) {
					il.Emit (OpCodes.Ldloca_S, j+locoffset);
					j++;
				} else if (Parameters [i-ArgumentOffset].ParameterType.IsArray && IsWrappedType (Parameters [i-ArgumentOffset].ParameterType.GetElementType ())) {
					il.Emit (OpCodes.Ldloc, j+locoffset);
					j++;
				} else if (typeof (INativeObject).IsAssignableFrom (Parameters [i-ArgumentOffset].ParameterType) && !IsWrappedType (Parameters [i-ArgumentOffset].ParameterType)) {
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Newobj, Parameters [i-ArgumentOffset].ParameterType.GetConstructor (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type [] { typeof (IntPtr) }, null));
				} else if (Parameters [i-ArgumentOffset].ParameterType == typeof (string)) {
					il.Emit (OpCodes.Ldloc, j+locoffset);
					j++;
				} else {
					il.Emit (OpCodes.Ldarg, i);
				}
			}
		}

		protected void UpdateByRefArguments (ILGenerator il, int locoffset) {
#if !MONOMAC_BOOTSTRAP
			// Keep in sync with DeclareLocals()
			for (int i = ArgumentOffset, j = 0; i < ParameterTypes.Length; i++) {
				if (Parameters [i-ArgumentOffset].ParameterType.IsByRef && IsWrappedType (Parameters[i-ArgumentOffset].ParameterType.GetElementType ())) {
					Label nullout = il.DefineLabel ();
					Label done = il.DefineLabel ();
					il.Emit (OpCodes.Ldloc, j+locoffset);
					il.Emit (OpCodes.Brfalse, nullout);
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Ldloc, j+locoffset);
					il.Emit (OpCodes.Call, gethandle);
					il.Emit (OpCodes.Stind_I);
					il.Emit (OpCodes.Br, done);
					il.MarkLabel (nullout);
					il.Emit (OpCodes.Ldarg, i);
					il.Emit (OpCodes.Ldsfld, intptrzero);
					il.Emit (OpCodes.Stind_I);
					il.MarkLabel (done);
					j++;
				} else if (Parameters [i-ArgumentOffset].ParameterType.IsArray && IsWrappedType (Parameters [i-ArgumentOffset].ParameterType.GetElementType ())) {
					j++;
				} else if (Parameters [i-ArgumentOffset].ParameterType == typeof (string)) {
					j++;
				}
			}
#endif
		}
	}
}
