//
// Copyright 2010, Novell, Inc.
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

//
// foundation-desktop.cs: Defines the API for the MonoMac specific
// Foundation APIs
// 
using System;
using System.Drawing;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;
using MonoMac.AppKit;

// In Apple headers, this is a typedef to a pointer to a private struct
using NSAppleEventManagerSuspensionID = System.IntPtr;
// These two are both four char codes i.e. defined on a uint with constant like 'xxxx'
using AEKeyword = System.UInt32;
using OSType = System.UInt32;

namespace MonoMac.Foundation {
	
	[BaseType (typeof (NSObject))]
	public interface NSAffineTransform {
		[Export ("initWithTransform:")]
		IntPtr Constructor (NSAffineTransform transform);

		[Export ("translateXBy:yBy:")]
		void Translate (float deltaX, float deltaY);

		[Export ("rotateByDegrees:")]
		void RotateByDegrees (float angle);

		[Export ("rotateByRadians:")]
		void RotateByRadians (float angle);

		[Export ("scaleBy:")]
		void Scale (float scale);

		[Export ("scaleXBy:yBy:")]
		void Scale (float scaleX, float scaleY);

		[Export ("invert")]
		void Invert ();

		[Export ("appendTransform:")]
		void AppendTransform (NSAffineTransform transform);

		[Export ("prependTransform:")]
		void PrependTransform (NSAffineTransform transform);

		[Export ("transformPoint:")]
		PointF TransformPoint (PointF aPoint);

		[Export ("transformSize:")]
		SizeF TransformSize (SizeF aSize);
		
		[Export ("transformBezierPath:")]
		NSBezierPath TransformBezierPath (NSBezierPath path);

		[Export ("set")]
		void Set ();

		[Export ("concat")]
		void Concat ();

		[Export ("transformStruct")]
		CGAffineTransform TransformStruct { get; set; }
	}
	
	// FAK Left off until I understand how to do structs
	//struct NSAffineTransformStruct {
	//	public float M11, M12, M21, M22;
	//	public float tX, tY;
	//}

	[BaseType (typeof (NSCharacterSet))]
	public interface NSMutableCharacterSet {
		[Export ("removeCharactersInRange:")]
		void RemoveCharacters (NSRange aRange);

		[Export ("addCharactersInString:")]
		void AddCharacters (string aString);

		[Export ("removeCharactersInString:")]
		void RemoveCharacters (string aString);

		[Export ("formUnionWithCharacterSet:")]
		void UnionWith (NSCharacterSet otherSet);

		[Export ("formIntersectionWithCharacterSet:")]
		void IntersectWith (NSCharacterSet otherSet);

		[Export ("invert")]
		void Invert ();

	}


	[BaseType (typeof (NSObject))]
	public interface NSOrthography {
		[Export ("dominantScript")]
		NSString DominantScript { get;  }

		[Export ("languageMap")]
		NSDictionary LanguageMap { get;  }

		[Export ("dominantLanguage")]
		string DominantLanguage { get;  }

		[Export ("allScripts")]
		string [] AllScripts { get;  }

		[Export ("allLanguages")]
		string [] AllLanguages { get;  }

		[Export ("languagesForScript:")]
		string [] LanguagesForScript (string script);

		[Export ("dominantLanguageForScript:")]
		string GetDominantLanguageForScript (string forScript);

		[Export ("initWithDominantScript:languageMap:")]
		IntPtr Constructor (string script, NSDictionary map);

	}

	[BaseType (typeof (NSObject))]
	public interface NSTextCheckingResult {
		[Export ("resultType")]
		NSTextCheckingType ResultType { get;  }

		[Export ("range")]
		NSRange Range { get;  }

	}

	[BaseType (typeof (NSObject))]
	public interface NSAppleEventDescriptor {
		[Static]
		[Export ("nullDescriptor")]
		NSAppleEventDescriptor NullDescriptor { get; }

		/*		[Static]
		[Export ("descriptorWithDescriptorType:bytes:length:")]
		NSAppleEventDescriptor DescriptorWithDescriptorTypebyteslength (DescType descriptorType, void bytes, uint byteCount);

		[Static]
		[Export ("descriptorWithDescriptorType:data:")]
		NSAppleEventDescriptor DescriptorWithDescriptorTypedata (DescType descriptorType, NSData data);*/

