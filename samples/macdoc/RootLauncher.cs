using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Unix;

namespace macdoc
{
	public enum AuthorizationResultCode
	{
		Success = 0,
		InvalidSet              = -60001,
		InvalidRef              = -60002,
		InvalidTag              = -60003,
		InvalidPointer          = -60004,
		Denied                  = -60005,
		Canceled                = -60006,
		InteractionNotAllowed   = -60007,
		Internal                = -60008,
		ExternalizeNotAllowed	= -60009,
		InternalizeNotAllowed	= -60010,
		InvalidFlags            = -60011,
		ToolExecuteFailure      = -60031,
		ToolEnvironmentError    = -60032,
		BadAddress              = -60033,
		FileNotFound            = -60035 // Addition
	}
	
	public class RootLauncherException : ApplicationException
	{
		public RootLauncherException (string message) : base (message)
		{
		}
		
		public AuthorizationResultCode ResultCode { get; set; }
	}
	
	// You were in need of some harsh reality? You have come to the right place
	public static class RootLauncher
	{
		const string SecurityFramework = "/System/Library/Frameworks/Security.framework/Versions/Current/Security";

		public static bool IsRootEnabled (string toolPath)
		{
			var stat = new UnixFileInfo (toolPath);
			return stat.IsSetUser && stat.IsSetGroup && stat.OwnerUserId == 0;
		}
		
		public static void LaunchExternalTool (string toolPath, string[] args)
		{
			if (!File.Exists (toolPath))
				throw new RootLauncherException ("[Launcher] Error, the tool doesn't exist and can't be launched") { ResultCode = AuthorizationResultCode.FileNotFound };
			
			IntPtr authReference = IntPtr.Zero;
			AuthorizationResultCode result = AuthorizationCreate (IntPtr.Zero, IntPtr.Zero, 0, out authReference);
			if (result != AuthorizationResultCode.Success)
				throw new RootLauncherException ("[Launcher] Error while creating Auth Reference") { ResultCode = result };
			
			result = AuthorizationExecuteWithPrivileges (authReference, toolPath, 0, (args ?? Enumerable.Empty<string> ()).Concat (Enumerable.Repeat ((string)null, 1)).ToArray (), IntPtr.Zero);
			if (result != AuthorizationResultCode.Success)
				throw new RootLauncherException ("[Launcher] Error while executing") { ResultCode = result };
		}
		
		[DllImport (SecurityFramework)]
		extern static AuthorizationResultCode AuthorizationCreate (IntPtr autorizationRights, IntPtr environment, int authFlags, out IntPtr authRef);
		
		[DllImport (SecurityFramework)]
		extern static AuthorizationResultCode AuthorizationExecuteWithPrivileges (IntPtr authRef, string pathToTool, int authFlags, string[] args, IntPtr pipe);
	}
	
	public static class UrlLauncher
	{
		public static void Launch (string url)
		{
			if (string.IsNullOrEmpty (url))
				throw new ArgumentNullException (url);
			Process.Start (new ProcessStartInfo (url));
		}
	}
}
