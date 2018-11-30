//
// CFHTTPMessage.cs:
//
// Authors:
//      Martin Baulig (martin.baulig@gmail.com)
//      Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012-2013 Xamarin Inc. (http://www.xamarin.com)
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
using System;
using System.Net;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

#if COREFX
using AuthenticationException = System.InvalidOperationException;
using InvalidCredentialException = System.InvalidOperationException;

static class HttpVersion
{
	public static Version Version10 = new Version(1, 0);
	public static Version Version11 = new Version(1, 1);
}
#else
using AuthenticationException = System.Security.Authentication.AuthenticationException;
using InvalidCredentialException = System.Security.Authentication.InvalidCredentialException;
#endif

namespace MonoMac.CoreServices {

	public class CFHTTPMessage : CFType, INativeObject, IDisposable {
		internal IntPtr handle;

		internal CFHTTPMessage (IntPtr handle)
			: this (handle, false)
		{
		}

		internal CFHTTPMessage (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);
			this.handle = handle;
		}

		static readonly NSString _HTTPVersion1_0;
		static readonly NSString _HTTPVersion1_1;
		static readonly NSString _AuthenticationSchemeBasic;
		static readonly NSString _AuthenticationSchemeNegotiate;
		static readonly NSString _AuthenticationSchemeNTLM;
		static readonly NSString _AuthenticationSchemeDigest;
		static readonly NSString _AuthenticationUsername;
		static readonly NSString _AuthenticationPassword;
		static readonly NSString _AuthenticationAccountDomain;

