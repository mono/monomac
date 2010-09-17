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
	[Register("CATransaction")]
	public partial class CATransaction : NSObject {
		static IntPtr selAnimationDuration = Selector.GetHandle ("animationDuration");
		static IntPtr selSetAnimationDuration = Selector.GetHandle ("setAnimationDuration:");
		static IntPtr selAnimationTimingFunction = Selector.GetHandle ("animationTimingFunction");
		static IntPtr selSetAnimationTimingFunction = Selector.GetHandle ("setAnimationTimingFunction:");
		static IntPtr selDisableActions = Selector.GetHandle ("disableActions");
		static IntPtr selSetDisableActions = Selector.GetHandle ("setDisableActions:");
		static IntPtr selSetSetCompletionBlock = Selector.GetHandle ("setSetCompletionBlock::");
		static IntPtr selBegin = Selector.GetHandle ("begin");
		static IntPtr selCommit = Selector.GetHandle ("commit");
		static IntPtr selFlush = Selector.GetHandle ("flush");
		static IntPtr selLock = Selector.GetHandle ("lock");
		static IntPtr selUnlock = Selector.GetHandle ("unlock");
		static IntPtr selValueForKey = Selector.GetHandle ("valueForKey:");
		static IntPtr selSetValueForKey = Selector.GetHandle ("setValue:forKey:");

		static IntPtr class_ptr = Class.GetHandle ("CATransaction");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CATransaction () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CATransaction (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CATransaction (NSObjectFlag t) : base (t) {}

		public CATransaction (IntPtr handle) : base (handle) {}

		[Export ("begin")]
		public static void Begin ()
		{
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (class_ptr, selBegin);
		}

		[Export ("commit")]
		public static void Commit ()
		{
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (class_ptr, selCommit);
		}

		[Export ("flush")]
		public static void Flush ()
		{
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (class_ptr, selFlush);
		}

		[Export ("lock")]
		public static void Lock ()
		{
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (class_ptr, selLock);
		}

		[Export ("unlock")]
		public static void Unlock ()
		{
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend (class_ptr, selUnlock);
		}

		[Export ("valueForKey:")]
		public static NSObject ValueForKey (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return (NSObject) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selValueForKey, key.Handle));
		}

		[Export ("setValue:forKey:")]
		public static void SetValueForKey (NSObject anObject, NSString key)
		{
			if (anObject == null)
				throw new ArgumentNullException ("anObject");
			if (key == null)
				throw new ArgumentNullException ("key");
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (class_ptr, selSetValueForKey, anObject.Handle, key.Handle);
		}

		public static System.Double AnimationDuration {
			[Export ("animationDuration")]
			get {
				return MonoMac.ObjCRuntime.Messaging.Double_objc_msgSend (class_ptr, selAnimationDuration);
			}

			[Export ("setAnimationDuration:")]
			set {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Double (class_ptr, selSetAnimationDuration, value);
			}
		}

		static MonoMac.CoreAnimation.CAMediaTimingFunction __mt_AnimationTimingFunction_var_static;
		public static CAMediaTimingFunction AnimationTimingFunction {
			[Export ("animationTimingFunction")]
			get {
				CAMediaTimingFunction ret;
				ret = (CAMediaTimingFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selAnimationTimingFunction));
				__mt_AnimationTimingFunction_var_static = ret;
				return ret;
			}

			[Export ("setAnimationTimingFunction:")]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (class_ptr, selSetAnimationTimingFunction, value.Handle);
			}
		}

		public static bool DisableActions {
			[Export ("disableActions")]
			get {
				return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (class_ptr, selDisableActions);
			}

			[Export ("setDisableActions:")]
			set {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (class_ptr, selSetDisableActions, value);
			}
		}

		public unsafe static NSAction CompletionBlock {
			[Export ("setSetCompletionBlock::")]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
			BlockLiteral *block_ptr_value;
			BlockLiteral block_value;
			block_value = new BlockLiteral ();
			block_ptr_value = &block_value;
			block_value.SetupBlock (static_InnerNSAction, value);

				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (class_ptr, selSetSetCompletionBlock, (IntPtr) block_ptr_value);
							block_ptr_value->CleanupBlock ();

			}
		}

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _AnimationDurationKey;
		public static NSString AnimationDurationKey {
			get {
				if (_AnimationDurationKey == null)
					_AnimationDurationKey = Dlfcn.GetStringConstant (libraryHandle, "kCATransactionAnimationDuration");
				return _AnimationDurationKey;
			}
		}
		static NSString _DisableActionsKey;
		public static NSString DisableActionsKey {
			get {
				if (_DisableActionsKey == null)
					_DisableActionsKey = Dlfcn.GetStringConstant (libraryHandle, "kCATransactionDisableActions");
				return _DisableActionsKey;
			}
		}
		static NSString _TimingFunctionKey;
		public static NSString TimingFunctionKey {
			get {
				if (_TimingFunctionKey == null)
					_TimingFunctionKey = Dlfcn.GetStringConstant (libraryHandle, "kCATransactionAnimationTimingFunction");
				return _TimingFunctionKey;
			}
		}
		static NSString _CompletionBlockKey;
		public static NSString CompletionBlockKey {
			get {
				if (_CompletionBlockKey == null)
					_CompletionBlockKey = Dlfcn.GetStringConstant (libraryHandle, "kCATransactionCompletionBlock");
				return _CompletionBlockKey;
			}
		}
	
	internal delegate void InnerNSAction (IntPtr block);
	static InnerNSAction static_InnerNSAction = new InnerNSAction (TrampolineNSAction);
	[MonoPInvokeCallback (typeof (InnerNSAction))]
	static unsafe void TrampolineNSAction (IntPtr block) {
		var descriptor = (BlockLiteral *) block;
		var del = (MonoMac.Foundation.NSAction) (descriptor->global_handle != IntPtr.Zero ? GCHandle.FromIntPtr (descriptor->global_handle).Target : GCHandle.FromIntPtr (descriptor->local_handle).Target);
		del ();
	}
	
	} /* class CATransaction */
}
