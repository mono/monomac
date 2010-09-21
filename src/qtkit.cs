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
		void Play (IntPtr sender);

		[Export ("pause:")]
		void Pause (IntPtr sender);

		[Export ("gotoBeginning:")]
		void GotoBeginning (IntPtr sender);

		[Export ("gotoEnd:")]
		void GotoEnd (IntPtr sender);

		[Export ("gotoNextSelectionPoint:")]
		void GotoNextSelectionPoint (IntPtr sender);

		[Export ("gotoPreviousSelectionPoint:")]
		void GotoPreviousSelectionPoint (IntPtr sender);

		[Export ("gotoPosterFrame:")]
		void GotoPosterFrame (IntPtr sender);

		[Export ("stepForward:")]
		void StepForward (IntPtr sender);

		[Export ("stepBackward:")]
		void StepBackward (IntPtr sender);

		[Export ("cut:")]
		void Cut (IntPtr sender);

		[Export ("copy:")]
		void Copy (IntPtr sender);

		[Export ("paste:")]
		void Paste (IntPtr sender);

		[Export ("selectAll:")]
		void SelectAll (IntPtr sender);

		[Export ("selectNone:")]
		void SelectNone (IntPtr sender);

		[Export ("delete:")]
		void Delete (IntPtr sender);

		[Export ("add:")]
		void Add (IntPtr sender);

		[Export ("addScaled:")]
		void AddScaled (IntPtr sender);

		[Export ("replace:")]
		void Replace (IntPtr sender);

		[Export ("trim:")]
		void Trim (IntPtr sender);
		
		[Export ("setBackButtonVisible:")]
		void SetBackButtonVisible (bool state);

		[Export ("setCustomButtonVisible:")]
		void SetCustomButtonVisible (bool state);

		[Export ("setHotSpotButtonVisible:")]
		void SetHotSpotButtonVisible (bool state);

		[Export ("setStepButtonsVisible:")]
		void SetStepButtonsVisible (bool state);

		[Export ("setTranslateButtonVisible:")]
		void SetTranslateButtonVisible (bool state);

		[Export ("setVolumeButtonVisible:")]
		void SetVolumeButtonVisible (bool state);

		[Export ("setZoomButtonsVisible:")]
		void SetZoomButtonsVisible (bool state);

		[Export ("isBackButtonVisible")]
		bool IsBackButtonVisible { get; }

		[Export ("isCustomButtonVisible")]
		bool IsCustomButtonVisible { get; }

		[Export ("isHotSpotButtonVisible")]
		bool IsHotSpotButtonVisible { get; }

		[Export ("areStepButtonsVisible")]
		bool AreStepButtonsVisible { get; }

		[Export ("isTranslateButtonVisible")]
		bool IsTranslateButtonVisible { get; }

		[Export ("isVolumeButtonVisible")]
		bool IsVolumeButtonVisible { get; }

		[Export ("areZoomButtonsVisible")]
		bool AreZoomButtonsVisible { get; }

		[Export ("delegate")]
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

//		[Export ("duration")]
//		QTTime Duration { get; }

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
		QTMovie Movie ();

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

//		[Export ("movieWithTimeRange:error:")]
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

//		[Export ("currentTime")]
//		QTTime CurrentTime { get; }

		[Export ("rate")]
		float Rate { get; }

		[Export ("volume")]
		float Volume { get; }

		[Export ("muted")]
		bool Muted { get; }
		
		[Export ("movieAttributes")]
		NSDictionary MovieAttributes { get; }

		[Export ("attributeForKey:")]
		IntPtr AttributeForKey (string attributeKey);

		[Export ("setAttribute:forKey:")]
		void SetAttributeForKey (IntPtr value, string attributeKey);

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

//		[Export ("setSelection:")]
//		void SetSelection (QTTimeRange selection);

//		[Export ("selectionStart")]
//		QTTime SelectionStart ();

//		[Export ("selectionEnd")]
//		QTTime SelectionEnd ();

//		[Export ("selectionDuration")]
//		QTTime SelectionDuration ();

		[Export ("replaceSelectionWithSelectionFromMovie:")]
		void ReplaceSelectionWithSelectionFromMovie (QTMovie movie);

		[Export ("appendSelectionFromMovie:")]
		void AppendSelectionFromMovie (QTMovie movie);

//		[Export ("insertSegmentOfMovie:timeRange:atTime:")]
//		void InsertSegmentOfMovieTimeRange (QTMovie movie, QTTimeRange range, QTTime time);

//		[Export ("insertSegmentOfMovie:fromRange:scaledToRange:")]
//		void InsertSegmentOfMovieFromRange (QTMovie movie, QTTimeRange srcRange, QTTimeRange dstRange);

//		[Export ("insertEmptySegmentAt:")]
//		void InsertEmptySegmentAt (QTTimeRange range);

//		[Export ("deleteSegment:")]
//		void DeleteSegment (QTTimeRange segment);

//		[Export ("scaleSegment:newDuration:")]
//		void ScaleSegmentNewDuration (QTTimeRange segment, QTTime newDuration);

//		[Export ("addImage:forDuration:withAttributes:")]
//		void AddImageForDuration (NSImage image, QTTime duration, NSDictionary attributes);

//		[Export ("insertSegmentOfTrack:timeRange:atTime:")]
//		QTTrack InsertSegmentOfTrackTimeRange (QTTrack track, QTTimeRange range, QTTime time);

//		[Export ("insertSegmentOfTrack:fromRange:scaledToRange:")]
//		QTTrack InsertSegmentOfTrackFromRange (QTTrack track, QTTimeRange srcRange, QTTimeRange dstRange);

//		[Export ("removeTrack:")]
//		void RemoveTrack (QTTrack track);

//		[Export ("delegate")]
//		QTMovieDelegate Delegate { get; set; }

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

//		[Export ("startTimeOfChapter:")]
//		QTTime StartTimeOfChapter (int chapterIndex);

//		[Export ("chapterIndexForTime:")]
//		int ChapterIndexForTime (QTTime time);

	}
}
