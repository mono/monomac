using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Preferences
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;
		PreferencesWindowController preferencesWindowController;

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);
		}

		// Action method binded in MainMenu.xib to "Preferences..." menu item.
		partial void ShowPreferencesWindow (NSObject sender)
		{
			if (preferencesWindowController == null)
				preferencesWindowController = new PreferencesWindowController ();
			preferencesWindowController.ShowWindow (this);
		}
	}
}

