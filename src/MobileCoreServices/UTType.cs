// MobileCoreServices.UTType
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;

namespace MonoMac.MobileCoreServices {
	
	public class UTType {
		
		public static NSString ExportedTypeDeclarationsKey => _ExportedTypeDeclarationsKey ?? (_ExportedTypeDeclarationsKey = GetString("kUTTypeExportedTypeDeclarationsKey"));
		public static NSString ImportedTypeDeclarationsKey => _ImportedTypeDeclarationsKey ?? (_ImportedTypeDeclarationsKey = GetString("kUTTypeImportedTypeDeclarationsKey"));
		public static NSString IdentifierKey => _IdentifierKey ?? (_IdentifierKey = GetString("kUTTypeIdentifierKey"));
		public static NSString TagSpecificationKey => _TagSpecificationKey ?? (_TagSpecificationKey = GetString("kUTTypeTagSpecificationKey"));
		public static NSString ConformsToKey => _ConformsToKey ?? (_ConformsToKey = GetString("kUTTypeConformsToKey"));
		public static NSString DescriptionKey => _DescriptionKey ?? (_DescriptionKey = GetString("kUTTypeDescriptionKey"));
		public static NSString IconFileKey => _IconFileKey ?? (_IconFileKey = GetString("kUTTypeIconFileKey"));
		public static NSString ReferenceURLKey => _ReferenceURLKey ?? (_ReferenceURLKey = GetString("kUTTypeReferenceURLKey"));
		public static NSString VersionKey => _VersionKey ?? (_VersionKey = GetString("kUTTypeVersionKey"));

		public static NSString TagClassFilenameExtension => _TagClassFilenameExtension ?? (_TagClassFilenameExtension = GetString("kUTTypeTagClassFilenameExtension"));
		public static NSString TagClassMIMEType => _TagClassMIMEType ?? (_TagClassMIMEType = GetString("kUTTypeTagClassMIMEType"));
#if MONOMAC
		public static NSString TagClassNSPboardType => _TagClassNSPboardType ?? (_TagClassNSPboardType = GetString("kUTTypeTagClassNSPboardType"));
		public static NSString TagClassOSType => _TagClassOSType ?? (_TagClassOSType = GetString("kUTTypeTagClassOSType"));
#endif
		public static NSString Item => _Item ?? (_Item = GetString("kUTTypeItem"));
		public static NSString Content => _Content ?? (_Content = GetString("kUTTypeContent"));
		public static NSString CompositeContent => _CompositeContent ?? (_CompositeContent = GetString("kUTTypeCompositeContent"));
		public static NSString Application => _Application ?? (_Application = GetString("kUTTypeApplication"));
		public static NSString Message => _Message ?? (_Message = GetString("kUTTypeMessage"));
		public static NSString Contact => _Contact ?? (_Contact = GetString("kUTTypeContact"));
		public static NSString Archive => _Archive ?? (_Archive = GetString("kUTTypeArchive"));
		public static NSString DiskImage => _DiskImage ?? (_DiskImage = GetString("kUTTypeDiskImage"));

		public static NSString Data => _Data ?? (_Data = GetString("kUTTypeData"));
		public static NSString Directory => _Directory ?? (_Directory = GetString("kUTTypeDirectory"));
		public static NSString Resolvable => _Resolvable ?? (_Resolvable = GetString("kUTTypeResolvable"));
		public static NSString SymLink => _SymLink ?? (_SymLink = GetString("kUTTypeSymLink"));
		public static NSString MountPoint => _MountPoint ?? (_MountPoint = GetString("kUTTypeMountPoint"));
		public static NSString AliasFile => _AliasFile ?? (_AliasFile = GetString("kUTTypeAliasFile"));
		public static NSString AliasRecord => _AliasRecord ?? (_AliasRecord = GetString("kUTTypeAliasRecord"));
		public static NSString URL => _URL ?? (_URL = GetString("kUTTypeURL"));
		public static NSString FileURL => _FileURL ?? (_FileURL = GetString("kUTTypeFileURL"));

