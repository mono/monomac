using System;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace macdoc
{
	public class MonodocDocumentController : NSDocumentController
	{
		public MonodocDocumentController () : base ()
		{
		}
		
		// We only have one type of document so force every request to use it
		public override Class DocumentClassForType (string typeName)
		{
			return new Class (typeof (MyDocument));
		}
		
		public override string TypeForUrl (NSUrl inAbsoluteUrl, out NSError outError)
		{
			outError = null;
			return "MyDocument";
		}
		
		public MyDocument CurrentMyDocument {
			get {
				return (MyDocument)CurrentDocument;
			}
		}
	}
}

