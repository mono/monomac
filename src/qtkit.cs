//
// Copyright 2010, Novell, Inc.
// Copyright 2010, Duane Wandless.
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.AppKit;
using System.Drawing;

namespace MonoMac.QTKit
{
	[BaseType (typeof (NSView))]
	interface QTMovieView {

		[Export ("movie")]
		QTMovie Movie { get; set; }
		
		[Export ("isControllerVisible")]
		bool IsControllerVisible { get; [Bind("setControllerVisible:")] set; }
		
		[Export ("isEditable")]
		bool Editable { get; [Bind("setEditable:")] set; }

		[Export ("controllerBarHeight")]
		float ControllerBarHeight { get; }
		
		[Export ("preservesAspectRatio")]
		bool PreservesAspectRatio { get; set; }
		
		[Export ("fillColor")]
		NSColor FillColor { get; set; }
			
		[Export ("movieBounds")]
		RectangleF MovieBounds { get; }

		[Export ("movieControllerBounds")]
		RectangleF MovieControllerBounds { get; }
		
		[Export ("setShowsResizeIndicator:")]
		void SetShowsResizeIndicator (bool show);
		
		[Export ("play:")]
		void Play (NSObject sender);

		[Export ("pause:")]
		void Pause (NSObject sender);

		[Export ("gotoBeginning:")]
		void GotoBeginning (NSObject sender);

		[Export ("gotoEnd:")]
		void GotoEnd (NSObject sender);

		[Export ("gotoNextSelectionPoint:")]
		void GotoNextSelectionPoint (NSObject sender);

		[Export ("gotoPreviousSelectionPoint:")]
		void GotoPreviousSelectionPoint (NSObject sender);

		[Export ("gotoPosterFrame:")]
		void GotoPosterFrame (NSObject sender);

		[Export ("stepForward:")]
		void StepForward (NSObject sender);

		[Export ("stepBackward:")]
		void StepBackward (NSObject sender);

		[Export ("cut:")]
		void Cut (NSObject sender);

		[Export ("copy:")]
		void Copy (NSObject sender);

		[Export ("paste:")]
		void Paste (NSObject sender);

		[Export ("selectAll:")]
		void SelectAll (NSObject sender);

		[Export ("selectNone:")]
		void SelectNone (NSObject sender);

		[Export ("delete:")]
		void Delete (NSObject sender);

		[Export ("add:")]
		void Add (NSObject sender);

		[Export ("addScaled:")]
		void AddScaled (NSObject sender);

		[Export ("replace:")]
		void Replace (NSObject sender);

		[Export ("trim:")]
		void Trim (NSObject sender);
		
		[Export ("backButtonVisible")]
		bool BackButtonVisible { [Bind ("isBackButtonVisible")] get; set; }

		[Export ("customButtonVisible")]
		bool CustomButtonVisible { [Bind ("isCustomButtonVisible")] get; set; }

		[Export ("hotSpotButtonVisible")]
		bool HotSpotButtonVisible { [Bind ("isHotSpotButtonVisible")] get; set; }

		[Export ("stepButtonsVisible")]
		bool SetStepButtonsVisible { [Bind ("areStepButtonsVisible")] get; set; }

		[Export ("translateButtonVisible")]
		bool TranslateButtonVisible { [Bind ("isTranslateButtonVisible")] get; set; }

		[Export ("volumeButtonVisible")]
		bool VolumeButtonVisible { [Bind ("isVolumeButtonVisible")] get; set; }

