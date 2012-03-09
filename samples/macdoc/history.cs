using MonoMac.AppKit;
using System;
using System.Collections.Generic;

namespace Monodoc {

	public abstract class PageVisit {
		public abstract void Go ();
	}
	
	delegate void SetSensitive (bool state);
	
	public class History {
		NSSegmentedCell navigation;
	
		int pos = -1;
		List<PageVisit> history = new List<PageVisit> ();
		
		public int Count {
			get { return history.Count; }
		}
		
		public bool Active {
			get {
				return active;
			}
			set {
				if (value) {
					navigation.SetEnabled (pos > 0, 0);
					navigation.SetEnabled (pos+1 == history.Count, 1);							
					active = value;
				}
			}
		}
		bool active, ignoring;
		
		public History (NSSegmentedCell navigation)
		{
			this.navigation = navigation;
			active = true;
			
			navigation.Activated += delegate(object sender, EventArgs e) {
				var control = sender as NSSegmentedControl;
				if (control.SelectedSegment == 0)
					BackClicked ();
				else
					ForwardClicked ();
			};
			navigation.SetEnabled (false, 0);
			navigation.SetEnabled (false, 1);							
		}

		internal bool BackClicked ()
		{
			if (!active || pos < 1)
				return false;
			pos--;
			PageVisit p = (PageVisit) history [pos];
			ignoring = true;
			p.Go ();
			ignoring = false;
			navigation.SetEnabled (pos > 0, 0);
			navigation.SetEnabled (true, 1);
			return true;
		}
	
		internal bool ForwardClicked ()
		{
			if (!active || pos+1 == history.Count)
				return false;
			pos++;
			var pageVisit = history [pos];
			ignoring = true;
			pageVisit.Go ();
			ignoring = false;
			navigation.SetEnabled (pos+1 < history.Count, 1);
			navigation.SetEnabled (true, 0);
			return true;
		}

		public void AppendHistory (PageVisit page)
		{
			if (ignoring)
				return;
			pos++;
			if (history.Count <= pos)
				history.Add (page);
			else
				history [pos] = page;

			navigation.SetEnabled (pos > 0, 0);
			navigation.SetEnabled (false, 1);
		}
	
		public void ActivateCurrent ()
		{
			if (pos < 0)
				return;
			var pageVisit = history [pos];
			pageVisit.Go ();
		}
	}
}
