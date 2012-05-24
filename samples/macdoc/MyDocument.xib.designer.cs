// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace macdoc
{
	[Register ("MyDocument")]
	partial class MyDocument
	{
		[Outlet]
		MonoMac.AppKit.NSSegmentedCell navigationCells { get; set; }

		[Outlet]
		MonoMac.AppKit.NSOutlineView outlineView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView searchResults { get; set; }

		[Outlet]
		MonoMac.WebKit.WebView webView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTabView tabSelector { get; set; }

		[Outlet]
		MonoMac.AppKit.NSView spinnerView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator spinnerWidget { get; set; }

		[Outlet]
		MonoMac.AppKit.NSView indexSpinnerView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator indexSpinnerWidget { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton addBookmarkBtn { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton bookmarkSelector { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton viewBookmarksBtn { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSegmentedCell bookmarkToolbar { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSplitView splitView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView multipleMatchResults { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView indexResults { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSearchField indexSearchEntry { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSearchField toolbarSearchEntry { get; set; }

		[Outlet]
		MonoMac.AppKit.NSScrollView searchScrollView { get; set; }
		
		[Action ("IndexItemClicked:")]
		partial void IndexItemClicked (MonoMac.AppKit.NSTableView sender);

		[Action ("StartSearch:")]
		partial void StartSearch (MonoMac.AppKit.NSSearchField sender);

		[Action ("MultipleMatchItemClicked:")]
		partial void MultipleMatchItemClicked (MonoMac.AppKit.NSTableView sender);

		[Action ("SearchItemClicked:")]
		partial void SearchItemClicked (MonoMac.AppKit.NSTableView sender);
 
		[Action ("StartIndexSearch:")]
		partial void StartIndexSearch (MonoMac.AppKit.NSSearchField sender);

		[Action ("BookmarkToolbarClicked:")]
		partial void BookmarkToolbarClicked (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (navigationCells != null) {
				navigationCells.Dispose ();
				navigationCells = null;
			}

			if (outlineView != null) {
				outlineView.Dispose ();
				outlineView = null;
			}

			if (searchResults != null) {
				searchResults.Dispose ();
				searchResults = null;
			}

			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}

			if (tabSelector != null) {
				tabSelector.Dispose ();
				tabSelector = null;
			}

			if (spinnerView != null) {
				spinnerView.Dispose ();
				spinnerView = null;
			}

			if (spinnerWidget != null) {
				spinnerWidget.Dispose ();
				spinnerWidget = null;
			}

			if (indexSpinnerView != null) {
				indexSpinnerView.Dispose ();
				indexSpinnerView = null;
			}

			if (indexSpinnerWidget != null) {
				indexSpinnerWidget.Dispose ();
				indexSpinnerWidget = null;
			}

			if (addBookmarkBtn != null) {
				addBookmarkBtn.Dispose ();
				addBookmarkBtn = null;
			}

			if (bookmarkSelector != null) {
				bookmarkSelector.Dispose ();
				bookmarkSelector = null;
			}

			if (viewBookmarksBtn != null) {
				viewBookmarksBtn.Dispose ();
				viewBookmarksBtn = null;
			}

			if (bookmarkToolbar != null) {
				bookmarkToolbar.Dispose ();
				bookmarkToolbar = null;
			}

			if (splitView != null) {
				splitView.Dispose ();
				splitView = null;
			}

			if (multipleMatchResults != null) {
				multipleMatchResults.Dispose ();
				multipleMatchResults = null;
			}

			if (indexResults != null) {
				indexResults.Dispose ();
				indexResults = null;
			}

			if (indexSearchEntry != null) {
				indexSearchEntry.Dispose ();
				indexSearchEntry = null;
			}

			if (toolbarSearchEntry != null) {
				toolbarSearchEntry.Dispose ();
				toolbarSearchEntry = null;
			}

			if (searchScrollView != null) {
				searchScrollView.Dispose ();
				searchScrollView = null;
			}
		}
	}
}
