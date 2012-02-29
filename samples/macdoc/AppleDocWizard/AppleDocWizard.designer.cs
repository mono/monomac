// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace macdoc
{
	[Register ("AppleDocWizard")]
	partial class AppleDocWizard
	{
		[Outlet]
		MonoMac.AppKit.NSTextField extraStageInfoLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator progressIndicator { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField stageLabel { get; set; }

		[Action ("CancelClicked:")]
		partial void CancelClicked (MonoMac.AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (extraStageInfoLabel != null) {
				extraStageInfoLabel.Dispose ();
				extraStageInfoLabel = null;
			}

			if (progressIndicator != null) {
				progressIndicator.Dispose ();
				progressIndicator = null;
			}

			if (stageLabel != null) {
				stageLabel.Dispose ();
				stageLabel = null;
			}
		}
	}

	[Register ("AppleDocWizardController")]
	partial class AppleDocWizardController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
