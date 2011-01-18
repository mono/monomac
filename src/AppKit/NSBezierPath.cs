//
// NSBezierPath.cs: Helper methods to expose strongly typed functions
//
// Author:
//   Regan Sarwas <rsarwas@gmail.com>
//
// Copyright 2010, Regan Sarwas.
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using System.Runtime.InteropServices;

namespace MonoMac.AppKit {
	public partial class NSBezierPath {

		public void GetLineDash (out float[] pattern, out float phase)
		{
			//Call the internal method with null to get the length of the pattern array
			int length;
			_GetLineDash ((IntPtr)null, out length, out phase);
			
			//Allocate space for the C-array
			float[] managedArray = new float[length];
			int size = Marshal.SizeOf(managedArray[0]) * length;
			IntPtr pNativeArray = Marshal.AllocHGlobal(size);
			
			//Call the method again to get the array
			_GetLineDash (pNativeArray, out length, out phase);

			Marshal.Copy(pNativeArray, managedArray, 0, length);
			Marshal.FreeHGlobal(pNativeArray);

			pattern = managedArray;
		}

		public void SetLineDash (float[] pattern, float phase)
		{
			if (pattern == null)
				throw new ArgumentNullException ("pattern");

			int size = Marshal.SizeOf(pattern[0]) * pattern.Length;
			IntPtr pNativeArray = Marshal.AllocHGlobal(size);
			Marshal.Copy(pattern, 0, pNativeArray, pattern.Length);

			_SetLineDash (pNativeArray, pattern.Length, phase);
			
			Marshal.FreeHGlobal(pNativeArray);
		}

		public NSBezierPathElement ElementAt (int index, out PointF[] points)
		{
			//return array will be 1 or 3 points, depending on type.
			int size = Marshal.SizeOf(typeof(PointF)) * 3;
			IntPtr pNativeArray = Marshal.AllocHGlobal(size);

			NSBezierPathElement bpe = _ElementAt (index, pNativeArray);
			
			int length = 1;
			if (bpe == NSBezierPathElement.CurveTo)
				length = 3;
			points = new PointF[length];

			IntPtr currentPtr = pNativeArray;
			for (int i = 0; i < length; i++)
			{
            	points[i] = (PointF)Marshal.PtrToStructure(currentPtr, typeof(PointF));
				currentPtr = (IntPtr)((long)currentPtr + Marshal.SizeOf(points[i]));
			}

			Marshal.FreeHGlobal(pNativeArray);

			return bpe;
		}

		public void SetAssociatedPointsAtIndex (PointF[] points, int index)
		{
		    if (points == null)
		        throw new ArgumentNullException ("points");
			if (points.Length < 1)
				throw new ArgumentException ("points array is empty");

			int size = Marshal.SizeOf(points[0]) * points.Length;
			IntPtr pNativeArray = Marshal.AllocHGlobal(size);
			IntPtr currentPtr = pNativeArray;
			for (int i = 0; i < points.Length; i++)
			{
				Marshal.StructureToPtr(points[i], currentPtr, false);
				currentPtr = (IntPtr)((long)currentPtr + Marshal.SizeOf(points[i]));
			}

			_SetAssociatedPointsAtIndex (pNativeArray, index);
			
			Marshal.FreeHGlobal(pNativeArray);
		}

		public void AppendPathWithPoints (PointF[] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length < 1)
				throw new ArgumentException ("points array is empty");

			int size = Marshal.SizeOf(points[0]) * points.Length;
			IntPtr pNativeArray = Marshal.AllocHGlobal(size);
			IntPtr currentPtr = pNativeArray;
			for (int i = 0; i < points.Length; i++)
			{
				Marshal.StructureToPtr(points[i], currentPtr, false);
				currentPtr = (IntPtr)((long)currentPtr + Marshal.SizeOf(points[i]));
			}

			_AppendPathWithPoints (pNativeArray, points.Length);
			
			Marshal.FreeHGlobal(pNativeArray);
		}

		public void AppendPathWithGlyphs (uint[] glyphs, NSFont font)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (glyphs.Length < 1)
				throw new ArgumentException ("glyphs array is empty");

			int size = Marshal.SizeOf(glyphs[0]);
			IntPtr pNativeArray = Marshal.AllocHGlobal(size * glyphs.Length);
			IntPtr currentPtr = pNativeArray;
			for (int i = 0; i < glyphs.Length; i++)
			{
				Marshal.WriteIntPtr(currentPtr, (IntPtr)glyphs[i]);
				currentPtr = (IntPtr)((long)currentPtr + size);
			}

			_AppendPathWithGlyphs (pNativeArray, glyphs.Length, font);
			
			Marshal.FreeHGlobal(pNativeArray);
		}

	}
}
