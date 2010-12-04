//
// Authors:
//   Miguel de Icaza
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
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using System.Drawing;
using MonoMac.CoreFoundation;
namespace MonoMac.CoreImage {
	public enum CIWrapMode {
		Black,
		Clamp
	}

	public enum CIFilterMode {
		Nearest, Linear
	}
	
	public class CISamplerOptions {
		public CISamplerOptions () {}

		public CGAffineTransform? AffineMatrix { get; set; }
		public CIWrapMode? WrapMode { get; set; }
		public CIFilterMode? FilterMode { get; set; }
		
		internal NSDictionary ToDictionary ()
		{
			var ret = new NSMutableDictionary ();

			if (AffineMatrix.HasValue){
				var a = AffineMatrix.Value;
				var affine = new NSNumber [6];
				affine [0] = NSNumber.FromFloat (a.xx);
				affine [1] = NSNumber.FromFloat (a.yx);
				affine [2] = NSNumber.FromFloat (a.xy);
				affine [3] = NSNumber.FromFloat (a.yy);
				affine [4] = NSNumber.FromFloat (a.x0);
				affine [5] = NSNumber.FromFloat (a.y0);
				ret.SetObject (NSArray.FromNSObjects (affine), CISampler.AffineMatrix);
			}
			if (WrapMode.HasValue){
				var k = WrapMode.Value == CIWrapMode.Black ? CISampler.WrapBlack : CISampler.FilterNearest;
				ret.SetObject (k, CISampler.WrapMode);
			}
			if (FilterMode.HasValue){
				var k = FilterMode.Value == CIFilterMode.Nearest ? CISampler.FilterNearest : CISampler.FilterLinear;
				ret.SetObject (k, CISampler.FilterMode);
			}
			return ret;
		}
	}
	
	public partial class CISampler {
		public CISampler FromImage (CIImage sourceImage, CISamplerOptions options)
		{
			if (options == null)
				return FromImage (sourceImage);
			return FromImage (sourceImage, options.ToDictionary ());
		}

		public CISampler (CIImage sourceImage, CISamplerOptions options) : this (sourceImage, options == null ? null : options.ToDictionary ())
		{
		}
	}
}