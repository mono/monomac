// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace macdoc
{
	[Register ("BookmarkAssistant")]
	partial class BookmarkAssistant
	{
		[Outlet]
		MonoMac.AppKit.NSTableView bookmarkTableView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton deleteButton { get; set; }

		[Action ("DeleteButtonClicked:")]
		partial void DeleteButtonClicked (MonoMac.AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bookmarkTableView != null) {
				bookmarkTableView.Dispose ();
				bookmarkTableView = null;
			}

			if (deleteButton != null) {
				deleteButton.Dispose ();
				deleteButton = null;
			}
		}
	}

	[Register ("BookmarkAssistantController")]
	partial class BookmarkAssistantController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
