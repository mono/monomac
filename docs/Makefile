# Path to doc fixer and dependencies
MACCORE=../../maccore/tools/docfixer

# Path to target framework binaries (e.g. MonoMac, MonoTouch)
BINROOT=../src

# Path to temp interface assembly used for binding
BINDING_LIB=$(BINROOT)/temp.dll

# Path to MonoMac / MonoTouch
FXLIB=$(BINROOT)/MonoMac.dll

DEFINES=-define:MONOMAC

.PHONY: assemble docs update-docs clean-docs install-docs

BIND_TOOL = _bmac.exe
DOCS_IDENTIFIER = monomac-docs
DOCS_TREE = monomac-lib.tree
DOCS_ZIP = monomac-lib.zip
DOCS_SOURCE = monomac-docs.source
DOCS_LIB = $(DOCS_TREE) $(DOCS_ZIP)
DOCS_PKG = ./MonoMacDocs.pkg
DOCS_ASSEMBLIES = $(FXLIB)
DOCS_INSTALL = /Library/Frameworks/Mono.framework/External/monodoc
DOCS_INSTALL2 = `pkg-config --variable=sourcesdir monodoc`
APPLE_OSX_DOCS = `echo ~/Library/Developer/Shared/Documentation/DocSets/com.apple.adc.documentation.AppleOSX10_8.CoreReference.docset`
APPLE_IOS_DOCS = `echo ~/Library/Developer/Shared/Documentation/DocSets/com.apple.adc.documentation.AppleiOS6.1.iOSLibrary.docset`
NS_PREFIX=MonoMac

docs: $(DOCS_MAN) $(DOCS_LIB)

# Steps to build
#  1. [update-docs] Update existing XML docs by applying assembly changes -> "en"
#  2. [stub-docs] Update stubs that can be generated -> "en"
#  3. [merge-apple-docs] Merge in Apple documentation -> "deploy/en"
#  4. [docs] Build monodoc bundle from "deploy/en"
#  5. Optionally [install-monodoc-docs] or [pkg] to an installer
build-docs: update-docs stub-docs merge-apple-docs docs

stub-docs: populate dgc

merge-apple-docs: run run2

update-docs:
	mdoc update --delete $(MDOC_UPDATE_OPTIONS) -o en $(DOCS_ASSEMBLIES)

update-docs-svn:
	`grep -v '.xml.s'` is to skip vim temporary files, e.g. '.foo.xml.swp'
	svn stat en | grep -v '.xml.s' | grep '^!' | cut -c 8- | xargs svn rm
	svn stat en | grep -v '.xml.s' | grep -v '.remove$$' | grep '^?' \
		| cut -c 8- | xargs svn add


docdir = $(MONOMAC_PREFIX)/usr/share/doc/monomac

install-docs: $(DOCS_MAN)
	-mkdir -p $(MONOMAC_PREFIX)/usr/share/doc/monomac
	(cd docs ; find en -name '*.xml' > .files)
	(cd docs ; tar cTf .files - ) | (cd $(docdir) ; tar xf - )
	rm .files
	cp $(DOCS_MAN) $(DOCS_SOURCE) $(docdir)

# Must be run as sudo
install-monodoc-docs: $(DOCS_MAN) $(DOCS_LIB) monomac-docs.source
	install -m 644 $^ $(DOCS_INSTALL)

clean-docs:
	-rm $(DOCS_MAN) $(DOCS_LIB)

push-docs: docs
	scp $(DOCS_MAN) $(DOCS_LIB) monomac-docs.source root@www.go-mono.com:/usr/lib/monodoc/sources

$(DOCS_TREE): $(DOCS_ZIP)
$(DOCS_ZIP): Makefile deploy/en
	mdoc assemble -o $(basename $@) deploy/en

fetch-shared-docs:
	make fetch-docs TARGET=.

fetch-docs-to-compare:
	-rm -rf compare
	-mkdir compare
	-cp -pr en compare/
	make fetch-docs TARGET=compare

