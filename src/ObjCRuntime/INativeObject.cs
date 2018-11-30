using System;

namespace MonoMac.ObjCRuntime {
	public interface INativeObject {
		IntPtr Handle { get; }
	}
}
