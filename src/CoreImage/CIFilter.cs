//
// CIFilter.cs: Extensions
//
// Authors:
//   Miguel de Icaza
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2011, 2012 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Diagnostics;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
#if !MONOMAC
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif

namespace MonoMac.CoreImage {
	public partial class CIFilter {
		internal CIFilter (string name)
			: base (CreateFilter (name))
		{
		}

		public static string [] FilterNamesInCategories (params string [] categories)
		{
			return _FilterNamesInCategories (categories);
		}

		public NSObject this [NSString key] {
			get {
				return ValueForKey (key);
			}
			set {
				SetValueForKey (value, key);
			}
		}

		internal NSObject ValueForKey (string key)
		{
			using (var nskey = new NSString (key))
				return ValueForKey (nskey);
		}

		internal void SetValue (string key, NSObject value)
		{
			using (var nskey = new NSString (key)){
				SetValueForKey (value, nskey);
			}
		}
		
		internal static IntPtr CreateFilter (string name)
		{
			using (var nsname = new NSString (name))
				return ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selFilterWithName_Handle, nsname.Handle);
		}

		// helper methods
		internal void SetFloat (string key, float value)
		{
			using (var nskey = new NSString (key))
				SetValueForKey (new NSNumber (value), nskey);
		}

		internal float GetFloat (string key)
		{
			using (var nskey = new NSString (key)){
				var v = ValueForKey (nskey);
				if (v is NSNumber)
					return (v as NSNumber).FloatValue;
				return 0;
			}
		}

		internal CIVector GetVector (string key)
		{
			return ValueForKey (key) as CIVector;
		}

		internal CIColor GetColor (string key)
		{
			return ValueForKey (key) as CIColor;
		}
		
		internal CIImage GetInputImage ()
		{
			return ValueForKey (CIFilterInputKey.Image) as CIImage;
		}

		internal void SetInputImage (CIImage value)
		{
			SetValueForKey (value, CIFilterInputKey.Image);
		}

		internal CIImage GetBackgroundImage ()
		{
			return GetImage ("inputBackgroundImage");
		}

		internal CIImage GetImage (string key)
		{
			using (var nsstr = new NSString (key))
				return ValueForKey (nsstr) as CIImage;
		}

		internal void SetBackgroundImage (CIImage value)
		{
			SetImage ("inputBackgroundImage", value);
		}

		internal void SetImage (string key, CIImage value)
		{
			using (var nsstr = new NSString (key))
				SetValueForKey (value, nsstr);
		}

		// Calls the selName selector for cases where we do not have an instance created
		static internal string GetFilterName (IntPtr filterHandle)
		{
			return NSString.FromHandle (ObjCRuntime.Messaging.IntPtr_objc_msgSend (filterHandle, selNameHandle));
		}
		
