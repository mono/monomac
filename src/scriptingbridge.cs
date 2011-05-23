//
// Copyright 2011, Kenneth J. Pouncey
//
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
// scriptingbridge.cs: Bindings for the ScriptingBridge.Framework API
//
using System;
using System.Drawing;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.ScriptingBridge {
	
	
	[BaseType (typeof (NSObject))]
	interface SBObject {

		[Export ("initWithProperties:")]
		IntPtr Constructor (NSDictionary properties);

		[Export ("initWithData:")]
		IntPtr Constructor (NSObject data);

		[Export ("get")]
		NSObject Get { get; }

		[Export ("lastError")]
		NSError LastError { get; }

	}
	
	// TODO: The documentation says these are rarely used so will clean these up later
//	interface SBObject {
//		[Export ("initWithElementCode:properties:data:")]
//		NSObject InitWithElementCodepropertiesdata (DescType code, NSDictionary properties, NSObject data);
//
//		[Export ("propertyWithCode:")]
//		SBObject PropertyWithCode (AEKeyword code);
//
//		[Export ("propertyWithClass:code:")]
//		SBObject PropertyWithClasscode (Class cls, AEKeyword code);
//
//		[Export ("elementArrayWithCode:")]
//		SBElementArray ElementArrayWithCode (DescType code);
//
//		[Export ("sendEvent:id:parameters:...")]
//		NSObject SendEventidparameters... (AEEventClass eventClass, AEEventID eventID, DescType firstParamCode,, );
//
//		[Export ("setTo:")]
//		void SetTo (NSObject value);
//
//	}
	
	[BaseType (typeof (SBObject),Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (SBApplicationDelegate)})]
	interface SBApplication {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithProcessIdentifier:")]
		IntPtr Constructor (int pid);
		
		[Static]
		[Export ("applicationWithBundleIdentifier:")]
		SBApplication FromBundleIdentifier (string ident );

		[Static]
		[Export ("applicationWithURL:url")]
		SBApplication FromURL (NSUrl url );

		[Static]
		[Export ("applicationWithProcessIdentifier")]
		SBApplication FromProcessIdentifier (int pid );

		[Export ("classForScriptingClass")]
		Class ClassForScripting (string className );

		[Export ("isRunning")]
		bool IsRunning { get; }

		[Export ("activate")]
		void Activate ();

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		SBApplicationDelegate Delegate { get; set; }

		[Export ("launchFlags")]
		LSLaunchFlags LaunchFlags { get; set; }

		[Export ("sendMode")]
		AESendMode SendMode { get; set; }

		[Export ("timeout")]
		long Timeout { get; set; }

	}

	[BaseType (typeof (NSObject))]
	[Model]
	interface SBApplicationDelegate {
		[Abstract]
		[Export ("eventDidFail:withError:"), DelegateName ("SBApplicationError"), DefaultValue (null)]
		//NSObject EventDidFailwithError (const AppleEvent event, NSError error);
		NSObject EventDidFailwithError (IntPtr appleEvent, NSError error);

	}
	

	[BaseType (typeof (NSMutableArray))]
	interface SBElementArray {
		[Export ("objectWithName:")]
		NSObject Object (string name);

		[Export ("objectWithID:")]
		NSObject Object (NSObject identifier);

		[Export ("objectAtLocation:")]
		NSObject ObjectAt (NSObject location);

		[Export ("arrayByApplyingSelector:")]
		NSArray GetArray (Selector selector);

		[Export ("arrayByApplyingSelector:withObject:")]
		NSArray GetArray (Selector aSelector, NSObject argument);

		[Export ("get")]
		NSArray Get { get; }

	}	
}