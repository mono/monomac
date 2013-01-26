using System;
using System.Collections.Generic;

using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AppKit {
		
	public partial class NSDocument {
		public delegate void DuplicateCallback (NSDocument document, bool didDuplicate);
		static List<Callback> proxies;
		
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
				if (proxies != null) {
					proxies.Remove (this);
					if (proxies.Count == 0)
						proxies = null;
				}
			}
		}
		
		public void DuplicateDocument (DuplicateCallback callback)
		{
			if (callback == null)
				_DuplicateDocument (null, null, IntPtr.Zero);
			var proxy = new Callback (callback);
			if (proxies == null)
				proxies = new List<Callback> ();
			proxies.Add (proxy);
			_DuplicateDocument (proxy, new Selector ("document:didDuplicate:contextInfo:"), IntPtr.Zero);
		}
	}
}