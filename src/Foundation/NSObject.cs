//
// Copyright 2010, Novell, Inc.
// Copyright 2012 - 2013, Xamarin Inc.
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	[StructLayout(LayoutKind.Sequential)]
	public partial class NSObject : INativeObject, IDisposable {
		public static readonly Assembly MonoMacAssembly = typeof (NSObject).Assembly;

		IntPtr gchandle;
		bool has_managed_ref;

		static object lock_obj = new object ();

		private void InitializeObject (bool alloced) {			
			if (alloced && handle == IntPtr.Zero && Class.ThrowOnInitFailure) {
				if (ClassHandle == IntPtr.Zero)
					throw new Exception (string.Format ("Could not create an native instance of the type '{0}': the native class hasn't been loaded.\n" +
					                                    "It is possible to ignore this condition by setting MonoMac.ObjCRuntime.Class.ThrowOnInitFailure to false.",
					                                    GetType ().FullName));
				throw new Exception (string.Format ("Failed to create a instance of the native type '{0}'.\n" +
				                                    "It is possible to ignore this condition by setting MonoMac.ObjCRuntime.Class.ThrowOnInitFailure to false.",
				                                    new Class (ClassHandle).Name));
			}

			IsDirectBinding = (this.GetType ().Assembly == NSObject.MonoMacAssembly);
			Runtime.RegisterNSObject (this, handle);
			CreateManagedRef (!alloced);
		}
		
		static int GetRetainCount (IntPtr @this)
		{
			return Messaging.int_objc_msgSend (@this, Selector.RetainCount);
		}

#if DEBUG_REF_COUNTING

		static string GetClassName (IntPtr @this)
		{
			return new Class (@this).Name;
		}

		static bool HasManagedRef (IntPtr @this)
		{
			NSObject obj = Runtime.TryGetNSObject (@this);
			return obj != null && obj.has_managed_ref;
		}

		static int GetGCHandle (IntPtr @this)
		{
			NSObject obj = Runtime.TryGetNSObject (@this);
			return obj != null ? obj.gchandle.ToInt32 () : 0;
		}
#endif

		void UnregisterObject ()
		{
			Runtime.NativeObjectHasDied (handle);
		}

		void FreeGCHandle ()
		{			
			if (gchandle != IntPtr.Zero) {
#if DEBUG_REF_COUNTING
				Console.WriteLine ("\tGCHandle {0} destroyed for object 0x{1}", gchandle, handle.ToString ("x"));
#endif
				GCHandle.FromIntPtr (gchandle).Free ();
				gchandle = IntPtr.Zero;
			} else {
#if DEBUG_REF_COUNTING
				Console.WriteLine ("\tNo GCHandle for the object 0x{0}", handle.ToString ("x"));
#endif
			}
		}

		void SwitchGCHandle (bool to_weak)
		{
			if (gchandle != IntPtr.Zero)
				GCHandle.FromIntPtr (gchandle).Free ();

			if (to_weak) {
				gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this, GCHandleType.WeakTrackResurrection));
			} else {
				gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this, GCHandleType.Normal));
			}
		}

		delegate IntPtr RetainTrampolineDelegate (IntPtr @this, IntPtr sel);
		delegate void ReleaseTrampolineDelegate (IntPtr @this, IntPtr sel);
		static IntPtr RetainTrampolineFunctionPointer;
		static IntPtr ReleaseTrampolineFunctionPointer;
		static RetainTrampolineDelegate retainTrampoline;
		static ReleaseTrampolineDelegate releaseTrampoline;

		static internal void OverrideRetainAndRelease (IntPtr @class)
		{
			// TODO: implement overriding of platform types too.
			lock (lock_obj) {
				if (ReleaseTrampolineFunctionPointer == IntPtr.Zero) {
					retainTrampoline = new RetainTrampolineDelegate (RetainTrampoline);
					releaseTrampoline = new ReleaseTrampolineDelegate (ReleaseTrampoline);
					RetainTrampolineFunctionPointer = Marshal.GetFunctionPointerForDelegate (retainTrampoline);
					ReleaseTrampolineFunctionPointer = Marshal.GetFunctionPointerForDelegate (releaseTrampoline);
				}
			}
			Class.class_addMethod (@class, Selector.RetainHandle, RetainTrampolineFunctionPointer, "@@:");
			Class.class_addMethod (@class, Selector.ReleaseHandle, ReleaseTrampolineFunctionPointer, "v@:");
		}

		static bool IsUserType (IntPtr @this)
		{
			IntPtr cls = object_getClass (@this);
			
			if (Class.class_getMethodImplementation (cls, Selector.RetainHandle) == RetainTrampolineFunctionPointer)
				return true;
			
			// Unfortunately just checking if the retain trampoline is ours does not always work.
			// Instruments may add its own retain method, intercepting our own, causing this check to fail.
			// http://stackoverflow.com/questions/14324507/nsactiondispatcher-is-garbage-collected-when-instruments-is-attached
			//
			// Check if the class is in our list of custom types instead.
			//
			// TODO: Compare performance with the retain trampoline check above.
			//

			var type = Class.Lookup (cls, false);
			return type != null && Class.IsCustomType (type);
		}
		
		void CreateGCHandle (bool force_weak)
		{
			// force_weak is to avoid calling retainCount unless needed, since some classes (UIWebView in iOS 5)
			// will crash if retainCount is called before init. See bug #9261.
			bool weak = force_weak || (GetRetainCount (handle) == 1);

			this.has_managed_ref = true;

			if (weak) {
				gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this, GCHandleType.WeakTrackResurrection));
			} else {
				gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this, GCHandleType.Normal));
			}
