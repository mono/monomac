//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, 2012 Xamarin Inc
// Copyright 2010, Novell, Inc.
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
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreFoundation;

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

namespace MonoMac.CoreImage {
	public class CIContextOptions {
		public CIContextOptions () {}
		public CGColorSpace OutputColorSpace { get; set; }
		public CGColorSpace WorkingColorSpace { get; set; }
		public bool UseSoftwareRenderer { get; set; }

		internal NSDictionary ToDictionary ()
		{
			if (OutputColorSpace == null && WorkingColorSpace == null && UseSoftwareRenderer == false)
				return null;
			var ret = new NSMutableDictionary ();
			if (OutputColorSpace != null)
				ret.LowlevelSetObject (OutputColorSpace.Handle, CIContext.OutputColorSpace.Handle);
			if (WorkingColorSpace != null)
				ret.LowlevelSetObject (WorkingColorSpace.Handle, CIContext.WorkingColorSpace.Handle);
			if (UseSoftwareRenderer)
				ret.LowlevelSetObject (CFBoolean.True.Handle, CIContext.UseSoftwareRenderer.Handle);

			return ret;
		}
	}
	
	public partial class CIContext {
#if MONOMAC
		public static CIContext FromContext (CGContext ctx, CIContextOptions options)
		{
			NSDictionary dict = options == null ? null : options.ToDictionary ();

			return FromContext (ctx, dict);
		}
		
		public static CIContext FromContext (CGContext ctx)
		{
			return FromContext (ctx, (CIContextOptions) null);
		}

		public CGLayer CreateCGLayer (CGSize size)
		{
			return CreateCGLayer (size, null);
		}
#else
		public static CIContext FromOptions (CIContextOptions options)
		{
			return FromOptions (options == null ? null : options.ToDictionary ());
		}
		
		public CGImage CreateCGImage (CIImage image, CGRect fromRect, CIFormat ciImageFormat, CGColorSpace colorSpace)
		{
			return CreateCGImage (image, fromRect, CIImage.CIFormatToInt (ciImageFormat), colorSpace);
		}
#endif
	}
}