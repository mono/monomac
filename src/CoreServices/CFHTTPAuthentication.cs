//
// MonoMac.CoreServices.CFHTTPAuthentication
//
// Authors:
//      Martin Baulig (martin.baulig@gmail.com)
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
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreServices {

	public class CFHTTPAuthentication : CFType, INativeObject, IDisposable {
		internal IntPtr handle;

		internal CFHTTPAuthentication (IntPtr handle)
			: this (handle, false)
		{
		}

		internal CFHTTPAuthentication (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);
			this.handle = handle;
		}

		[DllImport (Constants.CFNetworkLibrary, EntryPoint="CFHTTPAuthenticationGetTypeID")]
		public extern static int GetTypeID ();

		~CFHTTPAuthentication ()
		{
			Dispose (false);
		}
		
		protected void CheckHandle ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException (GetType ().Name);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get {
				CheckHandle ();
				return handle;
			}
		}
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero) {
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPAuthenticationCreateFromResponse (IntPtr allocator, IntPtr response);

		public static CFHTTPAuthentication CreateFromResponse (CFHTTPMessage response)
		{
			if (response.IsRequest)
				throw new InvalidOperationException ();

			var handle = CFHTTPAuthenticationCreateFromResponse (IntPtr.Zero, response.Handle);
			if (handle == IntPtr.Zero)
				return null;

			return new CFHTTPAuthentication (handle);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPAuthenticationIsValid (IntPtr handle, IntPtr error);

		public bool IsValid {
			get { return CFHTTPAuthenticationIsValid (Handle, IntPtr.Zero); }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPAuthenticationAppliesToRequest (IntPtr handle, IntPtr request);

		public bool AppliesToRequest (CFHTTPMessage request)
		{
			if (!request.IsRequest)
				throw new InvalidOperationException ();

			return CFHTTPAuthenticationAppliesToRequest (Handle, request.Handle);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPAuthenticationRequiresAccountDomain (IntPtr handle);

		public bool RequiresAccountDomain {
			get { return CFHTTPAuthenticationRequiresAccountDomain (Handle); }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPAuthenticationRequiresOrderedRequests (IntPtr handle);

		public bool RequiresOrderedRequests {
			get { return CFHTTPAuthenticationRequiresOrderedRequests (Handle); }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPAuthenticationRequiresUserNameAndPassword (IntPtr handle);

		public bool RequiresUserNameAndPassword {
			get { return CFHTTPAuthenticationRequiresUserNameAndPassword (Handle); }
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPAuthenticationCopyMethod (IntPtr handle);

		public string GetMethod ()
		{
			var ptr = CFHTTPAuthenticationCopyMethod (Handle);
			if (ptr == IntPtr.Zero)
				return null;
			using (var method = new CFString (ptr))
				return method;
		}
	}
}
