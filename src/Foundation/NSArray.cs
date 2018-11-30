//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif


namespace MonoMac.Foundation {

	public partial class NSArray {
		internal NSArray (bool empty)
		{
			Handle = IntPtr.Zero;
		}

		//
		// Creates an array with the elements;   If the value passed is null, it
		// still creates an NSArray object, but the Handle is set to IntPtr.Zero,
		// this is so it makes it simpler for the generator to support
		// [NullAllowed] on array parameters.
		//
		static public NSArray FromNSObjects (params NSObject [] items)
		{
			IList<NSObject> _items = items;
			return FromNSObjects (_items);
		}

		static public NSArray FromNSObjects (int count, params NSObject [] items)
		{
			if (items.Length < count)
				throw new ArgumentException ("count is larger than the number of items", "count");
			
			IList<NSObject> _items = items;
			return FromNSObjects (_items, count);
		}

		static public NSArray FromNSObjects (params INativeObject [] items)
		{
			return FromNativeObjects ((IList<INativeObject>)items);
		}

		static public NSArray FromNSObjects (int count, params INativeObject [] items)
		{
			if (items.Length < count)
				throw new ArgumentException ("count is larger than the number of items", "count");
			
			return FromNativeObjects ((IList<INativeObject>)items, count);
		}

		public static NSArray FromObjects (params object [] items)
		{
			if (items == null)
				return new NSArray (true);
			return FromObjects (items.Length, items);
		}

		public static NSArray FromObjects (int count, params object [] items)
		{
			if (items == null)
				return new NSArray (true);

			if (count > items.Length)
				throw new ArgumentException ("count is larger than the number of items", "count");

			NSObject [] nsoa = new NSObject [count];
			for (int i = 0; i < count; i++){
				var k = NSObject.FromObject (items [i]);
				if (k == null)
					throw new ArgumentException (String.Format ("Do not know how to marshal object of type '{0}' to an NSObject", items [i].GetType ()));
				nsoa [i] = k;
			}
			return FromNSObjects (nsoa);
		}

		public static NSArray FromObjects (IntPtr array, int count)
		{
			return FromObjects (array, (nuint)count);
		}

		internal static NSArray FromNSObjects (IList<NSObject> items)
		{
			if (items == null)
				return new NSArray (true);
			
			IntPtr buf = Marshal.AllocHGlobal (items.Count * IntPtr.Size);
			for (int i = 0; i < items.Count; i++)
				Marshal.WriteIntPtr (buf, i * IntPtr.Size, items [i]?.Handle ?? NSNull.Null.Handle);
			NSArray arr = NSArray.FromObjects (buf, items.Count);
			Marshal.FreeHGlobal (buf);
			return arr;
		}

		internal static NSArray FromNSObjects (IList<NSObject> items, int count)
		{
			if (items == null)
				return new NSArray (true);
			
			IntPtr buf = Marshal.AllocHGlobal (items.Count * IntPtr.Size);
			for (int i = 0; i < count; i++)
				Marshal.WriteIntPtr (buf, i * IntPtr.Size, items [i]?.Handle ?? NSNull.Null.Handle);
			NSArray arr = NSArray.FromObjects (buf, count);
			Marshal.FreeHGlobal (buf);
			return arr;
		}

		internal static NSArray FromNativeObjects (IList<INativeObject> items)
		{
			if (items == null)
				return new NSArray (true);
			
			IntPtr buf = Marshal.AllocHGlobal (items.Count * IntPtr.Size);
			for (int i = 0; i < items.Count; i++)
				Marshal.WriteIntPtr (buf, i * IntPtr.Size, items [i]?.Handle ?? NSNull.Null.Handle);
			NSArray arr = NSArray.FromObjects (buf, items.Count);
			Marshal.FreeHGlobal (buf);
			return arr;
		}

		internal static NSArray FromNativeObjects (IList<INativeObject> items, int count)
		{
			if (items == null)
				return new NSArray (true);
			
			IntPtr buf = Marshal.AllocHGlobal (items.Count * IntPtr.Size);
			for (int i = 0; i < count; i++)
				Marshal.WriteIntPtr (buf, i * IntPtr.Size, items [i]?.Handle ?? NSNull.Null.Handle);
			NSArray arr = NSArray.FromObjects (buf, count);
			Marshal.FreeHGlobal (buf);
			return arr;
		}
		static public NSArray FromStrings (params string [] items)
		{
			if (items == null)
				throw new ArgumentNullException ("items");
			
			IntPtr buf = Marshal.AllocHGlobal (items.Length * IntPtr.Size);
			try {
				NSString [] strings = new NSString [items.Length];
				
				for (int i = 0; i < items.Length; i++){
					IntPtr val;
					
					if (items [i] == null)
						throw new ArgumentNullException (string.Format ("items[{0}]", i));
					
					strings [i] = (NSString)items [i];
					val = strings [i]?.Handle ?? NSNull.Null.Handle;
	
					Marshal.WriteIntPtr (buf, i * IntPtr.Size, val);
				}
				NSArray arr = NSArray.FromObjects (buf, items.Length);
				foreach (NSString ns in strings)
					ns.Dispose ();
				return arr;
			} finally {
				Marshal.FreeHGlobal (buf);
			}
		}

