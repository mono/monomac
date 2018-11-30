// 
// AudioSettings.cs: Implements strongly typed access for AV audio settings
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, Xamarin Inc.
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
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.AudioToolbox;

namespace MonoMac.AVFoundation {

	// Should be called AVAudioSetting but AVAudioSetting has been already used by keys class
	[Since (6,0)]
	public class AudioSettings : DictionaryContainer
	{
#if !COREBUILD
		public AudioSettings ()
			: base (new NSMutableDictionary ())
		{
		}

		public AudioSettings (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public AudioFormatType? Format {
			set {
				SetNumberValue (AVAudioSettings.AVFormatIDKey, (int?) value);
			}
			get {
				return (AudioFormatType?)GetInt32Value (AVAudioSettings.AVFormatIDKey);
			}
		}

		public float? SampleRate {
			set {
				SetNumberValue (AVAudioSettings.AVSampleRateKey, value);
			}
			get {
				return GetFloatValue (AVAudioSettings.AVSampleRateKey);
			}
		}			

		public int? NumberChannels {
			set {
				SetNumberValue (AVAudioSettings.AVNumberOfChannelsKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVNumberOfChannelsKey);
			}
		}

		public int? LinearPcmBitDepth {
			set {
				if (!(value == 8 || value == 16 || value == 24 || value == 32))
					throw new ArgumentOutOfRangeException ("value must be of 8, 16, 24 or 32");

				SetNumberValue (AVAudioSettings.AVLinearPCMBitDepthKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVLinearPCMBitDepthKey);
			}
		}

		public bool? LinearPcmBigEndian {
			set {
				SetBooleanValue (AVAudioSettings.AVLinearPCMIsBigEndianKey, value);
			}
			get {
				return GetBoolValue (AVAudioSettings.AVLinearPCMIsBigEndianKey);
			}
		}

		public bool? LinearPcmFloat {
			set {
				SetBooleanValue (AVAudioSettings.AVLinearPCMIsFloatKey, value);
			}
			get {
				return GetBoolValue (AVAudioSettings.AVLinearPCMIsFloatKey);
			}
		}

		public bool? LinearPcmNonInterleaved {
			set {
				SetBooleanValue (AVAudioSettings.AVLinearPCMIsNonInterleaved, value);
			}
			get {
				return GetBoolValue (AVAudioSettings.AVLinearPCMIsNonInterleaved);
			}
		}

		public AVAudioQuality? AudioQuality {
			set {
				SetNumberValue (AVAudioSettings.AVEncoderAudioQualityKey, (int?) value);
			}
			get {
				return (AVAudioQuality?) GetInt32Value (AVAudioSettings.AVEncoderAudioQualityKey);
			}
		}

		public AVAudioQuality? SampleRateConverterAudioQuality {
			set {
				SetNumberValue (AVAudioSettings.AVSampleRateConverterAudioQualityKey, (int?) value);
			}
			get {
				return (AVAudioQuality?) GetInt32Value (AVAudioSettings.AVSampleRateConverterAudioQualityKey);
			}
		}

		public int? EncoderBitRate {
			set {
				SetNumberValue (AVAudioSettings.AVEncoderBitRateKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVEncoderBitRateKey);
			}			
		}

		public int? EncoderBitRatePerChannel {
			set {
				SetNumberValue (AVAudioSettings.AVEncoderBitRatePerChannelKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVEncoderBitRatePerChannelKey);
			}			
		}			

		public int? EncoderBitDepthHint {
			set {
				if (value < 8 || value > 32)
					throw new ArgumentOutOfRangeException ("value is required to be between 8 and 32");

				SetNumberValue (AVAudioSettings.AVEncoderBitDepthHintKey, value);
			}
			get {
				return GetInt32Value (AVAudioSettings.AVEncoderBitDepthHintKey);
			}			
		}

		public AudioChannelLayout ChannelLayout {
			set {
				SetNativeValue (AVAudioSettings.AVChannelLayoutKey, value == null ? null : value.AsData ());
			}
		}

#endif
	}
}

