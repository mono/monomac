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
	[Register("CATextLayer")]
	public partial class CATextLayer : CALayer {
		static IntPtr selString = Selector.GetHandle ("string");
		static IntPtr selSetString = Selector.GetHandle ("setString:");
		static IntPtr selFontSize = Selector.GetHandle ("fontSize");
		static IntPtr selSetFontSize = Selector.GetHandle ("setFontSize:");
		static IntPtr selForegroundColor = Selector.GetHandle ("foregroundColor");
		static IntPtr selSetForegroundColor = Selector.GetHandle ("setForegroundColor:");
		static IntPtr selIsWrapped = Selector.GetHandle ("isWrapped");
		static IntPtr selSetWrapped = Selector.GetHandle ("setWrapped:");
		static IntPtr selTruncationMode = Selector.GetHandle ("truncationMode");
		static IntPtr selSetTruncationMode = Selector.GetHandle ("setTruncationMode:");
		static IntPtr selAlignmentMode = Selector.GetHandle ("alignmentMode");
		static IntPtr selSetAlignmentMode = Selector.GetHandle ("setAlignmentMode:");

		static IntPtr class_ptr = Class.GetHandle ("CATextLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CATextLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CATextLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CATextLayer (NSObjectFlag t) : base (t) {}

		public CATextLayer (IntPtr handle) : base (handle) {}

		public virtual string String {
			[Export ("string", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selString));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selString));
				}
			}

			[Export ("setString:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetString, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetString, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual float FontSize {
			[Export ("fontSize")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selFontSize);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selFontSize);
				}
			}

			[Export ("setFontSize:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetFontSize, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetFontSize, value);
				}
			}
		}

		public virtual CGColor ForegroundColor {
			[Export ("foregroundColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selForegroundColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selForegroundColor));
				}
			}

			[Export ("setForegroundColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetForegroundColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetForegroundColor, value.handle);
				}
			}
		}

		public virtual bool Wrapped {
			[Export ("isWrapped")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsWrapped);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsWrapped);
				}
			}

			[Export ("setWrapped:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetWrapped, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetWrapped, value);
				}
			}
		}

		public virtual string TruncationMode {
			[Export ("truncationMode", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selTruncationMode));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selTruncationMode));
				}
			}

			[Export ("setTruncationMode:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetTruncationMode, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetTruncationMode, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual string AlignmentMode {
			[Export ("alignmentMode", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selAlignmentMode));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selAlignmentMode));
				}
			}

			[Export ("setAlignmentMode:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetAlignmentMode, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetAlignmentMode, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _TruncationNone;
		public static NSString TruncationNone {
			get {
				if (_TruncationNone == null)
					_TruncationNone = Dlfcn.GetStringConstant (libraryHandle, "kCATruncationNone");
				return _TruncationNone;
			}
		}
		static NSString _TruncantionStart;
		public static NSString TruncantionStart {
			get {
				if (_TruncantionStart == null)
					_TruncantionStart = Dlfcn.GetStringConstant (libraryHandle, "kCATruncationStart");
				return _TruncantionStart;
			}
		}
		static NSString _TruncantionEnd;
		public static NSString TruncantionEnd {
			get {
				if (_TruncantionEnd == null)
					_TruncantionEnd = Dlfcn.GetStringConstant (libraryHandle, "kCATruncationEnd");
				return _TruncantionEnd;
			}
		}
		static NSString _TruncantionMiddle;
		public static NSString TruncantionMiddle {
			get {
				if (_TruncantionMiddle == null)
					_TruncantionMiddle = Dlfcn.GetStringConstant (libraryHandle, "kCATruncationMiddle");
				return _TruncantionMiddle;
			}
		}
		static NSString _AlignmentNatural;
		public static NSString AlignmentNatural {
			get {
				if (_AlignmentNatural == null)
					_AlignmentNatural = Dlfcn.GetStringConstant (libraryHandle, "kCAAlignmentNatural");
				return _AlignmentNatural;
			}
		}
		static NSString _AlignmentLeft;
		public static NSString AlignmentLeft {
			get {
				if (_AlignmentLeft == null)
					_AlignmentLeft = Dlfcn.GetStringConstant (libraryHandle, "kCAAlignmentLeft");
				return _AlignmentLeft;
			}
		}
		static NSString _AlignmentRight;
		public static NSString AlignmentRight {
			get {
				if (_AlignmentRight == null)
					_AlignmentRight = Dlfcn.GetStringConstant (libraryHandle, "kCAAlignmentRight");
				return _AlignmentRight;
			}
		}
		static NSString _AlignmentCenter;
		public static NSString AlignmentCenter {
			get {
				if (_AlignmentCenter == null)
					_AlignmentCenter = Dlfcn.GetStringConstant (libraryHandle, "kCAAlignmentCenter");
				return _AlignmentCenter;
			}
		}
		static NSString _AlignmentJustified;
		public static NSString AlignmentJustified {
			get {
				if (_AlignmentJustified == null)
					_AlignmentJustified = Dlfcn.GetStringConstant (libraryHandle, "kCAAlignmentJustified");
				return _AlignmentJustified;
			}
		}
	
	} /* class CATextLayer */
}
