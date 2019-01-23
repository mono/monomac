// 
// CMFormatDescription.cs: Implements the managed CMFormatDescription
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//   Frank Krueger
//   Mono Team
//   Marek Safar (marek.safar@gmail.com)	
//     
// Copyright 2010 Novell, Inc
// Copyright 2012 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using MonoMac;
using MonoMac.CoreGraphics;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreVideo;
using MonoMac.AudioToolbox;

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

#if SDCONVERT
using CMVideoDimensions = System.Drawing.Size;
#endif

namespace MonoMac.CoreMedia {

#if !SDCOMPAT
	[StructLayout(LayoutKind.Sequential)]
	public struct CMVideoDimensions
	{
		public int Width;
		public int Height;
	}
	
#endif

    public enum CMFormatDescriptionError {
		None				= 0,
		InvalidParameter	= -12710,
		AllocationFailed	= -12711,
	}

	[Since (4,0)]
	public class CMFormatDescription : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMFormatDescription (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMFormatDescription (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMFormatDescription ()
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
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		extern static CFPropertyListRef CMFormatDescriptionGetExtension (
		   CMFormatDescriptionRef desc,
		   CFStringRef extensionKey
		);*/
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMFormatDescriptionGetExtensions (IntPtr handle);

#if !COREBUILD
		
		public NSDictionary GetExtensions ()
		{
			var cfDictRef = CMFormatDescriptionGetExtensions (handle);
			if (cfDictRef == IntPtr.Zero)
			{
				return null;
			}
			else
			{
				return Runtime.GetNSObject<NSDictionary> (cfDictRef);
			}
		}

#endif
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMFormatDescriptionGetMediaSubType (IntPtr handle);

		public uint MediaSubType
		{
			get
			{
				return CMFormatDescriptionGetMediaSubType (handle);
			}
		}

		public AudioFormatType AudioFormatType {
			get {
				return MediaType == CMMediaType.Audio ? (AudioFormatType) MediaSubType : 0;
			}
		}

		public CMSubtitleFormatType SubtitleFormatType {
			get {
				return MediaType == CMMediaType.Subtitle ? (CMSubtitleFormatType) MediaSubType : 0;
			}
		}

		public CMClosedCaptionFormatType ClosedCaptionFormatType {
			get {
				return MediaType == CMMediaType.ClosedCaption ? (CMClosedCaptionFormatType) MediaSubType : 0;				
			}
		}

		public CMMuxedStreamType MuxedStreamType {
			get {
				return MediaType == CMMediaType.Muxed ? (CMMuxedStreamType) MediaSubType : 0;	
			}
		}

		public CMVideoCodecType VideoCodecType {
			get {
				return MediaType == CMMediaType.Video ? (CMVideoCodecType) MediaSubType : 0;
			}
		}

		public CMMetadataFormatType MetadataFormatType {
			get {
				return MediaType == CMMediaType.Metadata ? (CMMetadataFormatType) MediaSubType : 0;
			}
		}

		public CMTimeCodeFormatType TimeCodeFormatType {
			get {
				return MediaType == CMMediaType.TimeCode ? (CMTimeCodeFormatType) MediaSubType : 0;				
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMMediaType CMFormatDescriptionGetMediaType (IntPtr handle);
		
		public CMMediaType MediaType
		{
			get
			{
				return CMFormatDescriptionGetMediaType (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMFormatDescriptionGetTypeID ();
		
		public static int GetTypeID ()
		{
			return CMFormatDescriptionGetTypeID ();
		}

#if !COREBUILD

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMFormatDescriptionError CMFormatDescriptionCreate (IntPtr allocator, CMMediaType mediaType, uint mediaSubtype, IntPtr extensions, out IntPtr handle);

		public static CMFormatDescription Create (CMMediaType mediaType, uint mediaSubtype, out CMFormatDescriptionError error)
		{
			IntPtr handle;
			error = CMFormatDescriptionCreate (IntPtr.Zero, mediaType, mediaSubtype, IntPtr.Zero, out handle);
			if (error != CMFormatDescriptionError.None)
				return null;

			return Create (mediaType, handle, true);
		}

		public static CMFormatDescription Create (IntPtr handle, bool owns)
		{
			return Create (CMFormatDescriptionGetMediaType (handle), handle, owns);
		}

		static CMFormatDescription Create (CMMediaType type, IntPtr handle, bool owns)
		{		
			switch (type) {
			case CMMediaType.Video:
				return new CMVideoFormatDescription (handle);
			case CMMediaType.Audio:
				return new CMAudioFormatDescription (handle);
			default:
				return new CMFormatDescription (handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMAudioFormatDescriptionGetStreamBasicDescription (IntPtr handle);

		public AudioStreamBasicDescription? AudioStreamBasicDescription {
			get {
				var ret = CMAudioFormatDescriptionGetStreamBasicDescription (handle);
				if (ret != IntPtr.Zero){
					unsafe {
						return *((AudioStreamBasicDescription *) ret);
					}
				}
				return null;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMAudioFormatDescriptionGetChannelLayout (IntPtr handle, out int size);
			
		public AudioChannelLayout AudioChannelLayout {
			get {
				int size;
				var res = CMAudioFormatDescriptionGetChannelLayout (handle, out size);
				if (res == IntPtr.Zero || size == 0)
					return null;
				return AudioChannelLayout.FromHandle (res);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMAudioFormatDescriptionGetFormatList (IntPtr handle, out int size);
		public AudioFormat [] AudioFormats {
			get {
				unsafe {
					int size;
					var v = CMAudioFormatDescriptionGetFormatList (handle, out size);
					if (v == IntPtr.Zero)
						return null;
					var items = size / sizeof (AudioFormat);
					var ret = new AudioFormat [items];
					var ptr = (AudioFormat *) v;
					for (int i = 0; i < items; i++)
						ret [i] = ptr [i];
					return ret;
				}
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMAudioFormatDescriptionGetMagicCookie (IntPtr handle, out int size);

		public byte [] AudioMagicCookie {
			get {
				int size;
				var h = CMAudioFormatDescriptionGetMagicCookie (handle, out size);
				if (h == IntPtr.Zero)
					return null;

				var result = new byte [size];
				for (int i = 0; i < size; i++)
					result [i] = Marshal.ReadByte (h, i);
				return result;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMAudioFormatDescriptionGetMostCompatibleFormat (IntPtr handle);

		public AudioFormat AudioMostCompatibleFormat {
			get {
				unsafe {
					var ret = (AudioFormat *) CMAudioFormatDescriptionGetMostCompatibleFormat (handle);
					if (ret == null)
						return new AudioFormat ();
					return *ret;
				}
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMAudioFormatDescriptionGetRichestDecodableFormat (IntPtr handle);

		public AudioFormat AudioRichestDecodableFormat {
			get {
				unsafe {
					var ret = (AudioFormat *) CMAudioFormatDescriptionGetRichestDecodableFormat (handle);
					if (ret == null)
						return new AudioFormat ();
					return *ret;
				}
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static CMVideoDimensions CMVideoFormatDescriptionGetDimensions (IntPtr handle);

		[Advice ("Use CMVideoFormatDescription")]
		public CMVideoDimensions  VideoDimensions {
			get {
				return CMVideoFormatDescriptionGetDimensions (handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static CGRect CMVideoFormatDescriptionGetCleanAperture (IntPtr handle, bool originIsAtTopLeft);

		[Advice ("Use CMVideoFormatDescription")]
		public CGRect GetVideoCleanAperture (bool originIsAtTopLeft)
		{
			return CMVideoFormatDescriptionGetCleanAperture (handle, originIsAtTopLeft);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static IntPtr CMVideoFormatDescriptionGetExtensionKeysCommonWithImageBuffers ();

		// Belongs to CMVideoFormatDescription
		public static NSObject [] GetExtensionKeysCommonWithImageBuffers ()
		{
			var arr = CMVideoFormatDescriptionGetExtensionKeysCommonWithImageBuffers ();
			return NSArray.ArrayFromHandle<NSString> (arr);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		internal extern static CGSize CMVideoFormatDescriptionGetPresentationDimensions (IntPtr handle, bool usePixelAspectRatio, bool useCleanAperture);

		[Advice ("Use CMVideoFormatDescription")]
		public CGSize GetVideoPresentationDimensions (bool usePixelAspectRatio, bool useCleanAperture)
		{
			return CMVideoFormatDescriptionGetPresentationDimensions (handle, usePixelAspectRatio, useCleanAperture);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static int CMVideoFormatDescriptionMatchesImageBuffer (IntPtr handle, IntPtr imageBufferRef);

		// Belongs to CMVideoFormatDescription
		public bool VideoMatchesImageBuffer (CVImageBuffer imageBuffer)
		{
			if (imageBuffer == null)
				throw new ArgumentNullException ("imageBuffer");
			return CMVideoFormatDescriptionMatchesImageBuffer (handle, imageBuffer.Handle) != 0;
		}
#endif
	}

	[Since (4,0)]
	public class CMAudioFormatDescription : CMFormatDescription {
		
		internal CMAudioFormatDescription (IntPtr handle)
			: base (handle)
		{
		}

		internal CMAudioFormatDescription (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		// TODO: Move more audio specific methods here
	}

	[Since (4,0)]
	public class CMVideoFormatDescription : CMFormatDescription {
		
		internal CMVideoFormatDescription (IntPtr handle)
			: base (handle)
		{
		}

		internal CMVideoFormatDescription (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreMediaLibrary)]
		static extern CMFormatDescriptionError CMVideoFormatDescriptionCreate (IntPtr allocator,
			CMVideoCodecType codecType,
			int width, int height,
			IntPtr extensions,
			out IntPtr outDesc);

		public CMVideoFormatDescription (CMVideoCodecType codecType, CMVideoDimensions size)
			: base (IntPtr.Zero)
		{
			var error = CMVideoFormatDescriptionCreate (IntPtr.Zero, codecType, size.Width, size.Height, IntPtr.Zero, out handle);
			if (error != CMFormatDescriptionError.None)
				throw new ArgumentException (error.ToString ());
		}

#if !COREBUILD
		public CMVideoDimensions Dimensions {
			get {
				return CMVideoFormatDescriptionGetDimensions (handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		static extern CMFormatDescriptionError CMVideoFormatDescriptionCreateForImageBuffer (IntPtr allocator, 
			IntPtr imageBuffer,
			out IntPtr outDesc);

		public static CMVideoFormatDescription CreateForImageBuffer (CVImageBuffer imageBuffer, out CMFormatDescriptionError error)
		{
			if (imageBuffer == null)
				throw new ArgumentNullException ("imageBuffer");

			IntPtr desc;
			error = CMVideoFormatDescriptionCreateForImageBuffer (IntPtr.Zero, imageBuffer.handle, out desc);
			if (error != CMFormatDescriptionError.None)
				return null;

			return new CMVideoFormatDescription (desc, true);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CGRect CMVideoFormatDescriptionGetCleanAperture (IntPtr handle, bool originIsAtTopLeft);

		public CGRect GetCleanAperture (bool originIsAtTopLeft)
		{
			return CMVideoFormatDescriptionGetCleanAperture (handle, originIsAtTopLeft);
		}

		public CGSize GetPresentationDimensions (bool usePixelAspectRatio, bool useCleanAperture)
		{
			return CMVideoFormatDescriptionGetPresentationDimensions (handle, usePixelAspectRatio, useCleanAperture);
		}
#endif
	}
}
