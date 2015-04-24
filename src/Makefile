#
# Figure out if I want to use [New] or remove from contract
#
WARNINGS_I_SHOULD_FIX=114,108
TARGET = MonoMac.dll
PEER=false
MACCORE_DIR ?= ../maccore
# Build 64-bit, forces the use of CGRect/CGSize/CGPoint
64BIT ?= 0
# Build 64-bit and for COREFX. Must specify COREFX_LIB path.
COREFX ?= 0
# Use System.Drawing structs, '0' to use CGRect/CGSize/CGPoint
SDCOMPAT ?= 1

#
# CORE_SOURCES are source files that are written by hand
# and are linked with the generator
#

CORE_SOURCES =	$(EXTRA_CORE_SOURCES)			\
	Constants.cs					\
	./AppKit/Enums.cs				\
	./AppKit/Defs.cs				\
	./Foundation/EnumDesktop.cs			\
	./Foundation/NSObject.cs			\
	./ImageKit/Enums.cs				\
	./ObjCRuntime/Class.cs				\
	./ObjCRuntime/Messaging.cs			\
	./ObjCRuntime/NativeConstructorBuilder.cs	\
	./ObjCRuntime/NativeImplementationBuilder.cs	\
	./ObjCRuntime/NativeMethodBuilder.cs		\
	./ObjCRuntime/MethodDescription.cs		\
	./ObjCRuntime/NSObjectMarshaler.cs		\
	./ObjCRuntime/SelectorMarshaler.cs		\
	./ObjCRuntime/Runtime.cs			\
	./ObjCRuntime/RequiredFrameworkAttribute.cs	\
	./ObjCRuntime/Selector.cs			\
	./OpenGL/OpenTK/Math/Quaternion.cs		\
	./OpenGL/OpenTK/Math/MathHelper.cs		\
	./OpenGL/OpenTK/Math/Matrix4.cs			\
	./OpenGL/OpenTK/Math/Vector2.cs			\
	./OpenGL/OpenTK/Math/Vector3.cs			\
	./OpenGL/OpenTK/Math/Vector4.cs			\
	./PdfKit/Enums.cs				\
	./QTKit/Defs.cs					\
	./QTKit/Enums.cs				\
	./OpenGL/CGLEnums.cs				\
	./OpenGL/CGLContext.cs				\
	./OpenGL/CGLPixelFormat.cs			\
	./ScriptingBridge/Enums.cs			\
	./WebKit/Enums.cs

# Sources that are not part of CORE_SOURCES or the generated templates
BASE_SOURCES = $(EXTRA_MONOMAC_SOURCES) \
	./Compat.cs					\
	./Foundation/NSObjectMac.cs			\
	./Foundation/NSConnection.cs			\
	./AppKit/EventArgs.cs				\
	./AppKit/AppKitSynchronizationContext.cs	\
	./AppKit/AppKitThreadAccessException.cs		\
	./AppKit/ActionDispatcher.cs			\
	./AppKit/BeginSheet.cs				\
	./AppKit/DoubleWrapper.cs			\
	./AppKit/Functions.cs				\
	./AppKit/NSAlert.cs				\
	./AppKit/NSActionCell.cs			\
	./AppKit/NSApplication.cs			\
	./AppKit/NSApplicationDelegate.cs		\
	./AppKit/NSArrayController.cs			\
	./AppKit/NSBezierPath.cs			\
	./AppKit/NSBitmapImageRep.cs			\
	./AppKit/NSBrowser.cs				\
	./AppKit/NSButton.cs				\
	./AppKit/NSCell.cs				\
	./AppKit/NSColor.cs				\
	./AppKit/NSControl.cs				\
	./AppKit/NSDocument.cs				\
	./AppKit/NSEvent.cs				\
	./AppKit/NSGradient.cs				\
	./AppKit/NSGraphics.cs				\
	./AppKit/NSGraphicsContext.cs			\
	./AppKit/NSImage.cs				\
	./AppKit/NSLevelIndicator.cs			\
	./AppKit/NSMatrix.cs				\
	./AppKit/NSMenu.cs				\
	./AppKit/NSMenuItem.cs				\
	./AppKit/NSOpenGLContext.cs			\
	./AppKit/NSOpenGLPixelFormat.cs			\
	./AppKit/NSPageLayout.cs			\
	./AppKit/NSPopUpButton.cs			\
	./AppKit/NSPopUpButtonCell.cs			\
	./AppKit/NSPrintPanel.cs			\
	./AppKit/NSScreen.cs				\
	./AppKit/NSSharingService.cs			\
	./AppKit/NSSegmentedControl.cs			\
	./AppKit/NSSpeechSynthesizer.cs			\
	./AppKit/NSSpellChecker.cs			\
	./AppKit/NSSound.cs				\
	./AppKit/NSTableView.cs				\
	./AppKit/NSTextField.cs				\
        ./AppKit/NSToolbarItem.cs                       \
	./AppKit/NSTreeController.cs			\
	./AppKit/NSView.cs				\
        ./AppKit/NSWindow.cs				\
	./CoreAnimation/CAEmitterCell.cs		\
	./CoreVideo/CVDisplayLink.cs			\
	./Foundation/ObjCException.cs			\
	./QTKit/QTCaptureDevice.cs			\
	./QTKit/QTMovie.cs				\
	./WebKit/DomNode.cs				\
	./WebKit/Enumerators.cs				\
	./WebKit/Indexers.cs				\
	./WebKit/WebPolicyDelegate.cs			\
	./WebKit/WebKit.cs				\
	./WebKit/WebView.cs				\
	./Security/Authorization.cs

