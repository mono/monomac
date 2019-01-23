//
// Copyright 2010, Novell, Inc.
// Copyright 2013, Xamarin Inc.
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
	public class Selector : IEquatable<Selector>{
		public static readonly IntPtr Init = Selector.GetHandle ("init");
		public static readonly IntPtr InitWithCoder = Selector.GetHandle ("initWithCoder:");
		static IntPtr MethodSignatureForSelector = Selector.GetHandle ("methodSignatureForSelector:");
		static IntPtr FrameLength = Selector.GetHandle ("frameLength");
		internal static IntPtr RetainCount = Selector.GetHandle ("retainCount");
		internal const string Alloc = "alloc";
		internal const string Release = "release";
		internal const string Retain = "retain";
		internal const string Autorelease = "autorelease";
		internal const string DoesNotRecognizeSelector = "doesNotRecognizeSelector:";
		internal const string PerformSelectorOnMainThreadWithObjectWaitUntilDone = "performSelectorOnMainThread:withObject:waitUntilDone:";
		internal const string PerformSelectorWithObjectAfterDelay = "performSelector:withObject:afterDelay:";

		internal static IntPtr AllocHandle = Selector.GetHandle (Alloc);
		internal static IntPtr ReleaseHandle = Selector.GetHandle (Release);
		internal static IntPtr RetainHandle = Selector.GetHandle (Retain);
		internal static IntPtr AutoreleaseHandle = Selector.GetHandle (Autorelease);
		internal static IntPtr DoesNotRecognizeSelectorHandle = Selector.GetHandle (DoesNotRecognizeSelector);
		internal static IntPtr PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle = GetHandle (PerformSelectorOnMainThreadWithObjectWaitUntilDone);
		internal static IntPtr PerformSelectorWithObjectAfterDelayHandle = GetHandle (PerformSelectorWithObjectAfterDelay);

		internal IntPtr handle;

		public Selector (IntPtr sel) :
			this (sel, true)
		{
		}

		internal Selector (IntPtr sel, bool check)
		{
			if (check && !sel_isMapped (sel))
				throw new ArgumentException ("sel is not a selector handle.");

			this.handle = sel;
		}

		public Selector (string name, bool alloc) {
			handle = GetHandle (name);
		}

		public Selector (string name) : this (name, false) {}

		[MonoNativeFunctionWrapper]
		delegate int getFrameLengthDelegate (IntPtr @this, IntPtr sel);

		[MonoPInvokeCallbackAttribute(typeof(getFrameLengthDelegate))]
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
			return !(left == right);
		}

		public static bool operator== (Selector left, Selector right) {
			if (((object)left) == null)
				return (((object)right) == null);
			if (((object)right) == null)
				return false;

			return left.handle == right.handle;
		}

		public override bool Equals (object right) {
			return Equals (right as Selector);
		}

		public bool Equals (Selector right) {
			if (right == null)
				return false;

			return handle == right.handle;
		}

		public override int GetHashCode () {
			return (int) handle;
		}

		// return null, instead of throwing, if an invalid pointer is used (e.g. IntPtr.Zero)
		// so this looks better in the debugger watch when no selector is assigned (ref: #10876)
		public static Selector FromHandle (IntPtr sel)
		{
			if (!sel_isMapped (sel))
				return null;
			// create the selector without duplicating the sel_isMapped check
			return new Selector (sel, false);
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr sel_getName (IntPtr sel);
		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint="sel_registerName")]
		public extern static IntPtr GetHandle (string name);
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static bool sel_isMapped (IntPtr sel);
	}
}
