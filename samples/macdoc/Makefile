
MDTOOL_MASTER = $(MONODEVELOP_DIR)/main/build/bin/mdtool.exe
MDTOOL_SYSTEM = /Applications/MonoDevelop.app/Contents/MacOS/mdtool

# If invoked from CI, we pass the just built MonoDevelop as mdtool
ifeq ($(wildcard $(MDTOOL_MASTER)),)
    MDTOOL_BUILD = $(MDTOOL_SYSTEM) build
else
    MDTOOL_BUILD = mono $(MDTOOL_MASTER) setup reg-build && mono $(MDTOOL_MASTER) build
endif

MONO_MAC_DLL = ../../src/MonoMac.dll
APPLEDOCWIZARD_APP = AppleDocWizard/bin/Debug/AppleDocWizard.app
MACDOC_APP = bin/Debug/macdoc.app
MONODOC_APP = $(dir $(MACDOC_APP))/MonoDoc.app
MONODOC_ARCHIVE = MonoDoc.tar.bz2

all: monomac macdoc monodoc

monomac: $(MONO_MAC_DLL)

macdoc: monomac
	@echo "MDTOOL_BUILD: " $(MDTOOL_BUILD)
	rm -Rf $(MACDOC_APP)
	$(MDTOOL_BUILD)

monodoc: macdoc
	rm -Rf $(MONODOC_APP)
	cp -R $(MACDOC_APP) $(MONODOC_APP)

dist: monodoc
	(cd $(dir $(MONODOC_APP)) && rm -f $(MONODOC_ARCHIVE) && tar -cjf $(MONODOC_ARCHIVE) $(notdir $(MONODOC_APP)))

$(MONO_MAC_DLL):
	(cd ../../src/ && make)
