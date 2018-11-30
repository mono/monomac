// 
// CGContextPDF.cs: Implements the managed CGContextPDF
//
// Authors: Mono Team
//     
// Copyright 2009-2010 Novell, Inc
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
	public class CGPDFPageInfo {
		static IntPtr kCGPDFContextMediaBox;
		static IntPtr kCGPDFContextCropBox;
		static IntPtr kCGPDFContextBleedBox;
		static IntPtr kCGPDFContextTrimBox;
		static IntPtr kCGPDFContextArtBox;
		
		static CGPDFPageInfo ()
		{
			IntPtr h = Dlfcn.dlopen (Constants.CoreGraphicsLibrary, 0);
			try {
				kCGPDFContextMediaBox = Dlfcn.GetIndirect (h, "kCGPDFContextMediaBox");
				kCGPDFContextCropBox = Dlfcn.GetIndirect (h, "kCGPDFContextCropBox");
				kCGPDFContextBleedBox = Dlfcn.GetIndirect (h, "kCGPDFContextBleedBox");
				kCGPDFContextTrimBox = Dlfcn.GetIndirect (h, "kCGPDFContextTrimBox");
				kCGPDFContextArtBox = Dlfcn.GetIndirect (h, "kCGPDFContextArtBox");
			} finally {
				Dlfcn.dlclose (h);
			}
		}
		
		public CGRect? MediaBox { get; set; }
		public CGRect? CropBox { get; set; }
		public CGRect? BleedBox { get; set; }
		public CGRect? TrimBox { get; set; }
		public CGRect? ArtBox { get; set; }

		static void Add (NSMutableDictionary dict, IntPtr key, CGRect? val)
		{
			if (!val.HasValue)
				return;
			NSData data;
			unsafe {
				CGRect f = val.Value;
				CGRect *pf = &f;
				data = NSData.FromBytes ((IntPtr) pf, 16);
			}
			dict.LowlevelSetObject (data, key);
		}
		
		internal virtual NSMutableDictionary ToDictionary ()
		{
			var ret = new NSMutableDictionary ();
			Add (ret, kCGPDFContextMediaBox, MediaBox);
			Add (ret, kCGPDFContextCropBox, CropBox);
			Add (ret, kCGPDFContextBleedBox, BleedBox);
			Add (ret, kCGPDFContextTrimBox, TrimBox);
			Add (ret, kCGPDFContextArtBox, ArtBox);
			return ret;
		}
	}

	public class CGPDFInfo : CGPDFPageInfo {
		static IntPtr kCGPDFContextTitle;
		static IntPtr kCGPDFContextAuthor;
		static IntPtr kCGPDFContextSubject;
		static IntPtr kCGPDFContextKeywords;
		static IntPtr kCGPDFContextCreator;
		static IntPtr kCGPDFContextOwnerPassword;
		static IntPtr kCGPDFContextUserPassword;
		static IntPtr kCGPDFContextEncryptionKeyLength;
		static IntPtr kCGPDFContextAllowsPrinting;
		static IntPtr kCGPDFContextAllowsCopying;

#if false
		static IntPtr kCGPDFContextOutputIntent;
		static IntPtr kCGPDFXOutputIntentSubtype;
		static IntPtr kCGPDFXOutputConditionIdentifier;
		static IntPtr kCGPDFXOutputCondition;
		static IntPtr kCGPDFXRegistryName;
		static IntPtr kCGPDFXInfo;
		static IntPtr kCGPDFXDestinationOutputProfile;
		static IntPtr kCGPDFContextOutputIntents;
#endif

		static CGPDFInfo ()
		{
			IntPtr h = Dlfcn.dlopen (Constants.CoreGraphicsLibrary, 0);
			try {
				kCGPDFContextTitle = Dlfcn.GetIntPtr (h, "kCGPDFContextTitle");
				kCGPDFContextAuthor = Dlfcn.GetIntPtr (h, "kCGPDFContextAuthor");
				kCGPDFContextSubject = Dlfcn.GetIntPtr (h, "kCGPDFContextSubject");
				kCGPDFContextKeywords = Dlfcn.GetIntPtr (h, "kCGPDFContextKeywords");
				kCGPDFContextCreator = Dlfcn.GetIntPtr (h, "kCGPDFContextCreator");
				kCGPDFContextOwnerPassword = Dlfcn.GetIntPtr (h, "kCGPDFContextOwnerPassword");
				kCGPDFContextUserPassword = Dlfcn.GetIntPtr (h, "kCGPDFContextUserPassword");
				kCGPDFContextEncryptionKeyLength = Dlfcn.GetIntPtr (h, "kCGPDFContextEncryptionKeyLength");
				kCGPDFContextAllowsPrinting = Dlfcn.GetIntPtr (h, "kCGPDFContextAllowsPrinting");
				kCGPDFContextAllowsCopying = Dlfcn.GetIntPtr (h, "kCGPDFContextAllowsCopying");
#if false
				kCGPDFContextOutputIntent = Dlfcn.GetIntPtr (h, "kCGPDFContextOutputIntent");
				kCGPDFXOutputIntentSubtype = Dlfcn.GetIntPtr (h, "kCGPDFXOutputIntentSubtype");
				kCGPDFXOutputConditionIdentifier = Dlfcn.GetIntPtr (h, "kCGPDFXOutputConditionIdentifier");
				kCGPDFXOutputCondition = Dlfcn.GetIntPtr (h, "kCGPDFXOutputCondition");
				kCGPDFXRegistryName = Dlfcn.GetIntPtr (h, "kCGPDFXRegistryName");
				kCGPDFXInfo = Dlfcn.GetIntPtr (h, "kCGPDFXInfo");
				kCGPDFXDestinationOutputProfile = Dlfcn.GetIntPtr (h, "kCGPDFXDestinationOutputProfile");
				kCGPDFContextOutputIntents = Dlfcn.GetIntPtr (h, "kCGPDFContextOutputIntents");
#endif
			} finally {
				Dlfcn.dlclose (h);
			}
		}

		public string Title { get; set; }
		public string Author { get; set; }
		public string Subject { get; set; }
		public string [] Keywords { get; set; }
		public string Creator { get; set; }
		public string OwnerPassword { get; set; }
		public string UserPassword { get; set; }
		public int? EncryptionKeyLength { get; set; }
		public bool? AllowsPrinting { get; set; }
		public bool? AllowsCopying { get; set; }
		//public NSDictionary OutputIntent { get; set; }

		internal override NSMutableDictionary ToDictionary ()
		{
			var ret = base.ToDictionary ();

			if (Title != null)
				ret.LowlevelSetObject ((NSString) Title, kCGPDFContextTitle);
			if (Author != null)
				ret.LowlevelSetObject ((NSString) Author, kCGPDFContextAuthor);
			if (Subject != null)
				ret.LowlevelSetObject ((NSString) Subject, kCGPDFContextSubject);
			if (Keywords != null && Keywords.Length > 0){
				if (Keywords.Length == 1)
					ret.LowlevelSetObject ((NSString) Keywords [0], kCGPDFContextKeywords);
				else
					ret.LowlevelSetObject (NSArray.FromStrings (Keywords), kCGPDFContextKeywords);
			}
			if (Creator != null)
				ret.LowlevelSetObject ((NSString) Creator, kCGPDFContextCreator);
			if (OwnerPassword != null)
				ret.LowlevelSetObject ((NSString) OwnerPassword, kCGPDFContextOwnerPassword);
			if (UserPassword != null)
				ret.LowlevelSetObject ((NSString) UserPassword, kCGPDFContextUserPassword);
			if (EncryptionKeyLength.HasValue)
				ret.LowlevelSetObject (NSNumber.FromInt32 (EncryptionKeyLength.Value), kCGPDFContextEncryptionKeyLength);
			if (AllowsPrinting.HasValue && AllowsPrinting.Value == false)
				ret.LowlevelSetObject (CFBoolean.False.Handle, kCGPDFContextAllowsPrinting);
			if (AllowsCopying.HasValue && AllowsCopying.Value == false)
				ret.LowlevelSetObject (CFBoolean.False.Handle, kCGPDFContextAllowsCopying);
			return ret;
		}
	}
	
	public class CGContextPDF : CGContext {
		bool closed;
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFContextCreateWithURL (IntPtr url, ref CGRect rect, IntPtr dictionary);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFContextCreateWithURL (IntPtr url, IntPtr rect, IntPtr dictionary);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFContextCreate (IntPtr dataConsumer, ref CGRect rect, IntPtr dictionary);
		
		
		public CGContextPDF (CGDataConsumer dataConsumer, CGRect mediaBox, CGPDFInfo info)
		{
			if (dataConsumer == null)
				throw new ArgumentNullException ("dataConsumer");
				
			handle = CGPDFContextCreate (dataConsumer.Handle, ref mediaBox, info == null ? IntPtr.Zero : info.ToDictionary ().Handle);
		}

		public CGContextPDF (NSUrl url, CGRect mediaBox, CGPDFInfo info)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			handle = CGPDFContextCreateWithURL (url.Handle, ref mediaBox, info == null ? IntPtr.Zero : info.ToDictionary ().Handle);
		}

		public CGContextPDF (NSUrl url, CGRect mediaBox)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			handle = CGPDFContextCreateWithURL (url.Handle, ref mediaBox, IntPtr.Zero);
		}

		public CGContextPDF (NSUrl url, CGPDFInfo info)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			handle = CGPDFContextCreateWithURL (url.Handle, IntPtr.Zero, info == null ? IntPtr.Zero : info.ToDictionary ().Handle);
		}

		public CGContextPDF (NSUrl url)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			handle = CGPDFContextCreateWithURL (url.Handle, IntPtr.Zero, IntPtr.Zero);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextClose(IntPtr handle);
		public void Close ()
		{
			if (closed)
				return;
			CGPDFContextClose (handle);
			closed = true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextBeginPage (IntPtr handle, IntPtr dict);
		
		public void BeginPage (CGPDFPageInfo info)
		{
			CGPDFContextBeginPage (handle, info == null ? IntPtr.Zero : info.ToDictionary ().Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextEndPage (IntPtr handle);
		public void EndPage ()
		{
			CGPDFContextEndPage (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextAddDocumentMetadata (IntPtr handle, IntPtr nsDataHandle);
		public void AddDocumentMetadata (NSData data)
		{
			if (data == null)
				return;
			CGPDFContextAddDocumentMetadata (handle, data.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextSetURLForRect (IntPtr handle, IntPtr urlh, CGRect rect);
		public void SetUrl (NSUrl url, CGRect region)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			CGPDFContextSetURLForRect (handle, url.Handle, region);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextAddDestinationAtPoint (IntPtr handle, IntPtr cfstring, CGPoint point);
		public void AddDestination (string name, CGPoint point)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			CGPDFContextAddDestinationAtPoint (handle, new NSString (name).Handle, point);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContextSetDestinationForRect (IntPtr handle, IntPtr cfstr, CGRect rect);
		public void SetDestination (string name, CGRect rect)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			CGPDFContextSetDestinationForRect (handle, new NSString (name).Handle, rect);
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing)
				Close ();

			base.Dispose (disposing);
		}
	}
}
