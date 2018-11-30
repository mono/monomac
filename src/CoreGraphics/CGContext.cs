// 
// CGContext.cs: Implements the managed CGContext
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

	public enum CGLineJoin {
		Miter,
		Round,
		Bevel
	}
	
	public enum CGLineCap {
		Butt,
		Round,
		Square
	}
	
	public enum CGPathDrawingMode {
		Fill,
		EOFill,
		Stroke,
		FillStroke,
		EOFillStroke
	}
	
	public enum CGTextDrawingMode {
		Fill,
		Stroke,
		FillStroke,
		Invisible,
		FillClip,
		StrokeClip,
		FillStrokeClip,
		Clip
	}
	
	public enum CGTextEncoding {
		FontSpecific,
		MacRoman
	}

	public enum CGInterpolationQuality {
		Default,
		None,	
		Low,	
		High,
		Medium		       /* Yes, in this order, since Medium was added in 4 */
	}
	
	public enum CGBlendMode {
		Normal,
		Multiply,
		Screen,
		Overlay,
		Darken,
		Lighten,
		ColorDodge,
		ColorBurn,
		SoftLight,
		HardLight,
		Difference,
		Exclusion,
		Hue,
		Saturation,
		Color,
		Luminosity,
		
		Clear,
		Copy,	
		SourceIn,	
		SourceOut,	
		SourceAtop,		
		DestinationOver,	
		DestinationIn,	
		DestinationOut,	
		DestinationAtop,	
		XOR,		
		PlusDarker,		
		PlusLighter		
	}

	public class CGContext : INativeObject, IDisposable {
		internal IntPtr handle;

		public CGContext (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CGContextRetain (handle);
			this.handle = handle;
		}

		internal CGContext ()
		{
		}
		
		[Preserve (Conditional=true)]
		internal CGContext (IntPtr handle, bool owns)
		{
			if (!owns)
				CGContextRetain (handle);

			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid handle");
			
			this.handle = handle;
		}

		~CGContext ()
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
		extern static void CGContextRelease (IntPtr handle);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGContextRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSaveGState (IntPtr context);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRestoreGState (IntPtr context);
		
		public void SaveState ()
		{
			CGContextSaveGState (handle);
		}

		public void RestoreState ()
		{
			CGContextRestoreGState (handle);
		}

		//
		// Transformation matrix
		//

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextScaleCTM (IntPtr ctx, nfloat sx, nfloat sy);
		public void ScaleCTM (nfloat sx, nfloat sy)
		{
			CGContextScaleCTM (handle, sx, sy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextTranslateCTM (IntPtr ctx, nfloat tx, nfloat ty);
		public void TranslateCTM (nfloat tx, nfloat ty)
		{
			CGContextTranslateCTM (handle, tx, ty);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRotateCTM (IntPtr ctx, nfloat angle);
		public void RotateCTM (nfloat angle)
		{
			CGContextRotateCTM (handle, angle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextConcatCTM (IntPtr ctx, CGAffineTransform transform);
		public void ConcatCTM (CGAffineTransform transform)
		{
			CGContextConcatCTM (handle, transform);
		}

		// Settings
		[DllImport (Constants.CoreGraphicsLibrary)]		
		extern static void CGContextSetLineWidth(IntPtr c, nfloat width);
		public void SetLineWidth (nfloat w)
		{
			CGContextSetLineWidth (handle, w);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineCap(IntPtr c, CGLineCap cap);
		public void SetLineCap (CGLineCap cap)
		{
			CGContextSetLineCap (handle, cap);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineJoin(IntPtr c, CGLineJoin join);
		public void SetLineJoin (CGLineJoin join)
		{
			CGContextSetLineJoin (handle, join);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetMiterLimit(IntPtr c, nfloat limit);
		public void SetMiterLimit (nfloat limit)
		{
			CGContextSetMiterLimit (handle, limit);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineDash(IntPtr c, nfloat phase, nfloat [] lengths, nint count);
		public void SetLineDash (nfloat phase, nfloat [] lengths)
		{
			SetLineDash (phase, lengths, lengths.Length);
		}

		public void SetLineDash (nfloat phase, nfloat [] lengths, int n)
		{
			if (lengths == null)
				n = 0;
			else if (n < 0 || n > lengths.Length)
				throw new ArgumentException ("n");
			CGContextSetLineDash (handle, phase, lengths, n);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetFlatness(IntPtr c, nfloat flatness);
		public void SetFlatness (nfloat flatness)
		{
			CGContextSetFlatness (handle, flatness);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetAlpha(IntPtr c, nfloat alpha);
		public void SetAlpha (nfloat alpha)
		{
			CGContextSetAlpha (handle, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetBlendMode(IntPtr context, CGBlendMode mode);
		public void SetBlendMode (CGBlendMode mode)
		{
			CGContextSetBlendMode (handle, mode);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetCTM (IntPtr c);
		public CGAffineTransform GetCTM ()
		{
			return CGContextGetCTM (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextBeginPath(IntPtr c);
		public void BeginPath ()
		{
			CGContextBeginPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextMoveToPoint(IntPtr c, nfloat x, nfloat y);
		public void MoveTo (nfloat x, nfloat y)
		{
			CGContextMoveToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLineToPoint(IntPtr c, nfloat x, nfloat y);
		public void AddLineToPoint (nfloat x, nfloat y)
		{
			CGContextAddLineToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddCurveToPoint(IntPtr c, nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y);
		public void AddCurveToPoint (nfloat cp1x, nfloat cp1y, nfloat cp2x, nfloat cp2y, nfloat x, nfloat y)
		{
			CGContextAddCurveToPoint (handle, cp1x, cp1y, cp2x, cp2y, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddQuadCurveToPoint(IntPtr c, nfloat cpx, nfloat cpy, nfloat x, nfloat y);
		public void AddQuadCurveToPoint (nfloat cpx, nfloat cpy, nfloat x, nfloat y)
		{
			CGContextAddQuadCurveToPoint (handle, cpx, cpy, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClosePath(IntPtr c);
		public void ClosePath ()
		{
			CGContextClosePath (handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRect(IntPtr c, CGRect rect);
		public void AddRect (CGRect rect)
		{
			CGContextAddRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRects(IntPtr c, CGRect [] rects, IntPtr size_t_count) ;
		public void AddRects (CGRect [] rects)
		{
			CGContextAddRects (handle, rects, new IntPtr(rects.Length));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLines(IntPtr c, CGPoint [] points, IntPtr size_t_count) ;
		public void AddLines (CGPoint [] points)
		{
			CGContextAddLines (handle, points, new IntPtr(points.Length));
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddEllipseInRect(IntPtr context, CGRect rect);
		public void AddEllipseInRect (CGRect rect)
		{
			CGContextAddEllipseInRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArc(IntPtr c, nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, int clockwise);
		public void AddArc (nfloat x, nfloat y, nfloat radius, nfloat startAngle, nfloat endAngle, bool clockwise)
		{
			CGContextAddArc (handle, x, y, radius, startAngle, endAngle, clockwise ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArcToPoint(IntPtr c, nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius);
		public void AddArcToPoint (nfloat x1, nfloat y1, nfloat x2, nfloat y2, nfloat radius)
		{
			CGContextAddArcToPoint (handle, x1, y1, x2, y2, radius);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddPath(IntPtr context, IntPtr path_ref);
		public void AddPath (CGPath path)
		{
			CGContextAddPath (handle, path.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextReplacePathWithStrokedPath(IntPtr c);
		public void ReplacePathWithStrokedPath ()
		{
			CGContextReplacePathWithStrokedPath (handle);
		}

		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextIsPathEmpty(IntPtr c);
		public bool IsPathEmpty ()
		{
			return CGContextIsPathEmpty (handle) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextGetPathCurrentPoint(IntPtr c);
		public CGPoint GetPathCurrentPoint ()
		{
			return CGContextGetPathCurrentPoint (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextGetPathBoundingBox(IntPtr c);
		public CGRect GetPathBoundingBox ()
		{
			return CGContextGetPathBoundingBox (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextPathContainsPoint(IntPtr context, CGPoint point, CGPathDrawingMode mode);
		public bool PathContainsPoint (CGPoint point, CGPathDrawingMode mode)
		{
			return CGContextPathContainsPoint (handle, point, mode) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPath(IntPtr c, CGPathDrawingMode mode);
		public void DrawPath (CGPathDrawingMode mode)
		{
			CGContextDrawPath (handle, mode);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillPath(IntPtr c);
		public void FillPath ()
		{
			CGContextFillPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOFillPath(IntPtr c);
		public void EOFillPath ()
		{
			CGContextEOFillPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokePath(IntPtr c);
		public void StrokePath ()
		{
			CGContextStrokePath (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRect(IntPtr c, CGRect rect);
		public void FillRect (CGRect rect)
		{
			CGContextFillRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRects(IntPtr c, CGRect [] rects, IntPtr size_t_count);
		public void ContextFillRects (CGRect [] rects)
		{
			CGContextFillRects (handle, rects, new IntPtr(rects.Length));
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRect(IntPtr c, CGRect rect);
		public void StrokeRect (CGRect rect)
		{
			CGContextStrokeRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRectWithWidth(IntPtr c, CGRect rect, nfloat width);
		public void StrokeRectWithWidth (CGRect rect, nfloat width)
		{
			CGContextStrokeRectWithWidth (handle, rect, width);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClearRect(IntPtr c, CGRect rect);
		public void ClearRect (CGRect rect)
		{
			CGContextClearRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillEllipseInRect(IntPtr context, CGRect rect);
		public void FillEllipseInRect (CGRect rect)
		{
			CGContextFillEllipseInRect (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeEllipseInRect(IntPtr context, CGRect rect);
		public void StrokeEllipseInRect (CGRect rect)
		{
			CGContextStrokeEllipseInRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeLineSegments(IntPtr c, CGPoint [] points, IntPtr size_t_count);
		public void StrokeLineSegments (CGPoint [] points)
		{
			CGContextStrokeLineSegments (handle, points, new IntPtr(points.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClip(IntPtr c);
		public void Clip ()
		{
			CGContextClip (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOClip(IntPtr c);
		public void EOClip ()
		{
			CGContextEOClip (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToMask(IntPtr c, CGRect rect, IntPtr mask);
		public void ClipToMask (CGRect rect, CGImage mask)
		{
			CGContextClipToMask (handle, rect, mask.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextGetClipBoundingBox(IntPtr c);
		public CGRect GetClipBoundingBox ()
		{
			return CGContextGetClipBoundingBox (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRect(IntPtr c, CGRect rect);
		public void ClipToRect (CGRect rect)
		{
			CGContextClipToRect (handle, rect);
		}
		       
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRects(IntPtr c, CGRect [] rects, IntPtr size_t_count);
		public void ClipToRects (CGRect [] rects)
		{
			CGContextClipToRects (handle, rects, new IntPtr(rects.Length));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorWithColor(IntPtr c, IntPtr color);
		public void SetFillColor (CGColor color)
		{
			CGContextSetFillColorWithColor (handle, color.handle);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetFillColorWithColor (CGColor color)
		{
			CGContextSetFillColorWithColor (handle, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorWithColor(IntPtr c, IntPtr color);
		public void SetStrokeColor (CGColor color)
		{
			CGContextSetStrokeColorWithColor (handle, color.handle);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetStrokeColorWithColor (CGColor color)
		{
			CGContextSetStrokeColorWithColor (handle, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorSpace(IntPtr context, IntPtr space);
		public void SetFillColorSpace (CGColorSpace space)
		{
			CGContextSetFillColorSpace (handle, space.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorSpace(IntPtr context, IntPtr space);
		public void SetStrokeColorSpace (CGColorSpace space)
		{
			CGContextSetStrokeColorSpace (handle, space.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColor(IntPtr context, nfloat [] components);
		public void SetFillColor (nfloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetFillColor (handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColor(IntPtr context, nfloat [] components);
		public void SetStrokeColor (nfloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetStrokeColor (handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillPattern(IntPtr context, IntPtr pattern, nfloat [] components);
		public void SetFillPattern (CGPattern pattern, nfloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetFillPattern (handle, pattern.handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokePattern(IntPtr context, IntPtr pattern, nfloat [] components);
		public void SetStrokePattern (CGPattern pattern, nfloat [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetStrokePattern (handle, pattern.handle, components);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetPatternPhase(IntPtr context, CGSize phase);
		public void SetPatternPhase (CGSize phase)
		{
			CGContextSetPatternPhase (handle, phase);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayFillColor(IntPtr context, nfloat gray, nfloat alpha);
		public void SetFillColor (nfloat gray, nfloat alpha)
		{
			CGContextSetGrayFillColor (handle, gray, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetGrayFillColor (nfloat gray, nfloat alpha)
		{
			CGContextSetGrayFillColor (handle, gray, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayStrokeColor(IntPtr context, nfloat gray, nfloat alpha);
		public void SetStrokeColor (nfloat gray, nfloat alpha)
		{
			CGContextSetGrayStrokeColor (handle, gray, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetGrayStrokeColor (nfloat gray, nfloat alpha)
		{
			CGContextSetGrayStrokeColor (handle, gray, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBFillColor(IntPtr context, nfloat red, nfloat green, nfloat blue, nfloat alpha);
		public void SetFillColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			CGContextSetRGBFillColor (handle, red, green, blue, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetRGBFillColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			CGContextSetRGBFillColor (handle, red, green, blue, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBStrokeColor(IntPtr context, nfloat red, nfloat green, nfloat blue, nfloat alpha);
		public void SetStrokeColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			CGContextSetRGBStrokeColor (handle, red, green, blue, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetRGBStrokeColor (nfloat red, nfloat green, nfloat blue, nfloat alpha)
		{
			CGContextSetRGBStrokeColor (handle, red, green, blue, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKFillColor(IntPtr context, nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha);
		public void SetFillColor (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			CGContextSetCMYKFillColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetCMYKFillColor (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			CGContextSetCMYKFillColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKStrokeColor(IntPtr context, nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha);
		public void SetStrokeColor (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			CGContextSetCMYKStrokeColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetCMYKStrokeColor (nfloat cyan, nfloat magenta, nfloat yellow, nfloat black, nfloat alpha)
		{
			CGContextSetCMYKStrokeColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRenderingIntent(IntPtr context, CGColorRenderingIntent intent);
		public void SetRenderingIntent (CGColorRenderingIntent intent)
		{
			CGContextSetRenderingIntent (handle, intent);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawImage(IntPtr c, CGRect rect, IntPtr image);
		public void DrawImage (CGRect rect, CGImage image)
		{
			CGContextDrawImage (handle, rect, image.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawTiledImage(IntPtr c, CGRect rect, IntPtr image);
		public void DrawTiledImage (CGRect rect, CGImage image)
		{
			CGContextDrawTiledImage (handle, rect, image.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGInterpolationQuality CGContextGetInterpolationQuality(IntPtr context);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetInterpolationQuality(IntPtr context, CGInterpolationQuality quality);
		
		public CGInterpolationQuality  InterpolationQuality {
			get {
				return CGContextGetInterpolationQuality (handle);
			}

			set {
				CGContextSetInterpolationQuality (handle, value);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadowWithColor(IntPtr context, CGSize offset, nfloat blur, IntPtr color);
		public void SetShadowWithColor (CGSize offset, nfloat blur, CGColor color)
		{
			CGContextSetShadowWithColor (handle, offset, blur, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadow(IntPtr context, CGSize offset, nfloat blur);
		public void SetShadow (CGSize offset, nfloat blur)
		{
			CGContextSetShadow (handle, offset, blur);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLinearGradient(IntPtr context, IntPtr gradient, CGPoint startPoint, CGPoint endPoint, CGGradientDrawingOptions options);
		public void DrawLinearGradient (CGGradient gradient, CGPoint startPoint, CGPoint endPoint, CGGradientDrawingOptions options)
		{
			CGContextDrawLinearGradient (handle, gradient.handle, startPoint, endPoint, options);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawRadialGradient (IntPtr context, IntPtr gradient, CGPoint startCenter, nfloat startRadius,
								CGPoint endCenter, nfloat endRadius, CGGradientDrawingOptions options);
		public void DrawRadialGradient (CGGradient gradient, CGPoint startCenter, nfloat startRadius, CGPoint endCenter, nfloat endRadius, CGGradientDrawingOptions options)
		{
			CGContextDrawRadialGradient (handle, gradient.handle, startCenter, startRadius, endCenter, endRadius, options);
		}
		
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawShading(IntPtr context, IntPtr shading);
		public void DrawShading (CGShading shading)
		{
			CGContextDrawShading (handle, shading.handle);
		}
#endif		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCharacterSpacing(IntPtr context, nfloat spacing);
		public void SetCharacterSpacing (nfloat spacing)
		{
			CGContextSetCharacterSpacing (handle, spacing);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextPosition(IntPtr c, nfloat x, nfloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextGetTextPosition(IntPtr context);

		public CGPoint TextPosition {
			get {
				return CGContextGetTextPosition (handle);
			}

			set {
				CGContextSetTextPosition (handle, value.X, value.Y);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextMatrix(IntPtr c, CGAffineTransform t);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetTextMatrix(IntPtr c);
		public CGAffineTransform TextMatrix {
			get {
				return CGContextGetTextMatrix (handle);
			}
			set {
				CGContextSetTextMatrix (handle, value);
			}
			
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextDrawingMode(IntPtr c, CGTextDrawingMode mode);
		public void SetTextDrawingMode (CGTextDrawingMode mode)
		{
			CGContextSetTextDrawingMode (handle, mode);
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFont(IntPtr c, IntPtr font);
		public void SetFont (CGFont font)
		{
			CGContextSetFont (handle, font.handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFontSize(IntPtr c, nfloat size);
		public void SetFontSize (nfloat size)
		{
			CGContextSetFontSize (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSelectFont(IntPtr c, string name, nfloat size, CGTextEncoding textEncoding);
		public void SelectFont (string name, nfloat size, CGTextEncoding textEncoding)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			CGContextSelectFont (handle, name, size, textEncoding);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPositions(IntPtr context, ushort [] glyphs, CGPoint [] positions, IntPtr size_t_count);
		public void ShowGlyphsAtPositions (ushort [] glyphs, CGPoint [] positions, int size_t_count)
		{
			if (positions == null)
				throw new ArgumentNullException ("positions");
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphsAtPositions (handle, glyphs, positions, new IntPtr(size_t_count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, string s, IntPtr size_t_length);
		public void ShowText (string str, int count)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			if (count > str.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, str, new IntPtr(count));
		}

		public void ShowText (string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowText (handle, str, new IntPtr(str.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, byte[] bytes, IntPtr size_t_length);
		public void ShowText (byte[] bytes, int count)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			if (count > bytes.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, bytes, new IntPtr(count));
		}
		
		public void ShowText (byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowText (handle, bytes, new IntPtr(bytes.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, nfloat x, nfloat y, string str, IntPtr size_t_length);
		public void ShowTextAtPoint (nfloat x, nfloat y, string str, int length)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, new IntPtr(length));
		}

		public void ShowTextAtPoint (nfloat x, nfloat y, string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, new IntPtr(str.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, nfloat x, nfloat y, byte[] bytes, IntPtr size_t_length);
		public void ShowTextAtPoint (nfloat x, nfloat y, byte[] bytes, int length)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, new IntPtr(length));
		}
		
		public void ShowTextAtPoint (nfloat x, nfloat y, byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, new IntPtr(bytes.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphs(IntPtr c, ushort [] glyphs, IntPtr size_t_count);
		public void ShowGlyphs (ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphs (handle, glyphs, new IntPtr(glyphs.Length));
		}

		public void ShowGlyphs (ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphs (handle, glyphs, new IntPtr(count));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPoint(IntPtr context, nfloat x, nfloat y, ushort [] glyphs, IntPtr size_t_count);
		public void ShowGlyphsAtPoint (nfloat x, nfloat y, ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, new IntPtr(count));
		}

		public void ShowGlyphsAtPoint (nfloat x, nfloat y, ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, new IntPtr(glyphs.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsWithAdvances(IntPtr c, ushort [] glyphs, CGSize [] advances, IntPtr size_t_count);
		public void ShowGlyphsWithAdvances (ushort [] glyphs, CGSize [] advances, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (advances == null)
				throw new ArgumentNullException ("advances");
			if (count > glyphs.Length || count > advances.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsWithAdvances (handle, glyphs, advances, new IntPtr(count));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPDFPage(IntPtr c, IntPtr page);
		public void DrawPDFPage (CGPDFPage page)
		{
			CGContextDrawPDFPage (handle, page.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, ref CGRect mediaBox);
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, IntPtr zero);
		public void BeginPage (CGRect? rect)
		{
			if (rect.HasValue){
				CGRect v = rect.Value;
				CGContextBeginPage (handle, ref v);
			} else {
				CGContextBeginPage (handle, IntPtr.Zero);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEndPage(IntPtr c);
		public void EndPage ()
		{
			CGContextEndPage (handle);
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static IntPtr CGContextRetain(IntPtr c);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFlush(IntPtr c);
		public void Flush ()
		{
			CGContextFlush (handle);
		}
		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSynchronize(IntPtr c);
		public void Synchronize ()
		{
			CGContextSynchronize (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldAntialias(IntPtr c, int shouldAntialias);
		public void SetShouldAntialias (bool shouldAntialias)
		{
			CGContextSetShouldAntialias (handle, shouldAntialias ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsAntialiasing(IntPtr context, int allowsAntialiasing);
		public void SetAllowsAntialiasing (bool allowsAntialiasing)
		{
			CGContextSetAllowsAntialiasing (handle, allowsAntialiasing ? 1 : 0);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldSmoothFonts(IntPtr c, int shouldSmoothFonts);
		public void SetShouldSmoothFonts (bool shouldSmoothFonts)
		{
			CGContextSetShouldSmoothFonts (handle, shouldSmoothFonts ? 1 : 0);
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextBeginTransparencyLayer(IntPtr context, CFDictionaryRef auxiliaryInfo);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextBeginTransparencyLayerWithRect(IntPtr context, CGRect rect, CFDictionaryRef auxiliaryInfo)
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextEndTransparencyLayer(IntPtr context);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetUserSpaceToDeviceSpaceTransform(IntPtr context);
		public CGAffineTransform GetUserSpaceToDeviceSpaceTransform()
		{
			return CGContextGetUserSpaceToDeviceSpaceTransform (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextConvertPointToDeviceSpace(IntPtr context, CGPoint point);
		public CGPoint PointToDeviceSpace (CGPoint point)
		{
			return CGContextConvertPointToDeviceSpace (handle, point);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGPoint CGContextConvertPointToUserSpace(IntPtr context, CGPoint point);
		public CGPoint ConvertPointToUserSpace (CGPoint point)
		{
			return CGContextConvertPointToUserSpace (handle, point);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGContextConvertSizeToDeviceSpace(IntPtr context, CGSize size);
		public CGSize ConvertSizeToDeviceSpace (CGSize size)
		{
			return CGContextConvertSizeToDeviceSpace (handle, size);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGContextConvertSizeToUserSpace(IntPtr context, CGSize size);
		public CGSize ConvertSizeToUserSpace (CGSize size)
		{
			return CGContextConvertSizeToUserSpace (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextConvertRectToDeviceSpace(IntPtr context, CGRect rect);
		public CGRect ConvertRectToDeviceSpace (CGRect rect)
		{
			return CGContextConvertRectToDeviceSpace (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGRect CGContextConvertRectToUserSpace(IntPtr context, CGRect rect);
		public CGRect ConvertRectToUserSpace (CGRect rect)
		{
			return CGContextConvertRectToUserSpace (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerInRect (IntPtr context, CGRect rect, IntPtr layer);
		public void DrawLayer (CGLayer layer, CGRect rect)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			CGContextDrawLayerInRect (handle, rect, layer.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerAtPoint (IntPtr context, CGPoint rect, IntPtr layer);

		public void DrawLayer (CGLayer layer, CGPoint point)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			CGContextDrawLayerAtPoint (handle, point, layer.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextCopyPath (IntPtr context);

		[Since (4,0)]
		public CGPath CopyPath ()
		{
			var r = CGContextCopyPath (handle);

			return new CGPath (r, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSmoothing (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsFontSmoothing (bool allows)
		{
			CGContextSetAllowsFontSmoothing (handle, allows);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSubpixelPositioning (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsSubpixelPositioning (bool allows)
		{
			CGContextSetAllowsFontSubpixelPositioning (handle, allows);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSubpixelQuantization (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsFontSubpixelQuantization (bool allows)
		{
			CGContextSetAllowsFontSubpixelQuantization (handle, allows);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetShouldSubpixelPositionFonts (IntPtr context, bool should);
		[Since (4,0)]
		public void SetShouldSubpixelPositionFonts (bool shouldSubpixelPositionFonts)
		{
			CGContextSetShouldSubpixelPositionFonts (handle, shouldSubpixelPositionFonts);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetShouldSubpixelQuantizeFonts (IntPtr context, bool should);
		[Since (4,0)]
		public void ShouldSubpixelQuantizeFonts (bool shouldSubpixelQuantizeFonts)
		{
			CGContextSetShouldSubpixelQuantizeFonts (handle, shouldSubpixelQuantizeFonts);
		}

#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextBeginTransparencyLayer (IntPtr context, IntPtr dictionary);
		public void BeginTransparencyLayer ()
		{
			CGContextBeginTransparencyLayer (handle, IntPtr.Zero);
		}
		
		public void BeginTransparencyLayer (NSDictionary auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayer (handle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextBeginTransparencyLayerWithRect (IntPtr context, CGRect rect, IntPtr dictionary);
		public void BeginTransparencyLayer (CGRect rectangle, NSDictionary auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
		}

		public void BeginTransparencyLayer (CGRect rectangle)
		{
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, IntPtr.Zero);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextEndTransparencyLayer (IntPtr context);
		public void EndTransparencyLayer ()
		{
			CGContextEndTransparencyLayer (handle);
		}
#endif
	}
}
