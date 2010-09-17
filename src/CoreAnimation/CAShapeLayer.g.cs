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
	[Register("CAShapeLayer")]
	public partial class CAShapeLayer : CALayer {
		static IntPtr selPath = Selector.GetHandle ("path");
		static IntPtr selSetPath = Selector.GetHandle ("setPath:");
		static IntPtr selFillColor = Selector.GetHandle ("fillColor");
		static IntPtr selSetFillColor = Selector.GetHandle ("setFillColor:");
		static IntPtr selFillRule = Selector.GetHandle ("fillRule");
		static IntPtr selSetFillRule = Selector.GetHandle ("setFillRule:");
		static IntPtr selLineCap = Selector.GetHandle ("lineCap");
		static IntPtr selSetLineCap = Selector.GetHandle ("setLineCap:");
		static IntPtr selLineDashPattern = Selector.GetHandle ("lineDashPattern");
		static IntPtr selSetLineDashPattern = Selector.GetHandle ("setLineDashPattern:");
		static IntPtr selLineDashPhase = Selector.GetHandle ("lineDashPhase");
		static IntPtr selSetLineDashPhase = Selector.GetHandle ("setLineDashPhase:");
		static IntPtr selLineJoin = Selector.GetHandle ("lineJoin");
		static IntPtr selSetLineJoin = Selector.GetHandle ("setLineJoin:");
		static IntPtr selLineWidth = Selector.GetHandle ("lineWidth");
		static IntPtr selSetLineWidth = Selector.GetHandle ("setLineWidth:");
		static IntPtr selMiterLimit = Selector.GetHandle ("miterLimit");
		static IntPtr selSetMiterLimit = Selector.GetHandle ("setMiterLimit:");
		static IntPtr selStrokeColor = Selector.GetHandle ("strokeColor");
		static IntPtr selSetStrokeColor = Selector.GetHandle ("setStrokeColor:");

		static IntPtr class_ptr = Class.GetHandle ("CAShapeLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAShapeLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAShapeLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAShapeLayer (NSObjectFlag t) : base (t) {}

		public CAShapeLayer (IntPtr handle) : base (handle) {}

		public virtual CGPath Path {
			[Export ("path")]
			get {
				if (IsDirectBinding) {
					return new CGPath (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selPath));
				} else {
					return new CGPath (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selPath));
				}
			}

			[Export ("setPath:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetPath, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetPath, value.handle);
				}
			}
		}

		public virtual CGColor FillColor {
			[Export ("fillColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selFillColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selFillColor));
				}
			}

			[Export ("setFillColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetFillColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetFillColor, value.handle);
				}
			}
		}

		public virtual string FillRule {
			[Export ("fillRule", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selFillRule));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selFillRule));
				}
			}

			[Export ("setFillRule:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetFillRule, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetFillRule, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual string LineCap {
			[Export ("lineCap", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selLineCap));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selLineCap));
				}
			}

			[Export ("setLineCap:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetLineCap, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetLineCap, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		MonoMac.Foundation.NSNumber[] __mt_LineDashPattern_var;
		public virtual NSNumber[] LineDashPattern {
			[Export ("lineDashPattern", ArgumentSemantic.Copy)]
			get {
				NSNumber[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSNumber>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selLineDashPattern));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSNumber>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selLineDashPattern));
				}
				__mt_LineDashPattern_var = ret;
				return ret;
			}

			[Export ("setLineDashPattern:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetLineDashPattern, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetLineDashPattern, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_LineDashPattern_var = value;
			}
		}

		public virtual float LineDashPhase {
			[Export ("lineDashPhase")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selLineDashPhase);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selLineDashPhase);
				}
			}

			[Export ("setLineDashPhase:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetLineDashPhase, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetLineDashPhase, value);
				}
			}
		}

		public virtual string LineJoin {
			[Export ("lineJoin", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selLineJoin));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selLineJoin));
				}
			}

			[Export ("setLineJoin:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetLineJoin, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetLineJoin, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual float LineWidth {
			[Export ("lineWidth")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selLineWidth);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selLineWidth);
				}
			}

			[Export ("setLineWidth:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetLineWidth, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetLineWidth, value);
				}
			}
		}

		public virtual float MiterLimit {
			[Export ("miterLimit")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selMiterLimit);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selMiterLimit);
				}
			}

			[Export ("setMiterLimit:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetMiterLimit, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetMiterLimit, value);
				}
			}
		}

		public virtual CGColor StrokeColor {
			[Export ("strokeColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selStrokeColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selStrokeColor));
				}
			}

			[Export ("setStrokeColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetStrokeColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetStrokeColor, value.handle);
				}
			}
		}

	
	} /* class CAShapeLayer */
}
