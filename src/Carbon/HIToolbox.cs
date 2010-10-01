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

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;

namespace MonoMac.Carbon {
	
	public enum SystemUIMode : uint
	{
		Normal                 = 0,
		ContentSuppressed      = 1,
		ContentHidden          = 2,
		AllSuppressed          = 4,
		AllHidden              = 3
	}
	
	public enum SystemUIOptions : uint
	{
		AutoShowMenuBar      = 1 << 0,
		DisableAppleMenu     = 1 << 2,
		DisableProcessSwitch = 1 << 3,
		DisableForceQuit     = 1 << 4,
		DisableSessionTerminate = 1 << 5,
		DisableHide          = 1 << 6,
		DisableMenuBarTransparency = 1 << 7
	}

	public class HIToolbox
	{		
		[DllImport (Constants.HIToolboxLibrary)]
		extern public static int SetSystemUIMode(SystemUIMode inMode, SystemUIOptions inOptions);
		
		[DllImport (Constants.HIToolboxLibrary)]
		extern public static void GetSystemUIMode(out SystemUIMode mode, out SystemUIOptions options);
	}
}