		internal static CIFilter FromName (string filterName, IntPtr handle)
		{
			switch (filterName){
			case "CIAdditionCompositing":
				return new CIAdditionCompositing (handle);
			case "CIAffineTransform":
				return new CIAffineTransform (handle);
			case "CICheckerboardGenerator":
				return new CICheckerboardGenerator (handle);
			case "CIColorBlendMode":
				return new CIColorBlendMode (handle);
			case "CIColorBurnBlendMode":
				return new CIColorBurnBlendMode (handle);
			case "CIColorControls":
				return new CIColorControls (handle);
			case "CIColorCube":
				return new CIColorCube (handle);
			case "CIColorDodgeBlendMode":
				return new CIColorDodgeBlendMode (handle);
			case "CIColorInvert":
				return new CIColorInvert (handle);
			case "CIColorMatrix":
				return new CIColorMatrix (handle);
			case "CIColorMonochrome":
				return new CIColorMonochrome (handle);
			case "CIConstantColorGenerator":
				return new CIConstantColorGenerator (handle);
			case "CICrop":
				return new CICrop (handle);
			case "CIDarkenBlendMode":
				return new CIDarkenBlendMode (handle);
			case "CIDifferenceBlendMode":
				return new CIDifferenceBlendMode (handle);
			case "CIExclusionBlendMode":
				return new CIExclusionBlendMode (handle);
			case "CIExposureAdjust":
				return new CIExposureAdjust (handle);
			case "CIFalseColor":
				return new CIFalseColor (handle);
			case "CIGammaAdjust":
				return new CIGammaAdjust (handle);
			case "CIGaussianGradient":
				return new CIGaussianGradient (handle);
			case "CIHardLightBlendMode":
				return new CIHardLightBlendMode (handle);
			case "CIHighlightShadowAdjust":
				return new CIHighlightShadowAdjust (handle);
			case "CIHueAdjust":
				return new CIHueAdjust (handle);
			case "CIHueBlendMode":
				return new CIHueBlendMode (handle);
			case "CILightenBlendMode":
				return new CILightenBlendMode (handle);
			case "CILinearGradient":
				return new CILinearGradient (handle);
			case "CILuminosityBlendMode":
				return new CILuminosityBlendMode (handle);
			case "CIMaximumCompositing":
				return new CIMaximumCompositing (handle);
			case "CIMinimumCompositing":
				return new CIMinimumCompositing (handle);
			case "CIMultiplyBlendMode":
				return new CIMultiplyBlendMode (handle);
			case "CIMultiplyCompositing":
				return new CIMultiplyCompositing (handle);
			case "CIOverlayBlendMode":
				return new CIOverlayBlendMode (handle);
			case "CIRadialGradient":
				return new CIRadialGradient (handle);
			case "CISaturationBlendMode":
				return new CISaturationBlendMode (handle);
			case "CIScreenBlendMode":
				return new CIScreenBlendMode (handle);
			case "CISepiaTone":
				return new CISepiaTone (handle);
			case "CISoftLightBlendMode":
				return new CISoftLightBlendMode (handle);
			case "CISourceAtopCompositing":
				return new CISourceAtopCompositing (handle);
			case "CISourceInCompositing":
				return new CISourceInCompositing (handle);
			case "CISourceOutCompositing":
				return new CISourceOutCompositing (handle);
			case "CISourceOverCompositing":
				return new CISourceOverCompositing (handle);
			case "CIStraightenFilter":
				return new CIStraightenFilter (handle);
			case "CIStripesGenerator":
				return new CIStripesGenerator (handle);
			case "CITemperatureAndTint":
				return new CITemperatureAndTint (handle);
			case "CIToneCurve":
				return new CIToneCurve (handle);
			case "CIVibrance":
				return new CIVibrance (handle);
			case "CIWhitePointAdjust":
				return new CIWhitePointAdjust (handle);
			case "CIFaceBalance":
				return new CIFaceBalance (handle);
			case "CIAffineClamp":
				return new CIAffineClamp (handle);
			case "CIAffineTile":
				return new CIAffineTile (handle);
			case "CIBlendWithMask":
				return new CIBlendWithMask (handle);
			case "CIBarsSwipeTransition":
				return new CIBarsSwipeTransition (handle);
			case "CICopyMachineTransition":
				return new CICopyMachineTransition (handle);
			case "CIDisintegrateWithMaskTransition":
				return new CIDisintegrateWithMaskTransition (handle);
			case "CIDissolveTransition":
				return new CIDissolveTransition (handle);
			case "CIFlashTransition":
				return new CIFlashTransition (handle);
			case "CIModTransition":
				return new CIModTransition (handle);
			case "CISwipeTransition":
				return new CISwipeTransition (handle);
			case "CIBloom":
				return new CIBloom (handle);
			case "CICircularScreen":
				return new CICircularScreen (handle);
			case "CIDotScreen":
				return new CIDotScreen (handle);
			case "CIHatchedScreen":
				return new CIHatchedScreen (handle);
			case "CILineScreen":
				return new CILineScreen (handle);
			case "CIColorMap":
				return new CIColorMap (handle);
			case "CIColorPosterize":
				return new CIColorPosterize (handle);
			case "CIEightfoldReflectedTile":
				return new CIEightfoldReflectedTile (handle);
			case "CIFourfoldReflectedTile":
				return new CIFourfoldReflectedTile (handle);
			case "CIFourfoldRotatedTile":
				return new CIFourfoldRotatedTile (handle);
			case "CIFourfoldTranslatedTile":
				return new CIFourfoldTranslatedTile (handle);
			case "CISixfoldReflectedTile":
				return new CISixfoldReflectedTile (handle);
			case "CISixfoldRotatedTile":
				return new CISixfoldRotatedTile (handle);
			case "CITwelvefoldReflectedTile":
				return new CITwelvefoldReflectedTile (handle);
			case "CIGaussianBlur":
				return new CIGaussianBlur (handle);
			case "CIGloom":
				return new CIGloom (handle);
			case "CIHoleDistortion":
				return new CIHoleDistortion (handle);
			case "CIPinchDistortion":
				return new CIPinchDistortion (handle);
			case "CITwirlDistortion":
				return new CITwirlDistortion (handle);
			case "CIVortexDistortion":
				return new CIVortexDistortion (handle);
			case "CILanczosScaleTransform":
				return new CILanczosScaleTransform (handle);
			case "CIMaskToAlpha":
				return new CIMaskToAlpha (handle);
			case "CIMaximumComponent":
				return new CIMaximumComponent (handle);
			case "CIMinimumComponent":
				return new CIMinimumComponent (handle);
			case "CIPerspectiveTile":
				return new CIPerspectiveTile (handle);
			case "CIPerspectiveTransform":
				return new CIPerspectiveTransform (handle);
			case "CIPixellate":
				return new CIPixellate (handle);
			case "CIRandomGenerator":
				return new CIRandomGenerator (handle);
			case "CISharpenLuminance":
				return new CISharpenLuminance (handle);
			case "CIStarShineGenerator":
				return new CIStarShineGenerator (handle);
			case "CIUnsharpMask":
				return new CIUnsharpMask (handle);
			case "CICircleSplashDistortion":
				return new CICircleSplashDistortion (handle);
#if MONOMAC
			case "CIDepthOfField":
				return new CIDepthOfField (handle);
			case "CIPageCurlTransition":
				return new CIPageCurlTransition (handle);
			case "CIRippleTransition":
				return new CIRippleTransition (handle);
#else
			case "CIVignette":
				return new CIVignette (handle);
			case "CILightTunnel":
				return new CILightTunnel (handle);
			case "CITriangleKaleidoscope":
				return new CITriangleKaleidoscope (handle);
#endif
			default:
				throw new NotImplementedException (String.Format ("Unknown filter type returned: `{0}', returning a default CIFilter", filterName));
			}
		}
	}

	public class CIFaceBalance : CIFilter {

		public CIFaceBalance (IntPtr handle): base (handle) {}
		
	}

	[Since (6,0)]
	public class CIAffineClamp : CIAffineFilter {
		public CIAffineClamp () : base ("CIAffineClamp") {}
		public CIAffineClamp (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIAffineTile : CIAffineFilter {
		public CIAffineTile () : base ("CIAffineTile") {}
		public CIAffineTile (IntPtr handle) : base (handle) {}
	}
	
	public class CIAffineTransform : CIAffineFilter {
		public CIAffineTransform () : base ("CIAffineTransform") {}
		public CIAffineTransform (IntPtr handle) : base (handle) {}
	}

	public abstract class CIAffineFilter : CIFilter {
		protected CIAffineFilter (string name) : base (CreateFilter (name)) {}
		protected CIAffineFilter (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
#if !MONOMAC
		public CGAffineTransform Transform {
			get {
				var val = ValueForKey ("inputTransform");
				if (val is NSValue)
					return (val as NSValue).CGAffineTransformValue;
				return new CGAffineTransform (1, 0, 0, 1, 0, 0);
			}
			set {
				SetValue ("inputTransform", NSValue.FromCGAffineTransform (value));
			}
		}
#endif
	}
	
	public class CICheckerboardGenerator : CIFilter {
		public CICheckerboardGenerator () : base (CreateFilter ("CICheckerboardGenerator")) {}
		public CICheckerboardGenerator (IntPtr handle) : base (handle) {}
	
		public CIVector Center {
			get {
				return GetVector ("inputCenter");
			}
			set {
				SetValue ("inputCenter", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}
		
		public float Sharpness {
			get {
				return GetFloat("inputSharpness");
			}
			set {
				SetFloat("inputSharpness", value);
			}
		}
		
	}
	
	public class CIColorControls : CIFilter {
		public CIColorControls () : base (CreateFilter ("CIColorControls")) {}
		public CIColorControls (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Saturation {
			get {
				return GetFloat("inputSaturation");
			}
			set {
				SetFloat("inputSaturation", value);
			}
		}
		
		public float Brightness {
			get {
				return GetFloat("inputBrightness");
			}
			set {
				SetFloat("inputBrightness", value);
			}
		}
		
		public float Contrast {
			get {
				return GetFloat("inputContrast");
			}
			set {
				SetFloat("inputContrast", value);
			}
		}
		
	}
	
	public class CIColorCube : CIFilter {
		public CIColorCube () : base (CreateFilter ("CIColorCube")) {}
		public CIColorCube (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float CubeDimension {
			get {
				return GetFloat("inputCubeDimension");
			}
			set {
				SetFloat("inputCubeDimension", value);
			}
		}
		
		public NSData CubeData {
			get {
				return ValueForKey ("inputCubeData") as NSData;
			}
			set {
				SetValue ("inputCubeData", value);
			}
		}
		
	}
	
	public class CIColorInvert : CIFilter {
		public CIColorInvert () : base (CreateFilter ("CIColorInvert")) {}
		public CIColorInvert (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
	}
	
	public class CIColorMatrix : CIFilter {
		public CIColorMatrix () : base (CreateFilter ("CIColorMatrix")) {}
		public CIColorMatrix (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIVector RVector {
			get {
				return GetVector("inputRVector");
			}
			set {
				SetValue("inputRVector", value);
			}
		}
		
		public CIVector GVector {
			get {
				return GetVector("inputGVector");
			}
			set {
				SetValue("inputGVector", value);
			}
		}
		
		public CIVector BVector {
			get {
				return GetVector("inputBVector");
			}
			set {
				SetValue("inputBVector", value);
			}
		}
		
		public CIVector AVector {
			get {
				return GetVector("inputAVector");
			}
			set {
				SetValue("inputAVector", value);
			}
		}
		
		public CIVector BiasVector {
			get {
				return GetVector("inputBiasVector");
			}
			set {
				SetValue("inputBiasVector", value);
			}
		}
		
	}
	
	public class CIColorMonochrome : CIFilter {
		public CIColorMonochrome () : base (CreateFilter ("CIColorMonochrome")) {}
		public CIColorMonochrome (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}
		
		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}
		
	}
	
	public class CIConstantColorGenerator : CIFilter {
		public CIConstantColorGenerator () : base (CreateFilter ("CIConstantColorGenerator")) {}
		public CIConstantColorGenerator (IntPtr handle) : base (handle) {}
	
		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}
		
	}
	
	public class CICrop : CIFilter {
		public CICrop () : base (CreateFilter ("CICrop")) {}
		public CICrop (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIVector Rectangle {
			get {
				return GetVector("inputRectangle");
			}
			set {
				SetValue("inputRectangle", value);
			}
		}
		
	}
	
	public class CIExposureAdjust : CIFilter {
		public CIExposureAdjust () : base (CreateFilter ("CIExposureAdjust")) {}
		public CIExposureAdjust (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float EV {
			get {
				return GetFloat("inputEV");
			}
			set {
				SetFloat("inputEV", value);
			}
		}
		
	}
	
	public class CIFalseColor : CIFilter {
		public CIFalseColor () : base (CreateFilter ("CIFalseColor")) {}
		public CIFalseColor (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
	}
	
	public class CIGammaAdjust : CIFilter {
		public CIGammaAdjust () : base (CreateFilter ("CIGammaAdjust")) {}
		public CIGammaAdjust (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Power {
			get {
				return GetFloat("inputPower");
			}
			set {
				SetFloat("inputPower", value);
			}
		}
		
	}
	
	public class CIGaussianGradient : CIFilter {
		public CIGaussianGradient () : base (CreateFilter ("CIGaussianGradient")) {}
		public CIGaussianGradient (IntPtr handle) : base (handle) {}
	
		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
		
	}
	
	public class CIHighlightShadowAdjust : CIFilter {
		public CIHighlightShadowAdjust () : base (CreateFilter ("CIHighlightShadowAdjust")) {}
		public CIHighlightShadowAdjust (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float ShadowAmount {
			get {
				return GetFloat("inputShadowAmount");
			}
			set {
				SetFloat("inputShadowAmount", value);
			}
		}
		
		public float HighlightAmount {
			get {
				return GetFloat("inputHighlightAmount");
			}
			set {
				SetFloat("inputHighlightAmount", value);
			}
		}
		
	}
	
	public class CIHueAdjust : CIFilter {
		public CIHueAdjust () : base (CreateFilter ("CIHueAdjust")) {}
		public CIHueAdjust (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
		
	}
	
	public class CILinearGradient : CIFilter {
		public CILinearGradient () : base (CreateFilter ("CILinearGradient")) {}
		public CILinearGradient (IntPtr handle) : base (handle) {}
	
		public CIVector Point0 {
			get {
				return GetVector("inputPoint0");
			}
			set {
				SetValue("inputPoint0", value);
			}
		}
		
		public CIVector Point1 {
			get {
				return GetVector("inputPoint1");
			}
			set {
				SetValue("inputPoint1", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
	}

#region Compositing filters

	[Since (6,0)]
	public class CIAdditionCompositing : CICompositingFilter {
		public CIAdditionCompositing () : base ("CIAdditionCompositing") {}
		public CIAdditionCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIMaximumCompositing : CICompositingFilter {
		public CIMaximumCompositing () : base ("CIMaximumCompositing") {}
		public CIMaximumCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIMinimumCompositing : CICompositingFilter {
		public CIMinimumCompositing () : base ("CIMinimumCompositing") {}
		public CIMinimumCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIMultiplyCompositing : CICompositingFilter {
		public CIMultiplyCompositing () : base ("CIMultiplyCompositing") {}
		public CIMultiplyCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISourceAtopCompositing : CICompositingFilter {
		public CISourceAtopCompositing () : base ("CISourceAtopCompositing") {}
		public CISourceAtopCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISourceInCompositing : CICompositingFilter {
		public CISourceInCompositing () : base ("CISourceInCompositing") {}
		public CISourceInCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISourceOutCompositing : CICompositingFilter {
		public CISourceOutCompositing () : base ("CISourceOutCompositing") {}
		public CISourceOutCompositing (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISourceOverCompositing : CICompositingFilter {
		public CISourceOverCompositing () : base ("CISourceOverCompositing") {}
		public CISourceOverCompositing (IntPtr handle) : base (handle) {}
	}

	public abstract class CICompositingFilter : CIFilter {
		protected CICompositingFilter (string name) : base (name) {}
		protected CICompositingFilter (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
	}
	
#endregion

#region Blend filters

	[Since (6,0)]
	public class CIBlendWithMask : CIBlendFilter {
		public CIBlendWithMask () : base ("CIBlendWithMask") {}
		public CIBlendWithMask (IntPtr handle) : base (handle) {}

		public CIImage Mask {
			get {
				return GetImage ("inputMaskImage");
			}
			set {
				SetImage ("inputMaskImage", value);
			}
		}
	}
	
	public class CIColorBlendMode : CIBlendFilter {
		public CIColorBlendMode () : base ("CIColorBlendMode") {}
		public CIColorBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIColorBurnBlendMode : CIBlendFilter {
		public CIColorBurnBlendMode () : base ("CIColorBurnBlendMode") {}
		public CIColorBurnBlendMode (IntPtr handle) : base (handle) {}
	}

	public class CIColorDodgeBlendMode : CIBlendFilter {
		public CIColorDodgeBlendMode () : base ("CIColorDodgeBlendMode") {}
		public CIColorDodgeBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIDarkenBlendMode : CIBlendFilter {
		public CIDarkenBlendMode () : base ("CIDarkenBlendMode") {}
		public CIDarkenBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIDifferenceBlendMode : CIBlendFilter {
		public CIDifferenceBlendMode () : base ("CIDifferenceBlendMode") {}
		public CIDifferenceBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIExclusionBlendMode : CIBlendFilter {
		public CIExclusionBlendMode () : base ("CIExclusionBlendMode") {}
		public CIExclusionBlendMode (IntPtr handle) : base (handle) {}
	}

	public class CIHardLightBlendMode : CIBlendFilter {
		public CIHardLightBlendMode () : base ("CIHardLightBlendMode") {}
		public CIHardLightBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIHueBlendMode : CIBlendFilter {
		public CIHueBlendMode () : base ("CIHueBlendMode") {}
		public CIHueBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CILightenBlendMode : CIBlendFilter {
		public CILightenBlendMode () : base ("CILightenBlendMode") {}
		public CILightenBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CILuminosityBlendMode : CIBlendFilter {
		public CILuminosityBlendMode () : base ("CILuminosityBlendMode") {}
		public CILuminosityBlendMode (IntPtr handle) : base (handle) {}
	}

	public class CIMultiplyBlendMode : CIBlendFilter {
		public CIMultiplyBlendMode () : base ("CIMultiplyBlendMode") {}
		public CIMultiplyBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIOverlayBlendMode : CIBlendFilter {
		public CIOverlayBlendMode () : base ("CIOverlayBlendMode") {}
		public CIOverlayBlendMode (IntPtr handle) : base (handle) {}
	}

	public class CISaturationBlendMode : CIBlendFilter {
		public CISaturationBlendMode () : base ("CISaturationBlendMode") {}
		public CISaturationBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CIScreenBlendMode : CIBlendFilter {
		public CIScreenBlendMode () : base ("CIScreenBlendMode") {}
		public CIScreenBlendMode (IntPtr handle) : base (handle) {}
	}
	
	public class CISoftLightBlendMode : CIBlendFilter {
		public CISoftLightBlendMode () : base ("CISoftLightBlendMode") {}
		public CISoftLightBlendMode (IntPtr handle) : base (handle) {}
	}

	public abstract class CIBlendFilter : CIFilter {
		protected CIBlendFilter (string name) : base (name) {}
		protected CIBlendFilter (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
	}

#endregion


#region Transition filters

	[Since (6,0)]
	public class CIBarsSwipeTransition : CITransitionFilter
	{
		public CIBarsSwipeTransition () : base ("CIBarsSwipeTransition") {}
		public CIBarsSwipeTransition (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}

		public float BarOffset {
			get {
				return GetFloat("inputBarOffset");
			}
			set {
				SetFloat("inputBarOffset", value);
			}
		}

		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}		
	}

	[Since (6,0)]
	public class CICopyMachineTransition : CITransitionFilter
	{
		public CICopyMachineTransition () : base ("CICopyMachineTransition") {}
		public CICopyMachineTransition (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}

		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}

		public CIVector Extent {
			get {
				return GetVector("inputExtent");
			}
			set {
				SetValue("inputExtent", value);
			}
		}

		public float Opacity {
			get {
				return GetFloat("inputOpacity");
			}
			set {
				SetFloat("inputOpacity", value);
			}
		}

		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}
	}

	[Since (6,0)]
	public class CIDisintegrateWithMaskTransition : CITransitionFilter
	{
		public CIDisintegrateWithMaskTransition () : base ("CIDisintegrateWithMaskTransition") {}
		public CIDisintegrateWithMaskTransition (IntPtr handle) : base (handle) {}

		public CIImage Mask {
			get {
				return GetImage ("inputMaskImage");
			}
			set {
				SetImage ("inputMaskImage", value);
			}
		}

		public float ShadowRadius {
			get {
				return GetFloat("inputShadowRadius");
			}
			set {
				SetFloat("inputShadowRadius", value);
			}
		}		

		public float ShadowDensity {
			get {
				return GetFloat("inputShadowDensity");
			}
			set {
				SetFloat("inputShadowDensity", value);
			}
		}

		public CIVector ShadowOffset {
			get {
				return GetVector("inputShadowOffset");
			}
			set {
				SetValue("inputShadowOffset", value);
			}
		}
	}

	[Since (6,0)]
	public class CIDissolveTransition : CITransitionFilter
	{
		public CIDissolveTransition () : base ("CIDissolveTransition") {}
		public CIDissolveTransition (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIFlashTransition : CITransitionFilter
	{
		public CIFlashTransition () : base ("CIFlashTransition") {}
		public CIFlashTransition (IntPtr handle) : base (handle) {}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}

		public CIVector Extent {
			get {
				return GetVector("inputExtent");
			}
			set {
				SetValue("inputExtent", value);
			}
		}

		public float FadeThreshold {
			get {
				return GetFloat("inputFadeThreshold");
			}
			set {
				SetFloat("inputFadeThreshold", value);
			}
		}

		public float MaxStriationRadius {
			get {
				return GetFloat("inputMaxStriationRadius");
			}
			set {
				SetFloat("inputMaxStriationRadius", value);
			}
		}

		public float MaxStriationStrength {
			get {
				return GetFloat("inputStriationStrength");
			}
			set {
				SetFloat("inputStriationStrength", value);
			}
		}

		public float MaxStriationContrast {
			get {
				return GetFloat("inputStriationContrast");
			}
			set {
				SetFloat("inputStriationContrast", value);
			}
		}
	}

	[Since (6,0)]
	public class CIModTransition : CITransitionFilter
	{
		public CIModTransition () : base ("CIModTransition") {}
		public CIModTransition (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public float Compression {
			get {
				return GetFloat("inputCompression");
			}
			set {
				SetFloat("inputCompression", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
	}

#if MONOMAC
	public class CIPageCurlTransition : CITransitionFilter
	{
		public CIPageCurlTransition () : base ("CIPageCurlTransition") {}
		public CIPageCurlTransition (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}

		public CIImage BacksideImage {
			get {
				return GetImage ("inputBacksideImage");
			}
			set {
				SetImage ("inputBacksideImage", value);
			}
		}

		public CIVector Extent {
			get {
				return GetVector("inputExtent");
			}
			set {
				SetValue("inputExtent", value);
			}
		}

		public CIImage ShadingImage {
			get {
				return GetImage ("inputShadingImage");
			}
			set {
				SetImage ("inputShadingImage", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
	}

	[Since (6,0)]
	public class CIRippleTransition : CITransitionFilter
	{
		public CIRippleTransition () : base ("CIRippleTransition") {}
		public CIRippleTransition (IntPtr handle) : base (handle) {}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public CIVector Extent {
			get {
				return GetVector("inputExtent");
			}
			set {
				SetValue("inputExtent", value);
			}
		}

		public float Scale {
			get {
				return GetFloat("inputScale");
			}
			set {
				SetFloat ("inputScale", value);
			}
		}

		public CIImage ShadingImage {
			get {
				return GetImage ("inputShadingImage");
			}
			set {
				SetImage ("inputShadingImage", value);
			}
		}

		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}
	}
#endif

	[Since (6,0)]
	public class CISwipeTransition : CITransitionFilter
	{
		public CISwipeTransition () : base ("CISwipeTransition") {}
		public CISwipeTransition (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}

		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}

		public CIVector Extent {
			get {
				return GetVector("inputExtent");
			}
			set {
				SetValue("inputExtent", value);
			}
		}

		public float Opacity {
			get {
				return GetFloat("inputOpacity");
			}
			set {
				SetFloat("inputOpacity", value);
			}
		}

		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}
	}


	public abstract class CITransitionFilter : CIFilter {
		protected CITransitionFilter (string name) : base (CreateFilter (name)) {}
		protected CITransitionFilter (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage TargetImage {
			get {
				return GetImage ("inputTargetImage");
			}
			set {
				SetImage ("inputTargetImage", value);
			}
		}

		public float Time {
			get {
				return GetFloat("inputTime");
			}
			set {
				SetFloat("inputTime", value);
			}
		}
	}


#endregion
	
	public class CIRadialGradient : CIFilter {
		public CIRadialGradient () : base (CreateFilter ("CIRadialGradient")) {}
		public CIRadialGradient (IntPtr handle) : base (handle) {}
	
		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}
		
		public float Radius0 {
			get {
				return GetFloat("inputRadius0");
			}
			set {
				SetFloat("inputRadius0", value);
			}
		}
		
		public float Radius1 {
			get {
				return GetFloat("inputRadius1");
			}
			set {
				SetFloat("inputRadius1", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
	}
	
	public class CISepiaTone : CIFilter {
		public CISepiaTone () : base (CreateFilter ("CISepiaTone")) {}
		public CISepiaTone (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}
		
	}
	
	public class CIStraightenFilter : CIFilter {
		public CIStraightenFilter () : base (CreateFilter ("CIStraightenFilter")) {}
		public CIStraightenFilter (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
		
	}
	
	public class CIStripesGenerator : CIFilter {
		public CIStripesGenerator () : base (CreateFilter ("CIStripesGenerator")) {}
		public CIStripesGenerator (IntPtr handle) : base (handle) {}
	
		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat("inputWidth", value);
			}
		}
		
		public float Sharpness {
			get {
				return GetFloat("inputSharpness");
			}
			set {
				SetFloat("inputSharpness", value);
			}
		}
		
	}
	
	public class CITemperatureAndTint : CIFilter {
		public CITemperatureAndTint () : base (CreateFilter ("CITemperatureAndTint")) {}
		public CITemperatureAndTint (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIVector Neutral {
			get {
				return GetVector("inputNeutral");
			}
			set {
				SetValue("inputNeutral", value);
			}
		}
		
		public CIVector TargetNeutral {
			get {
				return GetVector("inputTargetNeutral");
			}
			set {
				SetValue("inputTargetNeutral", value);
			}
		}
	}
	
	public class CIToneCurve : CIFilter {
		public CIToneCurve () : base (CreateFilter ("CIToneCurve")) {}
		public CIToneCurve (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		public CIVector Point0 {
			get {
				return GetVector("inputPoint0");
			}
			set {
				SetValue("inputPoint0", value);
			}
		}
		
		public CIVector Point1 {
			get {
				return GetVector("inputPoint1");
			}
			set {
				SetValue("inputPoint1", value);
			}
		}
		
		public CIVector Point2 {
			get {
				return GetVector("inputPoint2");
			}
			set {
				SetValue("inputPoint2", value);
			}
		}
		
		public CIVector Point3 {
			get {
				return GetVector("inputPoint3");
			}
			set {
				SetValue("inputPoint3", value);
			}
		}
		
		public CIVector Point4 {
			get {
				return GetVector("inputPoint4");
			}
			set {
				SetValue("inputPoint4", value);
			}
		}
		
	}
	
	public class CIVibrance : CIFilter {
		public CIVibrance () : base (CreateFilter ("CIVibrance")) {}
		public CIVibrance (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Amount {
			get {
				return GetFloat("inputAmount");
			}
			set {
				SetFloat("inputAmount", value);
			}
		}
		
	}
#if !MONOMAC
	public class CIVignette : CIFilter {
		public CIVignette () : base (CreateFilter ("CIVignette")) {}
		public CIVignette (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}
		
		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
		
	}
#endif
	public class CIWhitePointAdjust : CIFilter {
		public CIWhitePointAdjust () : base (CreateFilter ("CIWhitePointAdjust")) {}
		public CIWhitePointAdjust (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}
	}

	[Since (6,0)]
	public class CIBloom : CIFilter {
		public CIBloom () : base ("CIBloom") {}
		public CIBloom (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
	}

#region Screen filters

	[Since (6,0)]
	public class CICircularScreen : CIScreenFilter {
		public CICircularScreen () : base ("CICircularScreen") {}
		public CICircularScreen (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIDotScreen : CIScreenFilter {
		public CIDotScreen () : base ("CIDotScreen") {}
		public CIDotScreen (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
	}

	[Since (6,0)]
	public class CIHatchedScreen : CIScreenFilter {
		public CIHatchedScreen () : base ("CIHatchedScreen") {}
		public CIHatchedScreen (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
	}

	[Since (6,0)]
	public class CILineScreen : CIScreenFilter {
		public CILineScreen () : base ("CILineScreen") {}
		public CILineScreen (IntPtr handle) : base (handle) {}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
	}

	public class CIScreenFilter : CIFilter {
		protected CIScreenFilter (string name) : base (name) {}
		protected CIScreenFilter (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
	
		public CIVector Center {
			get {
				return GetVector ("inputCenter");
			}
			set {
				SetValue ("inputCenter", value);
			}
		}
		
		public float Sharpness {
			get {
				return GetFloat("inputSharpness");
			}
			set {
				SetFloat("inputSharpness", value);
			}
		}
		
		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}
	}

#endregion

	[Since (6,0)]
	public class CIColorMap : CIFilter {
		public CIColorMap () : base ("CIColorMap") {}
		public CIColorMap (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIImage GradientImage {
			get {
				return GetImage ("inputGradientImage");
			}
			set {
				SetImage ("inputGradientImage", value);
			}
		}
	}

	[Since (6,0)]
	public class CIColorPosterize : CIFilter {
		public CIColorPosterize () : base ("CIColorPosterize") {}
		public CIColorPosterize (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Levels {
			get {
				return GetFloat("inputLevels");
			}
			set {
				SetFloat ("inputLevels", value);
			}
		}
	}

#if MONOMAC
	[Since (6,0)]
	public class CIDepthOfField : CIFilter {
		public CIDepthOfField () : base ("CIDepthOfField") {}
		public CIDepthOfField (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector Point1 {
			get {
				return GetVector("inputPoint1");
			}
			set {
				SetValue("inputPoint1", value);
			}
		}
		
		public CIVector Point2 {
			get {
				return GetVector("inputPoint2");
			}
			set {
				SetValue("inputPoint2", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat ("inputRadius", value);
			}
		}

		public float Saturation {
			get {
				return GetFloat("inputSaturation");
			}
			set {
				SetFloat ("inputSaturation", value);
			}
		}

		public float UnsharpMaskIntensity {
			get {
				return GetFloat("inputUnsharpMaskIntensity");
			}
			set {
				SetFloat ("inputUnsharpMaskIntensity", value);
			}
		}

		public float UnsharpMaskRadius {
			get {
				return GetFloat("inputUnsharpMaskRadius");
			}
			set {
				SetFloat ("inputUnsharpMaskRadius", value);
			}
		}
	}

#endif

#region Tile filters

	[Since (6,0)]
	public class CIEightfoldReflectedTile : CITileFilter {
		public CIEightfoldReflectedTile () : base ("CIEightfoldReflectedTile") {}
		public CIEightfoldReflectedTile (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIFourfoldReflectedTile : CITileFilter {
		public CIFourfoldReflectedTile () : base ("CIFourfoldReflectedTile") {}
		public CIFourfoldReflectedTile (IntPtr handle) : base (handle) {}

		public float AcuteAngle {
			get {
				return GetFloat("inputAcuteAngle");
			}
			set {
				SetFloat("inputAcuteAngle", value);
			}
		}
	}

	[Since (6,0)]
	public class CIFourfoldRotatedTile : CITileFilter {
		public CIFourfoldRotatedTile () : base ("CIFourfoldRotatedTile") {}
		public CIFourfoldRotatedTile (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIFourfoldTranslatedTile : CITileFilter {
		public CIFourfoldTranslatedTile () : base ("CIFourfoldTranslatedTile") {}
		public CIFourfoldTranslatedTile (IntPtr handle) : base (handle) {}

		public float AcuteAngle {
			get {
				return GetFloat("inputAcuteAngle");
			}
			set {
				SetFloat("inputAcuteAngle", value);
			}
		}
	}

	[Since (6,0)]
	public class CIGlideReflectedTile : CITileFilter {
		public CIGlideReflectedTile () : base ("CIFourfoldTranslatedTile") {}
		public CIGlideReflectedTile (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISixfoldReflectedTile : CITileFilter {
		public CISixfoldReflectedTile () : base ("CISixfoldReflectedTile") {}
		public CISixfoldReflectedTile (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISixfoldRotatedTile : CITileFilter {
		public CISixfoldRotatedTile () : base ("CISixfoldRotatedTile") {}
		public CISixfoldRotatedTile (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CITwelvefoldReflectedTile : CITileFilter {
		public CITwelvefoldReflectedTile () : base ("CITwelvefoldReflectedTile") {}
		public CITwelvefoldReflectedTile (IntPtr handle) : base (handle) {}
	}

	public class CITileFilter : CIFilter {
		protected CITileFilter (string name) : base (name) {}
		protected CITileFilter (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat("inputWidth", value);
			}
		}		
	}	

#endregion

	[Since (6,0)]
	public class CIGaussianBlur : CIFilter {
		public CIGaussianBlur () : base ("CIGaussianBlur") {}
		public CIGaussianBlur (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
	}

	[Since (6,0)]
	public class CIGloom : CIFilter {
		public CIGloom () : base ("CIGloom") {}
		public CIGloom (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
	}

#region Distortion filters
	[Since (6,0)]
	public class CICircleSplashDistortion : CIDistortionFilter {
		public CICircleSplashDistortion () : base ("CICircleSplashDistortion") {}
		public CICircleSplashDistortion (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIHoleDistortion : CIDistortionFilter {
		public CIHoleDistortion () : base ("CIHoleDistortion") {}
		public CIHoleDistortion (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIPinchDistortion : CIDistortionFilter {
		public CIPinchDistortion () : base ("CIPinchDistortion") {}
		public CIPinchDistortion (IntPtr handle) : base (handle) {}
	
		public float Scale {
			get {
				return GetFloat("inputScale");
			}
			set {
				SetFloat("inputScale", value);
			}
		}
	}

	[Since (6,0)]
	public class CITwirlDistortion : CIDistortionFilter {
		public CITwirlDistortion () : base ("CITwirlDistortion") {}
		public CITwirlDistortion (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CIVortexDistortion : CIDistortionFilter {
		public CIVortexDistortion () : base ("CIVortexDistortion") {}
		public CIVortexDistortion (IntPtr handle) : base (handle) {}
	
		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}		
	}

	public abstract class CIDistortionFilter : CIFilter {
		protected CIDistortionFilter (string name) : base (name) {}
		protected CIDistortionFilter (IntPtr handle) : base (handle) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
	}

#endregion

	[Since (6,0)]
	public class CILanczosScaleTransform : CIFilter {
		public CILanczosScaleTransform () : base ("CILanczosScaleTransform") {}
		public CILanczosScaleTransform (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float AspectRatio {
			get {
				return GetFloat("inputAspectRatio");
			}
			set {
				SetFloat("inputAspectRatio", value);
			}
		}

		public float Scale {
			get {
				return GetFloat("inputScale");
			}
			set {
				SetFloat("inputScale", value);
			}
		}		
	}

#if !MONOMAC
	[Since (6,0)]
	public class CILightTunnel : CIFilter {
		public CILightTunnel () : base ("CILightTunnel") {}
		public CILightTunnel (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}

		public float Rotation {
			get {
				return GetFloat("inputRotation");
			}
			set {
				SetFloat("inputRotation", value);
			}
		}
	}
#endif

	[Since (6,0)]
	public class CIMaskToAlpha : CIFilter {
		public CIMaskToAlpha () : base ("CIMaskToAlpha") {}
		public CIMaskToAlpha (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
	}

	[Since (6,0)]
	public class CIMaximumComponent : CIFilter {
		public CIMaximumComponent () : base ("CIMaximumComponent") {}
		public CIMaximumComponent (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
	}

	[Since (6,0)]
	public class CIMinimumComponent : CIFilter {
		public CIMinimumComponent () : base ("CIMinimumComponent") {}
		public CIMinimumComponent (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
	}

	[Since (6,0)]
	public class CIPerspectiveTile : CIFilter {
		public CIPerspectiveTile () : base ("CIPerspectiveTile") {}
		public CIPerspectiveTile (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector BottomLeft {
			get {
				return GetVector ("inputBottomLeft");
			}
			set {
				SetValue ("inputBottomLeft", value);
			}
		}

		public CIVector BottomRight {
			get {
				return GetVector ("inputBottomRight");
			}
			set {
				SetValue ("inputBottomRight", value);
			}
		}

		public CIVector TopLeft {
			get {
				return GetVector ("inputTopLeft");
			}
			set {
				SetValue ("inputTopLeft", value);
			}
		}

		public CIVector TopRight {
			get {
				return GetVector ("inputTopRight");
			}
			set {
				SetValue ("inputTopRight", value);
			}
		}
	}

	[Since (6,0)]
	public class CIPerspectiveTransform : CIFilter {
		public CIPerspectiveTransform () : base ("CIPerspectiveTransform") {}
		public CIPerspectiveTransform (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector BottomLeft {
			get {
				return GetVector ("inputBottomLeft");
			}
			set {
				SetValue ("inputBottomLeft", value);
			}
		}

		public CIVector BottomRight {
			get {
				return GetVector ("inputBottomRight");
			}
			set {
				SetValue ("inputBottomRight", value);
			}
		}

		public CIVector TopLeft {
			get {
				return GetVector ("inputTopLeft");
			}
			set {
				SetValue ("inputTopLeft", value);
			}
		}

		public CIVector TopRight {
			get {
				return GetVector ("inputTopRight");
			}
			set {
				SetValue ("inputTopRight", value);
			}
		}
	}

	[Since (6,0)]
	public class CIPixellate : CIFilter {
		public CIPixellate () : base ("CIPixellate") {}
		public CIPixellate (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}

		public float Scale {
			get {
				return GetFloat("inputScale");
			}
			set {
				SetFloat("inputScale", value);
			}
		}
	}

	[Since (6,0)]
	public class CIRandomGenerator : CIFilter {
		public CIRandomGenerator () : base ("CIRandomGenerator") {}
		public CIRandomGenerator (IntPtr handle) : base (handle) {}
	}

	[Since (6,0)]
	public class CISharpenLuminance : CIFilter {
		public CISharpenLuminance () : base ("CISharpenLuminance") {}
		public CISharpenLuminance (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Sharpness {
			get {
				return GetFloat("inputSharpness");
			}
			set {
				SetFloat("inputSharpness", value);
			}
		}
	}

	[Since (6,0)]
	public class CIStarShineGenerator : CIFilter {
		public CIStarShineGenerator () : base ("CIStarShineGenerator") {}
		public CIStarShineGenerator (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}

		public float CrossAngle {
			get {
				return GetFloat("inputCrossAngle");
			}
			set {
				SetFloat("inputCrossAngle", value);
			}
		}

		public float CrossOpacity {
			get {
				return GetFloat("inputCrossOpacity");
			}
			set {
				SetFloat("inputCrossOpacity", value);
			}
		}

		public float CrossScale {
			get {
				return GetFloat("inputCrossScale");
			}
			set {
				SetFloat("inputCrossScale", value);
			}
		}

		public float CrossWidth {
			get {
				return GetFloat("inputCrossWidth");
			}
			set {
				SetFloat("inputCrossWidth", value);
			}
		}

		public float Epsilon {
			get {
				return GetFloat("inputEpsilon");
			}
			set {
				SetFloat("inputEpsilon", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat ("inputRadius", value);
			}
		}
	}

#if !MONOMAC
	[Since (6,0)]
	public class CITriangleKaleidoscope : CIFilter {
		public CITriangleKaleidoscope () : base ("CITriangleKaleidoscope") {}
		public CITriangleKaleidoscope (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public CIVector Point {
			get {
				return GetVector("inputPoint");
			}
			set {
				SetValue("inputPoint", value);
			}
		}

		public float Decay {
			get {
				return GetFloat("inputDecay");
			}
			set {
				SetFloat("inputDecay", value);
			}
		}

		public float Rotation {
			get {
				return GetFloat("inputRotation");
			}
			set {
				SetFloat("inputRotation", value);
			}
		}

		public float Size {
			get {
				return GetFloat("inputSize");
			}
			set {
				SetFloat("inputSize", value);
			}
		}
	}
#endif

	[Since (6,0)]
	public class CIUnsharpMask : CIFilter {
		public CIUnsharpMask () : base ("CIUnsharpMask") {}
		public CIUnsharpMask (IntPtr handle) : base (handle) {}

		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}

		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}

		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}		
	}
}