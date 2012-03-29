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
			Canceled,
			Error,
			NotAdmin
		}
		
		const string ConfigDir = "/Library/Frameworks/Mono.framework/Versions/Current/etc/";
		const string LogFile = "/Library/Frameworks/Mono.framework/Versions/Current/var/appledocwizard.log";
		
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
			handler = new AppleDocHandler (ConfigDir);
			handler.AppleDocProgress += HandleAppleDocProgress;
		}
		
		public void VerifyFreshnessAndLaunchDocProcess ()
		{
			Task.Factory.StartNew (() => {
				if (System.Security.Principal.WindowsIdentity.GetCurrent().Name != "root") {
					ShowAlert (FinishState.NotAdmin);
					return;
				}
				
				AppleDocHandler.AppleDocInformation infos;
				var resourcePath = NSBundle.MainBundle.ResourcePath;
				
				if (handler.CheckAppleDocFreshness (AppleDocHandler.IosAtomFeed, out infos)) {
					handler.DownloadAppleDocs (infos, source.Token);
					handler.LaunchMergeProcess (infos, resourcePath, source.Token);
					ShowAlert (source.IsCancellationRequested ? FinishState.Canceled : FinishState.Processed);
				} else if (handler.CheckMergedDocumentationFreshness (infos)) {
					handler.LaunchMergeProcess (infos, resourcePath, source.Token);
					ShowAlert (source.IsCancellationRequested ? FinishState.Canceled : FinishState.Processed);
				} else {
					handler.AdvertiseEarlyFinish ();
					ShowAlert (FinishState.NothingToDo);
				}
			}).ContinueWith (t => {
				var errorLog = string.Format ("Exception occured during doc process{0}{1}", Environment.NewLine, t.Exception.ToString ());
				Console.WriteLine (errorLog);
				try {
					System.IO.File.WriteAllText (LogFile, errorLog);
				} catch {}
				ShowAlert (FinishState.Error);
			}, TaskContinuationOptions.OnlyOnFaulted);
		}
		
		void ShowAlert (FinishState finishState)
		{
			InvokeOnMainThread (() => {
				var alert = new NSAlert ();
				switch (finishState) {
				case FinishState.NothingToDo:
					alert.MessageText = "Up to date";
					alert.InformativeText = "Your MonoTouch documentation is already based on the latest Apple documentation.";
					break;
				case FinishState.Processed:
					alert.MessageText = "Success";
					alert.InformativeText = "Your MonoTouch documentation was successfully merged with the latest Apple documentation.";
					break;
				case FinishState.Canceled:
					alert.MessageText = "Cancelled";
					alert.InformativeText = "The update operation was cancelled.";
					break;
				case FinishState.Error:
					alert.MessageText = "An error occurred";
					alert.InformativeText = "A fatal error occurred during one of the merge steps. Please report it.";
					break;
				case FinishState.NotAdmin:
					alert.MessageText = "Not enough rights";
					alert.InformativeText = "You need to be an administrator to use this tool.";
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

