using MonoMac.AppKit;

namespace Preferences
{
	// Each tab on preferences window implements this interface
	interface IPreferencesTab
	{
		string Name { get; }
		NSImage Icon { get; }
		NSView View { get; }
	}
}

