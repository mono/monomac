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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public class NSObjectFlag {
		public static NSObjectFlag Empty = null;

		NSObjectFlag () {}
	}

	[StructLayout(LayoutKind.Sequential)]
	public partial class NSObject : INativeObject, IDisposable {
		public static Assembly MonoMacAssembly = typeof (NSObject).Assembly;

		static IntPtr selAlloc = Selector.GetHandle ("alloc");
		static IntPtr selAwakeFromNib = Selector.GetHandle ("awakeFromNib");
		static IntPtr selDoesNotRecognizeSelector = Selector.GetHandle ("doesNotRecognizeSelector:");
		static IntPtr selDrain = Selector.GetHandle ("drain:");
		static IntPtr selPerformSelectorOnMainThreadWithObjectWaitUntilDone = Selector.GetHandle ("performSelectorOnMainThread:withObject:waitUntilDone:");
		static IntPtr selPerformSelectorWithObjectAfterDelay = Selector.GetHandle ("performSelector:withObject:afterDelay:");
		static IntPtr selRelease = Selector.GetHandle ("release");
		static IntPtr selRespondsToSelector = Selector.GetHandle ("respondsToSelector:");
		static IntPtr selRetain = Selector.GetHandle ("retain");
		static IntPtr selRetainCount = Selector.GetHandle ("retainCount");

		static MonoMac_Disposer disposer = new MonoMac_Disposer ();
			
		private IntPtr handle;
		private IntPtr super;
		private IntPtr super_ptr;
		
#if COREBUILD
		static IntPtr class_ptr = Class.GetHandle ("NSObject");
		public virtual IntPtr ClassHandle  { get { return class_ptr; } }
#endif
		protected bool IsDirectBinding;

		[Export ("init")]
		public NSObject () {
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}

		// This is just here as a constructor chain that can will
		// only do Init at the most derived class.
		public NSObject (NSObjectFlag x)
		{
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}

		public NSObject (IntPtr handle) : this (handle, false) {
		}
		
		private NSObject (IntPtr handle, bool alloced) {
			this.handle = handle;
			InitializeObject (alloced);
		}

		~NSObject () {
			Dispose (false);
		}

		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		[Export ("respondsToSelector:")]
		public virtual bool RespondsToSelector (Selector sel) {
			if (IsDirectBinding) {
				return Messaging.bool_objc_msgSend_intptr (Handle, selRespondsToSelector, sel.Handle);
			} else {
				return Messaging.bool_objc_msgSendSuper_intptr (SuperHandle, selRespondsToSelector, sel.Handle);
			}
		}

		[Export ("doesNotRecognizeSelector:")]
		public virtual void DoesNotRecognizeSelector (Selector sel) {
			Console.WriteLine ("CRITICAL WARNING: [{0} {1}] is not recognized", this.GetType (), sel.Name);

			Messaging.void_objc_msgSendSuper_intptr (SuperHandle, selDoesNotRecognizeSelector, sel.Handle);
		}

		private void InitializeObject (bool alloced) {
			IsDirectBinding = (this.GetType ().Assembly == NSObject.MonoMacAssembly);
			super_ptr = ClassHandle;	
			Runtime.RegisterNSObject (this, handle);
			if (!alloced)
				Messaging.void_objc_msgSend (handle, selRetain);

#if !OBJECT_REF_TRACKING
			GCHandle h = GCHandle.Alloc (this);
			SetObjCIvar ("__monoObjectGCHandle", (IntPtr) h);
#endif
		}

		protected virtual void Dispose (bool disposing) {
			if (handle != IntPtr.Zero) {
				Runtime.UnregisterNSObject (handle);
				if (disposing) {
					Release ();
					Marshal.FreeHGlobal (SuperHandle);
				} else {
					if (disposer.Add (handle))
						Messaging.void_objc_msgSend_intptr_intptr_bool (SuperHandle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, selDrain, IntPtr.Zero, false);
				}
				handle = IntPtr.Zero;
				super = IntPtr.Zero;
			}
		}

#if OBJECT_REF_TRACKING
		internal void Release () {
			Messaging.void_objc_msgSend (handle, selRelease);
		}

		internal void Retain () {
			Messaging.void_objc_msgSend (handle, selRetain);
		}

		[Export ("release")]
		internal void NativeRelease () {
			uint count = Messaging.uint_objc_msgSend (handle, selRetainCount);
			Messaging.void_objc_msgSendSuper (SuperHandle, selRelease);

			Console.WriteLine ("Releasing a {0}:{1}", this.GetType (), count);
			if (count == 2) {
				IntPtr hptr = GetObjCIvar ("__monoObjectGCHandle");

				if (hptr != IntPtr.Zero) {
					GCHandle h = (GCHandle) hptr;
					h.Free ();
					SetObjCIvar ("__monoObjectGCHandle", IntPtr.Zero);
				} else {
					Console.WriteLine ("WARNING: How did this happen: NativeRelease");
				}
			}
		}

		[Export ("retain")]
		internal IntPtr NativeRetain () {
			uint count = Messaging.uint_objc_msgSend (handle, selRetainCount);
			Messaging.void_objc_msgSendSuper (SuperHandle, selRetain);

			if (count == 1) {
				IntPtr hptr = GetObjCIvar ("__monoObjectGCHandle");

				if (hptr == IntPtr.Zero) {
					GCHandle h = GCHandle.Alloc (this);
					SetObjCIvar ("__monoObjectGCHandle", (IntPtr) h);
				} else {
					Console.WriteLine ("WARNING: How did this happen: NativeRetain");
				}
			}

			return handle;
		}
#else
		internal void Release () {}
		internal IntPtr Retain () {
			return IntPtr.Zero;
		}
#endif

		public IntPtr SuperHandle {
			get {
				if (super == IntPtr.Zero) {
					super = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (objc_super)));
#if AOT_COMPILER_STRUCTS_FIXED
					objc_super sup = new objc_super ();
					sup.receiver = handle;
					sup.super = super_ptr;
					Marshal.StructureToPtr (sup, super, false);
#else
					Marshal.WriteIntPtr (super, handle);
					Marshal.WriteIntPtr (super, 4, super_ptr);
#endif
				}
				return super;
			}
		}

		public IntPtr Handle {
			get { return handle; }
			set {
				if (handle != value) {
					Runtime.UnregisterNSObject (handle);
				 	handle = value;
					Runtime.RegisterNSObject (this, handle);
				}
			}
		}

		private bool AllocIfNeeded () {
			if (handle == IntPtr.Zero) {
				handle = Messaging.intptr_objc_msgSend (new Class (this.GetType ()).Handle, selAlloc);
				return true;
			}
			return false;
		}

		private IntPtr GetObjCIvar (string name) {
			IntPtr buf;
			IntPtr native;
			
			buf = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			object_getInstanceVariable (handle, name, buf);
			native = Marshal.ReadIntPtr (buf);
			Marshal.FreeHGlobal (buf);
	
			return native;
		}

		public NSObject GetNativeField (string name) {
			IntPtr field = GetObjCIvar (name);

			if (field == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject (field);
		}
		
		private void SetObjCIvar (string name, IntPtr value) {
			object_setInstanceVariable (handle, name, value);
		}

		public void SetNativeField (string name, NSObject value) {
			if (value == null)
				SetObjCIvar (name, IntPtr.Zero);
			else
				SetObjCIvar (name, value.Handle);
		}

		[Export ("performSelector:withObject:afterDelay:")]
		public virtual void PerformSelector (Selector sel, NSObject obj, float delay) {
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr_intptr_float (this.Handle, selPerformSelectorWithObjectAfterDelay, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			} else {
				Messaging.void_objc_msgSendSuper_intptr_intptr_float (this.SuperHandle, selPerformSelectorWithObjectAfterDelay, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			}
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_getInstanceVariable (IntPtr obj, string name, IntPtr val);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_setInstanceVariable (IntPtr obj, string name, IntPtr val);

		private struct objc_super {
			public IntPtr receiver;
			public IntPtr super;
		}

		[Export ("awakeFromNib")]
		public virtual void AwakeFromNib ()
		{
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend (this.Handle, selAwakeFromNib);
			} else {
				Messaging.void_objc_msgSendSuper (this.SuperHandle, selAwakeFromNib);
			}
		}

		private void InvokeOnMainThread (Selector sel, NSObject obj, bool wait)
		{
			Messaging.void_objc_msgSend_intptr_intptr_bool (this.Handle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, sel.Handle, obj.Handle, wait);
		}

		public void BeginInvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, false);
		}

		public void InvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, true);
		}

		public void BeginInvokeOnMainThread (NSAction action)
		{
			var d = new NSActionDispatcher (action);
			Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, 
					NSActionDispatcher.Selector.Handle, d.Handle, false);
			GC.KeepAlive (d);
		}

		public void InvokeOnMainThread (NSAction action)
		{
			var d = new NSActionDispatcher (action);
			Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, 
					NSActionDispatcher.Selector.Handle, d.Handle, true);
			GC.KeepAlive (d);
		}


		[Register ("__MonoMac_Disposer")][Preserve (AllMembers=true)]
		internal class MonoMac_Disposer : NSObject {
			List <IntPtr> handles;
			object lock_obj;
	

			internal MonoMac_Disposer () {
				handles = new List <IntPtr> ();
				lock_obj = new object ();
			}
	
			internal bool Add (IntPtr handle) {
				if (this.handle == handle)
					return false;

				lock (lock_obj) {
					handles.Add (handle);
					return handles.Count == 1;
				}
			}
	
			[Export ("drain:")]
			internal void Drain (NSObject ctx) {
				lock (lock_obj) {
					foreach (IntPtr x in handles) {
						Messaging.void_objc_msgSendSuper (x, selRelease);
						Marshal.FreeHGlobal (x);
					}
					handles.Clear ();
				}
			}
		}
	}
}
