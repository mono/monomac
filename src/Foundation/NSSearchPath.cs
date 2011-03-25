using System;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public static class NSSearchPath {
	
		[DllImport (Constants.FoundationLibrary)]
		extern static IntPtr  NSSearchPathForDirectoriesInDomains (NSSearchPathDirectory directory, NSSearchPathDomain domainMask, bool expandTilde);
	
		public static string[] GetDirectories (NSSearchPathDirectory directory, NSSearchPathDomain domainMask, bool expandTilde)
		{
			IntPtr values = NSSearchPathForDirectoriesInDomains(directory, domainMask, expandTilde);
			return NSArray.StringArrayFromHandle (values);
		}
	}
}
