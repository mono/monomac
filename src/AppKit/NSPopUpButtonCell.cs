namespace MonoMac.AppKit {
	public partial class NSPopUpButtonCell {
		public NSMenuItem this [int idx] {
			get {
				return ItemAt (idx);
			}
		}

		public NSMenuItem this [string title]{
			get {
				return ItemWithTitle (title); 
			}
		}
	}
}