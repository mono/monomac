using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class BookmarkPopover : MonoMac.AppKit.NSView
	{
		public class BookmarkEventArgs : EventArgs
		{
			public string Name { get; set; }
			public string Notes { get; set; }
		}
		
		public event EventHandler<BookmarkEventArgs> Done;
		public event EventHandler Delete;
		
		public BookmarkPopover (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		[Export ("initWithCoder:")]
		public BookmarkPopover (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		public void PostInitialization ()
		{
			nameField.StringValue = Name ?? string.Empty;
			notesField.PlaceholderString = "You can add additional notes to this bookmark if you wish";
			doneButton.Activated += (sender, e) => Done (this, new BookmarkEventArgs () { Name = nameField.StringValue, Notes = notesField.StringValue });
			deleteButton.Activated += (sender, e) => Delete (this, EventArgs.Empty);
		}
		
		void Initialize ()
		{
		}
		
		public string Name {
			get;
			set;
		}
	}
}

