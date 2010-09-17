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
	[Register("CAAnimation")]
	public partial class CAAnimation : NSObject {
		static IntPtr selTimingFunction = Selector.GetHandle ("timingFunction");
		static IntPtr selSetTimingFunction = Selector.GetHandle ("setTimingFunction:");
		static IntPtr selDelegate = Selector.GetHandle ("delegate");
		static IntPtr selSetDelegate = Selector.GetHandle ("setDelegate:");
		static IntPtr selIsRemovedOnCompletion = Selector.GetHandle ("isRemovedOnCompletion");
		static IntPtr selSetRemovedOnCompletion = Selector.GetHandle ("setRemovedOnCompletion:");
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
		static IntPtr selAnimation = Selector.GetHandle ("animation");
		static IntPtr selDefaultValueForKey = Selector.GetHandle ("defaultValueForKey:");
		static IntPtr selWillChangeValueForKey = Selector.GetHandle ("willChangeValueForKey:");
		static IntPtr selDidChangeValueForKey = Selector.GetHandle ("didChangeValueForKey:");
		static IntPtr selShouldArchiveValueForKey = Selector.GetHandle ("shouldArchiveValueForKey:");

		static IntPtr class_ptr = Class.GetHandle ("CAAnimation");

		public override IntPtr ClassHandle { get { return class_ptr; } }

		[Export ("init")]
		public CAAnimation () : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}

		[Export ("initWithCoder:")]
		public CAAnimation (NSCoder coder) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);
			}
		}

		public CAAnimation (NSObjectFlag t) : base (t) {}

		public CAAnimation (IntPtr handle) : base (handle) {}

		[Export ("animation")]
		public static CAAnimation CreateAnimation ()
		{
			return (CAAnimation) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, selAnimation));
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

		[Export ("willChangeValueForKey:")]
		public virtual void WillChangeValueForKey (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selWillChangeValueForKey, nskey.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selWillChangeValueForKey, nskey.Handle);
			}
						nskey.Dispose ();

		}

		[Export ("didChangeValueForKey:")]
		public virtual void DidChangeValueForKey (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selDidChangeValueForKey, nskey.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selDidChangeValueForKey, nskey.Handle);
			}
						nskey.Dispose ();

		}

		[Export ("shouldArchiveValueForKey:")]
		public virtual bool ShouldArchiveValueForKey (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = new NSString (key);

			bool ret;
			if (IsDirectBinding) {
				ret = MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend_IntPtr (this.Handle, selShouldArchiveValueForKey, nskey.Handle);
			} else {
				ret = MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper_IntPtr (this.SuperHandle, selShouldArchiveValueForKey, nskey.Handle);
			}
						nskey.Dispose ();

			return ret;
		}

		MonoMac.CoreAnimation.CAMediaTimingFunction __mt_TimingFunction_var;
		public virtual CAMediaTimingFunction TimingFunction {
			[Export ("timingFunction", ArgumentSemantic.Retain)]
			get {
				CAMediaTimingFunction ret;
				if (IsDirectBinding) {
					ret = (CAMediaTimingFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, selTimingFunction));
				} else {
					ret = (CAMediaTimingFunction) Runtime.GetNSObject (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, selTimingFunction));
				}
				__mt_TimingFunction_var = ret;
				return ret;
			}

			[Export ("setTimingFunction:", ArgumentSemantic.Retain)]
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetTimingFunction, value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetTimingFunction, value.Handle);
				}
				__mt_TimingFunction_var = value;
			}
		}

		public CAAnimationDelegate Delegate {
			get { return WeakDelegate as CAAnimationDelegate; }
			set { WeakDelegate = value; }
		}

		MonoMac.Foundation.NSObject __mt_WeakDelegate_var;
		public virtual NSObject WeakDelegate {
			[Export ("delegate", ArgumentSemantic.Retain)]
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

			[Export ("setDelegate:", ArgumentSemantic.Retain)]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr (this.Handle, selSetDelegate, value == null ? IntPtr.Zero : value.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr (this.SuperHandle, selSetDelegate, value == null ? IntPtr.Zero : value.Handle);
				}
				__mt_WeakDelegate_var = value;
			}
		}

		public virtual bool RemovedOnCompletion {
			[Export ("isRemovedOnCompletion")]
			get {
				if (IsDirectBinding) {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSend (this.Handle, selIsRemovedOnCompletion);
				} else {
					return MonoMac.ObjCRuntime.Messaging.Boolean_objc_msgSendSuper (this.SuperHandle, selIsRemovedOnCompletion);
				}
			}

			[Export ("setRemovedOnCompletion:")]
			set {
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_Boolean (this.Handle, selSetRemovedOnCompletion, value);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_Boolean (this.SuperHandle, selSetRemovedOnCompletion, value);
				}
			}
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

		static IntPtr libraryHandle = Dlfcn.dlopen (Constants.CoreAnimationLibrary, 0);
		static NSString _TransitionFade;
		public static NSString TransitionFade {
			get {
				if (_TransitionFade == null)
					_TransitionFade = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionFade");
				return _TransitionFade;
			}
		}
		static NSString _TransitionMoveIn;
		public static NSString TransitionMoveIn {
			get {
				if (_TransitionMoveIn == null)
					_TransitionMoveIn = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionMoveIn");
				return _TransitionMoveIn;
			}
		}
		static NSString _TransitionPush;
		public static NSString TransitionPush {
			get {
				if (_TransitionPush == null)
					_TransitionPush = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionPush");
				return _TransitionPush;
			}
		}
		static NSString _TransitionReveal;
		public static NSString TransitionReveal {
			get {
				if (_TransitionReveal == null)
					_TransitionReveal = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionReveal");
				return _TransitionReveal;
			}
		}
		static NSString _TransitionFromRight;
		public static NSString TransitionFromRight {
			get {
				if (_TransitionFromRight == null)
					_TransitionFromRight = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionFromRight");
				return _TransitionFromRight;
			}
		}
		static NSString _TransitionFromLeft;
		public static NSString TransitionFromLeft {
			get {
				if (_TransitionFromLeft == null)
					_TransitionFromLeft = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionFromLeft");
				return _TransitionFromLeft;
			}
		}
		static NSString _TransitionFromTop;
		public static NSString TransitionFromTop {
			get {
				if (_TransitionFromTop == null)
					_TransitionFromTop = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionFromTop");
				return _TransitionFromTop;
			}
		}
		static NSString _TransitionFromBottom;
		public static NSString TransitionFromBottom {
			get {
				if (_TransitionFromBottom == null)
					_TransitionFromBottom = Dlfcn.GetStringConstant (libraryHandle, "kCATransitionFromBottom");
				return _TransitionFromBottom;
			}
		}
		//
		// Events and properties from the delegate
		//

		_CAAnimationDelegate EnsureCAAnimationDelegate ()
		{
			var del = WeakDelegate;
			if (del == null || (!(del is _CAAnimationDelegate))){
				del = new _CAAnimationDelegate ();
				WeakDelegate = del;
			}
			return (_CAAnimationDelegate) del;
		}

		[Register]
		class _CAAnimationDelegate : CAAnimationDelegate { 
			public _CAAnimationDelegate () {}

			internal EventHandler animationStarted;
			[Preserve (Conditional = true)]
			public override Void AnimationStarted (CAAnimation anim)
			{
				if (animationStarted != null)
					animationStarted (anim, EventArgs.Empty);
			}

			internal EventHandler<CAAnimationStateEventArgs> animationStopped;
			[Preserve (Conditional = true)]
			public override Void AnimationStopped (CAAnimation anim, bool finished)
			{
				if (animationStopped != null)
					animationStopped (anim, new CAAnimationStateEventArgs (finished));
			}

		}
		
		public event EventHandler AnimationStarted {
			add { EnsureCAAnimationDelegate ().animationStarted += value; }
			remove { EnsureCAAnimationDelegate ().animationStarted -= value; }
		}

		public event EventHandler<CAAnimationStateEventArgs> AnimationStopped {
			add { EnsureCAAnimationDelegate ().animationStopped += value; }
			remove { EnsureCAAnimationDelegate ().animationStopped -= value; }
		}

	
	} /* class CAAnimation */
	

	//
	// EventArgs classes
	//
	public partial class CAAnimationStateEventArgs : EventArgs {
		public CAAnimationStateEventArgs (bool finished)
		{
			this.Finished = finished;
		}
		public bool Finished { get; set; }
	}

}
