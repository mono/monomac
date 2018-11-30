//
// CIImage.cs: Extensions
//
// Copyright 2011 Xamarin Inc.
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
#if !MONOMAC
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#else
using MonoMac.CoreGraphics;
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


namespace MonoMac.CoreImage {
	public class CIAutoAdjustmentFilterOptions {

		// The default value is true.
		public bool? Enhance;

		// The default value is true
		public bool? RedEye;

		public CIFeature [] Features;

		public CIImageOrientation? ImageOrientation;
		
		internal NSDictionary ToDictionary ()
		{
			int n = 0;
			if (Enhance.HasValue && Enhance.Value == false)
				n++;
			if (RedEye.HasValue && RedEye.Value == false)
				n++;
			if (ImageOrientation.HasValue)
				n++;
			if (Features != null && Features.Length != 0)
				n++;
			if (n == 0)
				return null;
			
			NSMutableDictionary dict = new NSMutableDictionary ();

			if (Enhance.HasValue && Enhance.Value == false){
				dict.LowlevelSetObject (CFBoolean.False.Handle, CIImage.AutoAdjustEnhanceKey.Handle);
			}
			if (RedEye.HasValue && RedEye.Value == false){
				dict.LowlevelSetObject (CFBoolean.False.Handle, CIImage.AutoAdjustRedEyeKey.Handle);
			}
			if (Features != null && Features.Length != 0){
				dict.LowlevelSetObject (NSArray.FromObjects (Features), CIImage.AutoAdjustFeaturesKey.Handle);
			}
			if (ImageOrientation.HasValue){
				dict.LowlevelSetObject (new NSNumber ((int)ImageOrientation.Value), CIImage.ImagePropertyOrientation.Handle);
			}
#if false
			for (i = 0; i < n; i++){
				Console.WriteLine ("{0} {1}-{2}", i, keys [i], values [i]);
			}
#endif
			return dict;
		}
	}
	
	public partial class CIImage {

		static CIFilter [] WrapFilters (NSArray filters)
		{
			if (filters == null)
				return new CIFilter [0];

			nuint count = filters.Count;
			if (count == 0)
				return new CIFilter [0];
			var ret = new CIFilter [count];
			for (nuint i = 0; i < count; i++){
				IntPtr filterHandle = filters.ValueAt (i);
				string filterName = CIFilter.GetFilterName (filterHandle);
									 
				ret [i] = CIFilter.FromName (filterName, filterHandle);
			}
			return ret;
		}

		public static CIImage FromCGImage (CGImage image, CGColorSpace colorSpace)
		{
			if (colorSpace == null)
				throw new ArgumentNullException ("colorSpace");
			
			using (var arr = NSArray.FromIntPtrs (new IntPtr [] { colorSpace.Handle })){
				using (var keys = NSArray.FromIntPtrs (new IntPtr [] { CIImageColorSpaceKey.Handle } )){
					using (var dict = NSDictionary.FromObjectsAndKeysInternal (arr, keys)){
						return FromCGImage (image, dict);
					}
				}
			}
		}
		
		public CIFilter [] GetAutoAdjustmentFilters ()
		{
			return WrapFilters (_GetAutoAdjustmentFilters ());
		}
		
		public CIFilter [] GetAutoAdjustmentFilters (CIAutoAdjustmentFilterOptions options)
		{
			if (options == null)
				return GetAutoAdjustmentFilters ();
			var dict = options.ToDictionary ();
			if (dict == null)
				return GetAutoAdjustmentFilters ();
			return WrapFilters (_GetAutoAdjustmentFilters (dict));
		}

		public static implicit operator CIImage (CGImage image)
		{
			return FromCGImage (image);
		}
		
		internal static nint CIFormatToInt (CIFormat format)
		{
			switch (format) {
#if MONOMAC
			case CIFormat.ARGB8: return FormatARGB8;			
			case CIFormat.RGBAh: return FormatRGBAh;
			case CIFormat.RGBA16: return FormatRGBA16;
			case CIFormat.RGBAf: return FormatRGBAf;
#else
			case CIFormat.ARGB8: return FormatARGB8;
			case CIFormat.RGBAh: return FormatRGBAh;
			case CIFormat.BGRA8: return FormatBGRA8;
			case CIFormat.RGBA8: return FormatRGBA8;
#endif
			default:
				throw new ArgumentOutOfRangeException ("format");
			}
		}
	}
#if MONOMAC
    public enum CIFormat {
		ARGB8,
		RGBAh,
		RGBA16,
		RGBAf
	}
#else
	public enum CIFormat {
		ARGB8,
		RGBAh,
		BGRA8,
		RGBA8,
	}
#endif
}