// 
// CGPDFDocument.cs: Implements the managed CGPDFDocument
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
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

namespace MonoMac.CoreGraphics {

	public class CGPDFDocument : INativeObject, IDisposable {
		internal IntPtr handle;

		~CGPDFDocument ()
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
		extern static void CGPDFDocumentRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFDocumentRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGPDFDocumentRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		/* invoked by marshallers */
		public CGPDFDocument (IntPtr handle)
		{
			this.handle = handle;
			CGPDFDocumentRetain (handle);
		}

		[Preserve (Conditional=true)]
		internal CGPDFDocument (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGPDFDocumentRetain (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDocumentCreateWithProvider (IntPtr /* CGDataProviderRef */ provider);
		
		public CGPDFDocument (CGDataProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException ("provider");
			handle = CGPDFDocumentCreateWithProvider (provider.Handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDocumentCreateWithURL (IntPtr url);

		public static CGPDFDocument FromFile (string str)
		{
			using (var url = CFUrl.FromFile (str)){
				if (url == null)
					return null;
				IntPtr handle = CGPDFDocumentCreateWithURL (url.Handle);
				if (handle == IntPtr.Zero)
					return null;
				return new CGPDFDocument (handle, true);
			}
			
		}
			
		public static CGPDFDocument FromUrl (string str)
		{
			using (var url = CFUrl.FromUrlString (str, null)){
				if (url == null)
					return null;
				IntPtr handle = CGPDFDocumentCreateWithURL (url.Handle);
				if (handle == IntPtr.Zero)
					return null;
				return new CGPDFDocument (handle, true);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFDocumentGetNumberOfPages (IntPtr handle);

		public int Pages {
			get {
				return CGPDFDocumentGetNumberOfPages (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDocumentGetPage (IntPtr handle, int page);
		
		public CGPDFPage GetPage (int page)
		{
			return new CGPDFPage (this, CGPDFDocumentGetPage (handle, page));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFDocumentGetVersion (IntPtr handle, out int major, out int minor);

		public void GetVersion (out int major, out int minor)
		{
			CGPDFDocumentGetVersion (handle, out major, out minor);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFDocumentIsEncrypted (IntPtr handle);

		public bool IsEncrypted {
			get {
				return CGPDFDocumentIsEncrypted (handle) != 0;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFDocumentUnlockWithPassword (IntPtr handle, string password);
		
		public bool UnlockWithPassword (string pass)
		{
			return CGPDFDocumentUnlockWithPassword (handle, pass) != 0;
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFDocumentIsUnlocked (IntPtr handle);

		public bool IsUnlocked {
			get {
				return CGPDFDocumentIsUnlocked (handle) != 0;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFDocumentAllowsPrinting (IntPtr handle);
		public bool AllowsPrinting {
			get {
				return CGPDFDocumentAllowsPrinting (handle) != 0;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFDocumentAllowsCopying (IntPtr handle);
		public bool AllowsCopying {
			get {
				return CGPDFDocumentAllowsCopying (handle) != 0;
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPDFDocumentGetMediaBox (IntPtr handle, int page);
		public CGRect GetMediaBox (int page)
		{
			return CGPDFDocumentGetMediaBox (handle, page);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPDFDocumentGetCropBox (IntPtr handle, int page);
		public CGRect GetCropBox (int page)
		{
			return CGPDFDocumentGetCropBox (handle, page);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPDFDocumentGetBleedBox (IntPtr handle, int page);
		public CGRect GetBleedBox (int page)
		{
			return CGPDFDocumentGetBleedBox (handle, page);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPDFDocumentGetTrimBox (IntPtr handle, int page);
		public CGRect GetTrimBox (int page)
		{
			return CGPDFDocumentGetTrimBox (handle, page);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPDFDocumentGetArtBox (IntPtr handle, int page);
		public CGRect GetArtBox (int page)
		{
			return CGPDFDocumentGetArtBox (handle, page);
		}
		
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDocumentGetCatalog (IntPtr handle);
		public CGPDFDictionary GetCatalog ()
		{
			return new CGPDFDictionary (CGPDFDocumentGetCatalog (handle));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDocumentGetInfo (IntPtr handle);
		public CGPDFDictionary GetInfo ()
		{
			return new CGPDFDictionary (CGPDFDocumentGetInfo (handle));
		}
#endif
	}
}
