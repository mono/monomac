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
		MonoMac.AppKit.NSButton thebutton { get; set; }

		[Action ("IndexItemClicked:")]
		partial void IndexItemClicked (MonoMac.AppKit.NSTableView sender);

		[Action ("StartSearch:")]
		partial void StartSearch (MonoMac.AppKit.NSSearchField sender);
		
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
		}
	}
}