		[Export ("zoomButtonsVisible")]
		bool ZoomButtonsVisible { [Bind ("areZoomButtonsVisible")] get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		QTMovieViewDelegate Delegate { get; set; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface QTMovieViewDelegate {
		[Export ("view:willDisplayImage:")]
		CIImage ViewWillDisplayImage (QTMovieView view, CIImage image);
	}
	
	[BaseType (typeof (NSObject))]
	interface QTMovie {
		[Export ("duration")]
		QTTime Duration { get; }

		[Static, Export ("canInitWithPasteboard:")]
		bool CanInitWithPasteboard (NSPasteboard pasteboard);

		[Static, Export ("canInitWithFile:")]
		bool CanInitWithFile (string fileName);

		[Static, Export ("canInitWithURL:")]
		bool CanInitWithUrl (NSUrl url);

		//[Static, Export ("canInitWithDataReference:")]
		//bool CanInitWithDataReference (QTDataReference dataReference);

		[Static, Export ("movieFileTypes:")]
		string[] MovieFileTypes (QTMovieFileTypeOptions types);

		[Static, Export ("movieUnfilteredFileTypes")]
		string[] MovieUnfilteredFileTypes ();

		//+ (NSArray *)movieUnfilteredPasteboardTypes;
		[Static, Export ("movieUnfilteredPasteboardTypes")]
		string[] MovieUnfilteredPasteboardTypes ();

		[Static, Export ("movieTypesWithOptions:")]
		string[] MovieTypesWithOptions ([Target] QTMovie qTMovieInitialization, QTMovieFileTypeOptions types);

		[Static, Export ("movie")]
		QTMovie Movie { get; }

		[Static, Export ("movieWithFile:error:")]
		QTMovie MovieWithFileError (string fileName, out NSError errorPtr);

		[Static, Export ("movieWithURL:error:")]
		QTMovie MovieWithURLError (NSUrl url, out NSError errorPtr);

		//[Static, Export ("movieWithDataReference:error:")]
		//QTMovie MovieWithDataReferenceError (QTDataReference dataReference, out NSError errorPtr);

		[Static, Export ("movieWithPasteboard:error:")]
		QTMovie MovieWithPasteboardError (NSPasteboard pasteboard, out NSError errorPtr);

		[Static, Export ("movieWithData:error:")]
		QTMovie MovieWithDataError (NSData data, out NSError errorPtr);

//		[Static, Export ("movieWithQuickTimeMovie:disposeWhenDone:error:")]
//		QTMovie MovieWithQuickTimeMovieDisposeWhenDone (Movie movie, bool dispose, out NSError errorPtr);

		[Static, Export ("movieWithAttributes:error:")]
		QTMovie MovieWithAttributesError (NSDictionary attributes, out NSError errorPtr);

		[Static, Export ("movieNamed:error:")]
		QTMovie MovieNamedError (string name, out NSError errorPtr);

		//- (id)initWithFile:(NSString *)fileName error:(NSError **)errorPtr;
//		[Export ("initWithFile:error:")]
//		IntPtr Constructor (string fileName, out NSError errorPtr);

		//- (id)initWithURL:(NSURL *)url error:(NSError **)errorPtr;
//		[Export ("initWithURL:error:")]
//		IntPtr Constructor (NSUrl url, out NSError errorPtr);

		//- (id)initWithDataReference:(QTDataReference *)dataReference error:(NSError **)errorPtr;
//		[Export ("initWithDataReference:error:")]
//		IntPtr Constructor (QTDataReference dataReference, out NSError errorPtr);

		//- (id)initWithPasteboard:(NSPasteboard *)pasteboard error:(NSError **)errorPtr;
//		[Export ("initWithPasteboard:error:")]
//		IntPtr Constructor (NSPasteboard pasteboard, out NSError errorPtr);

		//- (id)initWithData:(NSData *)data error:(NSError **)errorPtr;
//		[Export ("initWithData:error:")]
//		IntPtr Constructor (NSData data, out NSError errorPtr);

		//- (id)initWithMovie:(QTMovie *)movie timeRange:(QTTimeRange)range error:(NSError **)errorPtr;
//		[Export ("initWithMovie:timeRange:error:")]
//		IntPtr Constructor (QTMovie movie, QTTimeRange range, out NSError errorPtr);

		//- (id)initWithQuickTimeMovie:(Movie)movie disposeWhenDone:(BOOL)dispose error:(NSError **)errorPtr;
//		[Export ("initWithQuickTimeMovie:disposeWhenDone:error:")]
//		IntPtr Constructor ([Movie movie, bool dispose, out NSError errorPtr);

		//- (id)initWithAttributes:(NSDictionary *)attributes error:(NSError **)errorPtr;
//		[Export ("initWithAttributes:error:")]
//		IntPtr Constructor (NSDictionary attributes, out NSError errorPtr);

//		[Static, Export ("movieWithTimeRange:error:")]
//		QTMovie MovieWithTimeRangeError (QTTimeRange range, out NSError errorPtr);

		//- (id)initToWritableFile:(NSString *)filename error:(NSError **)errorPtr;
//		[Export ("initToWritableFile:error:")]
//		IntPtr Constructor (string filename, out NSError errorPtr);

		//- (id)initToWritableData:(NSMutableData *)data error:(NSError **)errorPtr;
//		[Export ("initToWritableData:error:")]
//		IntPtr Constructor (NSMutableData data, out NSError errorPtr);

		//- (id)initToWritableDataReference:(QTDataReference *)dataReference error:(NSError **)errorPtr;
//		[Export ("initToWritableDataReference:error:")]
//		IntPtr Constructor (QTDataReference dataReference, out NSError errorPtr);

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("currentTime")]
		QTTime CurrentTime { get; }

		[Export ("rate")]
		float Rate { get; }

		[Export ("volume")]
		float Volume { get; }

		[Export ("muted")]
		bool Muted { get; }
		
		[Export ("movieAttributes")]
		NSDictionary MovieAttributes { get; }

		[Export ("attributeForKey:")]
		NSObject AttributeForKey (string attributeKey);

		[Export ("setAttribute:forKey:")]
		void SetAttributeForKey (NSObject value, string attributeKey);

//		[Export ("tracks")]
//		QTTrack[] Tracks { get; }

//		[Export ("tracksOfMediaType:")]
//		QTTrack[] TracksOfMediaType (string type);

		[Export ("posterImage")]
		NSImage PosterImage { get; }

		[Export ("currentFrameImage")]
		NSImage CurrentFrameImage { get; }

//		[Export ("frameImageAtTime:")]
//		NSImage FrameImageAtTime (QTTime time);

//		[Export ("frameImageAtTime:withAttributes:error:")]
//		void FrameImageAtTime (QTTime time, NSDictionary attributes, out NSError errorPtr);

		[Export ("movieFormatRepresentation")]
		NSData MovieFormatRepresentation ();

		[Export ("writeToFile:withAttributes:")]
		bool WriteToFile (string fileName, NSDictionary attributes);

		[Export ("writeToFile:withAttributes:error:")]
		bool WriteToFile (string fileName, NSDictionary attributes, out NSError errorPtr);

		[Export ("canUpdateMovieFile")]
		bool CanUpdateMovieFile { get; }

		[Export ("updateMovieFile")]
		bool UpdateMovieFile ();

		[Export ("autoplay")]
		void Autoplay ();

		[Export ("play")]
		void Play ();

		[Export ("stop")]
		void Stop ();

		[Export ("gotoBeginning")]
		void GotoBeginning ();

		[Export ("gotoEnd")]
		void GotoEnd ();

		[Export ("gotoNextSelectionPoint")]
		void GotoNextSelectionPoint ();

		[Export ("gotoPreviousSelectionPoint")]
		void GotoPreviousSelectionPoint ();

		[Export ("gotoPosterTime")]
		void GotoPosterTime ();

		[Export ("stepForward")]
		void StepForward ();

		[Export ("stepBackward")]
		void StepBackward ();

		[Export ("setSelection:")]
		void SetSelection (QTTimeRange selection);

		[Export ("selectionStart")]
		QTTime SelectionStart ();

		[Export ("selectionEnd")]
		QTTime SelectionEnd ();

		[Export ("selectionDuration")]
		QTTime SelectionDuration ();

		[Export ("replaceSelectionWithSelectionFromMovie:")]
		void ReplaceSelectionWithSelectionFromMovie (QTMovie movie);

		[Export ("appendSelectionFromMovie:")]
		void AppendSelectionFromMovie (QTMovie movie);

		[Export ("insertSegmentOfMovie:timeRange:atTime:")]
		void InsertSegmentOfMovieTimeRange (QTMovie movie, QTTimeRange range, QTTime time);

		[Export ("insertSegmentOfMovie:fromRange:scaledToRange:")]
		void InsertSegmentOfMovieFromRange (QTMovie movie, QTTimeRange srcRange, QTTimeRange dstRange);

		[Export ("insertEmptySegmentAt:")]
		void InsertEmptySegmentAt (QTTimeRange range);

		[Export ("deleteSegment:")]
		void DeleteSegment (QTTimeRange segment);

		[Export ("scaleSegment:newDuration:")]
		void ScaleSegmentNewDuration (QTTimeRange segment, QTTime newDuration);

		[Export ("addImage:forDuration:withAttributes:")]
		void AddImageForDuration (NSImage image, QTTime duration, NSDictionary attributes);

		[Export ("insertSegmentOfTrack:timeRange:atTime:")]
		QTTrack InsertSegmentOfTrackTimeRange (QTTrack track, QTTimeRange range, QTTime time);

		[Export ("insertSegmentOfTrack:fromRange:scaledToRange:")]
		QTTrack InsertSegmentOfTrackFromRange (QTTrack track, QTTimeRange srcRange, QTTimeRange dstRange);

		[Export ("removeTrack:")]
		void RemoveTrack (QTTrack track);

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		//[Wrap ("WeakDelegate")]
		//QTMovieDelegate Delegate { get; set; }

		//- (Movie)quickTimeMovie;
//		[Export ("quickTimeMovie")]
//		Movie QuickTimeMovie ();

		//- (MovieController)quickTimeMovieController;
//		[Export ("quickTimeMovieController")]
//		MovieController QuickTimeMovieController ();

		//- (void)generateApertureModeDimensions;
		[Export ("generateApertureModeDimensions")]
		void GenerateApertureModeDimensions ();

		//- (void)removeApertureModeDimensions;
		[Export ("removeApertureModeDimensions")]
		void RemoveApertureModeDimensions ();

		//- (QTVisualContextRef)visualContext;
//		[Export ("visualContext")]
//		QTVisualContextRef VisualContext ();

		[Static, Export ("enterQTKitOnThread")]
		void EnterQTKitOnThread ();

		[Static, Export ("enterQTKitOnThreadDisablingThreadSafetyProtection")]
		void EnterQTKitOnThreadDisablingThreadSafetyProtection ();

		[Static, Export ("exitQTKitOnThread")]
		void ExitQTKitOnThread ();

		[Export ("attachToCurrentThread")]
		bool AttachToCurrentThread ();

		[Export ("detachFromCurrentThread")]
		bool DetachFromCurrentThread ();

		[Export ("isIdling")]
		bool Idling { get; }

		[Export ("hasChapters")]
		bool HasChapters { get; }

		[Export ("chapterCount")]
		int ChapterCount { get; }

		[Export ("chapters")]
		NSDictionary[] Chapters ();

//		[Export ("addChapters:withAttributes:error:")]
//		void AddChaptersWithAttributes (NSArray chapters, NSDictionary attributes, out NSError errorPtr);

		[Export ("removeChapters")]
		bool RemoveChapters ();

		[Export ("startTimeOfChapter:")]
		QTTime StartTimeOfChapter (int chapterIndex);

		[Export ("chapterIndexForTime:")]
		int ChapterIndexForTime (QTTime time);
	}

	
	[BaseType (typeof (NSObject))]
	interface QTTrack {
	}
}
