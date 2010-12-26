
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Monodoc;

namespace macdoc
{
	public partial class MyDocument : MonoMac.AppKit.NSDocument
	{
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

			outlineView.DataSource = new DocTreeDataSource ();
			outlineView.Delegate = new OutlineDelegate (this);
			webView.DecidePolicyForNavigation += delegate(object sender, MonoMac.WebKit.WebNavigatioPolicyEventArgs e) {
				Console.WriteLine (e.Request);
				WebPolicyDelegate.DecisionUse (e.WebPolicyDecisionListener);
			};
		}
		
		// 
		// Save support:
		//    Override one of GetAsData, GetAsFileWrapper, or WriteToUrl.
		//

		// This method should store the contents of the document using the given typeName
		// on the return NSData value.
		public override NSData GetAsData (string documentType, out NSError outError)
		{
			outError = NSError.FromDomain (NSError.OsStatusErrorDomain, -4);
			return null;
		}

		// 
		// Load support:
		//    Override one of ReadFromData, ReadFromFileWrapper or ReadFromUrl
		//
		public override bool ReadFromData (NSData data, string typeName, out NSError outError)
		{
			outError = NSError.FromDomain (NSError.OsStatusErrorDomain, -4);
			return false;
		}

		// If this returns the name of a NIB file instead of null, a NSDocumentController 
		// is automatically created for you.
		public override string WindowNibName {
			get { return "MyDocument"; }
		}
		
		public void LoadHtml (string html)
		{
			webView.MainFrame.LoadHtmlString (html, new NSUrl ("file:///"));
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
		Dictionary<Node,WrapNode> nodeToWrapper = new Dictionary<Node, WrapNode> ();
		
		static Node GetNode (NSObject obj)
		{
			return WrapNode.FromObject (obj);
		}
		
		public DocTreeDataSource ()
		{
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

