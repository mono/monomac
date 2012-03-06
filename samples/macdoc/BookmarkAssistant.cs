using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class BookmarkAssistant : MonoMac.AppKit.NSView
	{
		public event Action<int> BookmarkDeleted;
		
		public BookmarkAssistant (IntPtr handle) : base (handle)
		{
		}
		
		[Export ("initWithCoder:")]
		public BookmarkAssistant (NSCoder coder) : base (coder)
		{
		}
		
		partial void DeleteButtonClicked (NSButton sender)
		{
			if (bookmarkTableView.SelectedRowCount != 1)
				return;
			
			var index = bookmarkTableView.SelectedRow;
			if (index < 0 || index > bookmarkTableView.RowCount)
				return;
			var temp = BookmarkDeleted;
			if (temp != null)
				temp (index);
		}
		
		public NSTableView TableView {
			get {
				return bookmarkTableView;
			}
		}
	}
}

