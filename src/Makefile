#
# CORE_SOURCES are source files that are written by hand
# and are linked with the generator
#

CORE_SOURCES =					\
	Constants.cs					\
	./AppKit/Enums.cs				\
	./CoreImage/Enums.cs				\
	./Foundation/EnumDesktop.cs			\
	./Foundation/NSObject.cs			\
	./ObjCRuntime/Class.cs				\
	./ObjCRuntime/Messaging.cs			\
	./ObjCRuntime/NativeConstructorBuilder.cs	\
	./ObjCRuntime/NativeImplementationBuilder.cs	\
	./ObjCRuntime/NativeMethodBuilder.cs		\
	./ObjCRuntime/MethodDescription.cs		\
	./ObjCRuntime/NSObjectMarshaler.cs		\
	./ObjCRuntime/SelectorMarshaler.cs		\
	./ObjCRuntime/Runtime.cs			\
	./ObjCRuntime/Selector.cs			\
	./WebKit/Enums.cs

# Sources that are not part of CORE_SOURCES or the generated templates
MONOMAC_SOURCES = \
	./Foundation/NSObjectMac.cs			\
	./AppKit/NSApplication.cs			\
	./AppKit/NSApplicationDelegate.cs		\
	./AppKit/NSBitmapImageRep.cs			\
	./AppKit/Functions.cs				\
	./AppKit/NSOpenGLPixelFormat.cs			\
	./AppKit/NSGraphics.cs				\
	./AppKit/NSMatrix.cs				\
	./AppKit/NSWindow.cs				\
	./Growl/Constants.cs

GENERATOR_SOURCES = \
	../../maccore/src/generator.cs			\
	../../maccore/src/Options.cs			\
	../../maccore/src/btouch.cs

SOURCES = \
	$(CORE_SOURCES)

DEFINES = -define:NET_2_0

TARGETS = bmac.exe monomac.dll  parse.exe