MONOMAC_SOURCES = $(BASE_SOURCES)		\
	./OpenGL/FrameEventArgs.cs			\
	./OpenGL/IGameWindow.cs				\
	./OpenGL/INativeWindow.cs			\
	./OpenGL/WindowBorder.cs			\
	./OpenGL/WindowState.cs				\
	./OpenGL/MonoMacGameView.cs			

OPENTK_SOURCES = \
	./OpenGL/OpenTK/Graphics/Color4.cs		\
	./OpenGL/OpenTK/Graphics/OpenGL/GL.cs		\
	./OpenGL/OpenTK/Graphics/OpenGL/GLCore.cs	\
	./OpenGL/OpenTK/Graphics/OpenGL/GLEnums.cs	\
	./OpenGL/OpenTK/Graphics/OpenGL/GLHelper.cs	\
	./OpenGL/OpenTK/Audio/ContextHandle.cs	\
	./OpenGL/OpenTK/Audio/OpenAL/AL/ALEnums.cs	\
	./OpenGL/OpenTK/Audio/BlittableValueType.cs	\
	./OpenGL/OpenTK/Audio/OpenAL/AL/AL.cs	\
	./OpenGL/OpenTK/Audio/OpenAL/Alc/AlcEnums.cs	\
	./OpenGL/OpenTK/Audio/OpenAL/Alc/Alc.cs	\
	./OpenGL/OpenTK/Math/Half.cs			\
	./OpenGL/OpenTK/Math/Matrix3d.cs		\
	./OpenGL/OpenTK/Math/Matrix4d.cs		\
	./OpenGL/OpenTK/Math/Quaterniond.cs		\
	./OpenGL/OpenTK/Math/Vector2d.cs		\
	./OpenGL/OpenTK/Math/Vector2h.cs		\
	./OpenGL/OpenTK/Math/Vector3d.cs		\
	./OpenGL/OpenTK/Math/Vector3h.cs		\
	./OpenGL/OpenTK/Math/Vector4d.cs		\
	./OpenGL/OpenTK/Math/Vector4h.cs

CFNETWORK_SOURCES = \
	$(MACCORE_DIR)/src/CFNetwork/Content.cs			\
	$(MACCORE_DIR)/src/CFNetwork/MessageHandler.cs		\
	$(MACCORE_DIR)/src/CFNetwork/WebRequestStream.cs	\
	$(MACCORE_DIR)/src/CFNetwork/WebResponseStream.cs	\
	$(MACCORE_DIR)/src/CFNetwork/WorkerThread.cs

GENERATOR_SOURCES = \
	$(MACCORE_DIR)/src/error.cs				\
	$(MACCORE_DIR)/src/generator.cs			\
	$(MACCORE_DIR)/src/Options.cs			\
	$(MACCORE_DIR)/src/btouch.cs

