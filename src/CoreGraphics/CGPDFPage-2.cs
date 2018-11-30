// 
// CGPDFPage.cs: Implements the managed CGPDFPage
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

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

namespace MonoMac.CoreGraphics {

	public enum CGPDFBox {
		Media = 0,
		Crop = 1,
		Bleed = 2,
		Trim = 3,
		Art = 4
	}

	public partial class CGPDFPage {
		CGPDFDocument doc;

		public CGPDFPage (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to CGPDFPage creation");

			CGPDFPageRetain (handle);
			this.handle = handle;
		}
		
		internal CGPDFPage (CGPDFDocument doc, IntPtr handle)
		{
			this.doc = doc;
			this.handle = handle;
			CGPDFPageRetain (handle);
		}

		public CGPDFDocument Document {
			get {
				return doc;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFPageGetPageNumber (IntPtr handle);
		public int PageNumber {
			get {
				return CGPDFPageGetPageNumber (handle);
 			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPDFPageGetBoxRect (IntPtr handle, CGPDFBox box);
		public CGRect GetBoxRect (CGPDFBox box)
		{
			return CGPDFPageGetBoxRect (handle, box);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFPageGetRotationAngle (IntPtr handle);
		public int RotationAngle {
			get {
				return CGPDFPageGetRotationAngle (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGPDFPageGetDrawingTransform (IntPtr handle, CGPDFBox box, CGRect rect, int rotate, int preserveAspectRatio);

		public CGAffineTransform GetDrawingTransform (CGPDFBox box, CGRect rect, int rotate, bool preserveAspectRatio)
		{
			return CGPDFPageGetDrawingTransform (handle, box, rect, rotate, preserveAspectRatio ? 1 : 0);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFPageGetDictionary (IntPtr pageHandle);

#if !COREBUILD
		public CGPDFDictionary Dictionary {
			get {
				return new CGPDFDictionary (CGPDFPageGetDictionary (handle));
			}
		}
#endif
	}
}

