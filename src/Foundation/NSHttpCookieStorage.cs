using MonoMac.ObjCRuntime;
using System;

namespace MonoMac.Foundation {
	public partial class NSHttpCookieStorage {
		public static NSString CookiesChangedNotification;
		public static NSString AcceptPolicyChangedNotification;

		static NSHttpCookieStorage ()
		{
			var handle = Libraries.Foundation.Handle;
			if (handle == IntPtr.Zero)
				return;

			CookiesChangedNotification = Dlfcn.GetStringConstant (handle, "NSHTTPCookieManagerAcceptPolicyChangedNotification");
			AcceptPolicyChangedNotification = Dlfcn.GetStringConstant (handle, "NSHTTPCookieManagerCookiesChangedNotification");
		}
	}
}