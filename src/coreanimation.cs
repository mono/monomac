//
// coreanimation.cs: API definition for CoreAnimation binding
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
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
#if MONOMAC
using MonoMac.AppKit;
using MonoMac.CoreVideo;
using MonoMac.CoreImage;
using MonoMac.OpenGL;
#else
using MonoMac.UIKit;
#endif
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;

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

namespace MonoMac.CoreAnimation {

#if false
	public interface CAMediaTiming {
		[Export ("beginTime")]
		double CFTimeInterval { get; set; }
	
		[Export ("duration")]
		double Duration { get; set; }
	
		[Export ("speed")]
		float Speed { get; set; }
	
		[Export ("timeOffset")]
		double TimeOffset { get; set; }
	
		[Export ("repeatCount")]
		float RepeatCount { get; set; }
	
		[Export ("repeatDuration")]
		double RepeatDuration { get; set; }
	
		[Export ("autoreverses")]
		bool AutoReverses { get;set; }
	
		[Export ("fillMode")]
		string FillMode { get; set; }
	}		
#endif

#if MONOMAC
	[BaseType (typeof (NSObject))]
	public interface CAConstraintLayoutManager {
		[Static]
		[Export ("layoutManager")]
		CAConstraintLayoutManager LayoutManager { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface CAConstraint {
		[Export ("attribute")]
		CAConstraintAttribute Attribute { get;  }

		[Export ("sourceName")]
		string SourceName { get;  }

		[Export ("sourceAttribute")]
		CAConstraintAttribute SourceAttribute { get;  }

		[Export ("scale")]
		float Scale { get;  }

		[Static]
		[Export ("constraintWithAttribute:relativeTo:attribute:scale:offset:")]
		CAConstraint Create (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttr, float scale, float offset);

		[Static]
		[Export ("constraintWithAttribute:relativeTo:attribute:offset:")]
		CAConstraint Create (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttr, float offset);

		[Static]
		[Export ("constraintWithAttribute:relativeTo:attribute:")]
		CAConstraint Create (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttribute);

		[Export ("initWithAttribute:relativeTo:attribute:scale:offset:")]
		IntPtr Constructor (CAConstraintAttribute attribute, string relativeToSource, CAConstraintAttribute srcAttr, float scale, float offset);
	}
	
#else
	[BaseType (typeof (NSObject))]
	public interface CADisplayLink {
		[Export ("displayLinkWithTarget:selector:")][Static]
		CADisplayLink Create (NSObject target, Selector sel);
	
		[Export ("addToRunLoop:forMode:")]
		void AddToRunLoop (NSRunLoop runloop, NSString mode);
	
		[Export ("removeFromRunLoop:forMode:")]
		void RemoveFromRunLoop (NSRunLoop runloop, NSString mode);
	
		[Export ("invalidate")]
		void Invalidate ();
	
		[Export ("timestamp")]
		double Timestamp { get; }
	
		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; set; }
	
		[Export ("frameInterval")]
		int FrameInterval { get; set;  }

		[Export ("duration")]
		double Duration { get; }
	}
#endif

	[BaseType (typeof (NSObject))]
	public interface CALayer {
		[Export ("layer")][Static]
		CALayer Create ();

		[Export ("presentationLayer")]
		CALayer PresentationLayer { get; }

		[Export ("modelLayer")]
		CALayer ModelLayer { get; }

		[Static]
		[Export ("defaultValueForKey:")]
		NSObject DefaultValue (string key);

		[Static]
		[Export ("needsDisplayForKey:")]
		bool NeedsDisplayForKey (string key);

		[Export ("bounds")]
		CGRect Bounds  { get; set; }

		[Export ("zPosition")]
		nfloat ZPosition { get; set; }
		
		[Export ("anchorPoint")]
		CGPoint AnchorPoint { get; set; }

		[Export ("anchorPointZ")]
		nfloat AnchorPointZ { get; set; }
		
		[Export ("position")]
		CGPoint Position { get; set; }

		[Export ("transform")]
		CATransform3D Transform { get; set; }

		[Export ("affineTransform")]
		CGAffineTransform AffineTransform { get; set; }

		[Export ("frame")]
		CGRect Frame { get; set; }

		[Export ("hidden")] // Setter needs setHidden instead
		bool Hidden { [Bind ("isHidden")] get; set; }  

		[Export ("doubleSided")]  // Setter needs setDoubleSided
		bool DoubleSided { [Bind ("isDoubleSided")] get; set; }

		[Export ("geometryFlipped")]
		bool GeometryFlipped { [Bind ("isGeometryFlipped")] get; set; }

		[Export ("contentsAreFlipped")]
		bool ContentsAreFlipped { get; }

		[Export ("superlayer")]
		CALayer SuperLayer { get; }

		[Export ("removeFromSuperlayer")]
		void RemoveFromSuperLayer ();

		[Export ("sublayers", ArgumentSemantic.Copy)]
		CALayer [] Sublayers { get; set; }
		
