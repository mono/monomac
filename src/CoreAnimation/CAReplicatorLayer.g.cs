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
	[Register("CAReplicatorLayer")]
	public partial class CAReplicatorLayer : CALayer {
		static IntPtr selInstanceCount = Selector.GetHandle ("instanceCount");
		static IntPtr selSetInstanceCount = Selector.GetHandle ("setInstanceCount:");
		static IntPtr selInstanceDelay = Selector.GetHandle ("instanceDelay");
		static IntPtr selSetInstanceDelay = Selector.GetHandle ("setInstanceDelay:");
		static IntPtr selInstanceTransform = Selector.GetHandle ("instanceTransform");
		static IntPtr selSetInstanceTransform = Selector.GetHandle ("setInstanceTransform:");
		static IntPtr selPreservesDepth = Selector.GetHandle ("preservesDepth");
		static IntPtr selSetPreservesDepth = Selector.GetHandle ("setPreservesDepth:");
		static IntPtr selInstanceColor = Selector.GetHandle ("instanceColor");
		static IntPtr selSetInstanceColor = Selector.GetHandle ("setInstanceColor:");
		static IntPtr selInstanceRedOffset = Selector.GetHandle ("instanceRedOffset");
		static IntPtr selSetInstanceRedOffset = Selector.GetHandle ("setInstanceRedOffset:");
		static IntPtr selInstanceGreenOffset = Selector.GetHandle ("instanceGreenOffset");
		static IntPtr selSetInstanceGreenOffset = Selector.GetHandle ("setInstanceGreenOffset:");
		static IntPtr selInstanceBlueOffset = Selector.GetHandle ("instanceBlueOffset");
		static IntPtr selSetInstanceBlueOffset = Selector.GetHandle ("setInstanceBlueOffset:");
		static IntPtr selInstanceAlphaOffset = Selector.GetHandle ("instanceAlphaOffset");
		static IntPtr selSetInstanceAlphaOffset = Selector.GetHandle ("setInstanceAlphaOffset:");

		static IntPtr class_ptr = Class.GetHandle ("CAReplicatorLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAReplicatorLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAReplicatorLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAReplicatorLayer (NSObjectFlag t) : base (t) {}

		public CAReplicatorLayer (IntPtr handle) : base (handle) {}

		public virtual int InstanceCount {
			[Export ("instanceCount")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, selInstanceCount);
				} else {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSendSuper (this.SuperHandle, selInstanceCount);
				}
			}

			[Export ("setInstanceCount:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, selSetInstanceCount, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, selSetInstanceCount, value);
				}
			}
		}

		public virtual System.Double InstanceDelay {
			[Export ("instanceDelay")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (this.Handle, selInstanceDelay);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper (this.SuperHandle, selInstanceDelay);
				}
			}

			[Export ("setInstanceDelay:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (this.Handle, selSetInstanceDelay, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Double (this.SuperHandle, selSetInstanceDelay, value);
				}
			}
		}

		public virtual CATransform3D InstanceTransform {
			[Export ("instanceTransform")]
			get {
				CATransform3D ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.CATransform3D_objc_msgSend_stret (out ret, this.Handle, selInstanceTransform);
				} else {
					MonoMac.ObjCRuntime.Messaging.CATransform3D_objc_msgSendSuper_stret (out ret, this.SuperHandle, selInstanceTransform);
				}
				return ret;
			}

			[Export ("setInstanceTransform:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_CATransform3D (this.Handle, selSetInstanceTransform, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_CATransform3D (this.SuperHandle, selSetInstanceTransform, value);
				}
			}
		}

		public virtual bool PreservesDepth {
			[Export ("preservesDepth")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selPreservesDepth);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selPreservesDepth);
				}
			}

			[Export ("setPreservesDepth:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetPreservesDepth, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetPreservesDepth, value);
				}
			}
		}

		public virtual CGColor InstanceColor {
			[Export ("instanceColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selInstanceColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selInstanceColor));
				}
			}

			[Export ("setInstanceColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetInstanceColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetInstanceColor, value.handle);
				}
			}
		}

		public virtual float InstanceRedOffset {
			[Export ("instanceRedOffset")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selInstanceRedOffset);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selInstanceRedOffset);
				}
			}

			[Export ("setInstanceRedOffset:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetInstanceRedOffset, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetInstanceRedOffset, value);
				}
			}
		}

		public virtual float InstanceGreenOffset {
			[Export ("instanceGreenOffset")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selInstanceGreenOffset);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selInstanceGreenOffset);
				}
			}

			[Export ("setInstanceGreenOffset:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetInstanceGreenOffset, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetInstanceGreenOffset, value);
				}
			}
		}

		public virtual float InstanceBlueOffset {
			[Export ("instanceBlueOffset")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selInstanceBlueOffset);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selInstanceBlueOffset);
				}
			}

			[Export ("setInstanceBlueOffset:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetInstanceBlueOffset, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetInstanceBlueOffset, value);
				}
			}
		}

		public virtual float InstanceAlphaOffset {
			[Export ("instanceAlphaOffset")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selInstanceAlphaOffset);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selInstanceAlphaOffset);
				}
			}

			[Export ("setInstanceAlphaOffset:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetInstanceAlphaOffset, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetInstanceAlphaOffset, value);
				}
			}
		}

	
	} /* class CAReplicatorLayer */
}
