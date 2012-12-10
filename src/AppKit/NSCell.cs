//
// Author:
//       Martin Baulig <martin.baulig@xamarin.com>
//
// Copyright 2010, Novell, Inc.
// Copyright (c) 2012 Xamarin Inc. (http://www.xamarin.com)
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
using System.Drawing;
using System.Runtime.InteropServices;

namespace MonoMac.AppKit {
	public partial class NSCell {

		[DllImport (Constants.AppKitLibrary)]
		extern static void NSDrawThreePartImage (RectangleF rect, IntPtr startCap, IntPtr centerFill, IntPtr endCap, bool vertial, int op, float alphaFraction, bool flipped);
		
		public void DrawThreePartImage (RectangleF frame, NSImage startCap, NSImage centerFill, NSImage endCap, bool vertical, NSCompositingOperation op, float alphaFraction, bool flipped)
		{
			NSDrawThreePartImage (
				frame, startCap != null ? startCap.Handle : IntPtr.Zero,
				centerFill != null ? centerFill.Handle : IntPtr.Zero,
				endCap != null ? endCap.Handle : IntPtr.Zero,
				vertical, (int)op, alphaFraction, flipped);
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static void NSDrawNinePartImage (RectangleF frame, IntPtr topLeftCorner, IntPtr topEdgeFill, IntPtr topRightCorner, IntPtr leftEdgeFill, IntPtr centerFill, IntPtr rightEdgeFill, IntPtr bottomLeftCorner, IntPtr bottomEdgeFill, IntPtr bottomRightCorner, int op, float alphaFraction, bool flipped);

		public void DrawNinePartImage (RectangleF frame, NSImage topLeftCorner, NSImage topEdgeFill, NSImage topRightCorner, NSImage leftEdgeFill, NSImage centerFill, NSImage rightEdgeFill, NSImage bottomLeftCorner, NSImage bottomEdgeFill, NSImage bottomRightCorner, NSCompositingOperation op, float alphaFraction, bool flipped)
		{
			NSDrawNinePartImage (
				frame, topLeftCorner != null ? topLeftCorner.Handle : IntPtr.Zero,
				topEdgeFill != null ? topEdgeFill.Handle : IntPtr.Zero,
				topRightCorner != null ? topRightCorner.Handle : IntPtr.Zero,
				leftEdgeFill != null ? leftEdgeFill.Handle : IntPtr.Zero,
				centerFill != null ? centerFill.Handle : IntPtr.Zero,
				rightEdgeFill != null ? rightEdgeFill.Handle : IntPtr.Zero,
				bottomLeftCorner != null ? bottomLeftCorner.Handle : IntPtr.Zero,
				bottomEdgeFill != null ? bottomEdgeFill.Handle : IntPtr.Zero,
				bottomRightCorner != null ? bottomRightCorner.Handle : IntPtr.Zero,
				(int)op, alphaFraction, flipped);
 		}
	}
}
