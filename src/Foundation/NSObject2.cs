// Copyright 2011 - 2013 Xamarin Inc
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
#if !MONOMAC
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
#endif
using MonoMac.CoreGraphics;

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif


namespace MonoMac.Foundation {
	public class NSObjectFlag {
		public static readonly NSObjectFlag Empty;
		
		NSObjectFlag () {}
	}

	public partial class NSObject {
		const string selConformsToProtocol = "conformsToProtocol:";
		const string selEncodeWithCoder = "encodeWithCoder:";
		const string selAwakeFromNib = "awakeFromNib";
		const string selRespondsToSelector = "respondsToSelector:";

#if MONOMAC
		static IntPtr selConformsToProtocolHandle = Selector.GetHandle (selConformsToProtocol);
		static IntPtr selEncodeWithCoderHandle = Selector.GetHandle (selEncodeWithCoder);
		static IntPtr selAwakeFromNibHandle = Selector.GetHandle (selAwakeFromNib);
		static IntPtr selRespondsToSelectorHandle = Selector.GetHandle (selRespondsToSelector);
#endif

		IntPtr handle;
		IntPtr super;
		bool disposed;

		protected bool IsDirectBinding;

#if COREBUILD
		static readonly IntPtr class_ptr = Class.GetHandle ("NSObject");
		public virtual IntPtr ClassHandle  { get { return class_ptr; } }
#endif
		
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
		
		public NSObject (IntPtr handle, bool alloced) {
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

#if !COREBUILD
		[Export ("encodeWithCoder:")]
		public virtual void EncodeTo (NSCoder coder)
		{
			if (coder == null)
				throw new ArgumentNullException ("coder");

#if MONOMAC
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr (this.Handle, selEncodeWithCoderHandle, coder.Handle);
			} else {
				Messaging.void_objc_msgSendSuper_intptr (this.SuperHandle, selEncodeWithCoderHandle, coder.Handle);
			}
#else
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr (this.Handle, Selector.GetHandle (selEncodeWithCoder), coder.Handle);
			} else {
				Messaging.void_objc_msgSendSuper_intptr (this.SuperHandle, Selector.GetHandle (selEncodeWithCoder), coder.Handle);
			}
#endif
		}
#endif

		[Export ("conformsToProtocol:")]
		[Preserve ()]
		public virtual bool ConformsToProtocol (IntPtr protocol)
		{
			bool does;

#if MONOMAC
			if (IsDirectBinding) {
				does = Messaging.bool_objc_msgSend_intptr (this.Handle, selConformsToProtocolHandle, protocol);
			} else {
				does = Messaging.bool_objc_msgSendSuper_intptr (this.SuperHandle, selConformsToProtocolHandle, protocol);
			}
#else
			if (IsDirectBinding) {
				does = Messaging.bool_objc_msgSend_intptr (this.Handle, Selector.GetHandle (selConformsToProtocol), protocol);
			} else {
				does = Messaging.bool_objc_msgSendSuper_intptr (this.SuperHandle, Selector.GetHandle (selConformsToProtocol), protocol);
			}
#endif

			if (does)
				return true;
			
			object [] adoptedProtocols = GetType ().GetCustomAttributes (typeof (AdoptsAttribute), true);
			foreach (AdoptsAttribute adopts in adoptedProtocols){
				if (adopts.ProtocolHandle == protocol)
					return true;
			}
			return false;
		}

		[Export ("respondsToSelector:")]
		public virtual bool RespondsToSelector (Selector sel) {
#if MONOMAC
			if (IsDirectBinding) {
				return Messaging.bool_objc_msgSend_intptr (this.Handle, selRespondsToSelectorHandle, sel.Handle);
			} else {
				return Messaging.bool_objc_msgSendSuper_intptr (this.SuperHandle, selRespondsToSelectorHandle, sel.Handle);
			}
#else
			if (IsDirectBinding) {
				return Messaging.bool_objc_msgSend_intptr (this.Handle, Selector.GetHandle (selRespondsToSelector), sel.Handle);
			} else {
				return Messaging.bool_objc_msgSendSuper_intptr (this.SuperHandle, Selector.GetHandle (selRespondsToSelector), sel.Handle);
			}
#endif
		}
		
