//
// NSGradient: Extensions to the API for NSGradient
//
// Author:
//   Regan Sarwas (find me on gmail as rsarwas)
//

using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using System.Runtime.InteropServices;

namespace MonoMac.AppKit {
	public partial class NSGradient : NSObject
	{
		static IntPtr selInitWithColorsAtLocationsColorSpace = Selector.GetHandle ("initWithColors:atLocations:colorSpace:");
		
		// The signature of this ObjC method is
		// - (id)initWithColorsAndLocations:(NSColor *)firstColor, ... NS_REQUIRES_NIL_TERMINATION;
		// where colors and locations (as CGFloats between 0.0 and 1.0) alternate until nil
		// ObjC example: 
		//    NSGradient *gradient = [[NSGradient alloc] initWithColorsAndLocations: [NSColor blackColor], 0.0,
		//                                                                           [NSColor blueColor], 0.33,
		//                                                                           [NSColor redColor], 1.0, nil];
		// which is a very un-C# thing to do.  The best correlation would be
		//   NSGradient (NSColor[] colors, float[] locations)
		// C# example:
		//    NSGradient gradient = new NSGradient(new[] {NSColor.Black, NSColor.Blue, NSColor.Red},
		//                                         new[] { 0.0f, 0.33f, 1.0f});
		// Per NSGradient.h, this initializer calls the designated initializer (below) with a
		// color space of NSColorSpace.GenericRGBColorSpace, so we will do the same.
		[Export ("initWithColorsAndLocations:")]
		public NSGradient (NSColor[] colors, float[] locations) : 
			this(colors, locations, NSColorSpace.GenericRGBColorSpace)
		{
		}
		
		// This constructor is here because the binding algorithm turns the float[] into a NSArray of floats
		// which is not what the ObjC method expects.  This code is a copy of the generator created code
		// with the IntPtr to NSArray replaced with a IntPtr to an unmanaged array of floats, which we create.
		[Export ("initWithColors:atLocations:colorSpace:")]
		public NSGradient (NSColor[] colors, float[] locations, NSColorSpace colorSpace) : base (NSObjectFlag.Empty)
		{
			if (colors == null)
				throw new ArgumentNullException ("colors");
			if (locations == null)
				throw new ArgumentNullException ("locations");
			if (colorSpace == null)
				throw new ArgumentNullException ("colorSpace");

			var nsa_colorArray = NSArray.FromNSObjects (colors);

        	int size = Marshal.SizeOf(locations[0]) * locations.Length;
        	IntPtr pNativeFloatArray = Marshal.AllocHGlobal(size);
			Marshal.Copy(locations,0, pNativeFloatArray, locations.Length);
			
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr_IntPtr (this.Handle, selInitWithColorsAtLocationsColorSpace, nsa_colorArray.Handle, pNativeFloatArray, colorSpace.Handle);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr_IntPtr_IntPtr (this.SuperHandle, selInitWithColorsAtLocationsColorSpace, nsa_colorArray.Handle, pNativeFloatArray, colorSpace.Handle);
			}
			nsa_colorArray.Dispose ();
			Marshal.FreeHGlobal(pNativeFloatArray);
		}
	}
}