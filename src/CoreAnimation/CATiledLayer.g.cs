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
	[Register("CATiledLayer")]
	public partial class CATiledLayer : CALayer {
		static IntPtr selFadeDuration = Selector.GetHandle ("fadeDuration");
		static IntPtr selSetFadeDuration = Selector.GetHandle ("setFadeDuration:");
		static IntPtr selLevelsOfDetail = Selector.GetHandle ("levelsOfDetail");
		static IntPtr selSetLevelsOfDetail = Selector.GetHandle ("setLevelsOfDetail:");
		static IntPtr selLevelsOfDetailBias = Selector.GetHandle ("levelsOfDetailBias");
		static IntPtr selSetLevelsOfDetailBias = Selector.GetHandle ("setLevelsOfDetailBias:");
		static IntPtr selTileSize = Selector.GetHandle ("tileSize");
		static IntPtr selSetTileSize = Selector.GetHandle ("setTileSize:");

		static IntPtr class_ptr = Class.GetHandle ("CATiledLayer");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CATiledLayer () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CATiledLayer (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CATiledLayer (NSObjectFlag t) : base (t) {}

		public CATiledLayer (IntPtr handle) : base (handle) {}

		public static System.Double FadeDuration {
			[Export ("fadeDuration")]
			get {
				return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (class_ptr, selFadeDuration);
			}

			[Export ("setFadeDuration:")]
			set {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (class_ptr, selSetFadeDuration, value);
			}
		}

		public virtual int LevelsOfDetail {
			[Export ("levelsOfDetail")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, selLevelsOfDetail);
				} else {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSendSuper (this.SuperHandle, selLevelsOfDetail);
				}
			}

			[Export ("setLevelsOfDetail:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, selSetLevelsOfDetail, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, selSetLevelsOfDetail, value);
				}
			}
		}

		public virtual int LevelsOfDetailBias {
			[Export ("levelsOfDetailBias")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, selLevelsOfDetailBias);
				} else {
					return MonoMac.ObjCRuntime.Messaging.int_objc_msgSendSuper (this.SuperHandle, selLevelsOfDetailBias);
				}
			}

			[Export ("setLevelsOfDetailBias:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, selSetLevelsOfDetailBias, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, selSetLevelsOfDetailBias, value);
				}
			}
		}

		public virtual System.Drawing.SizeF TileSize {
			[Export ("tileSize")]
			get {
				System.Drawing.SizeF ret;
				if (IsDirectBinding) {
					ret = MonoMac.ObjCRuntime.Messaging.SizeF_objc_msgSend (this.Handle, selTileSize);
				} else {
					ret = MonoMac.ObjCRuntime.Messaging.SizeF_objc_msgSendSuper (this.SuperHandle, selTileSize);
				}
				return ret;
			}

			[Export ("setTileSize:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_SizeF (this.Handle, selSetTileSize, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_SizeF (this.SuperHandle, selSetTileSize, value);
				}
			}
		}

	
	} /* class CATiledLayer */
}
