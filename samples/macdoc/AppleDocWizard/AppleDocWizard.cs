using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace macdoc
{
	public partial class AppleDocWizard : MonoMac.AppKit.NSWindow
	{
		ProcessStage currentStage = ProcessStage.Initializing;
		
		public AppleDocWizard (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public AppleDocWizard (NSCoder coder) : base (coder)
		{
		}
		
		public void PostInitialize ()
		{
			
		}
		
		public CancellationTokenSource CancellationSource {
			get;
			set;
		}
		
		public void UpdateProgress (AppleDocEventArgs e)
		{
			switch (e.Stage) {
			case ProcessStage.GettingManifest:
				currentStage = ProcessStage.GettingManifest;
				progressIndicator.Indeterminate = true;
				progressIndicator.StartAnimation (this);
				stageLabel.StringValue = "Getting Apple Documentation feed";
				extraStageInfoLabel.Hidden = true;
				break;
			case ProcessStage.Downloading:
				if (currentStage == ProcessStage.Downloading) {
					progressIndicator.DoubleValue = e.Percentage;
					extraStageInfoLabel.StringValue = e.Percentage + " %";
				} else {
					currentStage = ProcessStage.Downloading;
					progressIndicator.Indeterminate = false;
					progressIndicator.StartAnimation (this);
					stageLabel.StringValue = "Downloading Apple documentation";
					extraStageInfoLabel.Hidden = false;
				}
				break;
			case ProcessStage.Extracting:
				if (currentStage == ProcessStage.Extracting)
					extraStageInfoLabel.StringValue = e.CurrentFile ?? "(none)";
				else {
					currentStage = ProcessStage.Extracting;
					progressIndicator.Indeterminate = true;
					progressIndicator.StartAnimation (this);
					stageLabel.StringValue = "Extracting Apple documentation";
					extraStageInfoLabel.Hidden = false;
					extraStageInfoLabel.StringValue = string.Empty;
				}
				break;
			case ProcessStage.Merging:
				if (currentStage == ProcessStage.Merging)
					extraStageInfoLabel.StringValue = e.CurrentFile;
				else {
					currentStage = ProcessStage.Merging;
					stageLabel.StringValue = "Merging MonoTouch documentation with Apple documentation";
					progressIndicator.Indeterminate = true;
					progressIndicator.StartAnimation (this);
					extraStageInfoLabel.Hidden = false;
					extraStageInfoLabel.StringValue = "Preparing merge";
				}
				break;
			case ProcessStage.Finished:
				currentStage = ProcessStage.Finished;
				progressIndicator.StopAnimation (this);
				break;
			default:
				break;
			}
		}
		
		partial void CancelClicked (MonoMac.AppKit.NSButton sender)
		{
			CancellationSource.Cancel ();
		}
	}
}

