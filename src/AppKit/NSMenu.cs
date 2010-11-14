using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;
using MonoMac.CoreAnimation;

namespace MonoMac.AppKit {
	public partial class NSMenu {
		NSMenuItem InsertItem (string title, string charCode, int index)
		{
			return InsertItem (title, null, charCode, index);
		}
		
	}
}
		