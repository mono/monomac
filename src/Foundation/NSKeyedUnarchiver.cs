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
// Copyright 2011, 2012 Xamarin Inc
using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSKeyedUnarchiver {

		public static void GlobalSetClass (Class kls, string codedName)
		{
			if (codedName == null)
				throw new ArgumentNullException ("codedName");
			if (kls == null)
				throw new ArgumentNullException ("kls");
			
			using (var nsname = new NSString (codedName))
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (class_ptr, selSetClassForClassName_Handle, kls.Handle, nsname.Handle);
		}

		public static Class GlobalGetClass (string codedName)
		{
			if (codedName == null)
				throw new ArgumentNullException ("codedName");
			using (var nsname = new NSString (codedName))
				return new Class (
						MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (
							class_ptr, selClassForClassName_Handle, nsname.Handle));
		}

	}
}
