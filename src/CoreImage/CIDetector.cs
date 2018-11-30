//
// Authors:
//   Miguel de Icaza
//
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
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using System.Collections.Generic;
using MonoMac.CoreFoundation;

namespace MonoMac.CoreImage {
	public enum FaceDetectorAccuracy
	{
		High,
		Low
	}

	public partial class CIDetector {
		public static CIDetector CreateFaceDetector (CIContext context, bool highAccuracy)
		{
			// TypeFace is the only detector supported now
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { highAccuracy ? AccuracyHigh : AccuracyLow },
									      new NSObject [] { Accuracy }))
				return FromType (TypeFace, context, options);
		}

		public static CIDetector CreateFaceDetector (CIContext context, bool highAccuracy, float minFeatureSize)
		{
			// MinFeatureSize exists only in iOS6+, before this the field is null (and would throw if used)
			if (MinFeatureSize == null)
				return CreateFaceDetector (context, highAccuracy);

			// TypeFace is the only detector supported now
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { highAccuracy ? AccuracyHigh : AccuracyLow, new NSNumber (minFeatureSize) },
									      new NSObject [] { Accuracy, MinFeatureSize, }))
				return FromType (TypeFace, context, options);
		}

		public static CIDetector CreateFaceDetector (CIContext context, FaceDetectorAccuracy? accuracy = null, float? minFeatureSize = null, bool? trackingEnabled = null)
		{
			List<NSObject> keys = new List<NSObject> (3);
			List<NSObject> values = new List<NSObject> (3);

			if (accuracy != null) {
				keys.Add (Accuracy);
				values.Add (accuracy == FaceDetectorAccuracy.High ? AccuracyHigh : AccuracyLow);
			}

			// MinFeatureSize exists only in iOS6+, before this the field is null (and would throw if used)
			if (MinFeatureSize != null && minFeatureSize != null) {
				keys.Add (MinFeatureSize);
				values.Add (new NSNumber (minFeatureSize.Value));
			}

			// Tracking exists only in iOS6+, before this the field is null (and would throw if used)
			if (Tracking != null && trackingEnabled != null) {
				keys.Add (Tracking);
				values.Add (NSObject.FromObject (true));
			}

			using (var options = NSDictionary.FromObjectsAndKeys (values.ToArray (), keys.ToArray ()))
				return FromType (TypeFace, context, options);
		}
		
		public CIFeature [] FeaturesInImage (CIImage image, CIImageOrientation orientation)
		{
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { new NSNumber ((int) orientation) },
									      new NSObject [] { ImageOrientation })){
				return FeaturesInImage (image, options);
			}
		}
	}
}