		[Export ("addSublayer:")][PostGet ("Sublayers")]
		void AddSublayer (CALayer layer);

		[Export ("insertSublayer:atIndex:")][PostGet ("Sublayers")]
		void InsertSublayer (CALayer layer, int index);

		[Export ("insertSublayer:below:")][PostGet ("Sublayers")]
		void InsertSublayerBelow (CALayer layer, CALayer sibling);
		
		[Export ("insertSublayer:above:")][PostGet ("Sublayers")]
		void InsertSublayerAbove (CALayer layer, CALayer sibling);

		[Export ("replaceSublayer:with:")][PostGet ("Sublayers")]
		void ReplaceSublayer (CALayer layer, CALayer with);

		[Export ("sublayerTransform")]
		CATransform3D SublayerTransform { get; set; }

		[Export ("mask", ArgumentSemantic.Retain)][NullAllowed]
		CALayer Mask { get; set; }

		[Export ("masksToBounds")]
		bool MasksToBounds { get; set; }

		[Export ("convertPoint:fromLayer:")]
		CGPoint ConvertPointFromLayer (CGPoint point, [NullAllowed] CALayer layer);
		
		[Export ("convertPoint:toLayer:")]
		CGPoint ConvertPointToLayer (CGPoint point, [NullAllowed] CALayer layer);
		
		[Export ("convertRect:fromLayer:")]
		CGRect ConvertRectFromLayer (CGRect rect, [NullAllowed] CALayer layer);
		
		[Export ("convertRect:toLayer:")]
		CGRect ConvertRectToLayer (CGRect rect, [NullAllowed] CALayer layer);

		[Export ("convertTime:fromLayer:")]
		double ConvertTimeFromLayer (double timeInterval, [NullAllowed] CALayer layer);
		
		[Export ("convertTime:toLayer:")]
		double ConvertTimeToLayer (double timeInterval, [NullAllowed] CALayer layer);

		[Export ("hitTest:")]
		CALayer HitTest (CGPoint p);

		[Export ("containsPoint:")]
		bool Contains (CGPoint p);

		[Export ("contents", ArgumentSemantic.Retain), NullAllowed]
		CGImage Contents { get; set; }

#if MONOMAC
		[Export ("layoutManager")]
		NSObject LayoutManager { get; set; }
#endif

		[Export ("contentsScale")]
		nfloat ContentsScale { get; set; }

		[Export ("contentsRect")]
		CGRect ContentsRect { get; set; }

		[Export ("contentsGravity", ArgumentSemantic.Copy)]
		string ContentsGravity { get; set; }

		[Export ("contentsCenter")]
		CGRect ContentsCenter { get; set; }

		[Export ("minificationFilter", ArgumentSemantic.Copy)]
		string MinificationFilter { get; set; }
		
		[Export ("magnificationFilter", ArgumentSemantic.Copy)]
		string MagnificationFilter { get; set; }

		[Export ("opaque")]
		bool Opaque { [Bind ("isOpaque")] get; set; }

		[Export ("display")]
		void Display ();

		[Export ("needsDisplay")]
		bool NeedsDisplay { get; }

		[Export ("setNeedsDisplay")]
		void SetNeedsDisplay ();

		[Export ("setNeedsDisplayInRect:")]
		void SetNeedsDisplayInRect (CGRect r);

		[Export ("displayIfNeeded")]
		void DisplayIfNeeded ();

		[Export ("needsDisplayOnBoundsChange")]
		bool NeedsDisplayOnBoundsChange { get; set; }

		[Export ("drawInContext:")]
		void DrawInContext (CGContext ctx);

		[Export ("renderInContext:")]
		void RenderInContext (CGContext ctx);

		[Export ("backgroundColor")]
		CGColor BackgroundColor { get; set; }

		[Export ("cornerRadius")]
		nfloat CornerRadius { get; set; }

		[Export ("borderWidth")]
		nfloat BorderWidth { get; set; }

		[Export ("borderColor")]
		CGColor BorderColor { get; set; }

		[Export ("opacity")]
		float Opacity { get; set; } // 32-bit

		[Export ("edgeAntialiasingMask")]
		CAEdgeAntialiasingMask EdgeAntialiasingMask { get; set; }

		// Layout methods

		[Export ("preferredFrameSize")]
		CGSize PreferredFrameSize ();

		[Export ("setNeedsLayout")]
		void SetNeedsLayout ();

		[Export ("needsLayout")]
		bool NeedsLayout ();

		[Export ("layoutIfNeeded")]
		void LayoutIfNeeded ();

		[Export ("layoutSublayers")]
		void LayoutSublayers ();

		[Static]
		[Export ("defaultActionForKey:")]
		NSObject DefaultActionForKey (string eventKey);

		[Export ("actionForKey:")]
		NSObject ActionForKey (string eventKey);

		[Export ("actions", ArgumentSemantic.Copy)]
		NSDictionary Actions { get; set; }

