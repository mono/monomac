namespace MonoMac.AppKit {
		
	public partial class NSDocument {
		public void delegate DuplicateCallback (NSDocument document, bool didDuplicate);
		static ArrayList proxies;
		
		[Register ("__NSDocumentDuplicateCallback")]
		internal class Callback : NSObject {
			DuplicateCallback callback;
			
			public Callback (DuplicateCallback callback)
			{
				this.callback = callback;
			}
			
			[Export ("document:didDuplicate:contextInfo:")]
			void SelectorCallback (NSDocument source, bool didDuplicate, IntPtr contextInfo)
			{
				callback (source, didDuplicate);
				proxies.Remove (this);
				if (proxies.Count == 0)
					proxies = null;
			}
		}
		
		public void DuplicateDocument (DuplicateCallback callback)
		{
			if (callback == null)
				_DuplicateDocument (null, null, IntPtr.Zero);
			proxy = new Callback ();
			if (proxies == null)
				proxies = new ArrayList ();
			proxies.Add (proxy);
			_DuplicateDocument (proxy, new Selector ("document:didDuplicate:contextInfo:"), IntPtr.Zero);
		}
	}
}