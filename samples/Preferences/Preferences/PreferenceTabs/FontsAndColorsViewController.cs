using System;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Preferences
{
	public partial class FontsAndColorsViewController : NSViewController, IPreferencesTab
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public FontsAndColorsViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public FontsAndColorsViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public FontsAndColorsViewController () : base ("FontsAndColorsView", NSBundle.MainBundle)
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
				return "Fonts & Colors";
			}
		}
		
		public NSImage Icon {
			get {
				return NSImage.ImageNamed("NSFont");
			}
		}
		
		#endregion
	}
}

