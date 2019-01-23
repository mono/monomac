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

namespace MonoMac.Foundation {

	public partial class NSUrl {

		public NSUrl (string path, string relativeToUrl)
			: this (path, new NSUrl (relativeToUrl))
		{
		}

		public override bool Equals (object t)
		{
			if (t == null)
				return false;
			
			if (t is NSUrl){
				return IsEqual ((NSUrl) t);
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}

		// Converts from an NSURL to a System.Uri
		public static implicit operator Uri (NSUrl url)
		{
			if (url.RelativePath == url.Path)
				return new Uri (url.AbsoluteString, UriKind.Absolute);
			else
				return new Uri (url.RelativePath, UriKind.Relative);
		}

		public static implicit operator NSUrl (Uri uri)
		{
			if (uri.IsAbsoluteUri)
				return new NSUrl (uri.AbsoluteUri);
			else
				return new NSUrl (uri.PathAndQuery);
		}

		public static NSUrl FromFilename (string url)
		{
			return new NSUrl (url, false);
		}
		
		public NSUrl MakeRelative (string url)
		{
			return _FromStringRelative (url, this);
		}

		public override string ToString ()
		{
			return AbsoluteString ?? base.ToString ();
		}

		public bool TryGetResource (string key, out NSObject value, out NSError error)
		{
			return GetResourceValue (out value, key, out error);
		}

		public bool TryGetResource (string key, out NSObject value)
		{
			NSError error;
			return GetResourceValue (out value, key, out error);
		}

		public bool SetResource (string key, NSObject value, out NSError error)
		{
			return SetResourceValue (value, key, out error);
		}

		public bool SetResource (string key, NSObject value)
		{
			NSError error;
			return SetResourceValue (value, key, out error);
		}
	}
}
