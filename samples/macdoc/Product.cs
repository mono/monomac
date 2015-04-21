using System;
using System.Linq;
using System.Collections.Generic;
using Monodoc;

namespace macdoc
{
	public enum Product
	{
		MonoTouch,
		MonoMac
	}

	public static class ProductUtils
	{
		public static string GetFriendlyName (Product product)
		{
			switch (product) {
			case Product.MonoTouch:
				return "Xamarin.iOS";
			case Product.MonoMac:
				return "Xamarin.Mac";
			default:
				return string.Empty;
			}
		}

		public static IEnumerable<Product> ToProducts (this IEnumerable<HelpSource> sources)
		{
			foreach (var hs in sources) {
				if (hs.Name.StartsWith ("MonoTouch", StringComparison.InvariantCultureIgnoreCase))
					yield return Product.MonoTouch;
				else if (hs.Name.StartsWith ("MonoMac", StringComparison.InvariantCultureIgnoreCase))
					yield return Product.MonoMac;
			}
		}

		public static string GetMergeToolForProduct (Product product)
		{
			switch (product) {
			case Product.MonoTouch:
				return "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/share/doc/MonoTouch/apple-doc-wizard";
			case Product.MonoMac:
				return "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/usr/share/doc/Xamarin.Mac/apple-doc-wizard";
			default:
				return null;
			}
		}
	}
}