		public static NSString Text => _Text ?? (_Text = GetString("kUTTypeText"));
		public static NSString PlainText => _PlainText ?? (_PlainText = GetString("kUTTypePlainText"));
		public static NSString UTF8PlainText => _UTF8PlainText ?? (_UTF8PlainText = GetString("kUTTypeUTF8PlainText"));
		public static NSString UTF16ExternalPlainText => _UTF16ExternalPlainText ?? (_UTF16ExternalPlainText = GetString("kUTTypeUTF16ExternalPlainText"));
		public static NSString UTF16PlainText => _UTF16PlainText ?? (_UTF16PlainText = GetString("kUTTypeUTF16PlainText"));
		public static NSString RTF => _RTF ?? (_RTF = GetString("kUTTypeRTF"));
		public static NSString HTML => _HTML ?? (_HTML = GetString("kUTTypeHTML"));
		public static NSString XML => _XML ?? (_XML = GetString("kUTTypeXML"));
		public static NSString SourceCode => _SourceCode ?? (_SourceCode = GetString("kUTTypeSourceCode"));
		public static NSString CSource => _CSource ?? (_CSource = GetString("kUTTypeCSource"));
		public static NSString ObjectiveCSource => _ObjectiveCSource ?? (_ObjectiveCSource = GetString("kUTTypeObjectiveCSource"));
		public static NSString CPlusPlusSource => _CPlusPlusSource ?? (_CPlusPlusSource = GetString("kUTTypeCPlusPlusSource"));
		public static NSString ObjectiveCPlusPlusSource => _ObjectiveCPlusPlusSource ?? (_ObjectiveCPlusPlusSource = GetString("kUTTypeObjectiveCPlusPlusSource"));
		public static NSString CHeader => _CHeader ?? (_CHeader = GetString("kUTTypeCHeader"));
		public static NSString CPlusPlusHeader => _CPlusPlusHeader ?? (_CPlusPlusHeader = GetString("kUTTypeCPlusPlusHeader"));
		public static NSString JavaSource => _JavaSource ?? (_JavaSource = GetString("kUTTypeJavaSource"));

		public static NSString PDF => _PDF ?? (_PDF = GetString("kUTTypePDF"));
		public static NSString RTFD => _RTFD ?? (_RTFD = GetString("kUTTypeRTFD"));
		public static NSString FlatRTFD => _FlatRTFD ?? (_FlatRTFD = GetString("kUTTypeFlatRTFD"));
		public static NSString TXNTextAndMultimediaData => _TXNTextAndMultimediaData ?? (_TXNTextAndMultimediaData = GetString("kUTTypeTXNTextAndMultimediaData"));
		public static NSString WebArchive => _WebArchive ?? (_WebArchive = GetString("kUTTypeWebArchive"));

		public static NSString Image => _Image ?? (_Image = GetString("kUTTypeImage"));
		public static NSString JPEG => _JPEG ?? (_JPEG = GetString("kUTTypeJPEG"));
		public static NSString JPEG2000 => _JPEG2000 ?? (_JPEG2000 = GetString("kUTTypeJPEG2000"));
		public static NSString TIFF => _TIFF ?? (_TIFF = GetString("kUTTypeTIFF"));
		public static NSString PICT => _PICT ?? (_PICT = GetString("kUTTypePICT"));
		public static NSString GIF => _GIF ?? (_GIF = GetString("kUTTypeGIF"));
		public static NSString PNG => _PNG ?? (_PNG = GetString("kUTTypePNG"));
		public static NSString QuickTimeImage => _QuickTimeImage ?? (_QuickTimeImage = GetString("kUTTypeQuickTimeImage"));
		public static NSString AppleICNS => _AppleICNS ?? (_AppleICNS = GetString("kUTTypeAppleICNS"));
		public static NSString BMP => _BMP ?? (_BMP = GetString("kUTTypeBMP"));
		public static NSString ICO => _ICO ?? (_ICO = GetString("kUTTypeICO"));

		public static NSString AudiovisualContent => _AudiovisualContent ?? (_AudiovisualContent = GetString("kUTTypeAudiovisualContent"));
		public static NSString Movie => _Movie ?? (_Movie = GetString("kUTTypeMovie"));
		public static NSString Video => _Video ?? (_Video = GetString("kUTTypeVideo"));
		public static NSString Audio => _Audio ?? (_Audio = GetString("kUTTypeAudio"));
		public static NSString QuickTimeMovie => _QuickTimeMovie ?? (_QuickTimeMovie = GetString("kUTTypeQuickTimeMovie"));
		public static NSString MPEG => _MPEG ?? (_MPEG = GetString("kUTTypeMPEG"));
		public static NSString MPEG4 => _MPEG4 ?? (_MPEG4 = GetString("kUTTypeMPEG4"));
		public static NSString MP3 => _MP3 ?? (_MP3 = GetString("kUTTypeMP3"));
		public static NSString MPEG4Audio => _MPEG4Audio ?? (_MPEG4Audio = GetString("kUTTypeMPEG4Audio"));
		public static NSString AppleProtectedMPEG4Audio => _AppleProtectedMPEG4Audio ?? (_AppleProtectedMPEG4Audio = GetString("kUTTypeAppleProtectedMPEG4Audio"));

