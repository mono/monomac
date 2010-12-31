using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using System.Runtime.InteropServices;

namespace MonoMac.AppKit {
	public partial class NSColor {

		public static NSColor FromColorSpace (NSColorSpace space, float[] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");

			int size = Marshal.SizeOf(components[0]) * components.Length;
			IntPtr pNativeFloatArray = Marshal.AllocHGlobal(size);
			Marshal.Copy(components, 0, pNativeFloatArray, components.Length);

			NSColor color = _FromColorSpace (space, pNativeFloatArray, components.Length);

			Marshal.FreeHGlobal(pNativeFloatArray);

			return color;
		}
		
		public void GetComponents(out float[] components)
		{
			int count = this.NumberOfComponents;
			float[] managedFloatArray = new float[count];
			int size = Marshal.SizeOf(managedFloatArray[0]) * count;
			IntPtr pNativeFloatArray = Marshal.AllocHGlobal(size);

			_GetComponents (pNativeFloatArray);
			Marshal.Copy(pNativeFloatArray, managedFloatArray, 0, count);
			Marshal.FreeHGlobal(pNativeFloatArray);

			components = managedFloatArray;
		}
	}
}

