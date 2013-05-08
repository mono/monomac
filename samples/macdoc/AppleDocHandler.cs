using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Collections.Generic;

namespace macdoc
{
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
			"/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/Documentation/DocSets",
			"/Applications/Xcode.app/Contents/Developer/Documentation/DocSets/",
			"/Library/Developer/Shared/Documentation/DocSets/",
			"/Developer/Platforms/iPhoneOS.platform/Developer/Documentation/DocSets/"
		};
		const string MonodocLibPath = "/Library/Frameworks/Mono.framework/External/monodoc/";
		const string MonoTouchLibPath = "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll";

		public const string Ios5AtomFeed = "https://developer.apple.com/rss/com.apple.adc.documentation.AppleiPhone5_0.atom";
		public const string Ios6AtomFeed = "https://developer.apple.com/rss/com.apple.adc.documentation.AppleiPhone6.0.atom";
		public const string MacLionAtomFeed = "http://developer.apple.com/rss/com.apple.adc.documentation.AppleLion.atom";
		public const string MacMountainLionFeed = "https://developer.apple.com/rss/com.apple.adc.documentation.AppleOSX10_8.atom";

		readonly XNamespace docsetNamespace = "http://developer.apple.com/rss/docset_extensions";
		readonly XNamespace atomNamespace = "http://www.w3.org/2005/Atom";
		readonly string baseApplicationPath;

		Dictionary<string, XDocument> appleFeeds = new Dictionary<string, XDocument> ();
		
		public AppleDocHandler (string baseApplicationPath)
		{
			this.baseApplicationPath = baseApplicationPath;
		}
		
		// We load the atom field that contains a timeline of the modifications down to documentation by Apple
		XDocument LoadAppleFeed (string feedUrl)
		{
			XDocument appleFeed;
			if (appleFeeds.TryGetValue (feedUrl, out appleFeed))
				return appleFeed;

			WebClient wc = new WebClient ();
			var feed = wc.DownloadString (feedUrl);
			return appleFeeds[feedUrl] = XDocument.Parse (feed);
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
			Logger.Log ("Installed doc version {0}, compared to remote {1}", installedVersion.ToString (), infos.Version.ToString ());
			return installedVersion >= infos.Version;
		}

		// atom feed is one of the Apple documentation feed, iOS and Lion are given in const form above
		// returns true if the documentation was updated, false otherwise. The progressDelegate parameter
		// is given the completion percentage
		public bool CheckAppleDocFreshness (string atomFeed, out AppleDocInformation infos)
		{
			Logger.Log ("Downloading Apple feed at {0}", atomFeed);
			var feed = LoadAppleFeed (atomFeed);
			infos = GetLatestAppleDocInformation (feed);
			var needRefresh = !CheckAppleDocAvailabilityAndFreshness (infos);
			
			return needRefresh;
		}
		
		public bool CheckMergedDocumentationFreshness (AppleDocInformation infos, Product product)
		{
			var statusFile = Path.Combine (baseApplicationPath, "macdoc");
			if (!Directory.Exists (statusFile)) {
				try {
					Directory.CreateDirectory (statusFile);
				} catch {}
				return true;
			}
			statusFile = Path.Combine (statusFile, product == Product.MonoMac ? "merge.mac.status" : "merge.status");
			if (!File.Exists (statusFile))
				return true;
			if (!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("APPLEDOCWIZARD_FORCE_MERGE")))
				return true;
			
			var mergedVersion = CloneFillWithZeros (new Version (File.ReadAllText (statusFile)));
			Logger.Log ("Comparing merged {0} with downloaded {1}", mergedVersion.ToString (), infos.Version.ToString ());
			return mergedVersion != infos.Version;
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

	}
}
