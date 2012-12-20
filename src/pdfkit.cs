//
// Copyright 2011, Novell, Inc.
// Copyright 2011, Regan Sarwas
//
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

//
// PdfKit.cs: Bindings for the PdfKit API
//
using System;
using System.Drawing;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

// Verify/Test Delegate Models
// Check for missing NullAllowed on all object properties
// Test methods returning typed arrays in lieu of NSArray
// Check classes with no public inits - Should I make the constructors private?
// Check the few abnormal properties

namespace MonoMac.PdfKit {

	[BaseType (typeof (NSObject), Name="PDFAction")]
	public interface PdfAction {
		//This is an abstract superclass with no public init - should it have a private constructor??
		//As it is, I can create instances, that segfault when you access the type method.
		//marking the method as [Abstract] doesn't work because the subclasses do not explictly
		//define this method (although they implement it)
		[Export ("type")]
		string Type { get; }
	}

	[BaseType (typeof (PdfAction), Name="PDFActionGoTo")]
	public interface PdfActionGoTo {
		[Export ("initWithDestination:")]
		IntPtr Constructor (PdfDestination destination);

		[Export ("destination")]
		PdfDestination Destination { get; set; }
	}

	[BaseType (typeof (PdfAction), Name="PDFActionNamed")]
	public interface PdfActionNamed {
		[Export ("initWithName:")]
		IntPtr Constructor (PdfActionNamedName name);

		[Export ("name")]
		PdfActionNamedName Name { get; set; }
	}

	[BaseType (typeof (PdfAction), Name="PDFActionRemoteGoTo")]
	public interface PdfActionRemoteGoTo {
		[Export ("initWithPageIndex:atPoint:fileURL:")]
		IntPtr Constructor (int pageIndex, PointF point, NSUrl fileUrl);

		[Export ("pageIndex")]
		int PageIndex { get; set; }

		[Export ("point")]
		PointF Point { get; set; }

		[Export ("URL")]
		NSUrl Url { get; set; }
	}

	[BaseType (typeof (PdfAction), Name="PDFActionResetForm")]
	public interface PdfActionResetForm {
		//has a public Init ???
		
		//NSArray of NSString
		[Export ("fields"), NullAllowed]
		string [] Fields { get; set; }

		[Export ("fieldsIncludedAreCleared")]
		bool FieldsIncludedAreCleared  { get; set; }
	}