		[Export ("doesNotRecognizeSelector:")]
		public virtual void DoesNotRecognizeSelector (Selector sel) {
#if MONOMAC
			Messaging.void_objc_msgSendSuper_intptr (SuperHandle, Selector.DoesNotRecognizeSelectorHandle, sel.Handle);
			#else
			Messaging.void_objc_msgSendSuper_intptr (SuperHandle, Selector.GetHandle (Selector.DoesNotRecognizeSelector), sel.Handle);
#endif
		}
		
		public void Release () {
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.ReleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Release));
#endif
		}
		
		public NSObject Retain () {
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.RetainHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Retain));
#endif
			return this;
		}

		public NSObject Autorelease ()
		{
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.AutoreleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Autorelease));
#endif
			return this;
		}
		
		public IntPtr SuperHandle {
			get {
				if (super == IntPtr.Zero) {
					super = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (objc_super)));
					unsafe {
						objc_super *sup = (objc_super *) super;
						sup->receiver = handle;
						sup->super = ClassHandle;
					}
				}
				return super;
			}
		}
		
		public IntPtr Handle {
			get { return handle; }
			set {
				if (handle == value)
					return;
				
				if (handle != IntPtr.Zero)
					Runtime.UnregisterNSObject (handle);
				
				handle = value;
				
				if (handle != IntPtr.Zero)
					Runtime.RegisterNSObject (this, handle);
			}
		}
		
		private bool AllocIfNeeded () {
			if (handle == IntPtr.Zero) {
#if MONOMAC
				handle = Messaging.intptr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.AllocHandle);
				#else
				handle = Messaging.intptr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.GetHandle (Selector.Alloc));
#endif
				return true;
			}
			return false;
		}
		
		private IntPtr GetObjCIvar (string name) {
			IntPtr native;
			
			object_getInstanceVariable (handle, name, out native);
			
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
		
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_getInstanceVariable (IntPtr obj, string name, out IntPtr val);

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_setInstanceVariable (IntPtr obj, string name, IntPtr val);
		
		struct objc_super {
			public IntPtr receiver;
			public IntPtr super;
		}
		
		[Export ("performSelector:withObject:afterDelay:")]
		public virtual void PerformSelector (Selector sel, NSObject obj, double delay) {
			if (sel == null)
				throw new ArgumentNullException ("sel");
#if MONOMAC
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr_intptr_double (this.Handle, Selector.PerformSelectorWithObjectAfterDelayHandle, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			} else {
				Messaging.void_objc_msgSendSuper_intptr_intptr_double (this.SuperHandle, Selector.PerformSelectorWithObjectAfterDelayHandle, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			}
#else
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr_intptr_double (this.Handle, Selector.GetHandle (Selector.PerformSelectorWithObjectAfterDelay), sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			} else {
				Messaging.void_objc_msgSendSuper_intptr_intptr_double (this.SuperHandle, Selector.GetHandle (Selector.PerformSelectorWithObjectAfterDelay), sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			}
#endif
		}
		
		[Export ("awakeFromNib")]
		public virtual void AwakeFromNib ()
		{
#if MONOMAC
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend (this.Handle, selAwakeFromNibHandle);
			} else {
				Messaging.void_objc_msgSendSuper (this.SuperHandle, selAwakeFromNibHandle);
			}
#else
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend (this.Handle, Selector.GetHandle (selAwakeFromNib));
			} else {
				Messaging.void_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle (selAwakeFromNib));
			}
#endif
		}
		
		private void InvokeOnMainThread (Selector sel, NSObject obj, bool wait)
		{
#if MONOMAC
			Messaging.void_objc_msgSend_intptr_intptr_bool (this.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, wait);
#else
			Messaging.void_objc_msgSend_intptr_intptr_bool (this.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, wait);
#endif
		}
		
		public void BeginInvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, false);
		}
		
		public void InvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, true);
		}
		
		public void BeginInvokeOnMainThread (Action action)
		{
			var d = new NSAsyncActionDispatcher (action);
#if MONOMAC
			Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, 
			                                                NSActionDispatcher.Selector.Handle, d.Handle, false);
