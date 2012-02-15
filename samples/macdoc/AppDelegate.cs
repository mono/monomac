using System;
using System.Linq;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Monodoc;
using System.IO;
using System.Runtime.InteropServices;

namespace macdoc
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		static public RootTree Root;
		static public string MonodocDir;
		static public NSUrl MonodocBaseUrl;
		static MonodocDocumentController controller;
		
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
			
			// Load documentation
			Root = RootTree.LoadTree (null);
			
			var macDocPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "macdoc");
			if (!Directory.Exists (macDocPath))
				Directory.CreateDirectory (macDocPath);
			IndexUpdateManager = new IndexUpdateManager (Root.HelpSources.Cast<HelpSource> ().Select (hs => Path.Combine (hs.BaseFilePath, hs.Name + ".zip")).Where (File.Exists),
			                                             macDocPath);
			BookmarkManager = new BookmarkManager (macDocPath);
			
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
					Console.WriteLine (t.Exception);
				else if (!t.Result)
					indexManager.PerformSearchIndexCreation ();
			});
		}
		
		public static IndexUpdateManager IndexUpdateManager {
			get;
			private set;
		}
		
		public static BookmarkManager BookmarkManager {
			get;
			private set;
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
			// Received event is a list (1-based) of URL strings
			for (int i = 1; i <= evt.NumberOfItems; i++) {
				var innerDesc = evt.DescriptorAtIndex (i);
				// The next call works fine but is Lion-specific 
				//controller.OpenDocument (new NSUrl (innerDesc.StringValue), i == evt.NumberOfItems, delegate {});
				Call_OpenDocument (new NSUrl (innerDesc.StringValue), i == evt.NumberOfItems, out error);
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
		
		partial void HandlePrint (NSObject sender)
		{
			controller.CurrentDocument.PrintDocument (sender);
		}
		
		public override void WillTerminate (NSNotification notification)
		{
			BookmarkManager.SaveBookmarks ();
		}
	}
}

