using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Monodoc;

namespace macdoc
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		static public RootTree Root;
		
		public AppDelegate ()
		{
			Root = RootTree.LoadTree (null);
		}

		public override void FinishedLaunching (NSObject notification)
		{
		}
	}
}

