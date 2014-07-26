using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Preferences
{
	class PreferencesToolbarDelegate : NSToolbarDelegate
	{
		internal event EventHandler SelectionChanged;
		IEnumerable<IPreferencesTab> tabs;
		
		internal PreferencesToolbarDelegate(IEnumerable<IPreferencesTab> tabs)
		{
			this.tabs = tabs;
		}

		// Creates a toolbar item for each preferences tab. This method is called by Cocoa framework.
		// Notice that tab name is used as identifier. This identifier is used in PreferencesWindowController
		// to find a correct view to be displayed when toolbar item has been activated (clicked).
		public override NSToolbarItem WillInsertItem(NSToolbar toolbar, string itemIdentifier, bool willBeInserted)
		{
			var tab = tabs.Single(s => s.Name.Equals(itemIdentifier));
			var item = new NSToolbarItem(tab.Name) { Image = tab.Icon, Label = tab.Name};
			item.Activated += HandleActivated;
			return item;
		}
		
		void HandleActivated(object sender, EventArgs e)
		{
			if(SelectionChanged != null)
				SelectionChanged(sender, e);
		}
		
		public override string[] DefaultItemIdentifiers(NSToolbar toolbar)
		{
			return TabNames;
		}
		
		public override string[] AllowedItemIdentifiers(NSToolbar toolbar)
		{
			return TabNames;
		}
		
		public override string[] SelectableItemIdentifiers(NSToolbar toolbar)
		{
			return TabNames;
		}
		
		public override void WillAddItem(NSNotification notification)
		{
		}
		
		public override void DidRemoveItem(NSNotification notification)
		{
		}
		
		string[] TabNames { get { return tabs.Select(s => s.Name).ToArray(); }}
	}
}

