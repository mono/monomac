// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace SearchField {
	
	
	// Should subclass MonoMac.AppKit.NSWindow
	[MonoMac.Foundation.Register("SearchFieldWindow")]
	public partial class SearchFieldWindow {
	}
	
	// Should subclass MonoMac.AppKit.NSWindowController
	[MonoMac.Foundation.Register("SearchFieldWindowController")]
	public partial class SearchFieldWindowController {
		
		private global::MonoMac.AppKit.NSSearchField __mt_searchField;
		
		private global::MonoMac.AppKit.NSWindow __mt_simpleSheet;
		
		#pragma warning disable 0169
		[MonoMac.Foundation.Export("sheetDone:")]
		partial void sheetDone (MonoMac.AppKit.NSButton sender);

		[MonoMac.Foundation.Connect("searchField")]
		private global::MonoMac.AppKit.NSSearchField searchField {
			get {
				this.__mt_searchField = ((global::MonoMac.AppKit.NSSearchField)(this.GetNativeField("searchField")));
				return this.__mt_searchField;
			}
			set {
				this.__mt_searchField = value;
				this.SetNativeField("searchField", value);
			}
		}
		
		[MonoMac.Foundation.Connect("simpleSheet")]
		private global::MonoMac.AppKit.NSWindow simpleSheet {
			get {
				this.__mt_simpleSheet = ((global::MonoMac.AppKit.NSWindow)(this.GetNativeField("simpleSheet")));
				return this.__mt_simpleSheet;
			}
			set {
				this.__mt_simpleSheet = value;
				this.SetNativeField("simpleSheet", value);
			}
		}
	}
}