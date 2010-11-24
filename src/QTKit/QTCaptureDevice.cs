using MonoMac.Foundation;
using System;

namespace MonoMac.QTKit {

	public partial class QTCaptureDevice {

		static NSString FromMediaType (QTMediaType mediaType)
		{
			switch (mediaType){
			case QTMediaType.Video:
				return QTMedia.TypeVideo;
			case QTMediaType.Sound:
				return QTMedia.TypeSound;
			case QTMediaType.Text:
				return QTMedia.TypeText;
			case QTMediaType.Base:
				return QTMedia.TypeBase;
			case QTMediaType.Mpeg:
				return QTMedia.TypeMpeg;
			case QTMediaType.Music:
				return QTMedia.TypeMusic;
			case QTMediaType.TimeCode:
				return QTMedia.TypeTimeCode;
			case QTMediaType.Sprite:
				return QTMedia.TypeSprite;
			case QTMediaType.Flash:
				return QTMedia.TypeFlash;
			case QTMediaType.Movie:
				return QTMedia.TypeMovie;
			case QTMediaType.Tween:
				return QTMedia.TypeTween;
			case QTMediaType.Type3D:
				return QTMedia.Type3D;
			case QTMediaType.Skin:
				return QTMedia.TypeSkin;
			case QTMediaType.Qtvr:
				return QTMedia.TypeQTVR;
			case QTMediaType.Hint:
				return QTMedia.TypeHint;
			case QTMediaType.Stream:
				return QTMedia.TypeStream;
			case QTMediaType.Muxed:
				return QTMedia.TypeMuxed;
			case QTMediaType.QuartzComposer:
				return QTMedia.TypeQuartzComposer;
			default:
				return null;
			}
		}

		public static QTCaptureDevice [] GetInputDevices (QTMediaType mediaType)
		{
			var t = FromMediaType (mediaType);
			if (t == null)
				return null;
			return _GetInputDevices (t);
		}

		public static QTCaptureDevice GetDefaultInputDevice (QTMediaType mediaType)
		{
			var t = FromMediaType (mediaType);
			if (t == null)
				return null;
			return _GetDefaultInputDevice (t);
		}
	}
}