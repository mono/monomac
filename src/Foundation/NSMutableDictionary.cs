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
// Copyright 2011, 2012 Xamarin Inc
//
using System;
using System.Collections;
using System.Collections.Generic;
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

	public partial class NSMutableDictionary : IDictionary, IDictionary<NSObject, NSObject> {
		
		// some API, like SecItemCopyMatching, returns a retained NSMutableDictionary
		internal NSMutableDictionary (IntPtr handle, bool owns)
			: base (handle)
		{
			if (!owns)
				Release ();
		}

		public static NSMutableDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			var no = NSArray.FromNSObjects (objects);
			var nk = NSArray.FromNSObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}

		public static NSMutableDictionary FromObjectsAndKeys (object [] objects, object [] keys)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			
			var no = NSArray.FromObjects (objects);
			var nk = NSArray.FromObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}

		public static NSMutableDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys, int count)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException ("count");
			
			var no = NSArray.FromNSObjects (objects);
			var nk = NSArray.FromNSObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}
		
		public static NSMutableDictionary FromObjectsAndKeys (object [] objects, object [] keys, int count)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException ("count");
			
			var no = NSArray.FromObjects (objects);
			var nk = NSArray.FromObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}

		#region ICollection<KeyValuePair<NSObject, NSObject>>
		void ICollection<KeyValuePair<NSObject, NSObject>>.Add (KeyValuePair<NSObject, NSObject> item)
		{
			SetObject (item.Value, item.Key);
		}

		public void Clear ()
		{
			RemoveAllObjects ();
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Contains (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			return ContainsKeyValuePair (keyValuePair);
		}

		void ICollection<KeyValuePair<NSObject, NSObject>>.CopyTo (KeyValuePair<NSObject, NSObject>[] array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index");
			// we want no exception for index==array.Length && Count == 0
			if (index > array.Length)
				throw new ArgumentException ("index larger than largest valid index of array");
			if (array.Length - index < (int)Count)
				throw new ArgumentException ("Destination array cannot hold the requested elements!");

			var e = GetEnumerator ();
			while (e.MoveNext ())
				array [index++] = e.Current;
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Remove (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			var count = Count;
			RemoveObjectForKey (keyValuePair.Key);
			return count != Count;
		}

		int ICollection<KeyValuePair<NSObject, NSObject>>.Count {
			get {return (int) Count;}
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.IsReadOnly {
			get {return false;}
		}
		#endregion

		#region IDictionary

		void IDictionary.Add (object key, object value)
		{
			var nsokey = key as NSObject;
			var nsovalue = value as NSObject;
			
			if (nsokey == null || nsovalue == null)
				throw new ArgumentException ("You can only use NSObjects for keys and values in an NSMutableDictionary");

			// Inverted args
			SetObject (nsovalue, nsokey);
		}

		bool IDictionary.Contains (object key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			NSObject _key = key as NSObject;
			if (_key == null)
				return false;
			return ContainsKey (_key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator ()
		{
			return new ShimEnumerator (this);
		}

#if !COREFX
		[Serializable]
#endif
		class ShimEnumerator : IDictionaryEnumerator, IEnumerator {
			IEnumerator<KeyValuePair<NSObject, NSObject>> e;

			public ShimEnumerator (NSMutableDictionary host)
			{
				e = host.GetEnumerator ();
			}

			public void Dispose ()
			{
				e.Dispose ();
			}

			public bool MoveNext ()
			{
				return e.MoveNext ();
			}

			public DictionaryEntry Entry {
				get { return new DictionaryEntry { Key = e.Current.Key, Value = e.Current.Value }; }
			}

			public object Key {
				get { return e.Current.Key; }
			}

			public object Value {
				get { return e.Current.Value; }
			}

			public object Current {
				get {return Entry;}
			}

			public void Reset ()
			{
				e.Reset ();
			}
		}

		void IDictionary.Remove (object key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = key as NSObject;
			if (nskey == null)
				throw new ArgumentException ("The key must be an NSObject");
			
			RemoveObjectForKey (nskey);
		}

		bool IDictionary.IsFixedSize {
			get {return false;}
		}

		bool IDictionary.IsReadOnly {
			get {return false;}
		}

		object IDictionary.this [object key] {
			get {
				NSObject _key = key as NSObject;
				if (_key == null)
					return null;
				return ObjectForKey (_key);
			}
			set {
				var nsokey = key as NSObject;
				var nsovalue = value as NSObject;

				if (nsokey == null || nsovalue == null)
					throw new ArgumentException ("You can only use NSObjects for keys and values in an NSMutableDictionary");
				
				SetObject (nsovalue, nsokey);
			}
		}

		ICollection IDictionary.Keys {
			get {return Keys;}
		}

		ICollection IDictionary.Values {
			get {return Values;}
		}

		#endregion

		#region IDictionary<NSObject, NSObject>

		public void Add (NSObject key, NSObject value)
		{
			// Inverted args.
			SetObject (value, key);
		}

		static readonly NSObject marker = new NSObject ();

		public bool Remove (NSObject key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var last = Count;
			RemoveObjectForKey (key);
			return last != Count;
		}

		public bool TryGetValue (NSObject key, out NSObject value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var keys   = NSArray.FromNSObjects (new [] {key});
			var values = ObjectsForKeys (keys, marker);
			if (object.ReferenceEquals (marker, values [0])) {
				value = null;
				return false;
			}
			value = values [0];
			return true;
		}

		public override NSObject this [NSObject key] {
			get {
				if (key == null)
					throw new ArgumentNullException ("key");
				return ObjectForKey (key);
			}
			set {
				if (key == null)
					throw new ArgumentNullException ("key");
				if (value == null)
					throw new ArgumentNullException ("value");
#if MONOMAC
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetObjectForKey_Handle, value.Handle, key.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selSetObjectForKey_Handle, value.Handle, key.Handle);
				}
#else
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle (selSetObjectForKey_), value.Handle, key.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, Selector.GetHandle (selSetObjectForKey_), value.Handle, key.Handle);
				}
#endif
			}
		}

		public override NSObject this [NSString key] {
			get {
				if (key == null)
					throw new ArgumentNullException ("key");
				return ObjectForKey (key);
			}
			set {
				if (key == null)
					throw new ArgumentNullException ("key");
				if (value == null)
					throw new ArgumentNullException ("value");
#if MONOMAC
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetObjectForKey_Handle, value.Handle, key.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selSetObjectForKey_Handle, value.Handle, key.Handle);
				}
#else
				if (IsDirectBinding) {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle (selSetObjectForKey_), value.Handle, key.Handle);
				} else {
					MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, Selector.GetHandle (selSetObjectForKey_), value.Handle, key.Handle);
				}
