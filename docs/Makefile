MACCORE=../../maccore/tools/docfixer

.PHONY: assemble docs update-docs clean-docs install-docs

DOCS_LIB = monomac-lib.tree monomac-lib.zip
DOCS_ASSEMBLIES = ../src/monomac.dll

docs: $(DOCS_MAN) $(DOCS_LIB)


update-docs:
	mdoc update $(MDOC_UPDATE_OPTIONS) -o en $(DOCS_ASSEMBLIES)

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
	for i in en/*/*.xml; do d=`echo $$i | sed -e 's,MonoMac,MonoTouch,' -e s,^,../../iphone/docs/,`; echo $$d; if test -f $$d; then sed -e 's/MonoTouch/MonoMac/g' -e 's/<AssemblyName>monotouch/<AssemblyName>monomac/' < $$d > $$i; fi; done

populate: populate.exe
	MONO_PATH=../src mono --debug populate.exe .

populate.exe: populate.cs
	gmcs populate.cs -r:../src/MonoMac.dll -r:System.Xml.Linq

docfixer.exe: $(MACCORE)/docfixer.cs $(MACCORE)/docfixer.mm.cs AgilityPack.dll
	gmcs -out:$@ -debug+ $(MACCORE)/docfixer.cs $(MACCORE)/docfixer.mm.cs -r:AgilityPack.dll -r:../src/MonoMac.dll -r:System.Xml.Linq -r:System.Xml

run: docfixer.exe
	MONO_PATH=../src mono docfixer.exe .

AgilityPack.dll: $(MACCORE)/AgilityPack.dll
	cp $< $@