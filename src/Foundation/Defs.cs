//
// Copyright 2011, Xamarin, Inc.
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
	public struct NSAppleEventManagerSuspensionID
	{
		// Typedef'ed to a pointer
		IntPtr __NSAppleEventManagerSuspension;
	}

	public struct FourCharCode
	{
		// Typedef'ed to an uint
		uint fourcc;

		public static FourCharCode FromRawValue (uint fourcc)
		{
			var result = new FourCharCode ();
			result.fourcc = fourcc;
			return result;
		}

		public uint ToRawValue ()
		{
			return fourcc;
		}
	}

	public struct AEKeyword
	{
		// Typedef'ed to a fourcc
		FourCharCode keyword;

		public static AEKeyword FromFourCharCode (FourCharCode code)
		{
			var result = new AEKeyword ();
			result.keyword = code;
			return result;
		}

		public FourCharCode ToFourCharCode ()
		{
			return keyword;
		}
	}

	public struct OSType
	{
		// Typedef'ed to a fourcc
		FourCharCode type;

		public static OSType FromFourCharCode (FourCharCode code)
		{
			var result = new OSType ();
			result.type = code;
			return result;
		}

		public FourCharCode ToFourCharCode ()
		{
			return type;
		}
	}
}