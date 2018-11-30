// Copyright 2011, 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif


namespace MonoMac.Foundation {
	public partial class NSInputStream : NSStream {
		const string selReadMaxLength = "read:maxLength:";

		CFStreamEventType flags;
		IntPtr callback;
		CFStreamClientContext context;

		public nint Read (byte [] buffer, nuint len) {
			return objc_msgSend (Handle, Selector.GetHandle (selReadMaxLength), buffer, len);
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern nint objc_msgSend (IntPtr handle, IntPtr sel, [In, Out] byte [] buffer, nuint len);

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern nint objc_msgSend (IntPtr handle, IntPtr sel, IntPtr buffer, nuint len);

		[Export ("read:maxLength:")]
		public virtual nint Read (IntPtr buffer, nuint len)
		{
			if (buffer == IntPtr.Zero)
				throw new ArgumentNullException ("buffer");
			
			nint ret;
			if (IsDirectBinding) {
				ret = objc_msgSend (this.Handle, Selector.GetHandle (selReadMaxLength), buffer, len);
			} else {
				ret = objc_msgSend (this.SuperHandle, Selector.GetHandle (selReadMaxLength), buffer, len);
			}

			return ret;
		}

		protected override void Dispose (bool disposing)
		{
			context.Release ();
			context.Info = IntPtr.Zero;
			
			base.Dispose (disposing);
		}
		
		[Export ("_setCFClientFlags:callback:context:")]
		protected virtual bool SetCFClientFlags (CFStreamEventType inFlags, IntPtr inCallback, IntPtr inContextPtr)
		{
			CFStreamClientContext inContext;
			
			if (inContextPtr == IntPtr.Zero)
				return false;
			
			inContext = (CFStreamClientContext) Marshal.PtrToStructure (inContextPtr, typeof (CFStreamClientContext));
			if (inContext.Version != 0)
				return false;
			
			context.Release ();
			context = inContext;
			context.Retain ();
			
			flags = inFlags;
			callback = inCallback;

			return true;
		}

		[Export ("getBuffer:length:")]
		protected unsafe virtual bool GetBuffer (out IntPtr buffer, out System.UInt32 len)
		{
			buffer = IntPtr.Zero;
			len = 0;
			return false;
		}

		public void Notify (CFStreamEventType eventType)
		{
			if ((flags & eventType) == 0)
				return;

			context.Invoke (callback, Handle, eventType);
		}
	}
}	
