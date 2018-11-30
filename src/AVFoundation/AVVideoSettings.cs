// 
// AVVideoSettings.cs: Implements strongly typed access for AV video settings
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
using MonoMac.CoreVideo;

namespace MonoMac.AVFoundation {

	public enum AVVideoCodec
	{
		H264 = 1,
		JPEG = 2
	}

	public enum AVVideoScalingMode
	{
		Fit,
		Resize,
		ResizeAspect,
		ResizeAspectFill
	}

	public enum AVVideoProfileLevelH264
	{
		Baseline30 = 1,
		Baseline31,
		Baseline41,
 		Main30,
		Main31,
		Main32,
		Main41,
#if !MONOMAC
		High40,
		High41
#endif
	}

	public class AVVideoSettingsUncompressed : CVPixelBufferAttributes
	{
#if !COREBUILD
		public AVVideoSettingsUncompressed ()
		{
		}

		public AVVideoSettingsUncompressed (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public AVVideoScalingMode? ScalingMode {
			set {
				NSString v;
				switch (value) {
				case AVVideoScalingMode.Fit:
					v = AVVideoScalingModeKey.Fit;
					break;
				case AVVideoScalingMode.Resize:
					v = AVVideoScalingModeKey.Resize;
					break;
				case AVVideoScalingMode.ResizeAspect:
					v = AVVideoScalingModeKey.ResizeAspect;
					break;
				case AVVideoScalingMode.ResizeAspectFill:
					v = AVVideoScalingModeKey.ResizeAspectFill;
					break;
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.ScalingModeKey);
				else
					SetNativeValue (AVVideo.ScalingModeKey, v);
			}
		}
#endif
	}

	public class AVVideoSettingsCompressed : DictionaryContainer
	{
#if !COREBUILD
		public AVVideoSettingsCompressed ()
			: base (new NSMutableDictionary ())
		{
		}

		public AVVideoSettingsCompressed (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public AVVideoCodec? Codec {
			set {
				NSString v;
				switch (value) {
				case AVVideoCodec.H264:
					v = AVVideo.CodecH264;
					break;
				case AVVideoCodec.JPEG:
					v = AVVideo.CodecJPEG;
					break;
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.CodecKey);
				else
					SetNativeValue (AVVideo.CodecKey, v);
			}
		}

		public int? Width {
			set {
				SetNumberValue (AVVideo.WidthKey, value);
			}
			get {
				return GetInt32Value (AVVideo.WidthKey);
			}
		}

		public int? Height {
			set {
				SetNumberValue (AVVideo.HeightKey, value);
			}
			get {
				return GetInt32Value (AVVideo.HeightKey);
			}
		}

		public AVVideoScalingMode? ScalingMode {
			set {
				NSString v;
				switch (value) {
				case AVVideoScalingMode.Fit:
					v = AVVideoScalingModeKey.Fit;
					break;
				case AVVideoScalingMode.Resize:
					v = AVVideoScalingModeKey.Resize;
					break;
				case AVVideoScalingMode.ResizeAspect:
					v = AVVideoScalingModeKey.ResizeAspect;
					break;
				case AVVideoScalingMode.ResizeAspectFill:
					v = AVVideoScalingModeKey.ResizeAspectFill;
					break;
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.ScalingModeKey);
				else
					SetNativeValue (AVVideo.ScalingModeKey, v);
			}
		}

		public AVVideoCodecSettings CodecSettings {
			set {
				SetNativeValue (AVVideo.CompressionPropertiesKey, value == null ? null : value.Dictionary);
			}
		}
#endif
	}

	public class AVVideoCodecSettings : DictionaryContainer
	{
#if !COREBUILD
		public AVVideoCodecSettings ()
			: base (new NSMutableDictionary ())
		{
		}

