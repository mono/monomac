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
// growl.cs: Definitions for the Growl Framework
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.AppKit;

namespace MonoMac.Growl {
	[BaseType (typeof (NSObject))]
	interface GrowlApplicationBridge {
		[Static]
		[Export ("isGrowlInstalled")]
		bool IsGrowlInstalled ();

		[Static]
		[Export ("isGrowlRunning")]
		bool IsGrowlRunning ();

		[Static]
		[Export ("notifyWithTitle:description:notificationName:iconData:priority:isSticky:clickContext:")]
		void Notify (string title, string description, string notifName, [NullAllowed] NSData iconData, int priority, bool isSticky, [NullAllowed] NSObject clickContext);

		[Static]
		[Export ("notifyWithTitle:description:notificationName:iconData:priority:isSticky:clickContext:identifier:")]
		void Notify (string title, string description, string notifName, [NullAllowed] NSData iconData, int priority, bool isSticky, [NullAllowed] NSObject clickContext, string identifier);

		[Static]
		[Export ("notifyWithDictionary:")]
		void Notify (NSDictionary userInfo);

		[Export ("registerWithDictionary:")]
		bool Register (NSDictionary regDict);

		[Static]
		[Export ("reregisterGrowlNotifications")]
		void ReRegister ();

		[Export ("registrationDictionaryFromDelegate")]
		NSDictionary RegistrationDictionaryFromDelegate ();

		[Static]
		[Export ("registrationDictionaryFromBundle:")]
		NSDictionary RegistrationDictionaryFromBundle (NSBundle bundle);

		[Static]
		[Export ("bestRegistrationDictionary")]
		NSDictionary BestRegistrationDictionary ();

		[Static]
		[Export ("registrationDictionaryByFillingInDictionary:")]
		NSDictionary FromDictionary (NSDictionary regDict);

		[Static]
		[Export ("registrationDictionaryByFillingInDictionary:restrictToKeys:")]
		NSDictionary FromDictionary (NSDictionary regDict, NSSet restrictToKeys);

		[Static]
		[Export ("notificationDictionaryByFillingInDictionary:")]
		NSDictionary NotificationDictionary (NSDictionary regDict);

		[Static]
		[Export ("frameworkInfoDictionary")]
		NSDictionary FrameworkInfoDictionary { get; }

		//Detected properties
		//[Export ("growlDelegate")]
		//NSObject<GrowlApplicationBridgeDelegate> GrowlDelegate { get; set; }
		
		[Static]
		[Export ("growlDelegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Static]
		[Wrap ("WeakDelegate")]
		GrowlDelegate Delegate { get; set; }

		[Static]
		[Export ("willRegisterWhenGrowlIsReady")]
		bool WillRegisterWhenGrowlIsReady { get; set; }

	}
	[BaseType (typeof(NSObject))]
	[Model]
	interface GrowlDelegate {
		[Export ("registrationDictionaryForGrowl")]
		NSDictionary RegistrationDictionaryForGrowl ();

		[Export ("applicationNameForGrowl")]
		string ApplicationNameForGrowl ();

		[Export ("applicationIconForGrowl")]
		NSImage ApplicationIconForGrowl ();

		[Export ("applicationIconDataForGrowl")]
		NSData ApplicationIconDataForGrowl ();

		[Export ("growlIsReady")]
		void GrowlIsReady ();

		[Export ("growlNotificationWasClicked:")]
		void GrowlNotificationWasClicked (NSObject clickContext);

		[Export ("growlNotificationTimedOut:")]
		void GrowlNotificationTimedOut (NSObject clickContext);

        // Installer informal protocol
		[Export ("growlInstallationWindowTitle")]
		string GrowlInstallationWindowTitle ();

		[Export ("growlUpdateWindowTitle")]
		string GrowlUpdateWindowTitle ();

		[Export ("growlInstallationInformation")]
		NSAttributedString GrowlInstallationInformation ();

		[Export ("growlUpdateInformation")]
		NSAttributedString GrowlUpdateInformation ();
	}
	
	/* Growl has two informal protocols (NSObject categories) rather than a delegate protocol since version 0.7 */
	/* This would be the literal interpretation of the ObjC code
	interface NSObject { 
		[Bind ("registrationDictionaryForGrowl")]
		NSDictionary RegistrationDictionaryForGrowl ([Target] NSObject target);

		[Bind ("applicationNameForGrowl")]
		string ApplicationNameForGrowl ([Target] NSObject target);

		[Bind ("applicationIconForGrowl")]
		NSImage ApplicationIconForGrowl ([Target] NSObject target);

		[Bind ("applicationIconDataForGrowl")]
		NSData ApplicationIconDataForGrowl ([Target] NSObject target);

		[Bind ("growlIsReady")]
		void GrowlIsReady ([Target] NSObject target);

		[Bind ("growlNotificationWasClicked:")]
		void GrowlNotificationWasClicked ([Target] NSObject target, NSObject clickContext);

		[Bind ("growlNotificationTimedOut:")]
		void GrowlNotificationTimedOut ([Target] NSObject target, NSObject clickContext);

        // Installer informal protocol
		[Bind ("growlInstallationWindowTitle")]
		string GrowlInstallationWindowTitle ([Target] NSObject target);

		[Bind ("growlUpdateWindowTitle")]
		string GrowlUpdateWindowTitle ([Target] NSObject target);

		[Bind ("growlInstallationInformation")]
		NSAttributedString GrowlInstallationInformation ([Target] NSObject target);

		[Bind ("growlUpdateInformation")]
		NSAttributedString GrowlUpdateInformation ([Target] NSObject target);

	}
	*/
}
