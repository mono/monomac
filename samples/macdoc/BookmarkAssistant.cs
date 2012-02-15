using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class BookmarkAssistant : MonoMac.AppKit.NSView
	{
		public BookmarkAssistant (IntPtr handle) : base (handle)
		{
		}
		
		[Export ("initWithCoder:")]
		public BookmarkAssistant (NSCoder coder) : base (coder)
		{
		}
		
		public NSTableView TableView {
			get {
				return bookmarkTableView;
			}
		}
		
		public NSButtonCell DeleteButtonCell {
			get {
				return bookmarkTableDeleteBtn;
			}
		}
	}
}

