//
// coreimage.cs: Definitions for CoreImage
//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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

//
//
// TODO: CIFilter, eliminate the use of the NSDictionary and use
// strongly typed accessors.
//
// TODO:
// CIImageProvider     - informal protocol, 
// CIPluginInterface   - informal protocol
// CIRAWFilter         - informal protocol
// CIVector
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;
using MonoMac.CoreImage;
using MonoMac.CoreVideo;
#if !MONOMAC
using MonoTouch.OpenGLES;
using MonoTouch.UIKit;
#else
using MonoMac.AppKit;
#endif

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif

namespace MonoMac.CoreImage {

	[BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	public interface CIColor {
		[Static]
		[Export ("colorWithCGColor:")]
		CIColor FromCGColor (CGColor c);

		[Static]
		[Export ("colorWithRed:green:blue:alpha:")]
		CIColor FromRgba (nfloat red, nfloat green, nfloat blue, nfloat alpha);

		[Static]
		[Export ("colorWithRed:green:blue:")]
		CIColor FromRgb (nfloat red, nfloat green, nfloat blue);

		[Static]
		[Export ("colorWithString:")]
		CIColor FromString (string representation);

		[Export ("initWithCGColor:")]
		IntPtr Constructor (CGColor c);

		[Export ("numberOfComponents")]
		int NumberOfComponents { get; }

		// FIXME: bdining
		//[Export ("components")]
		//const nfloat Components ();

		[Export ("alpha")]
		nfloat Alpha { get; }

		[Export ("colorSpace")]
		CGColorSpace ColorSpace { get; }

		[Export ("red")]
		nfloat Red { get; }

		[Export ("green")]
		nfloat Green { get; }

		[Export ("blue")]
		nfloat Blue { get; }

		[Export ("stringRepresentation")]
		string StringRepresentation ();

#if !MONOMAC
		[Export ("initWithColor:")]
		IntPtr Constructor (UIColor color);
#endif
	}

        [BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	public interface CIContext {
		// When we bind OpenGL add these:
		//[Export ("contextWithCGLContext:pixelFormat:colorSpace:options:")]
		//CIContext ContextWithCGLContextpixelFormatcolorSpaceoptions (CGLContextObj ctx, CGLPixelFormatObj pf, CGColorSpaceRef cs, NSDictionary dict, );

#if MONOMAC
		[Internal, Static]
		[Export ("contextWithCGContext:options:")]
		CIContext FromContext (CGContext ctx, [NullAllowed] NSDictionary options);
#else
		[Static]
		[Wrap ("FromOptions ((NSDictionary) null)")]
		CIContext Create ();

		[Static]
		[Export ("contextWithEAGLContext:")]
		CIContext FromContext (EAGLContext eaglContext);

		[Static, Internal]
		[Export ("contextWithOptions:")]
		CIContext FromOptions ([NullAllowed] NSDictionary dictionary);

		[Export ("render:toCVPixelBuffer:")]
		void Render (CIImage image, CVPixelBuffer buffer);

		[Export ("render:toCVPixelBuffer:bounds:colorSpace:")]
		void Render (CIImage image, CVPixelBuffer buffer, CGRect rectangle, CGColorSpace cs);

		[Export ("inputImageMaximumSize")]
		CGSize InputImageMaximumSize { get; }

		[Export ("outputImageMaximumSize")]
		CGSize OutputImageMaximumSize { get; }
#endif

		[Obsolete ("Deprecated in iOS 6.0. Use DrawImage (CIImage, RectangleF, RectangleF) instead")]
		[Export ("drawImage:atPoint:fromRect:")]
		void DrawImage (CIImage image, CGPoint atPoint, CGRect fromRect);

		[Export ("drawImage:inRect:fromRect:")]
		void DrawImage (CIImage image, CGRect inRectangle, CGRect fromRectangle);

		[Export ("createCGImage:fromRect:")]
		[return: Release ()]
		CGImage CreateCGImage (CIImage image, CGRect fromRectangle);

		[Export ("createCGImage:fromRect:format:colorSpace:")]
		[return: Release ()]
		CGImage CreateCGImage (CIImage image, CGRect fromRect, nint ciImageFormat, [NullAllowed] CGColorSpace colorSpace);
#if MONOMAC
		[Internal, Export ("createCGLayerWithSize:info:")]
		CGLayer CreateCGLayer (CGSize size, [NullAllowed] NSDictionary info);
#endif
		[Export ("render:toBitmap:rowBytes:bounds:format:colorSpace:")]
		void RenderToBitmap (CIImage image, IntPtr bitmapPtr, nint bytesPerRow, CGRect bounds, nint bitmapFormat, CGColorSpace colorSpace);

		//[Export ("render:toIOSurface:bounds:colorSpace:")]
		//void RendertoIOSurfaceboundscolorSpace (CIImage im, IOSurfaceRef surface, CGRect r, CGColorSpaceRef cs, );

#if MONOMAC
		[Export ("reclaimResources")]
		void ReclaimResources ();

		[Export ("clearCaches")]
		void ClearCaches ();
#endif

		[Internal, Field ("kCIContextOutputColorSpace", "+CoreImage")]
		NSString OutputColorSpace { get; }

		[Internal, Field ("kCIContextWorkingColorSpace", "+CoreImage")]
		NSString WorkingColorSpace { get; }
		
		[Internal, Field ("kCIContextUseSoftwareRenderer", "+CoreImage")]
		NSString UseSoftwareRenderer { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	public interface CIFilter {
		[Export ("inputKeys")]
		string [] InputKeys { get; }

		[Export ("outputKeys")]
		string [] OutputKeys { get; }

		[Export ("setDefaults")]
		void SetDefaults ();

		[Export ("attributes")]
		NSDictionary Attributes { get; }

		[Export ("name")]
		string Name { get; }

		[Static]
		[Export ("filterWithName:")]
		CIFilter FromName (string name);

		[Static]
		[Export ("filterNamesInCategory:")]
		string [] FilterNamesInCategory (string category);

		[Static]
		[Export ("filterNamesInCategories:"), Internal]
		string [] _FilterNamesInCategories (string [] categories);

#if MONOMAC
		[Export ("apply:arguments:options:")]
		CIImage Apply (CIKernel k, NSArray args, NSDictionary options);

		[Static]
		[Export ("registerFilterName:constructor:classAttributes:")]
		void RegisterFilterName (string name, NSObject constructorObject, NSDictionary classAttributes);

		[Static]
		[Export ("localizedNameForFilterName:")]
		string FilterLocalizedName (string filterName);

		[Static]
		[Export ("localizedNameForCategory:")]
		string CategoryLocalizedName (string category);

		[Static]
		[Export ("localizedDescriptionForFilterName:")]
		string FilterLocalizedDescription (string filterName);

		[Static]
		[Export ("localizedReferenceDocumentationForFilterName:")]
		NSUrl FilterLocalizedReferenceDocumentation (string filterName);
#else
		[Export ("outputImage")]
		CIImage OutputImage { get; }

		[Since (6,0)]
		[Export ("serializedXMPFromFilters:inputImageExtent:"), Static]
		NSData SerializedXMP (CIFilter[] filters, CGRect extent); 

		[Since (6,0)]
		[Export ("filterArrayFromSerializedXMP:inputImageExtent:error:"), Static]
		CIFilter[] FromSerializedXMP (NSData xmpData, CGRect extent, out NSError error);
#endif

		[Export ("setValue:forKey:"), Internal]
		void SetValueForKey ([NullAllowed] NSObject value, NSString key);

		[Export ("valueForKey:"), Internal]
		NSObject ValueForKey (NSString key);
	}

	[Static]
	public interface CIFilterOutputKey {
		[Field ("kCIOutputImageKey", "+CoreImage")]
		NSString Image  { get; }
	}
	
	[Static]
	public interface CIFilterInputKey {
		[Field ("kCIInputBackgroundImageKey", "+CoreImage")]
		NSString BackgroundImage  { get; }

		[Field ("kCIInputImageKey", "+CoreImage")]
		NSString Image  { get; }
#if !MONOMAC
		[Field ("kCIInputVersionKey", "+CoreImage")]
		NSString Version { get; }
#else
		[Field ("kCIInputTimeKey", "+CoreImage")]
		NSString Time  { get; }

		[Field ("kCIInputTransformKey", "+CoreImage")]
		NSString Transform  { get; }

		[Field ("kCIInputScaleKey", "+CoreImage")]
		NSString Scale  { get; }

		[Field ("kCIInputAspectRatioKey", "+CoreImage")]
		NSString AspectRatio  { get; }

		[Field ("kCIInputCenterKey", "+CoreImage")]
		NSString Center  { get; }

		[Field ("kCIInputRadiusKey", "+CoreImage")]
		NSString Radius  { get; }

		[Field ("kCIInputAngleKey", "+CoreImage")]
		NSString Angle  { get; }

		[Field ("kCIInputRefractionKey", "+CoreImage")]
		NSString Refraction  { get; }

		[Field ("kCIInputWidthKey", "+CoreImage")]
		NSString Width  { get; }

		[Field ("kCIInputSharpnessKey", "+CoreImage")]
		NSString Sharpness  { get; }

		[Field ("kCIInputIntensityKey", "+CoreImage")]
		NSString Intensity  { get; }

		[Field ("kCIInputEVKey", "+CoreImage")]
		NSString EV  { get; }

		[Field ("kCIInputSaturationKey", "+CoreImage")]
		NSString Saturation  { get; }

		[Field ("kCIInputColorKey", "+CoreImage")]
		NSString Color  { get; }

		[Field ("kCIInputBrightnessKey", "+CoreImage")]
		NSString Brightness  { get; }

		[Field ("kCIInputContrastKey", "+CoreImage")]
		NSString Contrast  { get; }

		[Field ("kCIInputGradientImageKey", "+CoreImage")]
		NSString GradientImage  { get; }

		[Field ("kCIInputMaskImageKey", "+CoreImage")]
		NSString MaskImage  { get; }

		[Field ("kCIInputShadingImageKey", "+CoreImage")]
		NSString ShadingImage  { get; }

		[Field ("kCIInputTargetImageKey", "+CoreImage")]
		NSString TargetImage  { get; }

		[Field ("kCIInputExtentKey", "+CoreImage")]
		NSString Extent  { get; }

#endif
	}
		
	[Static]
	public interface CIFilterAttributes {
		[Field ("kCIAttributeFilterName", "+CoreImage")]
		NSString FilterName  { get; }

		[Field ("kCIAttributeFilterDisplayName", "+CoreImage")]
		NSString FilterDisplayName  { get; }

#if MONOMAC
		[Field ("kCIAttributeDescription", "+CoreImage")]
		NSString Description  { get; }

		[Field ("kCIAttributeReferenceDocumentation", "+CoreImage")]
		NSString ReferenceDocumentation  { get; }
#endif

		[Field ("kCIAttributeFilterCategories", "+CoreImage")]
		NSString FilterCategories  { get; }

		[Field ("kCIAttributeClass", "+CoreImage")]
		NSString Class  { get; }

		[Field ("kCIAttributeType", "+CoreImage")]
		NSString Type  { get; }

		[Field ("kCIAttributeMin", "+CoreImage")]
		NSString Min  { get; }

		[Field ("kCIAttributeMax", "+CoreImage")]
		NSString Max  { get; }

		[Field ("kCIAttributeSliderMin", "+CoreImage")]
		NSString SliderMin  { get; }

		[Field ("kCIAttributeSliderMax", "+CoreImage")]
		NSString SliderMax  { get; }

		[Field ("kCIAttributeDefault", "+CoreImage")]
		NSString Default  { get; }

		[Field ("kCIAttributeIdentity", "+CoreImage")]
		NSString Identity  { get; }

		[Field ("kCIAttributeName", "+CoreImage")]
		NSString Name  { get; }

		[Field ("kCIAttributeDisplayName", "+CoreImage")]
		NSString DisplayName  { get; }

#if MONOMAC
		[Field ("kCIUIParameterSet", "+CoreImage")]
		NSString UIParameterSet  { get; }

#endif
		[Field ("kCIAttributeTypeTime", "+CoreImage")]
		NSString TypeTime  { get; }

		[Field ("kCIAttributeTypeScalar", "+CoreImage")]
		NSString TypeScalar  { get; }

		[Field ("kCIAttributeTypeDistance", "+CoreImage")]
		NSString TypeDistance  { get; }

		[Field ("kCIAttributeTypeAngle", "+CoreImage")]
		NSString TypeAngle  { get; }

		[Field ("kCIAttributeTypeBoolean", "+CoreImage")]
		NSString TypeBoolean  { get; }

		[Field ("kCIAttributeTypeInteger", "+CoreImage")]
		NSString TypeInteger  { get; }

		[Field ("kCIAttributeTypeCount", "+CoreImage")]
		NSString TypeCount  { get; }

		[Field ("kCIAttributeTypePosition", "+CoreImage")]
		NSString TypePosition  { get; }

		[Field ("kCIAttributeTypeOffset", "+CoreImage")]
		NSString TypeOffset  { get; }

		[Field ("kCIAttributeTypePosition3", "+CoreImage")]
		NSString TypePosition3  { get; }

		[Field ("kCIAttributeTypeRectangle", "+CoreImage")]
		NSString TypeRectangle  { get; }

#if MONOMAC
		[Field ("kCIAttributeTypeOpaqueColor", "+CoreImage")]
		NSString TypeOpaqueColor  { get; }

		[Field ("kCIAttributeTypeGradient", "+CoreImage")]
		NSString TypeGradient  { get; }
#else
		[Field ("kCIAttributeTypeImage", "+CoreImage")]
		NSString TypeImage  { get; }

		[Field ("kCIAttributeTypeTransform", "+CoreImage")]
		NSString TypeTransform  { get; }
#endif
	}

	[Static]
	public interface CIFilterCategory {
		[Field ("kCICategoryDistortionEffect", "+CoreImage")]
		NSString DistortionEffect  { get; }

		[Field ("kCICategoryGeometryAdjustment", "+CoreImage")]
		NSString GeometryAdjustment  { get; }

		[Field ("kCICategoryCompositeOperation", "+CoreImage")]
		NSString CompositeOperation  { get; }

		[Field ("kCICategoryHalftoneEffect", "+CoreImage")]
		NSString HalftoneEffect  { get; }

		[Field ("kCICategoryColorAdjustment", "+CoreImage")]
		NSString ColorAdjustment  { get; }

		[Field ("kCICategoryColorEffect", "+CoreImage")]
		NSString ColorEffect  { get; }

		[Field ("kCICategoryTransition", "+CoreImage")]
		NSString Transition  { get; }

		[Field ("kCICategoryTileEffect", "+CoreImage")]
		NSString TileEffect  { get; }

		[Field ("kCICategoryGenerator", "+CoreImage")]
		NSString Generator  { get; }

		[Field ("kCICategoryReduction", "+CoreImage")]
		NSString Reduction  { get; }

		[Field ("kCICategoryGradient", "+CoreImage")]
		NSString Gradient  { get; }

		[Field ("kCICategoryStylize", "+CoreImage")]
		NSString Stylize  { get; }

		[Field ("kCICategorySharpen", "+CoreImage")]
		NSString Sharpen  { get; }

		[Field ("kCICategoryBlur", "+CoreImage")]
		NSString Blur  { get; }

		[Field ("kCICategoryVideo", "+CoreImage")]
		NSString Video  { get; }

		[Field ("kCICategoryStillImage", "+CoreImage")]
		NSString StillImage  { get; }

		[Field ("kCICategoryInterlaced", "+CoreImage")]
		NSString Interlaced  { get; }

		[Field ("kCICategoryNonSquarePixels", "+CoreImage")]
		NSString NonSquarePixels  { get; }

		[Field ("kCICategoryHighDynamicRange", "+CoreImage")]
		NSString HighDynamicRange  { get; }

		[Field ("kCICategoryBuiltIn", "+CoreImage")]
		NSString BuiltIn  { get; }

#if MONOMAC
		[Field ("kCICategoryFilterGenerator", "+CoreImage")]
		NSString FilterGenerator  { get; }
#endif
	}
	
#if MONOMAC
	[Static]
	public interface CIUIParameterSet {
		[Field ("kCIUISetBasic", "+CoreImage")]
		NSString Basic  { get; }

		[Field ("kCIUISetIntermediate", "+CoreImage")]
		NSString Intermediate  { get; }

		[Field ("kCIUISetAdvanced", "+CoreImage")]
		NSString Advanced  { get; }

		[Field ("kCIUISetDevelopment", "+CoreImage")]
		NSString Development  { get; }
	}

	[Static]
	public interface CIFilterApply {
		[Field ("kCIApplyOptionExtent", "+CoreImage")]
		NSString OptionExtent  { get; }

		[Field ("kCIApplyOptionDefinition", "+CoreImage")]
		NSString OptionDefinition  { get; }

		[Field ("kCIApplyOptionUserInfo", "+CoreImage")]
		NSString OptionUserInfo  { get; }

		[Field ("kCIApplyOptionColorSpace", "+CoreImage")]
		NSString OptionColorSpace  { get; }
	}
#endif
	
#if MONOMAC
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface CIFilterGenerator {
		[Static, Export ("filterGenerator")]
		CIFilterGenerator Create ();

		[Static]
		[Export ("filterGeneratorWithContentsOfURL:")]
		CIFilterGenerator FromUrl (NSUrl aURL);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl aURL);

		[Export ("connectObject:withKey:toObject:withKey:")]
		void ConnectObject (NSObject sourceObject, string withSourceKey, NSObject targetObject, string targetKey);

		[Export ("disconnectObject:withKey:toObject:withKey:")]
		void DisconnectObject (NSObject sourceObject, string sourceKey, NSObject targetObject, string targetKey);

		[Export ("exportKey:fromObject:withName:")]
		void ExportKey (string key, NSObject targetObject, string exportedKeyName);

		[Export ("removeExportedKey:")]
		void RemoveExportedKey (string exportedKeyName);

		[Export ("exportedKeys")]
		NSDictionary ExportedKeys { get; }

		[Export ("setAttributes:forExportedKey:")]
		void SetAttributesforExportedKey (NSDictionary attributes, NSString exportedKey);

		[Export ("filter")]
		CIFilter CreateFilter ();

		[Export ("registerFilterName:")]
		void RegisterFilterName (string name);

		[Export ("writeToURL:atomically:")]
		bool Save (NSUrl toUrl, bool atomically);

		//Detected properties
		[Export ("classAttributes")]
		NSDictionary ClassAttributes { get; set; }

		[Field ("kCIFilterGeneratorExportedKey", "+CoreImage")]
		NSString ExportedKey { get; }

		[Field ("kCIFilterGeneratorExportedKeyTargetObject", "+CoreImage")]
		NSString ExportedKeyTargetObject { get; }

		[Field ("kCIFilterGeneratorExportedKeyName", "+CoreImage")]
		NSString ExportedKeyName { get; }
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface CIFilterShape {
		[Static]
		[Export ("shapeWithRect:")]
		CIFilterShape FromRect (CGRect rect);

		[Export ("initWithRect:")]
		IntPtr Constructor (CGRect rect);

		[Export ("transformBy:interior:")]
		CIFilterShape Transform (CGAffineTransform transformation, bool interiorFlag);

		[Export ("insetByX:Y:")]
		CIFilterShape Inset (nint dx, nint dy);

		[Export ("unionWith:")]
		CIFilterShape Union (CIFilterShape other);

		[Export ("unionWithRect:")]
		CIFilterShape Union (CGRect rectangle);

		[Export ("intersectWith:")]
		CIFilterShape Intersect (CIFilterShape other);

		[Export ("intersectWithRect:")]
		CIFilterShape IntersectWithRect (CGRect rectangle);
	}
#endif
	
	[BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	public interface CIImage {
		[Static]
		[Export ("imageWithCGImage:")]
		CIImage FromCGImage (CGImage image);

		[Static]
		[Export ("imageWithCGImage:options:")]
		CIImage FromCGImage (CGImage image, [NullAllowed] NSDictionary d);

		[Static]
		[Wrap ("FromCGImage (image, options == null ? null : options.Dictionary)")]
		CIImage FromCGImage (CGImage image, [NullAllowed] CIImageInitializationOptionsWithMetadata options);

#if MONOMAC
		[Static]
		[Export ("imageWithCGLayer:")]
		CIImage FromLayer (CGLayer layer);

		[Static]
		[Export ("imageWithCGLayer:options:")]
		CIImage FromLayer (CGLayer layer, NSDictionary options);
#endif

		[Static]
		[Export ("imageWithBitmapData:bytesPerRow:size:format:colorSpace:")]
		// TODO: pixelFormat should be enum of kCIFormatARGB8, kCIFormatRGBA16, kCIFormatRGBAf, kCIFormatRGBAh
		CIImage FromData (NSData bitmapData, nint bytesPerRow, CGSize size, nint pixelFormat, [NullAllowed] CGColorSpace colorSpace);

#if MONOMAC
		[Since (6,0)]
		[Static]
		[Export ("imageWithTexture:size:flipped:colorSpace:")]
		CIImage ImageWithTexture (nuint glTextureName, CGSize size, bool flipped, CGColorSpace colorspace);
#endif

		[Static]
		[Export ("imageWithContentsOfURL:")]
		CIImage FromUrl (NSUrl url);

		[Static]
		[Export ("imageWithContentsOfURL:options:")]
		CIImage FromUrl (NSUrl url, [NullAllowed] NSDictionary d);

		[Static]
		[Wrap ("FromUrl (url, options == null ? null : options.Dictionary)")]
		CIImage FromUrl (NSUrl url, [NullAllowed] CIImageInitializationOptions options);

		[Static]
		[Export ("imageWithData:")]
		CIImage FromData (NSData data);

		[Static]
		[Export ("imageWithData:options:")]
		CIImage FromData (NSData data, [NullAllowed] NSDictionary d);

		[Static]
		[Wrap ("FromData (data, options == null ? null : options.Dictionary)")]
		CIImage FromData (NSData data, [NullAllowed] CIImageInitializationOptionsWithMetadata options);

#if MONOMAC
		[Static]
		[Export ("imageWithCVImageBuffer:")]
		CIImage FromImageBuffer (CVImageBuffer imageBuffer);

		[Static]
		[Export ("imageWithCVImageBuffer:options:")]
		CIImage FromImageBuffer (CVImageBuffer imageBuffer, NSDictionary dict);
#else
		[Static]
		[Export ("imageWithCVPixelBuffer:")]
		CIImage FromImageBuffer (CVPixelBuffer buffer);

		[Static]
		[Export ("imageWithCVPixelBuffer:options:")]
		CIImage FromImageBuffer (CVPixelBuffer buffer, [NullAllowed] NSDictionary dict);

		[Static]
		[Wrap ("FromImageBuffer (buffer, options == null ? null : options.Dictionary)")]
		CIImage FromImageBuffer (CVPixelBuffer buffer, [NullAllowed] CIImageInitializationOptions options);
#endif
		//[Export ("imageWithIOSurface:")]
		//CIImage ImageWithIOSurface (IOSurfaceRef surface, );
		//
		//[Static]
		//[Export ("imageWithIOSurface:options:")]
		//CIImage ImageWithIOSurfaceoptions (IOSurfaceRef surface, NSDictionary d, );

		[Static]
		[Export ("imageWithColor:")]
		CIImage ImageWithColor (CIColor color);

		[Static]
		[Export ("emptyImage")]
		CIImage EmptyImage { get; }
		
		[Export ("initWithCGImage:")]
		IntPtr Constructor (CGImage image);

		[Export ("initWithCGImage:options:")]
		IntPtr Constructor (CGImage image, [NullAllowed] NSDictionary d);

		[Wrap ("this (image, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (CGImage image, [NullAllowed] CIImageInitializationOptionsWithMetadata options);

		[Export ("initWithCGLayer:")]
		IntPtr Constructor (CGLayer layer);

		[Export ("initWithCGLayer:options:")]
		IntPtr Constructor (CGLayer layer, [NullAllowed] NSDictionary d);

		[Wrap ("this (layer, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (CGLayer layer, [NullAllowed] CIImageInitializationOptions options);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("initWithData:options:")]
		IntPtr Constructor (NSData data, [NullAllowed] NSDictionary d);

		[Wrap ("this (data, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (NSData data, [NullAllowed] CIImageInitializationOptionsWithMetadata options);

		[Export ("initWithBitmapData:bytesPerRow:size:format:colorSpace:")]
		IntPtr Constructor (NSData d, nint bytesPerRow, CGSize size, nint pixelFormat, CGColorSpace colorSpace);

		[Since (6,0)]
		[Export ("initWithTexture:size:flipped:colorSpace:")]
		IntPtr Constructor (nint glTextureName, CGSize size, bool flipped, CGColorSpace colorSpace);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithContentsOfURL:options:")]
		IntPtr Constructor (NSUrl url, [NullAllowed] NSDictionary d);

		[Wrap ("this (url, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (NSUrl url, [NullAllowed] CIImageInitializationOptions options);

		// FIXME: bindings
		//[Export ("initWithIOSurface:")]
		//NSObject InitWithIOSurface (IOSurfaceRef surface, );
		//
		//[Export ("initWithIOSurface:options:")]
		//NSObject InitWithIOSurfaceoptions (IOSurfaceRef surface, NSDictionary d, );
		//
		[Export ("initWithCVImageBuffer:")]
		IntPtr Constructor (CVImageBuffer imageBuffer);

		[Export ("initWithCVImageBuffer:options:")]
		IntPtr Constructor (CVImageBuffer imageBuffer, [NullAllowed] NSDictionary dict);

		[Wrap ("this (imageBuffer, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (CVImageBuffer imageBuffer, [NullAllowed] CIImageInitializationOptions options);

		[Export ("initWithColor:")]
		IntPtr Constructor (CIColor color);

#if MONOMAC
		[Export ("initWithBitmapImageRep:")]
		IntPtr Constructor (NSImageRep imageRep);
		
		[Export ("drawAtPoint:fromRect:operation:fraction:")]
		void Draw (CGPoint point, CGRect srcRect, NSCompositingOperation op, nfloat delta); 

		[Export ("drawInRect:fromRect:operation:fraction:")]
		void Draw (CGRect dstRect, CGRect srcRect, NSCompositingOperation op, nfloat delta); 
#endif

		[Export ("imageByApplyingTransform:")]
		CIImage ImageByApplyingTransform (CGAffineTransform matrix);

		[Export ("imageByCroppingToRect:")]
		CIImage ImageByCroppingToRect (CGRect r);

		[Export ("extent")]
		CGRect Extent { get; }

		[Since (5,0)]
		[Export ("properties"), Internal]
		NSDictionary WeakProperties { get; }

		[Since (5,0)]
		[Wrap ("WeakProperties")]
		CGImageProperties Properties { get; }

#if MONOMAC
		//[Export ("definition")]
		//CIFilterShape Definition ();

		[Field ("kCIFormatARGB8")]
		nint FormatARGB8 { get; }

		[Field ("kCIFormatRGBA16")]
		nint FormatRGBA16 { get; }

		[Field ("kCIFormatRGBAf")]
		nint FormatRGBAf { get; }

		[Field ("kCIFormatRGBAh")]
		nint FormatRGBAh { get; }
#else

		[Field ("kCIFormatARGB8")]
		[Since (6,0)]
		nint FormatARGB8 { get; }
		
		[Field ("kCIFormatRGBAh")]
		[Since (6,0)]
		nint FormatRGBAh { get; }

		[Field ("kCIFormatBGRA8")]
		[Since (5,0)]
		nint FormatBGRA8 { get; }

		[Field ("kCIFormatRGBA8")]
		[Since (5,0)]
		nint FormatRGBA8 { get; }

		// UIKit extensions
		[Since (5,0)]
		[Export ("initWithImage:")]
		IntPtr Constructor (UIImage image);

		[Since (5,0)]
		[Export ("initWithImage:options")]
		IntPtr Constructor (UIImage image, [NullAllowed] NSDictionary options);

		[Since (5,0)]
		[Wrap ("this (image, options == null ? null : options.Dictionary)")]
		IntPtr Constructor (UIImage image, [NullAllowed] CIImageInitializationOptions options);
#endif
		[MountainLion]
		[Field ("kCIImageAutoAdjustFeatures"), Internal]
		NSString AutoAdjustFeaturesKey { get; }

		[MountainLion]
		[Field ("kCIImageAutoAdjustRedEye"), Internal]
		NSString AutoAdjustRedEyeKey { get; }

		[MountainLion]
		[Field ("kCIImageAutoAdjustEnhance"), Internal]
		NSString AutoAdjustEnhanceKey { get; }
		
		[Export ("autoAdjustmentFilters"), Internal]
		NSArray _GetAutoAdjustmentFilters ();

		[Export ("autoAdjustmentFiltersWithOptions:"), Internal]
		NSArray _GetAutoAdjustmentFilters (NSDictionary opts);

		[Field ("kCGImagePropertyOrientation", "ImageIO"), Internal]
		NSString ImagePropertyOrientation { get; }

		[Field ("kCIImageColorSpace"), Internal]
		NSString CIImageColorSpaceKey { get; }

		[MountainLion]
		[Field ("kCIImageProperties"), Internal]
		NSString CIImagePropertiesKey { get; }
	}

#if MONOMAC
	[BaseType (typeof (NSObject))]
	public interface CIImageAccumulator {
		[Static]
		[Export ("imageAccumulatorWithExtent:format:")]
		CIImageAccumulator FromRectangle (CGRect rect, nint ciImageFormat);

		[Export ("initWithExtent:format:")]
		IntPtr Constructor (CGRect rectangle, nint ciImageFormat);

		[Export ("extent")]
		CGRect Extent { get; }

		[Export ("format")]
		nint CIImageFormat { get; }

		[Export ("setImage:dirtyRect:")]
		void SetImageDirty (CIImage image, CGRect dirtyRect);

		[Export ("clear")]
		void Clear ();

		//Detected properties
		[Export ("image")]
		CIImage Image { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // avoid crashes
	public interface CIKernel {
		[Static, Export ("kernelsWithString:")]
		CIKernel [] FromProgram (string coreImageShaderProgram);

		[Export ("name")]
		string Name { get; }

		[Export ("setROISelector:")]
		void SetRegionOfInterestSelector (Selector aMethod);
	}

	[BaseType (typeof (NSObject))]
	public interface CIPlugIn {
		[Static]
		[Export ("loadAllPlugIns")]
		void LoadAllPlugIns ();

		[Static]
		[Export ("loadNonExecutablePlugIns")]
		void LoadNonExecutablePlugIns ();

		[Static]
		[Export ("loadPlugIn:allowNonExecutable:")]
		void LoadPlugIn (NSUrl pluginUrl, bool allowNonExecutable);
	}

	[BaseType (typeof (NSObject))]
	public interface CISampler {
		[Static, Export ("samplerWithImage:")]
		CISampler FromImage (CIImage sourceImage);

		[Internal, Static]
		[Export ("samplerWithImage:options:")]
		CISampler FromImage (CIImage sourceImag, NSDictionary options);

		[Export ("initWithImage:")]
		IntPtr Constructor (CIImage sourceImage);

		[Internal, Export ("initWithImage:options:")]
		NSObject Constructor (CIImage image, NSDictionary options);

		[Export ("definition")]
		CIFilterShape Definition { get; }

		[Export ("extent")]
		CGRect Extent { get; }

		[Field ("kCISamplerAffineMatrix", "+CoreImage"), Internal]
		NSString AffineMatrix { get; }
		[Field ("kCISamplerWrapMode", "+CoreImage"), Internal]
		NSString WrapMode { get; }
		[Field ("kCISamplerFilterMode", "+CoreImage"), Internal]
		NSString FilterMode { get; }

		[Field ("kCISamplerWrapBlack", "+CoreImage"), Internal]
		NSString WrapBlack { get; }
		[Field ("kCISamplerWrapClamp", "+CoreImage"), Internal]
		NSString WrapClamp { get; }
		
		[Field ("kCISamplerFilterNearest", "+CoreImage"), Internal]
		NSString FilterNearest { get; }

		[Field ("kCISamplerFilterLinear", "+CoreImage"), Internal]
		NSString FilterLinear { get; }
	}
#endif
	
	[BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	interface CIVector {
		[Static, Internal, Export ("vectorWithValues:count:")]
		CIVector _FromValues (IntPtr values, nint count);

		[Static]
		[Export ("vectorWithX:")]
		CIVector Create (nfloat x);

		[Static]
		[Export ("vectorWithX:Y:")]
		CIVector Create (nfloat x, nfloat y);

		[Static]
		[Export ("vectorWithX:Y:Z:")]
		CIVector Create (nfloat x, nfloat y, nfloat z);

		[Static]
		[Export ("vectorWithX:Y:Z:W:")]
		CIVector Create (nfloat x, nfloat y, nfloat z, nfloat w);

#if !MONOMAC
		[Static]
		[Export ("vectorWithCGPoint:")]
		CIVector Create (CGPoint point);

		[Static]
		[Export ("vectorWithCGRect:")]
		CIVector Create (CGRect point);

		[Static]
		[Export ("vectorWithCGAffineTransform:")]
		CIVector Create (CGAffineTransform affineTransform);
#endif

		[Static]
		[Export ("vectorWithString:")]
		CIVector FromString (string representation);

		[Internal, Export ("initWithValues:count:")]
		IntPtr Constructor (IntPtr values, nint count);

		[Export ("initWithX:")]
		IntPtr Constructor(nfloat x);

		[Export ("initWithX:Y:")]
		IntPtr Constructor (nfloat x, nfloat y);

		[Export ("initWithX:Y:Z:")]
		IntPtr Constructor (nfloat x, nfloat y, nfloat z);

		[Export ("initWithX:Y:Z:W:")]
		IntPtr Constructor (nfloat x, nfloat y, nfloat z, nfloat w);

		[Export ("initWithString:")]
		IntPtr Constructor (string representation);

		[Export ("valueAtIndex:"), Internal]
		nfloat ValueAtIndex (nint index);

		[Export ("count")]
		int Count { get; }

		[Export ("X")]
		nfloat X { get; }

		[Export ("Y")]
		nfloat Y { get; }

		[Export ("Z")]
		nfloat Z { get; }

		[Export ("W")]
		nfloat W { get; }

#if !MONOMAC
		[Export ("CGPointValue")]
		CGPoint Point { get; }

		[Export ("CGRectValue")]
		CGRect Rectangle { get; }

		[Export ("CGAffineTransformValue")]
		CGAffineTransform AffineTransform { get; }
#endif

		[Export ("stringRepresentation"), Internal]
		string StringRepresentation ();

	}

	[BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	interface CIDetector {
		[Static, Export ("detectorOfType:context:options:"), Internal]
		CIDetector FromType ([NullAllowed] NSString detectorType, [NullAllowed] CIContext context, [NullAllowed] NSDictionary options);

		[Export ("featuresInImage:")]
		CIFeature [] FeaturesInImage (CIImage image);

		[Export ("featuresInImage:options:")]
		CIFeature [] FeaturesInImage (CIImage image, NSDictionary options);

		[Field ("CIDetectorTypeFace"), Internal]
		NSString TypeFace { get; }

		[MountainLion]
		[Field ("CIDetectorImageOrientation"), Internal]
		NSString ImageOrientation { get; }

		[Field ("CIDetectorAccuracy"), Internal]
		NSString Accuracy { get; }

		[Field ("CIDetectorAccuracyLow"), Internal]
		NSString AccuracyLow { get; }

		[Field ("CIDetectorAccuracyHigh"), Internal]
		NSString AccuracyHigh { get; }

		[Since (6,0)]
		[Field ("CIDetectorTracking"), Internal]
		NSString Tracking { get; }

		[Since (6,0)]
		[Field ("CIDetectorMinFeatureSize"), Internal]
		NSString MinFeatureSize { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Since (5,0)]
	[DisableDefaultCtor]
	interface CIFeature {
		[Export ("type")]
		NSString Type { get; }

		[Export ("bounds")]
		CGRect Bounds { get; }

		[Field ("CIFeatureTypeFace")]
		NSString TypeFace { get; }
	}

	[BaseType (typeof (CIFeature))]
	[Since (5,0)]
	[DisableDefaultCtor]
	interface CIFaceFeature {
		[Export ("hasLeftEyePosition")]
		bool HasLeftEyePosition { get; }
		
		[Export ("leftEyePosition")]
		CGPoint LeftEyePosition { get; }
		
		[Export ("hasRightEyePosition")]
		bool HasRightEyePosition { get; }
		
		[Export ("rightEyePosition")]
		CGPoint RightEyePosition { get; }
		
		[Export ("hasMouthPosition")]
		bool HasMouthPosition { get; }
		
		[Export ("mouthPosition")]
		CGPoint MouthPosition { get; }

		[Since (6,0)]
		[Export ("hasTrackingID")]
		bool HasTrackingId { get; }
		
		[Since (6,0)]
		[Export ("trackingID")]
		int TrackingId { get; }
		
		[Since (6,0)]
		[Export ("hasTrackingFrameCount")]
		bool HasTrackingFrameCount { get; }

		[Since (6,0)]
		[Export ("trackingFrameCount")]
		int TrackingFrameCount { get; }
	}
}