#else
			Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), 
			                                                Selector.GetHandle (NSActionDispatcher.SelectorName), d.Handle, false);
#endif
		}
		
		
		public void InvokeOnMainThread (Action action)
		{
			using (var d = new NSActionDispatcher (action)) {
#if MONOMAC
				Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, 
				                                                NSActionDispatcher.Selector.Handle, d.Handle, true);
#else
				Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), 
				                                                Selector.GetHandle (NSActionDispatcher.SelectorName), d.Handle, true);
#endif
			}
		}
		
#if !COREBUILD		
		[Export ("retainCount")]
		public virtual int RetainCount {
			get {
#if MONOMAC
				if (IsDirectBinding) {
					return Messaging.int_objc_msgSend (this.Handle, Selector.RetainCount);
				} else {
					return Messaging.int_objc_msgSendSuper (this.SuperHandle, Selector.RetainCount);
				}
#else
				if (IsDirectBinding) {
					return Messaging.int_objc_msgSend (this.Handle, Selector.GetHandle ("retainCount"));
				} else {
					return Messaging.int_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("retainCount"));
				}
#endif
			}
		}
#endif

#if !COREBUILD
		public static NSObject FromObject (object obj)
		{
			if (obj == null)
				return NSNull.Null;
			var t = obj.GetType ();
			if (t == typeof (NSObject) || t.IsSubclassOf (typeof (NSObject)))
				return (NSObject) obj;
			
			switch (Type.GetTypeCode (t)){
			case TypeCode.Boolean:
				return new NSNumber ((bool) obj);
			case TypeCode.Char:
				return new NSNumber ((ushort) (char) obj);
			case TypeCode.SByte:
				return new NSNumber ((sbyte) obj);
			case TypeCode.Byte:
				return new NSNumber ((byte) obj);
			case TypeCode.Int16:
				return new NSNumber ((short) obj);
			case TypeCode.UInt16:
				return new NSNumber ((ushort) obj);
			case TypeCode.Int32:
				return new NSNumber ((int) obj);
			case TypeCode.UInt32:
				return new NSNumber ((uint) obj);
			case TypeCode.Int64:
				return new NSNumber ((long) obj);
			case TypeCode.UInt64:
				return new NSNumber ((ulong) obj);
			case TypeCode.Single:
				return new NSNumber ((float) obj);
			case TypeCode.Double:
				return new NSNumber ((double) obj);
			case TypeCode.String:
				return new NSString ((string) obj);
			default:
				if (t == typeof (IntPtr))
					return NSValue.ValueFromPointer ((IntPtr) obj);
#if MAC64
				if (t == typeof (CGSize))
					return NSValue.FromSize ((CGSize) obj);
				else if (t == typeof (CGRect))
					return NSValue.FromRectangle ((CGRect) obj);
				else if (t == typeof (CGPoint))
					return NSValue.FromPoint ((CGPoint) obj);
#else
				if (t == typeof (System.Drawing.SizeF))
					return NSValue.FromSizeF ((System.Drawing.SizeF) obj);
				else if (t == typeof (System.Drawing.RectangleF))
					return NSValue.FromRectangleF ((System.Drawing.RectangleF) obj);
				else if (t == typeof (System.Drawing.PointF))
					return NSValue.FromPointF ((System.Drawing.PointF) obj);
#endif
#if !MONOMAC
				if (t == typeof (CGAffineTransform))
					return NSValue.FromCGAffineTransform ((CGAffineTransform) obj);
				else if (t == typeof (UIEdgeInsets))
					return NSValue.FromUIEdgeInsets ((UIEdgeInsets) obj);
				else if (t == typeof (CATransform3D))
					return NSValue.FromCATransform3D ((CATransform3D) obj);
#endif
				// last chance for types like CGPath, CGColor... that are not NSObject but are CFObject
				// see https://bugzilla.xamarin.com/show_bug.cgi?id=8458
				INativeObject native = (obj as INativeObject);
				if (native != null)
					return Runtime.GetNSObject (native.Handle);
				return null;
			}
		}

		public void SetValueForKeyPath (IntPtr handle, NSString keyPath)
		{
			if (keyPath == null)
				throw new ArgumentNullException ("keyPath");
#if MONOMAC
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetValueForKeyPath_Handle, handle, keyPath.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selSetValueForKeyPath_Handle, handle, keyPath.Handle);
			}
