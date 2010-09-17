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
	[Register("CATransition")]
	public partial class CATransition : CAAnimation {
		static IntPtr selType = Selector.GetHandle ("type");
		static IntPtr selSetType = Selector.GetHandle ("setType:");
		static IntPtr selSubtype = Selector.GetHandle ("subtype");
		static IntPtr selSetSubtype = Selector.GetHandle ("setSubtype:");
		static IntPtr selStartProgress = Selector.GetHandle ("startProgress");
		static IntPtr selSetStartProgress = Selector.GetHandle ("setStartProgress:");
		static IntPtr selEndProgress = Selector.GetHandle ("endProgress");
		static IntPtr selSetEndProgress = Selector.GetHandle ("setEndProgress:");
		static IntPtr selFilter = Selector.GetHandle ("filter");
		static IntPtr selSetFilter = Selector.GetHandle ("setFilter:");
		static IntPtr selAnimation = Selector.GetHandle ("animation");

		static IntPtr class_ptr = Class.GetHandle ("CATransition");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CATransition () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CATransition (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CATransition (NSObjectFlag t) : base (t) {}

		public CATransition (IntPtr handle) : base (handle) {}

		[Export ("animation")]
		public new static CATransition CreateAnimation ()
		{
			return (CATransition) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selAnimation));
		}

		public virtual string Type {
			[Export ("type", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selType));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selType));
				}
			}

			[Export ("setType:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetType, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetType, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual string Subtype {
			[Export ("subtype", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selSubtype));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selSubtype));
				}
			}

			[Export ("setSubtype:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetSubtype, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetSubtype, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual float StartProgress {
			[Export ("startProgress")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selStartProgress);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selStartProgress);
				}
			}

			[Export ("setStartProgress:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetStartProgress, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetStartProgress, value);
				}
			}
		}

		public virtual float EndProgress {
			[Export ("endProgress")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selEndProgress);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selEndProgress);
				}
			}

			[Export ("setEndProgress:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetEndProgress, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetEndProgress, value);
				}
			}
		}

		MonoMac.Foundation.NSObject __mt_filter_var;
		public virtual NSObject filter {
			[Export ("filter", ArgumentSemantic.Retain)]
			get {
				NSObject ret;
				if (IsDirectBinding) {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selFilter));
				} else {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selFilter));
				}
				__mt_filter_var = ret;
				return ret;
			}

			[Export ("setFilter:", ArgumentSemantic.Retain)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetFilter, value == null ? IntPtr.Zero : value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetFilter, value == null ? IntPtr.Zero : value.Handle);
				}
				__mt_filter_var = value;
			}
		}

	
	} /* class CATransition */
}