		[Export ("addAnimation:forKey:")]
		void AddAnimation (CAAnimation animation, [NullAllowed] string key);
		
		[Export ("removeAllAnimations")]
		void RemoveAllAnimations ();

		[Export ("removeAnimationForKey:")]
		void RemoveAnimation (string key);

		[Export ("animationKeys")]
		string [] AnimationKeys { get; }

		[Export ("animationForKey:")]
		CAAnimation AnimationForKey (string key);

		[Export ("name", ArgumentSemantic.Copy)]
		string Name { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		CALayerDelegate Delegate { get; set; }
		
		//
		// From CAMediaTiming
		//
		[Export ("beginTime")]
		double BeginTime { get; set; }
	
		[Export ("duration")]
		double Duration { get; set; }
	
		[Export ("speed")]
		float Speed { get; set; } // 32-bit
	
		[Export ("timeOffset")]
		double TimeOffset { get; set; }
	
		[Export ("repeatCount")]
		float RepeatCount { get; set; } // 32-bit
	
		[Export ("repeatDuration")]
		double RepeatDuration { get; set; }
	
		[Export ("autoreverses")]
		bool AutoReverses { get;set; }
	
		[Export ("fillMode")]
		string FillMode { get; set; }

		[Since (3,2)]
		[Export ("shadowColor")]
		CGColor ShadowColor { get; set; }

		[Since (3,2)]
		[Export ("shadowOffset")]
		CGSize ShadowOffset { get; set; }

		[Since (3,2)]
		[Export ("shadowOpacity")]
		float ShadowOpacity { get; set; } // 32-bit

		[Since (3,2)]
		[Export ("shadowRadius")]
		nfloat ShadowRadius { get; set; }

		[Field ("kCATransition")]
		NSString Transition { get; } 

		[Field ("kCAGravityCenter")]
		NSString GravityCenter { get; }

		[Field ("kCAGravityTop")]
		NSString GravityTop { get; }

		[Field ("kCAGravityBottom")]
		NSString GravityBottom { get; }

		[Field ("kCAGravityLeft")]
		NSString GravityLeft { get; }

		[Field ("kCAGravityRight")]
		NSString GravityRight { get; }

		[Field ("kCAGravityTopLeft")]
		NSString GravityTopLeft { get; }

		[Field ("kCAGravityTopRight")]
		NSString GravityTopRight { get; }

		[Field ("kCAGravityBottomLeft")]
		NSString GravityBottomLeft { get; }

		[Field ("kCAGravityBottomRight")]
		NSString GravityBottomRight { get; }

		[Field ("kCAGravityResize")]
		NSString GravityResize { get; }

		[Field ("kCAGravityResizeAspect")]
		NSString GravityResizeAspect { get; }

		[Field ("kCAGravityResizeAspectFill")]
		NSString GravityResizeAspectFill { get; }

		[Field ("kCAFilterNearest")]
		NSString FilterNearest { get; }

		[Field ("kCAFilterLinear")]
		NSString FilterLinear { get; }

		[Field ("kCAFilterTrilinear")]
		NSString FilterTrilinear { get; }

		[Field ("kCAOnOrderIn")]
		NSString OnOrderIn { get; }

		[Field ("kCAOnOrderOut")]
		NSString OnOrderOut { get; }

		[Export ("visibleRect")]
		CGRect VisibleRect { get;  }

		[Export ("scrollPoint:")]
		void ScrollPoint (CGPoint p);

		[Export ("scrollRectToVisible:")]
		void ScrollRectToVisible (CGRect r);

#if MONOMAC
		[Export ("autoresizingMask")]
		CAAutoresizingMask AutoresizingMask { get; set; }

		[Export ("resizeSublayersWithOldSize:")]
		void ResizeSublayers (CGSize oldSize);

		[Export ("resizeWithOldSuperlayerSize:")]
		void Resize (CGSize oldSuperlayerSize);
		
		[Export ("constraints")]
		CAConstraint[] Constraints { get; set;  }

		[Export ("addConstraint:")]
		void AddConstraint (CAConstraint c);

		[Export ("filters")]
		CIFilter [] Filters { get; set; }
#else
		[Since (3,2)]
		[Export ("shouldRasterize")]
		bool ShouldRasterize { get; set; }

		[Since (3,2)]
		[Export ("shadowPath")]
		CGPath ShadowPath { get; set; }

		[Since (3,2)]
		[Export ("rasterizationScale")]
		float RasterizationScale { get; set; }

		[Since (6,0)]
		[Export ("drawsAsynchronously")]
		bool DrawsAsynchronously { get; set; }
#endif
	}

	[BaseType (typeof (CALayer))]
	public interface CATiledLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();
		
		[Static][Export ("fadeDuration")]
		double FadeDuration { get; }

		[Export ("levelsOfDetail")]
		int LevelsOfDetail { get; set; }

		[Export ("levelsOfDetailBias")]
		int LevelsOfDetailBias { get; set; }

		[Export ("tileSize")]
		CGSize TileSize { get; set; }
	}

