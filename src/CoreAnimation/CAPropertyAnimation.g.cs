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
	[Register("CAPropertyAnimation")]
	public partial class CAPropertyAnimation : CAAnimation {
		static IntPtr selKeyPath = Selector.GetHandle ("keyPath");
		static IntPtr selSetKeyPath = Selector.GetHandle ("setKeyPath:");
		static IntPtr selIsAdditive = Selector.GetHandle ("isAdditive");
		static IntPtr selSetAdditive = Selector.GetHandle ("setAdditive:");
		static IntPtr selIsCumulative = Selector.GetHandle ("isCumulative");
		static IntPtr selSetCumulative = Selector.GetHandle ("setCumulative:");
		static IntPtr selValueFunction = Selector.GetHandle ("valueFunction");
		static IntPtr selSetValueFunction = Selector.GetHandle ("setValueFunction:");
		static IntPtr selAnimationWithKeyPath = Selector.GetHandle ("animationWithKeyPath:");

		static IntPtr class_ptr = Class.GetHandle ("CAPropertyAnimation");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAPropertyAnimation () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAPropertyAnimation (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAPropertyAnimation (NSObjectFlag t) : base (t) {}

		public CAPropertyAnimation (IntPtr handle) : base (handle) {}

		[Export ("animationWithKeyPath:")]
		public static CAPropertyAnimation FromKeyPath (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			var nspath = new NSString (path);

			CAPropertyAnimation ret;
			ret = (CAPropertyAnimation) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selAnimationWithKeyPath, nspath.Handle));
						nspath.Dispose ();

			return ret;
		}

		public virtual string KeyPath {
			[Export ("keyPath", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selKeyPath));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selKeyPath));
				}
			}

			[Export ("setKeyPath:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetKeyPath, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetKeyPath, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual bool Additive {
			[Export ("isAdditive")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsAdditive);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsAdditive);
				}
			}

			[Export ("setAdditive:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetAdditive, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetAdditive, value);
				}
			}
		}

		public virtual bool Cumulative {
			[Export ("isCumulative")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsCumulative);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsCumulative);
				}
			}

			[Export ("setCumulative:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetCumulative, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetCumulative, value);
				}
			}
		}

		MonoMac.CoreAnimation.CAValueFunction __mt_ValueFunction_var;
		public virtual CAValueFunction ValueFunction {
			[Export ("valueFunction", ArgumentSemantic.Retain)]
			get {
				CAValueFunction ret;
				if (IsDirectBinding) {
					ret = (CAValueFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selValueFunction));
				} else {
					ret = (CAValueFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selValueFunction));
				}
				__mt_ValueFunction_var = ret;
				return ret;
			}

			[Export ("setValueFunction:", ArgumentSemantic.Retain)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetValueFunction, value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetValueFunction, value.Handle);
				}
				__mt_ValueFunction_var = value;
			}
		}

	
	} /* class CAPropertyAnimation */
}
