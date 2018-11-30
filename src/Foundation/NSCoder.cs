//
// NSCoder support
//
// Author:
//   Miguel de Icaza
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

namespace MonoMac.Foundation {

	public partial class NSCoder {
		public void Encode (byte [] buffer, string key)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (key == null)
				throw new ArgumentNullException ("key");

			unsafe {
				fixed (byte *p = &buffer[0]){
					EncodeBlock ((IntPtr) p, buffer.Length, key);
				}
			}
		}

		public void Encode (byte [] buffer, int offset, int count, string key)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (key == null)
				throw new ArgumentNullException ("key");

			if (offset < 0)
				throw new ArgumentException ("offset < 0");
			if (count < 0)
				throw new ArgumentException ("count < 0");

                        if (offset > buffer.Length - count)
                                throw new ArgumentException ("Reading would overrun buffer");
			
			unsafe {
				fixed (byte *p = &buffer[0]){
					EncodeBlock ((IntPtr) p, buffer.Length, key);
				}
			}
		}

		public byte [] DecodeBytes (string key)
		{
			unsafe {
				int len = 0;
				int *pl = &len;
				IntPtr ret = DecodeBytes (key, (IntPtr) pl);
				if (ret == IntPtr.Zero)
					return null;
				
				byte [] retarray = new byte [len];
				Marshal.Copy (ret, retarray, 0, len);
				
				return retarray;
			}
		}
	}
}