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
using System.Text;

namespace macdoc
{
	public partial class AppDelegate : NSApplicationDelegate
	{
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
			
			// Some UI feature we use rely on Lion or better, so special case it
			try {
				var version = new NSDictionary ("/System/Library/CoreServices/SystemVersion.plist");
				var osxVersion = Version.Parse (version.ObjectForKey (new NSString ("ProductVersion")).ToString ());
				isOnLion = osxVersion.Major == 10 && osxVersion.Minor >= 7;
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
					RootTree.AddUncompiledSource (dir);

			Root = RootTree.LoadTree ();

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
		}

		int Run (string command)
		{
			var psi = new System.Diagnostics.ProcessStartInfo (command, " --need-update");
			var t = ProcessUtils.StartProcess (psi, null, null, CancellationToken.None);
			t.Wait ();

			try {
				return t.Result;
			} catch {
				return 1;
			}
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

			// Check if there is a MonoTouch/MonoMac documentation installed and launch accordingly
			var products = Root.HelpSources.Where (hs => hs != null && hs.Name != null).ToProducts ().Distinct ().ToArray ();
			var message = new StringBuilder ("We have detected that your documentation for the following products can be improved by merging the Apple documentation:\n");
			var toUpdate = new List<Product> ();
			foreach (var p in products) {
				bool needUpdate = false;
				var tool = ProductUtils.GetMergeToolForProduct (p);

				if (!File.Exists (tool))
					continue;

				if (Run (tool) == 0) {
					toUpdate.Add (p);
					message.AppendFormat ("{0}\n", ProductUtils.GetFriendlyName (p));
				}
			}

			if (toUpdate.Count > 0)
				LaunchDocumentationUpdate (toUpdate.ToArray (), message.ToString ());
		}
		
		public static IndexUpdateManager IndexUpdateManager {
			get;
			private set;
		}
		
		public static BookmarkManager BookmarkManager {
			get;
			private set;
		}

		public static bool IsOnLionOrBetter {
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
		
		void LaunchDocumentationUpdate (Product [] products, string informative)
		{
			var infoDialog = new NSAlert {
				AlertStyle = NSAlertStyle.Informational,
				MessageText = "Documentation update available",
				InformativeText = informative + "\n\nWarning: If you have not downloaded the documentation with Xcode, this program will download the documentation from Apple servers which can take a long time.\n\nWould you like to update the documentation now?"
			};
			
			infoDialog.AddButton ("Update now");
			infoDialog.AddButton ("Remind me later");
			var dialogResult = infoDialog.RunModal ();
			// If Cancel was clicked, just return
			if (dialogResult == (int)NSAlertButtonReturn.Second)
				return;

			var mergerTasks = products.Select (p => Task.Factory.StartNew (() => {
				var mergeToolPath = ProductUtils.GetMergeToolForProduct (p);

				var psi = new System.Diagnostics.ProcessStartInfo (mergeToolPath, null);
				return ProcessUtils.StartProcess (psi, null, null, CancellationToken.None);
			}).Unwrap ());

			// No Task.WhenAll yet
			var tcs = new TaskCompletionSource<int> ();
			Task.Factory.ContinueWhenAll (mergerTasks.ToArray (), ts => {
				var faulteds = ts.Where (t => t.IsFaulted);
				if (faulteds.Any ())
					tcs.SetException (faulteds.Select (t => t.Exception));
				else
					tcs.SetResult (ts.Select (t => t.Result).FirstOrDefault (r => r != 0));
			});

			var mergeController = new AppleDocMergeWindowController ();
			mergeController.TrackProcessTask (tcs.Task);
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