		[Static]
		[Export ("descriptorWithBoolean:")]
		NSAppleEventDescriptor DescriptorWithBoolean (Boolean boolean);

		[Static]
		[Export ("descriptorWithEnumCode:")]
		NSAppleEventDescriptor DescriptorWithEnumCode (OSType enumerator);

		[Static]
		[Export ("descriptorWithInt32:")]
		NSAppleEventDescriptor DescriptorWithInt32 (int signedInt);

		[Static]
		[Export ("descriptorWithTypeCode:")]
		NSAppleEventDescriptor DescriptorWithTypeCode (OSType typeCode);

		[Static]
		[Export ("descriptorWithString:")]
		NSAppleEventDescriptor DescriptorWithString (string str);

		/*[Static]
		[Export ("appleEventWithEventClass:eventID:targetDescriptor:returnID:transactionID:")]
		NSAppleEventDescriptor AppleEventWithEventClasseventIDtargetDescriptorreturnIDtransactionID (AEEventClass eventClass, AEEventID eventID, NSAppleEventDescriptor targetDescriptor, AEReturnID returnID, AETransactionID transactionID);*/

		[Static]
		[Export ("listDescriptor")]
		NSAppleEventDescriptor ListDescriptor { get; }

		[Static]
		[Export ("recordDescriptor")]
		NSAppleEventDescriptor RecordDescriptor { get; }

		/*[Export ("initWithAEDescNoCopy:")]
		NSObject InitWithAEDescNoCopy (const AEDesc aeDesc);

		[Export ("initWithDescriptorType:bytes:length:")]
		NSObject InitWithDescriptorTypebyteslength (DescType descriptorType, void bytes, uint byteCount);

		[Export ("initWithDescriptorType:data:")]
		NSObject InitWithDescriptorTypedata (DescType descriptorType, NSData data);

		[Export ("initWithEventClass:eventID:targetDescriptor:returnID:transactionID:")]
		NSObject InitWithEventClasseventIDtargetDescriptorreturnIDtransactionID (AEEventClass eventClass, AEEventID eventID, NSAppleEventDescriptor targetDescriptor, AEReturnID returnID, AETransactionID transactionID);*/

		[Export ("initListDescriptor")]
		NSObject InitListDescriptor ();

		[Export ("initRecordDescriptor")]
		NSObject InitRecordDescriptor ();

		/*[Export ("aeDesc")]
		const AEDesc AeDesc ();

		[Export ("descriptorType")]
		DescType DescriptorType ();*/

		[Export ("data")]
		NSData Data { get; }

		[Export ("booleanValue")]
		Boolean BooleanValue { get; }

		[Export ("enumCodeValue")]
		OSType EnumCodeValue ();

		[Export ("int32Value")]
		Int32 Int32Value { get; }

		[Export ("typeCodeValue")]
		OSType TypeCodeValue { get; }

		[Export ("stringValue")]
		string StringValue { get; }

		[Export ("eventClass")]
		AEEventClass EventClass { get; }

		[Export ("eventID")]
		AEEventID EventID { get; }

		/*[Export ("returnID")]
		AEReturnID ReturnID ();

		[Export ("transactionID")]
		AETransactionID TransactionID ();*/

		[Export ("setParamDescriptor:forKeyword:")]
		void SetParamDescriptorforKeyword (NSAppleEventDescriptor descriptor, AEKeyword keyword);

		[Export ("paramDescriptorForKeyword:")]
		NSAppleEventDescriptor ParamDescriptorForKeyword (AEKeyword keyword);

		[Export ("removeParamDescriptorWithKeyword:")]
		void RemoveParamDescriptorWithKeyword (AEKeyword keyword);

		[Export ("setAttributeDescriptor:forKeyword:")]
		void SetAttributeDescriptorforKeyword (NSAppleEventDescriptor descriptor, AEKeyword keyword);

		[Export ("attributeDescriptorForKeyword:")]
		NSAppleEventDescriptor AttributeDescriptorForKeyword (AEKeyword keyword);

