// 
// CTFont.cs: Implements the managed CTFont
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;
using MonoMac.Foundation;

using CGGlyph = System.UInt16;

namespace MonoMac.CoreText {

	public enum CTFontManagerScope  {
		None = 0, Process = 1, User = 2, Session = 3
	}

	public enum CTFontManagerAutoActivation {
		Default = 0, Disabled = 1, Enabled = 2, PromptUser = 3,
	}

	public enum CTFontManagerError {
		None = 0,
		FileNotFount = 101,
		InsufficientPermissions = 102,
		UnrecognizedFormat = 103,
		InvalidFontData = 104,
		AlreadyRegistered = 105,
		NotRegistered = 201,
		InUse = 202,
		SystemRequired = 202
	}
	
	[Since (4,1)]
	public class CTFontManager {
		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerIsSupportedFont (IntPtr url);
		public static bool IsFontSupported (NSUrl url)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			return CTFontManagerIsSupportedFont (url.Handle);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerRegisterFontsForURL (IntPtr fontUrl, CTFontManagerScope scope, IntPtr error);
		public static NSError RegisterFontsForUrl (NSUrl fontUrl, CTFontManagerScope scope)
		{
			if (fontUrl == null)
				throw new ArgumentNullException ("fontUrl");
			
			NSError e = new NSError (ErrorDomain, 0);

			if (CTFontManagerRegisterFontsForURL (fontUrl.Handle, scope, e.Handle))
				return null;
			else
				return e;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerRegisterFontsForURLs(IntPtr arrayRef, CTFontManagerScope scope, IntPtr error);
		public static NSError [] RegisterFontsForUrl (NSUrl [] fontUrls, CTFontManagerScope scope)
		{
			if (fontUrls == null)
				throw new ArgumentNullException ("fontUrls");

			foreach (var furl in fontUrls)
				if (furl == null)
					throw new ArgumentException ("contains a null entry", "fontUrls");

			var arr = NSArray.FromNSObjects (fontUrls);
			var _errors = new NSError [fontUrls.Length];
			for (int i = 0; i < fontUrls.Length; i++)
				_errors [i] = new NSError (ErrorDomain, 0);
			var errors = NSArray.FromNSObjects (_errors);

			if (CTFontManagerRegisterFontsForURLs (arr.Handle, scope, errors.Handle))
				return null;
			else
				return _errors;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerUnregisterFontsForURL(IntPtr fotUrl, CTFontManagerScope scope, IntPtr error);
		public static NSError UnregisterFontsForUrl (NSUrl fontUrl, CTFontManagerScope scope)
		{
			if (fontUrl == null)
				throw new ArgumentNullException ("fontUrl");

			var e = new NSError (ErrorDomain, 0);
			if (CTFontManagerUnregisterFontsForURLs (fontUrl.Handle, scope, e.Handle))
				return null;
			else
				return e;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerUnregisterFontsForURLs(IntPtr arrayRef, CTFontManagerScope scope, IntPtr error);
		public static NSError [] UnregisterFontsForUrl (NSUrl [] fontUrls, CTFontManagerScope scope)
		{
			if (fontUrls == null)
				throw new ArgumentNullException ("fontUrls");

			foreach (var furl in fontUrls)
				if (furl == null)
					throw new ArgumentException ("contains a null entry", "fontUrls");

			var arr = NSArray.FromNSObjects (fontUrls);
			var _errors = new NSError [fontUrls.Length];
			for (int i = 0; i < fontUrls.Length; i++)
				_errors [i] = new NSError (ErrorDomain, 0);
			var errors = NSArray.FromNSObjects (_errors);

			if (CTFontManagerUnregisterFontsForURLs (arr.Handle, scope, errors.Handle))
				return null;
			else
				return _errors;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerRegisterGraphicsFont (IntPtr cgfont, out IntPtr error);

		public static bool RegisterGraphicsFont (CGFont font, out NSError error)
		{
			if (font == null)
				throw new ArgumentNullException ("font");
			IntPtr h;
			var ret = CTFontManagerRegisterGraphicsFont (font.Handle, out h);
			if (ret)
				error = null;
			else 
				error = new NSError (h);
			return ret;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontManagerUnregisterGraphicsFont (IntPtr cgfont, out IntPtr error);
		public static bool UnregisterGraphicsFont (CGFont font, out NSError error)
		{
			if (font == null)
				throw new ArgumentNullException ("font");
			IntPtr h;
			var ret = CTFontManagerUnregisterGraphicsFont (font.Handle, out h);
			if (ret)
				error = null;
			else 
				error = new NSError (h);
			return ret;
		}
		
		static CTFontManager ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreTextLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				ErrorDomain  = Dlfcn.GetStringConstant (handle, "kCTFontManagerErrorDomain");
				ErrorFontUrlsKey  = Dlfcn.GetStringConstant (handle, "kCTFontManagerErrorFontURLsKey");

			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}
		
		public readonly static NSString ErrorDomain, ErrorFontUrlsKey;
	}
}

