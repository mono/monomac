// 
// CFArray.cs: P/Invokes for CFArray
//
// Authors:
//    Mono Team
//    Rolf Bjarne Kvinge (rolf@xamarin.com)
//
//     
// Copyright 2010 Novell, Inc
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

using CFIndex = System.Int32;

namespace MonoMac.CoreFoundation {
	
	class CFArray : INativeObject, IDisposable {

		internal IntPtr handle;

		internal CFArray (IntPtr handle)
			: this (handle, false)
		{
		}

		[Preserve (Conditional = true)]
		internal CFArray (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");

			this.handle = handle;
			if (!owns)
				CFObject.CFRetain (handle);
		}
		
		public IntPtr Handle {
			get {return handle;}
		}

		[DllImport (Constants.CoreFoundationLibrary, EntryPoint="CFArrayGetTypeID")]
		public extern static int GetTypeID ();

		~CFArray ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		// pointer to a const struct (REALLY APPLE?)
		static readonly IntPtr kCFTypeArrayCallbacks_ptr;

		static CFArray ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreFoundationLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				kCFTypeArrayCallbacks_ptr = Dlfcn.GetIndirect (handle, "kCFTypeArrayCallBacks");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}

		public static CFArray FromIntPtrs (params IntPtr[] values)
		{
			return new CFArray (Create (values), true);
		}

		public static CFArray FromNativeObjects (params INativeObject[] values)
		{
			return new CFArray (Create (values), true);
		}

		public int Count {
			get {return CFArrayGetCount (handle);}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFArrayCreate (IntPtr allocator, IntPtr values, CFIndex numValues, IntPtr callbacks);

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFArrayGetValueAtIndex (IntPtr theArray, IntPtr index);

		public IntPtr GetValue (int index)
		{
			return CFArrayGetValueAtIndex (handle, new IntPtr (index));
		}

		public static unsafe IntPtr Create (params IntPtr[] values)
		{
			if (values == null)
				throw new ArgumentNullException ("values");
			fixed (IntPtr* pv = values) {
				return CFArrayCreate (IntPtr.Zero, 
						(IntPtr) pv,
						values.Length,
						kCFTypeArrayCallbacks_ptr);
			}
		}

		public static IntPtr Create (params INativeObject[] values)
		{
			if (values == null)
				throw new ArgumentNullException ("values");
			IntPtr[] _values = new IntPtr [values.Length];
			for (int i = 0; i < _values.Length; ++i)
				_values [i] = values [i].Handle;
			return Create (_values);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static CFIndex CFArrayGetCount (IntPtr theArray);
		public static int GetCount (IntPtr array)
		{
			return CFArrayGetCount (array);
		}
	}
}

