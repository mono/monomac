// Copyright 2013 Xamarin Inc.

using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSUrlCredential {

		public NSUrlCredential (IntPtr trust, bool ignored) : base (NSObjectFlag.Empty)
		{
			if (IsDirectBinding) {
				Handle = Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle ("initWithTrust:"), trust);
			} else {
				Handle = Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle ("initWithTrust:"), trust);
			}
		}
	}
}