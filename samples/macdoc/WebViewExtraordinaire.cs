using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.WebKit;
using MonoMac.ObjCRuntime;

namespace macdoc
{

	public enum SwipeSide
	{
		None,
		Left,
		Right
	}
	
	public class SwipeEventArgs : EventArgs
	{
		public SwipeSide Side { get; set; }
	}
	
	public partial class WebViewExtraordinaire : WebView
	{
		public event EventHandler<SwipeEventArgs> SwipeEvent;
		public event EventHandler<SwipeEventArgs> OngoingSwipeEvent;
		
		FindBarExtraordinaireController findBarController;
		NSScrollView currentScrollView;
		
		public WebViewExtraordinaire (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		[Export ("initWithCoder:")]
		public WebViewExtraordinaire (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		void Initialize ()
		{
			// The initializations we do here are Lion-specific
			if (!AppDelegate.IsOnLionOrBetter)
				return;
			findBarController = new FindBarExtraordinaireController ();
			IsSwipeEnabled = NSEvent.IsSwipeTrackingFromScrollEventsEnabled;
			FinishedLoad += (sender, e) => SetupFindBar (MainFrame.FrameView.DocumentView.EnclosingScrollView);
			findBarController.View.FindTextChanged += (sender, e) => InternalPerformFinderAction (NSTextFinderAction.SetSearchString);
			findBarController.View.CloseFindPanel += (sender, e) => InternalPerformFinderAction (NSTextFinderAction.HideFindInterface);
		}
		
		public bool IsSwipeEnabled { get; set; }
		
		public override void ScrollWheel (NSEvent theEvent)
		{
			if (!IsSwipeEnabled)
				return;
			if (theEvent.Phase != NSEventPhase.Began)
				return;
			if (theEvent.DeltaX == 0 || Math.Abs (theEvent.DeltaX) <= Math.Abs (theEvent.DeltaY) * 8)
				return;
			if (Math.Abs (theEvent.DeltaX) < 1f)
				return;
			
			FireSwipeEvent (theEvent.DeltaX > 0 ? SwipeSide.Left : SwipeSide.Right);
		}

		public override void SwipeWithEvent (NSEvent theEvent)
		{
			// We are only interested in horizontal swipe
			if (theEvent.DeltaX == 0)
				return;
			FireSwipeEvent (theEvent.DeltaX > 0 ? SwipeSide.Left : SwipeSide.Right);
		}
		
		public override bool WantsForwardedScrollEventsForAxis (NSEventGestureAxis axis)
		{
			return axis == NSEventGestureAxis.Horizontal && IsSwipeEnabled;
		}
	
		public override bool AcceptsFirstResponder ()
		{
			return AppDelegate.IsOnLionOrBetter;
		}
		
		void FireSwipeEvent (SwipeSide side)
		{
			var temp = SwipeEvent;
			if (temp != null)
				temp (this, new SwipeEventArgs { Side = side });
		}
		
		void FireOngoingSwipeEvent (SwipeSide status)
		{
			var temp = OngoingSwipeEvent;
			if (temp != null)
				temp (this, new SwipeEventArgs { Side = status });
		}
		
		public override void PerformFindPanelAction (NSObject sender)
		{
			if (!AppDelegate.IsOnLionOrBetter)
				return;

			var ctrl = sender as NSMenuItem;
			if (ctrl == null)
				return;
			
			InternalPerformFinderAction ((NSTextFinderAction)ctrl.Tag);
		}
		
		void InternalPerformFinderAction (NSTextFinderAction action)
		{
			switch (action) {
			case NSTextFinderAction.ShowFindInterface:
				if (currentScrollView != null)
					currentScrollView.FindBarVisible = true;
				findBarController.View.GrabFocus ();
				break;
			case NSTextFinderAction.HideFindInterface:
				if (currentScrollView != null)
					currentScrollView.FindBarVisible = false;
				break;
			case NSTextFinderAction.NextMatch:
			case NSTextFinderAction.SetSearchString:
				DoSearch (true);
				break;
			case NSTextFinderAction.PreviousMatch:
				DoSearch (false);
				break;
			default:
				break;
			}
		}
		
		void SetupFindBar (NSScrollView scrollView)
		{
			currentScrollView = scrollView;
			scrollView.FindBarView = findBarController.View;
			scrollView.FindBarPosition = NSScrollViewFindBarPosition.BelowContent;
		}
		
		void DoSearch (bool forward)
		{
			var findBar = findBarController.View;
			Search (findBar.FindText, forward, findBar.CaseSensitive, findBar.Wrap);
		}
	}
}