		static CFHTTPMessage ()
		{
			var handle = Dlfcn.dlopen (Constants.CFNetworkLibrary, 0);
			if (handle == IntPtr.Zero)
				throw new InvalidOperationException ();

			try {
				_HTTPVersion1_0 = GetStringConstant (handle, "kCFHTTPVersion1_0");
				_HTTPVersion1_1 = GetStringConstant (handle, "kCFHTTPVersion1_1");

				_AuthenticationSchemeBasic = GetStringConstant (
					handle, "kCFHTTPAuthenticationSchemeBasic");
				_AuthenticationSchemeNegotiate = GetStringConstant (
					handle, "kCFHTTPAuthenticationSchemeNegotiate");
				_AuthenticationSchemeNTLM = GetStringConstant (
					handle, "kCFHTTPAuthenticationSchemeNTLM");
				_AuthenticationSchemeDigest = GetStringConstant (
					handle, "kCFHTTPAuthenticationSchemeDigest");

				_AuthenticationUsername = GetStringConstant (
					handle, "kCFHTTPAuthenticationUsername");
				_AuthenticationPassword = GetStringConstant (
					handle, "kCFHTTPAuthenticationPassword");
				_AuthenticationAccountDomain = GetStringConstant (
					handle, "kCFHTTPAuthenticationAccountDomain");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}

		static NSString GetStringConstant (IntPtr handle, string name)
		{
			var result = Dlfcn.GetStringConstant (handle, name);
			if (result == null)
				throw new InvalidOperationException (
					string.Format ("Cannot get '{0}' property.", name));
			return result;
		}

		[DllImport (Constants.CFNetworkLibrary, EntryPoint="CFHTTPMessageGetTypeID")]
		public extern static int GetTypeID ();

		~CFHTTPMessage ()
		{
			Dispose (false);
		}
		
		protected void CheckHandle ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException (GetType ().Name);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get {
				CheckHandle ();
				return handle;
			}
		}
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero) {
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		static IntPtr GetVersion (Version version)
		{
			if ((version == null) || version.Equals (HttpVersion.Version11))
				return _HTTPVersion1_1.Handle;
			else if (version.Equals (HttpVersion.Version10))
				return _HTTPVersion1_0.Handle;
			else
				throw new ArgumentException ();
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPMessageCreateEmpty (IntPtr allocator, bool isRequest);

		public static CFHTTPMessage CreateEmpty (bool request)
		{
			var handle = CFHTTPMessageCreateEmpty (IntPtr.Zero, request);
			if (handle == IntPtr.Zero)
				return null;

			return new CFHTTPMessage (handle);			
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPMessageCreateRequest (IntPtr allocator, IntPtr requestMethod,
		                                                 IntPtr url, IntPtr httpVersion);

		public static CFHTTPMessage CreateRequest (CFUrl url, NSString method, Version version)
		{
			var handle = CFHTTPMessageCreateRequest (
				IntPtr.Zero, method.Handle, url.Handle, GetVersion (version));
			if (handle == IntPtr.Zero)
				return null;

			return new CFHTTPMessage (handle);
		}

#if !COREFX
		public static CFHTTPMessage CreateRequest (Uri uri, string method)
		{
			return CreateRequest (uri, method, null);
		}

		public static CFHTTPMessage CreateRequest (Uri uri, string method, Version version)
		{
			CFUrl urlRef = null;
			NSString methodRef = null;

			var escaped = Uri.EscapeUriString (uri.ToString ());

			try {
				urlRef = CFUrl.FromUrlString (escaped, null);
				if (urlRef == null)
					throw new ArgumentException ("Invalid URL.");
				methodRef = new NSString (method);

				var msg = CreateRequest (urlRef, methodRef, version);
				if (msg == null)
					throw new ArgumentException ("Invalid URL.");

				return msg;
			} finally {
				if (urlRef != null)
					urlRef.Dispose ();
				if (methodRef != null)
					methodRef.Dispose ();
			}
		}
#endif

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPMessageIsRequest (IntPtr handle);

		public bool IsRequest {
			get {
				CheckHandle ();
				return CFHTTPMessageIsRequest (Handle);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static CFIndex CFHTTPMessageGetResponseStatusCode (IntPtr handle);

#if COREFX
		public CFIndex ResponseStatusCode {
			get {
				if (IsRequest)
					throw new InvalidOperationException ();
				return CFHTTPMessageGetResponseStatusCode (Handle);
			}
		}
#else
		public HttpStatusCode ResponseStatusCode {
			get {
				if (IsRequest)
					throw new InvalidOperationException ();
				int status = CFHTTPMessageGetResponseStatusCode (Handle);
				return (HttpStatusCode)status;
			}
		}
#endif

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPMessageCopyResponseStatusLine (IntPtr handle);

		public string ResponseStatusLine {
			get {
				if (IsRequest)
					throw new InvalidOperationException ();
				var ptr = CFHTTPMessageCopyResponseStatusLine (Handle);
				if (ptr == IntPtr.Zero)
					return null;
				using (var line = new NSString (ptr))
					return line.ToString ();
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPMessageCopyVersion (IntPtr handle);

		public Version Version {
			get {
				CheckHandle ();
				IntPtr ptr = CFHTTPMessageCopyVersion (handle);
				try {
					if (ptr.Equals (_HTTPVersion1_0.Handle))
						return HttpVersion.Version10;
					else
						return HttpVersion.Version11;
				} finally {
					if (ptr != IntPtr.Zero)
						CFObject.CFRelease (ptr);
				}
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPMessageIsHeaderComplete (IntPtr handle);

		public bool IsHeaderComplete {
			get {
				CheckHandle ();
				return CFHTTPMessageIsHeaderComplete (Handle);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPMessageAppendBytes (IntPtr message, ref byte[] newBytes, CFIndex numBytes);

		public bool AppendBytes (byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");

			return AppendBytes (bytes, bytes.Length);
		}

		public bool AppendBytes (byte[] bytes, int count)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");

			return CFHTTPMessageAppendBytes (Handle, ref bytes, count);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFHTTPMessageCopyAllHeaderFields (IntPtr handle);

		public NSDictionary GetAllHeaderFields ()
		{
			CheckHandle ();
			IntPtr ptr = CFHTTPMessageCopyAllHeaderFields (handle);
			if (ptr == IntPtr.Zero)
				return null;
			return new NSDictionary (ptr);
		}

		#region Authentication

		struct CFStreamError {
			public int domain;
			public int code;
		}

		enum ErrorHTTPAuthentication {
			TypeUnsupported = -1000,
			BadUserName = -1001,
			BadPassword = -1002
		}

		AuthenticationException GetException (ErrorHTTPAuthentication code)
		{
			switch (code) {
			case ErrorHTTPAuthentication.BadUserName:
				throw new InvalidCredentialException ("Bad username.");
			case ErrorHTTPAuthentication.BadPassword:
				throw new InvalidCredentialException ("Bad password.");
			case ErrorHTTPAuthentication.TypeUnsupported:
				throw new AuthenticationException ("Authentication type not supported.");
			default:
				throw new AuthenticationException ("Unknown error.");
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPMessageApplyCredentials (IntPtr request, IntPtr auth,
		                                                  IntPtr user, IntPtr pass,
		                                                  out CFStreamError error);

#if COREFX
		public void ApplyCredentials (CFHTTPAuthentication auth, string userName, string password, string domain = null)
		{
			if (auth.RequiresAccountDomain) {
				ApplyCredentialDictionary (auth, userName, password, domain);
				return;
			}

			var cfusername = new CFString (userName);
			var cfpassword = new CFString (password);

			try {
				CFStreamError error;

				var ok = CFHTTPMessageApplyCredentials (
					Handle, auth.Handle, cfusername.Handle, cfpassword.Handle,
					out error);
				if (!ok)
					throw GetException ((ErrorHTTPAuthentication)error.code);
			} finally {
				cfusername.Dispose ();
				cfpassword.Dispose ();
			}
		}
#else
		public void ApplyCredentials (CFHTTPAuthentication auth, NetworkCredential credential)
		{
			if (auth.RequiresAccountDomain) {
				ApplyCredentialDictionary (auth, credential);
				return;
			}

			var username = new CFString (credential.UserName);
			var password = new CFString (credential.Password);

			try {
				CFStreamError error;

				var ok = CFHTTPMessageApplyCredentials (
					Handle, auth.Handle, username.Handle, password.Handle,
					out error);
				if (!ok)
					throw GetException ((ErrorHTTPAuthentication)error.code);
			} finally {
				username.Dispose ();
				password.Dispose ();
			}
		}
#endif

		public enum AuthenticationScheme {
			Default,
			Basic,
			Negotiate,
			NTLM,
			Digest
		}

		internal static IntPtr GetAuthScheme (AuthenticationScheme scheme)
		{
			switch (scheme) {
			case AuthenticationScheme.Default:
				return IntPtr.Zero;
			case AuthenticationScheme.Basic:
				return _AuthenticationSchemeBasic.Handle;
			case AuthenticationScheme.Negotiate:
				return _AuthenticationSchemeNegotiate.Handle;
			case AuthenticationScheme.NTLM:
				return _AuthenticationSchemeNTLM.Handle;
			case AuthenticationScheme.Digest:
				return _AuthenticationSchemeDigest.Handle;
			default:
				throw new ArgumentException ();
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPMessageAddAuthentication (IntPtr request, IntPtr response,
		                                                   IntPtr username, IntPtr password,
		                                                   IntPtr scheme, bool forProxy);

		public bool AddAuthentication (CFHTTPMessage failureResponse, NSString username,
		                               NSString password, AuthenticationScheme scheme,
		                               bool forProxy)
		{
			return CFHTTPMessageAddAuthentication (
				Handle, failureResponse.Handle, username.Handle,
				password.Handle, GetAuthScheme (scheme), forProxy);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static bool CFHTTPMessageApplyCredentialDictionary (IntPtr request, IntPtr auth,
		                                                           IntPtr dict, out CFStreamError error);

#if COREFX
		public void ApplyCredentialDictionary (CFHTTPAuthentication auth, string userName, string password, string domain = null)
		{
			var keys = new NSString [3];
			var values = new CFString [3];
			keys [0] = _AuthenticationUsername;
			keys [1] = _AuthenticationPassword;
			keys [2] = _AuthenticationAccountDomain;
			values [0] = (CFString)userName;
			values [1] = (CFString)password;
			values [2] = domain != null ? (CFString)domain : null;

			var dict = CFDictionary.FromObjectsAndKeys (values, keys);

			try {
				CFStreamError error;
				var ok = CFHTTPMessageApplyCredentialDictionary (
					Handle, auth.Handle, dict.Handle, out error);
				if (ok)
					return;
				throw GetException ((ErrorHTTPAuthentication)error.code);
			} finally {
				dict.Dispose ();
				values [0].Dispose ();
				values [1].Dispose ();
				if (values [2] != null)
					values [2].Dispose ();
			}
		}
#else
		public void ApplyCredentialDictionary (CFHTTPAuthentication auth, NetworkCredential credential)
		{
			var keys = new NSString [3];
			var values = new CFString [3];
			keys [0] = _AuthenticationUsername;
			keys [1] = _AuthenticationPassword;
			keys [2] = _AuthenticationAccountDomain;
			values [0] = (CFString)credential.UserName;
			values [1] = (CFString)credential.Password;
			values [2] = credential.Domain != null ? (CFString)credential.Domain : null;

			var dict = CFDictionary.FromObjectsAndKeys (values, keys);

			try {
				CFStreamError error;
				var ok = CFHTTPMessageApplyCredentialDictionary (
					Handle, auth.Handle, dict.Handle, out error);
				if (ok)
					return;
				throw GetException ((ErrorHTTPAuthentication)error.code);
			} finally {
				dict.Dispose ();
				values [0].Dispose ();
				values [1].Dispose ();
				if (values [2] != null)
					values [2].Dispose ();
			}
		}
#endif

		#endregion

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFHTTPMessageSetHeaderFieldValue (IntPtr message, IntPtr headerField,
		                                                     IntPtr value);

		public void SetHeaderFieldValue (string name, string value)
		{
			NSString nstr = (NSString)name;
			NSString vstr = value != null ? (NSString)value : null;
			IntPtr vptr = vstr != null ? vstr.Handle : IntPtr.Zero;

			CFHTTPMessageSetHeaderFieldValue (Handle, nstr.Handle, vptr);

			nstr.Dispose ();
			if (vstr != null)
				vstr.Dispose ();
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFHTTPMessageSetBody (IntPtr message, IntPtr data);

		internal void SetBody (CFData data)
		{
			CFHTTPMessageSetBody (Handle, data.Handle);
		}

		public void SetBody (byte[] buffer)
		{
			using (var data = new CFDataBuffer (buffer))
				CFHTTPMessageSetBody (Handle, data.Handle);
		}
	}
}