SHARED_SOURCE = \
	./AudioToolbox/AudioFile.cs			\
	./AudioToolbox/AudioFileStream.cs		\
	./AudioToolbox/AudioQueue.cs			\
	./AudioToolbox/AudioServices.cs			\
	./AudioToolbox/AudioSession.cs			\
	./AudioToolbox/AudioSessions.cs			\
	./AudioToolbox/AudioType.cs			\
	./AudioToolbox/SystemSound.cs			\
	./CoreAnimation/CATransform3D.cs		\
	./CoreAnimation/CAEnums.cs			\
	./CoreAnimation/CADefs.cs			\
	./CoreData/Enums.cs				\
	./CoreFoundation/CFArray.cs                     \
	./CoreFoundation/CFBoolean.cs                   \
	./CoreFoundation/CFDictionary.cs                \
	./CoreFoundation/CFString.cs                    \
	./CoreFoundation/CFUrl.cs                       \
	./CoreFoundation/CFRunLoop.cs                   \
	./CoreFoundation/CFException.cs                 \
	./CoreFoundation/Dispatch.cs                    \
	./CoreFoundation/CFBundle.cs                    \
	./CoreGraphics/CGAffineTransform.cs		\
	./CoreGraphics/CGBitmapContext.cs		\
	./CoreGraphics/CGColor.cs			\
	./CoreGraphics/CGColorSpace.cs			\
	./CoreGraphics/CGContext.cs			\
	./CoreGraphics/CGDataProvider.cs		\
	./CoreGraphics/CGFont.cs			\
	./CoreGraphics/CGGeometry.cs			\
	./CoreGraphics/CGGradient.cs			\
	./CoreGraphics/CGImage.cs			\
	./CoreGraphics/CGLayer.cs			\
	./CoreGraphics/CGPDFDocument.cs			\
	./CoreGraphics/CGPDFPage-2.cs			\
	./CoreGraphics/CGPDFPage.cs			\
	./CoreGraphics/CGPath.cs			\
	./CoreGraphics/CGPattern.cs			\
        ./ImageIO/CGImageSource.cs                      \
	./CoreText/Adapter.cs				\
	./CoreText/CTFont.Generator.cs			\
	./CoreText/CTFont.cs				\
	./CoreText/CTFontCollection.cs			\
	./CoreText/CTFontDescriptor.cs			\
	./CoreText/CTFontNameKey.cs			\
	./CoreText/CTFontTrait.cs			\
	./CoreText/CTFrame.cs				\
	./CoreText/CTFramesetter.cs			\
	./CoreText/CTGlyphInfo.cs			\
	./CoreText/CTLine.cs				\
	./CoreText/CTParagraphStyle.cs			\
	./CoreText/CTRun.cs				\
	./CoreText/CTRunDelegate.cs			\
	./CoreText/CTStringAttributes.cs		\
	./CoreText/CTTextTab.cs				\
	./CoreText/CTTypesetter.cs			\
	./CoreText/ConstructorError.cs			\
	./Foundation/ConnectAttribute.cs		\
	./Foundation/Enum.cs 				\
	./Foundation/ExportAttribute.cs			\
	./Foundation/ModelAttribute.cs			\
	./Foundation/ModelNotImplementedException.cs	\
	./Foundation/NSAutoreleasePool.cs		\
	./Foundation/NSAction.cs			\
	./Foundation/NSArray.cs				\
	./Foundation/NSBundle.cs			\
	./Foundation/NSCoder.cs				\
	./Foundation/NSData.cs				\
	./Foundation/NSDate.cs				\
	./Foundation/NSError.cs				\
	./Foundation/NSHttpCookie.cs			\
	./Foundation/NSHttpCookieStorage.cs		\
	./Foundation/NSIndexPath.cs			\
	./Foundation/NSInputStream.cs			\
	./Foundation/NSKeyedArchiver.cs			\
	./Foundation/NSKeyedUnarchiver.cs		\
	./Foundation/NSMutableAttributedString.cs	\
	./Foundation/NSMutableData.cs			\
	./Foundation/NSMutableDictionary.cs		\
	./Foundation/NSDictionary.cs			\
	./Foundation/NSMutableUrlRequest.cs		\
	./Foundation/NSNotificationCenter.cs		\
	./Foundation/NSNumber.cs			\
	./Foundation/NSNumber2.cs			\
	./Foundation/NSObject2.cs			\
	./Foundation/NSOutputStream.cs			\
	./Foundation/NSRange.cs				\
	./Foundation/NSSet.cs				\
	./Foundation/NSString.cs			\
	./Foundation/NSString2.cs			\
	./Foundation/NSTimer.cs				\
	./Foundation/NSUrl.cs				\
	./Foundation/NSUrlConnection.cs			\
	./Foundation/NSUserDefaults.cs			\
	./Foundation/NSValue.cs				\
	./Foundation/PreserveAttribute.cs		\
	./Foundation/RegisterAttribute.cs		\
	./ImageIO/CGImageSource.cs			\
	./ObjCRuntime/ArgumentSemantic.cs		\
	./ObjCRuntime/AlphaAttribute.cs			\
	./ObjCRuntime/Blocks.cs				\
	./ObjCRuntime/INativeObject.cs			\
	./ObjCRuntime/Dlfcn.cs				\
	./ObjCRuntime/SinceAttribute.cs			\
	./ObjCRuntime/TypeConverter.cs			\
	./MonoPInvokeCallbackAttribute.cs

# Not working
#	./CoreGraphics/CGShading.cs			\

