// 
// CGImage.cs: Implements the managed CGImage
//
// Authors: Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
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

namespace MonoMac.CoreGraphics {

	public partial class CGDataProvider : INativeObject, IDisposable {
		internal IntPtr handle;
		IntPtr buffer;
		byte [] reference;
		
		// invoked by marshallers
		public CGDataProvider (IntPtr handle)
			: this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGDataProvider (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGDataProviderRetain (handle);
		}
		
		~CGDataProvider ()
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
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGDataProviderRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGDataProviderRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal (buffer);
				buffer = IntPtr.Zero;
				CGDataProviderRelease (handle);
				handle = IntPtr.Zero;
			}
			reference = null;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGDataProviderCreateWithFilename (string filename);

		static public CGDataProvider FromFile (string file)
		{
			if (file == null)
				throw new ArgumentNullException ("file");

			var handle = CGDataProviderCreateWithFilename (file);
			if (handle == IntPtr.Zero)
				return null;

			return new CGDataProvider (handle, true);
		}

		public CGDataProvider (string file)
		{
			if (file == null)
				throw new ArgumentNullException ("file");

			handle = CGDataProviderCreateWithFilename (file);
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("Could not create provider from the specified file");
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGDataProviderCreateWithData (IntPtr info, IntPtr data, IntPtr size, IntPtr releaseData);

		public CGDataProvider (IntPtr memoryBlock, int size)
		{
			handle = CGDataProviderCreateWithData (IntPtr.Zero, memoryBlock, new IntPtr(size), IntPtr.Zero);
		}

		public CGDataProvider (IntPtr memoryBlock, int size, bool ownBuffer)
		{
			handle = CGDataProviderCreateWithData (IntPtr.Zero, memoryBlock, new IntPtr(size), IntPtr.Zero);
			if (ownBuffer)
				buffer = memoryBlock;
		}

		public CGDataProvider (byte [] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentException ("offset");
			if (offset + count > buffer.Length)
				throw new ArgumentException ("offset");

			// Keep a reference alive to the byte array.
			reference = buffer;
			unsafe {
				fixed (byte *p = &buffer [offset]){
					handle = CGDataProviderCreateWithData (IntPtr.Zero, (IntPtr) p, new IntPtr(count), IntPtr.Zero);
				}
			}
		}
	}
}
