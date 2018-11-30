// 
// ABMultiValue.cs: Implements the managed ABMultiValue
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc.
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
using System.Runtime.InteropServices;

using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AddressBook {

	static class ABMultiValue {
		public const int Mask = (1 << 8);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueCopyValueAtIndex")]
		public static extern IntPtr CopyValueAtIndex (IntPtr multiValue, int index);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueCopyLabelAtIndex")]
		public static extern IntPtr CopyLabelAtIndex (IntPtr multiValue, int index);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueGetIdentifierAtIndex")]
		public static extern int GetIdentifierAtIndex (IntPtr multiValue, int index);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueCopyArrayOfAllValues")]
		public static extern IntPtr CopyArrayOfAllValues (IntPtr multiValue);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueGetCount")]
		public static extern int GetCount (IntPtr multiValue);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueGetFirstIndexOfValue")]
		public static extern int GetFirstIndexOfValue (IntPtr multiValue, IntPtr value);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueGetIndexForIdentifier")]
		public static extern int GetIndexForIdentifier (IntPtr multiValue, int identifier);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueGetPropertyType")]
		public static extern ABPropertyType GetPropertyType (IntPtr multiValue);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueCreateMutable")]
		public static extern IntPtr CreateMutable (ABPropertyType type);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueCreateMutableCopy")]
		public static extern IntPtr CreateMutableCopy (IntPtr multiValue);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueAddValueAndLabel")]
		public static extern bool AddValueAndLabel (IntPtr multiValue, IntPtr value, IntPtr label, out int outIdentifier);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueReplaceValueAtIndex")]
		public static extern bool ReplaceValueAtIndex (IntPtr multiValue, IntPtr value, int index);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueReplaceLabelAtIndex")]
		public static extern bool ReplaceLabelAtIndex (IntPtr multiValue, IntPtr value, int index);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueInsertValueAndLabelAtIndex")]
		public static extern bool InsertValueAndLabelAtIndex (IntPtr multiValue, IntPtr value, IntPtr label, int index, out int outIdentifier);

		[DllImport (Constants.AddressBookLibrary, EntryPoint="ABMultiValueRemoveValueAndLabelAtIndex")]
		public static extern bool RemoveValueAndLabelAtIndex (IntPtr multiValue, int index);

		public static IntPtr ToIntPtr (NSObject value)
		{
			return value == null ? IntPtr.Zero : value.Handle;
		}
	}

	public struct ABMultiValueEntry<T>
	{
		ABMultiValue<T> self;
		int index;

		internal ABMultiValueEntry (ABMultiValue<T> self, int index)
		{
			this.self   = self;
			this.index  = index;
		}

		internal void AssertValid ()
		{
			if (self == null)
				throw new InvalidOperationException ();
		}

		public bool IsReadOnly {
			get {return self.IsReadOnly;}
		}

		IntPtr ToIntPtr (T value)
		{
			var mutable = self as ABMutableMultiValue<T>;
			if (mutable == null)
				throw CreateNotSupportedException ();
			return mutable.toNative (value);
		}

		static Exception CreateNotSupportedException ()
		{
			return new NotSupportedException ("ABMultiValue record is read-only. " + 
					"To update properties, use an ABMutableMultiValue<T>. " +
					"See ABMultiValue<T>.ToMutableMultiValue().");
		}

		public T Value {
			get {
				AssertValid ();
				return self.toManaged (ABMultiValue.CopyValueAtIndex (self.Handle, index));
			}
			set {
				if (IsReadOnly)
					throw CreateNotSupportedException ();
				AssertValid ();
				if (!ABMultiValue.ReplaceValueAtIndex (self.Handle, ToIntPtr (value), index))
					throw new ArgumentException ("Value cannot be set");
			}
		}

		public NSString Label {
			get {
				AssertValid ();
				return Runtime.GetNSObject<NSString> (ABMultiValue.CopyLabelAtIndex (self.Handle, index));
			}
			set {
				if (IsReadOnly)
					throw CreateNotSupportedException ();
				AssertValid ();
				ABMultiValue.ReplaceLabelAtIndex (self.Handle, ABMultiValue.ToIntPtr (value), index);
			}
		}

		public int Identifier {
			get {
				AssertValid ();
				return ABMultiValue.GetIdentifierAtIndex (self.Handle, index);
			}
		}
	}

	public class ABMultiValue<T> : INativeObject, IDisposable, IEnumerable<ABMultiValueEntry<T>>
	{
		IntPtr handle;
		internal Converter<IntPtr, T> toManaged;
		internal Converter<T, IntPtr> toNative;

		internal ABMultiValue (IntPtr handle)
			: this (handle, 
					v => (T) (object) Runtime.GetNSObject (v), 
					v => v == null ? IntPtr.Zero : ((INativeObject) v).Handle)
		{
			if (!typeof (NSObject).IsAssignableFrom (typeof (T)))
				throw new InvalidOperationException ("T must be an NSObject!");
		}

		internal ABMultiValue (IntPtr handle, Converter<IntPtr, T> toManaged, Converter<T, IntPtr> toNative)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("Handle must not be null.", "handle");
			if (toManaged == null)
				throw new ArgumentNullException ("toManaged");
			if (toNative == null)
				throw new ArgumentNullException ("toNative");

			this.handle = handle;
			this.toManaged = toManaged;
			this.toNative  = toNative;
		}

		~ABMultiValue ()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero)
				CFObject.CFRelease (handle);
			handle = IntPtr.Zero;
		}

		internal void AssertValid ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException ("");
		}

		public IntPtr Handle {
			get {
				AssertValid ();
				return handle;
			}
		}

		public virtual bool IsReadOnly {
			get {
				AssertValid ();
				return true;
			}
		}

		public ABPropertyType PropertyType {
			get {return ABMultiValue.GetPropertyType (Handle);}
		}

		public T[] GetValues ()
		{
			return NSArray.ArrayFromHandle (ABMultiValue.CopyArrayOfAllValues (Handle), toManaged)
				?? new T [0];
		}

		public int Count {
			get {
				return ABMultiValue.GetCount (Handle);
			}
		}

		public ABMultiValueEntry<T> this [int index] {
			get {
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException ();
				return new ABMultiValueEntry<T> (this, index);
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<ABMultiValueEntry<T>> GetEnumerator ()
		{
			int c = Count;
			for (int i = 0; i < c; ++i)
				yield return this [i];
		}

		public int GetFirstIndexOfValue (NSObject value)
		{
			return ABMultiValue.GetFirstIndexOfValue (Handle, value.Handle);
		}

		public int GetIndexForIdentifier (int identifier)
		{
			return ABMultiValue.GetIndexForIdentifier (Handle, identifier);
		}

		public ABMutableMultiValue<T> ToMutableMultiValue ()
		{
			return new ABMutableMultiValue<T> (ABMultiValue.CreateMutableCopy (Handle), toManaged, toNative);
		}
	}

	public class ABMutableMultiValue<T> : ABMultiValue<T>
	{
		internal ABMutableMultiValue (IntPtr handle)
			: base (handle)
		{
		}

		internal ABMutableMultiValue (IntPtr handle, Converter<IntPtr, T> toManaged, Converter<T, IntPtr> toNative)
			: base (handle, toManaged, toNative)
		{
		}

		public override bool IsReadOnly {
			get {
				AssertValid ();
				return false;
			}
		}

		public bool Add (T value, NSString label)
		{
			int _;
			return ABMultiValue.AddValueAndLabel (Handle, 
						toNative (value),
						ABMultiValue.ToIntPtr (label),
						out _);
		}

		public bool Insert (int index, T value, NSString label)
		{
			int _;
			return ABMultiValue.InsertValueAndLabelAtIndex (Handle,
					toNative (value),
					ABMultiValue.ToIntPtr (label),
					index,
					out _);
		}

		public bool RemoveAt (int index)
		{
			return ABMultiValue.RemoveValueAndLabelAtIndex (Handle, index);
		}
	}

	public class ABMutableDateMultiValue : ABMutableMultiValue<NSDate> {
		public ABMutableDateMultiValue ()
			: base (ABMultiValue.CreateMutable (ABPropertyType.MultiDateTime))
		{
		}
	}

	public class ABMutableDictionaryMultiValue : ABMutableMultiValue<NSDictionary> {
		public ABMutableDictionaryMultiValue ()
			: base (ABMultiValue.CreateMutable (ABPropertyType.MultiDictionary))
		{
		}
	}

	public class ABMutableStringMultiValue : ABMutableMultiValue<string> {
		public ABMutableStringMultiValue ()
			: base (ABMultiValue.CreateMutable (ABPropertyType.MultiString), 
					ABPerson.ToString, ABPerson.ToIntPtr)
		{
		}
	}
}

