//
// MonoMac.CoreFoundation.CFWriteStream
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
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {

	public class CFWriteStream : CFStream {
		internal CFWriteStream (IntPtr handle)
			: base (handle)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFWriteStreamCopyError (IntPtr handle);

		public override CFException GetError ()
		{
			var error = CFWriteStreamCopyError (Handle);
			if (error == IntPtr.Zero)
				return null;
			return CFException.FromCFError (error);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFWriteStreamOpen (IntPtr handle);

		protected override bool DoOpen ()
		{
			return CFWriteStreamOpen (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFWriteStreamClose (IntPtr handle);

		protected override void DoClose ()
		{
			CFWriteStreamClose (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static CFStreamStatus CFWriteStreamGetStatus (IntPtr handle);

		protected override CFStreamStatus DoGetStatus ()
		{
			return CFWriteStreamGetStatus (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFWriteStreamCanAcceptBytes (IntPtr handle);

		public bool CanAcceptBytes ()
		{
			return CFWriteStreamCanAcceptBytes (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern CFIndex CFWriteStreamWrite (IntPtr handle, IntPtr buffer, CFIndex count);

		public int Write (byte[] buffer)
		{
			return Write (buffer, 0, buffer.Length);
		}

		public int Write (byte[] buffer, int offset, int count)
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
				return CFWriteStreamWrite (Handle, ptr, count);
			} finally {
				gch.Free ();
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern bool CFWriteStreamSetClient (IntPtr stream, CFIndex eventTypes,
		                                           CFStreamCallback cb, IntPtr context);

		protected override bool DoSetClient (CFStreamCallback callback, CFIndex eventTypes,
		                                     IntPtr context)
		{
			return CFWriteStreamSetClient (Handle, eventTypes, callback, context);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFWriteStreamScheduleWithRunLoop (IntPtr handle, IntPtr loop, IntPtr mode);

		protected override void ScheduleWithRunLoop (CFRunLoop loop, NSString mode)
		{
			CFWriteStreamScheduleWithRunLoop (Handle, loop.Handle, mode.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFWriteStreamUnscheduleFromRunLoop (IntPtr handle, IntPtr loop, IntPtr mode);

		protected override void UnscheduleFromRunLoop (CFRunLoop loop, NSString mode)
		{
			CFWriteStreamUnscheduleFromRunLoop (Handle, loop.Handle, mode.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFReadStreamCopyProperty (IntPtr handle, IntPtr name);

		protected override IntPtr DoGetProperty (NSString name)
		{
			return CFReadStreamCopyProperty (Handle, name.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFWriteStreamSetProperty (IntPtr handle, IntPtr name, IntPtr value);

		protected override bool DoSetProperty (NSString name, INativeObject value)
		{
			return CFWriteStreamSetProperty (Handle, name.Handle, value.Handle);
		}
	}
}