		public AVVideoCodecSettings (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public int? AverageBitRate {
			set {
				SetNumberValue (AVVideo.AverageBitRateKey, (int?) value);
			}
			get {
				return GetInt32Value (AVVideo.AverageBitRateKey);
			}
		}

		public float? JPEGQuality {
			set {
				SetNumberValue (AVVideo.QualityKey, value);
			}
			get {
				return GetFloatValue (AVVideo.QualityKey);
			}
		}

		public int? MaxKeyFrameInterval {
			set {
				SetNumberValue (AVVideo.MaxKeyFrameIntervalKey, (int?) value);
			}
			get {
				return GetInt32Value (AVVideo.MaxKeyFrameIntervalKey);
			}
		}

		public AVVideoProfileLevelH264? ProfileLevelH264 {
			set {
				NSString v;
				switch (value) {
				case AVVideoProfileLevelH264.Baseline30:
					v = AVVideo.ProfileLevelH264Baseline30;
					break;
				case AVVideoProfileLevelH264.Baseline31:
					v = AVVideo.ProfileLevelH264Baseline31;
					break;
				case AVVideoProfileLevelH264.Baseline41:
					v = AVVideo.ProfileLevelH264Baseline41;
					break;
				case AVVideoProfileLevelH264.Main30:
					v = AVVideo.ProfileLevelH264Main30;
					break;
				case AVVideoProfileLevelH264.Main31:
					v = AVVideo.ProfileLevelH264Main31;
					break;
				case AVVideoProfileLevelH264.Main32:
					v = AVVideo.ProfileLevelH264Main32;
					break;
				case AVVideoProfileLevelH264.Main41:
					v = AVVideo.ProfileLevelH264Main41;
					break;
#if !MONOMAC
				case AVVideoProfileLevelH264.High40:
					v = AVVideo.ProfileLevelH264High40;
					break;
				case AVVideoProfileLevelH264.High41:
					v = AVVideo.ProfileLevelH264High41;
					break;
#endif
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.ProfileLevelKey);
				else
					SetNativeValue (AVVideo.ProfileLevelKey, v);
			}
		}

		public AVVideoPixelAspectRatioSettings PixelAspectRatio {
			set {
				SetNativeValue (AVVideo.PixelAspectRatioKey, value == null ? null : value.Dictionary);
			}
		}

		public AVVideoCleanApertureSettings VideoCleanAperture {
			set {
				SetNativeValue (AVVideo.CleanApertureKey, value == null ? null : value.Dictionary);
			}			
		} 
#endif
	}

	public class AVVideoPixelAspectRatioSettings : DictionaryContainer
	{
#if !COREBUILD
		public AVVideoPixelAspectRatioSettings ()
			: base (new NSMutableDictionary ())
		{
		}

		public AVVideoPixelAspectRatioSettings (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public int? HorizontalSpacing {
			set {
				SetNumberValue (AVVideo.PixelAspectRatioHorizontalSpacingKey, value);
			}
			get {
				return GetInt32Value (AVVideo.PixelAspectRatioHorizontalSpacingKey);
			}
		}

		public int? VerticalSpacing {
			set {
				SetNumberValue (AVVideo.PixelAspectRatioVerticalSpacingKey, value);
			}
			get {
				return GetInt32Value (AVVideo.PixelAspectRatioVerticalSpacingKey);
			}
		}
#endif
	}

	public class AVVideoCleanApertureSettings : DictionaryContainer
	{
#if !COREBUILD
		public AVVideoCleanApertureSettings ()
			: base (new NSMutableDictionary ())
		{
		}

		public AVVideoCleanApertureSettings (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public int? Width {
			set {
				SetNumberValue (AVVideo.CleanApertureWidthKey, value);
			}
			get {
				return GetInt32Value (AVVideo.CleanApertureWidthKey);
			}
		}

		public int? Height {
			set {
				SetNumberValue (AVVideo.CleanApertureHeightKey, value);
			}
			get {
				return GetInt32Value (AVVideo.CleanApertureHeightKey);
			}
		}

		public int? HorizontalOffset {
			set {
				SetNumberValue (AVVideo.CleanApertureHorizontalOffsetKey, value);
			}
			get {
				return GetInt32Value (AVVideo.CleanApertureHorizontalOffsetKey);
			}
		}

		public int? VerticalOffset {
			set {
				SetNumberValue (AVVideo.CleanApertureVerticalOffsetKey, value);
			}
			get {
				return GetInt32Value (AVVideo.CleanApertureVerticalOffsetKey);
			}
		}
#endif
	}
}
