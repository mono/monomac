using System;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Monodoc;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace macdoc
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		const string mergeToolPath = "/Developer/MonoTouch/usr/share/doc/MonoTouch/apple-doc-wizard.sh";
		
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
				
				using (var output = File.Create (image))
					mdocAssembly.GetManifestResourceStream (res).CopyTo (output);
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
			Root = RootTree.LoadTree (null);
			
			var macDocPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "macdoc");
			if (!Directory.Exists (macDocPath))
				Directory.CreateDirectory (macDocPath);
			IndexUpdateManager = new IndexUpdateManager (Root.HelpSources.Cast<HelpSource> ().Select (hs => Path.Combine (hs.BaseFilePath, hs.Name + ".zip")).Where (File.Exists),
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
			var indexManager = IndexUpdateManager;
			indexManager.CheckIndexIsFresh ().ContinueWith (t => {
				if (t.IsFaulted)
					Console.WriteLine ("Error while checking indexes: {0}", t.Exception);
				else if (!t.Result)
					indexManager.PerformSearchIndexCreation ();
				else
					indexManager.AdvertiseFreshIndex ();
			}).ContinueWith (t => Console.WriteLine ("Error while creating indexes: {0}", t.Exception), TaskContinuationOptions.OnlyOnFaulted);
			
			// Check if there is a MonoTouch documentation installed and launch accordingly
			if (Root.HelpSources.Cast<HelpSource> ().Any (hs => hs.Name.StartsWith ("MonoTouch", StringComparison.InvariantCultureIgnoreCase))
			    && File.Exists (mergeToolPath)) {
				Task.Factory.StartNew (() => {
					AppleDocHandler.AppleDocInformation infos;
					bool mergeOutdated = false;
					bool docOutdated = AppleDocHandler.CheckAppleDocFreshness (AppleDocHandler.IosAtomFeed, out infos);
					if (!docOutdated)
						mergeOutdated = AppleDocHandler.CheckMergedDocumentationFreshness (infos);
					return Tuple.Create (docOutdated || mergeOutdated, docOutdated, mergeOutdated);
				}).ContinueWith (t => {
					Console.WriteLine ("Merged status {0}", t.Result);
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
				if (!string.IsNullOrEmpty (innerDesc.StringValue))
					Call_OpenDocument (new NSUrl (innerDesc.StringValue), true, out error);
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
			// First get the directory
			try {
				RootLauncher.LaunchExternalTool (mergeToolPath, docOutdated ? new string[] { "--force-download" } : (string[])null);
			} catch (RootLauncherException ex) {
				var alertDialog = new NSAlert {
					AlertStyle = NSAlertStyle.Critical,
					MessageText = "Documentation updater error",
					InformativeText = string.Format ("There was an error launching the documentation updater: {0}{1}{2}", ex.ResultCode.ToString (), Environment.NewLine, ex.Message)
				};
				alertDialog.RunModal ();
			}
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

