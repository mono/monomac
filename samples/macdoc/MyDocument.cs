
using System;
using System.Collections.Generic;
using System.Linq;
using Monodoc;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.WebKit;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace macdoc
{
	public partial class MyDocument : MonoMac.AppKit.NSDocument
	{
		internal Dictionary<Node,WrapNode> nodeToWrapper = new Dictionary<Node, WrapNode> ();
		History history;
		bool ignoreSelect;
		// This is used if the user click on different urls while some are still loading so that only the most recent content is displayed
		long loadUrlTimestamp = long.MinValue;
		
		string initialLoadFromUrl;
		Node match;
		string currentUrl;
		string currentTitle;
		
		SearchableIndex searchIndex;
		IndexSearcher mdocSearch;
		IndexEntry current_entry;
		
		// Called when created from unmanaged code
		public MyDocument (IntPtr handle) : base(handle)
		{
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MyDocument (NSCoder coder) : base(coder)
		{
		}
		
		public override string DisplayName {
			get {
				return "Mono Documentation Browser";
			}
		}

		public void LoadWithSearch (string searchTerm)
		{
			if (!string.IsNullOrEmpty (searchTerm)) {
				toolbarSearchEntry.StringValue = searchTerm;
				tabSelector.SelectAt (2);
				Search (searchTerm);
				Logger.Log ("Searched: '{0}'", searchTerm);
			}
		}
		
		public override bool ReadFromUrl (NSUrl url, string typeName, out NSError outError)
		{
			Logger.Log ("ReadFromUrl: {0}", url.ToString ());
			outError = null;

			// if scheme is not right, we ignore the url
			if (url.Scheme != "monodoc" && url.Scheme != "mdoc")
				return true;
			
			// ResourceSpecifier is e.g. "//T:System.String"
			initialLoadFromUrl = Uri.UnescapeDataString (url.ResourceSpecifier.Substring (2));
			this.FileUrl = url;
			
			return true;
		}
		
		public override void WindowControllerDidLoadNib (NSWindowController windowController)
		{
			base.WindowControllerDidLoadNib (windowController);
			tabSelector.SelectFirst (this);
			history = new History (navigationCells);
			SetupOutline ();
			SetupSearch ();
			SetupBookmarks ();
			webView.DecidePolicyForNavigation += HandleWebViewDecidePolicyForNavigation;
			webView.FinishedLoad += HandleWebViewFinishedLoad;
			((WebViewExtraordinaire)webView).SwipeEvent += (sender, e) => {
				if (e.Side == SwipeSide.Left && history.BackClicked ())
					navigationCells.SetSelected (true, 0);
				else if (history.ForwardClicked ())
					navigationCells.SetSelected (true, 1);
			};
			HideMultipleMatches ();
			LoadUrl (string.IsNullOrEmpty (initialLoadFromUrl) ? "root:" : initialLoadFromUrl);
		}

		void HandleAddBookmarkBtnActivated (object sender, EventArgs e)
		{
			var title = string.IsNullOrWhiteSpace (currentTitle) ? "No Title" : currentTitle;
			
			var entry = new BookmarkManager.Entry () { Name = title, Url = currentUrl, Notes = string.Empty };
			AppDelegate.BookmarkManager.AddBookmark (entry);
			var popover = new NSPopover ();
			popover.Behavior = NSPopoverBehavior.Transient;
			popover.ContentViewController = new BookmarkPopoverController (popover, entry);
			popover.Show (new RectangleF (0, 0, 0, 0), (NSView)sender, NSRectEdge.MinYEdge);
		}
		
		void HandleRemoveBookmarkBtnActivated (object sender, EventArgs e)
		{
			var selected = bookmarkSelector.IndexOfSelectedItem;
			if (selected < 0 || selected >= bookmarkSelector.ItemCount)
				return;
			var bk = AppDelegate.BookmarkManager.GetAllBookmarks ()
				.Where (b => b.Name.Equals (bookmarkSelector.TitleOfSelectedItem, StringComparison.InvariantCultureIgnoreCase))
				.FirstOrDefault ();
			if (bk == null)
				return;
			AppDelegate.BookmarkManager.DeleteBookmark (bk);
		}
		
		void SetupOutline ()
		{
			outlineView.DataSource = new DocTreeDataSource (this);
			outlineView.Delegate = new OutlineDelegate (this);
			if (AppDelegate.IsOnLionOrBetter)
				outlineView.EnclosingScrollView.HorizontalScrollElasticity = outlineView.EnclosingScrollView.VerticalScrollElasticity = NSScrollElasticity.None;
		}
		
		void SetupSearch ()
		{
			AppDelegate.IndexUpdateManager.UpdaterChange += ToggleSearchCreationStatus;
			searchIndex = AppDelegate.Root.GetSearchIndex ();
			mdocSearch = new IndexSearcher (AppDelegate.IndexUpdateManager.IsFresh ? AppDelegate.Root.GetIndex () : null);
			indexResults.Source = new IndexDataSource (mdocSearch);
			multipleMatchResults.Source = new MultipleMatchDataSource (this);
			searchResults.Source = new ResultDataSource ();
			splitView.Delegate = new SplitViewDelegate ();
			tabSelector.DidSelect += (sender, e) => {
				if (e.Item.TabView.IndexOf (e.Item) == 2)
					WindowForSheet.MakeFirstResponder (toolbarSearchEntry);
			};
		}
		
		class SplitViewDelegate : NSSplitViewDelegate
		{
			public override float ConstrainSplitPosition (NSSplitView splitView, float proposedPosition, int subviewDividerIndex)
			{
				if (subviewDividerIndex != 0)
					return proposedPosition;
				var middle = (splitView.MaxPositionOfDivider (0) - splitView.MinPositionOfDivider (0)) / 2;
				return proposedPosition < middle ? middle : proposedPosition;
			}
		}
		
		void SetupBookmarks ()
		{
			if (!AppDelegate.IsOnLionOrBetter){
				addBookmarkBtn.Hidden = true;
				
				viewBookmarksBtn.Hidden = true;
				bookmarkSelector.Hidden = true;
				return;
			}
			var manager = AppDelegate.BookmarkManager;
			manager.BookmarkListChanged += (sender, e) => {
				switch (e.EventType) {
				case BookmarkEventType.Modified:
					int index = manager.GetAllBookmarks ().IndexOf (e.Entry);
					var item = bookmarkSelector.ItemAtIndex (index);
					if (item != null)
						item.Title = e.Entry.Name;
					break;
				case BookmarkEventType.Deleted:
					bookmarkSelector.RemoveItem (e.Entry.Name);
					break;
				case BookmarkEventType.Added:
					bookmarkSelector.AddItem (e.Entry.Name);
					bookmarkSelector.SelectItem (e.Entry.Name);
					break;
				}
			};
			bookmarkSelector.AddItems (manager.GetAllBookmarks ().Select (i => i.Name).ToArray ());
			bookmarkSelector.Activated += (sender, e) => {
				var bmarks = manager.GetAllBookmarks ();
				var index = bookmarkSelector.IndexOfSelectedItem;
				if (index >= 0 && index < bmarks.Count)
					LoadUrl (bmarks[index].Url, true);
			};
			bookmarkSelector.SelectItem (-1);
		}

		void HandleViewBookmarksBtnActivated (object sender, EventArgs e)
		{
			var popover = new NSPopover ();
			popover.Behavior = NSPopoverBehavior.Transient;
			popover.ContentViewController = new BookmarkAssistantController (AppDelegate.BookmarkManager.GetAllBookmarks ());
			popover.Show (new RectangleF (0, 0, 0, 0), (NSView)sender, NSRectEdge.MinYEdge);
		}
		
		void ToggleSearchCreationStatus (object sender, EventArgs e)
		{
			var manager = (IndexUpdateManager)sender;

			if (!manager.IsCreatingSearchIndex) {
				InvokeOnMainThread (delegate {
					var indexSpinnerHeight = indexSpinnerView.Frame.Height * 3 /4;
					var searchSpinnerHeight = spinnerView.Frame.Height * 3 / 4;
					
					spinnerWidget.StopAnimation (this);
					spinnerView.Hidden = true;
					indexSpinnerWidget.StopAnimation (this);
					indexSpinnerView.Hidden = true;

					searchIndex = AppDelegate.Root.GetSearchIndex ();
					indexSearchEntry.Enabled = true;
					mdocSearch.Index = AppDelegate.Root.GetIndex ();
					indexResults.ReloadData ();
					
					var splitViewFrame = splitView.Frame;
					splitView.Frame = new RectangleF (splitViewFrame.X,
					                                  splitViewFrame.Y - indexSpinnerHeight,
					                                  splitViewFrame.Width,
					                                  splitViewFrame.Height + indexSpinnerHeight);
					
					var searchScrollViewFrame = searchScrollView.Frame;
					searchScrollView.Frame = new RectangleF (searchScrollViewFrame.X,
					                                         searchScrollViewFrame.Y - searchSpinnerHeight,
					                                         searchScrollViewFrame.Width,
					                                         searchScrollViewFrame.Height + searchSpinnerHeight);
				});
			} else {
				InvokeOnMainThread (delegate {
					spinnerView.Hidden = false;
					spinnerWidget.StartAnimation (this);
					indexSpinnerView.Hidden = false;
					indexSpinnerWidget.StartAnimation (this);
					indexSearchEntry.Enabled = false;
				});
			}
		}
		
		internal void LoadUrl (string url, bool syncTreeView = false, HelpSource source = null, bool addToHistory = true)
		{
			if (url.StartsWith ("#"))
				return;
			// In case user click on an external link e.g. [Android documentation] link at bottom of MonoDroid docs
			if (url.StartsWith ("http://")) {
				UrlLauncher.Launch (url);
				return;
			}
			Logger.Log ("Loading {0}", url);
			var ts = Interlocked.Increment (ref loadUrlTimestamp);
			Task.Factory.StartNew (() => {
				Node node;
				var res = DocTools.GetHtml (url, source, out node);
				return new { Node = node, Html = res };
			}).ContinueWith (t => {
				var node = t.Result.Node;
				var res = t.Result.Html;
				if (res != null){
					BeginInvokeOnMainThread (() => {
						if (ts < loadUrlTimestamp)
							return;
						currentUrl = node == null ? url : node.PublicUrl;
						if (AppDelegate.IsOnLionOrBetter)
							InvalidateRestorableState ();
						if (addToHistory)
							history.AppendHistory (new LinkPageVisit (this, currentUrl));
						LoadHtml (res);
						this.match = node;
						if (syncTreeView) {
							// When navigation occurs after a link on search result is clicked
							// we need to show the panel so that ShowNode work as expected
							tabSelector.SelectAt (0);
						}
						// Bookmark spinner management
						var bookmarkIndex = AppDelegate.BookmarkManager.FindIndexOfBookmarkFromUrl (url);
						if (bookmarkIndex == -1 || bookmarkIndex < bookmarkSelector.ItemCount)
							bookmarkSelector.SelectItem (bookmarkIndex);
					});
				}
			});
		}
		
		void Search (string text)
		{
			// We may have a null search index if it's the first time the app is launched
			// In that case try to grab a snapshot and if still nothing exits cleanly
			if (searchIndex == null) {
				searchIndex = AppDelegate.Root.GetSearchIndex ();
				if (searchIndex == null)
					return;
			}
			var dataSource = ((ResultDataSource)searchResults.Source);
			dataSource.LatestSearchTerm = text;
			Result results = searchIndex.FastSearch (text, 5);
			dataSource.ClearResultSet ();
			dataSource.AddResultSet (results);
			// No SynchronizationContext for MonoMac yet
			Task.Factory.StartNew (() => searchIndex.Search (text, 20)).ContinueWith (t => InvokeOnMainThread (() => {
				var rs = t.Result;
				if (rs == null || rs.Count == 0 || text != dataSource.LatestSearchTerm)
					return;
				dataSource.AddResultSet (rs);
				searchResults.ReloadData ();
			}));
			searchResults.ReloadData ();
			if (results.Count > 0) {
				searchResults.SelectRow (1, false);
				searchResults.ScrollRowToVisible (0);
				OnSearchRowSelected (1);
			}
		}
		
		void IndexSearch (string text)
		{
			int targetRow = mdocSearch.FindClosest (text);
			indexResults.SelectRow (targetRow, false);

			OnIndexRowSelected (targetRow);
			indexResults.ScrollRowToVisible (targetRow);
		}
		
		void OnSearchRowSelected (int targetRow)
		{
			var url = ((ResultDataSource)searchResults.Source).GetResultUrl (targetRow);
			if (url == null)
				return;
			LoadUrl (url);
		}
		
		void OnIndexRowSelected (int targetRow)
		{
			current_entry = mdocSearch.GetIndexEntry (targetRow);
			if (current_entry == null)
				return;
			multipleMatchResults.ReloadData ();
			if (current_entry.Count > 1){
				multipleMatchResults.SelectRow (0, false);
				multipleMatchResults.ScrollRowToVisible (0);
				ShowMultipleMatches ();
			} else {
				HideMultipleMatches ();
				LoadUrl (current_entry [0].Url);
			}
		}
		
		void HideMultipleMatches ()
		{
			splitView.SetPositionOfDivider (splitView.MaxPositionOfDivider (0), 0);
		}
		
		void ShowMultipleMatches ()
		{
			float middle = (splitView.MaxPositionOfDivider (0) - splitView.MinPositionOfDivider (0))/2;
			splitView.SetPositionOfDivider (middle, 0);
		}
		
		// Action: when the user clicks on the index table view
		partial void IndexItemClicked (NSTableView sender)
		{
			OnIndexRowSelected (sender.ClickedRow);
		}
		
		// Action: when the user clicks on the index table view
		partial void SearchItemClicked (NSTableView sender)
		{
			OnSearchRowSelected (sender.ClickedRow);
		}
		
		// Action: when the user clicks on the multiple matches table view
		partial void MultipleMatchItemClicked (NSTableView sender)
		{
			string url = null;
			try {
				url = current_entry [sender.ClickedRow].Url;
			} catch {
				return;
			}
			LoadUrl (url);
		}
		
		// Action: when the user starts typing on the toolbar search bar	
		partial void StartSearch (NSSearchField sender)
		{
			var contents = sender.StringValue;
			if (string.IsNullOrEmpty (contents))
				return;
			tabSelector.SelectAt (2);
			Search (contents);
			// Unselect the search term in case user is typing slowly
			if (sender.CurrentEditor != null)
				sender.CurrentEditor.SelectedRange = new NSRange (contents.Length, 0);
		}
		
		// Typing in the index panel
		partial void StartIndexSearch (NSSearchField sender)
		{
			var contents = sender.StringValue;
			if (contents == null || contents == "")
				return;
			tabSelector.SelectAt (1);
			IndexSearch (contents);
		}
		
		partial void BookmarkToolbarClicked (NSObject sender)
		{
			switch (bookmarkToolbar.SelectedSegment) {
			case 0: // Add button
				HandleAddBookmarkBtnActivated (bookmarkToolbar.ControlView, EventArgs.Empty);
				break;
			case 1: // Remove button
				HandleRemoveBookmarkBtnActivated (bookmarkToolbar.ControlView, EventArgs.Empty);
				break;
			case 2: // Settings button
				HandleViewBookmarksBtnActivated (bookmarkToolbar.ControlView, EventArgs.Empty);
				break;
			}
		}

		void HandleWebViewDecidePolicyForNavigation (object sender, WebNavigationPolicyEventArgs e)
		{
			if (LoadingFromString){
				WebView.DecideUse (e.DecisionToken);
				return;
			}
			
			var mainUrl = e.Frame.WebView.MainFrameUrl;
			var url = e.Request.Url.AbsoluteString;
			
			// Let WebKit take care of the anchors.
			if (mainUrl != null && url.StartsWith (mainUrl) && url.Length > mainUrl.Length && url [mainUrl.Length] == '#'){
				WebView.DecideUse (e.DecisionToken);
				return;
			}
			
			WebView.DecideIgnore (e.DecisionToken);
			LoadUrl (url, true);
		}
		
		void HandleWebViewFinishedLoad (object sender, WebFrameEventArgs e)
		{
			if (match != null) {
				ShowNode (match);
				match = null;
			}
			if (navigationCells.SelectedSegment != -1)
				navigationCells.SetSelected (false, navigationCells.SelectedSegment);
			var dom = e.ForFrame.DomDocument;
			
			// Update the title of the current page
			var elements = dom.GetElementsByTagName ("title");
			if (elements.Count > 0 && !string.IsNullOrWhiteSpace (elements[0].TextContent))
				currentTitle = elements[0].TextContent.Length > 2 ? elements[0].TextContent.Substring (2) : elements[0].TextContent;
			
			// Process embedded images coming from doc source
			// Because WebView doesn't let me answer a NSUrlRequest myself I have to resort to this piece of crap of a solution
			var imgs = dom.GetElementsByTagName ("img").Where (node => node.Attributes["src"].NodeValue.StartsWith ("source-id"));
			byte[] buffer = new byte[4096];
			
			foreach (var img in imgs) {
				var src = img.Attributes["src"].NodeValue;
				var imgStream = AppDelegate.Root.GetImage (src);
				if (imgStream == null)
					continue;
				var length = imgStream.Read (buffer, 0, buffer.Length);
				var read = length;
				while (read != 0) {
					if (length == buffer.Length) {
						var oldBuffer = buffer;
						buffer = new byte[oldBuffer.Length * 2];
						Buffer.BlockCopy (oldBuffer, 0, buffer, 0, oldBuffer.Length);
					}
					length += read = imgStream.Read (buffer, length, buffer.Length - length);
				}
				
				var data = Convert.ToBase64String (buffer, 0, length, Base64FormattingOptions.None);
				var uri = "data:image/" + src.Substring (src.LastIndexOf ('.')) + ";base64," + data;
				((DomElement)img).SetAttribute ("src", uri);
			}
		}
		
		bool LoadingFromString;
		void LoadHtml (string html)
		{
			LoadingFromString = true;
			webView.MainFrame.LoadHtmlString (html, AppDelegate.MonodocBaseUrl);
			LoadingFromString = false;
		}
		
		internal void ShowNode (Node n)
		{
			if (n == null)
				return;
			
			if (!nodeToWrapper.ContainsKey (n))
				ShowNode (n.Parent);
			// If the dictionary still doesn't contain anything about us, time to leave
			if (!nodeToWrapper.ContainsKey (n))
				return;
			
			var item = nodeToWrapper [n];
			outlineView.ExpandItem (item);
			
			// Focus the last child, then this child to ensure we show as much as possible
			if (n.ChildNodes.Count > 0)
				ScrollToVisible ((Node) n.ChildNodes [n.ChildNodes.Count-1]);
			var row = ScrollToVisible (n);
			ignoreSelect = true;
			if (row > 0)
				outlineView.SelectRows (new NSIndexSet (row), false);
			ignoreSelect = false;
		}
		
		int ScrollToVisible (Node n)
		{
			if (!nodeToWrapper.ContainsKey (n))
				return 0;

			var item = nodeToWrapper [n];
			var row = outlineView.RowForItem (item);
			outlineView.ScrollRowToVisible (row);
			return row;
		}
		
		public class OutlineDelegate : NSOutlineViewDelegate {
			MyDocument parent;
			
			public OutlineDelegate (MyDocument parent)
			{
				this.parent = parent;
			}
			
			public override void SelectionDidChange (NSNotification notification)
			{
				if (parent.ignoreSelect)
					return;
				
				var indexes = parent.outlineView.SelectedRows;
				if (indexes.Count == 0)
					return;
				
				var node = WrapNode.FromObject (parent.outlineView.ItemAtRow ((int) indexes.FirstIndex));
				parent.LoadUrl (node.PublicUrl, false, node.Tree.HelpSource);
			}
		}

		// If this returns the name of a NIB file instead of null, a NSDocumentController 
		// is automatically created for you.
		public override string WindowNibName {
			get { return "MyDocument"; }
		}
		
		public WebView MainWebView {
			get {
				return webView;
			}
		}
		
		public override NSPrintOperation PrintOperation (NSDictionary printSettings, out NSError outError)
		{
			outError = null;
			return webView.MainFrame.FrameView.GetPrintOperation (new NSPrintInfo (printSettings));
		}
		
		public override bool ShouldChangePrintInfo (NSPrintInfo newPrintInfo)
		{
			return false;
		}
		
		public override void EncodeRestorableState (NSCoder coder)
		{
			base.EncodeRestorableState (coder);
			coder.Encode (new NSString (string.IsNullOrEmpty (currentUrl) ? string.Empty : currentUrl), "monodoc.currentUrl");
		}
		
		public override void RestoreState (NSCoder coder)
		{
			base.RestoreState (coder);
			if (!coder.ContainsKey ("monodoc.currentUrl"))
				return;
			NSString url = coder.DecodeObject ("monodoc.currentUrl") as NSString;
			if (url != null && !string.IsNullOrEmpty (url.ToString ()))
				LoadUrl (url.ToString (), true);
		}
		
		public class MultipleMatchDataSource : NSTableViewSource 
		{
			MyDocument doc;
				
			public MultipleMatchDataSource (MyDocument doc)
			{
				this.doc = doc;
			}
				
			public override int GetRowCount (NSTableView tableView)
			{
				if (doc.current_entry == null)
					return 0;
				return doc.current_entry.Count;
			}
				
			public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row)
			{
				Topic topic = doc.current_entry [row];
				return new NSString (RenderTopicMatch (topic));
			}
				
			// Names from the ECMA provider are somewhat
			// ambigious (you have like a million ToString
			// methods), so lets give the user the full name
			string RenderTopicMatch (Topic t)
			{
				// Filter out non-ecma
				if (t.Url [1] != ':')
					return t.Caption;
	
				switch (t.Url [0]) {
				case 'C': return t.Url.Substring (2) + " constructor";
				case 'M': return t.Url.Substring (2) + " method";
				case 'P': return t.Url.Substring (2) + " property";
				case 'F': return t.Url.Substring (2) + " field";
				case 'E': return t.Url.Substring (2) + " event";
				}
				return t.Caption;
			}
		}
	}
}
