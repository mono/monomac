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
	[Register("CAAnimationGroup")]
	public partial class CAAnimationGroup : CAAnimation {
		static IntPtr selAnimations = Selector.GetHandle ("animations");
		static IntPtr selSetAnimations = Selector.GetHandle ("setAnimations:");
		static IntPtr selAnimation = Selector.GetHandle ("animation");

		static IntPtr class_ptr = Class.GetHandle ("CAAnimationGroup");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAAnimationGroup () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAAnimationGroup (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAAnimationGroup (NSObjectFlag t) : base (t) {}

		public CAAnimationGroup (IntPtr handle) : base (handle) {}

		[Export ("animation")]
		public new static CAAnimationGroup CreateAnimation ()
		{
			return (CAAnimationGroup) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selAnimation));
		}

		MonoMac.CoreAnimation.CAAnimation[] __mt_Animations_var;
		public virtual CAAnimation[] Animations {
			[Export ("animations", ArgumentSemantic.Copy)]
			get {
				CAAnimation[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CAAnimation>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selAnimations));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CAAnimation>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selAnimations));
				}
				__mt_Animations_var = ret;
				return ret;
			}

			[Export ("setAnimations:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetAnimations, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetAnimations, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_Animations_var = value;
			}
		}

	
	} /* class CAAnimationGroup */
}
