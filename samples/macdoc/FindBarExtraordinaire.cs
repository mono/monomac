using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class FindBarExtraordinaire : MonoMac.AppKit.NSView
	{
		public event EventHandler FindTextChanged;
		public event EventHandler CloseFindPanel;
		
		public FindBarExtraordinaire (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		[Export ("initWithCoder:")]
		public FindBarExtraordinaire (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		void Initialize ()
		{
		}
		
		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			NSColor.WindowBackground.Set ();
			NSBezierPath.FillRect (dirtyRect);
		}
		
		partial void StartSearch (NSObject sender)
		{
			var temp = FindTextChanged;
			if (temp != null)
				temp (this, EventArgs.Empty);
		}
		
		partial void CloseFind (NSObject sender)
		{
			var temp = CloseFindPanel;
			if (temp != null)
				temp (this, EventArgs.Empty);
		}
		
		public void GrabFocus ()
		{
			Window.MakeFirstResponder (searchField);
		}
		
		public string FindText {
			get {
				return searchField.StringValue;
			}
		}
		
		public bool Wrap {
			get {
				return wrapButton.State == NSCellStateValue.On;
			}
		}
		
		public bool CaseSensitive {
			get {
				return caseSensitiveButton.State == NSCellStateValue.Off;
			}
		}
	}
}