		[Export ("numberOfItems")]
		int NumberOfItems { get; }

		[Export ("insertDescriptor:atIndex:")]
		void InsertDescriptoratIndex (NSAppleEventDescriptor descriptor, int index);

		[Export ("descriptorAtIndex:")]
		NSAppleEventDescriptor DescriptorAtIndex (int index);

		[Export ("removeDescriptorAtIndex:")]
		void RemoveDescriptorAtIndex (int index);

		[Export ("setDescriptor:forKeyword:")]
		void SetDescriptorforKeyword (NSAppleEventDescriptor descriptor, AEKeyword keyword);

		[Export ("descriptorForKeyword:")]
		NSAppleEventDescriptor DescriptorForKeyword (AEKeyword keyword);

		[Export ("removeDescriptorWithKeyword:")]
		void RemoveDescriptorWithKeyword (AEKeyword keyword);

		[Export ("keywordForDescriptorAtIndex:")]
		AEKeyword KeywordForDescriptorAtIndex (int index);

		/*[Export ("coerceToDescriptorType:")]
		NSAppleEventDescriptor CoerceToDescriptorType (DescType descriptorType);*/

	}

	[BaseType (typeof (NSObject))]
	public interface NSAppleEventManager {
		[Static]
		[Export ("sharedAppleEventManager")]
		NSAppleEventManager SharedAppleEventManager { get; }

		[Export ("setEventHandler:andSelector:forEventClass:andEventID:")]
		void SetEventHandler (NSObject handler, Selector handleEventSelector, AEEventClass eventClass, AEEventID eventID);

		[Export ("removeEventHandlerForEventClass:andEventID:")]
		void RemoveEventHandlerForEventClassandEventID (AEEventClass eventClass, AEEventID eventID);

		[Export ("currentAppleEvent")]
		NSAppleEventDescriptor CurrentAppleEvent { get; }

		[Export ("currentReplyAppleEvent")]
		NSAppleEventDescriptor CurrentReplyAppleEvent { get; }

		[Export ("suspendCurrentAppleEvent")]
		NSAppleEventManagerSuspensionID SuspendCurrentAppleEvent ();

		[Export ("appleEventForSuspensionID:")]
		NSAppleEventDescriptor AppleEventForSuspensionID (NSAppleEventManagerSuspensionID suspensionID);

		[Export ("replyAppleEventForSuspensionID:")]
		NSAppleEventDescriptor ReplyAppleEventForSuspensionID (NSAppleEventManagerSuspensionID suspensionID);

		[Export ("setCurrentAppleEventAndReplyEventWithSuspensionID:")]
		void SetCurrentAppleEventAndReplyEventWithSuspensionID (NSAppleEventManagerSuspensionID suspensionID);

		[Export ("resumeWithSuspensionID:")]
		void ResumeWithSuspensionID (NSAppleEventManagerSuspensionID suspensionID);

	}

	[BaseType (typeof (NSObject))]
	public interface NSTask {
		[Export ("launch")]
		void Launch ();

		[Export ("interrupt")]
		void Interrupt ();

		[Export ("terminate")]
		void Terminate ();

		[Export ("suspend")]
		bool Suspend ();

		[Export ("resume")]
		bool Resume ();

		[Export ("waitUntilExit")]
		void WaitUntilExit ();

		[Static]
		[Export ("launchedTaskWithLaunchPath:arguments:")]
		NSTask LaunchFromPath (string path, string[] arguments);

		//Detected properties
		[Export ("launchPath")]
		string LaunchPath { get; set; }

		[Export ("arguments")]
		string [] Arguments { get; set; }

		[Export ("environment")]
		NSDictionary Environment { get; set; }

		[Export ("currentDirectoryPath")]
		string CurrentDirectoryPath { get; set; }

		[Export ("standardInput")]
		NSObject StandardInput { get; set; }

		[Export ("standardOutput")]
		NSObject StandardOutput { get; set; }

		[Export ("standardError")]
		NSObject StandardError { get; set; }

		[Export ("isRunning")]
		bool IsRunning { get; }

		[Export ("processIdentifier")]
		int ProcessIdentifier { get; }

		[Export ("terminationStatus")]
		int TerminationStatus { get; }

