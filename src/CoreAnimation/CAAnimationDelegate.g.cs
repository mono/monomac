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
	[Register("CAAnimationDelegate")]
	[Model]
	public partial class CAAnimationDelegate : NSObject {

		static IntPtr class_ptr = Class.GetHandle ("CAAnimationDelegate");

		[Export ("init")]
		public CAAnimationDelegate () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAAnimationDelegate (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAAnimationDelegate (NSObjectFlag t) : base (t) {}

		public CAAnimationDelegate (IntPtr handle) : base (handle) {}

		[Export ("animationDidStart:")]
		public virtual void AnimationStarted (CAAnimation anim)
		{
			throw new You_Should_Not_Call_base_In_This_Method ();
		}

		[Export ("animationDidStop:finished:")]
		public virtual void AnimationStopped (CAAnimation anim, bool finished)
		{
			throw new You_Should_Not_Call_base_In_This_Method ();
		}

	
	} /* class CAAnimationDelegate */
}
