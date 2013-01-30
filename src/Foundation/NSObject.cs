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
	[StructLayout(LayoutKind.Sequential)]
	public partial class NSObject : INativeObject, IDisposable {
		public static readonly Assembly MonoMacAssembly = typeof (NSObject).Assembly;

		static IntPtr selAlloc = Selector.GetHandle ("alloc");
		static IntPtr selDoesNotRecognizeSelector = Selector.GetHandle ("doesNotRecognizeSelector:");
		static IntPtr selPerformSelectorOnMainThreadWithObjectWaitUntilDone = Selector.GetHandle ("performSelectorOnMainThread:withObject:waitUntilDone:");
		static IntPtr selPerformSelectorWithObjectAfterDelay = Selector.GetHandle ("performSelector:withObject:afterDelay:");
	
		private IntPtr gchandle;
#if OBJECT_REF_TRACKING
		private object lock_obj = new object ();
#endif

		private void InitializeObject (bool alloced) {			
			if (alloced && handle == IntPtr.Zero && Class.ThrowOnInitFailure) {
				if (ClassHandle == IntPtr.Zero)
					throw new Exception (string.Format ("Could not create an native instance of the type '{0}': the native class hasn't been loaded.\n" +
					                                    "It is possible to ignore this condition by setting Class.ThrowOnInitFailure to false.",
					                                    GetType ().FullName));
				throw new Exception (string.Format ("Failed to create a instance of the native type '{0}'.\n" +
				                                    "It is possible to ignore this condition by setting Class.ThrowOnInitFailure to false.",
				                                    new Class (ClassHandle).Name));
			}

			IsDirectBinding = (this.GetType ().Assembly == NSObject.MonoMacAssembly);
			Runtime.RegisterNSObject (this, handle);

			if (!alloced)
				Retain ();
#if !OBJECT_REF_TRACKING
			gchandle = GCHandle.ToIntPtr (GCHandle.Alloc (this));
#endif
		}

		protected virtual void Dispose (bool disposing) {
			if (handle != IntPtr.Zero) {
				Runtime.UnregisterNSObject (handle);
				if (disposing) {
					if (Class.IsCustomType (this.GetType ()))
						Messaging.void_objc_msgSendSuper (SuperHandle, Selector.Release);
					else
						Messaging.void_objc_msgSend (handle, Selector.Release);
					
					if (super != IntPtr.Zero) {
						Marshal.FreeHGlobal (super);
						super = IntPtr.Zero;
					}
				} else {
					if (Class.IsCustomType (this.GetType ())) {
						NSObject_Disposer.AddSuper (SuperHandle);
					} else {
						NSObject_Disposer.AddDirect (handle);
						if (super != IntPtr.Zero) {
							Marshal.FreeHGlobal (super);
							super = IntPtr.Zero;
						}
					}
				}
				handle = IntPtr.Zero;
			}
		}

#if OBJECT_REF_TRACKING
		[Export ("release")]
		internal void NativeRelease () {
			lock (lock_obj) {
				uint count = Messaging.UInt32_objc_msgSend (handle, Selector.RetainCount);
				Messaging.void_objc_msgSendSuper (SuperHandle, Selector.Release);

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
				uint count = Messaging.UInt32_objc_msgSend (handle, Selector.RetainCount);
				Messaging.void_objc_msgSendSuper (SuperHandle, Selector.Retain);

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
#endif

		internal void SetAsProxy () {
			IsDirectBinding = true;
		}

		[Register ("__NSObject_Disposer")][Preserve (AllMembers=true)]
		internal class NSObject_Disposer : NSObject {
			static readonly List <IntPtr> direct_handles = new List<IntPtr> ();
			static readonly List <IntPtr> super_handles = new List<IntPtr> ();
			static readonly object lock_obj = new object ();
			static readonly IntPtr class_ptr = Class.GetHandle ("__NSObject_Disposer");
			static readonly IntPtr selDrain = Selector.GetHandle ("drain:");
			static readonly IntPtr selPerformSelectorOnMainThreadWithObjectWaitUntilDone = Selector.GetHandle ("performSelectorOnMainThread:withObject:waitUntilDone:");
			
			private NSObject_Disposer () { }
	
			internal static void AddDirect (IntPtr handle) {
				Add (direct_handles, handle);
			}

			internal static void AddSuper (IntPtr handle) {
				Add (super_handles, handle);
			}

			static void Add (List<IntPtr> list, IntPtr handle)
			{
				bool call_drain;
				lock (lock_obj) {
					list.Add (handle);
					call_drain = list.Count == 1;
				}
				if (!call_drain)
					return;

				Messaging.void_objc_msgSend_intptr_intptr_bool (class_ptr, selPerformSelectorOnMainThreadWithObjectWaitUntilDone, selDrain, IntPtr.Zero, false);
			}
	
			[Export ("drain:")]
			internal static void Drain (NSObject ctx) {
				List<IntPtr> direct = null;
				List<IntPtr> super = null;

				lock (lock_obj) {
					if (direct_handles.Count > 0) {
						direct = new List<IntPtr> (direct_handles);
						direct_handles.Clear ();
					}

					if (super_handles.Count > 0) {
						super = new List<IntPtr> (super_handles);
						super_handles.Clear ();
					}
				}

				if (super != null)
					foreach (IntPtr x in super) {
						Messaging.void_objc_msgSendSuper (x, Selector.Release);
						Marshal.FreeHGlobal (x);
					}
				if (direct != null)
					foreach (IntPtr x in direct)
						Messaging.void_objc_msgSend (x, Selector.Release);
			}
		}
	}
}
