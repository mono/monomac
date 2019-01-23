//
// MonoMac.CoreServices.CFHost
//
// Authors:
//      Martin Baulig (martin.baulig@xamarin.com)
//
// Copyright 2012 Xamarin Inc. (http://www.xamarin.com)
//
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
using System.Net;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreServices {

	class CFHost : INativeObject, IDisposable {
		internal IntPtr handle;

		CFHost (IntPtr handle)
		{
			this.handle = handle;
		}

		~CFHost ()
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

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHostCreateWithAddress (IntPtr allocator, IntPtr address);

		public static CFHost Create (IPEndPoint endpoint)
		{
			using (var data = new CFSocketAddress (endpoint))
				return new CFHost (CFHostCreateWithAddress (IntPtr.Zero, data.Handle));
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHostCreateWithName (IntPtr allocator, IntPtr name);

		public static CFHost Create (string name)
		{
			var ptr = new CFString (name);
			return new CFHost (CFHostCreateWithName (IntPtr.Zero, ptr.Handle));
		}
	}
}
