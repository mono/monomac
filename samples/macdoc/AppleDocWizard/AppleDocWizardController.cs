using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class AppleDocWizardController : MonoMac.AppKit.NSWindowController
	{
		enum FinishState {
			NothingToDo,
			Processed,
			Canceled
		}
		
		CancellationTokenSource source = new CancellationTokenSource ();
		AppleDocHandler handler;
		
		public AppleDocWizardController (IntPtr handle) : base (handle)
		{
		}
		
		[Export ("initWithCoder:")]
		public AppleDocWizardController (NSCoder coder) : base (coder)
		{
		}
		
		// Call to load from the XIB/NIB file
		public AppleDocWizardController () : base ("AppleDocWizard")
		{
			Window.CancellationSource = source;
			handler = new AppleDocHandler ();
			handler.AppleDocProgress += HandleAppleDocProgress;
			
			VerifyFreshnessAndLaunchDocProcess ();
		}
		
		void VerifyFreshnessAndLaunchDocProcess ()
		{
			Task.Factory.StartNew (() => {
				AppleDocHandler.AppleDocInformation infos;
				
				if (handler.CheckAppleDocFreshness (AppleDocHandler.IosAtomFeed, out infos)) {
					handler.DownloadAppleDocs (infos, source.Token);
					handler.LaunchMergeProcess (infos, source.Token);
					ShowAlert (source.IsCancellationRequested ? FinishState.Canceled : FinishState.Processed);
				} else if (handler.CheckMergedDocumentationFreshness (infos)) {
					handler.LaunchMergeProcess (infos, source.Token);
					ShowAlert (source.IsCancellationRequested ? FinishState.Canceled : FinishState.Processed);
				} else {
					handler.AdvertiseEarlyFinish ();
					ShowAlert (FinishState.NothingToDo);
				}
			});
		}
		
		void ShowAlert (FinishState finishState)
		{
			InvokeOnMainThread (() => {
				var alert = new NSAlert ();
				switch (finishState) {
				case FinishState.NothingToDo:
					alert.MessageText = "Up-to-date";
					alert.InformativeText = "Your MonoTouch documentation is already based on the latest version of the Apple documentation";
					break;
				case FinishState.Processed:
					alert.MessageText = "Success";
					alert.InformativeText = "Your MonoTouch documentation was successfully updated";
					break;
				case FinishState.Canceled:
					alert.MessageText = "Canceled";
					alert.MessageText = "The update operation was canceled";
					break;
				}
				
				alert.RunModal ();
				NSApplication.SharedApplication.Terminate (this);
			});
		}
		
		void HandleAppleDocProgress (object sender, AppleDocEventArgs e)
		{
			InvokeOnMainThread (delegate {
				Window.UpdateProgress (e);
			});
		}
		
		public new AppleDocWizard Window {
			get {
				return (AppleDocWizard)base.Window;
			}
		}
	}
}

