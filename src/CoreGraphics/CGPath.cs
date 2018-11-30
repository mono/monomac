// 
// CGPath.cs: Implements the managed CGPath
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
	public enum CGPathElementType {
		MoveToPoint,
		AddLineToPoint,
		AddQuadCurveToPoint,
		AddCurveToPoint,
		CloseSubpath
	}

	public struct CGPathElement {
		public CGPathElementType Type;

		public CGPathElement (int t)
		{
			Type = (CGPathElementType) t;
			Point1 = Point2 = Point3 = new CGPoint (0,0);
		}
		
		// Set for MoveToPoint, AddLineToPoint, AddQuadCurveToPoint, AddCurveToPoint
		public CGPoint Point1;

		// Set for AddQuadCurveToPoint, AddCurveToPoint
		public CGPoint Point2;

		// Set for AddCurveToPoint
		public CGPoint Point3;
	}
	
	public class CGPath : INativeObject, IDisposable {
		internal IntPtr handle;

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateMutable();
		public CGPath ()
		{
			handle = CGPathCreateMutable ();
		}

#if !MONOMAC
		[Since (5,0)]
		public CGPath (CGPath reference, CGAffineTransform transform)
		{
			if (reference == null)
				throw new ArgumentNullException ("reference");
			handle = CGPathCreateMutableCopyByTransformingPath (reference.Handle, ref transform);
		}
#endif
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateMutableCopy(IntPtr path);
		public CGPath (CGPath basePath)
		{
			if (basePath == null)
				throw new ArgumentNullException ("basePath");
			handle = CGPathCreateMutableCopy (basePath.handle);
		}

		//
		// For use by marshallrs
		//
		public CGPath (IntPtr handle)
		{
			CGPathRetain (handle);
			this.handle = handle;
		}

		// Indicates that we own it `owns'
		[Preserve (Conditional=true)]
		internal CGPath (IntPtr handle, bool owns)
		{
			if (!owns)
				CGPathRetain (handle);
			
			this.handle = handle;
		}
		
		~CGPath ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGPathRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPathEqualToPath(IntPtr path1, IntPtr path2);

		public static bool operator == (CGPath path1, CGPath path2)
		{
			return Object.Equals (path1, path2);
		}

		public static bool operator != (CGPath path1, CGPath path2)
		{
			return !Object.Equals (path1, path2);
		}

		public override int GetHashCode ()
		{
			return handle.GetHashCode ();
		}

		public override bool Equals (object o)
		{
			CGPath other = o as CGPath;
			if (other == null)
				return false;

			return CGPathEqualToPath (this.handle, other.handle);
		}
       
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathMoveToPoint(IntPtr path, ref CGAffineTransform m, nfloat x, nfloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathMoveToPoint(IntPtr path, IntPtr zero, nfloat x, nfloat y);
		public void MoveToPoint (nfloat x, nfloat y)
		{
			CGPathMoveToPoint (handle, IntPtr.Zero, x, y);

		}

		public void MoveToPoint (CGPoint point)
		{
			CGPathMoveToPoint (handle, IntPtr.Zero, point.X, point.Y);
		}
		
		public void MoveToPoint (CGAffineTransform transform, nfloat x, nfloat y)
		{
			CGPathMoveToPoint (handle, ref transform, x, y);
		}

		public void MoveToPoint (CGAffineTransform transform, CGPoint point)
		{
			CGPathMoveToPoint (handle, ref transform, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLineToPoint(IntPtr path, ref CGAffineTransform m, nfloat x, nfloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLineToPoint(IntPtr path, IntPtr m, nfloat x, nfloat y);

		[Advice ("Use AddLineToPoint instead")] // Bad name
		public void CGPathAddLineToPoint (nfloat x, nfloat y)
		{
			AddLineToPoint (x, y);
		}

		public void AddLineToPoint (nfloat x, nfloat y)
		{
			CGPathAddLineToPoint (handle, IntPtr.Zero, x, y);
		}

		public void AddLineToPoint (CGPoint point)
		{
			CGPathAddLineToPoint (handle, IntPtr.Zero, point.X, point.Y);
		}
		
		[Advice ("Use AddLineToPoint instead")] // Bad name
		public void CGPathAddLineToPoint (CGAffineTransform transform, nfloat x, nfloat y)
		{
			AddLineToPoint (transform, x, y);
		}

		public void AddLineToPoint (CGAffineTransform transform, nfloat x, nfloat y)
		{
			CGPathAddLineToPoint (handle, ref transform, x, y);
		}

		public void AddLineToPoint (CGAffineTransform transform, CGPoint point)
		{
			CGPathAddLineToPoint (handle, ref transform, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddQuadCurveToPoint(IntPtr path, ref CGAffineTransform m, nfloat cpx, nfloat cpy, nfloat x, nfloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddQuadCurveToPoint(IntPtr path, IntPtr zero, nfloat cpx, nfloat cpy, nfloat x, nfloat y);
		public void AddQuadCurveToPoint (nfloat cpx, nfloat cpy, nfloat x, nfloat y)
		{
			CGPathAddQuadCurveToPoint (handle, IntPtr.Zero, cpx, cpy, x, y);
		}

		public void AddQuadCurveToPoint (CGAffineTransform transform, nfloat cpx, nfloat cpy, nfloat x, nfloat y)
		{
			CGPathAddQuadCurveToPoint (handle, ref transform, cpx, cpy, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddCurveToPoint(IntPtr path, ref CGAffineTransform m, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y);
		public void AddCurveToPoint (CGAffineTransform transform, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
		{
			CGPathAddCurveToPoint (handle, ref transform, cp1x, cp1y, cp2x, cp2y, x, y);
		}

		public void AddCurveToPoint (CGAffineTransform transform, CGPoint cp1, CGPoint cp2, CGPoint point)
		{
			CGPathAddCurveToPoint (handle, ref transform, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddCurveToPoint(IntPtr path, IntPtr zero, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y);
		public void AddCurveToPoint (nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
		{
			CGPathAddCurveToPoint (handle, IntPtr.Zero, cp1x, cp1y, cp2x, cp2y, x, y);
		}
			
		public void AddCurveToPoint (CGPoint cp1, CGPoint cp2, CGPoint point)
		{
			CGPathAddCurveToPoint (handle, IntPtr.Zero, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathCloseSubpath(IntPtr path);
		public void CloseSubpath ()
		{
			CGPathCloseSubpath (handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRect(IntPtr path, ref CGAffineTransform m, CGRect rect);
		public void AddRect (CGAffineTransform transform, CGRect rect)
		{
			CGPathAddRect (handle, ref transform, rect);
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRect(IntPtr path, IntPtr zero, CGRect rect);
		public void AddRect (CGRect rect)
		{
			CGPathAddRect (handle, IntPtr.Zero, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRects(IntPtr path, ref CGAffineTransform m, CGRect [] rects, IntPtr size_t_count);
		public void AddRects (CGAffineTransform m, CGRect [] rects)
		{
			CGPathAddRects (handle, ref m, rects, new IntPtr(rects.Length));
		}
		public void AddRects (CGAffineTransform m, CGRect [] rects, nint count)
		{
			if (count > rects.Length)
				throw new ArgumentException ("counts");
			CGPathAddRects (handle, ref m, rects, new IntPtr(count));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRects(IntPtr path, IntPtr Zero, CGRect [] rects, IntPtr size_t_count);
		public void AddRects (CGRect [] rects)
		{
			CGPathAddRects (handle, IntPtr.Zero, rects, new IntPtr(rects.Length));
		}
		public void AddRects (CGRect [] rects, nint count)
		{
			if (count > rects.Length)
				throw new ArgumentException ("count");
			CGPathAddRects (handle, IntPtr.Zero, rects, new IntPtr(count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLines(IntPtr path, ref CGAffineTransform m, CGPoint [] points, IntPtr size_t_count);
		public void AddLines (CGAffineTransform m, CGPoint [] points)
		{
			CGPathAddLines (handle, ref m, points, new IntPtr(points.Length));
		}
		public void AddLines (CGAffineTransform m, CGPoint [] points, nint count)
		{
			if (count > points.Length)
				throw new ArgumentException ("count");
			CGPathAddLines (handle, ref m, points, new IntPtr(count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLines(IntPtr path, IntPtr zero, CGPoint [] points, IntPtr size_t_count);
		public void AddLines (CGPoint [] points)
		{
			AddLines (points, points.Length);
		}
		public void AddLines (CGPoint [] points, nint count)
		{
			if (count > points.Length)
				throw new ArgumentException ("count");
			CGPathAddLines (handle, IntPtr.Zero, points, new IntPtr(count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddEllipseInRect(IntPtr path, ref CGAffineTransform m, CGRect rect);
		public void AddEllipseInRect (CGAffineTransform m, CGRect rect)
		{
			CGPathAddEllipseInRect (handle, ref m, rect);
		}
		
		[Obsolete ("Use AddEllipseInRect instead")]
		public void AddElipseInRect (CGAffineTransform m, CGRect rect)
		{
			AddEllipseInRect (m, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddEllipseInRect(IntPtr path, IntPtr zero, CGRect rect);
		public void AddElipseInRect (CGRect rect)
		{
			CGPathAddEllipseInRect (handle, IntPtr.Zero, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArc(IntPtr path, ref CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise);
		public void AddArc (CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
		{
			CGPathAddArc (handle, ref m, x, y, radius, startAngle, endAngle, clockwise);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArc(IntPtr path, IntPtr zero, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise);
		public void AddArc (nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
		{
			CGPathAddArc (handle, IntPtr.Zero, x, y, radius, startAngle, endAngle, clockwise);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArcToPoint(IntPtr path, ref CGAffineTransform m, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius);
		public void AddArcToPoint (CGAffineTransform m, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
		{
			CGPathAddArcToPoint (handle, ref m, x1, y1, x2, y2, radius);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArcToPoint(IntPtr path, IntPtr zero, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius);
		public void AddArcToPoint (nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
		{
			CGPathAddArcToPoint (handle, IntPtr.Zero, x1, y1, x2, y2, radius);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRelativeArc(IntPtr path, ref CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta);
		public void AddRelativeArc (CGAffineTransform m, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
		{
			CGPathAddRelativeArc (handle, ref m, x, y, radius, startAngle, delta);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRelativeArc(IntPtr path, IntPtr zero, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta);
		public void AddRelativeArc (nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat delta)
		{
			CGPathAddRelativeArc (handle, IntPtr.Zero, x, y, radius, startAngle, delta);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddPath(IntPtr path1, ref CGAffineTransform m, IntPtr path2);
		public void AddPath (CGAffineTransform t, CGPath path2)
		{
			if (path2 == null)
				throw new ArgumentNullException ("path2");
			CGPathAddPath (handle, ref t, path2.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddPath(IntPtr path1, IntPtr zero, IntPtr path2);
		public void AddPath (CGPath path2)
		{
			if (path2 == null)
				throw new ArgumentNullException ("path2");
			CGPathAddPath (handle, IntPtr.Zero, path2.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPathIsEmpty(IntPtr path);
		public bool IsEmpty {
			get {
				return CGPathIsEmpty (handle) != 0;
			}
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPathIsRect(IntPtr path, out CGRect rect);
		public bool IsRect (out CGRect rect)
		{
			return CGPathIsRect (handle, out rect) != 0;
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGPathGetCurrentPoint(IntPtr path);
		public CGPoint CurrentPoint {
			get {
				return CGPathGetCurrentPoint (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPathGetBoundingBox(IntPtr path);
		public CGRect BoundingBox {
			get {
				return CGPathGetBoundingBox (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGPathGetPathBoundingBox(IntPtr path);
		public CGRect PathBoundingBox {
			get {
				return CGPathGetPathBoundingBox (handle);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPathContainsPoint(IntPtr path, ref CGAffineTransform m, CGPoint point, bool eoFill);
		public bool ContainsPoint (CGAffineTransform m, CGPoint point, bool eoFill)
		{
			return CGPathContainsPoint (handle, ref m, point, eoFill);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPathContainsPoint(IntPtr path, IntPtr zero, CGPoint point, bool eoFill);
		public bool ContainsPoint (CGPoint point, bool eoFill)
		{
			return CGPathContainsPoint (handle, IntPtr.Zero, point, eoFill);
		}

		//typedef void (*CGPathApplierFunction)(void *info, const CGPathElement *element);
		public delegate void ApplierFunction (CGPathElement element);
		delegate void CGPathApplierFunction (IntPtr info, IntPtr CGPathElementPtr);
		
#if !MONOMAC
		[MonoPInvokeCallback (typeof (CGPathApplierFunction))]
#endif
		unsafe static void ApplierCallback (IntPtr info, IntPtr element_ptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (info);
			CGPathElement element = new CGPathElement (Marshal.ReadInt32 (element_ptr, 0));
			ApplierFunction func = (ApplierFunction) gch.Target;

			IntPtr ptr = Marshal.ReadIntPtr (element_ptr, sizeof(IntPtr));
			int ptsize = Marshal.SizeOf (typeof (CGPoint));

			switch (element.Type){
			case CGPathElementType.CloseSubpath:
				break;
			case CGPathElementType.MoveToPoint:
			case CGPathElementType.AddLineToPoint:
				element.Point1 = (CGPoint) Marshal.PtrToStructure (ptr, typeof (CGPoint));
				break;

			case CGPathElementType.AddQuadCurveToPoint:
				element.Point1 = (CGPoint) Marshal.PtrToStructure (ptr, typeof (CGPoint));
				element.Point2 = (CGPoint) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + ptsize)), typeof (CGPoint));
				break;

			case CGPathElementType.AddCurveToPoint:
				element.Point1 = (CGPoint) Marshal.PtrToStructure (ptr, typeof (CGPoint));
				element.Point2 = (CGPoint) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + ptsize)), typeof (CGPoint));
				element.Point3 = (CGPoint) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + (2*ptsize))), typeof (CGPoint));
				break;
			}
			func (element);
		}


		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathApply(IntPtr path, IntPtr info, CGPathApplierFunction function);
		public void Apply (ApplierFunction func)
		{
			GCHandle gch = GCHandle.Alloc (func);
			CGPathApply (handle, GCHandle.ToIntPtr (gch), ApplierCallback);
			gch.Free ();
		}

		static CGPath MakeMutable (IntPtr source)
		{
			var mutable = CGPathCreateMutableCopy (source);
			CGPathRelease (source);

			return new CGPath (mutable, true);
		}

#if !MONOMAC
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByDashingPath (IntPtr handle, ref CGAffineTransform transform, nfloat [] phase, IntPtr count);

		[Since(5,0)]
		public CGPath CopyByDashingPath (CGAffineTransform transform, nfloat [] phase)
		{
			return MakeMutable (CGPathCreateCopyByDashingPath (handle, ref transform, phase, phase == null ? IntPtr.Zero : new IntPtr(phase.Length)));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByDashingPath (IntPtr handle, IntPtr transform, nfloat [] phase, IntPtr count);

		[Since(5,0)]
		public CGPath CopyByDashingPath (nfloat [] phase)
		{
			return MakeMutable (CGPathCreateCopyByDashingPath (handle, IntPtr.Zero, phase, phase == null ? IntPtr.Zero : new IntPtr(phase.Length)));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByStrokingPath (IntPtr handle, ref CGAffineTransform transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit);

		[Since(5,0)]
		public CGPath CopyByStrokingPath (CGAffineTransform transform, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
		{
			return MakeMutable (CGPathCreateCopyByStrokingPath (handle, ref transform, lineWidth, lineCap, lineJoin, miterLimit));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByStrokingPath (IntPtr handle, IntPtr zero, nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit);

		[Since(5,0)]
		public CGPath CopyByStrokingPath (nfloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, nfloat miterLimit)
		{
			return MakeMutable (CGPathCreateCopyByStrokingPath (handle, IntPtr.Zero, lineWidth, lineCap, lineJoin, miterLimit));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateMutableCopyByTransformingPath (IntPtr handle, ref CGAffineTransform transform);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateWithEllipseInRect (CGRect boundingRect, ref CGAffineTransform transform);

		[Since (5,0)]
		static public CGPath EllipseFromRect (CGRect boundingRect, CGAffineTransform transform)
		{
			return MakeMutable (CGPathCreateWithEllipseInRect (boundingRect, ref transform));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateWithRect (CGRect boundingRect, ref CGAffineTransform transform);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateWithRect (CGRect boundingRect, IntPtr transform);
		
		[Since (5,0)]
		static public CGPath FromRect (CGRect rectangle, CGAffineTransform transform)
		{
			return MakeMutable (CGPathCreateWithRect (rectangle, ref transform));
		}

		[Since (5,0)]
		static public CGPath FromRect (CGRect rectangle)
		{
			return MakeMutable (CGPathCreateWithRect (rectangle, IntPtr.Zero));
		}
#endif
	}
}
