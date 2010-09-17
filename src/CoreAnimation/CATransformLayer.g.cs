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
	[Register("CATransformLayer")]
	public partial class CATransformLayer : CALayer {
		static IntPtr selHitTest = Selector.GetHandle ("hitTest:");

		static IntPtr class_ptr = Class.GetHandle ("CATransformLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CATransformLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CATransformLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CATransformLayer (NSObjectFlag t) : base (t) {}

		public CATransformLayer (IntPtr handle) : base (handle) {}

		[Export ("hitTest:")]
		public virtual CALayer HitTest (System.Drawing.PointF thePoint)
		{
			if (IsDirectBinding) {
				return (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_PointF (this.Handle, selHitTest, thePoint));
			} else {
				return (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_PointF (this.SuperHandle, selHitTest, thePoint));
			}
		}

	
	} /* class CATransformLayer */
}
