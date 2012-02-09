using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Monodoc;
using System.IO;

namespace macdoc
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		static public RootTree Root;
		static public string MonodocDir;
		static public NSUrl MonodocBaseUrl;
		
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
			
			// Load documentation
			Root = RootTree.LoadTree (null);
			
			// Configure the documentation rendering.
			SettingsHandler.Settings.EnableEditing = false;
			SettingsHandler.Settings.preferred_font_size = 200;
			HelpSource.use_css = true;
			
		}
		
		public override void FinishedLaunching (NSObject notification)
		{
		}
	}
}

