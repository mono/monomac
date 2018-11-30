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

// For now, only support MAC64 for CGSize in order to make sure
// we didn't mess up the 32 bit build
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
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CGSize {
	
		public static readonly CGSize Empty;
		
		public CGSize(nfloat width, nfloat height)
		{
			Width = width;
			Height = height;
		}
		
		public override int GetHashCode()
		{
			return Width.GetHashCode() ^ Height.GetHashCode();
		}

		public static bool operator ==(CGSize left, CGSize right)
		{
			return left.Width == right.Width && left.Height == right.Height;
		}

		public static bool operator !=(CGSize left, CGSize right)
		{
			return left.Width != right.Width || left.Height != right.Height;
		}

		public static CGSize operator +(CGSize size1, CGSize size2)
		{
			return new CGSize (size1.Width + size2.Width, size1.Height + size2.Height);
		}

		public static CGSize operator -(CGSize size1, CGSize size2)
		{
			return new CGSize (size1.Width - size2.Width, size1.Height - size2.Height);
		}

		public override string ToString ()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{Width={0},Height={1}}}", Width, Height);
		}

#if !COREFX		
		public CGSize(System.Drawing.SizeF size)
		{
			Width = size.Width;
			Height = size.Height;
		}
		
		public static implicit operator CGSize (System.Drawing.SizeF size)
		{
			return new CGSize (size.Width, size.Height);
		}
		
		public static explicit operator System.Drawing.SizeF (CGSize size)
		{
			return new System.Drawing.SizeF ((float)size.Width, (float)size.Height);
		}
#endif

		public nfloat Width;
		public nfloat Height;
	}
}
#endif