SOURCES = \
	$(CORE_SOURCES)

DEFINES = -define:NET_2_0
PKG_CONFIG=/Library/Frameworks/Mono.framework/Commands/pkg-config
ifeq ($(HAVE_ASYNC),)
HAVE_ASYNC := $(shell if $(PKG_CONFIG) --atleast-version=2.11 mono; then echo 1; else echo 0; fi)
endif

ifeq ($(HAVE_ASYNC),1)
	ASYNC_TARGETS = MonoMac.CFNetwork.dll
	UPDATE_FILES = MonoMac.dll* MonoMac.CFNetwork.dll*
	MCS ?= mcs -sdk:4
else
	ASYNC_TARGETS = 
	UPDATE_FILES = MonoMac.dll*
	MCS ?= dmcs
endif

ifeq ($(COREFX),1)
	64BIT=1
	DEFINES64 += -define:COREFX
	BMAC_DEFINES64 += -d=COREFX
	MONOMAC_SOURCES = $(BASE_SOURCES)
	MONOMAC_DEFINES = -nostdlib -define:COREFX -lib:$(COREFX_LIB) -r:mscorlib -r:System.Xml.XmlDocument -r:System.Xml.XmlSerializer -r:System.ComponentModel -r:System.Linq -r:System.Runtime -r:System.Runtime.InteropServices -r:System.Net.Sockets -r:System.Security.Cryptography.X509Certificates -r:System.Reflection -r:System.Diagnostics.Debug -r:System.Net.Http -r:System.Collections
	CFNETWORK_DEFINES = $(MONOMAC_DEFINES) -r:System.Text.RegularExpressions -r:System.Collections.Concurrent -r:System.IO
	OPENTK_SOURCES =
	# skip MonoMac.CFNetwork for now, seems System.Uri is private, but needed for HttpContentHeaders.ContentLocation, among other things.
	ASYNC_TARGETS =
	UPDATE_FILES = MonoMac.dll*
else
	MONOMAC_DEFINES = -r:System.Drawing
	CFNETWORK_DEFINES = -r:System.Net.Http
endif

# Pass 64BIT=1 to make to build for 64-bit
ifeq ($(64BIT),1)
	DEFINES64 += -define:MAC64
	BMAC_DEFINES64 += -d=MAC64
	SDCOMPAT=0
endif
ifeq ($(SDCOMPAT),1)
	DEFINES64 += -define:SDCOMPAT
	BMAC_DEFINES64 += -d=SDCOMPAT
endif

TARGETS = _bmac.exe $(TARGET) parse.exe bmac.exe $(ASYNC_TARGETS)

include Make.shared

SHARED_API = \
	./avfoundation.cs	\
	./foundation.cs		\
	./coreanimation.cs	\
	./coredata.cs		\
	./coreimage.cs		\
	./coremidi.cs		\
	./corelocation.cs	\
	./imageio.cs		\
	./quicklook.cs

SHARED_SOURCES = $(patsubst %,$(MACCORE_DIR)/src/%,$(SHARED_SOURCE))
SHARED_CORE_SOURCES = $(patsubst %,$(MACCORE_DIR)/src/%,$(SHARED_CORE_SOURCE))
SHARED_APIS = $(patsubst %,$(MACCORE_DIR)/src/%,$(SHARED_API))

ifneq ($(PEER),false)
include $(PEER)/Make.extra
endif

APIS = \
	appkit.cs		\
	corewlan.cs		\
	foundation-desktop.cs 	\
	imagekit.cs		\
	qtkit.cs		\
	pdfkit.cs		\
	webkit.cs		\
	composer.cs		\
	scriptingbridge.cs	\
	$(SHARED_APIS) $(EXTRA_APIS)


all: info $(TARGETS)

info:
ifeq ($(COREFX),1)
ifeq ($(COREFX_LIB),)
	@echo "Error: You must set the COREFX_LIB variable to the CoreFX library path"
	@exit 1
endif
	@echo "BUILDING 64-bit, CoreFX...";
else ifeq ($(64BIT),1)
	@echo "BUILDING 64-bit...";
else
	@echo "BUILDING 32-bit...";
