//
// ActionDispatcher.cs: Helper to map Target-Action to event
//
// Author:
//   Michael Hutchinson <mhutchinson@novell.com>
//
// Copyright 2010, Novell, Inc.
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
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.AppKit
{
	[Register ("__monomac_internal_ActionDispatcher")]
	internal class ActionDispatcher : NSObject
	{
		const string skey = "__monomac_internal_ActionDispatcher_activated:";
		public static Selector Action = new Selector (skey);

		public EventHandler Activated;

		[Export (skey)]
		public void OnActivated (NSObject sender)
		{
			if (Activated != null)
				Activated (sender, EventArgs.Empty);
		}

		public ActionDispatcher (EventHandler handler)
		{
			Activated = handler;
		}

		public ActionDispatcher ()
		{
		}
	}
}
