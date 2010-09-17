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
	[Register("CAGradientLayer")]
	public partial class CAGradientLayer : CALayer {
		static IntPtr selColors = Selector.GetHandle ("colors");
		static IntPtr selSetColors = Selector.GetHandle ("setColors:");
		static IntPtr selLocations = Selector.GetHandle ("locations");
		static IntPtr selSetLocations = Selector.GetHandle ("setLocations:");
		static IntPtr selStartPoint = Selector.GetHandle ("startPoint");
		static IntPtr selSetStartPoint = Selector.GetHandle ("setStartPoint:");
		static IntPtr selEndPoint = Selector.GetHandle ("endPoint");
		static IntPtr selSetEndPoint = Selector.GetHandle ("setEndPoint:");
		static IntPtr selType = Selector.GetHandle ("type");
		static IntPtr selSetType = Selector.GetHandle ("setType:");

		static IntPtr class_ptr = Class.GetHandle ("CAGradientLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAGradientLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAGradientLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAGradientLayer (NSObjectFlag t) : base (t) {}

		public CAGradientLayer (IntPtr handle) : base (handle) {}

		internal virtual System.IntPtr _Colors {
			[Export ("colors", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selColors);
				} else {
					return MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selColors);
				}
			}

			[Export ("setColors:", ArgumentSemantic.Copy)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetColors, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetColors, value);
				}
			}
		}

		MonoMac.Foundation.NSNumber[] __mt_Locations_var;
		public virtual NSNumber[] Locations {
			[Export ("locations", ArgumentSemantic.Copy)]
			get {
				NSNumber[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSNumber>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selLocations));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSNumber>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selLocations));
				}
				__mt_Locations_var = ret;
				return ret;
			}

			[Export ("setLocations:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetLocations, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetLocations, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_Locations_var = value;
			}
		}

		public virtual System.Drawing.PointF StartPoint {
			[Export ("startPoint")]
			get {
				System.Drawing.PointF ret;
				if (IsDirectBinding) {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSend (this.Handle, selStartPoint);
				} else {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSendSuper (this.SuperHandle, selStartPoint);
				}
				return ret;
			}

			[Export ("setStartPoint:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_PointF (this.Handle, selSetStartPoint, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_PointF (this.SuperHandle, selSetStartPoint, value);
				}
			}
		}

		public virtual System.Drawing.PointF EndPoint {
			[Export ("endPoint")]
			get {
				System.Drawing.PointF ret;
				if (IsDirectBinding) {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSend (this.Handle, selEndPoint);
				} else {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSendSuper (this.SuperHandle, selEndPoint);
				}
				return ret;
			}

			[Export ("setEndPoint:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_PointF (this.Handle, selSetEndPoint, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_PointF (this.SuperHandle, selSetEndPoint, value);
				}
			}
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

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _GradientLayerAxial;
		public static NSString GradientLayerAxial {
			get {
				if (_GradientLayerAxial == null)
					_GradientLayerAxial = Dlfcn.GetStringConstant (libraryHandle, "kCAGradientLayerAxial");
				return _GradientLayerAxial;
			}
		}
	
	} /* class CAGradientLayer */
}
