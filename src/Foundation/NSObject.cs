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
		private IntPtr gchandle;
		private object lock_obj = new object ();
		
#if COREBUILD
		static IntPtr class_ptr = Class.GetHandle ("NSObject");
		public virtual IntPtr ClassHandle  { get { return class_ptr; } }
#endif
		protected bool IsDirectBinding;

		[Export ("init")]
		public NSObject () {
			AllocIfNeeded ();
			InitializeObject ();
		}

		// This is just here as a constructor chain that can will
		// only do Init at the most derived class.
		public NSObject (NSObjectFlag x)
		{
			AllocIfNeeded ();
			InitializeObject ();
		}

		public NSObject (IntPtr handle) {
			this.handle = handle;
			InitializeObject ();
			Retain ();
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

		private void InitializeObject () {
			IsDirectBinding = (this.GetType ().Assembly == NSObject.MonoMacAssembly);
			Runtime.RegisterNSObject (this, handle);

#if !OBJECT_REF_TRACKING
			gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this));
#endif
		}

		protected virtual void Dispose (bool disposing) {
			if (handle != IntPtr.Zero) {
				Runtime.UnregisterNSObject (handle);
				if (disposing) {
					if (Class.IsCustomType (this.GetType ()))
						Messaging.void_objc_msgSendSuper (SuperHandle, selRelease);
					else
						Messaging.void_objc_msgSend (handle, selRelease);

					Marshal.FreeHGlobal (SuperHandle);
				} else if (disposer != null) {
					bool calldrain = false;

					if (Class.IsCustomType (this.GetType ()))
						calldrain = disposer.AddSuper (SuperHandle);
					else
						calldrain = disposer.AddDirect (handle);

					if (calldrain)
						Messaging.void_objc_msgSend_intptr_intptr_bool (disposer.Handle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, selDrain, IntPtr.Zero, false);
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
			lock (lock_obj) {
				uint count = Messaging.UInt32_objc_msgSend (handle, selRetainCount);
				Messaging.void_objc_msgSendSuper (SuperHandle, selRelease);

				if (count == 2) {
					if (gchandle != IntPtr.Zero) {
						GCHandle h = (GCHandle) gchandle;
						h.Free ();
						gchandle = IntPtr.Zero;
					} else {
						Console.WriteLine ("[NativeRelease ERROR]: type: {0} handle: {1} count: {2} gchandle: {3}", this.GetType (), (int) handle, count, gchandle);
					}
				}
			}
		}

		[Export ("retain")]
		internal IntPtr NativeRetain () {
			lock (lock_obj) {
				uint count = Messaging.UInt32_objc_msgSend (handle, selRetainCount);
				Messaging.void_objc_msgSendSuper (SuperHandle, selRetain);

				if (count == 1) {
					if (gchandle == IntPtr.Zero) {
						gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this));
					} else {
						Console.WriteLine ("[NativeRetain ERROR]: type: {0} handle: {1} count: {2} gchandle: {3}", this.GetType (), (int) handle, count, gchandle);
					}
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
					objc_super sup = new objc_super ();
					sup.receiver = handle;
					// Find the threshold class
					sup.super = ClassHandle;
					Marshal.StructureToPtr (sup, super, false);
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

		private void AllocIfNeeded () {
			if (handle == IntPtr.Zero)
				handle = Messaging.intptr_objc_msgSend (new Class (this.GetType ()).Handle, selAlloc);
		}

		private IntPtr GetObjCIvar (string name) {
			IntPtr native = IntPtr.Zero;
			
			object_getInstanceVariable (handle, name, ref native);
	
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

		internal void SetAsProxy () {
			IsDirectBinding = true;
		}

		[Export ("performSelector:withObject:afterDelay:")]
		public virtual void PerformSelector (Selector sel, NSObject obj, double delay) {
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr_intptr_double (this.Handle, selPerformSelectorWithObjectAfterDelay, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			} else {
				Messaging.void_objc_msgSendSuper_intptr_intptr_double (this.SuperHandle, selPerformSelectorWithObjectAfterDelay, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			}
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_getInstanceVariable (IntPtr obj, string name, ref IntPtr val);
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
			Messaging.void_objc_msgSend_intptr_intptr_bool (this.Handle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, wait);
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
			var d = new NSAsyncActionDispatcher (action);
			Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, 
					NSActionDispatcher.Selector.Handle, d.Handle, false);
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
			List <IntPtr> direct_handles;
			List <IntPtr> super_handles;
			object lock_obj;
	

			internal MonoMac_Disposer () {
				super_handles = new List <IntPtr> ();
				direct_handles = new List <IntPtr> ();
				lock_obj = new object ();
			}
	
			internal bool AddDirect (IntPtr handle) {
				if (this.handle == handle)
					return false;

				lock (lock_obj) {
					direct_handles.Add (handle);
					return direct_handles.Count == 1;
				}
			}

			internal bool AddSuper (IntPtr handle) {
				if (this.handle == handle)
					return false;

				lock (lock_obj) {
					super_handles.Add (handle);
					return super_handles.Count == 1;
				}
			}
	
			[Export ("drain:")]
			internal void Drain (NSObject ctx) {
				lock (lock_obj) {
					foreach (IntPtr x in super_handles) {
						Messaging.void_objc_msgSendSuper (x, selRelease);
						Marshal.FreeHGlobal (x);
					}
					super_handles.Clear ();
					foreach (IntPtr x in direct_handles) {
						Messaging.void_objc_msgSend (x, selRelease);
					}
					direct_handles.Clear ();
				}
			}

			protected override void Dispose (bool disposing)
			{
				NSObject.disposer = null;
				base.Dispose (disposing);
			}
		}
	}
}
