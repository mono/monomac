// 
// CGDataConsumer.cs: Implements the managed CGDataConsumer
//
// Authors: Ademar Gonzalez
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

	public partial class CGDataConsumer : INativeObject, IDisposable {
		internal IntPtr handle;
		IntPtr buffer;
		byte [] reference;
		
		// invoked by marshallers
		public CGDataConsumer (IntPtr handle)
			: this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGDataConsumer (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGDataConsumerRetain (handle);
		}
		
		~CGDataConsumer ()
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
		extern static void CGDataConsumerRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGDataConsumerRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal (buffer);
				buffer = IntPtr.Zero;
				CGDataConsumerRelease (handle);
				handle = IntPtr.Zero;
			}
			reference = null;
		}


		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGDataConsumerCreateWithCFData (IntPtr data);
		
		public CGDataConsumer(NSMutableData data){
			handle = CGDataConsumerCreateWithCFData(data.Handle);
		}

	}
}
