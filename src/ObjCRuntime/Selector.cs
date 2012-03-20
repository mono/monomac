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

namespace MonoMac.ObjCRuntime {
	[StructLayout(LayoutKind.Sequential)]
	public class Selector {
		public static IntPtr Init = Selector.GetHandle ("init");
		public static IntPtr InitWithCoder = Selector.GetHandle ("initWithCoder:");
		static IntPtr MethodSignatureForSelector = Selector.GetHandle ("methodSignatureForSelector:");
		static IntPtr FrameLength = Selector.GetHandle ("frameLength");
		internal static IntPtr Alloc = Selector.GetHandle ("alloc");

		internal IntPtr handle;

		public Selector (IntPtr sel) {
			if (!sel_isMapped (sel))
				throw new ArgumentException ("sel is not a selector handle.");

			this.handle = sel;
		}

		public Selector (string name, bool alloc) {
			if (alloc) {
				IntPtr selstr_ptr = Marshal.StringToHGlobalAuto (name);
				handle = sel_registerName (selstr_ptr);

				if (selstr_ptr != sel_getName (handle))
					Marshal.FreeHGlobal (selstr_ptr);
			} else {
				handle = sel_registerName (name);
			}
		}

		public Selector (string name) : this (name, false) {}

		public static int GetFrameLength (IntPtr @this, IntPtr sel)
		{
			IntPtr sig = Messaging.IntPtr_objc_msgSend_IntPtr (@this, MethodSignatureForSelector, sel);
			return Messaging.int_objc_msgSend (sig, FrameLength);
		}

		public IntPtr Handle {
			get { return handle; }
		}

		public string Name {
			get { return Marshal.PtrToStringAuto (sel_getName (handle)); }
		}

		public static Selector Register (IntPtr handle) {
			return new Selector (handle);
		}

		public static bool operator!= (Selector left, Selector right) {
			if (((object)left) == null)
				return (((object)right) != null);
			if (((object)right) == null)
				return true;

			return !sel_isEqual (left.handle, right.handle);
		}

		public static bool operator== (Selector left, Selector right) {
			if (((object)left) == null)
				return (((object)right) == null);
			if (((object)right) == null)
				return false;

			return sel_isEqual (left.handle, right.handle);
		}

		public override bool Equals (object right) {
			if (right == null)
				return false;

			if (right is Selector)
				return sel_isEqual (handle, ((Selector) right).handle);

			return false;
		}

		public override int GetHashCode () {
			return (int) handle;
		}
		
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr sel_getName (IntPtr sel);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr sel_registerName (IntPtr name);
		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr sel_registerName (string name);
		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint="sel_registerName")]
		public extern static IntPtr GetHandle (string name);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static bool sel_isMapped (IntPtr sel);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static bool sel_isEqual (IntPtr lhs, IntPtr rhs);
	}
}
