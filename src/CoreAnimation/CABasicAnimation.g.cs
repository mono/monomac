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
	[Register("CABasicAnimation")]
	public partial class CABasicAnimation : CAPropertyAnimation {
		static IntPtr selFromValue = Selector.GetHandle ("fromValue");
		static IntPtr selSetFromValue = Selector.GetHandle ("setFromValue:");
		static IntPtr selToValue = Selector.GetHandle ("toValue");
		static IntPtr selSetToValue = Selector.GetHandle ("setToValue:");
		static IntPtr selByValue = Selector.GetHandle ("byValue");
		static IntPtr selSetByValue = Selector.GetHandle ("setByValue:");
		static IntPtr selAnimationWithKeyPath = Selector.GetHandle ("animationWithKeyPath:");

		static IntPtr class_ptr = Class.GetHandle ("CABasicAnimation");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CABasicAnimation () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CABasicAnimation (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CABasicAnimation (NSObjectFlag t) : base (t) {}

		public CABasicAnimation (IntPtr handle) : base (handle) {}

		[Export ("animationWithKeyPath:")]
		public new static CABasicAnimation FromKeyPath (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			var nspath = new NSString (path);

			CABasicAnimation ret;
			ret = (CABasicAnimation) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selAnimationWithKeyPath, nspath.Handle));
						nspath.Dispose ();

			return ret;
		}

		MonoMac.Foundation.NSObject __mt_From_var;
		public virtual NSObject From {
			[Export ("fromValue", ArgumentSemantic.Retain)]
			get {
				NSObject ret;
				if (IsDirectBinding) {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selFromValue));
				} else {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selFromValue));
				}
				__mt_From_var = ret;
				return ret;
			}

			[Export ("setFromValue:", ArgumentSemantic.Retain)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetFromValue, value == null ? IntPtr.Zero : value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetFromValue, value == null ? IntPtr.Zero : value.Handle);
				}
				__mt_From_var = value;
			}
		}

		MonoMac.Foundation.NSObject __mt_To_var;
		public virtual NSObject To {
			[Export ("toValue", ArgumentSemantic.Retain)]
			get {
				NSObject ret;
				if (IsDirectBinding) {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selToValue));
				} else {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selToValue));
				}
				__mt_To_var = ret;
				return ret;
			}

			[Export ("setToValue:", ArgumentSemantic.Retain)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetToValue, value == null ? IntPtr.Zero : value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetToValue, value == null ? IntPtr.Zero : value.Handle);
				}
				__mt_To_var = value;
			}
		}

		MonoMac.Foundation.NSObject __mt_By_var;
		public virtual NSObject By {
			[Export ("byValue", ArgumentSemantic.Retain)]
			get {
				NSObject ret;
				if (IsDirectBinding) {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selByValue));
				} else {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selByValue));
				}
				__mt_By_var = ret;
				return ret;
			}

			[Export ("setByValue:", ArgumentSemantic.Retain)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetByValue, value == null ? IntPtr.Zero : value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetByValue, value == null ? IntPtr.Zero : value.Handle);
				}
				__mt_By_var = value;
			}
		}

	
	} /* class CABasicAnimation */
}
