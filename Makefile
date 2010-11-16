DIRS = src samples

all:
	for i in $(DIRS); do (cd $$i; make); done

update-docs:
	mdoc update $(MDOC_UPDATE_OPTIONS) -o docs/en src/MonoMac.dll