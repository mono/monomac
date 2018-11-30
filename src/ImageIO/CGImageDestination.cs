//
// Authors:
//   Miguel de Icaza
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

	public class CGImageDestinationOptions {
		static IntPtr kLossyCompressionQuality;
		static IntPtr kBackgroundColor;
		
		static void Init ()
		{
			if (kLossyCompressionQuality != IntPtr.Zero)
				return;
			
			IntPtr lib = Libraries.ImageIO.Handle;
			kLossyCompressionQuality = Dlfcn.GetIntPtr (lib, "kCGImageDestinationLossyCompressionQuality");
			kBackgroundColor = Dlfcn.GetIntPtr (lib, "kCGImageDestinationBackgroundColor");
		}

		public float? LossyCompressionQuality { get; set; }
		public CGColor DestinationBackgroundColor { get; set; }

		internal NSMutableDictionary ToDictionary ()
		{
			Init ();
			var dict = new NSMutableDictionary ();

			if (LossyCompressionQuality.HasValue)
				dict.LowlevelSetObject (new NSNumber (LossyCompressionQuality.Value), kLossyCompressionQuality);
			if (DestinationBackgroundColor != null)
				dict.LowlevelSetObject (DestinationBackgroundColor.Handle, kBackgroundColor);

			return dict;
		}
	}
	
	public class CGImageDestination : INativeObject, IDisposable {
		internal IntPtr handle;

		// invoked by marshallers
		internal CGImageDestination (IntPtr handle) : this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGImageDestination (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CFObject.CFRetain (handle);
		}

		~CGImageDestination ()
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
				
		[DllImport (Constants.ImageIOLibrary, EntryPoint="CGImageDestinationGetTypeID")]
		public extern static int GetTypeID ();
		
		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageDestinationCopyTypeIdentifiers ();

		public static string [] TypeIdentifiers {
			get {
				return NSArray.StringArrayFromHandle (CGImageDestinationCopyTypeIdentifiers ());
			}
		}

		// TODO: CGImageDestinationCreateWithDataConsumer
		// Missing the CGDataConsumer API

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageDestinationCreateWithData (IntPtr data, IntPtr stringType, IntPtr count, IntPtr options);
		
		public static CGImageDestination FromData (NSData data, string typeIdentifier, int imageCount)
		{
			return FromData (data, typeIdentifier, imageCount, null);
		}

		public static CGImageDestination FromData (NSData data, string typeIdentifier, int imageCount, CGImageDestinationOptions options)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			if (typeIdentifier == null)
				throw new ArgumentNullException ("typeIdentifier");

			var dict = options == null ? null : options.ToDictionary ();
			var typeId = NSString.CreateNative (typeIdentifier);
			IntPtr p = CGImageDestinationCreateWithData (data.Handle, typeId, (IntPtr) imageCount, dict == null ? IntPtr.Zero : dict.Handle);
			NSString.ReleaseNative (typeId);
			var ret = p == IntPtr.Zero ? null : new CGImageDestination (p, true);
			if (dict != null)
				dict.Dispose ();
			return ret;
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageDestinationCreateWithURL (IntPtr url, IntPtr stringType, IntPtr count, IntPtr options);

		public static CGImageDestination FromUrl (NSUrl url, string typeIdentifier, int imageCount)
		{
			return FromUrl (url, typeIdentifier, imageCount, null);
		}
		
		public static CGImageDestination FromUrl (NSUrl url, string typeIdentifier, int imageCount, CGImageDestinationOptions options)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			if (typeIdentifier == null)
				throw new ArgumentNullException ("typeIdentifier");

			var dict = options == null ? null : options.ToDictionary ();
			var typeId = NSString.CreateNative (typeIdentifier);
			IntPtr p = CGImageDestinationCreateWithURL (url.Handle, typeId, (IntPtr) imageCount, dict == null ? IntPtr.Zero : dict.Handle);
			NSString.ReleaseNative (typeId);
			var ret = p == IntPtr.Zero ? null : new CGImageDestination (p, true);
			if (dict != null)
				dict.Dispose ();
			return ret;
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static void CGImageDestinationSetProperties (IntPtr handle, IntPtr props);

		public void SetProperties (NSDictionary properties)
		{
			if (properties == null)
				throw new ArgumentNullException ("properties");
			CGImageDestinationSetProperties (handle, properties.Handle);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static void CGImageDestinationAddImage (IntPtr handle, IntPtr image, IntPtr properties);
		public void AddImage (CGImage image, NSDictionary properties)
		{
			if (image == null)
				throw new ArgumentNullException ("image");
			
			CGImageDestinationAddImage (handle, image.Handle, properties == null ? IntPtr.Zero : properties.Handle);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static void CGImageDestinationAddImageFromSource (IntPtr handle, IntPtr sourceHandle, IntPtr index, IntPtr properties);

		public void AddImage (CGImageSource source, int index, NSDictionary properties)
		{
			if (source == null)
				throw new ArgumentNullException ("source");
			
			CGImageDestinationAddImageFromSource (handle, source.Handle, (IntPtr) index, properties == null ? IntPtr.Zero : properties.Handle);
		}

		[DllImport (Constants.ImageIOLibrary)]
		extern static bool CGImageDestinationFinalize (IntPtr handle);

		public bool Close ()
		{
			var success = CGImageDestinationFinalize (handle);
			Dispose ();
			return success;
		}
	}
}