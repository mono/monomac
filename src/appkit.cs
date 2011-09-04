///
// Copyright 2010, Novell, Inc.
// Copyright 2010, Kenneth Pouncey
// Coprightt 2010, James Clancey
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
using MonoMac.CoreData;
using MonoMac.OpenGL;

namespace MonoMac.AppKit {
		
	//[BaseType (typeof (NSObject))]
	//interface CIImage {
	//	[Export ("drawInRect:fromRect:operation:fraction:")]
	//	void Draw (RectangleF inRect, RectangleF fromRect, NSCompositingOperation operation, float fractionDelta);
	//
	//	[Export ("drawAtPoint:fromRect:operation:fraction:")]
	//	void DrawAtPoint (PointF atPoint, RectangleF fromRect, NSCompositingOperation operation, float fractionDelta);
	//}
	
	[BaseType (typeof (NSCell))]
	public interface NSActionCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);
	
		[Export ("target"), NullAllowed]
		NSObject Target  { get; set; }
	
		[Export ("action"), NullAllowed]
		Selector Action  { get; set; }
	
		[Export ("tag")]
		int Tag  { get; set; }
	
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSAnimationDelegate)})]
	public interface NSAnimation {
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

		[Field ("NSAnimationProgressMarkNotification")]
		NSString ProgressMarkNotification { get; }

		[Field ("NSAnimationProgressMark")]
		NSString ProgressMark { get; }

		[Field ("NSAnimationTriggerOrderIn")]
		NSString TriggerOrderIn { get; }

		[Field ("NSAnimationTriggerOrderOut")]
		NSString TriggerOrderOut { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSAnimationDelegate {
		[Export ("animationShouldStart:"), DelegateName ("NSAnimationPredicate"), DefaultValue (true)]
		bool AnimationShouldStart (NSAnimation animation);
	
		[Export ("animationDidStop:"), EventArgs ("NSAnimation")]
		void AnimationDidStop (NSAnimation animation);
	
		[Export ("animationDidEnd:"), EventArgs ("NSAnimation")]
		void AnimationDidEnd (NSAnimation animation);
	
		[Export ("animation:valueForProgress:"), DelegateName ("NSAnimationProgress"), DefaultValueFromArgumentAttribute ("progress")]
		float ComputeAnimationCurve (NSAnimation animation, float progress);
	
		[Export ("animation:didReachProgressMark:"), EventArgs ("NSAnimation")]
		void AnimationDidReachProgressMark (NSAnimation animation, float progress);
	}

	[BaseType (typeof (NSObject))]
	public interface NSAnimationContext {
		[Static]
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
	public interface NSAlert {
		[Static, Export ("alertWithError:")]
		NSAlert WithError (NSError  error);
	
		[Static, Export ("alertWithMessageText:defaultButton:alternateButton:otherButton:informativeTextWithFormat:")]
		NSAlert WithMessage([NullAllowed] string message, [NullAllowed] string defaultButton, [NullAllowed] string alternateButton, [NullAllowed]  string otherButton, string full);
	
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
		void BeginSheet (NSWindow  window, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);
	
		[Export ("window")]
		NSObject Window  { get; }
	
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSAlertDelegate {
		[Export ("alertShowHelp:"), DelegateName ("NSAlertPredicate"), DefaultValue (false)]
		bool ShowHelp (NSAlert  alert);
	}

	[BaseType (typeof (NSResponder), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSApplicationDelegate) })]
	public interface NSApplication {
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
		void BeginSheet (NSWindow sheet, NSWindow docWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);
	
		[Export ("endSheet:")]
		void EndSheet (NSWindow sheet);
	
		[Export ("endSheet:returnCode:")]
		void EndSheet (NSWindow  sheet, int returnCode);
	
		[Export ("nextEventMatchingMask:untilDate:inMode:dequeue:")]
		NSEvent NextEvent (NSEventMask mask, NSDate expiration, string mode, bool deqFlag);
	
		[Export ("discardEventsMatchingMask:beforeEvent:")]
		void DiscardEvents (NSEventMask mask, NSEvent lastEvent);
	
		[Export ("postEvent:atStart:")]
		void PostEvent (NSEvent theEvent, bool atStart);
	
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
		NSImage ApplicationIconImage { get; set; }
	
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

		[Lion, Export ("disableRelaunchOnLogin")]
		void DisableRelaunchOnLogin ();

		[Lion, Export ("enableRelaunchOnLogin")]
		void EnableRelaunchOnLogin ();

		[Lion, Export ("enabledRemoteNotificationTypes")]
		NSRemoteNotificationType EnabledRemoteNotificationTypes ();

		[Lion, Export ("registerForRemoteNotificationTypes")]
		void RegisterForRemoteNotificationTypes (NSRemoteNotificationType types);

		[Lion, Export ("unregisterForRemoteNotifications")]
		void UnregisterForRemoteNotifications ();

		[Field ("NSApplicationDidBecomeActiveNotification")]
		NSString DidBecomeActiveNotification { get; }

		[Field ("NSApplicationDidHideNotification")]
		NSString DidHideNotification { get; }

		[Field ("NSApplicationDidFinishLaunchingNotification")]
		NSString DidFinishLaunchingNotification { get; }

		[Field ("NSApplicationDidResignActiveNotification")]
		NSString DidResignActiveNotification { get; }

		[Field ("NSApplicationDidUnhideNotification")]
		NSString DidUnhideNotification { get; }

		[Field ("NSApplicationDidUpdateNotification")]
		NSString DidUpdateNotification { get; }

		[Field ("NSApplicationWillBecomeActiveNotification")]
		NSString WillBecomeActiveNotification { get; }

		[Field ("NSApplicationWillHideNotification")]
		NSString WillHideNotification { get; }

		[Field ("NSApplicationWillFinishLaunchingNotification")]
		NSString WillFinishLaunchingNotification { get; }

		[Field ("NSApplicationWillResignActiveNotification")]
		NSString WillResignActiveNotification { get; }

		[Field ("NSApplicationWillUnhideNotification")]
		NSString WillUnhideNotification { get; }

		[Field ("NSApplicationWillUpdateNotification")]
		NSString WillUpdateNotification { get; }

		[Field ("NSApplicationWillTerminateNotification")]
		NSString WillTerminateNotification { get; }

		[Field ("NSApplicationDidChangeScreenParametersNotification")]
		NSString DidChangeScreenParametersNotification { get; }

		[Lion, Field ("NSApplicationLaunchRemoteNotificationKe")]
		NSString LaunchRemoteNotificationKe { get; }

		[Lion, Field ("NSApplicationLaunchIsDefaultLaunchKe")]
		NSString LaunchIsDefaultLaunchKe { get; }

		[Lion, Field ("NSApplicationLaunchIsDefaultLaunchKey")]
		NSString LaunchIsDefaultLaunchKey  { get; }

		[Lion, Field ("NSApplicationLaunchRemoteNotificationKey")]
		NSString LaunchRemoteNotificationKey { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSApplicationDelegate {
		[Export ("applicationShouldTerminate:"), DelegateName ("NSApplicationTermination"), DefaultValue (NSApplicationTerminateReply.Now)]
		NSApplicationTerminateReply ApplicationShouldTerminate (NSApplication  sender);
	
		[Export ("application:openFile:"), DelegateName ("NSApplicationFile"), DefaultValue (false)]
		bool OpenFile (NSApplication sender, string  filename);
	
		[Export ("application:openFiles:"), EventArgs ("NSApplicationFiles")]
		void OpenFiles (NSApplication sender, string [] filenames);
	
		[Export ("application:openTempFile:"), DelegateName ("NSApplicationFile"), DefaultValue (false)]
		bool OpenTempFile (NSApplication sender, string  filename);
	
		[Export ("applicationShouldOpenUntitledFile:"), DelegateName ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationShouldOpenUntitledFile (NSApplication  sender);
	
		[Export ("applicationOpenUntitledFile:"), DelegateName ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationOpenUntitledFile (NSApplication sender);
	
		[Export ("application:openFileWithoutUI:"), DelegateName ("NSApplicationFileCommand"), DefaultValue (false)]
		bool OpenFileWithoutUI (NSObject sender, string filename);
	
		[Export ("application:printFile:"), DelegateName ("NSApplicationFile"), DefaultValue (false)]
		bool PrintFile (NSApplication sender, string filename);
	
		[Export ("application:printFiles:withSettings:showPrintPanels:"), DelegateName ("NSApplicationPrint"), DefaultValue (NSApplicationPrintReply.Failure)]
		NSApplicationPrintReply PrintFiles (NSApplication application, string [] fileNames, NSDictionary printSettings, bool showPrintPanels);
	
		[Export ("applicationShouldTerminateAfterLastWindowClosed:"), DelegateName ("NSApplicationPredicate"), DefaultValue (false)]
		bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender);
	
		[Export ("applicationShouldHandleReopen:hasVisibleWindows:"), DelegateName ("NSApplicationReopen"), DefaultValue (false)]
		bool ApplicationShouldHandleReopen (NSApplication sender, bool hasVisibleWindows);
	
		[Export ("applicationDockMenu:"), DelegateName ("NSApplicationMenu"), DefaultValue (null)]
		NSMenu ApplicationDockMenu (NSApplication sender);
	
		[Export ("application:willPresentError:"), DelegateName ("NSApplicationError"), DefaultValue (null)]
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
		void DidBecomeActive (NSNotification notification);
	
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

		[Export ("registerServicesMenuSendTypes:returnTypes:"), EventArgs ("NSApplicationRegister")]
		void RegisterServicesMenu (string [] sendTypes, string [] returnTypes);
	
		[Export ("writeSelectionToPasteboard:types:"), DelegateName ("NSApplicationSelection"), DefaultValue (false)]
		bool WriteSelectionToPasteboard (NSPasteboard board, string [] types);
	
		[Export ("readSelectionFromPasteboard:"), DelegateName ("NSPasteboardPredicate"), DefaultValue (false)]
		bool ReadSelectionFromPasteboard (NSPasteboard pboard);
	
		[Export ("orderFrontStandardAboutPanel:"), EventArgs ("NSObject")]
		void OrderFrontStandardAboutPanel (NSObject sender);
	
		[Export ("orderFrontStandardAboutPanelWithOptions:"), EventArgs ("NSDictionary")]
		void OrderFrontStandardAboutPanelWithOptions (NSDictionary optionsDictionary);

		[Lion, Export ("application:didRegisterForRemoteNotificationsWithDeviceToken:"), EventArgs ("NSData")]
		void RegisteredForRemoteNotifications (NSApplication application, NSData deviceToken);

		[Lion, Export ("application:didFailToRegisterForRemoteNotificationsWithError:"), EventArgs ("NSError")]
		void FailedToRegisterForRemoteNotifications (NSApplication application, NSError error);

		[Lion, Export ("application:didReceiveRemoteNotification:"), EventArgs ("NSDictionary")]
		void ReceivedRemoteNotification (NSApplication application, NSDictionary userInfo);

		[Lion, Export ("application:willEncodeRestorableState:"), EventArgs ("NSCoder")]
		void WillEncodeRestorableState (NSApplication app, NSCoder encoder);

		[Lion, Export ("application:didDecodeRestorableState:"), EventArgs ("NSCoder")]
		void DecodedRestorableState (NSApplication app, NSCoder state);
	}
	
	[BaseType (typeof (NSObjectController))]
	public interface NSArrayController {
		[Export ("rearrangeObjects")]
		void RearrangeObjects ();

		[Export ("automaticRearrangementKeyPaths")]
		NSObject [] AutomaticRearrangementKeyPaths ();

		[Export ("didChangeArrangementCriteria")]
		void DidChangeArrangementCriteria ();

		[Export ("arrangeObjects:")]
		NSObject [] ArrangeObjects (NSObject [] objects);

		[Export ("arrangedObjects")]
		NSObject [] ArrangedObjects ();

		[Export ("addSelectionIndexes:")]
		bool AddSelectionIndexes (NSIndexSet indexes);

		[Export ("removeSelectionIndexes:")]
		bool RemoveSelectionIndexes (NSIndexSet indexes);

		[Export ("addSelectedObjects:")]
		bool AddSelectedObjects (NSObject [] objects);

		[Export ("removeSelectedObjects:")]
		bool RemoveSelectedObjects (NSObject [] objects);

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("remove:")]
		void RemoveOp (NSObject sender);

		[Export ("insert:")]
		void Insert (NSObject sender);

		[Export ("canInsert")]
		bool CanInsert ();

		[Export ("selectNext:")]
		void SelectNext (NSObject sender);

		[Export ("selectPrevious:")]
		void SelectPrevious (NSObject sender);

		[Export ("canSelectNext")]
		bool CanSelectNext ();

		[Export ("canSelectPrevious")]
		bool CanSelectPrevious ();

		[Export ("addObject:")]
		void AddObject (NSObject aObject);

		[Export ("addObjects:")]
		void AddObjects (NSArray objects);

		[Export ("insertObject:atArrangedObjectIndex:")]
		void Insert (NSObject aObject, int index);

		[Export ("insertObjects:atArrangedObjectIndexes:")]
		void Insert (NSObject [] objects, NSIndexSet indexes);

		[Export ("removeObjectAtArrangedObjectIndex:")]
		void RemoveAt (int index);

		[Export ("removeObjectsAtArrangedObjectIndexes:")]
		void Remove (NSIndexSet indexes);

		[Export ("removeObject:")]
		void Remove (NSObject aObject);

		[Export ("removeObjects:")]
		void Remove (NSObject [] objects);

		//Detected properties
		[Export ("automaticallyRearrangesObjects")]
		bool AutomaticallyRearrangesObjects { get; set; }

		[Export ("sortDescriptors")]
		NSObject [] SortDescriptors { get; set; }

		[Export ("filterPredicate")]
		NSPredicate FilterPredicate { get; set; }

		[Export ("clearsFilterPredicateOnInsertion")]
		bool ClearsFilterPredicateOnInsertion { get; set; }

		[Export ("avoidsEmptySelection")]
		bool AvoidsEmptySelection { get; set; }

		[Export ("preservesSelection")]
		bool PreservesSelection { get; set; }

		[Export ("selectsInsertedObjects")]
		bool SelectsInsertedObjects { get; set; }

		[Export ("alwaysUsesMultipleValuesMarker")]
		bool AlwaysUsesMultipleValuesMarker { get; set; }

		[Export ("selectionIndexes")]
		NSIndexSet SelectionIndexes { get; set; }

		[Export ("selectionIndex")]
		int SelectionIndex { get; set; }

		[Export ("selectedObjects")]
		NSObject [] SelectedObjects { get; set; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSBezierPath {

		[Static]
		[Export ("bezierPathWithRect:")]
		NSBezierPath FromRect (RectangleF rect);

		[Static]
		[Export ("bezierPathWithOvalInRect:")]
		NSBezierPath FromOvalInRect (RectangleF rect);

		[Static]
		[Export ("bezierPathWithRoundedRect:xRadius:yRadius:")]
		NSBezierPath FromRoundedRect (RectangleF rect, float xRadius, float yRadius);

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

		//IntPtr is exposed because the packedGlyphs should be treated as a "black box"
		[Static]
		[Export ("drawPackedGlyphs:atPoint:")]
		void DrawPackedGlyphsAtPoint (IntPtr packedGlyphs, PointF point);

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

		[Export ("getLineDash:count:phase:"), Internal]
		void _GetLineDash (IntPtr pattern, out int count, out float phase);

		[Export ("setLineDash:count:phase:"), Internal]
		void _SetLineDash (IntPtr pattern, int count, float phase);

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

		[Export ("elementAtIndex:associatedPoints:"), Internal]
		NSBezierPathElement _ElementAt (int index, IntPtr points);

		[Export ("elementAtIndex:")]
		NSBezierPathElement ElementAt (int index);

		[Export ("setAssociatedPoints:atIndex:"), Internal]
		void _SetAssociatedPointsAtIndex (IntPtr points, int index);

		[Export ("appendBezierPath:")]
		void AppendPath (NSBezierPath path);

		[Export ("appendBezierPathWithRect:")]
		void AppendPathWithRect (RectangleF rect);

		[Export ("appendBezierPathWithPoints:count:"), Internal]
		void _AppendPathWithPoints (IntPtr points, int count);

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

		[Export ("appendBezierPathWithGlyphs:count:inFont:"), Internal]
		void _AppendPathWithGlyphs (IntPtr glyphs, int count, NSFont font);

		//IntPtr is exposed because the packedGlyphs should be treated as a "black box"
		[Export ("appendBezierPathWithPackedGlyphs:")]
		void AppendPathWithPackedGlyphs (IntPtr packedGlyphs);

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
	public interface NSBitmapImageRep {
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
		NSData TiffRepresentationUsingCompressionFactor (NSTiffCompression comp, float factor);

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
		
		[Export ("representationUsingType:properties:")]
		NSData RepresentationUsingTypeProperties(NSBitmapImageFileType storageType, NSDictionary properties);
	}

	[BaseType (typeof (NSView))]
	public interface NSBox {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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
	public interface NSBrowser {
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

		[Export ("isLeafItem:")]
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
	public interface NSBrowserDelegate {
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
		//FIXME: NSBrowserDropOperation is also a ref (in/out) parameter
		NSDragOperation ValidateDrop (NSBrowser browser, NSDraggingInfo info, ref int row, ref int column, NSBrowserDropOperation dropOperation);

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
	public interface NSBrowserCell {
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
	public interface NSButtonCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

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
	public interface NSButton {
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
		NSCellStateValue State { get; set; }
	
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

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }

		[Export ("attributedAlternateTitle")]
		NSAttributedString AttributedAlternateTitle  { get; set; }

		[Export ("bezelStyle")]
		NSBezelStyle BezelStyle { get; set; }

		[Export ("allowsMixedState")]
		bool AllowsMixedState { get; set;}
		
		[Export ("setNextState")]
		void SetNextState ();

		[Export ("showsBorderOnlyWhileMouseInside")]
		bool ShowsBorderOnlyWhileMouseInside ();

		[Export ("sound")]
		NSSound Sound { get; set; }
	}
	
	[BaseType (typeof (NSImageRep))]
	public interface NSCachedImageRep {
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
	public interface NSCell {
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
		NSCellStateValue State { get; set; }
	
		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }
	
		[Export ("action"), NullAllowed]
		Selector Action { get; set; }
	
		[Export ("tag")]
		int Tag { get; set; }
	
		[Export ("title")]
		string Title { get; set; }
	
		[Export ("isOpaque")]
		bool IsOpaque { get; } 
	
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }
	
		[Export ("sendActionOn:")]
		int SendActionOn (int mask);
	
		[Export ("continuous")]
		bool IsContinuous { [Bind ("isContinuous")] get; set; }
	
		[Export ("editable")]
		bool Editable { [Bind ("isEditable")] get; set; }
	
		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")] get; set; }
	
		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")] get; set; }
	
		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")] get; set; }
	
		[Export ("scrollable")]
		bool Scrollable { [Bind ("isScrollable")] get; set; }
	
		[Export ("highlighted")]
		bool Highlighted { [Bind ("isHighlighted")] get; set; }
	
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
		void Highlight (bool highlight, RectangleF withFrame, NSView  inView);
	
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
		NSFocusRingType FocusRingType { get; set; }
	
		[Static, Export ("defaultFocusRingType")]
		NSFocusRingType DefaultFocusRingType { get; }
	
		[Export ("wantsNotificationForMarkedText")]
		bool WantsNotificationForMarkedText { get; }
	
		// NSCell(NSCellAttributedStringMethods)
		[Export ("attributedStringValue")]
		NSAttributedString AttributedStringValue { get; set; }
	
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
	
		[Lion, Export ("draggingImageComponentsWithFrame:inView:")]
		NSDraggingImageComponent [] GenerateDraggingImageComponents (RectangleF frame, NSView view);

		[Lion, Export ("drawFocusRingMaskWithFrame:inView:")]
		void DrawFocusRing (RectangleF cellFrameMask, NSView inControlView);

		[Lion, Export ("focusRingMaskBoundsForFrame:inView:")]
		RectangleF GetFocusRingMaskBounds (RectangleF cellFrame, NSView controlView);
	}

	[BaseType (typeof (NSImageRep))]
	public interface NSCIImageRep {
		[Static]
		[Export ("imageRepWithCIImage:")]
		NSCIImageRep FromCIImage (CIImage image);

		[Export ("initWithCIImage:")]
		IntPtr Constructor (CIImage image);

		[Export ("CIImage")]
		CIImage CIImage { get; }
	}
	
	[BaseType (typeof (NSView))]
	public interface NSClipView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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
		RectangleF DocumentVisibleRect ();
	
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

	[BaseType (typeof (NSViewController))]
	public interface NSCollectionViewItem {
		[Export ("collectionView")]
		NSCollectionView CollectionView { get; }

		[Export ("selected")]
		bool Selected { [Bind ("isSelected")]get; set; }
	}

	[BaseType (typeof (NSView))]
	public interface NSCollectionView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("isFirstResponder")]
		bool IsFirstResponder { get; } 

		[Export ("newItemForRepresentedObject:")]
		NSCollectionViewItem NewItemForRepresentedObject (NSObject obj);

		[Export ("itemAtIndex:")]
		NSCollectionViewItem ItemAtIndex (int index);

		[Export ("frameForItemAtIndex:")]
		RectangleF FrameForItemAtIndex (int index);

		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSource (NSDragOperation dragOperationMask, bool localDestination);

		//[Export ("draggingImageForItemsAtIndexes:withEvent:offset:")]
		//NSImage DraggingImage (NSIndexSet itemIndexes, NSEvent evt, NSPointPointer dragImageOffset);

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		NSCollectionViewDelegate Delegate { get; set; }

		[Export ("content")]
		NSObject [] Content { get; set; }

		[Export ("selectable")]
		bool Selectable { [Bind ("isSelectable")]get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("selectionIndexes")]
		NSIndexSet SelectionIndexes { get; set; }

		[Export ("itemPrototype")]
		NSCollectionViewItem ItemPrototype { get; set; }

		[Export ("maxNumberOfRows")]
		int MaxNumberOfRows { get; set; }

		[Export ("maxNumberOfColumns")]
		int MaxNumberOfColumns { get; set; }

		[Export ("minItemSize")]
		SizeF MinItemSize { get; set; }

		[Export ("maxItemSize")]
		SizeF MaxItemSize { get; set; }

		[Export ("backgroundColors"), NullAllowed]
		NSColor [] BackgroundColors { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSCollectionViewDelegate {
		[Export ("collectionView:canDragItemsAtIndexes:withEvent:")]
		bool CanDragItems (NSCollectionView collectionView, NSIndexSet indexes, NSEvent evt);

		[Export ("collectionView:writeItemsAtIndexes:toPasteboard:")]
		bool WriteItems (NSCollectionView collectionView, NSIndexSet indexes, NSPasteboard toPasteboard);

		[Export ("collectionView:namesOfPromisedFilesDroppedAtDestination:forDraggedItemsAtIndexes:")]
		string [] NamesOfPromisedFilesDroppedAtDestination (NSCollectionView collectionView, NSUrl dropUrl, NSIndexSet indexes);

		//[Export ("collectionView:draggingImageForItemsAtIndexes:withEvent:offset:")]
		//NSImage DraggingImageForItems (NSCollectionView collectionView, NSIndexSet indexes, NSEvent evg, NSPointPointer dragImageOffset);

		[Export ("collectionView:validateDrop:proposedIndex:dropOperation:")]
		//FIXME: NSCollectionViewDropOperation is also a ref (in/out) parameter
		NSDragOperation ValidateDrop (NSCollectionView collectionView, NSDraggingInfo draggingInfo, ref int dropIndex, NSCollectionViewDropOperation dropOperation);

		[Export ("collectionView:acceptDrop:index:dropOperation:")]
		bool AcceptDrop (NSCollectionView collectionView, NSDraggingInfo draggingInfo, int index, NSCollectionViewDropOperation dropOperation);

	}
	
	[BaseType (typeof (NSObject))]
	public interface NSColor {
		[Static]
		[Export ("colorWithCalibratedWhite:alpha:")]
		NSColor FromCalibratedWhite (float white, float alpha);

		[Static]
		[Export ("colorWithCalibratedHue:saturation:brightness:alpha:")]
		NSColor FromCalibratedHsba (float hue, float saturation, float brightness, float alpha);

		[Static]
		[Export ("colorWithCalibratedRed:green:blue:alpha:")]
		NSColor FromCalibratedRgba (float red, float green, float blue, float alpha);

		[Static]
		[Export ("colorWithDeviceWhite:alpha:")]
		NSColor FromDeviceWhite (float white, float alpha);

		[Static]
		[Export ("colorWithDeviceHue:saturation:brightness:alpha:")]
		NSColor FromDeviceHsba (float hue, float saturation, float brightness, float alpha);

		[Static]
		[Export ("colorWithDeviceRed:green:blue:alpha:")]
		NSColor FromDeviceRgba (float red, float green, float blue, float alpha);

		[Static]
		[Export ("colorWithDeviceCyan:magenta:yellow:black:alpha:")]
		NSColor FromDeviceCymka (float cyan, float magenta, float yellow, float black, float alpha);

		[Static]
		[Export ("colorWithCatalogName:colorName:")]
		NSColor FromCatalogName (string listName, string colorName);

		[Static]
		[Export ("colorWithColorSpace:components:count:"), Internal]
		NSColor _FromColorSpace (NSColorSpace space, IntPtr components, int numberOfComponents);
		
		[Static]
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

		[Static]
		[Export ("alternateSelectedControlColor")]
		NSColor AlternateSelectedControl { get; }

		[Static]
		[Export ("alternateSelectedControlTextColor")]
		NSColor AlternateSelectedControlText { get; }

		[Static]
		[Export ("controlAlternatingRowBackgroundColors")]
		NSColor [] ControlAlternatingRowBackgroundColors ();

		[Export ("highlightWithLevel:")]
		NSColor HighlightWithLevel (float highlightLevel);

		[Export ("shadowWithLevel:")]
		NSColor ShadowWithLevel (float shadowLevel);

		[Static]
		[Export ("colorForControlTint:")]
		NSColor FromControlTint (NSControlTint controlTint);

		[Static]
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
		NSColor UsingColorSpace ([NullAllowed] string colorSpaceName);

		[Export ("colorUsingColorSpaceName:device:")]
		NSColor UsingColorSpace ([NullAllowed] string colorSpaceName, [NullAllowed] NSDictionary deviceDescription);

		[Export ("colorUsingColorSpace:")]
		NSColor UsingColorSpace (NSColorSpace colorSpace);

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
		float BlueComponent { get; }

		[Export ("getRed:green:blue:alpha:")]
		void GetRgba (out float red, out float green, out float blue, out float alpha);

		[Export ("hueComponent")]
		float HueComponent { get; }

		[Export ("saturationComponent")]
		float SaturationComponent { get; }

		[Export ("brightnessComponent")]
		float BrightnessComponent { get; }

		[Export ("getHue:saturation:brightness:alpha:")]
		void GetHsba (out float hue, out float saturation, out float brightness, out float alpha);

		[Export ("whiteComponent")]
		float WhiteComponent { get; }

		[Export ("getWhite:alpha:")]
		void GetWhiteAlpha (out float white, out float alpha);

		[Export ("cyanComponent")]
		float CyanComponent { get; }

		[Export ("magentaComponent")]
		float MagentaComponent { get; }

		[Export ("yellowComponent")]
		float YellowComponent { get; }

		[Export ("blackComponent")]
		float BlackComponent { get; }

		[Export ("getCyan:magenta:yellow:black:alpha:")]
		void GetCmyka (out float cyan, out float magenta, out float yellow, out float black, out float alpha);

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("numberOfComponents")]
		int ComponentCount { get; }

		[Export ("getComponents:"), Internal]
		void _GetComponents (IntPtr components);

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

		[Static]
		[Export ("ignoresAlpha")]
		bool IgnoresAlpha { get; set; }

		[Static]
		[Export ("colorWithCIColor:")]
		NSColor FromCIColor (CIColor color);
	}

	[BaseType (typeof (NSObject))]
	public interface NSColorList {
		[Static]
		[Export ("availableColorLists")]
		NSColorList [] AvailableColorLists { get; }

		[Static]
		[Export ("colorListNamed:")]
		NSColorList ColorListNamed (string name);

		[Export ("initWithName:")]
		IntPtr Constructor (string name);

		[Export ("initWithName:fromFile:")]
		IntPtr Constructor (string name, [NullAllowed] string path);

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
		bool WriteToFile ([NullAllowed] string path);

		[Export ("removeFile")]
		void RemoveFile ();
	}

	[BaseType (typeof (NSPanel))]
	public interface NSColorPanel {
		[Static, Export ("sharedColorPanel")]
		NSColorPanel SharedColorPanel { get; }

		[Static]
		[Export ("sharedColorPanelExists")]
		bool SharedColorPanelExists { get; }

		[Static]
		[Export ("dragColor:withEvent:fromView:")]
		bool DragColor (NSColor color, NSEvent theEvent, NSView sourceView);

		[Static]
		[Export ("setPickerMask:")]
		void SetPickerStyle (NSColorPanelFlags mask);

		[Static]
		[Export ("setPickerMode:")]
		void SetPickerMode (NSColorPanelMode mode);

		[Export ("alpha")]
		float Alpha { get; }

		[Export ("setAction:")]
		void SetAction ([NullAllowed] Selector aSelector);

		[Export ("setTarget:")]
		void SetTarget ([NullAllowed] NSObject anObject);

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
	public interface NSColorPicker {
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
	public interface NSColorSpace {
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

		[Field ("NSCalibratedWhiteColorSpace")]
		NSString CalibratedWhite { get; }

		[Field ("NSCalibratedBlackColorSpace")]
		NSString CalibratedBlack { get; }
		
		[Field ("NSCalibratedRGBColorSpace")]
		NSString CalibratedRGB { get; }

		[Field ("NSDeviceWhiteColorSpace")]
		NSString DeviceWhite { get; }

		[Field ("NSDeviceBlackColorSpace")]
		NSString DeviceBlack { get; }

		[Field ("NSDeviceRGBColorSpace")]
		NSString DeviceRGB { get; }

		[Field ("NSDeviceCMYKColorSpace")]
		NSString DeviceCMYK { get; }

		[Field ("NSNamedColorSpace")]
		NSString Named { get; }

		[Field ("NSPatternColorSpace")]
		NSString Pattern { get; }

		[Field ("NSCustomColorSpace")]
		NSString Custom { get; }
	}

	[BaseType (typeof (NSControl))]
	public interface NSColorWell {
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


	[BaseType (typeof (NSTextField))]
	public interface NSComboBox {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);
		
		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		[Export ("intercellSpacing")]
		SizeF IntercellSpacing { get; set; }

		[Export ("itemHeight")]
		float ItemHeight { get; set; }

		[Export ("numberOfVisibleItems")]
		int VisibleItems { get; set; }

		[Export ("buttonBordered:")]
		bool ButtonBordered { [Bind ("isButtonBordered")] get; set; }

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("noteNumberOfItemsChanged")]
		void NoteNumberOfItemsChanged ();

		[Export ("usesDataSource")]
		bool UsesDataSource { get; set; }

		[Export ("scrollItemAtIndexToTop:")]
		void ScrollItemAtIndexToTop (int scrollItemIndex);

		[Export ("scrollItemAtIndexToVisible:")]
		void ScrollItemAtIndexToVisible (int scrollItemIndex);

		[Export ("selectItemAtIndex:")]
		void SelectItem (int itemIndex);

		[Export ("deselectItemAtIndex:")]
		void DeselectItem (int itemIndex);

		//- (NSInteger)indexOfSelectedItem;
		[Export ("indexOfSelectedItem")]
		int SelectedIndex { get; }

		[Export ("numberOfItems")]
		int Count { get; }

		[Export ("completes")]
		bool Completes { get; set; }

		[Export ("dataSource")]
		NSComboBoxDataSource DataSource { get; set; }

		[Export ("addItemWithObjectValue:")]
		void Add (NSObject object1);

		[Export ("addItemsWithObjectValues:")]
		void Add (NSObject [] items);

		[Export ("insertItemWithObjectValue:atIndex:")]
		void Insert (NSObject object1, int index);

		[Export ("removeItemWithObjectValue:")]
		void Remove (NSObject object1);

		[Export ("removeItemAtIndex:")]
		void RemoveAt (int index);

		[Export ("removeAllItems")]
		void RemoveAll ();

		[Export ("selectItemWithObjectValue:")]
		void Select (NSObject object1);

		[Export ("itemObjectValueAtIndex:")]
		NSComboBox GetItem (int index);

		[Export ("objectValueOfSelectedItem")]
		NSObject SelectedValue { get; }

		[Export ("indexOfItemWithObjectValue:")]
		int IndexOf (NSObject object1);

		[Export ("objectValues")]
		NSObject [] Values { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSComboBoxDataSource {
		[Export ("comboBox:objectValueForItemAtIndex:")]
		NSObject ObjectValueForItem (NSComboBox comboBox, int index);
		
		[Export ("numberOfItemsInComboBox:")]
		int ItemCount (NSComboBox comboBox);
		
		[Export ("comboBox:completedString:")]
		string CompletedString (NSComboBox comboBox, string uncompletedString);
		
		[Export ("comboBox:indexOfItemWithStringValue:")]
		int IndexOfItem (NSComboBox comboBox, string value);
	}
	
	[BaseType (typeof (NSView))]
	public interface NSControl {
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

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("action"), NullAllowed]
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
		NSTextAlignment Alignment { get; set; }

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
	public interface NSController {
		[Export ("objectDidBeginEditing:")]
		void ObjectDidBeginEditing (NSObject editor);

		[Export ("objectDidEndEditing:")]
		void ObjectDidEndEditing (NSObject editor);

		[Export ("discardEditing")]
		void DiscardEditing ();

		[Export ("commitEditing")]
		bool CommitEditing { get; }

		[Export ("commitEditingWithDelegate:didCommitSelector:contextInfo:")]
		void CommitEditingWithDelegate (NSObject delegate1, Selector didCommitSelector, IntPtr contextInfo);

		[Export ("isEditing")]
		bool IsEditing { get; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSCursor {
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
	public interface NSCustomImageRep {
		[Export ("initWithDrawSelector:delegate:")]
		IntPtr Constructor (Selector drawSelectorMethod, NSObject delegateObject);

		[Export ("drawSelector")]
		Selector DrawSelector { get; }
		
		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]  
		NSObject WeakDelegate { get; set; }  
		
		[Wrap ("WeakDelegate")][NullAllowed]  
		NSObject Delegate { get; set; }  

	}	

	[BaseType (typeof (NSControl), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] {typeof (NSDatePickerCellDelegate)})]
	public interface NSDatePicker {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		//Detected properties
		[Export ("datePickerStyle")]
		NSDatePickerStyle DatePickerStyle { get; set; }

		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")]get; set; }

		[Export ("bordered")]
		bool Bordered { [Bind ("isBordered")]get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("cell")]
		NSDatePickerCell Cell { get; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("datePickerMode")]
		NSDatePickerMode DatePickerMode { get; set; }

		[Export ("datePickerElements")]
		NSDatePickerElementFlags DatePickerElements { get; set; }

		[Export ("calendar")]
		NSCalendar Calendar { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("dateValue")]
		NSDate DateValue { get; set; }

		[Export ("timeInterval")]
		double TimeInterval { get; set; }

		[Export ("minDate")]
		NSDate MinDate { get; set; }

		[Export ("maxDate")]
		NSDate MaxDate { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSDatePickerCellDelegate Delegate { get; set; }
	}

	[BaseType (typeof (NSActionCell), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] {typeof (NSDatePickerCellDelegate)})]
	public interface NSDatePickerCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		//Detected properties
		[Export ("datePickerStyle")]
		NSDatePickerStyle DatePickerStyle { get; set; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("textColor")]
		NSColor TextColor { get; set; }

		[Export ("datePickerMode")]
		NSDatePickerMode DatePickerMode { get; set; }

		[Export ("datePickerElements")]
		NSDatePickerElementFlags DatePickerElements { get; set; }

		[Export ("calendar")]
		NSCalendar Calendar { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("dateValue")]
		NSDate DateValue { get; set; }

		[Export ("timeInterval")]
		double TimeInterval { get; set; }

		[Export ("minDate")]
		NSDate MinDate { get; set; }

		[Export ("maxDate")]
		NSDate MaxDate { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSDatePickerCellDelegate Delegate { get; set; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDatePickerCellDelegate {
		[Export ("datePickerCell:validateProposedDateValue:timeInterval:"), EventArgs ("NSDatePickerValidator")]
		void ValidateProposedDateValue (NSDatePickerCell aDatePickerCell, ref NSDate proposedDateValue, double proposedTimeInterval);
	}

	[BaseType (typeof (NSObject))]
	public interface NSDockTile {
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

		[Export ("badgeLabel"), NullAllowed]
		string BadgeLabel { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDockTilePlugIn {
		[Abstract]
		[Export ("setDockTile:")]
		void SetDockTile (NSDockTile dockTile);

		[Abstract]
		[Export ("dockMenu")]
		NSMenu DockMenu ();
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSDocument {
		[Export ("initWithType:error:")]
		IntPtr Constructor (string typeName, out NSError outError);

		[Export ("canConcurrentlyReadDocumentsOfType:")]
		bool CanConcurrentlyReadDocumentsOfType (string typeName);

		// Binding out error
		[Export ("initWithContentsOfURL:ofType:error:")]
		IntPtr Constructor (NSUrl absoluteUrl, string typeName, out NSError outError);

		[Export ("initForURL:withContentsOfURL:ofType:error:")]
		IntPtr Constructor (NSUrl absoluteDocumentUrl, NSUrl absoluteDocumentContentsUrl, string typeName, out NSError outError);

		 [Export ("revertDocumentToSaved:")]
		 void RevertDocumentToSaved (NSObject sender);

		 [Export ("revertToContentsOfURL:ofType:error:")]
		 bool RevertToContentsOfUrl (NSUrl absoluteUrl, string typeName, out NSError outError);

		[Export ("readFromURL:ofType:error:")]
		bool ReadFromUrl (NSUrl absoluteUrl, string typeName, out NSError outError);

		[Export ("readFromFileWrapper:ofType:error:")]
		bool ReadFromFileWrapper (NSFileWrapper fileWrapper, string typeName, out NSError outError);

		[Export ("readFromData:ofType:error:")]
		bool ReadFromData (NSData data, string typeName, out NSError outError);

		[Export ("writeToURL:ofType:error:")]
		bool WriteToUrl (NSUrl absoluteUrl, string typeName, out NSError outError);

		[Export ("fileWrapperOfType:error:")]
		NSFileWrapper GetAsFileWrapper (string typeName, out NSError outError);

		[Export ("dataOfType:error:")]
		NSData GetAsData (string typeName, out NSError outError);

		[Export ("writeSafelyToURL:ofType:forSaveOperation:error:")]
		bool WriteSafelyToUrl (NSUrl absoluteUrl, string typeName, NSSaveOperationType saveOperation, out NSError outError);

		[Export ("writeToURL:ofType:forSaveOperation:originalContentsURL:error:")]
		bool WriteToUrl (NSUrl absoluteUrl, string typeName, NSSaveOperationType saveOperation, NSUrl absoluteOriginalContentsUrl, out NSError outError);

		[Export ("fileAttributesToWriteToURL:ofType:forSaveOperation:originalContentsURL:error:")]
		NSDictionary FileAttributesToWrite (NSUrl toUrl, string typeName, NSSaveOperationType saveOperation, NSUrl absoluteOriginalContentsUrl, out NSError outError);

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
		void SaveToUrl (NSUrl absoluteUrl, string typeName, NSSaveOperationType saveOperation, NSObject delegateObject, Selector didSaveSelector, IntPtr contextInfo);

		[Export ("saveToURL:ofType:forSaveOperation:error:")]
		bool SaveToUrl (NSUrl absoluteUrl, string typeName, NSSaveOperationType saveOperation, out NSError outError);

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
		NSPrintOperation PrintOperation (NSDictionary printSettings, out NSError outError);

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
		NSUrl AutosavedContentsFileUrl { get; set; }

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; set; }

		[Export ("undoManager")]
		NSUndoManager UndoManager { get; set; }

		[Export ("hasUndoManager")]
		bool HasUndoManager { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSDocumentController {
		[Static, Export ("sharedDocumentController")]
		NSObject SharedDocumentController { get; }

		[Export ("documents")]
		NSDocument [] Documents { get; }

		[Export ("currentDocument")]
		NSDocument CurrentDocument { get; }

		[Export ("currentDirectory")]
		string CurrentDirectory { get; }

		[Export ("documentForURL:")]
		NSDocument DocumentForUrl (NSUrl absoluteUrl);

		[Export ("documentForWindow:")]
		NSDocument DocumentForWindow (NSWindow window);

		[Export ("addDocument:")]
		void AddDocument (NSDocument document);

		[Export ("removeDocument:")]
		void RemoveDocument (NSDocument document);

		[Export ("newDocument:")]
		void NewDocument ([NullAllowed] NSObject sender);

		[Export ("openUntitledDocumentAndDisplay:error:")]
		NSObject OpenUntitledDocument (bool displayDocument, out NSError outError);

		[Export ("makeUntitledDocumentOfType:error:")]
		NSObject MakeUntitledDocument (string typeName, out NSError error);

		[Export ("openDocument:")]
		void OpenDocument ([NullAllowed] NSObject sender);

		[Export ("URLsFromRunningOpenPanel")]
		NSUrl [] UrlsFromRunningOpenPanel ();

		[Export ("runModalOpenPanel:forTypes:")]
		int RunModalOpenPanelforTypes (NSOpenPanel openPanel, string [] types);

		[Export ("openDocumentWithContentsOfURL:display:error:")]
		NSObject OpenDocument (NSUrl absoluteUrl, bool displayDocument, out NSError outError);

		[Export ("makeDocumentWithContentsOfURL:ofType:error:")]
		NSObject MakeDocument (NSUrl absoluteUrl, string typeName, out NSError outError);

		[Export ("reopenDocumentForURL:withContentsOfURL:error:")]
		bool ReopenDocument (NSUrl absoluteDocumentUrl, NSUrl absoluteDocumentContentsUrl, out NSError outError);

		[Export ("makeDocumentForURL:withContentsOfURL:ofType:error:")]
		NSObject MakeDocument (NSUrl absoluteDocumentUrl, NSUrl absoluteDocumentContentsUrl, string typeName, out NSError outError);

		[Export ("saveAllDocuments:")]
		void SaveAllDocuments ([NullAllowed] NSObject sender);

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
		void ClearRecentDocuments ([NullAllowed] NSObject sender);

		[Export ("noteNewRecentDocument:")]
		void NoteNewRecentDocument (NSDocument document);

		[Export ("noteNewRecentDocumentURL:")]
		void NoteNewRecentDocumentURL (NSUrl absoluteUrl);

		[Export ("recentDocumentURLs")]
		NSUrl [] RecentDocumentUrls { get; }

		[Export ("defaultType")]
		string DefaultType { get; }

		[Export ("typeForContentsOfURL:error:")]
		string TypeForUrl (NSUrl inAbsoluteUrl, out NSError outError);

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

	[Lion]
	[BaseType (typeof (NSObject))]
	public interface NSDraggingImageComponent {
		[Export ("key")]
		string Key { get; set;  }

		[Export ("contents")]
		NSObject Contents { get; set;  }

		[Export ("frame")]
		RectangleF Frame { get; set;  }

		[Static]
		[Export ("draggingImageComponentWithKey:")]
		NSDraggingImageComponent FromKey (string key);

		[Export ("initWithKey:")]
		IntPtr Constructor (string key);

		[Field ("NSDraggingImageComponentIconKey")]
		NSString IconKey { get; }

		[Field ("NSDraggingImageComponentLabelKey")]
		NSString LabelKey { get; }
	}

	delegate NSDraggingImageComponent [] NSDraggingItemImagesContentProvider ();
	
	[BaseType (typeof (NSObject))]
	interface NSDraggingItem {
		[Export ("item")]
		NSObject Item { get;  }

		[Export ("draggingFrame")]
		RectangleF DraggingFrame { get; set;  }

		[Export ("imageComponents")]
		NSDraggingImageComponent [] ImageComponents { get;  }

		[Export ("initWithPasteboardWriter:")]
		IntPtr Constructor (NSPasteboardWriting pasteboardWriter);

		[Export ("setImageComponentsProvider:")]
		void SetImagesContentProvider ([NullAllowed] NSDraggingItemImagesContentProvider provider);

		[Export ("setDraggingFrame:contents:")]
		void SetDraggingFrame (RectangleF frame, NSObject contents);

	}
	
	//NSDraggingInfo is documented as a protocol, but it doesn't work as a protocol.
	//per the docs: "In Java, sender is an NSDragDestination object, which implements the NSDraggingInfo interface." - from Drag and Drop Programming Topics for Cocoa
	//furthermore, "you never need to create a class that implements the NSDraggingInfo protocol" from NSDraggingInfo Protocol Reference
	[BaseType (typeof (NSObject), Name="NSDragDestination")]
	public interface NSDraggingInfo  {
		[Export ("draggingDestinationWindow")]
		NSWindow DraggingDestinationWindow { get; }

		[Export ("draggingSourceOperationMask")]
		NSDragOperation DraggingSourceOperationMask { get; }

		[Export ("draggingLocation")]
		PointF DraggingLocation { get; }
	
		[Export ("draggedImageLocation")]
		PointF DraggedImageLocation { get; }

		[Export ("draggedImage")]
		NSImage DraggedImage { get; }

		[Export ("draggingPasteboard")]
		NSPasteboard DraggingPasteboard { get; }

		[Export ("draggingSource")]
		NSObject DraggingSource { get; }

		[Export ("draggingSequenceNumber")]
		int DraggingSequenceNumber { get; }

		[Export ("slideDraggedImageTo:")]
		void SlideDraggedImageTo (PointF screenPoint);

		[Export ("namesOfPromisedFilesDroppedAtDestination:")]
		string [] PromisedFilesDroppedAtDestination (NSUrl dropDestination);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDraggingDestination {
		[Export ("draggingEntered:"), DefaultValue (NSDragOperation.None)]
		NSDragOperation DraggingEntered (NSDraggingInfo sender);

		[Export ("draggingUpdated:"), DefaultValue (NSDragOperation.None)]
		NSDragOperation DraggingUpdated (NSDraggingInfo sender);

		[Export ("draggingExited:")]
		void DraggingExited (NSDraggingInfo sender);

		[Export ("prepareForDragOperation:"), DefaultValue (false)]
		bool PrepareForDragOperation (NSDraggingInfo sender);

		[Export ("performDragOperation:"), DefaultValue (false)]
		bool PerformDragOperation (NSDraggingInfo sender);

		[Export ("concludeDragOperation:")]
		void ConcludeDragOperation (NSDraggingInfo sender);

		[Export ("draggingEnded:")]
		void DraggingEnded (NSDraggingInfo sender);

		[Export ("wantsPeriodicDraggingUpdates"), DefaultValue (true)]
		bool WantsPeriodicDraggingUpdates { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDraggingSource {
		[Export ("draggingSourceOperationMaskForLocal:"), DefaultValue (NSDragOperation.None)]
		NSDragOperation DraggingSourceOperationMaskForLocal (bool flag);

		[Export ("namesOfPromisedFilesDroppedAtDestination:"), DefaultValue (new string[0])]
		string [] NamesOfPromisedFilesDroppedAtDestination (NSUrl dropDestination);

		[Export ("draggedImage:beganAt:")]
		void DraggedImageBeganAt (NSImage image, PointF screenPoint);

		[Export ("draggedImage:endedAt:operation:")]
		void DraggedImageEndedAtOperation (NSImage image, PointF screenPoint, NSDragOperation operation);

		[Export ("draggedImage:movedTo:")]
		void DraggedImageMovedTo (NSImage image, PointF screenPoint);

		[Export ("ignoreModifierKeysWhileDragging"), DefaultValue (false)]
		bool IgnoreModifierKeysWhileDragging { get; }

		[Obsolete ("On 10.1 and newer, use DraggedImageEndedAtOperation() instead")]
		[Export ("draggedImage:endedAt:deposited:")]
		void DraggedImageEndedAtDeposited (NSImage image, PointF screenPoint, bool deposited);
	}
	
	[BaseType (typeof (NSResponder), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSDrawerDelegate)})]
	public interface NSDrawer {
		[Export ("initWithContentSize:preferredEdge:")]
		IntPtr Constructor (SizeF contentSize, NSRectEdge edge);

		[Export ("parentWindow")]
		NSWindow ParentWindow { get; set; }

		[Export ("contentView")]
		NSView ContentView { get; set; }

		[Export ("preferredEdge")]
		NSRectEdge PreferredEdge { get; set; }
		
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSDrawerDelegate Delegate { get; set; }

		//[Export ("open")]
		//void Open ();

		[Export ("openOnEdge:")]
		void OpenOnEdge (NSRectEdge edge);

		//[Export ("close")]
		//void Close ();

		[Export ("open:")]
		void Open (NSObject sender);

		[Export ("close:")]
		void Close (NSObject sender);

		[Export ("toggle:")]
		void Toggle (NSObject sender);

		[Export ("state")]
		NSDrawerState State { get; }

		[Export ("edge")]
		NSRectEdge Edge { get; }

		[Export ("contentSize")]
		SizeF ContentSize { get; set; }

		[Export ("minContentSize")]
		SizeF MinContentSize { get; set; }

		[Export ("maxContentSize")]
		SizeF MaxContentSize { get; set; }

		[Export ("leadingOffset")]
		float LeadingOffset { get; set; }

		[Export ("trailingOffset")]
		float TrailingOffset { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSDrawerDelegate {
		[Export ("drawerDidClose:"), EventArgs ("NSNotification")]
		void DrawerDidClose (NSNotification notification);
		
		[Export ("drawerDidOpen:"), EventArgs ("NSNotification")]
		void DrawerDidOpen (NSNotification notification);

		[Export ("drawerShouldClose:"), DelegateName ("DrawerShouldCloseDelegate"), DefaultValue (true)]
		bool DrawerShouldClose (NSDrawer sender);

		[Export ("drawerShouldOpen:"), DelegateName ("DrawerShouldOpenDelegate"), DefaultValue (true)]
		bool DrawerShouldOpen (NSDrawer sender);
	
		[Export ("drawerWillClose:"), EventArgs ("NSNotification")]
		void DrawerWillClose (NSNotification notification);
	
		[Export ("drawerWillOpen:"), EventArgs ("NSNotification")]
		void DrawerWillOpen (NSNotification notification);

		[Export ("drawerWillResizeContents:toSize:"), DelegateName ("DrawerWillResizeContentsDelegate"), DefaultValue (null)]
		SizeF DrawerWillResizeContents (NSDrawer sender, SizeF toSize);

	}
	
	[BaseType (typeof (NSObject))]
	public interface NSFileWrapper {
		[Export ("initWithURL:options:error:")]
		IntPtr Constructor (NSUrl url, NSFileWrapperReadingOptions options, out NSError outError);

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
		bool MatchesContentsOfUrl (NSUrl url);

		[Export ("readFromURL:options:error:")]
		bool ReadFrom (NSUrl url, NSFileWrapperReadingOptions options, out NSError outError);

		//[Export ("writeToURL:options:originalContentsURL:error:")]
		//bool WriteTo (NSUrl url, NSFileWrapperWritingOptions options, NSUrl originalContentsURL, out NSError outError);

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
	public interface NSFont {
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

		[Static]
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

		[Static]
		[Export("menuBarFontOfSize:")]
		NSFont MenuBarFontOfSize (float fontSize);

		[Static]
		[Export("messageFontOfSize:")]
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
	public interface NSFontDescriptor {
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

	[BaseType (typeof (NSObject))]
	public interface NSFontManager {
		[Static, Export ("setFontPanelFactory:")]
		void SetFontPanelFactory (Class factoryId);

		[Static, Export ("setFontManagerFactory:")]
		void SetFontManagerFactory (Class factoryId);

		[Static, Export ("sharedFontManager")]
		NSFontManager SharedFontManager { get; }

		[Export ("isMultiple")]
		bool IsMultiple { get; }

		[Export ("selectedFont")]
		NSFont SelectedFont { get; }

		[Export ("setSelectedFont:isMultiple:")]
		void SetSelectedFont (NSFont fontObj, bool isMultiple);

		[Export ("setFontMenu:")]
		void SetFontMenu (NSMenu newMenu);

		[Export ("fontMenu:")]
		NSMenu FontMenu (bool create);

		[Export ("fontPanel:")]
		NSFontPanel FontPanel (bool create);

		[Export ("fontWithFamily:traits:weight:size:")]
		NSFont FontWithFamily (string family, NSFontTraitMask traits, int weight, float size);

		[Export ("traitsOfFont:")]
		NSFontTraitMask TraitsOfFont (NSFont fontObj);

		[Export ("weightOfFont:")]
		int WeightOfFont (NSFont fontObj);

		[Export ("availableFonts")]
		string [] AvailableFonts { get; }

		[Export ("availableFontFamilies")]
		string [] AvailableFontFamilies { get; }

		[Export ("availableMembersOfFontFamily:")]
		NSArray [] AvailableMembersOfFontFamily (string fam);

		[Export ("convertFont:")]
		NSFont ConvertFont (NSFont fontObj);

		[Export ("convertFont:toSize:")]
		NSFont ConvertFont (NSFont fontObj, float size);

		[Export ("convertFont:toFace:")]
		NSFont ConvertFont (NSFont fontObj, string typeface);

		[Export ("convertFont:toFamily:")]
		NSFont ConvertFontToFamily (NSFont fontObj, string family);

		[Export ("convertFont:toHaveTrait:")]
		NSFont ConvertFont (NSFont fontObj, NSFontTraitMask trait);

		[Export ("convertFont:toNotHaveTrait:")]
		NSFont ConvertFontToNotHaveTrait (NSFont fontObj, NSFontTraitMask trait);

		[Export ("convertWeight:ofFont:")]
		NSFont ConvertWeight (bool increaseWeight, NSFont fontObj);

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("action"), NullAllowed]
		Selector Action { get; set; }

		[Export ("sendAction")]
		bool SendAction { get; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; } 

		[Export ("localizedNameForFamily:face:")]
		string LocalizedNameForFamily (string family, string faceKey);

		[Export ("setSelectedAttributes:isMultiple:")]
		void SetSelectedAttributes (NSDictionary attributes, bool isMultiple);

		[Export ("convertAttributes:")]
		NSDictionary ConvertAttributes (NSDictionary attributes);

		[Export ("availableFontNamesMatchingFontDescriptor:")]
		string [] AvailableFontNamesMatchingFontDescriptor (NSFontDescriptor descriptor);

		[Export ("collectionNames")]
		string [] CollectionNames { get; }

		[Export ("fontDescriptorsInCollection:")]
		NSArray FontDescriptorsInCollection (string collectionNames);

		[Export ("addCollection:options:")]
		bool AddCollection (string collectionName, NSFontCollectionOptions collectionOptions);

		[Export ("removeCollection:")]
		bool RemoveCollection (string collectionName);

		[Export ("addFontDescriptors:toCollection:")]
		void AddFontDescriptors (NSFontDescriptor [] descriptors, string collectionName);

		[Export ("removeFontDescriptor:fromCollection:")]
		void RemoveFontDescriptor (NSFontDescriptor descriptor, string collection);

		[Export ("currentFontAction")]
		int CurrentFontAction { get; }

		[Export ("convertFontTraits:")]
		NSFontTraitMask ConvertFontTraits (NSFontTraitMask traits);

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("fontNamed:hasTraits:")]
		bool FontNamedHasTraits (string fName, NSFontTraitMask someTraits);

		[Export ("availableFontNamesWithTraits:")]
		string [] AvailableFontNamesWithTraits (NSFontTraitMask someTraits);

		[Export ("addFontTrait:")]
		void AddFontTrait (NSObject sender);

		[Export ("removeFontTrait:")]
		void RemoveFontTrait (NSObject sender);

		[Export ("modifyFontViaPanel:")]
		void ModifyFontViaPanel (NSObject sender);

		[Export ("modifyFont:")]
		void ModifyFont (NSObject sender);

		[Export ("orderFrontFontPanel:")]
		void OrderFrontFontPanel (NSObject sender);

		[Export ("orderFrontStylesPanel:")]
		void OrderFrontStylesPanel (NSObject sender);
	}

	[BaseType (typeof (NSPanel))]
	public interface NSFontPanel {
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
	public interface NSForm  {
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
		void SetBordered (bool bordered);

		[Export ("setBezeled:")]
		void SetBezeled (bool bezeled);

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
	public interface NSFormCell {
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

	[BaseType (typeof (NSObject))]
	public interface NSGlyphGenerator {
		[Export ("generateGlyphsForGlyphStorage:desiredNumberOfCharacters:glyphIndex:characterIndex:")]
		void GenerateGlyphs (NSObject nsGlyphStorageOrNSLayoutManager, int nchars, int glyphIndex, int charIndex);

		[Static, Export ("sharedGlyphGenerator")]
		NSGlyphGenerator SharedGlyphGenerator { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSGradient {
		[Export ("initWithStartingColor:endingColor:")]
		IntPtr Constructor  (NSColor startingColor, NSColor endingColor);

		[Export ("initWithColors:")]
		IntPtr Constructor  (NSColor[] colorArray);

		// See AppKit/NSGradiant.cs
		//[Export ("initWithColorsAndLocations:")]
		//[Export ("initWithColors:atLocations:colorSpace:")]

		[Export ("drawFromPoint:toPoint:options:")]
		void DrawFromPoint (PointF startingPoint, PointF endingPoint, NSGradientDrawingOptions options);

		[Export ("drawInRect:angle:")]
		void DrawInRect (RectangleF rect, float angle);

		[Export ("drawInBezierPath:angle:")]
		void DrawInBezierPath (NSBezierPath path, float angle);

		[Export ("drawFromCenter:radius:toCenter:radius:options:")]
		void DrawFromCenterRadius (PointF startCenter, float startRadius, PointF endCenter, float endRadius, NSGradientDrawingOptions options);

		[Export ("drawInRect:relativeCenterPosition:")]
		void DrawInRect (RectangleF rect, PointF relativeCenterPosition);

		[Export ("drawInBezierPath:relativeCenterPosition:")]
		void DrawInBezierPath (NSBezierPath path, PointF relativeCenterPosition);

		[Export ("colorSpace")]
		NSColorSpace ColorSpace { get; }

		[Export ("numberOfColorStops")]
		int ColorStopsCount { get; }

		[Export ("getColor:location:atIndex:")]
		void GetColor (out NSColor color, out float location, int index);

		[Export ("interpolatedColorAtLocation:")]
		NSColor GetInterpolatedColor(float location);
	}

	[BaseType (typeof (NSObject))]
	public interface NSGraphicsContext {
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
		CGContext GraphicsPort {get; }
	
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

	[BaseType (typeof (NSImageRep))]
	public interface NSEPSImageRep {
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

	public delegate void GlobalEventHandler (NSEvent theEvent);
	public delegate NSEvent LocalEventHandler (NSEvent theEvent);

	[BaseType (typeof (NSObject))]
	public interface NSEvent {
		[Export ("type")]
		NSEventType Type { get; }

		[Export ("modifierFlags")]
		NSEventModifierMask ModifierFlags { get; }

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
		NSEvent KeyEvent (NSEventType type, PointF location, NSEventModifierMask flags, double time, int wNum, NSGraphicsContext context, string keys, string ukeys, bool isARepeat, ushort code);

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
		NSEventModifierMask CurrentModifierFlags { get; }

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
		[Export ("addGlobalMonitorForEventsMatchingMask:handler:")]
		NSObject AddGlobalMonitorForEventsMatchingMask (NSEventMask mask, GlobalEventHandler handler);
		
		[Static]
		[Export ("addLocalMonitorForEventsMatchingMask:handler:")]
		NSObject AddLocalMonitorForEventsMatchingMask (NSEventMask mask, LocalEventHandler handler);
		
		[Static]
		[Export ("removeMonitor:")]
		void RemoveMonitor (NSObject eventMonitor);

		//Detected properties
		[Static]
		[Export ("mouseCoalescingEnabled")]
		bool MouseCoalescingEnabled { [Bind ("isMouseCoalescingEnabled")]get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSMenu {
		[Export ("initWithTitle:")]
		IntPtr Constructor (string aTitle);

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
		NSMenuItem InsertItem (string title, [NullAllowed] Selector action, string charCode, int index);

		[Export ("addItemWithTitle:action:keyEquivalent:")]
		NSMenuItem AddItem (string title, [NullAllowed] Selector action, string charCode);

		[Export ("removeItemAtIndex:")]
		void RemoveItemAt (int index);

		[Export ("removeItem:")]
		void RemoveItem (NSMenuItem item);

		[Export ("setSubmenu:forItem:")]
		void SetSubmenu (NSMenu aMenu, NSMenuItem anItem);

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
		bool AutoEnablesItems { get; set; }

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
	public interface NSMenuDelegate {
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
	public interface NSMenuItem {
		[Static]
		[Export ("separatorItem")]
		NSMenuItem SeparatorItem { get; }

		[Export ("initWithTitle:action:keyEquivalent:")]
		IntPtr Constructor (string title, [NullAllowed] Selector selectorAction, string charCode);

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
		NSCellStateValue State { get; set; }

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

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("action"), NullAllowed]
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

	[BaseType (typeof (NSButtonCell))]
	public interface NSMenuItemCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("calcSize")]
		void CalcSize ();

		[Export ("stateImageWidth")]
		float StateImageWidth ();

		[Export ("imageWidth")]
		float ImageWidth { get; }

		[Export ("titleWidth")]
		float TitleWidth { get; }

		[Export ("keyEquivalentWidth")]
		float KeyEquivalentWidth { get; }

		[Export ("stateImageRectForBounds:")]
		RectangleF StateImageRectForBounds (RectangleF cellFrame);

		[Export ("titleRectForBounds:")]
		RectangleF TitleRectForBounds (RectangleF cellFrame);

		[Export ("keyEquivalentRectForBounds:")]
		RectangleF KeyEquivalentRectForBounds (RectangleF cellFrame);

		[Export ("drawSeparatorItemWithFrame:inView:")]
		void DrawSeparatorItem (RectangleF cellFrame, NSView controlView);

		[Export ("drawStateImageWithFrame:inView:")]
		void DrawStateImage (RectangleF cellFrame, NSView controlView);

		[Export ("drawImageWithFrame:inView:")]
		void DrawImage (RectangleF cellFrame, NSView controlView);

		[Export ("drawTitleWithFrame:inView:")]
		void DrawTitle (RectangleF cellFrame, NSView controlView);

		[Export ("drawKeyEquivalentWithFrame:inView:")]
		void DrawKeyEquivalent (RectangleF cellFrame, NSView controlView);

		[Export ("drawBorderAndBackgroundWithFrame:inView:")]
		void DrawBorderAndBackground (RectangleF cellFrame, NSView controlView);

		[Export ("tag")]
		int Tag { get; }

		//Detected properties
		[Export ("menuItem")]
		NSMenuItem MenuItem { get; set; }

		[Export ("menuView")]
		NSMenuView MenuView { get; set; }

		[Export ("needsSizing")]
		bool NeedsSizing { get; set; }

		[Export ("needsDisplay")]
		bool NeedsDisplay { get; set; }

	}

	[BaseType (typeof (NSView))]
	public interface NSMenuView {
		[Static]
		[Export ("menuBarHeight")]
		float MenuBarHeight { get; }

		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frame);

		[Export ("initAsTearOff")]
		IntPtr Constructor (int tokenInitAsTearOff);

		[Export ("itemChanged:")]
		void ItemChanged (NSNotification notification);

		[Export ("itemAdded:")]
		void ItemAdded (NSNotification notification);

		[Export ("itemRemoved:")]
		void ItemRemoved (NSNotification notification);

		[Export ("update")]
		void Update ();

		[Export ("innerRect")]
		RectangleF InnerRect { get; }

		[Export ("rectOfItemAtIndex:")]
		RectangleF RectOfItemAtIndex (int index);

		[Export ("indexOfItemAtPoint:")]
		int IndexOfItemAtPoint (PointF point);

		[Export ("setNeedsDisplayForItemAtIndex:")]
		void SetNeedsDisplay (int itemAtIndex);

		[Export ("stateImageOffset")]
		float StateImageOffset { get; }

		[Export ("stateImageWidth")]
		float StateImageWidth { get; }

		[Export ("imageAndTitleOffset")]
		float ImageAndTitleOffset { get; }

		[Export ("imageAndTitleWidth")]
		float ImageAndTitleWidth { get; }

		[Export ("keyEquivalentOffset")]
		float KeyEquivalentOffset { get; }

		[Export ("keyEquivalentWidth")]
		float KeyEquivalentWidth { get; }

		[Export ("setMenuItemCell:forItemAtIndex:")]
		void SetMenuItemCell (NSMenuItemCell cell, int itemAtIndex);

		[Export ("menuItemCellForItemAtIndex:")]
		NSMenuItemCell GetMenuItemCell (int itemAtIndex);

		[Export ("attachedMenuView")]
		NSMenuView AttachedMenuView { get; }

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("attachedMenu")]
		NSMenu AttachedMenu { get; }

		[Export ("isAttached")]
		bool IsAttached { get; }

		[Export ("isTornOff")]
		bool IsTornOff { get; }

		[Export ("locationForSubmenu:")]
		PointF LocationForSubmenu (NSMenu aSubmenu);

		[Export ("setWindowFrameForAttachingToRect:onScreen:preferredEdge:popUpSelectedItem:")]
		void SetWindowFrameForAttachingToRect (RectangleF screenRect, NSScreen onScreen, NSRectEdge preferredEdge, int popupSelectedItem);

		[Export ("detachSubmenu")]
		void DetachSubmenu ();

		[Export ("attachSubmenuForItemAtIndex:")]
		void AttachSubmenuForItemAtIndex (int index);

		[Export ("performActionWithHighlightingForItemAtIndex:")]
		void PerformActionWithHighlighting (int forItemAtIndex);

		[Export ("trackWithEvent:")]
		bool TrackWithEvent (NSEvent theEvent);

		//Detected properties
		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("horizontal")]
		bool Horizontal { [Bind ("isHorizontal")]get; set; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("highlightedItemIndex")]
		int HighlightedItemIndex { get; set; }

		[Export ("needsSizing")]
		bool NeedsSizing { get; set; }

		[Export ("horizontalEdgePadding")]
		float HorizontalEdgePadding { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSNib {
		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl nibFileUrl);

		[Export ("initWithNibNamed:bundle:")]
		IntPtr Constructor (string nibName, NSBundle bundle);

		[Export ("instantiateNibWithExternalNameTable:")]
		bool InstantiateNib (NSDictionary externalNameTable);

		// This requires an "out NSArray"
		//[Export ("instantiateNibWithOwner:topLevelObjects:")]
		//bool InstantiateNib (NSObject owner, NSArray topLevelObjects);
	}	

	[BaseType (typeof (NSController))]
	public interface NSObjectController {
		[Export ("initWithContent:")]
		IntPtr Constructor (NSObject content);

		[Export ("content")]
		NSObject Content { get; set; }

		[Export ("selection")]
		NSObjectController Selection { get; }

		[Export ("selectedObjects")]
		NSObject [] SelectedObjects { get; }

		[Export ("automaticallyPreparesContent")]
		bool AutomaticallyPreparesContent { get; set; }

		[Export ("prepareContent")]
		void PrepareContent ();

		[Export ("objectClass")]
		Class ObjectClass { get; set; }

		// TODO: Geoff, can you review if we need to make this a [Factory]?
		[Export ("newObject")]
		NSObjectController NewObject { get; }

		[Export ("addObject:")]
		void AddObject (NSObject object1);

		[Export ("removeObject:")]
		void RemoveObject (NSObject object1);

		[Export ("setEditable:")]
		void SetEditable (bool editable);

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")] get; set; }

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("canAdd")]
		bool CanAdd { get; }

		[Export ("remove:")]
		void Remove (NSObject sender);

		[Export ("canRemove")]
		bool CanRemove { get; }

		[Export ("validateUserInterfaceItem:")]
		bool ValidateUserInterfaceItem (NSObject item);

		//[Export ("managedObjectContext")]
		//NSManagedObjectContext ManagedObjectContext { get; set; }

		[Export ("entityName")]
		string EntityName { get; set; }

		[Export ("fetchPredicate")]
		NSPredicate FetchPredicate { get; set; }

		//[Export ("fetchWithRequest:merge:error:")]
		//bool FetchWithRequestMerge (NSFetchRequest fetchRequest, bool merge, NSError error);

		[Export ("fetch:")]
		void Fetch (NSObject sender);

		[Export ("usesLazyFetching")]
		bool UsesLazyFetching { get; set; }

		//[Export ("defaultFetchRequest")]
		//NSFetchRequest DefaultFetchRequest { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSOpenGLPixelFormat {
		[Export ("initWithData:")]
		IntPtr Constructor (NSData attribs);

		// TODO: wrap the CLContext and take a CLContext here instead.
		//[Export ("initWithCGLPixelFormatObj:")]
		//IntPtr Constructor (IntPtr cglContextHandle);

		[Export ("getValues:forAttribute:forVirtualScreen:")]
		IntPtr GetValue (ref int vals, NSOpenGLPixelFormatAttribute attrib, int screen);

		[Export ("numberOfVirtualScreens")]
		int NumberOfVirtualScreens { get; }

		[Export ("CGLPixelFormatObj")]
		CGLPixelFormat CGLPixelFormat { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSOpenGLPixelBuffer {
		[Export ("initWithTextureTarget:textureInternalFormat:textureMaxMipMapLevel:pixelsWide:pixelsHigh:")]
		IntPtr Constructor (NSGLTextureTarget targetGlEnum, NSGLFormat format, int maxLevel, int pixelsWide, int pixelsHigh);

		// FIXME: This conflicts with our internal ctor
		// [Export ("initWithCGLPBufferObj:")]
		// IntPtr Constructor (IntPtr pbuffer);

		[Export ("CGLPBufferObj")]
		IntPtr CGLPBuffer { get; }

		[Export ("pixelsWide")]
		int PixelsWide { get; }

		[Export ("pixelsHigh")]
		int PixelsHigh { get; }

		[Export ("textureTarget")]
		NSGLTextureTarget TextureTarget { get; }

		[Export ("textureInternalFormat")]
		NSGLFormat TextureInternalFormat { get; }

		[Export ("textureMaxMipMapLevel")]
		int TextureMaxMipMapLevel { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSOpenGLContext {
		[Export ("initWithFormat:shareContext:")]
		IntPtr Constructor (NSOpenGLPixelFormat format, [NullAllowed] NSOpenGLContext shareContext);

		// FIXME: This conflicts with our internal ctor
		// [Export ("initWithCGLContextObj:")]
		// IntPtr Constructor (IntPtr cglContext);

		[Export ("setFullScreen")]
		void SetFullScreen ();

		[Export ("setOffScreen:width:height:rowbytes:")]
		void SetOffScreen (IntPtr baseaddr, int width, int height, int rowbytes);

		[Export ("clearDrawable")]
		void ClearDrawable ();

		[Export ("update")]
		void Update ();

		[Export ("flushBuffer")]
		void FlushBuffer ();

		[Export ("makeCurrentContext")]
		void MakeCurrentContext ();

		[Static]
		[Export ("clearCurrentContext")]
		void ClearCurrentContext ();

		[Static]
		[Export ("currentContext")]
		NSOpenGLContext CurrentContext { get; }

		[Export ("copyAttributesFromContext:withMask:")]
		void CopyAttributes (NSOpenGLContext context, uint mask);

		[Export ("setValues:forParameter:")]
		void SetValues (IntPtr vals, NSOpenGLContextParameter param);

		[Export ("getValues:forParameter:")]
		void GetValues (IntPtr vals, NSOpenGLContextParameter param);

		[Export ("createTexture:fromView:internalFormat:")]
		void CreateTexture (int targetIdentifier, NSView view, int format);

		[Export ("CGLContextObj")]
		CGLContext CGLContext { get; }

		[Export ("setPixelBuffer:cubeMapFace:mipMapLevel:currentVirtualScreen:")]
		void SetPixelBuffer (NSOpenGLPixelBuffer pixelBuffer, NSGLTextureCubeMap face, int level, int screen);

		[Export ("pixelBuffer")]
		NSOpenGLPixelBuffer PixelBuffer { get; }

		[Export ("pixelBufferCubeMapFace")]
		int PixelBufferCubeMapFace { get; }

		[Export ("pixelBufferMipMapLevel")]
		int PixelBufferMipMapLevel { get; }

		// TODO: fixme enumerations
		// GL_FRONT, GL_BACK, GL_AUX0
		[Export ("setTextureImageToPixelBuffer:colorBuffer:")]
		void SetTextureImage (NSOpenGLPixelBuffer pixelBuffer, NSGLColorBuffer source);

		//Detected properties
		[Export ("view")]
		NSView View { get; set; }

		[Export ("currentVirtualScreen")]
		int CurrentVirtualScreen { get; set; }
	}

	[BaseType (typeof (NSView))]
	public interface NSOpenGLView {
		[Static]
		[Export ("defaultPixelFormat")]
		NSOpenGLPixelFormat DefaultPixelFormat { get; }

		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("initWithFrame:pixelFormat:")]
		IntPtr Constructor (RectangleF frameRect, NSOpenGLPixelFormat format);

		[Export ("clearGLContext")]
		void ClearGLContext ();

		[Export ("update")]
		void Update ();

		[Export ("reshape")]
		void Reshape ();

		[Export ("prepareOpenGL")]
		void PrepareOpenGL ();

		//Detected properties
		[Export ("openGLContext")]
		NSOpenGLContext OpenGLContext { get; set; }

		[Export ("pixelFormat")]
		NSOpenGLPixelFormat PixelFormat { get; set; }
	}

	[BaseType (typeof (NSSavePanel))]
	public interface NSOpenPanel {
		[Static]
		[Export ("openPanel")]
		NSOpenPanel OpenPanel { get; }

		[Export ("URLs")]
		NSUrl [] Urls { get; }

		//Detected properties
		[Export ("resolvesAliases")]
		bool ResolvesAliases { get; set; }

		[Export ("canChooseDirectories")]
		bool CanChooseDirectories { get; set; }

		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("canChooseFiles")]
		bool CanChooseFiles { get; set; }

		// Deprecated methods, but needed to run on pre 10.6 systems
		[Obsolete ("On 10.6 and newer, use Uris")]
		[Export ("filenames")]
		string [] Filenames { get; set; }

		//runModalForWindows:Completeion
		[Obsolete ("On 10.6 and newer use runModalForWindow:")]
		[Export ("beginSheetForDirectory:file:types:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (string directory, string fileName, string [] fileTypes, NSWindow modalForWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[Obsolete ("On 10.6 and newer use runWithCompletionHandler:")]
		[Export ("beginForDirectory:file:types:modelessDelegate:didEndSelector:contextInfo:")]
		void Begin (string directory, string fileName, string [] fileTypes, NSObject modelessDelegate, Selector didEndSelector, IntPtr contextInfo);
		
		[Obsolete ("On 10.6 and newer use runModal:")]
		[Export ("runModalForDirectory:file:types:")]
		int RunModal ([NullAllowed] string directory, [NullAllowed] string fileName, [NullAllowed] string [] types);

		[Obsolete ("On 10.6 and newer use runModal:")]
		[Export ("runModalForTypes:")]
		int RunModal (string [] types);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSOpenSavePanelDelegate {
		[Export ("panel:shouldEnableURL:"), DelegateName ("NSOpenSavePanelUrl"), DefaultValue (true)]
		bool ShouldEnableUrl (NSSavePanel panel, NSUrl url);

		[Export ("panel:validateURL:error:"), DelegateName ("NSOpenSavePanelValidate"), DefaultValue (true)]
		bool ValidateUrl (NSSavePanel panel, NSUrl url, out NSError outError);

		[Export ("panel:didChangeToDirectoryURL:"), EventArgs ("NSOpenSavePanelUrl")]
		void DidChangeToDirectory (NSSavePanel panel, NSUrl newDirectoryUrl);

		[Export ("panel:userEnteredFilename:confirmed:"), DelegateName ("NSOpenSaveFilenameConfirmation"), DefaultValueFromArgument ("filename")]
		string UserEnteredFilename (NSSavePanel panel, string filename, bool confirmed);

		[Export ("panel:willExpand:"), EventArgs ("NSOpenSaveExpanding")]
		void WillExpand (NSSavePanel panel, bool expanding);

		[Export ("panelSelectionDidChange:"), EventArgs ("NSOpenSaveSelectionChanged")]
		void SelectionDidChange (NSSavePanel panel);

		[Obsolete ("On 10.6 and newer use ValidateUrlError")]
		[Export ("panel:isValidFilename:"), DelegateName ("NSOpenSaveFilename"), DefaultValue (true)]
		bool IsValidFilename (NSSavePanel panel, string fileName);

		[Obsolete ("On 10.6 and newer Use DidChangeToDirectoryUrl instead")]
		[Export ("panel:directoryDidChange:"), EventArgs ("NSOpenSaveFilename")]
		void DirectoryDidChange (NSSavePanel panel, string path);

		[Obsolete ("After 10.6, this method is obsolete and does not control sorting order")]
		[Export ("panel:compareFilename:with:caseSensitive"), DelegateName ("NSOpenSaveCompare"), DefaultValue (NSComparisonResult.Same)]
		NSComparisonResult CompareFilenames (NSSavePanel panel, string name1, string name2, bool caseSensitive);

		[Obsolete ("On 10.6 and newer use ShouldEnableUrl")]
		[Export ("panel:shouldShowFilename:"), DelegateName ("NSOpenSaveFilename"), DefaultValue (true)]
		bool ShouldShowFilename (NSSavePanel panel, string filename);
	}

	
	[BaseType (typeof (NSTableView))]
	public interface NSOutlineView {
		[Export ("outlineTableColumn")]
		NSTableColumn OutlineTableColumn { get; set; }

		[Export ("isExpandable:")]
		bool IsExpandable (NSObject item);

		[Export ("expandItem:expandChildren:")]
		void ExpandItem (NSObject item, bool expandChildren);

		[Export ("expandItem:")]
		void ExpandItem (NSObject item);

		[Export ("collapseItem:collapseChildren:")]
		void CollapseItem (NSObject item, bool collapseChildren);

		[Export ("collapseItem:")]
		void CollapseItem (NSObject item);

		[Export ("reloadItem:reloadChildren:")]
		void ReloadItem (NSObject item, bool reloadChildren);

		[Export ("reloadItem:")]
		void ReloadItem (NSObject item);

		[Export ("parentForItem:")]
		NSObject GetParent (NSObject item);

		[Export ("itemAtRow:")]
		NSObject ItemAtRow (int row);

		[Export ("rowForItem:")]
		int RowForItem (NSObject item);

		[Export ("levelForItem:")]
		int LevelForItem (NSObject item);

		[Export ("levelForRow:")]
		int LevelForRow (int row);

		[Export ("isItemExpanded:")]
		bool IsItemExpanded (NSObject item);

		[Export ("indentationPerLevel")]
		float IndentationPerLevel { get; set; }

		[Export ("indentationMarkerFollowsCell")]
		bool IndentationMarkerFollowsCell { get; set; }

		[Export ("autoresizesOutlineColumn")]
		bool AutoresizesOutlineColumn { get; set; }

		[Export ("frameOfOutlineCellAtRow:")]
		RectangleF FrameOfOutlineCellAtRow (int row);

		[Export ("setDropItem:dropChildIndex:")]
		void SetDropItem (NSObject item, int index);

		[Export ("shouldCollapseAutoExpandedItemsForDeposited:")]
		bool ShouldCollapseAutoExpandedItems (bool forDeposited);

		[Export ("autosaveExpandedItems")]
		bool AutosaveExpandedItems { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate  { get; set; }

		[Wrap ("WeakDelegate")]
		NSOutlineViewDelegate Delegate  { get; set; }

		[Export ("dataSource")]
		NSObject WeakDataSource  { get; set; }

		[Wrap ("WeakDataSource")]
		NSOutlineViewDataSource DataSource  { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSOutlineViewDelegate {
		[Export ("outlineView:willDisplayCell:forTableColumn:item:")]
		void WillDisplayCell (NSOutlineView outlineView, NSObject cell, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:shouldEditTableColumn:item:")] [DefaultValue (false)]
		bool ShouldEditTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("selectionShouldChangeInOutlineView:")] [DefaultValue (false)]
		bool SelectionShouldChange (NSOutlineView outlineView);
	
		[Export ("outlineView:shouldSelectItem:")] [DefaultValue (true)]
		bool ShouldSelectItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:selectionIndexesForProposedSelection:")]
		NSIndexSet GetSelectionIndexes (NSOutlineView outlineView, NSIndexSet proposedSelectionIndexes);
	
		[Export ("outlineView:shouldSelectTableColumn:")]
		bool ShouldSelectTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn);
	
		[Export ("outlineView:mouseDownInHeaderOfTableColumn:")]
		void MouseDown (NSOutlineView outlineView, NSTableColumn tableColumn);
	
		[Export ("outlineView:didClickTableColumn:")]
		void DidClickTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn);
	
		[Export ("outlineView:didDragTableColumn:")]
		void DidDragTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn);
		
		//FIXME: Binding NSRectPointer	
		//[Export ("outlineView:toolTipForCell:rect:tableColumn:item:mouseLocation:")]
		//string ToolTipForCell (NSOutlineView outlineView, NSCell cell, NSRectPointer rect, NSTableColumn tableColumn, NSObject item, PointF mouseLocation);
	
		[Export ("outlineView:heightOfRowByItem:")]
		float GetRowHeight (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:typeSelectStringForTableColumn:item:")]
		string GetSelectString (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:nextTypeSelectMatchFromItem:toItem:forString:")]
		NSObject GetNextTypeSelectMatch (NSOutlineView outlineView, NSObject startItem, NSObject endItem, string searchString);
	
		[Export ("outlineView:shouldTypeSelectForEvent:withCurrentSearchString:")]
		bool ShouldTypeSelect (NSOutlineView outlineView, NSEvent theEvent, string searchString);
	
		[Export ("outlineView:shouldShowCellExpansionForTableColumn:item:")]
		bool ShouldShowCellExpansion (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:shouldTrackCell:forTableColumn:item:")]
		bool ShouldTrackCell (NSOutlineView outlineView, NSCell cell, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:dataCellForTableColumn:item:")]
		NSCell GetCell (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:isGroupItem:")]
		bool IsGroupItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:shouldExpandItem:")]
		bool ShouldExpandItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:shouldCollapseItem:")]
		bool ShouldCollapseItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:willDisplayOutlineCell:forTableColumn:item:")]
		void WillDisplayOutlineCell (NSOutlineView outlineView, NSObject cell, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:sizeToFitWidthOfColumn:")]
		float GetSizeToFitColumnWidth (NSOutlineView outlineView, int column);
	
		[Export ("outlineView:shouldReorderColumn:toColumn:")]
		bool ShouldReorder (NSOutlineView outlineView, int columnIndex, int newColumnIndex);
	
		[Export ("outlineView:shouldShowOutlineCellForItem:")]
		bool ShouldShowOutlineCell (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineViewColumnDidMove:")]
		void ColumnDidMove (NSNotification notification);
	
		[Export ("outlineViewColumnDidResize:")]
		void ColumnDidResize (NSNotification notification);
	
		[Export ("outlineViewSelectionIsChanging:")]
		void SelectionIsChanging (NSNotification notification);
	
		[Export ("outlineViewItemWillExpand:")]
		void ItemWillExpand (NSNotification notification);
	
		[Export ("outlineViewItemDidExpand:")]
		void ItemDidExpand (NSNotification notification);
	
		[Export ("outlineViewItemWillCollapse:")]
		void ItemWillCollapse (NSNotification notification);
	
		[Export ("outlineViewItemDidCollapse:")]
		void ItemDidCollapse (NSNotification notification);

		[Export ("outlineViewSelectionDidChange:")]
		void SelectionDidChange (NSNotification notification);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSOutlineViewDataSource {
		[Export ("outlineView:child:ofItem:")]
		NSObject GetChild (NSOutlineView outlineView, int childIndex, NSObject ofItem);
	
		[Export ("outlineView:isItemExpandable:")]
		bool ItemExpandable (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:numberOfChildrenOfItem:")]
		int GetChildrenCount (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:objectValueForTableColumn:byItem:")]
		NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn forTableColumn, NSObject byItem);
	
		[Export ("outlineView:setObjectValue:forTableColumn:byItem:")]
		void SetObjectValue (NSOutlineView outlineView, NSObject theObject, NSTableColumn tableColumn, NSObject item);
	
		[Export ("outlineView:itemForPersistentObject:")]
		NSObject ItemForPersistentObject (NSOutlineView outlineView, NSObject theObject);
	
		[Export ("outlineView:persistentObjectForItem:")]
		NSObject PersistentObjectForItem (NSOutlineView outlineView, NSObject item);
	
		[Export ("outlineView:sortDescriptorsDidChange:")]
		void SortDescriptorsChanged (NSOutlineView outlineView, NSSortDescriptor [] oldDescriptors);
	
		[Export ("outlineView:writeItems:toPasteboard:")]
		bool OutlineViewwriteItemstoPasteboard (NSOutlineView outlineView, NSArray items, NSPasteboard pboard);
	
		[Export ("outlineView:validateDrop:proposedItem:proposedChildIndex:")]
		NSDragOperation ValidateDrop (NSOutlineView outlineView, NSDraggingInfo info, NSObject item, int index);
	
		[Export ("outlineView:acceptDrop:item:childIndex:")]
		bool AcceptDrop (NSOutlineView outlineView, NSDraggingInfo info, NSObject item, int index);
	
		[Export ("outlineView:namesOfPromisedFilesDroppedAtDestination:forDraggedItems:")]
		string [] FilesDropped (NSOutlineView outlineView, NSUrl dropDestination, NSArray items);
	}
	

	[BaseType (typeof (NSObject))]
	public interface NSHelpManager {
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

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSImageDelegate)})]
	public interface NSImage {
		[Static]
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
		void Draw (PointF point, RectangleF fromRect, NSCompositingOperation op, float delta);

		[Export ("drawInRect:fromRect:operation:fraction:")]
		void Draw (RectangleF rect, RectangleF fromRect, NSCompositingOperation op, float delta);

		[Export ("drawInRect:fromRect:operation:fraction:respectFlipped:hints:")]
		void Draw (RectangleF dstSpacePortionRect, RectangleF srcSpacePortionRect, NSCompositingOperation op, float requestedAlpha, bool respectContextIsFlipped, NSDictionary hints);

		[Export ("drawRepresentation:inRect:")]
		bool Draw (NSImageRep imageRep, RectangleF rect);

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
		
		[Static]
		[Export ("imageTypes")]
		string [] ImageTypes { get; }

		[Static]
		[Export ("imageUnfilteredTypes")]
		string [] ImageUnfilteredTypes { get; }
		
		[Static]
		[Export ("canInitWithPasteboard:")]
		bool CanInitWithPasteboard (NSPasteboard pasteboard);

		[Export ("cancelIncrementalLoad")]
		void CancelIncrementalLoad ();

		[Export ("accessibilityDescription")]
		string AccessibilityDescription	 { get; set; }

		[Export ("initWithCGImage:size:")]
		IntPtr Constructor (CGImage cgImage, SizeF size);

		[Export ("CGImageForProposedRect:context:hints:")]
		CGImage AsCGImage (RectangleF proposedDestRect, [NullAllowed] NSGraphicsContext referenceContext, [NullAllowed] NSDictionary hints);

		[Export ("bestRepresentationForRect:context:hints:")]
		NSImageRep BestRepresentation (RectangleF rect, [NullAllowed] NSGraphicsContext referenceContext, [NullAllowed] NSDictionary hints);

		[Export ("hitTestRect:withImageDestinationRect:context:hints:flipped:")]
		bool HitTestRect (RectangleF testRectDestSpace, RectangleF imageRectDestSpace, NSGraphicsContext context, NSDictionary hints, bool flipped);

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

		[Bind ("sizeWithAttributes:")]
		SizeF StringSize ([Target] string str, NSDictionary attributes);

		[Bind ("drawInRect:withAttributes:")]
		void DrawInRect ([Target] string str, RectangleF rect, NSDictionary attributes);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSImageDelegate {
		[Export ("imageDidNotDraw:inRect:"), DelegateName ("NSImageRect"), DefaultValue (null)]
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

	[BaseType (typeof (NSCell))]
	public interface NSImageCell {
		//Detected properties
		[Export ("imageAlignment")]
		NSImageAlignment ImageAlignment { get; set; }

		[Export ("imageScaling")]
		NSImageScale ImageScaling { get; set; }

		[Export ("imageFrameStyle")]
		NSImageFrameStyle ImageFrameStyle { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSImageRep {
		[Export ("draw")]
		bool Draw ();

		[Export ("drawAtPoint:")]
		bool DrawAtPoint (PointF point);

		[Export ("drawInRect:")]
		bool DrawInRect (RectangleF rect);

		[Export ("drawInRect:fromRect:operation:fraction:respectFlipped:hints:")]
		bool DrawInRect (RectangleF dstSpacePortionRect, RectangleF srcSpacePortionRect, NSCompositingOperation op, float requestedAlpha, bool respectContextIsFlipped, NSDictionary hints);

		[Export ("setAlpha:")]
		void SetAlpha (bool alpha);

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
		CGImage AsCGImage (RectangleF proposedDestRect, [NullAllowed] NSGraphicsContext context, [NullAllowed] NSDictionary hints);

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

	[BaseType (typeof (NSControl))]
	public interface NSImageView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		//Detected properties
		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("imageAlignment")]
		NSImageAlignment ImageAlignment { get; set; }

		[Export ("imageScaling")]
		NSImageScale ImageScaling { get; set; }

		[Export ("imageFrameStyle")]
		NSImageFrameStyle ImageFrameStyle { get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("animates")]
		bool Animates { get; set; }

		[Export ("allowsCutCopyPaste")]
		bool AllowsCutCopyPaste { get; set; }
	}

	[BaseType (typeof (NSControl), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSMatrixDelegate)})]
	public interface NSMatrix {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("initWithFrame:mode:prototype:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (RectangleF frameRect, NSMatrixMode aMode, NSCell aCell, int rowsHigh, int colsWide);

		[Export ("initWithFrame:mode:cellClass:numberOfRows:numberOfColumns:")]
		IntPtr Constructor (RectangleF frameRect, NSMatrixMode aMode, Class factoryId, int rowsHigh, int colsWide);

		[Export ("makeCellAtRow:column:")]
		NSCell MakeCellAtRowcolumn (int row, int col);

		[Export ("sendAction:to:forAllCells:")]
		void SendAction (Selector aSelector, NSObject anObject, bool forAllCells);

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
		void HighlightCellatRowColumn (bool highlight, int row, int col);

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
		void Changed (NSNotification notification);

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

	[BaseType (typeof (NSControl))]
	public interface NSLevelIndicator {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("warningValue")]
		double WarningValue { get; set; }

		[Export ("criticalValue")]
		double CriticalValue { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("numberOfTickMarks")]
		int TickMarkCount { get; set; }

		[Export ("numberOfMajorTickMarks")]
		int MajorTickMarkCount { get; set; }

		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValueAt (int index);

		[Export ("rectOfTickMarkAtIndex:")]
		RectangleF RectOfTickMark (int index);
	}

	[BaseType (typeof (NSActionCell))]
	public interface NSLevelIndicatorCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("initWithLevelIndicatorStyle:")]
		IntPtr Constructor (NSLevelIndicatorStyle levelIndicatorStyle);

		[Export ("levelIndicatorStyle")]
		NSLevelIndicatorStyle LevelIndicatorStyle { get; set; }

		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("warningValue")]
		double WarningValue { get; set; }

		[Export ("criticalValue")]
		double CriticalValue { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("numberOfTickMarks")]
		int TickMarkCount { get; set; }

		[Export ("numberOfMajorTickMarks")]
		int MajorTickMarkCount { get; set; }

		[Export ("rectOfTickMarkAtIndex:")]
		RectangleF RectOfTickMarkAt (int index);

		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValueAt (int index);

		[Export ("setImage:")]
		void SetImage (NSImage image);
	}

	[BaseType (typeof (NSObject))]
	public interface NSLayoutManager {
		[Export ("attributedString")]
		NSAttributedString AttributedString { get; }

		[Export ("replaceTextStorage:")]
		void ReplaceTextStorage (NSTextStorage newTextStorage);

		[Export ("textContainers")]
		NSTextContainer [] TextContainers { get; }

		[Export ("addTextContainer:")]
		void AddTextContainer (NSTextContainer container);

		[Export ("insertTextContainer:atIndex:")]
		void InsertTextContainer (NSTextContainer container, int index);

		[Export ("removeTextContainerAtIndex:")]
		void RemoveTextContainer (int index);

		[Export ("textContainerChangedGeometry:")]
		void TextContainerChangedGeometry (NSTextContainer container);

		[Export ("textContainerChangedTextView:")]
		void TextContainerChangedTextView (NSTextContainer container);

		[Export ("layoutOptions")]
		NSGlyphStorageOptions LayoutOptions { get; }

		[Export ("hasNonContiguousLayout")]
		bool HasNonContiguousLayout { get; }

		//[Export ("invalidateGlyphsForCharacterRange:changeInLength:actualCharacterRange:")]
		//void InvalidateGlyphs (NSRange charRange, int changeInLength, NSRangePointer actualCharRange);

		//[Export ("invalidateLayoutForCharacterRange:actualCharacterRange:")]
		//void InvalidateLayout (NSRange charRange, NSRangePointer actualCharRange);

		//[Export ("invalidateLayoutForCharacterRange:isSoft:actualCharacterRange:")]
		//void InvalidateLayout (NSRange charRange, bool isSoft, NSRangePointer actualCharRange);

		[Export ("invalidateDisplayForCharacterRange:")]
		void InvalidateDisplayForCharacterRange (NSRange charRange);

		[Export ("invalidateDisplayForGlyphRange:")]
		void InvalidateDisplayForGlyphRange (NSRange glyphRange);

		[Export ("textStorage:edited:range:changeInLength:invalidatedRange:")]
		void TextStorageEdited (NSTextStorage str, NSTextStorageEditedFlags editedMask, NSRange newCharRange, int changeInLength, NSRange invalidatedCharRange);

		[Export ("ensureGlyphsForCharacterRange:")]
		void EnsureGlyphsForCharacterRange (NSRange charRange);

		[Export ("ensureGlyphsForGlyphRange:")]
		void EnsureGlyphsForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForCharacterRange:")]
		void EnsureLayoutForCharacterRange (NSRange charRange);

		[Export ("ensureLayoutForGlyphRange:")]
		void EnsureLayoutForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForTextContainer:")]
		void EnsureLayoutForTextContainer (NSTextContainer container);

		[Export ("ensureLayoutForBoundingRect:inTextContainer:")]
		void EnsureLayoutForBoundingRect (RectangleF bounds, NSTextContainer container);

		//[Export ("insertGlyphs:length:forStartingGlyphAtIndex:characterIndex:")]
		//void InsertGlyphs (uint [] glyphs, int length, int glyphIndex, int charIndex);

		[Export ("insertGlyph:atGlyphIndex:characterIndex:")]
		void InsertGlyph (uint glyph, int glyphIndex, int charIndex);

		[Export ("replaceGlyphAtIndex:withGlyph:")]
		void ReplaceGlyphAtIndex (int glyphIndex, uint newGlyph);

		[Export ("deleteGlyphsInRange:")]
		void DeleteGlyphs (NSRange glyphRange);

		[Export ("setCharacterIndex:forGlyphAtIndex:")]
		void SetCharacterIndex (int charIndex, int glyphIndex);

		[Export ("setIntAttribute:value:forGlyphAtIndex:")]
		void SetIntAttribute (int attributeTag, int value, int glyphIndex);

		[Export ("invalidateGlyphsOnLayoutInvalidationForGlyphRange:")]
		void InvalidateGlyphsOnLayoutInvalidation (NSRange glyphRange);

		[Export ("numberOfGlyphs")]
		int NumberOfGlyphs { get; }

		[Export ("glyphAtIndex:isValidIndex:")]
		uint GlyphAtIndexisValidIndex (int glyphIndex, bool isValidIndex);

		[Export ("glyphAtIndex:")]
		uint GlyphCount (int glyphIndex);

		[Export ("isValidGlyphIndex:")]
		bool IsValidGlyphIndex (int glyphIndex);

		[Export ("characterIndexForGlyphAtIndex:")]
		uint CharacterIndexForGlyphAtIndex (int glyphIndex);

		[Export ("glyphIndexForCharacterAtIndex:")]
		uint GlyphIndexForCharacterAtIndex (int charIndex);

		[Export ("intAttribute:forGlyphAtIndex:")]
		int IntAttributeforGlyphAtIndex (int attributeTag, int glyphIndex);

		// TODO: bind this with a safe version
		[Export ("getGlyphsInRange:glyphs:characterIndexes:glyphInscriptions:elasticBits:"), Internal]
		int GetGlyphs (NSRange glyphRange, IntPtr glyphBuffer, uint charIndexBuffer, NSGlyphInscription inscribeBuffer, bool elasticBuffer);

		// TODO: bind this with a safe version
		[Internal, Export ("getGlyphsInRange:glyphs:characterIndexes:glyphInscriptions:elasticBits:bidiLevels:")]
		int GetGlyphs (NSRange glyphRange, IntPtr glyphBuffer, uint charIndexBuffer, NSGlyphInscription inscribeBuffer, bool elasticBuffer, ushort bidiLevelBuffer);

		// TODO: bidn this with a safe version
		[Internal, Export ("getGlyphs:range:")]
		uint GetGlyphsrange (IntPtr glyphArray, NSRange glyphRange);

		[Export ("setTextContainer:forGlyphRange:")]
		void SetTextContainerForRange (NSTextContainer container, NSRange glyphRange);

		[Export ("setLineFragmentRect:forGlyphRange:usedRect:")]
		void SetLineFragmentRect (RectangleF fragmentRect, NSRange glyphRange, RectangleF usedRect);

		[Export ("setExtraLineFragmentRect:usedRect:textContainer:")]
		void SetExtraLineFragmentRect (RectangleF fragmentRect, RectangleF usedRect, NSTextContainer container);

		[Export ("setLocation:forStartOfGlyphRange:")]
		void SetLocation (PointF location, NSRange forStartOfGlyphRange);

		//[Export ("setLocations:startingGlyphIndexes:count:forGlyphRange:")]
		//void SetLocations (NSPointArray locations, int glyphIndexes, uint count, NSRange glyphRange);

		[Export ("setNotShownAttribute:forGlyphAtIndex:")]
		void SetNotShownAttribute (bool flag, int glyphIndex);

		[Export ("setDrawsOutsideLineFragment:forGlyphAtIndex:")]
		void SetDrawsOutsideLineFragment (bool flag, int glyphIndex);

		[Export ("setAttachmentSize:forGlyphRange:")]
		void SetAttachmentSize (SizeF attachmentSize, NSRange glyphRange);

		[Export ("getFirstUnlaidCharacterIndex:glyphIndex:")]
		void GetFirstUnlaidCharacterIndex (int charIndex, int glyphIndex);

		[Export ("firstUnlaidCharacterIndex")]
		int FirstUnlaidCharacterIndex { get; }

		[Export ("firstUnlaidGlyphIndex")]
		int FirstUnlaidGlyphIndex { get; }

		//[Export ("textContainerForGlyphAtIndex:effectiveRange:")]
		//NSTextContainer TextContainerForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("usedRectForTextContainer:")]
		RectangleF GetUsedRectForTextContainer (NSTextContainer container);

		//[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:")]
		//RectangleF LineFragmentRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:")]
		//RectangleF LineFragmentUsedRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		//RectangleF LineFragmentRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		//[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		//RectangleF LineFragmentUsedRectForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		//[Export ("textContainerForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		//NSTextContainer TextContainerForGlyphAt (int glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		[Export ("extraLineFragmentRect")]
		RectangleF ExtraLineFragmentRect { get; }

		[Export ("extraLineFragmentUsedRect")]
		RectangleF ExtraLineFragmentUsedRect { get; }

		[Export ("extraLineFragmentTextContainer")]
		NSTextContainer ExtraLineFragmentTextContainer { get; }

		[Export ("locationForGlyphAtIndex:")]
		PointF LocationForGlyphAtIndex (int glyphIndex);

		[Export ("notShownAttributeForGlyphAtIndex:")]
		bool NotShownAttributeForGlyphAtIndex (int glyphIndex);

		[Export ("drawsOutsideLineFragmentForGlyphAtIndex:")]
		bool DrawsOutsideLineFragmentForGlyphAt (int glyphIndex);

		[Export ("attachmentSizeForGlyphAtIndex:")]
		SizeF AttachmentSizeForGlyphAt (int glyphIndex);

		[Export ("setLayoutRect:forTextBlock:glyphRange:")]
		void SetLayoutRect (RectangleF layoutRect, NSTextBlock forTextBlock, NSRange glyphRange);

		[Export ("setBoundsRect:forTextBlock:glyphRange:")]
		void SetBoundsRect (RectangleF boundsRect, NSTextBlock forTextBlock, NSRange glyphRange);

		[Export ("layoutRectForTextBlock:glyphRange:")]
		RectangleF LayoutRect (NSTextBlock block, NSRange glyphRange);

		[Export ("boundsRectForTextBlock:glyphRange:")]
		RectangleF BoundsRect (NSTextBlock block, NSRange glyphRange);

		//[Export ("layoutRectForTextBlock:atIndex:effectiveRange:")]
		//RectangleF LayoutRect (NSTextBlock block, int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("boundsRectForTextBlock:atIndex:effectiveRange:")]
		//RectangleF BoundsRect (NSTextBlock block, int glyphIndex, NSRangePointer effectiveGlyphRange);

		//[Export ("glyphRangeForCharacterRange:actualCharacterRange:")]
		//NSRange GetGlyphRange (NSRange charRange, NSRangePointer actualCharRange);

		//[Export ("characterRangeForGlyphRange:actualGlyphRange:")]
		//NSRange GetCharacterRange (NSRange glyphRange, NSRangePointer actualGlyphRange);

		[Export ("glyphRangeForTextContainer:")]
		NSRange GetGlyphRange (NSTextContainer container);

		[Export ("rangeOfNominallySpacedGlyphsContainingIndex:")]
		NSRange RangeOfNominallySpacedGlyphsContainingIndex (int glyphIndex);

		//[Export ("rectArrayForCharacterRange:withinSelectedCharacterRange:inTextContainer:rectCount:")]
		//NSRectArray RectArrayForCharacterRangewithinSelectedCharacterRangeinTextContainerrectCount (NSRange charRange, NSRange selCharRange, NSTextContainer container, uint rectCount);

		//[Export ("rectArrayForGlyphRange:withinSelectedGlyphRange:inTextContainer:rectCount:")]
		//NSRectArray RectArrayForGlyphRangewithinSelectedGlyphRangeinTextContainerrectCount (NSRange glyphRange, NSRange selGlyphRange, NSTextContainer container, uint rectCount);

		[Export ("boundingRectForGlyphRange:inTextContainer:")]
		RectangleF BoundingRectForGlyphRange (NSRange glyphRange, NSTextContainer container);

		[Export ("glyphRangeForBoundingRect:inTextContainer:")]
		NSRange GlyphRangeForBoundingRect (RectangleF bounds, NSTextContainer container);

		[Export ("glyphRangeForBoundingRectWithoutAdditionalLayout:inTextContainer:")]
		NSRange GlyphRangeForBoundingRectWithoutAdditionalLayout (RectangleF bounds, NSTextContainer container);

		[Export ("glyphIndexForPoint:inTextContainer:fractionOfDistanceThroughGlyph:")]
		uint GlyphIndexForPointInTextContainer (PointF point, NSTextContainer container, float fractionOfDistanceThroughGlyph);

		[Export ("glyphIndexForPoint:inTextContainer:")]
		uint GlyphIndexForPoint (PointF point, NSTextContainer container);

		[Export ("fractionOfDistanceThroughGlyphForPoint:inTextContainer:")]
		float FractionOfDistanceThroughGlyphForPoint (PointF point, NSTextContainer container);

		[Export ("characterIndexForPoint:inTextContainer:fractionOfDistanceBetweenInsertionPoints:")]
		uint CharacterIndexForPoint (PointF point, NSTextContainer container, float fractionOfDistanceBetweenInsertionPoints);

		[Export ("getLineFragmentInsertionPointsForCharacterAtIndex:alternatePositions:inDisplayOrder:positions:characterIndexes:")]
		uint GetLineFragmentInsertionPoints (int charIndex, bool aFlag, bool dFlag, float positions, uint charIndexes);

		//[Export ("temporaryAttributesAtCharacterIndex:effectiveRange:")]
		//NSDictionary GetTemporaryAttributes (int charIndex, NSRangePointer effectiveCharRange);

		[Export ("setTemporaryAttributes:forCharacterRange:")]
		void SetTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[Export ("addTemporaryAttributes:forCharacterRange:")]
		void AddTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[Export ("removeTemporaryAttribute:forCharacterRange:")]
		void RemoveTemporaryAttribute (string attrName, NSRange charRange);

		//[Export ("temporaryAttribute:atCharacterIndex:effectiveRange:")]
		//NSObject GetTemporaryAttribute (string attrName, uint location, NSRangePointer range);

		//[Export ("temporaryAttribute:atCharacterIndex:longestEffectiveRange:inRange:")]
		//NSObject GetTemporaryAttribute (string attrName, uint location, NSRangePointer range, NSRange rangeLimit);

		//[Export ("temporaryAttributesAtCharacterIndex:longestEffectiveRange:inRange:")]
		//NSDictionary GetTemporaryAttributes (int characterIndex, NSRangePointer longestEffectiveRange, NSRange rangeLimit);

		[Export ("addTemporaryAttribute:value:forCharacterRange:")]
		void AddTemporaryAttribute (string attrName, NSObject value, NSRange charRange);

		[Export ("substituteFontForFont:")]
		NSFont SubstituteFontForFont (NSFont originalFont);

		[Export ("defaultLineHeightForFont:")]
		float DefaultLineHeightForFont (NSFont theFont);

		[Export ("defaultBaselineOffsetForFont:")]
		float DefaultBaselineOffsetForFont (NSFont theFont);

		//Detected properties
		[Export ("textStorage")]
		NSTextStorage TextStorage { get; set; }

		[Export ("glyphGenerator")]
		NSGlyphGenerator GlyphGenerator { get; set; }

		[Export ("typesetter")]
		NSTypesetter Typesetter { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSLayoutManagerDelegate Delegate { get; set; }

		[Export ("backgroundLayoutEnabled")]
		bool BackgroundLayoutEnabled { get; set; }

		[Export ("usesScreenFonts")]
		bool UsesScreenFonts { get; set; }

		[Export ("showsInvisibleCharacters")]
		bool ShowsInvisibleCharacters { get; set; }

		[Export ("showsControlCharacters")]
		bool ShowsControlCharacters { get; set; }

		[Export ("hyphenationFactor")]
		float HyphenationFactor { get; set; }

		[Export ("defaultAttachmentScaling")]
		NSImageScaling DefaultAttachmentScaling { get; set; }

		[Export ("typesetterBehavior")]
		NSTypesetterBehavior TypesetterBehavior { get; set; }

		[Export ("allowsNonContiguousLayout")]
		bool AllowsNonContiguousLayout { get; set; }

		[Export ("usesFontLeading")]
		bool UsesFontLeading { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSLayoutManagerDelegate {
		[Export ("layoutManagerDidInvalidateLayout:")]
		void LayoutInvalidated (NSLayoutManager sender);

		[Export ("layoutManager:didCompleteLayoutForTextContainer:atEnd:")]
		void LayoutCompleted (NSLayoutManager layoutManager, NSTextContainer textContainer, bool layoutFinishedFlag);

		[Export ("layoutManager:shouldUseTemporaryAttributes:forDrawingToScreen:atCharacterIndex:effectiveRange:")]
		NSDictionary ShouldUseTemporaryAttributes (NSLayoutManager layoutManager, NSDictionary temporaryAttributes, bool drawingToScreen, int charIndex, IntPtr effectiveCharRange);

	}

	[Model]
	[BaseType (typeof (NSObject))]
	public interface NSMatrixDelegate {
		[Export ("control:textShouldBeginEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSControl control, NSText fieldEditor);

		[Export ("control:textShouldEndEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldEndEditing (NSControl control, NSText fieldEditor);

		[Export ("control:didFailToFormatString:errorDescription:"), DelegateName ("NSControlTextError"), DefaultValue (true)]
		bool DidFailToFormatString (NSControl control, string str, string error);
		
		[Export ("control:didFailToValidatePartialString:errorDescription:"), EventArgs ("NSControlTextError")]
		void DidFailToValidatePartialString (NSControl control, string str, string error);
		
		[Export ("control:isValidObject:"), DelegateName ("NSControlTextValidation"), DefaultValue (true)]
		bool IsValidObject (NSControl control, NSObject objectToValidate);

		[Export ("control:textView:doCommandBySelector:"), DelegateName ("NSControlCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSControl control, NSTextView textView, Selector commandSelector);

		[Export ("control:textView:completions:forPartialWordRange:indexOfSelectedItem:"), DelegateName ("NSControlTextCompletion"), DefaultValue (null)]
		string [] GetCompletions (NSControl control, NSTextView textView, string [] words, NSRange charRange, int index);
	}

	[BaseType (typeof (NSObject))]
	public interface NSPageLayout {
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
		void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, [NullAllowed] NSObject del, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithPrintInfo:")]
		int RunModalWithPrintInfo (NSPrintInfo printInfo);

		[Export ("runModal")]
		int RunModal ();

		[Export ("printInfo")]
		NSPrintInfo PrintInfo { get; }
	}

	[BaseType (typeof (NSWindow))]
	public interface NSPanel {
		//Detected properties
		[Export ("floatingPanel")]
		bool FloatingPanel { [Bind ("isFloatingPanel")]get; set; }

		[Export ("becomesKeyOnlyIfNeeded")]
		bool BecomesKeyOnlyIfNeeded { get; set; }

		[Export ("worksWhenModal")]
		bool WorksWhenModal { get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSParagraphStyle {
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
	public interface NSMutableParagraphStyle {
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
	public interface NSPasteboard {
		[Static]
		[Export ("generalPasteboard")]
		NSPasteboard GeneralPasteboard { get; }

		[Static]
		[Export ("pasteboardWithName:")]
		NSPasteboard FromName (string name);

		[Static]
		[Export ("pasteboardWithUniqueName")]
		NSPasteboard CreateWithUniqueName ();

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
		NSObject [] ReadObjectsForClasses (NSPasteboardReading [] classArray, NSDictionary options);

		[Export ("pasteboardItems")]
		NSPasteboardItem [] PasteboardItems { get; }

		[Export ("indexOfPasteboardItem:")]
		int IndexOf (NSPasteboardItem pasteboardItem);

		[Export ("canReadItemWithDataConformingToTypes:")]
		bool CanReadItemWithDataConformingToTypes (string [] utiTypes);

		[Export ("canReadObjectForClasses:options:")]
		bool CanReadObjectForClasses (NSObject [] classArray, NSDictionary options);

		[Export ("declareTypes:owner:")]
		int DeclareTypes (string [] newTypes, [NullAllowed] NSObject newOwner);

		[Export ("addTypes:owner:")]
		int AddTypes (string [] newTypes, [NullAllowed] NSObject newOwner);

		[Export ("types")]
		string [] Types { get; }

		[Export ("availableTypeFromArray:")]
		string GetAvailableTypeFromArray (string [] types);

		[Export ("setData:forType:")]
		bool SetDataForType (NSData data, string dataType);

		[Export ("setPropertyList:forType:")]
		bool SetPropertyListForType (NSObject plist, string dataType);

		[Export ("setString:forType:")]
		bool SetStringForType (string str, string dataType);

		[Export ("dataForType:")]
		NSData GetDataForType (string dataType);

		[Export ("propertyListForType:")]
		NSObject GetPropertyListForType (string dataType);

		[Export ("stringForType:")]
		string GetStringForType (string dataType);

		// Pasteboard data types

		[Field ("NSStringPboardType")]
		NSString NSStringType{ get; }
		
		[Field ("NSFilenamesPboardType")]
		NSString NSFilenamesType{ get; }
		
		[Field ("NSPostScriptPboardType")]
		NSString NSPostScriptType{ get; }
        
		[Field ("NSTIFFPboardType")]
		NSString NSTiffType{ get; }
		
		[Field ("NSRTFPboardType")]
		NSString NSRtfType{ get; }
		
		[Field ("NSTabularTextPboardType")]
		NSString NSTabularTextType{ get; }
		
		[Field ("NSFontPboardType")]
		NSString NSFontType{ get; }
		
		[Field ("NSRulerPboardType")]
		NSString NSRulerType{ get; }
		
		[Field ("NSFileContentsPboardType")]
		NSString NSFileContentsType{ get; }
		
		[Field ("NSColorPboardType")]
		NSString NSColorType{ get; }
		
		[Field ("NSRTFDPboardType")]
		NSString NSRtfdType{ get; }
		
		[Field ("NSHTMLPboardType")]
		NSString NSHtmlType{ get; }
		
		[Field ("NSPICTPboardType")]
		NSString NSPictType{ get; }
		
		[Field ("NSURLPboardType")]
		NSString NSUrlType{ get; }
		
		[Field ("NSPDFPboardType")]
		NSString NSPdfType{ get; }
		
		[Field ("NSVCardPboardType")]
		NSString NSVCardType{ get; }
		
		[Field ("NSFilesPromisePboardType")]
		NSString NSFilesPromiseType{ get; }
		
		[Field ("NSMultipleTextSelectionPboardType")]
		NSString NSMultipleTextSelectionType{ get; }

		// Pasteboard names: for NSPasteboard.FromName()

		[Field ("NSGeneralPboard")]
		NSString NSGeneralPasteboardName { get; }

		[Field ("NSFontPboard")]
		NSString NSFontPasteboardName { get; }

		[Field ("NSRulerPboard")]
		NSString NSRulerPasteboardName { get; }

		[Field ("NSFindPboard")]
		NSString NSFindPasteboardName { get; }

		[Field ("NSDragPboard")]
		NSString NSDragPasteboardName { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPasteboardWriting {
		[Export ("writableTypesForPasteboard:")]
		string [] GetWritableTypesForPasteboard (NSPasteboard pasteboard);

		[Export ("writingOptionsForType:pasteboard:")]
		NSPasteboardWritingOptions GetWritingOptionsForType (string type, NSPasteboard pasteboard);

		[Export ("pasteboardPropertyListForType:")]
		NSObject GetPasteboardPropertyListForType (string type);
	}

	[BaseType (typeof (NSObject))]
	public interface NSPasteboardItem {
		[Export ("types")]
		string [] Types { get; }

		[Export ("availableTypeFromArray:")]
		string GetAvailableTypeFromArray (string [] types);

		[Export ("setDataProvider:forTypes:")]
		bool SetDataProviderForTypes (NSPasteboardItemDataProvider dataProvider, string [] types);

		[Export ("setData:forType:")]
		bool SetDataForType (NSData data, string type);

		[Export ("setString:forType:")]
		bool SetStringForType (string str, string type);

		[Export ("setPropertyList:forType:")]
		bool SetPropertyListForType (NSObject propertyList, string type);

		[Export ("dataForType:")]
		NSData GetDataForType (string type);

		[Export ("stringForType:")]
		string GetStringForType (string type);

		[Export ("propertyListForType:")]
		NSObject GetPropertyListForType (string type);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPasteboardItemDataProvider {
		[Abstract]
		[Export ("pasteboard:item:provideDataForType:")]
		void ProvideDataForType (NSPasteboard pasteboard, NSPasteboardItem item, string type);

		[Abstract]
		[Export ("pasteboardFinishedWithDataProvider:")]
		void FinishedWithDataProvider (NSPasteboard pasteboard);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPasteboardReading {
		[Abstract]
		[Export ("readableTypesForPasteboard:")]
		string [] GetReadableTypesForPasteboard (NSPasteboard pasteboard);

		[Abstract]
		[Export ("readingOptionsForType:pasteboard:")]
		NSPasteboardReadingOptions GetReadingOptionsForType (string type, NSPasteboard pasteboard);

		[Abstract]
		[Export ("initWithPasteboardPropertyList:ofType:")]
		NSObject InitWithPasteboardPropertyList (NSObject propertyList, string type);
	}
	
	[BaseType (typeof (NSActionCell), Events=new Type [] { typeof (NSPathCellDelegate) }, Delegates=new string [] { "WeakDelegate" })]
	public interface NSPathCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("pathStyle")]
		NSPathStyle PathStyle { get; set; }

		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("setObjectValue:")]
		void SetObjectValue (NSObject obj);

		[Export ("allowedTypes")]
		string [] AllowedTypes { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSPathCellDelegate Delegate { get; set; }

		[Static, Export ("pathComponentCellClass")]
		Class PathComponentCellClass { get; }

		[Export ("pathComponentCells")]
		NSPathComponentCell [] PathComponentCells { get; set; }

		[Export ("rectOfPathComponentCell:withFrame:inView:")]
		RectangleF GetRect (NSPathComponentCell componentCell, RectangleF withFrame, NSView inView);

		[Export ("pathComponentCellAtPoint:withFrame:inView:")]
		NSPathComponentCell GetPathComponent (PointF point, RectangleF frame, NSView view);

		[Export ("clickedPathComponentCell")]
		NSPathComponentCell ClickedPathComponentCell { get; }

		[Export ("mouseEntered:withFrame:inView:")]
		void MouseEntered (NSEvent evt, RectangleF frame, NSView view);

		[Export ("mouseExited:withFrame:inView:")]
		void MouseExited (NSEvent evt, RectangleF frame, NSView view);

		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("placeholderString")]
		string PlaceholderString { get; set; }

		[Export ("placeholderAttributedString")]
		NSAttributedString PlaceholderAttributedString { get; set; }

		[Export ("setControlSize:")]
		void SetControlSize (NSControlSize size);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPathCellDelegate {
		[Export ("pathCell:willDisplayOpenPanel:"), EventArgs ("NSPathCellDisplayPanel")]
		void WillDisplayOpenPanel (NSPathCell pathCell, NSOpenPanel openPanel);

		[Export ("pathCell:willPopUpMenu:"), EventArgs ("NSPathCellMenu")]
		void WillPopupMenu (NSPathCell pathCell, NSMenu menu);
	}

	[BaseType (typeof (NSTextFieldCell))]
	public interface NSPathComponentCell {
		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("URL")]
		NSUrl Url { get; set; }
	}


	[BaseType (typeof (NSControl))]
	public interface NSPathControl {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("clickedPathComponentCell")]
		NSPathComponentCell ClickedPathComponentCell { get; }

		[Export ("setDraggingSourceOperationMask:forLocal:")]
		void SetDraggingSource (NSDragOperation operationMask, bool isLocal);

		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("pathStyle")]
		NSPathStyle PathStyle { get; set; }

		[Export ("pathComponentCells")]
		NSPathComponentCell [] PathComponentCells { get; set; }

		[Export ("backgroundColor"), NullAllowed]
		NSColor BackgroundColor { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		[Wrap ("WeakDelegate")]
		NSPathControlDelegate Delegate { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSPathControlDelegate {
		[Abstract]
		[Export ("pathControl:shouldDragPathComponentCell:withPasteboard:")]
		bool ShouldDragPathComponentCell (NSPathControl pathControl, NSPathComponentCell pathComponentCell, NSPasteboard pasteboard);

		[Abstract]
		[Export ("pathControl:validateDrop:")]
		NSDragOperation ValidateDrop (NSPathControl pathControl, NSDraggingInfo info);

		[Abstract]
		[Export ("pathControl:acceptDrop:")]
		bool AcceptDrop (NSPathControl pathControl, NSDraggingInfo info);

		[Abstract]
		[Export ("pathControl:willDisplayOpenPanel:")]
		void WillDisplayOpenPanel (NSPathControl pathControl, NSOpenPanel openPanel);

		[Abstract]
		[Export ("pathControl:willPopUpMenu:")]
		void WillPopUpMenu (NSPathControl pathControl, NSMenu menu);
	}

	[BaseType (typeof (NSButton))]
	public interface NSPopUpButton {
		[Export ("initWithFrame:pullsDown:")]
		IntPtr Constructor (RectangleF buttonFrame, bool pullsDown);

		[Export ("addItemWithTitle:")]
		void AddItem (string title);

		[Export ("addItemsWithTitles:")]
		void AddItems (string [] itemTitles);

		[Export ("insertItemWithTitle:atIndex:")]
		void InsertItem (string title, int index);

		[Export ("removeItemWithTitle:")]
		void RemoveItem (string title);

		[Export ("removeItemAtIndex:")]
		void RemoveItem (int index);

		[Export ("removeAllItems")]
		void RemoveAllItems ();

		[Export ("itemArray")]
		NSMenuItem [] Items ();

		[Export ("numberOfItems")]
		int ItemCount { get; }

		[Export ("indexOfItem:")]
		int IndexOfItem (NSMenuItem item);

		[Export ("indexOfItemWithTitle:")]
		int IndexOfItem (string title);

		[Export ("indexOfItemWithTag:")]
		int IndexOfItem (int tag);

		[Export ("indexOfItemWithRepresentedObject:")]
		int IndexOfItem (NSObject obj);

		[Export ("indexOfItemWithTarget:andAction:")]
		int IndexOfItem (NSObject target, Selector actionSelector);

		[Export ("itemAtIndex:")]
		NSMenuItem ItemAtIndex (int index);

		[Export ("itemWithTitle:")]
		NSMenuItem ItemWithTitle (string title);

		[Export ("lastItem")]
		NSMenuItem LastItem { get; }

		[Export ("selectItem:")]
		void SelectItem (NSMenuItem item);

		[Export ("selectItemAtIndex:")]
		void SelectItem (int index);

		[Export ("selectItemWithTitle:")]
		void SelectItem (string title);

		[Export ("selectItemWithTag:")]
		bool SelectItemWithTag (int tag);

		[Export ("setTitle:")]
		void SetTitle (string aString);

		[Export ("selectedItem")]
		NSMenuItem SelectedItem { get; }

		[Export ("indexOfSelectedItem")]
		int IndexOfSelectedItem { get; }

		[Export ("synchronizeTitleAndSelectedItem")]
		void SynchronizeTitleAndSelectedItem ();

		[Export ("itemTitleAtIndex:")]
		string ItemTitle (int index);

		[Export ("itemTitles")]
		string [] ItemTitles ();

		[Export ("titleOfSelectedItem")]
		string TitleOfSelectedItem { get; }

		//Detected properties
		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("pullsDown")]
		bool PullsDown { get; set; }

		[Export ("autoenablesItems")]
		bool AutoEnablesItems { get; set; }

		[Export ("preferredEdge")]
		NSRectEdge PreferredEdge { get; set; }

	}


	[BaseType (typeof (NSMenuItemCell))]
	public interface NSPopUpButtonCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("initTextCell:pullsDown:")]
		IntPtr Constructor (string stringValue, bool pullDown);

		[Export ("addItemWithTitle:")]
		void AddItem (string title);

		[Export ("addItemsWithTitles:")]
		void AddItems (string [] itemTitles);

		[Export ("insertItemWithTitle:atIndex:")]
		void InsertItem (string title, int index);

		[Export ("removeItemWithTitle:")]
		void RemoveItem (string title);

		[Export ("removeItemAtIndex:")]
		void RemoveItemAt (int index);

		[Export ("removeAllItems")]
		void RemoveAllItems ();

		[Export ("itemArray")]
		NSMenuItem [] Items { get; }

		[Export ("numberOfItems")]
		int Count { get; }

		[Export ("indexOfItem:")]
		int IndexOf (NSMenuItem item);

		[Export ("indexOfItemWithTitle:")]
		int IndexOfItemWithTitle (string title);

		[Export ("indexOfItemWithTag:")]
		int IndexOfItemWithTag (int tag);

		[Export ("indexOfItemWithRepresentedObject:")]
		int IndexOfItemWithRepresentedObject (NSObject obj);

		[Export ("indexOfItemWithTarget:andAction:")]
		int IndexOfItemWithTargetandAction (NSObject target, Selector actionSelector);

		[Export ("itemAtIndex:")]
		NSMenuItem ItemAt (int index);

		[Export ("itemWithTitle:")]
		NSMenuItem ItemWithTitle (string title);

		[Export ("lastItem")]
		NSMenuItem LastItem { get; }

		[Export ("selectItem:")]
		void SelectItem (NSMenuItem item);

		[Export ("selectItemAtIndex:")]
		void SelectItemAt (int index);

		[Export ("selectItemWithTitle:")]
		void SelectItemWithTitle (string title);

		[Export ("selectItemWithTag:")]
		bool SelectItemWithTag (int tag);

		[Export ("setTitle:")]
		void SetTitle (string aString);

		[Export ("selectedItem")]
		NSMenuItem SelectedItem { get; }

		[Export ("indexOfSelectedItem")]
		int SelectedItemIndex { get; }

		[Export ("synchronizeTitleAndSelectedItem")]
		void SynchronizeTitleAndSelectedItem ();

		[Export ("itemTitleAtIndex:")]
		string GetItemTitle (int index);

		[Export ("itemTitles")]
		string [] ItemTitles { get; }

		[Export ("titleOfSelectedItem")]
		string TitleOfSelectedItem { get; }

		[Export ("attachPopUpWithFrame:inView:")]
		void AttachPopUp (RectangleF cellFrame, NSView inView);

		[Export ("dismissPopUp")]
		void DismissPopUp ();

		[Export ("performClickWithFrame:inView:")]
		void PerformClick (RectangleF withFrame, NSView controlView);

		//Detected properties
		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("pullsDown")]
		bool PullsDown { get; set; }

		[Export ("autoenablesItems")]
		bool AutoenablesItems { get; set; }

		[Export ("preferredEdge")]
		NSRectEdge PreferredEdge { get; set; }

		[Export ("usesItemFromMenu")]
		bool UsesItemFromMenu { get; set; }

		[Export ("altersStateOfSelectedItem")]
		bool AltersStateOfSelectedItem { get; set; }

		[Export ("arrowPosition")]
		NSPopUpArrowPosition ArrowPosition { get; set; }

		[Export ("objectValue")]
		NSObject ObjectValue { get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSPrinter {
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
	public interface NSPrintInfo {
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
	public interface NSPrintOperation {
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
	public interface NSPrintPanelAccessorizing {
		[Abstract]
		[Export ("localizedSummaryItems")]
		NSDictionary [] LocalizedSummaryItems ();

		[Abstract]
		[Export ("keyPathsForValuesAffectingPreview")]
		NSSet KeyPathsForValuesAffectingPreview ();
	}

	[BaseType (typeof (NSObject))]
	public interface NSPrintPanel {
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
		void BeginSheet (NSPrintInfo printInfo, NSWindow docWindow, [NullAllowed] NSObject del, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

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

	[BaseType (typeof (NSView))]
	public interface NSProgressIndicator {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("incrementBy:")]
		void IncrementBy (double delta);

		[Export ("startAnimation:")]
		void StartAnimation ([NullAllowed] NSObject sender);

		[Export ("stopAnimation:")]
		void StopAnimation ([NullAllowed] NSObject sender);

		[Export ("style")]
		NSProgressIndicatorStyle Style { get; set; }

		[Export ("sizeToFit")]
		void SizeToFit ();

		[Export ("displayedWhenStopped")]
		bool IsDisplayedWhenStopped { [Bind ("isDisplayedWhenStopped")] get; set; }

		//Detected properties
		[Export ("indeterminate")]
		bool Indeterminate { [Bind ("isIndeterminate")]get; set; }

		[Export ("bezeled")]
		bool Bezeled { [Bind ("isBezeled")]get; set; }

		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }

		[Export ("doubleValue")]
		double DoubleValue { get; set; }

		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("usesThreadedAnimation")]
		bool UsesThreadedAnimation { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSResponder {
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
		[Export ("nextResponder")][NullAllowed]
		NSResponder NextResponder { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }
	}


	[BaseType (typeof (NSObject))]
	public interface NSRulerMarker {
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
	public interface NSRulerView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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

		[Export ("markers"), NullAllowed]
		NSRulerMarker [] Markers { get; set; }

		[Export ("accessoryView")]
		NSView AccessoryView { get; set; }
	}

	public delegate void NSSavePanelComplete (int result);
	
	[BaseType (typeof (NSPanel), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSOpenSavePanelDelegate)})]
	public interface NSSavePanel {
		[Static]
		[Export ("savePanel")]
		NSSavePanel SavePanel { get; }

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("isExpanded")]
		bool IsExpanded { get; }

		[Export ("validateVisibleColumns")]
		void ValidateVisibleColumns ();

		[Export ("ok:")]
		void Ok (NSObject sender);

		[Export ("cancel:")]
		void Cancel (NSObject sender);

		[Export ("beginSheetModalForWindow:completionHandler:")]
		void BeginSheet (NSWindow window, NSSavePanelComplete onComplete);

		[Export ("beginWithCompletionHandler:")]
		void Begin (NSSavePanelComplete onComplete);

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

		[Obsolete ("On 10.6 and newer use Url instead")]
		[Export ("filename")]
		string Filename { get; }

		[Obsolete ("On 10.6 and newer use DirectoryUrl instead")]
		[Export ("directory")]
		string Directory { get; set; }

		[Obsolete ("On 10.6 and newer use AllowedFileTypes instead")]
		[Export ("requiredFileType")]
		string RequiredFileType { get; set; }

		[Obsolete ("On 10.6 and newer use Begin with the callback")]
		[Export ("beginSheetForDirectory:file:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void Begin (string directory, string filename, NSWindow docWindow, NSObject modalDelegate, Selector selector, IntPtr context);

		[Obsolete ("On 10.6 and newer use RunModal without parameters instead")]
		[Export ("runModalForDirectory:file:")]
		int RunModal ([NullAllowed] string directory, [NullAllowed]  string filename);
	}

	[BaseType (typeof (NSObject))]
	public interface NSScreen {
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
	public interface NSScroller {
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
	public interface NSScrollView {
		[Static]
		[Export ("frameSizeForContentSize:hasHorizontalScroller:hasVerticalScroller:borderType:")]
		SizeF FrameSizeForContentSize (SizeF cSize, bool hFlag, bool vFlag, NSBorderType aType);

		[Static]
		[Export ("contentSizeForFrameSize:hasHorizontalScroller:hasVerticalScroller:borderType:")]
		SizeF ContentSizeForFrame (SizeF fSize, bool hFlag, bool vFlag, NSBorderType aType);

		[Export ("documentVisibleRect")]
		RectangleF DocumentVisibleRect { get; }

		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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
		
		[Export ("hasVerticalRuler")]
		bool HasVerticalRuler { get; set; }

		[Export ("hasHorizontalRuler")]
		bool HasHorizontalRuler { get; set; }
		
		[Export ("rulersVisible")]
        bool RulersVisible { get; set; }
       
        [Export ("horizontalRulerView")]
        NSRulerView HorizontalRulerView { get; set; }
   
        [Export ("verticalRulerView")]
        NSRulerView VerticalRulerView { get; set; }    
	}

	[BaseType (typeof (NSTextField))]
	public interface NSSearchField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("recentSearches")]
		string [] RecentSearches { get; set; }

		[Export ("recentsAutosaveName")]
		string RecentsAutosaveName { get; set; }

		[New, Export ("cell")]
		NSSearchFieldCell Cell { get; set; }
	}

	[BaseType (typeof (NSTextFieldCell))]
	public interface NSSearchFieldCell {
		[Export ("searchButtonCell")]
		NSButtonCell SearchButtonCell { get; set; }

		[Export ("cancelButtonCell")]
		NSButtonCell CancelButtonCell { get; set; }

		[Export ("resetSearchButtonCell")]
		void ResetSearchButtonCell ();

		[Export ("resetCancelButtonCell")]
		void ResetCancelButtonCell ();

		[Export ("searchTextRectForBounds:")]
		RectangleF SearchTextRectForBounds (RectangleF rect);

		[Export ("searchButtonRectForBounds:")]
		RectangleF SearchButtonRectForBounds (RectangleF rect);

		[Export ("cancelButtonRectForBounds:")]
		RectangleF CancelButtonRectForBounds (RectangleF rect);

		[Export ("searchMenuTemplate")]
		NSMenu SearchMenuTemplate { get; set; }

		[Export ("sendsWholeSearchString")]
		bool SendsWholeSearchString { get; set; }

		[Export ("maximumRecents")]
		int MaximumRecents { get; set; }

		[Export ("recentSearches")]
		string [] RecentSearches { get; set; }

		[Export ("recentsAutosaveName")]
		string RecentsAutosaveName { get; set; }

		[Export ("sendsSearchStringImmediately")]
		bool SendsSearchStringImmediately { get; set; }
	}
	
	[BaseType (typeof (NSControl))]
	public interface NSSegmentedControl {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("selectSegmentWithTag:")]
		bool SelectSegment (int tag);

		[Export ("setWidth:forSegment:")]
		void SetWidth (float width, int segment);

		[Export ("widthForSegment:")]
		float GetWidth (int segment);

		[Export ("setImage:forSegment:")]
		void SetImage (NSImage image, int segment);

		[Export ("imageForSegment:")]
		NSImage GetImage (int segment);

		[Export ("setImageScaling:forSegment:")]
		void SetImageScaling (NSImageScaling scaling, int segment);

		[Export ("imageScalingForSegment:")]
		NSImageScaling GetImageScaling (int segment);

		[Export ("setLabel:forSegment:")]
		void SetLabel (string label, int segment);

		[Export ("labelForSegment:")]
		string GetLabel (int segment);

		[Export ("setMenu:forSegment:")]
		void SetMenu (NSMenu menu, int segment);

		[Export ("menuForSegment:")]
		NSMenu GetMenu (int segment);

		[Export ("setSelected:forSegment:")]
		void SetSelected (bool selected, int segment);

		[Export ("isSelectedForSegment:")]
		bool IsSelectedForSegment (int segment);

		[Export ("setEnabled:forSegment:")]
		void SetEnabled (bool enabled, int segment);

		[Export ("isEnabledForSegment:")]
		bool IsEnabled (int segment);

		//Detected properties
		[Export ("segmentCount")]
		int SegmentCount { get; set; }

		[Export ("selectedSegment")]
		int SelectedSegment { get; set; }

		[Export ("segmentStyle")]
		NSSegmentStyle SegmentStyle { get; set; }

	}
	
	[BaseType (typeof (NSActionCell))]
	public interface NSSegmentedCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("selectSegmentWithTag:")]
		bool SelectSegment (int tag);

		[Export ("makeNextSegmentKey")]
		void InsertSegmentAfterSelection ();

		[Export ("makePreviousSegmentKey")]
		void InsertSegmentBeforeSelection ();

		[Export ("setWidth:forSegment:")]
		void SetWidth (float width, int forSegment);

		[Export ("widthForSegment:")]
		float GetWidth (int forSegment);

		[Export ("setImage:forSegment:")]
		void SetImage (NSImage image, int forSegment);

		[Export ("imageForSegment:")]
		NSImage GetImageForSegment (int forSegment);

		[Export ("setImageScaling:forSegment:")]
		void SetImageScaling (NSImageScaling scaling, int forSegment);

		[Export ("imageScalingForSegment:")]
		NSImageScaling GetImageScaling (int forSegment);

		[Export ("setLabel:forSegment:")]
		void SetLabel (string label, int forSegment);

		[Export ("labelForSegment:")]
		string GetLabel (int forSegment);

		[Export ("setSelected:forSegment:")]
		void SetSelected (bool selected, int forSegment);

		[Export ("isSelectedForSegment:")]
		bool IsSelected (int forSegment);

		[Export ("setEnabled:forSegment:")]
		void SetEnabled (bool enabled, int forSegment);

		[Export ("isEnabledForSegment:")]
		bool IsEnabled (int forSegment);

		[Export ("setMenu:forSegment:")]
		void SetMenu (NSMenu menu, int forSegment);

		[Export ("menuForSegment:")]
		NSMenu GetMenu (int forSegment);

		[Export ("setToolTip:forSegment:")]
		void SetToolTip (string toolTip, int forSegment);

		[Export ("toolTipForSegment:")]
		string GetToolTip (int forSegment);

		[Export ("setTag:forSegment:")]
		void SetTag (int tag, int forSegment);

		[Export ("tagForSegment:")]
		int GetTag (int forSegment);

		[Export ("drawSegment:inFrame:withView:")]
		void DrawSegment (int segment, RectangleF frame, NSView controlView);

		//Detected properties
		[Export ("segmentCount")]
		int SegmentCount { get; set; }

		[Export ("selectedSegment")]
		int SelectedSegment { get; set; }

		[Export ("trackingMode")]
		NSSegmentSwitchTracking TrackingMode { get; set; }

		[Export ("segmentStyle")]
		NSSegmentStyle SegmentStyle { get; set; }

	}

	[BaseType (typeof (NSControl))]
	public interface NSSlider {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("isVertical")]
		int IsVertical { get; }

		[Export ("acceptsFirstMouse:")]
		bool AcceptsFirstMouse (NSEvent theEvent);

		//Detected properties
		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("altIncrementValue")]
		double AltIncrementValue { get; set; }

		[Export ("titleCell")]
		NSObject TitleCell { get; set; }

		[Export ("titleColor")]
		NSColor TitleColor { get; set; }

		[Export ("titleFont")]
		NSFont TitleFont { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("knobThickness")]
		float KnobThickness { get; set; }

		[Export ("image")]
		NSImage Image { get; set; }
	
		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValue (int index);

		[Export ("rectOfTickMarkAtIndex:")]
		RectangleF RectOfTick (int index);

		[Export ("indexOfTickMarkAtPoint:")]
		int IndexOfTickMark (PointF point);

		[Export ("closestTickMarkValueToValue:")]
		double ClosestTickMarkValue (double value);

		//Detected properties
		[Export ("numberOfTickMarks")]
		int TickMarksCount { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("allowsTickMarkValuesOnly")]
		bool AllowsTickMarkValuesOnly { get; set; }

	}
	
	[BaseType (typeof (NSActionCell))]
	public interface NSSliderCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

		[Export ("prefersTrackingUntilMouseUp")]
		bool PrefersTrackingUntilMouseUp ();

		[Export ("isVertical")]
		int IsVertical { get; }

		[Export ("knobRectFlipped:")]
		RectangleF KnobRectFlipped (bool flipped);

		[Export ("drawKnob")]
		void DrawKnob (RectangleF knobRect);

		[Export ("drawKnob")]
		void DrawKnob ();

		[Export ("drawBarInside:flipped:")]
		void DrawBar (RectangleF aRect, bool flipped);

		[Export ("trackRect")]
		RectangleF TrackRect{ get; }

		//Detected properties
		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("altIncrementValue")]
		double AltIncrementValue { get; set; }

		[Export ("titleColor")]
		NSColor TitleColor { get; set; }

		[Export ("titleFont")]
		NSFont TitleFont { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("titleCell")]
		NSObject TitleCell { get; set; }

		[Export ("knobThickness")]
		float KnobThickness { get; set; }

		[Export ("sliderType")]
		NSSliderType SliderType { get; set; }
	
		[Export ("tickMarkValueAtIndex:")]
		double TickMarkValue (int index);

		[Export ("rectOfTickMarkAtIndex:")]
		RectangleF RectOfTickMark (int index);

		[Export ("indexOfTickMarkAtPoint:")]
		int IndexOfTickMark (PointF point);

		[Export ("closestTickMarkValueToValue:")]
		double ClosestTickMarkValue (double value);

		//Detected properties
		[Export ("numberOfTickMarks")]
		int TickMarks { get; set; }

		[Export ("tickMarkPosition")]
		NSTickMarkPosition TickMarkPosition { get; set; }

		[Export ("allowsTickMarkValuesOnly")]
		bool AllowsTickMarkValuesOnly { get; set; }

	}
	
	[BaseType (typeof (NSObject))]
	public interface NSSpeechRecognizer {
		[Export ("startListening")]
		void StartListening ();

		[Export ("stopListening")]
		void StopListening ();

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSpeechRecognizerDelegate Delegate { get; set; }

		[Export ("commands")]
		string [] Commands { get; set; }

		[Export ("displayedCommandsTitle")]
		string DisplayedCommandsTitle { get; set; }

		[Export ("listensInForegroundOnly")]
		bool ListensInForegroundOnly { get; set; }

		[Export ("blocksOtherRecognizers")]
		bool BlocksOtherRecognizers { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSpeechRecognizerDelegate {
		[Export ("speechRecognizer:didRecognizeCommand:")]
		void DidRecognizeCommand (NSSpeechRecognizer sender, string command);
	}

	[BaseType (typeof (NSObject))]
	public interface NSSpeechSynthesizer {
		[Export ("initWithVoice:")]
		IntPtr Constructor (string voice);

		[Export ("startSpeakingString:")]
		bool StartSpeakingString (string theString);

		[Export ("startSpeakingString:toURL:")]
		bool StartSpeakingStringtoURL (string theString, NSUrl url);

		[Export ("isSpeaking")]
		bool IsSpeaking { get; }

		[Export ("stopSpeaking")]
		void StopSpeaking ();

		[Export ("stopSpeakingAtBoundary:")]
		void StopSpeaking (NSSpeechBoundary boundary);

		[Export ("pauseSpeakingAtBoundary:")]
		void PauseSpeaking (NSSpeechBoundary boundary);

		[Export ("continueSpeaking")]
		void ContinueSpeaking ();

		[Export ("addSpeechDictionary:")]
		void AddSpeechDictionary (NSDictionary speechDictionary);

		[Export ("phonemesFromText:")]
		string PhonemesFromText (string text);

		[Export ("objectForProperty:error:")]
		NSObject ObjectForProperty (string property, out NSError outError);

		[Export ("setObject:forProperty:error:")]
		bool SetObjectforProperty (NSObject theObject, string property, out NSError outError);

		[Export ("isAnyApplicationSpeaking")]
		bool IsAnyApplicationSpeaking { get; }

		[Static]
		[Export ("defaultVoice")]
		string DefaultVoice { get; }

		[Static]
		[Export ("availableVoices")]
		string [] AvailableVoices { get; }

		[Static]
		[Export ("attributesForVoice:")]
		NSDictionary AttributesForVoice (string voice);

		//Detected properties
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSpeechSynthesizerDelegate Delegate { get; set; }

		[Export ("voice")]
		string Voice { get; set; }

		[Export ("rate")]
		float Rate { get; set; }

		[Export ("volume")]
		float Volume { get; set; }

		[Export ("usesFeedbackWindow")]
		bool UsesFeedbackWindow { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSpeechSynthesizerDelegate {
		[Export ("speechSynthesizer:didFinishSpeaking:")]
		void DidFinishSpeaking (NSSpeechSynthesizer sender, bool finishedSpeaking);

		[Export ("speechSynthesizer:willSpeakWord:ofString:")]
		void WillSpeakWordofString (NSSpeechSynthesizer sender, NSRange characterRange, string theString);

		[Export ("speechSynthesizer:willSpeakPhoneme:")]
		void WillSpeakPhoneme (NSSpeechSynthesizer sender, short phonemeOpcode);

		[Export ("speechSynthesizer:didEncounterErrorAtIndex:ofString:message:")]
		void DidEncounterError (NSSpeechSynthesizer sender, uint characterIndex, string theString, string message);

		[Export ("speechSynthesizer:didEncounterSyncMessage:")]
		void DidEncounterSyncMessage (NSSpeechSynthesizer sender, string message);
	}

	[BaseType (typeof (NSObject))]
	public interface NSSpellChecker {
		[Static]
		[Export ("sharedSpellChecker")]
		NSSpellChecker SharedSpellChecker { get; }

		[Static]
		[Export ("sharedSpellCheckerExists")]
		bool SharedSpellCheckerExists { get; }

		[Static]
		[Export ("uniqueSpellDocumentTag")]
		int UniqueSpellDocumentTag { get; }

		[Export ("checkSpellingOfString:startingAt:language:wrap:inSpellDocumentWithTag:wordCount:")]
		NSRange CheckSpelling (string stringToCheck, int startingOffset, string language, bool wrapFlag, int documentTag, int wordCount);

		[Export ("checkSpellingOfString:startingAt:")]
		NSRange CheckSpelling (string stringToCheck, int startingOffset);

		[Export ("countWordsInString:language:")]
		int CountWords (string stringToCount, string language);

		[Export ("checkGrammarOfString:startingAt:language:wrap:inSpellDocumentWithTag:details:")]
		NSRange CheckGrammar (string stringToCheck, int startingOffset, string language, bool wrapFlag, int documentTag, NSDictionary[] details );

		[Export ("checkString:range:types:options:inSpellDocumentWithTag:orthography:wordCount:")]
		NSTextCheckingResult [] CheckString (string stringToCheck, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, int documentTag, NSOrthography orthography, int wordCount);

		//FIXME:
		//[Export ("requestCheckingOfString:range:types:options:inSpellDocumentWithTag:completionHandler:NSIntegersequenceNumber,NSArray*results,NSOrthography*orthography,NSIntegerwordCount))completionHandler")]
		//int RequestChecking (string stringToCheck, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, int tag, IntPtr completionHandler );

		[Export ("menuForResult:string:options:atLocation:inView:")]
		NSMenu MenuForResults (NSTextCheckingResult result, string checkedString, NSDictionary options, PointF location, NSView view);

		[Export ("userQuotesArrayForLanguage:")]
		string [] UserQuotesArrayForLanguage (string language);

		[Export ("userReplacementsDictionary")]
		NSDictionary UserReplacementsDictionary { get; }

		[Export ("updateSpellingPanelWithMisspelledWord:")]
		void UpdateSpellingPanelWithMisspelledWord (string word);

		[Export ("updateSpellingPanelWithGrammarString:detail:")]
		void UpdateSpellingPanelWithGrammarl (string theString, NSDictionary detail);

		[Export ("spellingPanel")]
		NSPanel SpellingPanel { get; }

		[Export ("substitutionsPanel")]
		NSPanel SubstitutionsPanel { get; }

		[Export ("updatePanels")]
		void UpdatePanels ();

		[Export ("ignoreWord:inSpellDocumentWithTag:")]
		void IgnoreWord (string wordToIgnore, int documentTag);

		[Export ("ignoredWordsInSpellDocumentWithTag:")]
		string [] IgnoredWords (int documentTag);

		[Export ("setIgnoredWords:inSpellDocumentWithTag:")]
		void SetIgnoredWords (string [] words, int documentTag);

		[Export ("guessesForWordRange:inString:language:inSpellDocumentWithTag:")]
		string [] GuessesForWordRange (NSRange range, string theString, string language, int documentTag);

		[Export ("completionsForPartialWordRange:inString:language:inSpellDocumentWithTag:")]
		string [] CompletionsForPartialWordRange (NSRange range, string theString, string language, int documentTag);

		[Export ("closeSpellDocumentWithTag:")]
		void CloseSpellDocument (int documentTag);

		[Export ("availableLanguages")]
		string [] AvailableLanguages { get; }

		[Export ("userPreferredLanguages")]
		string [] UserPreferredLanguages { get; }

		[Export ("setWordFieldStringValue:")]
		void SetWordFieldStringValue (string aString);

		[Export ("learnWord:")]
		void LearnWord (string word);

		[Export ("hasLearnedWord:")]
		bool HasLearnedWord (string word);

		[Export ("unlearnWord:")]
		void UnlearnWord (string word);

		//Detected properties
		[Export ("accessoryView")]
		NSView AccessoryView { get; set; }

		[Export ("substitutionsPanelAccessoryViewController")]
		NSViewController SubstitutionsPanelAccessoryViewController { get; set; }

		[Export ("automaticallyIdentifiesLanguages")]
		bool AutomaticallyIdentifiesLanguages { get; set; }

		[Export ("language")]
		string Language { get; set; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSSoundDelegate) })]
	public interface NSSound {
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
	public interface NSSoundDelegate {
		[Export ("sound:didFinishPlaying:"), EventArgs ("NSSoundFinished")]
		void DidFinishPlaying (NSSound sound, bool finished);
	}

	[BaseType (typeof (NSView))]
	public interface NSSplitView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("drawDividerInRect:")]
		void DrawDivider (RectangleF rect);

		[Export ("dividerColor")]
		NSColor DividerColor { get; }

		[Export ("dividerThickness")]
		float DividerThickness { get; }

		[Export ("adjustSubviews")]
		void AdjustSubviews ();

		[Export ("isSubviewCollapsed:")]
		bool IsSubviewCollapsed (NSView subview);

		[Export ("minPossiblePositionOfDividerAtIndex:")]
		float MinPositionOfDivider (int dividerIndex);

		[Export ("maxPossiblePositionOfDividerAtIndex:")]
		float MaxPositionOfDivider (int dividerIndex);

		[Export ("setPosition:ofDividerAtIndex:")]
		void SetPositionofDivider (float position, int dividerIndex);

		//Detected properties
		[Export ("vertical")]
		bool IsVertical { [Bind ("isVertical")]get; set; }

		[Export ("dividerStyle")]
		NSSplitViewDividerStyle DividerStyle { get; set; }

		[Export ("autosaveName")]
		string AutosaveName { get; set; }
		
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSSplitViewDelegate Delegate { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSSplitViewDelegate {
		[Export ("splitView:canCollapseSubview:") ] [DefaultValue (true)]
		bool CanCollapse (NSSplitView splitView, NSView subview);

		[Export ("splitView:shouldCollapseSubview:forDoubleClickOnDividerAtIndex:")] [DefaultValue (true)]
		bool ShouldCollapseForDoubleClick (NSSplitView splitView, NSView subview, int doubleClickAtDividerIndex);

		[Export ("splitView:constrainMinCoordinate:ofSubviewAt:")]
		float SetMinCoordinateofSubview (NSSplitView splitView, float proposedMinimumPosition, int subviewDividerIndex);

		[Export ("splitView:constrainMaxCoordinate:ofSubviewAt:")]
		float SetMaxCoordinateofSubview (NSSplitView splitView, float proposedMaximumPosition, int subviewDividerIndex);

		[Export ("splitView:constrainSplitPosition:ofSubviewAt:")]
		float ConstrainSplitPosition (NSSplitView splitView, float proposedPosition, int subviewDividerIndex);

		[Export ("splitView:resizeSubviewsWithOldSize:")]
		void Resize (NSSplitView splitView, SizeF oldSize);

		[Export ("splitView:shouldAdjustSizeOfSubview:")][DefaultValue (true)]
		bool ShouldAdjustSize (NSSplitView splitView, NSView view);

		[Export ("splitView:shouldHideDividerAtIndex:")] [DefaultValue (false)]
		bool ShouldHideDivider (NSSplitView splitView, int dividerIndex);

		[Export ("splitView:effectiveRect:forDrawnRect:ofDividerAtIndex:")]
		RectangleF GetEffectiveRect (NSSplitView splitView, RectangleF proposedEffectiveRect, RectangleF drawnRect, int dividerIndex);

		[Export ("splitView:additionalEffectiveRectOfDividerAtIndex:")]
		RectangleF GetAdditionalEffectiveRect (NSSplitView splitView, int dividerIndex);

		[Export ("splitViewWillResizeSubviews:")]
		void SplitViewWillResizeSubviews (NSNotification notification);

		[Export ("splitViewDidResizeSubviews:")]
		void DidResizeSubviews (NSNotification notification);
	}

	[BaseType (typeof (NSObject))]
	public interface NSStatusBar {
		[Static, Export ("systemStatusBar")]
		NSStatusBar SystemStatusBar { get; }

		[Export ("statusItemWithLength:")]
		NSStatusItem CreateStatusItem (float length);

		[Export ("removeStatusItem:")]
		void RemoveStatusItem (NSStatusItem item);

		[Export ("isVertical")]
		bool IsVertical { get; }

		[Export ("thickness")]
		float Thickness { get; }
	}

	[BaseType (typeof (NSObject))]
	[PrivateDefaultCtor]
	public interface NSStatusItem {
		[Export ("statusBar")]
		NSStatusBar StatusBar { get; }

		[Export ("length")]
		float Length { get; set; }

		[Export ("action"), NullAllowed]
		Selector Action { get; set; }

		[Export ("sendActionOn:")]
		int SendActionOn (NSEventMask mask);

		[Export ("popUpStatusItemMenu:")]
		void PopUpStatusItemMenu (NSMenu menu);

		[Export ("drawStatusBarBackgroundInRect:withHighlight:")]
		void DrawStatusBarBackgroundInRectwithHighlight (RectangleF rect, bool highlight);

		//Detected properties
		[Export ("doubleAction")]
		Selector DoubleAction { get; set; }

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("attributedTitle")]
		NSAttributedString AttributedTitle { get; set; }

		[Export ("image")]
		NSImage Image { get; set; }

		[Export ("alternateImage")]
		NSImage AlternateImage { get; set; }

		[Export ("menu")]
		NSMenu Menu { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }

		[Export ("highlightMode")]
		bool HighlightMode { get; set; }

		[Export ("view")]
		NSView View { get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSShadow {
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
	public interface NSView {
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
		[PreSnippet ("var mySuper = Superview;")]
		[PostSnippet ("__mt_Superview_var = null;\n\tif (mySuper != null) {\n\t#pragma warning disable 168\n\tvar flush = mySuper.Subviews;\n#pragma warning restore 168\n\t}")]
		void RemoveFromSuperview ();

		[Export ("replaceSubview:with:")]
		void ReplaceSubviewWith (NSView oldView, NSView newView);

		[Export ("removeFromSuperviewWithoutNeedingDisplay")]
		[PreSnippet ("var mySuper = Superview;")]
		[PostSnippet ("__mt_Superview_var = null;\n\tif (mySuper != null) {\n\t#pragma warning disable 168\n\tvar flush = mySuper.Subviews;\n#pragma warning restore 168\n\t}")]
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
		PointF ConvertPointFromView (PointF aPoint, [NullAllowed] NSView aView);

		[Export ("convertPoint:toView:")]
		PointF ConvertPointToView (PointF aPoint, [NullAllowed] NSView aView);

		[Export ("convertSize:fromView:")]
		SizeF ConvertSizeFromView (SizeF aSize, [NullAllowed] NSView aView);

		[Export ("convertSize:toView:")]
		SizeF ConvertSizeToView (SizeF aSize, [NullAllowed] NSView aView);

		[Export ("convertRect:fromView:")]
		RectangleF ConvertRectFromView (RectangleF aRect, NSView aView);

		[Export ("convertRect:toView:")]
		RectangleF ConvertRectToView (RectangleF aRect, NSView aView);

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

		//[Export ("setNeedsDisplay:")]
		//void SetNeedsDisplay (bool flag);
		
		[Export ("lockFocus")]
		void LockFocus ();

		[Export ("unlockFocus")]
		void UnlockFocus ();

		[Export ("lockFocusIfCanDraw")]
		bool LockFocusIfCanDraw ();

		[Export ("lockFocusIfCanDrawInContext:")]
		bool LockFocusIfCanDrawInContext (NSGraphicsContext context);

		[Export ("focusView")][Static]
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
		int AddTrackingRect (RectangleF aRect, NSObject anObject, IntPtr data, bool assumeInside);

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

		[Export ("backgroundFilters"), NullAllowed]
		CIFilter [] BackgroundFilters { get; set; }

		[Export ("compositingFilter"), NullAllowed]
		CIFilter CompositingFilter { get; set; }

		[Export ("contentFilters"), NullAllowed]
		CIFilter [] ContentFilters { get; set; }

		[Export ("shadow")]
		NSShadow Shadow { get; set; }

		[Export ("postsBoundsChangedNotifications")]
		bool PostsBoundsChangedNotifications { get; set; }

		[Export ("toolTip")]
		string ToolTip { get; set; }
				
		[Export ("registerForDraggedTypes:")]
		void RegisterForDraggedTypes (string [] newTypes);

		[Export ("unregisterDraggedTypes")]
		void UnregisterDraggedTypes ();
		
		[Export ("registeredDraggedTypes")]
		string[] RegisteredDragTypes();
        
		[Export ("dragImage:at:offset:event:pasteboard:source:slideBack:")]
		void DragImage (NSImage anImage, PointF viewLocation, SizeF initialOffset, NSEvent theEvent, NSPasteboard pboard, NSObject sourceObj, bool slideFlag);

		[Export ("dragFile:fromRect:slideBack:event:")]
		bool DragFile (string filename, RectangleF aRect, bool slideBack, NSEvent theEvent);
		
		[Export ("dragPromisedFilesOfTypes:fromRect:source:slideBack:event:")]
		bool DragPromisedFilesOfTypes (string[] typeArray, RectangleF aRect, NSObject sourceObject, bool slideBack, NSEvent theEvent);
		
		[Export ("exitFullScreenModeWithOptions:")]
		void ExitFullscreenModeWithOptions(NSDictionary options);
		
		[Export ("enterFullScreenMode:withOptions:")]
		void EnterFullscreenModeWithOptions(NSScreen screen, NSDictionary options);
		
		[Export ("isInFullScreenMode")]
		bool IsInFullscreenMode { get; }
		
		/* 10.6+ Only - How do you specify that? - jm
		   [Field ("NSFullScreenModeApplicationPresentationOptions")]   
		   NSString NSFullScreenModeApplicationPresentationOptions { get; }
		*/
		
		// Fields
		[Field ("NSFullScreenModeAllScreens")]
		NSString NSFullScreenModeAllScreens { get; }
		
		[Field ("NSFullScreenModeSetting")]
		NSString NSFullScreenModeSetting { get; }
		
		[Field ("NSFullScreenModeWindowLevel")]
		NSString NSFullScreenModeWindowLevel { get; }

		[Field ("NSViewFrameDidChangeNotification")]
		NSString NSViewFrameDidChangeNotification { get; }
 
		[Field ("NSViewFocusDidChangeNotification")]
		NSString NSViewFocusDidChangeNotification { get; }

		[Field ("NSViewBoundsDidChangeNotification")]
		NSString NSViewBoundsDidChangeNotification { get; }

		[Field ("NSViewGlobalFrameDidChangeNotification")]
		NSString NSViewGlobalFrameDidChangeNotification { get; }

		[Field ("NSViewDidUpdateTrackingAreasNotification")]
		NSString NSViewDidUpdateTrackingAreasNotification { get; }

#region From NSAnimatablePropertyContainer
		[Export ("animator")]
		NSObject Animator { [return: Proxy] get; }
	
		[Export ("animations")]
		NSDictionary Animations { get; set; }
	
		[Export ("animationForKey:")]
		NSObject AnimationFor (NSString key);
	
		[Static, Export ("defaultAnimationForKey:")]
		NSObject DefaultAnimationFor (NSString key);
#endregion
	}

	[BaseType (typeof (NSAnimation))]
	public interface NSViewAnimation { 
		[Export ("initWithViewAnimations:")]
		IntPtr Constructor (NSDictionary [] viewAnimations);
	
		[Export ("viewAnimations")]
		NSDictionary [] ViewAnimations { get; set; }
	
		[Export ("animator")]
		NSObject Animator { [return: Proxy] get; }
	
		[Export ("animations")]
		NSDictionary Animations  { get; set; }
	
		[Export ("animationForKey:")]
		NSObject AnimationForKey (string  key);
	
		[Static]
		[Export ("defaultAnimationForKey:")]
		NSObject DefaultAnimationForKey (string  key);
	
		[Field ("NSViewAnimationTargetKey")]
		NSString TargetKey { get; }
		
		[Field ("NSViewAnimationStartFrameKey")]
		NSString StartFrameKey { get; }
		
		[Field ("NSViewAnimationEndFrameKey")]
		NSString EndFrameKey { get; }
		
		[Field ("NSViewAnimationEffectKey")]
		NSString EffectKey { get; }
		
		[Field ("NSViewAnimationFadeInEffect")]
		NSString FadeInEffect { get; }
		
		[Field ("NSViewAnimationFadeOutEffect")]
		NSString FadeOutEffect { get; }
	}
	

	[BaseType (typeof (NSResponder))]
	public interface NSViewController {
		[Export ("initWithNibName:bundle:")]
		IntPtr Constructor ([NullAllowed] string nibNameOrNil, [NullAllowed] NSBundle nibBundleOrNil);

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
	public interface NSTableColumn {
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
	
	
	[BaseType (typeof (NSControl), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTableViewDelegate)})]
	public interface NSTableView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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
		void SelectAll ([NullAllowed] NSObject sender);
	
		[Export ("deselectAll:")]
		void DeselectAll ([NullAllowed] NSObject sender);
	
		[Export ("selectColumnIndexes:byExtendingSelection:")]
		void SelectColumns (NSIndexSet indexes, bool byExtendingSelection);
	
		[Export ("selectRowIndexes:byExtendingSelection:")]
		void SelectRows (NSIndexSet indexes, bool byExtendingSelection);
	
		[Export ("selectedColumnIndexes")]
		NSIndexSet SelectedColumns { get; }
	
		[Export ("selectedRowIndexes")]
		NSIndexSet SelectedRows { get; }
	
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
		NSObject WeakDataSource { get; set; }

		[Wrap ("WeakDataSource")]
		NSTableViewDataSource DataSource { get; set; }
	
		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }
	
		[Wrap ("WeakDelegate")][NullAllowed]
		NSTableViewDelegate Delegate { get; set; }
	
		[Export ("headerView"), NullAllowed]
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
	public interface NSTableViewDelegate {
		[Export ("tableView:willDispayCell:forTableColumn:row:"), EventArgs ("NSTableViewCell")]
		void WillDisplayCell (NSTableView tableView, NSObject cell, NSTableColumn tableColumn, int row);
	
		[Export ("tableView:shouldEditTableColumn:row:"), DelegateName ("NSTableViewColumnRowPredicate"), DefaultValue (false)]
		bool ShouldEditTableColumn (NSTableView tableView, NSTableColumn tableColumn, int row);
	
		[Export ("selectionShouldChangeInTableView:"), DelegateName ("NSTableViewPredicate"), DefaultValue (false)]
		bool SelectionShouldChange (NSTableView tableView);
	
		[Export ("tableView:shouldSelectRow:"), DelegateName ("NSTableViewRowPredicate")] [DefaultValue (true)]
		bool ShouldSelectRow (NSTableView tableView, int row);
	
		[Export ("tableView:selectionIndexesForProposedSelection:"), DelegateName ("NSTableViewIndexFilter"), DefaultValueFromArgument ("proposedSelectionIndexes")]
		NSIndexSet GetSelectionIndexes (NSTableView tableView, NSIndexSet proposedSelectionIndexes);
	
		[Export ("tableView:shouldSelectTableColumn:"), DelegateName ("NSTableViewColumnPredicate"), DefaultValue (true)]
		bool ShouldSelectTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:mouseDownInHeaderOfTableColumn:"), EventArgs ("NSTableViewTable")]
		void MouseDown (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didClickTableColumn:"), EventArgs ("NSTableViewTable")]
		void DidClickTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		[Export ("tableView:didDragTableColumn:"), EventArgs ("NSTableViewTable")]
		void DidDragTableColumn (NSTableView tableView, NSTableColumn tableColumn);
	
		//FIXME: Binding NSRectPointer
		//[Export ("tableView:toolTipForCell:rect:tableColumn:row:mouseLocation:")]
		//string TableViewtoolTipForCellrecttableColumnrowmouseLocation (NSTableView tableView, NSCell cell, NSRectPointer rect, NSTableColumn tableColumn, int row, PointF mouseLocation);
	
		[Export ("tableView:heightOfRow:"), DelegateName ("NSTableViewRowHeight"), DefaultValue (16f)]
		float GetRowHeight (NSTableView tableView, int row );
	
		[Export ("tableView:typeSelectStringForTableColumn:row:"), DelegateName ("NSTableViewColumnRowString"), DefaultValue ("String.Empty")]
		string GetSelectString (NSTableView tableView, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:nextTypeSelectMatchFromRow:toRow:forString:"), DelegateName ("NSTableViewSearchString"), DefaultValue (-1)]
		int GetNextTypeSelectMatch (NSTableView tableView, int startRow, int endRow, string searchString);
	
		[Export ("tableView:shouldTypeSelectForEvent:withCurrentSearchString:"), DelegateName ("NSTableViewEventString"), DefaultValue (false)]
		bool ShouldTypeSelect (NSTableView tableView, NSEvent theEvent, string searchString );
	
		[Export ("tableView:shouldShowCellExpansionForTableColumn:row:"), DelegateName ("NSTableViewColumnRowPredicate"), DefaultValue (false)]
		bool ShouldShowCellExpansion (NSTableView tableView, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:shouldTrackCell:forTableColumn:row:"), DelegateName ("NSTableViewCell"), DefaultValue (false)]
		bool ShouldTrackCell (NSTableView tableView, NSCell cell, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:dataCellForTableColumn:row:"), DelegateName ("NSTableViewCellGetter"), DefaultValue (null)]
		NSCell GetDataCell (NSTableView tableView, NSTableColumn tableColumn, int row );
	
		[Export ("tableView:isGroupRow:"), DelegateName ("NSTableViewRowPredicate"), DefaultValue (false)]
		bool IsGroupRow (NSTableView tableView, int row );
	
		[Export ("tableView:sizeToFitWidthOfColumn:"), DelegateName ("NSTableViewColumnWidth"), DefaultValue (80)]
		float GetSizeToFitColumnWidth (NSTableView tableView, int column );
	
		[Export ("tableView:shouldReorderColumn:toColumn:"), DelegateName ("NSTableReorder"), DefaultValue (false)]
		bool ShouldReorder (NSTableView tableView, int columnIndex, int newColumnIndex );
	
		[Export ("tableViewSelectionDidChange:"), EventArgs ("NSNotification")]
		void SelectionDidChange (NSNotification notification);
	
		[Export ("tableViewColumnDidMove:"), EventArgs ("NSNotification")]
		void ColumnDidMove (NSNotification notification);
	
		[Export ("tableViewColumnDidResize:"), EventArgs ("NSNotification")]
		void ColumnDidResize (NSNotification notification);
	
		[Export ("tableViewSelectionIsChanging:"), EventArgs ("NSNotification")]
		void SelectionIsChanging (NSNotification notification);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTableViewDataSource {
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

	//
	// This is the mixed NSTableViewDataSource and NSTableViewDelegate
	//
	[Model]
	[BaseType (typeof (NSObject))]
	public interface NSTableViewSource {
		//
		// These come form NSTableViewDataSource
		//
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
	
		[Export ("tableView:isGroupRow:"), DefaultValue (false)]
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

		// NSTableViewDataSource
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
	public interface NSTableHeaderCell {
		[Export ("drawSortIndicatorWithFrame:inView:ascending:priority:")]
		void DrawSortIndicator (RectangleF cellFrame, NSView controlView, bool ascending, int priority );
	
		[Export ("sortIndicatorRectForBounds:")]
		RectangleF GetSortIndicatorRect (RectangleF theRect );
	}
	
	[BaseType (typeof (NSView))]
	public interface NSTableHeaderView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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
		
	[BaseType (typeof (NSView), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTabViewDelegate)})]
	public interface NSTabView {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("selectTabViewItem:")]
		void Select (NSTabViewItem tabViewItem);

		[Export ("selectTabViewItemAtIndex:")]
		void SelectAt (int index);

		[Export ("selectTabViewItemWithIdentifier:")]
		void Select (NSObject identifier);

		[Export ("takeSelectedTabViewItemFromSender:")]
		void TakeSelectedTabViewItemFrom (NSObject sender);

		[Export ("selectFirstTabViewItem:")]
		void SelectFirst (NSObject sender);

		[Export ("selectLastTabViewItem:")]
		void SelectLast (NSObject sender);

		[Export ("selectNextTabViewItem:")]
		void SelectNext (NSObject sender);

		[Export ("selectPreviousTabViewItem:")]
		void SelectPrevious (NSObject sender);

		[Export ("selectedTabViewItem")]
		NSTabViewItem Selected { get; }

		[Export ("font")]
		NSFont Font { get; set; }

		[Export ("tabViewType")]
		NSTabViewType TabViewType { get; set; }

		[Export ("tabViewItems")]
		NSTabViewItem [] Items { get; }

		[Export ("allowsTruncatedLabels")]
		bool AllowsTruncatedLabels { get; set; }

		[Export ("minimumSize")]
		SizeF MinimumSize { get; }

		[Export ("drawsBackground")]
		bool DrawsBackground { get; set; }

		[Export ("controlTint")]
		NSControlTint ControlTint { get; set; }

		[Export ("controlSize")]
		NSControlSize ControlSize { get; set; }

		[Export ("addTabViewItem:")]
		void Add (NSTabViewItem tabViewItem);

		[Export ("insertTabViewItem:atIndex:")]
		void Insert (NSTabViewItem tabViewItem, int index);

		[Export ("removeTabViewItem:")]
		void Remove (NSTabViewItem tabViewItem);

		[Export ("delegate"), NullAllowed]
		NSTabViewDelegate Delegate { get; set; }

		[Export ("tabViewItemAtPoint:")]
		NSTabViewItem TabViewItemAtPoint (PointF point);

		[Export ("contentRect")]
		RectangleF ContentRect { get; }

		[Export ("numberOfTabViewItems")]
		int Count { get; }

		[Export ("indexOfTabViewItem:")]
		int IndexOf (NSTabViewItem tabViewItem);

		[Export ("tabViewItemAtIndex:")]
		NSTabViewItem Item (int index);

		[Export ("indexOfTabViewItemWithIdentifier:")]
		int IndexOf (NSObject identifier);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTabViewDelegate {
		[Export ("tabView:shouldSelectTabViewItem:"), DelegateName ("NSTabViewPredicate"), DefaultValue (true)]
		bool ShouldSelectTabViewItem (NSTabView tabView, NSTabViewItem item);
		
		[Export ("tabView:willSelectTabViewItem:"), EventArgs ("NSTabViewItem")]
		void WillSelect (NSTabView tabView, NSTabViewItem item);

		[Export ("tabView:didSelectTabViewItem:"), EventArgs ("NSTabViewItem")]
		void DidSelect (NSTabView tabView, NSTabViewItem item);
	 
		[Export ("tabViewDidChangeNumberOfTabViewItems:")]
		void NumberOfItemsChanged (NSTabView tabView);
	}

	[BaseType (typeof (NSObject))]
	public interface NSTabViewItem {
		[Export ("initWithIdentifier:")]
		IntPtr Constructor (NSObject identifier);

		[Export ("identifier")]
		NSObject Identifier { get; set; }

		[Export ("view")]
		NSView View { get; set; }

		[Export ("initialFirstResponder")]
		NSObject InitialFirstResponder { get; set; }

		[Export ("label")]
		string Label { get; set; }

		[Export ("color")]
		NSColor Color { get; set; }

		[Export ("tabState")]
		NSTabState TabState { get; }

		[Export ("tabView")]
		NSTabView TabView { get; }

		[Export ("drawLabel:inRect:")]
		void DrawLabel (bool shouldTruncateLabel, RectangleF labelRect);

		[Export ("sizeOfLabel:")]
		SizeF SizeOfLabel (bool computeMin);
	}
	
	[BaseType (typeof (NSView), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextDelegate)})]
	public interface NSText {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

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
	public interface NSTextDelegate {
		[Export ("textShouldBeginEditing:"), DelegateName ("NSTextPredicate"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSText textObject);

		[Export ("textShouldEndEditing:"), DelegateName ("NSTextPredicate"), DefaultValue (true)]
		bool TextShouldEndEditing (NSText textObject);

		[Export ("textDidBeginEditing:"), EventArgs ("NSNotification")]
		void TextDidBeginEditing (NSNotification notification);

		[Export ("textDidEndEditing:"), EventArgs ("NSNotification")]
		void TextDidEndEditing (NSNotification notification);

		[Export ("textDidChange:"), EventArgs ("NSNotification")]
		void TextDidChange (NSNotification notification);
	}

	[BaseType (typeof (NSCell))]
	public interface NSTextAttachmentCell {
		[Export ("wantsToTrackMouse")]
		bool WantsToTrackMouse ();

		[Export ("highlight:withFrame:inView:")]
		void Highlight (bool highlight, RectangleF cellFrame, NSView controlView);

		[Export ("trackMouse:inRect:ofView:untilMouseUp:")]
		bool TrackMouseinRectofViewuntilMouseUp (NSEvent theEvent, RectangleF cellFrame, NSView controlView, bool untilMouseUp);

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
		bool TrackMouse (NSEvent theEvent, RectangleF cellFrame, NSView controlView, uint charIndex, bool untilMouseUp);

		[Export ("cellFrameForTextContainer:proposedLineFragment:glyphPosition:characterIndex:")]
		RectangleF CellFrameForTextContainer (NSTextContainer textContainer, RectangleF lineFrag, PointF position, uint charIndex);

		//Detected properties
		[Export ("attachment")]
		NSTextAttachment Attachment { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSTextAttachment {
		[Export ("initWithFileWrapper:")]
		IntPtr Constructor (NSFileWrapper fileWrapper);

		//Detected properties
		[Export ("fileWrapper")]
		NSFileWrapper FileWrapper { get; set; }

		[Export ("attachmentCell")]
		NSTextAttachmentCell AttachmentCell { get; set; }

	}

	[BaseType (typeof (NSObject))]
	public interface NSTextBlock {
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
	public interface NSTextField {
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

	[BaseType (typeof (NSTextField))]
	public interface NSSecureTextField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTextFieldDelegate {
		[Export ("control:textShouldBeginEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldBeginEditing (NSControl control, NSText fieldEditor);

		[Export ("control:textShouldEndEditing:"), DelegateName ("NSControlText"), DefaultValue (true)]
		bool TextShouldEndEditing (NSControl control, NSText fieldEditor);

		[Export ("control:didFailToFormatString:errorDescription:"), DelegateName ("NSControlTextError"), DefaultValue (true)]
		bool DidFailToFormatString (NSControl control, string str, string error);
		
		[Export ("control:didFailToValidatePartialString:errorDescription:"), EventArgs ("NSControlTextError")]
		void DidFailToValidatePartialString (NSControl control, string str, string error);
		
		[Export ("control:isValidObject:"), DelegateName ("NSControlTextValidation"), DefaultValue (true)]
		bool IsValidObject (NSControl control, NSObject objectToValidate);

		[Export ("control:textView:doCommandBySelector:"), DelegateName ("NSControlCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSControl control, NSTextView textView, Selector commandSelector);

		[Export ("control:textView:completions:forPartialWordRange:indexOfSelectedItem:"), DelegateName ("NSControlTextFilter"), DefaultValue ("new string[0]")]
		string [] GetCompletions (NSControl control, NSTextView textView, string [] words, NSRange charRange, int index);

		[Export ("controlTextDidEndEditing:"), EventArgs ("NSNotification")]
		void EditingEnded (NSNotification notification);

		[Export ("controlTextDidChange:"), EventArgs ("NSNotification")]
		void Changed (NSNotification notification);

		[Export ("controlTextDidBeginEditing:"), EventArgs ("NSNotification")]
		void EditingBegan (NSNotification notification);	
	}
	
	[BaseType (typeof (NSActionCell))]
	public interface NSTextFieldCell {
		[Export ("initTextCell:")]
		IntPtr Constructor (string aString);
	
		[Export ("initImageCell:")]
		IntPtr Constructor (NSImage  image);

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

	[BaseType (typeof (NSTextFieldCell))]
	public interface NSSecureTextFieldCell {
		[Export ("echosBullets")]
		bool EchosBullets { get; set; }
	}  

	[BaseType (typeof (NSObject))]
	public interface NSTextInputContext {
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
	public interface NSTextList {
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
	public interface NSTextTableBlock {
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
	public interface NSTextTable {
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
	public interface NSTextContainer {
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
	public interface NSTextStorage {
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
	public interface NSTextStorageDelegate {
		[Export ("textStorageWillProcessEditing:")]
		void TextStorageWillProcessEditing (NSNotification notification);

		[Export ("textStorageDidProcessEditing:")]
		void TextStorageDidProcessEditing (NSNotification notification);
	}

	[BaseType (typeof (NSObject))]
	public interface NSTextTab {
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

	[BaseType (typeof (NSText), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextViewDelegate)})]
	public interface NSTextView {
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
		void DrawInsertionPointInRectcolorturnedOn (RectangleF rect, NSColor color, bool turnedOn);

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
		void Complete ([NullAllowed] NSObject sender);

		[Export ("rangeForUserCompletion")]
		NSRange RangeForUserCompletion ();

		[Export ("completionsForPartialWordRange:indexOfSelectedItem:")]
		string [] CompletionsForPartialWord (NSRange charRange, int index);

		[Export ("insertCompletion:forPartialWordRange:movement:isFinal:")]
		void InsertCompletionforPartialWord (string word, NSRange charRange, int movement, bool isFinal);

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
		void SetSelectedRangesaffinitystillSelecting (NSArray /*NSRange []*/ ranges, NSSelectionAffinity affinity, bool stillSelectingFlag);

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
		bool ShouldChangeTextInRangesreplacementStrings (NSArray /* NSRange [] */ affectedRanges, string [] replacementStrings);

		[Export ("rangesForUserTextChange")]
		NSArray /* NSRange [] */ RangesForUserTextChange ();

		[Export ("rangesForUserCharacterAttributeChange")]
		NSArray /* NSRange [] */ RangesForUserCharacterAttributeChange ();

		[Export ("rangesForUserParagraphAttributeChange")]
		NSArray /* NSRange [] */ RangesForUserParagraphAttributeChange ();

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
		NSArray /* NSRange [] */ SelectedRanges { get; set; }

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
	public interface NSTextViewDelegate {
		[Export ("textView:clickedOnLink:atIndex:"), DelegateName ("NSTextViewLink"), DefaultValue (false)]
		bool LinkClicked (NSTextView textView, NSObject link, uint charIndex);

		[Export ("textView:clickedOnCell:inRect:atIndex:"), EventArgs ("NSTextViewClicked")]
		void CellClicked (NSTextView textView, NSTextAttachmentCell cell, RectangleF cellFrame, uint charIndex);

		[Export ("textView:doubleClickedOnCell:inRect:atIndex:"), EventArgs ("NSTextViewDoubleClick")]
		void CellDoubleClicked (NSTextView textView, NSTextAttachmentCell cell, RectangleF cellFrame, uint charIndex);

		// 
		[Export ("textView:writablePasteboardTypesForCell:atIndex:"), DelegateName ("NSTextViewCellPosition"),DefaultValue (null)]
		string [] GetWritablePasteboardTypes (NSTextView view, NSTextAttachmentCell forCell, uint charIndex);

		[Export ("textView:writeCell:atIndex:toPasteboard:type:"), DelegateName ("NSTextViewCellPasteboard"), DefaultValue (true)]
		bool WriteCell (NSTextView view, NSTextAttachmentCell cell, uint charIndex, NSPasteboard pboard, string type);

		[Export ("textView:willChangeSelectionFromCharacterRange:toCharacterRange:"), DelegateName ("NSTextViewSelectionChange"), DefaultValueFromArgument ("newSelectedCharRange")]
		NSRange WillChangeSelection (NSTextView textView, NSRange oldSelectedCharRange, NSRange newSelectedCharRange);

		[Export ("textView:willChangeSelectionFromCharacterRanges:toCharacterRanges:"), DelegateName ("NSTextViewSelectionWillChange"), DefaultValueFromArgument ("newSelectedCharRanges")]
		NSValue [] WillChangeSelectionFromRanges (NSTextView textView, NSValue [] oldSelectedCharRanges, NSValue [] newSelectedCharRanges);

		[Export ("textView:shouldChangeTextInRanges:replacementStrings:"), DelegateName ("NSTextViewSelectionShouldChange"), DefaultValueFromArgument ("null")]
		bool ShouldChangeTextInRanges (NSTextView textView, NSValue [] affectedRanges, string [] replacementStrings);

		[Export ("textView:shouldChangeTypingAttributes:toAttributes:"), DelegateName ("NSTextViewTypeAttribute"), DefaultValueFromArgument ("newTypingAttributes")]
		NSDictionary ShouldChangeTypingAttributes (NSTextView textView, NSDictionary oldTypingAttributes, NSDictionary newTypingAttributes);

		[Export ("textViewDidChangeSelection:"), EventArgs ("NSTextViewNotification")]
		void DidChangeSelection (NSNotification notification);

		[Export ("textViewDidChangeTypingAttributes:"), EventArgs ("NSTextViewNotification")]
		void DidChangeTypingAttributes (NSNotification notification);

		[Export ("textView:willDisplayToolTip:forCharacterAtIndex:"), DelegateName ("NSTextViewTooltip"), DefaultValueFromArgument ("tooltip")]
		string WillDisplayToolTip (NSTextView textView, string tooltip, uint characterIndex);

		[Export ("textView:completions:forPartialWordRange:indexOfSelectedItem:"), DelegateName ("NSTextViewCompletion"), DefaultValue (null)]
		string [] GetCompletions (NSTextView textView, string [] words, NSRange charRange, int index);

		[Export ("textView:shouldChangeTextInRange:replacementString:"), DelegateName ("NSTextViewChangeText"), DefaultValue (true)]
		bool ShouldChangeTextInRange (NSTextView textView, NSRange affectedCharRange, string replacementString);

		[Export ("textView:doCommandBySelector:"), DelegateName ("NSTextViewSelectorCommand"), DefaultValue (false)]
		bool DoCommandBySelector (NSTextView textView, Selector commandSelector);

		[Export ("textView:shouldSetSpellingState:range:"), DelegateName ("NSTextViewSpellingQuery"), DefaultValue (0)]
		int ShouldSetSpellingState (NSTextView textView, int value, NSRange affectedCharRange);

		[Export ("textView:menu:forEvent:atIndex:"), DelegateName ("NSTextViewEventMenu"), DefaultValueFromArgument ("menu")]
		NSMenu MenuForEvent (NSTextView view, NSMenu menu, NSEvent theEvent, uint charIndex);

		[Export ("textView:willCheckTextInRange:options:types:"), DelegateName ("NSTextViewOnTextCheck"), DefaultValueFromArgument ("options")]
		NSDictionary WillCheckText (NSTextView view, NSRange range, NSDictionary options, NSTextCheckingTypes checkingTypes);

		[Export ("textView:didCheckTextInRange:types:options:results:orthography:wordCount:"), DelegateName ("NSTextViewTextChecked"), DefaultValueFromArgument ("results")]
		NSTextCheckingResult [] DidCheckText (NSTextView view, NSRange range, NSTextCheckingTypes checkingTypes, NSDictionary options, NSTextCheckingResult [] results, NSOrthography orthography, int wordCount);

		[Export ("textView:draggedCell:inRect:event:"), EventArgs ("NSTextViewDraggedCell")]
		void DraggedCell (NSTextView view, NSTextAttachmentCell cell, RectangleF rect, NSEvent theevent);

		[Export ("undoManagerForTextView:"), DelegateName ("NSTextViewGetUndoManager"), DefaultValue (null)]
		NSUndoManager GetUndoManager (NSTextView view);
	}
	
	
	[BaseType (typeof (NSTextField))]
	public interface NSTokenField {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		[Export ("setTokenStyle:style")]
		void SetTokenStylestyle (NSTokenStyle style );

		[Export ("tokenStyle")]
		NSTokenStyle TokenStyle { get; }

		[Export ("setCompletionDelay:delay")]
		void SetCompletionDelaydelay (double delay );

		[Export ("completionDelay")]
		double CompletionDelay { get; }

		[Static]
		[Export ("defaultCompletionDelay")]
		double DefaultCompletionDelay { get; }

		[Static]
		[Export ("defaultTokenizingCharacterSet")]
		NSCharacterSet DefaultCharacterSet { get; }

		//Detected properties
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSTokenFieldDelegate Delegate { get; set; }

		[Export ("tokenizingCharacterSet")]
		NSCharacterSet CharacterSet { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSTokenFieldDelegate {
		[Abstract]
		[Export ("tokenField:completionsForSubstring:indexOfToken:indexOfSelectedItem:")]
		string [] GetCompletionStrings (NSTokenField tokenField, string substring, int tokenIndex, int selectedIndex);

		[Abstract]
		[Export ("tokenField:shouldAddObjects:atIndex:")]
		NSTokenField [] ShouldAddObjects (NSTokenField tokenField, NSTokenField [] tokens, uint index);

		[Abstract]
		[Export ("tokenField:displayStringForRepresentedObject:")]
		string GetDisplayString (NSTokenField tokenField, NSObject representedObject);

		[Abstract]
		[Export ("tokenField:editingStringForRepresentedObject:")]
		string GetEditingString (NSTokenField tokenField, NSObject representedObject);

		[Abstract]
		[Export ("tokenField:representedObjectForEditingString:")]
		NSObject GetRepresentedObject (NSTokenField tokenField, string editingString);

		[Abstract]
		[Export ("tokenField:writeRepresentedObjects:toPasteboard:")]
		bool WriteRepresented (NSTokenField tokenField, NSArray objects, NSPasteboard pboard);

		[Abstract]
		[Export ("tokenField:readFromPasteboard:")]
		NSObject [] Read (NSTokenField tokenField, NSPasteboard pboard);

		[Abstract]
		[Export ("tokenField:menuForRepresentedObject:")]
		NSMenu GetMenu (NSTokenField tokenField, NSObject representedObject);

		[Abstract]
		[Export ("tokenField:hasMenuForRepresentedObject:")]
		bool HasMenu (NSTokenField tokenField, NSObject representedObject);

		[Abstract]
		[Export ("tokenField:styleForRepresentedObject:")]
		NSTokenStyle GetStyle (NSTokenField tokenField, NSObject representedObject);

	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSToolbarDelegate)})]
	public interface NSToolbar {
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

		[Field ("NSToolbarSeparatorItemIdentifier")]
		NSString NSToolbarSeparatorItemIdentifier { get; }
		
		[Field ("NSToolbarSpaceItemIdentifier")]
		NSString NSToolbarSpaceItemIdentifier { get; }
		
		[Field ("NSToolbarFlexibleSpaceItemIdentifier")]
		NSString NSToolbarFlexibleSpaceItemIdentifier { get; }
		
		[Field ("NSToolbarShowColorsItemIdentifier")]
		NSString NSToolbarShowColorsItemIdentifier { get; }
		
		[Field ("NSToolbarShowFontsItemIdentifier")]
		NSString NSToolbarShowFontsItemIdentifier { get; }
		
		[Field ("NSToolbarCustomizeToolbarItemIdentifier")]
		NSString NSToolbarCustomizeToolbarItemIdentifier { get; }
		
		[Field ("NSToolbarPrintItemIdentifier")]
		NSString NSToolbarPrintItemIdentifier { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSToolbarDelegate {
		[Abstract]
		[Export ("toolbar:itemForItemIdentifier:willBeInsertedIntoToolbar:"), DelegateName ("NSToolbarWillInsert"), DefaultValue (null)]
		NSToolbarItem WillInsertItem (NSToolbar toolbar, string itemIdentifier, bool willBeInserted);

		[Abstract]
		[Export ("toolbarDefaultItemIdentifiers:"), DelegateName ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] DefaultItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarAllowedItemIdentifiers:"), DelegateName ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] AllowedItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarSelectableItemIdentifiers:"), DelegateName ("NSToolbarIdentifiers"), DefaultValue (null)]
		string [] SelectableItemIdentifiers (NSToolbar toolbar);

		[Abstract]
		[Export ("toolbarWillAddItem:"), EventArgs ("NSNotification")]
		void WillAddItem (NSNotification notification);

		[Abstract]
		[Export ("toolbarDidRemoveItem:"), EventArgs ("NSNotification")]
		void DidRemoveItem (NSNotification notification);
	}


	[BaseType (typeof (NSObject))]
	public interface NSToolbarItem {
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

		[Export ("target"), NullAllowed]
		NSObject Target { get; set; }

		[Export ("action"), NullAllowed]
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
	public interface NSTouch {
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
	public interface NSTrackingArea {
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
	
	[BaseType (typeof (NSObject))]
	public interface NSTreeNode {
		[Static, Export ("treeNodeWithRepresentedObject:")]
		NSTreeNode FromRepresentedObject (NSObject modelObject);

		[Export ("initWithRepresentedObject:")]
		IntPtr Constructor (NSObject modelObject);

		[Export ("representedObject")]
		NSTreeNode RepresentedObject { get; }

		[Export ("indexPath")]
		NSIndexPath IndexPath { get; }

		[Export ("isLeaf")]
		bool IsLeaf { get; }

		[Export ("childNodes")]
		NSTreeNode [] Children { get; }

		//[Export ("mutableChildNodes")]
		//NSMutableArray MutableChildren { get; }

		[Export ("descendantNodeAtIndexPath:")]
		NSTreeNode DescendantNode (NSIndexPath atIndexPath);

		[Export ("parentNode")]
		NSTreeNode ParentNode { get; }

		[Export ("sortWithSortDescriptors:recursively:")]
		void SortWithSortDescriptors (NSSortDescriptor [] sortDescriptors, bool recursively);

	}

	[BaseType (typeof (NSObjectController))]
	public interface NSTreeController {
		[Export ("rearrangeObjects")]
		void RearrangeObjects ();

		[Export ("arrangedObjects")]
		NSObject ArrangedObjects { get; }

		[Export ("childrenKeyPath")]
		string ChildrenKeyPath { get; set; }

		[Export ("countKeyPath")]
		string CountKeyPath { get; set; }

		[Export ("leafKeyPath")]
		string LeafKeyPath { get; set; }

		[Export ("sortDescriptors")]
		NSSortDescriptor [] SortDescriptors { get; set; }

		[Export ("content")]
		NSTreeController Content { get; set; }

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("remove:")]
		void Remove (NSObject sender);

		[Export ("addChild:")]
		void AddChild (NSObject sender);

		[Export ("insert:")]
		void Insert (NSObject sender);

		[Export ("insertChild:")]
		void InsertChild (NSObject sender);

		[Export ("canInsert")]
		bool CanInsert { get; }

		[Export ("canInsertChild")]
		bool CanInsertChild { get; }

		[Export ("canAddChild")]
		bool CanAddChild { get; }

		[Export ("insertObject:atArrangedObjectIndexPath:")]
		void InsertObject (NSObject object1, NSIndexPath indexPath);

		[Export ("insertObjects:atArrangedObjectIndexPaths:")]
		void InsertObjects (NSObject [] objects, NSArray indexPaths);

		[Export ("removeObjectAtArrangedObjectIndexPath:")]
		void RemoveObjectAtArrangedObjectIndexPath (NSIndexPath indexPath);

		[Export ("removeObjectsAtArrangedObjectIndexPaths:")]
		void RemoveObjectsAtArrangedObjectIndexPaths (NSIndexPath [] indexPaths);

		[Export ("avoidsEmptySelection")]
		bool AvoidsEmptySelection { get; set; }

		[Export ("preservesSelection")]
		bool PreservesSelection { get; set; }

		[Export ("selectsInsertedObjects")]
		bool SelectsInsertedObjects { get; set; }

		[Export ("alwaysUsesMultipleValuesMarker")]
		bool AlwaysUsesMultipleValuesMarker { get; set; }

		[Export ("selectedObjects")]
		NSObject [] SelectedObjects { get; }

		[Export ("setSelectionIndexPaths:")]
		bool SetSelectionIndexPaths (NSArray indexPaths);

		[Export ("selectionIndexPaths")]
		NSIndexPath [] SelectionIndexPaths { get; }

		[Export ("selectionIndexPath")]
		NSIndexPath SelectionIndexPath { get; set; }

		[Export ("addSelectionIndexPaths:")]
		bool AddSelectionIndexPaths (NSIndexPath [] indexPaths);

		[Export ("removeSelectionIndexPaths:")]
		bool RemoveSelectionIndexPaths (NSIndexPath [] indexPaths);

		[Export ("selectedNodes")]
		NSTreeNode [] SelectedNodes { get; }

		[Export ("moveNode:toIndexPath:")]
		void MoveNode (NSTreeNode node, NSIndexPath indexPath);

		[Export ("moveNodes:toIndexPath:")]
		void MoveNodes (NSTreeNode [] nodes, NSIndexPath startingIndexPath);

		[Export ("childrenKeyPathForNode:")]
		string ChildrenKeyPathForNode (NSTreeNode node);

		[Export ("countKeyPathForNode:")]
		string CountKeyPathForNode (NSTreeNode node);

		[Export ("leafKeyPathForNode:")]
		string LeafKeyPathForNode (NSTreeNode node);
	}

	[BaseType (typeof (NSObject))]
	public interface NSTypesetter {
		// Must bound
	}
	
	[BaseType (typeof (NSResponder), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSWindowDelegate)})]
	public interface NSWindow {
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
		IntPtr Constructor (RectangleF contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation);
	
		[Export ("initWithContentRect:styleMask:backing:defer:screen:")]
		IntPtr Constructor (RectangleF contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation, NSScreen  screen);
	
		[Export ("title")]
		string Title  { get; set; }
	
		[Export ("representedURL")]
		NSUrl RepresentedUrl { get; set; }
	
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

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }
	
		[Wrap ("WeakDelegate")][NullAllowed]
		NSWindowDelegate Delegate { get; set; }
	
		[Export ("windowNumber")]
		int WindowNumber { get; }
	
		[Export ("styleMask")]
		NSWindowStyle StyleMask { get; set; }
	
		[Export ("fieldEditor:forObject:")]
		NSText FieldEditor (bool createFlag, NSObject forObject);
	
		[Export ("endEditingFor:")]
		void EndEditingFor ([NullAllowed] NSObject anObject);
	
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
	
		/* NSWindow.Close by default calls [window release]
		 * This will cause a double free in our code since we're not aware of this
		 * and we end up GCing the proxy eventually and sending our own release
		 * Removing this method for now
		[Export ("close")]
		void Close ();
		 */
	
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
		bool HidesOnDeactivate  { get; set; }
	
		[Export ("canHide")]
		bool CanHide  { get; set; }
	
		[Export ("center")]
		void Center ();
	
		[Export ("makeKeyAndOrderFront:")]
		void MakeKeyAndOrderFront (NSObject sender);
	
		[Export ("orderFront:")]
		void OrderFront (NSObject sender);
		
		[Export ("orderBack:")]
		void OrderBack (NSObject sender);
	
		[Export ("orderOut:")]
		void OrderOut (NSObject sender);
	
		[Export ("orderWindow:relativeTo:")]
		void OrderWindow (NSWindowOrderingMode place, int relativeTo);
	
		[Export ("orderFrontRegardless")]
		void OrderFrontRegardless ();
	
		[Export ("miniwindowImage")]
		NSImage MiniWindowImage { get; set; }
	
		[Export ("miniwindowTitle")]
		string MiniWindowTitle  { get; set; }
	
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
		NSData DataWithEpsInsideRect (RectangleF rect);
	
		[Export ("dataWithPDFInsideRect:")]
		NSData DataWithPdfInsideRect (RectangleF rect);
	
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
		NSWindowLevel Level  { get; set; }
	
		[Export ("depthLimit")]
		NSWindowDepth DepthLimit  { get; set; }
	
		[Export ("dynamicDepthLimit")]
		bool HasDynamicDepthLimit { [Bind ("hasDynamicDepthLimit")] get; set; }
	
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
		void PostEvent (NSEvent theEvent, bool atStart);
	
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
		NSToolbar Toolbar { get; set; }
	
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
	
#region From NSAnimatablePropertyContainer
		[Export ("animator")]
		NSObject Animator { [return: Proxy] get; }
	
		[Export ("animations")]
		NSDictionary Animations { get; set; }
	
		[Export ("animationForKey:")]
		NSObject AnimationFor (NSString key);
	
		[Static, Export ("defaultAnimationForKey:")]
		NSObject DefaultAnimationFor (NSString key);
#endregion
	}
	
	[BaseType (typeof (NSResponder))]
	public interface NSWindowController {
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
		string WindowFrameAutosaveName { get; set; }
	
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
	public interface NSWindowDelegate {
		[Export ("windowShouldClose:"), DelegateName ("NSObjectPredicate"), DefaultValue (true)]
		bool WindowShouldClose (NSObject sender);
	
		[Export ("windowWillReturnFieldEditor:toObject:"), DelegateName ("NSWindowClient"), DefaultValue (null)]
		NSObject WillReturnFieldEditor (NSWindow  sender, NSObject client);
	
		[Export ("windowWillResize:toSize:"), DelegateName ("NSWindowResize"), DefaultValueFromArgument ("toFrameSize")]
		SizeF WillResize (NSWindow sender, SizeF toFrameSize);
	
		[Export ("windowWillUseStandardFrame:defaultFrame:"), DelegateName ("NSWindowFrame"), DefaultValueFromArgument ("newFrame")]
		RectangleF WillUseStandardFrame (NSWindow window, RectangleF newFrame);
	
		[Export ("windowShouldZoom:toFrame:"), DelegateName ("NSWindowFramePredicate"), DefaultValue (true)]
		bool ShouldZoom (NSWindow  window, RectangleF newFrame);
	
		[Export ("windowWillReturnUndoManager:"), DelegateName ("NSWindowUndoManager"), DefaultValue (null)]
		NSUndoManager WillReturnUndoManager (NSWindow  window);
	
		[Export ("window:willPositionSheet:usingRect:"), DelegateName ("NSWindowSheetRect"), DefaultValueFromArgument ("usingRect")]
		RectangleF WillPositionSheet (NSWindow  window, NSWindow  sheet, RectangleF usingRect);
	
		[Export ("window:shouldPopUpDocumentPathMenu:"), DelegateName ("NSWindowMenu"), DefaultValue (true)]
		bool ShouldPopUpDocumentPathMenu (NSWindow  window, NSMenu  menu);
	
		[Export ("window:shouldDragDocumentWithEvent:from:withPasteboard:"), DelegateName ("NSWindowDocumentDrag"), DefaultValue (true)]
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

	public delegate void NSWorkspaceUrlHandler (NSDictionary newUrls, NSError error);
	
	[BaseType (typeof (NSObject))]
	public interface NSWorkspace {
		[Static]
		[Export ("sharedWorkspace")]
		NSWorkspace SharedWorkspace { get; }
		
		[Export ("notificationCenter")]
		NSNotificationCenter NotificationCenter { get; }
		
		[Export ("openFile:")]
		bool OpenFile (string fullPath);
		
		[Export ("openFile:withApplication:")]
		bool OpenFile (string fullPath, string appName);
		
		[Export ("openFile:withApplication:andDeactivate:")]
		bool OpenFile (string fullPath, string appName, bool deactivate);
		
		[Export ("openFile:fromImage:at:inView:")]
		bool OpenFile (string fullPath, NSImage anImage, PointF point, NSView aView);
		
		[Export ("openURL:")]
		bool OpenUrl (NSUrl url);
		
		[Export ("launchApplication:")]
		bool LaunchApplication (string appName);
		
		[Export ("launchApplicationAtURL:options:configuration:error:")]
		NSRunningApplication LaunchApplication (NSUrl url, NSWorkspaceLaunchOptions options, NSDictionary configuration, NSError error );
		
		[Export ("launchApplication:showIcon:autolaunch:")]
		bool LaunchApplication (string appName, bool showIcon, bool autolaunch);
		
		[Export ("fullPathForApplication:")]
		string FullPathForApplication (string appName);
		
		[Export ("selectFile:inFileViewerRootedAtPath:")]
		bool SelectFile (string fullPath, string rootFullPath);
		
		[Export ("activateFileViewerSelectingURLs:")]
		void ActivateFileViewer (NSUrl[] fileUrls);
		
		[Export ("showSearchResultsForQueryString:")]
		bool ShowSearchResults (string queryString );
		
		[Export ("noteFileSystemChanged:")]
		void NoteFileSystemChanged (string path);
		
		[Export ("getInfoForFile:application:type:")]
		bool GetInfo (string fullPath, string appName, string type);
		
		[Export ("isFilePackageAtPath:")]
		bool IsFilePackage (string fullPath);
		
		[Export ("iconForFile:")]
		NSImage IconForFile (string fullPath);
		
		[Export ("iconForFiles:")]
		NSImage IconForFiles (string[] fullPaths);
		
		[Export ("iconForFileType:")]
		NSImage IconForFileType (string fileType);
		
		[Export ("setIcon:forFile:options:")]
		bool SetIconforFile (NSImage image, string fullPath, NSWorkspaceIconCreationOptions options);
		
		[Export ("fileLabels")]
		string[] FileLabels { get ; }
		
		[Export ("fileLabelColors")]
		NSColor[] FileLabelColors { get; }
		
		[Export ("recycleURLs:completionHandler:")]
		void RecycleUrls (NSDictionary urls, NSWorkspaceUrlHandler completionHandler);
		
		[Export ("duplicateURLs:completionHandler:")]
		void DuplicateUrls (NSDictionary urls, NSWorkspaceUrlHandler completionHandler);
		
		[Export ("getFileSystemInfoForPath:isRemovable:isWritable:isUnmountable:description:type:")]
		bool GetFileSystemInfo (string fullPath, bool removableFlag, bool writableFlag, bool unmountableFlag, string description, string fileSystemType);
		
		[Export ("performFileOperation:source:destination:files:tag:")]
		bool PerformFileOperation (string operation, string source, string destination, string[] files, int tag);
		
		[Export ("unmountAndEjectDeviceAtPath:")]
		bool UnmountAndEjectDevice(string path);

		[Export ("unmountAndEjectDeviceAtURL:error:")]
		bool UnmountAndEjectDevice (NSUrl url, out NSError error);
		
		[Export ("extendPowerOffBy:")]
		int ExtendPowerOffBy (int requested);
		
		[Export ("hideOtherApplications")]
		void HideOtherApplications ();
		
		[Export ("mountedLocalVolumePaths")]
		string[] MountedLocalVolumePaths { get; }
		
		[Export ("mountedRemovableMedia")]
		string[] MountedRemovableMedia {  get; }
		
		[Export ("URLForApplicationWithBundleIdentifier:")]
		NSUrl UrlForApplication (string bundleIdentifier );
		
		[Export ("URLForApplicationToOpenURL:")]
		NSUrl UrlForApplication (NSUrl url );
		
		[Export ("absolutePathForAppBundleWithIdentifier:")]
		string AbsolutePathForAppBundle (string bundleIdentifier);
		
		[Export ("launchAppWithBundleIdentifier:options:additionalEventParamDescriptor:launchIdentifier:")]
		bool LaunchApp (string bundleIdentifier, NSWorkspaceLaunchOptions options, NSAppleEventDescriptor descriptor, NSNumber identifier);
		
		[Export ("openURLs:withAppBundleIdentifier:options:additionalEventParamDescriptor:launchIdentifiers:")]
		bool OpenUrls (NSUrl[] urls, string bundleIdentifier, NSWorkspaceLaunchOptions options, NSAppleEventDescriptor descriptor, string[] identifiers);
		
		[Export ("launchedApplications")]
		NSDictionary [] LaunchedApplications { get; }
		
		[Export ("activeApplication")]
		NSDictionary ActiveApplication { get; }
		
		[Export ("typeOfFile:error:")]
		string TypeOfFile (string absoluteFilePath, out NSError outError);
		
		[Export ("localizedDescriptionForType:")]
		string LocalizedDescription (string typeName);
		
		[Export ("preferredFilenameExtensionForType:")]
		string PreferredFilenameExtension (string typeName);
		
		[Export ("filenameExtension:isValidForType:")]
		bool IsFilenameExtensionValid (string filenameExtension, string typeName);
		
		[Export ("type:conformsToType:")]
		bool TypeConformsTo (string firstTypeName, string secondTypeName);
		
		[Export ("setDesktopImageURL:forScreen:options:error:")]
		bool SetDesktopImageUrl (NSUrl url, NSScreen screen, NSDictionary options, NSError error );
		
		[Export ("desktopImageURLForScreen:")]
		NSUrl DesktopImageUrl (NSScreen screen );
		
		[Export ("desktopImageOptionsForScreen:")]
		NSDictionary DesktopImageOptions (NSScreen screen);		
		
		[Export ("runningApplications")]
		NSDictionary[] RunningApplications { get; }
	
	}
	
	
	[BaseType (typeof (NSObject))]
	public interface NSRunningApplication {
		[Export ("terminated")]
		bool Terminated { [Bind ("isTerminated")] get;  }
		
		[Export ("finishedLaunching")]
		bool FinishedLaunching { [Bind ("isFinishedLaunching")] get;  }
		
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get;  }
		
		[Export ("active")]
		bool Active { [Bind ("isActive")] get;  }
		
		[Export ("activationPolicy")]
		NSApplicationActivationPolicy ActivationPolicy { get;  }
		
		[Export ("localizedName")]
		string LocalizedName { get;  }
		
		[Export ("bundleIdentifier")]
		string BundleIdentifier { get;  }
		
		[Export ("bundleURL")]
		NSUrl BundleUrl { get;  }
		
		[Export ("executableURL")]
		NSUrl ExecutableUrl { get;  }
		
		// changed pid_t to int
		[Export ("processIdentifier")]
		int ProcessIdentifier { get;  }
		
		[Export ("launchDate")]
		NSDate LaunchDate { get;  }
		
		[Export ("icon")]
		NSImage Icon { get;  }
		
		[Export ("executableArchitecture")]
		int ExecutableArchitecture { get;  }
		
		[Export ("hide")]
		bool Hide { get; }
		
		[Export ("unhide")]
		bool Unhide { get; }
		
		[Export ("activateWithOptions:")]
		bool Activate (NSApplicationActivationOptions options);
		
		[Export ("terminate")]
		bool Terminate ();
		
		[Export ("forceTerminate")]
		bool ForceTerminate ();
		
		[Static]
		[Export ("runningApplicationsWithBundleIdentifier:")]
		NSRunningApplication[] GetRunningApplications (string bundleIdentifier);
		
		[Static]
		[Export ("runningApplicationWithProcessIdentifier:")]
		NSRunningApplication GetRunningApplication (int pid);
		
		[Static]
		[Export ("currentApplication")]
		NSRunningApplication CurrentApplication { get ; }
	
	}	

	[BaseType (typeof (NSControl))]
	public interface NSStepper {
		[Export ("initWithFrame:")]
		IntPtr Constructor (RectangleF frameRect);

		//Detected properties
		[Export ("minValue")]
		double MinValue { get; set; }

		[Export ("maxValue")]
		double MaxValue { get; set; }

		[Export ("increment")]
		double Increment { get; set; }

		[Export ("valueWraps")]
		bool ValueWraps { get; set; }

		[Export ("autorepeat")]
		bool Autorepeat { get; set; }

	}
	
	[BaseType (typeof (NSObject))]
	public interface NSPredicateEditorRowTemplate {
	        [Export ("matchForPredicate:")]
		double MatchForPredicate (NSPredicate predicate);

		[Export ("templateViews")]
		NSObject[] TemplateViews { get; }

		[Export ("setPredicate:")]
		NSPredicate Predicate { set; }

		[Export ("predicateWithSubpredicates:")]
		NSPredicate PredicateWithSubpredicates (NSPredicate[] subpredicates);
		
		[Export ("displayableSubpredicatesOfPredicate:")]
		NSPredicate[] DisplayableSubpredicatesOfPredicate (NSPredicate predicate);

		[Export ("initWithLeftExpressions:rightExpressions:modifier:operators:options:")]
		//NSObject InitWithLeftExpressionsrightExpressionsmodifieroperatorsoptions (NSArray leftExpressions, NSArray rightExpressions, NSComparisonPredicateModifier modifier, NSArray operators, uint options);
		IntPtr Constructor (NSExpression[] leftExpressions, NSExpression[] rightExpressions, NSComparisonPredicateModifier modifier, NSObject[] operators, NSComparisonPredicateOptions options);

		[Export ("initWithLeftExpressions:rightExpressionAttributeType:modifier:operators:options:")]
		//NSObject InitWithLeftExpressionsrightExpressionAttributeTypemodifieroperatorsoptions (NSArray leftExpressions, NSAttributeType attributeType, NSComparisonPredicateModifier modifier, NSArray operators, uint options);
		IntPtr Constructor (NSExpression[] leftExpressions, NSAttributeType attributeType, NSComparisonPredicateModifier modifier, NSObject[] operators, NSComparisonPredicateOptions options);

		[Export ("initWithCompoundTypes:")]
		IntPtr Constructor (NSNumber[] compoundTypes);

		[Export ("leftExpressions")]
		NSExpression[] LeftExpressions { get; }

		[Export ("rightExpressions")]
		NSExpression[] RightExpressions { get; }

		[Export ("rightExpressionAttributeType")]
		NSAttributeType RightExpressionAttributeType { get; }

		[Export ("modifier")]
		NSComparisonPredicateModifier Modifier { get; }

		[Export ("operators")]
		NSObject[] Operators { get; }

		[Export ("options")]
		NSComparisonPredicateOptions Options { get; }

		[Export ("compoundTypes")]
		NSNumber[] CompoundTypes { get; }

		[Static]
		[Export ("templatesWithAttributeKeyPaths:inEntityDescription:")]
		//NSArray TemplatesWithAttributeKeyPathsinEntityDescription (NSArray keyPaths, NSEntityDescription entityDescription);
		NSPredicateEditorRowTemplate[] GetTemplates (string[] keyPaths, NSEntityDescription entityDescription);

	}
   
	[BaseType (typeof (NSControl), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSRuleEditorDelegate)})]
	public interface NSRuleEditor {
		[Export ("reloadCriteria")]
		void ReloadCriteria ();

		[Export ("predicate")]
		NSPredicate Predicate { get; }

		[Export ("reloadPredicate")]
		void ReloadPredicate ();

		[Export ("predicateForRow:")]
		NSPredicate GetPredicate (int row);

		[Export ("numberOfRows")]
		int NumberOfRows { get; }

		[Export ("subrowIndexesForRow:")]
		NSIndexSet SubrowIndexes (int rowIndex);

		[Export ("criteriaForRow:")]
		NSArray Criteria (int row);

		[Export ("displayValuesForRow:")]
		NSObject[] DisplayValues (int row);

		[Export ("rowForDisplayValue:")]
		int Row (NSObject displayValue);

		[Export ("rowTypeForRow:")]
		NSRuleEditorRowType RowType (int rowIndex);

		[Export ("parentRowForRow:")]
		int ParentRow (int rowIndex);

		[Export ("addRow:")]
		void AddRow (NSObject sender);

		[Export ("insertRowAtIndex:withType:asSubrowOfRow:animate:")]
		void InsertRowAtIndex (int rowIndex, NSRuleEditorRowType rowType, int parentRow, bool shouldAnimate);

		[Export ("setCriteria:andDisplayValues:forRowAtIndex:")]
		void SetCriteria (NSArray criteria, NSArray values, int rowIndex);

		[Export ("removeRowAtIndex:")]
		void RemoveRowAtIndex (int rowIndex);

		[Export ("removeRowsAtIndexes:includeSubrows:")]
		void RemoveRowsAtIndexes (NSIndexSet rowIndexes, bool includeSubrows);

		[Export ("selectedRowIndexes")]
		NSIndexSet SelectedRows { get; }

		[Export ("selectRowIndexes:byExtendingSelection:")]
		void SelectRows (NSIndexSet indexes, bool extend);

		//Detected properties
		//[Export ("delegate")]
		//NSRuleEditorDelegate Delegate { get; set; }
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSRuleEditorDelegate WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSRuleEditorDelegate Delegate { get; set; }
       
		[Export ("formattingStringsFilename")]
		string FormattingStringsFilename { get; set; }

		[Export ("formattingDictionary")]
		NSDictionary FormattingDictionary { get; set; }

		[Export ("nestingMode")]
		NSRuleEditorNestingMode NestingMode { get; set; }

		[Export ("rowHeight")]
		float RowHeight { get; set; }

		[Export ("editable")]
		bool Editable { [Bind ("isEditable")]get; set; }

		[Export ("canRemoveAllRows")]
		bool CanRemoveAllRows { get; set; }

		[Export ("rowClass")]
		Class RowClass { get; set; }

		[Export ("rowTypeKeyPath")]
		string RowTypeKeyPath { get; set; }

		[Export ("subrowsKeyPath")]
		string SubrowsKeyPath { get; set; }

		[Export ("criteriaKeyPath")]
		string CriteriaKeyPath { get; set; }

		[Export ("displayValuesKeyPath")]
		string DisplayValuesKeyPath { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSRuleEditorDelegate {
		[Abstract]
		[Export ("ruleEditor:numberOfChildrenForCriterion:withRowType:"), DelegateName ("NSRuleEditorNumberOfChildren"), DefaultValue(0)]
		int NumberOfChildren (NSRuleEditor editor, NSObject criterion, NSRuleEditorRowType rowType);

		[Abstract]
		[Export ("ruleEditor:child:forCriterion:withRowType:"), DelegateName ("NSRulerEditorChildCriterion"), DefaultValue(null)]
		NSObject ChildForCriterion (NSRuleEditor editor, int index, NSObject criterion, NSRuleEditorRowType rowType);

		[Abstract]
		[Export ("ruleEditor:displayValueForCriterion:inRow:"), DelegateName ("NSRulerEditorDisplayValue"), DefaultValue(null)]
		NSObject DisplayValue (NSRuleEditor editor, NSObject criterion, int row);

		[Abstract]
		[Export ("ruleEditor:predicatePartsForCriterion:withDisplayValue:inRow:"), DelegateName ("NSRulerEditorPredicateParts"), DefaultValue(null)]
		NSDictionary PredicateParts (NSRuleEditor editor, NSObject criterion, NSObject value, int row);

		[Abstract]
		[Export ("ruleEditorRowsDidChange:"), EventArgs ("NSNotification")]
		void RowsDidChange (NSNotification notification);
		
		[Export ("controlTextDidEndEditing:"), EventArgs ("NSNotification")]
		void EditingEnded (NSNotification notification);

		[Export ("controlTextDidChange:"), EventArgs ("NSNotification")]
		void Changed (NSNotification notification);

		[Export ("controlTextDidBeginEditing:"), EventArgs ("NSNotification")]
		void EditingBegan (NSNotification notification);			

	}
   
	[BaseType (typeof (NSRuleEditor))]
	public interface NSPredicateEditor {
		//Detected properties
		[Export ("rowTemplates")]
		NSPredicateEditorRowTemplate[] RowTemplates { get; set; }

	} 	
}
