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
		
		static void ExtractImages ()
		{
			var mdocAssembly = typeof (Node).Assembly;
			
			foreach (var res in mdocAssembly.GetManifestResourceNames ()){
				if (!res.EndsWith (".png") || res.EndsWith (".jpg"))
					continue;
				
				using (var output = File.Create (Path.Combine (MonodocDir, "mdocimages", res)))
					mdocAssembly.GetManifestResourceStream (res).CopyTo (output);
			}
		}
		
		public AppDelegate ()
		{
			MonodocDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Library/Caches/MacDoc/");
			var mdocimages = Path.Combine (MonodocDir, "mdocimages");
			MonodocBaseUrl = new NSUrl (MonodocDir);
			if (!Directory.Exists (mdocimages)){
				try {
					Directory.CreateDirectory (mdocimages);
				} catch {}
			}
				
			ExtractImages ();
			
			Root = RootTree.LoadTree (null);
			SettingsHandler.Settings.EnableEditing = false;
			SettingsHandler.Settings.preferred_font_size = 200;
			HelpSource.use_css = true;
			
		}

		public override void FinishedLaunching (NSObject notification)
		{
		}
	}
}