fetch-docs:
	for i in en/*/*.xml; do d=`echo $$i | sed -e 's,MonoMac,MonoTouch,' -e s,^,../../monotouch/docs/,`; if test $$i != en/MonoMac.Security/SecKeyChain.xml -a $$i != en/MonoMac/Constants.xml; then if test -f $$d; then echo $$d; sed -e 's/MonoTouch/MonoMac/g' -e 's/<AssemblyName>monotouch/<AssemblyName>monomac/' < $$d > $(TARGET)/$$i; fi; fi; done

populate: populate.exe
	MONO_PATH=$(BINROOT) mono --debug populate.exe .

populate.exe: populate.cs
	dmcs populate.cs -r:$(FXLIB) -r:System.Xml.Linq

docfixer.exe: $(MACCORE)/docfixer.cs $(MACCORE)/AppleDocMerger.cs $(MACCORE)/AppleDocKnownIssues.cs $(MACCORE)/Options.cs $(MACCORE)/MDocArchive.cs $(MACCORE)/samples.cs $(MACCORE)/SQLite.cs AgilityPack.dll Mono.Cecil.dll Ionic.Zip.dll $(FXLIB)
	dmcs -out:$@ -debug+ $(MACCORE)/docfixer.cs $(MACCORE)/AppleDocMerger.cs $(MACCORE)/AppleDocKnownIssues.cs $(MACCORE)/Options.cs $(MACCORE)/MDocArchive.cs $(MACCORE)/samples.cs $(MACCORE)/SQLite.cs -r:AgilityPack.dll -r:$(FXLIB) -r:System.Xml.Linq -r:System.Xml -r:$(MACCORE)/Mono.Cecil.dll -r:$(MACCORE)/Ionic.Zip.dll

dgc: document-generated-code

document-generated-code: document-generated-code.exe
	MONO_PATH=$(BINROOT) mono --debug document-generated-code.exe --ns-prefix=$(NS_PREFIX) $(BINDING_LIB) .

document-generated-code.exe: $(MACCORE)/document-generated-code.cs $(MACCORE)/docfixer.cs $(MACCORE)/AppleDocMerger.cs $(MACCORE)/AppleDocKnownIssues.cs $(MACCORE)/MDocArchive.cs $(MACCORE)/samples.cs $(MACCORE)/SQLite.cs AgilityPack.dll Mono.Cecil.dll Ionic.Zip.dll $(BINROOT)/$(BIND_TOOL) $(BINROOT)/core.dll
	dmcs -debug -main:DocumentGeneratedCode -out:document-generated-code.exe $(MACCORE)/document-generated-code.cs $(MACCORE)/docfixer.cs $(MACCORE)/AppleDocMerger.cs $(MACCORE)/AppleDocKnownIssues.cs $(MACCORE)/MDocArchive.cs $(MACCORE)/samples.cs $(MACCORE)/SQLite.cs -r:System.Xml.Linq -r:$(BINROOT)/$(BIND_TOOL) -r:$(BINROOT)/core.dll -r:AgilityPack.dll -r:$(MACCORE)/Mono.Cecil.dll -r:$(MACCORE)/Ionic.Zip.dll $(DEFINES)

pkg: $(DOCS_ZIP)
	-mkdir deploy/pkgroot
	cp $(DOCS_TREE) deploy/pkgroot
	cp $(DOCS_ZIP) deploy/pkgroot
	cp $(DOCS_SOURCE) deploy/pkgroot
	pkgbuild --identifier $(DOCS_IDENTIFIER) --root deploy/pkgroot --install-location $(DOCS_INSTALL) $(PKG_SIGN) $(DOCS_PKG)

run: docfixer.exe 
	-mkdir deploy
	rsync -a en deploy
	chmod -R +rw deploy
	MONO_PATH=$(BINROOT) mono --debug docfixer.exe --allow-missing=300 --use-raw-doc --reference-assembly=$(FXLIB) --ns-prefix=$(NS_PREFIX) --apple-doc-dir=$(APPLE_OSX_DOCS) deploy

#
# this second stage documents other bits, using the metadata from the API definition file, not the resulting assembly
#
run2: document-generated-code.exe
	MONO_PATH=$(BINROOT) mono --debug document-generated-code.exe --ns-prefix=$(NS_PREFIX) --apple-doc-dir=$(APPLE_OSX_DOCS) $(BINDING_LIB) deploy

push:
	rsync -a deploy/en/ root@mono.ximian.com:/srv/www/mono-website/monomac-docs/Docs/monomac.dll
	
AgilityPack.dll: $(MACCORE)/AgilityPack.dll
	cp $< $@
	
Mono.Cecil.dll: $(MACCORE)/Mono.Cecil.dll
	cp $< $@

Ionic.Zip.dll: $(MACCORE)/Ionic.Zip.dll
	cp $< $@
	
audit: document-generated-code.exe
	MONO_PATH=$(BINROOT) mono --debug document-generated-code.exe --ns-prefix=$(NS_PREFIX) --debugdoc $(BINDING_LIB) deploy > log
