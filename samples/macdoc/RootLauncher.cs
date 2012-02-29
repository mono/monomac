using System;
using System.Runtime.InteropServices;

namespace macdoc
{
	// You were in need of some harsh reality? You have come to the right place
	public static class RootLauncher
	{
		const string SecurityFramework = "/System/Library/Frameworks/Security.framework/Versions/Current/Security";
		
		public static void LaunchExternalTool (string toolPath)
		{
			IntPtr authReference = IntPtr.Zero;
			int result = AuthorizationCreate (IntPtr.Zero, IntPtr.Zero, 0, out authReference);
			if (result != 0) {
				Console.WriteLine ("Error while creating Auth Reference: {0}", result);
				return;
			}
			AuthorizationExecuteWithPrivileges (authReference, toolPath, 0, new string[] { null }, IntPtr.Zero);
		}
		
		[DllImport (SecurityFramework)]
		extern static int AuthorizationCreate (IntPtr autorizationRights, IntPtr environment, int authFlags, out IntPtr authRef);
		
		[DllImport (SecurityFramework)]
		extern static int AuthorizationExecuteWithPrivileges (IntPtr authRef, string pathToTool, int authFlags, string[] args, IntPtr pipe);
	}
}
