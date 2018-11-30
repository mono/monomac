// 
// CALayer.cs: support for CALayer
//
// Authors:
//   Geoff Norton.
//     
// Copyright 2009-2010 Novell, Inc
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

using MonoMac.Foundation; 
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;

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

	public partial class CALayer {
		const string selInitWithLayer = "initWithLayer:";

		[Export ("initWithLayer:")]
		public CALayer (CALayer other)
		{
			if (this.GetType () == typeof (CALayer)){
				Messaging.intptr_objc_msgSend_intptr (Handle, Selector.GetHandle (selInitWithLayer), other.Handle);
			} else {
				Messaging.intptr_objc_msgSendSuper_intptr (SuperHandle, Selector.GetHandle (selInitWithLayer), other.Handle);
				Clone (other);
			}
		}

		public virtual void Clone (CALayer other)
		{
			// Subclasses must copy any instance values that they care from other
		}

		[Obsolete ("Use BeginTime instead")]
		public double CFTimeInterval {
			get { return BeginTime; }
			set { BeginTime = value; }
		}
		
		[Obsolete ("Use ConvertRectFromLayer instead")]
		public CGRect ConvertRectfromLayer (CGRect rect, CALayer layer)
		{
			return ConvertRectFromLayer (rect, layer);
		}
	}

#if !MONOMAC
	public partial class CADisplayLink {
		NSActionDispatcher dispatcher;

		public static CADisplayLink Create (Action action)
		{
			var dispatcher = new NSActionDispatcher (action);
			var rv = Create (dispatcher, NSActionDispatcher.Selector);
			rv.dispatcher = dispatcher;
			return rv;
		}
	}
#endif

	public partial class CAAnimation {
		[Obsolete ("Use BeginTime instead")]
		public double CFTimeInterval {
			get { return BeginTime; }
			set { BeginTime = value; }
		}
	}
}


