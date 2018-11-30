// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc.
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
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AVFoundation {

	public enum AVAudioQuality {
		Min = 0,
		Low = 0x20,
		Medium = 0x40,
		High = 0x60,
		Max = 0x7F
	}

	[Since (4,0)]
	public enum AVAssetExportSessionStatus {
		Unknown,
		Waiting,
		Exporting,
		Completed,
		Failed,
		Cancelled
	}

	[Since (4,0)]
	public enum AVAssetReaderStatus {
		Unknown = 0,
		Reading,
		Completed,
		Failed,
		Cancelled,
	}

	[Since (4,1)]
	public enum AVAssetWriterStatus {
		Unknown = 0,
		Writing,
		Completed,
		Failed,
		Cancelled,
	}
	
	[Since (4,0)]
	public enum AVCaptureVideoOrientation {
		Portrait = 1,
		PortraitUpsideDown,
		LandscapeRight,
		LandscapeLeft,
	}

	[Since (4,0)]
	public enum AVCaptureFlashMode {
		Off, On, Auto
	}

	[Since (4,0)]
	public enum AVCaptureTorchMode {
		Off, On, Auto
	}

	[Since (4,0)]
	public enum AVCaptureFocusMode {
		ModeLocked,
		ModeAutoFocus,
		ModeContinuousAutoFocus
	}

	[Since (4,0)]
	public enum AVCaptureDevicePosition {
		Back = 1,
		Front = 2
	}
	
	[Since (4,0)]
	public enum AVCaptureExposureMode {
		Locked, AutoExpose, ContinuousAutoExposure
	}

	[Since (4,0)]
	public enum AVCaptureWhiteBalanceMode {
		Locked, AutoWhiteBalance, ContinuousAutoWhiteBalance
	}

	[Flags]
	[Since (4,0)]
	public enum AVAudioSessionInterruptionFlags {
		ShouldResume = 1
	}

	public enum AVError {
		Unknown = -11800,
		OutOfMemory = -11801,
		SessionNotRunning = -11803,
		DeviceAlreadyUsedByAnotherSession = -11804,
		NoDataCaptured = -11805,
		SessionConfigurationChanged = -11806,
		DiskFull = -11807,
		DeviceWasDisconnected = -11808,
		MediaChanged = -11809,
		MaximumDurationReached = -11810,
		MaximumFileSizeReached = -11811,
		MediaDiscontinuity = -11812,
		MaximumNumberOfSamplesForFileFormatReached = -11813,
		DeviceNotConnected = -11814,
		DeviceInUseByAnotherApplication = -11815,
		DeviceLockedForConfigurationByAnotherProcess = -11817,
		SessionWasInterrupted = -11818,
		MediaServicesWereReset = -11819,
		ExportFailed = -11820,
		DecodeFailed = -11821,
		InvalidSourceMedia = - 11822,
		FileAlreadyExists = -11823,
		CompositionTrackSegmentsNotContiguous = -11824,
		InvalidCompositionTrackSegmentDuration = -11825,
		InvalidCompositionTrackSegmentSourceStartTime= -11826,
		InvalidCompositionTrackSegmentSourceDuration = -11827,
		FormatNotRecognized = -11828,
		FailedToParse = -11829,
		MaximumStillImageCaptureRequestsExceeded = 11830,
		ContentIsProtected = -11831,
		NoImageAtTime = -11832,
		DecoderNotFound = -11833,
		EncoderNotFound = -11834,
		ContentIsNotAuthorized = -11835,
		ApplicationIsNotAuthorized = -11836,
		DeviceIsNotAvailableInBackground = -11837,
		OperationNotSupportedForAsset = -11838,
		DecoderTemporarilyUnavailable = -11839,
		EncoderTemporarilyUnavailable = -11840,
		InvalidVideoComposition = -11841,
		//[Since (5,1)]
		ReferenceForbiddenByReferencePolicy = -11842,
		InvalidOutputURLPathExtension = -11843,
		ScreenCaptureFailed = -11844,
		DisplayWasDisabled = -11845,
		TorchLevelUnavailable = -11846,
		OperationInterrupted = -11847,
		IncompatibleAsset = -11848,
		FailedToLoadMediaData = -11849,
		ServerIncorrectlyConfigured = -11850,			
	}

	[Since (4,0)]
	public enum AVPlayerActionAtItemEnd {
		Pause = 1, None
	}

	[Since (4,0)]
	public enum AVPlayerItemStatus {
		Unknown, ReadyToPlay, Failed
	}

	[Flags]
	[Since (4,0)]
	public enum AVAudioSessionFlags {
		NotifyOthersOnDeactivation = 1
	}

	[Since (4,0)]
	public enum AVKeyValueStatus {
		Unknown, Loading, Loaded, Failed, Cancelled
	}

	[Since (4,0)]
	public enum AVPlayerStatus {
		Unknown,
		ReadyToPlay,
		Failed
	}

	public enum AVAssetReferenceRestrictions {
		ForbidNone = 0,
		ForbidRemoteReferenceToLocal = (1 << 0),
		ForbidLocalReferenceToRemote = (1 << 1),
		ForbidCrossSiteReference     = (1 << 2),
		ForbidLocalReferenceToLocal  = (1 << 3),
		ForbidAll = 0xFFFF,
	}

	public enum AVAssetImageGeneratorResult {
		Succeeded, Failed, Cancelled
	}

	public enum AVCaptureDeviceTransportControlsPlaybackMode {
		NotPlaying, Playing
	}

	public enum AVVideoFieldMode {
		Both, TopOnly, BottomOnly, Deinterlace
	}

	[Flags]
	public enum AVAudioSessionInterruptionOptions {
		ShouldResume = 1
	}

	[Flags]
	public enum AVAudioSessionSetActiveOptions {
		NotifyOthersOnDeactivation = 1
	}

	public enum AVAudioSessionPortOverride {
		None = 0,
		Speaker = 0x73706b72 // 'spkr'
	}

	public enum AVAudioSessionRouteChangeReason {
		Unknown,
		NewDeviceAvailable,
		OldDeviceUnavailable,
		CategoryChange,
		Override,
		WakeFromSleep,
		NoSuitableRouteForCategory
	}

	[Flags]
	public enum AVAudioSessionCategoryOptions {
		MixWithOthers = 1,
		DuckOthers = 2,
		AllowBluetooth = 4,
		DefaultToSpeaker = 8 
	}

	public enum AVAudioSessionInterruptionType  {
		Ended, Began
	}
}
