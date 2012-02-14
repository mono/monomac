using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class BookmarkPopoverController : MonoMac.AppKit.NSViewController
	{
		NSPopover parentPopover;
		BookmarkManager.Entry entry;
		
		public BookmarkPopoverController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		[Export ("initWithCoder:")]
		public BookmarkPopoverController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		public BookmarkPopoverController (NSPopover popover, BookmarkManager.Entry entry) : base ("BookmarkPopover", NSBundle.MainBundle)
		{
			this.parentPopover = popover;
			this.entry = entry;
			Initialize ();
		}
		
		void Initialize ()
		{
			View.Delete += (sender, e) => { AppDelegate.BookmarkManager.DeleteBookmark (entry); parentPopover.PerformClose (this); };
			View.Done += (sender, e) => { entry.Name = e.Name; entry.Notes = e.Notes; parentPopover.PerformClose (this); };
			View.Name = entry.Name;
			View.PostInitialization ();
		}

		public new BookmarkPopover View {
			get {
				return (BookmarkPopover)base.View;
			}
		}
	}
}

