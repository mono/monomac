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
	[Register("CAEAGLLayer")]
	public partial class CAEAGLLayer : CALayer {
		static IntPtr selDrawableProperties = Selector.GetHandle ("drawableProperties");
		static IntPtr selSetDrawableProperties = Selector.GetHandle ("setDrawableProperties:");

		static IntPtr class_ptr = Class.GetHandle ("CAEAGLLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAEAGLLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAEAGLLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAEAGLLayer (NSObjectFlag t) : base (t) {}

		public CAEAGLLayer (IntPtr handle) : base (handle) {}

		MonoMac.Foundation.NSDictionary __mt_DrawableProperties_var;
		public virtual NSDictionary DrawableProperties {
			[Export ("drawableProperties", ArgumentSemantic.Copy)]
			get {
				NSDictionary ret;
				if (IsDirectBinding) {
					ret = (NSDictionary) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selDrawableProperties));
				} else {
					ret = (NSDictionary) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selDrawableProperties));
				}
				__mt_DrawableProperties_var = ret;
				return ret;
			}

			[Export ("setDrawableProperties:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetDrawableProperties, value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetDrawableProperties, value.Handle);
				}
				__mt_DrawableProperties_var = value;
			}
		}

	
	} /* class CAEAGLLayer */
}
