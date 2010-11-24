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

// TODO:
//   API: QTCaptureDevice - hide the NSDictionary with attribtues, and instead
//        expose a C# type, hide all fields
//   API: QTSampleBuffer.h expose a couple of AudioBufferList methods 
//   API: QTMovie expose a bunch of commented out methods
//
//   QTCaptureAudioPreviewOutput.h
//   QTCaptureDecompressedAudioOutput.h
//   QTCaptureDecompressedVideoOutput.h
//   QTCaptureLayer.h
//   QTCaptureMovieFileOutput.h
//   QTCaptureVideoPreviewOutput.h
//   QTDataReference.h
//   QTError.h
//   QTKit.h
//   QTKitDefines.h
//   QTMedia.h
//   QTMovieLayer.h
//   QTTime.h
//   QTTimeRange.h
//   QTTrack.h
//   QTUtilities.h

using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.AppKit;
using System.Drawing;

namespace MonoMac.QTKit
{
	[BaseType (typeof (NSObject))]
	interface QTCaptureConnection {
		[Export ("owner")]
		NSObject Owner { get; }

		[Export ("mediaType")]
		string MediaType { get; }

		[Export ("formatDescription")]
		QTFormatDescription FormatDescription { get; }

		[Export ("attributeIsReadOnly:")]
		bool IsAttributeReadOnly (string attributeKey);

		[Export ("attributeForKey:")]
		NSObject GetAttribute (string attributeKey);

		[Export ("setAttribute:forKey:")]
		void SetAttribute (NSObject attribute, string key);

		//Detected properties
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")]get; set; }

		[Export ("connectionAttributes")]
		NSDictionary ConnectionAttributes { get; set; }

		[Field ("QTCaptureConnectionFormatDescriptionWillChangeNotification")]
		NSString FormatDescriptionWillChangeNotification { get; }

		[Field ("QTCaptureConnectionFormatDescriptionDidChangeNotification")]
		NSString FormatDescriptionDidChangeNotification { get; }

		[Field ("QTCaptureConnectionAttributeWillChangeNotification")]
		NSString AttributeWillChangeNotification { get; }

		[Field ("QTCaptureConnectionAttributeDidChangeNotification")]
		NSString AttributeDidChangeNotification { get; }

		[Field ("QTCaptureConnectionChangedAttributeKey")]
		NSString ChangedAttributeKey { get; }

		[Field ("QTCaptureConnectionAudioAveragePowerLevelsAttribute")]
		NSString AudioAveragePowerLevelsAttribute { get; }

		[Field ("QTCaptureConnectionAudioPeakHoldLevelsAttribute")]
		NSString AudioPeakHoldLevelsAttribute { get; }

		[Field ("QTCaptureConnectionAudioMasterVolumeAttribute")]
		NSString AudioMasterVolumeAttribute { get; }

		[Field ("QTCaptureConnectionAudioVolumesAttribute")]
		NSString AudioVolumesAttribute { get; }

		[Field ("QTCaptureConnectionEnabledAudioChannelsAttribute")]
		NSString EnabledAudioChannelsAttribute { get; }
	}

	[BaseType (typeof (NSObject))]
	interface QTCaptureDevice {
		[Static]
		[Export ("inputDevices")]
		QTCaptureDevice [] InputDevices { get; }

		[Static]
		[Export ("inputDevicesWithMediaType:")]
		QTCaptureDevice [] GetInputDevices (string forMediaType);

		[Static]
		[Export ("defaultInputDeviceWithMediaType:")]
		QTCaptureDevice GetDefaultInputDevice (string forMediaType);

		[Static]
		[Export ("deviceWithUniqueID:")]
		QTCaptureDevice FromUniqueID (string deviceUniqueID);

		[Export ("uniqueID")]
		string UniqueID { get; }

		[Export ("modelUniqueID")]
		string ModelUniqueID { get; }

		[Export ("localizedDisplayName")]
		string LocalizedDisplayName { get; }

		[Export ("formatDescriptions")]
		QTFormatDescription [] FormatDescriptions { get; }

		[Export ("hasMediaType:")]
		bool HasMediaType (string mediaType);

		[Export ("attributeIsReadOnly:")]
		bool IsAttributeReadOnly (string attributeKey);

		[Export ("attributeForKey:")]
		NSObject GetAttribute (string attributeKey);

		[Export ("setAttribute:forKey:")]
		void SetAttribute (NSObject attribute, string attributeKey);

		[Export ("isConnected")]
		bool IsConnected { get; }

