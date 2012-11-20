
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class AppleDocMergeWindow : MonoMac.AppKit.NSWindow
	{
		public AppleDocMergeWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		[Export ("initWithCoder:")]
		public AppleDocMergeWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
	}
}

