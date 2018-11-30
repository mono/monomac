// 
// CFUrl.cs: Implements the managed CFUrl
//
// Authors:
//     Miguel de Icaza
//     Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2009 Novell, Inc
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
	public enum CFUrlPathStyle {
		POSIX = 0,
		HFS = 1,
		Windows = 2
	};
	
	public class CFUrl : INativeObject, IDisposable {
		internal IntPtr handle;

		~CFUrl ()
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

		[DllImport (Constants.CoreFoundationLibrary, CharSet=CharSet.Unicode)]
		extern static IntPtr CFURLCreateWithFileSystemPath (IntPtr allocator, IntPtr cfstringref, CFUrlPathStyle pathstyle, bool isdir);
		
		internal CFUrl (IntPtr handle)
		{
			this.handle = handle;
		}
		
		static public CFUrl FromFile (string filename)
		{
			using (var str = new CFString (filename)){
				IntPtr handle = CFURLCreateWithFileSystemPath (IntPtr.Zero, str.Handle, CFUrlPathStyle.POSIX, false);
				if (handle == IntPtr.Zero)
					return null;
				return new CFUrl (handle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary, CharSet=CharSet.Unicode)]
		extern static IntPtr CFURLCreateWithString (IntPtr allocator, IntPtr stringref, IntPtr baseUrl);

		static public CFUrl FromUrlString (string url, CFUrl baseurl)
		{
			using (var str = new CFString (url)){
				return FromStringHandle (str.Handle, baseurl);
			}
		}

		internal static CFUrl FromStringHandle (IntPtr cfstringHandle, CFUrl baseurl)
		{
			IntPtr handle = CFURLCreateWithString (IntPtr.Zero, cfstringHandle, baseurl != null ? baseurl.Handle : IntPtr.Zero);
			if (handle == IntPtr.Zero)
				return null;
			return new CFUrl (handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFURLGetString (IntPtr anURL);
		
		public override string ToString ()
		{
			using (var str = new CFString (CFURLGetString (handle))) {
				return str.ToString ();
			}
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFURLCopyFileSystemPath (IntPtr cfUrl, int style);
		
		public string FileSystemPath {
			get {
				return GetFileSystemPath (handle);
			}
		}

		static internal string GetFileSystemPath (IntPtr hcfurl)
		{
			using (var str = new CFString (CFURLCopyFileSystemPath (hcfurl, 0), true))
				return str.ToString ();
		}

		[DllImport (Constants.CoreFoundationLibrary, EntryPoint="CFURLGetTypeID")]
		public extern static int GetTypeID ();
	}
	
}