SHARED_CORE_SOURCE = \
	./CoreAnimation/CATransform3D.cs		\
	./CoreAnimation/CAEnums.cs			\
	./CoreData/Enums.cs				\
	./CoreFoundation/CFString.cs                    \
	./CoreFoundation/CFUrl.cs                       \
	./CoreFoundation/CFRunLoop.cs                   \
	./CoreFoundation/Dispatch.cs                    \
	./CoreGraphics/CGAffineTransform.cs             \
	./CoreGraphics/CGBitmapContext.cs               \
	./CoreGraphics/CGColor.cs                       \
	./CoreGraphics/CGColorSpace.cs                  \
	./CoreGraphics/CGContext.cs                     \
	./CoreGraphics/CGDataProvider.cs                \
	./CoreGraphics/CGFont.cs                        \
	./CoreGraphics/CGGeometry.cs                    \
	./CoreGraphics/CGGradient.cs                    \
	./CoreGraphics/CGImage.cs                       \
	./CoreGraphics/CGLayer.cs			\
	./CoreGraphics/CGPDFDocument.cs                 \
	./CoreGraphics/CGPDFPage-2.cs                   \
	./CoreGraphics/CGPDFPage.cs                     \
	./CoreGraphics/CGPath.cs                        \
	./CoreGraphics/CGPattern.cs                     \
	./CoreMedia/CMSampleBuffer.cs			\
	./CoreMedia/CoreMedia.cs			\
	./Foundation/ConnectAttribute.cs		\
	./Foundation/Enum.cs 				\
	./Foundation/ExportAttribute.cs			\
	./Foundation/ModelAttribute.cs			\
	./Foundation/NSAction.cs			\
	./Foundation/NSRange.cs				\
	./Foundation/NSString.cs			\
	./Foundation/RegisterAttribute.cs		\
	./Foundation/PreserveAttribute.cs		\
        ./ObjCRuntime/ArgumentSemantic.cs               \
        ./ObjCRuntime/AlphaAttribute.cs                 \
	./ObjCRuntime/Blocks.cs				\
        ./ObjCRuntime/INativeObject.cs                  \
        ./ObjCRuntime/SinceAttribute.cs                 \
        ./ObjCRuntime/TypeConverter.cs                  \
	./MonoPInvokeCallbackAttribute.cs


SHARED_API = \
	./foundation.cs		\
	./coreanimation.cs	\
	./coredata.cs

SHARED_SOURCES = $(patsubst %,../../maccore/src/%,$(SHARED_SOURCE))
SHARED_CORE_SOURCES = $(patsubst %,../../maccore/src/%,$(SHARED_CORE_SOURCE))
SHARED_APIS = $(patsubst %,../../maccore/src/%,$(SHARED_API))

APIS = \
	appkit.cs		\
	coreimage.cs		\
	foundation-desktop.cs 	\
	growl.cs		\
	webkit.cs		\
	$(SHARED_APIS)

all: $(TARGETS)

parse.exe: parse.cs
	gmcs parse.cs

core.dll: $(CORE_SOURCES) $(SHARED_CORE_SOURCES) Makefile
	gmcs -target:library -debug -unsafe -out:core.dll -define:MONOMAC,MONOMAC_BOOTSTRAP $(CORE_SOURCES) $(SHARED_CORE_SOURCES) -r:System.Drawing

bmac.exe: $(APIS) core.dll $(GENERATOR_SOURCES)
	gmcs -unsafe -out:bmac.exe -debug $(DEFINES) -define:GENERATOR,MONOMAC,MONOMAC_BOOTSTRAP $(GENERATOR_SOURCES) -r:core.dll

generated_sources: bmac.exe $(APIS) $(SHARED_APIS)
	mono --debug bmac.exe -a -d=MONOMAC,MONOMAC_BOOTSTRAP --core --sourceonly=generated_sources -v -tmpdir=. $(APIS) --baselib=core.dll --ns=MonoMac.ObjCRuntime -r=System.Drawing

count: 
	make pass | wc -l

counts:
	make pass | grep 'could not be' | sed 's/.*`//' | sed "s/'.*//" | sort | uniq -c | sort -n

monomac.dll: $(SOURCES) generated_sources $(MONOMAC_SOURCES) $(SHARED_SOURCES)
	gmcs -nowarn:0414 -debug -unsafe $(SOURCES) @generated_sources $(MONOMAC_SOURCES) $(SHARED_SOURCES) -target:library -define:GENERATOR -define:MONOMAC -out:monomac.dll -r:System.Drawing

clean:
	rm -f $(TARGETS) core.dll

all: monomac.dll
