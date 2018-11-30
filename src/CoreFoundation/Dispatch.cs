//
// Dispatch.cs: Support for Grand Central Dispatch framework
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Novell, Inc.
// Copyright 2011-2013 Xamarin Inc
//
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
using System.Runtime.InteropServices;
using System.Threading;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation {

	public enum DispatchQueuePriority {
		High = 2,
		Default = 0,
		Low = -2,
	}
	
	public abstract class DispatchObject : INativeObject, IDisposable  {
		internal IntPtr handle;

		//
		// Constructors and lifecycle
		//
		[Preserve (Conditional = true)]
		internal DispatchObject (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			
			this.handle = handle;
			if (!owns)
				dispatch_retain (handle);
		}

		internal DispatchObject ()
		{
		}      
		
		[DllImport ("libc")]
		extern static IntPtr dispatch_release (IntPtr o);

		[DllImport ("libc")]
		extern static IntPtr dispatch_retain (IntPtr o);

               ~DispatchObject ()
                {
                        Dispose (false);
                }
                
                public void Dispose ()
                {
                        Dispose (true);
                        GC.SuppressFinalize (this);
                }

                public IntPtr Handle {
                        get { return handle; }
                }
             
                protected virtual void Dispose (bool disposing)
                {
                        if (handle != IntPtr.Zero){
                                dispatch_release (handle);
                                handle = IntPtr.Zero;
                        }
                }

		public static bool operator == (DispatchObject a, DispatchObject b)
		{
			var oa = a as object;
			var ob = b as object;
			
			if (oa == null){
				if (ob == null)
					return true;
				return false;
			} else {
				if (ob == null)
					return false;
				return a.handle == b.handle;
			}
		}

		public static bool operator != (DispatchObject a, DispatchObject b)
		{
			var oa = a as object;
			var ob = b as object;
			
			if (oa == null){
				if (ob == null)
					return false;
				return true;
			} else {
				if (ob == null)
					return true;
				return a.handle != b.handle;
			}
		}

		public override bool Equals (object other)
		{
			if (other == null)
				return false;
			
			var od = other as DispatchQueue;
			return od.handle == handle;
		}

		public override int GetHashCode ()
		{
			return (int) handle;
		}

		protected void Check ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException (GetType ().ToString ());
		}			

		[DllImport ("libc")]
		extern static void dispatch_set_target_queue (/* dispatch_object_t */ IntPtr queue, /* dispatch_queue_t */ IntPtr target);

		public void SetTargetQueue (DispatchQueue queue)
		{
			// note: null is allowed because DISPATCH_TARGET_QUEUE_DEFAULT is defined as NULL (dispatch/queue.h)
			IntPtr q = queue == null ? IntPtr.Zero : queue.Handle;
			dispatch_set_target_queue (handle, q);
		}
	}

	public class DispatchQueue : DispatchObject  {
		[Preserve (Conditional = true)]
		internal DispatchQueue (IntPtr handle, bool owns) : base (handle, owns)
		{
		}

		public DispatchQueue (IntPtr handle) : base (handle, false)
		{
		}
		
		public DispatchQueue (string label) : base ()
		{
			// Initialized in owned state for the queue.
			handle = dispatch_queue_create (label, IntPtr.Zero);
			if (handle == IntPtr.Zero)
				throw new Exception ("Error creating dispatch queue");
		}
		
		//
		// Properties and methods
		//

		public string Label {
			get {
				if (handle == IntPtr.Zero)
					throw new ObjectDisposedException ("DispatchQueue");
				
				return Marshal.PtrToStringAnsi (dispatch_queue_get_label (handle));
			}
		}

		[DllImport ("libc")]
		extern static void dispatch_suspend (IntPtr o);
		public void Suspend ()
		{
			Check ();
			dispatch_suspend (handle);
		}

		[DllImport ("libc")]
		extern static void dispatch_resume (IntPtr o);
		
		public void Resume ()
		{
			Check ();
			dispatch_resume (handle);
		}

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_context (IntPtr o);

		[DllImport ("libc")]
		extern static void dispatch_set_context (IntPtr o, IntPtr ctx);

		public IntPtr Context {
			get {
				Check ();
				return dispatch_get_context (handle);
			}
			set {
				Check ();
				dispatch_set_context (handle, value);
			}
		}
	
		[Obsolete ("Deprecated in iOS 6.0")]
		public static DispatchQueue CurrentQueue {
			get {
				return new DispatchQueue (dispatch_get_current_queue (), false);
			}
		}

		public static DispatchQueue GetGlobalQueue (DispatchQueuePriority priority)
		{
			return new DispatchQueue (dispatch_get_global_queue ((IntPtr) priority, IntPtr.Zero), false);
		}

		public static DispatchQueue DefaultGlobalQueue {
			get {
				return new DispatchQueue (dispatch_get_global_queue ((IntPtr) DispatchQueuePriority.Default, IntPtr.Zero), false);
			}
		}
#if MONOMAC
		static DispatchQueue PInvokeDispatchGetMainQueue ()
		{
			return new DispatchQueue (dispatch_get_main_queue (), false);
		}

#endif
		static IntPtr main_q;
		static object lockobj = new object ();

		public static DispatchQueue MainQueue {
			get {
				lock (lockobj) {
					if (main_q == IntPtr.Zero) {
						// Try loading the symbol from our address space first, should work everywhere
						main_q = Dlfcn.dlsym ((IntPtr) (-2), "_dispatch_main_q");

						// Last case: this is technically not right for the simulator, as this path
						// actually points to the MacOS library, not the one in the SDK.
						if (main_q == IntPtr.Zero){
							var h = Dlfcn.dlopen ("/usr/lib/libSystem.dylib", 0x0);
							main_q = Dlfcn.GetIndirect (h, "_dispatch_main_q");
							Dlfcn.dlclose (h);
						}
					}
				}
#if MONOMAC
				// For Snow Leopard
				if (main_q == IntPtr.Zero)
					return PInvokeDispatchGetMainQueue ();
#endif
				return new DispatchQueue (main_q, false);
			}
		}


		//
		// Dispatching
		//
		internal delegate void dispatch_callback_t (IntPtr context);
		internal static readonly dispatch_callback_t static_dispatch = static_dispatcher_to_managed;
		
		[MonoPInvokeCallback (typeof (dispatch_callback_t))]
		static void static_dispatcher_to_managed (IntPtr context)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
			var obj = gch.Target as Tuple<Action, DispatchQueue>;
			if (obj != null) {
				var sc = SynchronizationContext.Current;

				// Set GCD synchronization context. Mainly used when await executes inside GCD to continue
				// execution on same dispatch queue. Set the context only when there is no user context
				// set, including UIKitSynchronizationContext
				//
				// This assumes that only 1 queue can run on thread at the same time
				//
				if (sc == null)
					SynchronizationContext.SetSynchronizationContext (new DispatchQueueSynchronizationContext (obj.Item2));

				try {
					obj.Item1 ();
				} catch {
					gch.Free ();
					throw;
				} finally {
					if (sc == null)
						SynchronizationContext.SetSynchronizationContext (null);
				}
			}

			gch.Free ();
		}
		
		public void DispatchAsync (Action action)
		{
			if (action == null)
				throw new ArgumentNullException ("action");
			
			dispatch_async_f (handle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, this)), static_dispatch);
		}

		public void DispatchSync (Action action)
		{
			if (action == null)
				throw new ArgumentNullException ("action");
			
			dispatch_sync_f (handle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, this)), static_dispatch);
		}

		//
		// Native methods
		//
		[DllImport ("libc")]
		extern static IntPtr dispatch_queue_create (string label, IntPtr attr);

		[DllImport ("libc")]
		extern static void dispatch_async_f (IntPtr queue, IntPtr context, dispatch_callback_t dispatch);

		[DllImport ("libc")]
		extern static void dispatch_sync_f (IntPtr queue, IntPtr context, dispatch_callback_t dispatch);

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_current_queue ();

		[DllImport ("libc")]
		// dispatch_queue_t dispatch_get_global_queue (long priority, unsigned long flags);
		// IntPtr used for 32/64 bits
		extern static IntPtr dispatch_get_global_queue (IntPtr priority, IntPtr flags);

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_main_queue ();

		[DllImport ("libc")]
		// this returns a "const char*" so we cannot make a string out of it since it will be freed (and crash)
		extern static IntPtr dispatch_queue_get_label (IntPtr queue);

