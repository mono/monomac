// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.WebKit;

namespace macdoc
{
	[Register ("MyDocument")]
	partial class MyDocument
	{
		[Outlet]
		NSSegmentedCell navigationCells { get; set; }

		[Outlet]
		NSOutlineView outlineView { get; set; }

		[Outlet]
		NSTableView searchResults { get; set; }

		[Outlet]
		WebView webView { get; set; }

		[Outlet]
		NSTabView tabSelector { get; set; }

		[Outlet]
		NSView spinnerView { get; set; }

		[Outlet]
		NSProgressIndicator spinnerWidget { get; set; }

		[Outlet]
		NSButton addBookmarkBtn { get; set; }

		[Outlet]
		NSPopUpButton bookmarkSelector { get; set; }

		[Outlet]
		NSButton viewBookmarksBtn { get; set; }

		[Outlet]
		NSSplitView splitView { get; set; }

		[Outlet]
		NSTableView multipleMatchResults { get; set; }

		[Outlet]
		NSTableView indexResults { get; set; }

		[Outlet]
		NSSearchField indexSearchEntry { get; set; }

		[Action ("IndexItemClicked:")]
		partial void IndexItemClicked (NSTableView sender);

		[Action ("StartSearch:")]
		partial void StartSearch (NSSearchField sender);

		[Action ("MultipleMatchItemClicked:")]
		partial void MultipleMatchItemClicked (NSTableView sender);

		[Action ("SearchItemClicked:")]
		partial void SearchItemClicked (NSTableView sender);

		[Action ("StartIndexSearch:")]
		partial void StartIndexSearch (NSSearchField sender);
		
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
		}
	}
}
