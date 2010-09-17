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
	[Register("CALayerDelegate")]
	[Model]
	public partial class CALayerDelegate : NSObject {

		static IntPtr class_ptr = Class.GetHandle ("CALayerDelegate");

		[Export ("init")]
		public CALayerDelegate () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CALayerDelegate (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CALayerDelegate (NSObjectFlag t) : base (t) {}

		public CALayerDelegate (IntPtr handle) : base (handle) {}

		[Export ("displayLayer:")]
		public virtual void DisplayLayer (CALayer layer)
		{
			throw new You_Should_Not_Call_base_In_This_Method ();
		}

		[Export ("drawLayer:inContext:")]
		public virtual void DrawLayer (CALayer layer, CGContext context)
		{
			throw new You_Should_Not_Call_base_In_This_Method ();
		}

		[Export ("layoutSublayersOfLayer:")]
		public virtual void LayoutSublayersOfLayer (CALayer layer)
		{
			throw new You_Should_Not_Call_base_In_This_Method ();
		}

		[Export ("actionForLayer:forKey:")]
		public virtual CAAction ActionForLAyer (CALayer layer, string eventKey)
		{
			throw new You_Should_Not_Call_base_In_This_Method ();
		}

	
	} /* class CALayerDelegate */
}
