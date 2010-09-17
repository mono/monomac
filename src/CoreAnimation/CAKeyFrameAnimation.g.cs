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
	[Register("CAKeyframeAnimation")]
	public partial class CAKeyFrameAnimation : CAPropertyAnimation {
		static IntPtr selValues = Selector.GetHandle ("values");
		static IntPtr selSetValues = Selector.GetHandle ("setValues:");
		static IntPtr selPath = Selector.GetHandle ("path");
		static IntPtr selSetPath = Selector.GetHandle ("setPath:");
		static IntPtr selKeyTimes = Selector.GetHandle ("keyTimes");
		static IntPtr selSetKeyTimes = Selector.GetHandle ("setKeyTimes:");
		static IntPtr selTimingFunctions = Selector.GetHandle ("timingFunctions");
		static IntPtr selSetTimingFunctions = Selector.GetHandle ("setTimingFunctions:");
		static IntPtr selCalculationMode = Selector.GetHandle ("calculationMode");
		static IntPtr selSetCalculationMode = Selector.GetHandle ("setCalculationMode:");
		static IntPtr selRotationMode = Selector.GetHandle ("rotationMode");
		static IntPtr selSetRotationMode = Selector.GetHandle ("setRotationMode:");
		static IntPtr selAnimationWithKeyPath = Selector.GetHandle ("animationWithKeyPath:");

		static IntPtr class_ptr = Class.GetHandle ("CAKeyframeAnimation");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAKeyFrameAnimation () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAKeyFrameAnimation (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAKeyFrameAnimation (NSObjectFlag t) : base (t) {}

		public CAKeyFrameAnimation (IntPtr handle) : base (handle) {}

		[Export ("animationWithKeyPath:")]
		public new static CAPropertyAnimation FromKeyPath (string path)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			var nspath = new NSString (path);

			CAPropertyAnimation ret;
			ret = (CAPropertyAnimation) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selAnimationWithKeyPath, nspath.Handle));
						nspath.Dispose ();

			return ret;
		}

		MonoMac.Foundation.NSObject[] __mt_Values_var;
		public virtual NSObject[] Values {
			[Export ("values", ArgumentSemantic.Copy)]
			get {
				NSObject[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSObject>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selValues));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSObject>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selValues));
				}
				__mt_Values_var = ret;
				return ret;
			}

			[Export ("setValues:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetValues, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetValues, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_Values_var = value;
			}
		}

		internal virtual System.IntPtr _Path {
			[Export ("path")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selPath);
				} else {
					return MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selPath);
				}
			}

			[Export ("setPath:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetPath, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetPath, value);
				}
			}
		}

		MonoMac.Foundation.NSNumber[] __mt_KeyTimes_var;
		public virtual NSNumber[] KeyTimes {
			[Export ("keyTimes", ArgumentSemantic.Copy)]
			get {
				NSNumber[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSNumber>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selKeyTimes));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.Foundation.NSNumber>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selKeyTimes));
				}
				__mt_KeyTimes_var = ret;
				return ret;
			}

			[Export ("setKeyTimes:", ArgumentSemantic.Copy)]
			set {
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetKeyTimes, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetKeyTimes, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_KeyTimes_var = value;
			}
		}

		MonoMac.CoreAnimation.CAMediaTimingFunction[] __mt_TimingFunctions_var;
		public virtual CAMediaTimingFunction[] TimingFunctions {
			[Export ("timingFunctions", ArgumentSemantic.Copy)]
			get {
				CAMediaTimingFunction[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CAMediaTimingFunction>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selTimingFunctions));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CAMediaTimingFunction>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selTimingFunctions));
				}
				__mt_TimingFunctions_var = ret;
				return ret;
			}

			[Export ("setTimingFunctions:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetTimingFunctions, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetTimingFunctions, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_TimingFunctions_var = value;
			}
		}

		public virtual string CalculationMode {
			[Export ("calculationMode", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selCalculationMode));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selCalculationMode));
				}
			}

			[Export ("setCalculationMode:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetCalculationMode, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetCalculationMode, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual string RotationMode {
			[Export ("rotationMode", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selRotationMode));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selRotationMode));
				}
			}

			[Export ("setRotationMode:", ArgumentSemantic.Copy)]
			set {
			var nsvalue = value == null ? null : new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetRotationMode, nsvalue == null ? IntPtr.Zero : nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetRotationMode, nsvalue == null ? IntPtr.Zero : nsvalue.Handle);
				}
							if (nsvalue != null)
				nsvalue.Dispose ();
			}
		}

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _CalculationLinear;
		public static NSString CalculationLinear {
			get {
				if (_CalculationLinear == null)
					_CalculationLinear = Dlfcn.GetStringConstant (libraryHandle, "kCAAnimationLinear");
				return _CalculationLinear;
			}
		}
		static NSString _CalculationDiscrete;
		public static NSString CalculationDiscrete {
			get {
				if (_CalculationDiscrete == null)
					_CalculationDiscrete = Dlfcn.GetStringConstant (libraryHandle, "kCAAnimationDiscrete");
				return _CalculationDiscrete;
			}
		}
		static NSString _CalculationPaced;
		public static NSString CalculationPaced {
			get {
				if (_CalculationPaced == null)
					_CalculationPaced = Dlfcn.GetStringConstant (libraryHandle, "kCAAnimationDiscrete");
				return _CalculationPaced;
			}
		}
	
	} /* class CAKeyFrameAnimation */
}
