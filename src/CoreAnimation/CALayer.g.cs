//
// Auto-generated from generator.cs, do not edit
//
using System;

using System.Drawing;

using System.Runtime.InteropServices;

using MonoMac.CoreFoundation;

using MonoMac.Foundation;

using MonoMac.ObjCRuntime;

using MonoMac.CoreGraphics;

using MonoMac.CoreAnimation;

namespace MonoMac.CoreAnimation {
	[Register("CALayer")]
	public partial class CALayer : NSObject {
		static IntPtr selPresentationLayer = Selector.GetHandle ("presentationLayer");
		static IntPtr selModelLayer = Selector.GetHandle ("modelLayer");
		static IntPtr selBounds = Selector.GetHandle ("bounds");
		static IntPtr selSetBounds = Selector.GetHandle ("setBounds:");
		static IntPtr selZPosition = Selector.GetHandle ("zPosition");
		static IntPtr selSetZPosition = Selector.GetHandle ("setZPosition:");
		static IntPtr selAnchorPoint = Selector.GetHandle ("anchorPoint");
		static IntPtr selSetAnchorPoint = Selector.GetHandle ("setAnchorPoint:");
		static IntPtr selAnchorPointZ = Selector.GetHandle ("anchorPointZ");
		static IntPtr selSetAnchorPointZ = Selector.GetHandle ("setAnchorPointZ:");
		static IntPtr selPosition = Selector.GetHandle ("position");
		static IntPtr selSetPosition = Selector.GetHandle ("setPosition:");
		static IntPtr selTransform = Selector.GetHandle ("transform");
		static IntPtr selSetTransform = Selector.GetHandle ("setTransform:");
		static IntPtr selAffineTransform = Selector.GetHandle ("affineTransform");
		static IntPtr selSetAffineTransform = Selector.GetHandle ("setAffineTransform:");
		static IntPtr selFrame = Selector.GetHandle ("frame");
		static IntPtr selSetFrame = Selector.GetHandle ("setFrame:");
		static IntPtr selIsHidden = Selector.GetHandle ("isHidden");
		static IntPtr selSetHidden = Selector.GetHandle ("setHidden:");
		static IntPtr selIsDoubleSided = Selector.GetHandle ("isDoubleSided");
		static IntPtr selSetDoubleSided = Selector.GetHandle ("setDoubleSided:");
		static IntPtr selIsGeometryFlipped = Selector.GetHandle ("isGeometryFlipped");
		static IntPtr selSetGeometryFlipped = Selector.GetHandle ("setGeometryFlipped:");
		static IntPtr selContentsAreFlipped = Selector.GetHandle ("contentsAreFlipped");
		static IntPtr selSuperlayer = Selector.GetHandle ("superlayer");
		static IntPtr selSublayers = Selector.GetHandle ("sublayers");
		static IntPtr selSetSublayers = Selector.GetHandle ("setSublayers:");
		static IntPtr selSublayerTransform = Selector.GetHandle ("sublayerTransform");
		static IntPtr selSetSublayerTransform = Selector.GetHandle ("setSublayerTransform:");
		static IntPtr selMask = Selector.GetHandle ("mask");
		static IntPtr selSetMask = Selector.GetHandle ("setMask:");
		static IntPtr selMasksToBounds = Selector.GetHandle ("masksToBounds");
		static IntPtr selSetMasksToBounds = Selector.GetHandle ("setMasksToBounds:");
		static IntPtr selContents = Selector.GetHandle ("contents");
		static IntPtr selSetContents = Selector.GetHandle ("setContents:");
		static IntPtr selContentsRect = Selector.GetHandle ("contentsRect");
		static IntPtr selSetContentsRect = Selector.GetHandle ("setContentsRect:");
		static IntPtr selContentsGravity = Selector.GetHandle ("contentsGravity");
		static IntPtr selSetContentsGravity = Selector.GetHandle ("setContentsGravity:");
		static IntPtr selContentsCenter = Selector.GetHandle ("contentsCenter");
		static IntPtr selSetContentsCenter = Selector.GetHandle ("setContentsCenter:");
		static IntPtr selMinificationFilter = Selector.GetHandle ("minificationFilter");
		static IntPtr selSetMinificationFilter = Selector.GetHandle ("setMinificationFilter:");
		static IntPtr selMagnificationFilter = Selector.GetHandle ("magnificationFilter");
		static IntPtr selSetMagnificationFilter = Selector.GetHandle ("setMagnificationFilter:");
		static IntPtr selIsOpaque = Selector.GetHandle ("isOpaque");
		static IntPtr selSetOpaque = Selector.GetHandle ("setOpaque:");
		static IntPtr selNeedsDisplay = Selector.GetHandle ("needsDisplay");
		static IntPtr selNeedsDisplayOnBoundsChange = Selector.GetHandle ("needsDisplayOnBoundsChange");
		static IntPtr selSetNeedsDisplayOnBoundsChange = Selector.GetHandle ("setNeedsDisplayOnBoundsChange:");
		static IntPtr selBackgroundColor = Selector.GetHandle ("backgroundColor");
		static IntPtr selSetBackgroundColor = Selector.GetHandle ("setBackgroundColor:");
		static IntPtr selCornerRadius = Selector.GetHandle ("cornerRadius");
		static IntPtr selSetCornerRadius = Selector.GetHandle ("setCornerRadius:");
		static IntPtr selBorderWidth = Selector.GetHandle ("borderWidth");
		static IntPtr selSetBorderWidth = Selector.GetHandle ("setBorderWidth:");
		static IntPtr selBorderColor = Selector.GetHandle ("borderColor");
		static IntPtr selSetBorderColor = Selector.GetHandle ("setBorderColor:");
		static IntPtr selOpacity = Selector.GetHandle ("opacity");
		static IntPtr selSetOpacity = Selector.GetHandle ("setOpacity:");
		static IntPtr selEdgeAntialiasingMask = Selector.GetHandle ("edgeAntialiasingMask");
		static IntPtr selSetEdgeAntialiasingMask = Selector.GetHandle ("setEdgeAntialiasingMask:");
		static IntPtr selActions = Selector.GetHandle ("actions");
		static IntPtr selSetActions = Selector.GetHandle ("setActions:");
		static IntPtr selAnimationKeys = Selector.GetHandle ("animationKeys");
		static IntPtr selName = Selector.GetHandle ("name");
		static IntPtr selSetName = Selector.GetHandle ("setName:");
		static IntPtr selDelegate = Selector.GetHandle ("delegate");
		static IntPtr selSetDelegate = Selector.GetHandle ("setDelegate:");
		static IntPtr selBeginTime = Selector.GetHandle ("beginTime");
		static IntPtr selSetBeginTime = Selector.GetHandle ("setBeginTime:");
		static IntPtr selDuration = Selector.GetHandle ("duration");
		static IntPtr selSetDuration = Selector.GetHandle ("setDuration:");
		static IntPtr selSpeed = Selector.GetHandle ("speed");
		static IntPtr selSetSpeed = Selector.GetHandle ("setSpeed:");
		static IntPtr selTimeOffset = Selector.GetHandle ("timeOffset");
		static IntPtr selSetTimeOffset = Selector.GetHandle ("setTimeOffset:");
		static IntPtr selRepeatCount = Selector.GetHandle ("repeatCount");
		static IntPtr selSetRepeatCount = Selector.GetHandle ("setRepeatCount:");
		static IntPtr selRepeatDuration = Selector.GetHandle ("repeatDuration");
		static IntPtr selSetRepeatDuration = Selector.GetHandle ("setRepeatDuration:");
		static IntPtr selAutoreverses = Selector.GetHandle ("autoreverses");
		static IntPtr selSetAutoreverses = Selector.GetHandle ("setAutoreverses:");
		static IntPtr selFillMode = Selector.GetHandle ("fillMode");
		static IntPtr selSetFillMode = Selector.GetHandle ("setFillMode:");
		static IntPtr selShadowColor = Selector.GetHandle ("shadowColor");
		static IntPtr selSetShadowColor = Selector.GetHandle ("setShadowColor:");
		static IntPtr selShadowOffset = Selector.GetHandle ("shadowOffset");
		static IntPtr selSetShadowOffset = Selector.GetHandle ("setShadowOffset:");
		static IntPtr selShadowOpacity = Selector.GetHandle ("shadowOpacity");
		static IntPtr selSetShadowOpacity = Selector.GetHandle ("setShadowOpacity:");
		static IntPtr selShadowRadius = Selector.GetHandle ("shadowRadius");
		static IntPtr selSetShadowRadius = Selector.GetHandle ("setShadowRadius:");
		static IntPtr selVisibleRect = Selector.GetHandle ("visibleRect");
		static IntPtr selAutoresizingMask = Selector.GetHandle ("autoresizingMask");
		static IntPtr selSetAutoresizingMask = Selector.GetHandle ("setAutoresizingMask:");
		static IntPtr selLayer = Selector.GetHandle ("layer");
		static IntPtr selDefaultValueForKey = Selector.GetHandle ("defaultValueForKey:");
		static IntPtr selNeedsDisplayForKey = Selector.GetHandle ("needsDisplayForKey:");
		static IntPtr selRemoveFromSuperlayer = Selector.GetHandle ("removeFromSuperlayer");
		static IntPtr selAddSublayer = Selector.GetHandle ("addSublayer:");
		static IntPtr selInsertSublayerAtIndex = Selector.GetHandle ("insertSublayer:atIndex:");
		static IntPtr selInsertSublayerBelow = Selector.GetHandle ("insertSublayer:below:");
		static IntPtr selInsertSublayerAbove = Selector.GetHandle ("insertSublayer:above:");
		static IntPtr selReplaceSublayerWith = Selector.GetHandle ("replaceSublayer:with:");
		static IntPtr selConvertPointFromLayer = Selector.GetHandle ("convertPoint:fromLayer:");
		static IntPtr selConvertPointToLayer = Selector.GetHandle ("convertPoint:toLayer:");
		static IntPtr selConvertRectFromLayer = Selector.GetHandle ("convertRect:fromLayer:");
		static IntPtr selConvertRectToLayer = Selector.GetHandle ("convertRect:toLayer:");
		static IntPtr selConvertTimeFromLayer = Selector.GetHandle ("convertTime:fromLayer:");
		static IntPtr selConvertTimeToLayer = Selector.GetHandle ("convertTime:toLayer:");
		static IntPtr selHitTest = Selector.GetHandle ("hitTest:");
		static IntPtr selContainsPoint = Selector.GetHandle ("containsPoint:");
		static IntPtr selDisplay = Selector.GetHandle ("display");
		static IntPtr selSetNeedsDisplay = Selector.GetHandle ("setNeedsDisplay");
		static IntPtr selSetNeedsDisplayInRect = Selector.GetHandle ("setNeedsDisplayInRect:");
		static IntPtr selDisplayIfNeeded = Selector.GetHandle ("displayIfNeeded");
		static IntPtr selDrawInContext = Selector.GetHandle ("drawInContext:");
		static IntPtr selRenderInContext = Selector.GetHandle ("renderInContext:");
		static IntPtr selPreferredFrameSize = Selector.GetHandle ("preferredFrameSize");
		static IntPtr selSetNeedsLayout = Selector.GetHandle ("setNeedsLayout");
		static IntPtr selNeedsLayout = Selector.GetHandle ("needsLayout");
		static IntPtr selLayoutIfNeeded = Selector.GetHandle ("layoutIfNeeded");
		static IntPtr selLayoutSublayers = Selector.GetHandle ("layoutSublayers");
		static IntPtr selDefaultActionForKey = Selector.GetHandle ("defaultActionForKey:");
		static IntPtr selActionForKey = Selector.GetHandle ("actionForKey:");
		static IntPtr selAddAnimationForKey = Selector.GetHandle ("addAnimation:forKey:");
		static IntPtr selRemoveAllAnimations = Selector.GetHandle ("removeAllAnimations");
		static IntPtr selRemoveAnimationForKey = Selector.GetHandle ("removeAnimationForKey:");
		static IntPtr selAnimationForKey = Selector.GetHandle ("animationForKey:");
		static IntPtr selScrollPoint = Selector.GetHandle ("scrollPoint:");
		static IntPtr selScrollRectToVisible = Selector.GetHandle ("scrollRectToVisible:");
		static IntPtr selResizeSublayersWithOldSize = Selector.GetHandle ("resizeSublayersWithOldSize");
		static IntPtr selResizeWithOldSuperlayerSize = Selector.GetHandle ("resizeWithOldSuperlayerSize:");

