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

//
// Used to preload the Foundation and AppKit libraries as our runtime
// requires this.   This will be replaced later with a dynamic system
//
using System;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public partial class NSObject {
		// Used to force the loading of AppKit and Foundation
		static IntPtr fl = Dlfcn.dlopen (Constants.FoundationLibrary, 1);
		static IntPtr al = Dlfcn.dlopen (Constants.AppKitLibrary, 1);
		static IntPtr ab = Dlfcn.dlopen (Constants.AddressBookLibrary, 1);
		static IntPtr ct = Dlfcn.dlopen (Constants.CoreTextLibrary, 1);
		static IntPtr wl = Dlfcn.dlopen (Constants.WebKitLibrary, 1);
		static IntPtr zl = Dlfcn.dlopen (Constants.QuartzLibrary, 1);
		static IntPtr ql = Dlfcn.dlopen (Constants.QTKitLibrary, 1);
		static IntPtr cl = Dlfcn.dlopen (Constants.CoreLocationLibrary, 1);
		static IntPtr ll = Dlfcn.dlopen (Constants.SecurityLibrary, 1);
		static IntPtr zc = Dlfcn.dlopen (Constants.QuartzComposerLibrary, 1);
		static IntPtr cw = Dlfcn.dlopen (Constants.CoreWlanLibrary, 1);
		static IntPtr pk = Dlfcn.dlopen (Constants.PdfKitLibrary, 1);
		static IntPtr ik = Dlfcn.dlopen (Constants.ImageKitLibrary, 1);
		static IntPtr sb = Dlfcn.dlopen (Constants.ScriptingBridgeLibrary, 1);
		static IntPtr av = Dlfcn.dlopen (Constants.AVFoundationLibrary, 1);
	}
}

