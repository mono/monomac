// 
// CGPDFDictionary.cs: Implements the managed CGPDFDictionary binding
//
// Authors:
//	Miguel de Icaza <miguel@xamarin.com>
//	Sebastien Pouliot <sebastien@xamarin.com>
// 
// Copyright 2010 Novell, Inc
// Copyright 2011 Xamarin Inc. All rights reserved
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
using MonoMac.CoreFoundation;

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif

namespace MonoMac.CoreGraphics {
	public class CGPDFDictionary : INativeObject {
		internal IntPtr handle;

		public IntPtr Handle {
			get { return handle; }
		}

		/* invoked by marshallers */
		public CGPDFDictionary (IntPtr handle)
		{
			this.handle = handle;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDictionaryGetCount (IntPtr handle);
		
		public int Count {
			get {
				return (int) CGPDFDictionaryGetCount (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetBoolean (IntPtr handle, string key, out bool result);

		public bool GetBoolean (string key, out bool result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return CGPDFDictionaryGetBoolean (handle, key, out result);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetInteger (IntPtr handle, string key, out int result);

		public bool GetInt (string key, out int result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return CGPDFDictionaryGetInteger (handle, key, out result);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetNumber (IntPtr handle, string key, out nfloat result);

		public bool GetFloat (string key, out nfloat result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return CGPDFDictionaryGetNumber (handle, key, out result);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetName (IntPtr handle, string key, out IntPtr result);

		public bool GetName (string key, out string result)
		{
			IntPtr res;
			
			if (key == null)
				throw new ArgumentNullException ("key");
			var r = CGPDFDictionaryGetName (handle, key, out res);
			if (!r){
				result = null;
				return false;
			}
			result = Marshal.PtrToStringAnsi (res);
			return true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetDictionary (IntPtr handle, string key, out IntPtr result);

		public bool GetDictionary (string key, out CGPDFDictionary result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			IntPtr res;
			var r = CGPDFDictionaryGetDictionary (handle, key, out res);
			if (!r){
				result = null;
				return false;
			}
			result = new CGPDFDictionary (res);
			return true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetStream (IntPtr handle, string key, out IntPtr result);

		public bool GetStream (string key, out CGPDFStream result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			IntPtr ptr;
			var r = CGPDFDictionaryGetStream (handle, key, out ptr); 
			if (!r){
				result = null;
				return false;
			}
			result = new CGPDFStream (ptr);
			return true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetArray (IntPtr handle, string key, out IntPtr result);

		public bool GetArray (string key, out CGPDFArray array)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			IntPtr ptr;
			var r = CGPDFDictionaryGetArray (handle, key, out ptr);
			if (!r){
				array = null;
				return false;
			}
			array = new CGPDFArray (ptr);
			return true;
		}

		delegate void ApplierFunction (string key, IntPtr pdfObject, IntPtr info);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFDictionaryApplyFunction (IntPtr dictRef, ApplierFunction function, IntPtr info);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPDFObjectGetType (IntPtr pdfobj);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFObjectGetValue (IntPtr pdfobj, int type, out byte bvar);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFObjectGetValue (IntPtr pdfobj, int type, out int ivar);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFObjectGetValue (IntPtr pdfobj, int type, out float rvar);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFObjectGetValue (IntPtr pdfobj, int type, out IntPtr ptrvar);
		
		static object MapFromCGPdfObject (IntPtr pdfObj)
		{
			IntPtr ip;
			
			switch (CGPDFObjectGetType (pdfObj)){
			case 1: // null
				return null;

			case 2: // boolean
				byte b;
				if (CGPDFObjectGetValue (pdfObj, 2, out b))
					return b != 0;
				return null;
				
			case 3: // int
				int i;
				if (CGPDFObjectGetValue (pdfObj, 3, out i))
					return i;
				return null;
				
			case 4: // real
				float f;
				if (CGPDFObjectGetValue (pdfObj, 4, out f))
					return f;
				return null;
				
			case 5: // name
				if (CGPDFObjectGetValue (pdfObj, 5, out ip))
					return Marshal.PtrToStringAnsi (ip);
				return null;
				
			case 6: // string
				if (CGPDFObjectGetValue (pdfObj, 6, out ip))
					return CGPDFString.ToString (ip);
				return null;
				
			case 7: // array
				if (CGPDFObjectGetValue (pdfObj, 7, out ip))
					return new CGPDFArray (ip);
				return null;
			case 8: // dictionary
				if (CGPDFObjectGetValue (pdfObj, 8, out ip))
					return new CGPDFDictionary (ip);
				return null;
			case 9: // stream
				if (CGPDFObjectGetValue (pdfObj, 9, out ip))
					return new CGPDFStream (ip);
				return null;
			}
			return null;
		}
		
#if !MONOMAC
		[MonoPInvokeCallback (typeof (ApplierFunction))]
#endif
		static void ApplyBridge (string key, IntPtr pdfObject, IntPtr info)
		{
			Action<string,object> callback = (Action<string,object>) GCHandle.FromIntPtr (info).Target;

			callback (key, MapFromCGPdfObject (pdfObject));
		}
	
		public void Apply (Action<string,object> callback)
		{
			GCHandle gch = GCHandle.Alloc (callback);
			CGPDFDictionaryApplyFunction (Handle, ApplyBridge, GCHandle.ToIntPtr (gch));
			gch.Free ();
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetString (IntPtr handle, string key, out IntPtr result);

		public bool GetString (string key, out string result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			IntPtr res;
			if (CGPDFDictionaryGetString (handle, key, out res)){
				result = CGPDFString.ToString (res);
				return true;
			}
			result = null;
			return false;
		}
	}
}
