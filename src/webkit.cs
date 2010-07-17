using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;

namespace MonoMac.WebKit {
	[BaseType (typeof (WebScriptObject), Name="DOMObject")]
	interface DomObject {
	}

	[BaseType (typeof (DomObject), Name="DOMRange")]
	interface DomRange {
	}

	[BaseType (typeof (DomObject), Name="DOMCSSStyleDeclaration")]
	interface DomCssStyleDeclaration {
	}
	
	[BaseType (typeof (DomObject), Name="DOMNode")]
	interface DomNode {
	}
	
	[BaseType (typeof (DomNode), Name="DOMDocument")]
	interface DomDocument {
	}
	
	[BaseType (typeof (DomNode), Name="DOMElement")]
	interface DomElement {
	}

	[BaseType (typeof (DomNode), Name="DOMHTMLElement")]
	interface DomHtmlElement {
	}

	[BaseType (typeof (NSObject))]
	interface WebArchive {
		[Export ("initWithMainResource:subresources:subframeArchives:")]
		IntPtr Constructor (WebResource mainResource, NSArray subresources, NSArray subframeArchives);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("mainResource")]
		WebResource MainResource { get; }

		[Export ("subresources")]
		WebResource [] Subresources { get; }

		[Export ("subframeArchives")]
		WebArchive [] SubframeArchives { get; }

