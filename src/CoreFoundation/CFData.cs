// 
// CFData.cs: Implements the managed CFData
//
// Authors:
//    Rolf Bjarne Kvinge (rolf@xamarin.com)
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

	class CFData : INativeObject, IDisposable {
		internal IntPtr handle;

		public CFData (IntPtr handle)
			: this (handle, false)
		{
		}

		public CFData (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);
			this.handle = handle;
		}

		~CFData ()
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
		
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint="CFDataGetTypeID")]
		public extern static int GetTypeID ();

		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
		
		public static CFData FromDataNoCopy (IntPtr buffer, int length)
		{
			return new CFData (CFDataCreateWithBytesNoCopy (IntPtr.Zero, buffer, length, CFAllocator.null_ptr), true);
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFDataCreateWithBytesNoCopy (IntPtr allocator, IntPtr bytes, int len, IntPtr deallocator);

		public int Length {
			get { return CFDataGetLength (handle); }
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static CFIndex CFDataGetLength (IntPtr data);

		public byte[] GetBuffer ()
		{
			var buffer = new byte [Length];
			var ptr = CFDataGetBytePtr (handle);
			Marshal.Copy (ptr, buffer, 0, buffer.Length);
			return buffer;
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFDataGetBytePtr (IntPtr data);

		/*
		 * Exposes a read-only pointer to the underlying storage.
		 */
		public IntPtr Bytes {
			get { return CFDataGetBytePtr (handle); }
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFDataCreate (IntPtr allocator, IntPtr bytes, CFIndex len);

		public static CFData FromData (IntPtr buffer, int length)
		{
			return new CFData (CFDataCreate (IntPtr.Zero, buffer, length), true);
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFDataCreateCopy (IntPtr allocator, IntPtr data);

		public CFData Copy ()
		{
			return new CFData (CFDataCreateCopy (IntPtr.Zero, Handle), true);
		}
	}
}
