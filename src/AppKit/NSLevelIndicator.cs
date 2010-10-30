//
// NSLevelIndicator: Support for the NSLevelIndicator class
//
// Author:
//   Pavel Sich (pavel.sich@me.com)
//

using System;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.AppKit {

	public partial class NSLevelIndicator {
		public new NSLevelIndicatorCell Cell {
			get { return (NSLevelIndicatorCell)base.Cell; }
			set { base.Cell = value; }
		}
	}
}
