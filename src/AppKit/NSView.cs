//
// Support code for NSView
//

using System;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using System.Drawing;

namespace MonoMac.AppKit {
	public partial class NSView {
		object __mt_tracking_var;

#if MAC64
		[Export ("initWithFrame:")]
		public NSView (RectangleF frameRect)
			: this (new NSRect(frameRect))
		{
		}

#endif
	}

}
		