endif
ifeq ($(SDCOMPAT),1)
	@echo "> Using System.Drawing structs";
else
	@echo "> Using CG structs";
endif
	@sleep 1

parse.exe: parse.cs $(MACCORE_DIR)/src/Options.cs
	$(MCS) -debug parse.cs $(MACCORE_DIR)/src/Options.cs

core.dll: $(CORE_SOURCES) $(SHARED_CORE_SOURCES) Makefile
	$(MCS) -target:library -debug -unsafe -out:core.dll $(DEFINES64) -define:MONOMAC,MONOMAC_BOOTSTRAP,COREBUILD$(CORE_DLL_DEFINES) $(CORE_SOURCES) $(SHARED_CORE_SOURCES) -r:System.Drawing

_bmac.exe: $(APIS) core.dll $(GENERATOR_SOURCES)
	$(MCS) -unsafe -out:_bmac.exe -debug $(DEFINES) $(DEFINES64) -define:GENERATOR,MONOMAC,MONOMAC_BOOTSTRAP $(GENERATOR_SOURCES) -r:core.dll -r:System.Drawing

generated_sources: _bmac.exe $(APIS) 
	mono --debug _bmac.exe -a -d=MONOMAC $(BMAC_DEFINES64) $(BMAC_DEFINES) --core --sourceonly=generated_sources -v -tmpdir=. $(APIS) --baselib=core.dll --ns=MonoMac.ObjCRuntime -r=System.Drawing $(BMAC_OPTIONS)

bmac.exe: $(TARGET) $(GENERATOR_SOURCES)
	$(MCS) -unsafe -out:bmac.exe -debug $(DEFINES) $(DEFINES64) -define:GENERATOR,MONOMAC,MONOMAC_BOOTSTRAP $(GENERATOR_SOURCES) -r:$(TARGET) -r:System.Drawing

count: 
	make pass | wc -l

counts:
	make pass | grep 'could not be' | sed 's/.*`//' | sed "s/'.*//" | sort | uniq -c | sort -n

$(TARGET): $(SOURCES) generated_sources $(MONOMAC_SOURCES) $(SHARED_SOURCES) $(SHARED_CORE_SOURCES) $(OPENTK_SOURCES)
	$(MCS) -define:OBJECT_REF_TRACKING $(DEFINES64) $(MONOMAC_DEFINES) -nowarn:3021,0219,0414,$(WARNINGS_I_SHOULD_FIX) -debug -unsafe $(SOURCES) @generated_sources $(MONOMAC_SOURCES) $(SHARED_SOURCES) $(SHARED_CORE_SOURCES) $(OPENTK_SOURCES) -target:library -define:MONOMAC -out:$(TARGET) 

MonoMac.CFNetwork.dll: $(CFNETWORK_SOURCES) $(TARGET)
	mcs -debug -target:library -define:MONOMAC $(DEFINES64) $(CFNETWORK_DEFINES) -r:$(TARGET) -out:MonoMac.CFNetwork.dll $(CFNETWORK_SOURCES)

clean:
	rm -f $(foreach t,$(TARGETS) core.dll temp.dll,$(t) $(t).mdb)
	if [ -a generated_sources ] ; then  xargs rm < generated_sources; fi
	rm -f generated_sources

all: $(TARGET) $(ASYNC_TARGETS)

update: MonoMac.dll $(ASYNC_TARGETS)
	cp $(UPDATE_FILES) /Applications/MonoDevelop.app/Contents/MacOS/lib/monodevelop/Addins
	cp $(UPDATE_FILES) /Applications/MonoDevelop.app/Contents/MacOS/lib/monodevelop/Addins/MonoDevelop.MonoMac

#
# Pushes this to the MonoDevelop source code repo
#
push-to-monodevelop: MonoMac.dll
	cp $(UPDATE_FILES) /cvs/md/monodevelop/extras/MonoDevelop.MonoMac/MonoDevelop.MonoMac

#
# Pushes the assembly to the system MonoDevelop, for testing
#
upgrade-monodevelop: MonoMac.dll
	cp $(UPDATE_FILES) /Applications/MonoDevelop.app/Contents/MacOS//lib/monodevelop/AddIns/MonoDevelop.MonoMac/
