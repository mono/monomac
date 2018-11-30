//
// Block support
//
// Copyright 2010, Novell, Inc.
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
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.ObjCRuntime {

	[StructLayout (LayoutKind.Sequential)]
	public struct BlockDescriptor {
		public IntPtr reserved;
		public IntPtr size;
		public IntPtr copy_helper;
		public IntPtr dispose;

		private unsafe static CopyHelperDelegate copy_helper_delegate = new CopyHelperDelegate (CopyHelper);
		private unsafe static DisposeHelperDelegate dispose_helper_delegate = new DisposeHelperDelegate (DisposeHelper);
		private static IntPtr copy_helper_ptr = Marshal.GetFunctionPointerForDelegate (copy_helper_delegate);
		private static IntPtr dispose_helper_ptr = Marshal.GetFunctionPointerForDelegate (dispose_helper_delegate);

		private unsafe delegate void CopyHelperDelegate (BlockLiteral *dest, BlockLiteral *src);
		private unsafe delegate void DisposeHelperDelegate (BlockLiteral *block);

		[MonoPInvokeCallback (typeof (CopyHelperDelegate))]
		static unsafe void CopyHelper (BlockLiteral *dest, BlockLiteral *source) {
			dest->global_handle = (IntPtr) GCHandle.Alloc (GCHandle.FromIntPtr (dest->local_handle).Target);
		}

		[MonoPInvokeCallback (typeof (DisposeHelperDelegate))]
		static unsafe void DisposeHelper (BlockLiteral *block) {
			GCHandle.FromIntPtr (block->global_handle).Free ();
		}

		internal static BlockDescriptor CreateDescriptor () {
			BlockDescriptor d = new BlockDescriptor ();
			d.copy_helper = copy_helper_ptr;
			d.dispose = dispose_helper_ptr;
			d.size = (IntPtr) (Marshal.SizeOf (typeof (IntPtr))*5 + Marshal.SizeOf (typeof (int))*2);

			return d;
		}	
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct BlockLiteral {
		public IntPtr isa;
		public BlockFlags flags;
		public int reserved;
		public IntPtr invoke;
		public IntPtr block_descriptor;
		public IntPtr local_handle;
		public IntPtr global_handle;

		internal static IntPtr block_class = Class.GetHandle ("__NSStackBlock");
		internal static BlockDescriptor global_descriptor = BlockDescriptor.CreateDescriptor ();
		internal static IntPtr global_descriptor_ptr;

		static BlockLiteral () {
			global_descriptor_ptr = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr))*2 + Marshal.SizeOf (typeof (IntPtr))*2);
			Marshal.StructureToPtr (global_descriptor, global_descriptor_ptr, false);
		}

		//
		// trampoline must be static, and someone else needs to keep a ref to it
		//
		public unsafe void SetupBlock (Delegate trampoline, Delegate userDelegate)
		{
			isa = block_class;
			invoke = Marshal.GetFunctionPointerForDelegate (trampoline);
			local_handle = (IntPtr) GCHandle.Alloc (userDelegate);
			global_handle = IntPtr.Zero;
			flags = BlockFlags.BLOCK_HAS_DESCRIPTOR | BlockFlags.BLOCK_HAS_COPY_DISPOSE;
			block_descriptor = global_descriptor_ptr;
		}

		public unsafe void CleanupBlock ()
		{
			GCHandle.FromIntPtr (local_handle).Free ();
		}
	}

	[Flags]
	public enum BlockFlags {
		BLOCK_REFCOUNT_MASK =     (0xffff),
		BLOCK_NEEDS_FREE =        (1 << 24),
		BLOCK_HAS_COPY_DISPOSE =  (1 << 25),
		BLOCK_HAS_CTOR =          (1 << 26), /* Helpers have C++ code. */
		BLOCK_IS_GC =             (1 << 27),
		BLOCK_IS_GLOBAL =         (1 << 28),
		BLOCK_HAS_DESCRIPTOR =    (1 << 29)
	}
}
