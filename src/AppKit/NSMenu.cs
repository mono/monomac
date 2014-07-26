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
		object __mt_items_var;
		
		NSMenuItem InsertItem (string title, string charCode, int index)
		{
			return InsertItem (title, null, charCode, index);
		}
		
	}
}
		