//
// Authors:
//   Duane Wandless
//   Miguel de Icaza
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
using MonoMac.CoreGraphics;

namespace MonoMac.ImageIO {
	public enum CGImageSourceStatus {
		Complete      = 0,
		Incomplete    = -1,
		ReadingHeader = -2,
		UnknownType   = -3,
		InvalidData   = -4,
		UnexpectedEOF = -5,
	}
	
	public class CGImageOptions {
		static IntPtr kTypeIdentifierHint;
		static IntPtr kShouldCache;
		static IntPtr kShouldAllowFloat;
		
		static void Init ()
		{
			if (kTypeIdentifierHint != IntPtr.Zero)
				return;
			
			IntPtr lib = Libraries.ImageIO.Handle;
			kTypeIdentifierHint = Dlfcn.GetIntPtr (lib, "kCGImageSourceTypeIdentifierHint");
			kShouldCache = Dlfcn.GetIntPtr (lib, "kCGImageSourceShouldCache");
			kShouldAllowFloat = Dlfcn.GetIntPtr (lib, "kCGImageSourceShouldAllowFloat");
		}

		public CGImageOptions ()
		{
			ShouldCache = true;
		}
		
		public string BestGuessTypeIdentifier { get; set; }
		public bool ShouldCache { get; set; }
		public bool ShouldAllowFloat { get; set; }
		
		internal virtual NSMutableDictionary ToDictionary ()
		{
			Init ();
			
			var dict = new NSMutableDictionary ();
			
			if (BestGuessTypeIdentifier != null)
				dict.LowlevelSetObject (new NSString (BestGuessTypeIdentifier), kTypeIdentifierHint);
			if (!ShouldCache)
				dict.LowlevelSetObject (CFBoolean.False.Handle, kShouldCache);
			if (ShouldAllowFloat)
				dict.LowlevelSetObject (CFBoolean.True.Handle, kShouldAllowFloat);

			return dict;
		}
	}

	public class CGImageThumbnailOptions : CGImageOptions {
		static IntPtr kCreateThumbnailFromImageIfAbsent;
		static IntPtr kCreateThumbnailFromImageAlways;
		static IntPtr kThumbnailMaxPixelSize;
		static IntPtr kCreateThumbnailWithTransform;

		static void Init ()
		{
			if (kCreateThumbnailWithTransform != IntPtr.Zero)
				return;
			
			IntPtr lib = Libraries.ImageIO.Handle;
			kCreateThumbnailFromImageIfAbsent = Dlfcn.GetIntPtr (lib, "kCGImageSourceCreateThumbnailFromImageIfAbsent");
			kCreateThumbnailFromImageAlways = Dlfcn.GetIntPtr (lib, "kCGImageSourceCreateThumbnailFromImageAlways");
			kThumbnailMaxPixelSize = Dlfcn.GetIntPtr (lib, "kCGImageSourceThumbnailMaxPixelSize");
			kCreateThumbnailWithTransform = Dlfcn.GetIntPtr (lib, "kCGImageSourceCreateThumbnailWithTransform");
		}

		public bool CreateThumbnailFromImageIfAbsent { get; set; }
		public bool CreateThumbnailFromImageAlways { get; set; }
		public int? MaxPixelSize { get; set; }
		public bool CreateThumbnailWithTransform { get; set; }
		
		internal override NSMutableDictionary ToDictionary ()
		{
			Init ();
			
			var dict = base.ToDictionary ();
			IntPtr thandle = CFBoolean.True.Handle;

			if (CreateThumbnailFromImageIfAbsent)
				dict.LowlevelSetObject (thandle, kCreateThumbnailFromImageIfAbsent);
			if (CreateThumbnailFromImageAlways)
				dict.LowlevelSetObject (thandle, kCreateThumbnailFromImageAlways);
			if (MaxPixelSize.HasValue)
				dict.LowlevelSetObject (new NSNumber (MaxPixelSize.Value), kThumbnailMaxPixelSize);
			if (CreateThumbnailWithTransform)
				dict.LowlevelSetObject (thandle, kCreateThumbnailWithTransform);
			
			return dict;
		}
	}
	
	public class CGImageSource : INativeObject, IDisposable
	{
		[DllImport (Constants.ImageIOLibrary, EntryPoint="CGImageSourceGetTypeID")]
		public extern static int GetTypeID ();

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCopyTypeIdentifiers ();

		public static string [] TypeIdentifiers {
			get {
				return NSArray.StringArrayFromHandle (CGImageSourceCopyTypeIdentifiers ());
			}
		}
		
		internal IntPtr handle;

		// invoked by marshallers
		internal CGImageSource (IntPtr handle) : this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGImageSource (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CFObject.CFRetain (handle);
		}

		~CGImageSource ()
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
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
				
		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateWithURL(IntPtr url, IntPtr options);

		public static CGImageSource FromUrl (NSUrl url)
		{
			return FromUrl (url, null);
		}
		
