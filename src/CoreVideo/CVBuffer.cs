// 
// CVBuffer.cs: Implements the managed CVBuffer
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreVideo {

	[Since (4,0)]
	public class CVBuffer : INativeObject, IDisposable {
		public static readonly NSString MovieTimeKey;
		public static readonly NSString TimeValueKey;
		public static readonly NSString TimeScaleKey;
		public static readonly NSString PropagatedAttachmentsKey;
		public static readonly NSString NonPropagatedAttachmentsKey;

		static CVBuffer ()
		{
			var hlib = Dlfcn.dlopen (Constants.CoreVideoLibrary, 0);
			if (hlib == IntPtr.Zero)
				return;
			try {
				MovieTimeKey = Dlfcn.GetStringConstant (hlib, "kCVBufferMovieTimeKey");
				TimeValueKey = Dlfcn.GetStringConstant (hlib, "kCVBufferTimeValueKey");
				TimeScaleKey = Dlfcn.GetStringConstant (hlib, "kCVBufferTimeScaleKey");
				PropagatedAttachmentsKey = Dlfcn.GetStringConstant (hlib, "kCVBufferPropagatedAttachmentsKey");
				NonPropagatedAttachmentsKey = Dlfcn.GetStringConstant (hlib, "kCVBufferNonPropagatedAttachmentsKey");
			}
			finally {
				Dlfcn.dlclose (hlib);
			}
		}

		internal IntPtr handle;

		internal CVBuffer ()
		{
		}

		internal CVBuffer (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CVBufferRetain (handle);
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CVBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CVBufferRetain (handle);

			this.handle = handle;
		}

		~CVBuffer ()
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
		extern static void CVBufferRelease (IntPtr handle);
		
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CVBufferRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRemoveAllAttachments (IntPtr buffer);
		public void RemoveAllAttachments ()
		{
			CVBufferRemoveAllAttachments (handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRemoveAttachment (IntPtr buffer, IntPtr key);
		public void RemoveAttachment (NSString key)
		{
			CVBufferRemoveAttachment (handle, key.Handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVBufferGetAttachment (IntPtr buffer, IntPtr key, out CVAttachmentMode attachmentMode);
		public NSObject GetAttachment (NSString key, out CVAttachmentMode attachmentMode)
		{
			return Runtime.GetNSObject<NSObject> (CVBufferGetAttachment (handle, key.Handle, out attachmentMode));
		}

#if !MONOMAC_BOOTSTRAP && !COREBUILD
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVBufferGetAttachments (IntPtr buffer, CVAttachmentMode attachmentMode);
		public NSDictionary GetAttachments (CVAttachmentMode attachmentMode)
		{
			return Runtime.GetNSObject<NSDictionary> (CVBufferGetAttachments (handle, attachmentMode));
		}
#endif

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferPropagateAttachments (IntPtr sourceBuffer, IntPtr destinationBuffer);
		public void PropogateAttachments (CVBuffer destinationBuffer)
		{
			CVBufferPropagateAttachments (handle, destinationBuffer.Handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferSetAttachment (IntPtr buffer, IntPtr key, IntPtr @value, CVAttachmentMode attachmentMode);
		public void SetAttachment (NSString key, NSObject @value, CVAttachmentMode attachmentMode)
		{
			CVBufferSetAttachment (handle, key.Handle, @value.Handle, attachmentMode);
		}

#if !MONOMAC_BOOTSTRAP && !COREBUILD
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferSetAttachments (IntPtr buffer, IntPtr theAttachments, CVAttachmentMode attachmentMode);
		public void SetAttachments (NSDictionary theAttachments, CVAttachmentMode attachmentMode)
		{
			CVBufferSetAttachments (handle, theAttachments.Handle, attachmentMode);
		}
#endif
	}
}
