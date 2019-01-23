//
// Dlfcn.cs: Support for looking up symbols in shared libraries
//
// Authors:
//   Jonathan Pryor:
//   Miguel de Icaza.
//
// Copyright 2009-2010, Novell, Inc.
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
//
using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;

namespace MonoMac.ObjCRuntime {
	
	public static class Dlfcn {

		[DllImport (Constants.SystemLibrary)]
		public static extern int dlclose (IntPtr handle);

		[DllImport (Constants.SystemLibrary)]
		public static extern IntPtr dlopen (string path, int mode);

		[DllImport (Constants.SystemLibrary)]
		public static extern IntPtr dlsym (IntPtr handle, string symbol);

		[DllImport (Constants.SystemLibrary, EntryPoint="dlerror")]
		internal static extern IntPtr dlerror_ ();

		public static string dlerror ()
		{
			// we can't free the string returned from dlerror
			return Marshal.PtrToStringAnsi (dlerror_ ());
		}

		public static NSString GetStringConstant (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return null;
			var actual = Marshal.ReadIntPtr (indirect);
			if (actual == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSString> (actual);
		}

		public static IntPtr GetIndirect (IntPtr handle, string symbol)
		{
			return dlsym (handle, symbol);
		}

#if !GENERATOR && !MONOMAC_BOOTSTRAP
		public static NSNumber GetNSNumber (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return null;
			var actual = Marshal.ReadIntPtr (indirect);
			if (actual == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSNumber> (actual);
		}
#endif

		public static int GetInt32 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadInt32 (indirect);
		}

		public static void SetInt32 (IntPtr handle, string symbol, int value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteInt32 (indirect, value);
		}
		
		public static long GetInt64 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadInt64 (indirect);
		}
		
		public static void SetInt64 (IntPtr handle, string symbol, long value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteInt64 (indirect, value);
		}

		public static void SetString (IntPtr handle, string symbol, string value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteIntPtr (indirect, value == null ? IntPtr.Zero : NSString.CreateNative (value));
		}
#if !(GENERATOR || COREBUILD)

		public static void SetString (IntPtr handle, string symbol, NSString value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			var strHandle = value == null ? IntPtr.Zero : value.Handle;
			if (strHandle != IntPtr.Zero)
				CFObject.CFRetain (strHandle);
			Marshal.WriteIntPtr (indirect, strHandle);
		}

		public static void SetArray (IntPtr handle, string symbol, NSArray array)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			var arrayHandle = array == null ? IntPtr.Zero : array.Handle;
			if (arrayHandle != IntPtr.Zero)
				CFObject.CFRetain (arrayHandle);
			Marshal.WriteIntPtr (indirect, arrayHandle);
		}
#endif

		public static IntPtr GetIntPtr (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return IntPtr.Zero;
			return Marshal.ReadIntPtr (indirect);
		}

		public static void SetIntPtr (IntPtr handle, string symbol, IntPtr value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			Marshal.WriteIntPtr (indirect, value);
		}

		#if SDCONVERT
		public static System.Drawing.SizeF GetSizeF (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return System.Drawing.SizeF.Empty;
			unsafe {
				float *ptr = (float *) indirect;
				return new System.Drawing.SizeF (ptr [0], ptr [1]);
			}
		}

		public static void SetSizeF (IntPtr handle, string symbol, System.Drawing.SizeF value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				float *ptr = (float *) indirect;
				ptr [0] = value.Width;
				ptr [1] = value.Height;
			}
		}
		#endif

		public static double GetDouble (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unsafe {
				double *d = (double *) indirect;

				return *d;
			}
		}

		public static void SetDouble (IntPtr handle, string symbol, double value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				*(double *) indirect = value;
			}
		}

		public static float GetFloat (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unsafe {
				float *d = (float *) indirect;

				return *d;
			}
		}

		public static void SetFloat (IntPtr handle, string symbol, float value)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return;
			unsafe {
				*(float *) indirect = value;
			}
		}
		
		internal static int SlowGetInt32 (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetInt32 (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static long SlowGetInt64 (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetInt64 (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static IntPtr SlowGetIntPtr (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return IntPtr.Zero;
			try {
				return GetIntPtr (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static double SlowGetDouble (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetDouble (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static NSString SlowGetStringConstant (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return null;
			try {
				return GetStringConstant (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}
	}

}
