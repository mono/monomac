//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2012, Xamarin Inc
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

using System;
using System.Collections;
using System.Collections.Generic;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSSet : IEnumerable<NSObject> {
		internal const string selSetWithArray = "setWithArray:";

		public NSSet (params NSObject [] objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		public NSSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		public NSSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}
		
		public T [] ToArray<T> () where T : NSObject
		{
			IntPtr nsarr = _AllObjects ();
			return NSArray.ArrayFromHandle<T> (nsarr);
		}

		public static NSSet MakeNSObjectSet<T> (T [] values) where T : NSObject
		{
			NSArray a = NSArray.FromNSObjects (values);
			return Runtime.GetNSObject<NSSet> (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle (selSetWithArray), a.Handle));
		}

		public IEnumerator<NSObject> GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;
			
			while ((obj = enumerator.NextObject ()) != null)
				yield return obj as NSObject;
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;
			
			while ((obj = enumerator.NextObject ()) != null)
				yield return obj;
		}

		public static NSSet operator + (NSSet first, NSSet second)
		{
			if (first == null)
				return second;
			if (second == null)
				return first;
			return first.SetByAddingObjectsFromSet (second);
		}

		public static NSSet operator - (NSSet first, NSSet second)
		{
			if (first == null)
				return null;
			if (second == null)
				return first;
			var copy = new NSMutableSet (first);
			copy.MinusSet (second);
			return copy;
		}

		public bool Contains (object obj)
		{
			return Contains (NSObject.FromObject (obj));
		}
	}
		
}