	[BaseType (typeof (CALayer))]
	public interface CAReplicatorLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("instanceCount")]
		int InstanceCount { get; set; }

		[Export ("instanceDelay")]
		double InstanceDelay { get; set; }

		[Export ("instanceTransform")]
		CATransform3D InstanceTransform { get; set; }

		[Export ("preservesDepth")]
		bool PreservesDepth { get; set; }

		[Export ("instanceColor")]
		CGColor InstanceColor { get; set; }

		[Export ("instanceRedOffset")]
		float InstanceRedOffset { get; set; }

		[Export ("instanceGreenOffset")]
		float InstanceGreenOffset { get; set; }

		[Export ("instanceBlueOffset")]
		float InstanceBlueOffset { get; set; }

		[Export ("instanceAlphaOffset")]
		float InstanceAlphaOffset { get; set; }
	}


	[BaseType (typeof (CALayer))]
	public interface CAScrollLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("scrollMode")]
		NSString ScrollMode { get; set;  }

		[Export ("scrollToPoint:")]
		void ScrollToPoint (CGPoint p);

		[Export ("scrollToRect:")]
		void ScrollToRect (CGRect r);

		[Field ("kCAScrollNone")]
		NSString ScrollNone { get; }

		[Field ("kCAScrollVertically")]
		NSString ScrollVertically { get; }

		[Field ("kCAScrollHorizontally")]
		NSString ScrollHorizontally { get; }

		[Field ("kCAScrollBoth")]
		NSString ScrollBoth { get; }
	}
	
	[BaseType (typeof (CALayer))]
	public interface CAShapeLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("path")] [NullAllowed]
		CGPath Path { get; set; }

		[Export ("fillColor")] [NullAllowed]
		CGColor FillColor { get; set; }

		[Export ("fillRule", ArgumentSemantic.Copy)]
		NSString FillRule { get; set; }

		[Export ("lineCap", ArgumentSemantic.Copy)]
		NSString LineCap { get; set; }

		[Export ("lineDashPattern", ArgumentSemantic.Copy)] [NullAllowed]
		NSNumber [] LineDashPattern { get; set; }

		[Export ("lineDashPhase")]
		float LineDashPhase { get; set; }

		[Export ("lineJoin", ArgumentSemantic.Copy)]
		NSString LineJoin { get; set; }

		[Export ("lineWidth")]
		float LineWidth { get; set; }

		[Export ("miterLimit")]
		float MiterLimit { get; set; }

		[Export ("strokeColor")] [NullAllowed]
		CGColor StrokeColor { get; set; }

		[Since (4,2)]
		[Export ("strokeStart")]
		float StrokeStart { get; set; }

		[Since (4,2)]
		[Export ("strokeEnd")]
		float StrokeEnd { get; set; }

		[Field ("kCALineJoinMiter")]
		NSString JoinMiter { get; }

		[Field ("kCALineJoinRound")]
		NSString JoinRound { get; }

		[Field ("kCALineJoinBevel")]
		NSString JoinBevel { get; }

		[Field ("kCALineCapButt")]
		NSString CapButt { get; }

		[Field ("kCALineCapRound")]
		NSString CapRound { get; }

		[Field ("kCALineCapSquare")]
		NSString CapSquare { get; }

		[Field ("kCAFillRuleNonZero")]
		NSString FillRuleNonZero { get; }

		[Field ("kCAFillRuleEvenOdd")]
		NSString FillRuleEvenOdd { get; }
	}

	[BaseType (typeof (CALayer))]
	public interface CATransformLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("hitTest:")]
		CALayer HitTest (CGPoint thePoint);
	}

	[Since (3,2)]
	[BaseType (typeof (CALayer))]
	public interface CATextLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("string", ArgumentSemantic.Copy)]
		string String { get; set; }

		[Export ("fontSize")]
		float FontSize { get; set; }

		[Export ("font"), Internal]
		IntPtr _Font { get; set; }
		
		[Export ("foregroundColor")]
		CGColor ForegroundColor { get; set; }

                [Export ("wrapped")]
                bool Wrapped { [Bind ("isWrapped")] get; set; }

		[Export ("truncationMode", ArgumentSemantic.Copy)]
		string TruncationMode { get; set; }

		[Export ("alignmentMode", ArgumentSemantic.Copy)]
		string AlignmentMode { get; set; }

		[Field ("kCATruncationNone")]
		NSString TruncationNone { get; }
		
		[Field ("kCATruncationStart")]
		NSString TruncantionStart { get; }
		
		[Field ("kCATruncationEnd")]
		NSString TruncantionEnd { get; }
		
		[Field ("kCATruncationMiddle")]
		NSString TruncantionMiddle { get; }
		
		[Field ("kCAAlignmentNatural")]
		NSString AlignmentNatural { get; }
		
		[Field ("kCAAlignmentLeft")]
		NSString AlignmentLeft { get; }
		
		[Field ("kCAAlignmentRight")]
		NSString AlignmentRight { get; }
		
		[Field ("kCAAlignmentCenter")]
		NSString AlignmentCenter { get; }
		
		[Field ("kCAAlignmentJustified")]
		NSString AlignmentJustified { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	public interface CALayerDelegate {
		[Export ("displayLayer:")]
		void DisplayLayer (CALayer layer);

		[Export ("drawLayer:inContext:"), EventArgs ("CALayerDrawEventArgs")]
		void DrawLayer (CALayer layer, CGContext context);

		[Export ("layoutSublayersOfLayer:")]
		void LayoutSublayersOfLayer (CALayer layer);

		[Export ("actionForLayer:forKey:"), EventArgs ("CALayerDelegateAction"), DefaultValue (null)]
		NSObject ActionForLayer (CALayer layer, string eventKey);
	}
	
#if !MONOMAC
	[BaseType (typeof (CALayer))]
	public interface CAEAGLLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		// From the interface  IEAGLDrawable
		[Export ("drawableProperties", ArgumentSemantic.Copy)]
		NSDictionary DrawableProperties { get; set; }
	}