		[Export ("isInUseByAnotherApplication")]
		bool IsInUseByAnotherApplication { get; }

		[Export ("isOpen")]
		bool IsOpen { get; }

		[Export ("open:")]
		bool Open (out NSError error);

		[Export ("close")]
		void Close ();

		//Detected properties
		[Export ("deviceAttributes")]
		NSDictionary DeviceAttributes { get; set; }

		[Field ("QTCaptureDeviceWasConnectedNotification")]
		NSString WasConnectedNotification { get; }
		
		[Field ("QTCaptureDeviceWasDisconnectedNotification")]
		NSString WasDisconnectedNotification { get; }

		[Field ("QTCaptureDeviceFormatDescriptionsWillChangeNotification")]
		NSString FormatDescriptionsWillChangeNotification { get; }
		
		[Field ("QTCaptureDeviceFormatDescriptionsDidChangeNotification")]
		NSString FormatDescriptionsDidChangeNotification { get; }
			
		[Field ("QTCaptureDeviceAttributeWillChangeNotification")]
		NSString AttributeWillChangeNotification { get; }
		
		[Field ("QTCaptureDeviceAttributeDidChangeNotification")]
		NSString AttributeDidChangeNotification { get; }

		[Field ("QTCaptureDeviceChangedAttributeKey")]
		NSString ChangedAttributeKey { get; }

		[Field ("QTCaptureDeviceSuspendedAttribute")]
		NSString SuspendedAttribute { get; }
		
		[Field ("QTCaptureDeviceLinkedDevicesAttribute")]
		NSString LinkedDevicesAttribute { get; }
		
		[Field ("QTCaptureDeviceAvailableInputSourcesAttribute")]
		NSString AvailableInputSourcesAttribute { get; }
		
		[Field ("QTCaptureDeviceInputSourceIdentifierAttribute")]
		NSString InputSourceIdentifierAttribute { get; }
		
		[Field ("QTCaptureDeviceInputSourceIdentifierKey")]
		NSString InputSourceIdentifierKey { get; }
		
		[Field ("QTCaptureDeviceInputSourceLocalizedDisplayNameKey")]
		NSString InputSourceLocalizedDisplayNameKey { get; }
		
		[Field ("QTCaptureDeviceLegacySequenceGrabberAttribute")]
		NSString LegacySequenceGrabberAttribute { get; }
		
		[Field ("QTCaptureDeviceAVCTransportControlsAttribute")]
		NSString AVCTransportControlsAttribute { get; }
		
		[Field ("QTCaptureDeviceAVCTransportControlsPlaybackModeKey")]
		NSString AVCTransportControlsPlaybackModeKey { get; }
		
		[Field ("QTCaptureDeviceAVCTransportControlsSpeedKey")]
		NSString AVCTransportControlsSpeedKey { get; }
		
	}

	[BaseType (typeof (QTCaptureInput))]
	interface QTCaptureDeviceInput {
		[Export ("deviceInputWithDevice:")]
		QTCaptureDeviceInput FromDevice (QTCaptureDevice device);

		[Export ("initWithDevice")]
		IntPtr Constructor (QTCaptureDevice device);
		
		[Export ("device")]
		QTCaptureDevice Device { get; }
	}

