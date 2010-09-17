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
	[Register("CAValueFunction")]
	public partial class CAValueFunction : NSObject {
		static IntPtr selName = Selector.GetHandle ("name");
		static IntPtr selFunctionWithName = Selector.GetHandle ("functionWithName:");

		static IntPtr class_ptr = Class.GetHandle ("CAValueFunction");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAValueFunction () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAValueFunction (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAValueFunction (NSObjectFlag t) : base (t) {}

		public CAValueFunction (IntPtr handle) : base (handle) {}

		[Export ("functionWithName:")]
		public virtual CAValueFunction FromName (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			var nsname = new NSString (name);

			CAValueFunction ret;
			if (IsDirectBinding) {
				ret = (CAValueFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, selFunctionWithName, nsname.Handle));
			} else {
				ret = (CAValueFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, selFunctionWithName, nsname.Handle));
			}
						nsname.Dispose ();

			return ret;
		}

		public virtual string Name {
			[Export ("name")]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selName));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selName));
				}
			}

		}

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _RotateX;
		public static NSString RotateX {
			get {
				if (_RotateX == null)
					_RotateX = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionRotateX");
				return _RotateX;
			}
		}
		static NSString _RotateY;
		public static NSString RotateY {
			get {
				if (_RotateY == null)
					_RotateY = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionRotateY");
				return _RotateY;
			}
		}
		static NSString _RotateZ;
		public static NSString RotateZ {
			get {
				if (_RotateZ == null)
					_RotateZ = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionRotateZ");
				return _RotateZ;
			}
		}
		static NSString _Scale;
		public static NSString Scale {
			get {
				if (_Scale == null)
					_Scale = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionScale");
				return _Scale;
			}
		}
		static NSString _ScaleX;
		public static NSString ScaleX {
			get {
				if (_ScaleX == null)
					_ScaleX = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionScaleX");
				return _ScaleX;
			}
		}
		static NSString _ScaleY;
		public static NSString ScaleY {
			get {
				if (_ScaleY == null)
					_ScaleY = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionScaleY");
				return _ScaleY;
			}
		}
		static NSString _ScaleZ;
		public static NSString ScaleZ {
			get {
				if (_ScaleZ == null)
					_ScaleZ = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionScaleZ");
				return _ScaleZ;
			}
		}
		static NSString _Translate;
		public static NSString Translate {
			get {
				if (_Translate == null)
					_Translate = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionTranslate");
				return _Translate;
			}
		}
		static NSString _TranslateX;
		public static NSString TranslateX {
			get {
				if (_TranslateX == null)
					_TranslateX = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionTranslateX");
				return _TranslateX;
			}
		}
		static NSString _TranslateY;
		public static NSString TranslateY {
			get {
				if (_TranslateY == null)
					_TranslateY = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionTranslateY");
				return _TranslateY;
			}
		}
		static NSString _TranslateZ;
		public static NSString TranslateZ {
			get {
				if (_TranslateZ == null)
					_TranslateZ = Dlfcn.GetStringConstant (libraryHandle, "kCAValueFunctionTranslateZ");
				return _TranslateZ;
			}
		}
	
	} /* class CAValueFunction */
}
