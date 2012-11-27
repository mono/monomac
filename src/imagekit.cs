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
// imagekit.cs: Bindings for the Image Kit API
//
using System;
using System.Drawing;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreImage;
//using MonoMac.ImageCaptureCore;
using MonoMac.CoreGraphics;
using MonoMac.CoreAnimation;

namespace MonoMac.ImageKit {

	[BaseType (typeof (NSView), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (IKCameraDeviceViewDelegate)})]
	public interface IKCameraDeviceView {
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IKCameraDeviceViewDelegate Delegate { get; set; }
		
//		FIXME need ImageCaptureCore;
//		[Export ("cameraDevice", ArgumentSemantic.Assign)]
//		ICCameraDevice CameraDevice { get; set;  }

		[Export ("hasDisplayModeTable")]
		bool HasDisplayModeTable { get; set;  }

		[Export ("hasDisplayModeIcon")]
		bool HasDisplayModeIcon { get; set;  }

		[Export ("downloadAllControlLabel", ArgumentSemantic.Copy)]
		string DownloadAllControlLabel { get; set;  }

		[Export ("downloadSelectedControlLabel", ArgumentSemantic.Copy)]
		string DownloadSelectedControlLabel { get; set;  }

		[Export ("iconSize")]
		int IconSize { get; set;  }

		[Export ("transferMode")]
		IKCameraDeviceViewTransferMode TransferMode { get; set;  }

		[Export ("displaysDownloadsDirectoryControl")]
		bool DisplaysDownloadsDirectoryControl { get; set;  }

		[Export ("downloadsDirectory", ArgumentSemantic.Retain)]
		NSUrl DownloadsDirectory { get; set;  }

		[Export ("displaysPostProcessApplicationControl")]
		bool DisplaysPostProcessApplicationControl { get; set;  }

		[Export ("postProcessApplication", ArgumentSemantic.Retain)]
		NSUrl PostProcessApplication { get; set;  }

		[Export ("canRotateSelectedItemsLeft")]
		bool CanRotateSelectedItemsLeft { get;  }

		[Export ("canRotateSelectedItemsRight")]
		bool CanRotateSelectedItemsRight { get;  }

		[Export ("canDeleteSelectedItems")]
		bool CanDeleteSelectedItems { get;  }

		[Export ("canDownloadSelectedItems")]
		bool CanDownloadSelectedItems { get;  }

		[Export ("selectedIndexes")]
		NSIndexSet SelectedIndexes { get;  }

		[Export ("selectIndexes:byExtendingSelection:")]
		void SelectItemsAt (NSIndexSet indexes, bool extendSelection);

		[Export ("rotateLeft:")]
		void RotateLeft (NSObject sender);

		[Export ("rotateRight:")]
		void RotateRight (NSObject sender);

		[Export ("deleteSelectedItems:")]
		void DeleteSelectedItems (NSObject sender);

		[Export ("downloadSelectedItems:")]
		void DownloadSelectedItems (NSObject sender);

		[Export ("downloadAllItems:")]
		void DownloadAllItems (NSObject sender);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKCameraDeviceViewDelegate {
		[Export ("cameraDeviceViewSelectionDidChange:"), EventArgs ("IKCameraDeviceView")]
		void SelectionDidChange (IKCameraDeviceView cameraDeviceView);

//		FIXME need ImageCaptureCore;
//		[Export ("cameraDeviceView:didDownloadFile:location:fileData:error:"), EventArgs ("IKCameraDeviceViewICCameraFileNSUrlNSDataNSError")]
//		void DidDownloadFile (IKCameraDeviceView cameraDeviceView, ICCameraFile file, NSUrl url, NSData data, NSError error);

		[Export ("cameraDeviceView:didEncounterError:"), EventArgs ("IKCameraDeviceViewNSError")]
		void DidEncounterError (IKCameraDeviceView cameraDeviceView, NSError error);
	}

	[BaseType (typeof (NSView), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (IKDeviceBrowserViewDelegate)})]
	public interface IKDeviceBrowserView {
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IKDeviceBrowserViewDelegate Delegate { get; set; }
		
		[Export ("displaysLocalCameras")]
		bool DisplaysLocalCameras { get; set;  }

		[Export ("displaysNetworkCameras")]
		bool DisplaysNetworkCameras { get; set;  }

		[Export ("displaysLocalScanners")]
		bool DisplaysLocalScanners { get; set;  }

		[Export ("displaysNetworkScanners")]
		bool DisplaysNetworkScanners { get; set;  }

		[Export ("mode")]
		IKDeviceBrowserViewDisplayMode Mode { get; set;  }

//		FIXME need ImageCaptureCore;
//		[Export ("selectedDevice")]
//		ICDevice SelectedDevice { get;  }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKDeviceBrowserViewDelegate {
//		FIXME need ImageCaptureCore;
//		[Abstract]
//		[Export ("deviceBrowserView:selectionDidChange:"), EventArgs ("IKDeviceBrowserViewICDevice")]
//		void SelectionDidChange (IKDeviceBrowserView deviceBrowserView, ICDevice device);

		[Export ("deviceBrowserView:didEncounterError:"), EventArgs ("IKDeviceBrowserViewNSError")]
		void DidEncounterError (IKDeviceBrowserView deviceBrowserView, NSError error);
	}

	[BaseType (typeof (NSPanel))]
	public interface IKFilterBrowserPanel {
		[Export ("filterBrowserPanelWithStyleMask:")]
		IntPtr Constructor (IKFilterBrowserPanelStyleMask styleMask);

		[Export ("filterName")]
		string FilterName { get; }
		
		//FIXME - can we do this in a more C#ish way.
		[Export ("beginWithOptions:modelessDelegate:didEndSelector:contextInfo:")]
		void Begin (NSDictionary options, NSObject modelessDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("beginSheetWithOptions:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSDictionary options, NSWindow docWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithOptions:")]
		int RunModal (NSDictionary options);

		[Export ("filterBrowserViewWithOptions:")]
		IKFilterBrowserView FilterBrowserView (NSDictionary options);

		[Export ("finish:")]
		void Finish (NSObject sender);

		//Check - Do we need Notifications strings?
		[Field ("IKFilterBrowserFilterSelectedNotification")]
		NSString FilterSelectedNotification { get; }

		[Field ("IKFilterBrowserFilterDoubleClickNotification")]
		NSString FilterDoubleClickNotification { get; }

		[Field ("IKFilterBrowserWillPreviewFilterNotification")]
		NSString WillPreviewFilterNotification { get; }

		//Dictionary Keys
		[Field ("IKFilterBrowserShowCategories")]
		NSString ShowCategories { get; }

		[Field ("IKFilterBrowserShowPreview")]
		NSString ShowPreview { get; }

		[Field ("IKFilterBrowserExcludeCategories")]
		NSString ExcludeCategories { get; }

		[Field ("IKFilterBrowserExcludeFilters")]
		NSString ExcludeFilters { get; }

		[Field ("IKFilterBrowserDefaultInputImage")]
		NSString DefaultInputImage { get; }
	}

	[BaseType (typeof (NSView))]
	public interface IKFilterBrowserView {
		[Export ("setPreviewState:")]
		void SetPreviewState (bool showPreview);

		[Export ("filterName")]
		string FilterName { get; }
	}

	//This protocol is an addition to CIFilter.  It is implemented by any filter that provides its own user interface.
	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKFilterCustomUIProvider {
		[Abstract]
		[Export ("provideViewForUIConfiguration:excludedKeys:")]
		IKFilterUIView GetFilterUIView (NSDictionary configurationOptions, NSArray excludedKeys);

		//UIConfiguration keys for NSDictionary
		[Field ("IKUISizeFlavor")]
		NSString SizeFlavor { get; }
		
		[Field ("IKUISizeMini")]
		NSString SizeMini { get; }

		[Field ("IKUISizeSmall")]
		NSString SizeSmall { get; }

		[Field ("IKUISizeRegular")]
		NSString SizeRegular { get; }

		[Field ("IKUImaxSize")]
		NSString MaxSize { get; }

		[Field ("IKUIFlavorAllowFallback")]
		NSString FlavorAllowFallback { get; }
	}

	[BaseType (typeof (NSView))]
	public interface IKFilterUIView {
		[Export ("initWithFrame:filter:")]
		IntPtr Constructor (RectangleF frame, CIFilter filter);

		//This is an extension to CIFilter
		[Export ("viewForUIConfiguration:excludedKeys:")]
		IKFilterUIView GetFilterUIView ([Target] CIFilter filter, NSDictionary configurationOptions, [NullAllowed] NSArray excludedKeys);

		[Export ("filter")]
		CIFilter Filter { get; }

		[Export ("objectController")]
		NSObjectController ObjectController { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface IKImageBrowserCell {
		[Export ("imageBrowserView")]
		IKImageBrowserView ImageBrowserView  { get; }

		[Export ("representedItem")]
		NSObject RepresentedItem  { get; }

		[Export ("indexOfRepresentedItem")]
		int IndexOfRepresentedItem  { get; }

		[Export ("frame")]
		RectangleF Frame  { get; }

		[Export ("imageContainerFrame")]
		RectangleF ImageContainerFrame  { get; }

		[Export ("imageFrame")]
		RectangleF ImageFrame  { get; }

		[Export ("selectionFrame")]
		RectangleF SelectionFrame  { get; }

		[Export ("titleFrame")]
		RectangleF TitleFrame  { get; }

		[Export ("subtitleFrame")]
		RectangleF SubtitleFrame  { get; }

		[Export ("imageAlignment")]
		NSImageAlignment ImageAlignment  { get; }

		[Export ("isSelected")]
		bool IsSelected  { get; }

		[Export ("cellState")]
		IKImageBrowserCellState CellState  { get; }

		[Export ("opacity")]
		float Opacity  { get; }

		[Export ("layerForType:")]
		CALayer Layer (string layerType);

		// layerType is one of the following
		[Field ("IKImageBrowserCellBackgroundLayer")]
		NSString BackgroundLayer { get; }

		[Field ("IKImageBrowserCellForegroundLayer")]
		NSString ForegroundLayer { get; }

		[Field ("IKImageBrowserCellSelectionLayer")]
		NSString SelectionLayer { get; }

		[Field ("IKImageBrowserCellPlaceHolderLayer")]
		NSString PlaceHolderLayer { get; }
	}

	[BaseType (typeof (NSView), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (IKImageBrowserDelegate)})]
	public interface IKImageBrowserView {
		//@category IKImageBrowserView (IKMainMethods)
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frame);

		//Having a weak and strong datasource seems to work.
		[Export ("dataSource", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		IKImageBrowserDataSource DataSource { get; set; }

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IKImageBrowserDelegate Delegate { get; set; }

		//@category IKImageBrowserView (IKAppearance)
		[Export ("cellsStyleMask")]
		IKCellsStyle CellsStyleMask { get; set; }

		[Export ("constrainsToOriginalSize")]
		bool ConstrainsToOriginalSize { get; set; }

		[Export ("backgroundLayer")]
		CALayer BackgroundLayer { get; set; }

		[Export ("foregroundLayer")]
		CALayer ForegroundLayer { get; set; }

		[Export ("newCellForRepresentedItem:")]
		IKImageBrowserCell NewCell (IKImageBrowserItem representedItem);

		[Export ("cellForItemAtIndex:")]
		IKImageBrowserCell GetCellAt (int itemIndex);

		//@category IKImageBrowserView (IKBrowsing)
		[Export ("zoomValue")]
		float ZoomValue { get; set; }

		[Export ("contentResizingMask")]
		NSViewResizingMask ContentResizingMask  { get; set; }

		[Export ("scrollIndexToVisible:")]
		void ScrollIndexToVisible (int index);

		[Export ("cellSize")]
		SizeF CellSize  { get; set; }

		[Export ("intercellSpacing")]
		SizeF IntercellSpacing { get; set; }

		[Export ("indexOfItemAtPoint:")]
		int GetIndexOfItem (PointF point);

		[Export ("itemFrameAtIndex:")]
		RectangleF GetItemFrame (int index);

		[Export ("visibleItemIndexes")]
		NSIndexSet GetVisibleItemIndexes ();

		[Export ("rowIndexesInRect:")]
		NSIndexSet GetRowIndexes (RectangleF rect);

		[Export ("columnIndexesInRect:")]
		NSIndexSet GetColumnIndexes (RectangleF rect);

		[Export ("rectOfColumn:")]
		RectangleF GetRectOfColumn (int columnIndex);

		[Export ("rectOfRow:")]
		RectangleF GetRectOfRow (int rowIndex);

		[Export ("numberOfRows")]
		int RowCount { get; }

		[Export ("numberOfColumns")]
		int ColumnCount { get; }

		[Export ("canControlQuickLookPanel")]
		bool CanControlQuickLookPanel { get; set; }

		//@category IKImageBrowserView (IKSelectionReorderingAndGrouping)
		[Export ("selectionIndexes")]
		NSIndexSet SelectionIndexes  { get; }

		[Export ("setSelectionIndexes:byExtendingSelection:")]
		void SelectItemsAt (NSIndexSet indexes, bool extendSelection);

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection  { get; set; }

		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection  { get; set; }

		[Export ("allowsReordering")]
		bool AllowsReordering  { get; set; }

		[Export ("animates")]
		bool Animates  { get; set; }

		[Export ("expandGroupAtIndex:")]
		void ExpandGroup (int index);

		[Export ("collapseGroupAtIndex:")]
		void CollapseGroup (int index);

		[Export ("isGroupExpandedAtIndex:")]
		bool IsGroupExpanded (int index);

		//@category IKImageBrowserView (IKDragNDrop)
		[Export ("draggingDestinationDelegate")]
		NSDraggingDestination DraggingDestinationDelegate  { get; set; }

		[Export ("indexAtLocationOfDroppedItem")]
		int GetIndexAtLocationOfDroppedItem ();

		[Export ("dropOperation")]
		IKImageBrowserDropOperation DropOperation ();

		[Export ("allowsDroppingOnItems")]
		bool AllowsDroppingOnItems  { get; set; }

		[Export ("setDropIndex:dropOperation:")]
		void SetDropIndex (int index, IKImageBrowserDropOperation operation);

		// Keys for the view options, set with base.setValue
		[Field ("IKImageBrowserBackgroundColorKey")]
		NSString BackgroundColorKey { get; }
		
		[Field ("IKImageBrowserSelectionColorKey")]
		NSString SelectionColorKey { get; }
		
		[Field ("IKImageBrowserCellsOutlineColorKey")]
		NSString CellsOutlineColorKey { get; }
		
		[Field ("IKImageBrowserCellsTitleAttributesKey")]
		NSString CellsTitleAttributesKey { get; }
		
		[Field ("IKImageBrowserCellsHighlightedTitleAttributesKey")]
		NSString CellsHighlightedTitleAttributesKey { get; }
		
		[Field ("IKImageBrowserCellsSubtitleAttributesKey")]
		NSString CellsSubtitleAttributesKey { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKImageBrowserDataSource {
		[Abstract]
		[Export ("numberOfItemsInImageBrowser:")]
		int ItemCount (IKImageBrowserView aBrowser);

		[Abstract]
		[Export ("imageBrowser:itemAtIndex:")]
		IKImageBrowserItem GetItem (IKImageBrowserView aBrowser, int index);

		[Export ("imageBrowser:removeItemsAtIndexes:")]
		void RemoveItems (IKImageBrowserView aBrowser, NSIndexSet indexes);

		[Export ("imageBrowser:moveItemsAtIndexes:toIndex:")]
		bool MoveItems (IKImageBrowserView aBrowser, NSIndexSet indexes, int destinationIndex);

		[Export ("imageBrowser:writeItemsAtIndexes:toPasteboard:")]
		int WriteItemsToPasteboard (IKImageBrowserView aBrowser, NSIndexSet itemIndexes, NSPasteboard pasteboard);

		[Export ("numberOfGroupsInImageBrowser:")]
		int GroupCount (IKImageBrowserView aBrowser);

		[Export ("imageBrowser:groupAtIndex:")]
		NSDictionary GetGroup (IKImageBrowserView aBrowser, int index);

		// Keys for Dictionary returned by GetGroup
		[Field ("IKImageBrowserGroupRangeKey")]
		NSString GroupRangeKey { get; }

		[Field ("IKImageBrowserGroupBackgroundColorKey")]
		NSString GroupBackgroundColorKey { get; }

		[Field ("IKImageBrowserGroupTitleKey")]
		NSString GroupTitleKey { get; }

		[Field ("IKImageBrowserGroupStyleKey")]
		NSString GroupStyleKey { get; }

		[Field ("IKImageBrowserGroupHeaderLayer")]
		NSString GroupHeaderLayer { get; }

		[Field ("IKImageBrowserGroupFooterLayer")]
		NSString GroupFooterLayer { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKImageBrowserItem {
		[Abstract]
		[Export ("imageUID")]
		string ImageUID { get; }

		[Abstract]
		[Export ("imageRepresentationType")]
		NSString ImageRepresentationType { get; }

		//possible strings returned by ImageRepresentationType
		[Field ("IKImageBrowserPathRepresentationType")]
		NSString PathRepresentationType { get; }

		[Field ("IKImageBrowserNSURLRepresentationType")]
		NSString NSURLRepresentationType { get; }

		[Field ("IKImageBrowserNSImageRepresentationType")]
		NSString NSImageRepresentationType { get; }

		[Field ("IKImageBrowserCGImageRepresentationType")]
		NSString CGImageRepresentationType { get; }

		[Field ("IKImageBrowserCGImageSourceRepresentationType")]
		NSString CGImageSourceRepresentationType { get; }

		[Field ("IKImageBrowserNSDataRepresentationType")]
		NSString NSDataRepresentationType { get; }

		[Field ("IKImageBrowserNSBitmapImageRepresentationType")]
		NSString NSBitmapImageRepresentationType { get; }

		[Field ("IKImageBrowserQTMovieRepresentationType")]
		NSString QTMovieRepresentationType { get; }

		[Field ("IKImageBrowserQTMoviePathRepresentationType")]
		NSString QTMoviePathRepresentationType { get; }

		[Field ("IKImageBrowserQCCompositionRepresentationType")]
		NSString QCCompositionRepresentationType { get; }

		[Field ("IKImageBrowserQCCompositionPathRepresentationType")]
		NSString QCCompositionPathRepresentationType { get; }

		[Field ("IKImageBrowserQuickLookPathRepresentationType")]
		NSString QuickLookPathRepresentationType { get; }

		[Field ("IKImageBrowserIconRefPathRepresentationType")]
		NSString IconRefPathRepresentationType { get; }

		[Field ("IKImageBrowserIconRefRepresentationType")]
		NSString IconRefRepresentationType { get; }

		[Field ("IKImageBrowserPDFPageRepresentationType")]
		NSString PDFPageRepresentationType { get; }

		[Abstract]
		[Export ("imageRepresentation")]
		NSObject ImageRepresentation { get; }

		[Export ("imageVersion")]
		int ImageVersion { get; }

		[Export ("imageTitle")]
		string ImageTitle { get; }

		[Export ("imageSubtitle")]
		string ImageSubtitle { get; }

		[Export ("isSelectable")]
		bool IsSelectable { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKImageBrowserDelegate {
		[Export ("imageBrowserSelectionDidChange:"), EventArgs ("IKImageBrowserView")]
		void SelectionDidChange (IKImageBrowserView browser);

		[Export ("imageBrowser:cellWasDoubleClickedAtIndex:"), EventArgs ("IKImageBrowserViewIndex")]
		void CellWasDoubleClicked (IKImageBrowserView browser, int index);

		[Export ("imageBrowser:cellWasRightClickedAtIndex:withEvent:"), EventArgs ("IKImageBrowserViewIndexEvent")]
		void CellWasRightClicked (IKImageBrowserView browser, int index, NSEvent nsevent);

		[Export ("imageBrowser:backgroundWasRightClickedWithEvent:"), EventArgs ("IKImageBrowserViewEvent")]
		void BackgroundWasRightClicked (IKImageBrowserView browser, NSEvent nsevent);
	}

	[BaseType (typeof (NSPanel))]
	[DisableDefaultCtor] // crash when disposed, sharedImageEditPanel must be used
	public interface IKImageEditPanel {
		[Static]
		[Export ("sharedImageEditPanel")]
		IKImageEditPanel SharedPanel { get; }

		[Export ("dataSource", ArgumentSemantic.Assign), NullAllowed]
		IKImageEditPanelDataSource DataSource { get; set; }

		[Export ("filterArray")]
		NSArray filterArray { get;  }

		[Export ("reloadData")]
		void ReloadData ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKImageEditPanelDataSource {
		[Abstract]
		[Export ("image")]
		CGImage Image { get; }

		[Abstract]
		[Export ("setImage:imageProperties:")]
		void SetImageAndProperties (CGImage image, NSDictionary metaData);

		[Export ("thumbnailWithMaximumSize:")]
		CGImage GetThumbnail (SizeF maximumSize);

		[Export ("imageProperties")]
		NSDictionary ImageProperties { get; }

		[Export ("hasAdjustMode")]
		bool HasAdjustMode { get; }

		[Export ("hasEffectsMode")]
		bool HasEffectsMode { get; }

		[Export ("hasDetailsMode")]
		bool HasDetailsMode { get; }
	}

	[BaseType (typeof (NSView))]
	public interface IKImageView {
		//There is no protocol for this delegate.  used to respond to messages in the responder chain
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject Delegate { get; set; }

		[Export ("zoomFactor")]
		float ZoomFactor { get; set; }

		[Export ("rotationAngle")]
		float RotationAngle { get; set; }

		[Export ("currentToolMode")]
		string CurrentToolMode { get; set; }

		[Export ("autoresizes")]
		bool Autoresizes { get; set; }

		[Export ("hasHorizontalScroller")]
		bool HasHorizontalScroller { get; set; }

		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		[Export ("autohidesScrollers")]
		bool AutohidesScrollers { get; set; }

		[Export ("supportsDragAndDrop")]
		bool SupportsDragAndDrop { get; set; }

		[Export ("editable")]
		bool Editable { get; set; }

		[Export ("doubleClickOpensImageEditPanel")]
		bool DoubleClickOpensImageEditPanel { get; set; }

		[Export ("imageCorrection", ArgumentSemantic.Assign)]
		CIFilter ImageCorrection { get; set;  }

		[Export ("backgroundColor", ArgumentSemantic.Assign)]
		NSColor BackgroundColor { get; set;  }

		[Export ("setImage:imageProperties:")]
		void SetImageimageProperties (CGImage image, NSDictionary metaData);

		[Export ("setImageWithURL:")]
		void SetImageWithURL (NSUrl url);

		[Export ("image")]
		CGImage Image { get; }

		[Export ("imageSize")]
		SizeF ImageSize { get; }

		[Export ("imageProperties")]
		NSDictionary ImageProperties { get; }

		[Export ("setRotationAngle:centerPoint:")]
		void SetRotation (float rotationAngle, PointF centerPoint);

		[Export ("rotateImageLeft:")]
		void RotateImageLeft (NSObject sender);

		[Export ("rotateImageRight:")]
		void RotateImageRight (NSObject sender);

		[Export ("setImageZoomFactor:centerPoint:")]
		void SetImageZoomFactor (float zoomFactor, PointF centerPoint);

		[Export ("zoomImageToRect:")]
		void ZoomImageToRect (RectangleF rect);

		[Export ("zoomImageToFit:")]
		void ZoomImageToFit (NSObject sender);

		[Export ("zoomImageToActualSize:")]
		void ZoomImageToActualSize (NSObject sender);

		[Export ("zoomIn:")]
		void ZoomIn (NSObject sender);

		[Export ("zoomOut:")]
		void ZoomOut (NSObject sender);

		[Export ("flipImageHorizontal:")]
		void FlipImageHorizontal (NSObject sender);

		[Export ("flipImageVertical:")]
		void FlipImageVertical (NSObject sender);

		[Export ("crop:")]
		void Crop (NSObject sender);

		[Export ("setOverlay:forType:")]
		void SetOverlay (CALayer layer, string layerType);

		[Export ("overlayForType:")]
		CALayer GetOverlay (string layerType);

		[Export ("scrollToPoint:")]
		void ScrollTo (PointF point);

		[Export ("scrollToRect:")]
		void ScrollTo (RectangleF rect);

		[Export ("convertViewPointToImagePoint:")]
		PointF ConvertViewPointToImagePoint (PointF viewPoint);

		[Export ("convertViewRectToImageRect:")]
		RectangleF ConvertViewRectToImageRect (RectangleF viewRect);

		[Export ("convertImagePointToViewPoint:")]
		PointF ConvertImagePointToViewPoint (PointF imagePoint);

		[Export ("convertImageRectToViewRect:")]
		RectangleF ConvertImageRectToViewRect (RectangleF imageRect);
	}

	[BaseType (typeof (NSPanel))]
	public interface IKPictureTaker {
		[Static]
		[Export ("pictureTaker")]
		IKPictureTaker SharedPictureTaker { get; }

		[Export ("runModal")]
		int RunModal ();

		//FIXME - Yuck.  What can I do to fix these three methods?
		[Export ("beginPictureTakerWithDelegate:didEndSelector:contextInfo:")]
		void BeginPictureTaker (NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("beginPictureTakerSheetForWindow:withDelegate:didEndSelector:contextInfo:")]
		void BeginPictureTakerSheet (NSWindow aWindow, NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("popUpRecentsMenuForView:withDelegate:didEndSelector:contextInfo:")]
		void PopUpRecentsMenu (NSView aView, NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("inputImage")]
		NSImage InputImage { get; set; }

		[Export ("outputImage")]
		NSImage GetOutputImage ();

		[Export ("mirroring")]
		bool Mirroring { get; set; }

		//Use with NSKeyValueCoding to customize the pictureTaker panel
		[Field ("IKPictureTakerAllowsVideoCaptureKey")]
		NSString AllowsVideoCaptureKey { get; }
		
		[Field ("IKPictureTakerAllowsFileChoosingKey")]
		NSString AllowsFileChoosingKey { get; }
		
		[Field ("IKPictureTakerShowRecentPictureKey")]
		NSString ShowRecentPictureKey { get; }
		
		[Field ("IKPictureTakerUpdateRecentPictureKey")]
		NSString UpdateRecentPictureKey { get; }

		[Field ("IKPictureTakerAllowsEditingKey")]
		NSString AllowsEditingKey { get; }

		[Field ("IKPictureTakerShowEffectsKey")]
		NSString ShowEffectsKey { get; }

		[Field ("IKPictureTakerInformationalTextKey")]
		NSString InformationalTextKey { get; }

		[Field ("IKPictureTakerImageTransformsKey")]
		NSString ImageTransformsKey { get; }

		[Field ("IKPictureTakerOutputImageMaxSizeKey")]
		NSString OutputImageMaxSizeKey { get; }

		[Field ("IKPictureTakerCropAreaSizeKey")]
		NSString CropAreaSizeKey { get; }

		[Field ("IKPictureTakerShowAddressBookPictureKey")]
		NSString ShowAddressBookPictureKey { get; }

		[Field ("IKPictureTakerShowEmptyPictureKey")]
		NSString ShowEmptyPictureKey { get; }

		[Field ("IKPictureTakerRemainOpenAfterValidateKey")]
		NSString RemainOpenAfterValidateKey { get; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (IKSaveOptionsDelegate)})]
	public interface IKSaveOptions {
		[Export ("imageProperties")]
		NSDictionary ImageProperties { get;  }

		[Export ("imageUTType")]
		string ImageUTType { get;  }

		[Export ("userSelection")]
		NSDictionary UserSelection { get;  }

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IKSaveOptionsDelegate Delegate { get; set; }

		[Export ("initWithImageProperties:imageUTType:")]
		IntPtr Constructor (NSDictionary imageProperties, string imageUTType);

		[Export ("addSaveOptionsAccessoryViewToSavePanel:")]
		void AddSaveOptionsToPanel (NSSavePanel savePanel);

		[Export ("addSaveOptionsToView:")]
		void AddSaveOptionsToView (NSView view);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKSaveOptionsDelegate {
		[Export ("saveOptions:shouldShowUTType:"), DelegateName ("SaveOptionsShouldShowUTType"), DefaultValue (false)]
		bool ShouldShowType (IKSaveOptions saveOptions, string imageUTType);
	}

	[BaseType (typeof (NSView), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (IKScannerDeviceViewDelegate)})]
	public interface IKScannerDeviceView {
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IKScannerDeviceViewDelegate Delegate { get; set; }

//		FIXME need ImageCaptureCore;
//		[Export ("scannerDevice", ArgumentSemantic.Assign)]
//		ICScannerDevice ScannerDevice { get; set; }

		[Export ("mode")]
		IKScannerDeviceViewDisplayMode DisplayMode { get; set; }

		[Export ("hasDisplayModeSimple")]
		bool HasDisplayModeSimple { get; set; }

		[Export ("hasDisplayModeAdvanced")]
		bool HasDisplayModeAdvanced { get; set; }

		[Export ("transferMode")]
		IKScannerDeviceViewTransferMode TransferMode { get; set; }

		[Export ("scanControlLabel", ArgumentSemantic.Copy)]
		string ScanControlLabel { get; set; }

		[Export ("overviewControlLabel", ArgumentSemantic.Copy)]
		string OverviewControlLabel { get; set; }

		[Export ("displaysDownloadsDirectoryControl")]
		bool DisplaysDownloadsDirectoryControl { get; set; }

		[Export ("downloadsDirectory", ArgumentSemantic.Retain)]
		NSUrl DownloadsDirectory { get; set; }

		[Export ("documentName", ArgumentSemantic.Copy)]
		string DocumentName { get; set; }

		[Export ("displaysPostProcessApplicationControl")]
		bool DisplaysPostProcessApplicationControl { get; set; }

		[Export ("postProcessApplication", ArgumentSemantic.Retain)]
		NSUrl PostProcessApplication { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKScannerDeviceViewDelegate {
		[Export ("scannerDeviceView:didScanToURL:fileData:error:"), EventArgs ("IKScannerDeviceViewScan")]
		void DidScan (IKScannerDeviceView scannerDeviceView, NSUrl url, NSData data, NSError error);

		[Export ("scannerDeviceView:didEncounterError:"), EventArgs ("IKScannerDeviceViewError")]
		void DidEncounterError (IKScannerDeviceView scannerDeviceView, NSError error);
	}

	[BaseType (typeof (NSObject))]
	public interface IKSlideshow {
		[Static]
		[Export ("sharedSlideshow")]
		IKSlideshow SharedSlideshow { get; }

		[Export ("autoPlayDelay")]
		double autoPlayDelay { get; set; }

		[Export ("runSlideshowWithDataSource:inMode:options:")]
		void RunSlideshow (IKSlideshowDataSource dataSource, string slideshowMode, NSDictionary slideshowOptions);

		[Export ("stopSlideshow:")]
		void StopSlideshow (NSObject sender);

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("reloadSlideshowItemAtIndex:")]
		void ReloadSlideshowItem (int index);

		[Export ("indexOfCurrentSlideshowItem")]
		int IndexOfCurrentSlideshowItem { get; }

		[Static]
		[Export ("canExportToApplication:")]
		bool CanExportToApplication (string applicationBundleIdentifier);

		[Static]
		[Export ("exportSlideshowItem:toApplication:")]
		void ExportSlideshowItemtoApplication (NSObject item, string applicationBundleIdentifier);

		[Field ("IKSlideshowModeImages")]
		NSString ModeImages { get; }

		[Field ("IKSlideshowModePDF")]
		NSString ModePDF { get; }

		[Field ("IKSlideshowModeOther")]
		NSString ModeOther { get; }
		
		[Field ("IKSlideshowWrapAround")]
		NSString WrapAround { get; }

		[Field ("IKSlideshowStartPaused")]
		NSString StartPaused { get; }

		[Field ("IKSlideshowStartIndex")]
		NSString StartIndex { get; }

		[Field ("IKSlideshowScreen")]
		NSString Screen { get; }

		[Field ("IKSlideshowAudioFile")]
		NSString AudioFile { get; }

		[Field ("IKSlideshowPDFDisplayBox")]
		NSString PDFDisplayBox { get; }

		[Field ("IKSlideshowPDFDisplayMode")]
		NSString PDFDisplayMode { get; }

		[Field ("IKSlideshowPDFDisplaysAsBook")]
		NSString PDFDisplaysAsBook { get; }

		[Field ("IK_iPhotoBundleIdentifier")]
		NSString IPhotoBundleIdentifier { get; }

		[Field ("IK_ApertureBundleIdentifier")]
		NSString ApertureBundleIdentifier { get; }

		[Field ("IK_MailBundleIdentifier")]
		NSString MailBundleIdentifier { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface IKSlideshowDataSource {
		[Abstract]
		[Export ("numberOfSlideshowItems")]
		int ItemCount { get; }

		[Abstract]
		[Export ("slideshowItemAtIndex:")]
		NSObject GetItemAt (int index);

		[Export ("nameOfSlideshowItemAtIndex:")]
		string GetNameOfItemAt (int index);

		[Export ("canExportSlideshowItemAtIndex:toApplication:")]
		bool CanExportItemToApplication (int index, string applicationBundleIdentifier);

		[Export ("slideshowWillStart")]
		void WillStart ();

		[Export ("slideshowDidStop")]
		void DidStop ();

		[Export ("slideshowDidChangeCurrentIndex:")]
		void DidChange (int newIndex);
	}
}
