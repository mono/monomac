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
// Copyright 2013 Xamarin Inc
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.Foundation;

namespace MonoMac.ObjCRuntime {
	 public class Class : INativeObject {
		public static bool ThrowOnInitFailure = true;

		static Dictionary <IntPtr, Type> type_map = new Dictionary <IntPtr, Type> ();
		static Dictionary <Type, Type> custom_types = new Dictionary <Type, Type> ();
		static List <Delegate> method_wrappers = new List <Delegate> ();
		static object lock_obj = new object ();

		internal IntPtr handle;

		public Class (string name) {
			this.handle = objc_getClass (name);

			if (this.handle == IntPtr.Zero)
				throw new ArgumentException (String.Format ("name {0} is an unknown class", name), "name");
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
		
		internal static string GetName (IntPtr @class)
		{
			return Marshal.PtrToStringAuto (class_getName (@class));
		}

		public static IntPtr GetHandle (string name) {
			return objc_getClass (name);
		}
		
		public static IntPtr GetHandle (Type type) {
			RegisterAttribute attr = (RegisterAttribute) Attribute.GetCustomAttribute (type, typeof (RegisterAttribute), false);
			string name = attr == null ? type.FullName : attr.Name ?? type.FullName;
			bool is_wrapper = attr == null ? false : attr.IsWrapper;
			var handle = objc_getClass (name);
			
			if (handle == IntPtr.Zero)
				handle = Class.Register (type, name, is_wrapper);
			
			return handle;
		}

		public static bool IsCustomType (Type type) {
			lock (lock_obj)
				return custom_types.ContainsKey (type);
		}

		internal static Type Lookup (IntPtr klass)
		{
			return Lookup (klass, true);
		}

		internal static Type Lookup (IntPtr klass, bool throw_on_error) {
			// FAST PATH
			lock (lock_obj) {
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

					if (kls == IntPtr.Zero) {
						if (!throw_on_error)
							return null;

						var message = "Could not find a valid superclass for type " + new Class (orig_klass).Name 
							+ ". Did you forget to register the bindings at " + typeof(Class).FullName
							+ ".Register() or call NSApplication.Init()?";
						throw new ArgumentException (message);
					}

					klass = kls;
				} while (true);
			}
		}

		internal static IntPtr Register (Type type) { 
			RegisterAttribute attr = (RegisterAttribute) Attribute.GetCustomAttribute (type, typeof (RegisterAttribute), false);
			string name = attr == null ? type.FullName : attr.Name ?? type.FullName;
			bool is_wrapper = attr == null ? false : attr.IsWrapper;
			return Register (type, name, is_wrapper);
		}

