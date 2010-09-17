//
// Auto-generated from generator.cs, do not edit
//
using System;

using System.Drawing;

using System.Runtime.InteropServices;

using MonoMac.CoreFoundation;

using MonoMac.Foundation;

using MonoMac.ObjCRuntime;

using MonoMac.CoreGraphics;

using MonoMac.CoreAnimation;

namespace MonoMac.CoreAnimation {
	[Register("CADisplayLink")]
	public partial class CADisplayLink : NSObject {
		static IntPtr selTimestamp = Selector.GetHandle ("timestamp");
		static IntPtr selIsPaused = Selector.GetHandle ("isPaused");
		static IntPtr selSetPaused = Selector.GetHandle ("setPaused:");
		static IntPtr selFrameInterval = Selector.GetHandle ("frameInterval");
		static IntPtr selSetFrameInterval = Selector.GetHandle ("setFrameInterval:");
		static IntPtr selDisplayLinkWithTargetSelector = Selector.GetHandle ("displayLinkWithTarget:selector:");
		static IntPtr selAddToRunLoopForMode = Selector.GetHandle ("addToRunLoop:forMode:");
		static IntPtr selRemoveFromRunLoopForMode = Selector.GetHandle ("removeFromRunLoop:forMode:");
		static IntPtr selInvalidate = Selector.GetHandle ("invalidate");

		static IntPtr class_ptr = Class.GetHandle ("CADisplayLink");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CADisplayLink () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CADisplayLink (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CADisplayLink (NSObjectFlag t) : base (t) {}

		public CADisplayLink (IntPtr handle) : base (handle) {}

		[Export ("displayLinkWithTarget:selector:")]
		public static CADisplayLink Create (NSObject target, Selector sel)
		{
			if (target == null)
				throw new ArgumentNullException ("target");
			if (sel == null)
				throw new ArgumentNullException ("sel");
			return (CADisplayLink) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr, selDisplayLinkWithTargetSelector, target.Handle, sel.Handle));
		}

		[Export ("addToRunLoop:forMode:")]
		public virtual void AddToRunLoop (NSRunLoop runloop, string mode)
		{
			if (runloop == null)
				throw new ArgumentNullException ("runloop");
			if (mode == null)
				throw new ArgumentNullException ("mode");
			var nsmode = new NSString (mode);

			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selAddToRunLoopForMode, runloop.Handle, nsmode.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selAddToRunLoopForMode, runloop.Handle, nsmode.Handle);
			}
						nsmode.Dispose ();

		}

		[Export ("removeFromRunLoop:forMode:")]
		public virtual void RemoveFromRunLoop (NSRunLoop runloop, string mode)
		{
			if (runloop == null)
				throw new ArgumentNullException ("runloop");
			if (mode == null)
				throw new ArgumentNullException ("mode");
			var nsmode = new NSString (mode);

			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selRemoveFromRunLoopForMode, runloop.Handle, nsmode.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selRemoveFromRunLoopForMode, runloop.Handle, nsmode.Handle);
			}
						nsmode.Dispose ();

		}

		[Export ("invalidate")]
		public virtual void Invalidate ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selInvalidate);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selInvalidate);
			}
		}

		public virtual System.Double Timestamp {
			[Export ("timestamp")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (this.Handle, selTimestamp);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper (this.SuperHandle, selTimestamp);
				}
			}

		}

		public virtual bool Paused {
			[Export ("isPaused")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsPaused);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsPaused);
				}
			}

			[Export ("setPaused:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetPaused, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetPaused, value);
				}
			}
		}

		public virtual int FrameInterval {
			[Export ("frameInterval")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, selFrameInterval);
				} else {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSendSuper (this.SuperHandle, selFrameInterval);
				}
			}

			[Export ("setFrameInterval:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, selSetFrameInterval, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, selSetFrameInterval, value);
				}
			}
		}

	
	} /* class CADisplayLink */
}
