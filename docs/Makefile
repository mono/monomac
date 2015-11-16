MACCORE=../../maccore/tools/docfixer

.PHONY: assemble docs update-docs clean-docs install-docs

DOCS_LIB = monomac-lib.tree monomac-lib.zip
DOCS_ASSEMBLIES = ../src/monomac.dll

docs: $(DOCS_MAN) $(DOCS_LIB)


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
	cp $(DOCS_MAN) monomac-docs.source $(docdir)

install-monodoc-docs: $(DOCS_MAN) $(DOCS_LIB) monomac-docs.source
install -m 644 $^ `pkg-config --variable=sourcesdir monodoc`

clean-docs:
	-rm $(DOCS_MAN) $(DOCS_LIB)

push-docs: docs
	scp $(DOCS_MAN) $(DOCS_LIB) monomac-docs.source root@www.go-mono.com:/usr/lib/monodoc/sources

monomac-lib.tree: monomac-lib.zip
monomac-lib.zip : Makefile
	mdoc assemble -o $(basename $@) en

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
	MONO_PATH=../src mono --debug populate.exe .

populate.exe: populate.cs
	dmcs populate.cs -r:../src/MonoMac.dll -r:System.Xml.Linq

docfixer.exe: $(MACCORE)/docfixer.cs $(MACCORE)/docfixer.mm.cs AgilityPack.dll
	gmcs -out:$@ -debug+ $(MACCORE)/docfixer.cs $(MACCORE)/docfixer.mm.cs -r:AgilityPack.dll -r:../src/MonoMac.dll -r:System.Xml.Linq -r:System.Xml

dgc: document-generated-code

document-generated-code: document-generated-code.exe
	MONO_PATH=../src mono --debug document-generated-code.exe ../src/temp.dll .

document-generated-code.exe: $(MACCORE)/document-generated-code.cs AgilityPack.dll $(MACCORE)/docfixer.cs $(MACCORE)/docfixer.mm.cs
	dmcs -debug -main:DocumentGeneratedCode -out:document-generated-code.exe $(MACCORE)/document-generated-code.cs $(MACCORE)/docfixer.cs $(MACCORE)/docfixer.mm.cs -r:System.Xml.Linq -r:../src/_bmac.exe -r:../src/core.dll -define:MONOMAC -r:AgilityPack.dll

run: docfixer.exe 
	-mkdir deploy
	rsync -a en deploy
	chmod -R +rw deploy
	MONO_PATH=../src mono --debug docfixer.exe --summary deploy

#
# this second stage documents other bits, using the metadata from the API definition file, not the resulting assembly
#
run2: document-generated-code.exe
	MONO_PATH=../src mono --debug document-generated-code.exe --appledocs ../src/temp.dll deploy

push:
	rsync -a deploy/en/ root@mono.ximian.com:/srv/www/mono-website/monomac-docs/Docs/monomac.dll
AgilityPack.dll: $(MACCORE)/AgilityPack.dll
	cp $< $@

audit: document-generated-code.exe
	MONO_PATH=../src mono --debug document-generated-code.exe --debugdoc  ../src/temp.dll deploy > log