		[Export ("terminationReason")]
		NSTaskTerminationReason TerminationReason { get; }

		// Fields
		[Field ("NSTaskDidTerminateNotification")]
		NSString NSTaskDidTerminateNotification { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSFileHandle 
	{
		[Export ("availableData")]
		NSData AvailableData ();
		
		[Export ("readDataToEndOfFile")]
		NSData ReadDataToEndOfFile ();

		[Export ("readDataOfLength:")]
		NSData ReadDataOfLength (uint length);

		[Export ("writeData:")]
		void WriteData (NSData data);

		[Export ("offsetInFile")]
		ulong OffsetInFile ();

		[Export ("seekToEndOfFile")]
		ulong SeekToEndOfFile ();

		[Export ("seekToFileOffset:")]
		void SeekToFileOffset (ulong offset);

		[Export ("truncateFileAtOffset:")]
		void TruncateFileAtOffset (ulong offset);

		[Export ("synchronizeFile")]
		void SynchronizeFile ();

		[Export ("closeFile")]
		void CloseFile ();
		
		[Static]
		[Export ("fileHandleWithStandardInput")]
		NSObject FileHandleWithStandardInput ();
		
		[Static]
		[Export ("fileHandleWithStandardOutput")]
		NSObject FileHandleWithStandardOutput ();

		[Static]
		[Export ("fileHandleWithStandardError")]
		NSObject FileHandleWithStandardError ();

		[Static]
		[Export ("fileHandleWithNullDevice")]
		NSObject FileHandleWithNullDevice ();

		[Static]
		[Export ("fileHandleForReadingAtPath:")]
		NSObject FileHandleForReadingAtPath (string path);

		[Static]
		[Export ("fileHandleForWritingAtPath:")]
		NSObject FileHandleForWritingAtPath (string path);

		[Static]
		[Export ("fileHandleForUpdatingAtPath:")]
		NSObject FileHandleForUpdatingAtPath (string path);

		[Static]
		[Export ("fileHandleForReadingFromURL:error:")]
		NSObject FileHandleForReadingFromURLerror (NSUrl url, out NSError error);

		[Static]
		[Export ("fileHandleForWritingToURL:error:")]
		NSObject FileHandleForWritingToURLerror (NSUrl url, out NSError error);

		[Static]
		[Export ("fileHandleForUpdatingURL:error:")]
		NSObject FileHandleForUpdatingURLerror (NSUrl url, out NSError error);
		
		[Export ("readInBackgroundAndNotifyForModes:")]
		void ReadInBackgroundAndNotifyForModes (NSArray modes);
		
		[Export ("readInBackgroundAndNotify")]
		void ReadInBackgroundAndNotify ();

		[Export ("readToEndOfFileInBackgroundAndNotifyForModes:")]
		void ReadToEndOfFileInBackgroundAndNotifyForModes (NSArray modes);

		[Export ("readToEndOfFileInBackgroundAndNotify")]
		void ReadToEndOfFileInBackgroundAndNotify ();

		[Export ("acceptConnectionInBackgroundAndNotifyForModes:")]
		void AcceptConnectionInBackgroundAndNotifyForModes (NSArray modes);

		[Export ("acceptConnectionInBackgroundAndNotify")]
		void AcceptConnectionInBackgroundAndNotify ();

		[Export ("waitForDataInBackgroundAndNotifyForModes:")]
		void WaitForDataInBackgroundAndNotifyForModes (NSArray modes);

		[Export ("waitForDataInBackgroundAndNotify")]
		void WaitForDataInBackgroundAndNotify ();
		
		[Export ("initWithFileDescriptor:closeOnDealloc:")]
		IntPtr Constructor (int fd, bool closeopt);
		
		[Export ("initWithFileDescriptor:")]
		IntPtr Constructor (int fd);

		[Export ("fileDescriptor")]
		int FileDescriptor { get; set; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSPipe {
		
		[Export ("fileHandleForReading")]
		NSFileHandle FileHandleForReading ();
		
		[Export ("fileHandleForWriting")]
		NSFileHandle FileHandleForWriting ();

		[Static]
		[Export ("pipe")]
		NSPipe Pipe ();
	}
}
