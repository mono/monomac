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

//
// Class.cs
//
// Copyright 2009 Novell, Inc
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.Foundation;

namespace MonoMac.ObjCRuntime {
	 public class Class : INativeObject {
#if OBJECT_REF_TRACKING
		static NativeMethodBuilder release_builder = new NativeMethodBuilder (typeof (NSObject).GetMethod ("Release", BindingFlags.NonPublic | BindingFlags.Instance));
		static NativeMethodBuilder retain_builder = new NativeMethodBuilder (typeof (NSObject).GetMethod ("Retain", BindingFlags.NonPublic | BindingFlags.Instance));
#endif
		static Dictionary <IntPtr, Type> type_map = new Dictionary <IntPtr, Type> ();
		static List <Delegate> method_wrappers = new List <Delegate> ();

		internal IntPtr handle;

		public Class (string name) {
			this.handle = objc_getClass (name);
			
			if (this.handle == IntPtr.Zero)
				throw new ArgumentException ("name is an unknown class", name);
		}

		public Class (Type type) {
			this.handle = Class.Register (type);
		}

		public Class (IntPtr handle) {
			this.handle = handle;
		}

		internal static Class Construct (IntPtr handle) {
			return new Class (handle);
		}

		public IntPtr Handle {
			get { return this.handle; }
		}

		public IntPtr SuperClass {
			get { return class_getSuperclass (handle); }
		}

		public string Name {
			get {
				IntPtr ptr = class_getName (this.handle);
				return Marshal.PtrToStringAuto (ptr);
			}
		}

		public static IntPtr GetHandle (string name) {
			return objc_getClass (name);
		}

		internal static Type Lookup (IntPtr klass) {
			// FAST PATH
			Type type;
			if (type_map.TryGetValue (klass, out type))
				return type;

			// TODO:  When we type walk we currently populate the type map
			// from the walk point with the target, we should gather some 
			// stats here, and see how many times there is a intermediate class
			// and see if we should populate them in the map as well
			IntPtr orig_klass = klass;

			do {
				IntPtr kls = class_getSuperclass (klass);

				if (type_map.TryGetValue (kls, out type)) {
					type_map [orig_klass] = type;
					return type;
				}

				klass = kls;
			} while (true);
		}

		internal static IntPtr Register (Type type) { 
			RegisterAttribute attr = (RegisterAttribute) Attribute.GetCustomAttribute (type, typeof (RegisterAttribute), false);
			string name = attr == null ? type.FullName : attr.Name ?? type.FullName;
			return Class.Register (type, name);
		}

		internal unsafe static IntPtr Register (Type type, string name) {
			IntPtr parent = IntPtr.Zero;
			IntPtr handle = IntPtr.Zero;
			objc_class *k;

			handle = objc_getClass (name);

			if (handle != IntPtr.Zero) {
				if (!type_map.ContainsKey (handle)) {
					type_map [handle] = type;
				}
				return handle;
			}

			Type parent_type = type.BaseType;
			string parent_name = null;
			while (Attribute.IsDefined (parent_type, typeof (ModelAttribute), false))
				parent_type = parent_type.BaseType;
			RegisterAttribute parent_attr = (RegisterAttribute) Attribute.GetCustomAttribute (parent_type, typeof (RegisterAttribute), false);
			parent_name = parent_attr == null ? parent_type.FullName : parent_attr.Name ?? parent_type.FullName;
			parent = objc_getClass (parent_name);
			if (parent == IntPtr.Zero && parent_type.Assembly != NSObject.MonoMacAssembly) {
				// Its possible as we scan that we might be derived from a type that isn't reigstered yet.
				Class.Register (parent_type, parent_name);
				parent = objc_getClass (parent_name);
			}
			if (parent == IntPtr.Zero) {
				// This spams mtouch, we need a way to differentiate from mtouch's (ab)use
				// Console.WriteLine ("CRITICAL WARNING: Falling back to NSObject for type {0} reported as {1}", type, parent_type);
				parent = objc_getClass ("NSObject");
			}
			handle = objc_allocateClassPair (parent, name, IntPtr.Zero);
			k = (objc_class *) handle;

			class_addIvar (handle, "__monoObjectGCHandle", (IntPtr) Marshal.SizeOf (typeof (Int32)), (ushort) 4, "i");

			foreach (PropertyInfo prop in type.GetProperties (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
				ConnectAttribute cattr = (ConnectAttribute) Attribute.GetCustomAttribute (prop, typeof (ConnectAttribute));
				if (cattr != null) {
					string ivar_name = cattr.Name ?? prop.Name;
					class_addIvar (handle, ivar_name, (IntPtr) Marshal.SizeOf (typeof (IntPtr)), (ushort) Math.Log (Marshal.SizeOf (typeof (IntPtr)), 2), "@");
				}
			}
	
#if OBJECT_REF_TRACKING
			class_addMethod (handle, release_builder.Selector, release_builder.Delegate, release_builder.Signature);
			class_addMethod (handle, retain_builder.Selector, retain_builder.Delegate, retain_builder.Signature);
#endif

			foreach (MethodInfo minfo in type.GetMethods (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
				ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (minfo.GetBaseDefinition (), typeof (ExportAttribute));
				if (ea == null || (minfo.IsVirtual && minfo.DeclaringType != type && minfo.DeclaringType.Assembly == NSObject.MonoMacAssembly))
					continue;
				
				NativeMethodBuilder builder = new NativeMethodBuilder (minfo);
				
				class_addMethod (minfo.IsStatic ? k->isa : handle, builder.Selector, builder.Delegate, builder.Signature);
				method_wrappers.Add (builder.Delegate);
#if DEBUG
				Console.WriteLine ("[METHOD] Registering {0}[0x{1:x}|{2}] on {3} -> ({4})", ea.Selector, (int) builder.Selector, builder.Signature, type, minfo);
#endif
			}
			
			ConstructorInfo default_ctor = type.GetConstructor (Type.EmptyTypes);
			if (default_ctor != null) {
				NativeConstructorBuilder builder = new NativeConstructorBuilder (default_ctor);

				class_addMethod (handle, builder.Selector, builder.Delegate, builder.Signature);
				method_wrappers.Add (builder.Delegate);
#if DEBUG
				Console.WriteLine ("[CTOR] Registering {0}[0x{1:x}|{2}] on {3} -> ({4})", "init", (int) builder.Selector, builder.Signature, type, default_ctor);
#endif
			}

			foreach (ConstructorInfo cinfo in type.GetConstructors (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)) {
				ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (cinfo, typeof (ExportAttribute));
				if (ea == null)
					continue;
				NativeConstructorBuilder builder = new NativeConstructorBuilder (cinfo);

				class_addMethod (handle, builder.Selector, builder.Delegate, builder.Signature);
				method_wrappers.Add (builder.Delegate);
#if DEBUG
				Console.WriteLine ("[CTOR] Registering {0}[0x{1:x}|{2}] on {3} -> ({4})", ea.Selector, (int) builder.Selector, builder.Signature, type, cinfo);
#endif
			}

			objc_registerClassPair (handle);			

			type_map [handle] = type;

			return handle;
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_allocateClassPair (IntPtr superclass, string name, IntPtr extraBytes);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_getClass (string name);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void objc_registerClassPair (IntPtr cls);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static bool class_addIvar (IntPtr cls, string name, IntPtr size, ushort alignment, string types);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static bool class_addMethod (IntPtr cls, IntPtr name, Delegate imp, string types);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr class_getName (IntPtr cls);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr class_getSuperclass (IntPtr cls);

		private struct objc_class {
			internal IntPtr isa;
		}
	}
}
