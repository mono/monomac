//
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

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

	/////////////////////////
	// DomObject subclasses

	[BaseType (typeof (DomObject), Name="DOMAbstractView")]
	interface DomAbstractView {
		[Export ("document")]
		DomDocument Document { get; }
	}

	[BaseType (typeof (DomObject), Name="DOMCSSRule")]
	interface DomCssRule {
		[Export ("type")]
		DomCssRuleType Type { get; }

		[Export ("cssText")]
		string CssText { get; }

		[Export ("parentStyleSheet")]
		DomCssStyleSheet ParentStyleSheet { get;  }

		[Export ("parentRule")]
		DomCssRule ParentRule { get;  }
	}

	[BaseType (typeof (DomObject), Name="DOMCSSRuleList")]
	interface DomCssRuleList {
		[Export ("length")]
		int Count { get; }

		[Export ("item:")]
		DomCssRule GetItem (int index);
	}

	[BaseType (typeof (DomObject), Name="DOMCSSStyleDeclaration")]
	interface DomCssStyleDeclaration {
		[Export ("cssText")]
		string CssText { get; }

		[Export ("length")]
		int Count { get; }

		[Export ("parentRule")]
		DomCssRule ParentRule { get;  }

		[Export ("getPropertyValue:")]
		string GetPropertyValue (string propertyName);

		[Export ("getPropertyCSSValue:")]
		DomCssValue GetPropertyCssValue (string propertyName);

		[Export ("removeProperty:")]
		string RemoveProperty (string propertyName);

		[Export ("getPropertyPriority:")]
		string GetPropertyPriority (string propertyName);

		[Export ("setProperty:value:priority:")]
		void SetProperty (string propertyName, string value, string priority);

		[Export ("item:")]
		string GetItem (int index);

		[Export ("getPropertyShorthand:")]
		string GetPropertyShorthand (string propertyName);

		[Export ("isPropertyImplicit:")]
		bool IsPropertyImplicit (string propertyName);
	}

	[BaseType (typeof (DomStyleSheet), Name="DOMCSSStyleSheet")]
	interface DomCssStyleSheet {
		[Export ("ownerRule")]
		DomCssRule OwnerRule { get; }
	
		[Export ("cssRules")]
		DomCssRuleList CssRules { get;  }

		[Export ("rules")]
		DomCssRuleList Rules { get;  }

		[Export ("insertRule:index:")]
		uint InsertRule (string rule, uint index);

		[Export ("deleteRule:")]
		void DeleteRule (uint index);

		[Export ("addRule:style:index:")]
		int AddRule (string selector, string style, uint index);

		[Export ("removeRule:")]
		void RemoveRule (uint index);
	}

	[BaseType (typeof (DomObject), Name="DOMCSSValue")]
	interface DomCssValue {
		[Export ("cssText")]
		string CssText { get; }

		[Export ("cssValueType")]
		DomCssValueType Type { get; }
	}

	[BaseType (typeof (DomObject), Name="DOMHTMLCollection")]
	interface DomHtmlCollection {
		[Export ("length")]
		int Count { get; }

		[Export ("item:")]
		DomNode GetItem (int index);

		[Export ("namedItem:")]
		DomNode GetNamedItem (string name);

		[Export ("tags:")]
		DomNodeList GetTags (string name);
	}

	[BaseType (typeof (DomObject), Name="DOMImplementation")]
	interface DomImplementation {
		[Export ("hasFeature:version:")]
		bool HasFeature (string feature, string version);

		[Export ("createDocumentType:publicId:systemId:")]
		DomDocumentType CreateDocumentType (string qualifiedName, string publicId, string systemId);

		[Export ("createDocument:qualifiedName:doctype:")]
		DomDocument CreateDocument (string namespaceUri, string qualifiedName, DomDocumentType doctype);

		[Export ("createCSSStyleSheet:media:")]
		DomCssStyleSheet CreateCssStyleSheet (string title, string media);

		[Export ("createHTMLDocument:")]
		DomHtmlDocument CreateHtmlDocument (string title);
	}

	[BaseType (typeof (DomObject), Name="DOMMediaList")]
	interface DomMediaList {
		[Export ("mediaText")]
		string MediaText { get; set; }

		[Export ("length")]
		int Count { get; }

		[Export ("item:")]
		string GetItem (int index);

		[Export ("deleteMedium:")]
		void DeleteMedium (string oldMedium);

		[Export ("appendMedium:")]
		void AppendMedium (string newMedium);
	}

	[BaseType (typeof (DomObject), Name="DOMNamedNodeMap")]
	interface DomNamedNodeMap {
		[Export ("length")]
		int Count { get; }

		[Export ("getNamedItem:")]
		DomNode GetNamedItem (string name);

		[Export ("setNamedItem:")]
		DomNode SetNamedItem (DomNode node);

		[Export ("removeNamedItem:")]
		DomNode RemoveNamedItem (string name);

		[Export ("item:")]
		DomNode GetItem (int index);

		[Export ("getNamedItemNS:localName:")]
		DomNode GetNamedItemNS (string namespaceUri, string localName);

		[Export ("setNamedItemNS:")]
		DomNode SetNamedItemNS (DomNode node);

		[Export ("removeNamedItemNS:localName:")]
		DomNode RemoveNamedItemNS (string namespaceURI, string localName);
	}

	[BaseType (typeof (DomObject), Name="DOMNode")]
	interface DomNode {
		[Export ("nodeName")]
		string Name { get; }

		[Export ("nodeValue")]
		string Value { get; }

		[Export ("nodeType")]
		DomNodeType NodeType { get;  }

		[Export ("parentNode")]
		DomNode ParentNode { get;  }

		[Export ("childNodes")]
		DomNodeList ChildNodes { get;  }

		[Export ("firstChild")]
		DomNode FirstChild { get;  }

		[Export ("lastChild")]
		DomNode LastChild { get;  }

		[Export ("previousSibling")]
		DomNode PreviousSibling { get;  }

		[Export ("nextSibling")]
		DomNode NextSibling { get;  }

		[Export ("attributes")]
		DomNamedNodeMap Attributes { get;  }

		[Export ("ownerDocument")]
		DomDocument OwnerDocument { get;  }

		[Export ("namespaceURI")]
		string NamespaceURI { get;  }

		[Export ("prefix")]
		string Prefix { get; set;  }

		[Export ("localName")]
		string LocalName { get;  }

		[Export ("baseURI")]
		string BaseURI { get;  }

		[Export ("textContent")]
		string TextContent { get; set;  }

		[Export ("parentElement")]
		DomElement ParentElement { get;  }

		[Export ("isContentEditable")]
		bool IsContentEditable { get;  }

		[Export ("insertBefore:refChild:")]
		DomNode InsertBefore (DomNode newChild, [NullAllowed] DomNode refChild);

		[Export ("replaceChild:oldChild:")]
		DomNode ReplaceChild (DomNode newChild, DomNode oldChild);

		[Export ("removeChild:")]
		DomNode RemoveChild (DomNode oldChild);

		[Export ("appendChild:")]
		DomNode AppendChild (DomNode newChild);

		[Export ("hasChildNodes")]
		bool HasChildNodes ();

		[Export ("cloneNode:")]
		DomNode CloneNode (bool deep);

		[Export ("normalize")]
		void Normalize ();

		[Export ("isSupported:version:")]
		bool IsSupported (string feature, string version);

		[Export ("hasAttributes")]
		bool HasAttributes ();

		[Export ("isSameNode:")]
		bool IsSameNode ([NullAllowed] DomNode other);

		[Export ("isEqualNode:")]
		bool IsEqualNode ([NullAllowed] DomNode other);

		[Export ("lookupPrefix:")]
		string LookupPrefix (string namespaceURI);

		[Export ("isDefaultNamespace:")]
		bool IsDefaultNamespace (string namespaceURI);

		[Export ("lookupNamespaceURI:")]
		string LookupNamespace (string prefix);

		[Export ("compareDocumentPosition")]
		DomDocumentPosition CompareDocumentPosition (DomNode other);

		//
		// These come from the adopted protocol DOMEventTarget
		//
		[Export ("addEventListener:listener:useCapture:")]
		void AddEventListener (string type, DomEventListener listener, bool useCapture);

		[Export ("removeEventListener:listener:useCapture:")]
		void RemoveEventListener (string type, DomEventListener listener, bool useCapture);

		[Export ("dispatchEvent:")]
		bool DispatchEvent (DomEvent evt);
	}

	[BaseType (typeof (DomObject), Name="DOMNodeList")]
	interface DomNodeList {
		[Export ("length")]
		int Count { get; }

		[Export ("item:")]
		DomNode GetItem (int index);
	}

	[BaseType (typeof (DomObject), Name="DOMRange")]
	interface DomRange {
		[Export ("startContainer")]
		DomNode StartContainer { get;  }

		[Export ("startOffset")]
		int StartOffset { get;  }

		[Export ("endContainer")]
		DomNode EndContainer { get;  }

		[Export ("endOffset")]
		int EndOffset { get;  }

		[Export ("collapsed")]
		bool Collapsed { get;  }

		[Export ("commonAncestorContainer")]
		DomNode CommonAncestorContainer { get;  }

		[Export ("text")]
		string Text { get;  }

		[Export ("setStart:offset:")]
		void SetStart (DomNode refNode, int offset );

		[Export ("setEnd:offset:")]
		void SetEnd (DomNode refNode, int offset);

		[Export ("setStartBefore:")]
		void SetStartBefore (DomNode refNode);

		[Export ("setStartAfter:")]
		void SetStartAfter (DomNode refNode);

		[Export ("setEndBefore:")]
		void SetEndBefore (DomNode refNode);

		[Export ("setEndAfter:")]
		void SetEndAfter (DomNode refNode);

		[Export ("collapse:")]
		void Collapse (bool toStart);

		[Export ("selectNode:")]
		void SelectNode (DomNode refNode);

		[Export ("selectNodeContents:")]
		void SelectNodeContents (DomNode refNode);

		[Export ("compareBoundaryPoints:sourceRange:")]
		short CompareBoundaryPoints (DomRangeCompareHow how, DomRange sourceRange);

		[Export ("deleteContents")]
		void DeleteContents ();

		[Export ("extractContents")]
		DomDocumentFragment ExtractContents ();

		[Export ("cloneContents")]
		DomDocumentFragment CloneContents ();

		[Export ("insertNode:")]
		void InsertNode (DomNode newNode);

		[Export ("surroundContents:")]
		void SurroundContents (DomNode newParent);

		[Export ("cloneRange")]
		DomRange CloneRange ();

		[Export ("toString")]
		string ToString ();

		[Export ("detach")]
		void Detach ();

		[Export ("createContextualFragment:")]
		DomDocumentFragment CreateContextualFragment (string html);

		[Export ("intersectsNode:")]
		bool IntersectsNode (DomNode refNode);

		[Export ("compareNode:")]
		short CompareNode (DomNode refNode);

		[Export ("comparePoint:offset:")]
		short ComparePoint (DomNode refNode, int offset);

		[Export ("isPointInRange:offset:")]
		bool IsPointInRange (DomNode refNode, int offset);
	}

	[BaseType (typeof (DomObject), Name="DOMStyleSheet")]
	interface DomStyleSheet {
		[Export ("type")]
		string Type { get; }

		[Export ("disabled")]
		bool Disabled { get; set; }

		[Export ("ownerNode")]
		DomNode OwnerNode { get;  }

		[Export ("parentStyleSheet")]
		DomStyleSheet ParentStyleSheet { get;  }

		[Export ("href")]
		string Href { get;  }

		[Export ("title")]
		string Title { get;  }

		[Export ("media")]
		DomMediaList Media { get;  }
	}

	[BaseType (typeof (DomObject), Name="DOMStyleSheetList")]
	interface DomStyleSheetList {
		[Export ("length")]
		int Count { get; }

		[Export ("item:")]
		DomStyleSheet GetItem (int index);
	}

	///////////////////////
	// DomNode subclasses

	[BaseType (typeof (DomNode), Name="DOMAttr")]
	interface DomAttr {
		[Export ("name")]
		string Name { get; }

		[Export ("specified")]
		bool Specified { get; }

		[Export ("value")]
		string Value { get; set;  }

		[Export ("ownerElement")]
		DomElement OwnerElement { get;  }

		[Export ("style")]
		DomCssStyleDeclaration Style { get;  }
	}

	[BaseType (typeof (DomNode), Name="DOMCharacterData")]
	interface DomCharacterData {
		[Export ("data")]
		string Data { get; }

		[Export ("length")]
		int Count { get; }

		[Export ("substringData:length:")]
		string SubstringData (uint offset, uint length);

		[Export ("appendData:")]
		void AppendData (string data);

		[Export ("insertData:data:")]
		void InsertData (uint offset, string data);

		[Export ("deleteData:length:")]
		void DeleteData (uint offset, uint length);

		[Export ("replaceData:length:data:")]
		void ReplaceData (uint offset, uint length, string data);
	}

	[BaseType (typeof (DomNode), Name="DOMDocument")]
	interface DomDocument {
		[Export ("doctype")]
		DomDocumentType DocumentType { get; }

		[Export ("implementation")]
		DomImplementation Implementation { get; }

		[Export ("documentElement")]
		DomElement DocumentElement { get;  }

		[Export ("inputEncoding")]
		string InputEncoding { get;  }

		[Export ("xmlEncoding")]
		string XmlEncoding { get;  }

		[Export ("xmlVersion")]
		string XmlVersion { get;  }

		[Export ("xmlStandalone")]
		bool XmlStandalone { get; set;  }

		[Export ("documentURI")]
		string DocumentURI { get; set;  }

		[Export ("defaultView")]
		DomAbstractView DefaultView { get;  }

		[Export ("styleSheets")]
		DomStyleSheetList StyleSheets { get;  }

		[Export ("title")]
		string Title { get; set;  }

		[Export ("referrer")]
		string Referrer { get;  }

		[Export ("domain")]
		string Domain { get;  }

		[Export ("URL")]
		string Url { get;  }

		[Export ("cookie")]
		string Cookie { get; set;  }

		[Export ("body")]
		DomHtmlElement body { get; set;  }

		[Export ("images")]
		DomHtmlCollection images { get;  }

		[Export ("applets")]
		DomHtmlCollection applets { get;  }

		[Export ("links")]
		DomHtmlCollection links { get;  }

		[Export ("forms")]
		DomHtmlCollection forms { get;  }

		[Export ("anchors")]
		DomHtmlCollection anchors { get;  }

		[Export ("lastModified")]
		string LastModified { get;  }

		[Export ("charset")]
		string Charset { get; set;  }

		[Export ("defaultCharset")]
		string DefaultCharset { get;  }

		[Export ("readyState")]
		string ReadyState { get;  }

		[Export ("characterSet")]
		string CharacterSet { get;  }

		[Export ("preferredStylesheetSet")]
		string PreferredStylesheetSet { get;  }

		[Export ("selectedStylesheetSet")]
		string SelectedStylesheetSet { get; set;  }

		[Export ("createElement:")]
		DomElement CreateElement (string tagName);

		[Export ("createDocumentFragment")]
		DomDocumentFragment CreateDocumentFragment ();

		[Export ("createTextNode:")]
		DomText CreateTextNode (string data);

		[Export ("createComment:")]
		DomComment CreateComment (string data);

		[Export ("createCDATASection:")]
		DomCDataSection CreateCDataSection (string data);

		[Export ("createProcessingInstruction:data:")]
		DomProcessingInstruction CreateProcessingInstruction (string target, string data);

		[Export ("createAttribute:")]
		DomAttr CreateAttribute (string name);

		[Export ("createEntityReference:")]
		DomEntityReference CreateEntityReference (string name);

		[Export ("getElementsByTagName:")]
		DomNodeList GetElementsByTagName (string tagname);

		[Export ("importNode:deep:")]
		DomNode ImportNode (DomNode importedNode, bool deep);

		[Export ("createElementNS:qualifiedName:")]
		DomElement CreateElementNS (string namespaceURI, string qualifiedName);

		[Export ("createAttributeNS:qualifiedName:")]
		DomAttr CreateAttributeNS (string namespaceURI, string qualifiedName);

		[Export ("getElementsByTagNameNS:localName:")]
		DomNodeList GetElementsByTagNameNS (string namespaceURI, string localName);

		[Export ("getElementById:")]
		DomElement GetElementById (string elementId);

		[Export ("adoptNode:")]
		DomNode AdoptNode (DomNode source);

		// TODO
		//[Export ("createEvent:")]
		//DomEvent CreateEvent (string eventType);

		[Export ("createRange")]
		DomRange CreateRange ();

		// TODO
		//[Export ("createNodeIterator:whatToShow:filter:expandEntityReferences:")]
		//DomNodeIterator CreateNodeIterator (DomNode root, unsigned whatToShow, id <DomNodeFilter> filter, bool expandEntityReferences);

		//[Export ("createTreeWalker:whatToShow:filter:expandEntityReferences:")]
		//DomTreeWalker CreateTreeWalker (DomNode root, unsigned whatToShow, id <DomNodeFilter> filter, bool expandEntityReferences);

		[Export ("getOverrideStyle:pseudoElement:")]
		DomCssStyleDeclaration GetOverrideStyle (DomElement element, string pseudoElement);

		//[Export ("createExpression:resolver:")]
		//DomXPathExpression CreateExpression (string expression, id <DomXPathNSResolver> resolver);

		//[Export ("createNSResolver:")]
		//id <DomXPathNSResolver> CreateNSResolver (DomNode nodeResolver);

		//[Export ("evaluate:contextNode:resolver:type:inResult:")]
		//DomXPathResult Evaluate (string expression, DomNode contextNode, id <DomXPathNSResolver> resolver, unsigned short type, DomXPathResult inResult);

		[Export ("execCommand:userInterface:value:")]
		bool ExecCommand (string command, bool userInterface, string value);

		[Export ("execCommand:userInterface:")]
		bool ExecCommand (string command, bool userInterface);

		[Export ("execCommand:")]
		bool ExecCommand (string command);

		[Export ("queryCommandEnabled:")]
		bool QueryCommandEnabled (string command);

		[Export ("queryCommandIndeterm:")]
		bool QueryCommandIndeterm (string command);

		[Export ("queryCommandState:")]
		bool QueryCommandState (string command);

		[Export ("queryCommandSupported:")]
		bool QueryCommandSupported (string command);

		[Export ("queryCommandValue:")]
		string QueryCommandValue (string command);

		[Export ("getElementsByName:")]
		DomNodeList GetElementsByName (string elementName);

		[Export ("elementFromPoint:y:")]
		DomElement ElementFromPoint (int x, int y);

		[Export ("createCSSStyleDeclaration")]
		DomCssStyleDeclaration CreateCssStyleDeclaration ();

		[Export ("getComputedStyle:pseudoElement:")]
		DomCssStyleDeclaration GetComputedStyle (DomElement element, string pseudoElement);

		[Export ("getMatchedCSSRules:pseudoElement:")]
		DomCssRuleList GetMatchedCSSRules (DomElement element, string pseudoElement);

		[Export ("getMatchedCSSRules:pseudoElement:authorOnly:")]
		DomCssRuleList GetMatchedCSSRules (DomElement element, string pseudoElement, bool authorOnly);

		[Export ("getElementsByClassName:")]
		DomNodeList GetElementsByClassName (string tagname);

		[Export ("querySelector:")]
		DomElement QuerySelector (string selectors);

		[Export ("querySelectorAll:")]
		DomNodeList QuerySelectorAll (string selectors);
	}

	[BaseType (typeof (DomNode), Name="DOMDocumentFragment")]
	interface DomDocumentFragment {
	}

	[BaseType (typeof (DomNode), Name="DOMDocumentType")]
	interface DomDocumentType {
		[Export ("name")]
		string Name { get;  }

		[Export ("entities")]
		DomNamedNodeMap Entities { get;  }

		[Export ("notations")]
		DomNamedNodeMap Notations { get;  }

		[Export ("publicId")]
		string PublicId { get;  }

		[Export ("systemId")]
		string SystemId { get;  }

		[Export ("internalSubset")]
		string InternalSubset { get;  }

	}
	
	[BaseType (typeof (DomNode), Name="DOMElement")]
	interface DomElement {
		[Export ("offsetLeft")]
		int OffsetLeft { get;  }

		[Export ("offsetTop")]
		int OffsetTop { get;  }

		[Export ("offsetWidth")]
		int OffsetWidth { get;  }

		[Export ("offsetHeight")]
		int OffsetHeight { get;  }

		[Export ("offsetParent")]
		DomElement OffsetParent { get;  }

		[Export ("clientLeft")]
		int ClientLeft { get;  }

		[Export ("clientTop")]
		int ClientTop { get;  }

		[Export ("clientWidth")]
		int ClientWidth { get;  }

		[Export ("clientHeight")]
		int ClientHeight { get;  }

		[Export ("scrollLeft")]
		int ScrollLeft { get; set;  }

		[Export ("scrollTop")]
		int ScrollTop { get; set;  }

		[Export ("scrollWidth")]
		int ScrollWidth { get;  }

		[Export ("scrollHeight")]
		int ScrollHeight { get;  }

		[Export ("firstElementChild")]
		DomElement FirstElementChild { get;  }

		[Export ("lastElementChild")]
		DomElement LastElementChild { get;  }

		[Export ("previousElementSibling")]
		DomElement PreviousElementSibling { get;  }

		[Export ("nextElementSibling")]
		DomElement NextElementSibling { get;  }

		[Export ("childElementCount")]
		uint ChildElementCount { get;  }

		[Export ("innerText")]
		string InnerText { get;  }

		[Export ("getAttribute:")]
		string GetAttribute (string name);

		[Export ("setAttribute:value:")]
		void SetAttribute (string name, string value);

		[Export ("removeAttribute:")]
		void RemoveAttribute (string name);

		[Export ("getAttributeNode:")]
		DomAttr GetAttributeNode (string name);

		[Export ("setAttributeNode:")]
		DomAttr SetAttributeNode (DomAttr newAttr);

		[Export ("removeAttributeNode:")]
		DomAttr RemoveAttributeNode (DomAttr oldAttr);

		[Export ("getElementsByTagName:")]
		DomNodeList GetElementsByTagName (string name);

		[Export ("getAttributeNS:localName:")]
		string GetAttributeNS (string namespaceURI, string localName);

		[Export ("setAttributeNS:qualifiedName:value:")]
		void SetAttributeNS (string namespaceURI, string qualifiedName, string value);

		[Export ("removeAttributeNS:localName:")]
		void RemoveAttributeNS (string namespaceURI, string localName);

		[Export ("getElementsByTagNameNS:localName:")]
		DomNodeList GetElementsByTagNameNS (string namespaceURI, string localName);

		[Export ("getAttributeNodeNS:localName:")]
		DomAttr GetAttributeNodeNS (string namespaceURI, string localName);

		[Export ("setAttributeNodeNS:")]
		DomAttr SetAttributeNodeNS (DomAttr newAttr);

		[Export ("hasAttribute:")]
		bool HasAttribute (string name);

		[Export ("hasAttributeNS:localName:")]
		bool HasAttributeNS (string namespaceURI, string localName);

		[Export ("focus")]
		void Focus ();

		[Export ("blur")]
		void Blur ();

		[Export ("scrollIntoView:")]
		void ScrollIntoView (bool alignWithTop);

		[Export ("contains:")]
		bool Contains (DomElement element);

		[Export ("scrollIntoViewIfNeeded:")]
		void ScrollIntoViewIfNeeded (bool centerIfNeeded);

		[Export ("scrollByLines:")]
		void ScrollByLines (int lines);

		[Export ("scrollByPages:")]
		void ScrollByPages (int pages);

		[Export ("getElementsByClassName:")]
		DomNodeList GetElementsByClassName (string name);

		[Export ("querySelector:")]
		DomElement QuerySelector (string selectors);

		[Export ("querySelectorAll:")]
		DomNodeList QuerySelectorAll (string selectors);
	}

	[BaseType (typeof (DomNode), Name="DOMEntityReference")]
	interface DomEntityReference {
	}

	[BaseType (typeof (DomObject), Name="DOMEvent")]
	interface DomEvent {
		[Export ("type")]
		string Type { get; }

		[Export ("target")]
		NSObject Target { get;  }

		[Export ("currentTarget")]
		NSObject CurrentTarget { get;  }

		[Export ("eventPhase")]
		DomEventPhase EventPhase { get;  }

		[Export ("bubbles")]
		bool Bubbles { get;  }

		[Export ("cancelable")]
		bool Cancelable { get;  }

		[Export ("timeStamp")]
		UInt64 TimeStamp { get;  }

		[Export ("srcElement")]
		NSObject SourceElement { get;  }

		[Export ("returnValue")]
		bool ReturnValue { get; set;  }

		[Export ("cancelBubble")]
		bool CancelBubble { get; set;  }

		[Export ("stopPropagation")]
		void StopPropagation ();

		[Export ("preventDefault")]
		void PreventDefault ();

		[Export ("initEvent:canBubbleArg:cancelableArg:")]
		void InitEvent (string eventTypeArg, bool canBubbleArg, bool cancelableArg);
	}

	[BaseType (typeof (NSObject), Name="DOMEventListener")]
	[Model]
	interface DomEventListener {
		[Abstract]
		[Export ("handleEvent:")]
		void HandleEvent (DomEvent evt);
	}
	
	[BaseType (typeof (DomNode), Name="DOMProcessingInstruction")]
	interface DomProcessingInstruction {
		[Export ("target")]
		string Target { get; }

		[Export ("data")]
		string Data { get; set; }

		[Export ("sheet")]
		DomStyleSheet Sheet { get;  }
	}

	////////////////////////////////
	// DomCharacterData subclasses

	[BaseType (typeof (DomCharacterData), Name="DOMText")]
	interface DomText {
		[Export("wholeText")]
		string WholeText { get; }

		[Export ("splitText:")]
		DomText SplitText (uint offset);

		[Export ("replaceWholeText:")]
		DomText ReplaceWholeText (string content);
	}

	[BaseType (typeof (DomCharacterData), Name="DOMComment")]
	interface DomComment {
	}

	///////////////////////////
	// DomText subclasses

	[BaseType (typeof (DomText), Name="DOMCDATASection")]
	interface DomCDataSection {
	}

	///////////////////////////
	// DomDocument subclasses

	[BaseType (typeof (DomDocument), Name="DOMHTMLDocument")]
	interface DomHtmlDocument {
		[Export ("embeds")]
		DomHtmlCollection Embeds { get;  }

		[Export ("plugins")]
		DomHtmlCollection Plugins { get;  }

		[Export ("scripts")]
		DomHtmlCollection Scripts { get;  }

		[Export ("width")]
		int Width { get;  }

		[Export ("height")]
		int Height { get;  }

		[Export ("dir")]
		string Dir { get; set;  }

		[Export ("designMode")]
		string DesignMode { get; set;  }

		[Export ("compatMode")]
		string CompatMode { get;  }

		[Export ("activeElement")]
		DomElement ActiveElement { get;  }

		[Export ("bgColor")]
		string BackgroundColor { get; set;  }

		[Export ("fgColor")]
		string ForegroundColor { get; set;  }

		[Export ("alinkColor")]
		string ALinkColor { get; set;  }

		[Export ("linkColor")]
		string LinkColor { get; set;  }

		[Export ("vlinkColor")]
		string VLinkColor { get; set;  }

		[Export ("open")]
		void Open ();

		[Export ("close")]
		void Close ();

		[Export ("write:")]
		void Write (string text);

		[Export ("writeln:")]
		void Writeln (string text);

		[Export ("clear")]
		void Clear ();

		[Export ("captureEvents")]
		void CaptureEvents ();

		[Export ("releaseEvents")]
		void ReleaseEvents ();

		[Export ("hasFocus")]
		bool HasFocus ();
	}

	//////////////////////////
	// DomElement subclasses

	[BaseType (typeof (DomElement), Name="DOMHTMLElement")]
	interface DomHtmlElement {
		[Export ("idName")]
		string IdName { get; set;  }

		[Export ("title")]
		string Title { get; set;  }

		[Export ("lang")]
		string Lang { get; set;  }

		[Export ("dir")]
		string Dir { get; set;  }

		[Export ("className")]
		string ClassName { get; set;  }

		[Export ("tabIndex")]
		int TabIndex { get; set;  }

		[Export ("innerHTML")]
		string InnerHTML { get; set;  }

		[Export ("innerText")]
		string InnerText { get; set;  }

		[Export ("outerHTML")]
		string OuterHTML { get; set;  }

		[Export ("outerText")]
		string OuterText { get; set;  }

		[Export ("children")]
		DomHtmlCollection Children { get;  }

		[Export ("contentEditable")]
		string ContentEditable { get; set;  }

		[Export ("isContentEditable")]
		bool IsContentEditable { get;  }

		[Export ("titleDisplayString")]
		string TitleDisplayString { get;  }
	}

	//////////////////////////////////////////////////////////////////

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

	//
	// This is a protocol that is adopted, in some internal classes
	// this is a problem, so I am hiding it for now
	//
	
//	[BaseType (typeof (NSObject))]
//	[Model]
//	interface WebDocumentView {
//		[Abstract]
//		[Export ("setDataSource:")]
//		void SetDataSource (WebDataSource dataSource);
//
//		[Abstract]
//		[Export ("dataSourceUpdated:")]
//		void DataSourceUpdated (WebDataSource dataSource);
//
//		[Abstract]
//		[Export ("setNeedsLayout:")]
//		void SetNeedsLayout (bool flag);
//
//		[Abstract]
//		[Export ("layout")]
//		void Layout ();
//
//		[Abstract]
//		[Export ("viewWillMoveToHostWindow:")]
//		void ViewWillMoveToHostWindow (NSWindow hostWindow);
//
//		[Abstract]
//		[Export ("viewDidMoveToHostWindow")]
//		void ViewDidMoveToHostWindow ();
//	}

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

		// This is an NSVIew<WebDocumentView>, so we need to figure what to do about that
		[Export ("documentView")]
		NSView DocumentView { get; }

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
