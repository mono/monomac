// 
// CGLayer.cs: Implements the managed CGLayer
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

	public class CGLayer : INativeObject, IDisposable {
		IntPtr handle;

		internal CGLayer (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to layer creation");
					
			this.handle = handle;
			CGLayerRetain (handle);
		}

		[Preserve (Conditional=true)]
		internal CGLayer (IntPtr handle, bool owns)
		{
			if (!owns)
				CGLayerRetain (handle);

			this.handle = handle;
		}

		~CGLayer ()
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
		extern static void CGLayerRelease (IntPtr handle);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGLayerRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGLayerRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGLayerGetSize (IntPtr layer);

		public CGSize Size {
			get {
				return CGLayerGetSize (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGLayerGetContext (IntPtr layer);

		public CGContext Context {
			get {
				return new CGContext (CGLayerGetContext (handle));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGLayerCreateWithContext (IntPtr context, CGSize size, IntPtr dictionary);

		public static CGLayer Create (CGContext context, CGSize size) {
			return new CGLayer (CGLayerCreateWithContext (context.Handle, size, IntPtr.Zero), true);
		}

	}
}
