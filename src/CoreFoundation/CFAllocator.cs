// 
// CFAllocator.cs
//
// Authors:
//    Rolf Bjarne Kvinge
//    Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012 Xamarin Inc
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

namespace MonoMac.CoreFoundation {	
	public class CFAllocator : INativeObject, IDisposable 
	{
		static CFAllocator Default_cf;
		static CFAllocator SystemDefault_cf;
		static CFAllocator Malloc_cf;
		static CFAllocator MallocZone_cf;
		static CFAllocator Null_cf;

		static readonly IntPtr default_ptr;
		static readonly IntPtr system_default_ptr;
		static readonly IntPtr malloc_ptr;
		static readonly IntPtr malloc_zone_ptr;
		internal static readonly IntPtr null_ptr;
		//static readonly IntPtr UseContextFlag;

		IntPtr handle;
		
		static CFAllocator ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreFoundationLibrary, 0);
			try {
				default_ptr = Dlfcn.GetIntPtr (handle, "kCFAllocatorDefault");
				system_default_ptr = Dlfcn.GetIntPtr (handle, "kCFAllocatorSystemDefault");
				malloc_ptr = Dlfcn.GetIntPtr (handle, "kCFAllocatorMalloc");
				malloc_zone_ptr = Dlfcn.GetIntPtr (handle, "kCFAllocatorMallocZone");
				null_ptr = Dlfcn.GetIntPtr (handle, "kCFAllocatorNull");
			//	UseContextFlag = Dlfcn.GetIntPtr (handle, "kCFAllocatorUseContext");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}

		public CFAllocator (IntPtr handle)
		{
			this.handle = handle;
		}

		public CFAllocator (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);
			this.handle = handle;
		}

		~CFAllocator ()
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

		public static CFAllocator Default {
			get {
				return Default_cf ?? (Default_cf = new CFAllocator (default_ptr)); 
			}
		}

		public static CFAllocator SystemDefault {
			get {
				return SystemDefault_cf ?? (SystemDefault_cf = new CFAllocator (system_default_ptr)); 
			}
		}
		
		public static CFAllocator Malloc {
			get {
				return Malloc_cf ?? (Malloc_cf = new CFAllocator (malloc_ptr)); 
			}
		}

		public static CFAllocator MallocZone {
			get {
				return MallocZone_cf ?? (MallocZone_cf = new CFAllocator (malloc_zone_ptr)); 
			}
		}

		public static CFAllocator Null {
			get {
				return Null_cf ?? (Null_cf = new CFAllocator (null_ptr)); 
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFAllocatorAllocate (IntPtr allocator, /*CFIndex*/ long size, CFAllocatorFlags hint);

		public IntPtr Allocate (long size, CFAllocatorFlags hint = 0)
		{
			return CFAllocatorAllocate (handle, size, hint);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFAllocatorDeallocate(IntPtr allocator, IntPtr ptr);

		public void Deallocate (IntPtr ptr)
		{
			CFAllocatorDeallocate (handle, ptr);
		}

		// TODO: Implement more methods
	}

	// Seems to be some sort of secret values
	[Flags]
	public enum CFAllocatorFlags : ulong
	{
		GCScannedMemory	= 0x200,
		GCObjectMemory	= 0x400,
	}
}