#if MONOMAC
		//
		// Not to be used by apps that use UIApplicationMain, NSApplicationMain or CFRunLoopRun,
		// so not available on Monotouch
		//
		[DllImport ("libc")]
		static extern IntPtr dispatch_main ();

		public static void MainIteration ()
		{
			dispatch_main ();
		}
#endif
	}

// FIXME: Remove when MONOMAC is more up-to-date
#if MONOMAC
	static class Tuple {
		public static Tuple<T1, T2> Create<T1, T2>
			(
			 T1 item1,
			 T2 item2) {
			return new Tuple<T1, T2> (item1, item2);
		}
	}

	class Tuple<T1, T2>
	{
		T1 item1;
		T2 item2;

		public Tuple (T1 item1, T2 item2)
		{
			 this.item1 = item1;
			 this.item2 = item2;
		}

		public T1 Item1 {
			get { return item1; }
		}

		public T2 Item2 {
			get { return item2; }
		}
	}
#endif

	public struct DispatchTime
	{
		public static readonly DispatchTime Now = new DispatchTime ();
		public static readonly DispatchTime Forever = new DispatchTime (ulong.MaxValue);

		public DispatchTime (ulong nanoseconds)
			: this ()
		{
			this.Nanoseconds = nanoseconds;
		}

		public ulong Nanoseconds { get; private set; }

		// TODO: Bind more
	}

	public class DispatchGroup : DispatchObject
	{
		private DispatchGroup (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public static DispatchGroup Create ()
		{
			var ptr = dispatch_group_create ();
			if (ptr == IntPtr.Zero)
				return null;

			return new DispatchGroup (ptr, true);
		}

		public void DispatchAsync (DispatchQueue queue, Action action)
		{
			if (queue == null)
				throw new ArgumentNullException ("queue");
			if (action == null)
				throw new ArgumentNullException ("action");

			Check ();
			dispatch_group_async_f (handle, queue.handle, (IntPtr) GCHandle.Alloc (Tuple.Create (action, queue)), DispatchQueue.static_dispatch);
		}

		public void Enter ()
		{
			Check ();			
			dispatch_group_enter (handle);
		}

		public void Leave ()
		{
			Check ();
			dispatch_group_leave (handle);
		}

		public bool Wait (DispatchTime timeout)
		{
			Check ();			
			return dispatch_group_wait (handle, timeout.Nanoseconds) == IntPtr.Zero;
		}

		// TODO: dispatch_group_notify_f

		[DllImport ("libc")]
		extern static IntPtr dispatch_group_create ();

		[DllImport ("libc")]
		extern static void dispatch_group_async_f (IntPtr group, IntPtr queue, IntPtr context, DispatchQueue.dispatch_callback_t block);

		[DllImport ("libc")]
		extern static void dispatch_group_enter (IntPtr group);

		[DllImport ("libc")]
		extern static void dispatch_group_leave (IntPtr group);

		[DllImport ("libc")]
		// return IntPtr used for 32/64 bits
		extern static IntPtr dispatch_group_wait (IntPtr group, ulong timeout);
	}
}
