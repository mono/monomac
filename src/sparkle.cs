//
// sparkle.cs: Definitions for the Sparkle framework - http://sparkle.andymatuschak.org/
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.AppKit;

namespace MonoMac.Sparkle {
	
	[BaseType (typeof (NSObject))]
	interface SUAppcast {
		[Export ("fetchAppcastFromURL:")]
		void FetchAppcastFromURL (NSUrl url);

		[Export ("setDelegate:")]
		void SetDelegate (NSObject delegateHandler);

		[Export ("setUserAgentString:")]
		void SetUserAgentString (string userAgentString);

		[Export ("items")]
		SUAppcastItem[] Items {[Bind("items")] get;}
	}

	[BaseType (typeof (NSObject))]
	interface SUUpdater {

		[Static]
		[Export ("updaterForBundle:")]
		SUUpdater UpdaterForBundle (NSBundle bundle);

		[Export ("setDelegate:")]
		void SetDelegate (NSObject delegateHandler);

		[Export ("checkForUpdates:")]
		void CheckForUpdates ();

		[Export ("checkForUpdatesInBackground")]
		void CheckForUpdatesInBackground ();

		[Export ("hostBundle")]
		NSBundle HostBundle { [Bind("hostBundle")] get;}

		[Export ("checkForUpdateInformation")]
		void CheckForUpdateInformation ();

		[Export ("resetUpdateCycle")]
		void ResetUpdateCycle ();

		[Export ("lastUpdateCheckDate")]
		NSDate LastUpdateCheckDate { [Bind("lastUpdateCheckDate")] get; }

		[Static]
		[Export ("sharedUpdater")]
		SUUpdater SharedUpdater { [Bind("sharedUpdater")] get;}

		[Export ("updateInProgress")]
		bool UpdateInProgress  { [Bind("updateInProgress")] get;}

		[Export ("automaticallyChecksForUpdates")]
		bool AutomaticallyChecksForUpdates { get; set; }

		[Export ("updateCheckInterval")]
		double UpdateCheckInterval { get; set; }

		[Export ("feedURL")]
		NSUrl FeedURL { get; set; }

		[Export ("sendsSystemProfile")]
		bool SendsSystemProfile { get; set; }

		[Export ("automaticallyDownloadsUpdates")]
		bool AutomaticallyDownloadsUpdates { get; set; }

	}

	[BaseType (typeof (NSObject))]
	interface SUAppcastItem {
		[Export ("dict")]
		NSDictionary Dict { [Bind("dict")] get; }

		[Export ("title")]
		string Title { [Bind("title")] get; }

		[Export ("versionString")]
		string VersionString { [Bind("versionString")] get; }

		[Export ("displayVersionString")]
		string DisplayVersionString { [Bind("displayVersionString")] get; }

		[Export ("date")]
		NSDate Date { [Bind("date")] get; }

		[Export ("itemDescription")]
		string ItemDescription { [Bind("itemDescription")] get; }

		[Export ("releaseNotesURL")]
		NSUrl ReleaseNotesURL { [Bind("releaseNotesURL")] get; }

		[Export ("fileURL")]
		NSUrl FileURL { [Bind("fileURL")] get; }

		[Export ("DSASignature")]
		string DSASignature { [Bind("DSASignature")] get; }

		[Export ("minimumSystemVersion")]
		string MinimumSystemVersion { [Bind("minimumSystemVersion")] get; }

		[Export ("propertiesDictionary")]
		NSDictionary PropertiesDictionary { [Bind("propertiesDictionary")] get; }

	}	

	[BaseType (typeof (NSObject))]
	[Model]
	interface SUVersionComparison {
		[Abstract]
		[Export ("compareVersion:toVersion:")]
		NSComparisonResult CompareVersiontoVersion (string versionA, string versionB);

	}

	/*Sparkle uses an informal delegate protocol for it's delegates as follows

	interface NSObject {
		[Export ("updaterShouldPromptForPermissionToCheckForUpdates:")]
		bool UpdaterShouldPromptForPermissionToCheckForUpdates (SUUpdater bundle);

		[Export ("updater:didFinishLoadingAppcast:")]
		void UpdaterdidFinishLoadingAppcast (SUUpdater updater, SUAppcast appcast);

		[Export ("bestValidUpdateInAppcast:forUpdater:")]
		SUAppcastItem BestValidUpdateInAppcastforUpdater (SUAppcast appcast, SUUpdater bundle);

		[Export ("updater:didFindValidUpdate:")]
		void UpdaterdidFindValidUpdate (SUUpdater updater, SUAppcastItem update);

		[Export ("updaterDidNotFindUpdate:")]
		void UpdaterDidNotFindUpdate (SUUpdater update);

		[Export ("updater:willInstallUpdate:")]
		void UpdaterwillInstallUpdate (SUUpdater updater, SUAppcastItem update);

		[Export ("updater:shouldPostponeRelaunchForUpdate:untilInvoking:")]
		bool UpdatershouldPostponeRelaunchForUpdateuntilInvoking (SUUpdater updater, SUAppcastItem update, NSInvocation invocation);

		[Export ("updaterWillRelaunchApplication:")]
		void UpdaterWillRelaunchApplication (SUUpdater updater);

		[Export ("versionComparatorForUpdater:")]
		id <SUVersionComparison> VersionComparatorForUpdater (SUUpdater updater);

		[Export ("pathToRelaunchForUpdater:")]
		string PathToRelaunchForUpdater (SUUpdater updater);

	}
	
	*/

}
