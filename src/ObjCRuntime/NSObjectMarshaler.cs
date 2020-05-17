//
// Copyright 2010, Novell, Inc.
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

namespace MonoMac.ObjCRuntime {
	public class NSObjectMarshaler<T> : ICustomMarshaler where T : NSObject {
		static NSObjectMarshaler<T> marshaler;

		public object MarshalNativeToManaged (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;

			var obj = Runtime.GetNSObject (handle);
			if (!(obj is T)) {
				throw new MarshalDirectiveException (
					string.Format ("NSObjectMarshaler<{0}>: Could not cast from type {1}", typeof(T), obj.GetType ()));
			}
			else {
				return (T) obj;
			}
		}

		public IntPtr MarshalManagedToNative (object obj) {
			if (obj == null)
				return IntPtr.Zero;
			if (!(obj is T))
				throw new MarshalDirectiveException ("This custom marshaler must be used on a NSObject derived type.");

			return (obj as T).Handle;
		}

		public void CleanUpNativeData (IntPtr handle) {
		}

		public void CleanUpManagedData (object obj) {
		}

		public int GetNativeDataSize () {
			return -1;
		}

		public static ICustomMarshaler GetInstance(string cookie) {
			if(marshaler == null)
				return marshaler = new NSObjectMarshaler<T> ();

			return marshaler;
		}
	}
}
