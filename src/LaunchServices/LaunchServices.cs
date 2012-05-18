using System;
using System.Runtime.InteropServices;
using MonoMac;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac
{
	
	public static class LaunchServices
	{

		static IntPtr LaunchServicesLibraryHandle = Dlfcn.dlopen(Constants.LaunchServicesLibrary, 0);

		public const string SessionLoginItems = "kLSSharedFileListSessionLoginItems";
		static NSString _LSSharedFileListSessionLoginItems;
		public static NSString LSSharedFileListSessionLoginItems
		{
			get
			{
				if (_LSSharedFileListSessionLoginItems == null)
					_LSSharedFileListSessionLoginItems = Dlfcn.GetStringConstant(LaunchServicesLibraryHandle, SessionLoginItems);
				return _LSSharedFileListSessionLoginItems;
			}
		}

		public const string ItemLast = "kLSSharedFileListItemLast";
		static IntPtr _LSSharedFileListItemLast;
		public static IntPtr LSSharedFileListItemLast
		{
			get
			{
				if (_LSSharedFileListItemLast.Equals(IntPtr.Zero))
					_LSSharedFileListItemLast = Dlfcn.GetIntPtr(LaunchServicesLibraryHandle, ItemLast);
				return _LSSharedFileListItemLast;
			}
		}
		
		[DllImport (Constants.LaunchServicesLibrary)]
		public extern static IntPtr LSSharedFileListCreate(IntPtr inAllocator, IntPtr inListType, IntPtr listOptions);

		[DllImport (Constants.LaunchServicesLibrary)]
		public extern static IntPtr LSSharedFileListCopySnapshot(IntPtr inList, ref UInt32 outSnapshotSeed);

		[DllImport (Constants.LaunchServicesLibrary)]
		public extern static Int32 LSSharedFileListItemResolve(IntPtr inItem, UInt32 inFlags, ref IntPtr outURL, ref IntPtr outRef);

		[DllImport (Constants.LaunchServicesLibrary)]
		public extern static IntPtr LSSharedFileListInsertItemURL(IntPtr inList, IntPtr insertAfterThisItem, IntPtr inDisplayName, IntPtr inIconRef, IntPtr inURL, IntPtr inPropertiesToSet, IntPtr inPropertiesToClear);
		
	}

	public static class CoreFoundationCalls
	{

		[DllImport(Constants.CoreFoundationLibrary)]
		public extern static void CFRelease(IntPtr cf);

	}
	
}
