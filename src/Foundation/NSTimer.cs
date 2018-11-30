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
// Copyright 2011, 2012 Xamarin Inc
//
using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSTimer {
		
		public static NSTimer CreateRepeatingScheduledTimer (TimeSpan when, Action action)
		{
			return CreateScheduledTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}

		public static NSTimer CreateRepeatingScheduledTimer (double seconds, Action action)
		{
			return CreateScheduledTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}
		
		public static NSTimer CreateScheduledTimer (TimeSpan when, Action action)
		{
			return CreateScheduledTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}

		public static NSTimer CreateScheduledTimer (double seconds, Action action)
		{
			return CreateScheduledTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}

		public static NSTimer CreateRepeatingTimer (TimeSpan when, Action action)
		{
			return CreateTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}

		public static NSTimer CreateRepeatingTimer (double seconds, Action action)
		{
			return CreateTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}
		
		public static NSTimer CreateTimer (TimeSpan when, Action action)
		{
			return CreateTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}

		public static NSTimer CreateTimer (double seconds, Action action)
		{
			return CreateTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}
		
		public NSTimer (NSDate date, TimeSpan when, Action action, System.Boolean repeats)
			: this (date, when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, repeats)
		{
		}
#if !MONOMAC
		[Obsolete ("This instance of NSTimer would be unusable. Symbol kept for binary compatibility", true)]
		public NSTimer () : base (IntPtr.Zero)
		{
		}
#endif
	}
}
