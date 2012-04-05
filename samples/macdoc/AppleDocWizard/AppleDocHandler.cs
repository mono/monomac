using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace macdoc
{
	public enum ProcessStage
	{
		Initializing,
		GettingManifest,
		Downloading,
		Extracting,
		Merging,
		Finished
	}
	
	public class AppleDocEventArgs : EventArgs
	{
		public ProcessStage Stage { get; set; }
		public int Percentage { get; set; } // Used during Downloading stage, a value of -1 indicates the stage just started
		public string CurrentFile { get; set; } // User during Extracting/Merging stage, a value of null indicates the stage just started
	}
	
	public class AppleDocHandler
	{
		public class AppleDocInformation
		{
			public Version Version { get; set; }
			public string ID { get; set; }
			public DateTime UpdateDate { get; set; }
			public string DownloadUrl { get; set; }
		}
		
		readonly string[] searchPaths = new[] {
			"/Library/Developer/Shared/Documentation/DocSets/",
			"/Developer/Platforms/iPhoneOS.platform/Developer/Documentation/DocSets/"
		};
		const string MonodocLibPath = "/Library/Frameworks/Mono.framework/External/monodoc/";
		const string MonoTouchLibPath = "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll";

		public const string IosAtomFeed = "https://developer.apple.com/rss/com.apple.adc.documentation.AppleiPhone5_0.atom";
		public const string MacLionAtomFeed = "http://developer.apple.com/rss/com.apple.adc.documentation.AppleLion.atom";
		
		public EventHandler<AppleDocEventArgs> AppleDocProgress;
		
		readonly XNamespace docsetNamespace = "http://developer.apple.com/rss/docset_extensions";
		readonly XNamespace atomNamespace = "http://www.w3.org/2005/Atom";
		readonly string baseApplicationPath;

		XDocument appleFeed;
		
		public AppleDocHandler (string baseApplicationPath)
		{
			this.baseApplicationPath = baseApplicationPath;
		}
		
		// We load the atom field that contains a timeline of the modifications down to documentation by Apple
		XDocument LoadAppleFeed (string feedUrl)
		{
			if (appleFeed != null)
				return appleFeed;
			
			WebClient wc = new WebClient ();
			var feed = wc.DownloadString (feedUrl);
			return appleFeed = XDocument.Parse (feed);
		}

		// This method transforms the Atom XML data into a POCO for the the most recent item of the feed
		AppleDocInformation GetLatestAppleDocInformation (XDocument feed)
		{
			var latestEntry = feed.Descendants (atomNamespace + "entry").LastOrDefault ();
			if (latestEntry == null)
				return null;
			
			var infos = new AppleDocInformation () {
				Version = CloneFillWithZeros (new Version (latestEntry.Element (docsetNamespace + "version").Value)),
				ID = latestEntry.Element (docsetNamespace + "identifier").Value,
				UpdateDate = DateTime.Parse (latestEntry.Element (atomNamespace + "updated").Value),
				DownloadUrl = latestEntry.Element (atomNamespace + "link").Attribute ("href").Value
			};

			return infos;
		}

		// This method read the Info.plist available in all Apple .docset to get the version of the bundle
		Version GetAppleDocVersion (string directory)
		{
			var plist = Path.Combine (directory, "Contents", "Info.plist");
			if (!File.Exists (plist))
				return null;

			var doc = XDocument.Load (plist);
			var version = doc.Descendants ("key")
				.First (k => k.Value.Equals ("CFBundleVersion", StringComparison.Ordinal))
				.ElementsAfterSelf ()
				.First ()
				.Value;

			return CloneFillWithZeros (new Version (version));
		}

		// This method checks that an iOS documentation set is installed on the user machine
		// and also checks if it's the latest available
		bool CheckAppleDocAvailabilityAndFreshness (AppleDocInformation infos)
		{
			var path = searchPaths
				.Select (p => Path.Combine (p, infos.ID + ".docset"))
				.FirstOrDefault (p => Directory.Exists (p));

			if (path == null)
				return false;

			var installedVersion = GetAppleDocVersion (path);
			return installedVersion >= infos.Version;
		}

		// atom feed is one of the Apple documentation feed, iOS and Lion are given in const form above
		// returns true if the documentation was updated, false otherwise. The progressDelegate parameter
		// is given the completion percentage
		public bool CheckAppleDocFreshness (string atomFeed, out AppleDocInformation infos)
		{
			FireAppleDocEvent (new AppleDocEventArgs () { Stage = ProcessStage.GettingManifest });
			var feed = LoadAppleFeed (atomFeed);
			infos = GetLatestAppleDocInformation (feed);
			var needRefresh = !CheckAppleDocAvailabilityAndFreshness (infos);
			
			return needRefresh;
		}
		
		public bool CheckMergedDocumentationFreshness (AppleDocInformation infos)
		{
			var statusFile = Path.Combine (baseApplicationPath, "macdoc");
			if (!Directory.Exists (statusFile)) {
				try {
					Directory.CreateDirectory (statusFile);
				} catch {}
				return true;
			}
			statusFile = Path.Combine (statusFile, "merge.status");
			if (!File.Exists (statusFile))
				return true;
			if (!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("APPLEDOCWIZARD_FORCE_MERGE")))
				return true;
			
			var mergedVersion = CloneFillWithZeros (new Version (File.ReadAllText (statusFile)));
			
			return mergedVersion != infos.Version;
		}
		
		public void DownloadAppleDocs (AppleDocInformation infos, CancellationToken token)
		{
			if (token.IsCancellationRequested)
				return;
			
			var tempPath = Path.GetTempFileName ();
			var evt = new ManualResetEvent (false);
			var evtArgs = new AppleDocEventArgs () { Stage = ProcessStage.Downloading };
			
			WebClient client = new WebClient ();
			client.DownloadFileCompleted += (sender, e) => HandleAppleDocDownloadFinished (e, infos, tempPath, evt, token);
			client.DownloadProgressChanged += (sender, e) => { 
				if (e.ProgressPercentage - evtArgs.Percentage < 1.0)
					return;
				evtArgs.Percentage = e.ProgressPercentage;
				FireAppleDocEvent (evtArgs);
			};

			FireAppleDocEvent (new AppleDocEventArgs () { Stage = ProcessStage.Downloading, Percentage = -1 });
			client.DownloadFileAsync (new Uri (infos.DownloadUrl), tempPath);
			token.Register (() => client.CancelAsync ());
			evt.WaitOne ();
		}

		void HandleAppleDocDownloadFinished (System.ComponentModel.AsyncCompletedEventArgs e, AppleDocInformation infos, string path, ManualResetEvent evt, CancellationToken token)
		{
			try {
				if (e.Cancelled || token.IsCancellationRequested) {
					return;
				}
				FireAppleDocEvent (new AppleDocEventArgs () { Stage = ProcessStage.Extracting, CurrentFile = null });
				var evtArgs = new AppleDocEventArgs () { Stage = ProcessStage.Extracting };
				XarApi.ExtractXar (path, searchPaths.First (), token, (filepath) => { evtArgs.CurrentFile = filepath; FireAppleDocEvent (evtArgs); });
				if (token.IsCancellationRequested) {
					var extractedDocDir = Path.Combine (searchPaths.First (), infos.ID + ".docset");
					if (Directory.Exists (extractedDocDir))
						Directory.Delete (extractedDocDir, true);
				}
			} finally {
				evt.Set ();
				// Delete the .xar file
				if (File.Exists (path))
					File.Delete (path);
			}
		}
		
		public void LaunchMergeProcess (AppleDocInformation infos, string resourcePath, CancellationToken token)
		{
			if (token.IsCancellationRequested)
				return;
			
			var evtArgs = new AppleDocEventArgs () { Stage = ProcessStage.Merging };
			FireAppleDocEvent (evtArgs);
			
			var mdocArchive = MDocZipArchive.ExtractAndLoad (Path.Combine (MonodocLibPath, "MonoTouch-lib.zip"));
			var merger = new AppleDocMerger (new AppleDocMerger.Options () {
				DocBase = Path.Combine (searchPaths.First (), infos.ID + ".docset", "Contents/Resources/Documents/documentation"),
				Assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly (MonoTouchLibPath),
				BaseAssemblyNamespace = "MonoTouch",
				ImportSamples = true,
				MonodocArchive = mdocArchive,
				SamplesRepositoryPath = Path.Combine (resourcePath, "samples.zip"),
				MergingPathCallback = path => { evtArgs.CurrentFile = path; FireAppleDocEvent (evtArgs); },
				CancellationToken = token
			});
			merger.MergeDocumentation ();
			
			if (!token.IsCancellationRequested) {
				mdocArchive.CommitChanges ();
				var statusDirectory = Path.Combine (baseApplicationPath, "macdoc");
				if (!Directory.Exists (statusDirectory))
					Directory.CreateDirectory (statusDirectory);
				var statusFile = Path.Combine (statusDirectory, "merge.status");
				File.WriteAllText (statusFile, infos.Version.ToString ());
			}
			FireAppleDocEvent (new AppleDocEventArgs () { Stage = ProcessStage.Finished });
		}
		
		public void AdvertiseEarlyFinish ()
		{
			FireAppleDocEvent (new AppleDocEventArgs () { Stage = ProcessStage.Finished });
		}
		
		static Version CloneFillWithZeros (Version v)
		{
			if (v == null)
				return null;
			int major = v.Major == -1 ? 0 : v.Major;
			int minor = v.Minor == -1 ? 0 : v.Minor;
			int build = v.Build == -1 ? 0 : v.Build;
			int revision = v.Revision == -1 ? 0 : v.Revision;

			return new Version (major, minor, build, revision);
		}
		
		void FireAppleDocEvent (AppleDocEventArgs e)
		{
			var temp = AppleDocProgress;
			if (temp != null)
				temp (this, e);
		}
	}
}
