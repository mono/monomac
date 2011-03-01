DEMOS = \
	generate-pdf.exe	\
	locator.exe		\
	HelloCoreWlanSample.exe		

all: $(DEMOS)

run1:
	MONO_PATH=../../src/ mono generate-pdf.exe

generate-pdf.exe: generate-pdf.cs
	mcs generate-pdf.cs -r:../../src/MonoMac.dll -r:System.Drawing

locator.exe: locator.cs
	mcs locator.cs -r:../../src/MonoMac.dll 

HelloCoreWlanSample.exe: HelloCoreWlanSample.cs
	mcs HelloCoreWlanSample.cs -r:../../src/MonoMac.dll