		static IntPtr Register (Type type, string name, bool is_wrapper) {
			IntPtr parent = IntPtr.Zero;
			IntPtr handle = IntPtr.Zero;

			handle = objc_getClass (name);

			lock (lock_obj) {
				if (handle != IntPtr.Zero) {
					if (!type_map.ContainsKey (handle)) {
						type_map [handle] = type;
					}
					return handle;
				}

				if (objc_getProtocol (name) != IntPtr.Zero)
					throw new ArgumentException ("Attempting to register a class named: " + name + " which is a valid protocol");
				
				if (is_wrapper)
					return IntPtr.Zero;

				Type parent_type = type.BaseType;
				string parent_name = null;
				while (Attribute.IsDefined (parent_type, typeof (ModelAttribute), false))
					parent_type = parent_type.BaseType;
				RegisterAttribute parent_attr = (RegisterAttribute)Attribute.GetCustomAttribute (parent_type, typeof(RegisterAttribute), false);
				parent_name = parent_attr == null ? parent_type.FullName : parent_attr.Name ?? parent_type.FullName;
				parent = objc_getClass (parent_name);
				if (parent == IntPtr.Zero && parent_type.Assembly != NSObject.MonoMacAssembly) {
					bool parent_is_wrapper = parent_attr == null ? false : parent_attr.IsWrapper;
					// Its possible as we scan that we might be derived from a type that isn't reigstered yet.
					Register (parent_type, parent_name, parent_is_wrapper);
					parent = objc_getClass (parent_name);
				}
				if (parent == IntPtr.Zero) {
					// This spams mtouch, we need a way to differentiate from mtouch's (ab)use
					// Console.WriteLine ("CRITICAL WARNING: Falling back to NSObject for type {0} reported as {1}", type, parent_type);
					parent = objc_getClass ("NSObject");
				}
				handle = objc_allocateClassPair (parent, name, IntPtr.Zero);

				foreach (PropertyInfo prop in type.GetProperties (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
					ConnectAttribute cattr = (ConnectAttribute)Attribute.GetCustomAttribute (prop, typeof(ConnectAttribute));
					if (cattr != null) {
						string ivar_name = cattr.Name ?? prop.Name;
						class_addIvar (handle, ivar_name, (IntPtr)Marshal.SizeOf (typeof(IntPtr)), (ushort)Math.Log (Marshal.SizeOf (typeof(IntPtr)), 2), "@");
					}

					RegisterProperty (prop, type, handle);
				}
	
				NSObject.OverrideRetainAndRelease (handle);

				foreach (MethodInfo minfo in type.GetMethods (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
					RegisterMethod (minfo, type, handle);

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
					ExportAttribute ea = (ExportAttribute)Attribute.GetCustomAttribute (cinfo, typeof(ExportAttribute));
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
				custom_types.Add (type, type);

				return handle;
			}
		}

		// FIXME: This doesn't properly handle virtual properties yet
		private unsafe static void RegisterProperty (PropertyInfo prop, Type type, IntPtr handle) {
			ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (prop, typeof (ExportAttribute));
			if (ea == null)
				return;
			
			if (prop.PropertyType.IsGenericType || prop.PropertyType.IsGenericTypeDefinition)
				throw new ArgumentException (string.Format ("Cannot export the property '{0}.{1}': it is generic.", prop.DeclaringType.FullName, prop.Name));

			var m = prop.GetGetMethod (true);
			if (m != null)
				RegisterMethod (m, ea.ToGetter (prop), type, handle);
			m = prop.GetSetMethod (true);
			if (m != null)
				RegisterMethod (m, ea.ToSetter (prop), type, handle);
				
			// http://developer.apple.com/library/mac/#documentation/Cocoa/Conceptual/ObjCRuntimeGuide/Articles/ocrtPropertyIntrospection.html
			int count = 0;
			var props = new objc_attribute_prop [3];
			props [count++] = new objc_attribute_prop { name = "T", value = TypeConverter.ToNative (prop.PropertyType) };
			switch (ea.ArgumentSemantic) {
			case ArgumentSemantic.Copy:
				props [count++] = new objc_attribute_prop { name = "C", value = "" };
				break;
			case ArgumentSemantic.Retain:
				props [count++] = new objc_attribute_prop { name = "&", value = "" };
				break;
			}
			props [count++] = new objc_attribute_prop { name = "V", value = ea.Selector };
			
			class_addProperty (handle, ea.Selector, props, count);
			
		}

		private unsafe static void RegisterMethod (MethodInfo minfo, Type type, IntPtr handle) {
			ExportAttribute ea = (ExportAttribute) Attribute.GetCustomAttribute (minfo.GetBaseDefinition (), typeof (ExportAttribute));
			if (ea == null || (minfo.IsVirtual && minfo.DeclaringType != type && minfo.DeclaringType.Assembly == NSObject.MonoMacAssembly))
				return;

			RegisterMethod (minfo, ea, type, handle);
		}
		
		static IntPtr memory;
		static int size_left;
		static IntPtr AllocExecMemory (int size)
		{
			IntPtr result;

			if (size_left < size) {
				size_left = 4096;
				memory = Marshal.AllocHGlobal (size_left);
				if (memory == IntPtr.Zero)
					throw new Exception (string.Format ("Could not allocate memory for specialized x86 floating point stret delegate thunk: {0}", Marshal.GetLastWin32Error ()));
				if (mprotect (memory, size_left, 7 /*MmapProts.PROT_READ | MmapProts.PROT_WRITE | MmapProts.PROT_EXEC*/) != 0)
					throw new Exception (string.Format ("Could not make allocated memory for specialized x86 floating point stret delegate thunk code executable: {0}", Marshal.GetLastWin32Error ()));
			}

			result = memory;
			size_left -= size;
			memory = new IntPtr (memory.ToInt32 () + size);
			return result;
		}
		
		
		static bool TypeRequiresFloatingPointTrampoline (Type t)
		{
			// this is an x86 requirement only
			if (IntPtr.Size != 4)
				return false;

			if (typeof (float) == t || typeof (double) == t)
				return false;
			
			if (!t.IsValueType || t.IsEnum)
				return false;

			if (Marshal.SizeOf (t) <= 8)
				return false;
			
			return TypeContainsFloatingPoint (t);
		}
		
		static bool TypeContainsFloatingPoint (Type t) {
			if (!t.IsValueType || t.IsEnum || t.IsPrimitive)
				return false;

			foreach (var field in t.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
				if (field.FieldType == typeof (double) || field.FieldType == typeof (float))
					return true;
				if (field.FieldType == t)
					continue;
				if (TypeContainsFloatingPoint (field.FieldType))
					return true;
			}

			return false;
		}

		[MonoNativeFunctionWrapper]
		delegate int getFrameLengthDelegate (IntPtr @this, IntPtr sel);
		static getFrameLengthDelegate getFrameLength = Selector.GetFrameLength;
		static IntPtr getFrameLengthPtr = Marshal.GetFunctionPointerForDelegate (getFrameLength);
		
		static IntPtr GetFunctionPointer (MethodInfo minfo, Delegate @delegate)
		{
			IntPtr fptr = Marshal.GetFunctionPointerForDelegate (@delegate);
			
			if (!TypeRequiresFloatingPointTrampoline (minfo.ReturnType))
				return fptr;
			
			var thunk_ptr = AllocExecMemory (83);
			var rel_Delegate = new IntPtr (fptr.ToInt32 () - thunk_ptr.ToInt32 () - 70);
			var rel_GetFrameLengthPtr = new IntPtr (getFrameLengthPtr.ToInt32 () - thunk_ptr.ToInt32 () - 27);
			var delptr = BitConverter.GetBytes (rel_Delegate.ToInt32 ());
			var getlen = BitConverter.GetBytes (rel_GetFrameLengthPtr.ToInt32 ());
			var thunk = new byte [] 
			{
/*
# 
# the problem we are trying to solve here is that the abi apparently requires
# us to leave the stack unbalanced upon return (pop off one more value than we get,
# this is the "ret $0x4" at the end).
#
# method definition:
# trampoline (void *buffer, id this, SEL sel, ...)
#
# input stack layout:
# %esp+20: ...
# %esp+16: second vararg
# %esp+12: first vararg
# %esp+8:  sel
# %esp+4:  this
# %esp:    buffer
# and %ebp+8 = %esp
#
# We extend the stack (big enough for all the arguments again),
# and copy all the arguments as-is there, before
# calling the original delegate with those copied arguments.
#

# prolog
pushl    %ebp                   */  0x55,                                                 /*
movl     %esp,%ebp              */  0x89, 0xe5,                                           /*
pushl    %esi                   */  0x56,                                                 /*
pushl    %edi                   */  0x57,                                                 /*
pushl    %ebx                   */  0x53,                                                 /*
subl     $0x3c,%esp             */  0x83, 0xec, 0x3c,                                     /*

# get the size of the stack space used by all the arguments 
# int frame_length = get_frame_length (this, sel)
movl     0x10(%ebp),%eax        */  0x8b, 0x45, 0x10,                                     /*
movl     %eax,0x04(%esp)        */  0x89, 0x44, 0x24, 0x04,                               /*
movl     0x0c(%ebp),%eax        */  0x8b, 0x45, 0x0c,                                     /*
movl     %eax,(%esp)            */  0x89, 0x04, 0x24,                                     /*
calll    _get_frame_length      */  0xe8, getlen [0], getlen [1], getlen [2], getlen [3], /*
movl     %eax,0xf0(%ebp)        */  0x89, 0x45, 0xf0,                                     /*

# use eax to extend the stack, but it needs to be aligned to 16 bytes first 
addl    $0x0f,%eax              */  0x83, 0xc0, 0x0f,                                     /*
shrl    $0x04,%eax              */  0xc1, 0xe8, 0x04,                                     /*
shll    $0x04,%eax              */  0xc1, 0xe0, 0x04,                                     /*
subl    %eax,%esp               */  0x29, 0xc4,                                           /*

# copy arguments from old location in the stack to new location in the stack
# %ecx will hold the amount of bytes left to copy
# %esi the current src location
# %edi the current dst location

# %ecx will already be a multiple of 4, since the abi requires it
# (arguments smaller than 4 bytes are extended to 4 bytes according to
# http://developer.apple.com/library/mac/#documentation/DeveloperTools/Conceptual/LowLevelABI/130-IA-32_Function_Calling_Conventions/IA32.html#//apple_ref/doc/uid/TP40002492-SW4)

# Do not use memcpy, it may not work since we can get arguments in registers memcpy is free to clobber (XMM0-XMM3)

movl    0xf0(%ebp),%ecx        */  0x8b, 0x4d, 0xf0,                                      /*
leal    0x08(%ebp),%esi        */  0x8d, 0x75, 0x08,                                      /*
movl    %esp,%edi              */  0x89, 0xe7,                                            /*

L_start:
cmpl    $0,%ecx                */  0x83, 0xf9, 0x00,                                      /*
je      L_end                  */  0x74, 0x0b,                                            /*
subl    $0x04,%ecx             */  0x83, 0xe9, 0x04,                                      /*
movl    (%esi,%ecx),%eax       */  0x8b, 0x04, 0x0e,                                      /*
movl    %eax,(%edi,%ecx)       */  0x89, 0x04, 0x0f,                                      /*
jmp     L_start                */  0xeb, 0xf0,                                            /*

L_end:
calll    delegate              */  0xe8, delptr [0], delptr [1], delptr [2], delptr [3], /*
# epilogue:
movl    0xf4(%ebp),%ebx        */  0x8b, 0x5d, 0xf4,                                      /*
movl    0xf8(%ebp),%edi        */  0x8b, 0x7d, 0xf8,                                      /*
movl    0xfc(%ebp),%esi        */  0x8b, 0x75, 0xfc,                                      /*
leave                          */  0xc9,                                                  /*
retl    $0x4                   */  0xc2, 0x04, 0x00,                                      /*

*/
			};
				
			Marshal.Copy (thunk, 0, thunk_ptr, thunk.Length);
			// Console.WriteLine ("Adding marshalling thunk for: {0} {1} ({2} params) new fptr: 0x{3} old fptr: 0x{4} thunk size: {5}", minfo.DeclaringType.FullName, minfo.Name, minfo.GetParameters ().Length, thunk_ptr.ToString ("X"), fptr.ToString ("X"), thunk.Length);
			fptr = thunk_ptr;

			return fptr;
		}
		
		internal unsafe static void RegisterMethod (MethodInfo minfo, ExportAttribute ea, Type type, IntPtr handle) {
			NativeMethodBuilder builder = new NativeMethodBuilder (minfo, type, ea);

			class_addMethod (minfo.IsStatic ? ((objc_class *) handle)->isa : handle, builder.Selector, GetFunctionPointer (minfo, builder.Delegate), builder.Signature);
			lock (lock_obj)
				method_wrappers.Add (builder.Delegate);
#if DEBUG
			Console.WriteLine ("[METHOD] Registering {0}[0x{1:x}|{2}] on {3} -> ({4})", ea.Selector, (int) builder.Selector, builder.Signature, type, minfo);
#endif
		}

		[DllImport ("libc", SetLastError=true)]
		extern static int mprotect (IntPtr addr, int len, int prot);
		[DllImport ("libc", SetLastError=true)]
		static extern IntPtr mmap (IntPtr start, ulong length, int prot, int flags, int fd, long offset);
		
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_allocateClassPair (IntPtr superclass, string name, IntPtr extraBytes);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_getClass (string name);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_getProtocol (string name);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void objc_registerClassPair (IntPtr cls);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static bool class_addIvar (IntPtr cls, string name, IntPtr size, ushort alignment, string types);
		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static bool class_addMethod (IntPtr cls, IntPtr name, Delegate imp, string types);
		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static bool class_addMethod (IntPtr cls, IntPtr name, IntPtr imp, string types);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr class_getName (IntPtr cls);
		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr class_getSuperclass (IntPtr cls);
		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr class_getMethodImplementation (IntPtr cls, IntPtr sel);
		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr class_getInstanceVariable (IntPtr cls, string name);

		[MonoNativeFunctionWrapper]
		delegate IntPtr addPropertyDelegate (IntPtr cls, string name, objc_attribute_prop [] attributes, int count);
		static addPropertyDelegate addProperty;
		static bool addPropertyInitialized;

		static IntPtr class_addProperty (IntPtr cls, string name, objc_attribute_prop [] attributes, int count)
		{
			if (!addPropertyInitialized) {
				var handle = Dlfcn.dlopen (Constants.ObjectiveCLibrary, 0);
				try {
					var fptr = Dlfcn.dlsym (handle, "class_addProperty");
					if (fptr != IntPtr.Zero)
						addProperty = (addPropertyDelegate) Marshal.GetDelegateForFunctionPointer (fptr, typeof (addPropertyDelegate));
				} finally {
					Dlfcn.dlclose (handle);
				}
				addPropertyInitialized = true;
			}
			if (addProperty == null)
				return IntPtr.Zero;
			return addProperty (cls, name, attributes, count);
		}

		[StructLayout (LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		private struct objc_attribute_prop {
			[MarshalAs (UnmanagedType.LPStr)] internal string name;
			[MarshalAs (UnmanagedType.LPStr)] internal string value;
		}
		
		internal struct objc_class {
			internal IntPtr isa;
		}
	}
}
