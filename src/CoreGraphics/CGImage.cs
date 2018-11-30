// 
// CGImage.cs: Implements the managed CGImage
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

#if MONOMAC
	[Flags]	
	public enum CGWindowImageOption {
		Default             = 0,
		BoundsIgnoreFraming = (1 << 0),
		ShouldBeOpaque      = (1 << 1),
		OnlyShadows         = (1 << 2)
	}

	[Flags]
	public enum CGWindowListOption {
		All                 = 0,
		OnScreenOnly        = (1 << 0),
		OnScreenAboveWindow = (1 << 1),
		OnScreenBelowWindow = (1 << 2),
		IncludingWindow     = (1 << 3),
		ExcludeDesktopElements    = (1 << 4)
	}
#endif
	
	public enum CGImageAlphaInfo {
		None,               
		PremultipliedLast,  
		PremultipliedFirst, 
		Last,               
		First,              
		NoneSkipLast,       
		NoneSkipFirst,      
		Only                
	}

	[Flags]
	public enum CGBitmapFlags {
		None,               
		PremultipliedLast,  
		PremultipliedFirst, 
		Last,               
		First,              
		NoneSkipLast,       
		NoneSkipFirst,      
		Only,
			
		AlphaInfoMask = 0x1F,
		FloatComponents = (1 << 8),
		
		ByteOrderMask     = 0x7000,
		ByteOrderDefault  = (0 << 12),
		ByteOrder16Little = (1 << 12),
		ByteOrder32Little = (2 << 12),
		ByteOrder16Big    = (3 << 12),
		ByteOrder32Big    = (4 << 12)
	}
	
	public class CGImage : INativeObject, IDisposable {
		internal IntPtr handle;

		// invoked by marshallers
		public CGImage (IntPtr handle)
			: this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGImage (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGImageRetain (handle);
		}
		
		~CGImage ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGImageRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGImageRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGImageRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreate(IntPtr size_t_width, IntPtr size_t_height, IntPtr size_t_bitsPerComponent,
						   IntPtr size_t_bitsPerPixel, IntPtr size_t_bytesPerRow,
						   IntPtr /* CGColorSpaceRef */ space,
						   CGBitmapFlags bitmapInfo,
						   IntPtr /* CGDataProviderRef */ provider,
						   nfloat [] decode,
						   bool shouldInterpolate,
						   CGColorRenderingIntent intent);

		public CGImage (nint width, nint height, nint bitsPerComponent, nint bitsPerPixel, nint bytesPerRow,
				CGColorSpace colorSpace, CGBitmapFlags bitmapFlags, CGDataProvider provider,
				nfloat [] decode, bool shouldInterpolate, CGColorRenderingIntent intent)
		{
			if (colorSpace == null)
				throw new ArgumentNullException ("colorSpace");
			if (width < 0)
				throw new ArgumentException ("width");
			if (height < 0)
				throw new ArgumentException ("height");
			if (bitsPerPixel < 0)
				throw new ArgumentException ("bitsPerPixel");
			if (bitsPerComponent < 0)
				throw new ArgumentException ("bitsPerComponent");
			if (bytesPerRow < 0)
				throw new ArgumentException ("bytesPerRow");

			handle = CGImageCreate (new IntPtr(width), new IntPtr(height), new IntPtr(bitsPerComponent),
			                        new IntPtr(bitsPerPixel), new IntPtr(bytesPerRow),
						colorSpace.Handle, bitmapFlags, provider == null ? IntPtr.Zero : provider.Handle,
						decode,
						shouldInterpolate, intent);
		}

		public CGImage (nint width, nint height, nint bitsPerComponent, nint bitsPerPixel, nint bytesPerRow,
				CGColorSpace colorSpace, CGImageAlphaInfo alphaInfo, CGDataProvider provider,
				nfloat [] decode, bool shouldInterpolate, CGColorRenderingIntent intent)
		{
			if (colorSpace == null)
				throw new ArgumentNullException ("colorSpace");
			if (width < 0)
				throw new ArgumentException ("width");
			if (height < 0)
				throw new ArgumentException ("height");
			if (bitsPerPixel < 0)
				throw new ArgumentException ("bitsPerPixel");
			if (bitsPerComponent < 0)
				throw new ArgumentException ("bitsPerComponent");
			if (bytesPerRow < 0)
				throw new ArgumentException ("bytesPerRow");

			handle = CGImageCreate (new IntPtr(width), new IntPtr(height), new IntPtr(bitsPerComponent),
			                        new IntPtr(bitsPerPixel), new IntPtr(bytesPerRow),
						colorSpace.Handle, (CGBitmapFlags) alphaInfo, provider == null ? IntPtr.Zero : provider.Handle,
						decode,
						shouldInterpolate, intent);
		}

#if MONOMAC
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGWindowListCreateImage(CGRect screenBounds, CGWindowListOption windowOption, uint windowID, CGWindowImageOption imageOption);
        
		public static CGImage ScreenImage (int windownumber, CGRect bounds)
		{
			IntPtr imageRef = CGWindowListCreateImage(bounds, CGWindowListOption.IncludingWindow, (uint)windownumber,
								  CGWindowImageOption.Default);
			return new CGImage(imageRef, true);                              
		}
#else
		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr UIGetScreenImage ();
		public static CGImage ScreenImage {
			get {
				return new CGImage (UIGetScreenImage (), true);
			}
		}
#endif

	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateWithJPEGDataProvider(IntPtr /* CGDataProviderRef */ source,
								       nfloat [] decode,
								       bool shouldInterpolate,
								       CGColorRenderingIntent intent);

		public static CGImage FromJPEG (CGDataProvider provider, nfloat [] decode, bool shouldInterpolate, CGColorRenderingIntent intent)
		{
			if (provider == null)
				throw new ArgumentNullException ("provider");
			
			var handle = CGImageCreateWithJPEGDataProvider (provider.Handle, decode, shouldInterpolate, intent);
			if (handle == IntPtr.Zero)
				return null;

			return new CGImage (handle, true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateWithPNGDataProvider(IntPtr /*CGDataProviderRef*/ source,
								      nfloat [] decode, bool shouldInterpolate,
								      CGColorRenderingIntent intent);

		public static CGImage FromPNG (CGDataProvider provider, nfloat [] decode, bool shouldInterpolate, CGColorRenderingIntent intent)
		{
			if (provider == null)
				throw new ArgumentNullException ("provider");
			
			var handle = CGImageCreateWithPNGDataProvider (provider.Handle, decode, shouldInterpolate, intent);
			if (handle == IntPtr.Zero)
				return null;

			return new CGImage (handle, true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageMaskCreate (IntPtr size_t_width, IntPtr size_t_height, IntPtr size_t_bitsPerComponent, IntPtr size_t_bitsPerPixel,
							IntPtr size_t_bytesPerRow, IntPtr /* CGDataProviderRef */ provider, nfloat [] decode, bool shouldInterpolate);

		public static CGImage CreateMask (nint width, nint height, nint bitsPerComponent, nint bitsPerPixel, nint bytesPerRow, CGDataProvider provider, nfloat [] decode, bool shouldInterpolate)
		{
			if (width < 0)
				throw new ArgumentException ("width");
			if (height < 0)
				throw new ArgumentException ("height");
			if (bitsPerPixel < 0)
				throw new ArgumentException ("bitsPerPixel");
			if (bytesPerRow < 0)
				throw new ArgumentException ("bytesPerRow");
			if (provider == null)
				throw new ArgumentNullException ("provider");

			var handle = CGImageMaskCreate (new IntPtr(width), new IntPtr(height), new IntPtr(bitsPerComponent),
			                                new IntPtr(bitsPerPixel), new IntPtr(bytesPerRow), provider.Handle, decode, shouldInterpolate);
			if (handle == IntPtr.Zero)
				return null;

			return new CGImage (handle, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateWithMaskingColors(IntPtr image, nfloat [] components);
		public CGImage WithMaskingColors (nfloat[] components)
		{
			int N = 2*ColorSpace.Components;
			if (components == null)
				throw new ArgumentNullException ("components");
			if (components.Length != N)
				throw new ArgumentException ("The argument 'components' must have 2N values, where N is the number of components in the color space of the image.", "components");
			return new CGImage (CGImageCreateWithMaskingColors (Handle, components), true);
		}
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateCopy(IntPtr image);
		public CGImage Clone ()
		{
			return new CGImage (CGImageCreateCopy (handle), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateCopyWithColorSpace(IntPtr image, IntPtr space);
		public CGImage WithColorSpace (CGColorSpace cs)
		{
			return new CGImage (CGImageCreateCopyWithColorSpace (handle, cs.handle), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateWithImageInRect(IntPtr image, CGRect rect);
		public CGImage WithImageInRect (CGRect rect)
		{
			return new CGImage (CGImageCreateWithImageInRect (handle, rect), true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageCreateWithMask(IntPtr image, IntPtr mask);
		public CGImage WithMask (CGImage mask)
		{
			return new CGImage (CGImageCreateWithMask (handle, mask.handle), true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGImageIsMask(IntPtr image);
		public bool IsMask {
			get {
				return CGImageIsMask (handle) != 0;
			}
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetWidth(IntPtr image);
		public int Width {
			get {
				return CGImageGetWidth (handle).ToInt32();
			}
		}
		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetHeight(IntPtr image);
		public int Height {
			get {
				return CGImageGetHeight (handle).ToInt32();
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetBitsPerComponent(IntPtr image);
		public int BitsPerComponent {
			get {
				return CGImageGetBitsPerComponent (handle).ToInt32();
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetBitsPerPixel(IntPtr image);
		public int BitsPerPixel {
			get {
				return CGImageGetBitsPerPixel (handle).ToInt32();
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetBytesPerRow(IntPtr image);
		public int BytesPerRow {
			get {
				return CGImageGetBytesPerRow (handle).ToInt32();
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetColorSpace(IntPtr image);
		public CGColorSpace ColorSpace {
			get {
				return new CGColorSpace (CGImageGetColorSpace (handle));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGImageAlphaInfo CGImageGetAlphaInfo(IntPtr image);
		public CGImageAlphaInfo AlphaInfo {
			get {
				return CGImageGetAlphaInfo (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGImageGetDataProvider(IntPtr image);

		public CGDataProvider DataProvider {
			get {
				return new CGDataProvider (CGImageGetDataProvider (handle));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static nfloat * CGImageGetDecode(IntPtr image);
		public unsafe nfloat *Decode {
			get {
				return CGImageGetDecode (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGImageGetShouldInterpolate(IntPtr image);
		public bool ShouldInterpolate {
			get {
				return CGImageGetShouldInterpolate (handle) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGColorRenderingIntent CGImageGetRenderingIntent(IntPtr image);
		public CGColorRenderingIntent RenderingIntent {
			get {
				return CGImageGetRenderingIntent (handle);
			}
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGBitmapFlags CGImageGetBitmapInfo(IntPtr image);
		public CGBitmapFlags BitmapInfo {
			get {
				return CGImageGetBitmapInfo (handle);
			}
		}
	}
}
