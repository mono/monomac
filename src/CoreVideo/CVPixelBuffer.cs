// 
// CVPixelBuffer.cs: Implements the managed CVPixelBuffer
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreVideo {

	[Since (4,0)]
	public class CVPixelBuffer : CVImageBuffer {
		public static readonly NSString PixelFormatTypeKey;
		public static readonly NSString MemoryAllocatorKey;
		public static readonly NSString WidthKey;
		public static readonly NSString HeightKey;
		public static readonly NSString ExtendedPixelsLeftKey;
		public static readonly NSString ExtendedPixelsTopKey;
		public static readonly NSString ExtendedPixelsRightKey;
		public static readonly NSString ExtendedPixelsBottomKey;
		public static readonly NSString BytesPerRowAlignmentKey;
		public static readonly NSString CGBitmapContextCompatibilityKey;
		public static readonly NSString CGImageCompatibilityKey;
		public static readonly NSString OpenGLCompatibilityKey;
		public static readonly NSString IOSurfacePropertiesKey;
		public static readonly NSString PlaneAlignmentKey;
		public static readonly NSString OpenGLESCompatibilityKey;

		public static readonly int CVImageBufferType;
		
		[DllImport (Constants.CoreVideoLibrary)]
		extern static int CVPixelBufferGetTypeID ();
		
		static CVPixelBuffer ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreVideoLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				PixelFormatTypeKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferPixelFormatTypeKey");
				MemoryAllocatorKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferMemoryAllocatorKey");
				WidthKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferWidthKey");
				HeightKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferHeightKey");
				ExtendedPixelsLeftKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferExtendedPixelsLeftKey");
				ExtendedPixelsTopKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferExtendedPixelsTopKey");
				ExtendedPixelsRightKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferExtendedPixelsRightKey");
				ExtendedPixelsBottomKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferExtendedPixelsBottomKey");
				BytesPerRowAlignmentKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferBytesPerRowAlignmentKey");
				CGBitmapContextCompatibilityKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferCGBitmapContextCompatibilityKey");
				CGImageCompatibilityKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferCGImageCompatibilityKey");
				OpenGLCompatibilityKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferOpenGLCompatibilityKey");
				IOSurfacePropertiesKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferIOSurfacePropertiesKey");
				PlaneAlignmentKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferPlaneAlignmentKey");
				CVImageBufferType = CVPixelBufferGetTypeID ();
				OpenGLESCompatibilityKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferOpenGLESCompatibilityKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}

		internal CVPixelBuffer (IntPtr handle) : base (handle)
		{
		}

		[Preserve (Conditional=true)]
		internal CVPixelBuffer (IntPtr handle, bool owns) : base (handle, owns)
		{
		}

#if !COREBUILD
		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferCreate (IntPtr allocator, IntPtr width, IntPtr height, CVPixelFormatType pixelFormatType, IntPtr pixelBufferAttributes, IntPtr pixelBufferOut);

#if SDCONVERT
		public CVPixelBuffer (System.Drawing.Size size, CVPixelFormatType pixelFormat)
			: this (size.Width, size.Height, pixelFormat, (NSDictionary)null)
		{
		}

		public CVPixelBuffer (System.Drawing.Size size, CVPixelFormatType pixelFormatType, CVPixelBufferAttributes attributes)
			: this (size.Width, size.Height, pixelFormatType, attributes == null ? null : attributes.Dictionary)
		{
		}
#endif
		public CVPixelBuffer (int width, int height, CVPixelFormatType pixelFormatType)
			: this (width, height, pixelFormatType, (NSDictionary)null)
		{
		}
		
		public CVPixelBuffer (int width, int height, CVPixelFormatType pixelFormatType, CVPixelBufferAttributes attributes)
			: this (width, height, pixelFormatType, attributes == null ? null : attributes.Dictionary)
		{
		}

		[Advice ("Use constructor with CVPixelBufferAttributes")]
		public CVPixelBuffer (int width, int height, CVPixelFormatType pixelFormatType, NSDictionary pixelBufferAttributes)
		{
			if (width <= 0)
				throw new ArgumentOutOfRangeException ("width");

			if (height <= 0)
				throw new ArgumentOutOfRangeException ("height");

			IntPtr pixelBufferOut = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			CVReturn ret = CVPixelBufferCreate (IntPtr.Zero, (IntPtr) width, (IntPtr) height, pixelFormatType,
				pixelBufferAttributes == null ? IntPtr.Zero : pixelBufferAttributes.Handle, pixelBufferOut);

			if (ret != CVReturn.Success) {
				Marshal.FreeHGlobal (pixelBufferOut);
				throw new ArgumentException (ret.ToString ());
			}

			this.handle = Marshal.ReadIntPtr (pixelBufferOut);
			Marshal.FreeHGlobal (pixelBufferOut);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferCreateResolvedAttributesDictionary (IntPtr allocator, IntPtr attributes, IntPtr resolvedDictionaryOut);
		public NSDictionary GetAttributes (NSDictionary [] attributes)
		{
			IntPtr resolvedDictionaryOut = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			NSArray attributeArray = NSArray.FromNSObjects (attributes);
			CVReturn ret = CVPixelBufferCreateResolvedAttributesDictionary (IntPtr.Zero, attributeArray.Handle, resolvedDictionaryOut);

			if (ret != CVReturn.Success) {
				Marshal.FreeHGlobal (resolvedDictionaryOut);
				throw new Exception ("CVPixelBufferCreate returned: " + ret);
			}
			
			NSDictionary dictionary = Runtime.GetNSObject<NSDictionary> (Marshal.ReadIntPtr (resolvedDictionaryOut));
			Marshal.FreeHGlobal (resolvedDictionaryOut);

			return dictionary;
		}
#endif

		// TODO: CVPixelBufferCreateWithBytes
		// TODO: CVPixelBufferCreateWithPlanarBytes
		// TODO: CVPixelBufferGetExtendedPixels
		// TODO: CVPixelBufferGetTypeID

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferFillExtendedPixels (IntPtr pixelBuffer);
		public CVReturn FillExtendedPixels ()
		{
			return CVPixelBufferFillExtendedPixels (handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetBaseAddress (IntPtr pixelBuffer);
		public IntPtr BaseAddress {
			get {
				return CVPixelBufferGetBaseAddress (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetBytesPerRow (IntPtr pixelBuffer);
		public int BytesPerRow {
			get {
				return (int) CVPixelBufferGetBytesPerRow (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetDataSize (IntPtr pixelBuffer);
		public int DataSize {
			get {
				return (int) CVPixelBufferGetDataSize (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetHeight (IntPtr pixelBuffer);
		public int Height {
			get {
				return (int) CVPixelBufferGetHeight (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetWidth (IntPtr pixelBuffer);
		public int Width {
			get {
				return (int) CVPixelBufferGetWidth (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetPlaneCount (IntPtr pixelBuffer);
		public int PlaneCount {
			get {
				return (int) CVPixelBufferGetPlaneCount (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static bool CVPixelBufferIsPlanar (IntPtr pixelBuffer);
		public bool IsPlanar {
			get {
				return CVPixelBufferIsPlanar (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVPixelFormatType CVPixelBufferGetPixelFormatType (IntPtr pixelBuffer);
		public CVPixelFormatType PixelFormatType {
			get {
				return CVPixelBufferGetPixelFormatType (handle);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetBaseAddressOfPlane (IntPtr pixelBuffer, IntPtr planeIndex);
		public IntPtr GetBaseAddress (int planeIndex) {
			return CVPixelBufferGetBaseAddressOfPlane (handle, (IntPtr) planeIndex);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetBytesPerRowOfPlane (IntPtr pixelBuffer, IntPtr planeIndex);
		public int GetBytesPerRowOfPlane (int planeIndex) {
			return (int) CVPixelBufferGetBytesPerRowOfPlane (handle, (IntPtr) planeIndex);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetHeightOfPlane (IntPtr pixelBuffer, IntPtr planeIndex);
		public int GetHeightOfPlane (int planeIndex) {
			return (int) CVPixelBufferGetHeightOfPlane (handle, (IntPtr) planeIndex);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferGetWidthtOfPlane (IntPtr pixelBuffer, IntPtr planeIndex);
		public int GetWidthtOfPlane (int planeIndex) {
			return (int) CVPixelBufferGetWidthtOfPlane (handle, (IntPtr) planeIndex);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferLockBaseAddress (IntPtr pixelBuffer, CVOptionFlags lockFlags);
		public CVReturn Lock (CVOptionFlags lockFlags) {
			return CVPixelBufferLockBaseAddress (handle, lockFlags);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferUnlockBaseAddress (IntPtr pixelBuffer, CVOptionFlags unlockFlags);
		public CVReturn Unlock (CVOptionFlags unlockFlags) {
			return CVPixelBufferUnlockBaseAddress (handle, unlockFlags);
		}
	}
}