	[BaseType (typeof (QTCaptureOutput), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (QTCaptureFileOutputDelegate)})]
	interface QTCaptureFileOutput {
		[Export ("outputFileURL")]
		NSUrl OutputFileUrl { get; }

		[Export ("recordToOutputFileURL:")]
		void RecordToOutputFile (NSUrl url);

		[Export ("recordToOutputFileURL:bufferDestination:")]
		void RecordToOutputFile (NSUrl url, QTCaptureDestination bufferDestination);

		[Export ("isRecordingPaused")]
		bool IsRecordingPaused { get; }

		[Export ("pauseRecording")]
		void PauseRecording ();

		[Export ("resumeRecording")]
		void ResumeRecording ();

		[Export ("compressionOptionsForConnection:")]
		QTCompressionOptions GetCompressionOptions (QTCaptureConnection forConnection);

		[Export ("setCompressionOptions:forConnection:")]
		void SetCompressionOptions (QTCompressionOptions compressionOptions, QTCaptureConnection forConnection);

		[Export ("recordedDuration")]
		QTTime RecordedDuration { get; }

		[Export ("recordedFileSize")]
		UInt64 RecordedFileSize { get; }

		//Detected properties
		[Export ("maximumVideoSize")]
		SizeF MaximumVideoSize { get; set; }

		[Export ("minimumVideoFrameInterval")]
		double MinimumVideoFrameInterval { get; set; }

		[Export ("maximumRecordedDuration")]
		QTTime MaximumRecordedDuration { get; set; }

		[Export ("maximumRecordedFileSize")]
		UInt64 MaximumRecordedFileSize { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		QTCaptureFileOutputDelegate Delegate { get; set; }

	}

	[BaseType (typeof (NSObject), Name="QTCaptureFileOutput_Delegate")]
	interface QTCaptureFileOutputDelegate {
		[Export ("captureOutput:didOutputSampleBuffer:fromConnection:"), EventArgs ("QTCaptureFileSample")]
		void DidOutputSampleBuffer (QTCaptureFileOutput captureOutput, QTSampleBuffer sampleBuffer, QTCaptureConnection connection);

		[Export ("captureOutput:willStartRecordingToOutputFileAtURL:forConnections:"), EventArgs ("QTCaptureFileUrl")]
		void WillStartRecording (QTCaptureFileOutput captureOutput, NSUrl fileUrl, QTCaptureConnection [] connections);

		[Export ("captureOutput:didStartRecordingToOutputFileAtURL:forConnections:"), EventArgs ("QTCaptureFileUrl")]
		void DidStartRecording (QTCaptureFileOutput captureOutput, NSUrl fileUrl, QTCaptureConnection [] connections);

		[Export ("captureOutput:shouldChangeOutputFileAtURL:forConnections:dueToError:"), EventArgs ("QTCaptureFileError"), DefaultValue (true)]
		bool ShouldChangeOutputFile (QTCaptureFileOutput captureOutput, NSUrl outputFileURL, QTCaptureConnection [] connections, NSError reason);

		[Export ("captureOutput:mustChangeOutputFileAtURL:forConnections:dueToError:"), EventArgs ("QTCaptureFileError")]
		void MustChangeOutputFile (QTCaptureFileOutput captureOutput, NSUrl outputFileURL, QTCaptureConnection [] connections, NSError reason);

		[Export ("captureOutput:willFinishRecordingToOutputFileAtURL:forConnections:dueToError:"), EventArgs ("QTCaptureFileError")]
		void WillFinishRecording (QTCaptureFileOutput captureOutput, NSUrl outputFileURL, QTCaptureConnection [] connections, NSError reason);

		[Export ("captureOutput:didFinishRecordingToOutputFileAtURL:forConnections:dueToError:"), EventArgs ("QTCaptureFileError")]
		void DidFinishRecording (QTCaptureFileOutput captureOutput, NSUrl outputFileURL, QTCaptureConnection [] connections, NSError reason);

		[Export ("captureOutput:didPauseRecordingToOutputFileAtURL:forConnections:"), EventArgs ("QTCaptureFileUrl")]
		void DidPauseRecording (QTCaptureFileOutput captureOutput, NSUrl fileUrl, QTCaptureConnection [] connections);

		[Export ("captureOutput:didResumeRecordingToOutputFileAtURL:forConnections:"), EventArgs ("QTCaptureFileUrl")]
		void DidResumeRecording (QTCaptureFileOutput captureOutput, NSUrl fileUrl, QTCaptureConnection [] connections);
	}

	[BaseType (typeof (NSObject))]
	interface QTCaptureInput {
		[Export ("connections")]
		QTCaptureConnection [] Connections { get; }
	}

	[BaseType (typeof (NSObject))]
	interface QTCaptureOutput {
		[Export ("connections")]
		QTCaptureConnection [] Connections { get; }
	}
	
	[BaseType (typeof (NSObject))]
	interface QTCaptureSession {
		[Export ("inputs")]
		QTCaptureInput [] Inputs { get; }

		[Export ("addInput:error:")]
		bool AddInput (QTCaptureInput input, out NSError errorPtr);

		[Export ("removeInput:")]
		void RemoveInput (QTCaptureInput input);

		[Export ("outputs")]
		QTCaptureOutput [] Outputs { get; }

		[Export ("addOutput:error:")]
		bool AddOutput (QTCaptureOutput output, out NSError errorPtr);

		[Export ("removeOutput:")]
		void RemoveOutput (QTCaptureOutput output);

		[Export ("isRunning")]
		bool IsRunning { get; }

		[Export ("startRunning")]
		void StartRunning ();

		[Export ("stopRunning")]
		void StopRunning ();

		[Field ("QTCaptureSessionRuntimeErrorNotification")]
		NSString RuntimeErrorNotification { get; }

		[Field ("QTCaptureSessionErrorKey")]
		NSString ErrorKey { get; }
	}

	[BaseType (typeof (NSView), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (QTCaptureViewDelegate)})]
	interface QTCaptureView {
		[Export ("availableVideoPreviewConnections")]
		QTCaptureConnection [] AvailableVideoPreviewConnections { get; }

		[Export ("previewBounds")]
		RectangleF PreviewBounds { get; }

		//Detected properties
		[Export ("captureSession")]
		QTCaptureSession CaptureSession { get; set; }

		[Export ("videoPreviewConnection")]
		QTCaptureConnection VideoPreviewConnection { get; set; }

		[Export ("fillColor")]
		NSColor FillColor { get; set; }

		[Export ("preservesAspectRatio")]
		bool PreservesAspectRatio { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		QTCaptureViewDelegate Delegate { get; set; }
	}

	[BaseType (typeof (NSObject), Name="QTCaptureView_Delegate")]
	interface QTCaptureViewDelegate {
		[Export ("view:willDisplayImage:"), EventArgs ("QTCaptureImageEvent"), DefaultValueFromArgument ("image")]
		CIImage WillDisplayImage (QTCaptureView view, CIImage image);
	}

	[BaseType (typeof (NSObject))]
	interface QTCompressionOptions {
		[Export ("compressionOptionsIdentifiersForMediaType:")]
		string [] GetCompressionOptionsIdentifiers (string forMediaType);

		[Static]
		[Export ("compressionOptionsWithIdentifier:")]
		NSObject FromIdentifier (string identifier);

		[Export ("mediaType")]
		string MediaType { get; }

		[Export ("localizedDisplayName")]
		string LocalizedDisplayName { get; }

		[Export ("localizedCompressionOptionsSummary")]
		string LocalizedCompressionOptionsSummary { get; }

		[Export ("isEqualToCompressionOptions:")]
		bool IsEqualToCompressionOptions (QTCompressionOptions compressionOptions);
	}

	[BaseType (typeof (NSObject))]
	interface QTFormatDescription {
		[Export ("mediaType")]
		string MediaType { get; }

		[Export ("formatType")]
		UInt32 FormatType { get; }

		[Export ("localizedFormatSummary")]
		string LocalizedFormatSummary { get; }

		[Export ("quickTimeSampleDescription")]
		NSData QuickTimeSampleDescription { get; }

		[Export ("formatDescriptionAttributes")]
		NSDictionary FormatDescriptionAttributes { get; }

		[Export ("attributeForKey:")]
		NSObject AttributeForKey (string key);

		[Export ("isEqualToFormatDescription:")]
		bool IsEqualToFormatDescription (QTFormatDescription formatDescription);

	}

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
		NSObject GetAttribute (string attributeKey);

		[Export ("setAttribute:forKey:")]
		void SetAttribute (NSObject value, string attributeKey);

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
	interface QTSampleBuffer {
		[Export ("bytesForAllSamples")]
		IntPtr BytesForAllSamples { get; }

		[Export ("lengthForAllSamples")]
		uint LengthForAllSamples { get; }

		[Export ("formatDescription")]
		QTFormatDescription FormatDescription { get; }

		[Export ("duration")]
		QTTime Duration { get; }

		[Export ("decodeTime")]
		QTTime DecodeTime { get; }

		[Export ("presentationTime")]
		QTTime PresentationTime { get; }

		[Export ("numberOfSamples")]
		int SampleCount { get; }

		[Export ("sampleBufferAttributes")]
		NSDictionary SampleBufferAttributes { get; }

		[Export ("attributeForKey:")]
		NSObject GetAttribute (string key);

		[Export ("sampleUseCount")]
		int SampleUseCount { get; }
		
		[Export ("incrementSampleUseCount")]
		void IncrementSampleUseCount ();

		[Export ("decrementSampleUseCount")]
		void DecrementSampleUseCount ();

		//[Export ("audioBufferListWithOptions:")]
		//AudioBufferList AudioBufferListWithOptions (QTSampleBufferAudioBufferListOptions options);

		//[Export ("getAudioStreamPacketDescriptions:inRange:")]
		//bool GetAudioStreamPacketDescriptionsinRange (AudioStreamPacketDescription audioStreamPacketDescriptions, NSRange range);
	}

	[BaseType (typeof (NSObject))]
	interface QTTrack {
	}
}