//
// MonoMac.CoreFoundation.CFReadStream
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
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {

	public class CFReadStream : CFStream {
		public CFReadStream (IntPtr handle)
			: base (handle)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFReadStreamCopyError (IntPtr handle);

		public override CFException GetError ()
		{
			var error = CFReadStreamCopyError (Handle);
			if (error == IntPtr.Zero)
				return null;
			return CFException.FromCFError (error);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFReadStreamOpen (IntPtr handle);

		protected override bool DoOpen ()
		{
			return CFReadStreamOpen (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFReadStreamClose (IntPtr handle);

		protected override void DoClose ()
		{
			CFReadStreamClose (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static CFStreamStatus CFReadStreamGetStatus (IntPtr handle);

		protected override CFStreamStatus DoGetStatus ()
		{
			return CFReadStreamGetStatus (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFReadStreamHasBytesAvailable (IntPtr handle);

		public bool HasBytesAvailable ()
		{
			return CFReadStreamHasBytesAvailable (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFReadStreamScheduleWithRunLoop (IntPtr handle, IntPtr loop, IntPtr mode);

		protected override void ScheduleWithRunLoop (CFRunLoop loop, NSString mode)
		{
			CFReadStreamScheduleWithRunLoop (Handle, loop.Handle, mode.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFReadStreamUnscheduleFromRunLoop (IntPtr handle, IntPtr loop, IntPtr mode);

		protected override void UnscheduleFromRunLoop (CFRunLoop loop, NSString mode)
		{
			CFReadStreamUnscheduleFromRunLoop (Handle, loop.Handle, mode.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern bool CFReadStreamSetClient (IntPtr stream, CFIndex eventTypes,
		                                          CFStreamCallback cb, IntPtr context);

		protected override bool DoSetClient (CFStreamCallback callback, CFIndex eventTypes,
		                                     IntPtr context)
		{
			return CFReadStreamSetClient (Handle, eventTypes, callback, context);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static CFIndex CFReadStreamRead (IntPtr handle, IntPtr buffer, CFIndex count);

		public int Read (byte[] buffer)
		{
			return Read (buffer, 0, buffer.Length);
		}

		public int Read (byte[] buffer, int offset, int count)
		{
			CheckHandle ();
			if (offset < 0)
				throw new ArgumentException ();
			if (count < 1)
				throw new ArgumentException ();
			if (offset + count > buffer.Length)
				throw new ArgumentException ();
			var gch = GCHandle.Alloc (buffer, GCHandleType.Pinned);
			try {
				var addr = gch.AddrOfPinnedObject ();
				var ptr = new IntPtr (addr.ToInt64 () + offset);
				return CFReadStreamRead (Handle, ptr, count);
			} finally {
				gch.Free ();
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFReadStreamCopyProperty (IntPtr handle, IntPtr name);

		protected override IntPtr DoGetProperty (NSString name)
		{
			return CFReadStreamCopyProperty (Handle, name.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFReadStreamSetProperty (IntPtr handle, IntPtr name, IntPtr value);

		protected override bool DoSetProperty (NSString name, INativeObject value)
		{
			return CFReadStreamSetProperty (Handle, name.Handle, value.Handle);
		}
	}
}
