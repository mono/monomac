CC = gcc
MDTOOL = /Applications/MonoDevelop.app/Contents/MacOS/mdtool
MDTOOL_BUILD = $(MDTOOL) build
MONO_MAC_DLL = ../../src/MonoMac.dll
APPLEDOCWIZARD_APP = AppleDocWizard/bin/Debug/AppleDocWizard.app/
MACDOC_APP = bin/Debug/macdoc.app
MONODOC_APP = $(dir $(MACDOC_APP))/MonoDoc.app
MONODOC_ARCHIVE = MonoDoc.tar.bz2

all: monomac monostub appledocwizard macdoc monodoc

monomac: $(MONO_MAC_DLL)

appledocwizard: monostub
	(cd AppleDocWizard && $(MDTOOL_BUILD))
	cp monostub $(APPLEDOCWIZARD_APP)/Contents/MacOS/AppleDocWizard
	rm -f $(APPLEDOCWIZARD_APP)/AppleDocWizard
	rm -f $(APPLEDOCWIZARD_APP)/Contents/MacOS/mono-version-check

macdoc: appledocwizard monostub monomac
	$(MDTOOL_BUILD)
	cp monostub $(MACDOC_APP)/Contents/MacOS/macdoc
	rm -f $(MACDOC_APP)/macdoc
	rm -f $(MACDOC_APP)/Contents/MacOS/mono-version-check
	cp -R $(APPLEDOCWIZARD_APP) $(MACDOC_APP)/Contents/MacOS/

monodoc: macdoc
	cp -R $(MACDOC_APP) $(MONODOC_APP)

dist: monodoc
	(cd $(dir $(MONODOC_APP)) && rm -f $(MONODOC_ARCHIVE) && tar -cjf $(MONODOC_ARCHIVE) $(notdir $(MONODOC_APP)))

$(MONO_MAC_DLL):
	(cd ../../src/ && make)

monostub: monostub.m
	$(CC) -m32 $^ -o $@ -framework AppKit -D_GNU_SOURCE