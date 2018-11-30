// 
// CGBitmapContext.cs:
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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

namespace MonoMac.CoreGraphics {

	public class CGBitmapContext : CGContext {

		[Preserve (Conditional=true)]
		internal CGBitmapContext (IntPtr handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextCreate (IntPtr data, UIntPtr width, UIntPtr height, UIntPtr bitsPerComponent, 
				UIntPtr bytesPerRow, IntPtr colorSpace, uint bitmapInfo);

		public CGBitmapContext (IntPtr data, int width, int height, int bitsPerComponent, int bytesPerRow, CGColorSpace colorSpace, CGImageAlphaInfo bitmapInfo)
			: base (CGBitmapContextCreate (data, (UIntPtr) width, (UIntPtr) height, (UIntPtr) bitsPerComponent, (UIntPtr) bytesPerRow, colorSpace.handle, (uint) bitmapInfo), true)
		{
		}

		public CGBitmapContext (IntPtr data, int width, int height, int bitsPerComponent, int bytesPerRow, CGColorSpace colorSpace, CGBitmapFlags bitmapInfo)
			: base (CGBitmapContextCreate (data, (UIntPtr) width, (UIntPtr) height, (UIntPtr) bitsPerComponent, (UIntPtr) bytesPerRow, colorSpace.handle, (uint) bitmapInfo), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextCreate (byte [] data, UIntPtr width, UIntPtr height, UIntPtr bitsPerComponent, 
				UIntPtr bytesPerRow, IntPtr colorSpace, uint bitmapInfo);

		public CGBitmapContext (byte [] data, int width, int height, int bitsPerComponent, int bytesPerRow, CGColorSpace colorSpace, CGImageAlphaInfo bitmapInfo)
			: base (CGBitmapContextCreate (data, (UIntPtr) width, (UIntPtr) height, (UIntPtr) bitsPerComponent, (UIntPtr) bytesPerRow, colorSpace.handle, (uint) bitmapInfo), true)
		{
		}

		public CGBitmapContext (byte [] data, int width, int height, int bitsPerComponent, int bytesPerRow, CGColorSpace colorSpace, CGBitmapFlags bitmapInfo)
			: base (CGBitmapContextCreate (data, (UIntPtr) width, (UIntPtr) height, (UIntPtr) bitsPerComponent, (UIntPtr) bytesPerRow, colorSpace.handle, (uint) bitmapInfo), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextGetData (IntPtr cgContextRef);
		public IntPtr Data {
			get {return CGBitmapContextGetData (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static UIntPtr CGBitmapContextGetWidth (IntPtr cgContextRef);
		public int Width {
			get {return (int) CGBitmapContextGetWidth (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static UIntPtr CGBitmapContextGetHeight (IntPtr cgContextRef);
		public int Height {
			get {return (int) CGBitmapContextGetHeight (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static UIntPtr CGBitmapContextGetBitsPerComponent (IntPtr cgContextRef);
		public int BitsPerComponent {
			get {return (int) CGBitmapContextGetBitsPerComponent (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static UIntPtr CGBitmapContextGetBitsPerPixel (IntPtr cgContextRef);
		public int BitsPerPixel {
			get {return (int) CGBitmapContextGetBitsPerPixel (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static UIntPtr CGBitmapContextGetBytesPerRow (IntPtr cgContextRef);
		public int BytesPerRow {
			get {return (int) CGBitmapContextGetBytesPerRow (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextGetColorSpace (IntPtr cgContextRef);
		public CGColorSpace ColorSpace {
			get {return new CGColorSpace (CGBitmapContextGetColorSpace (Handle), true);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGImageAlphaInfo CGBitmapContextGetAlphaInfo (IntPtr cgContextRef);
		public CGImageAlphaInfo AlphaInfo {
			get {return CGBitmapContextGetAlphaInfo (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static uint CGBitmapContextGetBitmapInfo (IntPtr cgContextRef);
		public uint BitmapInfo {
			get {return CGBitmapContextGetBitmapInfo (Handle);}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGBitmapContextCreateImage (IntPtr c);
		public CGImage ToImage ()
		{
			return new CGImage (CGBitmapContextCreateImage (Handle), true);
		}
	}
}