		public static NSString Folder => _Folder ?? (_Folder = GetString("kUTTypeFolder"));
		public static NSString Volume => _Volume ?? (_Volume = GetString("kUTTypeVolume"));
		public static NSString Package => _Package ?? (_Package = GetString("kUTTypePackage"));
		public static NSString Bundle => _Bundle ?? (_Bundle = GetString("kUTTypeBundle"));
		public static NSString Framework => _Framework ?? (_Framework = GetString("kUTTypeFramework"));

		public static NSString ApplicationBundle => _ApplicationBundle ?? (_ApplicationBundle = GetString("kUTTypeApplicationBundle"));
		public static NSString ApplicationFile => _ApplicationFile ?? (_ApplicationFile = GetString("kUTTypeApplicationFile"));

		public static NSString VCard => _VCard ?? (_VCard = GetString("kUTTypeVCard"));

		public static NSString InkText => _InkText ?? (_InkText = GetString("kUTTypeInkText"));
		
		
		static NSString _ExportedTypeDeclarationsKey;
		static NSString _ImportedTypeDeclarationsKey;
		static NSString _IdentifierKey;
		static NSString _TagSpecificationKey;
		static NSString _ConformsToKey;
		static NSString _DescriptionKey;
		static NSString _IconFileKey;
		static NSString _ReferenceURLKey;
		static NSString _VersionKey;

		static NSString _TagClassFilenameExtension;
		static NSString _TagClassMIMEType;
#if MONOMAC
		static NSString _TagClassNSPboardType;
		static NSString _TagClassOSType;
#endif
		static NSString _Item;
		static NSString _Content;
		static NSString _CompositeContent;
		static NSString _Application;
		static NSString _Message;
		static NSString _Contact;
		static NSString _Archive;
		static NSString _DiskImage;

		static NSString _Data;
		static NSString _Directory;
		static NSString _Resolvable;
		static NSString _SymLink;
		static NSString _MountPoint;
		static NSString _AliasFile;
		static NSString _AliasRecord;
		static NSString _URL;
		static NSString _FileURL;

		static NSString _Text;
		static NSString _PlainText;
		static NSString _UTF8PlainText;
		static NSString _UTF16ExternalPlainText;
		static NSString _UTF16PlainText;
		static NSString _RTF;
		static NSString _HTML;
		static NSString _XML;
		static NSString _SourceCode;
		static NSString _CSource;
		static NSString _ObjectiveCSource;
		static NSString _CPlusPlusSource;
		static NSString _ObjectiveCPlusPlusSource;
		static NSString _CHeader;
		static NSString _CPlusPlusHeader;
		static NSString _JavaSource;

		static NSString _PDF;
		static NSString _RTFD;
		static NSString _FlatRTFD;
		static NSString _TXNTextAndMultimediaData;
		static NSString _WebArchive;

		static NSString _Image;
		static NSString _JPEG;
		static NSString _JPEG2000;
		static NSString _TIFF;
		static NSString _PICT;
		static NSString _GIF;
		static NSString _PNG;
		static NSString _QuickTimeImage;
		static NSString _AppleICNS;
		static NSString _BMP;
		static NSString _ICO;

		static NSString _AudiovisualContent;
		static NSString _Movie;
		static NSString _Video;
		static NSString _Audio;
		static NSString _QuickTimeMovie;
		static NSString _MPEG;
		static NSString _MPEG4;
		static NSString _MP3;
		static NSString _MPEG4Audio;
		static NSString _AppleProtectedMPEG4Audio;

		static NSString _Folder;
		static NSString _Volume;
		static NSString _Package;
		static NSString _Bundle;
		static NSString _Framework;

		static NSString _ApplicationBundle;
		static NSString _ApplicationFile;

		static NSString _VCard;

		static NSString _InkText;

		
		static IntPtr handle = Dlfcn.dlopen (Constants.CoreServicesLibrary, 0);
		
		static NSString GetString(string name)
		{
			return Dlfcn.GetStringConstant (handle, name);
		}
	}
}