
using System;
using System.Collections.Generic;
using System.Linq;
using Monodoc;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.WebKit;
using System.IO;
using System.Threading.Tasks;

namespace macdoc
{
	public partial class MyDocument : MonoMac.AppKit.NSDocument
	{
		internal Dictionary<Node,WrapNode> nodeToWrapper = new Dictionary<Node, WrapNode> ();
		string resourcesPath = NSBundle.MainBundle.ResourceUrl.Path;
		History history;
		bool ignoreSelect;
		string initialLoadFromUrl;
		Node match;
		
		SearchableIndex searchIndex;
		
		// Called when created from unmanaged code
		public MyDocument (IntPtr handle) : base(handle)
		{
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MyDocument (NSCoder coder) : base(coder)
		{
		}

		void LoadImages ()
		{
			navigationCells.SetImage (new NSImage (Path.Combine (resourcesPath, "Images", "back.png")), 0);
			navigationCells.SetImage (new NSImage (Path.Combine (resourcesPath, "Images", "forward.png")), 1);
		}
		
		public override string DisplayName {
			get {
				return "Mono Documentation Browser";
			}
		}
		
		public override bool ReadFromUrl (NSUrl url, string typeName, out NSError outError)
		{
			Console.WriteLine ("ReadFromUrl : {0}", url.ToString ());
			outError = null;
			const int NSServiceMiscellaneousError = 66800;
			if (url.Scheme != "monodoc" && url.Scheme != "mdoc") {
				outError = new NSError (NSError.CocoaErrorDomain,
				                       	NSServiceMiscellaneousError,
				                      	NSDictionary.FromObjectAndKey (NSError.LocalizedFailureReasonErrorKey, new NSString (string.Format ("Scheme {0} isn't supported", url.Scheme))));
				return false;
			}
			
			// ResourceSpecifier is e.g. "//T:System.String"
			initialLoadFromUrl = url.ResourceSpecifier.Substring (2);
			this.FileUrl = url;
			
			return true;
		}
		
		public override void WindowControllerDidLoadNib (NSWindowController windowController)
		{
			base.WindowControllerDidLoadNib (windowController);
		
			tabSelector.SelectFirst (this);
			LoadImages ();
			history = new History (navigationCells);
			SetupOutline ();
			SetupSearch ();
			webView.DecidePolicyForNavigation += HandleWebViewDecidePolicyForNavigation;
			webView.FinishedLoad += HandleWebViewFinishedLoad;
			if (!string.IsNullOrEmpty (initialLoadFromUrl))
				LoadUrl (initialLoadFromUrl);
		}
		
		void SetupOutline ()
		{
			outlineView.DataSource = new DocTreeDataSource (this);
			outlineView.Delegate = new OutlineDelegate (this);
		}
		
		void SetupSearch ()
		{
			AppDelegate.IndexUpdateManager.UpdaterChange += ToggleSearchCreationStatus;
			searchIndex = AppDelegate.Root.GetSearchIndex ();
			searchResults.Source = new ResultDataSource ();
		}
		
		void ToggleSearchCreationStatus (object sender, EventArgs e)
		{
			var manager = (IndexUpdateManager)sender;

			if (!manager.IsCreatingSearchIndex) {
				InvokeOnMainThread (delegate {
					spinnerWidget.StopAnimation (this);
					spinnerView.Hidden = true;
					searchIndex = AppDelegate.Root.GetSearchIndex ();
				});
			} else {
				InvokeOnMainThread (delegate {
					spinnerView.Hidden = false;
					spinnerWidget.StartAnimation (this);
				});
			}
		}
		
		void LoadUrl (string url)
		{
			if (url.StartsWith ("#")){
				Console.WriteLine ("FIXME: Anchor jump");
				return;
			}
			Node node;
			string res = DocTools.GetHtml (url, null, out node);
			if (res != null){
					history.AppendHistory (new LinkPageVisit (this, node.PublicUrl));
					LoadHtml (res);
			}
		}
		
		void IndexSearch (string text)
		{
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
				Console.WriteLine ("adding result");
				dataSource.AddResultSet (rs);
				searchResults.ReloadData ();
			}));
			searchResults.ReloadData ();
			if (results.Count > 0) {
				searchResults.SelectRow (1, false);
				searchResults.ScrollRowToVisible (0);
				OnIndexRowSelected (1);
			}
		}
		
		void OnIndexRowSelected (int targetRow)
		{
			var url = ((ResultDataSource)searchResults.Source).GetResultUrl (targetRow);
			if (url == null)
				return;
			LoadUrl (url);
		}
		
		// Action: when the user clicks on the index table view
		partial void IndexItemClicked (NSTableView sender)
		{
			OnIndexRowSelected (sender.ClickedRow);
		}
		
		// Action: when the user starts typing on the toolbar search bar	
		partial void StartSearch (NSSearchField sender)
		{
			var contents = sender.StringValue;
			if (contents == null || contents == "")
				return;
			tabSelector.SelectAt (1);
			IndexSearch (contents);
		}

		void HandleWebViewDecidePolicyForNavigation (object sender, WebNavigatioPolicyEventArgs e)
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
			
			Node match;
			WebView.DecideIgnore (e.DecisionToken);
			var res = DocTools.GetHtml (url, null, out match);
			if (res == null)
				return;
			
			history.AppendHistory (new LinkPageVisit (this, url));
			LoadHtml (res);
			// When navigation occurs after a link on search result is clicked
			// we need to show the panel so that ShowNode work as expected
			tabSelector.SelectAt (0);
			this.match = match;
		}
		
		// Because WebView doesn't let me answer a NSUrlRequest myself I have to resort to this piece of crap of a solution
		void HandleWebViewFinishedLoad (object sender, WebFrameEventArgs e)
		{
			if (match != null) {
				ShowNode (match);
				match = null;
			}
			var dom = e.ForFrame.DomDocument;
			var imgs = dom.GetElementsByTagName ("img").Where (node => node.Attributes["src"].Value.StartsWith ("source-id"));
			byte[] buffer = new byte[4096];
			
			foreach (var img in imgs) {
				var src = img.Attributes["src"].Value;
				var imgStream = AppDelegate.Root.GetImage (src);
				if (imgStream == null)
					continue;
				Console.WriteLine ("Buffer length {0}", buffer.Length);
				var length = imgStream.Read (buffer, 0, buffer.Length);
				var read = length;
				Console.WriteLine (length);
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
		public void LoadHtml (string html)
		{
			LoadingFromString = true;
			webView.MainFrame.LoadHtmlString (html, AppDelegate.MonodocBaseUrl);
#if debug_html
			using (var x = System.IO.File.CreateText (AppDelegate.MonodocDir + "/foo.html")){
				x.Write (html);
			}
#endif
			LoadingFromString = false;
		}
		
		internal void ShowNode (Node n)
		{
			if (n == null)
				return;
			
			if (!nodeToWrapper.ContainsKey (n))
				ShowNode (n.Parent);
			
			var item = nodeToWrapper [n];
			outlineView.ExpandItem (item);
			
			// Focus the last child, then this child to ensure we show as much as possible
			if (n.Nodes.Count > 0)
				ScrollToVisible ((Node) n.Nodes [n.Nodes.Count-1]);
			var row = ScrollToVisible (n);
			ignoreSelect = true;
			outlineView.SelectRows (new NSIndexSet (row), false);
			ignoreSelect = false;
		}
		
		int ScrollToVisible (Node n)
		{
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
				string html = DocTools.GetHtml (node.PublicUrl, node.tree.HelpSource);
				if (html != null){
					parent.history.AppendHistory (new LinkPageVisit (parent, node.PublicUrl));
					parent.LoadHtml (html);
					return;
				}
				
				parent.LoadHtml ("<html><body>do we really need anything else: " + node.PublicUrl);
			}			
		}

		// If this returns the name of a NIB file instead of null, a NSDocumentController 
		// is automatically created for you.
		public override string WindowNibName {
			get { return "MyDocument"; }
		}
	}

	class LinkPageVisit : PageVisit {
		MyDocument document;
		string url;
		
		public LinkPageVisit (MyDocument document, string url)
		{
			this.document = document;
			this.url = url;
		}
		
		public override void Go ()
		{
			Node match;
			string res =  DocTools.GetHtml (url, null, out match);
			document.LoadHtml (res);
			document.ShowNode (match);
		}
	}
	
	// 
	// The OutlineView works by passing NSObject tokens around for its data.
	// We conveniently have a Node structure from MonoDoc that we can use
	// so we just wrap that in this WrapNode class
	//
	public class WrapNode : NSObject {
		public WrapNode (Node n) { Node = n; }
		public Node Node { get; set; }
		public static Node FromObject (NSObject obj)
		{
			return (obj as WrapNode).Node;
		}
	}
	
	//
	// Data source for rendering the tree 
	//
	public class DocTreeDataSource : NSOutlineViewDataSource {
		RootTree Root = AppDelegate.Root;
			
		// We need to keep all objects that we have ever handed out to the Outline View around
		Dictionary<Node,WrapNode> nodeToWrapper;
		
		static Node GetNode (NSObject obj)
		{
			return WrapNode.FromObject (obj);
		}
		
		public DocTreeDataSource (MyDocument parent)
		{
			nodeToWrapper = parent.nodeToWrapper;
		}
		
		public override NSObject GetChild (NSOutlineView outlineView, int index, NSObject item)
		{
			WrapNode wrap;
			Node n = (Node) (item == null ? Root : (Node) GetNode (item)).Nodes [index];

			if (nodeToWrapper.ContainsKey (n))
				return nodeToWrapper [n];
			wrap = new WrapNode (n);
			nodeToWrapper [n] = wrap;
			return wrap;
		}
		
		public override bool ItemExpandable (NSOutlineView outlineView, NSObject item)
		{
			if (item == null)
				return true;
			return GetNode (item).Nodes.Count > 0;
		}
		
		public override int GetChildrenCount (NSOutlineView outlineView, NSObject item)
		{
			if (item == null)
				return Root.Nodes.Count;
			return GetNode (item).Nodes.Count;
		}
		
		public override NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
		{
			if (item == null)
				return new NSString ("Root");
			
			return new NSString (GetNode (item).Caption);
		}
	}
	
	//
	// Data source for rendering the index
	//
	public class IndexDataSource : NSTableViewSource {
		RootTree Root = AppDelegate.Root;
		IndexReader index_reader;
		
		public IndexDataSource ()
		{
			index_reader = Root.GetIndex ();
			
		}
		
		public override int GetRowCount (NSTableView tableView)
		{
			if (index_reader == null)
				return 0;
			return index_reader.Rows;
		}
		
		public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			return new NSString (index_reader.GetValue (row));
		}
	}
	
	public class ResultDataSource : NSTableViewSource
	{
		struct ResultDataEntry
		{
			// value is element is a section, null if it's a section child
			public string SectionName { get; set; }
			public Result ResultSet { get; set; }
			public int Index { get; set; }
		}
		
		// Dict key is section name, value is a sorted list of section element (result it comes from and the index in it) with key being the url (to avoid duplicates)
		Dictionary<string, SortedList<string, Tuple<Result, int>>> sections = new Dictionary<string, SortedList<string, Tuple<Result, int>>> ();
		List<ResultDataEntry> data = new List<ResultDataEntry> ();
		
		NSTextFieldCell normalCell;
		NSTextFieldCell headerCell;
		
		public ResultDataSource ()
		{
			normalCell = new NSTextFieldCell ();
			
			headerCell = new NSTextFieldCell ();
			headerCell.TextColor = NSColor.Gray;
			headerCell.Alignment = NSTextAlignment.Center;
			headerCell.Selectable = false;
			headerCell.Editable = false;
			headerCell.FocusRingType = NSFocusRingType.None;
			headerCell.LineBreakMode = NSLineBreakMode.TruncatingMiddle;
		}
		
		public void AddResultSet (Result result)
		{
			for (int i = 0; i < result.Count; i++) {
				string fullTitle = result.GetFullTitle (i);
				string section = string.IsNullOrWhiteSpace (fullTitle) ? "Other" : fullTitle.Split (':')[0];
				SortedList<string, Tuple<Result, int>> sectionContent;
				var newItem = Tuple.Create (result, i);
				var url = result.GetUrl (i);
				
				if (!sections.TryGetValue (section, out sectionContent))
					sections[section] = new SortedList<string, Tuple<Result, int>> () { { url, newItem } };
				else
					sectionContent[url] = newItem;
			}
			// Flatten everything back to a list
			data.Clear ();
			foreach (var kvp in sections) {
				data.Add (new ResultDataEntry { SectionName = kvp.Key });
				foreach (var item in kvp.Value)
					data.Add (new ResultDataEntry { ResultSet = item.Value.Item1, Index = item.Value.Item2 });
			}
		}
		
		public void ClearResultSet ()
		{
			sections.Clear ();
		}
		
		public override int GetRowCount (NSTableView tableView)
		{
			return data.Count;
		}
		
		public override NSCell GetCell (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			if (tableView == null)
				return null;
			var resultEntry = data[row];
			return !string.IsNullOrEmpty (resultEntry.SectionName) ? headerCell : normalCell;
		}
		
		public string GetResultUrl (int row)
		{
			var resultEntry = data[row];
			return resultEntry.ResultSet == null ? null : resultEntry.ResultSet.GetUrl (resultEntry.Index);
		}
		
		public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			var resultEntry = data[row];
			return new NSString (!string.IsNullOrEmpty (resultEntry.SectionName) ? resultEntry.SectionName : resultEntry.ResultSet.GetTitle (resultEntry.Index));
		}
		
		public override bool ShouldSelectRow (NSTableView tableView, int row)
		{
			// If it's a section, do not select
			return string.IsNullOrEmpty (data[row].SectionName);
		}
		
		// Keep the search term in memory so that heavy search can check if its result are still fresh enough
		public string LatestSearchTerm { get; set; }
	}
}

