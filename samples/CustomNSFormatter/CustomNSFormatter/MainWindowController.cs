using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace CustomNSFormatter
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}
		
		#endregion
		
		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}
	}
	
	partial class CustomURIFormatter : NSFormatterDelegate
	{
		#region Constructors
		public CustomURIFormatter (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public CustomURIFormatter (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}
		#endregion
		
		public override string StringFor (NSObject value)
		{
			return value.ToString ();
		}
		
		public override bool ObjectFor (ref NSObject objectFor, string forString, bool needsErrorDescription, ref string errorDescription)
		{
			if (Uri.IsWellFormedUriString (forString, UriKind.Absolute)) {
				objectFor = new NSString (forString);
				return true;
			} else {
				errorDescription = "URI not valid.";
				return false;
			}
		}
	}
}

