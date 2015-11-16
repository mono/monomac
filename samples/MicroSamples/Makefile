MONOMAC_DLL = ../../src/MonoMac.dll

DEMOS = \
	generate-pdf.exe	\
	locator.exe		\
	HelloCoreWlanSample.exe		

.PHONY: all clean

all: $(DEMOS)

clean:
	rm -f *.dll *.exe demo.pdf

MonoMac.dll: $(MONOMAC_DLL)
	cp "$<" $@

generate-pdf.exe: MonoMac.dll generate-pdf.cs
	mcs generate-pdf.cs -r:MonoMac.dll -r:System.Drawing

locator.exe: MonoMac.dll locator.cs
	mcs locator.cs -r:MonoMac.dll

HelloCoreWlanSample.exe: MonoMac.dll HelloCoreWlanSample.cs
	mcs HelloCoreWlanSample.cs -r:MonoMac.dll
