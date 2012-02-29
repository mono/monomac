using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace macdoc
{
	class MainClass
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();
			NSApplication.SharedApplication.Delegate = new AppleDocWizardDelegate ();
			NSApplication.Main (args);
		}
	}
}

