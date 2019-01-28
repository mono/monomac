#!/bin/sh

msbuild MonoMac.sln /t:Restore
msbuild MonoMac.sln /t:Build /p:Configuration=Release /p:Platform=x64
msbuild MonoMac.sln /t:Build /p:Configuration=Release /p:Platform=x86
