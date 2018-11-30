//
// AVFoundation.cs: This file describes the API that the generator will produce for AVFoundation
//
// Authors:
//   Miguel de Icaza
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011-2012, Xamarin, Inc.
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
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreMedia;
using MonoMac.CoreGraphics;
using MonoMac.CoreAnimation;
using MonoMac.CoreVideo;
using System;
#if !MONOMAC
using MonoTouch.MediaToolbox;
#endif

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif

namespace MonoMac.AVFoundation {

	delegate void AVAssetImageGeneratorCompletionHandler (CMTime requestedTime, IntPtr imageRef, CMTime actualTime, AVAssetImageGeneratorResult result, NSError error);
	delegate void AVCompletion (bool finished);
	
	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVMediaType {
		[Field ("AVMediaTypeVideo")]
		NSString Video { get; }
		
		[Field ("AVMediaTypeAudio")]
		NSString Audio { get; }

		[Field ("AVMediaTypeText")]
		NSString Text { get; }

		[Field ("AVMediaTypeClosedCaption")]
		NSString ClosedCaption { get; }

		[Field ("AVMediaTypeSubtitle")]
		NSString Subtitle { get; }

		[Field ("AVMediaTypeTimecode")]
		NSString Timecode { get; }

		[Field ("AVMediaTypeTimedMetadata")]
		[Obsolete ("Deprecated in iOS 6.0, see Metadata")]
		NSString TimedMetadata { get; }

		[Field ("AVMediaTypeMuxed")]
		NSString Muxed { get; }
#if !MONOMAC
		[Since (6,0)]
		[Field ("AVMediaTypeMetadata")]
		NSString Metadata { get; }
#endif
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVMediaCharacteristic {
		[Field ("AVMediaCharacteristicVisual")]
		NSString Visual { get; }

		[Field ("AVMediaCharacteristicAudible")]
		NSString Audible { get; }

		[Field ("AVMediaCharacteristicLegible")]
		NSString Legible { get; }

		[Field ("AVMediaCharacteristicFrameBased")]
		NSString FrameBased { get; }

		[MountainLion]
		[Field ("AVMediaCharacteristicIsMainProgramContent")]
		NSString IsMainProgramContent { get; }

		[MountainLion]
		[Field ("AVMediaCharacteristicIsAuxiliaryContent")]
		NSString IsAuxiliaryContent { get; }

		[MountainLion]
		[Field ("AVMediaCharacteristicContainsOnlyForcedSubtitles")]
		NSString ContainsOnlyForcedSubtitles { get; }

		[MountainLion]
		[Field ("AVMediaCharacteristicTranscribesSpokenDialogForAccessibility")]
		NSString TranscribesSpokenDialogForAccessibility { get; }

		[MountainLion]
		[Field ("AVMediaCharacteristicDescribesMusicAndSoundForAccessibility")]
		NSString DescribesMusicAndSoundForAccessibility { get; }

		[MountainLion]
		[Field ("AVMediaCharacteristicDescribesVideoForAccessibility")]
		NSString DescribesVideoForAccessibility { get;  }
#if !MONOMAC
		[Since (6,0)]
		[Field ("AVMediaCharacteristicEasyToRead")]
		NSString EasyToRead { get; }
#endif
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVFileType {
		[Field ("AVFileTypeQuickTimeMovie")]
		NSString QuickTimeMovie { get; }
		
		[Field ("AVFileTypeMPEG4")]
		NSString Mpeg4 { get; }
		
		[Field ("AVFileTypeAppleM4V")]
		NSString AppleM4V { get; }
		
		[Field ("AVFileType3GPP")]
		NSString ThreeGpp { get; }
		
		[Field ("AVFileTypeAppleM4A")]
		NSString AppleM4A { get; }
		
		[Field ("AVFileTypeCoreAudioFormat")]
		NSString CoreAudioFormat { get; }
		
		[Field ("AVFileTypeWAVE")]
		NSString Wave { get; }
		
		[Field ("AVFileTypeAIFF")]
		NSString Aiff { get; }
		
		[Field ("AVFileTypeAIFC")]
		NSString Aifc { get; }
		
		[Field ("AVFileTypeAMR")]
		NSString Amr { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVVideo {
		[Field ("AVVideoCodecKey")]
		NSString CodecKey { get; }
		
		[Field ("AVVideoCodecH264")]
		NSString CodecH264 { get; }
		
		[Field ("AVVideoCodecJPEG")]
		NSString CodecJPEG { get; }
		
		[Field ("AVVideoWidthKey")]
		NSString WidthKey { get; }
		
		[Field ("AVVideoHeightKey")]
		NSString HeightKey { get; }

		[Since (5,0)]
		[Field ("AVVideoScalingModeKey")]
		NSString ScalingModeKey { get; }
		
		[Field ("AVVideoCompressionPropertiesKey")]
		NSString CompressionPropertiesKey { get; }
		
		[Field ("AVVideoAverageBitRateKey")]
		NSString AverageBitRateKey { get; }
		
		[Field ("AVVideoMaxKeyFrameIntervalKey")]
		NSString MaxKeyFrameIntervalKey { get; }
		
		[Field ("AVVideoProfileLevelKey")]
		NSString ProfileLevelKey { get; }

		[Since (5,0)]
		[Field ("AVVideoQualityKey")]
		NSString QualityKey { get; }
		
		[Field ("AVVideoProfileLevelH264Baseline30")]
		NSString ProfileLevelH264Baseline30 { get; }
		
		[Field ("AVVideoProfileLevelH264Baseline31")]
		NSString ProfileLevelH264Baseline31 { get; }

		[Field ("AVVideoProfileLevelH264Main30")]
		NSString ProfileLevelH264Main30 { get; }
		
		[Field ("AVVideoProfileLevelH264Main31")]
		NSString ProfileLevelH264Main31 { get; }

		[Since (5,0)]
		[Field ("AVVideoProfileLevelH264Baseline41")]
		NSString ProfileLevelH264Baseline41 { get; }

		[Since (5,0)][MountainLion]
		[Field ("AVVideoProfileLevelH264Main32")]
		NSString ProfileLevelH264Main32 { get; }

		[Since (5,0)]
		[Field ("AVVideoProfileLevelH264Main41")]
		NSString ProfileLevelH264Main41 { get; }
#if !MONOMAC
		[Since (6,0)]
		[Field ("AVVideoProfileLevelH264High40")]
		NSString ProfileLevelH264High40 { get; }

		[Since (6,0)]
		[Field ("AVVideoProfileLevelH264High41")]
		NSString ProfileLevelH264High41 { get; }
#endif
		[Field ("AVVideoPixelAspectRatioKey")]
		NSString PixelAspectRatioKey { get; }
		
		[Field ("AVVideoPixelAspectRatioHorizontalSpacingKey")]
		NSString PixelAspectRatioHorizontalSpacingKey { get; }
		
		[Field ("AVVideoPixelAspectRatioVerticalSpacingKey")]
		NSString PixelAspectRatioVerticalSpacingKey { get; }
		
		[Field ("AVVideoCleanApertureKey")]
		NSString CleanApertureKey { get; }
		
		[Field ("AVVideoCleanApertureWidthKey")]
		NSString CleanApertureWidthKey { get; }
		
		[Field ("AVVideoCleanApertureHeightKey")]
		NSString CleanApertureHeightKey { get; }
		
		[Field ("AVVideoCleanApertureHorizontalOffsetKey")]
		NSString CleanApertureHorizontalOffsetKey { get; }
		
		[Field ("AVVideoCleanApertureVerticalOffsetKey")]
		NSString CleanApertureVerticalOffsetKey { get; }
	}

	[Since (5,0)]
	[Static]
	interface AVVideoScalingModeKey
	{
		[Field ("AVVideoScalingModeFit")]
		NSString Fit { get; }

		[Field ("AVVideoScalingModeResize")]
		NSString Resize { get; }

		[Field ("AVVideoScalingModeResizeAspect")]
		NSString ResizeAspect { get; }

		[Field ("AVVideoScalingModeResizeAspectFill")]
		NSString ResizeAspectFill { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVAudioPlayer {
		[Export ("initWithContentsOfURL:error:")][Internal]
		IntPtr Constructor (NSUrl url, IntPtr outError);
	
		[Export ("initWithData:error:")][Internal]
		IntPtr Constructor (NSData  data, IntPtr outError);
	
		[Export ("prepareToPlay")]
		bool PrepareToPlay ();
	
		[Export ("play")]
		bool Play ();
	
		[Export ("pause")]
		void Pause ();
	
		[Export ("stop")]
		void Stop ();
	
		[Export ("playing")]
		bool Playing { [Bind ("isPlaying")] get;  }
	
		[Export ("numberOfChannels")]
		uint NumberOfChannels { get;  }
	
		[Export ("duration")]
		double Duration { get;  }
	
		[Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set;  }

		[Wrap ("WeakDelegate")]
		AVAudioPlayerDelegate Delegate { get; set; }
	
		[Export ("url")]
		NSUrl Url { get;  }
	
		[Export ("data")]
		NSData Data { get;  }
	
		[Export ("volume")]
		float Volume { get; set;  }
	
		[Export ("currentTime")]
		double CurrentTime { get; set;  }
	
		[Export ("numberOfLoops")]
		int NumberOfLoops { get; set;  }
	
		[Export ("meteringEnabled")]
		bool MeteringEnabled { [Bind ("isMeteringEnabled")] get; set;  }
	
		[Export ("updateMeters")]
		void UpdateMeters ();
	
		[Export ("peakPowerForChannel:")]
		float PeakPower (uint channelNumber);
	
		[Export ("averagePowerForChannel:")]
		float AveragePower (uint channelNumber);

		[Since (4,0)]
		[Export ("deviceCurrentTime")]
		double DeviceCurrentTime { get;  }

		[Since (4,0)]
		[Export ("pan")]
		float Pan { get; set; }

		[Since (4,0)]
		[Export ("playAtTime:")]
		bool PlayAtTime (double time);

		[Since (4,0)]
		[Export ("settings")][Protected]
		NSDictionary WeakSettings { get;  }

		[Wrap ("WeakSettings")]
		AudioSettings SoundSetting { get; }

		[Since (5,0)]
		[Export ("enableRate")]
		bool EnableRate { get; set; }

		[Since (5,0)]
		[Export ("rate")]
		float Rate { get; set; }		
#if !MONOMAC
		[Since (6,0)]
		[Export ("channelAssignments")]
		AVAudioSessionChannelDescription [] ChannelAssignments { get; set; }
#endif
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVAudioPlayerDelegate {
		[Export ("audioPlayerDidFinishPlaying:successfully:"), CheckDisposed]
		void FinishedPlaying (AVAudioPlayer player, bool flag);
	
		[Export ("audioPlayerDecodeErrorDidOccur:error:")]
		void DecoderError (AVAudioPlayer player, NSError  error);
	
#if !MONOMAC
		[Export ("audioPlayerBeginInterruption:")]
		void BeginInterruption (AVAudioPlayer  player);
	
		[Export ("audioPlayerEndInterruption:")]
		[Obsolete ("Deprecated in iOS 6.0")]
		void EndInterruption (AVAudioPlayer player);

		// Deprecated in iOS 6.0 but we have same C# signature
		[Since (4,0)]
		[Export ("audioPlayerEndInterruption:withFlags:")]
		void EndInterruption (AVAudioPlayer player, AVAudioSessionInterruptionFlags flags);

		//[Since (6,0)]
		//[Export ("audioPlayerEndInterruption:withOptions:")]
		//void EndInterruption (AVAudioPlayer player, AVAudioSessionInterruptionFlags flags);
#endif
	}

	[BaseType (typeof (NSObject))]
	interface AVAudioRecorder {
		[Export ("initWithURL:settings:error:")][Internal]
		IntPtr Constructor (NSUrl url, NSDictionary settings, IntPtr outError);
	
		[Export ("prepareToRecord")]
		bool PrepareToRecord ();
	
		[Export ("record")]
		bool Record ();
	
		[Export ("recordForDuration:")]
		bool RecordFor (double duration);
	
		[Export ("pause")]
		void Pause ();
	
		[Export ("stop")]
		void Stop ();
	
		[Export ("deleteRecording")]
		bool DeleteRecording ();
	
		[Export ("recording")]
		bool Recording { [Bind ("isRecording")] get;  }
	
		[Export ("url")]
		NSUrl Url { get;  }
	
		[Advice ("Use AudioSettings")]
		[Export ("settings")]
		NSDictionary Settings { get;  }

		[Wrap ("Settings")]
		AudioSettings AudioSettings { get; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set;  }

		[Wrap ("WeakDelegate")]
		AVAudioRecorderDelegate Delegate { get; set;  }
	
		[Export ("currentTime")]
		double currentTime { get; }
	
		[Export ("meteringEnabled")]
		bool MeteringEnabled { [Bind ("isMeteringEnabled")] get; set;  }
	
		[Export ("updateMeters")]
		void UpdateMeters ();
	
		[Export ("peakPowerForChannel:")]
		float PeakPower (uint channelNumber);
	
		[Export ("averagePowerForChannel:")]
		float AveragePower (uint channelNumber);
#if !MONOMAC
		[Since (6,0)]
		[Export ("channelAssignments")]
		AVAudioSessionChannelDescription [] ChannelAssignments { get; set; }

		[Since (6,0)]
		[Export ("recordAtTime:")]
		bool RecordAt (double time);

		[Since (6,0)]
		[Export ("recordAtTime:forDuration:")]
		bool RecordAt (double time, double duration);

		[Since (6,0)]
		[Export ("deviceCurrentTime")]
		double DeviceCurrentTime { get; }
#endif
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVAudioRecorderDelegate {
		[Export ("audioRecorderDidFinishRecording:successfully:"), CheckDisposed]
		void FinishedRecording (AVAudioRecorder recorder, bool flag);
	
		[Export ("audioRecorderEncodeErrorDidOccur:error:")]
		void EncoderError (AVAudioRecorder recorder, NSError  error);

#if !MONOMAC	
		[Export ("audioRecorderBeginInterruption:")]
		void BeginInterruption (AVAudioRecorder  recorder);
	
		[Obsolete ("Deprecated in iOS 6.0")]
		[Export ("audioRecorderEndInterruption:")]
		void EndInterruption (AVAudioRecorder  recorder);

		// Deprecated in iOS 6.0 but we have same C# signature
		[Since (4,0)]
		[Export ("audioRecorderEndInterruption:withFlags:")]
		void EndInterruption (AVAudioRecorder recorder, AVAudioSessionInterruptionFlags flags);

		//[Since (6,0)]
		//[Export ("audioRecorderEndInterruption:withOptions:")]
		//void EndInterruption (AVAudioRecorder recorder, AVAudioSessionInterruptionFlags flags);
#endif
	}

#if !MONOMAC
	[BaseType (typeof (NSObject))]
	interface AVAudioSession {
		[Export ("sharedInstance"), Static]
		AVAudioSession SharedInstance ();
	
		[Export ("delegate")]
		NSObject WeakDelegate { get; set;  }

		[Wrap ("WeakDelegate")]
		[Obsolete ("Deprecated in iOS 6.0. Use AVAudioSession.Notification.Observe* methods instead")]
		AVAudioSessionDelegate Delegate { get; set;  }
	
		[Export ("setActive:error:")]
		bool SetActive (bool beActive, out NSError outError);

		[Since (4,0)]
		[Export ("setActive:withFlags:error:")]
		[Obsolete ("Deprecated in iOS 6.0. Use SetActive (bool, AVAudioSessionSetActiveOptions, out NSError) instead")]
		bool SetActive (bool beActive, AVAudioSessionFlags flags, out NSError outError);

		[Export ("setCategory:error:")]
		bool SetCategory (NSString theCategory, out NSError outError);
	
		[Obsolete ("Deprecated in iOS 6.0, use SetPreferredSampleRate")]
		[Export ("setPreferredHardwareSampleRate:error:")]
		bool SetPreferredHardwareSampleRate (double sampleRate, out NSError outError);
	
		[Export ("setPreferredIOBufferDuration:error:")]
		bool SetPreferredIOBufferDuration (double duration, out NSError outError);
	
		[Export ("category")]
		NSString Category { get;  }

		[Export ("mode")]
		NSString Mode { get; }

		[Export ("setMode:error:")]
		bool SetMode (NSString mode, out NSError error);
	
		[Export ("preferredHardwareSampleRate")]
		[Obsolete ("Deprecated in iOS 6.0")]
		double PreferredHardwareSampleRate { get;  }
	
		[Export ("preferredIOBufferDuration")]
		double PreferredIOBufferDuration { get;  }
	
		[Export ("inputIsAvailable")]
		[Obsolete ("Deprecated in iOS 6.0")]
		bool InputIsAvailable { get;  }
	
		[Export ("currentHardwareSampleRate")]
		[Obsolete ("Deprecated in iOS 6.0, use SampleRate instead")]
		double CurrentHardwareSampleRate { get;  }
	
		[Export ("currentHardwareInputNumberOfChannels")]
		[Obsolete ("Deprecated in iOS 6.0, use InputNumberOfChannels instead")]
		int CurrentHardwareInputNumberOfChannels { get;  }
	
		[Export ("currentHardwareOutputNumberOfChannels")]
		[Obsolete ("Deprecated in iOS 6.0, use OutputNumberOfChannels instead")]
		int CurrentHardwareOutputNumberOfChannels { get;  }

		[Field ("AVAudioSessionCategoryAmbient")]
		NSString CategoryAmbient { get; }

		[Field ("AVAudioSessionCategorySoloAmbient")]
		NSString CategorySoloAmbient { get; }

		[Field ("AVAudioSessionCategoryPlayback")]
		NSString CategoryPlayback { get; }

		[Field ("AVAudioSessionCategoryRecord")]
		NSString CategoryRecord { get; }

		[Field ("AVAudioSessionCategoryPlayAndRecord")]
		NSString CategoryPlayAndRecord { get; }

		[Field ("AVAudioSessionCategoryAudioProcessing")]
		NSString CategoryAudioProcessing { get; }

		[Field ("AVAudioSessionModeDefault")]
		NSString ModeDefault { get; }

		[Field ("AVAudioSessionModeVoiceChat")]
		NSString ModeVoiceChat { get; }

		[Field ("AVAudioSessionModeVideoRecording")]
		NSString ModeVideoRecording { get; }

		[Field ("AVAudioSessionModeMeasurement")]
		NSString ModeMeasurement { get; }

		[Field ("AVAudioSessionModeGameChat")]
		NSString ModeGameChat { get; }

		[Since (6,0)]
		[Export ("setActive:withOptions:error:")]
		bool SetActive (bool active, AVAudioSessionSetActiveOptions options, out NSError outError);

		[Since (6,0)]
		[Export ("setCategory:withOptions:error:")]
		bool SetCategory (string category, AVAudioSessionCategoryOptions options, out NSError outError);

		[Since (6,0)]
		[Export ("categoryOptions")]
		AVAudioSessionCategoryOptions CategoryOptions { get;  }

		[Since (6,0)]
		[Export ("overrideOutputAudioPort:error:")]
		bool OverrideOutputAudioPort (AVAudioSessionPortOverride portOverride, out NSError outError);

		[Since (6,0)]
		[Export ("otherAudioPlaying")]
		bool OtherAudioPlaying { [Bind ("isOtherAudioPlaying")] get;  }

		[Since (6,0)]
		[Export ("currentRoute")]
		AVAudioSessionRouteDescription CurrentRoute { get;  }

		[Since (6,0)]
		[Export ("setPreferredSampleRate:error:")]
		bool SetPreferredSampleRate (double sampleRate, out NSError error);
		
		[Since (6,0)]
		[Export ("preferredSampleRate")]
		double PreferredSampleRate { get;  }

		[Since (6,0)]
		[Export ("inputGain")]
		float InputGain { get;  }

		[Since (6,0)]
		[Export ("inputGainSettable")]
		bool InputGainSettable { [Bind ("isInputGainSettable")] get;  }

		[Since (6,0)]
		[Export ("inputAvailable")]
		bool InputAvailable { [Bind ("isInputAvailable")] get;  }

		[Since (6,0)]
		[Export ("sampleRate")]
		double SampleRate { get;  }

		[Since (6,0)]
		[Export ("inputNumberOfChannels")]
		int InputNumberOfChannels { get;  }

		[Since (6,0)]
		[Export ("outputNumberOfChannels")]
		int OutputNumberOfChannels { get;  }

		[Since (6,0)]
		[Export ("outputVolume")]
		float OutputVolume { get;  }

		[Since (6,0)]
		[Export ("inputLatency")]
		double InputLatency { get;  }

		[Since (6,0)]
		[Export ("outputLatency")]
		double OutputLatency { get;  }

		[Since (6,0)]
		[Export ("IOBufferDuration")]
		double IOBufferDuration { get;  }

		[Since (6,0)]
		[Export ("setInputGain:error:")]
		bool SetInputGain (float gain, out NSError outError);

		[Since (6,0)]
		[Field ("AVAudioSessionInterruptionNotification")]
		[Notification (typeof (AVAudioSessionInterruptionEventArgs))]
		NSString InterruptionNotification { get; }

		[Since (6,0)]
		[Field ("AVAudioSessionRouteChangeNotification")]
		[Notification (typeof (AVAudioSessionRouteChangeEventArgs))]
		NSString RouteChangeNotification { get; }

		[Since (6,0)]
		[Field ("AVAudioSessionMediaServicesWereResetNotification")]
		[Notification]
		NSString MediaServicesWereResetNotification { get; }

		[Since (6,0)]
		[Field ("AVAudioSessionCategoryMultiRoute")]
		NSString CategoryMultiRoute { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionModeMoviePlayback")]
		NSString ModeMoviePlayback { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortLineIn")]
		NSString PortLineIn { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortBuiltInMic")]
		NSString PortBuiltInMic { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortHeadsetMic")]
		NSString PortHeadsetMic { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortLineOut")]
		NSString PortLineOut { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortHeadphones")]
		NSString PortHeadphones { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortBluetoothA2DP")]
		NSString PortBluetoothA2DP { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortBuiltInReceiver")]
		NSString PortBuiltInReceiver { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortBuiltInSpeaker")]
		NSString PortBuiltInSpeaker { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortHDMI")]
		NSString PortHdmi { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortAirPlay")]
		NSString PortAirPlay { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortBluetoothHFP")]
		NSString PortBluetoothHfp { get; }
		
		[Since (6,0)]
		[Field ("AVAudioSessionPortUSBAudio")]
		NSString PortUsbAudio { get; }

		[Since (6,0)]
		[Export ("inputDataSources")]
                AVAudioSessionDataSourceDescription [] InputDataSources { get;  }

		[Since (6,0)]
                [Export ("inputDataSource")]
                AVAudioSessionDataSourceDescription InputDataSource { get;  }

		[Since (6,0)]
                [Export ("outputDataSources")]
                AVAudioSessionDataSourceDescription [] OutputDataSources { get;  }

		[Since (6,0)]
                [Export ("outputDataSource")]
                AVAudioSessionDataSourceDescription OutputDataSource { get;  }
		
		[Since (6,0)]
                [Export ("setInputDataSource:error:")]
		[PostGet ("InputDataSource")]
                bool SetInputDataSource (AVAudioSessionDataSourceDescription dataSource, out NSError outError);

		[Since (6,0)]
                [Export ("setOutputDataSource:error:")]
		[PostGet ("OutputDataSource")]
                bool SetOutputDataSource (AVAudioSessionDataSourceDescription dataSource, out NSError outError);

	}

	[Since (6,0)]
        [BaseType (typeof (NSObject))]
        interface AVAudioSessionDataSourceDescription {
                [Export ("dataSourceID")]
                NSNumber DataSourceID { get;  }

                [Export ("dataSourceName")]
                string DataSourceName { get;  }
        }

	interface AVAudioSessionInterruptionEventArgs {
		[Export ("AVAudioSessionInterruptionTypeKey")]
		AVAudioSessionInterruptionType InterruptionType { get; }

		[Export ("AVAudioSessionInterruptionOptionKey")]
		AVAudioSessionInterruptionOptions Option { get; }
	}

	interface AVAudioSessionRouteChangeEventArgs {
		[Export ("AVAudioSessionRouteChangeReasonKey")]
		AVAudioSessionRouteChangeReason Reason { get; }
		
		[Export ("AVAudioSessionRouteChangePreviousRouteKey")]
		AVAudioSessionRouteDescription PreviousRoute { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVAudioSessionDelegate {
		[Export ("beginInterruption")]
		void BeginInterruption ();
	
		[Export ("endInterruption")]
		void EndInterruption ();

		[Export ("inputIsAvailableChanged:")]
		void InputIsAvailableChanged (bool isInputAvailable);
	
		[Since (4,0)]
		[Export ("endInterruptionWithFlags:")]
		void EndInterruption (AVAudioSessionInterruptionFlags flags);
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	interface AVAudioSessionChannelDescription {
		[Export ("channelName")]
		string ChannelName { get;  }

		[Export ("owningPortUID")]
		string OwningPortUID { get;  }

		[Export ("channelNumber")]
		int ChannelNumber { get;  }
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	interface AVAudioSessionPortDescription {
		[Export ("portType")]
		NSString PortType { get;  }

		[Export ("portName")]
		string PortName { get;  }

		[Export ("UID")]
		string UID { get;  }

		[Export ("channels")]
		AVAudioSessionChannelDescription [] Channels { get;  }
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	interface AVAudioSessionRouteDescription {
		[Export ("inputs")]
		AVAudioSessionPortDescription [] Inputs { get;  }

		[Export ("outputs")]
		AVAudioSessionPortDescription [] Outputs { get;  }

	}
#endif
	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** initialization method -init cannot be sent to an abstract object of class AVAsset: Create a concrete instance!
	[DisableDefaultCtor]
	interface AVAsset {
		[Export ("duration")]
		CMTime Duration { get;  }

		[Export ("preferredRate")]
		float PreferredRate { get;  }

		[Export ("preferredVolume")]
		float PreferredVolume { get;  }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get;  }

		[Export ("naturalSize"), Obsolete ("Deprecated in iOS 5.0. Use NaturalSize/PreferredTransform as appropriate on the video track")]
		CGSize NaturalSize { get;  }

		[Export ("providesPreciseDurationAndTiming")]
		bool ProvidesPreciseDurationAndTiming { get;  }

		[Export ("cancelLoading")]
		void CancelLoading ();

		[Export ("tracks")]
		AVAssetTrack [] Tracks { get;  }

		[Export ("trackWithTrackID:")]
		AVAssetTrack TrackWithTrackID (int trackID);

		[Export ("tracksWithMediaType:")]
		AVAssetTrack [] TracksWithMediaType (string mediaType);

		[Export ("tracksWithMediaCharacteristic:")]
		AVAssetTrack [] TracksWithMediaCharacteristic (string mediaCharacteristic);

		[Export ("lyrics")]
		string Lyrics { get;  }

		[Export ("commonMetadata")]
		AVMetadataItem [] CommonMetadata { get;  }

		[Export ("availableMetadataFormats")]
		string [] AvailableMetadataFormats { get;  }

		[Export ("metadataForFormat:")]
		AVMetadataItem [] MetadataForFormat (string format);

		[Since (4,2)]
		[Export ("hasProtectedContent")]
		bool ProtectedContent { get; }

		[Since (4,3)]
		[Export ("availableChapterLocales")]
		NSLocale [] AvailableChapterLocales { get; }

		[Since (4,3)]
		[Export ("chapterMetadataGroupsWithTitleLocale:containingItemsWithCommonKeys:")]
		AVMetadataItem [] ChapterMetadataGroups (NSLocale forLocale, [NullAllowed] AVMetadataItem [] commonKeys);

		[Since (4,3)]
		[Export ("isPlayable")]
		bool Playable { get; }

		[Since (4,3)]
		[Export ("isExportable")]
		bool Exportable { get; }

		[Since (4,3)]
		[Export ("isReadable")]
		bool Readable { get; }

		[Since (4,3)]
		[Export ("isComposable")]
		bool Composable { get; }

		// 5.0 APIs:
		[Since (5,0)]
		[Static, Export ("assetWithURL:")]
		AVAsset FromUrl (NSUrl url);
#if !MONOMAC
		[Since (5,0)]
		[Export ("availableMediaCharacteristicsWithMediaSelectionOptions")]
		string [] AvailableMediaCharacteristicsWithMediaSelectionOptions { get; }

		[Since (5,0)]
		[Export ("compatibleWithSavedPhotosAlbum")]
		bool CompatibleWithSavedPhotosAlbum  { [Bind ("isCompatibleWithSavedPhotosAlbum")] get; }

		[Since (5,0)]
		[Export ("creationDate")]
		AVMetadataItem CreationDate { get; }
#endif
		[Since (5,0)]
		[Export ("referenceRestrictions")]
		AVAssetReferenceRestrictions ReferenceRestrictions { get; }
#if !MONOMAC
		[Since (5,0)]
		[Export ("mediaSelectionGroupForMediaCharacteristic:")]
		AVMediaSelectionGroup MediaSelectionGroupForMediaCharacteristic (string avMediaCharacteristic);
#endif
		[Export ("statusOfValueForKey:error:")]
		AVKeyValueStatus StatusOfValue (string key, out NSError error);

		[Export ("loadValuesAsynchronouslyForKeys:completionHandler:")]
		[Async ("LoadValuesTaskAsync")]
		void LoadValuesAsynchronously (string [] keys, Action handler);
#if !MONOMAC
		[Since (6,0)]
		[Export ("chapterMetadataGroupsBestMatchingPreferredLanguages:")]
		AVTimedMetadataGroup [] GetChapterMetadataGroupsBestMatchingPreferredLanguages (string [] languages);
#endif
	}

	[BaseType (typeof (NSObject))]
	// <quote>You create an asset generator using initWithAsset: or assetImageGeneratorWithAsset:</quote> http://developer.apple.com/library/ios/#documentation/AVFoundation/Reference/AVAssetImageGenerator_Class/Reference/Reference.html
	// calling 'init' returns a NIL handle
	[DisableDefaultCtor]
	interface AVAssetImageGenerator {
		[Export ("maximumSize")]
		CGSize MaximumSize { get; set;  }

		[Export ("apertureMode")]
		NSString ApertureMode { get; set;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("appliesPreferredTrackTransform")]
		bool AppliesPreferredTrackTransform { get; set; }

		[Static]
		[Export ("assetImageGeneratorWithAsset:")]
		AVAssetImageGenerator FromAsset (AVAsset asset);

		[Export ("initWithAsset:")]
		IntPtr Constructor (AVAsset asset);

		[Export ("copyCGImageAtTime:actualTime:error:")]
		CGImage CopyCGImageAtTime (CMTime requestedTime, out CMTime actualTime, out NSError outError);

		[Export ("generateCGImagesAsynchronouslyForTimes:completionHandler:")]
		void GenerateCGImagesAsynchronously (NSValue[] cmTimesRequestedTimes, AVAssetImageGeneratorCompletionHandler handler);

		[Export ("cancelAllCGImageGeneration")]
		void CancelAllCGImageGeneration ();

		[Field ("AVAssetImageGeneratorApertureModeCleanAperture")]
		NSString ApertureModeCleanAperture { get; }

		[Field ("AVAssetImageGeneratorApertureModeProductionAperture")]
		NSString ApertureModeProductionAperture { get; }

		[Field ("AVAssetImageGeneratorApertureModeEncodedPixels")]
		NSString ApertureModeEncodedPixels { get; }

		// 5.0 APIs
		[Since (5,0)]
		[Export ("requestedTimeToleranceBefore")]
		CMTime RequestedTimeToleranceBefore { get; set;  }

		[Since (5,0)]
		[Export ("requestedTimeToleranceAfter")]
		CMTime RequestedTimeToleranceAfter { get; set;  }
#if !MONOMAC
		[Since (6,0)]
		[Export ("asset")]
		AVAsset Asset { get; }
#endif
	}
	
	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[AVAssetReader initWithAsset:error:] invalid parameter not satisfying: asset != ((void*)0)
	[DisableDefaultCtor]
	interface AVAssetReader {
		[Export ("asset")]
		AVAsset Asset { get;  }

		[Export ("status")]
		AVAssetReaderStatus Status { get;  }

		[Export ("error")]
		NSError Error { get;  }

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; set;  }

		[Export ("outputs")]
		AVAssetReaderOutput [] Outputs { get;  }

		[Static, Export ("assetReaderWithAsset:error:")]
		AVAssetReader FromAsset (AVAsset asset, out NSError error);

		[Export ("initWithAsset:error:")]
		IntPtr Constructor (AVAsset asset, out NSError error);

		[Export ("canAddOutput:")]
		bool CanAddOutput (AVAssetReaderOutput output);

		[Export ("addOutput:")]
		void AddOutput (AVAssetReaderOutput output);

		[Export ("startReading")]
		bool StartReading ();

		[Export ("cancelReading")]
		void CancelReading ();
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** initialization method -init cannot be sent to an abstract object of class AVAssetReaderOutput: Create a concrete instance!
	[DisableDefaultCtor]
	interface AVAssetReaderOutput {
		[Export ("mediaType")]
		string MediaType { get; }

		[Export ("copyNextSampleBuffer")]
		CMSampleBuffer CopyNextSampleBuffer ();

		[Export ("alwaysCopiesSampleData")]
		bool AlwaysCopiesSampleData { get; set; }
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[AVAssetReaderTrackOutput initWithTrack:outputSettings:] invalid parameter not satisfying: track != ((void*)0)
	[DisableDefaultCtor]
	interface AVAssetReaderTrackOutput {
		[Export ("track")]
		AVAssetTrack Track { get;  }

		[Advice ("Use Create method")]
		[Static, Export ("assetReaderTrackOutputWithTrack:outputSettings:")]
		AVAssetReaderTrackOutput FromTrack (AVAssetTrack track, [NullAllowed] NSDictionary outputSettings);

		[Static, Wrap ("FromTrack (track, settings == null ? null : settings.Dictionary)")]
		AVAssetReaderTrackOutput Create (AVAssetTrack track, [NullAllowed] AudioSettings settings);

		[Static, Wrap ("FromTrack (track, settings == null ? null : settings.Dictionary)")]
		AVAssetReaderTrackOutput Create (AVAssetTrack track, [NullAllowed] AVVideoSettingsUncompressed settings);		

		[Export ("initWithTrack:outputSettings:")]
		IntPtr Constructor (AVAssetTrack track, [NullAllowed] NSDictionary outputSettings);

		[Wrap ("this (track, settings == null ? null : settings.Dictionary)")]		
		IntPtr Constructor (AVAssetTrack track, [NullAllowed] AudioSettings settings);

		[Wrap ("this (track, settings == null ? null : settings.Dictionary)")]		
		IntPtr Constructor (AVAssetTrack track, [NullAllowed] AVVideoSettingsUncompressed settings);

		[Export ("outputSettings")]
		NSDictionary OutputSettings { get; }
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[AVAssetReaderAudioMixOutput initWithAudioTracks:audioSettings:] invalid parameter not satisfying: audioTracks != ((void*)0)
	[DisableDefaultCtor]
	interface AVAssetReaderAudioMixOutput {
		[Export ("audioTracks")]
		AVAssetTrack [] AudioTracks { get;  }

		[Export ("audioMix", ArgumentSemantic.Copy)]
		AVAudioMix AudioMix { get; set;  }

		[Advice ("Use Create method")]
		[Static, Export ("assetReaderAudioMixOutputWithAudioTracks:audioSettings:")]
		AVAssetReaderAudioMixOutput FromTracks (AVAssetTrack [] audioTracks, [NullAllowed] NSDictionary audioSettings);

		[Wrap ("FromTracks (audioTracks, settings == null ? null : settings.Dictionary)")]
		AVAssetReaderAudioMixOutput Create (AVAssetTrack [] audioTracks, [NullAllowed] AudioSettings settings);

		[Export ("initWithAudioTracks:audioSettings:")]
		IntPtr Constructor (AVAssetTrack [] audioTracks, [NullAllowed] NSDictionary audioSettings);

		[Wrap ("this (audioTracks, settings == null ? null : settings.Dictionary)")]
		IntPtr Constructor (AVAssetTrack [] audioTracks, [NullAllowed] AudioSettings settings);

		[Advice ("Use Settings property")]
		[Export ("audioSettings")]
		NSDictionary AudioSettings { get; }

		[Wrap ("AudioSettings")]
		AudioSettings Settings { get; }
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	// crash application if 'init' is called
	[DisableDefaultCtor]
	interface AVAssetReaderVideoCompositionOutput {
		[Export ("videoTracks")]
		AVAssetTrack [] VideoTracks { get;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Advice ("Use Create method")]
		[Static]
		[Export ("assetReaderVideoCompositionOutputWithVideoTracks:videoSettings:")]
		AVAssetReaderVideoCompositionOutput WeakFromTracks (AVAssetTrack [] videoTracks, [NullAllowed] NSDictionary videoSettings);

		[Wrap ("WeakFromTracks (videoTracks, settings == null ? null : settings.Dictionary)")]
		[Static]
		AVAssetReaderVideoCompositionOutput Create (AVAssetTrack [] videoTracks, [NullAllowed] CVPixelBufferAttributes settings);

		[Export ("initWithVideoTracks:videoSettings:")]
		IntPtr Constructor (AVAssetTrack [] videoTracks, [NullAllowed] NSDictionary videoSettings);

		[Wrap ("this (videoTracks, settings == null ? null : settings.Dictionary)")]
		IntPtr Constructor (AVAssetTrack [] videoTracks, [NullAllowed] CVPixelBufferAttributes settings);		

		[Export ("videoSettings")]
		NSDictionary WeakVideoSettings { get; }

		[Wrap ("WeakVideoSettings")]
		CVPixelBufferAttributes UncompressedVideoSettings { get; }
	}
#if !MONOMAC
	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	interface AVAssetResourceLoader {
		[Export ("delegate")]
		AVAssetResourceLoaderDelegate Delegate { get;  }

		[Export ("delegateQueue")]
		DispatchQueue DelegateQueue { get;  }

		[Export ("setDelegate:queue:")]
		void SetDelegate (AVAssetResourceLoaderDelegate resourceLoaderDelegate, DispatchQueue delegateQueue);
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVAssetResourceLoaderDelegate {
		[Abstract]
		[Export ("resourceLoader:shouldWaitForLoadingOfRequestedResource:")]
		bool ShouldWaitForLoadingOfRequestedResource (AVAssetResourceLoader resourceLoader, AVAssetResourceLoadingRequest loadingRequest);
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	interface AVAssetResourceLoadingRequest {
		[Export ("request")]
		NSUrlRequest Request { get;  }

		[Export ("finished")]
		bool Finished { get;  }

		[Export ("finishLoadingWithResponse:data:redirect:")]
		void FinishLoading (NSUrlResponse usingResponse, [NullAllowed] NSData data, [NullAllowed] NSUrlRequest redirect);

		[Export ("finishLoadingWithError:")]
		void FinishLoadingWithError (NSError error);

		[Export ("streamingContentKeyRequestDataForApp:contentIdentifier:options:error:")]
		NSData GetStreamingContentKey (NSData appIdentifier, NSData contentIdentifier, [NullAllowed] NSDictionary options, out NSError error);
	}
#endif
	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[AVAssetWriter initWithURL:fileType:error:] invalid parameter not satisfying: outputURL != ((void*)0)
	[DisableDefaultCtor]
	interface AVAssetWriter {
		[Export ("outputURL", ArgumentSemantic.Copy)]
		NSUrl OutputURL { get;  }

		[Export ("outputFileType")]
		string OutputFileType { get;  }

		[Export ("status")]
		AVAssetWriterStatus Status { get;  }

		[Export ("error")]
		NSError Error { get;  }

		[Export ("movieFragmentInterval")]
		CMTime MovieFragmentInterval { get; set;  }

		[Export ("shouldOptimizeForNetworkUse")]
		bool ShouldOptimizeForNetworkUse { get; set;  }

		[Export ("inputs")]
		AVAssetWriterInput [] inputs { get;  }

		[Export ("metadata")]
		AVMetadataItem [] Metadata { get; set;  }

		[Static, Export ("assetWriterWithURL:fileType:error:")]
		AVAssetWriter FromUrl (NSUrl outputUrl, string outputFileType, out NSError error);

		[Export ("initWithURL:fileType:error:")]
		IntPtr Constructor (NSUrl outputUrl, string outputFileType, out NSError error);

		[Export ("canApplyOutputSettings:forMediaType:")]
		bool CanApplyOutputSettings (NSDictionary outputSettings, string mediaType);

		[Wrap ("CanApplyOutputSettings (outputSettings == null ? null : outputSettings.Dictionary, mediaType)")]
		bool CanApplyOutputSettings (AudioSettings outputSettings, string mediaType);

		[Wrap ("CanApplyOutputSettings (outputSettings == null ? null : outputSettings.Dictionary, mediaType)")]
		bool CanApplyOutputSettings (AVVideoSettingsCompressed outputSettings, string mediaType);

		[Export ("canAddInput:")]
		bool CanAddInput (AVAssetWriterInput input);

		[Export ("addInput:")]
		void AddInput (AVAssetWriterInput input);

		[Export ("startWriting")]
		bool StartWriting ();

		[Export ("startSessionAtSourceTime:")]
		void StartSessionAtSourceTime (CMTime startTime);

		[Export ("endSessionAtSourceTime:")]
		void EndSessionAtSourceTime (CMTime endTime);

		[Export ("cancelWriting")]
		void CancelWriting ();

		[Export ("finishWriting")]
		[Obsolete ("Deprecated in iOS 6.0. Use the asynchronous FinishWriting(Action completionHandler) instead")]
		bool FinishWriting ();
#if !MONOMAC
		[Since (6,0)]
		[Export ("finishWritingWithCompletionHandler:")]
		[Async]
		void FinishWriting (Action completionHandler);
#endif
		[Export ("movieTimeScale")]
		int MovieTimeScale { get; set; }
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[AVAssetWriterInput initWithMediaType:outputSettings:] invalid parameter not satisfying: mediaType != ((void*)0)
	[DisableDefaultCtor]
	interface AVAssetWriterInput {
#if !MONOMAC
		[Since (6,0)]
		[Protected]
		[Export ("initWithMediaType:outputSettings:sourceFormatHint:")]
		IntPtr Constructor (string mediaType, [NullAllowed] NSDictionary outputSettings, CMFormatDescription sourceFormatHint);

		[Since (6,0)]
		[Wrap ("this (mediaType, outputSettings == null ? null : outputSettings.Dictionary, sourceFormatHint)")]
		IntPtr Constructor (string mediaType, [NullAllowed] AudioSettings outputSettings, CMFormatDescription sourceFormatHint);

		[Since (6,0)]
		[Wrap ("this (mediaType, outputSettings == null ? null : outputSettings.Dictionary, sourceFormatHint)")]
		IntPtr Constructor (string mediaType, [NullAllowed] AVVideoSettingsCompressed outputSettings, CMFormatDescription sourceFormatHint);

		[Since (6,0)]
		[Static, Internal]
		[Export ("assetWriterInputWithMediaType:outputSettings:sourceFormatHint:")]
		AVAssetWriterInput Create (string mediaType, [NullAllowed] NSDictionary outputSettings, CMFormatDescription sourceFormatHint);

		[Since (6,0)]
		[Static]
		[Wrap ("Create(mediaType, outputSettings == null ? null : outputSettings.Dictionary, sourceFormatHint)")]
		AVAssetWriterInput Create (string mediaType, [NullAllowed] AudioSettings outputSettings, CMFormatDescription sourceFormatHint);

		[Since (6,0)]
		[Static]
		[Wrap ("Create(mediaType, outputSettings == null ? null : outputSettings.Dictionary, sourceFormatHint)")]
		AVAssetWriterInput Create (string mediaType, [NullAllowed] AVVideoSettingsCompressed outputSettings, CMFormatDescription sourceFormatHint);
#endif

		[Export ("mediaType")]
		string MediaType { get;  }

		[Export ("outputSettings")]
		NSDictionary OutputSettings { get;  }

		[Export ("transform")]
		CGAffineTransform Transform { get; set;  }

		[Export ("metadata")]
		AVMetadataItem [] Metadata { get; set;  }

		[Export ("readyForMoreMediaData")]
		bool ReadyForMoreMediaData { [Bind ("isReadyForMoreMediaData")] get;  }

		[Export ("expectsMediaDataInRealTime")]
		bool ExpectsMediaDataInRealTime { get; set;  }

		[Advice ("Use constructor or Create method")]
		[Static, Export ("assetWriterInputWithMediaType:outputSettings:")]
		AVAssetWriterInput FromType (string mediaType, [NullAllowed] NSDictionary outputSettings);

		[Static, Wrap ("FromType (mediaType, outputSettings == null ? null : outputSettings.Dictionary)")]
		AVAssetWriterInput Create (string mediaType, [NullAllowed] AudioSettings outputSettings);

		[Static, Wrap ("FromType (mediaType, outputSettings == null ? null : outputSettings.Dictionary)")]
		AVAssetWriterInput Create (string mediaType, [NullAllowed] AVVideoSettingsCompressed outputSettings);

		// Should be protected
		[Export ("initWithMediaType:outputSettings:")]
		IntPtr Constructor (string mediaType, [NullAllowed] NSDictionary outputSettings);

		[Wrap ("this (mediaType, outputSettings == null ? null : outputSettings.Dictionary)")]		
		IntPtr Constructor (string mediaType, [NullAllowed] AudioSettings outputSettings);

		[Wrap ("this (mediaType, outputSettings == null ? null : outputSettings.Dictionary)")]		
		IntPtr Constructor (string mediaType, [NullAllowed] AVVideoSettingsCompressed outputSettings);

		[Export ("requestMediaDataWhenReadyOnQueue:usingBlock:")]
		void RequestMediaData (DispatchQueue queue, Action action);

		[Export ("appendSampleBuffer:")]
		bool AppendSampleBuffer (CMSampleBuffer sampleBuffer);

		[Export ("markAsFinished")]
		void MarkAsFinished ();

		[Export ("mediaTimeScale")]
		int MediaTimeScale { get; set; }

#if !MONOMAC
		[Since (6,0)]
		[Export ("sourceFormatHint")]
		CMFormatDescription SourceFormatHint { get; }
#endif
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[AVAssetWriterInputPixelBufferAdaptor initWithAssetWriterInput:sourcePixelBufferAttributes:] invalid parameter not satisfying: input != ((void*)0)
	[DisableDefaultCtor]
	interface AVAssetWriterInputPixelBufferAdaptor {
		[Export ("assetWriterInput")]
		AVAssetWriterInput AssetWriterInput { get;  }

		[Export ("sourcePixelBufferAttributes")]
		NSDictionary SourcePixelBufferAttributes { get;  }

		[Wrap ("SourcePixelBufferAttributes")]
		CVPixelBufferAttributes Attributes { get; }

		[Export ("pixelBufferPool")]
		CVPixelBufferPool PixelBufferPool { get;  }

		[Advice ("Use Create method")]
		[Static, Export ("assetWriterInputPixelBufferAdaptorWithAssetWriterInput:sourcePixelBufferAttributes:")]
		AVAssetWriterInputPixelBufferAdaptor FromInput (AVAssetWriterInput input, [NullAllowed] NSDictionary sourcePixelBufferAttributes);

		[Static, Wrap ("FromInput (input, attributes == null ? null : attributes.Dictionary)")]
		AVAssetWriterInputPixelBufferAdaptor Create (AVAssetWriterInput input, [NullAllowed] CVPixelBufferAttributes attributes);

		[Export ("initWithAssetWriterInput:sourcePixelBufferAttributes:")]
		IntPtr Constructor (AVAssetWriterInput input, [NullAllowed] NSDictionary sourcePixelBufferAttributes);

		[Wrap ("this (input, attributes == null ? null : attributes.Dictionary)")]
		IntPtr Constructor (AVAssetWriterInput input, [NullAllowed] CVPixelBufferAttributes attributes);		

		[Export ("appendPixelBuffer:withPresentationTime:")]
		bool AppendPixelBufferWithPresentationTime (CVPixelBuffer pixelBuffer, CMTime presentationTime);
	}

	[Since (4,0)]
	[BaseType (typeof (AVAsset), Name="AVURLAsset")]
	// 'init' returns NIL
	[DisableDefaultCtor]
	interface AVUrlAsset {
		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get;  }

		[Advice ("Use Create or constructor")]
		[Static, Export ("URLAssetWithURL:options:")]
		AVUrlAsset FromUrl (NSUrl URL, [NullAllowed] NSDictionary options);

		[Static]
		[Wrap ("FromUrl (url, options == null ? null : options.Dictionary)")]
		AVUrlAsset Create (NSUrl url, [NullAllowed] AVUrlAssetOptions options);

		[Export ("initWithURL:options:")]
		IntPtr Constructor (NSUrl URL, [NullAllowed] NSDictionary options);

		[Wrap ("this (url, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (NSUrl url, [NullAllowed] AVUrlAssetOptions options);

		[Export ("compatibleTrackForCompositionTrack:")]
		AVAssetTrack CompatibleTrack (AVCompositionTrack forCompositionTrack);

		[Field ("AVURLAssetPreferPreciseDurationAndTimingKey")]
		NSString PreferPreciseDurationAndTimingKey { get; }

		[Field ("AVURLAssetReferenceRestrictionsKey")]
		NSString ReferenceRestrictionsKey { get; }

		[Since (5,0)]
		[Static, Export ("audiovisualMIMETypes")]
		string [] AudiovisualMimeTypes { get; }

		[Since (5,0)]
		[Static, Export ("audiovisualTypes")]
		string [] AudiovisualTypes { get; }

		[Since (5,0)]
		[Static, Export ("isPlayableExtendedMIMEType:")]
		bool IsPlayable (string extendedMimeType);
#if !MONOMAC
		[Since (6,0)]
		[Export ("resourceLoader")]
		AVAssetResourceLoader ResourceLoader { get;  }
#endif
	}

	[BaseType (typeof (NSObject))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	interface AVAssetTrack {
		[Export ("trackID")]
		int TrackID { get;  }

		[Export ("asset")]
		AVAsset Asset { get; }

		[Export ("mediaType")]
		string MediaType { get;  }

		// Weak version
		[Export ("formatDescriptions")]
		NSObject [] FormatDescriptionsAsObjects { get;  }

		[Wrap ("FormatDescriptionsAsObjects.Select (l => CMFormatDescription.Create (l.Handle, false)).ToArray ()")]
		CMFormatDescription[] FormatDescriptions { get; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get;  }

		[Export ("selfContained")]
		bool SelfContained { [Bind ("isSelfContained")] get;  }

		[Export ("totalSampleDataLength")]
		long TotalSampleDataLength { get;  }

		[Export ("hasMediaCharacteristic:")]
		bool HasMediaCharacteristic (string mediaCharacteristic);

		[Export ("timeRange")]
		CMTimeRange TimeRange { get;  }

		[Export ("naturalTimeScale")]
		int NaturalTimeScale { get;  }

		[Export ("estimatedDataRate")]
		float EstimatedDataRate { get;  }

		[Export ("languageCode")]
		string LanguageCode { get;  }

		[Export ("extendedLanguageTag")]
		string ExtendedLanguageTag { get;  }

		[Export ("naturalSize")]
		CGSize NaturalSize { get;  }

		[Export ("preferredVolume")]
		float PreferredVolume { get;  }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get; }

		[Export ("nominalFrameRate")]
		float NominalFrameRate { get;  }

		[Export ("segments", ArgumentSemantic.Copy)]
		AVAssetTrackSegment [] Segments { get;  }

		[Export ("segmentForTrackTime:")]
		AVAssetTrackSegment SegmentForTrackTime (CMTime trackTime);

		[Export ("samplePresentationTimeForTrackTime:")]
		CMTime SamplePresentationTimeForTrackTime (CMTime trackTime);

		[Export ("availableMetadataFormats")]
		string [] AvailableMetadataFormats { get;  }

		[Export ("commonMetadata")]
		AVMetadataItem [] CommonMetadata { get; }

		[Export ("metadataForFormat:")]
		AVMetadataItem [] MetadataForFormat (string format);

		[Export ("isPlayable")]
		bool Playable { get; }
	}
#if !MONOMAC
	[BaseType (typeof (NSObject))]
	interface AVMediaSelectionGroup {
		[Export ("options")]
		AVMediaSelectionOption [] Options { get;  }
		
		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get;  }

		[Export ("mediaSelectionOptionWithPropertyList:")]
		AVMediaSelectionOption GetMediaSelectionOptionForPropertyList (NSObject propertyList);

		[Static]
		[Export ("playableMediaSelectionOptionsFromArray:")]
		AVMediaSelectionOption [] PlayableMediaSelectionOptions (AVMediaSelectionOption [] source);

		[Static]
		[Export ("mediaSelectionOptionsFromArray:withLocale:")]
		AVMediaSelectionOption [] MediaSelectionOptions (AVMediaSelectionOption [] source, NSLocale locale);

		[Static]
		[Export ("mediaSelectionOptionsFromArray:withMediaCharacteristics:")]
		AVMediaSelectionOption [] MediaSelectionOptions (AVMediaSelectionOption [] source, NSString [] avmediaCharacteristics);

		[Static]
		[Export ("mediaSelectionOptionsFromArray:withoutMediaCharacteristics:")]
		AVMediaSelectionOption [] MediaSelectionOptionsExcludingCharacteristics (AVMediaSelectionOption [] source, NSString [] avmediaCharacteristics);
	}

	[BaseType (typeof (NSObject))]
	interface AVMediaSelectionOption {
		[Export ("mediaType")]
		string MediaType { get;  }

		[Export ("mediaSubTypes")]
		NSNumber []  MediaSubTypes { get;  }

		[Export ("playable")]
		bool Playable { [Bind ("isPlayable")] get;  }

		[Export ("locale")]
		NSLocale Locale { get;  }

		[Export ("commonMetadata")]
		AVMetadataItem [] CommonMetadata { get;  }

		[Export ("availableMetadataFormats")]
		string [] AvailableMetadataFormats { get;  }

		[Export ("hasMediaCharacteristic:")]
		bool HasMediaCharacteristic (string mediaCharacteristic);

		[Export ("metadataForFormat:")]
		AVMetadataItem [] GetMetadataForFormat (string format);

		[Export ("associatedMediaSelectionOptionInMediaSelectionGroup:")]
		AVMediaSelectionOption AssociatedMediaSelectionOptionInMediaSelectionGroup (AVMediaSelectionGroup mediaSelectionGroup);

		[Export ("propertyList")]
		NSObject PropertyList { get; }
	}
#endif
	[Static]
	interface AVMetadata {
		[Field ("AVMetadataKeySpaceCommon")]
		NSString KeySpaceCommon { get; }
		
		[Field ("AVMetadataCommonKeyTitle")]
		NSString CommonKeyTitle { get; }
		
		[Field ("AVMetadataCommonKeyCreator")]
		NSString CommonKeyCreator { get; }
		
		[Field ("AVMetadataCommonKeySubject")]
		NSString CommonKeySubject { get; }
		
		[Field ("AVMetadataCommonKeyDescription")]
		NSString CommonKeyDescription { get; }
		
		[Field ("AVMetadataCommonKeyPublisher")]
		NSString CommonKeyPublisher { get; }
		
		[Field ("AVMetadataCommonKeyContributor")]
		NSString CommonKeyContributor { get; }
		
		[Field ("AVMetadataCommonKeyCreationDate")]
		NSString CommonKeyCreationDate { get; }
		
		[Field ("AVMetadataCommonKeyLastModifiedDate")]
		NSString CommonKeyLastModifiedDate { get; }
		
		[Field ("AVMetadataCommonKeyType")]
		NSString CommonKeyType { get; }
		
		[Field ("AVMetadataCommonKeyFormat")]
		NSString CommonKeyFormat { get; }
		
		[Field ("AVMetadataCommonKeyIdentifier")]
		NSString CommonKeyIdentifier { get; }
		
		[Field ("AVMetadataCommonKeySource")]
		NSString CommonKeySource { get; }
		
		[Field ("AVMetadataCommonKeyLanguage")]
		NSString CommonKeyLanguage { get; }
		
		[Field ("AVMetadataCommonKeyRelation")]
		NSString CommonKeyRelation { get; }
		
		[Field ("AVMetadataCommonKeyLocation")]
		NSString CommonKeyLocation { get; }
		
		[Field ("AVMetadataCommonKeyCopyrights")]
		NSString CommonKeyCopyrights { get; }
		
		[Field ("AVMetadataCommonKeyAlbumName")]
		NSString CommonKeyAlbumName { get; }
		
		[Field ("AVMetadataCommonKeyAuthor")]
		NSString CommonKeyAuthor { get; }
		
		[Field ("AVMetadataCommonKeyArtist")]
		NSString CommonKeyArtist { get; }
		
		[Field ("AVMetadataCommonKeyArtwork")]
		NSString CommonKeyArtwork { get; }
		
		[Field ("AVMetadataCommonKeyMake")]
		NSString CommonKeyMake { get; }
		
		[Field ("AVMetadataCommonKeyModel")]
		NSString CommonKeyModel { get; }
		
		[Field ("AVMetadataCommonKeySoftware")]
		NSString CommonKeySoftware { get; }

		[Field ("AVMetadataFormatQuickTimeUserData")]
		NSString FormatQuickTimeUserData { get; }
		
		[Field ("AVMetadataKeySpaceQuickTimeUserData")]
		NSString KeySpaceQuickTimeUserData { get; }
	
		[Field ("AVMetadataQuickTimeUserDataKeyAlbum")]
		NSString QuickTimeUserDataKeyAlbum { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyArranger")]
		NSString QuickTimeUserDataKeyArranger { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyArtist")]
		NSString QuickTimeUserDataKeyArtist { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyAuthor")]
		NSString QuickTimeUserDataKeyAuthor { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyChapter")]
		NSString QuickTimeUserDataKeyChapter { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyComment")]
		NSString QuickTimeUserDataKeyComment { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyComposer")]
		NSString QuickTimeUserDataKeyComposer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyCopyright")]
		NSString QuickTimeUserDataKeyCopyright { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyCreationDate")]
		NSString QuickTimeUserDataKeyCreationDate { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyDescription")]
		NSString QuickTimeUserDataKeyDescription { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyDirector")]
		NSString QuickTimeUserDataKeyDirector { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyDisclaimer")]
		NSString QuickTimeUserDataKeyDisclaimer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyEncodedBy")]
		NSString QuickTimeUserDataKeyEncodedBy { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyFullName")]
		NSString QuickTimeUserDataKeyFullName { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyGenre")]
		NSString QuickTimeUserDataKeyGenre { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyHostComputer")]
		NSString QuickTimeUserDataKeyHostComputer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyInformation")]
		NSString QuickTimeUserDataKeyInformation { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyKeywords")]
		NSString QuickTimeUserDataKeyKeywords { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyMake")]
		NSString QuickTimeUserDataKeyMake { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyModel")]
		NSString QuickTimeUserDataKeyModel { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyOriginalArtist")]
		NSString QuickTimeUserDataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyOriginalFormat")]
		NSString QuickTimeUserDataKeyOriginalFormat { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyOriginalSource")]
		NSString QuickTimeUserDataKeyOriginalSource { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyPerformers")]
		NSString QuickTimeUserDataKeyPerformers { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyProducer")]
		NSString QuickTimeUserDataKeyProducer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyPublisher")]
		NSString QuickTimeUserDataKeyPublisher { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyProduct")]
		NSString QuickTimeUserDataKeyProduct { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeySoftware")]
		NSString QuickTimeUserDataKeySoftware { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeySpecialPlaybackRequirements")]
		NSString QuickTimeUserDataKeySpecialPlaybackRequirements { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyTrack")]
		NSString QuickTimeUserDataKeyTrack { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyWarning")]
		NSString QuickTimeUserDataKeyWarning { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyWriter")]
		NSString QuickTimeUserDataKeyWriter { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyURLLink")]
		NSString QuickTimeUserDataKeyURLLink { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyLocationISO6709")]
		NSString QuickTimeUserDataKeyLocationISO6709 { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyTrackName")]
		NSString QuickTimeUserDataKeyTrackName { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyCredits")]
		NSString QuickTimeUserDataKeyCredits { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyPhonogramRights")]
		NSString QuickTimeUserDataKeyPhonogramRights { get; }

		[MountainLion]
		[Field ("AVMetadataQuickTimeUserDataKeyTaggedCharacteristic")]
		NSString QuickTimeUserDataKeyTaggedCharacteristic { get; }
		
		[Field ("AVMetadataISOUserDataKeyCopyright")]
		NSString ISOUserDataKeyCopyright { get; }
		
		[Field ("AVMetadata3GPUserDataKeyCopyright")]
		NSString K3GPUserDataKeyCopyright { get; }
		
		[Field ("AVMetadata3GPUserDataKeyAuthor")]
		NSString K3GPUserDataKeyAuthor { get; }
		
		[Field ("AVMetadata3GPUserDataKeyPerformer")]
		NSString K3GPUserDataKeyPerformer { get; }
		
		[Field ("AVMetadata3GPUserDataKeyGenre")]
		NSString K3GPUserDataKeyGenre { get; }
		
		[Field ("AVMetadata3GPUserDataKeyRecordingYear")]
		NSString K3GPUserDataKeyRecordingYear { get; }
		
		[Field ("AVMetadata3GPUserDataKeyLocation")]
		NSString K3GPUserDataKeyLocation { get; }
		
		[Field ("AVMetadata3GPUserDataKeyTitle")]
		NSString K3GPUserDataKeyTitle { get; }
		
		[Field ("AVMetadata3GPUserDataKeyDescription")]
		NSString K3GPUserDataKeyDescription { get; }
		

		[Field ("AVMetadataFormatQuickTimeMetadata")]
		NSString FormatQuickTimeMetadata { get; }
		
		[Field ("AVMetadataKeySpaceQuickTimeMetadata")]
		NSString KeySpaceQuickTimeMetadata { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyAuthor")]
		NSString QuickTimeMetadataKeyAuthor { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyComment")]
		NSString QuickTimeMetadataKeyComment { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCopyright")]
		NSString QuickTimeMetadataKeyCopyright { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCreationDate")]
		NSString QuickTimeMetadataKeyCreationDate { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDirector")]
		NSString QuickTimeMetadataKeyDirector { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDisplayName")]
		NSString QuickTimeMetadataKeyDisplayName { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyInformation")]
		NSString QuickTimeMetadataKeyInformation { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyKeywords")]
		NSString QuickTimeMetadataKeyKeywords { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyProducer")]
		NSString QuickTimeMetadataKeyProducer { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyPublisher")]
		NSString QuickTimeMetadataKeyPublisher { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyAlbum")]
		NSString QuickTimeMetadataKeyAlbum { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyArtist")]
		NSString QuickTimeMetadataKeyArtist { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyArtwork")]
		NSString QuickTimeMetadataKeyArtwork { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDescription")]
		NSString QuickTimeMetadataKeyDescription { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeySoftware")]
		NSString QuickTimeMetadataKeySoftware { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyYear")]
		NSString QuickTimeMetadataKeyYear { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyGenre")]
		NSString QuickTimeMetadataKeyGenre { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyiXML")]
		NSString QuickTimeMetadataKeyiXML { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationISO6709")]
		NSString QuickTimeMetadataKeyLocationISO6709 { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyMake")]
		NSString QuickTimeMetadataKeyMake { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyModel")]
		NSString QuickTimeMetadataKeyModel { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyArranger")]
		NSString QuickTimeMetadataKeyArranger { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyEncodedBy")]
		NSString QuickTimeMetadataKeyEncodedBy { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyOriginalArtist")]
		NSString QuickTimeMetadataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyPerformer")]
		NSString QuickTimeMetadataKeyPerformer { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyComposer")]
		NSString QuickTimeMetadataKeyComposer { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCredits")]
		NSString QuickTimeMetadataKeyCredits { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyPhonogramRights")]
		NSString QuickTimeMetadataKeyPhonogramRights { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCameraIdentifier")]
		NSString QuickTimeMetadataKeyCameraIdentifier { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCameraFrameReadoutTime")]
		NSString QuickTimeMetadataKeyCameraFrameReadoutTime { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyTitle")]
		NSString QuickTimeMetadataKeyTitle { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCollectionUser")]
		NSString QuickTimeMetadataKeyCollectionUser { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyRatingUser")]
		NSString QuickTimeMetadataKeyRatingUser { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationName")]
		NSString QuickTimeMetadataKeyLocationName { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationBody")]
		NSString QuickTimeMetadataKeyLocationBody { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationNote")]
		NSString QuickTimeMetadataKeyLocationNote { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationRole")]
		NSString QuickTimeMetadataKeyLocationRole { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationDate")]
		NSString QuickTimeMetadataKeyLocationDate { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDirectionFacing")]
		NSString QuickTimeMetadataKeyDirectionFacing { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDirectionMotion")]
		NSString QuickTimeMetadataKeyDirectionMotion { get; }
		
		[Field ("AVMetadataFormatiTunesMetadata")]
		NSString FormatiTunesMetadata { get; }
		
		[Field ("AVMetadataKeySpaceiTunes")]
		NSString KeySpaceiTunes { get; }
		

		[Field ("AVMetadataiTunesMetadataKeyAlbum")]
		NSString iTunesMetadataKeyAlbum { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArtist")]
		NSString iTunesMetadataKeyArtist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyUserComment")]
		NSString iTunesMetadataKeyUserComment { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyCoverArt")]
		NSString iTunesMetadataKeyCoverArt { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyCopyright")]
		NSString iTunesMetadataKeyCopyright { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyReleaseDate")]
		NSString iTunesMetadataKeyReleaseDate { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyEncodedBy")]
		NSString iTunesMetadataKeyEncodedBy { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPredefinedGenre")]
		NSString iTunesMetadataKeyPredefinedGenre { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyUserGenre")]
		NSString iTunesMetadataKeyUserGenre { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySongName")]
		NSString iTunesMetadataKeySongName { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyTrackSubTitle")]
		NSString iTunesMetadataKeyTrackSubTitle { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyEncodingTool")]
		NSString iTunesMetadataKeyEncodingTool { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyComposer")]
		NSString iTunesMetadataKeyComposer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAlbumArtist")]
		NSString iTunesMetadataKeyAlbumArtist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAccountKind")]
		NSString iTunesMetadataKeyAccountKind { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAppleID")]
		NSString iTunesMetadataKeyAppleID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArtistID")]
		NSString iTunesMetadataKeyArtistID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySongID")]
		NSString iTunesMetadataKeySongID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDiscCompilation")]
		NSString iTunesMetadataKeyDiscCompilation { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDiscNumber")]
		NSString iTunesMetadataKeyDiscNumber { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyGenreID")]
		NSString iTunesMetadataKeyGenreID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyGrouping")]
		NSString iTunesMetadataKeyGrouping { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPlaylistID")]
		NSString iTunesMetadataKeyPlaylistID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyContentRating")]
		NSString iTunesMetadataKeyContentRating { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyBeatsPerMin")]
		NSString iTunesMetadataKeyBeatsPerMin { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyTrackNumber")]
		NSString iTunesMetadataKeyTrackNumber { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArtDirector")]
		NSString iTunesMetadataKeyArtDirector { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArranger")]
		NSString iTunesMetadataKeyArranger { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAuthor")]
		NSString iTunesMetadataKeyAuthor { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyLyrics")]
		NSString iTunesMetadataKeyLyrics { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAcknowledgement")]
		NSString iTunesMetadataKeyAcknowledgement { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyConductor")]
		NSString iTunesMetadataKeyConductor { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDescription")]
		NSString iTunesMetadataKeyDescription { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDirector")]
		NSString iTunesMetadataKeyDirector { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyEQ")]
		NSString iTunesMetadataKeyEQ { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyLinerNotes")]
		NSString iTunesMetadataKeyLinerNotes { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyRecordCompany")]
		NSString iTunesMetadataKeyRecordCompany { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyOriginalArtist")]
		NSString iTunesMetadataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPhonogramRights")]
		NSString iTunesMetadataKeyPhonogramRights { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyProducer")]
		NSString iTunesMetadataKeyProducer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPerformer")]
		NSString iTunesMetadataKeyPerformer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPublisher")]
		NSString iTunesMetadataKeyPublisher { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySoundEngineer")]
		NSString iTunesMetadataKeySoundEngineer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySoloist")]
		NSString iTunesMetadataKeySoloist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyCredits")]
		NSString iTunesMetadataKeyCredits { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyThanks")]
		NSString iTunesMetadataKeyThanks { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyOnlineExtras")]
		NSString iTunesMetadataKeyOnlineExtras { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyExecProducer")]
		NSString iTunesMetadataKeyExecProducer { get; }
		
		[Field ("AVMetadataFormatID3Metadata")]
		NSString FormatID3Metadata { get; }
		
		[Field ("AVMetadataKeySpaceID3")]
		NSString KeySpaceID3 { get; }
		

		[Field ("AVMetadataID3MetadataKeyAudioEncryption")]
		NSString ID3MetadataKeyAudioEncryption { get; }
		
		[Field ("AVMetadataID3MetadataKeyAttachedPicture")]
		NSString ID3MetadataKeyAttachedPicture { get; }
		
		[Field ("AVMetadataID3MetadataKeyAudioSeekPointIndex")]
		NSString ID3MetadataKeyAudioSeekPointIndex { get; }
		
		[Field ("AVMetadataID3MetadataKeyComments")]
		NSString ID3MetadataKeyComments { get; }
		
		[Field ("AVMetadataID3MetadataKeyCommerical")]
		NSString ID3MetadataKeyCommerical { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncryption")]
		NSString ID3MetadataKeyEncryption { get; }
		
		[Field ("AVMetadataID3MetadataKeyEqualization")]
		NSString ID3MetadataKeyEqualization { get; }
		
		[Field ("AVMetadataID3MetadataKeyEqualization2")]
		NSString ID3MetadataKeyEqualization2 { get; }
		
		[Field ("AVMetadataID3MetadataKeyEventTimingCodes")]
		NSString ID3MetadataKeyEventTimingCodes { get; }
		
		[Field ("AVMetadataID3MetadataKeyGeneralEncapsulatedObject")]
		NSString ID3MetadataKeyGeneralEncapsulatedObject { get; }
		
		[Field ("AVMetadataID3MetadataKeyGroupIdentifier")]
		NSString ID3MetadataKeyGroupIdentifier { get; }
		
		[Field ("AVMetadataID3MetadataKeyInvolvedPeopleList_v23")]
		NSString ID3MetadataKeyInvolvedPeopleList { get; }
		
		[Field ("AVMetadataID3MetadataKeyLink")]
		NSString ID3MetadataKeyLink { get; }
		
		[Field ("AVMetadataID3MetadataKeyMusicCDIdentifier")]
		NSString ID3MetadataKeyMusicCDIdentifier { get; }
		
		[Field ("AVMetadataID3MetadataKeyMPEGLocationLookupTable")]
		NSString ID3MetadataKeyMPEGLocationLookupTable { get; }
		
		[Field ("AVMetadataID3MetadataKeyOwnership")]
		NSString ID3MetadataKeyOwnership { get; }
		
		[Field ("AVMetadataID3MetadataKeyPrivate")]
		NSString ID3MetadataKeyPrivate { get; }
		
		[Field ("AVMetadataID3MetadataKeyPlayCounter")]
		NSString ID3MetadataKeyPlayCounter { get; }
		
		[Field ("AVMetadataID3MetadataKeyPopularimeter")]
		NSString ID3MetadataKeyPopularimeter { get; }
		
		[Field ("AVMetadataID3MetadataKeyPositionSynchronization")]
		NSString ID3MetadataKeyPositionSynchronization { get; }
		
		[Field ("AVMetadataID3MetadataKeyRecommendedBufferSize")]
		NSString ID3MetadataKeyRecommendedBufferSize { get; }
		
		[Field ("AVMetadataID3MetadataKeyRelativeVolumeAdjustment")]
		NSString ID3MetadataKeyRelativeVolumeAdjustment { get; }
		
		[Field ("AVMetadataID3MetadataKeyRelativeVolumeAdjustment2")]
		NSString ID3MetadataKeyRelativeVolumeAdjustment2 { get; }
		
		[Field ("AVMetadataID3MetadataKeyReverb")]
		NSString ID3MetadataKeyReverb { get; }
		
		[Field ("AVMetadataID3MetadataKeySeek")]
		NSString ID3MetadataKeySeek { get; }
		
		[Field ("AVMetadataID3MetadataKeySignature")]
		NSString ID3MetadataKeySignature { get; }
		
		[Field ("AVMetadataID3MetadataKeySynchronizedLyric")]
		NSString ID3MetadataKeySynchronizedLyric { get; }
		
		[Field ("AVMetadataID3MetadataKeySynchronizedTempoCodes")]
		NSString ID3MetadataKeySynchronizedTempoCodes { get; }
		
		[Field ("AVMetadataID3MetadataKeyAlbumTitle")]
		NSString ID3MetadataKeyAlbumTitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyBeatsPerMinute")]
		NSString ID3MetadataKeyBeatsPerMinute { get; }
		
		[Field ("AVMetadataID3MetadataKeyComposer")]
		NSString ID3MetadataKeyComposer { get; }
		
		[Field ("AVMetadataID3MetadataKeyContentType")]
		NSString ID3MetadataKeyContentType { get; }
		
		[Field ("AVMetadataID3MetadataKeyCopyright")]
		NSString ID3MetadataKeyCopyright { get; }
		
		[Field ("AVMetadataID3MetadataKeyDate")]
		NSString ID3MetadataKeyDate { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncodingTime")]
		NSString ID3MetadataKeyEncodingTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyPlaylistDelay")]
		NSString ID3MetadataKeyPlaylistDelay { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalReleaseTime")]
		NSString ID3MetadataKeyOriginalReleaseTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyRecordingTime")]
		NSString ID3MetadataKeyRecordingTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyReleaseTime")]
		NSString ID3MetadataKeyReleaseTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyTaggingTime")]
		NSString ID3MetadataKeyTaggingTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncodedBy")]
		NSString ID3MetadataKeyEncodedBy { get; }
		
		[Field ("AVMetadataID3MetadataKeyLyricist")]
		NSString ID3MetadataKeyLyricist { get; }
		
		[Field ("AVMetadataID3MetadataKeyFileType")]
		NSString ID3MetadataKeyFileType { get; }
		
		[Field ("AVMetadataID3MetadataKeyTime")]
		NSString ID3MetadataKeyTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyContentGroupDescription")]
		NSString ID3MetadataKeyContentGroupDescription { get; }
		
		[Field ("AVMetadataID3MetadataKeyTitleDescription")]
		NSString ID3MetadataKeyTitleDescription { get; }
		
		[Field ("AVMetadataID3MetadataKeySubTitle")]
		NSString ID3MetadataKeySubTitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyInitialKey")]
		NSString ID3MetadataKeyInitialKey { get; }
		
		[Field ("AVMetadataID3MetadataKeyLanguage")]
		NSString ID3MetadataKeyLanguage { get; }
		
		[Field ("AVMetadataID3MetadataKeyLength")]
		NSString ID3MetadataKeyLength { get; }
		
		[Field ("AVMetadataID3MetadataKeyMusicianCreditsList")]
		NSString ID3MetadataKeyMusicianCreditsList { get; }
		
		[Field ("AVMetadataID3MetadataKeyMediaType")]
		NSString ID3MetadataKeyMediaType { get; }
		
		[Field ("AVMetadataID3MetadataKeyMood")]
		NSString ID3MetadataKeyMood { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalAlbumTitle")]
		NSString ID3MetadataKeyOriginalAlbumTitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalFilename")]
		NSString ID3MetadataKeyOriginalFilename { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalLyricist")]
		NSString ID3MetadataKeyOriginalLyricist { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalArtist")]
		NSString ID3MetadataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalReleaseYear")]
		NSString ID3MetadataKeyOriginalReleaseYear { get; }
		
		[Field ("AVMetadataID3MetadataKeyFileOwner")]
		NSString ID3MetadataKeyFileOwner { get; }
		
		[Field ("AVMetadataID3MetadataKeyLeadPerformer")]
		NSString ID3MetadataKeyLeadPerformer { get; }
		
		[Field ("AVMetadataID3MetadataKeyBand")]
		NSString ID3MetadataKeyBand { get; }
		
		[Field ("AVMetadataID3MetadataKeyConductor")]
		NSString ID3MetadataKeyConductor { get; }
		
		[Field ("AVMetadataID3MetadataKeyModifiedBy")]
		NSString ID3MetadataKeyModifiedBy { get; }
		
		[Field ("AVMetadataID3MetadataKeyPartOfASet")]
		NSString ID3MetadataKeyPartOfASet { get; }
		
		[Field ("AVMetadataID3MetadataKeyProducedNotice")]
		NSString ID3MetadataKeyProducedNotice { get; }
		
		[Field ("AVMetadataID3MetadataKeyPublisher")]
		NSString ID3MetadataKeyPublisher { get; }
		
		[Field ("AVMetadataID3MetadataKeyTrackNumber")]
		NSString ID3MetadataKeyTrackNumber { get; }
		
		[Field ("AVMetadataID3MetadataKeyRecordingDates")]
		NSString ID3MetadataKeyRecordingDates { get; }
		
		[Field ("AVMetadataID3MetadataKeyInternetRadioStationName")]
		NSString ID3MetadataKeyInternetRadioStationName { get; }
		
		[Field ("AVMetadataID3MetadataKeyInternetRadioStationOwner")]
		NSString ID3MetadataKeyInternetRadioStationOwner { get; }
		
		[Field ("AVMetadataID3MetadataKeySize")]
		NSString ID3MetadataKeySize { get; }
		
		[Field ("AVMetadataID3MetadataKeyAlbumSortOrder")]
		NSString ID3MetadataKeyAlbumSortOrder { get; }
		
		[Field ("AVMetadataID3MetadataKeyPerformerSortOrder")]
		NSString ID3MetadataKeyPerformerSortOrder { get; }
		
		[Field ("AVMetadataID3MetadataKeyTitleSortOrder")]
		NSString ID3MetadataKeyTitleSortOrder { get; }
		
		[Field ("AVMetadataID3MetadataKeyInternationalStandardRecordingCode")]
		NSString ID3MetadataKeyInternationalStandardRecordingCode { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncodedWith")]
		NSString ID3MetadataKeyEncodedWith { get; }
		
		[Field ("AVMetadataID3MetadataKeySetSubtitle")]
		NSString ID3MetadataKeySetSubtitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyYear")]
		NSString ID3MetadataKeyYear { get; }
		
		[Field ("AVMetadataID3MetadataKeyUserText")]
		NSString ID3MetadataKeyUserText { get; }
		
		[Field ("AVMetadataID3MetadataKeyUniqueFileIdentifier")]
		NSString ID3MetadataKeyUniqueFileIdentifier { get; }
		
		[Field ("AVMetadataID3MetadataKeyTermsOfUse")]
		NSString ID3MetadataKeyTermsOfUse { get; }
		
		[Field ("AVMetadataID3MetadataKeyUnsynchronizedLyric")]
		NSString ID3MetadataKeyUnsynchronizedLyric { get; }
		
		[Field ("AVMetadataID3MetadataKeyCommercialInformation")]
		NSString ID3MetadataKeyCommercialInformation { get; }
		
		[Field ("AVMetadataID3MetadataKeyCopyrightInformation")]
		NSString ID3MetadataKeyCopyrightInformation { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialAudioFileWebpage")]
		NSString ID3MetadataKeyOfficialAudioFileWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialArtistWebpage")]
		NSString ID3MetadataKeyOfficialArtistWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialAudioSourceWebpage")]
		NSString ID3MetadataKeyOfficialAudioSourceWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialInternetRadioStationHomepage")]
		NSString ID3MetadataKeyOfficialInternetRadioStationHomepage { get; }
		
		[Field ("AVMetadataID3MetadataKeyPayment")]
		NSString ID3MetadataKeyPayment { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialPublisherWebpage")]
		NSString ID3MetadataKeyOfficialPublisherWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyUserURL")]
		NSString ID3MetadataKeyUserURL { get; }
	}
	
	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVMetadataItem {
		[Export ("commonKey", ArgumentSemantic.Copy)]
		string CommonKey { get;  }

		[Export ("keySpace", ArgumentSemantic.Copy)]
		string KeySpace { get;  }

		[Export ("locale", ArgumentSemantic.Copy)]
		NSLocale Locale { get;  }

		[Export ("time")]
		CMTime Time { get;  }

		[Export ("value", ArgumentSemantic.Copy)]
		NSObject Value { get;  }

		[Export ("extraAttributes", ArgumentSemantic.Copy)]
		NSDictionary ExtraAttributes { get;  }

		[Export ("key", ArgumentSemantic.Copy)]
		NSObject Key { get; }

		[Export ("stringValue")]
		string StringValue { get;  }

		[Export ("numberValue")]
		NSNumber NumberValue { get;  }

		[Export ("dateValue")]
		NSDate DateValue { get;  }

		[Export ("dataValue")]
		NSData DataValue { get;  }

		[Static]
		[Export ("metadataItemsFromArray:withLocale:")]
		AVMetadataItem [] FilterWithLocale (AVMetadataItem [] arrayToFilter, NSLocale locale);

		[Static]
		[Export ("metadataItemsFromArray:withKey:keySpace:")]
		AVMetadataItem [] FilterWithKey (AVMetadataItem [] metadataItems, NSObject key, string keySpace);

		[Since (4,2)]
		[Export ("duration")]
		CMTime Duration { get; }

		[Export ("statusOfValueForKey:error:")]
		AVKeyValueStatus StatusOfValueForKeyerror (string key, out NSError error);

		[Export ("loadValuesAsynchronouslyForKeys:completionHandler:")]
		[Async ("LoadValuesTaskAsync")]
		void LoadValuesAsynchronously (string [] keys, Action handler);

		[Since (6,0)]
		[Static, Export ("metadataItemsFromArray:filteredAndSortedAccordingToPreferredLanguages:")]
		AVMetadataItem [] FilterFromPreferredLanguages (AVMetadataItem [] metadataItems, string [] preferredLanguages);
	}
#if !MONOMAC
	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSGenericException Reason: Cannot instantiate AVMetadataObject because it is an abstract superclass.
	[DisableDefaultCtor]
	interface AVMetadataObject {
		[Export ("duration")]
		CMTime Duration { get;  }

		[Export ("bounds")]
		CGRect Bounds { get;  }

		[Export ("type")]
		string Type { get;  }

		[Export ("time")]
		CMTime Time{ get;}

		[Field ("AVMetadataObjectTypeFace")]
		NSString TypeFace { get; }
	}

	[Since (6,0)]
	[BaseType (typeof (AVMetadataObject))]
	interface AVMetadataFaceObject {
		[Export ("hasRollAngle")]
		bool HasRollAngle { get;  }

		[Export ("rollAngle")]
		float RollAngle { get;  }

		[Export ("hasYawAngle")]
		bool HasYawAngle { get;  }

		[Export ("yawAngle")]
		float YawAngle { get;  }

		[Export ("faceID")]
		int FaceID { get; }
	}
#endif
	[Since (4,0)]
	[BaseType (typeof (AVMetadataItem))]
	interface AVMutableMetadataItem {
		[Export ("keySpace", ArgumentSemantic.Copy)]
		string KeySpace { get; set;  }

		[Export ("metadataItem"), Static]
		AVMutableMetadataItem Create ();
		
		[Export ("locale", ArgumentSemantic.Copy)]
		NSLocale Locale { get; set;  }

		[Export ("time")]
		CMTime Time { get; set;  }

		[Export ("value", ArgumentSemantic.Copy)]
		NSObject Value { get; set;  }

		[Export ("extraAttributes", ArgumentSemantic.Copy)]
		NSDictionary ExtraAttributes { get; set;  }

		[Export ("key", ArgumentSemantic.Copy)]
		NSObject Key { get; set; }
		
		[Since (4,2)]
		[Export ("duration")]
		CMTime Duration { get; set; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVAssetTrack))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	interface AVCompositionTrack {
		[Export ("segments", ArgumentSemantic.Copy)]
		AVCompositionTrackSegment [] Segments { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVCompositionTrack))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	interface AVMutableCompositionTrack {
		[Export ("segments", ArgumentSemantic.Copy)]
		AVCompositionTrackSegment [] Segments { get; set; }

		[Export ("insertTimeRange:ofTrack:atTime:error:")]
		bool InsertTimeRange (CMTimeRange timeRange, AVAssetTrack ofTrack, CMTime atTime, out NSError error);

		[Export ("insertEmptyTimeRange:")]
		void InsertEmptyTimeRange (CMTimeRange timeRange);

		[Export ("removeTimeRange:")]
		void RemoveTimeRange (CMTimeRange timeRange);

		[Export ("scaleTimeRange:toDuration:")]
		void ScaleTimeRange (CMTimeRange timeRange, CMTime duration);

		[Export ("validateTrackSegments:error:")]
		bool ValidateTrackSegments (AVCompositionTrackSegment [] trackSegments, out NSError error);

		[Export ("extendedLanguageTag")]
		string ExtendedLanguageTag { get; set; }

		[Export ("languageCode")]
		string LanguageCode { get; set; }

		[Export ("naturalTimeScale")]
		int NaturalTimeScale { get; set; }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get; set; }

		[Export ("preferredVolume")]
		float PreferredVolume { get; set; }

		// 5.0
		[Since (5,0)]
		[Export ("insertTimeRanges:ofTracks:atTime:error:")]
		bool InsertTimeRanges (NSValue cmTimeRanges, AVAssetTrack [] tracks, CMTime startTime, out NSError error);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVAssetTrackSegment {
		[Export ("empty")]
		bool Empty { [Bind ("isEmpty")] get;  }

		[Export ("timeMapping")]
		CMTimeMapping TimeMapping { get; }

	}

	[Since (4,0)]
	[BaseType (typeof (AVAsset))]
	interface AVComposition {
		[Export ("tracks")]
		AVCompositionTrack [] Tracks { get; }

		[Obsolete ("Deprecated in iOS 5.0 and OSX 10.8")]
		[Export ("naturalSize")]
		CGSize NaturalSize { get; [NotImplemented] set; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVComposition))]
	interface AVMutableComposition {
		[Export ("composition"), Static]
		AVMutableComposition Create ();

		[Export ("insertTimeRange:ofAsset:atTime:error:")]
		bool Insert (CMTimeRange insertTimeRange, AVAsset sourceAsset, CMTime atTime, out NSError error);

		[Export ("insertEmptyTimeRange:")]
		void InserEmptyTimeRange (CMTimeRange timeRange);

		[Export ("removeTimeRange:")]
		void RemoveTimeRange (CMTimeRange timeRange);

		[Export ("scaleTimeRange:toDuration:")]
		void ScaleTimeRange (CMTimeRange timeRange, CMTime duration);

		[Export ("addMutableTrackWithMediaType:preferredTrackID:")]
		AVMutableCompositionTrack AddMutableTrack (string mediaType, int preferredTrackId);

		[Export ("removeTrack:")]
		void RemoveTrack (AVCompositionTrack track);

		[Export ("mutableTrackCompatibleWithTrack:")]
		AVMutableCompositionTrack CreateMutableTrack (AVAssetTrack referenceTrack);

		[Obsolete ("Deprecated in iOS 5.0 and OSX 10.8")]
		[Export ("naturalSize")]
		[Override]
		CGSize NaturalSize { get; set; }
	}
	
	[Since (4,0)]
	[BaseType (typeof (AVAssetTrackSegment))]
	interface AVCompositionTrackSegment {
		[Export ("sourceURL")]
		NSUrl SourceUrl { get;  }

		[Export ("sourceTrackID")]
		int SourceTrackID { get;  }

		[Static]
		[Export ("compositionTrackSegmentWithURL:trackID:sourceTimeRange:targetTimeRange:")]
		IntPtr FromUrl (NSUrl url, int trackID, CMTimeRange sourceTimeRange, CMTimeRange targetTimeRange);

		[Static]
		[Export ("compositionTrackSegmentWithTimeRange:")]
		IntPtr FromTimeRange (CMTimeRange timeRange);

		[Export ("initWithURL:trackID:sourceTimeRange:targetTimeRange:")]
		IntPtr Constructor (NSUrl URL, int trackID, CMTimeRange sourceTimeRange, CMTimeRange targetTimeRange);

		[Export ("initWithTimeRange:")]
		IntPtr Constructor (CMTimeRange timeRange);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	interface AVAssetExportSession {
		[Export ("presetName")]
		string PresetName { get;  }

		[Export ("supportedFileTypes")]
		NSObject [] SupportedFileTypes { get;  }

		[Export ("outputFileType", ArgumentSemantic.Copy)]
		string OutputFileType { get; set;  }

		[Export ("outputURL", ArgumentSemantic.Copy)]
		NSUrl OutputUrl { get; set;  }

		[Static, Export ("exportSessionWithAsset:presetName:")]
		AVAssetExportSession FromAsset (AVAsset asset, string presetName);
		
		[Export ("status")]
		AVAssetExportSessionStatus Status { get;  }

		[Export ("progress")]
		float Progress { get;  }

		[Export ("maxDuration")]
		CMTime MaxDuration { get;  }

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; set;  }

		[Export ("metadata", ArgumentSemantic.Copy)]
		AVMetadataItem [] Metadata { get; set;  }

		[Export ("fileLengthLimit")]
		long FileLengthLimit { get; set;  }

		[Export ("audioMix", ArgumentSemantic.Copy)]
		AVAudioMix AudioMix { get; set;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("shouldOptimizeForNetworkUse")]
		bool ShouldOptimizeForNetworkUse { get; set;  }

		[Static, Export ("allExportPresets")]
		string [] AllExportPresets { get; }

		[Static]
		[Export ("exportPresetsCompatibleWithAsset:")]
		string [] ExportPresetsCompatibleWithAsset (AVAsset asset);

		[Export ("initWithAsset:presetName:")]
		IntPtr Constructor (AVAsset asset, string presetName);

		[Export ("exportAsynchronouslyWithCompletionHandler:")]
		[Async ("ExportTaskAsync")]
		void ExportAsynchronously (AVCompletionHandler handler);

		[Export ("cancelExport")]
		void CancelExport ();

		[Export ("error")]
		NSError Error { get; }
#if !MONOMAC
		[Field ("AVAssetExportPresetLowQuality")]
		NSString PresetLowQuality { get; }

		[Field ("AVAssetExportPresetMediumQuality")]
		NSString PresetMediumQuality { get; }

		[Field ("AVAssetExportPresetHighestQuality")]
		NSString PresetHighestQuality { get; }
#endif
		[Field ("AVAssetExportPreset640x480")]
		NSString Preset640x480 { get; }

		[Field ("AVAssetExportPreset960x540")]
		NSString Preset960x540 { get; }

		[Field ("AVAssetExportPreset1280x720")]
		NSString Preset1280x720 { get; }

		[Field ("AVAssetExportPresetAppleM4A")]
		NSString PresetAppleM4A { get; }

		[Field ("AVAssetExportPresetPassthrough")]
		NSString PresetPassthrough { get; }

		// 5.0 APIs
		[Export ("asset")]
		AVAsset Asset { get; }

		[Export ("estimatedOutputFileLength")]
		long EstimatedOutputFileLength { get; }
#if !MONOMAC
		[Since (6,0)]
		[Static, Export ("determineCompatibilityOfExportPreset:withAsset:outputFileType:completionHandler:")]
		[Async]
		void DetermineCompatibilityOfExportPreset (string presetName, AVAsset asset, string outputFileType, Action<bool> isCompatibleResult);

		[Since (6,0)]
		[Export ("determineCompatibleFileTypesWithCompletionHandler:")]
		[Async]
		void DetermineCompatibleFileTypes (Action<string []> compatibleFileTypesHandler);
#endif
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVAudioMix {
		[Export ("inputParameters", ArgumentSemantic.Copy)]
		AVAudioMixInputParameters [] InputParameters { get;  }
	}

	[Since (4,0)]
	[BaseType (typeof (AVAudioMix))]
	interface AVMutableAudioMix {
		[Export ("inputParameters", ArgumentSemantic.Copy)]
		AVAudioMixInputParameters [] InputParameters { get; set;  }

		[Static, Export ("audioMix")]
		AVMutableAudioMix Create ();
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVAudioMixInputParameters {
		[Export ("trackID")]
		int TrackID { get;  }

		[Export ("getVolumeRampForTime:startVolume:endVolume:timeRange:")]
		bool GetVolumeRamp (CMTime forTime, ref float startVolume, ref float endVolume, ref CMTimeRange timeRange);

#if !MONOMAC
		[Since (6,0)]
		[Export ("audioTapProcessor", ArgumentSemantic.Retain)]
		MTAudioProcessingTap AudioTapProcessor { get; }
#endif
	}


	[BaseType (typeof (AVAudioMixInputParameters))]
	interface AVMutableAudioMixInputParameters {
		[Export ("trackID")]
		int TrackID { get; set;  }

		[Static]
		[Export ("audioMixInputParametersWithTrack:")]
		AVMutableAudioMixInputParameters FromTrack (AVAssetTrack track);

		[Static]
		[Export ("audioMixInputParameters")]
		AVMutableAudioMixInputParameters Create ();
		
		[Export ("setVolumeRampFromStartVolume:toEndVolume:timeRange:")]
		void SetVolumeRamp (float startVolume, float endVolume, CMTimeRange timeRange);

		[Export ("setVolume:atTime:")]
		void SetVolume (float volume, CMTime atTime);

#if !MONOMAC
		[Since (6,0)]
		[Export ("audioTapProcessor", ArgumentSemantic.Retain)]
		MTAudioProcessingTap AudioTapProcessor { get; set; }
#endif
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoComposition {
		[Export ("frameDuration")]
		CMTime FrameDuration { get;  }

		[Export ("renderSize")]
		CGSize RenderSize { get;  }

		[Export ("instructions", ArgumentSemantic.Copy)]
		AVVideoCompositionInstruction [] Instructions { get;  }

		[Export ("animationTool", ArgumentSemantic.Retain)]
		AVVideoCompositionCoreAnimationTool AnimationTool { get;  }
#if !MONOMAC
		[Export ("renderScale")]
		float RenderScale { get; set; }
#endif
		[Since (5,0)]
		[Export ("isValidForAsset:timeRange:validationDelegate:")]
                bool IsValidForAsset (AVAsset asset, CMTimeRange timeRange, AVVideoCompositionValidationHandling validationDelegate);
#if !MONOMAC
		[Since (6,0)]
		[Static, Export ("videoCompositionWithPropertiesOfAsset:")]
		AVVideoComposition FromAssetProperties (AVAsset asset);
#endif
	}

	[Since (5,0)]
	[BaseType (typeof (NSObject))]
        [Model]
	[Protocol]
	[DisableDefaultCtor]
        interface AVVideoCompositionValidationHandling {
                [Export ("videoComposition:shouldContinueValidatingAfterFindingInvalidValueForKey:")]
                bool ShouldContinueValidatingAfterFindingInvalidValueForKey (AVVideoComposition videoComposition, string key);

                [Export ("videoComposition:shouldContinueValidatingAfterFindingEmptyTimeRange:")]
                bool ShouldContinueValidatingAfterFindingEmptyTimeRange (AVVideoComposition videoComposition, CMTimeRange timeRange);

                [Export ("videoComposition:shouldContinueValidatingAfterFindingInvalidTimeRangeInInstruction:")]
                bool ShouldContinueValidatingAfterFindingInvalidTimeRangeInInstruction (AVVideoComposition videoComposition, AVVideoCompositionInstruction videoCompositionInstruction);

                [Export ("videoComposition:shouldContinueValidatingAfterFindingInvalidTrackIDInInstruction:layerInstruction:asset:")]
                bool ShouldContinueValidatingAfterFindingInvalidTrackIDInInstruction (AVVideoComposition videoComposition, AVVideoCompositionInstruction videoCompositionInstruction, AVVideoCompositionLayerInstruction layerInstruction, AVAsset asset);
        }

	[Since (4,0)]
	[BaseType (typeof (AVVideoComposition))]
	interface AVMutableVideoComposition {
		[Export ("frameDuration")]
		CMTime FrameDuration { get; set;  }

		[Export ("renderSize")]
		CGSize RenderSize { get; set;  }

		[Export ("instructions", ArgumentSemantic.Copy)]
		AVVideoCompositionInstruction [] Instructions { get; set;  }

		[Export ("animationTool")]
		AVVideoCompositionCoreAnimationTool AnimationTool { get; set;  }
#if !MONOMAC
		[Export ("renderScale")]
		float RenderScale { get; set; }
#endif
		[Static, Export ("videoComposition")]
		AVMutableVideoComposition Create ();
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoCompositionInstruction {
		[Export ("timeRange")]
		CMTimeRange TimeRange { get;  }

		[Export ("backgroundColor")]
		CGColor BackgroundColor { get; set;  }

		[Export ("layerInstructions", ArgumentSemantic.Copy)]
		AVVideoCompositionLayerInstruction [] LayerInstructions { get;  }

		[Export ("enablePostProcessing")]
		bool EnablePostProcessing { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVVideoCompositionInstruction))]
	interface AVMutableVideoCompositionInstruction {
		[Export ("timeRange")]
		CMTimeRange TimeRange { get; set;  }

		[Export ("backgroundColor")]
		CGColor BackgroundColor { get; set;  }

		[Export ("enablePostProcessing")]
		bool EnablePostProcessing { get; set; }

		[Export ("layerInstructions", ArgumentSemantic.Copy)]
		AVVideoCompositionLayerInstruction [] LayerInstructions { get; set;  }

		[Static, Export ("videoCompositionInstruction")]
		AVVideoCompositionInstruction Create (); 		
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoCompositionLayerInstruction {
		[Export ("trackID")]
		int TrackID { get;  }

		[Export ("getTransformRampForTime:startTransform:endTransform:timeRange:")]
		bool GetTransformRamp (CMTime time, ref CGAffineTransform startTransform, ref CGAffineTransform endTransform, ref CMTimeRange timeRange);

		[Export ("getOpacityRampForTime:startOpacity:endOpacity:timeRange:")]
		bool GetOpacityRamp (CMTime time, ref float startOpacity, ref float endOpacity, ref CMTimeRange timeRange);
	}

	[Since (4,0)]
	[BaseType (typeof (AVVideoCompositionLayerInstruction))]
	interface AVMutableVideoCompositionLayerInstruction {
		[Export ("trackID")]
		int TrackID { get; set;  }

		[Static]
		[Export ("videoCompositionLayerInstructionWithAssetTrack:")]
		AVMutableVideoCompositionLayerInstruction FromAssetTrack (AVAssetTrack track);

		[Static]
		[Export ("videoCompositionLayerInstruction")]
		AVMutableVideoCompositionLayerInstruction Create ();
		
		[Export ("setTransformRampFromStartTransform:toEndTransform:timeRange:")]
		void SetTransformRamp (CGAffineTransform startTransform, CGAffineTransform endTransform, CMTimeRange timeRange);

		[Export ("setTransform:atTime:")]
		void SetTransform (CGAffineTransform transform, CMTime atTime);

		[Export ("setOpacityRampFromStartOpacity:toEndOpacity:timeRange:")]
		void SetOpacityRamp (float startOpacity, float endOpacity, CMTimeRange timeRange);

		[Export ("setOpacity:atTime:")]
		void SetOpacity (float opacity, CMTime time);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoCompositionCoreAnimationTool {
		[Static]
		[Export ("videoCompositionCoreAnimationToolWithAdditionalLayer:asTrackID:")]
		AVVideoCompositionCoreAnimationTool FromLayer (CALayer layer, int trackID);

		[Static]
		[Export ("videoCompositionCoreAnimationToolWithPostProcessingAsVideoLayer:inLayer:")]
		AVVideoCompositionCoreAnimationTool FromLayer (CALayer videoLayer, CALayer animationLayer);
	}

	interface AVCaptureSessionRuntimeErrorEventArgs {
		[Export ("AVCaptureSessionErrorKey")]
		NSError Error { get; }
	}
	
	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureSession {
		[Export ("sessionPreset")]
		NSString SessionPreset { get; set;  }

		[Export ("inputs")]
		AVCaptureInput [] Inputs { get;  }

		[Export ("outputs")]
		AVCaptureOutput [] Outputs { get;  }

		[Export ("running")]
		bool Running { [Bind ("isRunning")] get;  }
#if !MONOMAC
		[Export ("interrupted")]
		bool Interrupted { [Bind ("isInterrupted")] get;  }
#endif
		[Export ("canSetSessionPreset:")]
		bool CanSetSessionPreset (NSString preset);

		[Export ("canAddInput:")]
		bool CanAddInput (AVCaptureInput input);

		[Export ("addInput:")]
		void AddInput (AVCaptureInput input);

		[Export ("removeInput:")]
		void RemoveInput (AVCaptureInput input);

		[Export ("canAddOutput:")]
		bool CanAddOutput (AVCaptureOutput output);

		[Export ("addOutput:")]
		void AddOutput (AVCaptureOutput output);

		[Export ("removeOutput:")]
		void RemoveOutput (AVCaptureOutput output);

		[Export ("beginConfiguration")]
		void BeginConfiguration ();

		[Export ("commitConfiguration")]
		void CommitConfiguration ();

		[Export ("startRunning")]
		void StartRunning ();

		[Export ("stopRunning")]
		void StopRunning ();

		[Field ("AVCaptureSessionPresetPhoto")]
		NSString PresetPhoto { get; }
		
		[Field ("AVCaptureSessionPresetHigh")]
		NSString PresetHigh { get; }
		
		[Field ("AVCaptureSessionPresetMedium")]
		NSString PresetMedium { get; }
		
		[Field ("AVCaptureSessionPresetLow")]
		NSString PresetLow { get; }
		
		[Field ("AVCaptureSessionPreset640x480")]
		NSString Preset640x480 { get; }
		
		[Field ("AVCaptureSessionPreset1280x720")]
		NSString Preset1280x720 { get; }

		[Field ("AVCaptureSessionPresetiFrame960x540")]
		NSString PresetiFrame960x540 { get; }

		[Field ("AVCaptureSessionPresetiFrame1280x720")]
		NSString PresetiFrame1280x720 { get; }

		[Field ("AVCaptureSessionPreset352x288")]
		NSString Preset352x288 { get; }

		[Field ("AVCaptureSessionRuntimeErrorNotification")]
		[Notification (typeof (AVCaptureSessionRuntimeErrorEventArgs))]
		NSString RuntimeErrorNotification { get; }
		
		[Field ("AVCaptureSessionErrorKey")]
		NSString ErrorKey { get; }
		
		[Field ("AVCaptureSessionDidStartRunningNotification")]
		[Notification]
		NSString DidStartRunningNotification { get; }
		
		[Field ("AVCaptureSessionDidStopRunningNotification")]
		[Notification]
		NSString DidStopRunningNotification { get; }
#if !MONOMAC
		[Field ("AVCaptureSessionWasInterruptedNotification")]
		[Notification]
		NSString WasInterruptedNotification { get; }
		
		[Field ("AVCaptureSessionInterruptionEndedNotification")]
		[Notification]
		NSString InterruptionEndedNotification { get; }
#endif
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVCaptureConnection {
		[Export ("output")]
		AVCaptureOutput Output { get;  }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("audioChannels")]
		AVCaptureAudioChannel AudioChannels { get;  }

		[Export ("videoMirrored")]
		bool VideoMirrored { [Bind ("isVideoMirrored")] get; set;  }

		[Export ("videoOrientation")]
		AVCaptureVideoOrientation VideoOrientation { get; set;  }

		[Export ("inputPorts")]
		AVCaptureInputPort [] InputPorts { get; }

		[Export ("isActive")]
		bool Active { get; }

		[Export ("isVideoMirroringSupported")]
		bool SupportsVideoMirroring { get; }

		[Export ("isVideoOrientationSupported")]
		bool SupportsVideoOrientation { get; }

		[Export ("supportsVideoMinFrameDuration"), Internal]
		bool _SupportsVideoMinFrameDuration { [Bind ("isVideoMinFrameDurationSupported")] get;  }

		[Export ("videoMinFrameDuration")]
		CMTime VideoMinFrameDuration { get; set;  }
#if !MONOMAC
		[Export ("supportsVideoMaxFrameDuration"), Internal]
		bool _SupportsVideoMaxFrameDuration { [Bind ("isVideoMaxFrameDurationSupported")] get;  }

		[Export ("videoMaxFrameDuration")]
		CMTime VideoMaxFrameDuration { get; set;  }

		[Export ("videoMaxScaleAndCropFactor")]
		float VideoMaxScaleAndCropFactor { get;  }

		[Export ("videoScaleAndCropFactor")]
		float VideoScaleAndCropFactor { get; set;  }
#endif
		[Since (6,0)]
		[Export ("videoPreviewLayer")]
		AVCaptureVideoPreviewLayer VideoPreviewLayer { get;  }

		[Since (6,0)]
		[Export ("automaticallyAdjustsVideoMirroring")]
		bool AutomaticallyAdjustsVideoMirroring { get; set;  }
#if !MONOMAC
		[Since (6,0)]
		[Export ("supportsVideoStabilization")]
		bool SupportsVideoStabilization { [Bind ("isVideoStabilizationSupported")] get;  }

		[Since (6,0)]
		[Export ("videoStabilizationEnabled")]
		bool VideoStabilizationEnabled { [Bind ("isVideoStabilizationEnabled")] get;  }

		[Since (6,0)]
		[Export ("enablesVideoStabilizationWhenAvailable")]
		bool EnablesVideoStabilizationWhenAvailable { get; set;  }
#endif
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVCaptureAudioChannel {
		[Export ("peakHoldLevel")]
		float PeakHoldLevel { get;  }

		[Export ("averagePowerLevel")]
		float AveragePowerLevel { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	// Objective-C exception thrown.  Name: NSGenericException Reason: Cannot instantiate AVCaptureInput because it is an abstract superclass.
	[DisableDefaultCtor]
	interface AVCaptureInput {
		[Export ("ports")]
		AVCaptureInputPort [] Ports { get; }

		[Field ("AVCaptureInputPortFormatDescriptionDidChangeNotification")]
		[Notification]
		NSString PortFormatDescriptionDidChangeNotification { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureInputPort {
		[Export ("mediaType")]
		string MediaType { get;  }

		[Export ("formatDescription")]
		CMFormatDescription FormatDescription { get;  }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("input")]
		AVCaptureInput Input  { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureInput))]
	// crash application if 'init' is called
	[DisableDefaultCtor]
	interface AVCaptureDeviceInput {
		[Export ("device")]
		AVCaptureDevice Device { get;  }

		[Static, Export ("deviceInputWithDevice:error:")]
		AVCaptureDeviceInput FromDevice (AVCaptureDevice device, out NSError error);

		[Export ("initWithDevice:error:")]
		IntPtr Constructor (AVCaptureDevice device, out NSError error);

	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSGenericException Reason: Cannot instantiate AVCaptureOutput because it is an abstract superclass.
	[DisableDefaultCtor]
	interface AVCaptureOutput {
		[Export ("connections")]
		AVCaptureConnection [] Connections { get; }

		[Export ("connectionWithMediaType:")]
                AVCaptureConnection ConnectionFromMediaType (NSString avMediaType);
#if !MONOMAC
		[Export ("transformedMetadataObjectForMetadataObject:connection:")]
		AVMetadataObject GetTransformedMetadataObject (AVMetadataObject metadataObject, AVCaptureConnection connection);
#endif
	}

	[Since (4,0)]
        [BaseType (typeof (CALayer))]
        interface AVCaptureVideoPreviewLayer {
                [Export ("session")]
                AVCaptureSession Session { get; set;  }
#if MONOMAC
		[Lion]
		[Export ("setSessionWithNoConnection:")]
		void SetSessionWithNoConnection (AVCaptureSession session);
#else
                [Export ("orientation")]
		[Obsolete ("Deprecated in iOS 6.0")]
                AVCaptureVideoOrientation Orientation { get; set;  }

                [Export ("automaticallyAdjustsMirroring")]
		[Obsolete ("Deprecated in iOS 6.0")]
                bool AutomaticallyAdjustsMirroring { get; set;  }

                [Export ("mirrored")]
		[Obsolete ("Deprecated in iOS 6.0")]
                bool Mirrored { [Bind ("isMirrored")] get; set;  }

		[Export ("isMirroringSupported")]
		[Obsolete ("Deprecated in iOS 6.0")]
		bool MirroringSupported { get; }

		[Export ("isOrientationSupported")]
		[Obsolete ("Deprecated in iOS 6.0")]
		bool OrientationSupported { get; }

		[Advice ("Use LayerVideoGravity")]
		[Export ("videoGravity")][Sealed]
		string VideoGravity { get; set; }

		[Export ("videoGravity")][Protected]
		NSString WeakVideoGravity { get; set; }
#endif
                [Static, Export ("layerWithSession:")]
                AVCaptureVideoPreviewLayer FromSession (AVCaptureSession session);

                [Export ("initWithSession:")]
                IntPtr Constructor (AVCaptureSession session);

		[Since (6,0)]
		[Export ("connection")]
		AVCaptureConnection Connection { get; }
#if !MONOMAC
		[Since (6,0)]
		[Export ("captureDevicePointOfInterestForPoint:")]
		CGPoint CaptureDevicePointOfInterestForPoint (CGPoint pointInLayer);

		[Since (6,0)]
		[Export ("pointForCaptureDevicePointOfInterest:")]
		CGPoint PointForCaptureDevicePointOfInterest (CGPoint captureDevicePointOfInterest);

		[Since (6,0)]
		[Export ("transformedMetadataObjectForMetadataObject:")]
		AVMetadataObject GetTransformedMetadataObject (AVMetadataObject metadataObject);
#endif
        }
	
	[Since (4,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureVideoDataOutput {
		[Export ("sampleBufferDelegate")]
		AVCaptureVideoDataOutputSampleBufferDelegate SampleBufferDelegate { get; }

		[Export ("sampleBufferCallbackQueue")]
		DispatchQueue SampleBufferCallbackQueue { get;  }

		[Export ("videoSettings", ArgumentSemantic.Copy), NullAllowed]
		NSDictionary WeakVideoSettings { get; set;  }

		[Wrap ("WeakVideoSettings")]
		AVVideoSettingsUncompressed UncompressedVideoSetting { get; set; }

		[Wrap ("WeakVideoSettings")]
		AVVideoSettingsCompressed CompressedVideoSetting { get; set; }

		[Export ("minFrameDuration")]
		[Obsolete ("Deprecated in iOS 5.0. Use AVCaptureConnection's MinVideoFrameDuration")]
		CMTime MinFrameDuration { get; set;  }

		[Export ("alwaysDiscardsLateVideoFrames")]
		bool AlwaysDiscardsLateVideoFrames { get; set;  }

		[Export ("setSampleBufferDelegate:queue:")]
		[PostGet ("SampleBufferDelegate")]
		[PostGet ("SampleBufferCallbackQueue")]
		void SetSampleBufferDelegate ([NullAllowed] AVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate, [NullAllowed] DispatchQueue sampleBufferCallbackQueue);

		// 5.0 APIs
		[Export ("availableVideoCVPixelFormatTypes")]
                NSNumber [] AvailableVideoCVPixelFormatTypes { get;  }

		// This is an NSString, because these are are codec types that can be used as keys in
		// the WeakVideoSettings properties.
                [Export ("availableVideoCodecTypes")]
                NSString [] AvailableVideoCodecTypes { get;  }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	[Model]
	[Protocol]
	interface AVCaptureVideoDataOutputSampleBufferDelegate {
		[Export ("captureOutput:didOutputSampleBuffer:fromConnection:")]
		// CMSampleBufferRef		
		void DidOutputSampleBuffer (AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection);
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureAudioDataOutput {
		[Export ("sampleBufferDelegate")]
		AVCaptureAudioDataOutputSampleBufferDelegate SampleBufferDelegate { get;  }

		[Export ("sampleBufferCallbackQueue")]
		DispatchQueue SampleBufferCallbackQueue { get;  }

		[Export ("setSampleBufferDelegate:queue:")]
		void SetSampleBufferDelegatequeue (AVCaptureAudioDataOutputSampleBufferDelegate sampleBufferDelegate, DispatchQueue sampleBufferCallbackDispatchQueue);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVCaptureAudioDataOutputSampleBufferDelegate {
		[Export ("captureOutput:didOutputSampleBuffer:fromConnection:")]
		void DidOutputSampleBuffer (AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection);

		[Export ("captureOutput:didDropSampleBuffer:fromConnection:")]
		void DidDropSampleBuffer (AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection);
	}

	[BaseType (typeof (AVCaptureOutput))]
	[Since (4,0)]
	// Objective-C exception thrown.  Name: NSGenericException Reason: Cannot instantiate AVCaptureFileOutput because it is an abstract superclass.
	[DisableDefaultCtor]
	interface AVCaptureFileOutput {
		[Export ("recordedDuration")]
		CMTime RecordedDuration { get;  }

		[Export ("recordedFileSize")]
		long RecordedFileSize { get;  }

		[Export ("isRecording")]
		bool Recording { get; }

		[Export ("maxRecordedDuration")]
		CMTime MaxRecordedDuration { get; set;  }

		[Export ("maxRecordedFileSize")]
		long MaxRecordedFileSize { get; set;  }

		[Export ("minFreeDiskSpaceLimit")]
		long MinFreeDiskSpaceLimit { get; set;  }

		[Export ("outputFileURL")]
		NSUrl OutputFileURL { get; }

		[Export ("startRecordingToOutputFileURL:recordingDelegate:")]
		void StartRecordingToOutputFile (NSUrl outputFileUrl, AVCaptureFileOutputRecordingDelegate recordingDelegate);

		[Export ("stopRecording")]
		void StopRecording ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Since (4,0)]
	[Protocol]
	interface AVCaptureFileOutputRecordingDelegate {
		[Export ("captureOutput:didStartRecordingToOutputFileAtURL:fromConnections:")]
		void DidStartRecording (AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject [] connections);

		[Export ("captureOutput:didFinishRecordingToOutputFileAtURL:fromConnections:error:"), CheckDisposed]
		void FinishedRecording (AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject [] connections, NSError error);
	}

#if !MONOMAC
	[Since (6,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureMetadataOutput {
		[Export ("metadataObjectsDelegate")]
		AVCaptureMetadataOutputObjectsDelegate Delegate { get;  }

		[Export ("metadataObjectsCallbackQueue")]
		DispatchQueue CallbackQueue { get;  }

		[Export ("availableMetadataObjectTypes")]
		NSString [] AvailableMetadataObjectTypes { get;  }

		[Export ("metadataObjectTypes")]
		NSArray MetadataObjectTypes { get; set;  }

		[Export ("setMetadataObjectsDelegate:queue:")]
		void SetDelegate (AVCaptureMetadataOutputObjectsDelegate  objectsDelegate, DispatchQueue objectsCallbackQueue);
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVCaptureMetadataOutputObjectsDelegate {
		[Export ("captureOutput:didOutputMetadataObjects:fromConnection:")]
		void DidOutputMetadataObjects (AVCaptureMetadataOutput captureOutput, AVMetadataObject [] metadataObjects, AVCaptureConnection connection);
	}
#endif
	
	[Since (4,0)]
	[BaseType (typeof (AVCaptureFileOutput))]
	interface AVCaptureMovieFileOutput {
		[Export ("metadata")]
		AVMetadataItem [] Metadata { get; set;  }

		[Export ("movieFragmentInterval")]
		CMTime MovieFragmentInterval { get; set; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureStillImageOutput {
		[Export ("availableImageDataCVPixelFormatTypes")]
		NSNumber [] AvailableImageDataCVPixelFormatTypes { get;  }

		[Export ("availableImageDataCodecTypes")]
		string [] AvailableImageDataCodecTypes { get; }
		
		[Export ("outputSettings", ArgumentSemantic.Copy)]
		NSDictionary OutputSettings { get; set; }

		[Wrap ("OutputSettings")]
		AVVideoSettingsUncompressed UncompressedVideoSetting { get; set; }

		[Wrap ("OutputSettings")]
		AVVideoSettingsCompressed CompressedVideoSetting { get; set; }

		[Export ("captureStillImageAsynchronouslyFromConnection:completionHandler:")]
		[Async ("CaptureStillImageTaskAsync")]
		void CaptureStillImageAsynchronously (AVCaptureConnection connection, AVCaptureCompletionHandler completionHandler);

		[Static, Export ("jpegStillImageNSDataRepresentation:")]
		NSData JpegStillToNSData (MonoMac.CoreMedia.CMSampleBuffer buffer);

		// 5.0
		[Export ("capturingStillImage")]
		bool CapturingStillImage { [Bind ("isCapturingStillImage")] get;  }
	}
		
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: Cannot instantiate a AVCaptureDevice directly.
	[DisableDefaultCtor]
	interface AVCaptureDevice {
		[Export ("uniqueID")]
		string UniqueID { get;  }

		[Export ("modelID")]
		string ModelID { get;  }

		[Export ("localizedName")]
		string LocalizedName { get;  }

		[Export ("connected")]
		bool Connected { [Bind ("isConnected")] get;  }

		[Static, Export ("devices")]
		AVCaptureDevice [] Devices { get;  }

		[Static]
		[Export ("devicesWithMediaType:")]
		AVCaptureDevice [] DevicesWithMediaType (string mediaType);

		[Static]
		[Export ("defaultDeviceWithMediaType:")]
		AVCaptureDevice DefaultDeviceWithMediaType (string mediaType);

		[Static]
		[Export ("deviceWithUniqueID:")]
		AVCaptureDevice DeviceWithUniqueID (string deviceUniqueID);

		[Export ("hasMediaType:")]
		bool HasMediaType (string mediaType);

		[Export ("lockForConfiguration:")]
		bool LockForConfiguration (out NSError error);

		[Export ("unlockForConfiguration")]
		void UnlockForConfiguration ();

		[Export ("supportsAVCaptureSessionPreset:")]
		bool SupportsAVCaptureSessionPreset (string preset);

		[Export ("flashMode")]
		AVCaptureFlashMode FlashMode { get; set;  }

		[Export ("isFlashModeSupported:")]
		bool IsFlashModeSupported (AVCaptureFlashMode flashMode);

		[Export ("torchMode")]
		AVCaptureTorchMode TorchMode { get; set;  }

		[Export ("isTorchModeSupported:")]
		bool IsTorchModeSupported (AVCaptureTorchMode torchMode);

		[Export ("isFocusModeSupported:")]
		bool IsFocusModeSupported (AVCaptureFocusMode focusMode);
		
		[Export ("focusMode")]
		AVCaptureFocusMode FocusMode { get; set;  }

		[Export ("focusPointOfInterestSupported")]
		bool FocusPointOfInterestSupported { [Bind ("isFocusPointOfInterestSupported")] get;  }

		[Export ("focusPointOfInterest")]
		CGPoint FocusPointOfInterest { get; set;  }

		[Export ("adjustingFocus")]
		bool AdjustingFocus { [Bind ("isAdjustingFocus")] get;  }

		[Export ("exposureMode")]
		AVCaptureExposureMode ExposureMode { get; set;  }

		[Export ("isExposureModeSupported:")]
		bool IsExposureModeSupported (AVCaptureExposureMode exposureMode);

		[Export ("exposurePointOfInterestSupported")]
		bool ExposurePointOfInterestSupported { [Bind ("isExposurePointOfInterestSupported")] get;  }

		[Export ("exposurePointOfInterest")]
		CGPoint ExposurePointOfInterest { get; set;  }

		[Export ("adjustingExposure")]
		bool AdjustingExposure { [Bind ("isAdjustingExposure")] get;  }

		[Export ("isWhiteBalanceModeSupported:")]
		bool IsWhiteBalanceModeSupported (AVCaptureWhiteBalanceMode whiteBalanceMode);
		
		[Export ("whiteBalanceMode")]
		AVCaptureWhiteBalanceMode WhiteBalanceMode { get; set;  }

		[Export ("adjustingWhiteBalance")]
		bool AdjustingWhiteBalance { [Bind ("isAdjustingWhiteBalance")] get;  }

		[Export ("position")]
		AVCaptureDevicePosition Position { get; }

		[Field ("AVCaptureDeviceWasConnectedNotification")]
		[Notification]
		NSString WasConnectedNotification { get; }

		[Field ("AVCaptureDeviceWasDisconnectedNotification")]
		[Notification]
		NSString WasDisconnectedNotification { get; }
#if !MONOMAC
		[Field ("AVCaptureDeviceSubjectAreaDidChangeNotification")]
		[Notification]
		NSString SubjectAreaDidChangeNotification { get; }

		// 5.0
		[Since(5,0)]
		[Export ("isFlashAvailable")]
		bool FlashAvailable { get;  }

		[Since(5,0)]
		[Export ("isFlashActive")]
		bool FlashActive { get; }

		[Since(5,0)]
		[Export ("isTorchAvailable")]
		bool TorchAvailable { get; }

		[Since(5,0)]
		[Export ("torchLevel")]
		float TorchLevel { get; }

		// 6.0
		[Since (6,0)]
		[Export ("torchActive")]
		bool TorchActive { [Bind ("isTorchActive")] get;  }

		[Since (6,0)]
		[Export ("setTorchModeOnWithLevel:error:")]
		bool SetTorchModeLevel (float torchLevel, out NSError outError);

		[Since (6,0)]
		[Field ("AVCaptureMaxAvailableTorchLevel")]
		float MaxAvailableTorchLevel { get; }

		[Since (6,0)]
		[Export ("lowLightBoostSupported")]
		bool LowLightBoostSupported { [Bind ("isLowLightBoostSupported")] get; }

		[Since (6,0)]
		[Export ("lowLightBoostEnabled")]
		bool LowLightBoostEnabled { [Bind ("isLowLightBoostEnabled")] get; }

		[Since (6,0)]
		[Export ("automaticallyEnablesLowLightBoostWhenAvailable")]
		bool AutomaticallyEnablesLowLightBoostWhenAvailable { get; set; }
#endif
	}

	public delegate void AVCompletionHandler ();
	public delegate void AVCaptureCompletionHandler (MonoMac.CoreMedia.CMSampleBuffer imageDataSampleBuffer, NSError error);

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVPlayer {
		[Export ("currentItem")]
		AVPlayerItem CurrentItem { get;  }

		[Export ("rate")]
		float Rate { get; set;  }

		// note: not a property in ObjC
		[Export ("currentTime")]
		CMTime CurrentTime { get; }

		[Export ("actionAtItemEnd")]
		AVPlayerActionAtItemEnd ActionAtItemEnd { get; set;  }

		[Export ("closedCaptionDisplayEnabled")]
		bool ClosedCaptionDisplayEnabled { [Bind ("isClosedCaptionDisplayEnabled")] get; set;  }

		[Static, Export ("playerWithURL:")]
		AVPlayer FromUrl (NSUrl URL);

		[Static]
		[Export ("playerWithPlayerItem:")]
		AVPlayer FromPlayerItem (AVPlayerItem item);

		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl URL);

		[Export ("initWithPlayerItem:")]
		IntPtr Constructor (AVPlayerItem item);

		[Export ("play")]
		void Play ();

		[Export ("pause")]
		void Pause ();

		[Export ("replaceCurrentItemWithPlayerItem:")]
		void ReplaceCurrentItemWithPlayerItem (AVPlayerItem item);

		[Export ("addPeriodicTimeObserverForInterval:queue:usingBlock:")]
		NSObject AddPeriodicTimeObserver (CMTime interval, DispatchQueue queue, AVTimeHandler handler);

		[Export ("addBoundaryTimeObserverForTimes:queue:usingBlock:")]
		NSObject AddBoundaryTimeObserver (NSValue []times, DispatchQueue queue, Action handler);

		[Export ("removeTimeObserver:")]
		void RemoveTimeObserver (NSObject observer);

		[Export ("seekToTime:")]
		void Seek (CMTime toTime);

		[Export ("seekToTime:toleranceBefore:toleranceAfter:")]
		void Seek (CMTime toTime, CMTime toleranceBefore, CMTime toleranceAfter);

		[Export ("error")]
		NSError Error { get; }

		[Export ("status")]
		AVPlayerStatus Status { get; }
#if !MONOMAC
		// 5.0
		[Since (5,0)]
		[Obsolete ("Deprecated in iOS 6.0. Use AllowsExternalPlayback instead")]
		[Export ("allowsAirPlayVideo")]
		bool AllowsAirPlayVideo { get; set;  }

		[Since (5,0)]
		[Obsolete ("Deprecated in iOS 6.0. Use ExternalPlaybackActive instead")]
		[Export ("airPlayVideoActive")]
		bool AirPlayVideoActive { [Bind ("isAirPlayVideoActive")] get;  }

		[Since (5,0)]
		[Obsolete ("Deprecated in iOS 6.0. Use UsesExternalPlaybackWhileExternalScreenIsActive instead")]
		[Export ("usesAirPlayVideoWhileAirPlayScreenIsActive")]
		bool UsesAirPlayVideoWhileAirPlayScreenIsActive { get; set;  }
#endif
		[Since (5,0)]
		[Export ("seekToTime:completionHandler:")]
		[Async]
		void Seek (CMTime time, AVCompletion completion);

		[Since (5,0)]
		[Export ("seekToTime:toleranceBefore:toleranceAfter:completionHandler:")]
		[Async]
		void Seek (CMTime time, CMTime toleranceBefore, CMTime toleranceAfter, AVCompletion completion);
#if !MONOMAC
		[Since (6,0)]
		[Export ("seekToDate:")]
		void Seek (NSDate date);

		[Since (6,0)]
		[Export ("seekToDate:completionHandler:")]
		[Async]
		void Seek (NSDate date, AVCompletion onComplete);
#endif
		[Since (6,0)]
		[Export ("setRate:time:atHostTime:")]
		void SetRate (float rate, CMTime itemTime, CMTime hostClockTime);

		[Since (6,0)]
		[Export ("prerollAtRate:completionHandler:")]
		[Async]
		void Preroll (float rate, AVCompletion onComplete);

		[Since (6,0)]
		[Export ("cancelPendingPrerolls")]
		void CancelPendingPrerolls ();
#if !MONOMAC
		[Since (6,0)]
		[Export ("outputObscuredDueToInsufficientExternalProtection")]
		bool OutputObscuredDueToInsufficientExternalProtection { get; }
#endif
		[Since (6,0)]
		[Export ("masterClock")]
		CMClock MasterClock { get; set; }
#if !MONOMAC
		[Since (6,0)]
		[Export ("allowsExternalPlayback")]
		bool AllowsExternalPlayback { get; set;  }

		[Since (6,0)]
		[Export ("externalPlaybackActive")]
		bool ExternalPlaybackActive { [Bind ("isExternalPlaybackActive")] get; }

		[Since (6,0)]
		[Export ("usesExternalPlaybackWhileExternalScreenIsActive")]
		bool UsesExternalPlaybackWhileExternalScreenIsActive { get; set;  }

		[Since (6,0)][Protected]
		[Export ("externalPlaybackVideoGravity")]
		NSString WeakExternalPlaybackVideoGravity { get; set; }
#endif
	}
#if !MONOMAC
	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	interface AVTextStyleRule {
		[Export ("textMarkupAttributes")][Protected]
		NSDictionary WeakTextMarkupAttributes { get;  }

		[Wrap ("WeakTextMarkupAttributes")]
		CMTextMarkupAttributes TextMarkupAttributes { get;  }

		[Export ("textSelector")]
		string TextSelector { get;  }

		[Static]
		[Export ("propertyListForTextStyleRules:")]
		NSObject ToPropertyList (AVTextStyleRule [] textStyleRules);

		[Static]
		[Export ("textStyleRulesFromPropertyList:")]
		AVTextStyleRule [] FromPropertyList (NSObject plist);

		[Static][Internal]
		[Export ("textStyleRuleWithTextMarkupAttributes:")]
		AVTextStyleRule FromTextMarkupAttributes (NSDictionary textMarkupAttributes);

		[Static]
		[Wrap ("FromTextMarkupAttributes (textMarkupAttributes == null ? null : textMarkupAttributes.Dictionary)")]
		AVTextStyleRule FromTextMarkupAttributes (CMTextMarkupAttributes textMarkupAttributes);

		[Static][Internal]
		[Export ("textStyleRuleWithTextMarkupAttributes:textSelector:")]
		AVTextStyleRule FromTextMarkupAttributes (NSDictionary textMarkupAttributes, [NullAllowed] string textSelector);

		[Static]
		[Wrap ("FromTextMarkupAttributes (textMarkupAttributes == null ? null : textMarkupAttributes.Dictionary, textSelector)")]
		AVTextStyleRule FromTextMarkupAttributes (CMTextMarkupAttributes textMarkupAttributes, [NullAllowed] string textSelector);

		[Export ("initWithTextMarkupAttributes:")]
		[Protected]
		IntPtr Constructor (NSDictionary textMarkupAttributes);

		[Wrap ("this (attributes == null ? null : attributes.Dictionary)")]
		IntPtr Constructor (CMTextMarkupAttributes attributes);

		[Export ("initWithTextMarkupAttributes:textSelector:")]
		[Protected]
		IntPtr Constructor (NSDictionary textMarkupAttributes, [NullAllowed] string textSelector);
	
		[Wrap ("this (attributes == null ? null : attributes.Dictionary, textSelector)")]
		IntPtr Constructor (CMTextMarkupAttributes attributes, string textSelector);
	}
#endif
	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	interface AVTimedMetadataGroup {
		[Export ("timeRange")]
		CMTimeRange TimeRange { get; [NotImplemented] set; }

		[Export ("items")]
		AVMetadataItem [] Items { get; [NotImplemented] set; }

		[Export ("initWithItems:timeRange:")]
		IntPtr Constructor (AVMetadataItem [] items, CMTimeRange timeRange);
	}

	[BaseType (typeof (AVTimedMetadataGroup))]
	interface AVMutableTimedMetadataGroup {
		[Export ("items")]
		[Override]
		AVMetadataItem [] Items { get; set;  }

		[Export ("timeRange")]
		[Override]
		CMTimeRange TimeRange { get; set; }
	}

	delegate void AVTimeHandler (CMTime time);

	interface AVPlayerItemErrorEventArgs {
		[Export ("AVPlayerItemFailedToPlayToEndTimeErrorKey")]
		NSError Error { get; }
	}
		
	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	interface AVPlayerItem {
		[Export ("status")]
		AVPlayerItemStatus Status { get;  }

		[Export ("asset")]
		AVAsset Asset { get;  }

		[Export ("tracks")]
		AVPlayerItemTrack [] Tracks { get;  }

		[Export ("presentationSize")]
		CGSize PresentationSize { get;  }

		[Export ("forwardPlaybackEndTime")]
		CMTime ForwardPlaybackEndTime { get; set;  }

		[Export ("reversePlaybackEndTime")]
		CMTime ReversePlaybackEndTime { get; set;  }

		[Export ("audioMix", ArgumentSemantic.Copy)]
		AVAudioMix AudioMix { get; set;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("currentTime")]
		CMTime CurrentTime { get; }

		[Export ("playbackLikelyToKeepUp")]
		bool PlaybackLikelyToKeepUp { [Bind ("isPlaybackLikelyToKeepUp")] get;  }

		[Export ("playbackBufferFull")]
		bool PlaybackBufferFull { [Bind ("isPlaybackBufferFull")] get;  }

		[Export ("playbackBufferEmpty")]
		bool PlaybackBufferEmpty { [Bind ("isPlaybackBufferEmpty")] get;  }

		[Export ("seekableTimeRanges")]
		NSValue [] SeekableTimeRanges { get;  }

		[Export ("loadedTimeRanges")]
		NSValue [] LoadedTimeRanges { get;  }

		[Export ("timedMetadata")]
		NSObject [] TimedMetadata { get;  }

		[Static, Export ("playerItemWithURL:")]
		AVPlayerItem FromUrl (NSUrl URL);

		[Static]
		[Export ("playerItemWithAsset:")]
		AVPlayerItem FromAsset (AVAsset asset);

		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl URL);

		[Export ("initWithAsset:")]
		IntPtr Constructor (AVAsset asset);

		[Export ("stepByCount:")]
		void StepByCount (int stepCount);

		[Export ("seekToDate:")]
		bool Seek (NSDate date);

		[Export ("seekToTime:")]
		void Seek (CMTime time);
		
		[Export ("seekToTime:toleranceBefore:toleranceAfter:")]
		void Seek (CMTime time, CMTime toleranceBefore, CMTime toleranceAfter);

		[Export ("error")]
		NSError Error { get; }

		[Field ("AVPlayerItemDidPlayToEndTimeNotification")]
		[Notification]
		NSString DidPlayToEndTimeNotification { get; }

		[Since (4,3)]
		[Field ("AVPlayerItemFailedToPlayToEndTimeNotification")]
		[Notification (typeof (AVPlayerItemErrorEventArgs))]
		NSString ItemFailedToPlayToEndTimeNotification { get; }

		[Since (4,3)]
		[Field ("AVPlayerItemFailedToPlayToEndTimeErrorKey")]
		NSString ItemFailedToPlayToEndTimeErrorKey { get; }

		[Since (4,3)]
		[Export ("accessLog")]
		AVPlayerItemAccessLog AccessLog { get; }

		[Since (4,3)]
		[Export ("errorLog")]
		AVPlayerItemErrorLog ErrorLog { get; }

		[Since (4,3)]
		[Export ("currentDate")]
		NSDate CurrentDate { get; }

		[Since (4,3)]
		[Export ("duration")]
		CMTime Duration { get; }

		[Since (5,0)]
		[Export ("canPlayFastReverse")]
		bool CanPlayFastReverse { get;  }

		[Since (5,0)]
		[Export ("canPlayFastForward")]
		bool CanPlayFastForward { get; }

		[Since (5,0)]
		[Field ("AVPlayerItemTimeJumpedNotification")]
		[Notification]
		NSString TimeJumpedNotification { get; }

		[Since (5,0)]
		[Export ("seekToTime:completionHandler:")]
		[Async]
		void Seek (CMTime time, AVCompletion completion);

		[Since (5,0)]
		[Export ("cancelPendingSeeks")]
		void CancelPendingSeeks ();

		[Since (5,0)]
		[Export ("seekToTime:toleranceBefore:toleranceAfter:completionHandler:")]
		[Async]
		void Seek (CMTime time, CMTime toleranceBefore, CMTime toleranceAfter, AVCompletion completion);
#if !MONOMAC
		[Since (5,0)]
		[Export ("selectMediaOption:inMediaSelectionGroup:")]
		void SelectMediaOption (AVMediaSelectionOption mediaSelectionOption, AVMediaSelectionGroup mediaSelectionGroup);

		[Export ("selectedMediaOptionInMediaSelectionGroup:")]
		AVMediaSelectionOption SelectedMediaOption (AVMediaSelectionGroup inMediaSelectionGroup);
#endif
		[Since (6,0)]
		[Export ("canPlaySlowForward")]
		bool CanPlaySlowForward { get;  }

		[Since (6,0)]
		[Export ("canPlayReverse")]
		bool CanPlayReverse { get;  }

		[Since (6,0)]
		[Export ("canPlaySlowReverse")]
		bool CanPlaySlowReverse { get;  }

		[Since (6,0)]
		[Export ("canStepForward")]
		bool CanStepForward { get;  }

		[Since (6,0)]
		[Export ("canStepBackward")]
		bool CanStepBackward { get;  }
		
		[Since (6,0)]
		[Export ("outputs")]
		AVPlayerItemOutput [] Outputs { get;  }

		[Since (6,0)]
		[Export ("addOutput:")]
		[PostGet ("Outputs")]
		void AddOutput (AVPlayerItemOutput output);

		[Since (6,0)]
		[Export ("removeOutput:")]
		[PostGet ("Outputs")]
		void RemoveOutput (AVPlayerItemOutput output);

		[Since (6,0)]
		[Export ("timebase")]
		CMTimebase Timebase { get;  }
#if !MONOMAC
		[Since (6,0)]
		[Export ("seekToDate:completionHandler:")]
		[Async]
		bool Seek (NSDate date, AVCompletion completion);

		[Since (6,0)]
		[Export ("seekingWaitsForVideoCompositionRendering")]
		bool SeekingWaitsForVideoCompositionRendering { get; set;  }

		[Since (6,0)]
		[Export ("textStyleRules")]
		AVTextStyleRule [] TextStyleRules { get; set;  }

		[Field ("AVPlayerItemPlaybackStalledNotification")]
		[Notification]
		NSString PlaybackStalledNotification { get; }
		
		[Field ("AVPlayerItemNewAccessLogEntryNotification")]
		[Notification]
		NSString NewAccessLogEntryNotification { get; }
		
		[Field ("AVPlayerItemNewErrorLogEntryNotification")]
		[Notification]
		NSString NewErrorLogEntryNotification { get; }
#endif
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** initialization method -init cannot be sent to an abstract object of class AVPlayerItemOutput: Create a concrete instance!
	[DisableDefaultCtor]
	interface AVPlayerItemOutput {
		[Export ("itemTimeForHostTime:")]
		CMTime GetItemTime (double hostTimeInSeconds);

		[Export ("itemTimeForMachAbsoluteTime:")]
		CMTime GetItemTime (long machAbsoluteTime);

		[Export ("suppressesPlayerRendering")]
		bool SuppressesPlayerRendering { get; set; }
	}

	[Since (6,0)]
	[BaseType (typeof (AVPlayerItemOutput))]
	interface AVPlayerItemVideoOutput {
		[Export ("delegate")]
		NSObject WeakDelegate { get;  }
		
		[Wrap ("WeakDelegate")]
		AVPlayerItemOutputPullDelegate Delegate  { get;  }

		[Export ("delegateQueue")]
		DispatchQueue DelegateQueue { get;  }

		[Export ("initWithPixelBufferAttributes:")]
		[Protected]
		IntPtr Constructor (NSDictionary pixelBufferAttributes);

		[Wrap ("this (attributes == null ? null : attributes.Dictionary)")]
		IntPtr Constructor (CVPixelBufferAttributes attributes);

		[Export ("hasNewPixelBufferForItemTime:")]
		bool HasNewPixelBufferForItemTime (CMTime itemTime);

		[Protected]
		[Export ("copyPixelBufferForItemTime:itemTimeForDisplay:")]
		IntPtr WeakCopyPixelBuffer (CMTime itemTime, ref CMTime outItemTimeForDisplay);

		[Export ("setDelegate:queue:")]
		void SetDelegate (AVPlayerItemOutputPullDelegate delegateClass, DispatchQueue delegateQueue);

		[Export ("requestNotificationOfMediaDataChangeWithAdvanceInterval:")]
		void RequestNotificationOfMediaDataChange (double advanceInterval);
	}

	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface AVPlayerItemOutputPullDelegate {
		[Export ("outputMediaDataWillChange:")]
		void OutputMediaDataWillChange (AVPlayerItemOutput sender);

		[Export ("outputSequenceWasFlushed:")]
		void OutputSequenceWasFlushed (AVPlayerItemOutput output);
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemAccessLog {
		[Export ("events")]
		AVPlayerItemAccessLogEvent [] Events { get; }

		[Export ("extendedLogDataStringEncoding")]
		NSStringEncoding ExtendedLogDataStringEncoding { get; }

		[Export ("extendedLogData")]
		NSData ExtendedLogData { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemErrorLog {
		[Export ("events")]
		AVPlayerItemErrorLogEvent [] Events { get; }

		[Export ("extendedLogDataStringEncoding")]
		NSStringEncoding ExtendedLogDataStringEncoding { get; }

		[Export ("extendedLogData")]
		NSData ExtendedLogData { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemAccessLogEvent {
		[Export ("numberOfSegmentsDownloaded")]
		int SegmentedDownloadedCount { get; }

		[Export ("playbackStartDate")]
		NSData PlaybackStartDate { get; }

		[Export ("URI")]
		string Uri { get; }

		[Export ("serverAddress")]
		string ServerAddress { get; }

		[Export ("numberOfServerAddressChanges")]
		int ServerAddressChangeCount { get; }

		[Export ("playbackSessionID")]
		string PlaybackSessionID { get; }

		[Export ("playbackStartOffset")]
		double PlaybackStartOffset { get; }

		[Export ("segmentsDownloadedDuration")]
		double SegmentsDownloadedDuration { get; }

		[Export ("durationWatched")]
		double DurationWatched { get; }

		[Export ("numberOfStalls")]
		int StallCount { get; }

		[Export ("numberOfBytesTransferred")]
		long BytesTransferred { get; }

		[Export ("observedBitrate")]
		double ObservedBitrate { get; }

		[Export ("indicatedBitrate")]
		double IndicatedBitrate { get; }

		[Export ("numberOfDroppedVideoFrames")]
		int DroppedVideoFrameCount { get; }
#if !MONOMAC
		[Since (6,0)]
		[Export ("numberOfMediaRequests")]
		int NumberOfMediaRequests { get; }
#endif
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemErrorLogEvent {
		[Export ("date")]
		NSDate Date { get; }

		[Export ("URI")]
		string Uri { get; }

		[Export ("serverAddress")]
		string ServerAddress { get; }

		[Export ("playbackSessionID")]
		string PlaybackSessionID { get; }

		[Export ("errorStatusCode")]
		int ErrorStatusCode { get; }

		[Export ("errorDomain")]
		string ErrorDomain { get; }

		[Export ("errorComment")]
		string ErrorComment { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (CALayer))]
	interface AVPlayerLayer {
		[Export ("player")]
		AVPlayer Player { get; set;  }

		[Static, Export ("playerLayerWithPlayer:")]
		AVPlayerLayer FromPlayer (AVPlayer player);

		[Advice ("Use LayerVideoGravity")]
		[Export ("videoGravity")][Sealed]
		string VideoGravity { get; set; }

		[Export ("videoGravity")][Protected]
		NSString WeakVideoGravity { get; set; }

		[Field ("AVLayerVideoGravityResizeAspect")]
		NSString GravityResizeAspect { get; }

		[Field ("AVLayerVideoGravityResizeAspectFill")]
		NSString GravityResizeAspectFill { get; }

		[Field ("AVLayerVideoGravityResize")]
		NSString GravityResize { get; }

		[Export ("isReadyForDisplay")]
		bool ReadyForDisplay { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVPlayerItemTrack {
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("assetTrack")]
		AVAssetTrack AssetTrack { get; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Since (4,0)]
	[Protocol]
	interface AVAsynchronousKeyValueLoading {
		[Abstract]
		[Export ("statusOfValueForKey:error:")]
		AVKeyValueStatus StatusOfValueForKeyerror (string key, IntPtr outError);

		[Abstract]
		[Export ("loadValuesAsynchronouslyForKeys:completionHandler:")]
		void LoadValuesAsynchronously (string [] keys, Action handler);
	}

	[Since (4,1)]
	[BaseType (typeof (AVPlayer))]
	interface AVQueuePlayer {
		[Static, Export ("queuePlayerWithItems:")]
		AVQueuePlayer FromItems (AVPlayerItem [] items);

		[Export ("initWithItems:")]
		IntPtr Constructor (AVPlayerItem [] items);

		[Export ("items")]
		AVPlayerItem [] Items { get; }

		[Export ("advanceToNextItem")]
		void AdvanceToNextItem ();

		[Export ("canInsertItem:afterItem:")]
		bool CanInsert (AVPlayerItem item, AVPlayerItem afterItem);

		[Export ("insertItem:afterItem:")]
		void InsertItem (AVPlayerItem item, AVPlayerItem afterItem);

		[Export ("removeItem:")]
		void RemoveItem (AVPlayerItem item);

		[Export ("removeAllItems")]
		void RemoveAllItems ();
	}

	[Static]
	public interface AVAudioSettings {
		[Field ("AVFormatIDKey")]
		NSString AVFormatIDKey { get; }
		
		[Field ("AVSampleRateKey")]
		NSString AVSampleRateKey { get; }
		
		[Field ("AVNumberOfChannelsKey")]
		NSString AVNumberOfChannelsKey { get; }
		
		[Field ("AVLinearPCMBitDepthKey")]
		NSString AVLinearPCMBitDepthKey { get; }
		
		[Field ("AVLinearPCMIsBigEndianKey")]
		NSString AVLinearPCMIsBigEndianKey { get; }
		
		[Field ("AVLinearPCMIsFloatKey")]
		NSString AVLinearPCMIsFloatKey { get; }
		
		[Field ("AVLinearPCMIsNonInterleaved")]
		NSString AVLinearPCMIsNonInterleaved { get; }
		
		[Field ("AVEncoderAudioQualityKey")]
		NSString AVEncoderAudioQualityKey { get; }
		
		[Field ("AVEncoderBitRateKey")]
		NSString AVEncoderBitRateKey { get; }
		
		[Field ("AVEncoderBitRatePerChannelKey")]
		NSString AVEncoderBitRatePerChannelKey { get; }
		
		[Field ("AVEncoderBitDepthHintKey")]
		NSString AVEncoderBitDepthHintKey { get; }
		
		[Field ("AVSampleRateConverterAudioQualityKey")]
		NSString AVSampleRateConverterAudioQualityKey { get; }
		
		[Field ("AVChannelLayoutKey")]
		NSString AVChannelLayoutKey { get; }
	}
	
}
