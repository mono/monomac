DIRS = src samples

all:
	for i in $(DIRS); do (cd $$i; make); done