// 
// CVPixelBufferPool.cs: Implements the managed CVPixelBufferPool
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2010 Novell, Inc
// Copyright 2012, Xamarin Inc.
//
using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreVideo {

	[Since (4,0)]
	public class CVPixelBufferPool : INativeObject, IDisposable {
		public static readonly NSString MinimumBufferCountKey;
		public static readonly NSString MaximumBufferAgeKey;

		static CVPixelBufferPool ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreVideoLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				MinimumBufferCountKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferPoolMinimumBufferCountKey");
				MaximumBufferAgeKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferPoolMaximumBufferAgeKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}

		IntPtr handle;

		internal CVPixelBufferPool (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("Invalid parameters to context creation");

			CVPixelBufferPoolRetain (handle);
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CVPixelBufferPool (IntPtr handle, bool owns)
		{
			if (!owns)
				CVPixelBufferPoolRetain (handle);

			this.handle = handle;
		}

		~CVPixelBufferPool ()
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
	
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVPixelBufferPoolRelease (IntPtr handle);
		
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVPixelBufferPoolRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CVPixelBufferPoolRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static int CVPixelBufferPoolGetTypeID ();
		public int TypeID {
			get {
				return CVPixelBufferPoolGetTypeID ();
			}
		}

#if !COREBUILD
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferPoolGetPixelBufferAttributes (IntPtr pool);

		// TODO: Return type is CVPixelBufferAttributes but need different name when this one is not WeakXXXX
		public NSDictionary PixelBufferAttributes {
			get {
				return Runtime.GetNSObject<NSDictionary> (CVPixelBufferPoolGetPixelBufferAttributes (handle));
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferPoolGetAttributes (IntPtr pool);

		public NSDictionary Attributes {
			get {
				return Runtime.GetNSObject<NSDictionary> (CVPixelBufferPoolGetAttributes (handle));
			}
		}

		public CVPixelBufferPoolSettings Settings {
			get {
				var attr = Attributes;
				return attr == null ? null : new CVPixelBufferPoolSettings (attr);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferPoolCreatePixelBuffer (IntPtr allocator, IntPtr pixelBufferPool, IntPtr pixelBufferOut);
		public CVPixelBuffer CreatePixelBuffer ()
		{
			IntPtr pixelBufferOut = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			CVReturn ret = CVPixelBufferPoolCreatePixelBuffer (IntPtr.Zero, handle, pixelBufferOut);

			if (ret != CVReturn.Success) {
				Marshal.FreeHGlobal (pixelBufferOut);
				throw new Exception ("CVPixelBufferPoolCreatePixelBuffer returned " + ret.ToString ());
			}

			CVPixelBuffer pixelBuffer = new CVPixelBuffer (Marshal.ReadIntPtr (pixelBufferOut));
			Marshal.FreeHGlobal (pixelBufferOut);
			return pixelBuffer;
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferPoolCreatePixelBufferWithAuxAttributes (IntPtr allocator, IntPtr pixelBufferPool, IntPtr auxAttributes, out IntPtr pixelBufferOut);

		public CVPixelBuffer CreatePixelBuffer (CVPixelBufferPoolAllocationSettings allocationSettings, out CVReturn error)
		{
			IntPtr pb;
			error = CVPixelBufferPoolCreatePixelBufferWithAuxAttributes (IntPtr.Zero, handle, allocationSettings == null ? IntPtr.Zero : allocationSettings.Dictionary.Handle, out pb);
			if (error != CVReturn.Success)
				return null;

			return new CVPixelBuffer (pb);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferPoolCreate (IntPtr allocator, IntPtr poolAttributes, IntPtr pixelBufferAttributes, IntPtr poolOut);

		[Advice ("Use overload with CVPixelBufferPoolSettings")]
		public CVPixelBufferPool (NSDictionary poolAttributes, NSDictionary pixelBufferAttributes)
		{
			if (pixelBufferAttributes == null)
				throw new ArgumentNullException ("pixelBufferAttributes");

			IntPtr poolOut = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			CVReturn ret = CVPixelBufferPoolCreate (IntPtr.Zero, poolAttributes == null ? IntPtr.Zero : poolAttributes.Handle, pixelBufferAttributes.Handle, poolOut);

			if (ret != CVReturn.Success) {
				Marshal.FreeHGlobal (poolOut);
				throw new Exception ("CVPixelBufferPoolCreate returned " + ret.ToString ());
			}

			this.handle = Marshal.ReadIntPtr (poolOut);
			Marshal.FreeHGlobal (poolOut);
		}

		public CVPixelBufferPool (CVPixelBufferPoolSettings settings, CVPixelBufferAttributes pixelBufferAttributes)
			: this (settings == null ? null : settings.Dictionary, pixelBufferAttributes == null ? null : pixelBufferAttributes.Dictionary)
		{
		}
#endif
	}
}
