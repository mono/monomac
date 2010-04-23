/
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

//
// appkit.cs: Definitions for AppKit
//
using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;
using MonoMac.CoreImage;
using MonoMac.CoreAnimation;

namespace MonoMac.AppKit {

	[BaseType (typeof (NSObject))]
	interface CIImage {
		[Export ("drawInRect:fromRect:operation:fraction:")]
		void Draw (RectangleF inRect, RectangleF fromRect, NSCompositingOperation operation, float fractionDelta);

		[Export ("drawAtPoint:fromRect:operation:fraction:")]
		void DrawAtPoint (PointF atPoint, RectangleF fromRect, NSCompositingOperation operation, float fractionDelta);

	}
	
	[BaseType (typeof (NSCell))]
	interface NSActionCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);
	
		[Export ("target")]
		NSObject Target  { get; set; }
	
		[Export ("action")]
		Selector Action  { get; set; }
	
		[Export ("tag")]
		int Tag  { get; set; }
	
	}

	[BaseType (typeof (NSObject))]
	interface NSAffineTransform {
		[Export ("transformBezierPath:")]
		NSBezierPath TransformBezierPath (NSBezierPath path);

		[Export ("set")]
		void Set ();

		[Export ("concat")]
		void Concat ();
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSAnimationDelegate)})]
	interface NSAnimation {
		// Constant: NSAnimationProgressMarkNotification
		// Constant: NSAnimationProgressMark
		// Constant: NSAnimationTriggerOrderIn
		// Constant: NSAnimationTriggerOrderOut
		[Export ("initWithDuration:animationCurve:")]
		IntPtr Constant (double duration, NSAnimationCurve animationCurve);
	
		[Export ("startAnimation")]
		void StartAnimation ();
	
		[Export ("stopAnimation")]
		void StopAnimation ();
	
		[Export ("isAnimating")]
		bool IsAnimating ();
	
		[Export ("currentProgress")]
		float CurrentProgress { get; set; }
	
		[Export ("duration")]
		double Duration  { get; set; }
	
		[Export ("animationBlockingMode")]
		NSAnimationBlockingMode AnimationBlockingMode  { get; set; }
	
		[Export ("frameRate")]
		float FrameRate  { get; set; }
	
		[Export ("animationCurve")]
		NSAnimationCurve AnimationCurve  { get; set; }
	
		[Export ("currentValue")]
		float CurrentValue { get; }
	
		[Export ("delegate")]
		NSAnimationDelegate Delegate  { get; set; }
	
		[Export ("progressMarks")]
		NSNumber [] ProgressMarks  { get; set; }
	
		[Export ("addProgressMark:")]
		void AddProgressMark (float progressMark);
	
		[Export ("removeProgressMark:")]
		void RemoveProgressMark (float progressMark);
	
		[Export ("startWhenAnimation:reachesProgress:")]
		void StartWhenAnimationReaches (NSAnimation animation, float startProgress);
	
		[Export ("stopWhenAnimation:reachesProgress:")]
		void StopWhenAnimationReaches (NSAnimation animation, float stopProgress);
	
		[Export ("clearStartAnimation")]
		void ClearStartAnimation ();
	
		[Export ("clearStopAnimation")]
		void ClearStopAnimation ();

		// TODO
		//[Export ("runLoopModesForAnimating")]
		//NSArray* runLoopModesForAnimating ();
	
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NSAnimationDelegate {
		[Export ("animationShouldStart:"), EventArgs ("NSAnimationPredicate"), DefaultValue (true)]
		bool AnimationShouldStart (NSAnimation animation);
	
		[Export ("animationDidStop:"), EventArgs ("NSAnimation")]
		void AnimationDidStop (NSAnimation animation);
	
		[Export ("animationDidEnd:"), EventArgs ("NSAnimation")]
		void AnimationDidEnd (NSAnimation animation);
	
		[Export ("animation:valueForProgress:"), EventArgs ("NSAnimationProgress"), DefaultValue (0.0)]
		float AnimationProgress (NSAnimation animation, float progress);
	
		[Export ("animation:didReachProgressMark:"), EventArgs ("NSAnimation")]
		void AnimationDidReachProgressMark (NSAnimation animation, float progress);
	}

	[BaseType (typeof (NSObject))]
	interface NSAnimationContext {
		[Export ("beginGrouping")]
		void BeginGrouping ();

		[Static]
		[Export ("endGrouping")]
		void EndGrouping ();

		[Static]
		[Export ("currentContext")]
		NSAnimationContext CurrentContext { get; }

		//Detected properties
		[Export ("duration")]
		double Duration { get; set; }
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSAlertDelegate)})]
	interface NSAlert {
		[Static, Export ("alertWithError:")]
		NSAlert WithError (NSError  error);
	
		[Static, Export ("alertWithMessageText:defaultButton:alternateButton:otherButton:informativeTextWithFormat:"), Internal]
		NSAlert WithMessage (string  message, string  defaultButton, string  alternateButton, string  otherButton, string full);
	
		[Export ("messageText")]
		string MessageText { get; set; }
	
		[Export ("informativeText")]
		string InformativeText { get; set; }
	
		[Export ("icon")]
		NSImage Icon { get; set; }
	
		[Export ("addButtonWithTitle:")]
		NSButton AddButton (string title);
	
		[Export ("buttons")]
		NSButton [] Buttons { get; }
	
		[Export ("showsHelp")]
		bool ShowsHelp { get; set; }
	
		[Export ("helpAnchor")]
		string HelpAnchor { get; set; }
	
		[Export ("alertStyle")]
		NSAlertStyle AlertStyle { get; set; }
	
		[Export ("delegate")]
		NSAlertDelegate Delegate { get; set; } 
	
		[Export ("showsSuppressionButton")]
		bool ShowsSuppressionButton { get; set; } 
	
		[Export ("suppressionButton")]
		NSButton SuppressionButton { get; } 
	
		[Export ("accessoryView")]
		NSView AccessoryView { get; set; } 
	
		[Export ("layout")]
		void Layout ();
	
		[Export ("runModal")]
		int RunModal ();
	
		[Export ("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSWindow  window, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);
	
		[Export ("window")]
		NSObject Window  { get; }
	
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NSAlertDelegate {
		[Export ("alertShowHelp:"), EventArgs ("NSAlertPredicate"), DefaultValue (false)]
		bool ShowHelp (NSAlert  alert);
	}

	[BaseType (typeof (NSResponder), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSApplicationDelegate) })]
	interface NSApplication {
		[Export ("sharedApplication"), Static]
		NSApplication SharedApplication { get; }
	
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSApplicationDelegate Delegate { get; set; }
	
		[Export ("context")]
		NSGraphicsContext Context { get; }
	
		[Export ("hide:")]
		void Hide (NSObject sender);
	
		[Export ("unhide:")]
		void Unhide (NSObject sender);
	
		[Export ("unhideWithoutActivation")]
		void UnhideWithoutActivation ();
	
		[Export ("windowWithWindowNumber:")]
		NSWindow WindowWithWindowNumber (int windowNum);
	
		[Export ("mainWindow")]
		NSWindow MainWindow { get; }
	
		[Export ("keyWindow")]
		NSWindow KeyWindow { get; }
	
		[Export ("isActive")]
		bool Active { get; }
	
		[Export ("isHidden")]
		bool Hidden { get; }
	
		[Export ("isRunning")]
		bool Running { get; }
	
		[Export ("deactivate")]
		void Deactivate ();
	
		[Export ("activateIgnoringOtherApps:")]
		void ActivateIgnoringOtherApps (bool flag);
	
		[Export ("hideOtherApplications:")]
		void HideOtherApplications (NSObject sender);
	
		[Export ("unhideAllApplications:")]
		void UnhideAllApplications (NSObject sender);
	
		[Export ("finishLaunching")]
		void FinishLaunching ();
	
		[Export ("run")]
		void Run ();
	
		[Export ("runModalForWindow:")]
		int RunModalForWindow (NSWindow theWindow);
	
		[Export ("stop:")]
		void Stop (NSObject sender);
	
		[Export ("stopModal")]
		void StopModal ();
	
		[Export ("stopModalWithCode:")]
		void StopModalWithCode (int returnCode);
	
		[Export ("abortModal")]
		void AbortModal ();
	
		[Export ("modalWindow")]
		NSWindow ModalWindow { get; }
	
		[Export ("beginModalSessionForWindow:")]
		IntPtr BeginModalSession (NSWindow theWindow);
	
		[Export ("runModalSession:")]
		int RunModalSession (IntPtr session);
	
		[Export ("endModalSession:")]
		void EndModalSession (IntPtr session);
	
		[Export ("terminate:")]
		void Terminate (NSObject sender);
	
		[Export ("requestUserAttention:")]
		int RequestUserAttention (NSRequestUserAttentionType requestType);
	
		[Export ("cancelUserAttentionRequest:")]
		void CancelUserAttentionRequest (int request);
	
		[Export ("beginSheet:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSWindow sheet, NSWindow docWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);
	
		[Export ("endSheet:")]
		void EndSheet (NSWindow sheet);
	
		[Export ("endSheet:returnCode:")]
		void EndSheet (NSWindow  sheet, int returnCode);
	
		[Export ("nextEventMatchingMask:untilDate:inMode:dequeue:")]
		NSEvent NextEvent (NSEventMask mask, NSDate expiration, string mode, bool deqFlag);
	
		[Export ("discardEventsMatchingMask:beforeEvent:")]
		void DiscardEvents (NSEventMask mask, NSEvent lastEvent);
	
		[Export ("postEvent:atStart:")]
		void PostEvent (NSEvent theEvent, bool flag);
	
		[Export ("currentEvent")]
		NSEvent CurrentEvent { get; }
	
		[Export ("sendEvent:")]
		void SendEvent (NSEvent theEvent);
	
		[Export ("preventWindowOrdering")]
		void PreventWindowOrdering ();
	
		[Export ("makeWindowsPerform:inOrder:")]
		NSWindow MakeWindowsPerform (Selector aSelector, bool inOrder);
	
		[Export ("windows")]
		NSWindow [] Windows { get; }
	
		[Export ("setWindowsNeedUpdate:")]
		void SetWindowsNeedUpdate (bool needUpdate);
	
		[Export ("updateWindows")]
		void UpdateWindows ();
	
		[Export ("setMainMenu:")]
		void SetMainMenu (NSMenu  aMenu);
	
		[Export ("mainMenu")]
		NSMenu MainMenu { get; }
	
		[Export ("setHelpMenu:")]
		NSMenu HelpMenu { get; set; }
	
		[Export ("applicationIconImage")]
		NSImage applicationIconImage { get; set; }
	
		[Export ("activationPolicy")]
		NSApplicationActivationPolicy ActivationPolicy { get; set; }
	
		[Export ("dockTile")]
		NSDockTile DockTile { get; }
	
		[Export ("sendAction:to:from:")]
		bool SendAction (Selector theAction, NSObject theTarget, NSObject sender);
	
		[Export ("targetForAction:")]
		NSObject TargetForAction (Selector theAction);
	
		[Export ("targetForAction:to:from:")]
		NSObject TargetForAction (Selector theAction, NSObject theTarget, NSObject sender);
	
		[Export ("tryToPerform:with:")]
		bool TryToPerform (Selector anAction, NSObject target);
	
		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestor (string sendType, string returnType);
	
		[Export ("reportException:")]
		void ReportException (NSException theException);
	
		[Static]
		[Export ("detachDrawingThread:toTarget:withObject:")]
		void DetachDrawingThread (Selector selector, NSObject target, NSObject argument);
	
		[Export ("replyToApplicationShouldTerminate:")]
		void ReplyToApplicationShouldTerminate (bool shouldTerminate);
	
		[Export ("replyToOpenOrPrint:")]
		void ReplyToOpenOrPrint (NSApplicationDelegateReply reply);
	
		[Export ("orderFrontCharacterPalette:")]
		void OrderFrontCharacterPalette (NSObject sender);
	
		[Export ("presentationOptions")]
		NSApplicationPresentationOptions PresentationOptions { get; set; }
	
		[Export ("currentSystemPresentationOptions")]
		NSApplicationPresentationOptions CurrentSystemPresentationOptions { get; }
	
		[Export ("windowsMenu")]
		NSMenu WindowsMenu { get; set; }
	
		[Export ("arrangeInFront:")]
		void ArrangeInFront (NSObject sender);
	
		[Export ("removeWindowsItem:")]
		void RemoveWindowsItem (NSWindow  win);
	
		[Export ("addWindowsItem:title:filename:")]
		void AddWindowsItem (NSWindow  win, string title, bool isFilename);
	
		[Export ("changeWindowsItem:title:filename:")]
		void ChangeWindowsItem (NSWindow  win, string title, bool isFilename);
	
		[Export ("updateWindowsItem:")]
		void UpdateWindowsItem (NSWindow  win);
	
		[Export ("miniaturizeAll:")]
		void MiniaturizeAll (NSObject sender);
	
		[Export ("isFullKeyboardAccessEnabled")]
		bool FullKeyboardAccessEnabled { get; }

		[Export ("servicesProvider")]
		NSObject ServicesProvider { get; set; }
	
		[Export ("userInterfaceLayoutDirection")]
		NSApplicationLayoutDirection UserInterfaceLayoutDirection { get; }

		[Export ("servicesMenu")]
		NSMenu ServicesMenu { get; set; }

		// From NSColorPanel
		[Export ("orderFrontColorPanel:")]
		void OrderFrontColorPanel (NSObject sender);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NSApplicationDelegate {
		[Export ("applicationShouldTerminate:"), EventArgs ("NSApplicationTermination"), DefaultValue (NSApplicationTerminateReply.Now)]
		NSApplicationTerminateReply ApplicationShouldTerminate (NSApplication  sender);
	
		[Export ("application:openFile:"), EventArgs ("NSApplicationFile"), DefaultValue (false)]
		bool OpenFile (NSApplication sender, string  filename);
	
		[Export ("application:openFiles:"), EventArgs ("NSApplicationFiles")]
		void OpenFiles (NSApplication sender, string [] filenames);
	
		[Export ("application:openTempFile:"), EventArgs ("NSApplicationFile"), DefaultValue (false)]
		bool OpenTempFile (NSApplication sender, string  filename);
	
		[Export ("applicationShouldOpenUntitledFile:"), EventArgs ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationShouldOpenUntitledFile (NSApplication  sender);
	
		[Export ("applicationOpenUntitledFile:"), EventArgs ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationOpenUntitledFile (NSApplication sender);
	
		[Export ("application:openFileWithoutUI:"), EventArgs ("NSApplicationFileCommand"), DefaultValue (false)]
		bool OpenFileWithoutUI (NSObject sender, string filename);
	
		[Export ("application:printFile:"), EventArgs ("NSApplicationFile"), DefaultValue (false)]
		bool PrintFile (NSApplication sender, string filename);
	
		[Export ("application:printFiles:withSettings:showPrintPanels:"), EventArgs ("NSApplicationPrint"), DefaultValue (NSApplicationPrintReply.Failure)]
		NSApplicationPrintReply PrintFiles (NSApplication application, string [] fileNames, NSDictionary printSettings, bool showPrintPanels);
	
		[Export ("applicationShouldTerminateAfterLastWindowClosed:"), EventArgs ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender);
	
		[Export ("applicationShouldHandleReopen:hasVisibleWindows:"), EventArgs ("NSApplicationReopen"), DefaultValue (false)]
		bool ApplicationShouldHandleReopen (NSApplication sender, bool hasVisibleWindows);
	
		[Export ("applicationDockMenu:"), EventArgs ("NSApplicationMenu"), DefaultValue (null)]
		NSMenu ApplicationDockMenu (NSApplication sender);
	
		[Export ("application:willPresentError:"), EventArgs ("NSApplicationError"), DefaultValue (null)]
		NSError WillPresentError (NSApplication application, NSError error);
	
		[Export ("applicationWillFinishLaunching:"), EventArgs ("NSNotification")]
		void WillFinishLaunching (NSNotification notification);
	
		[Export ("applicationDidFinishLaunching:"), EventArgs ("NSNotification")]
		void DidFinishLaunching (NSNotification notification);
	
		[Export ("applicationWillHide:"), EventArgs ("NSNotification")]
		void WillHide (NSNotification notification);
	
		[Export ("applicationDidHide:"), EventArgs ("NSNotification")]
		void DidHide (NSNotification notification);
	
		[Export ("applicationWillUnhide:"), EventArgs ("NSNotification")]
		void WillUnhide (NSNotification notification);
	
		[Export ("applicationDidUnhide:"), EventArgs ("NSNotification")]
		void DidUnhide (NSNotification notification);
	
		[Export ("applicationWillBecomeActive:"), EventArgs ("NSNotification")]
		void WillBecomeActive (NSNotification notification);
	
		[Export ("applicationDidBecomeActive:"), EventArgs ("NSNotification")]
		void DidBecomeActivep (NSNotification notification);
	
		[Export ("applicationWillResignActive:"), EventArgs ("NSNotification")]
		void WillResignActive (NSNotification notification);
	
		[Export ("applicationDidResignActive:"), EventArgs ("NSNotification")]
		void DidResignActive (NSNotification notification);
	
		[Export ("applicationWillUpdate:"), EventArgs ("NSNotification")]
		void WillUpdate (NSNotification notification);
	
		[Export ("applicationDidUpdate:"), EventArgs ("NSNotification")]
		void DidUpdate (NSNotification notification);
	
		[Export ("applicationWillTerminate:"), EventArgs ("NSNotification")]
		void WillTerminate (NSNotification notification);
	
		[Export ("applicationDidChangeScreenParameters:"), EventArgs ("NSNotification")]
		void ScreenParametersChanged (NSNotification notification);

		[Export ("registerServicesMenuSendTypes:Returntypes:"), EventArgs ("NSApplicationRegister")]
		void RegisterServicesMenu (string [] sendTypes, string [] returnTypes);
	
		[Export ("writeSelectionToPasteboard:types:"), EventArgs ("NSApplicationSelection"), DefaultValue (false)]
		bool WriteSelectionToPasteboard (NSPasteboard board, string [] types);
	
		[Export ("readSelectionFromPasteboard:"), EventArgs ("NSPasteboardPredicate"), DefaultValue (false)]
		bool ReadSelectionFromPasteboard (NSPasteboard pboard);
	
		[Export ("orderFrontStandardAboutPanel:"), EventArgs ("NSObject")]
		void OrderFrontStandardAboutPanel (NSObject sender);
	
		[Export ("orderFrontStandardAboutPanelWithOptions:"), EventArgs ("NSDictionary")]
		void OrderFrontStandardAboutPanelWithOptions (NSDictionary optionsDictionary);
	}


	[BaseType (typeof (NSObject))]
	interface NSBezierPath {
		[Static]
		[Export ("bezierPath")]
		NSBezierPath CreateBezierPath ();

		[Static]
		[Export ("bezierPathWithRect:")]
		NSBezierPath FromRect (RectangleF rect);

		[Static]
		[Export ("bezierPathWithOvalInRect:")]
		NSBezierPath FromOvalInRect (RectangleF rect);

		[Static]
		[Export ("bezierPathWithRoundedRect:xRadius:yRadius:")]
		NSBezierPath BezierPathWithRoundedRect (RectangleF rect, float xRadius, float yRadius);

		[Static]
		[Export ("fillRect:")]
		void FillRect (RectangleF rect);

		[Static]
		[Export ("strokeRect:")]
		void StrokeRect (RectangleF rect);

		[Static]
		[Export ("clipRect:")]
		void ClipRect (RectangleF rect);

		[Static]
		[Export ("strokeLineFromPoint:toPoint:")]
		void StrokeLine (PointF point1, PointF point2);

		//[Static]
		//[Export ("drawPackedGlyphs:atPoint:")]
		//void DrawPackedGlyphsatPoint (IntPtr *packedGlyphs, PointF point);

		[Export ("moveToPoint:")]
		void MoveTo (PointF point);

		[Export ("lineToPoint:")]
		void LineTo (PointF point);

		[Export ("curveToPoint:controlPoint1:controlPoint2:")]
		void CurveTo (PointF endPoint, PointF controlPoint1, PointF controlPoint2);

		[Export ("closePath")]
		void ClosePath ();

		[Export ("removeAllPoints")]
		void RemoveAllPoints ();

		[Export ("relativeMoveToPoint:")]
		void RelativeMoveTo (PointF point);

		[Export ("relativeLineToPoint:")]
		void RelativeLineTo (PointF point);

		[Export ("relativeCurveToPoint:controlPoint1:controlPoint2:")]
		void RelativeCurveTo (PointF endPoint, PointF controlPoint1, PointF controlPoint2);

		//[Export ("getLineDash:count:phase:")]
		//void GetLineDashcountphase (float pattern, 

		//[Export ("setLineDash:count:phase:")]
		//void SetLineDashcountphase (float *pattern, int count, float phase);

		[Export ("stroke")]
		void Stroke ();

		[Export ("fill")]
		void Fill ();

		[Export ("addClip")]
		void AddClip ();

		[Export ("setClip")]
		void SetClip ();

		[Export ("bezierPathByFlatteningPath")]
		NSBezierPath BezierPathByFlatteningPath ();

		[Export ("bezierPathByReversingPath")]
		NSBezierPath BezierPathByReversingPath ();

		[Export ("transformUsingAffineTransform:")]
		void TransformUsingAffineTransform (NSAffineTransform transform);

		[Export ("isEmpty")]
		bool IsEmpty { get; }

		[Export ("currentPoint")]
		PointF CurrentPoint { get; }

		[Export ("controlPointBounds")]
		RectangleF ControlPointBounds { get; }

		[Export ("bounds")]
		RectangleF Bounds { get; }

		[Export ("elementCount")]
		int ElementCount { get; }

		// FIXME: marshal NSPoint as a NSPoint *, instead of an NSArray
		//[Export ("elementAtIndex:associatedPoints:")]
		//NSBezierPathElement ElementAt (int index, NSPoint [] points);

		[Export ("elementAtIndex:")]
		NSBezierPathElement ElementAt (int index);

		// FIXME: marshal
		//[Export ("setAssociatedPoints:atIndex:")]
		//void SetAssociatedPointsatIndex (NSPointArray points, int index);

		[Export ("appendBezierPath:")]
		void AppendPath (NSBezierPath path);

		[Export ("appendBezierPathWithRect:")]
		void AppendPathWithRect (RectangleF rect);

		// FIXME: marshal
		//[Export ("appendBezierPathWithPoints:count:")]
		//void AppendPathWithPoints (NSPointArray points, int count);

		[Export ("appendBezierPathWithOvalInRect:")]
		void AppendPathWithOvalInRect (RectangleF rect);

		[Export ("appendBezierPathWithArcWithCenter:radius:startAngle:endAngle:clockwise:")]
		void AppendPathWithArc (PointF center, float radius, float startAngle, float endAngle, bool clockwise);

		[Export ("appendBezierPathWithArcWithCenter:radius:startAngle:endAngle:")]
		void AppendPathWithArc (PointF center, float radius, float startAngle, float endAngle);

		[Export ("appendBezierPathWithArcFromPoint:toPoint:radius:")]
		void AppendPathWithArc (PointF point1, PointF point2, float radius);

		[Export ("appendBezierPathWithGlyph:inFont:")]
		void AppendPathWithGlyph (uint glyph, NSFont font);

		// FIXME: Marshal NSGlypy *
		//[Export ("appendBezierPathWithGlyphs:count:inFont:")]
		//void AppendPathWithGlyphs (uint *glyphs, int count, NSFont font);

		// FIXME: Marshal NSGlyph
		//[Export ("appendBezierPathWithPackedGlyphs:")]
		//void AppendPathWithPackedGlyphs (const char packedGlyphs);

		[Export ("appendBezierPathWithRoundedRect:xRadius:yRadius:")]
		void AppendPathWithRoundedRect (RectangleF rect, float xRadius, float yRadius);

		[Export ("containsPoint:")]
		bool Contains (PointF point);

		//Detected properties
		[Static]
		[Export ("defaultMiterLimit")]
		float DefaultMiterLimit { get; set; }

		[Static]
		[Export ("defaultFlatness")]
		float DefaultFlatness { get; set; }

		[Static]
		[Export ("defaultWindingRule")]
		NSWindingRule DefaultWindingRule { get; set; }

		[Static]
		[Export ("defaultLineCapStyle")]
		NSLineCapStyle DefaultLineCapStyle { get; set; }

		[Static]
		[Export ("defaultLineJoinStyle")]
		NSLineJoinStyle DefaultLineJoinStyle { get; set; }

		[Static]
		[Export ("defaultLineWidth")]
		float DefaultLineWidth { get; set; }

		[Export ("lineWidth")]
		float LineWidth { get; set; }

		[Export ("lineCapStyle")]
		NSLineCapStyle LineCapStyle { get; set; }

		[Export ("lineJoinStyle")]
		NSLineJoinStyle LineJoinStyle { get; set; }

		[Export ("windingRule")]
		NSWindingRule WindingRule { get; set; }

		[Export ("miterLimit")]
		float MiterLimit { get; set; }

		[Export ("flatness")]
		float Flatness { get; set; }
	}


	[BaseType (typeof (NSImageRep))]
	interface NSBitmapImageRep {
		[Export ("initWithFocusedViewRect:")]
		IntPtr Constructor (RectangleF rect);

		[Export ("initWithBitmapDataPlanes:pixelsWide:pixelsHigh:bitsPerSample:samplesPerPixel:hasAlpha:isPlanar:colorSpaceName:bytesPerRow:bitsPerPixel:")]
		IntPtr Constructor (IntPtr planes, int width, int height, int bps, int spp, bool alpha, bool isPlanar,
				    string colorSpaceName, int rBytes, int pBits);

		[Export ("initWithBitmapDataPlanes:pixelsWide:pixelsHigh:bitsPerSample:samplesPerPixel:hasAlpha:isPlanar:colorSpaceName:bitmapFormat:bytesPerRow:bitsPerPixel:")]
		IntPtr Constructor (IntPtr planes, int width, int height, int bps, int spp, bool alpha, bool isPlanar, string colorSpaceName,
				    NSBitmapFormat bitmapFormat, int rBytes, int pBits);

		[Export ("initWithCGImage:")]
		IntPtr Constructor (CGImage cgImage);

		[Export ("initWithCIImage:")]
		IntPtr Constructor (MonoMac.CoreImage.CIImage ciImage);

		[Export ("imageRepsWithData:")]
		NSImageRep [] ImageRepsWithData (NSData data);

		[Static]
		[Export ("imageRepWithData:")]
		NSImageRep ImageRepFromData (NSData data);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("bitmapData")]
		IntPtr BitmapData { get; }

		[Export ("getBitmapDataPlanes:")]
		void GetBitmapDataPlanes (IntPtr data);

		[Export ("isPlanar")]
		bool IsPlanar { get; }

		[Export ("samplesPerPixel")]
		int SamplesPerPixel { get; }

		[Export ("bitsPerPixel")]
		int BitsPerPixel { get; }

		[Export ("bytesPerRow")]
		int BytesPerRow { get; }

		[Export ("bytesPerPlane")]
		int BytesPerPlane { get; }

		[Export ("numberOfPlanes")]
		int Planes { get; }

		[Export ("bitmapFormat")]
		NSBitmapFormat BitmapFormat { get; }

		[Export ("getCompression:factor:")]
		void GetCompressionfactor (NSTiffCompression compression, float factor);

		[Export ("setCompression:factor:")]
		void SetCompressionfactor (NSTiffCompression compression, float factor);

		[Export ("TIFFRepresentation")]
		NSData TiffRepresentation { get; }

		[Export ("TIFFRepresentationUsingCompression:factor:")]
		NSData TiffRepresentationUsingCompressionfactor (NSTiffCompression comp, float factor);

		[Static]
		[Export ("TIFFRepresentationOfImageRepsInArray:")]
		NSData ImagesAsTiff (NSImageRep [] imageReps);

		[Static]
		[Export ("TIFFRepresentationOfImageRepsInArray:usingCompression:factor:")]
		NSData ImagesAsTiff (NSImageRep [] imageReps, NSTiffCompression comp, float factor);

		// FIXME: binding
		//[Static]
		//[Export ("getTIFFCompressionTypes:count:")]
		//void GetTiffCompressionTypes (const NSTIFFCompression list, int numTypes);

		[Static]
		[Export ("localizedNameForTIFFCompressionType:")]
		string LocalizedNameForTiffCompressionType (NSTiffCompression compression);

		[Export ("canBeCompressedUsing:")]
		bool CanBeCompressedUsing (NSTiffCompression compression);

		[Export ("colorizeByMappingGray:toColor:blackMapping:whiteMapping:")]
		void Colorize (float midPoint, NSColor midPointColor, NSColor shadowColor, NSColor lightColor);

		[Export ("incrementalLoadFromData:complete:")]
		int IncrementalLoad (NSData data, bool complete);

		[Export ("setColor:atX:y:")]
		void SetColorAt (NSColor color, int x, int y);

		[Export ("colorAtX:y:")]
		NSColor ColorAt (int x, int y);

		// FIXME: BINDING
		//[Export ("getPixel:atX:y:")]
		//void GetPixel (int[] p, int x, int y);
		//[Export ("setPixel:atX:y:")]
		//void SetPixel (int[] p, int x, int y);

		[Export ("CGImage")]
		CGImage CGImage { get; }

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("bitmapImageRepByConvertingToColorSpace:renderingIntent:")]
		NSBitmapImageRep ConvertingToColorSpace (NSColorSpace targetSpace, NSColorRenderingIntent renderingIntent);

		[Export ("bitmapImageRepByRetaggingWithColorSpace:")]
		NSBitmapImageRep RetaggedWithColorSpace (NSColorSpace newSpace);
	}

	[BaseType (typeof (NSView))]
	interface NSBox {
		[Export ("borderType")]
		NSBorderType BorderType { get; set; }
	
		[Export ("titlePosition")]
		NSTitlePosition TitlePosition { get; set; }
	
		[Export ("boxType")]
		NSBoxType BoxType { get; set; }
	
		[Export ("title")]
		string Title { get; set; }
	
		[Export ("titleFont")]
		NSFont TitleFont { get; set; }
	
		[Export ("borderRect")]
		RectangleF BorderRect { get; } 
	
		[Export ("titleRect")]
		RectangleF TitleRect { get; }
	
		[Export ("titleCell")]
		NSObject TitleCell { get; }
	
		[Export ("sizeToFit")]
		void SizeToFit ();
	
		[Export ("contentViewMargins")]
		SizeF ContentViewMargins { get; set; } 
	
		[Export ("setFrameFromContentFrame:")]
		void SetFrameFromContentFrame (RectangleF contentFrame);
	
		[Export ("contentView")]
		NSObject ContentView { get; set; }
	
		[Export ("transparent")]
		bool Transparent { [Bind ("isTransparent")] get; set; }

		[Export ("setTitleWithMnemonic")]
		void SetTitleWithMnemonic (string stringWithMnemonic);

		[Export ("borderWidth")]
		float BorderWidth { get; set; }
	
		[Export ("cornerRadius")]
		float CornerRadius { get; set; }
	
		[Export ("borderColor")]
		NSColor BorderColor { get; set; }
	
		[Export ("fillColor")]
		NSColor FillColor { get; set; }
	}
		
	[BaseType (typeof (NSControl))]
		// , Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSBrowserDelegate)})]
	interface NSBrowser {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("loadColumnZero")]
		void LoadColumnZero ();

		[Export ("isLoaded")]
		bool Loaded { get; }

		[Export ("autohidesScroller")]
		bool AutohidesScroller  { get; set; }

		[Export ("itemAtIndexPath:")]
		NSObject ItemAtIndexPath (NSIndexPath indexPath);

		[Export ("itemAtRow:inColumn:")]
		NSObject ItemAtRowinColumn (int row, int column);

		[Export ("indexPathForColumn:")]
		NSIndexPath IndexPathForColumn (int column);

		[Export ("isLeafItem")]
		bool IsLeafItem (NSObject item);

		[Export ("reloadDataForRowIndexes:inColumn:")]
		void ReloadData (NSIndexSet rowIndexes, int column);

		[Export ("parentForItemsInColumn:")]
		NSObject ParentForItems (int column);

		[Export ("scrollRowToVisible:inColumn:")]
		void ScrollRowToVisible (int row, int column);

		[Export ("setTitle:ofColumn:")]
		void SetTitle (string aString, int column);

		[Export ("titleOfColumn:")]
		string ColumnTitle (int column);

		[Export ("pathToColumn:")]
		string ColumnPath (int column);

		[Export ("clickedColumn")]
		int ClickedColumn ();

		[Export ("clickedRow")]
		int ClickedRow ();

		[Export ("selectedColumn")]
		int SelectedColumn ();

		[Export ("selectedCell")]
		NSObject SelectedCell ();

		[Export ("selectedCellInColumn:")]
		NSObject SelectedCellInColumn (int column);

		[Export ("selectedCells")]
		NSCell [] SelectedCells ();

		[Export ("selectRow:inColumn:")]
		void Select (int row, int column);

		[Export ("selectedRowInColumn:")]
		int SelectedRow (int column);

		[Export ("selectionIndexPath")]
		NSIndexPath SelectionIndexPath { get; set; }

		[Export ("selectionIndexPaths")]
		NSIndexPath [] SelectionIndexPaths  { get; set; }

		[Export ("selectRowIndexes:inColumn:")]
		void SelectRowIndexes (NSIndexSet indexes, int column);

		[Export ("selectedRowIndexesInColumn:")]
		NSIndexSet SelectedRowIndexes (int column);

		[Export ("reloadColumn:")]
		void ReloadColumn (int column);

		[Export ("validateVisibleColumns")]
		void ValidateVisibleColumns ();

		[Export ("scrollColumnsRightBy:")]
		void ScrollColumnsRightBy (int shiftAmount);

		[Export ("scrollColumnsLeftBy:")]
		void ScrollColumnsLeftBy (int shiftAmount);

		[Export ("scrollColumnToVisible:")]
		void ScrollColumnToVisible (int column);

		[Export ("addColumn")]
		void AddColumn ();

		[Export ("numberOfVisibleColumns")]
		int VisibleColumns { get; }

		[Export ("firstVisibleColumn")]
		int FirstVisibleColumn { get; }

		[Export ("lastVisibleColumn")]
		int LastVisibleColumn { get; }

		[Export ("columnOfMatrix:")]
		int ColumnOfMatrix (NSMatrix matrix);

		[Export ("matrixInColumn:")]
		NSMatrix MatrixInColumn (int column);

		[Export ("loadedCellAtRow:column:")]
		NSCell LoadedCell (int row, int col);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("tile")]
		void Tile ();

		[Export ("doClick:")]
		void DoClick (NSObject sender);

		[Export ("doDoubleClick:")]
		void DoDoubleClick (NSObject sender);

		[Export ("sendAction")]
		bool SendAction ();

		[Export ("titleFrameOfColumn:")]
		RectangleF TitleFrameOfColumn (int column);

		[Export ("drawTitleOfColumn:inRect:")]
		void DrawTitle (int column, RectangleF aRect);

		[Export ("titleHeight")]
		float TitleHeight { get; }

		[Export ("frameOfColumn:")]
		RectangleF ColumnFrame (int column);

		[Export ("frameOfInsideOfColumn:")]
		RectangleF ColumnInsideFrame (int column);

		[Export ("frameOfRow:inColumn:")]
		RectangleF RowFrame (int row, int column);

		[Export ("getRow:column:forPoint:")]
		bool GetRowColumnforPoint (out int row, out int column, PointF point);

		[Export ("columnWidthForColumnContentWidth:")]
		float ColumnWidthForColumnContentWidth (float columnContentWidth);

		[Export ("columnContentWidthForColumnWidth:")]
		float ColumnContentWidthForColumnWidth (float columnWidth);

		[Export ("setColumnResizingType:")]
		void SetColumnResizingType (NSBrowserColumnResizingType columnResizingType);

		[Export ("columnResizingType")]
		NSBrowserColumnResizingType ColumnResizingType { get; }

		[Export ("prefersAllColumnUserResizing")]
		bool PrefersAllColumnUserResizing { get; set; }

		[Export ("setWidth:ofColumn:")]
		void SetColumnWidth (float columnWidth, int columnIndex);

		[Export ("widthOfColumn:")]
		float GetColumnWidth (int column);

		[Export ("rowHeight")]
		float RowHeight { get; set; }

		[Export ("noteHeightOfRowsWithIndexesChanged:inColumn:")]
		void NoteHeightOfRows (NSIndexSet indexSet, int columnIndex);

		[Export ("defaultColumnWidth")]
		float DefaultColumnWidth { get; set; }

		[Export ("columnsAutosaveName")]
		string ColumnsAutosaveName  { get; set; }

		[Static]
		[Export ("removeSavedColumnsWithAutosaveName:")]
		void RemoveSavedColumnsWithAutosaveName (string name);

		[Export ("canDragRowsWithIndexes:inColumn:withEvent:")]
		bool CanDragRowsWithIndexes (NSIndexSet rowIndexes, int column, NSEvent theEvent);

		// FIXME: binding, NSPointPointer
		//[Export ("draggingImageForRowsWithIndexes:inColumn:withEvent:offset:")]
		//NSImage DraggingImageForRowsWithIndexes (NSIndexSet rowIndexes, int column, NSEvent theEvent, NSPointPointer dragImageOffset);

		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSourceOperationMask (NSDragOperation mask, bool isLocal);

		[Export ("allowsTypeSelect")]
		bool AllowsTypeSelect  { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor  { get; set; }

		[Export ("editItemAtIndexPath:withEvent:select:")]
		void EditItemAtIndexPath (NSIndexPath indexPath, NSEvent theEvent, bool select);

		//Detected properties
		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("matrixClass")]
		Class MatrixClass { get; set; }

		[Export ("cellClass")]
		Class CellClass { get; set; }

		[Export ("cellPrototype")]
		NSObject CellPrototype { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSBrowserDelegate Delegate { get; set; }

		[Export ("reusesColumns")]
		bool ReusesColumns { get; set; }

		[Export ("hasHorizontalScroller")]
		bool HasHorizontalScroller { get; set; }

		[Export ("separatesColumns")]
		bool SeparatesColumns { get; set; }

		[Export ("titled")]
		bool Titled { [Bind ("isTitled")]get; set; }

		[Export ("minColumnWidth")]
		float MinColumnWidth { get; set; }

		[Export ("maxVisibleColumns")]
		int MaxVisibleColumns { get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("allowsBranchSelection")]
		bool AllowsBranchSelection { get; set; }

		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }

		[Export ("takesTitleFromPreviousColumn")]
		bool TakesTitleFromPreviousColumn { get; set; }

		[Export ("sendsActionOnArrowKeys")]
		bool SendsActionOnArrowKeys { get; set; }

		[Export ("pathSeparator")]
		string PathSeparator { get; set; }

		[Export ("path")]
		string Path { get; set; }

		[Export ("lastColumn")]
		int LastColumn { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSBrowserDelegate {
		[Export ("browser:numberOfRowsInColumn:"), EventArgs ("NSBrowserColumn")]
		int RowsInColumn (NSBrowser sender, int column);

		[Export ("browser:createRowsForColumn:inMatrix:")]
		void CreateRowsForColumn (NSBrowser sender, int column, NSMatrix matrix);

		[Export ("browser:numberOfChildrenOfItem:")]
		int CountChildren (NSBrowser browser, NSObject item);

		[Export ("browser:child:ofItem:")]
		NSObject GetChild (NSBrowser browser, int index, NSObject item);

		[Export ("browser:isLeafItem:")]
		bool IsLeafItem (NSBrowser browser, NSObject item);

		[Export ("browser:objectValueForItem:")]
		NSObject ObjectValueForItem (NSBrowser browser, NSObject item);

		[Export ("browser:heightOfRow:inColumn:")]
		float RowHeight (NSBrowser browser, int row, int columnIndex);

		[Export ("rootItemForBrowser:")]
		NSObject RootItemForBrowser (NSBrowser browser);

		[Export ("browser:setObjectValue:forItem:")]
		void SetObjectValue (NSBrowser browser, NSObject obj, NSObject item);

		[Export ("browser:shouldEditItem:")]
		bool ShouldEditItem (NSBrowser browser, NSObject item);

		[Export ("browser:willDisplayCell:atRow:column:")]
		void WillDisplayCell (NSBrowser sender, NSObject cell, int row, int column);

		[Export ("browser:titleOfColumn:")]
		string ColumnTitle (NSBrowser sender, int column);

		[Export ("browser:selectCellWithString:inColumn:")]
		bool SelectCellWithString (NSBrowser sender, string title, int column);

		[Export ("browser:selectRow:inColumn:")]
		bool SelectRowinColumn (NSBrowser sender, int row, int column);

		[Export ("browser:isColumnValid:")]
		bool IsColumnValid (NSBrowser sender, int column);

		[Export ("browserWillScroll:")]
		void WillScroll (NSBrowser sender);

		[Export ("browserDidScroll:")]
		void DidScroll (NSBrowser sender);

		[Export ("browser:shouldSizeColumn:forUserResize:toWidth:")]
		float ShouldSizeColumn (NSBrowser browser, int columnIndex, bool userResize, float suggestedWidth);

		[Export ("browser:sizeToFitWidthOfColumn:")]
		float SizeToFitWidth (NSBrowser browser, int columnIndex);

		[Export ("browserColumnConfigurationDidChange:")]
		void ColumnConfigurationDidChange (NSNotification notification);

		[Export ("browser:shouldShowCellExpansionForRow:column:")]
		bool ShouldShowCellExpansion (NSBrowser browser, int row, int column);

		[Export ("browser:writeRowsWithIndexes:inColumn:toPasteboard:")]
		bool WriteRowsWithIndexesToPasteboard (NSBrowser browser, NSIndexSet rowIndexes, int column, NSPasteboard pasteboard);

		[Export ("browser:namesOfPromisedFilesDroppedAtDestination:forDraggedRowsWithIndexes:inColumn:")]
		string [] PromisedFilesDroppedAtDestination (NSBrowser browser, NSUrl dropDestination, NSIndexSet rowIndexes, int column);

		[Export ("browser:canDragRowsWithIndexes:inColumn:withEvent:")]
		bool CanDragRowsWithIndexes (NSBrowser browser, NSIndexSet rowIndexes, int column, NSEvent theEvent);

		// FIXME: NSPOintPointer is a pointer to a PointF, so we need to support refs
		//[Export ("browser:draggingImageForRowsWithIndexes:inColumn:withEvent:offset:")]
		//NSImage DraggingImageForRowsWithIndexes (NSBrowser browser, NSIndexSet rowIndexes, int column, NSEvent theEvent, NSPointPointer dragImageOffset);

		[Export ("browser:validateDrop:proposedRow:column:dropOperation:")]
		NSDragOperation ValidateDrop (NSBrowser browser, NSDraggingInfo info, int proposedRow, int column, NSBrowserDropOperation dropOperation);

		[Export ("browser:acceptDrop:atRow:column:dropOperation:")]
		bool AcceptDrop (NSBrowser browser, NSDraggingInfo info, int row, int column, NSBrowserDropOperation dropOperation);

		[Export ("browser:typeSelectStringForRow:inColumn:")]
		string TypeSelectString (NSBrowser browser, int row, int column);

		[Export ("browser:shouldTypeSelectForEvent:withCurrentSearchString:")]
		bool ShouldTypeSelectForEvent (NSBrowser browser, NSEvent theEvent, string currentSearchString);

		[Export ("browser:nextTypeSelectMatchFromRow:toRow:inColumn:forString:")]
		int NextTypeSelectMatch (NSBrowser browser, int startRow, int endRow, int column, string searchString);

		[Export ("browser:previewViewControllerForLeafItem:")]
		NSViewController PreviewViewControllerForLeafItem (NSBrowser browser, NSObject item);

		[Export ("browser:headerViewControllerForItem:")]
		NSViewController HeaderViewControllerForItem (NSBrowser browser, NSObject item);

		[Export ("browser:didChangeLastColumn:toColumn:")]
		void DidChangeLastColumntoColumn (NSBrowser browser, int oldLastColumn, int column);

		[Export ("browser:selectionIndexesForProposedSelection:inColumn:")]
		NSIndexSet SelectionIndexesForProposedSelection (NSBrowser browser, NSIndexSet proposedSelectionIndexes, int inColumn);

	}

	[BaseType (typeof (NSCell))]
	interface NSBrowserCell {
		[Static]
		[Export ("branchImage")]
		NSImage BranchImage { get; }

		[Static]
		[Export ("highlightedBranchImage")]
		NSImage HighlightedBranchImage { get; }

		[Export ("highlightColorInView:")]
		NSColor HighlightColorInView (NSView controlView);

		[Export ("reset")]
		void Reset ();

		[Export ("set")]
		void Set ();

		//Detected properties
		[Export ("leaf")]
		bool Leaf { [Bind ("isLeaf")]get; set; }

		[Export ("loaded")]
		bool Loaded { [Bind ("isLoaded")]get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("alternateImage")]
		NSImage AlternateImage { get; set; }

	}

	[BaseType (typeof (NSActionCell))]
	interface NSButtonCell {
		[Export ("title")]
		string Title { get; set; }
	
		[Export ("alternateTitle")]
		string AlternateTitle { get; set; }
	
		[Export ("alternateImage")]
		NSImage AlternateImage { get; set; }
	
		[Export ("imagePosition")]
		NSCellImagePosition ImagePosition { get; set; }
	
		[Export ("imageScaling")]
		NSImageScale ImageScale { get; set; }
	
		[Export ("highlightsBy")]
		int HighlightsBy { get; set; }
	
		[Export ("showsStateBy")]
		int ShowsStateBy { get; set; }
	
		[Export ("setShowsStateBy:")]
		void SetShowsStateBy (int aType);
	
		[Export ("setButtonType:")]
		void SetButtonType (NSButtonType aType);
	
		[Export ("isOpaque")]
		bool IsOpaque { get; }
	
		[Export ("setFont:")]
		void SetFont (NSFont  fontObj);
	
		[Export ("transparent")]
		bool Transparent { [Bind ("isTransparent")] get; set; }
	
		[Export ("setPeriodicDelay:interval:")]
		void SetPeriodicDelay (float delay, float interval);
	
		[Export ("getPeriodicDelay:interval:")]
		void GetPeriodicDelay (out float  delay, out float  interval);
	
		[Export ("keyEquivalent")]
		string KeyEquivalent { get; set; }
	
		[Export ("keyEquivalentModifierMask")]
		NSEventModifierMask KeyEquivalentModifierMask { get; set; }
	
		[Export ("keyEquivalentFont")]
		NSFont KeyEquivalentFont { get; set; }
	
		[Export ("setKeyEquivalentFont:size:")]
		void SetKeyEquivalentFont (string  fontName, float fontSize);
	
		[Export ("performClick:")]
		void PerformClick (NSObject sender);
	
		[Export ("drawImage:withFrame:inView:")]
		void DrawImage (NSImage image, RectangleF frame, NSView controlView);
	
		[Export ("drawTitle:withFrame:inView:")]
		RectangleF DrawTitle (NSAttributedString title, RectangleF frame, NSView controlView);
	
		[Export ("drawBezelWithFrame:inView:")]
		void DrawBezelWithFrame (RectangleF frame, NSView controlView);

		[Export ("alternateMnemonicLocation")]
		int AlternateMnemonicLocation { get; set; }
	
		[Export ("alternateMnemonic")]
		string AlternateMnemonic { get; [Bind ("setAlternateTitleWithMnemonic:")] set; }
	
		[Export ("setGradientType:")]
		void SetGradientType (NSGradientType type);
	
		[Export ("imageDimsWhenDisabled")]
		bool ImageDimsWhenDisabled { get; set; }
	
		[Export ("showsBorderOnlyWhileMouseInside")]
		bool ShowsBorderOnlyWhileMouseInside { get; set; }
	
		[Export ("mouseEntered:")]
		void MouseEntered (NSEvent theEvent);
	
		[Export ("mouseExited:")]
		void MouseExited (NSEvent theEvent);
	
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }
	
		[Export ("attributedAlternateTitle")]
		NSAttributedString AttributedAlternateTitle { get; set; }
	
		[Export ("bezelStyle")]
		NSBezelStyle BezelStyle { get; set; }

		[Export ("sound")]
		NSSound Sound { get; set; }
	
	}
	
	[BaseType (typeof (NSControl))]
	interface NSButton {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("title")]
		string Title { get; set; } 
	
		[Export ("alternateTitle")]
		string AlternateTitle { get; set; }
	
		[Export ("image")]
		NSImage Image { get; set; }
	
		[Export ("alternateImage")]
		NSImage AlternateImage  { get; set; }
	
		[Export ("imagePosition")]
		NSCellImagePosition ImagePosition  { get; set; }
	
		[Export ("setButtonType:")]
		void SetButtonType (NSButtonType aType);
	
		[Export ("state")]
		int State { get; set; }
	
		[Export ("bordered")]
		bool Bordered  { [Bind ("isBordered")] get; set; }
	
		[Export ("transparent")]
		bool Transparent  { [Bind ("isTransparent")] get; set; }
	
		[Export ("setPeriodicDelay:interval:")]
		void SetPeriodicDelay (float delay, float interval);
	
		[Export ("getPeriodicDelay:interval:")]
		void GetPeriodicDelay (ref float delay, ref float interval);
	
		[Export ("keyEquivalent")]
		string KeyEquivalent  { get; set; }
	
		[Export ("keyEquivalentModifierMask")]
		NSEventModifierMask KeyEquivalentModifierMask  { get; set; }
	
		[Export ("highlight:")]
		void Highlight (bool flag);
	
		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent  key);

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string mnemonic);

		[Export ("setAttributedTitle:")]
		NSAttributedString AttributedTitle { get; set; }

		[Export ("attributedAlternateTitle")]
		NSAttributedString AttributedAlternateTitle  { get; set; }

		[Export ("bezelStyle")]
		NSBezelStyle bezelStyle ();

		[Export ("allowsMixedState")]
		bool AllowsMixedState { get; }

		[Export ("setNextState")]
		void SetNextState ();

		[Export ("showsBorderOnlyWhileMouseInside")]
		bool ShowsBorderOnlyWhileMouseInside ();

		[Export ("sound")]
		NSSound Sound { get; set; }
	}
	
	[BaseType (typeof (NSImageRep))]
	interface NSCachedImageRep {
		[Export ("initWithIdentifier:")]
	   	IntPtr Constructor (NSWindow win, RectangleF rect);
		
		[Export ("initWithSize:depth:separate:alpha:")]
		IntPtr Constructor (SizeF size, NSWindowDepth depth, bool separate, bool alpha);

		[Export ("window")]
		NSWindow Window { get; }

		[Export ("rect")]
		RectangleF Rectangle { get; }

	}
	
	[BaseType (typeof (NSObject))]
	interface NSCell {
		[Static, Export ("prefersTrackingUntilMouseUp")]
		bool PrefersTrackingUntilMouseUp { get; }
	
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);
	
		[Export ("controlView")]
		NSView ControlView { get; set; }
	
		[Export ("type")]
		NSCellType CellType { get; set; }
	
		[Export ("state")]
		int State { get; set; }
	
		[Export ("target")]
		NSObject Target { get; set; }
	
		[Export ("action")]
		Selector Action { get; set; }
	
		[Export ("tag")]
		int Tag { get; set; }
	
		[Export ("title")]
		string Title { get; set; }
	
		[Export ("isOpaque")]
		bool IsOpaque { get; } 
	
		[Export ("isEnabled")]
		bool Enabled { get; set; }
	
		[Export ("sendActionOn:")]
		int SendActionOn (int mask);
	
		[Export ("isContinuous")]
		bool IsContinuous { get; set; }
	
		[Export ("isEditable")]
		bool Editable { get; set; }
	
		[Export ("isSelectable")]
		bool Selectable { get; set; }
	
		[Export ("isBordered")]
		bool Bordered { get; set; }
	
		[Export ("isBezeled")]
		bool Bezeled { get; set; }
	
		[Export ("isScrollable")]
		bool Scrollable { get; set; }
	
		[Export ("isHighlighted")]
		bool Highlighted { get; set; }
	
		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }
	
		[Export ("wraps")]
		bool Wraps { get; set; }
	
		[Export ("font")]
		NSFont Font { get; set; }
	
		[Export ("isEntryAcceptable:")]
		bool IsEntryAcceptable (string  aString);
	
		[Export ("keyEquivalent")]
		string KeyEquivalent { get; }
	
		[Export ("formatter")]
		NSFormatter Formatter { get; set; }
	
		[Export ("objectValue")]
		NSObject ObjectValue { get; set; }
	
		[Export ("hasValidObjectValue")]
		bool HasValidObjectValue { get; }
	
		[Export ("stringValue")]
		string StringValue { get; set; }
	
		[Export ("compare:")]
		NSComparisonResult Compare (NSObject otherCell);
	
		[Export ("intValue")]
		int IntValue { get; set; }
	
		[Export ("floatValue")]
		float FloatValue { get; set; }
	
		[Export ("doubleValue")]
		double DoubleValue { get; set; }
	
		[Export ("takeIntValueFrom:")]
		void TakeIntValueFrom (NSObject sender);
	
		[Export ("takeFloatValueFrom:")]
		void TakeFloatValueFrom (NSObject sender);
	
		[Export ("takeDoubleValueFrom:")]
		void TakeDoubleValueFrom (NSObject sender);
	
		[Export ("takeStringValueFrom:")]
		void TakeStringValueFrom (NSObject sender);
	
		[Export ("takeObjectValueFrom:")]
		void TakeObjectValueFrom (NSObject sender);
	
		[Export ("image")]
		NSImage Image  { get; set; }
	
		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }
	
		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }
	
		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }
	
		[Export ("cellAttribute:")]
		int CellAttribute (NSCellAttribute aParameter);
	
		[Export ("setCellAttribute:to:")]
		void SetCellAttribute (NSCellAttribute aParameter, int value);
	
		[Export ("imageRectForBounds:")]
		RectangleF ImageRectForBounds (RectangleF theRect);
	
		[Export ("titleRectForBounds:")]
		RectangleF TitleRectForBounds (RectangleF theRect);
	
		[Export ("drawingRectForBounds:")]
		RectangleF DrawingRectForBounds (RectangleF theRect);
	
		[Export ("cellSize")]
		SizeF CellSize { get; }
	
		[Export ("cellSizeForBounds:")]
		SizeF CellSizeForBounds (RectangleF bounds);
	
		[Export ("highlightColorWithFrame:inView:")]
		NSColor HighlightColor (RectangleF cellFrame, NSView controlView);
	
		[Export ("calcDrawInfo:")]
		void CalcDrawInfo (RectangleF aRect);
	
		[Export ("setUpFieldEditorAttributes:")]
		NSText SetUpFieldEditorAttributes (NSText textObj);
	
		[Export ("drawInteriorWithFrame:inView:")]
		void DrawInteriorWithFrame (RectangleF cellFrame, NSView  inView);
	
		[Export ("drawWithFrame:inView:")]
		void DrawWithFrame (RectangleF cellFrame, NSView inView);
	
		[Export ("highlight:withFrame:inView:")]
		void Highlight (bool flag, RectangleF withFrame, NSView  inView);
	
		[Export ("mouseDownFlags")]
		int MouseDownFlags { get; }
	
		[Export ("getPeriodicDelay:interval:")]
		void GetPeriodicDelay (ref float delay, ref float interval);
	
		[Export ("startTrackingAt:inView:")]
		bool StartTracking (PointF startPoint, NSView inView);
	
		[Export ("continueTracking:at:inView:")]
		bool ContinueTracking (PointF lastPoint, PointF currentPoint, NSView inView);
	
		[Export ("stopTracking:at:inView:mouseIsUp:")]
		void StopTracking (PointF lastPoint, PointF stopPoint, NSView inView, bool mouseIsUp);
	
		[Export ("trackMouse:inRect:ofView:untilMouseUp:")]
		bool TrackMouse (NSEvent  theEvent, RectangleF cellFrame, NSView  controlView, bool untilMouseUp);
	
		[Export ("editWithFrame:inView:editor:delegate:event:")]
		void EditWithFrame (RectangleF aRect, NSView  inView, NSText editor, NSObject delegateObject, NSEvent theEvent);
	
		[Export ("selectWithFrame:inView:editor:delegate:start:length:")]
		void SelectWithFrame (RectangleF aRect, NSView inView, NSText editor, NSObject delegateObject, int selStart, int selLength);
	
		[Export ("endEditing:")]
		void EndEditing (NSText textObj);
	
		[Export ("resetCursorRect:inView:")]
		void ResetCursorRect (RectangleF cellFrame, NSView  inView);
	
		[Export ("menu")]
		NSMenu Menu { get; set; }
	
		[Export ("menuForEvent:inRect:ofView:")]
		NSMenu MenuForEvent (NSEvent theEvent, RectangleF cellFrame, NSView  view);
	
		[Static]
		[Export ("defaultMenu")]
		NSMenu DefaultMenu { get; }
	
		[Export ("setSendsActionOnEndEditing:")]
		void SetSendsActionOnEndEditing (bool flag);
	
		[Export ("sendsActionOnEndEditing")]
		bool SendsActionOnEndEditing ();
	
		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; set; }
       
		[Export ("lineBreakMode")]
		NSLineBreakMode LineBreakMode { get; set; }
	
		[Export ("allowsUndo")]
		bool AllowsUndo { get; set; }
	
		[Export ("integerValue")]
		int IntegerValue { get; set; }
	
		[Export ("takeIntegerValueFrom:")]
		void TakeIntegerValueFrom (NSObject sender);
	
		[Export ("truncatesLastVisibleLine")]
		bool TruncatesLastVisibleLine { get; set; }
	
		[Export ("userInterfaceLayoutDirection")]
		NSUserInterfaceLayoutDirection UserInterfaceLayoutDirection { get; set; }
	
		[Export ("fieldEditorForView:")]
		NSTextView FieldEditorForView (NSView  aControlView);
	
		[Export ("usesSingleLineMode")]
		bool UsesSingleLineMode { get; set; }

		//  NSCell(NSCellAttributedStringMethods)
		[Export ("refusesFirstResponder")]
		bool RefusesFirstResponder ();
	
		[Export ("acceptsFirstResponder")]
		bool AcceptsFirstResponder ();
	
		[Export ("showsFirstResponder")]
		bool ShowsFirstResponder { get; set; }

		[Export ("mnemonicLocation")]
		int MnemonicLocation { get; set; }
	
		[Export ("mnemonic")]
		string Mnemonic { get; }
	
		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string  stringWithAmpersand);
	
		[Export ("performClick:")]
		void PerformClick (NSObject sender);
	
		[Export ("focusRingType")]
		NSFocusRingType focusRingType { get; set; }
	
		[Static, Export ("defaultFocusRingType")]
		NSFocusRingType DefaultFocusRingType { get; }
	
		[Export ("wantsNotificationForMarkedText")]
		bool WantsNotificationForMarkedText { get; }
	
		// NSCell(NSCellAttributedStringMethods)
		[Export ("setAttributedStringValue:")]
		void SetAttributedStringValue (NSAttributedString  obj);
	
		[Export ("allowsEditingTextAttributes")]
		bool AllowsEditingTextAttributes { get; set; }
	
		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }
       
		// NSCell(NSCellMixedState) {
		[Export ("allowsMixedState")]
		bool AllowsMixedState { get; }
	
		[Export ("nextState")]
		int NextState { get; }
	
		[Export ("setNextState")]
		void SetNextState ();
	
		// 
		[Export ("hitTestForEvent:inRect:ofView:")]
		NSCellHit HitTest (NSEvent forEvent, RectangleF inRect, NSView  ofView);
	
		// NSCell(NSCellExpansion) 
		[Export ("expansionFrameWithFrame:inView:")]
		RectangleF ExpansionFrame (RectangleF withFrame, NSView inView);
	
		[Export ("drawWithExpansionFrame:inView:")]
		void DrawWithExpansionFrame (RectangleF cellFrame, NSView inView);
	
		[Export ("backgroundStyle")]
		NSBackgroundStyle BackgroundStyle { get; set; }
	
		[Export ("interiorBackgroundStyle")]
		NSBackgroundStyle InteriorBackgroundStyle { get; }
	
	}

	[BaseType (typeof (NSImageRep))]
	interface NSCIImageRep {
		[Static]
		[Export ("imageRepWithCIImage:")]
		NSCIImageRep FromCIImage (CIImage image);

		[Export ("initWithCIImage:")]
		IntPtr Constructor (CIImage image);

		[Export ("CIImage")]
		CIImage CIImage { get; }
	}
	
	[BaseType (typeof (NSObject))]
	interface NSColor {
		[Static]
		[Export ("colorWithCalibratedWhite:alpha:")]
		NSColor FromCalibratedWhite (float white, float alpha);

		[Static]
		[Export ("colorWithCalibratedHue:saturation:brightness:alpha:")]
		NSColor FromCalibratedHSBA (float hue, float saturation, float brightness, float alpha);

		[Static]
		[Export ("colorWithCalibratedRed:green:blue:alpha:")]
		NSColor FromCalibratedRGBA (float red, float green, float blue, float alpha);

		[Static]
		[Export ("colorWithDeviceWhite:alpha:")]
		NSColor FromDeviceWhite (float white, float alpha);

		[Static]
		[Export ("colorWithDeviceHue:saturation:brightness:alpha:")]
		NSColor ColorWithDeviceHSBA (float hue, float saturation, float brightness, float alpha);

		[Static]
		[Export ("colorWithDeviceRed:green:blue:alpha:")]
		NSColor ColorWithDeviceRGBA (float red, float green, float blue, float alpha);

		[Static]
		[Export ("colorWithDeviceCyan:magenta:yellow:black:alpha:")]
		NSColor ColorWithDeviceCYMKA (float cyan, float magenta, float yellow, float black, float alpha);

		[Static]
		[Export ("colorWithCatalogName:colorName:")]
		NSColor FromCatalogName (string listName, string colorName);

		// FIXME: binding components pointer
		//[Export ("colorWithColorSpace:components:count:")]
		//NSColor ColorWithColorSpace (NSColorSpace space, const CGFloat components, int numberOfComponents);

		[Export ("blackColor")]
		NSColor Black { get; }

		[Static]
		[Export ("darkGrayColor")]
		NSColor DarkGray { get; } 

		[Static]
		[Export ("lightGrayColor")]
		NSColor LightGray { get; }

		[Static]
		[Export ("whiteColor")]
		NSColor White { get; }

		[Static]
		[Export ("grayColor")]
		NSColor Gray { get; }

		[Static]
		[Export ("redColor")]
		NSColor Red { get; }

		[Static]
		[Export ("greenColor")]
		NSColor Green { get; }

		[Static]
		[Export ("blueColor")]
		NSColor Blue { get; }

		[Static]
		[Export ("cyanColor")]
		NSColor Cyan { get; }

		[Static]
		[Export ("yellowColor")]
		NSColor Yellow { get; }

		[Static]
		[Export ("magentaColor")]
		NSColor Magenta { get; }

		[Static]
		[Export ("orangeColor")]
		NSColor Orange { get; }

		[Static]
		[Export ("purpleColor")]
		NSColor Purple { get; }

		[Static]
		[Export ("brownColor")]
		NSColor Brown { get; }

		[Static]
		[Export ("clearColor")]
		NSColor Clear { get; }

		[Static]
		[Export ("controlShadowColor")]
		NSColor ControlShadow { get; }

		[Static]
		[Export ("controlDarkShadowColor")]
		NSColor ControlDarkShadow { get; }

		[Static]
		[Export ("controlColor")]
		NSColor Control { get; }

		[Static]
		[Export ("controlHighlightColor")]
		NSColor ControlHighlight { get; }

		[Static]
		[Export ("controlLightHighlightColor")]
		NSColor ControlLightHighlight { get; }

		[Static]
		[Export ("controlTextColor")]
		NSColor ControlText { get; }

		[Static]
		[Export ("controlBackgroundColor")]
		NSColor ControlBackground { get; }

		[Static]
		[Export ("selectedControlColor")]
		NSColor SelectedControl { get; }

		[Static]
		[Export ("secondarySelectedControlColor")]
		NSColor SecondarySelectedControl { get; }

		[Static]
		[Export ("selectedControlTextColor")]
		NSColor SelectedControlText { get; }

		[Static]
		[Export ("disabledControlTextColor")]
		NSColor DisabledControlText { get; }

		[Static]
		[Export ("textColor")]
		NSColor Text { get; }

		[Static]
		[Export ("textBackgroundColor")]
		NSColor TextBackground { get; }

		[Static]
		[Export ("selectedTextColor")]
		NSColor SelectedText { get; }

		[Static]
		[Export ("selectedTextBackgroundColor")]
		NSColor SelectedTextBackground { get; }

		[Static]
		[Export ("gridColor")]
		NSColor Grid { get; }

		[Static]
		[Export ("keyboardFocusIndicatorColor")]
		NSColor KeyboardFocusIndicator { get; }

		[Static]
		[Export ("windowBackgroundColor")]
		NSColor WindowBackground { get; }

		[Static]
		[Export ("scrollBarColor")]
		NSColor ScrollBar { get; }

		[Static]
		[Export ("knobColor")]
		NSColor Knob { get; }

		[Static]
		[Export ("selectedKnobColor")]
		NSColor SelectedKnob { get; }

		[Static]
		[Export ("windowFrameColor")]
		NSColor WindowFrame { get; }

		[Static]
		[Export ("windowFrameTextColor")]
		NSColor WindowFrameText { get; }

		[Static]
		[Export ("selectedMenuItemColor")]
		NSColor SelectedMenuItem { get; }

		[Static]
		[Export ("selectedMenuItemTextColor")]
		NSColor SelectedMenuItemText { get; }

		[Static]
		[Export ("highlightColor")]
		NSColor Highlight { get; }

		[Static]
		[Export ("shadowColor")]
		NSColor Shadow { get; }

		[Static]
		[Export ("headerColor")]
		NSColor Header { get; }

		[Static]
		[Export ("headerTextColor")]
		NSColor HeaderText { get; }

		[Export ("alternateSelectedControlColor")]
		NSColor AlternateSelectedControl { get; }

		[Static]
		[Export ("alternateSelectedControlTextColor")]
		NSColor AlternateSelectedControlText { get; }

		[Export ("controlAlternatingRowBackgroundColors")]
		NSColor [] ControlAlternatingRowBackgroundColors ();

		[Export ("highlightWithLevel:")]
		NSColor HighlightWithLevel (float val);

		[Export ("shadowWithLevel:")]
		NSColor ShadowWithLevel (float val);

		[Static]
		[Export ("colorForControlTint:")]
		NSColor ColorForControlTint (NSControlTint controlTint);

		[Export ("currentControlTint")]
		NSControlTint CurrentControlTint { get; }

		[Export ("set")]
		void Set ();

		[Export ("setFill")]
		void SetFill ();

		[Export ("setStroke")]
		void SetStroke ();

		[Export ("colorSpaceName")]
		string ColorSpaceName { get; }

		[Export ("colorUsingColorSpaceName:")]
		NSColor ColorUsingColorSpaceName (string colorSpace);

		[Export ("colorUsingColorSpaceName:device:")]
		NSColor CreateFromColorSpaceName (string colorSpace, NSDictionary deviceDescription);

		[Export ("colorUsingColorSpace:")]
		NSColor CreateUsingColorSpace (NSColorSpace space);

		[Export ("blendedColorWithFraction:ofColor:")]
		NSColor BlendedColor (float fraction, NSColor color);

		[Export ("colorWithAlphaComponent:")]
		NSColor ColorWithAlphaComponent (float alpha);

		[Export ("catalogNameComponent")]
		string CatalogNameComponent { get; }

		[Export ("colorNameComponent")]
		string ColorNameComponent { get; }

		[Export ("localizedCatalogNameComponent")]
		string LocalizedCatalogNameComponent { get; }

		[Export ("localizedColorNameComponent")]
		string LocalizedColorNameComponent { get; }

		[Export ("redComponent")]
		float RedComponent { get; }

		[Export ("greenComponent")]
		float GreenComponent { get; }

		[Export ("blueComponent")]
		float BlueComponent ();

		// FIXME: binding, need out values here
		//[Export ("getRed:green:blue:alpha:")]
		//void GetRedgreenbluealpha (out float red, out float green, out float blue, outfloat alpha);

		[Export ("hueComponent")]
		float HueComponent { get; }

		[Export ("saturationComponent")]
		float SaturationComponent { get; }

		[Export ("brightnessComponent")]
		float BrightnessComponent { get; }

		// FIXME: binding need out alues hre
		// [Export ("getHue:saturation:brightness:alpha:")]
		//void GetHuesaturationbrightnessalpha (out float hue, out float saturation, out float brightness, out float alpha);

		[Export ("whiteComponent")]
		float WhiteComponent { get; }

		// FIXME: binding need out values here
		//[Export ("getWhite:alpha:")]
		//void GetWhitealpha (out float white, out float alpha);

		[Export ("cyanComponent")]
		float CyanComponent { get; }

		[Export ("magentaComponent")]
		float MagentaComponent { get; }

		[Export ("yellowComponent")]
		float YellowComponent { get; }

		[Export ("blackComponent")]
		float BlackComponent { get; }

		// FIXME: binding need out values here
		//[Export ("getCyan:magenta:yellow:black:alpha:")]
		//void GetCyanmagentayellowblackalpha (out float cyan, out float magenta, out float yellow, out float black, out float alpha);

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("numberOfComponents")]
		int Components { get; }

		// FIXME: binding, returns array
		//[Export ("getComponents:")]
		//void GetComponents (float components);

		[Export ("alphaComponent")]
		float AlphaComponent { get; }

		[Static]
		[Export ("colorFromPasteboard:")]
		NSColor FromPasteboard (NSPasteboard pasteBoard);

		[Export ("writeToPasteboard:")]
		void WriteToPasteboard (NSPasteboard pasteBoard);

		[Static]
		[Export ("colorWithPatternImage:")]
		NSColor FromPatternImage (NSImage image);

		[Export ("patternImage")]
		NSImage PatternImage { get; }

		[Export ("drawSwatchInRect:")]
		void DrawSwatchInRect (RectangleF rect);

		//Detected properties
		[Static]
		[Export ("ignoresAlpha")]
		bool IgnoresAlpha { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSColorList {
		[Static]
		[Export ("availableColorLists")]
		NSColorList [] AvailableColorLists { get; }

		[Static]
		[Export ("colorListNamed:")]
		NSColorList ColorListNamed (string name);

		[Export ("initWithName:")]
		IntPtr Constructor (string name);

		[Export ("initWithName:fromFile:")]
		IntPtr Constructor (string name, string path);

		[Export ("name")]
		string Name { get; }

		[Export ("setColor:forKey:")]
		void SetColorForKey (NSColor color, string key);

		[Export ("insertColor:key:atIndex:")]
		void InsertColor (NSColor color, string key, int indexPos);

		[Export ("removeColorWithKey:")]
		void RemoveColor (string key);

		[Export ("colorWithKey:")]
		NSColor ColorWithKey (string key);

		[Export ("allKeys")]
		string [] AllKeys ();

		[Export ("isEditable")]
		bool IsEditable { get; }

		[Export ("writeToFile:")]
		bool WriteToFile (string path);

		[Export ("removeFile")]
		void RemoveFile ();
	}

	[BaseType (typeof (NSPanel))]
	interface NSColorPanel {
		[Static, Export ("sharedColorPanel")]
		NSColorPanel SharedColorPanel { get; }

		[Static]
		[Export ("sharedColorPanelExists")]
		bool SharedColorPanelExists { get; }

		[Static]
		[Export ("dragColor:withEvent:fromView:")]
		bool DragColorwithEventfromView (NSColor color, NSEvent theEvent, NSView sourceView);

		[Static]
		[Export ("setPickerMask:")]
		void SetPickerStyle (NSColorPanelFlags mask);

		[Static]
		[Export ("setPickerMode:")]
		void SetPickerMode (NSColorPanelMode mode);

		[Export ("alpha")]
		float Alpha { get; }

		[Export ("setAction:")]
		void SetAction (Selector aSelector);

		[Export ("setTarget:")]
		void SetTarget (NSObject anObject);

		[Export ("attachColorList:")]
		void AttachColorList (NSColorList colorList);

		[Export ("detachColorList:")]
		void DetachColorList (NSColorList colorList);

		//Detected properties
		[Export ("accessoryView")]
		NSView AccessoryView { get; set; }

		[Export ("continuous")]
		bool Continuous { [Bind ("isContinuous")]get; set; }

		[Export ("showsAlpha")]
		bool ShowsAlpha { get; set; }

		[Export ("mode")]
		NSColorPanelFlags Mode { get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

	}

	[BaseType (typeof (NSObject))]
	interface NSColorPicker {
		[Export ("initWithPickerMask:colorPanel:")]
		IntPtr Constructor (NSColorPanelFlags mask, NSColorPanel owningColorPanel);

		[Export ("colorPanel")]
		NSColorPanel ColorPanel { get; }

		[Export ("provideNewButtonImage")]
		NSImage ProvideNewButtonImage ();

		[Export ("insertNewButtonImage:in:")]
		void InsertNewButtonImage (NSImage newButtonImage, NSButtonCell buttonCell);

		[Export ("viewSizeChanged:")]
		void ViewSizeChanged (NSObject sender);

		[Export ("attachColorList:")]
		void AttachColorList (NSColorList colorList);

		[Export ("detachColorList:")]
		void DetachColorList (NSColorList colorList);

		[Export ("setMode:")]
		void SetMode (NSColorPanelMode mode);

		[Export ("buttonToolTip")]
		string ButtonToolTip { get; }

		[Export ("minContentSize")]
		SizeF MinContentSize { get; }
	}

	[BaseType (typeof (NSObject))]
	interface NSColorSpace {
		[Export ("initWithICCProfileData:")]
		IntPtr Constructor (NSData iccData);

		[Export ("ICCProfileData")]
		NSData ICCProfileData { get; }

		// Conflicts with the built-in handle intptr
		//[Export ("initWithColorSyncProfile:")]
		//IntPtr Constructor (IntPtr colorSyncProfile);

		[Export ("colorSyncProfile")]
		IntPtr ColorSyncProfile { get; }

		[Export ("initWithCGColorSpace:")]
		IntPtr Constructor (MonoMac.CoreGraphics.CGColorSpace cgColorSpace);

		[Export ("CGColorSpace")]
		MonoMac.CoreGraphics.CGColorSpace ColorSpace { get; }

		[Export ("numberOfColorComponents")]
		int ColorComponents { get; }

		[Export ("colorSpaceModel")]
		NSColorSpaceModel ColorSpaceModel { get; }

		[Export ("localizedName")]
		string LocalizedName { get; }

		[Static]
		[Export ("genericRGBColorSpace")]
		NSColorSpace GenericRGBColorSpace { get; }

		[Static]
		[Export ("genericGrayColorSpace")]
		NSColorSpace GenericGrayColorSpace { get; }

		[Static]
		[Export ("genericCMYKColorSpace")]
		NSColorSpace GenericCMYKColorSpace { get; }

		[Static]
		[Export ("deviceRGBColorSpace")]
		NSColorSpace DeviceRGBColorSpace { get; }

		[Static]
		[Export ("deviceGrayColorSpace")]
		NSColorSpace DeviceGrayColorSpace { get; }

		[Static]
		[Export ("deviceCMYKColorSpace")]
		NSColorSpace DeviceCMYKColorSpace { get; }

		[Static]
		[Export ("sRGBColorSpace")]
		NSColorSpace SRGBColorSpace { get; }

		[Static]
		[Export ("genericGamma22GrayColorSpace")]
		NSColorSpace GenericGamma22GrayColorSpace { get; }

		[Static]
		[Export ("adobeRGB1998ColorSpace")]
		NSColorSpace AdobeRGB1998ColorSpace { get; }

		[Static]
		[Export ("availableColorSpacesWithModel:")]
		NSColorSpace [] AvailableColorSpacesWithModel (NSColorSpaceModel model);
	}

	[BaseType (typeof (NSControl))]
	interface NSColorWell {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("deactivate")]
		void Deactivate ();

		[Export ("activate:")]
		void Activate (bool exclusive);

		[Export ("isActive")]
		bool IsActive { get; }

		[Export ("drawWellInside:")]
		void DrawWellInside (RectangleF insideRect);

		[Export ("takeColorFrom:")]
		void TakeColorFrom (NSObject sender);

		//Detected properties
		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")]get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

	}

	[BaseType (typeof (NSView))]
	interface NSControl {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("calcSize")]
		void CalcSize ();

		[Export ("selectedCell")]
		NSCell SelectedCell { get; }

		[Export ("selectedTag")]
		int SelectedTag { get; }

		[Export ("sendActionOn:")]
		int SendActionOn (int mask);

		[Export ("setNeedsDisplay")]
		void SetNeedsDisplay ();

		[Export ("updateCell:")]
		void UpdateCell (NSCell aCell);

		[Export ("updateCellInside:")]
		void UpdateCellInside (NSCell aCell);

		[Export ("drawCellInside:")]
		void DrawCellInside (NSCell aCell);

		[Export ("drawCell:")]
		void DrawCell (NSCell aCell);

		[Export ("selectCell:")]
		void SelectCell (NSCell aCell);

		[Export ("sendAction:to:")]
		bool SendAction (Selector theAction, NSObject theTarget);

		[Export ("takeIntValueFrom:")]
		void TakeIntValueFrom (NSObject sender);

		[Export ("takeFloatValueFrom:")]
		void TakeFloatValueFrom (NSObject sender);

		[Export ("takeDoubleValueFrom:")]
		void TakeDoubleValueFrom (NSObject sender);

		[Export ("takeStringValueFrom:")]
		void TakeStringValueFrom (NSObject sender);

		[Export ("takeObjectValueFrom:")]
		void TakeObjectValueFrom (NSObject sender);

		[Export ("currentEditor")]
		NSText CurrentEditor { get; }

		[Export ("abortEditing")]
		bool AbortEditing ();

		[Export ("validateEditing")]
		void ValidateEditing ();

		[Export ("mouseDown:")]
		void MouseDown (NSEvent theEvent);

		[Export ("takeIntegerValueFrom:")]
		void TakeIntegerValueFrom (NSObject sender);

		//Detected properties
		[Static]
		[Export ("cellClass")]
		Class CellClass { get; set; }

		[Export ("cell")]
		NSCell Cell { get; set; }

		[Export ("target")]
		NSObject Target { get; set; }

		[Export ("action")]
		Selector Action { get; set; }

		[Export ("tag")]
		int Tag { get; set; }

		[Export ("ignoresMultiClick")]
		bool IgnoresMultiClick { get; set; }

		[Export ("continuous")]
		bool Continuous { [Bind ("isContinuous")]get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("alignment")]
		uint Alignment { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("formatter")]
		NSObject Formatter { get; set; }

		[Export ("objectValue")]
		NSObject ObjectValue { get; set; }

		[Export ("stringValue")]
		string StringValue { get; set; }

		[Export ("intValue")]
		int IntValue { get; set; }

		[Export ("floatValue")]
		float FloatValue { get; set; }

		[Export ("doubleValue")]
		double DoubleValue { get; set; }

		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; set; }

		[Export ("integerValue")]
		int IntegerValue { get; set; }

		[Export ("performClick:")]
		void PerformClick (NSObject sender);

		[Export ("refusesFirstResponder")]
		bool RefusesFirstResponder { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSCursor {
		[Static]
		[Export ("currentCursor")]
		NSCursor CurrentCursor { get; }

		[Static]
		[Export ("currentSystemCursor")]
		NSCursor CurrentSystemCursor { get; }

		[Static]
		[Export ("arrowCursor")]
		NSCursor ArrowCursor { get; }

		[Static]
		[Export ("IBeamCursor")]
		NSCursor IBeamCursor { get; }

		[Static]
		[Export ("pointingHandCursor")]
		NSCursor PointingHandCursor { get; }

		[Static]
		[Export ("closedHandCursor")]
		NSCursor ClosedHandCursor { get; }

		[Static]
		[Export ("openHandCursor")]
		NSCursor OpenHandCursor { get; }

		[Static]
		[Export ("resizeLeftCursor")]
		NSCursor ResizeLeftCursor { get; }

		[Static]
		[Export ("resizeRightCursor")]
		NSCursor ResizeRightCursor { get; }

		[Static]
		[Export ("resizeLeftRightCursor")]
		NSCursor ResizeLeftRightCursor { get; }

		[Static]
		[Export ("resizeUpCursor")]
		NSCursor ResizeUpCursor { get; }

		[Static]
		[Export ("resizeDownCursor")]
		NSCursor ResizeDownCursor { get; }

		[Static]
		[Export ("resizeUpDownCursor")]
		NSCursor ResizeUpDownCursor { get; }

		[Static]
		[Export ("crosshairCursor")]
		NSCursor CrosshairCursor { get; }

		[Static]
		[Export ("disappearingItemCursor")]
		NSCursor DisappearingItemCursor { get; }

		[Static]
		[Export ("operationNotAllowedCursor")]
		NSCursor OperationNotAllowedCursor { get; }

		[Static]
		[Export ("dragLinkCursor")]
		NSCursor DragLinkCursor { get; }

		[Static]
		[Export ("dragCopyCursor")]
		NSCursor DragCopyCursor { get; }

		[Static]
		[Export ("contextualMenuCursor")]
		NSCursor ContextualMenuCursor { get; }

		[Export ("initWithImage:hotSpot:")]
		NSObject InitWithImagehotSpot (NSImage newImage, PointF aPoint);

		[Export ("initWithImage:foregroundColorHint:backgroundColorHint:hotSpot:")]
		IntPtr Constructor (NSImage newImage, NSColor fg, NSColor bg, PointF hotSpot);

		[Static]
		[Export ("hide")]
		void Hide ();

		[Static]
		[Export ("unhide")]
		void Unhide ();

		[Static]
		[Export ("setHiddenUntilMouseMoves:")]
		void SetHiddenUntilMouseMoves (bool flag);

		//[Static]
		//[Export ("pop")]
		//void Pop ();

		[Export ("image")]
		NSImage Image { get; }

		[Export ("hotSpot")]
		PointF HotSpot { get; }

		[Export ("push")]
		void Push ();

		[Export ("pop")]
		void Pop ();

		[Export ("set")]
		void Set ();

		[Export ("setOnMouseExited:")]
		void SetOnMouseExited (bool flag);

		[Export ("setOnMouseEntered:")]
		void SetOnMouseEntered (bool flag);

		[Export ("isSetOnMouseExited")]
		bool IsSetOnMouseExited ();

		[Export ("isSetOnMouseEntered")]
		bool IsSetOnMouseEntered ();

		[Export ("mouseEntered:")]
		void MouseEntered (NSEvent theEvent);

		[Export ("mouseExited:")]
		void MouseExited (NSEvent theEvent);
	}

	[BaseType (typeof (NSImageRep))]
	interface NSCustomImageRep {
		[Export ("initWithDrawSelector:delegate:")]
		IntPtr Constructor (Selector drawSelectorMethod, NSObject delegateObject);

		[Export ("drawSelector")]
		Selector DrawSelector { get; }
		
		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]  
		NSObject WeakDelegate { get; set; }  
		
		[Wrap ("WeakDelegate")][NullAllowed]  
		NSObject Delegate { get; set; }  

	}	
	
	[BaseType (typeof (NSObject))]
	interface NSDocument {
		[Export ("initWithType:error:")]
		IntPtr Constructor (string typeName, NSError outError);

		[Export ("canConcurrentlyReadDocumentsOfType:")]
		bool CanConcurrentlyReadDocumentsOfType (string typeName);

		// Binding out error
		//[Export ("initWithContentsOfURL:ofType:error:")]
		//IntPtr Constructor (NSUrl absoluteURL, string typeName, NSError outError);
		//
		//[Export ("initForURL:withContentsOfURL:ofType:error:")]
		//IntPtr Constructor (NSUrl absoluteDocumentUrl, NSUrl absoluteDocumentContentsUrl, string typeName, NSError outError);

		// [Export ("revertDocumentToSaved:")]
		// void RevertDocumentToSaved (NSObject sender);
		// 
		// [Export ("revertToContentsOfURL:ofType:error:")]
		// bool RevertToContentsOfUrl (NSUrl absoluteURL, string typeName, NSError outError);
		// 
		// [Export ("readFromURL:ofType:error:")]
		// bool ReadFromUrl (NSUrl absoluteURL, string typeName, NSError outError);
		// 
		// [Export ("readFromFileWrapper:ofType:error:")]
		// bool ReadFromFileWrapper (NSFileWrapper fileWrapper, string typeName, NSError outError);
		// 
		// [Export ("readFromData:ofType:error:")]
		// bool ReadFromData (NSData data, string typeName, NSError outError);

		//[Export ("writeToURL:ofType:error:")]
		//bool WriteToUrl (NSUrl absoluteURL, string typeName, NSError outError);

		// BINDING out error
		//[Export ("fileWrapperOfType:error:")]
		//NSFileWrapper FileWrapper (string typeName, NSError outError);

		//[Export ("dataOfType:error:")]
		//NSData DataOfType (string typeName, NSError outError);
		//
		//[Export ("writeSafelyToURL:ofType:forSaveOperation:error:")]
		//bool WriteSafelyToUrl (NSUrl absoluteURL, string typeName, NSSaveOperationType saveOperation, NSError outError);
		//
		//[Export ("writeToURL:ofType:forSaveOperation:originalContentsURL:error:")]
		//bool WriteToUrl (NSUrl absoluteURL, string typeName, NSSaveOperationType saveOperation, NSUrl absoluteOriginalContentsURL, NSError outError);
		//
		//[Export ("fileAttributesToWriteToURL:ofType:forSaveOperation:originalContentsURL:error:")]
		//NSDictionary FileAttributes (NSUrl absoluteURL, string typeName, NSSaveOperationType saveOperation, NSUrl absoluteOriginalContentsURL, NSError outError);

		[Export ("keepBackupFile")]
		bool KeepBackupFile ();

		[Export ("saveDocument:")]
		void SaveDocument (NSObject sender);

		[Export ("saveDocumentAs:")]
		void SaveDocumentAs (NSObject sender);

		[Export ("saveDocumentTo:")]
		void SaveDocumentTo (NSObject sender);

		[Export ("saveDocumentWithDelegate:didSaveSelector:contextInfo:")]
		void SaveDocument (NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("runModalSavePanelForSaveOperation:delegate:didSaveSelector:contextInfo:")]
		void RunModalSavePanelForSaveOperation (NSSaveOperationType saveOperation, NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("shouldRunSavePanelWithAccessoryView")]
		bool ShouldRunSavePanelWithAccessoryView { get; }

		[Export ("prepareSavePanel:")]
		bool PrepareSavePanel (NSSavePanel savePanel);

		[Export ("fileNameExtensionWasHiddenInLastRunSavePanel")]
		bool FileNameExtensionWasHiddenInLastRunSavePanel { get; }

		[Export ("fileTypeFromLastRunSavePanel")]
		string FileTypeFromLastRunSavePanel { get; }

		[Export ("saveToURL:ofType:forSaveOperation:delegate:didSaveSelector:contextInfo:")]
		void SaveToUrl (NSUrl absoluteURL, string typeName, NSSaveOperationType saveOperation, NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("saveToURL:ofType:forSaveOperation:error:")]
		bool SaveToUrl (NSUrl absoluteURL, string typeName, NSSaveOperationType saveOperation, NSError outError);

		[Export ("hasUnautosavedChanges")]
		bool HasUnautosavedChanges { get; }

		[Export ("autosaveDocumentWithDelegate:didAutosaveSelector:contextInfo:")]
		void AutosaveDocument (NSObject delegateObject, Selector didAutosaveSelector, IntPtr contextInfo);

		[Export ("autosavingFileType")]
		string AutosavingFileType { get; }

		[Export ("canCloseDocumentWithDelegate:shouldCloseSelector:contextInfo:")]
		void CanCloseDocument (NSObject delegateObject, Selector shouldCloseSelector, IntPtr contextInfo);

		[Export ("close")]
		void Close ();

		[Export ("runPageLayout:")]
		void RunPageLayout (NSObject sender);

		[Export ("runModalPageLayoutWithPrintInfo:delegate:didRunSelector:contextInfo:")]
		void RunModalPageLayout (NSPrintInfo printInfo, NSObject delegateObject, Selector didRunSelector, IntPtr contextInfo);

		[Export ("preparePageLayout:")]
		bool PreparePageLayout (NSPageLayout pageLayout);

		[Export ("shouldChangePrintInfo:")]
		bool ShouldChangePrintInfo (NSPrintInfo newPrintInfo);

		[Export ("printDocument:")]
		void PrintDocument (NSObject sender);

		[Export ("printDocumentWithSettings:showPrintPanel:delegate:didPrintSelector:contextInfo:")]
		void PrintDocument (NSDictionary printSettings, bool showPrintPanel, NSObject delegateObject, Selector didPrintSelector, IntPtr contextInfo);

		[Export ("printOperationWithSettings:error:")]
		NSPrintOperation PrintOperation (NSDictionary printSettings, NSError outError);

		[Export ("runModalPrintOperation:delegate:didRunSelector:contextInfo:")]
		void RunModalPrintOperation (NSPrintOperation printOperation, NSObject delegateObject, Selector didRunSelector, IntPtr contextInfo);

		[Export ("isDocumentEdited")]
		bool IsDocumentEdited { get; }

		[Export ("updateChangeCount:")]
		void UpdateChangeCount (NSDocumentChangeType change);

		[Export ("presentError:modalForWindow:delegate:didPresentSelector:contextInfo:")]
		void PresentError (NSError error, NSWindow window, NSObject delegateObject, Selector didPresentSelector, IntPtr contextInfo);

		[Export ("presentError:")]
		bool PresentError (NSError error);

		[Export ("willPresentError:")]
		NSError WillPresentError (NSError error);

		[Export ("makeWindowControllers")]
		void MakeWindowControllers ();

		[Export ("windowNibName")]
		string WindowNibName { get; }

		[Export ("windowControllerWillLoadNib:")]
		void WindowControllerWillLoadNib (NSWindowController windowController);

		[Export ("windowControllerDidLoadNib:")]
		void WindowControllerDidLoadNib (NSWindowController windowController);

		[Export ("setWindow:")]
		void SetWindow (NSWindow window);

		[Export ("addWindowController:")]
		void AddWindowController (NSWindowController windowController);

		[Export ("removeWindowController:")]
		void RemoveWindowController (NSWindowController windowController);

		[Export ("showWindows")]
		void ShowWindows ();

		[Export ("windowControllers")]
		NSWindowController [] WindowControllers { get; }

		[Export ("shouldCloseWindowController:delegate:shouldCloseSelector:contextInfo:")]
		void ShouldCloseWindowController (NSWindowController windowController, NSObject delegateObject, Selector shouldCloseSelector, IntPtr contextInfo);

		[Export ("displayName")]
		string DisplayName { get; }

		[Export ("windowForSheet")]
		NSWindow WindowForSheet { get; }

		[Static, Export ("readableTypes")]
		string [] ReadableTypes { get; }

		[Static]
		[Export ("writableTypes")]
		string [] WritableTypes ();

		[Static]
		[Export ("isNativeType:")]
		bool IsNativeType (string type);

		[Export ("writableTypesForSaveOperation:")]
		string [] WritableTypesForSaveOperation (NSSaveOperationType saveOperation);

		[Export ("fileNameExtensionForType:saveOperation:")]
		string FileNameExtensionForSaveOperation (string typeName, NSSaveOperationType saveOperation);

		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject /* Must implement NSValidatedUserInterfaceItem */ anItem);

		//Detected properties
		[Export ("fileType")]
		string FileType { get; set; }

		[Export ("fileURL")]
		NSUrl FileUrl { get; set; }

		[Export ("fileModificationDate")]
		NSDate FileModificationDate { get; set; }

		[Export ("autosavedContentsFileURL")]
		NSUrl AutosavedContentsFileURL { get; set; }

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; set; }

		[Export ("undoManager")]
		NSUndoManager UndoManager { get; set; }

		[Export ("hasUndoManager")]
		bool HasUndoManager { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSDocumentController {
		[Static, Export ("sharedDocumentController")]
		NSObject SharedDocumentController { get; }

		[Export ("documents")]
		NSDocument [] Documents { get; }

		[Export ("currentDocument")]
		NSDocument CurrentDocument { get; }

		[Export ("currentDirectory")]
		string CurrentDirectory { get; }

		[Export ("documentForURL:")]
		NSDocument DocumentForUrl (NSUrl absoluteURL);

		[Export ("documentForWindow:")]
		NSDocument DocumentForWindow (NSWindow window);

		[Export ("addDocument:")]
		void AddDocument (NSDocument document);

		[Export ("removeDocument:")]
		void RemoveDocument (NSDocument document);

		[Export ("newDocument:")]
		void NewDocument (NSObject sender);

		//[Export ("openUntitledDocumentAndDisplay:error:"), Internal]
		//NSObject OpenUntitledDocument (bool displayDocument, IntPtr outError);

		//[Export ("makeUntitledDocumentOfType:error:"), Internal]
		//NSObject MakeUntitledDocument (string typeName, IntPtr outError);

		[Export ("openDocument:")]
		void OpenDocument (NSObject sender);

		[Export ("URLsFromRunningOpenPanel")]
		NSUrl [] UrlsFromRunningOpenPanel ();

		[Export ("runModalOpenPanel:forTypes:")]
		int RunModalOpenPanelforTypes (NSOpenPanel openPanel, string [] types);

		[Export ("openDocumentWithContentsOfURL:display:error:")]
		NSObject OpenDocument (NSUrl absoluteURL, bool displayDocument, NSError outError);

		[Export ("makeDocumentWithContentsOfURL:ofType:error:")]
		NSObject MakeDocument (NSUrl absoluteURL, string typeName, NSError outError);

		[Export ("reopenDocumentForURL:withContentsOfURL:error:")]
		bool ReopenDocument (NSUrl absoluteDocumentURL, NSUrl absoluteDocumentContentsURL, NSError outError);

		[Export ("makeDocumentForURL:withContentsOfURL:ofType:error:")]
		NSObject MakeDocument (NSUrl absoluteDocumentURL, NSUrl absoluteDocumentContentsURL, string typeName, NSError outError);

		[Export ("saveAllDocuments:")]
		void SaveAllDocuments (NSObject sender);

		[Export ("hasEditedDocuments")]
		bool HasEditedDocuments { get; }

		[Export ("reviewUnsavedDocumentsWithAlertTitle:cancellable:delegate:didReviewAllSelector:contextInfo:")]
		void ReviewUnsavedDocuments (string title, bool cancellable, NSObject delegateObject, Selector didReviewAllSelector, IntPtr contextInfo);

		[Export ("closeAllDocumentsWithDelegate:didCloseAllSelector:contextInfo:")]
		void CloseAllDocuments (NSObject delegateObject, Selector didCloseAllSelector, IntPtr contextInfo);

		[Export ("presentError:modalForWindow:delegate:didPresentSelector:contextInfo:")]
		void PresentError (NSError error, NSWindow window, NSObject delegateObject, Selector didPresentSelector, IntPtr contextInfo);

		[Export ("presentError:")]
		bool PresentError (NSError error);

		[Export ("willPresentError:")]
		NSError WillPresentError (NSError error);

		[Export ("maximumRecentDocumentCount")]
		int MaximumRecentDocumentCount { get; }

		[Export ("clearRecentDocuments:")]
		void ClearRecentDocuments (NSObject sender);

		[Export ("noteNewRecentDocument:")]
		void NoteNewRecentDocument (NSDocument document);

		[Export ("noteNewRecentDocumentURL:")]
		void NoteNewRecentDocumentURL (NSUrl absoluteURL);

		[Export ("recentDocumentURLs")]
		NSUrl [] RecentDocumentURLs { get; }

		[Export ("defaultType")]
		string DefaultType { get; }

		[Export ("typeForContentsOfURL:error:")]
		string TypeForUrl (NSUrl inAbsoluteUrl, NSError outError);

		[Export ("documentClassNames")]
		string [] DocumentClassNames  {get; }

		[Export ("documentClassForType:")]
		Class DocumentClassForType (string typeName);

		[Export ("displayNameForType:")]
		string DisplayNameForType (string typeName);

		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject /* must implement NSValidatedUserInterfaceItem */ anItem);

		//Detected properties
		[Export ("autosavingDelay")]
		double AutosavingDelay { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSDraggingInfo {
		[Abstract]
		[Export ("draggingSourceOperationMask")]
		NSDragOperation DraggingSourceOperationMask { get; }

		[Abstract]
		[Export ("draggingLocation")]
		PointF DraggingLocation { get; }

		[Abstract]
		[Export ("draggedImageLocation")]
		PointF DraggedImageLocation { get; }

		[Abstract]
		[Export ("draggedImage")]
		NSImage DraggedImage { get; }

		[Abstract]
		[Export ("draggingPasteboard")]
		NSPasteboard DraggingPasteboard { get; }

		[Abstract]
		[Export ("draggingSource")]
		NSObject DraggingSource { get; }

		[Abstract]
		[Export ("draggingSequenceNumber")]
		int DraggingSequenceNumber { get; }

		[Abstract]
		[Export ("slideDraggedImageTo:")]
		void SlideDraggedImageTo (PointF screenPoint);

		[Abstract]
		[Export ("namesOfPromisedFilesDroppedAtDestination:")]
		string [] romisedFilesDroppedAtDestination (NSUrl dropDestination);
	}

	[BaseType (typeof (NSObject))]
	interface NSFileWrapper {
		// FIXME: Binding out error
		//[Export ("initWithURL:options:error:")]
		//IntPtr Constructor (NSUrl url, NSFileWrapperReadingOptions options, out NSError outError);

		[Export ("initDirectoryWithFileWrappers:")]
		IntPtr Constructor (NSDictionary childrenByPreferredName);

		[Export ("initRegularFileWithContents:")]
		IntPtr Constructor (NSData contents);

		[Export ("initSymbolicLinkWithDestinationURL:")]
		IntPtr Constructor (NSUrl url);

		//[Export ("initWithSerializedRepresentation:")]
		//IntPtr Constructor (NSData serializeRepresentation);

		[Export ("isDirectory")]
		bool IsDirectory { get; }

		[Export ("isRegularFile")]
		bool IsRegularFile { get; }

		[Export ("isSymbolicLink")]
		bool IsSymbolicLink { get; }

		[Export ("matchesContentsOfURL:")]
		bool MatchesContentsOfURL (NSUrl url);

		// FIXME: bind out
		//[Export ("readFromURL:options:error:")]
		//bool ReadFrom (NSUrl url, NSFileWrapperReadingOptions options, out NSError outError);

		// FIXME: bind out
		//[Export ("writeToURL:options:originalContentsURL:error:")]
		//bool WriteTo (NSUrl url, NSFileWrapperWritingOptions options, NSUrl originalContentsURL, out NSError outError, );

		[Export ("serializedRepresentation")]
		NSData SerializedRepresentation { get; }

		[Export ("addFileWrapper:")]
		string AddFileWrapper (NSFileWrapper child);

		[Export ("addRegularFileWithContents:preferredFilename:")]
		string AddRegularFile (NSData data, string fileName);

		[Export ("removeFileWrapper:")]
		void RemoveFileWrapper (NSFileWrapper child);

		[Export ("fileWrappers")]
		NSDictionary FileWrappers { get; }

		[Export ("keyForFileWrapper:")]
		string KeyForFileWrapper (NSFileWrapper child);

		[Export ("regularFileContents")]
		NSData RegularFileContents { get; }

		[Export ("symbolicLinkDestinationURL")]
		NSUrl SymbolicLinkDestinationUrl { get; }

		//Detected properties
		[Export ("preferredFilename")]
		string PreferredFilename { get; set; }

		[Export ("filename")]
		string Filename { get; set; }

		[Export ("fileAttributes")]
		NSDictionary FileAttributes { get; set; }

		[Export ("icon")]
		NSImage Icon { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSFont {
		[Static]
		[Export ("fontWithName:size:")]
		NSFont FromFontName (string fontName, float fontSize);

		//[Static]
		//[Export ("fontWithName:matrix:")]
		//NSFont FromFontName (string fontName, float [] fontMatrix);

		[Export ("fontWithDescriptor:size:")]
		NSFont FromDescription (NSFontDescriptor fontDescriptor, float fontSize);

		[Static]
		[Export ("fontWithDescriptor:textTransform:")]
		NSFont FromDescription (NSFontDescriptor fontDescriptor, NSAffineTransform textTransform);

		[Export ("userFontOfSize:")]
		NSFont UserFontOfSize (float fontSize);

		[Static]
		[Export ("userFixedPitchFontOfSize:")]
		NSFont UserFixedPitchFontOfSize (float fontSize);

		[Static]
		[Export ("setUserFont:")]
		void SetUserFont (NSFont aFont);

		[Static]
		[Export ("setUserFixedPitchFont:")]
		void SetUserFixedPitchFont (NSFont aFont);

		[Static]
		[Export ("systemFontOfSize:")]
		NSFont SystemFontOfSize (float fontSize);

		[Static]
		[Export ("boldSystemFontOfSize:")]
		NSFont BoldSystemFontOfSize (float fontSize);

		[Static]
		[Export ("labelFontOfSize:")]
		NSFont LabelFontOfSize (float fontSize);

		[Static]
		[Export ("titleBarFontOfSize:")]
		NSFont TitleBarFontOfSize (float fontSize);

		[Static]
		[Export ("menuFontOfSize:")]
		NSFont MenuFontOfSize (float fontSize);

		[Export ("menuBarFontOfSize:")]
		NSFont MenuBarFontOfSize (float fontSize);

		[Export ("messageFontOfSize:")]
		NSFont MessageFontOfSize (float fontSize);

		[Static]
		[Export ("paletteFontOfSize:")]
		NSFont PaletteFontOfSize (float fontSize);

		[Static]
		[Export ("toolTipsFontOfSize:")]
		NSFont ToolTipsFontOfSize (float fontSize);

		[Static]
		[Export ("controlContentFontOfSize:")]
		NSFont ControlContentFontOfSize (float fontSize);

		[Static]
		[Export ("systemFontSize")]
		float SystemFontSize { get; }

		[Static]
		[Export ("smallSystemFontSize")]
		float SmallSystemFontSize { get; }

		[Static]
		[Export ("labelFontSize")]
		float LabelFontSize { get; }

		[Export ("systemFontSizeForControlSize:")]
		float SystemFontSizeForControlSize (NSControlSize controlSize);

		[Export ("fontName")]
		string FontName { get; }

		[Export ("pointSize")]
		float PointSize { get; }

		//[Export ("matrix")]
		//  FIXME
		//IntPtr *float Matrix { get; }

		[Export ("familyName")]
		string FamilyName { get; }

		[Export ("displayName")]
		string DisplayName { get; }

		[Export ("fontDescriptor")]
		NSFontDescriptor FontDescriptor { get; }

		[Export ("textTransform")]
		NSAffineTransform TextTransform { get; }

		[Export ("numberOfGlyphs")]
		int GlyphCount { get; }

		[Export ("mostCompatibleStringEncoding")]
		NSStringEncoding MostCompatibleStringEncoding { get; }

		[Export ("glyphWithName:")]
		uint GlyphWithName (string aName);

		[Export ("coveredCharacterSet")]
		NSCharacterSet CoveredCharacterSet { get; }

		[Export ("boundingRectForFont")]
		RectangleF BoundingRectForFont { get; }

		[Export ("maximumAdvancement")]
		SizeF MaximumAdvancement { get; }

		[Export ("ascender")]
		float Ascender { get; }

		[Export ("descender")]
		float Descender { get; }

		[Export ("leading")]
		float Leading { get; }

		[Export ("underlinePosition")]
		float UnderlinePosition { get; }

		[Export ("underlineThickness")]
		float UnderlineThickness { get; }

		[Export ("italicAngle")]
		float ItalicAngle { get; }

		[Export ("capHeight")]
		float CapHeight { get; }

		[Export ("xHeight")]
		float XHeight { get; }

		[Export ("isFixedPitch")]
		bool IsFixedPitch { get; }

		[Export ("boundingRectForGlyph:")]
		RectangleF BoundingRectForGlyph (uint aGlyph);

		[Export ("advancementForGlyph:")]
		SizeF AdvancementForGlyph (uint ag);

		// FIXME binding
		//[Export ("getBoundingRects:forGlyphs:count:")]
		//void GetBoundingRectsforGlyphscount (NSRect *bounds, uint glyphs, int glyphCount);

		// FIXME binding
		//[Export ("getAdvancements:forGlyphs:count:")]
		//void GetAdvancementsforGlyphscount (NSSizeArray advancements, const uint glyphs, int glyphCount);

		// FIXME binding
		//[Export ("getAdvancements:forPackedGlyphs:length:")]
		//void GetAdvancementsforPackedGlyphslength (NSSizeArray advancements, void *packedGlyphs, uint length);

		[Export ("set")]
		void Set ();

		[Export ("setInContext:")]
		void SetInContext (NSGraphicsContext graphicsContext);

		[Export ("printerFont")]
		NSFont PrinterFont { get; }

		[Export ("screenFont")]
		NSFont ScreenFont { get; }

		[Export ("screenFontWithRenderingMode:")]
		NSFont ScreenFontWithRenderingMode (NSFontRenderingMode renderingMode);

		[Export ("renderingMode")]
		NSFontRenderingMode RenderingMode { get; }
	}


	[BaseType (typeof (NSObject))]
	interface NSFontDescriptor {
		[Export ("postscriptName")]
		string PostscriptName { get; }

		[Export ("pointSize")]
		float PointSize { get; }

		[Export ("matrix")]
		NSAffineTransform Matrix { get; }

		[Export ("symbolicTraits")]
		NSFontSymbolicTraits SymbolicTraits { get; }

		[Export ("objectForKey:")]
		NSObject ObjectForKey (string key);

		[Export ("fontAttributes")]
		NSDictionary FontAttributes { get; }

		[Static]
		[Export ("fontDescriptorWithFontAttributes:")]
		NSFontDescriptor FromAttributes (NSDictionary attributes);

		[Static]
		[Export ("fontDescriptorWithName:size:")]
		NSFontDescriptor FromNameSize (string fontName, float size);

		[Export ("fontDescriptorWithName:matrix:")]
		NSFontDescriptor FromNameMatrix (string fontName, NSAffineTransform matrix);

		[Export ("initWithFontAttributes:")]
		IntPtr Constructor (NSDictionary attributes);

		[Export ("matchingFontDescriptorsWithMandatoryKeys:")]
		NSFontDescriptor [] MatchingFontDescriptors (NSSet mandatoryKeys);

		[Export ("matchingFontDescriptorWithMandatoryKeys:")]
		NSFontDescriptor MatchingFontDescriptorWithMandatoryKeys (NSSet mandatoryKeys);

		[Export ("fontDescriptorByAddingAttributes:")]
		NSFontDescriptor FontDescriptorByAddingAttributes (NSDictionary attributes);

		[Export ("fontDescriptorWithSymbolicTraits:")]
		NSFontDescriptor FontDescriptorWithSymbolicTraits (NSFontSymbolicTraits symbolicTraits);

		[Export ("fontDescriptorWithSize:")]
		NSFontDescriptor FontDescriptorWithSize (float newPointSize);

		[Export ("fontDescriptorWithMatrix:")]
		NSFontDescriptor FontDescriptorWithMatrix (NSAffineTransform matrix);

		[Export ("fontDescriptorWithFace:")]
		NSFontDescriptor FontDescriptorWithFace (string newFace);

		[Export ("fontDescriptorWithFamily:")]
		NSFontDescriptor FontDescriptorWithFamily (string newFamily);
	}

	[BaseType (typeof (NSImageRep))]
	interface NSEPSImageRep {
		[Static]
		[Export ("imageRepWithData:")]
		NSObject FromData (NSData epsData);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData epsData);

		[Export ("prepareGState")]
		void PrepareGState ();

		[Export ("EPSRepresentation")]
		NSData EPSRepresentation { get; }

		[Export ("boundingBox")]
		RectangleF BoundingBox { get; }
	}
	
	[BaseType (typeof (NSObject))]
	interface NSEvent {
		[Export ("type")]
		NSEventType Type { get; }

		// TODO: TYPE FOR THIS?
		[Export ("modifierFlags")]
		int ModifierFlags { get; }

		[Export ("timestamp")]
		double Timestamp { get; }

		[Export ("window")]
		NSWindow Window { get; }

		[Export ("windowNumber")]
		int WindowNumber { get; }

		[Export ("context")]
		NSGraphicsContext Context { get; }

		[Export ("clickCount")]
		int ClickCount { get; }

		[Export ("buttonNumber")]
		int ButtonNumber { get; }

		[Export ("eventNumber")]
		int EventNumber { get; }

		[Export ("pressure")]
		float Pressure { get; }

		[Export ("locationInWindow")]
		PointF LocationInWindow { get; }

		[Export ("deltaX")]
		float DeltaX { get; }

		[Export ("deltaY")]
		float DeltaY { get; }

		[Export ("deltaZ")]
		float DeltaZ { get; }

		[Export ("characters")]
		string Characters { get; }

		[Export ("charactersIgnoringModifiers")]
		string CharactersIgnoringModifiers { get; }

		[Export ("isARepeat")]
		bool IsARepeat { get; }

		[Export ("keyCode")]
		ushort KeyCode { get; }

		[Export ("trackingNumber")]
		int TrackingNumber { get; }

		[Export ("userData")]
		IntPtr UserData { get; }

		[Export ("trackingArea")]
		NSTrackingArea TrackingArea { get; }

		[Export ("subtype")]
		short Subtype { get; }

		[Export ("data1")]
		int Data1 { get; }

		[Export ("data2")]
		int Data2 { get; }

		//[Export ("eventRef")]
		//const void * EventRef ();

		[Static]
		[Export ("eventWithEventRef:")]
		NSEvent EventWithEventRef (IntPtr cgEventRef);

		[Export ("CGEvent")]
		IntPtr CGEvent { get; }

		[Static]
		[Export ("eventWithCGEvent:")]
		NSEvent EventWithCGEvent (IntPtr cgEventPtr);

		[Export ("magnification")]
		float Magnification { get; }

		[Export ("deviceID")]
		uint DeviceID { get; }

		[Export ("rotation")]
		float Rotation { get; }

		[Export ("absoluteX")]
		int AbsoluteX { get; }

		[Export ("absoluteY")]
		int AbsoluteY { get; }

		[Export ("absoluteZ")]
		int AbsoluteZ { get; }

		// TODO: What is the type?
		[Export ("buttonMask")]
		uint ButtonMask { get; }

		[Export ("tilt")]
		PointF Tilt { get; }

		[Export ("tangentialPressure")]
		float TangentialPressure { get; }

		[Export ("vendorDefined")]
		NSObject VendorDefined { get; }

		[Export ("vendorID")]
		uint VendorID { get; }

		[Export ("tabletID")]
		uint TabletID { get; }

		[Export ("pointingDeviceID")]
		uint PointingDeviceID ();

		[Export ("systemTabletID")]
		uint SystemTabletID { get; }

		[Export ("vendorPointingDeviceType")]
		uint VendorPointingDeviceType { get; }

		[Export ("pointingDeviceSerialNumber")]
		uint PointingDeviceSerialNumber { get; }

		[Export ("uniqueID")]
		long UniqueID { get; }

		[Export ("capabilityMask")]
		uint CapabilityMask { get; }

		[Export ("pointingDeviceType")]
		NSPointingDeviceType PointingDeviceType { get; }

		[Export ("isEnteringProximity")]
		bool IsEnteringProximity { get; }

		[Export ("touchesMatchingPhase:inView:")]
		NSSet TouchesMatchingPhaseinView (NSTouchPhase phase, NSView view);

		[Static]
		[Export ("startPeriodicEventsAfterDelay:withPeriod:")]
		void StartPeriodicEventsAfterDelay (double delay, double period);

		[Static]
		[Export ("stopPeriodicEvents")]
		void StopPeriodicEvents ();

		[Static]
		[Export ("mouseEventWithType:location:modifierFlags:timestamp:windowNumber:context:eventNumber:clickCount:pressure:")]
		NSEvent MouseEvent (NSEventType type, PointF location, NSEventModifierMask flags, double time, int wNum, NSGraphicsContext context, int eNum, int cNum, float pressure);

		[Static]
		[Export ("keyEventWithType:location:modifierFlags:timestamp:windowNumber:context:characters:charactersIgnoringModifiers:isARepeat:keyCode:")]
		NSEvent KeyEvent (NSEventType type, PointF location, NSEventModifierMask flags, double time, int wNum, NSGraphicsContext context, string keys, string ukeys, bool flag, ushort code);

		[Static]
		[Export ("enterExitEventWithType:location:modifierFlags:timestamp:windowNumber:context:eventNumber:trackingNumber:userData:")]
		NSEvent EnterExitEvent (NSEventType type, PointF location, NSEventModifierMask flags, double time, int wNum, NSGraphicsContext context, int eNum, int tNum, IntPtr data);

		[Static]
		[Export ("otherEventWithType:location:modifierFlags:timestamp:windowNumber:context:subtype:data1:data2:")]
		NSEvent OtherEvent (NSEventType type, PointF location, NSEventModifierMask flags, double time, int wNum, NSGraphicsContext context, short subtype, int d1, int d2);

		[Static]
		[Export ("mouseLocation")]
		PointF CurrentMouseLocation { get; }

		[Static]
		[Export ("modifierFlags")]
		uint CurrentModifierFlags { get; }

		[Static]
		[Export ("pressedMouseButtons")]
		uint CurrentPressedMouseButtons { get; }

		[Static]
		[Export ("doubleClickInterval")]
		double DoubleClickInterval { get; }

		[Static]
		[Export ("keyRepeatDelay")]
		double KeyRepeatDelay { get; }

		[Static]
		[Export ("keyRepeatInterval")]
		double KeyRepeatInterval { get; }

		[Static]
		[Export ("removeMonitor:")]
		void RemoveMonitor (NSObject eventMonitor);

		//Detected properties
		[Static]
		[Export ("mouseCoalescingEnabled")]
		bool MouseCoalescingEnabled { [Bind ("isMouseCoalescingEnabled")]get; set; }

	}


	[BaseType (typeof (NSObject))]
	interface NSMenu {
		[Export ("initWithTitle:")]
		IntPtr NSObject (string aTitle);

		[Export ("popUpContextMenu:withEvent:forView:")]
		void PopUpContextMenu (NSMenu menu, NSEvent theEvent, NSView view);

		[Static]
		[Export ("popUpContextMenu:withEvent:forView:withFont:")]
		void PopUpContextMenu (NSMenu menu, NSEvent theEvent, NSView view, NSFont font);

		[Export ("popUpMenuPositioningItem:atLocation:inView:")]
		bool PopUpMenu (NSMenuItem item, PointF location, NSView view);

		[Export ("insertItem:atIndex:")]
		void InsertItematIndex (NSMenuItem newItem, int index);

		[Export ("addItem:")]
		void AddItem (NSMenuItem newItem);

		[Export ("insertItemWithTitle:action:keyEquivalent:atIndex:")]
		NSMenuItem InsertItem (string title, Selector action, string charCode, int index);

		[Export ("addItemWithTitle:action:keyEquivalent:")]
		NSMenuItem AddItem (string title, Selector action, string charCode);

		[Export ("removeItemAtIndex:")]
		void RemoveItemAt (int index);

		[Export ("removeItem:")]
		void RemoveItem (NSMenuItem item);

		[Export ("setSubmenu:forItem:")]
		void SetSubmenuforItem (NSMenu aMenu, NSMenuItem anItem);

		[Export ("removeAllItems")]
		void RemoveAllItems ();

		[Export ("itemArray")]
		NSMenuItem [] ItemArray ();

		[Export ("numberOfItems")]
		int Count { get; }

		[Export ("itemAtIndex:")]
		NSMenuItem ItemAt (int index);

		[Export ("indexOfItem:")]
		int IndexOf (NSMenuItem index);

		[Export ("indexOfItemWithTitle:")]
		int IndexOf (string aTitle);

		[Export ("indexOfItemWithTag:")]
		int IndexOf (int itemTag);

		[Export ("indexOfItemWithRepresentedObject:")]
		int IndexOfItem (NSObject obj);

		[Export ("indexOfItemWithSubmenu:")]
		int IndexOfItem (NSMenu submenu);

		[Export ("indexOfItemWithTarget:andAction:")]
		int IndexOfItem (NSObject target, Selector actionSelector);

		[Export ("itemWithTitle:")]
		NSMenuItem ItemWithTitle (string title);

		[Export ("itemWithTag:")]
		NSMenuItem ItemWithTag (int tag);

		[Export ("update")]
		void Update ();

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("itemChanged:")]
		void ItemChanged (NSMenuItem item);

		[Export ("performActionForItemAtIndex:")]
		void PerformActionForItem (int index);

		[Export ("menuBarHeight")]
		float MenuBarHeight { get; }

		[Export ("cancelTracking")]
		void CancelTracking ();

		[Export ("cancelTrackingWithoutAnimation")]
		void CancelTrackingWithoutAnimation ();

		[Export ("highlightedItem")]
		NSMenuItem HighlightedItem { get; }

		[Export ("size")]
		SizeF Size { get; }

		// TODO: Bind NSZone
		//[Static]
		//[Export ("menuZone")]
		//NSZone MenuZone { get; }

		[Export ("helpRequested:")]
		void HelpRequested (NSEvent eventPtr);

		[Export ("isTornOff")]
		bool IsTornOff { get; }

		//Detected properties
		[Export ("title")]
		string Title { get; set; }

		[Static]
		[Export ("menuBarVisible")]
		bool MenuBarVisible { get; set; }

		[Export ("supermenu")]
		NSMenu Supermenu { get; set; }

		[Export ("autoenablesItems")]
		bool AutoenablesItems { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSMenuDelegate Delegate { get; set; }
		
		[Export ("minimumWidth")]
		float MinimumWidth { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("allowsContextMenuPlugIns")]
		bool AllowsContextMenuPlugIns { get; set; }

		[Export ("showsStateColumn")]
		bool ShowsStateColumn { get; set; }

		[Export ("menuChangedMessagesEnabled")]
		bool MenuChangedMessagesEnabled { get; set; }

		[Export ("propertiesToUpdate")]
		NSMenuProperty PropertiesToUpdate ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSMenuDelegate {
		[Export ("menuNeedsUpdate:")]
		void NeedsUpdate (NSMenu menu);

		[Export ("numberOfItemsInMenu:")]
		int MenuItemCount (NSMenu menu);

		[Export ("menu:updateItem:atIndex:shouldCancel:")]
		bool UpdateItem (NSMenu menu, NSMenuItem item, int atIndex, bool shouldCancel);

		[Export ("menuHasKeyEquivalent:forEvent:target:action:")]
		bool HasKeyEquivalentForEvent (NSMenu menu, NSEvent theEvent, NSObject target, Selector action);

		[Export ("menuWillOpen:")]
		void MenuWillOpen (NSMenu menu);

		[Export ("menuDidClose:")]
		void MenuDidClose (NSMenu menu);

		[Abstract]
		[Export ("menu:willHighlightItem:")]
		void MenuWillHighlightItem (NSMenu menu, NSMenuItem item);

		[Export ("confinementRectForMenu:onScreen:")]
		RectangleF ConfinementRectForMenu (NSMenu menu, NSScreen screen);
	}

	[BaseType (typeof (NSObject))]
	interface NSMenuItem {
		[Static]
		[Export ("separatorItem")]
		NSMenuItem SeparatorItem { get; }

		[Export ("initWithTitle:action:keyEquivalent:")]
		IntPtr Constructor (string aString, Selector aSelector, string charCode);

		[Export ("hasSubmenu")]
		bool HasSubmenu { get; }

		[Export ("parentItem")]
		NSMenuItem ParentItem { get; }

		[Export ("isSeparatorItem")]
		bool IsSeparatorItem { get; }

		[Export ("userKeyEquivalent")]
		string UserKeyEquivalent { get; }

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string stringWithAmpersand);

		[Export ("isHighlighted")]
		bool Highlighted { get; }

		[Export ("isHiddenOrHasHiddenAncestor")]
		bool IsHiddenOrHasHiddenAncestor { get; }

		//Detected properties
		[Static]
		[Export ("usesUserKeyEquivalents")]
		bool UsesUserKeyEquivalents { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("submenu")]
		NSMenu Submenu { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }

		[Export ("keyEquivalent")]
		string KeyEquivalent { get; set; }

		[Export ("keyEquivalentModifierMask")]
		NSEventModifierMask KeyEquivalentModifierMask { get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("state")]
		int State { get; set; }

		[Export ("onStateImage")]
		NSImage OnStateImage { get; set; }

		[Export ("offStateImage")]
		NSImage OffStateImage { get; set; }

		[Export ("mixedStateImage")]
		NSImage MixedStateImage { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("alternate")]
		bool Alternate { [Bind ("isAlternate")]get; set; }

		[Export ("indentationLevel")]
		int IndentationLevel { get; set; }

		[Export ("target")]
		NSObject Target { get; set; }

		[Export ("action")]
		Selector Action { get; set; }

		[Export ("tag")]
		int Tag { get; set; }

		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }

		[Export ("view")]
		NSView View { get; set; }

		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")]get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }
	}

	[BaseType (typeof (NSSavePanel), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSOpenSavePanelDelegate)})]
	interface NSOpenPanel {
		[Static]
		[Export ("openPanel")]
		NSOpenPanel OpenPanel { get; }

		[Export ("URLs")]
		string [] Urls { get; }

		//Detected properties
		[Export ("resolvesAliases")]
		bool ResolvesAliases { get; set; }

		[Export ("canChooseDirectories")]
		bool CanChooseDirectories { get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("canChooseFiles")]
		bool CanChooseFiles { get; set; }
	}

	[BaseType (typeof (NSView))]
	interface NSClipView {
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }
	
		[Export ("documentView")]
		NSView DocumentView { get; set; }
	
		[Export ("documentRect")]
		RectangleF DocumentRect { get; }
	
		[Export ("documentCursor")]
		NSCursor DocumentCursor { get; set; }
	
		[Export ("documentVisibleRect")]
		RectangleF documentVisibleRect ();
	
		[Export ("viewFrameChanged:")]
		void ViewFrameChanged (NSNotification  notification);
	
		[Export ("viewBoundsChanged:")]
		void ViewBoundsChanged (NSNotification  notification);
	
		[Export ("copiesOnScroll")]
		bool CopiesOnScroll { get; set; }
	
		[Export ("autoscroll:")]
		bool Autoscroll (NSEvent  theEvent);
	
		[Export ("constrainScrollPoint:")]
		PointF ConstrainScrollPoint (PointF newOrigin);
	
		[Export ("scrollToPoint:")]
		void ScrollToPoint (PointF newOrigin);

		[Export ("scrollClipView:toPoint:")]
		void ScrollClipView (NSClipView  aClipView, PointF aPoint);
	}

	[BaseType (typeof (NSObject))]
	interface NSDockTile {
		[Export ("size")]
		SizeF Size { get; }

		[Export ("display")]
		void Display ();

		[Export ("owner")]
		NSObject Owner { get; }

		//Detected properties
		[Export ("contentView")]
		NSView ContentView { get; set; }

		[Export ("showsApplicationBadge")]
		bool ShowsApplicationBadge { get; set; }

		[Export ("badgeLabel")]
		string BadgeLabel { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSDockTilePlugIn {
		[Abstract]
		[Export ("setDockTile:")]
		void SetDockTile (NSDockTile dockTile);

		[Abstract]
		[Export ("dockMenu")]
		NSMenu DockMenu ();
	}
	
	[BaseType (typeof (NSObject))]
	interface NSGraphicsContext {
		[Static, Export ("graphicsContextWithAttributes:")]
		NSGraphicsContext FromAttributes (NSDictionary attributes);
	
		[Static, Export ("graphicsContextWithWindow:")]
		NSGraphicsContext FromWindow (NSWindow window);
	
		[Static, Export ("graphicsContextWithBitmapImageRep:")]
		NSGraphicsContext FromBitmap (NSBitmapImageRep bitmapRep);
	
		[Static, Export ("graphicsContextWithGraphicsPort:flipped:")]
		NSGraphicsContext FromGraphicsPort (IntPtr graphicsPort, bool initialFlippedState);
	
		[Static, Export ("currentContext")]
		NSGraphicsContext CurrentContext { get; set; }
	
		[Static, Export ("currentContextDrawingToScreen")]
		bool IsCurrentContextDrawingToScreen { get; }
	
		[Static, Export ("saveGraphicsState")]
		void GlobalSaveGraphicsState ();
	
		[Static, Export ("restoreGraphicsState")]
		void GlobalRestoreGraphicsState ();
	
		[Static, Export ("setGraphicsState:")]
		void SetGraphicsState (int gState);
	
		[Export ("attributes")]
		NSDictionary Attributes { get; } 
	
		[Export ("isDrawingToScreen")]
		bool IsDrawingToScreen { get; }
	
		[Export ("saveGraphicsState")]
		void SaveGraphicsState ();
	
		[Export ("restoreGraphicsState")]
		void RestoreGraphicsState ();
	
		[Export ("flushGraphics")]
		void FlushGraphics ();
	
		[Export ("graphicsPort")]
		IntPtr GraphicsPort {get; }
	
		[Export ("isFlipped")]
		bool IsFlipped { get; }
	
		[Export ("shouldAntialias")]
		bool ShouldAntialias { get; set; }
	
		[Export ("imageInterpolation")]
		NSImageInterpolation ImageInterpolation { get; set; }
	
		[Export ("patternPhase")]
		PointF PatternPhase { get; set; }
	
		[Export ("compositingOperation")]
		NSComposite CompositingOperation { get; set; }
	
		[Export ("colorRenderingIntent")]
		NSColorRenderingIntent ColorRenderingIntent { get; set; }

		[Export ("CIContext")]
		MonoMac.CoreImage.CIContext CIContext { get; } 
	}

	[BaseType (typeof (NSPanel))]
	interface NSFontPanel {
		[Static]
		[Export ("sharedFontPanel")]
		NSFontPanel SharedFontPanel { get; }

		[Static]
		[Export ("sharedFontPanelExists")]
		bool SharedFontPanelExists { get; }

		[Export ("setPanelFont:isMultiple:")]
		void SetPanelFont (NSFont fontObj, bool isMultiple);

		[Export ("panelConvertFont:")]
		NSFont PanelConvertFont (NSFont fontObj);

		[Export ("worksWhenModal")]
		bool WorksWhenModal { get; }

		[Export ("reloadDefaultFontFamilies")]
		void ReloadDefaultFontFamilies ();

		//Detected properties
		[Export ("accessoryView")]
		NSView AccessoryView { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }
	}
	
	[BaseType (typeof (NSMatrix))]
	interface NSForm  {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("initWithFrame:mode:prototype:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (RectangleF frameRect, NSMatrixMode aMode, NSCell aCell, int rowsHigh, int colsWide);

		[Export ("initWithFrame:mode:cellClass:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (RectangleF frameRect, NSMatrixMode aMode, Class factoryId, int rowsHigh, int colsWide);

		[Export ("indexOfSelectedItem")]
		int SelectedItemIndex { get; }

		[Export ("setEntryWidth:")]
		void SetEntryWidth (float width);

		[Export ("setInterlineSpacing:")]
		void SetInterlineSpacing (float spacing);

		[Export ("setBordered:")]
		void SetBordered (bool flag);

		[Export ("setBezeled:")]
		void SetBezeled (bool flag);

		[Export ("setTitleAlignment:")]
		void SetTitleAlignment (NSTextAlignment mode);

		[Export ("setTextAlignment:")]
		void SetTextAlignment (NSTextAlignment mode);

		[Export ("setTitleFont:")]
		void SetTitleFont (NSFont fontObj);

		[Export ("setTextFont:")]
		void SetTextFont (NSFont fontObj);

		[Export ("cellAtIndex:")]
		NSObject CellAtIndex (int index);

		[Export ("drawCellAtIndex:")]
		void DrawCellAtIndex (int index);

		[Export ("addEntry:")]
		NSFormCell AddEntry (string title);

		[Export ("insertEntry:atIndex:")]
		NSFormCell InsertEntryatIndex (string title, int index);

		[Export ("removeEntryAtIndex:")]
		void RemoveEntryAtIndex (int index);

		[Export ("indexOfCellWithTag:")]
		int IndexOfCellWithTag (int aTag);

		[Export ("selectTextAtIndex:")]
		void SelectTextAtIndex (int index);

		[Export ("setFrameSize:")]
		void SetFrameSize (SizeF newSize);

		[Export ("setTitleBaseWritingDirection:")]
		void SetTitleBaseWritingDirection (NSWritingDirection writingDirection);

		[Export ("setTextBaseWritingDirection:")]
		void SetTextBaseWritingDirection (NSWritingDirection writingDirection);
	}
	
	[BaseType (typeof (NSActionCell))]
	interface NSFormCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("initTextCell:")]
		IntPtr ConstrainScrollPoint (string aString);

		[Export ("isOpaque")]
		bool IsOpaque { get; }

		//Detected properties
		[Export ("titleWidth")]
		float TitleWidth { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("titleFont")]
		NSFont TitleFont { get; set; }

		[Export ("titleAlignment")]
		NSTextAlignment TitleAlignment { get; set; }

		[Export ("placeholderString")]
		string PlaceholderString { get; set; }

		[Export ("placeholderAttributedString")]
		NSAttributedString PlaceholderAttributedString { get; set; }

		[Export ("titleBaseWritingDirection")]
		NSWritingDirection TitleBaseWritingDirection { get; set; }

		[Export ("setTitleWithMnemonic:")]
		void SetTitleWithMnemonic (string  stringWithAmpersand);
		
		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSImageDelegate)})]
	interface NSImage {
		[Export ("imageNamed:")]
		NSImage ImageNamed (string name);

		[Export ("initWithSize:")]
		IntPtr Constructor (SizeF aSize);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("initWithContentsOfFile:")]
		IntPtr Constructor (string fileName);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		//[Export ("initByReferencingFile:")]
		//IntPtr Constructor (string fileName);
		//[Export ("initByReferencingURL:")]
		//IntPtr Constructor (NSUrl url);

		// FIXME: need IconRec
		//[Export ("initWithIconRef:")]
		//IntPtr Constructor (IconRef iconRef);

		[Export ("initWithPasteboard:")]
		IntPtr Constructor (NSPasteboard pasteboard);

		//[Export ("initWithDataIgnoringOrientation:")]
		//IntPtr Constructor (NSData data);

		[Export ("drawAtPoint:fromRect:operation:fraction:")]
		void DrawAtPointfromRectoperationfraction (PointF point, RectangleF fromRect, NSCompositingOperation op, float delta);

		[Export ("drawInRect:fromRect:operation:fraction:")]
		void DrawInRectfromRectoperationfraction (RectangleF rect, RectangleF fromRect, NSCompositingOperation op, float delta);

		[Export ("drawInRect:fromRect:operation:fraction:respectFlipped:hints:")]
		void DrawInRectfromRectoperationfractionrespectFlippedhints (RectangleF dstSpacePortionRect, RectangleF srcSpacePortionRect, NSCompositingOperation op, float requestedAlpha, bool respectContextIsFlipped, NSDictionary hints);

		[Export ("drawRepresentation:inRect:")]
		bool DrawRepresentationinRect (NSImageRep imageRep, RectangleF rect);

		[Export ("recache")]
		void Recache ();

		[Export ("TIFFRepresentation")]
		NSData AsTiff ();

		[Export ("TIFFRepresentationUsingCompression:factor:")]
		NSData AsTiff (NSTiffCompression comp, float aFloat);

		[Export ("representations")]
		NSImageRep [] Representations ();

		[Export ("addRepresentations:")]
		void AddRepresentations (NSImageRep [] imageReps);

		[Export ("addRepresentation:")]
		void AddRepresentation (NSImageRep imageRep);

		[Export ("removeRepresentation:")]
		void RemoveRepresentation (NSImageRep imageRep);

		[Export ("isValid")]
		bool IsValid { get; }

		[Export ("lockFocus")]
		void LockFocus ();

		[Export ("lockFocusFlipped:")]
		void LockFocusFlipped (bool flipped);

		[Export ("unlockFocus")]
		void UnlockFocus ();

		[Export ("bestRepresentationForDevice:")]
		NSImageRep BestRepresentationForDevice (NSDictionary deviceDescription);

		[Static]
		[Export ("imageUnfilteredFileTypes")]
		NSObject [] ImageUnfilteredFileTypes ();

		[Static]
		[Export ("imageUnfilteredPasteboardTypes")]
		string [] ImageUnfilteredPasteboardTypes ();

		[Static]
		[Export ("imageFileTypes")]
		string [] ImageFileTypes { get; }

		[Static]
		[Export ("imagePasteboardTypes")]
		string [] ImagePasteboardTypes { get; }

		[Export ("imageTypes")]
		string [] ImageTypes { get; }

		[Static]
		[Export ("imageUnfilteredTypes")]
		string [] ImageUnfilteredTypes { get; }

		[Export ("canInitWithPasteboard:")]
		bool CanInitWithPasteboard (NSPasteboard pasteboard);

		[Export ("cancelIncrementalLoad")]
		void CancelIncrementalLoad ();

		[Export ("accessibilityDescription")]
		string AccessibilityDescription	 { get; set; }

		[Export ("initWithCGImage:size:")]
		IntPtr Constructor (CGImage cgImage, SizeF size);

		[Export ("CGImageForProposedRect:context:hints:")]
		CGImage CGImageForProposedRect (RectangleF proposedDestRect, NSGraphicsContext referenceContext, NSDictionary hints);

		[Export ("bestRepresentationForRect:context:hints:")]
		NSImageRep BestRepresentationForRect (RectangleF rect, NSGraphicsContext referenceContext, NSDictionary hints);

		[Export ("hitTestRect:withImageDestinationRect:context:hints:flipped:")]
		bool HitTestRectwithImageDestinationRect (RectangleF testRectDestSpace, RectangleF imageRectDestSpace, NSGraphicsContext context, NSDictionary hints, bool flipped);

		//Detected properties
		[Export ("size")]
		SizeF Size { get; set; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("usesEPSOnResolutionMismatch")]
		bool UsesEpsOnResolutionMismatch { get; set; }

		[Export ("prefersColorMatch")]
		bool PrefersColorMatch { get; set; }

		[Export ("matchesOnMultipleResolution")]
		bool MatchesOnMultipleResolution { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSImageDelegate Delegate { get; set; }

		[Export ("cacheMode")]
		NSImageCacheMode CacheMode { get; set; }

		[Export ("alignmentRect")]
		RectangleF AlignmentRect { get; set; }

		[Export ("template")]
		bool Template { [Bind ("isTemplate")]get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSHelpManager {
		[Export ("sharedHelpManager")]
		NSHelpManager SharedHelpManager ();

		[Export ("setContextHelp:forObject:")]
		void SetContext (NSAttributedString attrString, NSObject theObject);

		[Export ("removeContextHelpForObject:")]
		void RemoveContext (NSObject theObject);

		[Export ("contextHelpForObject:")]
		NSAttributedString Context (NSObject theObject);

		[Export ("showContextHelpForObject:locationHint:")]
		bool ShowContext (NSObject theObject, PointF pt);

		[Export ("openHelpAnchor:inBook:")]
		void OpenHelpAnchor (string anchor, string book);

		[Export ("findString:inBook:")]
		void FindString (string query, string book);

		[Export ("registerBooksInBundle:")]
		bool RegisterBooks (NSBundle bundle );

		//Detected properties
		[Static]
		[Export ("contextHelpModeActive")]
		bool ContextHelpModeActive { [Bind ("isContextHelpModeActive")]get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSImageDelegate {
		[Export ("imageDidNotDraw:inRect:"), EventArgs ("NSImageRect"), DefaultValue (null)]
		NSImage ImageDidNotDraw (NSObject sender, RectangleF aRect);

		[Export ("image:willLoadRepresentation:"), EventArgs ("NSImageLoad")]
		void WillLoadRepresentation (NSImage image, NSImageRep rep);

		[Export ("image:didLoadRepresentationHeader:"), EventArgs ("NSImageLoad")]
		void DidLoadRepresentationHeader (NSImage image, NSImageRep rep);

		[Export ("image:didLoadPartOfRepresentation:withValidRows:"), EventArgs ("NSImagePartial")]
		void DidLoadPartOfRepresentation (NSImage image, NSImageRep rep, int rows);

		[Export ("image:didLoadRepresentation:withStatus:"), EventArgs ("NSImageLoadRepresentation")]
		void DidLoadRepresentation (NSImage image, NSImageRep rep, NSImageLoadStatus status);
	}

	[BaseType (typeof (NSObject))]
	interface NSImageRep {
		[Export ("draw")]
		bool Draw ();

		[Export ("drawAtPoint:")]
		bool DrawAtPoint (PointF point);

		[Export ("drawInRect:")]
		bool DrawInRect (RectangleF rect);

		[Export ("drawInRect:fromRect:operation:fraction:respectFlipped:hints:")]
		bool DrawInRect (RectangleF dstSpacePortionRect, RectangleF srcSpacePortionRect, NSCompositingOperation op, float requestedAlpha, bool respectContextIsFlipped, NSDictionary hints);

		[Export ("setAlpha:")]
		void SetAlpha (bool flag);

		[Export ("hasAlpha")]
		bool HasAlpha { get; }

		[Static]
		[Export ("registerImageRepClass:")]
		void RegisterImageRepClass (Class imageRepClass);

		[Static]
		[Export ("unregisterImageRepClass:")]
		void UnregisterImageRepClass (Class imageRepClass);

		//[Static]
		//[Export ("registeredImageRepClasses")]
		//Class [] RegisteredImageRepClasses ();

		[Static]
		[Export ("imageRepClassForFileType:")]
		Class ImageRepClassForFileType (string type);

		[Static]
		[Export ("imageRepClassForPasteboardType:")]
		Class ImageRepClassForPasteboardType (string type);

		[Export ("imageRepClassForType:")]
		Class ImageRepClassForType (string type);

		[Export ("imageRepClassForData:")]
		Class ImageRepClassForData (NSData data);

		[Export ("canInitWithData:")]
		bool CanInitWithData (NSData data);

		[Static]
		[Export ("imageUnfilteredFileTypes")]
		string [] ImageUnfilteredFileTypes { get; }

		[Static]
		[Export ("imageUnfilteredPasteboardTypes")]
		string [] ImageUnfilteredPasteboardTypes { get; }

		[Static]
		[Export ("imageFileTypes")]
		string [] ImageFileTypes { get; }

		[Static]
		[Export ("imagePasteboardTypes")]
		string [] ImagePasteboardTypes { get; }

		[Export ("imageUnfilteredTypes")]
		string []ImageUnfilteredTypes { get; }

		[Static]
		[Export ("imageTypes")]
		string [] ImageTypes { get; }

		[Export ("canInitWithPasteboard:")]
		bool CanInitWithPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("imageRepsWithContentsOfFile:")]
		NSImageRep [] ImageRepsFromFile (string filename);

		[Static]
		[Export ("imageRepWithContentsOfFile:")]
		NSImageRep ImageRepFromFile (string filename);

		[Static]
		[Export ("imageRepsWithContentsOfURL:")]
		NSImageRep [] ImageRepsFromUrl (NSUrl url);

		[Static]
		[Export ("imageRepWithContentsOfURL:")]
		NSImageRep ImageRepFromUrl (NSUrl url);

		[Static]
		[Export ("imageRepsWithPasteboard:")]
		NSImageRep [] ImageRepsFromPasteboard (NSPasteboard pasteboard);

		[Static]
		[Export ("imageRepWithPasteboard:")]
		NSImageRep ImageRepFromPasteboard (NSPasteboard pasteboard);

		[Export ("CGImageForProposedRect:context:hints:")]
		CGImage CGImageForProposedRect (RectangleF proposedDestRect, NSGraphicsContext context, NSDictionary hints);

		//Detected properties
		[Export ("size")]
		SizeF Size { get; set; }

		[Export ("opaque")]
		bool Opaque { [Bind ("isOpaque")]get; set; }

		[Export ("colorSpaceName")]
		string ColorSpaceName { get; set; }

		[Export ("bitsPerSample")]
		int BitsPerSample { get; set; }

		[Export ("pixelsWide")]
		int PixelsWide { get; set; }

		[Export ("pixelsHigh")]
		int PixelsHigh { get; set; }
	}

	[BaseType (typeof (NSControl), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSMatrixDelegate)})]
	interface NSMatrix {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("initWithFrame:mode:prototype:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (RectangleF frameRect, NSMatrixMode aMode, NSCell aCell, int rowsHigh, int colsWide);

		[Export ("initWithFrame:mode:cellClass:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (RectangleF frameRect, NSMatrixMode aMode, Class factoryId, int rowsHigh, int colsWide);

		[Export ("makeCellAtRow:column:")]
		NSCell MakeCellAtRowcolumn (int row, int col);

		[Export ("sendAction:to:forAllCells:")]
		void SendAction (Selector aSelector, NSObject anObject, bool flag);

		[Export ("cells")]
		NSCell [] Cells { get; }

		[Export ("sortUsingSelector:")]
		void Sort (Selector comparator);

		//[Export ("sortUsingFunction:context:")][Internal]
		// We need to define NSCompareFunc as:
		// (NSInteger (*)(id, id, void *))
		//void Sort (NSCompareFunc func, IntPtr context);

		[Export ("selectedCell")]
		NSCell SelectedCell { get; }

		[Export ("selectedCells")]
		NSCell [] SelectedCells { get; }

		[Export ("selectedRow")]
		int SelectedRow { get; }

		[Export ("selectedColumn")]
		int SelectedColumn { get; }

		[Export ("setSelectionFrom:to:anchor:highlight:")]
		void SetSelection (int startPos, int endPos, int anchorPos, bool highlight);

		[Export ("deselectSelectedCell")]
		void DeselectSelectedCell ();

		[Export ("deselectAllCells")]
		void DeselectAllCells ();

		[Export ("selectCellAtRow:column:")]
		void SelectCellAtRowcolumn (int row, int col);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("selectCellWithTag:")]
		bool SelectCellWithTag (int tag);

		[Export ("setScrollable:")]
		void SetScrollable (bool flag);

		[Export ("setState:atRow:column:")]
		void SetState (int state, int row, int col);

		[Export ("getNumberOfRows:columns:")]
		void GetRowsAndColumnsCount (out int rowCount, out int colCount);

		[Export ("numberOfRows")]
		int Rows { get; }

		[Export ("numberOfColumns")]
		int Columns { get; }

		[Export ("cellAtRow:column:")][Internal]
		NSCell CellAtRowColumn (int row, int col);

		[Export ("cellFrameAtRow:column:")]
		RectangleF CellFrameAtRowColumn (int row, int col);

		[Export ("getRow:column:ofCell:")]
		bool GetRowColumn (out int row, out int col, NSCell aCell);

		[Export ("getRow:column:forPoint:")]
		bool GetRowcolumnForPoint (out int row, out int col, PointF aPoint);

		[Export ("renewRows:columns:")]
		void RenewRowsColumns (int newRows, int newCols);

		[Export ("putCell:atRow:column:")]
		void PutCellatRowColumn (NSCell newCell, int row, int col);

		[Export ("addRow")]
		void AddRow ();

		[Export ("addRowWithCells:")]
		void AddRowWithCells (NSCell [] newCells);

		[Export ("insertRow:")]
		void InsertRow (int row);

		[Export ("insertRow:withCells:")]
		void InsertRow (int row, NSCell [] newCells);

		[Export ("removeRow:")]
		void RemoveRow (int row);

		[Export ("addColumn")]
		void AddColumn ();

		[Export ("addColumnWithCells:")]
		void AddColumnWithCells (NSCell [] newCells);

		[Export ("insertColumn:")]
		void InsertColumn (int column);

		[Export ("insertColumn:withCells:")]
		void InsertColumnwithCells (int column, NSCell [] newCells);

		[Export ("removeColumn:")]
		void RemoveColumn (int col);

		[Export ("cellWithTag:")]
		NSCell CellWithTag (int anInt);

		[Export ("sizeToCells")]
		void SizeToCells ();
									       
		[Export ("setValidateSize:")]
		void SetValidateSize (bool flag);

		[Export ("drawCellAtRow:column:")]
		void DrawCellAtRowColumn (int row, int col);

		[Export ("highlightCell:atRow:column:")]
		void HighlightCellatRowColumn (bool flag, int row, int col);

		[Export ("scrollCellToVisibleAtRow:column:")]
		void ScrollCellToVisibleAtRowcolumn (int row, int col);

		[Export ("mouseDownFlags")]
		int MouseDownFlags ();

		[Export ("mouseDown:")]
		void MouseDown (NSEvent theEvent);

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("sendAction")]
		bool SendAction ();

		[Export ("sendDoubleAction")]
		void SendDoubleAction ();

		[Export ("textShouldBeginEditing:")]
		bool ShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:")]
		bool ShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:")]
		void DidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:")]
		void DidEndEditing (NSNotification notification);

		[Export ("textDidChange:")]
		void DidChange (NSNotification notification);

		[Export ("selectText:")]
		void SelectText (NSObject sender);

		[Export ("selectTextAtRow:column:")]
		NSObject SelectTextAtRowColumn (int row, int col);

		[Export ("acceptsFirstMouse:")]
		bool AcceptsFirstMouse (NSEvent theEvent);

		[Export ("resetCursorRects")]
		void ResetCursorRects ();

		[Export ("setToolTip:forCell:")]
		void SetToolTipforCell (string toolTipString, NSCell cell);

		[Export ("toolTipForCell:")]
		string ToolTipForCell (NSCell cell);

		//Detected properties
		[Export ("cellClass")]
		Class CellClass { get; set; }

		[Export ("prototype")]
		NSCell Prototype { get; set; }

		[Export ("mode")]
		NSMatrixMode Mode { get; set; }

		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }

		[Export ("selectionByRect")]
		bool SelectionByRect { [Bind ("isSelectionByRect")]get; set; }

		[Export ("cellSize")]
		SizeF CellSize { get; set; }

		[Export ("intercellSpacing")]
		SizeF IntercellSpacing { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("cellBackgroundColor")]
		NSColor CellBackgroundColor { get; set; }

		[Export ("drawsCellBackground")]
		bool DrawsCellBackground { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("autosizesCells")]
		bool AutosizesCells { get; set; }

		[Export ("autoscroll")]
		bool Autoscroll { [Bind ("isAutoscroll")]get; set; }

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSMatrixDelegate Delegate { get; set; }

		//Detected properties
		[Export ("tabKeyTraversesCells")]
		bool TabKeyTraversesCells { get; set; }

		[Export ("keyCell")]
		NSObject KeyCell { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSLayoutManager {
	}
	
	[Model]
	[BaseType (typeof (NSObject))]
	interface NSMatrixDelegate {
		[Export ("control:textShouldBeginEditing:"), EventArgs ("NSControlText"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSControl control, NSText fieldEditor);

		[Export ("control:textShouldEndEditing:"), EventArgs ("NSControlText"), DefaultValue (true)]
		bool TextShouldEndEditing (NSControl control, NSText fieldEditor);

		[Export ("control:didFailToFormatString:errorDescription:"), EventArgs ("NSControlTextError"), DefaultValue (true)]
		bool DidFailToFormatString (NSControl control, string str, string error);
		
		[Export ("control:didFailToValidatePartialString:errorDescription:"), EventArgs ("NSControlTextError")]
		void DidFailToValidatePartialString (NSControl control, string str, string error);
		
		[Export ("control:isValidObject:"), EventArgs ("NSControlTextValidation"), DefaultValue (true)]
		bool IsValidObject (NSControl control, NSObject objectToValidate);

		[Export ("control:textView:doCommandBySelector:"), EventArgs ("NSControlCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSControl control, NSTextView textView, Selector commandSelector);

		[Export ("control:textView:completions:forPartialWordRange:indexOfSelectedItem:"), EventArgs ("NSControlTextFilter"), DefaultValue (null)]
		string [] FilterCompletions (NSControl control, NSTextView textView, string [] words, NSRange charRange, int index);
	}

	[BaseType (typeof (NSObject))]
	interface NSPageLayout {
		[Static]
		[Export ("pageLayout")]
		NSPageLayout PageLayout { get; }

		[Export ("addAccessoryController:")]
		void AddAccessoryController (NSViewController accessoryController);

		[Export ("removeAccessoryController:")]
		void RemoveAccessoryController (NSViewController accessoryController);

		[Export ("accessoryControllers")]
		NSViewController [] AccessoryControllers ();

		[Export ("beginSheetWithPrintInfo:modalForWindow:delegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, NSObject del, Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithPrintInfo:")]
		int RunModalWithPrintInfo (NSPrintInfo printInfo);

		[Export ("runModal")]
		int RunModal ();

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; }
	}

	[BaseType (typeof (NSWindow))]
	interface NSPanel {
		//Detected properties
		[Export ("floatingPanel")]
		bool FloatingPanel { [Bind ("isFloatingPanel")]get; set; }

		[Export ("becomesKeyOnlyIfNeeded")]
		bool BecomesKeyOnlyIfNeeded { get; set; }

		[Export ("worksWhenModal")]
		bool WorksWhenModal { get; set; }

	}

	[BaseType (typeof (NSObject))]
	interface NSParagraphStyle {
		[Static]
		[Export ("defaultParagraphStyle")]
		NSParagraphStyle DefaultParagraphStyle { get; }

		[Export ("defaultWritingDirectionForLanguage:")]
		NSWritingDirection DefaultWritingDirection (string languageName);

		[Export ("lineSpacing")]
		float LineSpacing { get; }

		[Export ("paragraphSpacing")]
		float ParagraphSpacing { get; }

		[Export ("alignment")]
		NSTextAlignment Alignment ();

		[Export ("headIndent")]
		float HeadIndent { get; }

		[Export ("tailIndent")]
		float TailIndent { get; }

		[Export ("firstLineHeadIndent")]
		float FirstLineHeadIndent { get; }

		[Export ("tabStops")]
		NSTextTab [] TabStops ();

		[Export ("minimumLineHeight")]
		float MinimumLineHeight { get; }

		[Export ("maximumLineHeight")]
		float MaximumLineHeight { get; }

		[Export ("lineBreakMode")]
		NSLineBreakMode LineBreakMode { get; }

		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; }

		[Export ("lineHeightMultiple")]
		float LineHeightMultiple { get; }

		[Export ("paragraphSpacingBefore")]
		float ParagraphSpacingBefore { get; }

		[Export ("defaultTabInterval")]
		float DefaultTabInterval { get; }

		[Export ("textBlocks")]
		NSTextTableBlock [] TextBlocks { get; }

		[Export ("textLists")]
		NSTextList[] TextLists { get; }

		[Export ("hyphenationFactor")]
		float HyphenationFactor { get; }

		[Export ("tighteningFactorForTruncation")]
		float TighteningFactorForTruncation { get; }

		[Export ("headerLevel")]
		int HeaderLevel { get; }
	}

	[BaseType (typeof (NSParagraphStyle))]
	interface NSMutableParagraphStyle {
		[Export ("setParagraphSpacing:")]
		void SetParagraphSpacing (float aFloat);

		[Export ("setAlignment:")]
		void SetAlignment (NSTextAlignment alignment);

		[Export ("setFirstLineHeadIndent:")]
		void SetFirstLineHeadIndent (float aFloat);

		[Export ("setHeadIndent:")]
		void SetHeadIndent (float aFloat);

		[Export ("setTailIndent:")]
		void SetTailIndent (float aFloat);

		[Export ("setLineBreakMode:")]
		void SetLineBreakMode (NSLineBreakMode mode);

		[Export ("setMinimumLineHeight:")]
		void SetMinimumLineHeight (float aFloat);

		[Export ("setMaximumLineHeight:")]
		void SetMaximumLineHeight (float aFloat);

		[Export ("addTabStop:")]
		void AddTabStop (NSTextTab anObject);

		[Export ("removeTabStop:")]
		void RemoveTabStop (NSTextTab anObject);

		[Export ("setTabStops:")]
		void SetTabStops (NSTextTab [] array);

		[Export ("setParagraphStyle:")]
		void SetParagraphStyle (NSParagraphStyle obj);

		[Export ("setBaseWritingDirection:")]
		void SetBaseWritingDirection (NSWritingDirection writingDirection);

		[Export ("setLineHeightMultiple:")]
		void SetLineHeightMultiple (float aFloat);

		[Export ("setParagraphSpacingBefore:")]
		void SetParagraphSpacingBefore (float aFloat);

		[Export ("setDefaultTabInterval:")]
		void SetDefaultTabInterval (float aFloat);

		[Export ("setTextBlocks:")]
		void SetTextBlocks (NSTextBlock [] array);

		[Export ("setTextLists:")]
		void SetTextLists (NSTextList [] array);

		[Export ("setHyphenationFactor:")]
		void SetHyphenationFactor (float aFactor);

		[Export ("setTighteningFactorForTruncation:")]
		void SetTighteningFactorForTruncation (float aFactor);

		[Export ("setHeaderLevel:")]
		void SetHeaderLevel (int level);

	}

	[BaseType (typeof (NSObject))]
	interface NSPasteboard {
		[Static]
		[Export ("generalPasteboard")]
		NSPasteboard GeneralPasteboard { get; }

		[Static]
		[Export ("pasteboardWithName:")]
		NSPasteboard PasteboardWithName (string name);

		[Static]
		[Export ("pasteboardWithUniqueName")]
		NSPasteboard PasteboardWithUniqueName { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("changeCount")]
		int ChangeCount { get; }

		[Export ("releaseGlobally")]
		void ReleaseGlobally ();

		[Export ("clearContents")]
		int ClearContents ();

		[Export ("writeObjects:")]
		bool WriteObjects (NSPasteboardReading [] objects);

		[Export ("readObjectsForClasses:options:")]
		NSObject [] ReadObjectsForClassesoptions (NSPasteboardReading [] classArray, NSDictionary options);

		[Export ("pasteboardItems")]
		NSPasteboardItem [] PasteboardItems { get; }

		[Export ("indexOfPasteboardItem:")]
		int IndexOf (NSPasteboardItem pasteboardItem);

		[Export ("canReadItemWithDataConformingToTypes:")]
		bool CanReadItemWithDataConformingToTypes (string [] utiTypes);

		[Export ("canReadObjectForClasses:options:")]
		bool CanReadObjectForClassesoptions (NSPasteboardReading [] classArray, NSDictionary options);

		[Export ("declareTypes:owner:")]
		int DeclareTypesowner (string [] newTypes, NSObject newOwner);

		[Export ("addTypes:owner:")]
		int AddTypes (string [] newTypes, NSObject newOwner);

		[Export ("types")]
		string [] Types { get; }

		[Export ("availableTypeFromArray:")]
		string AvailableTypeFromArray (string [] types);

		[Export ("setData:forType:")]
		bool SetDataforType (NSData data, string dataType);

		[Export ("setPropertyList:forType:")]
		bool SetPropertyListforType (NSObject plist, string dataType);

		[Export ("setString:forType:")]
		bool SetStringforType (string str, string dataType);

		[Export ("dataForType:")]
		NSData DataForType (string dataType);

		[Export ("propertyListForType:")]
		NSObject PropertyListForType (string dataType);

		[Export ("stringForType:")]
		string StringForType (string dataType);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NSPasteboardWriting {
		[Export ("writableTypesForPasteboard:")]
		string [] WritableTypesForPasteboard (NSPasteboard pasteboard);

		[Export ("writingOptionsForType:pasteboard:")]
		NSPasteboardWritingOptions WritingOptions (string type, NSPasteboard pasteboard);

		[Export ("pasteboardPropertyListForType:")]
		NSObject PasteboardPropertyListForType (string type);
	}

	[BaseType (typeof (NSObject))]
	interface NSPasteboardItem {
		[Export ("types")]
		string [] Types { get; }

		[Export ("availableTypeFromArray:")]
		string AvailableTypeFromArray (string [] types);

		[Export ("setDataProvider:forTypes:")]
		bool SetDataProviderforTypes (NSPasteboardItemDataProvider dataProvider, string [] types);

		[Export ("setData:forType:")]
		bool SetDataforType (NSData data, string type);

		[Export ("setString:forType:")]
		bool SetStringforType (string str, string type);

		[Export ("setPropertyList:forType:")]
		bool SetPropertyListforType (NSObject propertyList, string type);

		[Export ("dataForType:")]
		NSData DataForType (string type);

		[Export ("stringForType:")]
		string StringForType (string type);

		[Export ("propertyListForType:")]
		NSObject PropertyListForType (string type);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSPasteboardItemDataProvider {
		[Abstract]
		[Export ("pasteboard:item:provideDataForType:")]
		void ProvideDataForType (NSPasteboard pasteboard, NSPasteboardItem item, string type);

		[Abstract]
		[Export ("pasteboardFinishedWithDataProvider:")]
		void FinishedWithDataProvider (NSPasteboard pasteboard);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSPasteboardReading {
		[Abstract]
		[Export ("readableTypesForPasteboard:")]
		string [] ReadableTypesForPasteboard (NSPasteboard pasteboard);

		[Abstract]
		[Export ("readingOptionsForType:pasteboard:")]
		NSPasteboardReadingOptions ReadingOptionsForTypepasteboard (string type, NSPasteboard pasteboard);

		[Abstract]
		[Export ("initWithPasteboardPropertyList:ofType:")]
		NSObject InitWithPasteboardPropertyListofType (NSObject propertyList, string type);
	}
	

	[BaseType (typeof (NSObject))]
	interface NSPrinter {
		[Static]
		[Export ("printerNames")]
		string [] PrinterNames{ get; }

		[Static]
		[Export ("printerTypes")]
		string [] PrinterTypes { get; }

		[Static]
		[Export ("printerWithName:")]
		NSPrinter PrinterWithName (string name);

		[Static]
		[Export ("printerWithType:")]
		NSPrinter PrinterWithType (string type);

		[Export ("name")]
		string Name { get; }

		[Export ("type")]
		string Type { get; }

		[Export ("languageLevel")]
		int LanguageLevel { get; }

		[Export ("pageSizeForPaper:")]
		SizeF PageSizeForPaper (string paperName); 

		[Export ("statusForTable:")]
		NSPrinterTableStatus StatusForTable (string tableName);

		[Export ("isKey:inTable:")]
		bool IsKeyInTable (string key, string table);

		[Export ("booleanForKey:inTable:")]
		bool BooleanForKey (string key, string table);

		[Export ("floatForKey:inTable:")]
		float FloatForKey (string key, string table);

		[Export ("intForKey:inTable:")]
		int IntForKey (string key, string table);

		[Export ("rectForKey:inTable:")]
		RectangleF RectForKey (string key, string table);

		[Export ("sizeForKey:inTable:")]
		SizeF SizeForKey (string key, string table);

		[Export ("stringForKey:inTable:")]
		string StringForKey (string key, string table);

		[Export ("stringListForKey:inTable:")]
		string [] StringListForKey (string key, string table);

		[Export ("deviceDescription")]
		NSDictionary DeviceDescription { get; }
	}

	[BaseType (typeof (NSObject))]
	interface NSPrintInfo {
		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary attributes);

		[Export ("dictionary")]
		NSMutableDictionary Dictionary { get; }

		[Export ("setUpPrintOperationDefaultValues")]
		void SetUpPrintOperationDefaultValues ();

		[Export ("imageablePageBounds")]
		RectangleF ImageablePageBounds { get; }

		[Export ("localizedPaperName")]
		string LocalizedPaperName { get; }

		[Export ("defaultPrinter")]
		NSPrinter DefaultPrinter { get; }

		[Export ("printSettings")]
		NSMutableDictionary PrintSettings { get; }

		[Export ("PMPrintSession")]
		void PMPrintSession ();

		[Export ("PMPageFormat")]
		void PMPageFormat ();

		[Export ("PMPrintSettings")]
		void PMPrintSettings ();

		[Export ("updateFromPMPageFormat")]
		void UpdateFromPMPageFormat ();

		[Export ("updateFromPMPrintSettings")]
		void UpdateFromPMPrintSettings ();

		//Detected properties
		[Static]
		[Export ("sharedPrintInfo")]
		NSPrintInfo SharedPrintInfo { get; set; }

		[Export ("paperName")]
		string PaperName { get; set; }

		[Export ("paperSize")]
		SizeF PaperSize { get; set; }

		[Export ("orientation")]
		NSPrintingOrientation Orientation { get; set; }

		[Export ("scalingFactor")]
		float ScalingFactor { get; set; }

		[Export ("leftMargin")]
		float LeftMargin { get; set; }

		[Export ("rightMargin")]
		float RightMargin { get; set; }

		[Export ("topMargin")]
		float TopMargin { get; set; }

		[Export ("bottomMargin")]
		float BottomMargin { get; set; }

		[Export ("horizontallyCentered")]
		bool HorizontallyCentered { [Bind ("isHorizontallyCentered")]get; set; }

		[Export ("verticallyCentered")]
		bool VerticallyCentered { [Bind ("isVerticallyCentered")]get; set; }

		[Export ("horizontalPagination")]
		NSPrintingPaginationMode HorizontalPagination { get; set; }

		[Export ("verticalPagination")]
		NSPrintingPaginationMode VerticalPagination { get; set; }

		[Export ("jobDisposition")]
		string JobDisposition { get; set; }

		[Export ("printer")]
		NSPrinter Printer { get; set; }

		[Export ("selectionOnly")]
		bool SelectionOnly { [Bind ("isSelectionOnly")]get; set; }

	}


	[BaseType (typeof (NSObject))]
	interface NSPrintOperation {
		[Static]
		[Export ("printOperationWithView:printInfo:")]
		NSPrintOperation FromView (NSView view, NSPrintInfo printInfo);

		[Static]
		[Export ("PDFOperationWithView:insideRect:toData:printInfo:")]
		NSPrintOperation PdfFromView (NSView view, RectangleF rect, NSMutableData data, NSPrintInfo printInfo);

		[Static]
		[Export ("PDFOperationWithView:insideRect:toPath:printInfo:")]
		NSPrintOperation PdfFromView (NSView view, RectangleF rect, string path, NSPrintInfo printInfo);

		[Static]
		[Export ("EPSOperationWithView:insideRect:toData:printInfo:")]
		NSPrintOperation EpsFromView (NSView view, RectangleF rect, NSMutableData data, NSPrintInfo printInfo);

		[Static]
		[Export ("EPSOperationWithView:insideRect:toPath:printInfo:")]
		NSPrintOperation EpsFromView (NSView view, RectangleF rect, string path, NSPrintInfo printInfo);

		[Static]
		[Export ("printOperationWithView:")]
		NSPrintOperation FromView (NSView view);

		[Static]
		[Export ("PDFOperationWithView:insideRect:toData:")]
		NSPrintOperation PdfFromView (NSView view, RectangleF rect, NSMutableData data);

		[Static]
		[Export ("EPSOperationWithView:insideRect:toData:")]
		NSPrintOperation EpsFromView (NSView view, RectangleF rect, NSMutableData data);

		[Export ("isCopyingOperation")]
		bool IsCopyingOperation { get; }

		[Export ("runOperationModalForWindow:delegate:didRunSelector:contextInfo:")]
		void RunOperationModal (NSWindow docWindow, NSObject del, Selector didRunSelector, IntPtr contextInfo);

		[Export ("runOperation")]
		bool RunOperation ();

		[Export ("view")]
		NSView View { get; }

		[Export ("context")]
		NSGraphicsContext Context { get; }

		[Export ("pageRange")]
		NSRange PageRange { get; }

		[Export ("currentPage")]
		int CurrentPage { get; }

		[Export ("createContext")]
		NSGraphicsContext CreateContext ();

		[Export ("destroyContext")]
		void DestroyContext ();

		[Export ("deliverResult")]
		bool DeliverResult ();

		[Export ("cleanUpOperation")]
		void CleanUpOperation ();

		//Detected properties
		[Static]
		[Export ("currentOperation")]
		NSPrintOperation CurrentOperation { get; set; }

		[Export ("jobTitle")]
		string JobTitle { get; set; }

		[Export ("showsPrintPanel")]
		bool ShowsPrintPanel { get; set; }

		[Export ("showsProgressPanel")]
		bool ShowsProgressPanel { get; set; }

		[Export ("printPanel")]
		NSPrintPanel PrintPanel { get; set; }

		[Export ("canSpawnSeparateThread")]
		bool CanSpawnSeparateThread { get; set; }

		[Export ("pageOrder")]
		NSPrintingPageOrder PageOrder { get; set; }

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSPrintPanelAccessorizing {
		[Abstract]
		[Export ("localizedSummaryItems")]
		NSDictionary [] LocalizedSummaryItems ();

		[Abstract]
		[Export ("keyPathsForValuesAffectingPreview")]
		NSSet KeyPathsForValuesAffectingPreview ();
	}

	[BaseType (typeof (NSObject))]
	interface NSPrintPanel {
		[Static]
		[Export ("printPanel")]
		NSPrintPanel PrintPanel { get; }

		[Export ("addAccessoryController:")]
		void AddAccessoryController (NSViewController accessoryController);

		[Export ("removeAccessoryController:")]
		void RemoveAccessoryController (NSViewController accessoryController);

		[Export ("accessoryControllers")]
		NSViewController [] AccessoryControllers ();

		[Export ("beginSheetWithPrintInfo:modalForWindow:delegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, NSObject del, Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithPrintInfo:")]
		int RunModalWithPrintInfo (NSPrintInfo printInfo);

		[Export ("runModal")]
		int RunModal ();

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; }

		//Detected properties
		[Export ("options")]
		NSPrintPanelOptions Options { get; set; }

		[Export ("defaultButtonTitle")]
		string DefaultButtonTitle { get; set; }

		[Export ("helpAnchor")]
		string HelpAnchor { get; set; }

		[Export ("jobStyleHint")]
		string JobStyleHint { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSResponder {
		[Export ("tryToPerform:with:")]
		bool TryToPerformwith (Selector anAction, NSObject anObject);

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestorForSendTypereturnType (string sendType, string returnType);

		[Export ("mouseDown:")]
		void MouseDown (NSEvent theEvent);

		[Export ("rightMouseDown:")]
		void RightMouseDown (NSEvent theEvent);

		[Export ("otherMouseDown:")]
		void OtherMouseDown (NSEvent theEvent);

		[Export ("mouseUp:")]
		void MouseUp (NSEvent theEvent);

		[Export ("rightMouseUp:")]
		void RightMouseUp (NSEvent theEvent);

		[Export ("otherMouseUp:")]
		void OtherMouseUp (NSEvent theEvent);

		[Export ("mouseMoved:")]
		void MouseMoved (NSEvent theEvent);

		[Export ("mouseDragged:")]
		void MouseDragged (NSEvent theEvent);

		[Export ("scrollWheel:")]
		void ScrollWheel (NSEvent theEvent);

		[Export ("rightMouseDragged:")]
		void RightMouseDragged (NSEvent theEvent);

		[Export ("otherMouseDragged:")]
		void OtherMouseDragged (NSEvent theEvent);

		[Export ("mouseEntered:")]
		void MouseEntered (NSEvent theEvent);

		[Export ("mouseExited:")]
		void MouseExited (NSEvent theEvent);

		[Export ("keyDown:")]
		void KeyDown (NSEvent theEvent);

		[Export ("keyUp:")]
		void KeyUp (NSEvent theEvent);

		[Export ("flagsChanged:")]
		void FlagsChanged (NSEvent theEvent);

		[Export ("tabletPoint:")]
		void TabletPoint (NSEvent theEvent);

		[Export ("tabletProximity:")]
		void TabletProximity (NSEvent theEvent);

		[Export ("cursorUpdate:")]
		void CursorUpdate (NSEvent theEvent);

		[Export ("magnifyWithEvent:")]
		void MagnifyWithEvent (NSEvent theEvent);

		[Export ("rotateWithEvent:")]
		void RotateWithEvent (NSEvent theEvent);

		[Export ("swipeWithEvent:")]
		void SwipeWithEvent (NSEvent theEvent);

		[Export ("beginGestureWithEvent:")]
		void BeginGestureWithEvent (NSEvent theEvent);

		[Export ("endGestureWithEvent:")]
		void EndGestureWithEvent (NSEvent theEvent);

		[Export ("touchesBeganWithEvent:")]
		void TouchesBeganWithEvent (NSEvent theEvent);

		[Export ("touchesMovedWithEvent:")]
		void TouchesMovedWithEvent (NSEvent theEvent);

		[Export ("touchesEndedWithEvent:")]
		void TouchesEndedWithEvent (NSEvent theEvent);

		[Export ("touchesCancelledWithEvent:")]
		void TouchesCancelledWithEvent (NSEvent theEvent);

		[Export ("noResponderFor:")]
		void NoResponderFor (Selector eventSelector);

		[Export ("acceptsFirstResponder")]
		bool AcceptsFirstResponder ();

		[Export ("becomeFirstResponder")]
		bool BecomeFirstResponder ();

		[Export ("resignFirstResponder")]
		bool ResignFirstResponder ();

		[Export ("interpretKeyEvents:")]
		void InterpretKeyEvents (NSEvent [] eventArray);

		[Export ("flushBufferedKeyEvents")]
		void FlushBufferedKeyEvents ();

		[Export ("showContextHelp:")]
		void ShowContextHelp (NSObject sender);

		[Export ("helpRequested:")]
		void HelpRequested (NSEvent theEventPtr);

		[Export ("shouldBeTreatedAsInkEvent:")]
		bool ShouldBeTreatedAsInkEvent (NSEvent theEvent);

		//Detected properties
		[Export ("nextResponder")]
		NSResponder NextResponder { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }
	}


	[BaseType (typeof (NSObject))]
	interface NSRulerMarker {
		[Export ("initWithRulerView:markerLocation:image:imageOrigin:")]
		IntPtr Constructor (NSRulerView ruler, float location, NSImage image, PointF imageOrigin);

		[Export ("ruler")]
		NSRulerView Ruler { get; }

		[Export ("isDragging")]
		bool IsDragging { get; }

		[Export ("imageRectInRuler")]
		RectangleF ImageRectInRuler { get; }

		[Export ("thicknessRequiredInRuler")]
		float ThicknessRequiredInRuler { get; }

		[Export ("drawRect:")]
		void DrawRect (RectangleF rect);

		[Export ("trackMouse:adding:")]
		bool TrackMouse (NSEvent mouseDownEvent, bool isAdding);

		//Detected properties
		[Export ("markerLocation")]
		float MarkerLocation { get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("imageOrigin")]
		PointF ImageOrigin { get; set; }

		[Export ("movable")]
		bool Movable { [Bind ("isMovable")]get; set; }

		[Export ("removable")]
		bool Removable { [Bind ("isRemovable")]get; set; }

		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }
	}

	[BaseType (typeof (NSView))]
	interface NSRulerView {
		[Static]
		[Export ("registerUnitWithName:abbreviation:unitToPointsConversionFactor:stepUpCycle:stepDownCycle:")]
		void RegisterUnit (string unitName, string abbreviation, float conversionFactor, NSNumber [] stepUpCycle, NSNumber [] stepDownCycle);

		[Export ("initWithScrollView:orientation:")]
		IntPtr Constructor (NSScrollView scrollView, NSRulerOrientation orientation);

		[Export ("baselineLocation")]
		float BaselineLocation { get; }

		[Export ("requiredThickness")]
		float RequiredThickness { get; }

		[Export ("addMarker:")]
		void AddMarker (NSRulerMarker marker);

		[Export ("removeMarker:")]
		void RemoveMarker (NSRulerMarker marker);

		[Export ("trackMarker:withMouseEvent:")]
		bool TrackMarker (NSRulerMarker marker, NSEvent theEvent);

		[Export ("moveRulerlineFromLocation:toLocation:")]
		void MoveRulerline (float oldLocation, float newLocation);

		[Export ("invalidateHashMarks")]
		void InvalidateHashMarks ();

		[Export ("drawHashMarksAndLabelsInRect:")]
		void DrawHashMarksAndLabels (RectangleF rect);

		[Export ("drawMarkersInRect:")]
		void DrawMarkers (RectangleF rect);

		[Export ("isFlipped")]
		bool IsFlipped { get; }

		//Detected properties
		[Export ("scrollView")]
		NSScrollView ScrollView { get; set; }

		[Export ("orientation")]
		NSRulerOrientation Orientation { get; set; }

		[Export ("ruleThickness")]
		float RuleThickness { get; set; }

		[Export ("reservedThicknessForMarkers")]
		float ReservedThicknessForMarkers { get; set; }

		[Export ("reservedThicknessForAccessoryView")]
		float ReservedThicknessForAccessoryView { get; set; }

		[Export ("measurementUnits")]
		string MeasurementUnits { get; set; }

		[Export ("originOffset")]
		float OriginOffset { get; set; }

		[Export ("clientView")]
		NSView ClientView { get; set; }

		[Export ("markers")]
		NSRulerMarker [] Markers { get; set; }

		[Export ("accessoryView")]
		NSView AccessoryView { get; set; }
	}

	[BaseType (typeof (NSPanel), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSOpenSavePanelDelegate)})]
	interface NSSavePanel {
		[Static]
		[Export ("savePanel")]
		NSSavePanel SavePanel { get; }

		[Export ("URL")]
		NSUrl URL { get; }

		[Export ("isExpanded")]
		bool IsExpanded { get; }

		[Export ("validateVisibleColumns")]
		void ValidateVisibleColumns ();

		[Export ("ok:")]
		void Ok (NSObject sender);

		[Export ("cancel:")]
		void Cancel (NSObject sender);

		// FIXME: uses blocks
		//[Export ("beginSheetModalForWindow:completionHandler:NSIntegerresult))handler")]
		//void BeginSheetModal (NSWindow window, void (^ (NSInteger, );

		// FIXME: uses blocks
		//[Export ("beginWithCompletionHandler:NSIntegerresult))handler")]
		//void BeginWithCompletionHandlerNSIntegerresult))handler (void (^ (NSInteger, );

		[Export ("runModal")]
		int RunModal ();

		//Detected properties
		[Export ("directoryURL")]
		NSUrl DirectoryUrl { get; set; }

		[Export ("allowedFileTypes")]
		string [] AllowedFileTypes { get; set; }

		[Export ("allowsOtherFileTypes")]
		bool AllowsOtherFileTypes { get; set; }

		[Export ("accessoryView")]
		NSView AccessoryView { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSOpenSavePanelDelegate Delegate { get; set; }

		[Export ("canCreateDirectories")]
		bool CanCreateDirectories { get; set; }

		[Export ("canSelectHiddenExtension")]
		bool CanSelectHiddenExtension { get; set; }

		[Export ("extensionHidden")]
		bool ExtensionHidden { [Bind ("isExtensionHidden")]get; set; }

		[Export ("treatsFilePackagesAsDirectories")]
		bool TreatsFilePackagesAsDirectories { get; set; }

		[Export ("prompt")]
		string Prompt { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("nameFieldLabel")]
		string NameFieldLabel { get; set; }

		[Export ("nameFieldStringValue")]
		string NameFieldStringValue { get; set; }

		[Export ("message")]
		string Message { get; set; }

		[Export ("showsHiddenFiles")]
		bool ShowsHiddenFiles { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSOpenSavePanelDelegate {
		[Export ("panel:shouldEnableURL:"), EventArgs ("NSOpenSavePanelUrl"), DefaultValue (true)]
		bool ShouldEnableURL (NSObject sender, NSUrl url);

		// FIXME: binding
		//[Export ("panel:validateURL:error:")]
		//bool ValidateUrlerror (NSObject sender, NSUrl url, out NSError outError);

		[Export ("panel:didChangeToDirectoryURL:"), EventArgs ("NSOpenSavePanelUrl")]
		void DidChangeToDirectoryURL (NSObject sender, NSUrl url);

		[Export ("panel:userEnteredFilename:confirmed:"), EventArgs ("NSOpenSaveFilename"), DefaultValueFromArgument ("filename")]
		string UserEnteredFilename (NSObject sender, string filename, bool confirmed);

		[Export ("panel:willExpand:"), EventArgs ("NSOpenSaveExpanding")]
		void WillExpand (NSObject sender, bool expanding);

		[Export ("panelSelectionDidChange:"), EventArgs ("NSOpenSaveSelectionChanged")]
		void SelectionDidChange (NSObject sender);
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSSoundDelegate) })]
	interface NSSound {
		[Static]
		[Export ("soundNamed:")]
		NSSound FromName (string name);

		[Export ("initWithContentsOfURL:byReference:")]
		IntPtr Constructor (NSUrl url, bool byRef);

		[Export ("initWithContentsOfFile:byReference:")]
		IntPtr Constructor (string path, bool byRef);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Static]
		[Export ("canInitWithPasteboard:")]
		bool CanCreateFromPasteboard (NSPasteboard pasteboard);

		[Export ("soundUnfilteredTypes")]
		string [] SoundUnfilteredTypes ();

		[Export ("initWithPasteboard:")]
		IntPtr Constructor (NSPasteboard pasteboard);

		[Export ("writeToPasteboard:")]
		void WriteToPasteboard (NSPasteboard pasteboard);

		[Export ("play")]
		bool Play ();

		[Export ("pause")]
		bool Pause ();

		[Export ("resume")]
		bool Resume ();

		[Export ("stop")]
		bool Stop ();

		[Export ("isPlaying")]
		bool IsPlaying ();

		[Export ("duration")]
		double Duration ();

		//Detected properties
		[Export ("name")]
		string Name { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSoundDelegate Delegate { get; set; }

		[Export ("volume")]
		float Volume { get; set; }

		[Export ("currentTime")]
		double CurrentTime { get; set; }

		[Export ("loops")]
		bool Loops { get; set; }

		[Export ("playbackDeviceIdentifier")]
		string PlaybackDeviceID { get; set; }

		// FIXME: Poor docs, no type defined for the array elements
		[Export ("channelMapping")]
		NSObject ChannelMapping { get; set; }
	}

	[Model, BaseType (typeof (NSObject))]
	interface NSSoundDelegate {
		[Export ("sound:didFinishPlaying:"), EventArgs ("NSSoundFinished")]
		void DidFinishPlaying (NSSound sound, bool finished);
	}


	[BaseType (typeof (NSObject))]
	interface NSScreen {
		[Static]
		[Export ("screens")]
		NSScreen [] Screens { get; }

		[Static]
		[Export ("mainScreen")]
		NSScreen MainScreen { get; }

		[Static]
		[Export ("deepestScreen")]
		NSScreen DeepestScreen { get; }

		[Export ("depth")]
		NSWindowDepth Depth { get; }

		[Export ("frame")]
		RectangleF Frame { get; }

		[Export ("visibleFrame")]
		RectangleF VisibleFrame { get; }

		[Export ("deviceDescription")]
		NSDictionary DeviceDescription { get; }

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("supportedWindowDepths")]
		NSWindowDepth SupportedWindowDepths { get; }

		[Export ("userSpaceScaleFactor")]
		float UserSpaceScaleFactor { get; }
	}

	[BaseType (typeof (NSControl))]
	interface NSScroller {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Static]
		[Export ("scrollerWidth")]
		float ScrollerWidth { get; }

		[Static]
		[Export ("scrollerWidthForControlSize:")]
		float ScrollerWidthForControlSize (NSControlSize controlSize);

		[Export ("drawParts")]
		void DrawParts ();

		[Export ("rectForPart:")]
		RectangleF RectForPart (NSScrollerPart partCode);

		[Export ("checkSpaceForParts")]
		void CheckSpaceForParts ();

		[Export ("usableParts")]
		NSUsableScrollerParts UsableParts { get; }

		[Export ("drawArrow:highlight:")]
		void DrawArrowhighlight (NSScrollerArrow whichArrow, bool highlight);

		[Export ("drawKnob")]
		void DrawKnob ();

		[Export ("drawKnobSlotInRect:highlight:")]
		void DrawKnobSlot (RectangleF slotRect, bool highlight);

		[Export ("highlight:")]
		void Highlight (bool flag);

		[Export ("testPart:")]
		NSScrollerPart TestPart (PointF thePoint);

		[Export ("trackKnob:")]
		void TrackKnob (NSEvent theEvent);

		[Export ("trackScrollButtons:")]
		void TrackScrollButtons (NSEvent theEvent);

		[Export ("hitPart")]
		NSScrollerPart HitPart { get; }

		//Detected properties
		[Export ("arrowsPosition")]
		NSScrollArrowPosition ArrowsPosition { get; set; }

		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }

		[Export ("knobProportion")]
		float KnobProportion { get; set; }

	}

	[BaseType (typeof (NSView))]
	interface NSScrollView {
		[Static]
		[Export ("frameSizeForContentSize:hasHorizontalScroller:hasVerticalScroller:borderType:")]
		SizeF FrameSizeForContentSize (SizeF cSize, bool hFlag, bool vFlag, NSBorderType aType);

		[Static]
		[Export ("contentSizeForFrameSize:hasHorizontalScroller:hasVerticalScroller:borderType:")]
		SizeF ContentSizeForFrame (SizeF fSize, bool hFlag, bool vFlag, NSBorderType aType);

		[Export ("documentVisibleRect")]
		RectangleF DocumentVisibleRect { get; }

		[Export ("contentSize")]
		SizeF ContentSize { get; }

		[Export ("tile")]
		void Tile ();

		[Export ("reflectScrolledClipView:")]
		void ReflectScrolledClipView (NSClipView cView);

		[Export ("scrollWheel:")]
		void ScrollWheel (NSEvent theEvent);

		//Detected properties
		[Export ("documentView")]
		NSObject DocumentView { get; set; }

		[Export ("contentView")]
		NSClipView ContentView { get; set; }

		[Export ("documentCursor")]
		NSCursor DocumentCursor { get; set; }

		[Export ("borderType")]
		NSBorderType BorderType { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		[Export ("hasHorizontalScroller")]
		bool HasHorizontalScroller { get; set; }

		[Export ("verticalScroller")]
		NSScroller VerticalScroller { get; set; }

		[Export ("horizontalScroller")]
		NSScroller HorizontalScroller { get; set; }

		[Export ("autohidesScrollers")]
		bool AutohidesScrollers { get; set; }

		[Export ("horizontalLineScroll")]
		float HorizontalLineScroll { get; set; }

		[Export ("verticalLineScroll")]
		float VerticalLineScroll { get; set; }

		[Export ("lineScroll")]
		float LineScroll { get; set; }

		[Export ("horizontalPageScroll")]
		float HorizontalPageScroll { get; set; }

		[Export ("verticalPageScroll")]
		float VerticalPageScroll { get; set; }

		[Export ("pageScroll")]
		float PageScroll { get; set; }

		[Export ("scrollsDynamically")]
		bool ScrollsDynamically { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSShadow {
		[Export ("set")]
		void Set ();

		//Detected properties
		[Export ("shadowOffset")]
		SizeF ShadowOffset { get; set; }

		[Export ("shadowBlurRadius")]
		float ShadowBlurRadius { get; set; }

		[Export ("shadowColor")]
		NSColor ShadowColor { get; set; }

	}

	[BaseType (typeof (NSResponder))]
	interface NSView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("window")]
		NSWindow Window { get; }

		[Export ("superview")]
		NSView Superview { get; }

		[Export ("isDescendantOf:")]
		bool IsDescendantOf (NSView aView);

		[Export ("ancestorSharedWithView:")]
		NSView AncestorSharedWithView (NSView aView);

		[Export ("opaqueAncestor")]
		NSView OpaqueAncestor { get; }

		[Export ("isHiddenOrHasHiddenAncestor")]
		bool IsHiddenOrHasHiddenAncestor { get; }

		//[Export ("getRectsBeingDrawn:count:")]
		// void GetRectsBeingDrawn

		[Export ("needsToDrawRect:")]
		bool NeedsToDraw (RectangleF aRect);

		[Export ("wantsDefaultClipping")]
		bool WantsDefaultClipping { get; }

		[Export ("viewDidHide")]
		void ViewDidHide ();

		[Export ("viewDidUnhide")]
		void ViewDidUnhide ();

		[Export ("addSubview:")]
		void AddSubview (NSView aView);

		[Export ("addSubview:positioned:relativeTo:")]
		void AddSubview (NSView aView, NSWindowOrderingMode place, NSView otherView);

		[Export ("viewWillMoveToWindow:")]
		void ViewWillMoveToWindow (NSWindow newWindow);

		[Export ("viewDidMoveToWindow")]
		void ViewDidMoveToWindow ();

		[Export ("viewWillMoveToSuperview:")]
		void ViewWillMoveToSuperview (NSView newSuperview);

		[Export ("viewDidMoveToSuperview")]
		void ViewDidMoveToSuperview ();

		[Export ("didAddSubview:")]
		void DidAddSubview (NSView subview);

		[Export ("willRemoveSubview:")]
		void WillRemoveSubview (NSView subview);

		[Export ("removeFromSuperview")]
		void RemoveFromSuperview ();

		[Export ("replaceSubview:with:")]
		void ReplaceSubviewwith (NSView oldView, NSView newView);

		[Export ("removeFromSuperviewWithoutNeedingDisplay")]
		void RemoveFromSuperviewWithoutNeedingDisplay ();

		[Export ("resizeSubviewsWithOldSize:")]
		void ResizeSubviewsWithOldSize (SizeF oldSize);

		[Export ("resizeWithOldSuperviewSize:")]
		void ResizeWithOldSuperviewSize (SizeF oldSize);

		[Export ("setFrameOrigin:")]
		void SetFrameOrigin (PointF newOrigin);

		[Export ("setFrameSize:")]
		void SetFrameSize (SizeF newSize);

		[Export ("setBoundsOrigin:")]
		void SetBoundsOrigin (PointF newOrigin);

		[Export ("setBoundsSize:")]
		void SetBoundsSize (SizeF newSize);

		[Export ("translateOriginToPoint:")]
		void TranslateOriginToPoint (PointF translation);

		[Export ("scaleUnitSquareToSize:")]
		void ScaleUnitSquareToSize (SizeF newUnitSize);

		[Export ("rotateByAngle:")]
		void RotateByAngle (float angle);

		[Export ("isFlipped")]
		bool IsFlipped { get; }

		[Export ("isRotatedFromBase")]
		bool IsRotatedFromBase { get; }

		[Export ("isRotatedOrScaledFromBase")]
		bool IsRotatedOrScaledFromBase { get; }

		[Export ("isOpaque")]
		bool IsOpaque { get; }

		[Export ("convertPoint:fromView:")]
		PointF ConvertPointfromView (PointF aPoint, NSView aView);

		[Export ("convertPoint:toView:")]
		PointF ConvertPointtoView (PointF aPoint, NSView aView);

		[Export ("convertSize:fromView:")]
		SizeF ConvertSizefromView (SizeF aSize, NSView aView);

		[Export ("convertSize:toView:")]
		SizeF ConvertSizetoView (SizeF aSize, NSView aView);

		[Export ("convertRect:fromView:")]
		RectangleF ConvertRectfromView (RectangleF aRect, NSView aView);

		[Export ("convertRect:toView:")]
		RectangleF ConvertRecttoView (RectangleF aRect, NSView aView);

		[Export ("centerScanRect:")]
		RectangleF CenterScanRect (RectangleF aRect);

		[Export ("convertPointToBase:")]
		PointF ConvertPointToBase (PointF aPoint);

		[Export ("convertPointFromBase:")]
		PointF ConvertPointFromBase (PointF aPoint);

		[Export ("convertSizeToBase:")]
		SizeF ConvertSizeToBase (SizeF aSize);

		[Export ("convertSizeFromBase:")]
		SizeF ConvertSizeFromBase (SizeF aSize);

		[Export ("convertRectToBase:")]
		RectangleF ConvertRectToBase (RectangleF aRect);

		[Export ("convertRectFromBase:")]
		RectangleF ConvertRectFromBase (RectangleF aRect);

		[Export ("canDraw")]
		bool CanDraw ();

		[Export ("setNeedsDisplayInRect:")]
		void SetNeedsDisplayInRect (RectangleF invalidRect);

		[Export ("lockFocus")]
		void LockFocus ();

		[Export ("unlockFocus")]
		void UnlockFocus ();

		[Export ("lockFocusIfCanDraw")]
		bool LockFocusIfCanDraw ();

		[Export ("lockFocusIfCanDrawInContext:")]
		bool LockFocusIfCanDrawInContext (NSGraphicsContext context);

		[Export ("focusView")]
		NSView FocusView ();

		[Export ("visibleRect")]
		RectangleF VisibleRect ();

		[Export ("display")]
		void Display ();

		[Export ("displayIfNeeded")]
		void DisplayIfNeeded ();

		[Export ("displayIfNeededIgnoringOpacity")]
		void DisplayIfNeededIgnoringOpacity ();

		[Export ("displayRect:")]
		void DisplayRect (RectangleF rect);

		[Export ("displayIfNeededInRect:")]
		void DisplayIfNeededInRect (RectangleF rect);

		[Export ("displayRectIgnoringOpacity:")]
		void DisplayRectIgnoringOpacity (RectangleF rect);

		[Export ("displayIfNeededInRectIgnoringOpacity:")]
		void DisplayIfNeededInRectIgnoringOpacity (RectangleF rect);

		[Export ("drawRect:")]
		void DrawRect (RectangleF dirtyRect);

		[Export ("displayRectIgnoringOpacity:inContext:")]
		void DisplayRectIgnoringOpacityinContext (RectangleF aRect, NSGraphicsContext context);

		[Export ("bitmapImageRepForCachingDisplayInRect:")]
		NSBitmapImageRep BitmapImageRepForCachingDisplayInRect (RectangleF rect);

		[Export ("cacheDisplayInRect:toBitmapImageRep:")]
		void CacheDisplayInRecttoBitmapImageRep (RectangleF rect, NSBitmapImageRep bitmapImageRep);

		[Export ("viewWillDraw")]
		void ViewWillDraw ();

		[Export ("gState")]
		int GState ();

		[Export ("allocateGState")]
		void AllocateGState ();

		[Export ("releaseGState")]
		void ReleaseGState ();

		[Export ("setUpGState")]
		void SetUpGState ();

		[Export ("renewGState")]
		void RenewGState ();

		[Export ("scrollPoint:")]
		void ScrollPoint (PointF aPoint);

		[Export ("scrollRectToVisible:")]
		bool ScrollRectToVisible (RectangleF aRect);

		[Export ("autoscroll:")]
		bool Autoscroll (NSEvent theEvent);

		[Export ("adjustScroll:")]
		RectangleF AdjustScroll (RectangleF newVisible);

		[Export ("scrollRect:by:")]
		void ScrollRectby (RectangleF aRect, SizeF delta);

		[Export ("translateRectsNeedingDisplayInRect:by:")]
		void TranslateRectsNeedingDisplayInRectby (RectangleF clipRect, SizeF delta);

		[Export ("hitTest:")]
		NSView HitTest (PointF aPoint);

		[Export ("mouse:inRect:")]
		bool MouseinRect (PointF aPoint, RectangleF aRect);

		[Export ("viewWithTag:")]
		NSObject ViewWithTag (int aTag);

		[Export ("tag")]
		int Tag { get; }

		[Export ("performKeyEquivalent:")]
		bool PerformKeyEquivalent (NSEvent theEvent);

		[Export ("acceptsFirstMouse:")]
		bool AcceptsFirstMouse (NSEvent theEvent);

		[Export ("shouldDelayWindowOrderingForEvent:")]
		bool ShouldDelayWindowOrderingForEvent (NSEvent theEvent);

		[Export ("needsPanelToBecomeKey")]
		bool NeedsPanelToBecomeKey { get; }

		[Export ("mouseDownCanMoveWindow")]
		bool MouseDownCanMoveWindow { get; }

		[Export ("addCursorRect:cursor:")]
		void AddCursorRectcursor (RectangleF aRect, NSCursor anObj);

		[Export ("removeCursorRect:cursor:")]
		void RemoveCursorRectcursor (RectangleF aRect, NSCursor anObj);

		[Export ("discardCursorRects")]
		void DiscardCursorRects ();

		[Export ("resetCursorRects")]
		void ResetCursorRects ();

		[Export ("addTrackingRect:owner:userData:assumeInside:")]
		int AddTrackingRect (RectangleF aRect, NSObject anObject, IntPtr data, bool flag);

		[Export ("removeTrackingRect:")]
		void RemoveTrackingRect (int tag);

		[Export ("makeBackingLayer")]
		CALayer MakeBackingLayer ();

		[Export ("addTrackingArea:")]
		void AddTrackingArea (NSTrackingArea trackingArea);

		[Export ("removeTrackingArea:")]
		void RemoveTrackingArea (NSTrackingArea trackingArea);

		[Export ("trackingAreas")]
		NSTrackingArea [] TrackingAreas ();

		[Export ("updateTrackingAreas")]
		void UpdateTrackingAreas ();

		[Export ("shouldDrawColor")]
		bool ShouldDrawColor { get; }

		[Export ("enclosingScrollView")]
		NSScrollView EnclosingScrollView { get; }

		[Export ("menuForEvent:")]
		NSMenu MenuForEvent (NSEvent theEvent);

		[Static]
		[Export ("defaultMenu")]
		NSMenu DefaultMenu ();

		[Export ("addToolTipRect:owner:userData:")]
		int AddToolTip (RectangleF aRect, NSObject anObject, IntPtr data);

		[Export ("removeToolTip:")]
		void RemoveToolTip (int tag);

		[Export ("removeAllToolTips")]
		void RemoveAllToolTips ();

		[Export ("viewWillStartLiveResize")]
		void ViewWillStartLiveResize ();

		[Export ("viewDidEndLiveResize")]
		void ViewDidEndLiveResize ();

		[Export ("inLiveResize")]
		bool InLiveResize { get; }

		[Export ("preservesContentDuringLiveResize")]
		bool PreservesContentDuringLiveResize { get; }

		[Export ("rectPreservedDuringLiveResize")]
		RectangleF RectPreservedDuringLiveResize { get; }

		//[Export ("getRectsExposedDuringLiveResize:count:")]
		// void GetRectsExposedDuringLiveResizecount

		[Export ("inputContext")]
		NSTextInputContext InputContext { get; }

		//Detected properties
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")]get; set; }

		[Export ("subviews")]
		NSView [] Subviews { get; set; }

		[Export ("postsFrameChangedNotifications")]
		bool PostsFrameChangedNotifications { get; set; }

		[Export ("autoresizesSubviews")]
		bool AutoresizesSubviews { get; set; }

		[Export ("autoresizingMask")]
		NSViewResizingMask AutoresizingMask { get; set; }

		[Export ("frame")]
		RectangleF Frame { get; set; }

		[Export ("frameRotation")]
		float FrameRotation { get; set; }

		[Export ("frameCenterRotation")]
		float FrameCenterRotation { get; set; }

		[Export ("boundsRotation")]
		float BoundsRotation { get; set; }

		[Export ("bounds")]
		RectangleF Bounds { get; set; }

		[Export ("canDrawConcurrently")]
		bool CanDrawConcurrently { get; set; }

		[Export ("needsDisplay")]
		bool NeedsDisplay { get; set; }

		[Export ("acceptsTouchEvents")]
		bool AcceptsTouchEvents { get; set; }

		[Export ("wantsRestingTouches")]
		bool WantsRestingTouches { get; set; }

		[Export ("layerContentsRedrawPolicy")]
		NSViewLayerContentsRedrawPolicy LayerContentsRedrawPolicy { get; set; }

		[Export ("layerContentsPlacement")]
		NSViewLayerContentsPlacement LayerContentsPlacement { get; set; }

		[Export ("wantsLayer")]
		bool WantsLayer { get; set; }

		[Export ("layer")]
		CALayer Layer { get; set; }

		[Export ("alphaValue")]
		float AlphaValue { get; set; }

		// FIXME: CIFilter
		
		//[Export ("backgroundFilters")]
		//CIFilter [] BackgroundFilters { get; set; }

		//[Export ("compositingFilter")]
		//CIFilter CompositingFilter { get; set; }

		//[Export ("contentFilters")]
		//CIFilter [] ContentFilters { get; set; }

		[Export ("shadow")]
		NSShadow Shadow { get; set; }

		[Export ("postsBoundsChangedNotifications")]
		bool PostsBoundsChangedNotifications { get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }
	}

	[BaseType (typeof (NSAnimation))]
	interface NSViewAnimation { 
		// Constant: NSString* NSViewAnimationTargetKey
		// Constant: NSString* NSViewAnimationStartFrameKey
		// Constant: NSString* NSViewAnimationEndFrameKey
		// Constant: NSString* NSViewAnimationEffectKey 
		// Constant: NSString* NSViewAnimationFadeInEffect
		// Constant: NSString* NSViewAnimationFadeOutEffect
		[Export ("initWithViewAnimations:")]
		IntPtr Constructor (NSDictionary [] viewAnimations);
	
		[Export ("viewAnimations")]
		NSDictionary [] ViewAnimations { get; set; }
	
		[Export ("animator")]
		NSObject Animator { get; }
	
		[Export ("animations")]
		NSDictionary Animations  { get; set; }
	
		[Export ("animationForKey:")]
		NSObject AnimationForKey (string  key);
	
		[Static]
		[Export ("defaultAnimationForKey:")]
		NSObject DefaultAnimationForKey (string  key);
	
	}
	

	[BaseType (typeof (NSResponder))]
	interface NSViewController {
		[Export ("initWithNibName:bundle:")]
		IntPtr Constructor (string nibNameOrNil, NSBundle nibBundleOrNil);

		[Export ("loadView")]
		void LoadView ();

		[Export ("nibName")]
		string NibName { get; }

		[Export ("nibBundle")]
		NSBundle NibBundle { get; }

		[Export ("commitEditingWithDelegate:didCommitSelector:contextInfo:")]
		void CommitEditing (NSObject delegateObject, Selector didCommitSelector, IntPtr contextInfo);

		[Export ("commitEditing")]
		bool CommitEditing ();

		[Export ("discardEditing")]
		void DiscardEditing ();

		//Detected properties
		[Export ("representedObject")]
		NSObject RepresentedObject { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("view")]
		NSView View { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSTableColumn {
		[Export ("initWithIdentifier:")]
		IntPtr Constructor (NSObject identifier);
	
		[Export ("dataCellForRow:")]
		NSCell DataCellForRow (int row);
		
		[Export ("sizeToFit")]
		void SizeToFit ();

		//Detected properties
		[Export ("identifier")]
		NSObject Identifier { get; set; }
		
		[Export ("tableView")]
		NSTableView TableView { get; set; }
		
		[Export ("width")]
		float Width { get; set; }
		
		[Export ("minWidth")]
		float MinWidth { get; set; }
		
		[Export ("maxWidth")]
		float MaxWidth { get; set; }
	
		[Export ("headerCell")]
		NSCell HeaderCell { get; set; }

		[Export ("dataCell")]
		NSCell DataCell { get; set; }
	
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }
	
		[Export ("sortDescriptorPrototype")]
		NSSortDescriptor SortDescriptorPrototype { get; set; }
	
		[Export ("resizingMask")]
		NSTableColumnResizingMask ResizingMask { get; set; }
	
		[Export ("headerToolTip")]
		string HeaderToolTip { get; set; }
	
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")]get; set; }
	}
	
	
	[BaseType (typeof (NSControl))]
	//, Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTableViewDelegate)})]
	interface NSTableView {
		[Export ("noteHeightOfRowsWithIndexesChanged:")]
		void NoteHeightOfRowsWithIndexesChanged (NSIndexSet indexSet );
	
		[Export ("tableColumns")]
		NSTableColumn[] TableColumns ();
	
		[Export ("numberOfColumns")]
		int ColumnCount { get; }
	
		[Export ("numberOfRows")]
		int RowCount { get; }
	
		[Export ("addTableColumn:")]
		void AddColumn (NSTableColumn tableColumn);
	
		[Export ("removeTableColumn:")]
		void RemoveColumn (NSTableColumn tableColumn);
	
		[Export ("moveColumn:toColumn:")]
		void MoveColumn (int oldIndex, int newIndex);
	
		[Export ("columnWithIdentifier:")]
		int FindColumn (NSObject identifier);
	
		[Export ("tableColumnWithIdentifier:")]
		NSTableColumn FindTableColumn (NSObject identifier);
	
		[Export ("tile")]
		void Tile ();
	
		[Export ("sizeToFit")]
		void SizeToFit ();
	
		[Export ("sizeLastColumnToFit")]
		void SizeLastColumnToFit ();
	
		[Export ("scrollRowToVisible:")]
		void ScrollRowToVisible (int row);
	
		[Export ("scrollColumnToVisible:")]
		void ScrollColumnToVisible (int column);
	
		[Export ("reloadData")]
		void ReloadData ();
	
		[Export ("noteNumberOfRowsChanged")]
		void NoteNumberOfRowsChanged ();
	
		[Export ("reloadDataForRowIndexes:columnIndexes:")]
		void ReloadData (NSIndexSet rowIndexes, NSIndexSet columnIndexes );
	
		[Export ("editedColumn")]
		int EditedColumn { get; }
	
		[Export ("editedRow")]
		int EditedRow { get; }
	
		[Export ("clickedColumn")]
		int ClickedColumn { get; }
	
		[Export ("clickedRow")]
		int ClickedRow { get; }
	
		[Export ("setIndicatorImage:inTableColumn:")]
		void SetIndicatorImage (NSImage anImage, NSTableColumn tableColumn);
	
		[Export ("indicatorImageInTableColumn:")]
		NSImage GetIndicatorImage (NSTableColumn tableColumn);
	
		[Export ("canDragRowsWithIndexes:atPoint:")]
		bool CanDragRows (NSIndexSet rowIndexes, PointF mouseDownPoint );
	
		// FIXME: binding, NSPointPointer
		//[Export ("dragImageForRowsWithIndexes:tableColumns:event:offset:")]
		//NSImage DragImageForRowsWithIndexestableColumnseventoffset (NSIndexSet dragRows, NSArray tableColumns, NSEvent dragEvent, NSPointPointer dragImageOffset );
	
		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSourceOperationMask (NSDragOperation mask, bool isLocal);
	
		[Export ("setDropRow:dropOperation:")]
		void SetDropRowDropOperation (int row, NSTableViewDropOperation dropOperation);
	
		[Export ("selectAll:")]
		void SelectAll (NSObject sender);
	
		[Export ("deselectAll:")]
		void DeselectAll (NSObject sender);
	
		[Export ("selectColumnIndexes:byExtendingSelection:")]
		void SelectColumns (NSIndexSet indexes, bool extend );
	
		[Export ("selectRowIndexes:byExtendingSelection:")]
		void SelectRows (NSIndexSet indexes, bool extend );
	
		[Export ("selectedColumnIndexes")]
		NSIndexSet SelectedColumns ();
	
		[Export ("selectedRowIndexes")]
		NSIndexSet SelectedRows ();
	
		[Export ("deselectColumn:")]
		void DeselectColumn (int column);
	
		[Export ("deselectRow:")]
		void DeselectRow (int row);
	
		[Export ("selectedColumn")]
		int SelectedColumn { get; }
	
		[Export ("selectedRow")]
		int SelectedRow { get; }
	
		[Export ("isColumnSelected:")]
		bool IsColumnSelected (int column);
	
		[Export ("isRowSelected:")]
		bool IsRowSelected (int row);
	
		[Export ("numberOfSelectedColumns")]
		int SelectedColumnsCount { get; }
	
		[Export ("numberOfSelectedRows")]
		int SelectedRowCount { get; }
	
		[Export ("rectOfColumn:")]
		RectangleF RectForColumn (int column);
	
		[Export ("rectOfRow:")]
		RectangleF RectForRow (int row);
	
		[Export ("columnIndexesInRect:")]
		NSIndexSet GetColumnIndexesInRect (RectangleF rect);
	
		[Export ("rowsInRect:")]
		NSRange RowsInRect (RectangleF rect);
	
		[Export ("columnAtPoint:")]
		int GetColumn (PointF point);
	
		[Export ("rowAtPoint:")]
		int GetRow (PointF point);
	
		[Export ("frameOfCellAtColumn:row:")]
		RectangleF GetCellFrame (int column, int row);
	
		[Export ("preparedCellAtColumn:row:")]
		NSCell GetCell (int column, int row );
	
		[Export ("textShouldBeginEditing:")]
		bool TextShouldBeginEditing (NSText textObject);
	
		[Export ("textShouldEndEditing:")]
		bool TextShouldEndEditing (NSText textObject);
	
		[Export ("textDidBeginEditing:")]
		void TextDidBeginEditing (NSNotification notification);
	
		[Export ("textDidEndEditing:")]
		void TextDidEndEditing (NSNotification notification);
	
		[Export ("textDidChange:")]
		void TextDidChange (NSNotification notification);
	
		[Export ("shouldFocusCell:atColumn:row:")]
		bool ShouldFocusCell (NSCell cell, int column, int row );
	
		[Export ("performClickOnCellAtColumn:row:")]
		void PerformClick (int column, int row );
	
		[Export ("editColumn:row:withEvent:select:")]
		void EditColumn (int column, int row, NSEvent theEvent, bool select);
	
		[Export ("drawRow:clipRect:")]
		void DrawRow (int row, RectangleF clipRect);
	
		[Export ("highlightSelectionInClipRect:")]
		void HighlightSelection (RectangleF clipRect);
	
		[Export ("drawGridInClipRect:")]
		void DrawGrid (RectangleF clipRect);
	
		[Export ("drawBackgroundInClipRect:")]
		void DrawBackground (RectangleF clipRect );
	
		//Detected properties
		[Export ("dataSource")][NullAllowed]
		NSTableViewDataSource DataSource { get; set; }
	
		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }
	
		[Wrap ("WeakDelegate")][NullAllowed]
		NSTableViewDelegate Delegate { get; set; }
	
		[Export ("headerView")]
		NSTableHeaderView HeaderView { get; set; }
	
		[Export ("cornerView")]
		NSView CornerView { get; set; }
	
		[Export ("allowsColumnReordering")]
		bool AllowsColumnReordering { get; set; }
	
		[Export ("allowsColumnResizing")]
		bool AllowsColumnResizing { get; set; }
	
		[Export ("columnAutoresizingStyle")]
		NSTableViewColumnAutoresizingStyle ColumnAutoresizingStyle { get; set; }
	
		[Export ("gridStyleMask")]
		NSTableViewGridStyleMask GridStyleMask { get; set; }
	
		[Export ("intercellSpacing")]
		SizeF IntercellSpacing { get; set; }
	
		[Export ("usesAlternatingRowBackgroundColors")]
		bool UsesAlternatingRowBackgroundColors { get; set; }
	
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("gridColor")]
		NSColor GridColor { get; set; }
	
		[Export ("rowHeight")]
		float RowHeight { get; set; }
	
		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }
	
		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; set; }
	
		[Export ("highlightedTableColumn")]
		NSTableColumn HighlightedTableColumn { get; set; }
	
		[Export ("verticalMotionCanBeginDrag")]
		bool VerticalMotionCanBeginDrag { get; set; }
	
		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }
	
		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }
	
		[Export ("allowsColumnSelection")]
		bool AllowsColumnSelection { get; set; }
	
		[Export ("allowsTypeSelect")]
		bool AllowsTypeSelect { get; set; }
	
		[Export ("selectionHighlightStyle")]
		NSTableViewSelectionHighlightStyle SelectionHighlightStyle { get; set; }
	
		[Export ("draggingDestinationFeedbackStyle")]
		NSTableViewDraggingDestinationFeedbackStyle DraggingDestinationFeedbackStyle { get; set; }
	
		[Export ("autosaveName")]
		string AutosaveName { get; set; }
	
		[Export ("autosaveTableColumns")]
		bool AutosaveTableColumns { get; set; }
	
		[Export ("focusedColumn")]
		int FocusedColumn { get; set; }
	} 
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NSTableViewDelegate {
		[Export ("tableView:willDispayCell:forTableColumn:row:")]
		void WillDisplayCell (NSTableView tableView, NSObject cell, NSTableColumn tableColumn, int row);
	
		[Export ("tableView:shouldEditTableColumn:row:")] [DefaultValue (false)]
		bool ShouldEditTableColumn (NSTableView tableView, NSTableColumn tableColumn, int row);
	
		[Export ("selectionShouldChangeInTableView:")] [DefaultValue (false)]
		bool SelectionShouldChange (NSTableView tableView);
	
		[Export ("tableView:shouldSelectRow:")] [DefaultValue (true)]
		bool ShouldSelectRow (NSTableView tableView, int row);
	
		[Export ("tableView:selectionIndexesForProposedSelection:")]
		NSIndexSet GetSelectionIndexes (NSTableView tableView, NSIndexSet proposedSelectionIndexes);
	
		[Export ("tableView:shouldSelectTableColumn:")] [DefaultValue (true)]
		bool ShouldSelectTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:mouseDownInHeaderOfTableColumn:")]
		void MouseDown (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didClickTableColumn:")]
		void DidClickTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didDragTableColumn:")]
		void DidDragTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		//FIXME: Binding NSRectPointer
		//[Export ("tableView:toolTipForCell:rect:tableColumn:row:mouseLocation:")]
		//string TableViewtoolTipForCellrecttableColumnrowmouseLocation (NSTableView tableView, NSCell cell, NSRectPointer rect, NSTableColumn tableColumn, int row, PointF mouseLocation);
	
		[Export ("tableView:heightOfRow:")]
		float GetRowHeight (NSTableView tableView, int row );
	
		[Export ("tableView:typeSelectStringForTableColumn:row:")]
		string GetSelectString (NSTableView tableView, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:nextTypeSelectMatchFromRow:toRow:forString:")]
		int GetNextTypeSelectMatch (NSTableView tableView, int startRow, int endRow, string searchString );
	
		[Export ("tableView:shouldTypeSelectForEvent:withCurrentSearchString:")]
		bool ShouldTypeSelect (NSTableView tableView, NSEvent theEvent, string searchString );
	
		[Export ("tableView:shouldShowCellExpansionForTableColumn:row:")]
		bool ShouldShowCellExpansion (NSTableView tableView, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:shouldTrackCell:forTableColumn:row:")]
		bool ShouldTrackCell (NSTableView tableView, NSCell cell, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:dataCellForTableColumn:row:")]
		NSCell GetCell (NSTableView tableView, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:isGroupRow:")] [DefaultValue (false)]
		bool IsGroupRow (NSTableView tableView, int row );
	
		[Export ("tableView:sizeToFitWidthOfColumn:")]
		float GetSizeToFitColumnWidth (NSTableView tableView, int column );
	
		[Export ("tableView:shouldReorderColumn:toColumn:")]
		bool ShouldReorder (NSTableView tableView, int columnIndex, int newColumnIndex );
	
		[Export ("tableViewSelectionDidChange:")]
		void SelectionDidChange (NSNotification notification);
	
		[Export ("tableViewColumnDidMove:")]
		void ColumnDidMove (NSNotification notification);
	
		[Export ("tableViewColumnDidResize:")]
		void ColumnDidResize (NSNotification notification);
	
		[Export ("tableViewSelectionIsChanging:")]
		void SelectionIsChanging (NSNotification notification);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface NSTableViewDataSource {
		[Export ("numberOfRowsInTableView:")]
		int GetRowCount (NSTableView tableView);
	
		[Export ("tableView:objectValueForTableColumn:row:")]
		NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row);
	
		[Export ("tableView:setObjectValue:forTableColumn:row:")]
		void SetObjectValue (NSTableView tableView, NSObject theObject, NSTableColumn tableColumn, int row);
	
		[Export ("tableView:sortDescriptorsDidChange:")]
		void SortDescriptorsChanged (NSTableView tableView, NSSortDescriptor [] oldDescriptors);
	
		[Export ("tableView:writeRowsWithIndexes:toPasteboard:")]
		bool WriteRows (NSTableView tableView, NSIndexSet rowIndexes, NSPasteboard pboard );
	
		[Export ("tableView:validateDrop:proposedRow:proposedDropOperation:")]
		NSDragOperation ValidateDrop (NSTableView tableView, NSDraggingInfo info, int row, NSTableViewDropOperation dropOperation);
	
		[Export ("tableView:acceptDrop:row:dropOperation:")]
		bool AcceptDrop (NSTableView tableView, NSDraggingInfo info, int row, NSTableViewDropOperation dropOperation);
	
		[Export ("tableView:namesOfPromisedFilesDroppedAtDestination:forDraggedRowsWithIndexes:")]
		string [] FilesDropped (NSTableView tableView, NSUrl dropDestination, NSIndexSet indexSet );
	}
	
	[BaseType (typeof (NSTextFieldCell))]
	interface NSTableHeaderCell {
		[Export ("drawSortIndicatorWithFrame:inView:ascending:priority:")]
		void DrawSortIndicator (RectangleF cellFrame, NSView controlView, bool ascending, int priority );
	
		[Export ("sortIndicatorRectForBounds:")]
		RectangleF GetSortIndicatorRect (RectangleF theRect );
	}
	
	[BaseType (typeof (NSView))]
	interface NSTableHeaderView {
		[Export ("draggedColumn")]
		int DraggedColumn { get; }
	
		[Export ("draggedDistance")]
		float DraggedDistance { get; }
	
		[Export ("resizedColumn")]
		int ResizedColumn { get; }
	
		[Export ("headerRectOfColumn:")]
		RectangleF GetHeaderRect (int column);
	
		[Export ("columnAtPoint:")]
		int GetColumn (PointF point);
	
		//Detected properties
		[Export ("tableView")]
		NSTableView TableView { get; set; }
	}
		
	[BaseType (typeof (NSView), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextDelegate)})]
	interface NSText {
		[Export ("replaceCharactersInRange:withString:")]
		void Replace (NSRange range, string aString);

		[Export ("replaceCharactersInRange:withRTF:")]
		void ReplaceWithRtf (NSRange range, NSData rtfData);

		[Export ("replaceCharactersInRange:withRTFD:")]
		void ReplaceWithRtfd (NSRange range, NSData rtfdData);

		[Export ("RTFFromRange:")]
		NSData RtfFromRange (NSRange range);

		[Export ("RTFDFromRange:")]
		NSData RtfdFromRange (NSRange range);

		[Export ("writeRTFDToFile:atomically:")]
		bool WriteRtfd (string path, bool atomically);

		[Export ("readRTFDFromFile:")]
		bool FromRtfdFile (string path);

		[Export ("isRulerVisible")]
		bool IsRulerVisible { get; }

		[Export ("scrollRangeToVisible:")]
		void ScrollRangeToVisible (NSRange range);

		[Export ("setTextColor:range:")]
		void SetTextColor (NSColor color, NSRange range);

		[Export ("setFont:range:")]
		void SetFont (NSFont font, NSRange range);

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("copy:")]
		void Copy (NSObject sender);

		[Export ("copyFont:")]
		void CopyFont (NSObject sender);

		[Export ("copyRuler:")]
		void CopyRuler (NSObject sender);

		[Export ("cut:")]
		void Cut (NSObject sender);

		[Export ("delete:")]
		void Delete (NSObject sender);

		[Export ("paste:")]
		void Paste (NSObject sender);

		[Export ("pasteFont:")]
		void PasteFont (NSObject sender);

		[Export ("pasteRuler:")]
		void PasteRuler (NSObject sender);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("changeFont:")]
		void ChangeFont (NSObject sender);

		[Export ("alignLeft:")]
		void AlignLeft (NSObject sender);

		[Export ("alignRight:")]
		void AlignRight (NSObject sender);

		[Export ("alignCenter:")]
		void AlignCenter (NSObject sender);

		[Export ("subscript:")]
		void Subscript (NSObject sender);

		[Export ("superscript:")]
		void Superscript (NSObject sender);

		[Export ("underline:")]
		void Underline (NSObject sender);

		[Export ("unscript:")]
		void Unscript (NSObject sender);

		[Export ("showGuessPanel:")]
		void ShowGuessPanel (NSObject sender);

		[Export ("checkSpelling:")]
		void CheckSpelling (NSObject sender);

		[Export ("toggleRuler:")]
		void ToggleRuler (NSObject sender);

		//Detected properties
		[Export ("string")]
		string Value { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSTextDelegate Delegate { get; set; }
		
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("richText")]
		bool RichText { [Bind ("isRichText")]get; set; }

		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }

		[Export ("fieldEditor")]
		bool FieldEditor { [Bind ("isFieldEditor")]get; set; }

		[Export ("usesFontPanel")]
		bool UsesFontPanel { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("selectedRange")]
		NSRange SelectedRange { get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }

		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; set; }

		[Export ("maxSize")]
		SizeF MaxSize { get; set; }

		[Export ("minSize")]
		SizeF MinSize { get; set; }

		[Export ("horizontallyResizable")]
		bool HorizontallyResizable { [Bind ("isHorizontallyResizable")]get; set; }

		[Export ("verticallyResizable")]
		bool VerticallyResizable { [Bind ("isVerticallyResizable")]get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSTextDelegate {
		[Export ("textShouldBeginEditing:"), EventArgs ("NSTextPredicate"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:"), EventArgs ("NSTextPredicate"), DefaultValue (true)]
		bool TextShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:"), EventArgs ("NSNotification")]
		void TextDidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:"), EventArgs ("NSNotification")]
		void TextDidEndEditing (NSNotification notification);

		[Export ("textDidChange:"), EventArgs ("NSNotification")]
		void TextDidChange (NSNotification notification);
	}

	[BaseType (typeof (NSCell))]
	interface NSTextAttachmentCell {
		[Export ("wantsToTrackMouse")]
		bool WantsToTrackMouse ();

		[Export ("highlight:withFrame:inView:")]
		void Highlight (bool flag, RectangleF cellFrame, NSView controlView);

		[Export ("trackMouse:inRect:ofView:untilMouseUp:")]
		bool TrackMouseinRectofViewuntilMouseUp (NSEvent theEvent, RectangleF cellFrame, NSView controlView, bool flag);

		[Export ("cellSize")]
		SizeF CellSize { get; }

		[Export ("cellBaselineOffset")]
		PointF CellBaselineOffset { get; }

		[Export ("drawWithFrame:inView:characterIndex:")]
		void DrawWithFrame (RectangleF cellFrame, NSView controlView, uint charIndex);

		[Abstract]
		[Export ("drawWithFrame:inView:characterIndex:layoutManager:")]
		void DrawWithFrame (RectangleF cellFrame, NSView controlView, uint charIndex, NSLayoutManager layoutManager);

		[Abstract]
		[Export ("wantsToTrackMouseForEvent:inRect:ofView:atCharacterIndex:")]
		bool WantsToTrackMouse (NSEvent theEvent, RectangleF cellFrame, NSView controlView, uint charIndex);

		[Export ("trackMouse:inRect:ofView:atCharacterIndex:untilMouseUp:")]
		bool TrackMouse (NSEvent theEvent, RectangleF cellFrame, NSView controlView, uint charIndex, bool flag);

		[Export ("cellFrameForTextContainer:proposedLineFragment:glyphPosition:characterIndex:")]
		RectangleF CellFrameForTextContainer (NSTextContainer textContainer, RectangleF lineFrag, PointF position, uint charIndex);

		//Detected properties
		[Export ("attachment")]
		NSTextAttachment Attachment { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSTextAttachment {
		[Export ("initWithFileWrapper:")]
		IntPtr Constructor (NSFileWrapper fileWrapper);

		//Detected properties
		[Export ("fileWrapper")]
		NSFileWrapper FileWrapper { get; set; }

		[Export ("attachmentCell")]
		NSTextAttachmentCell AttachmentCell { get; set; }

	}

	[BaseType (typeof (NSObject))]
	interface NSTextBlock {
		[Export ("setValue:type:forDimension:")]
		void SetValue (float val, NSTextBlockValueType type, NSTextBlockDimension dimension);

		[Export ("valueForDimension:")]
		float GetValue (NSTextBlockDimension dimension);

		[Export ("valueTypeForDimension:")]
		NSTextBlockValueType GetValueType (NSTextBlockDimension dimension);

		[Export ("setContentWidth:type:")]
		void SetContentWidth (float val, NSTextBlockValueType type);

		[Export ("contentWidth")]
		float ContentWidth { get; }

		[Export ("contentWidthValueType")]
		NSTextBlockValueType ContentWidthValueType { get; }

		[Export ("setWidth:type:forLayer:edge:")]
		void SetWidth (float val, NSTextBlockValueType type, NSTextBlockLayer layer, NSRectEdge edge);

		[Export ("setWidth:type:forLayer:")]
		void SetWidth (float val, NSTextBlockValueType type, NSTextBlockLayer layer);

		[Export ("widthForLayer:edge:")]
		float GetWidth (NSTextBlockLayer layer, NSRectEdge edge);

		[Export ("widthValueTypeForLayer:edge:")]
		NSTextBlockValueType WidthValueTypeForLayeredge (NSTextBlockLayer layer, NSRectEdge edge);

		[Export ("setBorderColor:forEdge:")]
		void SetBorderColor (NSColor color, NSRectEdge edge);

		[Export ("setBorderColor:")]
		void SetBorderColor (NSColor color);

		[Export ("borderColorForEdge:")]
		NSColor GetBorderColor (NSRectEdge edge);

		[Export ("rectForLayoutAtPoint:inRect:textContainer:characterRange:")]
		RectangleF GetRectForLayout (PointF startingPoint, RectangleF rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("boundsRectForContentRect:inRect:textContainer:characterRange:")]
		RectangleF GetBoundsRect (RectangleF contentRect, RectangleF rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("drawBackgroundWithFrame:inView:characterRange:layoutManager:")]
		void DrawBackground (RectangleF frameRect, NSView controlView, NSRange charRange, NSLayoutManager layoutManager);

		//Detected properties
		[Export ("verticalAlignment")]
		NSTextBlockVerticalAlignment VerticalAlignment { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

	}

	[BaseType (typeof (NSControl), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextFieldDelegate)})]
	interface NSTextField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);
		
		[Export ("selectText:")]
		void SelectText (NSObject sender);

		[Export ("textShouldBeginEditing:")]
		bool ShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:")]
		bool ShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:")]
		void DidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:")]
		void DidEndEditing (NSNotification notification);

		[Export ("textDidChange:")]
		void DidChange (NSNotification notification);

		[Export ("acceptsFirstResponder")]
		bool AcceptsFirstResponder ();

		//Detected properties
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")]get; set; }

		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")]get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		NSTextFieldDelegate Delegate { get; set; }

		[Export ("bezelStyle")]
		NSTextFieldBezelStyle BezelStyle { get; set; }

		[Export ("allowsEditingTextAttributes")]
		bool AllowsEditingTextAttributes { get; set; }

		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSTextFieldDelegate {
		[Export ("control:textShouldBeginEditing:"), EventArgs ("NSControlText"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSControl control, NSText fieldEditor);

		[Export ("control:textShouldEndEditing:"), EventArgs ("NSControlText"), DefaultValue (true)]
		bool TextShouldEndEditing (NSControl control, NSText fieldEditor);

		[Export ("control:didFailToFormatString:errorDescription:"), EventArgs ("NSControlTextError"), DefaultValue (true)]
		bool DidFailToFormatString (NSControl control, string str, string error);
		
		[Export ("control:didFailToValidatePartialString:errorDescription:"), EventArgs ("NSControlTextError")]
		void DidFailToValidatePartialString (NSControl control, string str, string error);
		
		[Export ("control:isValidObject:"), EventArgs ("NSControlTextValidation"), DefaultValue (true)]
		bool IsValidObject (NSControl control, NSObject objectToValidate);

		[Export ("control:textView:doCommandBySelector:"), EventArgs ("NSControlCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSControl control, NSTextView textView, Selector commandSelector);

		[Export ("control:textView:completions:forPartialWordRange:indexOfSelectedItem:"), EventArgs ("NSControlTextFilter"), DefaultValue (null)]
		string [] FilterCompletions (NSControl control, NSTextView textView, string [] words, NSRange charRange, int index);
	}
	
	[BaseType (typeof (NSActionCell))]
	interface NSTextFieldCell {
		[Export ("setUpFieldEditorAttributes:")]
		NSText SetUpFieldEditorAttributes (NSText textObj);
	
		[Export ("setWantsNotificationForMarkedText:")]
		void SetWantsNotificationForMarkedText (bool flag);
	
		//Detected properties
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }
	
		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }
	
		[Export ("textColor")]
		NSColor TextColor { get; set; }
	
		[Export ("bezelStyle")]
		NSTextFieldBezelStyle BezelStyle { get; set; }
	
		[Export ("placeholderString")]
		string PlaceholderString { get; set; }
	
		[Export ("placeholderAttributedString")]
		NSAttributedString PlaceholderAttributedString { get; set; }
	
		[Export ("allowedInputSourceLocales")]
		string [] AllowedInputSourceLocales { get; set; }
	}
	
	[BaseType (typeof (NSObject))]
	interface NSTextInputContext {
		[Export ("currentInputContext")]
		NSTextInputContext CurrentInputContext { get; }

		[Export ("activate")]
		void Activate ();

		[Export ("deactivate")]
		void Deactivate ();

		[Export ("handleEvent:")]
		bool HandleEvent (NSEvent theEvent);

		[Export ("discardMarkedText")]
		void DiscardMarkedText ();

		[Export ("invalidateCharacterCoordinates")]
		void InvalidateCharacterCoordinates ();

		[Static]
		[Export ("localizedNameForInputSource:")]
		string LocalizedNameForInputSource (string inputSourceIdentifier);
	}

	[BaseType (typeof (NSObject))]
	interface NSTextList {
		[Export ("initWithMarkerFormat:options:")]
		IntPtr Constructor (string format, NSTextListOptions mask);

		[Export ("markerFormat")]
		string MarkerFormat { get; }

		[Export ("listOptions")]
		NSTextListOptions ListOptions { get; }

		[Export ("markerForItemNumber:")]
		string GetMarker (int itemNum);

		//Detected properties
		[Export ("startingItemNumber")]
		int StartingItemNumber { get; set; }

	}
	
	[BaseType (typeof (NSTextBlock))]
	interface NSTextTableBlock {
		[Export ("initWithTable:startingRow:rowSpan:startingColumn:columnSpan:")]
		IntPtr Constructor (NSTextTable table, int row, int rowSpan, int col, int colSpan);

		[Export ("table")]
		NSTextTable Table { get; }

		[Export ("startingRow")]
		int StartingRow { get; }

		[Export ("rowSpan")]
		int RowSpan { get; }

		[Export ("startingColumn")]
		int StartingColumn { get; }

		[Export ("columnSpan")]
		int ColumnSpan { get; }
	}

	[BaseType (typeof (NSTextBlock))]
	interface NSTextTable {
		[Export ("rectForBlock:layoutAtPoint:inRect:textContainer:characterRange:")]
		RectangleF GetRectForBlock (NSTextTableBlock block, PointF startingPoint, RectangleF rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("boundsRectForBlock:contentRect:inRect:textContainer:characterRange:")]
		RectangleF GetBoundsRect (NSTextTableBlock block, RectangleF contentRect, RectangleF rect, NSTextContainer textContainer, NSRange charRange);

		[Export ("drawBackgroundForBlock:withFrame:inView:characterRange:layoutManager:")]
		void DrawBackground (NSTextTableBlock block, RectangleF frameRect, NSView controlView, NSRange charRange, NSLayoutManager layoutManager);

		//Detected properties
		[Export ("numberOfColumns")]
		int Columns { get; set; }

		[Export ("layoutAlgorithm")]
		NSTextTableLayoutAlgorithm LayoutAlgorithm { get; set; }

		[Export ("collapsesBorders")]
		bool CollapsesBorders { get; set; }

		[Export ("hidesEmptyCells")]
		bool HidesEmptyCells { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSTextContainer {
		[Export ("initWithContainerSize:")]
		IntPtr Constructor (SizeF size);

		[Export ("replaceLayoutManager:")]
		void ReplaceLayoutManager (NSLayoutManager newLayoutManager);

		// FIXME: Binding
		//[Export ("lineFragmentRectForProposedRect:sweepDirection:movementDirection:remainingRect:")]
		//RectangleF LineFragmentRect (RectangleF proposedRect, NSLineSweepDirection sweepDirection, NSLineMovementDirection movementDirection, NSRectPointer remainingRect);

		[Export ("isSimpleRectangularTextContainer")]
		bool IsSimpleRectangularTextContainer { get; }

		[Export ("containsPoint:")]
		bool ContainsPoint (PointF point);

		//Detected properties
		[Export ("layoutManager")]
		NSLayoutManager LayoutManager { get; set; }

		[Export ("textView")]
		NSTextView TextView { get; set; }

		[Export ("widthTracksTextView")]
		bool WidthTracksTextView { get; set; }

		[Export ("heightTracksTextView")]
		bool HeightTracksTextView { get; set; }

		[Export ("containerSize")]
		SizeF ContainerSize { get; set; }

		[Export ("lineFragmentPadding")]
		float LineFragmentPadding { get; set; }
	}

	[BaseType (typeof (NSMutableAttributedString), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextStorageDelegate)})]
	interface NSTextStorage {
		[Export ("addLayoutManager:")]
		void AddLayoutManager (NSLayoutManager obj);

		[Export ("removeLayoutManager:")]
		void RemoveLayoutManager (NSLayoutManager obj);

		[Export ("layoutManagers")]
		NSLayoutManager [] LayoutManagers { get; }

		[Export ("edited:range:changeInLength:")]
		void Edited (uint editedMask, NSRange range, int delta);

		[Export ("processEditing")]
		void ProcessEditing ();

		[Export ("invalidateAttributesInRange:")]
		void InvalidateAttributes (NSRange range);

		[Export ("ensureAttributesAreFixedInRange:")]
		void EnsureAttributesAreFixed (NSRange range);

		[Export ("fixesAttributesLazily")]
		bool FixesAttributesLazily { get; }

		[Export ("editedMask")]
		NSTextStorageEditedFlags EditedMask { get; }

		[Export ("editedRange")]
		NSRange EditedRange { get; }

		[Export ("changeInLength")]
		int ChangeInLength { get; }

		//Detected properties
		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }
		[Wrap ("WeakDelegate")]
		NSTextStorageDelegate Delegate { get; set; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSTextStorageDelegate {
		[Export ("textStorageWillProcessEditing:")]
		void TextStorageWillProcessEditing (NSNotification notification);

		[Export ("textStorageDidProcessEditing:")]
		void TextStorageDidProcessEditing (NSNotification notification);
	}

	[BaseType (typeof (NSObject))]
	interface NSTextTab {
		[Export ("initWithTextAlignment:location:options:")]
		IntPtr Constructor (NSTextAlignment alignment, float loc, NSDictionary options);

		[Export ("alignment")]
		NSTextAlignment Alignment { get; }

		[Export ("options")]
		NSDictionary Options { get; }

		[Export ("initWithType:location:")]
		IntPtr Constructor (NSTextTabType type, float loc);

		[Export ("location")]
		float Location { get; }

		[Export ("tabStopType")]
		NSTextTabType TabStopType { get; }
	}

	[BaseType (typeof (NSText))]
	interface NSTextView {
		[Export ("initWithFrame:textContainer:")]
		IntPtr Constructor (RectangleF frameRect, NSTextContainer container);

		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("replaceTextContainer:")]
		void ReplaceTextContainer (NSTextContainer newContainer);

		[Export ("textContainerOrigin")]
		PointF TextContainerOrigin { get; }

		[Export ("invalidateTextContainerOrigin")]
		void InvalidateTextContainerOrigin ();

		[Export ("layoutManager")]
		NSLayoutManager LayoutManager { get; }

		[Export ("textStorage")]
		NSTextStorage TextStorage { get; }

		[Export ("insertText:")]
		void InsertText (NSObject insertString);

		[Export ("setConstrainedFrameSize:")]
		void SetConstrainedFrameSize (SizeF desiredSize);

		[Export ("setAlignment:range:")]
		void SetAlignmentrange (NSTextAlignment alignment, NSRange range);

		[Export ("setBaseWritingDirection:range:")]
		void SetBaseWritingDirectionrange (NSWritingDirection writingDirection, NSRange range);

		[Export ("turnOffKerning:")]
		void TurnOffKerning (NSObject sender);

		[Export ("tightenKerning:")]
		void TightenKerning (NSObject sender);

		[Export ("loosenKerning:")]
		void LoosenKerning (NSObject sender);

		[Export ("useStandardKerning:")]
		void UseStandardKerning (NSObject sender);

		[Export ("turnOffLigatures:")]
		void TurnOffLigatures (NSObject sender);

		[Export ("useStandardLigatures:")]
		void UseStandardLigatures (NSObject sender);

		[Export ("useAllLigatures:")]
		void UseAllLigatures (NSObject sender);

		[Export ("raiseBaseline:")]
		void RaiseBaseline (NSObject sender);

		[Export ("lowerBaseline:")]
		void LowerBaseline (NSObject sender);

		[Export ("toggleTraditionalCharacterShape:")]
		void ToggleTraditionalCharacterShape (NSObject sender);

		[Export ("outline:")]
		void Outline (NSObject sender);

		[Export ("performFindPanelAction:")]
		void PerformFindPanelAction (NSObject sender);

		[Export ("alignJustified:")]
		void AlignJustified (NSObject sender);

		[Export ("changeColor:")]
		void ChangeColor (NSObject sender);

		[Export ("changeAttributes:")]
		void ChangeAttributes (NSObject sender);

		[Export ("changeDocumentBackgroundColor:")]
		void ChangeDocumentBackgroundColor (NSObject sender);

		[Export ("orderFrontSpacingPanel:")]
		void OrderFrontSpacingPanel (NSObject sender);

		[Export ("orderFrontLinkPanel:")]
		void OrderFrontLinkPanel (NSObject sender);

		[Export ("orderFrontListPanel:")]
		void OrderFrontListPanel (NSObject sender);

		[Export ("orderFrontTablePanel:")]
		void OrderFrontTablePanel (NSObject sender);

		[Export ("rulerView:didMoveMarker:")]
		void RulerViewDidMoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:didRemoveMarker:")]
		void RulerViewDidRemoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:didAddMarker:")]
		void RulerViewDidAddMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:shouldMoveMarker:")]
		bool RulerViewShouldMoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:shouldAddMarker:")]
		bool RulerViewShouldAddMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:willMoveMarker:toLocation:")]
		float RulerViewWillMoveMarkertoLocation (NSRulerView ruler, NSRulerMarker marker, float location);

		[Export ("rulerView:shouldRemoveMarker:")]
		bool RulerViewShouldRemoveMarker (NSRulerView ruler, NSRulerMarker marker);

		[Export ("rulerView:willAddMarker:atLocation:")]
		float RulerViewWillAddMarkeratLocation (NSRulerView ruler, NSRulerMarker marker, float location);

		[Export ("rulerView:handleMouseDown:")]
		void RulerViewHandleMouseDown (NSRulerView ruler, NSEvent theEvent);

		[Export ("setNeedsDisplayInRect:avoidAdditionalLayout:")]
		void SetNeedsDisplay (RectangleF rect, bool avoidAdditionalLayout);

		[Export ("shouldDrawInsertionPoint")]
		bool ShouldDrawInsertionPoint { get; }

		[Export ("drawInsertionPointInRect:color:turnedOn:")]
		void DrawInsertionPointInRectcolorturnedOn (RectangleF rect, NSColor color, bool flag);

		[Export ("drawViewBackgroundInRect:")]
		void DrawViewBackgroundInRect (RectangleF rect);

		[Export ("updateRuler")]
		void UpdateRuler ();

		[Export ("updateFontPanel")]
		void UpdateFontPanel ();

		[Export ("updateDragTypeRegistration")]
		void UpdateDragTypeRegistration ();

		[Export ("selectionRangeForProposedRange:granularity:")]
		NSRange SelectionRange (NSRange proposedCharRange, NSSelectionGranularity granularity);

		[Export ("clickedOnLink:atIndex:")]
		void ClickedOnLink (NSObject link, uint charIndex);

		[Export ("startSpeaking:")]
		void StartSpeaking (NSObject sender);

		[Export ("stopSpeaking:")]
		void StopSpeaking (NSObject sender);

		[Export ("characterIndexForInsertionAtPoint:")]
		uint CharacterIndex (PointF point);

		//Detected properties
		[Export ("textContainer")]
		NSTextContainer TextContainer { get; set; }

		[Export ("textContainerInset")]
		SizeF TextContainerInset { get; set; }

		//
		// Completion support
		//
		[Export ("complete:")]
		void Complete (NSObject sender);

		[Export ("rangeForUserCompletion")]
		NSRange RangeForUserCompletion ();

		[Export ("completionsForPartialWordRange:indexOfSelectedItem:")]
		string [] CompletionsForPartialWord (NSRange charRange, int index);

		[Export ("insertCompletion:forPartialWordRange:movement:isFinal:")]
		void InsertCompletionforPartialWord (string word, NSRange charRange, int movement, bool flag);

		// Pasteboard
		[Export ("writablePasteboardTypes")]
		string [] WritablePasteboardTypes ();

		[Export ("writeSelectionToPasteboard:type:")]
		bool WriteSelectionToPasteboardtype (NSPasteboard pboard, string type);

		[Export ("writeSelectionToPasteboard:types:")]
		bool WriteSelectionToPasteboardtypes (NSPasteboard pboard, string [] types);

		[Export ("readablePasteboardTypes")]
		string [] ReadablePasteboardTypes ();

		[Export ("preferredPasteboardTypeFromArray:restrictedToTypesFromArray:")]
		string PreferredPasteboardTypeFromArrayrestrictedToTypesFromArray (string [] availableTypes, string [] allowedTypes);

		[Export ("readSelectionFromPasteboard:type:")]
		bool ReadSelectionFromPasteboardtype (NSPasteboard pboard, string type);

		[Export ("readSelectionFromPasteboard:")]
		bool ReadSelectionFromPasteboard (NSPasteboard pboard);

		[Export ("registerForServices")]
		void RegisterForServices ();

		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestorForSendTypereturnType (string sendType, string returnType);

		[Export ("pasteAsPlainText:")]
		void PasteAsPlainText (NSObject sender);

		[Export ("pasteAsRichText:")]
		void PasteAsRichText (NSObject sender);

		//
		// Dragging support
		//

		// FIXME: Binding
		//[Export ("dragImageForSelectionWithEvent:origin:")]
		//NSImage DragImageForSelection (NSEvent theEvent, NSPointPointer origin);

		[Export ("acceptableDragTypes")]
		string [] AcceptableDragTypes ();

		[Export ("dragOperationForDraggingInfo:type:")]
		NSDragOperation DragOperationForDraggingInfotype (NSDraggingInfo dragInfo, string type);

		[Export ("cleanUpAfterDragOperation")]
		void CleanUpAfterDragOperation ();

		[Export ("setSelectedRanges:affinity:stillSelecting:")]
		void SetSelectedRangesaffinitystillSelecting (NSRange [] ranges, NSSelectionAffinity affinity, bool stillSelectingFlag);

		[Export ("setSelectedRange:affinity:stillSelecting:")]
		void SetSelectedRangeaffinitystillSelecting (NSRange charRange, NSSelectionAffinity affinity, bool stillSelectingFlag);

		[Export ("selectionAffinity")]
		NSSelectionAffinity SelectionAffinity ();

		[Export ("updateInsertionPointStateAndRestartTimer:")]
		void UpdateInsertionPointStateAndRestartTimer (bool restartFlag);

		[Export ("toggleContinuousSpellChecking:")]
		void ToggleContinuousSpellChecking (NSObject sender);

		[Export ("spellCheckerDocumentTag")]
		int SpellCheckerDocumentTag ();

		[Export ("toggleGrammarChecking:")]
		void ToggleGrammarChecking (NSObject sender);

		[Export ("setSpellingState:range:")]
		void SetSpellingStaterange (int value, NSRange charRange);

		[Export ("shouldChangeTextInRanges:replacementStrings:")]
		bool ShouldChangeTextInRangesreplacementStrings (NSRange [] affectedRanges, string [] replacementStrings);

		[Export ("rangesForUserTextChange")]
		NSRange [] RangesForUserTextChange ();

		[Export ("rangesForUserCharacterAttributeChange")]
		NSRange [] RangesForUserCharacterAttributeChange ();

		[Export ("rangesForUserParagraphAttributeChange")]
		NSRange [] RangesForUserParagraphAttributeChange ();

		[Export ("shouldChangeTextInRange:replacementString:")]
		bool ShouldChangeTextInRangereplacementString (NSRange affectedCharRange, string replacementString);

		[Export ("rangeForUserTextChange")]
		NSRange RangeForUserTextChange ();

		[Export ("rangeForUserCharacterAttributeChange")]
		NSRange RangeForUserCharacterAttributeChange ();

		[Export ("rangeForUserParagraphAttributeChange")]
		NSRange RangeForUserParagraphAttributeChange ();

		[Export ("breakUndoCoalescing")]
		void BreakUndoCoalescing ();

		[Export ("isCoalescingUndo")]
		bool IsCoalescingUndo ();

		[Export ("showFindIndicatorForRange:")]
		void ShowFindIndicatorForRange (NSRange charRange);

		[Export ("setSelectedRange:")]
		void SetSelectedRange (NSRange charRange);

		//Detected properties
		[Export ("selectedRanges")]
		NSRange [] SelectedRanges { get; set; }

		[Export ("selectionGranularity")]
		NSSelectionGranularity SelectionGranularity { get; set; }

		[Export ("selectedTextAttributes")]
		NSDictionary SelectedTextAttributes { get; set; }

		[Export ("insertionPointColor")]
		NSColor InsertionPointColor { get; set; }

		[Export ("markedTextAttributes")]
		NSDictionary MarkedTextAttributes { get; set; }

		[Export ("linkTextAttributes")]
		NSDictionary LinkTextAttributes { get; set; }

		[Export ("displaysLinkToolTips")]
		bool DisplaysLinkToolTips { get; set; }

		[Export ("acceptsGlyphInfo")]
		bool AcceptsGlyphInfo { get; set; }

		[Export ("rulerVisible")]
		bool RulerVisible { [Bind ("isRulerVisible")]get; set; }

		[Export ("usesRuler")]
		bool UsesRuler { get; set; }

		[Export ("continuousSpellCheckingEnabled")]
		bool ContinuousSpellCheckingEnabled { [Bind ("isContinuousSpellCheckingEnabled")]get; set; }

		[Export ("grammarCheckingEnabled")]
		bool GrammarCheckingEnabled { [Bind ("isGrammarCheckingEnabled")]get; set; }

		[Export ("typingAttributes")]
		NSDictionary TypingAttributes { get; set; }

		[Export ("usesFindPanel")]
		bool UsesFindPanel { get; set; }

		[Export ("allowsDocumentBackgroundColorChange")]
		bool AllowsDocumentBackgroundColorChange { get; set; }

		[Export ("defaultParagraphStyle")]
		NSParagraphStyle DefaultParagraphStyle { get; set; }

		[Export ("allowsUndo")]
		bool AllowsUndo { get; set; }

		[Export ("allowsImageEditing")]
		bool AllowsImageEditing { get; set; }

		[Export ("delegate")]
		NSTextViewDelegate Delegate { get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("richText")]
		bool RichText { [Bind ("isRichText")]get; set; }

		[Export ("importsGraphics")]
		bool ImportsGraphics { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("fieldEditor")]
		bool FieldEditor { [Bind ("isFieldEditor")]get; set; }

		[Export ("usesFontPanel")]
		bool UsesFontPanel { get; set; }

		[Export ("allowedInputSourceLocales")]
		string [] AllowedInputSourceLocales { get; set; }

		[Export ("setSelectedRange:affinity:stillSelecting:")]
		void SetSelectedRange (NSRange charRange, NSSelectionAffinity affinity, bool stillSelectingFlag);

		// FIXME: binding
		//[Export ("shouldChangeTextInRanges:replacementStrings:")]
		//bool ShouldChangeTextInRanges (NSArray affectedRanges, NSArray replacementStrings);

		// FIXME: binding
		//[Export ("rangesForUserTextChange")]
		//NSArray RangesForUserTextChange ();

		// FIXME: binding
		//[Export ("rangesForUserCharacterAttributeChange")]
		//NSArray RangesForUserCharacterAttributeChange ();

		// FIXME: binding
		//[Export ("rangesForUserParagraphAttributeChange")]
		//NSArray RangesForUserParagraphAttributeChange ();

		[Export ("shouldChangeTextInRange:replacementString:")]
		bool ShouldChangeText (NSRange affectedCharRange, string replacementString);

		[Export ("didChangeText")]
		void DidChangeText ();

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		//
		// Smart copy/paset support
		//
		[Export ("smartDeleteRangeForProposedRange:")]
		NSRange SmartDeleteRangeForProposedRange (NSRange proposedCharRange);

		[Export ("toggleSmartInsertDelete:")]
		void ToggleSmartInsertDelete (NSObject sender);

		[Export ("smartInsertForString:replacingRange:beforeString:afterString:")]
		void SmartInsert (string pasteString, NSRange charRangeToReplace, string beforeString, string afterString);

		[Export ("smartInsertBeforeStringForString:replacingRange:")]
		string SmartInsertBefore (string pasteString, NSRange charRangeToReplace);

		[Export ("smartInsertAfterStringForString:replacingRange:")]
		string SmartInsertAfter (string pasteString, NSRange charRangeToReplace);

		[Export ("toggleAutomaticQuoteSubstitution:")]
		void ToggleAutomaticQuoteSubstitution (NSObject sender);

		[Export ("toggleAutomaticLinkDetection:")]
		void ToggleAutomaticLinkDetection (NSObject sender);

		[Export ("toggleAutomaticDataDetection:")]
		void ToggleAutomaticDataDetection (NSObject sender);

		[Export ("toggleAutomaticDashSubstitution:")]
		void ToggleAutomaticDashSubstitution (NSObject sender);

		[Export ("toggleAutomaticTextReplacement:")]
		void ToggleAutomaticTextReplacement (NSObject sender);

		[Export ("toggleAutomaticSpellingCorrection:")]
		void ToggleAutomaticSpellingCorrection (NSObject sender);

		[Export ("checkTextInRange:types:options:")]
		void CheckText (NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options);

		[Export ("handleTextCheckingResults:forRange:types:options:orthography:wordCount:")]
		void HandleTextChecking (NSTextCheckingResult [] results, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, NSOrthography orthography, int wordCount);

		[Export ("orderFrontSubstitutionsPanel:")]
		void OrderFrontSubstitutionsPanel (NSObject sender);

		[Export ("checkTextInSelection:")]
		void CheckTextInSelection (NSObject sender);

		[Export ("checkTextInDocument:")]
		void CheckTextInDocument (NSObject sender);

		//Detected properties
		[Export ("smartInsertDeleteEnabled")]
		bool SmartInsertDeleteEnabled { get; set; }

		[Export ("automaticQuoteSubstitutionEnabled")]
		bool AutomaticQuoteSubstitutionEnabled { [Bind ("isAutomaticQuoteSubstitutionEnabled")]get; set; }

		[Export ("automaticLinkDetectionEnabled")]
		bool AutomaticLinkDetectionEnabled { [Bind ("isAutomaticLinkDetectionEnabled")]get; set; }

		[Export ("automaticDataDetectionEnabled")]
		bool AutomaticDataDetectionEnabled { [Bind ("isAutomaticDataDetectionEnabled")]get; set; }

		[Export ("automaticDashSubstitutionEnabled")]
		bool AutomaticDashSubstitutionEnabled { [Bind ("isAutomaticDashSubstitutionEnabled")]get; set; }

		[Export ("automaticTextReplacementEnabled")]
		bool AutomaticTextReplacementEnabled { [Bind ("isAutomaticTextReplacementEnabled")]get; set; }

		[Export ("automaticSpellingCorrectionEnabled")]
		bool AutomaticSpellingCorrectionEnabled { [Bind ("isAutomaticSpellingCorrectionEnabled")]get; set; }

		[Export ("enabledTextCheckingTypes")]
		NSTextCheckingTypes EnabledTextCheckingTypes { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSTextViewDelegate {
		[Export ("textView:clickedOnLink:atIndex:")]
		bool LinkClicked (NSTextView textView, NSObject link, uint charIndex);

		[Export ("textView:clickedOnCell:inRect:atIndex:")]
		void CellClicked (NSTextView textView, NSTextAttachmentCell cell, RectangleF cellFrame, uint charIndex);

		[Export ("textView:doubleClickedOnCell:inRect:atIndex:")]
		void CellDoubleClicked (NSTextView textView, NSTextAttachmentCell cell, RectangleF cellFrame, uint charIndex);

		[Export ("textView:draggedCell:inRect:event:atIndex:")]
		void DraggedCell (NSTextView view, NSTextAttachmentCell cell, RectangleF rect, NSEvent theEvent, uint charIndex);

		[Export ("textView:writablePasteboardTypesForCell:atIndex:")]
		string [] WritablePasteboardTypes (NSTextView view, NSTextAttachmentCell forCell, uint charIndex);

		[Export ("textView:writeCell:atIndex:toPasteboard:type:")]
		bool WriteCell (NSTextView view, NSTextAttachmentCell cell, uint charIndex, NSPasteboard pboard, string type);

		[Export ("textView:willChangeSelectionFromCharacterRange:toCharacterRange:")]
		NSRange WillChangeSelection (NSTextView textView, NSRange oldSelectedCharRange, NSRange newSelectedCharRange);

		// FIXME: binding for NSArray, what is the type?
		//[Export ("textView:willChangeSelectionFromCharacterRanges:toCharacterRanges:")]
		//NSArray WillChangeSelection (NSTextView textView, NSArray oldSelectedCharRanges, NSArray newSelectedCharRanges);

		// FIXME: binding
		//[Export ("textView:shouldChangeTextInRanges:replacementStrings:")]
		//bool ShouldChangeText (NSTextView textView, NSArray affectedRanges, NSArray replacementStrings);

		[Export ("textView:shouldChangeTypingAttributes:toAttributes:")]
		NSDictionary ShouldChangeTypingAttributes (NSTextView textView, NSDictionary oldTypingAttributes, NSDictionary newTypingAttributes);

		[Export ("textViewDidChangeSelection:")]
		void DidChangeSelection (NSNotification notification);

		[Export ("textViewDidChangeTypingAttributes:")]
		void DidChangeTypingAttributes (NSNotification notification);

		[Export ("textView:willDisplayToolTip:forCharacterAtIndex:")]
		string WillDisplayToolTip (NSTextView textView, string tooltip, uint characterIndex);

		[Export ("textView:completions:forPartialWordRange:indexOfSelectedItem:")]
		string [] GetCompletions (NSTextView textView, string [] words, NSRange charRange, int index);

		[Export ("textView:shouldChangeTextInRange:replacementString:")]
		bool ShouldChangeText (NSTextView textView, NSRange affectedCharRange, string replacementString);

		[Export ("textView:doCommandBySelector:")]
		bool DoCommandBySelector (NSTextView textView, Selector commandSelector);

		[Export ("textView:shouldSetSpellingState:range:")]
		int ShouldSetSpellingState (NSTextView textView, int value, NSRange affectedCharRange);

		[Export ("textView:menu:forEvent:atIndex:")]
		NSMenu MenuForEvent (NSTextView view, NSMenu menu, NSEvent theEvent, uint charIndex);

		[Export ("textView:willCheckTextInRange:options:types:")]
		NSDictionary WillCheckText (NSTextView view, NSRange range, NSDictionary options, NSTextCheckingTypes checkingTypes);

		[Export ("textView:didCheckTextInRange:types:options:results:orthography:wordCount:")]
		NSTextCheckingResult [] DidCheckText (NSTextView view, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, NSTextCheckingResult [] results, NSOrthography orthography, int wordCount);

		[Export ("textView:clickedOnLink:")]
		bool LinkClicked (NSTextView textView, NSObject link);

		[Export ("textView:clickedOnCell:inRect:")]
		void CellClicked (NSTextView textView, NSTextAttachmentCell cell, RectangleF cellFrame);

		[Export ("textView:doubleClickedOnCell:inRect:")]
		void CellDoubleClicked (NSTextView textView, NSTextAttachmentCell cell, RectangleF cellFrame);

		[Export ("textView:draggedCell:inRect:event:")]
		void DraggedCell (NSTextView view, NSTextAttachmentCell cell, RectangleF rect, NSEvent theevent);

		[Export ("undoManagerForTextView:")]
		NSUndoManager GetUndoManager (NSTextView view);
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSToolbarDelegate)})]
	interface NSToolbar {
		[Export ("initWithIdentifier:")]
		IntPtr Constructor (string identifier);

		[Export ("insertItemWithItemIdentifier:atIndex:")]
		void InsertItem (string itemIdentifier, int index);

		[Export ("removeItemAtIndex:")]
		void RemoveItem (int index);

		[Export ("runCustomizationPalette:")]
		void RunCustomizationPalette (NSObject sender);

		[Export ("customizationPaletteIsRunning")]
		bool IsCustomizationPaletteRunning { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("items")]
		NSToolbarItem [] Items { get; }

		[Export ("visibleItems")]
		NSToolbarItem [] VisibleItems { get; }

		[Export ("setConfigurationFromDictionary:")]
		void SetConfigurationFromDictionary (NSDictionary configDict);

		[Export ("configurationDictionary")]
		NSDictionary ConfigurationDictionary { get; }

		[Export ("validateVisibleItems")]
		void ValidateVisibleItems ();

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSToolbarDelegate Delegate { get; set; }

		[Export ("visible")]
		bool Visible { [Bind ("isVisible")]get; set; }

		[Export ("displayMode")]
		NSToolbarDisplayMode DisplayMode { get; set; }

		[Export ("selectedItemIdentifier")]
		string SelectedItemIdentifier { get; set; }

		[Export ("sizeMode")]
		NSToolbarSizeMode SizeMode { get; set; }

		[Export ("showsBaselineSeparator")]
		bool ShowsBaselineSeparator { get; set; }

		[Export ("allowsUserCustomization")]
		bool AllowsUserCustomization { get; set; }

		[Export ("autosavesConfiguration")]
		bool AutosavesConfiguration { get; set; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSToolbarDelegate {
		[Abstract]
		[Export ("toolbar:itemForItemIdentifier:willBeInsertedIntoToolbar:"), EventArgs ("NSToolbarWillInsert"), DefaultValue (null)]
		NSToolbarItem WillInsertItem (NSToolbar toolbar, string itemIdentifier, bool willBeInserted);

		[Abstract]
		[Export ("toolbarDefaultItemIdentifiers:"), EventArgs ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] DefaultItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarAllowedItemIdentifiers:"), EventArgs ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] AllowedItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarSelectableItemIdentifiers:"), EventArgs ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] SelectableItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarWillAddItem:"), EventArgs ("NSNotification")]
		void WillAddItem (NSNotification notification);

		[Abstract]
		[Export ("toolbarDidRemoveItem:"), EventArgs ("NSNotification")]
		void DidRemoveItem (NSNotification notification);
	}


	[BaseType (typeof (NSObject))]
	interface NSToolbarItem {
		[Export ("initWithItemIdentifier:")]
		IntPtr Constructor (string itemIdentifier);

		[Export ("itemIdentifier")]
		string Identifier { get; }

		[Export ("toolbar")]
		NSToolbar Toolbar { get; }

		[Export ("validate")]
		void Validate ();

		[Export ("allowsDuplicatesInToolbar")]
		bool AllowsDuplicatesInToolbar { get; }

		//Detected properties
		[Export ("label")]
		string Label { get; set; }

		[Export ("paletteLabel")]
		string PaletteLabel { get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }

		[Export ("menuFormRepresentation")]
		NSMenuItem MenuFormRepresentation { get; set; }

		[Export ("tag")]
		int Tag { get; set; }

		[Export ("target")]
		NSObject Target { get; set; }

		[Export ("action")]
		Selector Action { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("view")]
		NSView View { get; set; }

		[Export ("minSize")]
		SizeF MinSize { get; set; }

		[Export ("maxSize")]
		SizeF MaxSize { get; set; }

		[Export ("visibilityPriority")]
		int VisibilityPriority { get; set; }

		[Export ("autovalidates")]
		bool Autovalidates { get; set; }
	}

	[BaseType (typeof (NSObject))]
	interface NSTouch {
		[Export ("identity", ArgumentSemantic.Retain)]
		NSObject Identity { get; }

		[Export ("phase")]
		NSTouchPhase Phase { get; }

		[Export ("normalizedPosition")]
		PointF NormalizedPosition { get; }

		[Export ("isResting")]
		bool IsResting { get; }

		[Export ("device", ArgumentSemantic.Retain)]
		NSObject Device { get; }

		[Export ("deviceSize")]
		SizeF DeviceSize { get; }
	}

	[BaseType (typeof (NSObject))]
	interface NSTrackingArea {
		[Export ("initWithRect:options:owner:userInfo:")]
		IntPtr Constructor (RectangleF rect, NSTrackingAreaOptions options, NSObject owner, NSDictionary userInfo);
		
		[Export ("rect")]
		RectangleF Rect { get; }

		[Export ("options")]
		NSTrackingAreaOptions Options { get; }

		[Export ("owner")]
		NSObject Owner { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }
	}
	
	[BaseType (typeof (NSResponder), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSWindowDelegate)})]
	interface NSWindow {
		[Static, Export ("frameRectForContentRect:styleMask:")]
		RectangleF FrameRectFor (RectangleF contectRect, NSWindowStyle styleMask);
	
		[Static]
		[Export ("contentRectForFrameRect:styleMask:")]
		RectangleF ContentRectFor (RectangleF forFrameRect, NSWindowStyle styleMask);
	
		[Static]
		[Export ("minFrameWidthWithTitle:styleMask:")]
		float MinFrameWidthWithTitle (string aTitle, NSWindowStyle aStyle);
	
		[Static]
		[Export ("defaultDepthLimit")]
		NSWindowDepth DefaultDepthLimit { get; }
	
		[Export ("frameRectForContentRect:")]
		RectangleF FrameRectFor (RectangleF contentRect);
	
		[Export ("contentRectForFrameRect:")]
		RectangleF ContentRectFor (RectangleF frameRect);
	
		[Export ("initWithContentRect:styleMask:backing:defer:")]
		IntPtr Constructor (RectangleF contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool flag);
	
		[Export ("initWithContentRect:styleMask:backing:defer:screen:")]
		IntPtr Constructor (RectangleF contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool flag, NSScreen  screen);
	
		[Export ("title")]
		string Title  { get; set; }
	
		[Export ("representedURL")]
		NSUrl RepresentedURL { get; set; }
	
		[Export ("representedFilename")]
		string RepresentedFilename  { get; set; }
	
		[Export ("setTitleWithRepresentedFilename:")]
		void SetTitleWithRepresentedFilename (string  filename);
	
		[Export ("setExcludedFromWindowsMenu:")]
		void SetExcludedFromWindowsMenu (bool flag);
	
		[Export ("isExcludedFromWindowsMenu")]
		bool ExcludedFromWindowsMenu { get; } 
	
		[Export ("contentView")]
		NSView ContentView  { get; set; }
	
		[Export ("delegate")]
		NSWindowDelegate Delegate { get; set; }
	
		[Export ("windowNumber")]
		int WindowNumber { get; }
	
		[Export ("styleMask")]
		NSWindowStyle StyleMask { get; set; }
	
		[Export ("fieldEditor:forObject:")]
		NSText FieldEditor (bool createFlag, NSObject forObject);
	
		[Export ("endEditingFor:")]
		void EndEditingFor (NSObject anObject);
	
		[Export ("constrainFrameRect:toScreen:")]
		RectangleF ConstrainFrameRect (RectangleF frameRect, NSScreen screen);
	
		[Export ("setFrame:display:")]
		void SetFrame (RectangleF frameRect, bool display);
	
		[Export ("setContentSize:")]
		void SetContentSize (SizeF aSize);
	
		[Export ("setFrameOrigin:")]
		void SetFrameOrigin (PointF aPoint);
	
		[Export ("setFrameTopLeftPoint:")]
		void SetFrameTopLeftPoint (PointF aPoint);
	
		[Export ("cascadeTopLeftFromPoint:")]
		PointF CascadeTopLeftFromPoint (PointF topLeftPoint);
	
		[Export ("frame")]
		RectangleF Frame { get; }
	
		[Export ("animationResizeTime:")]
		double AnimationResizeTime (RectangleF newFrame);
	
		[Export ("setFrame:display:animate:")]
		void SetFrame (RectangleF frameRect, bool display, bool animate);
	
		[Export ("inLiveResize")]
		bool InLiveResize { get; } 
	
		[Export ("showsResizeIndicator")]
		bool ShowsResizeIndicator { get; set; }
	
		[Export ("resizeIncrements")]
		SizeF ResizeIncrements  { get; set; }
	
		[Export ("aspectRatio")]
		SizeF AspectRatio  { get; set; }
	
		[Export ("contentResizeIncrements")]
		SizeF ContentResizeIncrements  { get; set; }
	
		[Export ("contentAspectRatio")]
		SizeF ContentAspectRatio  { get; set; }
	
		[Export ("useOptimizedDrawing:")]
		void UseOptimizedDrawing (bool flag);
	
		[Export ("disableFlushWindow")]
		void DisableFlushWindow ();
	
		[Export ("enableFlushWindow")]
		void EnableFlushWindow ();
	
		[Export ("isFlushWindowDisabled")]
		bool FlushWindowDisabled { get; }
	
		[Export ("flushWindow")]
		void FlushWindow ();
	
		[Export ("flushWindowIfNeeded")]
		void FlushWindowIfNeeded ();
	
		[Export ("viewsNeedDisplay")]
		bool ViewsNeedDisplay  { get; set; }
	
		[Export ("displayIfNeeded")]
		void DisplayIfNeeded ();
	
		[Export ("display")]
		void Display ();
	
		[Export ("autodisplay")]
		bool Autodisplay  { [Bind ("isAutodisplay")] get; set; }
	
		[Export ("preservesContentDuringLiveResize")]
		bool PreservesContentDuringLiveResize  { get; set; }
	
		[Export ("update")]
		void Update ();
	
		[Export ("makeFirstResponder:")]
		bool MakeFirstResponder (NSResponder  aResponder);
	
		[Export ("firstResponder")]
		NSResponder FirstResponder { get; }
	
		[Export ("resizeFlags")]
		int ResizeFlags { get; }
	
		[Export ("keyDown:")]
		void KeyDown (NSEvent  theEvent);
	
		[Export ("close")]
		void Close ();
	
		[Export ("releasedWhenClosed")]
		bool ReleasedWhenClosed  { [Bind ("isReleasedWhenClosed")] get; set; }
	
		[Export ("miniaturize:")]
		void Miniaturize (NSObject sender);
	
		[Export ("deminiaturize:")]
		void Deminiaturize (NSObject sender);
	
		[Export ("isZoomed")]
		bool IsZoomed { get; }
	
		[Export ("zoom:")]
		void Zoom (NSObject sender);
	
		[Export ("isMiniaturized")]
		bool IsMiniaturized { get; }
	
		[Export ("tryToPerform:with:")]
		bool TryToPerform (Selector anAction, NSObject anObject);
		
		[Export ("validRequestorForSendType:returnType:")]
		NSObject ValidRequestorForSendType (string sendType, string returnType);
	
		[Export ("backgroundColor")]
		NSColor BackgroundColor  { get; set; }
	
		[Export ("setContentBorderThickness:forEdge:")]
		void SetContentBorderThickness (float thickness, NSRectEdge edge);
	
		[Export ("contentBorderThicknessForEdge:")]
		float ContentBorderThicknessForEdge (NSRectEdge edge);
	
		[Export ("setAutorecalculatesContentBorderThickness:forEdge:")]
		void SetAutorecalculatesContentBorderThickness (bool flag, NSRectEdge forEdge);
	
		[Export ("autorecalculatesContentBorderThicknessForEdge:")]
		bool AutorecalculatesContentBorderThickness (NSRectEdge forEdgeedge);
	
		[Export ("movable")]
		bool IsMovable  { [Bind ("isMovable")] get; set; }
	
		[Export ("movableByWindowBackground")]
		bool MovableByWindowBackground  { [Bind ("isMovableByWindowBackground")] get; set; }
	
		[Export ("hidesOnDeactivate")]
		bool hidesOnDeactivate  { get; set; }
	
		[Export ("canHide")]
		bool CanHide  { get; set; }
	
		[Export ("center")]
		void Center ();
	
		[Export ("makeKeyAndOrderFront:")]
		void MakeKeyAndOrderFront (NSObject sender);
	
		[Export ("orderFront:")]
		void OrderFront (NSObject sender);
		
		[Export ("orderBack:")]
		void orderBack (NSObject sender);
	
		[Export ("orderOut:")]
		void OrderOut (NSObject sender);
	
		[Export ("orderWindow:relativeTo:")]
		void OrderWindow (NSWindowOrderingMode place, int relativeTo);
	
		[Export ("orderFrontRegardless")]
		void OrderFrontRegardless ();
	
		[Export ("miniwindowImage")]
		NSImage MiniwindowImage { get; set; }
	
		[Export ("miniwindowTitle")]
		string miniwindowTitle  { get; set; }
	
		[Export ("dockTile")]
		NSDockTile DockTile { get; } 
	
		[Export ("documentEdited")]
		bool DocumentEdited  { [Bind ("isDocumentEdited")] get; set; }
	
		[Export ("isVisible")]
		bool IsVisible  { get; set; }
	
		[Export ("isKeyWindow")]
		bool IsKeyWindow  { get; set; }
	
		[Export ("isMainWindow")]
		bool IsMainWindow  { get; set; }
		
		[Export ("canBecomeKeyWindow")]
		bool CanBecomeKeyWindow { get; }
		
		[Export ("canBecomeMainWindow")]
		bool CanBecomeMainWindow { get; }
	
		[Export ("makeKeyWindow")]
		void MakeKeyWindow ();
	
		[Export ("makeMainWindow")]
		void MakeMainWindow ();
	
		[Export ("becomeKeyWindow")]
		void BecomeKeyWindow ();
		
		[Export ("resignKeyWindow")]
		void ResignKeyWindow ();
		
		[Export ("becomeMainWindow")]
		void BecomeMainWindow ();
	
		[Export ("resignMainWindow")]
		void ResignMainWindow ();
		
		[Export ("worksWhenModal")]
		bool WorksWhenModal ();
		
		[Export ("preventsApplicationTerminationWhenModal")]
		bool PreventsApplicationTerminationWhenModal  { get; set; }
	
		[Export ("convertBaseToScreen:")]
		PointF ConvertBaseToScreen (PointF aPoint);
	
		[Export ("convertScreenToBase:")]
		PointF ConvertScreenToBase (PointF aPoint);
	
		[Export ("performClose:")]
		void PerformClose (NSObject sender);
		
		[Export ("performMiniaturize:")]
		void PerformMiniaturize (NSObject sender);
	
		[Export ("performZoom:")]
		void PerformZoom (NSObject sender);
	
		[Export ("gState")]
		int GState ();
	
		[Export ("setOneShot:")]
		void SetOneShot (bool flag);
	
		[Export ("isOneShot")]
		bool IsOneShot { get; }
	
		[Export ("dataWithEPSInsideRect:")]
		NSData DataWithEPSInsideRect (RectangleF rect);
	
		[Export ("dataWithPDFInsideRect:")]
		NSData DataWithPDFInsideRect (RectangleF rect);
	
		[Export ("print:")]
		void Print (NSObject sender);
	
		[Export ("disableCursorRects")]
		void DisableCursorRects ();
	
		[Export ("enableCursorRects")]
		void EnableCursorRects ();
	
		[Export ("discardCursorRects")]
		void DiscardCursorRects ();
	
		[Export ("areCursorRectsEnabled")]
		bool AreCursorRectsEnabled { get; }
	
		[Export ("invalidateCursorRectsForView:")]
		void InvalidateCursorRectsForView (NSView  aView);
	
		[Export ("resetCursorRects")]
		void ResetCursorRects ();
	
		[Export ("allowsToolTipsWhenApplicationIsInactive")]
		bool AllowsToolTipsWhenApplicationIsInactive  { get; set; }
	
		[Export ("backingType")]
		NSBackingStore BackingType  { get; set; }
	
		[Export ("level")]
		int Level  { get; set; }
	
		[Export ("depthLimit")]
		NSWindowDepth DepthLimit  { get; set; }
	
		[Export ("dynamicDepthLimit")]
		bool hasDynamicDepthLimit { [Bind ("hasDynamicDepthLimit")] get; set; }
	
		[Export ("screen")]
		NSScreen Screen { get; }
	
		[Export ("deepestScreen")]
		NSScreen DeepestScreen { get; }
	
		[Export ("canStoreColor")]
		bool CanStoreColor { get; }
	
		[Export ("hasShadow")]
		bool HasShadow  { get; set; }
	
		[Export ("invalidateShadow")]
		void InvalidateShadow ();
	
		[Export ("alphaValue")]
		float AlphaValue  { get; set; }
	
		[Export ("opaque")]
		bool IsOpaque  { [Bind ("isOpaque")]get; set; }
	
		[Export ("sharingType")]
		NSWindowSharingType SharingType  { get; set; }
	
		[Export ("preferredBackingLocation")]
		NSWindowBackingLocation PreferredBackingLocation  { get; set; }
	
		[Export ("backingLocation")]
		NSWindowBackingLocation BackingLocation { get; }
	
		[Export ("allowsConcurrentViewDrawing")]
		bool AllowsConcurrentViewDrawing  { get; set; }
	
		[Export ("displaysWhenScreenProfileChanges")]
		bool DisplaysWhenScreenProfileChanges  { get; set; }
	
		[Export ("disableScreenUpdatesUntilFlush")]
		void DisableScreenUpdatesUntilFlush ();
	
		[Export ("canBecomeVisibleWithoutLogin")]
		bool CanBecomeVisibleWithoutLogin { get; set; }
	
		[Export ("collectionBehavior")]
		NSWindowCollectionBehavior CollectionBehavior  { get; set; }
	
		[Export ("isOnActiveSpace")]
		bool IsOnActiveSpace { get; }
	
		[Export ("stringWithSavedFrame")]
		string StringWithSavedFrame ();
	
		[Export ("setFrameFromString:")]
		void SetFrameFroom (string str);
	
		[Export ("saveFrameUsingName:")]
		void SaveFrameUsingName (string  name);
	
		[Export ("setFrameUsingName:force:")]
		bool SetFrameUsingName (string  name, bool force);
	
		[Export ("setFrameUsingName:")]
		bool SetFrameUsingName (string  name);
	
		[Export ("frameAutosaveName")]
		string FrameAutosaveName  { get; set; }
	
		[Static]
		[Export ("removeFrameUsingName:")]
		void RemoveFrameUsingName (string  name);
	
		[Export ("cacheImageInRect:")]
		void CacheImageInRect (RectangleF aRect);
	
		[Export ("restoreCachedImage")]
		void RestoreCachedImage ();
	
		[Export ("discardCachedImage")]
		void DiscardCachedImage ();
	
		[Export ("minSize")]
		SizeF MinSize  { get; set; }
	
		[Export ("maxSize")]
		SizeF MaxSize  { get; set; }
	
		[Export ("contentMinSize")]
		SizeF ContentMinSize  { get; set; }
	
		[Export ("contentMaxSize")]
		SizeF ContentMaxSize  { get; set; }
	
		[Export ("nextEventMatchingMask:")]
		NSEvent NextEventMatchingMask (NSEventMask mask);
	
		[Export ("nextEventMatchingMask:untilDate:inMode:dequeue:")]
		NSEvent NextEventMatchingMask (NSEventMask mask, NSDate  expiration, string  mode, bool deqFlag);
	
		[Export ("discardEventsMatchingMask:beforeEvent:")]
		void DiscardEventsMatchingMask (NSEventMask mask, NSEvent beforeLastEvent);
	
		[Export ("postEvent:atStart:")]
		void PostEvent (NSEvent theEvent, bool flag);
	
		[Export ("currentEvent")]
		NSEvent CurrentEvent ();
	
		[Export ("acceptsMouseMovedEvents")]
		bool AcceptsMouseMovedEvents  { get; set; }
	
		[Export ("ignoresMouseEvents")]
		bool IgnoresMouseEvents  { get; set; }
	
		[Export ("deviceDescription")]
		NSDictionary DeviceDescription { get; }
	
		[Export ("sendEvent:")]
		void SendEvent (NSEvent  theEvent);
	
		[Export ("mouseLocationOutsideOfEventStream")]
		PointF MouseLocationOutsideOfEventStream { get; }
	
		[Static]
		[Export ("menuChanged:")]
		void MenuChanged (NSMenu  menu);
	
		[Export ("windowController")]
		NSObject WindowController { get; set; }
	
		[Export ("isSheet")]
		bool IsSheet { get; }
	
		[Export ("attachedSheet")]
		NSWindow AttachedSheet { get; }
	
		[Export ("standardWindowButton:forStyleMask:")]
		NSButton StandardWindowButton (NSWindowButton b, NSWindowStyle styleMask);
	
		[Export ("standardWindowButton:")]
		NSButton StandardWindowButton (NSWindowButton b);
	
		[Export ("addChildWindow:ordered:")]
		void AddChildWindow (NSWindow  childWin, NSWindowOrderingMode place);
	
		[Export ("removeChildWindow:")]
		void RemoveChildWindow (NSWindow  childWin);
	
		[Export ("childWindows")]
		NSWindow [] ChildWindows { get; }
	
		[Export ("parentWindow")]
		NSWindow ParentWindow { get; set; }
	
		[Export ("graphicsContext")]
		NSGraphicsContext GraphicsContext { get; }
	
		[Export ("userSpaceScaleFactor")]
		float UserSpaceScaleFactor { get; }
	
		[Export ("colorSpace")]
		NSColorSpace ColorSpace  { get; set; }
	
		[Static]
		[Export ("windowNumbersWithOptions:")]
		NSArray WindowNumbersWithOptions (NSWindowNumberListOptions options);
	
		[Static]
		[Export ("windowNumberAtPoint:belowWindowWithWindowNumber:")]
		int WindowNumberAtPoint (PointF point, int windowNumber);
	
		[Export ("initialFirstResponder")]
		NSView InitialFirstResponder { get; }
	
		[Export ("selectNextKeyView:")]
		void SelectNextKeyView (NSObject sender);
	
		[Export ("selectPreviousKeyView:")]
		void SelectPreviousKeyView (NSObject sender);
	
		[Export ("selectKeyViewFollowingView:")]
		void SelectKeyViewFollowingView (NSView aView);
	
		[Export ("selectKeyViewPrecedingView:")]
		void SelectKeyViewPrecedingView (NSView aView);
	
		[Export ("keyViewSelectionDirection")]
		NSSelectionDirection KeyViewSelectionDirection ();
	
		[Export ("defaultButtonCell")]
		NSButtonCell DefaultButtonCell { get; set; }
	
		[Export ("disableKeyEquivalentForDefaultButtonCell")]
		void DisableKeyEquivalentForDefaultButtonCell ();
	
		[Export ("enableKeyEquivalentForDefaultButtonCell")]
		void EnableKeyEquivalentForDefaultButtonCell ();
	
		[Export ("autorecalculatesKeyViewLoop")]
		bool AutorecalculatesKeyViewLoop  { get; set; }
	
		[Export ("recalculateKeyViewLoop")]
		void RecalculateKeyViewLoop ();
	
		[Export ("toolbar")]
		NSToolbar Toolbar ();
	
		[Export ("toggleToolbarShown:")]
		void ToggleToolbarShown (NSObject sender);
	
		[Export ("runToolbarCustomizationPalette:")]
		void RunToolbarCustomizationPalette (NSObject sender);
	
		[Export ("showsToolbarButton")]
		bool ShowsToolbarButton { get; set; }

		[Export ("registerForDraggedTypes:")]
		void RegisterForDraggedTypes (string [] newTypes);
	
		[Export ("unregisterDraggedTypes")]
		void UnregisterDraggedTypes ();
	
		[Export ("windowRef")]
		IntPtr WindowRef { get; }
	
	}
	
	[BaseType (typeof (NSResponder))]
	interface NSWindowController {
		[Export ("initWithWindow:")]
		IntPtr Constructor (NSWindow  window);
	
		[Export ("initWithWindowNibName:")]
		IntPtr Constructor (string  windowNibName);
	
		[Export ("initWithWindowNibName:owner:")]
		IntPtr Constructor (string  windowNibName, NSObject owner);
	
		[Export ("windowNibName")]
		string WindowNibName { get; }
	
		[Export ("windowNibPath")]
		string WindowNibPath { get; }
	
		[Export ("owner")]
		NSObject Owner { get; }
	
		[Export ("windowFrameAutosaveName")]
		string windowFrameAutosaveName { get; set; }
	
		[Export ("shouldCascadeWindows")]
		bool ShouldCascadeWindows  { get; set; }
	
		[Export ("document")]
		NSDocument Document { get; set; }
	
		[Export ("setDocumentEdited:")]
		void SetDocumentEdited (bool dirtyFlag);
	
		[Export ("shouldCloseDocument")]
		bool ShouldCloseDocument  { get; set; }
	
		[Export ("window")]
		NSWindow Window { get; set; }
	
		[Export ("synchronizeWindowTitleWithDocumentName")]
		void SynchronizeWindowTitleWithDocumentName ();
	
		[Export ("windowTitleForDocumentDisplayName:")]
		string WindowTitleForDocumentDisplayName (string  displayName);
	
		[Export ("close")]
		void Close ();
	
		[Export ("showWindow:")]
		void ShowWindow (NSObject sender);
	
		[Export ("isWindowLoaded")]
		bool IsWindowLoaded  { get; set; }
	
		[Export ("windowWillLoad")]
		void WindowWillLoad ();
	
		[Export ("windowDidLoad")]
		void WindowDidLoad ();
	
		[Export ("loadWindow")]
		void LoadWindow ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface NSWindowDelegate {
		[Export ("windowShouldClose:"), EventArgs ("NSObjectPredicate"), DefaultValue (true)]
		bool WindowShouldClose (NSObject sender);
	
		[Export ("windowWillReturnFieldEditor:toObject:"), EventArgs ("NSWindowClient"), DefaultValue (null)]
		NSObject WillReturnFieldEditor (NSWindow  sender, NSObject client);
	
		[Export ("windowWillResize:toSize:"), EventArgs ("NSWindowResize"), DefaultValueFromArgument ("toFrameSize")]
		SizeF WillResize (NSWindow sender, SizeF toFrameSize);
	
		[Export ("windowWillUseStandardFrame:defaultFrame:"), EventArgs ("NSWindowFrame"), DefaultValueFromArgument ("newFrame")]
		RectangleF WillUseStandardFrame (NSWindow window, RectangleF newFrame);
	
		[Export ("windowShouldZoom:toFrame:"), EventArgs ("NSWindowFramePredicate"), DefaultValue (true)]
		bool ShouldZoom (NSWindow  window, RectangleF newFrame);
	
		[Export ("windowWillReturnUndoManager:"), EventArgs ("NSWindowUndoManager"), DefaultValue (null)]
		NSUndoManager WillReturnUndoManager (NSWindow  window);
	
		[Export ("window:willPositionSheet:usingRect:"), EventArgs ("NSWindowSheetRect"), DefaultValueFromArgument ("usingRect")]
		RectangleF WillPositionSheet (NSWindow  window, NSWindow  sheet, RectangleF usingRect);
	
		[Export ("window:shouldPopUpDocumentPathMenu:"), EventArgs ("NSWindowMenu"), DefaultValue (true)]
		bool ShouldPopUpDocumentPathMenu (NSWindow  window, NSMenu  menu);
	
		[Export ("window:shouldDragDocumentWithEvent:from:withPasteboard:"), EventArgs ("NSWindowDocumentDrag"), DefaultValue (true)]
		bool ShouldDragDocumentWithEvent (NSWindow  window, NSEvent theEvent, PointF dragImageLocation, NSPasteboard  withPasteboard);
	
		[Export ("windowDidResize:"), EventArgs ("NSNotification")]
		void DidResize (NSNotification  notification);
	
		[Export ("windowDidExpose:"), EventArgs ("NSNotification")]
		void DidExpose (NSNotification  notification);
	
		[Export ("windowWillMove:"), EventArgs ("NSNotification")]
		void WillMove (NSNotification  notification);
	
		[Export ("windowDidMove:"), EventArgs ("NSNotification")]
		void DidMoved (NSNotification  notification);
	
		[Export ("windowDidBecomeKey:"), EventArgs ("NSNotification")]
		void DidBecomeKey (NSNotification  notification);
	
		[Export ("windowDidResignKey:"), EventArgs ("NSNotification")]
		void DidResignKey (NSNotification  notification);
	
		[Export ("windowDidBecomeMain:"), EventArgs ("NSNotification")]
		void DidBecomeMain (NSNotification  notification);
	
		[Export ("windowDidResignMain:"), EventArgs ("NSNotification")]
		void DidResignMain (NSNotification  notification);
	
		[Export ("windowWillClose:"), EventArgs ("NSNotification")]
		void WillClose (NSNotification  notification);
	
		[Export ("windowWillMiniaturize:"), EventArgs ("NSNotification")]
		void WillMiniaturize (NSNotification  notification);
	
		[Export ("windowDidMiniaturize:"), EventArgs ("NSNotification")]
		void DidMiniaturize (NSNotification  notification);
	
		[Export ("windowDidDeminiaturize:"), EventArgs ("NSNotification")]
		void DidDeminiaturize (NSNotification  notification);
	
		[Export ("windowDidUpdate:"), EventArgs ("NSNotification")]
		void DidUpdate (NSNotification  notification);
	
		[Export ("windowDidChangeScreen:"), EventArgs ("NSNotification")]
		void DidChangeScreen (NSNotification  notification);
	
		[Export ("windowDidChangeScreenProfile:"), EventArgs ("NSNotification")]
		void DidChangeScreenProfile (NSNotification  notification);
	
		[Export ("windowWillBeginSheet:"), EventArgs ("NSNotification")]
		void WillBeginSheet (NSNotification  notification);
	
		[Export ("windowDidEndSheet:"), EventArgs ("NSNotification")]
		void DidEndSheet (NSNotification  notification);
	
		[Export ("windowWillStartLiveResize:"), EventArgs ("NSNotification")]
		void WillStartLiveResize (NSNotification  notification);
	
		[Export ("windowDidEndLiveResize:"), EventArgs ("NSNotification")]
		void DidEndLiveResize (NSNotification  notification);
	}
}