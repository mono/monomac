//
// Helper methods for NSDistributedNotificationCenter
//
// Author:
//   Miguel de Icaza
//
// Copyright 2011 Xamarin Inc
//
using System;
using MonoTouch.ObjCRuntime;

namespace MonoMac.Foundation {
	public partial class NSDistributedNotificationCenter {
		[Advice ("Use AddObserver (NSObject, Selector, string, NSObject) instead")]
		public void AddObserver (NSObject observer, Selector aSelector, string aName, string anObject)
		{
			using (var n = new NSString (anObject)){
				AddObserver (observer, aSelector, aName, n);
			}
		}

		[Advice ("Use RemoveObserver (NSObject, string, NSObject) instead")]
		public void RemoveObserver (NSObject observer, string aName, string anObject)
		{
			using (var n = new NSString (anObject)){
				RemoveObserver (observer, aName, n);
			}
		}
	}
}
