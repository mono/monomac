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
using System.Drawing;
using MonoMac.ObjCRuntime;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac;

namespace MonoMac.AppKit {
	public static class NSGraphics {
		public static readonly float White = 1;
		public static readonly float Black = 0;
		public static readonly float LightGray = (float) 2/3.0f;
		public static readonly float DarkGray = (float) 1/3.0f;
		
		[DllImport (Constants.AppKitLibrary)]
		extern static NSWindowDepth NSBestDepth (IntPtr colorspaceHandle, int bitsPerSample, int bitsPerPixel, bool planar, ref bool exactMatch);
		
		public static NSWindowDepth BestDepth (NSString colorspace, int bitsPerSample, int bitsPerPixel, bool planar, ref bool exactMatch)
		{
			if (colorspace == null)
				throw new ArgumentNullException ("colorspace");
			
			return NSBestDepth (colorspace.Handle, bitsPerSample, bitsPerPixel, planar, ref exactMatch);
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static int NSPlanarFromDepth (NSWindowDepth depth);
		
		public static bool PlanarFromDepth (NSWindowDepth depth)
		{
			return NSPlanarFromDepth (depth) != 0;
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static IntPtr NSColorSpaceFromDepth (NSWindowDepth depth);

		public static NSString ColorSpaceFromDepth (NSWindowDepth depth)
		{
			return new NSString (NSColorSpaceFromDepth (depth));
		}

		[DllImport (Constants.AppKitLibrary, EntryPoint = "NSBitsPerSampleFromDepth")]
		public extern static int BitsPerSampleFromDepth (NSWindowDepth depth);

		[DllImport (Constants.AppKitLibrary, EntryPoint = "NSBitsPerPixelFromDepth")]
		public extern static int BitsPerPixelFromDepth (NSWindowDepth depth);

		[DllImport (Constants.AppKitLibrary)]
		extern static int NSNumberOfColorComponents (IntPtr str);

		public static int NumberOfColorComponents (NSString colorspaceName)
		{
			if (colorspaceName == null)
				throw new ArgumentNullException ("colorspaceName");
			return NSNumberOfColorComponents (colorspaceName.Handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static IntPtr NSAvailableWindowDepths ();

		public static NSWindowDepth [] AvailableWindowDepths {
			get {
				IntPtr depPtr = NSAvailableWindowDepths ();
				int count;

				for (count = 0; Marshal.ReadInt32 (depPtr, count) != 0; count++)
					;

				var ret = new NSWindowDepth [count];
				for (int i = 0; i < count; count++)
					ret [i] = (NSWindowDepth) Marshal.ReadInt32 (depPtr, i);

				return ret;

			}
		}

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSRectFill")]
		public extern static void RectFill (RectangleF rect);
		
		[DllImport (Constants.AppKitLibrary, EntryPoint="NSRectFillList")]
		unsafe extern static void RectFillList (RectangleF *rects, int count);

		public static void RectFill (RectangleF [] rects)
		{
			if (rects == null)
				throw new ArgumentNullException ("rects");
			unsafe {
				fixed (RectangleF *ptr = &rects [0])
					RectFillList (ptr, rects.Length);
			}
		}

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSRectClip")]
		public extern static void RectClip (RectangleF rect);
		
		[DllImport (Constants.AppKitLibrary, EntryPoint="NSFrameRect")]
		public extern static void FrameRect (RectangleF rect);		

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSFrameRectWithWidth")]
		public extern static void FrameRect (RectangleF rect, float frameWidth);		

		// Bad naming, added the overload above
		[DllImport (Constants.AppKitLibrary, EntryPoint="NSFrameRectWithWidth")]
		public extern static void FrameRectWithWidth (RectangleF rect, float frameWidth);		

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSFrameRectWithWidthUsingOperation")]
		public extern static void FrameRect (RectangleF rect, float frameWidth, NSCompositingOperation operation);		
		
		[DllImport (Constants.AppKitLibrary, EntryPoint="NSShowAnimationEffect")]
		public extern static void ShowAnimationEffect (NSAnimationEffect animationEffect, PointF centerLocation, SizeF size, NSObject animationDelegate, Selector didEndSelector, IntPtr contextInfo);

		public static void ShowAnimationEffect (NSAnimationEffect animationEffect, PointF centerLocation, SizeF size, NSAction endedCallback)
		{
			var d = new NSAsyncActionDispatcher (endedCallback);
			ShowAnimationEffect (animationEffect, centerLocation, size, d, NSActionDispatcher.Selector, IntPtr.Zero);
			GC.KeepAlive (d);
		}

#if false
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
		[DllImport (Constants.AppKitLibrary)]
		public extern static ;
#endif

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSDrawWhiteBezel")]
		public extern static void DrawWhiteBezel (RectangleF aRect, RectangleF clipRect);

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSDrawLightBezel")]
		public extern static void DrawLightBezel (RectangleF aRect, RectangleF clipRect);

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSDrawGrayBezel")]
		public extern static void DrawGrayBezel (RectangleF aRect, RectangleF clipRect);

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSDrawDarkBezel")]
		public extern static void DrawDarkBezel (RectangleF aRect, RectangleF clipRect);

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSDrawGroove")]
		public extern static void DrawGroove (RectangleF aRect, RectangleF clipRect);

		[DllImport (Constants.AppKitLibrary, EntryPoint="NSDrawTiledRects")]
		unsafe extern static RectangleF DrawTiledRects (RectangleF aRect, RectangleF clipRect, NSRectEdge* sides, float* grays, int count);

		public static RectangleF DrawTiledRects (RectangleF aRect, RectangleF clipRect, NSRectEdge[] sides, float[] grays)
		{
			if (sides == null)
				throw new ArgumentNullException ("sides");
			if (grays == null)
				throw new ArgumentNullException ("grays");
			if (sides.Length != grays.Length)
				throw new ArgumentOutOfRangeException ("grays", "Both array parameters must have the same length");
			unsafe {
				fixed (NSRectEdge *ptr = &sides [0])
				fixed (float *ptr2 = &grays [0])
					return DrawTiledRects (aRect, clipRect, ptr, ptr2, sides.Length);
			}
		}
	}
}
