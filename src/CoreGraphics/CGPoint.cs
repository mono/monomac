//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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
using System.Globalization;
using System.Runtime.InteropServices;

#if !SDCOMPAT

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#endif

namespace MonoMac.CoreGraphics {
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CGPoint {
		
		public static readonly CGPoint Empty;
		
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public static bool operator ==(CGPoint left, CGPoint right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		public static bool operator !=(CGPoint left, CGPoint right)
		{
			return left.X != right.X || left.Y != right.Y;
		}
		
		public static CGPoint operator +(CGPoint pt, CGSize sz)
		{
			return new CGPoint (pt.X + sz.Width, pt.Y + sz.Height);
		}

		public static CGPoint operator -(CGPoint pt, CGSize sz)
		{
			return new CGPoint (pt.X - sz.Width, pt.Y - sz.Height);
		}

		public override string ToString ()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{X={0},Y={1}}}", X, Y);
		}

#if !COREFX
		public CGPoint(System.Drawing.PointF point)
		{
			X = point.X;
			Y = point.Y;
		}

		public static implicit operator CGPoint (System.Drawing.PointF point)
		{
			return new CGPoint (point.X, point.Y);
		}
		
		public static explicit operator System.Drawing.PointF (CGPoint point)
		{
			return new System.Drawing.PointF ((float)point.X, (float)point.Y);
		}
#endif

		public CGPoint(int x, int y)
		{
			X = (nfloat)x;
			Y = (nfloat)y;
		}

		public CGPoint(nfloat x, nfloat y)
		{
			X = x;
			Y = y;
		}

		public nfloat X;
		public nfloat Y;
	}
}
#endif