#endif
			}
		}

		public override NSObject this [string key] {
			get {
				if (key == null)
					throw new ArgumentNullException ("key");
				using (var nss = new NSString (key)){
					return ObjectForKey (nss);
				}
			}
			set {
				if (key == null)
					throw new ArgumentNullException ("key");
				if (value == null)
					throw new ArgumentNullException ("value");
				using (var nss = new NSString (key)){
#if MONOMAC
					if (IsDirectBinding) {
						MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetObjectForKey_Handle, value.Handle, nss.Handle);
					} else {
						MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selSetObjectForKey_Handle, value.Handle, nss.Handle);
					}
#else
					if (IsDirectBinding) {
						MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle (selSetObjectForKey_), value.Handle, nss.Handle);
					} else {
						MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, Selector.GetHandle (selSetObjectForKey_), value.Handle, nss.Handle);
					}
#endif
				}
			}
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Keys {
			get {return Keys;}
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Values {
			get {return Values;}
		}

		#endregion

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<KeyValuePair<NSObject, NSObject>> GetEnumerator ()
		{
			foreach (var key in Keys) {
				yield return new KeyValuePair<NSObject, NSObject> (key, ObjectForKey (key));
			}
		}

		public static NSMutableDictionary LowlevelFromObjectAndKey (IntPtr obj, IntPtr key)
		{
#if MONOMAC
			return Runtime.GetNSObject<NSMutableDictionary> (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr, selDictionaryWithObjectForKey_Handle, obj, key));
#else
			return Runtime.GetNSObject<NSMutableDictionary> (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr, Selector.GetHandle (selDictionaryWithObjectForKey_), obj, key));
#endif
		}

		public void LowlevelSetObject (IntPtr obj, IntPtr key)
		{
#if MONOMAC
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetObjectForKey_Handle, obj, key);
#else
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle (selSetObjectForKey_), obj, key);
#endif
		}

		public void LowlevelSetObject (NSObject obj, IntPtr key)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
#if MONOMAC
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetObjectForKey_Handle, obj.Handle, key);
#else
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle (selSetObjectForKey_), obj.Handle, key);
#endif
		}
	}
}
