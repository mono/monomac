using System;
using System.Text;
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
			int count = this.ComponentCount;
			float[] managedFloatArray = new float[count];
			int size = Marshal.SizeOf(managedFloatArray[0]) * count;
			IntPtr pNativeFloatArray = Marshal.AllocHGlobal(size);

			_GetComponents (pNativeFloatArray);
			Marshal.Copy(pNativeFloatArray, managedFloatArray, 0, count);
			Marshal.FreeHGlobal(pNativeFloatArray);

			components = managedFloatArray;
		}

		public override string ToString ()
		{
			try {
				string name = this.ColorSpaceName;
				if (name == "NSNamedColorSpace")
					return this.LocalizedCatalogNameComponent +"/" + this.LocalizedColorNameComponent;
				if (name == "NSPatternColorSpace")
					return "Pattern Color: " + this.PatternImage.Name;
				
				StringBuilder sb = new StringBuilder (this.ColorSpace.LocalizedName);
				float[] components;
				this.GetComponents (out components);
				if (components.Length > 0)
					sb.Append ("(" + components [0]);
				for (int i = 1; i < components.Length; i++)
					sb.Append ("," + components [i]);
				sb.Append (")");
				
				return sb.ToString ();
			} catch {
				//fallback to base method if we have an unexpected condition.
				return base.ToString ();
			}
		}
	}
}