#else
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle (selSetValueForKeyPath_), handle, keyPath.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, Selector.GetHandle (selSetValueForKeyPath_), handle, keyPath.Handle);
			}
#endif
		}

		public override string ToString ()
		{
			return Description ?? base.ToString ();
		}
#endif

		public virtual void Invoke (Action action, double delay)
		{
			var d = new NSAsyncActionDispatcher (action);
			PerformSelector (NSActionDispatcher.Selector, d, delay);
		}

		public virtual void Invoke (Action action, TimeSpan delay)
		{
			var d = new NSAsyncActionDispatcher (action);
			PerformSelector (NSActionDispatcher.Selector, d, delay.TotalSeconds);
		}
		
		internal void ClearHandle ()
		{
			handle = IntPtr.Zero;
		}
		
		protected virtual void Dispose (bool disposing) {
			if (disposed)
				return;
			disposed = true;
			
			if (handle != IntPtr.Zero) {
				if (disposing) {
					ReleaseManagedRef ();
				} else {
					NSObject_Disposer.Add (this);
				}
			}
			if (super != IntPtr.Zero) {
				Marshal.FreeHGlobal (super);
				super = IntPtr.Zero;
			}
		}

		[Register ("__NSObject_Disposer")]
		[Preserve (AllMembers=true)]
		internal class NSObject_Disposer : NSObject {
			static readonly List <NSObject> drainList1 = new List<NSObject> ();
			static readonly List <NSObject> drainList2 = new List<NSObject> ();
			static List <NSObject> handles = drainList1;
			
			new static readonly IntPtr class_ptr = Class.GetHandle ("__NSObject_Disposer");
#if MONOMAC
			static readonly IntPtr drainHandle = Selector.GetHandle ("drain:");
#endif
			
			new static readonly object lock_obj = new object ();
			
			private NSObject_Disposer ()
			{
				// Disable default ctor, there should be no instances of this class.
			}
			
			static internal void Add (NSObject handle) {
				bool call_drain;
				lock (lock_obj) {
					handles.Add (handle);
					call_drain = handles.Count == 1;
				}
				if (!call_drain)
					return;
#if MONOMAC
				Messaging.void_objc_msgSend_intptr_intptr_bool (class_ptr, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, drainHandle, IntPtr.Zero, false);
#else
				Messaging.void_objc_msgSend_intptr_intptr_bool (class_ptr, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), Selector.GetHandle ("drain:"), IntPtr.Zero, false);
#endif
			}
			
			[Export ("drain:")]
			static  void Drain (NSObject ctx) {
				List<NSObject> drainList;
				
				lock (lock_obj) {
					drainList = handles;
					if (handles == drainList1)
						handles = drainList2;
					else
						handles = drainList1;
				}
				
				foreach (NSObject x in drainList)
					x.ReleaseManagedRef ();
				drainList.Clear();
			}
		}
	}
}
