
using System;
using System.Collections.Generic;
using System.Linq;
using Monodoc;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.WebKit;
using System.IO;

namespace macdoc
{
	public partial class MyDocument : MonoMac.AppKit.NSDocument
	{
		internal Dictionary<Node,WrapNode> nodeToWrapper = new Dictionary<Node, WrapNode> ();
		string resourcesPath = NSBundle.MainBundle.ResourceUrl.Path;
		History history;
		bool ignoreSelect;
		
		// For the index
		IndexReader index_reader;
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
		public override void WindowControllerDidLoadNib (NSWindowController windowController)
		{
			base.WindowControllerDidLoadNib (windowController);
		
			LoadImages ();
			history = new History (navigationCells);
			SetupOutline ();
			SetupIndexSearch ();
			HideMultipleMatches ();
			webView.DecidePolicyForNavigation += HandleWebViewDecidePolicyForNavigation; 
		}
		
		void HideMultipleMatches ()
		{
			splitView.SetPositionofDivider (splitView.MaxPositionOfDivider (0), 0);
		}
		
		void ShowMultipleMatches ()
		{
			float middle = (splitView.MaxPositionOfDivider (0) - splitView.MinPositionOfDivider (0))/2;
			splitView.SetPositionofDivider (middle, 0);
		}
		
		void SetupOutline ()
		{
			outlineView.DataSource = new DocTreeDataSource (this);
			outlineView.Delegate = new OutlineDelegate (this);
		}
		
		void SetupIndexSearch ()
		{
			index_reader = AppDelegate.Root.GetIndex ();
			searchResults.Source = new IndexDataSource ();
			searchField.Changed += HandleChanged;
			multipleMatchResults.Source = new MultipleMatchDataSource (this);
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
		
		void HandleChanged (object sender, EventArgs e)
		{
			var text = searchField.StringValue;
			if (text == null || text == "")
				return;
			IndexSearch (text);
		}
		
		void IndexSearch (string text)
		{
			int targetRow = FindClosest (text);
			searchResults.SelectRow (targetRow, false);
			searchResults.ScrollRowToVisible (targetRow);

			OnIndexRowSelected (targetRow);
		}
		
		void OnIndexRowSelected (int targetRow)
		{
			current_entry = index_reader.GetIndexEntry (targetRow);
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
		
		// Action: when the user clicks on the index table view
		partial void IndexItemClicked (NSTableView sender)
		{
			OnIndexRowSelected (sender.ClickedRow);
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
			if (contents == null || contents == "")
				return;
			tabSelector.SelectAt (1);
			IndexSearch (contents);
		}

		int FindClosest (string text)
		{
			int low = 0;
			int top = index_reader.Rows-1;
			int high = top;
			int best_rate_idx = Int32.MaxValue, best_rate = -1;
			
			while (low < high){
				int mid = (high+low)/2;
				int p = mid;
				string s;
				
				for (s = index_reader.GetValue (mid); s [0] == ' ';){
					if (p == high){
						if (p == low){
							if (best_rate_idx != Int32.MaxValue)
								return best_rate_idx;
							else
								return p;
						}
						high = mid;
						break;
					}
					if (p < 0)
						return 0;
					s = index_reader.GetValue (++p);
				}
				if (s [0] == ' ')
					continue;
				int c, rate;
				c = Rate (text, s, out rate);
				if (rate > best_rate){
					best_rate = rate;
					best_rate_idx = p;
				}
				if (c == 0)
					return mid;
				if (low == high){
					if (best_rate_idx != Int32.MaxValue)
						return best_rate_idx;
					else
						return low;
				}
				if (c < 0)
					high = mid;
				else {
					if (low == mid)
						low = high;
					else
						low = mid;
				}
			}
			return high;
		}
		
		int Rate (string user_text, string db_text, out int rate)
		{
			int c = String.Compare (user_text, db_text, true);
			if (c == 0){
				rate = 0;
				return 0;
			}
			int i;
			for (i = 0; i < user_text.Length; i++){
				if (db_text [i] != user_text [i]){
					rate = i;
					return c;
				}
			}
			rate = i;
			return c;
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
			ShowNode (match);
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
		
		// 
		// Table view model that renders the contents when there is more than one
		// match for an entry in the index, for example "ToString"
		//
		public class MultipleMatchDataSource : NSTableViewSource {
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
}

