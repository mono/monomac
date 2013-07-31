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
// typedef double NSTimeInterval;
using NSTimeInterval = System.Double;

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

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface NSConnection {
		[Static, Export ("connectionWithReceivePort:sendPort:")]
		NSConnection Create ([NullAllowed] NSPort receivePort, [NullAllowed] NSPort sendPort);

		[Export ("runInNewThread")]
		void RunInNewThread ();

		// enableMultipleThreads, multipleThreadsEnabled - no-op in 10.5+ (always enabled)

		[Export ("addRunLoop:")]
		void AddRunLoop (NSRunLoop runLoop);

		[Export ("removeRunLoop:")]
		void RemoveRunLoop (NSRunLoop runLoop);

		[Static, Export ("serviceConnectionWithName:rootObject:usingNameServer:")]
		NSConnection CreateService (string name, NSObject root, NSPortNameServer server);

		[Static, Export ("serviceConnectionWithName:rootObject:")]
		NSConnection CreateService (string name, NSObject root);

		[Export ("registerName:")]
		bool RegisterName (string name);

		[Export ("registerName:withNameServer:")]
		bool RegisterName (string name, NSPortNameServer server);

		[Export ("rootObject")]
		NSObject RootObject { get; set; }

		[Static, Export ("connectionWithRegisteredName:host:")]
		NSConnection LookupService (string name, [NullAllowed] string hostName);

		[Static, Export ("connectionWithRegisteredName:host:usingNameServer:")]
		NSConnection LookupService (string name, [NullAllowed] string hostName, NSPortNameServer server);

		[Internal, Export ("rootProxy")]
		IntPtr _GetRootProxy ();

		[Internal, Static, Export ("rootProxyForConnectionWithRegisteredName:host:")]
		IntPtr _GetRootProxy (string name, [NullAllowed] string hostName);

		[Internal, Static, Export ("rootProxyForConnectionWithRegisteredName:host:usingNameServer:")]
		IntPtr _GetRootProxy (string name, [NullAllowed] string hostName, NSPortNameServer server);

		[Export ("remoteObjects")]
		NSObject [] RemoteObjects { get; }

		[Export ("localObjects")]
		NSObject [] LocalObjects { get; }

		[Static, Export ("currentConversation")]
		NSObject CurrentConversation { get; }

		[Static, Export ("allConnections")]
		NSConnection [] AllConnections { get; }

		[Export ("requestTimeout")]
		NSTimeInterval RequestTimeout { get; set; }

		[Export ("replyTimeout")]
		NSTimeInterval ReplyTimeout { get; set; }

		[Export ("independentConversationQueueing")]
		bool IndependentConversationQueueing { get; set; }

		[Export ("addRequestMode:")]
		void AddRequestMode (NSString runLoopMode);

		[Export ("removeRequestMode:")]
		void RemoveRequestMode (NSString runLoopMode);

		[Export ("requestModes")]
		NSString [] RequestModes { get; }

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("isValid")]
		bool IsValid { get; }

		[Export ("receivePort")]
		NSPort ReceivePort { get; }

		[Export ("sendPort")]
		NSPort SendPort { get; }

		[Export ("dispatchWithComponents:")]
		void Dispatch (NSArray components);

		[Export ("statistics")]
		NSDictionary Statistics { get; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSConnectionDelegate Delegate { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSConnectionDelegate {
		[Export ("authenticateComponents:withData:")]
		bool AuthenticateComponents (NSArray components, NSData authenticationData);

		[Export ("authenticationDataForComponents:")]
		NSData GetAuthenticationData (NSArray components);

		[Export ("connection:shouldMakeNewConnection:")]
		bool ShouldMakeNewConnection (NSConnection parentConnection, NSConnection newConnection);

		[Export ("connection:handleRequest:")]
		bool HandleRequest (NSConnection connection, NSDistantObjectRequest request);

		[Export ("createConversationForConnection:")]
		NSObject CreateConversation (NSConnection connection);

		[Export ("makeNewConnection:sender:")]
		bool AllowNewConnection (NSConnection newConnection, NSConnection parentConnection);
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface NSDistantObjectRequest {
		[Export ("connection")]
		NSConnection Connection { get; }

		[Export ("conversation")]
		NSObject Conversation { get; }

		[Export ("invocation")]
		NSInvocation Invocation { get; }

		[Export ("replyWithException:")]
		void Reply ([NullAllowed] NSException exception);
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface NSPortNameServer {
		[Static, Export ("systemDefaultPortNameServer")]
		NSPortNameServer SystemDefault { get; }

		[Export ("portForName:")]
		NSPort GetPort (string portName);

		[Export ("portForName:host:")]
		NSPort GetPort (string portName, string hostName);

		[Export ("registerPort:name:")]
		bool RegisterPort (NSPort port, string portName);

		[Export ("removePortForName:")]
		bool RemovePort (string portName);
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
	[DisableDefaultCtor] // An uncaught exception was raised: *** -range cannot be sent to an abstract object of class NSTextCheckingResult: Create a concrete instance!
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
	public interface NSValueTransformer {
		[Export ("reverseTransformedValue:")]
		NSObject ReverseTransformedValue (NSObject value);

		[Export ("transformedValue:")]
		NSObject TransformedValue (NSObject value);
	}

	[MountainLion]
	[BaseType (typeof (NSObject))]
	public interface NSUserNotification 
	{
		[Export ("title", ArgumentSemantic.Copy)]
		string Title { get; set; }
		
		[Export ("subtitle", ArgumentSemantic.Copy)]
		string Subtitle { get; set; }
		
		[Export ("informativeText", ArgumentSemantic.Copy)]
		string InformativeText { get; set; }
		
		[Export ("actionButtonTitle", ArgumentSemantic.Copy)]
		string ActionButtonTitle { get; set; }
		
		[Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary UserInfo { get; set; }
		
		[Export ("deliveryDate", ArgumentSemantic.Copy)]
		NSDate DeliveryDate { get; set; }
		
		[Export ("deliveryTimeZone", ArgumentSemantic.Copy)]
		NSTimeZone DeliveryTimeZone { get; set; }
		
		[Export ("deliveryRepeatInterval", ArgumentSemantic.Copy)]
		NSDateComponents DeliveryRepeatInterval { get; set; }
		
		[Export ("actualDeliveryDate")]
		NSDate ActualDeliveryDate { get; }
		
		[Export ("presented")]
		bool Presented { [Bind("isPresented")] get; }
		
		[Export ("remote")]
		bool Remote { [Bind("isRemote")] get; }
		
		[Export ("soundName")]
		string SoundName { get; set; }
		
		[Export ("hasActionButton")]
		bool HasActionButton { get; set; }
		
		[Export ("activationType")]
		NSUserNotificationActivationType ActivationType { get; }
		
		[Export ("otherButtonTitle", ArgumentSemantic.Copy)]
		string OtherButtonTitle { get; set; }

		[Field ("NSUserNotificationDefaultSoundName")]
		NSString NSUserNotificationDefaultSoundName { get; }
	}
	
	[MountainLion]
	[BaseType (typeof (NSObject),
	           Delegates=new string [] {"WeakDelegate"},
	Events=new Type [] { typeof (NSUserNotificationCenterDelegate) })]
	[DisableDefaultCtor] // crash with: NSUserNotificationCenter designitated initializer is _centerForBundleIdentifier
	public interface NSUserNotificationCenter 
	{
		[Export ("defaultUserNotificationCenter")][Static]
		NSUserNotificationCenter DefaultUserNotificationCenter { get; }
		
		[Export ("delegate")][NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")][NullAllowed]
		NSUserNotificationCenterDelegate Delegate { get; set; }
		
		[Export ("scheduledNotifications")]
		NSUserNotification [] ScheduledNotifications { get; set; }
		
		[Export ("scheduleNotification:")][PostGet ("ScheduledNotifications")]
		void ScheduleNotification (NSUserNotification notification);
		
		[Export ("removeScheduledNotification:")][PostGet ("ScheduledNotifications")]
		void RemoveScheduledNotification (NSUserNotification notification);
		
		[Export ("deliveredNotifications")]
		NSUserNotification [] DeliveredNotifications { get; }
		
		[Export ("deliverNotification:")][PostGet ("DeliveredNotifications")]
		void DeliverNotification (NSUserNotification notification);
		
		[Export ("removeDeliveredNotification:")][PostGet ("DeliveredNotifications")]
		void RemoveDeliveredNotification (NSUserNotification notification);
		
		[Export ("removeAllDeliveredNotifications")][PostGet ("DeliveredNotifications")]
		void RemoveAllDeliveredNotifications ();
	}
	
	[MountainLion]
	[BaseType (typeof (NSObject))]
	[Model]
	public interface NSUserNotificationCenterDelegate 
	{
		[Export ("userNotificationCenter:didDeliverNotification:"), EventArgs ("UNCDidDeliverNotification")]
		void DidDeliverNotification (NSUserNotificationCenter center, NSUserNotification notification);
		
		[Export ("userNotificationCenter:didActivateNotification:"), EventArgs ("UNCDidActivateNotification")]
		void DidActivateNotification (NSUserNotificationCenter center, NSUserNotification notification);
		
		[Export ("userNotificationCenter:shouldPresentNotification:"), DelegateName ("UNCShouldPresentNotification"), DefaultValue (false)]
		bool ShouldPresentNotification (NSUserNotificationCenter center, NSUserNotification notification);
	}

}