#endif

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	[DisableDefaultCtor]
	public interface CAAction {
		[Export ("runActionForKey:object:arguments:")]
		void RunAction (string eventKey, NSObject obj, NSDictionary arguments);
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] { typeof (CAAnimationDelegate)})]
	public interface CAAnimation {
		[Export ("animation"), Static]
		CAAnimation CreateAnimation ();
	
		[Static]
		[Export ("defaultValueForKey:")]
		NSObject DefaultValue (string key);
	
		[Export ("timingFunction", ArgumentSemantic.Retain)]
		CAMediaTimingFunction TimingFunction { get; set; }
	
		[Wrap ("WeakDelegate")]
		CAAnimationDelegate Delegate { get; set; }
	
		[Export ("delegate", ArgumentSemantic.Retain)][NullAllowed]
		NSObject WeakDelegate { get; set; }
	
		[Export ("removedOnCompletion")]
		bool RemovedOnCompletion { [Bind ("isRemovedOnCompletion")] get; set; }

		[Export ("willChangeValueForKey:")]
		void WillChangeValueForKey (string key);
	
		[Export ("didChangeValueForKey:")]
		void DidChangeValueForKey (string key);

		[Export ("shouldArchiveValueForKey:")]
		[Since (4,0)]
		bool ShouldArchiveValueForKey (string key);

		//
		// From CAMediaTiming
		//
		[Export ("beginTime")]
		double BeginTime { get; set; }
	
		[Export ("duration")]
		double Duration { get; set; }
	
		[Export ("speed")]
		float Speed { get; set; }
	
		[Export ("timeOffset")]
		double TimeOffset { get; set; }
	
		[Export ("repeatCount")]
		float RepeatCount { get; set; }
	
		[Export ("repeatDuration")]
		double RepeatDuration { get; set; }
	
		[Export ("autoreverses")]
		bool AutoReverses { get;set; }
	
		[Export ("fillMode")]
		string FillMode { get; set; }

		[Field ("kCATransitionFade")]
		NSString TransitionFade { get; }
		
		[Field ("kCATransitionMoveIn")]
		NSString TransitionMoveIn { get; }

		[Field ("kCATransitionPush")]
		NSString TransitionPush { get; }
		
		[Field ("kCATransitionReveal")]
		NSString TransitionReveal { get; }
		
		[Field ("kCATransitionFromRight")]
		NSString TransitionFromRight { get; }
		
		[Field ("kCATransitionFromLeft")]
		NSString TransitionFromLeft { get; }

		[Field ("kCATransitionFromTop")]
		NSString TransitionFromTop { get; }

		[Field ("kCATransitionFromBottom")]
		NSString TransitionFromBottom { get; }
		
		/* 'calculationMode' strings. */
		[Field ("kCAAnimationLinear")]
		NSString AnimationLinear { get; }
				
		[Field ("kCAAnimationDiscrete")]
		NSString AnimationDescrete { get; }
		
		[Field ("kCAAnimationPaced")]
		NSString AnimationPaced { get; }
		
		/* 'rotationMode' strings. */
		[Field ("kCAAnimationRotateAuto")]
		NSString RotateModeAuto { get; }

		[Field ("kCAAnimationRotateAutoReverse")]
		NSString RotateModeAutoReverse { get; }

#if MONOMAC
		[MountainLion]
		[Export ("usesSceneTimeBase")]
		bool UsesSceneTimeBase { get; set; }
#endif
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	[Synthetic]
	public interface CAAnimationDelegate {
		[Export ("animationDidStart:")]
		void AnimationStarted (CAAnimation anim);
	
		[Export ("animationDidStop:finished:"), EventArgs ("CAAnimationState")]
		void AnimationStopped (CAAnimation anim, bool finished);
	
	}
	
	[BaseType (typeof (CAAnimation))]
	public interface CAPropertyAnimation {
		[Static]
		[Export ("animationWithKeyPath:")]
		CAPropertyAnimation FromKeyPath (string path);
	
		[Export ("keyPath", ArgumentSemantic.Copy)]
		string KeyPath { get; set; }
	
		[Export ("additive")]
		bool Additive { [Bind ("isAdditive")] get; set; }
	
		[Export ("cumulative")]
		bool Cumulative { [Bind ("isCumulative")] get; set; }
	
		[Export ("valueFunction", ArgumentSemantic.Retain)]
		CAValueFunction ValueFunction { get; set; }
	
	}
	
	[BaseType (typeof (CAPropertyAnimation))]
	public interface CABasicAnimation {
		[Static, New, Export ("animationWithKeyPath:")]
		CABasicAnimation FromKeyPath (string path);

		[Export ("fromValue", ArgumentSemantic.Retain)][NullAllowed]
		NSObject From { get; set; }
	
		[Export ("toValue", ArgumentSemantic.Retain)][NullAllowed]
		NSObject To { get; set; }
	
		[Export ("byValue", ArgumentSemantic.Retain)][NullAllowed]
		NSObject By { get; set; }
	}
	
	[BaseType (typeof (CAPropertyAnimation), Name="CAKeyframeAnimation")]
	public interface CAKeyFrameAnimation {
		[Static, New, Export ("animationWithKeyPath:")]
		CAKeyFrameAnimation GetFromKeyPath (string path);

		[Export ("values", ArgumentSemantic.Copy)]
		NSObject [] Values { get; set; }
	
		[Export ("path")]
		CGPath Path { get; set; }
	
		[Export ("keyTimes", ArgumentSemantic.Copy)][NullAllowed]
		NSNumber [] KeyTimes { get; set; }
	
		[Export ("timingFunctions", ArgumentSemantic.Copy)]
		CAMediaTimingFunction [] TimingFunctions { get; set; }
	
		[Export ("calculationMode", ArgumentSemantic.Copy)]
		string CalculationMode { get; set; }
	
		[Export ("rotationMode", ArgumentSemantic.Copy)][NullAllowed]
		string RotationMode { get; set; }

		[Field ("kCAAnimationLinear")]
		NSString CalculationLinear { get; }

		[Field ("kCAAnimationDiscrete")]
		NSString CalculationDiscrete { get; }
		
		[Field ("kCAAnimationDiscrete")]
		NSString CalculationPaced { get; }
	}
	
	[BaseType (typeof (CAAnimation))]
	public interface CATransition {
		[Export ("animation"), Static, New]
		CATransition CreateAnimation ();

		[Export ("type", ArgumentSemantic.Copy)]
		string Type { get; set; }
	
		[Export ("subtype", ArgumentSemantic.Copy)]
		string Subtype { get; set; }
	
		[Export ("startProgress")]
		float StartProgress { get; set; }
	
		[Export ("endProgress")]
		float EndProgress { get; set; }
	
		[Export ("filter", ArgumentSemantic.Retain)][NullAllowed]
		NSObject filter { get; set; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface CATransaction {
		[Static]
		[Export ("begin")]
		void Begin ();
	
		[Static]
		[Export ("commit")]
		void Commit ();
	
		[Static]
		[Export ("flush")]
		void Flush ();
	
		[Static]
		[Export ("lock")]
		void Lock ();
	
		[Static]
		[Export ("unlock")]
		void Unlock ();
	
		[Static]
		[Export ("animationDuration")]
		double AnimationDuration { get; set; }
	
		[Static, NullAllowed]
		[Export ("animationTimingFunction")]
		CAMediaTimingFunction AnimationTimingFunction { get; set; }
	
		[Static]
		[Export ("disableActions")]
		bool DisableActions { get; set; }
	
		[Static]
		[Export ("valueForKey:")]
		NSObject ValueForKey (NSString key);
	
		[Static]
		[Export ("setValue:forKey:")]
		void SetValueForKey (NSObject anObject, NSString key);

		[Since (4,0)]
		[Static, Export ("completionBlock"), NullAllowed]
		Action CompletionBlock { get; set; }

		[Field ("kCATransactionAnimationDuration")]
		NSString AnimationDurationKey { get; }
		
		[Field ("kCATransactionDisableActions")]
		NSString DisableActionsKey { get; }
		
		[Field ("kCATransactionAnimationTimingFunction")]
		NSString TimingFunctionKey { get; }
		
		[Field ("kCATransactionCompletionBlock")]
		NSString CompletionBlockKey { get; }
	}

	[BaseType (typeof (CAAnimation))]
	public interface CAAnimationGroup {
		[Export ("animations", ArgumentSemantic.Copy)]
		CAAnimation [] Animations { get; set; }

		[Export ("animation"), Static, New]
		CAAnimationGroup CreateAnimation ();
	}

	[BaseType (typeof (CALayer))]
	public interface CAGradientLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("colors", ArgumentSemantic.Copy)][Internal]
		IntPtr _Colors { get; set;  }
	
		[Export ("locations", ArgumentSemantic.Copy)]
		NSNumber [] Locations { get; set;  }
	
		[Export ("startPoint")]
		CGPoint StartPoint { get; set;  }

		[Export ("endPoint")]
		CGPoint EndPoint { get; set;  }
	
		[Export ("type", ArgumentSemantic.Copy)]
		string Type { get; set;  }

		[Field ("kCAGradientLayerAxial")]
		NSString GradientLayerAxial { get; }
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface CAMediaTimingFunction {
		[Export ("functionWithName:")][Static]
		CAMediaTimingFunction FromName (NSString  name);

		[Static]
		[Export ("functionWithControlPoints::::")]
		CAMediaTimingFunction FromControlPoints (float c1x, float c1y, float c2x, float c2y);
	
		[Export ("initWithControlPoints::::")]
		IntPtr Constructor (float c1x, float c1y, float c2x, float c2y);
	
		[Export ("getControlPointAtIndex:values:"), Internal]
		void GetControlPointAtIndex (int idx, IntPtr point);
	
		[Field("kCAMediaTimingFunctionLinear")]
		NSString Linear { get; }
		
		[Field("kCAMediaTimingFunctionEaseIn")]
		NSString EaseIn { get; }
		
		[Field("kCAMediaTimingFunctionEaseOut")]
		NSString EaseOut { get; }
		
		[Field("kCAMediaTimingFunctionEaseInEaseOut")]
		NSString EaseInEaseOut { get; }

		[Field("kCAMediaTimingFunctionDefault")]
		NSString Default { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface CAValueFunction {
		[Export ("functionWithName:"), Static]
		CAValueFunction FromName (string name);

		[Export ("name")]
		string Name { get; }

		[Field ("kCAValueFunctionRotateX")]
		NSString RotateX { get; }
		
		[Field ("kCAValueFunctionRotateY")]
		NSString RotateY { get; }
		
		[Field ("kCAValueFunctionRotateZ")]
		NSString RotateZ { get; }
		
		[Field ("kCAValueFunctionScale")]
		NSString Scale { get; }
		
		[Field ("kCAValueFunctionScaleX")]
		NSString ScaleX { get; }
		
		[Field ("kCAValueFunctionScaleY")]
		NSString ScaleY { get; }
		
		[Field ("kCAValueFunctionScaleZ")]
		NSString ScaleZ { get; }
		
		[Field ("kCAValueFunctionTranslate")]
		NSString Translate { get; }
		
		[Field ("kCAValueFunctionTranslateX")]
		NSString TranslateX { get; }
		
		[Field ("kCAValueFunctionTranslateY")]
		NSString TranslateY { get; }
		
		[Field ("kCAValueFunctionTranslateZ")]
		NSString TranslateZ { get; }
		
	}

#if MONOMAC
	[BaseType (typeof (CALayer))]
	interface CAOpenGLLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("asynchronous")]
		bool Asynchronous { [Bind ("isAsynchronous")]get; set; }	

		[Export ("canDrawInCGLContext:pixelFormat:forLayerTime:displayTime:")]
		bool CanDrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, ref CVTimeStamp timeStamp);

		[Export ("drawInCGLContext:pixelFormat:forLayerTime:displayTime:")]
		void DrawInCGLContext (CGLContext glContext, CGLPixelFormat pixelFormat, double timeInterval, ref CVTimeStamp timeStamp);

		[Export ("copyCGLPixelFormatForDisplayMask:")]
		CGLPixelFormat CopyCGLPixelFormatForDisplayMask (UInt32 mask);

		[Export ("releaseCGLPixelFormat:")]
		void Release (CGLPixelFormat pixelFormat);

		[Export ("copyCGLContextForPixelFormat:")]
		CGLContext CopyContext (CGLPixelFormat pixelFormat);

		[Export ("releaseCGLContext:")]
		void Release (CGLContext glContext);

	}
#endif

	[BaseType (typeof (NSObject))]
	interface CAEmitterCell {
		[Export ("name")]
		string Name { get; set;  }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("birthRate")]
		float BirthRate { get; set;  }

		[Export ("lifetime")]
		float LifeTime { get; set;  }

		[Export ("lifetimeRange")]
		float LifetimeRange { get; set;  }

		[Export ("emissionLatitude")]
		float EmissionLatitude { get; set;  }

		[Export ("emissionLongitude")]
		float EmissionLongitude { get; set;  }

		[Export ("emissionRange")]
		float EmissionRange { get; set;  }

		[Export ("velocity")]
		float Velocity { get; set;  }

		[Export ("velocityRange")]
		float VelocityRange { get; set;  }

		[Export ("xAcceleration")]
		float AccelerationX { get; set;  }

		[Export ("yAcceleration")]
		float AccelerationY { get; set;  }

		[Export ("zAcceleration")]
		float AccelerationZ { get; set;  }

		[Export ("scale")]
		float Scale { get; set;  }

		[Export ("scaleRange")]
		float ScaleRange { get; set;  }

		[Export ("scaleSpeed")]
		float ScaleSpeed { get; set;  }

		[Export ("spin")]
		float Spin { get; set;  }

		[Export ("spinRange")]
		float SpinRange { get; set;  }
		
		[Export ("color")]
		CGColor Color { get; set;  }

		[Export ("redSpeed")]
		float RedSpeed { get; set;  }

		[Export ("greenSpeed")]
		float GreenSpeed { get; set;  }

		[Export ("blueSpeed")]
		float BlueSpeed { get; set;  }

		[Export ("alphaSpeed")]
		float AlphaSpeed { get; set;  }

		[Export ("contents")]
		NSObject WeakContents { get; set;  }

		[Internal][Sealed]
		[Export ("contents")]
		IntPtr _Contents { get; set; }

		[Export ("contentsRect")]
		CGRect ContentsRect { get; set;  }

		[Export ("minificationFilter")]
		string MinificationFilter { get; set;  }

		[Export ("magnificationFilter")]
		string MagnificationFilter { get; set;  }

		[Export ("minificationFilterBias")]
		float MinificationFilterBias { get; set;  }

		[Export ("emitterCells")]
		CAEmitterCell[] Cells { get; set;  }

		[Export ("style", ArgumentSemantic.Copy)]
		NSDictionary Style { get; set;  }
		
		[Static]
		[Export ("emitterCell")]
		CAEmitterCell EmitterCell ();

		[Static]
		[Export ("defaultValueForKey:")]
		NSObject DefaultValueForKey (string key);

		[Export ("shouldArchiveValueForKey:")]
		bool ShouldArchiveValueForKey (string key);
#if !MONOMAC
		[Export ("redRange")]
		float RedRange { get; set; }
		
		[Export ("greenRange")]
		float GreenRange { get; set; }

		[Export ("blueRange")]
		float BlueRange { get; set; }

		[Export ("alphaRange")]
		float AlphaRange { get; set; }
#endif
	}
	
	[BaseType (typeof (CALayer))]
	interface CAEmitterLayer {
		[Export ("layer"), New, Static]
		CALayer Create ();

		[Export ("emitterCells")]
		CAEmitterCell[] Cells { get; set;  }

		[Export ("birthRate")]
		float BirthRate { get; set;  } // 32-bit

		[Export ("lifetime")]
		float LifeTime { get; set;  } // 32-bit

		[Export ("emitterPosition")]
		CGPoint Position { get; set;  }

		[Export ("emitterZPosition")]
		nfloat ZPosition { get; set;  }

		[Export ("emitterSize")]
		CGSize Size { get; set;  }

		[Export ("emitterDepth")]
		nfloat Depth { get; set;  }

		[Export ("emitterShape")]
		string Shape { get; set;  }

		[Export ("emitterMode")]
		string Mode { get; set;  }

		[Export ("renderMode")]
		string RenderMode { get; set;  }

		[Export ("preservesDepth")]
		bool PreservesDepth { get; set;  }

		[Export ("velocity")]
		float Velocity { get; set;  } // 32-bit

		[Export ("scale")]
		float Scale { get; set;  } // 32-bit

		[Export ("spin")]
		float Spin { get; set;  } // 32-bit

		[Export ("seed")]
		int Seed { get; set;  } // 32-bit
		
		/** `emitterShape' values. **/
		[Field ("kCAEmitterLayerPoint")]
		NSString ShapePoint { get; }		

		[Field ("kCAEmitterLayerLine")]
		NSString ShapeLine { get; }		

		[Field ("kCAEmitterLayerRectangle")]
		NSString ShapeRectangle { get; }		
		
		[Field ("kCAEmitterLayerCuboid")]
		NSString ShapeCuboid { get; }		
		
		[Field ("kCAEmitterLayerCircle")]
		NSString ShapeCircle { get; }		
		
		[Field ("kCAEmitterLayerSphere")]
		NSString ShapeSphere { get; }		
	
		/** `emitterMode' values. **/
		[Field ("kCAEmitterLayerPoints")]
		NSString ModePoints { get; }			

		[Field ("kCAEmitterLayerOutline")]
		NSString ModeOutline { get; }			

		[Field ("kCAEmitterLayerSurface")]
		NSString ModeSurface { get; }			

		[Field ("kCAEmitterLayerVolume")]
		NSString ModeVolume { get; }			

		/** `renderOrder' values. **/		
		[Field ("kCAEmitterLayerUnordered")]
		NSString RenderUnordered { get; }			

		[Field ("kCAEmitterLayerOldestFirst")]
		NSString RenderOldestFirst { get; }			

		[Field ("kCAEmitterLayerOldestLast")]
		NSString RenderOldestLast { get; }			

		[Field ("kCAEmitterLayerBackToFront")]
		NSString RenderBackToFront { get; }			

		[Field ("kCAEmitterLayerAdditive")]
		NSString RenderAdditive { get; }			

		
	}
}
