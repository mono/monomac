
using System;
using System.Collections.Generic;
using System.Linq;
using Monodoc;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.WebKit;

namespace macdoc
{
	public partial class MyDocument : MonoMac.AppKit.NSDocument
	{
		internal Dictionary<Node,WrapNode> nodeToWrapper = new Dictionary<Node, WrapNode> ();
		
		// Called when created from unmanaged code
		public MyDocument (IntPtr handle) : base(handle)
		{
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MyDocument (NSCoder coder) : base(coder)
		{
		}

		public override void WindowControllerDidLoadNib (NSWindowController windowController)
		{
			base.WindowControllerDidLoadNib (windowController);
			
			outlineView.DataSource = new DocTreeDataSource (this);
			outlineView.Delegate = new OutlineDelegate (this);
			webView.DecidePolicyForNavigation += HandleWebViewDecidePolicyForNavigation; 
		}

		void HandleWebViewDecidePolicyForNavigation (object sender, WebNavigatioPolicyEventArgs e)
		{
			if (LoadingFromString){
				WebView.DecideUse (e.DecisionToken);
				return;
			}
			
			var mainUrl = e.Frame.WebView.MainFrameUrl;
			var abs = e.Request.Url.AbsoluteString;
			
			// Let WebKit take care of the anchors.
			if (mainUrl != null && abs.StartsWith (mainUrl) && abs.Length > mainUrl.Length && abs [mainUrl.Length] == '#'){
				WebView.DecideUse (e.DecisionToken);
				return;
			}
			
			Node match;
			WebView.DecideIgnore (e.DecisionToken);
			var res = DocTools.GetHtml (abs, null, out match);
			if (res == null)
				return;
			
			LoadHtml (res);	
			ShowNode (match);
		}
		
		// If this returns the name of a NIB file instead of null, a NSDocumentController 
		// is automatically created for you.
		public override string WindowNibName {
			get { return "MyDocument"; }
		}
		
		bool LoadingFromString;
		public void LoadHtml (string html)
		{
			LoadingFromString = true;
			webView.MainFrame.LoadHtmlString (html, new NSUrl ("file:///"));
			LoadingFromString = false;
		}
		
		void ShowNode (Node n)
		{
			if (!nodeToWrapper.ContainsKey (n))
				ShowNode (n.Parent);
			
			var item = nodeToWrapper [n];
			outlineView.ExpandItem (item);
			
			// Focus the last child, then this child to ensure we show as much as possible
			if (n.Nodes.Count > 0)
				ScrollToVisible ((Node) n.Nodes [n.Nodes.Count-1]);
			var row = ScrollToVisible (n);
			outlineView.SelectRows (new NSIndexSet (row), false);
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
				var indexes = parent.outlineView.SelectedRows;
				if (indexes.Count == 0)
					return;
				
				var node = WrapNode.FromObject (parent.outlineView.ItemAtRow ((int) indexes.FirstIndex));
				string html = DocTools.GetHtml (node.PublicUrl, node.tree.HelpSource);
				if (html != null){
					parent.LoadHtml (html);
					return;
				}
				
				parent.LoadHtml ("<html><body>do we really need anything else: " + node.PublicUrl);
			}			

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
			Node n;
			return new NSString (GetNode (item).Caption);
		}
	}
}

