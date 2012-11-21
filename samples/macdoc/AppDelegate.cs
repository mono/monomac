using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

using Monodoc;

namespace macdoc
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		const string MergeToolPath = "/Developer/MonoTouch/usr/share/doc/MonoTouch/apple-doc-wizard";
		
		static public RootTree Root;
		static public string MonodocDir;
		static public NSUrl MonodocBaseUrl;
		static MonodocDocumentController controller;
		static bool isOnLion = false;
		
		bool shouldOpenInitialFile = true;
		
		static void PrepareCache ()
		{
			MonodocDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Library/Caches/MacDoc/");
			var mdocimages = Path.Combine (MonodocDir, "mdocimages");
			MonodocBaseUrl = new NSUrl (MonodocDir);
			if (!Directory.Exists (mdocimages)){
				try {
					Directory.CreateDirectory (mdocimages);
				} catch {}
			}
		}
		
		static void ExtractImages ()
		{
			var mdocAssembly = typeof (Node).Assembly;
			
			foreach (var res in mdocAssembly.GetManifestResourceNames ()){
				if (!res.EndsWith (".png") || res.EndsWith (".jpg"))
					continue;
				
				var image = Path.Combine (MonodocDir, "mdocimages", res);
				if (File.Exists (image))
					continue;

				try {
					using (var output = File.Create (image))
						mdocAssembly.GetManifestResourceStream (res).CopyTo (output);
				} catch (UnauthorizedAccessException) {}
			}
		}
		
		public AppDelegate ()
		{
			PrepareCache ();
			ExtractImages ();
			controller = new MonodocDocumentController ();
			
			// Some UI feature we use rely on Lion, so special case it
			try {
				var version = new NSDictionary ("/System/Library/CoreServices/SystemVersion.plist");
				isOnLion = version.ObjectForKey (new NSString ("ProductVersion")).ToString ().StartsWith ("10.7");
			} catch {}
			
			// Load documentation
			var args = Environment.GetCommandLineArgs ();
			IEnumerable<string> extraDocs = null, extraUncompiledDocs = null;
			if (args != null && args.Length > 1) {
				var extraDirs = args.Skip (1);
				extraDocs = extraDirs
					.Where (d => d.StartsWith ("+"))
					.Select (d => d.Substring (1))
					.Where (d => Directory.Exists (d));
				extraUncompiledDocs = extraDirs
					.Where (d => d.StartsWith ("@"))
					.Select (d => d.Substring (1))
					.Where (d => Directory.Exists (d));
			}

			if (extraUncompiledDocs != null)
				foreach (var dir in extraUncompiledDocs)
					RootTree.UncompiledHelpSources.Add (dir);

			Root = RootTree.LoadTree (null);

			if (extraDocs != null)
				foreach (var dir in extraDocs)
					Root.AddSource (dir);
			
			var macDocPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "macdoc");
			if (!Directory.Exists (macDocPath))
				Directory.CreateDirectory (macDocPath);
			var helpSources = Root.HelpSources
				.Cast<HelpSource> ()
				.Where (hs => !string.IsNullOrEmpty (hs.BaseFilePath) && !string.IsNullOrEmpty (hs.Name))
				.Select (hs => Path.Combine (hs.BaseFilePath, hs.Name + ".zip"))
				.Where (File.Exists);
			IndexUpdateManager = new IndexUpdateManager (helpSources,
			                                             macDocPath);
			BookmarkManager = new BookmarkManager (macDocPath);
			AppleDocHandler = new AppleDocHandler ("/Library/Frameworks/Mono.framework/Versions/Current/etc/");
			
			// Configure the documentation rendering.
			SettingsHandler.Settings.EnableEditing = false;
			SettingsHandler.Settings.preferred_font_size = 200;
			HelpSource.use_css = true;
		}
		
		public override void FinishedLaunching (NSObject notification)
		{
			// Check if we are loaded with a search term and load a document for it
			var args = Environment.GetCommandLineArgs ();
			NSError error;
			var searchArgIdx = Array.IndexOf<string> (args, "--search");
			if (searchArgIdx != -1 && args.Length > searchArgIdx + 1 && !string.IsNullOrEmpty (args [searchArgIdx + 1])) {
				var document = controller.OpenUntitledDocument (true, out error);
				if (document != null)
					((MyDocument)document).LoadWithSearch (args[searchArgIdx + 1]);
			}

			var indexManager = IndexUpdateManager;
			indexManager.CheckIndexIsFresh ().ContinueWith (t => {
				if (t.IsFaulted)
					Logger.LogError ("Error while checking indexes", t.Exception);
				else if (!t.Result)
					indexManager.PerformSearchIndexCreation ();
				else
					indexManager.AdvertiseFreshIndex ();
			}).ContinueWith (t => Logger.LogError ("Error while creating indexes", t.Exception), TaskContinuationOptions.OnlyOnFaulted);
			
			// Check if there is a MonoTouch documentation installed and launch accordingly
			if (Root.HelpSources.Cast<HelpSource> ().Any (hs => hs != null && hs.Name != null && hs.Name.StartsWith ("MonoTouch", StringComparison.InvariantCultureIgnoreCase))
			    && File.Exists (MergeToolPath)) {
				Task.Factory.StartNew (() => {
					AppleDocHandler.AppleDocInformation infos;
					bool mergeOutdated = false;
					bool docOutdated = AppleDocHandler.CheckAppleDocFreshness (AppleDocHandler.IosAtomFeed, out infos);
					if (!docOutdated)
						mergeOutdated = AppleDocHandler.CheckMergedDocumentationFreshness (infos);
					return Tuple.Create (docOutdated || mergeOutdated, docOutdated, mergeOutdated);
				}).ContinueWith (t => {
					Logger.Log ("Merged status {0}", t.Result);
					if (!t.Result.Item1)
						return;
					BeginInvokeOnMainThread (() => LaunchDocumentationUpdate (t.Result.Item2, t.Result.Item3));
				});
			}
		}
		
		public static IndexUpdateManager IndexUpdateManager {
			get;
			private set;
		}
		
		public static BookmarkManager BookmarkManager {
			get;
			private set;
		}
		
		public static AppleDocHandler AppleDocHandler {
			get;
			private set;
		}
		
		public static bool IsOnLion {
			get {
				return isOnLion;
			}
		}

		public static bool RestartRequested {
			get;
			set;
		}
		
		public override void WillFinishLaunching (NSNotification notification)
		{
			var selector = new MonoMac.ObjCRuntime.Selector ("handleGetURLEvent:withReplyEvent:");
			NSAppleEventManager.SharedAppleEventManager.SetEventHandler (this,
			                                                             selector,
			                                                             AEEventClass.Internet,
			                                                             AEEventID.GetUrl);
		}
		
		[Export ("handleGetURLEvent:withReplyEvent:")]
		public void HandleGetURLEvent (NSAppleEventDescriptor evt, NSAppleEventDescriptor replyEvt)
		{
			NSError error;
			shouldOpenInitialFile = evt.NumberOfItems == 0;
			
			// Received event is a list (1-based) of URL strings
			for (int i = 1; i <= evt.NumberOfItems; i++) {
				var innerDesc = evt.DescriptorAtIndex (i);
				// The next call works fine but is Lion-specific 
				// controller.OpenDocument (new NSUrl (innerDesc.StringValue), i == evt.NumberOfItems, delegate {});
				if (!string.IsNullOrEmpty (innerDesc.StringValue)) {
					NSUrl url = new NSUrl (innerDesc.StringValue);
					Call_OpenDocument (url, true, out error);
				}
			}
		}
		
		// If the application was launched with an url, we don't open a default window
		public override bool ApplicationShouldOpenUntitledFile (NSApplication sender)
		{
			return shouldOpenInitialFile;
		}
		
		// Prevent new document from being created when already launched
		public override bool ApplicationShouldHandleReopen (NSApplication sender, bool hasVisibleWindows)
		{
			return !hasVisibleWindows;
		}
				
		partial void HandlePrint (NSObject sender)
		{
			controller.CurrentDocument.PrintDocument (sender);
		}
		
		partial void HandleFind (NSMenuItem sender)
		{
			controller.CurrentMyDocument.MainWebView.PerformFindPanelAction (sender);
		}
		
		partial void HandleSearch (NSObject sender)
		{
			var searchField = controller.CurrentMyDocument.WindowForSheet.Toolbar.VisibleItems.Last ().View;
			controller.CurrentDocument.WindowForSheet.MakeFirstResponder (searchField);
		}
		
		public override void WillTerminate (NSNotification notification)
		{
			BookmarkManager.SaveBookmarks ();
			// Relaunch ourselves if it was requested
			if (RestartRequested)
				NSWorkspace.SharedWorkspace.LaunchApp (NSBundle.MainBundle.BundleIdentifier,
				                                       NSWorkspaceLaunchOptions.NewInstance | NSWorkspaceLaunchOptions.Async,
				                                       NSAppleEventDescriptor.NullDescriptor,
				                                       IntPtr.Zero);
		}
		
		void LaunchDocumentationUpdate (bool docOutdated, bool mergeOutdated)
		{
			var infoDialog = new NSAlert {
				AlertStyle = NSAlertStyle.Informational,
				MessageText = "Documentation update available",
				InformativeText = "We have detected your MonoTouch documentation can be upgraded with Apple documentation."
					+ Environment.NewLine
					+ Environment.NewLine
					+ "Would you like to update the documentation now? You can continue to browse the documentation while the update is performed."
			};
			
			infoDialog.AddButton ("Update now");
			infoDialog.AddButton ("Remind me later");
			var dialogResult = infoDialog.RunModal ();
			// If Cancel was clicked, just return
			if (dialogResult == (int)NSAlertButtonReturn.Second)
				return;
			
			// Launching AppleDocWizard as root
			var mergerTask = Task.Factory.StartNew (() => {
				// If the script has its setuid bit on and user as root, then we launch it directly otherwise we first restore it
				if (!RootLauncher.IsRootEnabled (MergeToolPath)) {
					RootLauncher.LaunchExternalTool (MergeToolPath, docOutdated ? new string[] { "--self-repair" } : (string[])null);
					// No good way to know when the process will finish, so wait a bit. Not ideal but since this is an unlikely codepath, shouldn't matter.
					System.Threading.Thread.Sleep (1000);
				}
				return ProcessUtils.StartProcess (new System.Diagnostics.ProcessStartInfo (MergeToolPath, "--force-download"), null, null, CancellationToken.None);
			}).Unwrap ();

			var mergeController = new AppleDocMergeWindowController ();
			mergeController.TrackProcessTask (mergerTask);
			mergeController.ShowWindow (this);
			mergeController.Window.Center ();
		}
		
		// We use a working OpenDocument method that doesn't return anything because of MonoMac bug#3380
		public void Call_OpenDocument (NSUrl absoluteUrl, bool displayDocument, out NSError outError)
		{
			outError = null;
			if (absoluteUrl == null)
				throw new ArgumentNullException ("absoluteUrl");
			IntPtr outErrorPtr = Marshal.AllocHGlobal(4);
			Marshal.WriteInt32(outErrorPtr, 0);

			MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_bool_IntPtr (controller.Handle, selOpenDocumentWithContentsOfURLDisplayError_, absoluteUrl.Handle, displayDocument, outErrorPtr);
		}
		
		IntPtr selOpenDocumentWithContentsOfURLDisplayError_  = new Selector ("openDocumentWithContentsOfURL:display:error:").Handle;
	}
}