		static IntPtr class_ptr = Class.GetHandle ("CALayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CALayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CALayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CALayer (NSObjectFlag t) : base (t) {}

		public CALayer (IntPtr handle) : base (handle) {}

		[Export ("layer")]
		public static CALayer Create ()
		{
			return (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selLayer));
		}

		[Export ("defaultValueForKey:")]
		public static NSObject DefaultValue (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			NSObject ret;
			ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selDefaultValueForKey, nskey.Handle));
						nskey.Dispose ();

			return ret;
		}

		[Export ("needsDisplayForKey:")]
		public static bool NeedsDisplayForKey (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			bool ret;
			ret = MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend_IntPtr (class_ptr, selNeedsDisplayForKey, nskey.Handle);
						nskey.Dispose ();

			return ret;
		}

		[Export ("removeFromSuperlayer")]
		public virtual void RemoveFromSuperLayer ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selRemoveFromSuperlayer);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selRemoveFromSuperlayer);
			}
		}

		[Export ("addSublayer:")]
		public virtual void AddSublayer (CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selAddSublayer, layer.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selAddSublayer, layer.Handle);
			}
			#pragma warning disable 168
			var postget = Sublayers;
			#pragma warning restore 168
		}

		[Export ("insertSublayer:atIndex:")]
		public virtual void InsertSublayer (CALayer layer, int index)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_int (this.Handle, selInsertSublayerAtIndex, layer.Handle, index);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_int (this.SuperHandle, selInsertSublayerAtIndex, layer.Handle, index);
			}
			#pragma warning disable 168
			var postget = Sublayers;
			#pragma warning restore 168
		}

		[Export ("insertSublayer:below:")]
		public virtual void InsertSublayerBelow (CALayer layer, CALayer sibling)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (sibling == null)
				throw new ArgumentNullException ("sibling");
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selInsertSublayerBelow, layer.Handle, sibling.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selInsertSublayerBelow, layer.Handle, sibling.Handle);
			}
			#pragma warning disable 168
			var postget = Sublayers;
			#pragma warning restore 168
		}

		[Export ("insertSublayer:above:")]
		public virtual void InsertSublayerAbove (CALayer layer, CALayer sibling)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (sibling == null)
				throw new ArgumentNullException ("sibling");
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selInsertSublayerAbove, layer.Handle, sibling.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selInsertSublayerAbove, layer.Handle, sibling.Handle);
			}
			#pragma warning disable 168
			var postget = Sublayers;
			#pragma warning restore 168
		}

		[Export ("replaceSublayer:with:")]
		public virtual void ReplaceSublayer (CALayer layer, CALayer with)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (with == null)
				throw new ArgumentNullException ("with");
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selReplaceSublayerWith, layer.Handle, with.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selReplaceSublayerWith, layer.Handle, with.Handle);
			}
			#pragma warning disable 168
			var postget = Sublayers;
			#pragma warning restore 168
		}

		[Export ("convertPoint:fromLayer:")]
		public virtual System.Drawing.PointF ConvertPointFromLayer (System.Drawing.PointF point, CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			System.Drawing.PointF ret;
			if (IsDirectBinding) {
				ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSend_PointF_IntPtr (this.Handle, selConvertPointFromLayer, point, layer.Handle);
			} else {
				ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSendSuper_PointF_IntPtr (this.SuperHandle, selConvertPointFromLayer, point, layer.Handle);
			}
			return ret;
		}

		[Export ("convertPoint:toLayer:")]
		public virtual System.Drawing.PointF ConvertPointToLayer (System.Drawing.PointF point, CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			System.Drawing.PointF ret;
			if (IsDirectBinding) {
				ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSend_PointF_IntPtr (this.Handle, selConvertPointToLayer, point, layer.Handle);
			} else {
				ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSendSuper_PointF_IntPtr (this.SuperHandle, selConvertPointToLayer, point, layer.Handle);
			}
			return ret;
		}

		[Export ("convertRect:fromLayer:")]
		public virtual System.Drawing.RectangleF ConvertRectfromLayer (System.Drawing.RectangleF rect, CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			System.Drawing.RectangleF ret;
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret_RectangleF_IntPtr (out ret, this.Handle, selConvertRectFromLayer, rect, layer.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret_RectangleF_IntPtr (out ret, this.SuperHandle, selConvertRectFromLayer, rect, layer.Handle);
			}
			return ret;
		}

		[Export ("convertRect:toLayer:")]
		public virtual System.Drawing.RectangleF ConvertRectToLayer (System.Drawing.RectangleF rect, CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			System.Drawing.RectangleF ret;
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret_RectangleF_IntPtr (out ret, this.Handle, selConvertRectToLayer, rect, layer.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret_RectangleF_IntPtr (out ret, this.SuperHandle, selConvertRectToLayer, rect, layer.Handle);
			}
			return ret;
		}

		[Export ("convertTime:fromLayer:")]
		public virtual System.Double ConvertTimeFromLayer (System.Double rect, CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (IsDirectBinding) {
				return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend_Double_IntPtr (this.Handle, selConvertTimeFromLayer, rect, layer.Handle);
			} else {
				return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper_Double_IntPtr (this.SuperHandle, selConvertTimeFromLayer, rect, layer.Handle);
			}
		}

		[Export ("convertTime:toLayer:")]
		public virtual System.Double ConvertTimeToLayer (System.Double t, CALayer layer)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			if (IsDirectBinding) {
				return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend_Double_IntPtr (this.Handle, selConvertTimeToLayer, t, layer.Handle);
			} else {
				return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper_Double_IntPtr (this.SuperHandle, selConvertTimeToLayer, t, layer.Handle);
			}
		}

		[Export ("hitTest:")]
		public virtual CALayer HitTest (System.Drawing.PointF p)
		{
			if (IsDirectBinding) {
				return (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_PointF (this.Handle, selHitTest, p));
			} else {
				return (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_PointF (this.SuperHandle, selHitTest, p));
			}
		}

		[Export ("containsPoint:")]
		public virtual bool Contains (System.Drawing.PointF p)
		{
			if (IsDirectBinding) {
				return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend_PointF (this.Handle, selContainsPoint, p);
			} else {
				return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper_PointF (this.SuperHandle, selContainsPoint, p);
			}
		}

		[Export ("display")]
		public virtual void Display ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selDisplay);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selDisplay);
			}
		}

		[Export ("setNeedsDisplay")]
		public virtual void SetNeedsDisplay ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selSetNeedsDisplay);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selSetNeedsDisplay);
			}
		}

		[Export ("setNeedsDisplayInRect:")]
		public virtual void SetNeedsDisplayInRect (System.Drawing.RectangleF r)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selSetNeedsDisplayInRect, r);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selSetNeedsDisplayInRect, r);
			}
		}

		[Export ("displayIfNeeded")]
		public virtual void DisplayIfNeeded ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selDisplayIfNeeded);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selDisplayIfNeeded);
			}
		}

		[Export ("drawInContext:")]
		public virtual void DrawInContext (CGContext ctx)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selDrawInContext, ctx.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selDrawInContext, ctx.Handle);
			}
		}

		[Export ("renderInContext:")]
		public virtual void RenderInContext (CGContext ctx)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selRenderInContext, ctx.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selRenderInContext, ctx.Handle);
			}
		}

		[Export ("preferredFrameSize")]
		public virtual System.Drawing.SizeF PreferredFrameSize ()
		{
			System.Drawing.SizeF ret;
			if (IsDirectBinding) {
				ret = MonoMac.ObjCRuntime.Messaging.SizeF_objc_msgSend (this.Handle, selPreferredFrameSize);
			} else {
				ret = MonoMac.ObjCRuntime.Messaging.SizeF_objc_msgSendSuper (this.SuperHandle, selPreferredFrameSize);
			}
			return ret;
		}

		[Export ("setNeedsLayout")]
		public virtual void SetNeedsLayout ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selSetNeedsLayout);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selSetNeedsLayout);
			}
		}

		[Export ("needsLayout")]
		public virtual bool NeedsLayout ()
		{
			if (IsDirectBinding) {
				return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selNeedsLayout);
			} else {
				return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selNeedsLayout);
			}
		}

		[Export ("layoutIfNeeded")]
		public virtual void LayoutIfNeeded ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selLayoutIfNeeded);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selLayoutIfNeeded);
			}
		}

		[Export ("layoutSublayers")]
		public virtual void LayoutSublayers ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selLayoutSublayers);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selLayoutSublayers);
			}
		}

		[Export ("defaultActionForKey:")]
		public virtual CAAction DefaultActionForKey (string eventKey)
		{
			if (eventKey == null)
				throw new ArgumentNullException ("eventKey");
			var nseventKey = new NSString (eventKey);

			CAAction ret;
			if (IsDirectBinding) {
				ret = (CAAction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, selDefaultActionForKey, nseventKey.Handle));
			} else {
				ret = (CAAction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, selDefaultActionForKey, nseventKey.Handle));
			}
						nseventKey.Dispose ();

			return ret;
		}

		[Export ("actionForKey:")]
		public virtual CAAction ActionForKey (string eventKey)
		{
			if (eventKey == null)
				throw new ArgumentNullException ("eventKey");
			var nseventKey = new NSString (eventKey);

			CAAction ret;
			if (IsDirectBinding) {
				ret = (CAAction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, selActionForKey, nseventKey.Handle));
			} else {
				ret = (CAAction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, selActionForKey, nseventKey.Handle));
			}
						nseventKey.Dispose ();

			return ret;
		}

		[Export ("addAnimation:forKey:")]
		public virtual void AddAnimation (CAAnimation animation, string key)
		{
			if (animation == null)
				throw new ArgumentNullException ("animation");
			var nskey = key == null ? null : new NSString (key);

			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selAddAnimationForKey, animation.Handle, nskey == null ? IntPtr.Zero : nskey.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selAddAnimationForKey, animation.Handle, nskey == null ? IntPtr.Zero : nskey.Handle);
			}
						if (nskey != null)
				nskey.Dispose ();
		}

		[Export ("removeAllAnimations")]
		public virtual void RemoveAllAnimations ()
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (this.Handle, selRemoveAllAnimations);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper (this.SuperHandle, selRemoveAllAnimations);
			}
		}

		[Export ("removeAnimationForKey:")]
		public virtual void RemoveAnimation (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selRemoveAnimationForKey, nskey.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selRemoveAnimationForKey, nskey.Handle);
			}
						nskey.Dispose ();

		}

		[Export ("animationForKey:")]
		public virtual CAAnimation AnimationForKey (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			CAAnimation ret;
			if (IsDirectBinding) {
				ret = (CAAnimation) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, selAnimationForKey, nskey.Handle));
			} else {
				ret = (CAAnimation) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, selAnimationForKey, nskey.Handle));
			}
						nskey.Dispose ();

			return ret;
		}

		[Export ("scrollPoint:")]
		public virtual void ScrollPoint (System.Drawing.PointF p)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_PointF (this.Handle, selScrollPoint, p);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_PointF (this.SuperHandle, selScrollPoint, p);
			}
		}

		[Export ("scrollRectToVisible:")]
		public virtual void ScrollRectToVisible (System.Drawing.RectangleF r)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selScrollRectToVisible, r);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selScrollRectToVisible, r);
			}
		}

		[Export ("resizeSublayersWithOldSize")]
		public virtual void ResizeSublayers (System.Drawing.SizeF oldSize)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_SizeF (this.Handle, selResizeSublayersWithOldSize, oldSize);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_SizeF (this.SuperHandle, selResizeSublayersWithOldSize, oldSize);
			}
		}

		[Export ("resizeWithOldSuperlayerSize:")]
		public virtual void Resize (System.Drawing.SizeF oldSuperlayerSize)
		{
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_SizeF (this.Handle, selResizeWithOldSuperlayerSize, oldSuperlayerSize);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_SizeF (this.SuperHandle, selResizeWithOldSuperlayerSize, oldSuperlayerSize);
			}
		}

		MonoMac.CoreAnimation.CALayer __mt_PresentationLayer_var;
		public virtual CALayer PresentationLayer {
			[Export ("presentationLayer")]
			get {
				CALayer ret;
				if (IsDirectBinding) {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selPresentationLayer));
				} else {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selPresentationLayer));
				}
				__mt_PresentationLayer_var = ret;
				return ret;
			}

		}

		MonoMac.CoreAnimation.CALayer __mt_ModelLayer_var;
		public virtual CALayer ModelLayer {
			[Export ("modelLayer")]
			get {
				CALayer ret;
				if (IsDirectBinding) {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selModelLayer));
				} else {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selModelLayer));
				}
				__mt_ModelLayer_var = ret;
				return ret;
			}

		}

		public virtual System.Drawing.RectangleF Bounds {
			[Export ("bounds")]
			get {
				System.Drawing.RectangleF ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret (out ret, this.Handle, selBounds);
				} else {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret (out ret, this.SuperHandle, selBounds);
				}
				return ret;
			}

			[Export ("setBounds:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selSetBounds, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selSetBounds, value);
				}
			}
		}

		public virtual float ZPosition {
			[Export ("zPosition")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selZPosition);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selZPosition);
				}
			}

			[Export ("setZPosition:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetZPosition, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetZPosition, value);
				}
			}
		}

		public virtual System.Drawing.PointF AnchorPoint {
			[Export ("anchorPoint")]
			get {
				System.Drawing.PointF ret;
				if (IsDirectBinding) {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSend (this.Handle, selAnchorPoint);
				} else {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSendSuper (this.SuperHandle, selAnchorPoint);
				}
				return ret;
			}

			[Export ("setAnchorPoint:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_PointF (this.Handle, selSetAnchorPoint, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_PointF (this.SuperHandle, selSetAnchorPoint, value);
				}
			}
		}

		public virtual float AnchorPointZ {
			[Export ("anchorPointZ")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selAnchorPointZ);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selAnchorPointZ);
				}
			}

			[Export ("setAnchorPointZ:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetAnchorPointZ, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetAnchorPointZ, value);
				}
			}
		}

		public virtual System.Drawing.PointF Position {
			[Export ("position")]
			get {
				System.Drawing.PointF ret;
				if (IsDirectBinding) {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSend (this.Handle, selPosition);
				} else {
					ret = MonoMac.ObjCRuntime.Messaging.PointF_objc_msgSendSuper (this.SuperHandle, selPosition);
				}
				return ret;
			}

			[Export ("setPosition:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_PointF (this.Handle, selSetPosition, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_PointF (this.SuperHandle, selSetPosition, value);
				}
			}
		}

		public virtual CATransform3D Transform {
			[Export ("transform")]
			get {
				CATransform3D ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.CATransform3D_objc_msgSend_stret (out ret, this.Handle, selTransform);
				} else {
					MonoMac.ObjCRuntime.Messaging.CATransform3D_objc_msgSendSuper_stret (out ret, this.SuperHandle, selTransform);
				}
				return ret;
			}

			[Export ("setTransform:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_CATransform3D (this.Handle, selSetTransform, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_CATransform3D (this.SuperHandle, selSetTransform, value);
				}
			}
		}

		public virtual CGAffineTransform AffineTransform {
			[Export ("affineTransform")]
			get {
				CGAffineTransform ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.CGAffineTransform_objc_msgSend_stret (out ret, this.Handle, selAffineTransform);
				} else {
					MonoMac.ObjCRuntime.Messaging.CGAffineTransform_objc_msgSendSuper_stret (out ret, this.SuperHandle, selAffineTransform);
				}
				return ret;
			}

			[Export ("setAffineTransform:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_CGAffineTransform (this.Handle, selSetAffineTransform, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_CGAffineTransform (this.SuperHandle, selSetAffineTransform, value);
				}
			}
		}

		public virtual System.Drawing.RectangleF Frame {
			[Export ("frame")]
			get {
				System.Drawing.RectangleF ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret (out ret, this.Handle, selFrame);
				} else {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret (out ret, this.SuperHandle, selFrame);
				}
				return ret;
			}

			[Export ("setFrame:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selSetFrame, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selSetFrame, value);
				}
			}
		}

		public virtual bool Hidden {
			[Export ("isHidden")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsHidden);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsHidden);
				}
			}

			[Export ("setHidden:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetHidden, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetHidden, value);
				}
			}
		}

		public virtual bool DoubleSided {
			[Export ("isDoubleSided")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsDoubleSided);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsDoubleSided);
				}
			}

			[Export ("setDoubleSided:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetDoubleSided, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetDoubleSided, value);
				}
			}
		}

		public virtual bool GeometryFlipped {
			[Export ("isGeometryFlipped")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsGeometryFlipped);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsGeometryFlipped);
				}
			}

			[Export ("setGeometryFlipped:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetGeometryFlipped, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetGeometryFlipped, value);
				}
			}
		}

		public virtual bool ContentsAreFlipped {
			[Export ("contentsAreFlipped")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selContentsAreFlipped);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selContentsAreFlipped);
				}
			}

		}

		MonoMac.CoreAnimation.CALayer __mt_SuperLayer_var;
		public virtual CALayer SuperLayer {
			[Export ("superlayer")]
			get {
				CALayer ret;
				if (IsDirectBinding) {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selSuperlayer));
				} else {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selSuperlayer));
				}
				__mt_SuperLayer_var = ret;
				return ret;
			}

		}

		MonoMac.CoreAnimation.CALayer[] __mt_Sublayers_var;
		public virtual CALayer[] Sublayers {
			[Export ("sublayers", ArgumentSemantic.Copy)]
			get {
				CALayer[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CALayer>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selSublayers));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CALayer>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selSublayers));
				}
				__mt_Sublayers_var = ret;
				return ret;
			}

			[Export ("setSublayers:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetSublayers, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetSublayers, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_Sublayers_var = value;
			}
		}

		public virtual CATransform3D SublayerTransform {
			[Export ("sublayerTransform")]
			get {
				CATransform3D ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.CATransform3D_objc_msgSend_stret (out ret, this.Handle, selSublayerTransform);
				} else {
					MonoMac.ObjCRuntime.Messaging.CATransform3D_objc_msgSendSuper_stret (out ret, this.SuperHandle, selSublayerTransform);
				}
				return ret;
			}

			[Export ("setSublayerTransform:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_CATransform3D (this.Handle, selSetSublayerTransform, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_CATransform3D (this.SuperHandle, selSetSublayerTransform, value);
				}
			}
		}

		MonoMac.CoreAnimation.CALayer __mt_Mask_var;
		public virtual CALayer Mask {
			[Export ("mask", ArgumentSemantic.Retain)]
			get {
				CALayer ret;
				if (IsDirectBinding) {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selMask));
				} else {
					ret = (CALayer) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selMask));
				}
				__mt_Mask_var = ret;
				return ret;
			}

			[Export ("setMask:", ArgumentSemantic.Retain)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetMask, value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetMask, value.Handle);
				}
				__mt_Mask_var = value;
			}
		}

		public virtual bool MasksToBounds {
			[Export ("masksToBounds")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selMasksToBounds);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selMasksToBounds);
				}
			}

			[Export ("setMasksToBounds:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetMasksToBounds, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetMasksToBounds, value);
				}
			}
		}

		public virtual CGImage Contents {
			[Export ("contents", ArgumentSemantic.Retain)]
			get {
				if (IsDirectBinding) {
					return new CGImage (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selContents));
				} else {
					return new CGImage (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selContents));
				}
			}

			[Export ("setContents:", ArgumentSemantic.Retain)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetContents, value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetContents, value.Handle);
				}
			}
		}

		public virtual System.Drawing.RectangleF ContentsRect {
			[Export ("contentsRect")]
			get {
				System.Drawing.RectangleF ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret (out ret, this.Handle, selContentsRect);
				} else {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret (out ret, this.SuperHandle, selContentsRect);
				}
				return ret;
			}

			[Export ("setContentsRect:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selSetContentsRect, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selSetContentsRect, value);
				}
			}
		}

		public virtual string ContentsGravity {
			[Export ("contentsGravity", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selContentsGravity));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selContentsGravity));
				}
			}

			[Export ("setContentsGravity:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetContentsGravity, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetContentsGravity, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual System.Drawing.RectangleF ContentsCenter {
			[Export ("contentsCenter")]
			get {
				System.Drawing.RectangleF ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret (out ret, this.Handle, selContentsCenter);
				} else {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret (out ret, this.SuperHandle, selContentsCenter);
				}
				return ret;
			}

			[Export ("setContentsCenter:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_RectangleF (this.Handle, selSetContentsCenter, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_RectangleF (this.SuperHandle, selSetContentsCenter, value);
				}
			}
		}

		public virtual string MinificationFilter {
			[Export ("minificationFilter", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selMinificationFilter));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selMinificationFilter));
				}
			}

			[Export ("setMinificationFilter:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetMinificationFilter, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetMinificationFilter, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual string MagnificationFilter {
			[Export ("magnificationFilter", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selMagnificationFilter));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selMagnificationFilter));
				}
			}

			[Export ("setMagnificationFilter:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetMagnificationFilter, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetMagnificationFilter, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual bool Opaque {
			[Export ("isOpaque")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsOpaque);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsOpaque);
				}
			}

			[Export ("setOpaque:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetOpaque, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetOpaque, value);
				}
			}
		}

		public virtual bool NeedsDisplay {
			[Export ("needsDisplay")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selNeedsDisplay);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selNeedsDisplay);
				}
			}

		}

		public virtual bool NeedsDisplayOnBoundsChange {
			[Export ("needsDisplayOnBoundsChange")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selNeedsDisplayOnBoundsChange);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selNeedsDisplayOnBoundsChange);
				}
			}

			[Export ("setNeedsDisplayOnBoundsChange:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetNeedsDisplayOnBoundsChange, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetNeedsDisplayOnBoundsChange, value);
				}
			}
		}

		public virtual CGColor BackgroundColor {
			[Export ("backgroundColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selBackgroundColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selBackgroundColor));
				}
			}

			[Export ("setBackgroundColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetBackgroundColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetBackgroundColor, value.handle);
				}
			}
		}

		public virtual float CornerRadius {
			[Export ("cornerRadius")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selCornerRadius);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selCornerRadius);
				}
			}

			[Export ("setCornerRadius:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetCornerRadius, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetCornerRadius, value);
				}
			}
		}

		public virtual float BorderWidth {
			[Export ("borderWidth")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selBorderWidth);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selBorderWidth);
				}
			}

			[Export ("setBorderWidth:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetBorderWidth, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetBorderWidth, value);
				}
			}
		}

		public virtual CGColor BorderColor {
			[Export ("borderColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selBorderColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selBorderColor));
				}
			}

			[Export ("setBorderColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetBorderColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetBorderColor, value.handle);
				}
			}
		}

		public virtual float Opacity {
			[Export ("opacity")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selOpacity);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selOpacity);
				}
			}

			[Export ("setOpacity:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetOpacity, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetOpacity, value);
				}
			}
		}

		public virtual CAEdgeAntialiasingMask EdgeAntialiasingMask {
			[Export ("edgeAntialiasingMask")]
			get {
				if (IsDirectBinding) {
					return (CAEdgeAntialiasingMask) MonoMac.ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, selEdgeAntialiasingMask);
				} else {
					return (CAEdgeAntialiasingMask) MonoMac.ObjCRuntime.Messaging.int_objc_msgSendSuper (this.SuperHandle, selEdgeAntialiasingMask);
				}
			}

			[Export ("setEdgeAntialiasingMask:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, selSetEdgeAntialiasingMask, (int)value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, selSetEdgeAntialiasingMask, (int)value);
				}
			}
		}

		MonoMac.CoreAnimation.CAAction[] __mt_Actions_var;
		public virtual CAAction[] Actions {
			[Export ("actions", ArgumentSemantic.Copy)]
			get {
				CAAction[] ret;
				if (IsDirectBinding) {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CAAction>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selActions));
				} else {
					ret = NSArray.ArrayFromHandle<MonoMac.CoreAnimation.CAAction>(MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selActions));
				}
				__mt_Actions_var = ret;
				return ret;
			}

			[Export ("setActions:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsa_value = NSArray.FromNSObjects (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetActions, nsa_value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetActions, nsa_value.Handle);
				}
							nsa_value.Dispose ();

				__mt_Actions_var = value;
			}
		}

		public virtual System.String[] AnimationKeys {
			[Export ("animationKeys")]
			get {
				if (IsDirectBinding) {
					return NSArray.StringArrayFromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selAnimationKeys));
				} else {
					return NSArray.StringArrayFromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selAnimationKeys));
				}
			}

		}

		public virtual string Name {
			[Export ("name", ArgumentSemantic.Copy)]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selName));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selName));
				}
			}

			[Export ("setName:", ArgumentSemantic.Copy)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetName, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetName, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		MonoMac.Foundation.NSObject __mt_WeakDelegate_var;
		public virtual NSObject WeakDelegate {
			[Export ("delegate", ArgumentSemantic.Assign)]
			get {
				NSObject ret;
				if (IsDirectBinding) {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selDelegate));
				} else {
					ret = (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selDelegate));
				}
				__mt_WeakDelegate_var = ret;
				return ret;
			}

			[Export ("setDelegate:", ArgumentSemantic.Assign)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetDelegate, value == null ? IntPtr.Zero : value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetDelegate, value == null ? IntPtr.Zero : value.Handle);
				}
				__mt_WeakDelegate_var = value;
			}
		}

		public CALayerDelegate Delegate {
			get { return WeakDelegate as CALayerDelegate; }
			set { WeakDelegate = value; }
		}

		public virtual System.Double BeginTime {
			[Export ("beginTime")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (this.Handle, selBeginTime);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper (this.SuperHandle, selBeginTime);
				}
			}

			[Export ("setBeginTime:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (this.Handle, selSetBeginTime, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Double (this.SuperHandle, selSetBeginTime, value);
				}
			}
		}

		public virtual System.Double Duration {
			[Export ("duration")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (this.Handle, selDuration);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper (this.SuperHandle, selDuration);
				}
			}

			[Export ("setDuration:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (this.Handle, selSetDuration, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Double (this.SuperHandle, selSetDuration, value);
				}
			}
		}

		public virtual float Speed {
			[Export ("speed")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selSpeed);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selSpeed);
				}
			}

			[Export ("setSpeed:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetSpeed, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetSpeed, value);
				}
			}
		}

		public virtual System.Double TimeOffset {
			[Export ("timeOffset")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (this.Handle, selTimeOffset);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper (this.SuperHandle, selTimeOffset);
				}
			}

			[Export ("setTimeOffset:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (this.Handle, selSetTimeOffset, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Double (this.SuperHandle, selSetTimeOffset, value);
				}
			}
		}

		public virtual float RepeatCount {
			[Export ("repeatCount")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selRepeatCount);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selRepeatCount);
				}
			}

			[Export ("setRepeatCount:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetRepeatCount, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetRepeatCount, value);
				}
			}
		}

		public virtual System.Double RepeatDuration {
			[Export ("repeatDuration")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (this.Handle, selRepeatDuration);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSendSuper (this.SuperHandle, selRepeatDuration);
				}
			}

			[Export ("setRepeatDuration:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (this.Handle, selSetRepeatDuration, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Double (this.SuperHandle, selSetRepeatDuration, value);
				}
			}
		}

		public virtual bool AutoReverses {
			[Export ("autoreverses")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selAutoreverses);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selAutoreverses);
				}
			}

			[Export ("setAutoreverses:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetAutoreverses, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetAutoreverses, value);
				}
			}
		}

		public virtual string FillMode {
			[Export ("fillMode")]
			get {
				if (IsDirectBinding) {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selFillMode));
				} else {
					return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selFillMode));
				}
			}

			[Export ("setFillMode:")]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			var nsvalue = new NSString (value);

				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetFillMode, nsvalue.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetFillMode, nsvalue.Handle);
				}
							nsvalue.Dispose ();

			}
		}

		public virtual CGColor ShadowColor {
			[Export ("shadowColor")]
			get {
				if (IsDirectBinding) {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selShadowColor));
				} else {
					return new CGColor (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selShadowColor));
				}
			}

			[Export ("setShadowColor:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetShadowColor, value.handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetShadowColor, value.handle);
				}
			}
		}

		public virtual System.Drawing.SizeF ShadowOffset {
			[Export ("shadowOffset")]
			get {
				System.Drawing.SizeF ret;
				if (IsDirectBinding) {
					ret = MonoMac.ObjCRuntime.Messaging.SizeF_objc_msgSend (this.Handle, selShadowOffset);
				} else {
					ret = MonoMac.ObjCRuntime.Messaging.SizeF_objc_msgSendSuper (this.SuperHandle, selShadowOffset);
				}
				return ret;
			}

			[Export ("setShadowOffset:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_SizeF (this.Handle, selSetShadowOffset, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_SizeF (this.SuperHandle, selSetShadowOffset, value);
				}
			}
		}

		public virtual float ShadowOpacity {
			[Export ("shadowOpacity")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selShadowOpacity);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selShadowOpacity);
				}
			}

			[Export ("setShadowOpacity:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetShadowOpacity, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetShadowOpacity, value);
				}
			}
		}

		public virtual float ShadowRadius {
			[Export ("shadowRadius")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSend (this.Handle, selShadowRadius);
				} else {
					return MonoMac.ObjCRuntime.Messaging.float_objc_msgSendSuper (this.SuperHandle, selShadowRadius);
				}
			}

			[Export ("setShadowRadius:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_float (this.Handle, selSetShadowRadius, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_float (this.SuperHandle, selSetShadowRadius, value);
				}
			}
		}

		public virtual System.Drawing.RectangleF VisibleRect {
			[Export ("visibleRect")]
			get {
				System.Drawing.RectangleF ret;
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSend_stret (out ret, this.Handle, selVisibleRect);
				} else {
					MonoMac.ObjCRuntime.Messaging.RectangleF_objc_msgSendSuper_stret (out ret, this.SuperHandle, selVisibleRect);
				}
				return ret;
			}

		}

		public virtual CAAutoresizingMask AutoresizinMask {
			[Export ("autoresizingMask")]
			get {
				if (IsDirectBinding) {
					return (CAAutoresizingMask) MonoMac.ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, selAutoresizingMask);
				} else {
					return (CAAutoresizingMask) MonoMac.ObjCRuntime.Messaging.int_objc_msgSendSuper (this.SuperHandle, selAutoresizingMask);
				}
			}

			[Export ("setAutoresizingMask:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, selSetAutoresizingMask, (int)value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, selSetAutoresizingMask, (int)value);
				}
			}
		}

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _Transition;
		public static NSString Transition {
			get {
				if (_Transition == null)
					_Transition = Dlfcn.GetStringConstant (libraryHandle, "kCATransition");
				return _Transition;
			}
		}
		static NSString _GravityCenter;
		public static NSString GravityCenter {
			get {
				if (_GravityCenter == null)
					_GravityCenter = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityCenter");
				return _GravityCenter;
			}
		}
		static NSString _GravityTop;
		public static NSString GravityTop {
			get {
				if (_GravityTop == null)
					_GravityTop = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityTop");
				return _GravityTop;
			}
		}
		static NSString _GravityBottom;
		public static NSString GravityBottom {
			get {
				if (_GravityBottom == null)
					_GravityBottom = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityBottom");
				return _GravityBottom;
			}
		}
		static NSString _GravityLeft;
		public static NSString GravityLeft {
			get {
				if (_GravityLeft == null)
					_GravityLeft = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityLeft");
				return _GravityLeft;
			}
		}
		static NSString _GravityRight;
		public static NSString GravityRight {
			get {
				if (_GravityRight == null)
					_GravityRight = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityRight");
				return _GravityRight;
			}
		}
		static NSString _GravityTopLeft;
		public static NSString GravityTopLeft {
			get {
				if (_GravityTopLeft == null)
					_GravityTopLeft = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityTopLeft");
				return _GravityTopLeft;
			}
		}
		static NSString _GravityTopRight;
		public static NSString GravityTopRight {
			get {
				if (_GravityTopRight == null)
					_GravityTopRight = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityTopRight");
				return _GravityTopRight;
			}
		}
		static NSString _GravityBottomLeft;
		public static NSString GravityBottomLeft {
			get {
				if (_GravityBottomLeft == null)
					_GravityBottomLeft = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityBottomLeft");
				return _GravityBottomLeft;
			}
		}
		static NSString _GravityBottomRight;
		public static NSString GravityBottomRight {
			get {
				if (_GravityBottomRight == null)
					_GravityBottomRight = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityBottomRight");
				return _GravityBottomRight;
			}
		}
		static NSString _GravityResize;
		public static NSString GravityResize {
			get {
				if (_GravityResize == null)
					_GravityResize = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityResize");
				return _GravityResize;
			}
		}
		static NSString _GravityResizeAspect;
		public static NSString GravityResizeAspect {
			get {
				if (_GravityResizeAspect == null)
					_GravityResizeAspect = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityResizeAspect");
				return _GravityResizeAspect;
			}
		}
		static NSString _GravityResizeAspectFill;
		public static NSString GravityResizeAspectFill {
			get {
				if (_GravityResizeAspectFill == null)
					_GravityResizeAspectFill = Dlfcn.GetStringConstant (libraryHandle, "kCAGravityResizeAspectFill");
				return _GravityResizeAspectFill;
			}
		}
		static NSString _FilterNearest;
		public static NSString FilterNearest {
			get {
				if (_FilterNearest == null)
					_FilterNearest = Dlfcn.GetStringConstant (libraryHandle, "kCAFilterNearest");
				return _FilterNearest;
			}
		}
		static NSString _FilterLinear;
		public static NSString FilterLinear {
			get {
				if (_FilterLinear == null)
					_FilterLinear = Dlfcn.GetStringConstant (libraryHandle, "kCAFilterLinear");
				return _FilterLinear;
			}
		}
		static NSString _FilterTrilinear;
		public static NSString FilterTrilinear {
			get {
				if (_FilterTrilinear == null)
					_FilterTrilinear = Dlfcn.GetStringConstant (libraryHandle, "kCAFilterTrilinear");
				return _FilterTrilinear;
			}
		}
		static NSString _OnOrderIn;
		public static NSString OnOrderIn {
			get {
				if (_OnOrderIn == null)
					_OnOrderIn = Dlfcn.GetStringConstant (libraryHandle, "kCAOnOrderIn");
				return _OnOrderIn;
			}
		}
		static NSString _OnOrderOut;
		public static NSString OnOrderOut {
			get {
				if (_OnOrderOut == null)
					_OnOrderOut = Dlfcn.GetStringConstant (libraryHandle, "kCAOnOrderOut");
				return _OnOrderOut;
			}
		}
	
	} /* class CALayer */
}
