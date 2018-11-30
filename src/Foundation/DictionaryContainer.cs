// 
// DictionaryContainer.cs: Foundation implementation for NSDictionary based setting classes
//
// Authors: Marek Safar (marek.safar@gmail.com)
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
//

using System;
using System.Runtime.InteropServices;

using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public abstract class DictionaryContainer
	{
#if !COREBUILD
		internal /*protected*/ DictionaryContainer ()
		{
			Dictionary = new NSMutableDictionary ();
		}

		internal /*protected*/ DictionaryContainer (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}
		
		public NSDictionary Dictionary { get; private set; }

		protected T[] GetArray<T> (NSString key) where T : NSObject
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
			if (value == IntPtr.Zero)
				return null;

			return NSArray.ArrayFromHandle<T> (value);
		}

		protected T[] GetArray<T> (NSString key, Func<IntPtr, T> creator)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
			if (value == IntPtr.Zero)
				return null;

			return NSArray.ArrayFromHandleFunc<T> (value, creator);
		}

		protected int? GetInt32Value (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).Int32Value;
		}

		protected long? GetLongValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).Int64Value;
		}

		protected uint? GetUIntValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).UInt32Value;
		}

		protected float? GetFloatValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).FloatValue;
		}

		protected double? GetDoubleValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;

			return ((NSNumber) value).DoubleValue;
		}

		protected bool? GetBoolValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var value = CFDictionary.GetValue (Dictionary.Handle, key.Handle);
			if (value == IntPtr.Zero)
				return null;

			return CFBoolean.GetValue (value);
		}

		protected NSDictionary GetNSDictionary (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			Dictionary.TryGetValue (key, out value);
			return value as NSDictionary;
		}

		protected string GetNSStringValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			Dictionary.TryGetValue (key, out value);
			return value as NSString;
		}

		protected string GetStringValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			NSObject value;
			if (!Dictionary.TryGetValue (key, out value))
				return null;
			
			return CFString.FetchString (value.Handle);
		}

		protected string GetStringValue (string key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			using (var str = new CFString (key)) {
				return CFString.FetchString (CFDictionary.GetValue (Dictionary.Handle, str.handle));
			}
		}		

		protected void SetArrayValue (NSString key, NSNumber[] values)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (values == null) {
				RemoveValue (key);
				return;
			}

			Dictionary [key] = NSArray.FromNSObjects (values);
		}

		protected void SetArrayValue (NSString key, string[] values)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (values == null) {
				RemoveValue (key);
				return;
			}

			Dictionary [key] = NSArray.FromStrings (values);
		}

		protected void SetArrayValue (NSString key, INativeObject[] values)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (values == null) {
				RemoveValue (key);
				return;			
			}

			CFMutableDictionary.SetValue (Dictionary.Handle, key.Handle, CFArray.FromNativeObjects (values).Handle);
		}

		#region Sets CFBoolean value

		protected void SetBooleanValue (NSString key, bool? value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			
			if (value == null) {
				RemoveValue (key);
				return;
			}

			CFMutableDictionary.SetValue (Dictionary.Handle, key.Handle, value.Value ? CFBoolean.True.Handle : CFBoolean.False.Handle);			
		}

		#endregion

		#region Sets NSNumber value

		protected void SetNumberValue (NSString key, int? value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (value == null) {
				RemoveValue (key);
				return;
			}

			Dictionary [key] = new NSNumber (value.Value);				
		}

		protected void SetNumberValue (NSString key, uint? value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (value == null) {
				RemoveValue (key);
				return;
			}

			Dictionary [key] = new NSNumber (value.Value);				
		}

		protected void SetNumberValue (NSString key, long? value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			Dictionary [key] = new NSNumber (value.Value);				
		}

		protected void SetNumberValue (NSString key, float? value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (value == null) {
				RemoveValue (key);
				return;
			}

			Dictionary [key] = new NSNumber (value.Value);				
		}

		protected void SetNumberValue (NSString key, double? value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (value == null) {
				RemoveValue (key);
				return;
			}

			Dictionary [key] = new NSNumber (value.Value);				
		}

		#endregion

		#region Sets NSString value

		protected void SetStringValue (NSString key, string value)
		{
			SetStringValue (key, value == null ? (NSString) null : new NSString (value));
		}

		protected void SetStringValue (NSString key, NSString value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (value == null) {
				RemoveValue (key);
			} else {
				Dictionary [key] = value;
			}			
		}

		#endregion

		#region Sets Native value

		protected void SetNativeValue (NSString key, INativeObject value, bool removeNullValue = true)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			if (value == null && removeNullValue) {
				RemoveValue (key);
			} else {
				CFMutableDictionary.SetValue (Dictionary.Handle, key.Handle, value == null ? IntPtr.Zero : value.Handle);
			}			
		}

		#endregion

		protected void RemoveValue (NSString key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			((NSMutableDictionary) Dictionary).Remove (key);
		}
#endif
	}
}