		[Export ("data")]
		NSData Data { get; }
	}

	[BaseType (typeof (NSObject))]
	interface WebBackForwardList {
		[Export ("addItem:")]
		void AddItem (WebHistoryItem item);

		[Export ("goBack")]
		void GoBack ();

		[Export ("goForward")]
		void GoForward ();

		[Export ("goToItem:")]
		void GoToItem (WebHistoryItem item);

		[Export ("backItem")]
		WebHistoryItem BackItem ();

		[Export ("currentItem")]
		WebHistoryItem CurrentItem ();

		[Export ("forwardItem")]
		WebHistoryItem ForwardItem ();

		[Export ("backListWithLimit:")]
		WebHistoryItem [] BackListWithLimit (int limit);

		[Export ("forwardListWithLimit:")]
		WebHistoryItem [] ForwardListWithLimit (int limit);

		[Export ("backListCount")]
		int BackListCount { get; }

		[Export ("forwardListCount")]
		int ForwardListCount { get; }

		[Export ("containsItem:")]
		bool ContainsItem (WebHistoryItem item);

		[Export ("itemAtIndex:")]
		WebHistoryItem ItemAtIndex (int index);

		//Detected properties
		[Export ("capacity")]
		int Capacity { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface WebDataSource {
		[Export ("initWithRequest:")]
		IntPtr Constructor (NSUrlRequest request);

		[Export ("data")]
		NSData Data { get; }

		[Export ("representation")]
		WebDocumentRepresentation Representation { get; }

		[Export ("webFrame")]
		WebFrame WebFrame { get; }

		[Export ("initialRequest")]
		NSUrlRequest InitialRequest { get; }

		[Export ("request")]
		NSMutableUrlRequest Request { get; }

		[Export ("response")]
		NSUrlResponse Response { get; }

		[Export ("textEncodingName")]
		string TextEncodingName { get; }

		[Export ("isLoading")]
		bool IsLoading { get; }

		[Export ("pageTitle")]
		string PageTitle { get; }

		[Export ("unreachableURL")]
		NSUrl UnreachableURL { get; }

		[Export ("webArchive")]
		WebArchive WebArchive { get; }

		[Export ("mainResource")]
		WebResource MainResource { get; }

		[Export ("subresources")]
		WebResource [] Subresources { get; }

		[Export ("subresourceForURL:")]
		WebResource SubresourceForUrl (NSUrl url);

		[Export ("addSubresource:")]
		void AddSubresource (WebResource subresource);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface WebDocumentRepresentation {
		[Abstract]
		[Export ("setDataSource:")]
		void SetDataSource (WebDataSource dataSource);

		[Abstract]
		[Export ("receivedData:withDataSource:")]
		void ReceivedData (NSData data, WebDataSource dataSource);

		[Abstract]
		[Export ("receivedError:withDataSource:")]
		void ReceivedError (NSError error, WebDataSource dataSource);

		[Abstract]
		[Export ("finishedLoadingWithDataSource:")]
		void FinishedLoading (WebDataSource dataSource);

		[Abstract]
		[Export ("canProvideDocumentSource")]
		bool CanProvideDocumentSource { get; }

		[Abstract]
		[Export ("documentSource")]
		string DocumentSource { get; }

		[Abstract]
		[Export ("title")]
		string Title { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface WebDocumentView {
		[Abstract]
		[Export ("setDataSource:")]
		void SetDataSource (WebDataSource dataSource);

		[Abstract]
		[Export ("dataSourceUpdated:")]
		void DataSourceUpdated (WebDataSource dataSource);

		[Abstract]
		[Export ("setNeedsLayout:")]
		void SetNeedsLayout (bool flag);

		[Abstract]
		[Export ("layout")]
		void Layout ();

		[Abstract]
		[Export ("viewWillMoveToHostWindow:")]
		void ViewWillMoveToHostWindow (NSWindow hostWindow);

		[Abstract]
		[Export ("viewDidMoveToHostWindow")]
		void ViewDidMoveToHostWindow ();
	}

	[BaseType (typeof (NSObject))]
	interface WebFrame {
		[Export ("initWithName:webFrameView:webView:")]
		IntPtr Constructor (string name, WebFrameView view, WebView webView);

		[Export ("name")]
		string Name { get; }

		[Export ("webView")]
		WebView WebView { get; }

		[Export ("frameView")]
		WebFrameView FrameView { get; }

		[Export ("DOMDocument")]
		DomDocument DomDocument { get; }

		[Export ("frameElement")]
		DomHtmlElement FrameElement { get; }

		[Export ("loadRequest:")]
		void LoadRequest (NSUrlRequest request);

		[Export ("loadData:MIMEType:textEncodingName:baseURL:")]
		void LoadData (NSData data, string mimeType, string textDncodingName, NSUrl baseUrl);

		[Export ("loadHTMLString:baseURL:")]
		void LoadHtmlString (string htmlString, NSUrl baseUrl);

		[Export ("loadAlternateHTMLString:baseURL:forUnreachableURL:")]
		void LoadAlternateHtmlString (string htmlString, NSUrl baseURL, NSUrl forUnreachableURL);

		[Export ("loadArchive:")]
		void LoadArchive (WebArchive archive);

		[Export ("dataSource")]
		WebDataSource DataSource { get; }

		[Export ("provisionalDataSource")]
		WebDataSource ProvisionalDataSource { get; }

		[Export ("stopLoading")]
		void StopLoading ();

		[Export ("reload")]
		void Reload ();

		[Export ("reloadFromOrigin")]
		void ReloadFromOrigin ();

		[Export ("findFrameNamed:")]
		WebFrame FindFrameNamed (string name);

		[Export ("parentFrame")]
		WebFrame ParentFrame { get; }

		[Export ("childFrames")]
		WebFrame [] ChildFrames { get; }

		[Export ("windowObject")]
		WebScriptObject WindowObject { get; }

		[Export ("globalContext")]
		/* JSGlobalContextRef */ IntPtr GlobalContext { get; }

	}

	[BaseType (typeof (NSView))]
	interface WebFrameView {
		[Export ("webFrame")]
		WebFrame WebFrame { get; }

		[Export ("documentView")]
		WebDocumentView DocumentView { get; }

		[Export ("canPrintHeadersAndFooters")]
		bool CanPrintHeadersAndFooters { get; }

		[Export ("printOperationWithPrintInfo:")]
		NSPrintOperation GetPrintOperation (NSPrintInfo printInfo);

		[Export ("documentViewShouldHandlePrint")]
		bool DocumentViewShouldHandlePrint { get; }

		[Export ("printDocumentView")]
		void PrintDocumentView ();

		//Detected properties
		[Export ("allowsScrolling")]
		bool AllowsScrolling { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface WebHistoryItem {
		[Export ("initWithURLString:title:lastVisitedTimeInterval:")]
		IntPtr Constructor (string urlString, string title, double lastVisitedTimeInterval);

		[Export ("originalURLString")]
		string OriginalUrlString { get; }

		[Export ("URLString")]
		string UrlString { get; }

		[Export ("title")]
		string Title { get; }

		[Export ("lastVisitedTimeInterval")]
		double LastVisitedTimeInterval { get; }

		[Export ("icon")]
		NSImage Icon { get; }

		//Detected properties
		[Export ("alternateTitle")]
		string AlternateTitle { get; set; }
	}


	[BaseType (typeof (NSObject))]
	interface WebPreferences {
		[Export ("standardPreferences")]
		WebPreferences StandardPreferences { get; }

		[Export ("initWithIdentifier:")]
		IntPtr Constructor (string identifier);

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("arePlugInsEnabled")]
		bool PlugInsEnabled { get; set; }

		//Detected properties
		[Export ("standardFontFamily")]
		string StandardFontFamily { get; set; }

		[Export ("fixedFontFamily")]
		string FixedFontFamily { get; set; }

		[Export ("serifFontFamily")]
		string SerifFontFamily { get; set; }

		[Export ("sansSerifFontFamily")]
		string SansSerifFontFamily { get; set; }

		[Export ("cursiveFontFamily")]
		string CursiveFontFamily { get; set; }

		[Export ("fantasyFontFamily")]
		string FantasyFontFamily { get; set; }

		[Export ("defaultFontSize")]
		int DefaultFontSize { get; set; }

		[Export ("defaultFixedFontSize")]
		int DefaultFixedFontSize { get; set; }

		[Export ("minimumFontSize")]
		int MinimumFontSize { get; set; }

		[Export ("minimumLogicalFontSize")]
		int MinimumLogicalFontSize { get; set; }

		[Export ("defaultTextEncodingName")]
		string DefaultTextEncodingName { get; set; }

		[Export ("userStyleSheetEnabled")]
		bool UserStyleSheetEnabled { get; set; }

		[Export ("userStyleSheetLocation")]
		NSUrl UserStyleSheetLocation { get; set; }

		[Export ("javaEnabled")]
		bool JavaEnabled { [Bind ("isJavaEnabled")]get; set; }

		[Export ("javaScriptEnabled")]
		bool JavaScriptEnabled { [Bind ("isJavaScriptEnabled")]get; set; }

		[Export ("javaScriptCanOpenWindowsAutomatically")]
		bool JavaScriptCanOpenWindowsAutomatically { get; set; }

		[Export ("allowsAnimatedImages")]
		bool AllowsAnimatedImages { get; set; }

		[Export ("allowsAnimatedImageLooping")]
		bool AllowsAnimatedImageLooping { get; set; }

		[Export ("loadsImagesAutomatically")]
		bool LoadsImagesAutomatically { get; set; }

		[Export ("autosaves")]
		bool Autosaves { get; set; }

		[Export ("shouldPrintBackgrounds")]
		bool ShouldPrintBackgrounds { get; set; }

		[Export ("privateBrowsingEnabled")]
		bool PrivateBrowsingEnabled { get; set; }

		[Export ("tabsToLinks")]
		bool TabsToLinks { get; set; }

		[Export ("usesPageCache")]
		bool UsesPageCache { get; set; }

		[Export ("cacheModel")]
		WebCacheModel CacheModel { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface WebResource {
		[Export ("initWithData:URL:MIMEType:textEncodingName:frameName:")]
		IntPtr Constructor (NSData data, NSUrl url, string mimeType, string textEncodingName, string frameName);

		[Export ("data")]
		NSData Data { get; }

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("MIMEType")]
		string MimeType { get; }

		[Export ("textEncodingName")]
		string TextEncodingName { get; }

		[Export ("frameName")]
		string FrameName { get; }
	}

	[BaseType (typeof (NSObject))]
	interface WebScriptObject {
		[Static, Export ("throwException:")]
		bool ThrowException (string exceptionMessage);

		[Export ("JSObject")]
		/* JSObjectRef */ IntPtr JSObject { get; }

		[Export ("callWebScriptMethod:withArguments:")]
		NSObject CallWebScriptMethod (string name, NSObject [] arguments);

		[Export ("evaluateWebScript:")]
		NSObject EvaluateWebScript (string script);

		[Export ("removeWebScriptKey:")]
		void RemoveWebScriptKey (string name);

		[Export ("stringRepresentation")]
		string StringRepresentation { get; }

		[Export ("webScriptValueAtIndex:")]
		NSObject WebScriptValueAtIndex (int index);

		[Export ("setWebScriptValueAtIndex:value:")]
		void SetWebScriptValueAtIndexvalue (int index, NSObject value);

		[Export ("setException:")]
		void SetException (string description);
	}

	[BaseType (typeof (NSView))]
	interface WebView {
		[Export ("canShowMIMEType:")]
		bool CanShowMimeType (string MimeType);

		[Static]
		[Export ("canShowMIMETypeAsHTML:")]
		bool CanShowMimeTypeAsHtml (string mimeType);

		[Static]
		[Export ("MIMETypesShownAsHTML")]
		string [] MimeTypesShownAsHtml { get; set; }

		[Static]
		[Export ("URLFromPasteboard:")]
		NSUrl UrlFromPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("URLTitleFromPasteboard:")]
		string UrlTitleFromPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("registerURLSchemeAsLocal:")]
		void RegisterUrlSchemeAsLocal (string scheme);

		[Export ("initWithFrame:frameName:groupName:")]
		IntPtr Constructor (RectangleF frame, string frameName, string groupName);

		[Export ("close")]
		void Close ();

		[Export ("UIDelegate")]
		NSObject UIDelegate { get; set; }

		[Export ("mainFrame")]
		WebFrame MainFrame { get; }

		[Export ("selectedFrame")]
		WebFrame SelectedFrame { get; }

		[Export ("backForwardList")]
		WebBackForwardList BackForwardList { get; } 

		[Export ("setMaintainsBackForwardList:")]
		void SetMaintainsBackForwardList (bool flag);

		[Export ("goBack")]
		bool GoBack ();

		[Export ("goForward")]
		bool GoForward ();

		[Export ("goToBackForwardItem:")]
		bool GoToBackForwardItem (WebHistoryItem item);

		[Export ("userAgentForURL:")]
		string UserAgentForUrl (NSUrl url);

		[Export ("supportsTextEncoding")]
		bool SupportsTextEncoding { get; }

		[Export ("stringByEvaluatingJavaScriptFromString:")]
		string StringByEvaluatingJavaScriptFromString (string script);

		[Export ("windowScriptObject")]
		WebScriptObject WindowScriptObject { get; }

		[Export ("searchFor:direction:caseSensitive:wrap:")]
		bool Search (string forString, bool forward, bool caseSensitive, bool wrap);

		[Static]
		[Export ("registerViewClass:representationClass:forMimeType:")]
		void RegisterViewClass (Class viewClass, Class representationClass, string MimeType);

		[Export ("estimatedProgress")]
		double EstimatedProgress { get; }

		[Export ("isLoading")]
		bool IsLoading { get; }

		[Export ("elementAtPoint:")]
		NSDictionary ElementAtPoint (PointF point);

		[Export ("pasteboardTypesForSelection")]
		NSPasteboard [] PasteboardTypesForSelection { get; }

		[Export ("writeSelectionWithPasteboardTypes:toPasteboard:")]
		void WriteSelection (NSObject [] types, NSPasteboard pasteboard);

		[Export ("pasteboardTypesForElement:")]
		NSObject [] PasteboardTypesForElement (NSDictionary element);

		[Export ("writeElement:withPasteboardTypes:toPasteboard:")]
		void WriteElement (NSDictionary element, NSObject [] pasteboardTypes, NSPasteboard toPasteboard);

		[Export ("moveDragCaretToPoint:")]
		void MoveDragCaretToPoint (PointF point);

		[Export ("removeDragCaret")]
		void RemoveDragCaret ();

		[Export ("mainFrameDocument")]
		DomDocument MainFrameDocument { get; }

		[Export ("mainFrameTitle")]
		string MainFrameTitle { get; }

		[Export ("mainFrameIcon")]
		NSImage MainFrameIcon { get; }

		//Detected properties
		[Export ("shouldCloseWithWindow")]
		bool ShouldCloseWithWindow { get; set; }

		[Export ("resourceLoadDelegate")]
		NSObject ResourceLoadDelegate { get; set; }

		[Export ("downloadDelegate")]
		NSObject DownloadDelegate { get; set; }

		[Export ("frameLoadDelegate")]
		NSObject FrameLoadDelegate { get; set; }

		[Export ("policyDelegate")]
		NSObject PolicyDelegate { get; set; }

		[Export ("textSizeMultiplier")]
		float TextSizeMultiplier { get; set; }

		[Export ("applicationNameForUserAgent")]
		string ApplicationNameForUserAgent { get; set; }

		[Export ("customUserAgent")]
		string CustomUserAgent { get; set; }

		[Export ("customTextEncodingName")]
		string CustomTextEncodingName { get; set; }

		[Export ("mediaStyle")]
		string MediaStyle { get; set; }

		[Export ("preferences")]
		WebPreferences Preferences { get; set; }

		[Export ("preferencesIdentifier")]
		string PreferencesIdentifier { get; set; }

		[Export ("hostWindow")]
		NSWindow HostWindow { get; set; }

		[Export ("groupName")]
		string GroupName { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("shouldUpdateWhileOffscreen")]
		bool UpdateWhileOffscreen { get; set; }

		[Export ("mainFrameURL")]
		string MainFrameUrl { get; set; }

		// NSUserInterfaceValidations
		[Export ("reload:")]
		void Reload (NSObject sender);

		[Export ("reloadFromOrigin:")]
		void ReloadFromOrigin (NSObject sender);

		[Export ("canGoBack")]
		bool CanGoBack ();

		//[Export ("goBack:")]
		//void GoBack (NSObject sender);

		[Export ("canGoForward")]
		bool CanGoForward ();

		//[Export ("goForward:")]
		//void GoForward (NSObject sender);

		[Export ("canMakeTextLarger")]
		bool CanMakeTextLarger ();

		[Export ("makeTextLarger:")]
		void MakeTextLarger (NSObject sender);

		[Export ("canMakeTextSmaller")]
		bool CanMakeTextSmaller ();

		[Export ("makeTextSmaller:")]
		void MakeTextSmaller (NSObject sender);

		[Export ("canMakeTextStandardSize")]
		bool CanMakeTextStandardSize ();

		[Export ("makeTextStandardSize:")]
		void MakeTextStandardSize (NSObject sender);

		[Export ("toggleContinuousSpellChecking:")]
		void ToggleContinuousSpellChecking (NSObject sender);

		[Export ("toggleSmartInsertDelete:")]
		void ToggleSmartInsertDelete (NSObject sender);

		[Export ("selectedDOMRange")]
		DomRange SelectedDomRange { get; }

		[Export ("selectionAffinity")]
		NSSelectionAffinity SelectionAffinity { get; }

		[Export ("maintainsInactiveSelection")]
		bool MaintainsInactiveSelection { get; }

		[Export ("spellCheckerDocumentTag")]
		int SpellCheckerDocumentTag { get; }

		[Export ("undoManager")]
		NSUndoManager UndoManager { get; }

		[Export ("styleDeclarationWithText:")]
		DomCssStyleDeclaration StyleDeclarationWithText (string text);

		//Detected properties
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("typingStyle")]
		DomCssStyleDeclaration TypingStyle { get; set; }

		[Export ("smartInsertDeleteEnabled")]
		bool SmartInsertDeleteEnabled { get; set; }

		[Export ("continuousSpellCheckingEnabled")]
		bool ContinuousSpellCheckingEnabled { [Bind ("isContinuousSpellCheckingEnabled")]get; set; }

		[Export ("editingDelegate")]
		NSObject EditingDelegate { get; set; }

		[Export ("replaceSelectionWithMarkupString:")]
		void ReplaceSelectionWithMarkupString (string markupString);

		[Export ("replaceSelectionWithArchive:")]
		void ReplaceSelectionWithArchive (WebArchive archive);

		[Export ("deleteSelection")]
		void DeleteSelection ();

		[Export ("applyStyle:")]
		void ApplyStyle (DomCssStyleDeclaration style);

		[Export ("cut:")]
		void Cut (NSObject sender);

		[Export ("paste:")]
		void Paste (NSObject sender);

		[Export ("copyFont:")]
		void CopyFont (NSObject sender);

		[Export ("pasteFont:")]
		void PasteFont (NSObject sender);

		[Export ("delete:")]
		void Delete (NSObject sender);

		[Export ("pasteAsPlainText:")]
		void PasteAsPlainText (NSObject sender);

		[Export ("pasteAsRichText:")]
		void PasteAsRichText (NSObject sender);

		[Export ("changeFont:")]
		void ChangeFont (NSObject sender);

		[Export ("changeAttributes:")]
		void ChangeAttributes (NSObject sender);

		[Export ("changeDocumentBackgroundColor:")]
		void ChangeDocumentBackgroundColor (NSObject sender);

		[Export ("changeColor:")]
		void ChangeColor (NSObject sender);

		[Export ("alignCenter:")]
		void AlignCenter (NSObject sender);

		[Export ("alignJustified:")]
		void AlignJustified (NSObject sender);

		[Export ("alignLeft:")]
		void AlignLeft (NSObject sender);

		[Export ("alignRight:")]
		void AlignRight (NSObject sender);

		[Export ("checkSpelling:")]
		void CheckSpelling (NSObject sender);

		[Export ("showGuessPanel:")]
		void ShowGuessPanel (NSObject sender);

		[Export ("performFindPanelAction:")]
		void PerformFindPanelAction (NSObject sender);

		[Export ("startSpeaking:")]
		void StartSpeaking (NSObject sender);

		[Export ("stopSpeaking:")]
		void StopSpeaking (NSObject sender);

		[Export ("moveToBeginningOfSentence:")]
		void MoveToBeginningOfSentence (NSObject sender);

		[Export ("moveToBeginningOfSentenceAndModifySelection:")]
		void MoveToBeginningOfSentenceAndModifySelection (NSObject sender);

		[Export ("moveToEndOfSentence:")]
		void MoveToEndOfSentence (NSObject sender);

		[Export ("moveToEndOfSentenceAndModifySelection:")]
		void MoveToEndOfSentenceAndModifySelection (NSObject sender);

		[Export ("selectSentence:")]
		void SelectSentence (NSObject sender);
	}
}