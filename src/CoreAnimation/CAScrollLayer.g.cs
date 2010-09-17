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
	[Register("CAScrollLayer")]
	public partial class CAScrollLayer : CALayer {
		static IntPtr selScrollMode = Selector.GetHandle ("scrollMode");
		static IntPtr selSetScrollMode = Selector.GetHandle ("setScrollMode:");
		static IntPtr selScrollToPoint = Selector.GetHandle ("scrollToPoint:");
		static IntPtr selScrollToRect = Selector.GetHandle ("scrollToRect:");

		static IntPtr class_ptr = Class.GetHandle ("CAScrollLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAScrollLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAScrollLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAScrollLayer (NSObjectFlag t) : base (t) {}

		public CAScrollLayer (IntPtr handle) : base (handle) {}

		[Export ("scrollToPoint:")]
		public virtual void ScrollToPoint (System.Drawing.PointF p)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_PointF (this.Handle, selScrollToPoint, p);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_PointF (this.SuperHandle, selScrollToPoint, p);
			}
		}

		[Export ("scrollToRect:")]
		public virtual void ScrollToRect (System.Drawing.RectangleF r)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selScrollToRect, r);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selScrollToRect, r);
			}
		}

		public virtual string scrollMode {
			[Export ("scrollMode")]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selScrollMode));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selScrollMode));
				}
			}

			[Export ("setScrollMode:")]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetScrollMode, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetScrollMode, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

	
	} /* class CAScrollLayer */
}
