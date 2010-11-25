//
// Copyright 2010, Novell, Inc.
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
using MonoMac.AppKit;

namespace MonoMac.QTKit {

	public enum QTCodecQuality {
		Lossless = 0x00000400,
		Max = 0x000003FF,
		Min = 0x00000000,
		Low = 0x00000100,
		Normal = 0x00000200,
		High = 0x00000300
	}
	
	public class QTImageAttributes {
		public QTImageAttributes ()
		{
		}
		
		public string CodecType { get; set; }
		public QTCodecQuality? Quality { get; set; }
		public int? TimeScale { get; set; }

		public NSDictionary ToDictionary ()
		{
			var dict = new NSMutableDictionary ();
			if (CodecType != null)
				dict.SetObject (new NSString (CodecType), QTMovie.ImageCodecType);
			if (Quality.HasValue)
				dict.SetObject (NSNumber.FromInt32 ((int) Quality.Value), QTMovie.ImageCodecQuality);
			if (TimeScale.HasValue)
				dict.SetObject (NSNumber.FromInt32 (TimeScale.Value), QTTrack.TimeScaleAttribute);
			return dict;
		}
	}
	
	public partial class QTMovie {
		public void AddImage (NSImage image, QTTime forDuration, QTImageAttributes attributes)
		{
			if (attributes == null)
				throw new ArgumentNullException ("attributes");
			AddImageForDuration (image, forDuration, attributes.ToDictionary ());
		}
	}
}