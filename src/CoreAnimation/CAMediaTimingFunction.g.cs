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
	[Register("CAMediaTimingFunction")]
	public partial class CAMediaTimingFunction : NSObject {
		static IntPtr selFunctionWithName = Selector.GetHandle ("functionWithName:");
		static IntPtr selFunctionWithControlPoints = Selector.GetHandle ("functionWithControlPoints::::");
		static IntPtr selInitWithControlPoints = Selector.GetHandle ("initWithControlPoints::::");

		static IntPtr class_ptr = Class.GetHandle ("CAMediaTimingFunction");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAMediaTimingFunction () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAMediaTimingFunction (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAMediaTimingFunction (NSObjectFlag t) : base (t) {}

		public CAMediaTimingFunction (IntPtr handle) : base (handle) {}

		[Export ("functionWithName:")]
		public static CAMediaTimingFunction FromName (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			var nsname = new NSString (name);

			CAMediaTimingFunction ret;
			ret = (CAMediaTimingFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selFunctionWithName, nsname.Handle));
						nsname.Dispose ();

			return ret;
		}

		[Export ("functionWithControlPoints::::")]
		public static CAMediaTimingFunction FromControlPoints (float c1x, float c1y, float c2x, float c2y)
		{
			return (CAMediaTimingFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_float_float_float_float (class_ptr, selFunctionWithControlPoints, c1x, c1y, c2x, c2y));
		}

		[Export ("initWithControlPoints::::")]
		public CAMediaTimingFunction (float c1x, float c1y, float c2x, float c2y) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_float_float_float_float (this.Handle, selInitWithControlPoints, c1x, c1y, c2x, c2y);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_float_float_float_float (this.SuperHandle, selInitWithControlPoints, c1x, c1y, c2x, c2y);
			}
		}

	
	} /* class CAMediaTimingFunction */
}