	[BaseType (typeof (PdfAction), Name="PDFActionURL")]
	public interface PdfActionUrl {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("URL")]
		NSUrl Url { get; set; }
	}

	[BaseType (typeof (NSObject), Name="PDFAnnotation")]
	public interface PdfAnnotation {
		[Export ("initWithBounds:")]
		IntPtr Constructor (RectangleF bounds);

		[Export ("page")]
		PdfPage Page { get; }

		[Export ("type")]
		string Type { get; }

		[Export ("bounds")]
		RectangleF Bounds { get; set; }
		
		[Export ("modificationDate")]
		NSDate ModificationDate { get; set; }

		[Export ("userName")]
		string UserName { get; set; }

		[Export ("popup")]
		PdfAnnotationPopup Popup { get; set; }

		[Export ("shouldDisplay")]
		bool ShouldDisplay { get; set; }
		
		[Export ("shouldPrint")]
		bool ShouldPrint { get; set; }

		[Export ("border")]
		PdfBorder Border { get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

		[Export ("mouseUpAction")]
		PdfAction MouseUpAction { get; set; }

		[Export ("contents")]
		string Contents { get; set; }
		
		[Export ("toolTip")]
		string ToolTip { get; }

		[Export ("hasAppearanceStream")]
		bool HasAppearanceStream { get; }

		[Export ("removeAllAppearanceStreams")]
		void RemoveAllAppearanceStreams ();

		[Export ("drawWithBox:")]
		void Draw (PdfDisplayBox box);
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationButtonWidget")]
	public interface PdfAnnotationButtonWidget {
		[Export ("controlType")]
		PdfWidgetControlType ControlType { get; set; }
		
		[Export ("state")]
		int State { get; set; }

		[Export ("highlighted")]
		bool Highlighted { [Bind ("isHighlighted")] get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
		
		[Export ("allowsToggleToOff")]
		bool AllowsToggleToOff { get; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		[Export ("caption")]
		string Caption { get; set; }

		[Export ("fieldName")]
		string FieldName { get; set; }

		[Export ("onStateValue")]
		string OnStateValue { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationChoiceWidget")]
	public interface PdfAnnotationChoiceWidget {
		[Export ("stringValue")]
		string Text { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		[Export ("fieldName")]
		string FieldName { get; set; }

		[Export ("isListChoice")]
		bool IsListChoice { get; set; }
		
		// NSArray of NSString
		[Export ("choices")]
		string [] Choices { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationCircle")]
	public interface PdfAnnotationCircle {
		[Export ("interiorColor")]
		NSColor InteriorColor { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationFreeText")]
	public interface PdfAnnotationFreeText {
		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationInk")]
	public interface PdfAnnotationInk {
		[Export ("paths")]
		NSBezierPath [] Paths { get; }

		[Export ("addBezierPath:")]
		void AddBezierPathpath (NSBezierPath path);

		[Export ("removeBezierPath:")]
		void RemoveBezierPathpath (NSBezierPath path);
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationLine")]
	public interface PdfAnnotationLine {
		[Export ("startPoint")]
		PointF StartPoint { get; set; }

		[Export ("endPoint")]
		PointF EndPoint { get; set; }

		[Export ("startLineStyle")]
		PdfLineStyle StartLineStyle { get; set; }

		[Export ("endLineStyle")]
		PdfLineStyle EndLineStyle { get; set; }

		[Export ("interiorColor")]
		NSColor InteriorColor { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationLink")]
	public interface PdfAnnotationLink {
		[Export ("destination")]
		PdfDestination Destination { get; set; }

		[Export ("URL")]
		NSUrl Url { get; set; }
		
		[Export ("setHighlighted:")]
		void SetHighlighted (bool highlighted);
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationMarkup")]
	public interface PdfAnnotationMarkup {
		//bindings cannot box PointF[] to NSArray
		[Export ("quadrilateralPoints")]
		NSArray QuadrilateralPoints { get; set; }
		//PointF [] QuadrilateralPoints { get; set; }

		[Export ("markupType")]
		PdfMarkupType MarkupType { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationPopup")]
	public interface PdfAnnotationPopup {
		[Export ("isOpen")]
		bool IsOpen { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationSquare")]
	public interface PdfAnnotationSquare {
		[Export ("interiorColor")]
		NSColor InteriorColor { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationStamp")]
	public interface PdfAnnotationStamp {
		[Export ("name")]
		string Name { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationText")]
	public interface PdfAnnotationText {
		[Export ("iconType")]
		PdfTextAnnotationIconType IconType { get; set; }
	}

	[BaseType (typeof (PdfAnnotation), Name="PDFAnnotationTextWidget")]
	public interface PdfAnnotationTextWidget {
		[Export ("stringValue")]
		string StringValue { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("rotation")]
		int Rotation { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }
		
		[Export ("maximumLength")]
		int MaximumLength { get; set; }

		[Export ("fieldName")]
		string FieldName { get; set; }
	}

	[BaseType (typeof (NSObject), Name="PDFBorder")]
	public interface PdfBorder {
		[Export ("style")]
		PdfBorderStyle Style { get; set; }

		[Export ("lineWidth")]
		float LineWidth { get; set; }

		[Export ("horizontalCornerRadius")]
		float HorizontalCornerRadius { get; set; }

		[Export ("verticalCornerRadius")]
		float VerticalCornerRadius { get; set; }
		
		//FIXME
		//NSArray of NSNumber see Docs say see NSBezierPath, which uses float []
		[Export ("dashPattern")]
		NSArray DashPattern { get; set; }
		//float [] DashPattern { get; set; }

		[Export ("drawInRect:")]
		void Draw (RectangleF rect);
	}

	[BaseType (typeof (NSObject), Name="PDFDestination")]
	public interface PdfDestination {
		[Export ("initWithPage:atPoint:")]
		IntPtr Constructor (PdfPage page, PointF point);

		[Export ("page")]
		PdfPage Page { get; }

		[Export ("point")]
		PointF Point { get; }
		
		//Should Compare be more more .Net ified ?
		[Export ("compare:")]
		NSComparisonResult Compare (PdfDestination destination);
	}

	//Add attributes for delegates/events
	[BaseType (typeof (NSObject), Name="PDFDocument", Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (PdfDocumentDelegate)})]
	public interface PdfDocument {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithData:")]
		IntPtr Constructor  (NSData data);

		[Export ("documentURL")]
		NSUrl DocumentUrl { get; }

		//[Export ("documentRef")]
		//CGPdfDocumentRef DocumentRef { get; }

		[Export ("documentAttributes")]
		NSDictionary DocumentAttributes { get; set; }

		[Export ("majorVersion")]
		int MajorVersion { get; }

		[Export ("minorVersion")]
		int MinorVersion { get; }

		[Export ("isEncrypted")]
		bool IsEncrypted { get; }

		[Export ("isLocked")]
		bool IsLocked { get; }

		[Export ("unlockWithPassword:")]
		bool Unlock (string password);

		[Export ("allowsPrinting")]
		bool AllowsPrinting { get; }

		[Export ("allowsCopying")]
		bool AllowsCopying { get; }

		[Export ("permissionsStatus")]
		PdfDocumentPermissions PermissionsStatus { get; }

		[Export ("string")]
		string Text { get; }
		
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		PdfDocumentDelegate Delegate { get; set; }
		
		[Export ("dataRepresentation")]
		NSData GetDataRepresentation ();

		[Export ("dataRepresentationWithOptions:")]
		NSData GetDataRepresentation (NSDictionary options);

		[Export ("writeToFile:")]
		bool Write (string path);

		[Export ("writeToFile:withOptions:")]
		bool Write (string path, NSDictionary options);

		[Export ("writeToURL:")]
		bool Write (NSUrl url);

		[Export ("writeToURL:withOptions:")]
		bool Write (NSUrl url, NSDictionary options);

		[Export ("outlineRoot")]
		PdfOutline OutlineRoot { get; set; }

		[Export ("outlineItemForSelection:")]
		PdfOutline OutlineItem (PdfSelection selection);

		[Export ("pageCount")]
		int PageCount { get; }

		[Export ("pageAtIndex:")]
		PdfPage GetPage (int index);

		[Export ("indexForPage:")]
		int GetPageIndex (PdfPage page);

		[Export ("insertPage:atIndex:")]
		void InsertPage (PdfPage page, int index);

		[Export ("removePageAtIndex:")]
		void RemovePage (int index);

		[Export ("exchangePageAtIndex:withPageAtIndex:")]
		void ExchangePages (int indexA, int indexB);
		
		// Check on how Classes map to Types
		[Export ("pageClass")]
		Class PageClass { get; }
		
		// Shouldn't options should be a bit flag of comparison methods - find enum.
		[Export ("findString:withOptions:")]
		PdfSelection [] Find (string text, int options);

		[Export ("beginFindString:withOptions:")]
		void FindAsync (string text, int options);

		[Export ("beginFindStrings:withOptions:")]
		void FindAsync (string [] text, int options);

		[Export ("findString:fromSelection:withOptions:")]
		PdfSelection Find (string text, PdfSelection selection, int options);

		[Export ("isFinding")]
		bool IsFinding { get; }

		[Export ("cancelFindString")]
		void CancelFind ();

		[Export ("selectionForEntireDocument")]
		PdfSelection SelectEntireDocument ();

		[Export ("selectionFromPage:atPoint:toPage:atPoint:")]
		PdfSelection GetSelection (PdfPage startPage, PointF startPoint, PdfPage endPage, PointF endPoint);

		[Export ("selectionFromPage:atCharacterIndex:toPage:atCharacterIndex:")]
		PdfSelection GetSelection (PdfPage startPage, int startCharIndex, PdfPage endPage, int endCharIndex);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface PdfDocumentDelegate {
		[Export ("didMatchString:"), EventArgs ("PdfSelection")]
		void DidMatchString (PdfSelection sender);

		//the delegate method needs to take at least one parameter
		//[Export ("classForPage"), DelegateName ("ClassForPageDelegate"), DefaultValue (null)]
		//Class ClassForPage ();

		[Export ("classForAnnotationClass:"), DelegateName ("ClassForAnnotationClassDelegate"), DefaultValue (null)]
		Class ClassForAnnotationClass (Class sender);

		[Export ("documentDidEndDocumentFind:"), EventArgs ("NSNotification")]
		void FindFinished (NSNotification notification);

		[Export ("documentDidBeginPageFind:"), EventArgs ("NSNotification")]
		void PageFindStarted (NSNotification notification);

		[Export ("documentDidEndPageFind:"), EventArgs ("NSNotification")]
		void PageFindFinished (NSNotification notification);

		[Export ("documentDidFindMatch:"), EventArgs ("NSNotification")]
		void MatchFound (NSNotification notification);
	}

	[BaseType (typeof (NSObject), Name="PDFOutline")]
	public interface PdfOutline {
		// Why did this have a special init????
		// Constructor/class needs special documentation on how to use one that is created (not obtained from another object).

		[Export ("document")]
		PdfDocument Document { get; }

		[Export ("parent")]
		PdfOutline Parent { get; }

		[Export ("numberOfChildren")]
		int ChildrenCount { get; }

		[Export ("index")]
		int Index { get; }

		[Export ("childAtIndex:")]
		PdfOutline Child (int index);

		[Export ("insertChild:atIndex:")]
		void InsertChild (PdfOutline child, int index);

		[Export ("removeFromParent")]
		void RemoveFromParent ();

		[Export ("label")]
		string Label { get; set; }

		[Export ("isOpen")]
		bool IsOpen { get; set; }

		[Export ("destination")]
		PdfDestination Destination { get; set; }

		[Export ("action")]
		PdfAction Action { get; set; }
	}

	[BaseType (typeof (NSObject), Name="PDFPage")]
	public interface PdfPage {
		[Export ("initWithImage:")]
		IntPtr Constructor (NSImage image);

		[Export ("document")]
		PdfDocument Document { get; }

		//[Export ("pageRef")]
		//CGPdfPageRef PageRef { get; }

		[Export ("label")]
		string Label { get; }

		[Export ("boundsForBox:")]
		RectangleF GetBoundsForBox (PdfDisplayBox box);

		[Export ("setBounds:forBox:")]
		void SetBoundsForBox (RectangleF bounds, PdfDisplayBox box);

		[Export ("rotation")]
		int Rotation { get; set; }
	
		//Check  Docs say: "array will _most likely_ be typed to subclasses of the PdfAnnotation class"
		//do they mean that if it isn't a subclass it is the base class ??
		//Maybe we should be safe and return NSArray ??
		[Export ("annotations")]
		PdfAnnotation [] Annotations { get; }

		[Export ("displaysAnnotations")]
		bool DisplaysAnnotations { get; set; }

		[Export ("addAnnotation:")]
		void AddAnnotation (PdfAnnotation annotation);

		[Export ("removeAnnotation:")]
		void RemoveAnnotation (PdfAnnotation annotation);

		[Export ("annotationAtPoint:")]
		PdfAnnotation GetAnnotation (PointF point);

		[Export ("drawWithBox:")]
		void Draw (PdfDisplayBox box);

		[Export ("transformContextForBox:")]
		void TransformContext (PdfDisplayBox box);

		[Export ("numberOfCharacters")]
		int CharacterCount { get; }

		[Export ("string")]
		string Text { get; }

		[Export ("attributedString")]
		NSAttributedString AttributedString { get; }

		[Export ("characterBoundsAtIndex:")]
		RectangleF GetCharacterBounds (int index);

		[Export ("characterIndexAtPoint:")]
		int GetCharacterIndex (PointF point);

		[Export ("selectionForRect:")]
		PdfSelection GetSelection (RectangleF rect);

		[Export ("selectionForWordAtPoint:")]
		PdfSelection SelectWord (PointF point);

		[Export ("selectionForLineAtPoint:")]
		PdfSelection SelectLine (PointF point);

		[Export ("selectionFromPoint:toPoint:")]
		PdfSelection GetSelection (PointF startPoint, PointF endPoint);

		[Export ("selectionForRange:")]
		PdfSelection GetSelection (NSRange range);

		[Export ("dataRepresentation")]
		NSData DataRepresentation { get; }
	}

	[BaseType (typeof (NSObject), Name="PDFSelection")]
	[DisableDefaultCtor] // An uncaught exception was raised: init: not a valid initializer for PDFSelection
	public interface PdfSelection {
		[Export ("initWithDocument:")]
		IntPtr Constructor (PdfDocument document);

		//verify NSArray
		[Export ("pages")]
		PdfPage [] Pages { get; }

		[Export ("color")]
		NSColor Color { get; set; }

		[Export ("string")]
		string Text { get; }

		[Export ("attributedString")]
		NSAttributedString AttributedString { get; }

		[Export ("boundsForPage:")]
		RectangleF GetBoundsForPage (PdfPage page);
	
		//verify NSArray
		[Export ("selectionsByLine")]
		PdfSelection [] SelectionsByLine ();

		[Export ("addSelection:")]
		void AddSelection (PdfSelection selection);

		[Export ("addSelections:")]
		void AddSelections (PdfSelection [] selections);

		[Export ("extendSelectionAtEnd:")]
		void ExtendSelectionAtEnd (int succeed);

		[Export ("extendSelectionAtStart:")]
		void ExtendSelectionAtStart (int precede);

		[Export ("drawForPage:active:")]
		void Draw (PdfPage page, bool active);

		[Export ("drawForPage:withBox:active:")]
		void Draw (PdfPage page, PdfDisplayBox box, bool active);
	}

	[BaseType (typeof (NSView), Name="PDFThumbnailView")]
	public interface PdfThumbnailView {
		[Export ("PDFView")]
		PdfView PdfView { get; set; }

		[Export ("thumbnailSize")]
		SizeF ThumbnailSize { get; set; }

		[Export ("maximumNumberOfColumns")]
		int MaximumNumberOfColumns { get; set; }

		[Export ("labelFont")]
		NSFont LabelFont { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("allowsDragging")]
		bool AllowsDragging { get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }
	
		//verify NSArray
		[Export ("selectedPages")]
		PdfPage [] SelectedPages { get; }
	}

	[BaseType (typeof (NSView), Name="PDFView", Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (PdfViewDelegate)})]
	public interface PdfView {
		[Export ("document")]
		PdfDocument Document { get; set; }

		[Export ("canGoToFirstPage")]
		bool CanGoToFirstPage { get; }
	
		//Verify
		[Export ("goToFirstPage:")]
		void GoToFirstPage (NSObject sender);

		[Export ("canGoToLastPage")]
		bool CanGoToLastPage { get; }

		[Export ("goToLastPage:")]
		void GoToLastPage (NSObject sender);

		[Export ("canGoToNextPage")]
		bool CanGoToNextPage { get; }

		[Export ("goToNextPage:")]
		void GoToNextPage (NSObject sender);

		[Export ("canGoToPreviousPage")]
		bool CanGoToPreviousPage { get; }

		[Export ("goToPreviousPage:")]
		void GoToPreviousPage (NSObject sender);

		[Export ("canGoBack")]
		bool CanGoBack { get; }

		[Export ("goBack:")]
		void GoBack (NSObject sender);

		[Export ("canGoForward")]
		bool CanGoForward { get; }

		[Export ("goForward:")]
		void GoForward (NSObject sender);

		[Export ("currentPage")]
		PdfPage CurrentPage { get; }

		[Export ("goToPage:")]
		void GoToPage (PdfPage page);

		[Export ("currentDestination")]
		PdfDestination CurrentDestination { get; }

		[Export ("goToDestination:")]
		void GoToDestination (PdfDestination destination);

		[Export ("goToSelection:")]
		void GoToSelection (PdfSelection selection);

		[Export ("goToRect:onPage:")]
		void GoToRectangle (RectangleF rect, PdfPage page);

		[Export ("displayMode")]
		PdfDisplayMode DisplayMode { get; set; }

		[Export ("displaysPageBreaks")]
		bool DisplaysPageBreaks { get; set; }

		[Export ("displayBox")]
		PdfDisplayBox DisplayBox { get; set; }

		[Export ("displaysAsBook")]
		bool DisplaysAsBook { get; set; }

		[Export ("shouldAntiAlias")]
		bool ShouldAntiAlias { get; set; }

		[Export ("greekingThreshold")]
		float GreekingThreshold { get; set; }

		[Export ("takeBackgroundColorFrom:")]
		void TakeBackgroundColor (NSObject sender);

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		PdfViewDelegate Delegate { get; set; }

		[Export ("scaleFactor")]
		float ScaleFactor { get; set; }

		[Export ("zoomIn:")]
		void ZoomIn (NSObject sender);

		[Export ("canZoomIn")]
		bool CanZoomIn { get; }

		[Export ("zoomOut:")]
		void ZoomOut (NSObject sender);

		[Export ("canZoomOut")]
		bool CanZoomOut { get; }

		[Export ("autoScales")]
		bool AutoScales { get; set; }

		[Export ("areaOfInterestForMouse:")]
		PdfAreaOfInterest GetAreaOfInterest (NSEvent mouseEvent);

		[Export ("setCursorForAreaOfInterest:")]
		void SetCursor (PdfAreaOfInterest area);

		[Export ("performAction:")]
		void PerformAction (PdfAction action);

		[Export ("currentSelection")]
		PdfSelection CurrentSelection { get; set; }

		[Export ("setCurrentSelection:animate:")]
		void SetCurrentSelection (PdfSelection selection, bool animate);

		[Export ("clearSelection")]
		void ClearSelection ();

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("scrollSelectionToVisible:")]
		void ScrollSelectionToVisible (NSObject sender);
	
		// Verify NSArray
		[Export ("highlightedSelections")]
		PdfSelection [] HighlightedSelections { get; set; }

		[Export ("takePasswordFrom:")]
		void TakePasswordFrom (NSObject sender);

		[Export ("drawPage:")]
		void DrawPage (PdfPage page);

		[Export ("drawPagePost:")]
		void DrawPagePost (PdfPage page);

		[Export ("copy:")]
		void Copy (NSObject sender);

		[Export ("printWithInfo:autoRotate:")]
		void Print (NSPrintInfo printInfo, bool doRotate);

		[Export ("printWithInfo:autoRotate:pageScaling:")]
		void Print (NSPrintInfo printInfo, bool doRotate, PdfPrintScalingMode scaleMode);

		[Export ("pageForPoint:nearest:")]
		PdfPage GetPage (PointF point, bool nearest);

		[Export ("convertPoint:toPage:")]
		PointF ConvertPointToPage (PointF point, PdfPage page);

		[Export ("convertRect:toPage:")]
		RectangleF ConvertRectangleToPage (RectangleF rect, PdfPage page);

		[Export ("convertPoint:fromPage:")]
		PointF ConvertPointFromPage (PointF point, PdfPage page);

		[Export ("convertRect:fromPage:")]
		RectangleF ConvertRectangleFromPage (RectangleF rect, PdfPage page);

		[Export ("documentView")]
		NSView DocumentView { get; }

		[Export ("layoutDocumentView")]
		void LayoutDocumentView ();

		[Export ("annotationsChangedOnPage:")]
		void AnnotationsChanged (PdfPage page);

		[Export ("rowSizeForPage:")]
		SizeF RowSize (PdfPage page);

		[Export ("allowsDragging")]
		bool AllowsDragging { get; set; }
	
		//Verify NSArray
		[Export ("visiblePages")]
		PdfPage [] VisiblePages { get; }

		[Export ("enableDataDetectors")]
		bool EnableDataDetectors { get; set; }

		[Field("PDFViewChangedHistoryNotification")]
		[Notification]
		NSString ChangedHistoryNotification { get; }

		[Field("PDFViewDocumentChangedNotification")]
		[Notification]
		NSString DocumentChangedNotification { get; }

		[Field ("PDFViewPageChangedNotification")]
		[Notification]
		NSString PageChangedNotification { get; }

		[Field ("PDFViewScaleChangedNotification")]
		[Notification]
		NSString ScaleChangedNotification { get; }

		[Field ("PDFViewAnnotationHitNotification")]
		[Notification (typeof (PdfViewAnnotationHitEventArgs))]
		NSString AnnotationHitNotification { get; }

		[Field ("PDFViewCopyPermissionNotification")]
		[Notification]
		NSString CopyPermissionNotification { get; }

		[Field ("PDFViewAnnotationWillHitNotification")]
		[Notification]
		NSString AnnotationWillHitNotification { get; }

		[Field ("PDFViewSelectionChangedNotification")]
		[Notification]
		NSString SelectionChangedNotification { get; }

		[Field ("PDFViewDisplayModeChangedNotification")]
		[Notification]
		NSString DisplayModeChangedNotification { get; }

		[Field ("PDFViewDisplayBoxChangedNotification")]
		[Notification]
		NSString DisplayBoxChangedNotification { get; }
	}
	
	public interface PdfViewAnnotationHitEventArgs {
		[Export ("PDFAnnotationHit")]
		PdfAnnotation AnnotationHit { get; }
	}
	
	//Verify delegate methods.  There are default actions (not just return null ) that should occur
	//if the delegate does not implement the method.
	[BaseType (typeof (NSObject))]
	[Model]
	public interface PdfViewDelegate {
		//from docs: 'By default, the scale factor is restricted to a range between 0.1 and 10.0 inclusive.'
		[Export ("PDFViewWillChangeScaleFactor:toScale:"), DelegateName ("PdfViewScale"), DefaultValueFromArgument ("scale")]
		float WillChangeScaleFactor (PdfView sender, float scale);

		[Export ("PDFViewWillClickOnLink:withURL:"), EventArgs ("PdfViewUrl")]
		void WillClickOnLink (PdfView sender, NSUrl url);

		// from the docs: 'By default, this method uses the string, if any, associated with the
		// 'Title' key in the view's PDFDocument attribute dictionary. If there is no such string,
		// this method uses the last path component if the document is URL-based.
		[Export ("PDFViewPrintJobTitle:"), DelegateName ("PdfViewTitle"), DefaultValue ("String.Empty")]
		string TitleOfPrintJob (PdfView sender);

		[Export ("PDFViewPerformFind:"), EventArgs ("PdfView")]
		void PerformFind (PdfView sender);

		[Export ("PDFViewPerformGoToPage:"), EventArgs ("PdfView")]
		void PerformGoToPage (PdfView sender);

		[Export ("PDFViewPerformPrint:"), EventArgs ("PdfView")]
		void PerformPrint (PdfView sender);

		[Export ("PDFViewOpenPDF:forRemoteGoToAction:"), EventArgs ("PdfViewAction")]
		void OpenPdf (PdfView sender, PdfActionRemoteGoTo action);
	}

}
