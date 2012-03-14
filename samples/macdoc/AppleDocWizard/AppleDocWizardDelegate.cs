using System;
using MonoMac.AppKit;

namespace macdoc
{
	public class AppleDocWizardDelegate : NSApplicationDelegate
	{
		AppleDocWizardController wizard;
		
		public override bool ApplicationShouldOpenUntitledFile (NSApplication sender)
		{
			return false;
		}
		
		public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
		{
			wizard = new AppleDocWizardController ();
			NSApplication.SharedApplication.ActivateIgnoringOtherApps (true);
			wizard.Window.MakeMainWindow ();
			wizard.Window.MakeKeyWindow ();
			wizard.Window.MakeKeyAndOrderFront (this);
			wizard.Window.Center ();
			wizard.VerifyFreshnessAndLaunchDocProcess ();
			NSApplication.SharedApplication.RunModalForWindow (wizard.Window);
		}
	}
}
