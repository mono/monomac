// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace macdoc
{
	[Register ("BookmarkPopover")]
	partial class BookmarkPopover
	{
		[Outlet]
		MonoMac.AppKit.NSButton doneButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton deleteButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSFormCell nameField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSFormCell notesField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (doneButton != null) {
				doneButton.Dispose ();
				doneButton = null;
			}

			if (deleteButton != null) {
				deleteButton.Dispose ();
				deleteButton = null;
			}

			if (nameField != null) {
				nameField.Dispose ();
				nameField = null;
			}

			if (notesField != null) {
				notesField.Dispose ();
				notesField = null;
			}
		}
	}

	[Register ("BookmarkPopoverController")]
	partial class BookmarkPopoverController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
