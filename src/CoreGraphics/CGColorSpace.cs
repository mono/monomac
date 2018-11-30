// 
// CGColorSpace.cs: Implements geometry classes
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

	public enum CGColorRenderingIntent {
		Default,
		AbsoluteColorimetric,
		RelativeColorimetric,
		Perceptual,
		Saturation
	};
	
	public enum CGColorSpaceModel {
		Unknown = -1,
		Monochrome,
		RGB,
		CMYK,
		Lab,
		DeviceN,
		Indexed,
		Pattern
	}

	public class CGColorSpace : INativeObject, IDisposable {
		internal IntPtr handle;
		
		public static CGColorSpace Null = new CGColorSpace (IntPtr.Zero);

		// Invoked by the marshallers, we need to take a ref
		public CGColorSpace (IntPtr handle)
		{
			this.handle = handle;
			CGColorSpaceRetain (handle);
		}

		[Preserve (Conditional=true)]
		internal CGColorSpace (IntPtr handle, bool owns)
		{
			if (!owns)
				CGColorSpaceRetain (handle);

			this.handle = handle;
		}

		~CGColorSpace ()
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
		extern static void CGColorSpaceRelease (IntPtr handle);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGColorSpaceRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGColorSpaceRelease (handle);
				handle = IntPtr.Zero;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateDeviceGray ();
		
		public static CGColorSpace CreateDeviceGray ()
		{
			return new CGColorSpace (CGColorSpaceCreateDeviceGray (), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateDeviceRGB ();

		public static CGColorSpace CreateDeviceRGB ()
		{
			return new CGColorSpace (CGColorSpaceCreateDeviceRGB (), true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateDeviceCMYK ();

		public static CGColorSpace CreateDeviceCMYK ()
		{
			return new CGColorSpace (CGColorSpaceCreateDeviceCMYK (), true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateCalibratedGray (nfloat [] whitepoint, nfloat [] blackpoint, nfloat gamma);

		public static CGColorSpace CreateCalibratedGray (nfloat [] whitepoint, nfloat [] blackpoint, nfloat gamma)
		{
			if (whitepoint.Length != 3)
				throw new ArgumentException ("Must have 3 values", "whitepoint");
			if (blackpoint.Length != 3)
				throw new ArgumentException ("Must have 3 values", "blackpoint");
			return new CGColorSpace (CGColorSpaceCreateCalibratedGray (whitepoint, blackpoint, gamma), true);
		}
		
		// 3, 3, 3, 9
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateCalibratedRGB (nfloat [] whitePoint, nfloat [] blackPoint, nfloat [] gamma, nfloat [] matrix);
		public static CGColorSpace CreateCalibratedRGB (nfloat [] whitepoint, nfloat [] blackpoint, nfloat [] gamma, nfloat [] matrix)
		{
			if (whitepoint.Length != 3)
				throw new ArgumentException ("Must have 3 values", "whitepoint");
			if (blackpoint.Length != 3)
				throw new ArgumentException ("Must have 3 values", "blackpoint");
			if (gamma.Length != 3)
				throw new ArgumentException ("Must have 3 values", "gamma");
			if (matrix.Length != 9)
				throw new ArgumentException ("Must have 9 values", "matrix");
			return new CGColorSpace (CGColorSpaceCreateCalibratedRGB (whitepoint, blackpoint, gamma, matrix), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateIndexed (IntPtr baseSpace,
								IntPtr lastIndex,
								byte[] colorTable);

		public static CGColorSpace CreateIndexed (CGColorSpace baseSpace, int lastIndex, byte[] colorTable)
		{
			return new CGColorSpace (CGColorSpaceCreateIndexed (baseSpace == null ? IntPtr.Zero : baseSpace.handle, new IntPtr(lastIndex), colorTable), true);
		}

			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreatePattern (IntPtr baseSpace);

		public static CGColorSpace CreatePattern (CGColorSpace baseSpace)
		{
			return new CGColorSpace (CGColorSpaceCreatePattern (baseSpace == null ? IntPtr.Zero : baseSpace.handle), true);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceCreateWithName (IntPtr name);

		public static CGColorSpace CreateWithName (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			return new CGColorSpace (CGColorSpaceCreateWithName (new NSString(name).Handle), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorSpaceGetBaseColorSpace (IntPtr space);

		public CGColorSpace GetBaseColorSpace ()
		{
			return new CGColorSpace (CGColorSpaceGetBaseColorSpace (handle), true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGColorSpaceModel CGColorSpaceGetModel (IntPtr space);

		public CGColorSpaceModel Model {
			get {
				return CGColorSpaceGetModel (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGColorSpaceGetNumberOfComponents (IntPtr space);
		
		public int Components {
			get {
				return CGColorSpaceGetNumberOfComponents (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /*size_t*/ IntPtr CGColorSpaceGetColorTableCount (IntPtr /* CGColorSpaceRef */ space);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGColorSpaceGetColorTable (IntPtr /* CGColorSpaceRef */ space, byte[] table);
		
		static byte[] Empty = new byte [0];
		
		public byte[] GetColorTable ()
		{
			int n = CGColorSpaceGetColorTableCount (handle).ToInt32 ();
			if (n == 0)
				return Empty;
			
			byte[] table = new byte [n * GetBaseColorSpace ().Components];
			CGColorSpaceGetColorTable (handle, table);
			return table;
		}
	}
}
