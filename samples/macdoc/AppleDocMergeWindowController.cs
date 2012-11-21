
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class AppleDocMergeWindowController : MonoMac.AppKit.NSWindowController
	{
		public AppleDocMergeWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		[Export ("initWithCoder:")]
		public AppleDocMergeWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		public AppleDocMergeWindowController () : base ("AppleDocMergeWindow")
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		public override void WindowDidLoad ()
		{
			ProgressWidget.StartAnimation (this);
		}

		public void TrackProcessTask (Task<int> task)
		{
			task.ContinueWith (t => {
				var faulted = t.IsFaulted;

				if (faulted)
					Logger.LogError ("Merger exception", t.Exception);

				BeginInvokeOnMainThread (() => Finish (faulted || t.Result > 0, faulted ? 99 : t.Result));
			});
		}

		void Finish (bool errored, int code)
		{
			ProgressWidget.Hidden = true;
			WizardButton.Hidden = false;

			if (errored) {
				WizardText.StringValue = string.Format ("There was a problem running the update (code {0}).", code);
				WizardButton.Title = "Close";
				WizardButton.Activated += CloseCallback;
			} else {
				WizardText.StringValue = "Update successful. Restart MacDoc for changes to take effect.";
				WizardButton.Title = "Restart MacDoc";
				WizardButton.Activated += RestartCallback;
			}
		}

		void CloseCallback (object sender, EventArgs e)
		{
			Close ();
		}

		void RestartCallback (object sender, EventArgs e)
		{
			AppDelegate.RestartRequested = true;
			NSApplication.SharedApplication.Terminate (this);
		}

		public new AppleDocMergeWindow Window {
			get {
				return (AppleDocMergeWindow)base.Window;
			}
		}
	}
}

