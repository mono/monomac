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
			document.LoadUrl (url, true, null, false);
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
			Node n = (Node) (item == null ? Root.RootNode : (Node) GetNode (item)).ChildNodes [index];

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
			return GetNode (item).ChildNodes.Count > 0;
		}
		
		public override int GetChildrenCount (NSOutlineView outlineView, NSObject item)
		{
			if (item == null)
				return Root.RootNode.ChildNodes.Count;
			return GetNode (item).ChildNodes.Count;
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
		IndexSearcher searcher;
		
		public IndexDataSource (IndexSearcher searcher)
		{
			this.searcher = searcher;
		}
		
		public override int GetRowCount (NSTableView tableView)
		{
			if (searcher.Index == null)
				return 0;
			return searcher.Index.Rows;
		}
		
		public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			return new NSString (searcher.Index.GetValue (row));
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
		NSTableHeaderCell headerCell;
		
		public ResultDataSource ()
		{
			normalCell = new NSTextFieldCell ();
			
			headerCell = new NSTableHeaderCell ();
			headerCell.LineBreakMode = NSLineBreakMode.TruncatingMiddle;
			headerCell.FocusRingType = NSFocusRingType.None;
			headerCell.Editable = false;
			headerCell.Selectable = false;
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
			if (data == null || row >= data.Count || row < 0)
				return null;
			
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

