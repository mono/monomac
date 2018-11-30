// 
// CoreMedia.cs: Basic definitions for CoreMedia
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {
	
	public enum CMMediaType : uint
	{
		Video         = 1986618469, // 'vide'
		Audio         = 1936684398, // 'soun'
		Muxed         = 1836415096, // 'muxx'
		Text          = 1952807028, // 'text'
		ClosedCaption = 1668047728, // 'clcp'
		Subtitle      = 1935832172, // 'sbtl'
		TimeCode      = 1953325924, // 'tmcd'
//		[Obsolete ("Use Metadata instead")]
		TimedMetadata = 1953326452, // 'tmet'
		Metadata      = TimedMetadata,
	}

	public enum CMClosedCaptionFormatType : uint
	{
		CEA608	= 0x63363038, // 'c608',
		CEA708	= 0x63373038, // 'c708',
		ATSC	= 0x61746363, // 'atcc'
	}

	public enum CMMuxedStreamType : uint
	{
		MPEG1System		= 0x6D703173, // 'mp1s',
		MPEG2Transport	= 0x6D703274, // 'mp2t',
		MPEG2Program	= 0x6D703270, // 'mp2p',
		DV				= 0x64762020, // 'dv  '
	}

	public enum CMSubtitleFormatType : uint
	{
		Text3G  = 0x74783367, // 'tx3g'
		WebVTT  = 0x77767474, // 'wvtt'
	}

	public enum CMMetadataFormatType : uint
	{
		ICY   = 0x69637920, // 'icy '
		ID3   = 0x69643320, // 'id3 '
		Boxed = 0x6d656278, // 'mebx'
	}

	public enum CMTimeCodeFormatType : uint
	{
		TimeCode32	= 0x746D6364, // 'tmcd',
		TimeCode64	= 0x74633634, // 'tc64',
		Counter32	= 0x636E3332, // 'cn32',
		Counter64	= 0x636E3634, // 'cn64'
	}

	public enum CMTimeRoundingMethod 
	{
		RoundHalfAwayFromZero = 1,
		RoundTowardZero = 2,
		RoundAwayFromZero = 3,
		QuickTime = 4,
		RoundTowardPositiveInfinity = 5,
		RoundTowardNegativeInfinity = 6,
		Default = RoundHalfAwayFromZero,
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct CMSampleTimingInfo
	{
		public CMTime Duration;
		public CMTime PresentationTimeStamp;
		public CMTime DecodeTimeStamp;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTimeRange {
		public CMTime Start;
		public CMTime Duration;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTimeMapping {
		public CMTimeRange Source;
		public CMTimeRange Target;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTimeScale
	{
		public static readonly CMTimeScale MaxValue = new CMTimeScale (0x7fffffff);

		public int Value;

		public CMTimeScale (int value)
		{
			if (value < 0 || value > 0x7fffffff)
				throw new ArgumentOutOfRangeException ("value");

			this.Value = value;
		}
	}

	public enum CMVideoCodecType 
	{
		YUV422YpCbCr8    = 0x32767579,
		Animation        = 0x726c6520,
		Cinepak          = 0x63766964,
		JPEG             = 0x6a706567,
		JPEG_OpenDML     = 0x646d6231,
		SorensonVideo    = 0x53565131,
		SorensonVideo3   = 0x53565133,
		H263             = 0x68323633,
		H264             = 0x61766331,
		Mpeg4Video       = 0x6d703476,
		Mpeg2Video       = 0x6d703276,
		Mpeg1Video       = 0x6d703176,
		DvcNtsc          = 0x64766320,
		DvcPal           = 0x64766370,
		DvcProPal        = 0x64767070,
		DvcPro50NTSC     = 0x6476356e,
		DvcPro50PAL      = 0x64763570,
		DvcProHD720p60   = 0x64766870,
		DvcProHD720p50   = 0x64766871,
		DvcProHD1080i60  = 0x64766836,
		DvcProHD1080i50  = 0x64766835,
		DvcProHD1080p30  = 0x64766833,
		DvcProHD1080p25  = 0x64766832,
		AppleProRes4444  = 0x61703468,
		AppleProRes422HQ = 0x61706368,
		AppleProRes422   = 0x6170636e,
		AppleProRes422LT = 0x61706373,
		AppleProRes422Proxy = 0x6170636f,
	}
}