		static public NSArray FromIntPtrs (IntPtr [] vals)
		{
			if (vals == null)
				throw new ArgumentNullException ("vals");
			int n = vals.Length;
			IntPtr buf = Marshal.AllocHGlobal (n * IntPtr.Size);
			for (int i = 0; i < n; i++)
				Marshal.WriteIntPtr (buf, i * IntPtr.Size, vals [i]);

			NSArray arr = NSArray.FromObjects (buf, vals.Length);

			Marshal.FreeHGlobal (buf);
			return arr;
		}
		
		static public string [] StringArrayFromHandle (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;

#if MONOMAC
			uint c = Messaging.UInt32_objc_msgSend (handle, selCountHandle);
#else
			uint c = Messaging.UInt32_objc_msgSend (handle, Selector.GetHandle (selCount));
#endif
			string [] ret = new string [c];

			for (uint i = 0; i < c; i++){
#if MONOMAC
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, selObjectAtIndex_Handle, i);
#else
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, Selector.GetHandle (selObjectAtIndex_), i);
#endif
				ret [i] = NSString.FromHandle (p);
			}
			return ret;
		}

		// Used for NSObjects only
		static public T [] ArrayFromHandle<T> (IntPtr handle) where T : NSObject
		{
			if (handle == IntPtr.Zero)
				return null;

#if MONOMAC
			uint c = Messaging.UInt32_objc_msgSend (handle, selCountHandle);
#else
			uint c = Messaging.UInt32_objc_msgSend (handle, Selector.GetHandle (selCount));
#endif
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++){
#if MONOMAC
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, selObjectAtIndex_Handle, i);
#else
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, Selector.GetHandle (selObjectAtIndex_), i);
#endif

				ret [i] = Runtime.GetNSObject<T> (p);
				ret [i].Handle = p;
			}
			return ret;
		}

		static public T [] FromArray<T> (NSArray weakArray) where T : NSObject
		{
			if (weakArray == null || weakArray.Handle == IntPtr.Zero)
				return null;
			try {
				nuint n = weakArray.Count;
				T [] ret = new T [n];
				for (nuint i = 0; i < n; i++){
					ret [i] = Runtime.GetNSObject<T> (weakArray.ValueAt (i));
				}
				return ret;
			} catch {
				return null;
			}
		}
		
		// Used when we need to provide our constructor
		static public T [] ArrayFromHandleFunc<T> (IntPtr handle, Func<IntPtr,T> createObject) 
		{
			if (handle == IntPtr.Zero)
				return null;

#if MONOMAC
			uint c = Messaging.UInt32_objc_msgSend (handle, selCountHandle);
#else
			uint c = Messaging.UInt32_objc_msgSend (handle, Selector.GetHandle (selCount));
#endif
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++){
#if MONOMAC
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, selObjectAtIndex_Handle, i);
#else
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, Selector.GetHandle (selObjectAtIndex_), i);
#endif

				ret [i] = createObject (p);
			}
			return ret;
		}
		
		static public T [] ArrayFromHandle<T> (IntPtr handle, Converter<IntPtr, T> creator) 
		{
			if (handle == IntPtr.Zero)
				return null;

#if MONOMAC
			uint c = Messaging.UInt32_objc_msgSend (handle, selCountHandle);
#else
			uint c = Messaging.UInt32_objc_msgSend (handle, Selector.GetHandle (selCount));
#endif
			T [] ret = new T [c];

			for (uint i = 0; i < c; i++){
#if MONOMAC
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, selObjectAtIndex_Handle, i);
				#else
				IntPtr p = Messaging.IntPtr_objc_msgSend_UInt32 (handle, Selector.GetHandle (selObjectAtIndex_), i);
#endif

				ret [i] = creator (p);
			}
			return ret;
		}
		
		private T UnsafeGetItem<T> (nuint index) where T : NSObject
		{
			IntPtr idx = ValueAt (index);

			if (idx == IntPtr.Zero || idx == NSNull.Null.Handle) {
				return (T)((object)null);
			} else {
				return Runtime.GetNSObject<T> (idx);
			}
		}
		
		public T GetItem<T> (nuint index) where T : NSObject
		{
			if (index >= Count) {
				throw new ArgumentOutOfRangeException ("index");
			}
			return UnsafeGetItem<T> (index);
		}
	}
}
