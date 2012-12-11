//
// NSAlert.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2012 Xamarin Inc. (http://xamarin.com)
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
using System.Collections.Generic;

using MonoMac.Foundation;

namespace MonoMac.AppKit
{
	[Register ("__MonoMac_NSAlertDidEndDispatcher")]
	internal class NSAlertDidEndDispatcher : NSObject
	{
		static List<NSAlertDidEndDispatcher> pendingInvokes = new List<NSAlertDidEndDispatcher> ();

		const string selector = "alertDidEnd:returnCode:contextInfo:";
		public static readonly global::MonoMac.ObjCRuntime.Selector Selector =
			new global::MonoMac.ObjCRuntime.Selector (selector);

		Action<int> action;

		public NSAlertDidEndDispatcher (Action<int> action)
		{
			this.action = action;
			pendingInvokes.Add (this);
		}

		[Export (selector)]
		[Preserve (Conditional = true)]
		public void OnAlertDidEnd (NSAlert alert, int returnCode, IntPtr context)
		{
			try {
				if (action != null)
					action (returnCode);
			} finally {
				action = null;
				pendingInvokes.Remove (this);
			}
		}
	}

	public partial class NSAlert
	{
		public void BeginSheet (NSWindow window)
		{
			BeginSheet (window, null, null, IntPtr.Zero);
		}

		public void BeginSheet (NSWindow window, NSAction onEnded)
		{
			BeginSheetForResponse (window, r => {
				if (onEnded != null)
					onEnded ();
			});
		}

		public void BeginSheetForResponse (NSWindow window, Action<int> onEnded)
		{
			BeginSheet (window, new NSAlertDidEndDispatcher (onEnded), NSAlertDidEndDispatcher.Selector, IntPtr.Zero);
		}

		public int RunSheetModal (NSWindow window)
		{
			return RunSheetModal (window, NSApplication.SharedApplication);
		}

		public int RunSheetModal (NSWindow window, NSApplication application)
		{
			if (application == null)
				throw new ArgumentNullException ("application");

			// same behavior as BeginSheet with a null window
			if (window == null)
				return RunModal ();

			int returnCode = -1000;

			BeginSheetForResponse (window, r => {
				returnCode = r;
				application.StopModal ();
			});

			application.RunModalForWindow (Window);

			return returnCode;
		}
	}
}
