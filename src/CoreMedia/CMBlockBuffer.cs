// 
// CMBlockBuffer.cs: Implements the managed CMBlockBuffer
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2010 Novell, Inc
// Copyright 2012 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {

	public enum CMBlockBufferError {
		None						= 0,
		StructureAllocationFailed	= -12700,
		BlockAllocationFailed		= -12701,
		BadCustomBlockSource		= -12702,
		BadOffsetParameter			= -12703,
		BadLengthParameter			= -12704,
		BadPointerParameter			= -12705,
		EmptyBlockBuffer			= -12706,
		UnallocatedBlock			= -12707,
	}

	[Flags]
	public enum CMBlockBufferFlags : uint {
		AssureMemoryNow			= (1<<0),
		AlwaysCopyData			= (1<<1),
		DontOptimizeDepth		= (1<<2),
		PermitEmptyReference	= (1<<3)
	}

	[Since (4,0)]
	public class CMBlockBuffer : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMBlockBuffer (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMBlockBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMBlockBuffer ()
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

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMBlockBufferError CMBlockBufferCreateEmpty (IntPtr allocator, uint subBlockCapacity, CMBlockBufferFlags flags, out IntPtr output);

		public static CMBlockBuffer CreateEmpty (uint subBlockCapacity, CMBlockBufferFlags flags, out CMBlockBufferError error)
		{
			IntPtr buffer;
			error = CMBlockBufferCreateEmpty (IntPtr.Zero, subBlockCapacity, flags, out buffer);
			if (error != CMBlockBufferError.None)
				return null;

			return new CMBlockBuffer (buffer, true);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMBlockBufferError CMBlockBufferCopyDataBytes (IntPtr handle, uint offsetToData, uint dataLength, IntPtr destination);
		
		public CMBlockBufferError CopyDataBytes (uint offsetToData, uint dataLength, IntPtr destination)
		{
			return CMBlockBufferCopyDataBytes (handle, offsetToData, dataLength, destination);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMBlockBufferGetDataLength (IntPtr handle);
		
		public uint DataLength
		{
			get
			{
				return CMBlockBufferGetDataLength (handle);
			}
		}
	}
}
