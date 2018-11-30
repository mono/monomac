// 
// CGAffineTransform.cs: Implements the managed side
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

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

namespace MonoMac.CoreGraphics {

	[StructLayout(LayoutKind.Sequential)]
	public struct CGAffineTransform {
		public nfloat xx;   // a
		public nfloat yx;   // b 
		public nfloat xy;   // c
		public nfloat yy;   // d
		public nfloat x0;   // tx
		public nfloat y0;   // ty

		//
		// Constructors
		//
		public CGAffineTransform (nfloat xx, nfloat yx, nfloat xy, nfloat yy, nfloat x0, nfloat y0)
		{
			this.xx = xx;
			this.yx = yx;
			this.xy = xy;
			this.yy = yy;
			this.x0 = x0;
			this.y0 = y0;
		}
		
		// Identity
		public static CGAffineTransform MakeIdentity ()
		{
			return new CGAffineTransform (1, 0, 0, 1, 0, 0);
		}
		
		public static CGAffineTransform MakeRotation (nfloat angle)
		{
			return new CGAffineTransform (
				(nfloat)Math.Cos (angle), (nfloat)Math.Sin (angle),
				(nfloat)(-Math.Sin (angle)), (nfloat)Math.Cos (angle),
				0, 0);
		}

		public static CGAffineTransform MakeScale (nfloat sx, nfloat sy)
		{
			return new CGAffineTransform (sx, 0, 0, sy, 0, 0);
		}

		public static CGAffineTransform MakeTranslation (nfloat tx, nfloat ty)
		{
			return new CGAffineTransform (1, 0, 0, 1, tx, ty);
		}

		//
		// Operations
		//
		public static CGAffineTransform Multiply (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform (a.xx * b.xx + a.yx * b.xy,
						      a.xx * b.yx + a.yx * b.yy,
						      a.xy * b.xx + a.yy * b.xy,
						      a.xy * b.yx + a.yy * b.yy,
						      a.x0 * b.xx + a.y0 * b.xy + b.x0,
						      a.x0 * b.yx + a.y0 * b.yy + b.y0);
		}

		public void Multiply (CGAffineTransform b)
		{
			var a = this;
			xx = a.xx * b.xx + a.yx * b.xy;
			yx = a.xx * b.yx + a.yx * b.yy;
			xy = a.xy * b.xx + a.yy * b.xy;
			yy = a.xy * b.yx + a.yy * b.yy;
			x0 = a.x0 * b.xx + a.y0 * b.xy + b.x0;
			y0 = a.x0 * b.yx + a.y0 * b.yy + b.y0;
		}
		
		public void Scale (nfloat sx, nfloat sy)
		{
			Multiply (MakeScale (sx, sy));
		}

		public void Translate (nfloat tx, nfloat ty)
		{
			Multiply (MakeTranslation (tx, ty));
		}

		public void Rotate (nfloat angle)
		{
			Multiply (MakeRotation (angle));
		}
		
		public bool IsIdentity {
			get {
				return xx == 1 && yx == 0 && xy == 0 && yy == 1 && x0 == 0 && y0 == 0;
			}
		}
		
                public override String ToString ()
                {
                        String s = String.Format ("xx:{0:##0.0#} yx:{1:##0.0#} xy:{2:##0.0#} yy:{3:##0.0#} x0:{4:##0.0#} y0:{5:##0.0#}", xx, yx, xy, yy, x0, y0);
                        return s;
                }
                
                public static bool operator == (CGAffineTransform lhs, CGAffineTransform rhs)
                {
                        return (lhs.xx == rhs.xx && lhs.xy == rhs.xy &&
                                lhs.yx == rhs.yx && lhs.yy == rhs.yy &&
                                lhs.x0 == rhs.x0 && lhs.y0 == rhs.y0 );
                }
                
                public static bool operator != (CGAffineTransform lhs, CGAffineTransform rhs)
                {
                        return !(lhs==rhs);
                }

		public static CGAffineTransform operator * (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform (a.xx * b.xx + a.yx * b.xy,
						      a.xx * b.yx + a.yx * b.yy,
						      a.xy * b.xx + a.yy * b.xy,
						      a.xy * b.yx + a.yy * b.yy,
						      a.x0 * b.xx + a.y0 * b.xy + b.x0,
						      a.x0 * b.yx + a.y0 * b.yy + b.y0);
		}
                
                public override bool Equals(object o)
                {
                        if (! (o is CGAffineTransform))
                                return false;
                        else
                                return (this == (CGAffineTransform) o);
                }
                
                public override int GetHashCode()
                {
                        return  (int)this.xx ^ (int)this.xy ^
                                (int)this.yx ^ (int)this.yy ^
                                (int)this.x0 ^ (int)this.y0;
                }
                
		public CGPoint TransformPoint (CGPoint point)
		{
			return new CGPoint (xx * point.X + xy * point.Y + x0,
					    yx * point.X + yy * point.Y + y0);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGRect CGRectApplyAffineTransform (CGRect rect, CGAffineTransform t);

		public CGRect TransformRect (CGRect rect)
		{
			return CGRectApplyAffineTransform (rect, this);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformInvert (CGAffineTransform t);

		public CGAffineTransform Invert ()
		{
			return CGAffineTransformInvert (this);
		}
	}
}