#if DEBUG_REF_COUNTING
			Console.WriteLine ("\tGCHandle created for 0x{0}: {1} (HasManagedRef: true)", handle.ToString ("x"), gchandle);
#endif
		}

		void CreateManagedRef (bool retain)
		{
			bool user_type = IsUserType (handle);
			
#if DEBUG_REF_COUNTING
			Console.WriteLine ("CreateManagedRef ({0} Handle=0x{1}) retainCount={2}; HasManagedRef={3} GCHandle={4} IsUserType={5}", 
			         GetClassName (handle), handle.ToString ("x"), GetRetainCount (handle), has_managed_ref, gchandle, user_type);
#endif
			
			if (user_type) {
				if (gchandle == IntPtr.Zero) {
					CreateGCHandle (!retain);
				} else {
#if DEBUG_REF_COUNTING
					Console.WriteLine ("GCHandle already exists for 0x{0}: {1}", handle.ToString ("x"), gchandle);
#endif
				}
			}
			
			if (retain)
				Messaging.void_objc_msgSend (handle, Selector.RetainHandle);
		}
		
		void ReleaseManagedRef ()
		{
			var handle = this.handle;
			bool user_type = IsUserType (handle);
			
#if DEBUG_REF_COUNTING
			Console.WriteLine ("ReleaseManagedRef ({0} Handle=0x{1}) retainCount={2}; HasManagedRef={3} GCHandle={4} IsUserType={5}", 
			                   GetClassName (handle), handle.ToString ("x"), Messaging.int_objc_msgSend (handle, Selector.RetainCount), has_managed_ref, gchandle, user_type);
#endif
			
			if (user_type) {
				has_managed_ref = false;
			} else {
				/* If we're a wrapper type, we need to unregister here, since we won't enter the release trampoline */
				UnregisterObject ();
			}
			
			Messaging.void_objc_msgSend (handle, Selector.ReleaseHandle);
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern IntPtr object_getClass (IntPtr @this);

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_msgSendSuper (ref objc_super super, IntPtr selector);

		static IntPtr InvokeObjCMethodImplementation (IntPtr @this, IntPtr sel)
		{
			objc_super sup;
			IntPtr klass = object_getClass (@this);
			IntPtr sklass = Class.class_getSuperclass (klass);
			
			IntPtr imp = Class.class_getMethodImplementation (klass, sel);
			IntPtr simp = Class.class_getMethodImplementation (sklass, sel);
			while (imp == simp) {
				sklass = Class.class_getSuperclass (sklass);
				simp = Class.class_getMethodImplementation (sklass, sel);
			}
			
			sup.receiver = @this;
			sup.super = sklass;
			return objc_msgSendSuper (ref sup, sel);
		}

		static void ReleaseTrampoline (IntPtr @this, IntPtr sel)
		{
			int ref_count = Messaging.int_objc_msgSend (@this, Selector.RetainCount);
			NSObject obj = null;

#if DEBUG_REF_COUNTING
			Console.WriteLine ("ReleaseTrampoline ({0} Handle=0x{1}) retainCount={2}; HasManagedRef={3} GCHandle={4}", 
				GetClassName (@this), @this.ToString ("x"), ref_count, HasManagedRef (@this), GetGCHandle (@this));
#endif
	
			/* Object is about to die. Unregister it and free any gchandles we may have */
			if (ref_count == 1) {
				obj = Runtime.TryGetNSObject (@this);
				if (obj != null) {
					obj.UnregisterObject ();
					obj.FreeGCHandle ();
				} else {
#if DEBUG_REF_COUNTING
					Console.WriteLine ("\tCould not find managed object");
#endif
				}
			}
	
			/*
			 * We need to decide if the gchandle should become a weak one.
			 * This happens if managed code will end up holding the only ref.
			 */
			if (ref_count == 2) {
				obj = Runtime.TryGetNSObject (@this);
				if (obj != null && obj.has_managed_ref)
					obj.SwitchGCHandle (true /* weak */);
			}

			InvokeObjCMethodImplementation (@this, sel);
		}

		static IntPtr RetainTrampoline (IntPtr @this, IntPtr sel)
		{
			int ref_count = Messaging.int_objc_msgSend (@this, Selector.RetainCount);
			NSObject obj = null;

#if DEBUG_REF_COUNTING
			bool had_managed_ref = HasManagedRef (@this);
			int pre_gchandle = GetGCHandle (@this);
#endif
			
			/*
			 * We need to decide if the gchandle should become a strong one.
			 * This happens if managed code has a ref, and the current refcount is 1.
			 */
			if (ref_count == 1) {
				obj = Runtime.TryGetNSObject (@this);
				if (obj != null && obj.has_managed_ref)
					obj.SwitchGCHandle (false /* strong */);
			}
			
			@this = InvokeObjCMethodImplementation (@this, sel);
			
#if DEBUG_REF_COUNTING
			Console.WriteLine ("RetainTrampoline ({0} Handle=0x{1}) initial retainCount={2}; new retainCount={3} HadManagedRef={4} HasManagedRef={5} old GCHandle={6} new GCHandle={7}",
			                   Class.GetName (Messaging.intptr_objc_msgSend (@this, Selector.GetHandle ("class"))), @this.ToString ("x"), ref_count, Messaging.int_objc_msgSend (@this, Selector.RetainCount),
			                   had_managed_ref, HasManagedRef (@this), pre_gchandle, GetGCHandle (@this));
#endif
			
			return @this;
		}

		internal void SetAsProxy () {
			IsDirectBinding = true;
		}
	}
}
