using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class FindBarExtraordinaireController : MonoMac.AppKit.NSViewController
	{
		public FindBarExtraordinaireController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		[Export ("initWithCoder:")]
		public FindBarExtraordinaireController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		public FindBarExtraordinaireController () : base ("FindBarExtraordinaire", NSBundle.MainBundle)
		{
			Initialize ();
		}
		
		void Initialize ()
		{
		}
		
		public new FindBarExtraordinaire View {
			get {
				return (FindBarExtraordinaire)base.View;
			}
		}
	}
}

