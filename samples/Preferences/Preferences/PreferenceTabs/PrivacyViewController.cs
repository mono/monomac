using System;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Preferences
{
	public partial class PrivacyViewController : NSViewController, IPreferencesTab
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public PrivacyViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public PrivacyViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public PrivacyViewController () : base ("PrivacyView", NSBundle.MainBundle)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}


		
		#endregion

		#region IPreferencesTab implementation
		
		public string Name {
			get {
				return "Privacy";
			}
		}
		
		public NSImage Icon {
			get {
				return NSImage.ImageNamed("NSUser");
			}
		}
		
		#endregion
	}
}