		public static CGImageSource FromUrl (NSUrl url, CGImageOptions options)
		{
			if (url == null)
				throw new ArgumentNullException ("url");

			using (var dict = options == null ? null : options.ToDictionary ())
				return new CGImageSource (CGImageSourceCreateWithURL (url.Handle, dict == null ? IntPtr.Zero : dict.Handle), true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateWithDataProvider (IntPtr provider, IntPtr options);
		public static CGImageSource FromDataProvider (CGDataProvider provider)
		{
			return FromDataProvider (provider, null);
		}
		
		public static CGImageSource FromDataProvider (CGDataProvider provider, CGImageOptions options)
		{
			if (provider == null)
				throw new ArgumentNullException ("provider");

			using (var dict = options == null ? null : options.ToDictionary ())
				return new CGImageSource (CGImageSourceCreateWithDataProvider (provider.Handle, dict == null ? IntPtr.Zero : dict.Handle), true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateWithData (IntPtr data, IntPtr options);
		public static CGImageSource FromData (NSData data)
		{
			return FromData (data, null);
		}
		
		public static CGImageSource FromData (NSData data, CGImageOptions options)
		{
			if (data == null)
				throw new ArgumentNullException ("data");

			using (var dict = options == null ? null : options.ToDictionary ())
				return new CGImageSource (CGImageSourceCreateWithData (data.Handle, dict == null ? IntPtr.Zero : dict.Handle), true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceGetType (IntPtr handle);
		
		public string TypeIdentifier {
			get {
				return NSString.FromHandle (CGImageSourceGetType (handle));
			}
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceGetCount (IntPtr handle);
		
		public int ImageCount {
			get {
				return CGImageSourceGetCount (handle).ToInt32();
			}
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCopyProperties (IntPtr handle, IntPtr dictOptions);

		[Advice ("Use GetProperties")]
		public NSDictionary CopyProperties (NSDictionary dict)
		{
			return new NSDictionary (CGImageSourceCopyProperties (handle, dict == null ? IntPtr.Zero : dict.Handle));
		}

		[Advice ("Use GetProperties")]
		public NSDictionary CopyProperties (CGImageOptions options)
		{
			if (options == null)
				throw new ArgumentNullException ("options");
			return CopyProperties (options.ToDictionary ());
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCopyPropertiesAtIndex (IntPtr handle, IntPtr idx, IntPtr dictOptions);

		[Advice ("Use GetProperties")]
		public NSDictionary CopyProperties (NSDictionary dict, int imageIndex)
		{
			return new NSDictionary (CGImageSourceCopyPropertiesAtIndex (handle, new IntPtr(imageIndex), dict == null ? IntPtr.Zero : dict.Handle));
		}

		[Advice ("Use GetProperties")]
		public NSDictionary CopyProperties (CGImageOptions options, int imageIndex)
		{
			if (options == null)
				throw new ArgumentNullException ("options");
			return CopyProperties (options.ToDictionary (), imageIndex);
		}
		
		public CoreGraphics.CGImageProperties GetProperties (CGImageOptions options = null)
		{
			return new CoreGraphics.CGImageProperties (CopyProperties (options == null ? null : options.ToDictionary ()));
		}

		public CoreGraphics.CGImageProperties GetProperties (int index, CGImageOptions options = null)
		{
			return new CoreGraphics.CGImageProperties (CopyProperties (options == null ? null : options.ToDictionary (), index));
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateImageAtIndex(IntPtr isrc, IntPtr index, IntPtr options);
		public CGImage CreateImage (int index, CGImageOptions options)
		{
			using (var dict = options == null ? null : options.ToDictionary ()) {
				var ret = CGImageSourceCreateImageAtIndex (handle, new IntPtr(index), dict == null ? IntPtr.Zero : dict.Handle);
				return new CGImage (ret, true);
			}
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateThumbnailAtIndex (IntPtr isrc, IntPtr index, IntPtr options);
		public CGImage CreateThumbnail (int index, CGImageThumbnailOptions options)
		{
			using (var dict = options == null ? null : options.ToDictionary ()) {
				var ret = CGImageSourceCreateThumbnailAtIndex (handle, new IntPtr(index), dict == null ? IntPtr.Zero : dict.Handle);
				return new CGImage (ret, true);
			}
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateIncremental (IntPtr options);
		public static CGImageSource CreateIncremental (CGImageOptions options)
		{
			using (var dict = options == null ? null : options.ToDictionary ())
				return new CGImageSource (CGImageSourceCreateIncremental (dict == null ? IntPtr.Zero : dict.Handle), true);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static void CGImageSourceUpdateData (IntPtr handle, IntPtr data, bool final);
		
		public void UpdateData (NSData data, bool final)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			CGImageSourceUpdateData (handle, data.Handle, final);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static void CGImageSourceUpdateDataProvider (IntPtr handle, IntPtr dataProvider);
		
		public void UpdateDataProvider (CGDataProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException ("provider");
			CGImageSourceUpdateDataProvider (handle, provider.Handle);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static CGImageSourceStatus CGImageSourceGetStatus (IntPtr handle);
		
		public CGImageSourceStatus GetStatus ()
		{
			return CGImageSourceGetStatus (handle);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static CGImageSourceStatus CGImageSourceGetStatusAtIndex (IntPtr handle, IntPtr idx);		

		public CGImageSourceStatus GetStatus (int index)
		{
			return CGImageSourceGetStatusAtIndex (handle, new IntPtr(index));
		}
	}
}
