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
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac.Foundation;
using MonoMac.CoreGraphics;

namespace MonoMac.ObjCRuntime {
	public static partial class Messaging {
		const string LIBOBJC_DYLIB = "/usr/lib/libobjc.dylib";

		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static uint uint_objc_msgSend (IntPtr receiver, IntPtr selector);
		
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static int int_objc_msgSend (IntPtr receiver, IntPtr selector);

		/* void returns */
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend (IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_intptr (IntPtr receiver, IntPtr selector, IntPtr arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_bool (IntPtr receiver, IntPtr selector, bool arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_rbool (IntPtr receiver, IntPtr selector, ref bool arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_rint (IntPtr receiver, IntPtr selector, ref int arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_rfloat (IntPtr receiver, IntPtr selector, ref float arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_rdouble (IntPtr receiver, IntPtr selector, ref double arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_rintptr (IntPtr receiver, IntPtr selector, ref IntPtr arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_cgsize (IntPtr receiver, IntPtr selector, SizeF arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_cgpoint (IntPtr receiver, IntPtr selector, PointF arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_cgrect (IntPtr receiver, IntPtr selector, RectangleF arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_nsrange (IntPtr receiver, IntPtr selector, NSRange arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_intptr_int (IntPtr receiver, IntPtr selector, IntPtr arg1, int arg2);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_cgpoint_intptr (IntPtr receiver, IntPtr selector, PointF arg1, IntPtr arg2);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_intptr_intptr_bool (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, bool arg3);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_intptr_intptr_float (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, float arg3);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static void void_objc_msgSend_intptr_intptr_double (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, double arg3);

		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper (IntPtr [] super, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper (IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper_intptr (IntPtr receiver, IntPtr selector, IntPtr arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper_cgsize (IntPtr receiver, IntPtr selector, SizeF arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper_cgrect (IntPtr receiver, IntPtr selector, RectangleF arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper_intptr_intptr_bool (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, bool arg3);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper_intptr_intptr_float (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, float arg3);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static void void_objc_msgSendSuper_intptr_intptr_double (IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2, double arg3);

		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend_stret")]
		public extern static void void_objc_msgSend_stret_rcgsize (ref SizeF stret, IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend_stret")]
		public extern static void void_objc_msgSend_stret_rcgrect (ref RectangleF stret, IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend_stret")]
		public extern static void void_objc_msgSend_stret_rnsrange (ref NSRange stret, IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend_stret")]
		public extern static void void_objc_msgSend_stret_rcgsize_cgpoint_intptr (ref SizeF stret, IntPtr receiver, IntPtr selector, PointF arg1, IntPtr arg2);

		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper_stret")]
		public extern static void void_objc_msgSendSuper_stret_rcgrect (ref RectangleF stret, IntPtr receiver, IntPtr selector);

		/* intptr returns */
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static IntPtr IntPtr_objc_msgSend_IntPtr (IntPtr receiver, IntPtr selector, IntPtr arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static IntPtr intptr_objc_msgSend (IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static IntPtr intptr_objc_msgSend_intptr (IntPtr receiver, IntPtr selector, IntPtr arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]		
		public extern static IntPtr intptr_objc_msgsend_intptr_int (IntPtr receiver, IntPtr selector, IntPtr arg1, int arg2);
		
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static IntPtr intptr_objc_msgSend_float (IntPtr receiver, IntPtr selector, float arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static IntPtr intptr_objc_msgSend_cgrect (IntPtr receiver, IntPtr selector, RectangleF arg1);
		
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static IntPtr intptr_objc_msgSendSuper (IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static IntPtr intptr_objc_msgSendSuper_cgrect (IntPtr receiver, IntPtr selector, RectangleF arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static IntPtr intptr_objc_msgSendSuper_intptr (IntPtr receiver, IntPtr selector, IntPtr arg1);

		/* bool returns */
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static bool bool_objc_msgSend (IntPtr receiver, IntPtr selector);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static bool bool_objc_msgSend_intptr (IntPtr receiver, IntPtr selector, IntPtr arg1);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static bool bool_objc_msgSendSuper_intptr (IntPtr receiver, IntPtr selector, IntPtr arg1);

		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static SizeF cgsize_objc_msgSend_cgpoint_intptr (IntPtr receiver, IntPtr selector, PointF arg1, IntPtr arg2);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static SizeF cgsize_objc_msgSend (IntPtr receiver, IntPtr selector);

		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSend")]
		public extern static Boolean Boolean_objc_msgSend_IntPtr_Double_IntPtr (IntPtr receiver, IntPtr selector, IntPtr arg1, Double arg2, IntPtr arg3);
		[DllImport (LIBOBJC_DYLIB, EntryPoint="objc_msgSendSuper")]
		public extern static Boolean Boolean_objc_msgSendSuper_IntPtr_Double_IntPtr (IntPtr receiver, IntPtr selector, IntPtr arg1, Double arg2, IntPtr arg3);
	}
}
