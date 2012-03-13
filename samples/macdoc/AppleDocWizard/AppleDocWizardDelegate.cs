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
			wizard.Window.Center ();
			NSApplication.SharedApplication.ArrangeInFront (this);
			NSApplication.SharedApplication.RunModalForWindow (wizard.Window);
		}
	}
}
