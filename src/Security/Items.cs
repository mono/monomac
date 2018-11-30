// 
// Items.cs: Implements the KeyChain query access APIs
//
// We use strong types and a helper SecQuery class to simplify the
// creation of the dictionary used to query the Keychain
// 
// Authors: Miguel de Icaza
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
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using System.Runtime.InteropServices;
using System.Collections;

namespace MonoMac.Security {

	public enum SecKind {
		InternetPassword,
#if !MONOMAC
		GenericPassword, Certificate, Key, Identity
#endif
	}

	public enum SecAccessible {
		WhenUnlocked,
		AfterFirstUnlock,
		Always,
		WhenUnlockedThisDeviceOnly,
		AfterFirstUnlockThisDeviceOnly,
		AlwaysThisDeviceOnly
	}

	public enum SecProtocol {
		Ftp, FtpAccount, Http, Irc, Nntp, Pop3, Smtp, Socks, Imap, Ldap, AppleTalk, Afp, Telnet, Ssh,
		Ftps, Https, HttpProxy, HttpsProxy, FtpProxy, Smb, Rtsp, RtspProxy, Daap, Eppc, Ipp,
		Nntps, Ldaps, Telnets, Imaps, Ircs, Pop3s, 
	}

	public enum SecAuthenticationType {
		Ntlm, Msn, Dpa, Rpa, HttpBasic, HttpDigest, HtmlForm, Default
	}

	public enum SecKeyClass {
		Public, Private, Symmetric
	}

	public enum SecKeyType {
		RSA, EC
	}

	public static class SecKeyChain {
		static NSNumber SetLimit (NSMutableDictionary dict, int max)
		{
			NSNumber n = null;
			IntPtr val;
			if (max == -1)
				val = SecMatchLimit.MatchLimitAll;
			else if (max == 1)
				val = SecMatchLimit.MatchLimitOne;
			else {
				n = NSNumber.FromInt32 (max);
				val = n.Handle;
			}
			
			dict.LowlevelSetObject (val, SecItem.MatchLimit);
			return n;
		}
		
		public static NSData QueryAsData (SecRecord query, bool wantPersistentReference, out SecStatusCode status)
		{
			if (query == null)
				throw new ArgumentNullException ("query");

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				SetLimit (copy, 1);
				copy.LowlevelSetObject (CFBoolean.True.Handle, SecItem.ReturnData);

				IntPtr ptr;
				status = SecItem.SecItemCopyMatching (copy.Handle, out ptr);
				if (status == SecStatusCode.Success)
					return new NSData (ptr, false);
				return null;
			}
		}

		public static NSData [] QueryAsData (SecRecord query, bool wantPersistentReference, int max, out SecStatusCode status)
		{
			if (query == null)
				throw new ArgumentNullException ("query");

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				var n = SetLimit (copy, max);
				copy.LowlevelSetObject (CFBoolean.True.Handle, SecItem.ReturnData);

				IntPtr ptr;
				status = SecItem.SecItemCopyMatching (copy.Handle, out ptr);
				n = null;
				if (status == SecStatusCode.Success){
					if (max == 1)
						return new NSData [] { new NSData (ptr, false) };

					var array = new NSArray (ptr);
					var records = new NSData [array.Count];
					for (uint i = 0; i < records.Length; i++)
						records [i] = new NSData (array.ValueAt (i), false);
					return records;
				}
				return null;
			}
		}
		
		public static NSData QueryAsData (SecRecord query)
		{
			SecStatusCode status;
			return QueryAsData (query, false, out status);
		}

		public static NSData [] QueryAsData (SecRecord query, int max)
		{
			SecStatusCode status;
			return QueryAsData (query, false, max, out status);
		}
		
		public static SecRecord QueryAsRecord (SecRecord query, out SecStatusCode result)
		{
			if (query == null)
				throw new ArgumentNullException ("query");
			
			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				SetLimit (copy, 1);
				copy.LowlevelSetObject (CFBoolean.True.Handle, SecItem.ReturnAttributes);
				copy.LowlevelSetObject (CFBoolean.True.Handle, SecItem.ReturnData);
				IntPtr ptr;
				result = SecItem.SecItemCopyMatching (copy.Handle, out ptr);
				if (result == SecStatusCode.Success)
					return new SecRecord (new NSMutableDictionary (ptr, false));
				return null;
			}
		}
		
		public static SecRecord [] QueryAsRecord (SecRecord query, int max, out SecStatusCode result)
		{
			if (query == null)
				throw new ArgumentNullException ("query");
			
			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				copy.LowlevelSetObject (CFBoolean.True.Handle, SecItem.ReturnAttributes);
				var n = SetLimit (copy, max);
				
				IntPtr ptr;
				result = SecItem.SecItemCopyMatching (copy.Handle, out ptr);
				n = null;
				if (result == SecStatusCode.Success){
					var array = new NSArray (ptr);
					var records = new SecRecord [array.Count];
					for (uint i = 0; i < records.Length; i++)
						records [i] = new SecRecord (new NSMutableDictionary (array.ValueAt (i), false));
					return records;
				}
				return null;
			}
		}

		public static SecStatusCode Add (SecRecord record)
		{
			if (record == null)
				throw new ArgumentNullException ("record");
			return SecItem.SecItemAdd (record.queryDict.Handle, IntPtr.Zero);
			
		}

		public static SecStatusCode Remove (SecRecord record)
		{
			if (record == null)
				throw new ArgumentNullException ("record");
			return SecItem.SecItemDelete (record.queryDict.Handle);
		}
		
		public static SecStatusCode Update (SecRecord query, SecRecord newAttributes)
		{
			if (query == null)
				throw new ArgumentNullException ("record");
			if (newAttributes == null)
				throw new ArgumentNullException ("newAttributes");

			return SecItem.SecItemUpdate (query.queryDict.Handle, newAttributes.queryDict.Handle);

		}
#if MONOMAC
		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainAddGenericPassword (
			IntPtr keychain,
			int serviceNameLength,
			IntPtr serviceName,
			int accountNameLength,
			IntPtr accountName,
			int passwordLength,
			IntPtr passwordData,
			IntPtr itemRef);

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainFindGenericPassword (
			IntPtr keychainOrArray,
			int serviceNameLength,
			IntPtr serviceName,
			int accountNameLength,
			IntPtr accountName,
			out int passwordLength,
			out IntPtr passwordData,
			IntPtr itemRef);

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainAddInternetPassword (
			IntPtr keychain,
			int serverNameLength,
			IntPtr serverName,
			int securityDomainLength,
			IntPtr securityDomain,
			int accountNameLength,
			IntPtr accountName,
			int pathLength,
			IntPtr path,
			short port,
			IntPtr protocol,
			IntPtr authenticationType,
			int passwordLength,
			IntPtr passwordData,
			IntPtr itemRef);

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainFindInternetPassword (
			IntPtr keychain,
			int serverNameLength,
			IntPtr serverName,
			int securityDomainLength,
			IntPtr securityDomain,
			int accountNameLength,
			IntPtr accountName,
			int pathLength,
			IntPtr path,
			short port,
			IntPtr protocol,
			IntPtr authenticationType,
			out int passwordLength,
			out IntPtr passwordData,
			IntPtr itemRef);

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainItemFreeContent (IntPtr attrList, IntPtr data);

		public static SecStatusCode AddInternetPassword (
			string serverName,
			string accountName,
			byte[] password,
			SecProtocol protocolType = SecProtocol.Http,
			short port = 0,
			string path = null,
			SecAuthenticationType authenticationType = SecAuthenticationType.Default,
			string securityDomain = null)
		{
			GCHandle serverHandle = new GCHandle ();
			GCHandle securityDomainHandle = new GCHandle ();
			GCHandle accountHandle = new GCHandle ();
			GCHandle pathHandle = new GCHandle ();
			GCHandle passwordHandle = new GCHandle ();
			
			int serverNameLength = 0;
			IntPtr serverNamePtr = IntPtr.Zero;
			int securityDomainLength = 0;
			IntPtr securityDomainPtr = IntPtr.Zero;
			int accountNameLength = 0;
			IntPtr accountNamePtr = IntPtr.Zero;
			int pathLength = 0;
			IntPtr pathPtr = IntPtr.Zero;
			int passwordLength = 0;
			IntPtr passwordPtr = IntPtr.Zero;
			
			try {
				
				if (!String.IsNullOrEmpty (serverName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (serverName);
					serverNameLength = bytes.Length;
					serverHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
					serverNamePtr = serverHandle.AddrOfPinnedObject ();
				}
				
				if (!String.IsNullOrEmpty (securityDomain)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (securityDomain);
					securityDomainLength = bytes.Length;
					securityDomainHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
				}
				
				if (!String.IsNullOrEmpty (accountName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (accountName);
					accountNameLength = bytes.Length;
					accountHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					accountNamePtr = accountHandle.AddrOfPinnedObject ();
				}
				
				if (!String.IsNullOrEmpty(path)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (path);
					pathLength = bytes.Length;
					pathHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					pathPtr = pathHandle.AddrOfPinnedObject ();
				}
				
				if (password != null && password.Length > 0) {
					passwordLength = password.Length;
					passwordHandle = GCHandle.Alloc (password, GCHandleType.Pinned);
					passwordPtr = passwordHandle.AddrOfPinnedObject ();
				}
				
				return SecKeychainAddInternetPassword (
					IntPtr.Zero,
					serverNameLength,
					serverNamePtr,
					securityDomainLength,
					securityDomainPtr,
					accountNameLength,
					accountNamePtr,
					pathLength,
					pathPtr,
					port,
					SecProtocolKeys.FromSecProtocol (protocolType),
					KeysAuthenticationType.FromSecAuthenticationType (authenticationType),
					passwordLength,
					passwordPtr,
					IntPtr.Zero);
			} finally {
				if (serverHandle.IsAllocated)
					serverHandle.Free ();
				if (accountHandle.IsAllocated)
					accountHandle.Free ();
				if (passwordHandle.IsAllocated)
					passwordHandle.Free ();
				if (securityDomainHandle.IsAllocated)
					securityDomainHandle.Free ();
				if (pathHandle.IsAllocated)
					pathHandle.Free ();
			}
		}
		
		
		public static SecStatusCode FindInternetPassword(
			string serverName,
			string accountName,
			out byte[] password,
			SecProtocol protocolType = SecProtocol.Http,
			short port = 0,
			string path = null,
			SecAuthenticationType authenticationType = SecAuthenticationType.Default,
			string securityDomain = null)
		{
			password = null;
			
			GCHandle serverHandle = new GCHandle ();
			GCHandle securityDomainHandle = new GCHandle ();
			GCHandle accountHandle = new GCHandle ();
			GCHandle pathHandle = new GCHandle ();
			
			int serverNameLength = 0;
			IntPtr serverNamePtr = IntPtr.Zero;
			int securityDomainLength = 0;
			IntPtr securityDomainPtr = IntPtr.Zero;
			int accountNameLength = 0;
			IntPtr accountNamePtr = IntPtr.Zero;
			int pathLength = 0;
			IntPtr pathPtr = IntPtr.Zero;
			IntPtr passwordPtr = IntPtr.Zero;
			
			try {
				if (!String.IsNullOrEmpty(serverName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (serverName);
					serverNameLength = bytes.Length;
					serverHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					serverNamePtr = serverHandle.AddrOfPinnedObject ();
				}
				
				if (!String.IsNullOrEmpty(securityDomain)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (securityDomain);
					securityDomainLength = bytes.Length;
					securityDomainHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
				}
				
				if (!String.IsNullOrEmpty(accountName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (accountName);
					accountNameLength = bytes.Length;
					accountHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					accountNamePtr = accountHandle.AddrOfPinnedObject ();
				}
				
				if (!String.IsNullOrEmpty(path)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (path);
					pathLength = bytes.Length;
					pathHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					pathPtr = pathHandle.AddrOfPinnedObject ();
				}
				
				int passwordLength = 0;
				
				SecStatusCode code = SecKeychainFindInternetPassword(
					IntPtr.Zero,
					serverNameLength,
					serverNamePtr,
					securityDomainLength,
					securityDomainPtr,
					accountNameLength,
					accountNamePtr,
					pathLength,
					pathPtr,
					port,
					SecProtocolKeys.FromSecProtocol(protocolType),
					KeysAuthenticationType.FromSecAuthenticationType(authenticationType),
					out passwordLength,
					out passwordPtr,
					IntPtr.Zero);
				
				if (code == SecStatusCode.Success && passwordLength > 0) {
					password = new byte[passwordLength];
					Marshal.Copy(passwordPtr, password, 0, passwordLength);
				}
				
				return code;
				
			} finally {
				if (serverHandle.IsAllocated)
					serverHandle.Free();
				if (accountHandle.IsAllocated)
					accountHandle.Free();
				if (securityDomainHandle.IsAllocated)
					securityDomainHandle.Free();
				if (pathHandle.IsAllocated)
					pathHandle.Free();
				if (passwordPtr != IntPtr.Zero)
					SecKeychainItemFreeContent(IntPtr.Zero, passwordPtr);
			}
		}

		public static SecStatusCode AddGenericPassword (string serviceName, string accountName, byte[] password)
		{
			GCHandle serviceHandle = new GCHandle ();
			GCHandle accountHandle = new GCHandle ();
			GCHandle passwordHandle = new GCHandle ();
			
			int serviceNameLength = 0;
			IntPtr serviceNamePtr = IntPtr.Zero;
			int accountNameLength = 0;
			IntPtr accountNamePtr = IntPtr.Zero;
			int passwordLength = 0;
			IntPtr passwordPtr = IntPtr.Zero;
			
			try {
				if (!String.IsNullOrEmpty(serviceName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (serviceName);
					serviceNameLength = bytes.Length;
					serviceHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					serviceNamePtr = serviceHandle.AddrOfPinnedObject ();
				}
				
				if (!String.IsNullOrEmpty(accountName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (accountName);
					accountNameLength = bytes.Length;
					accountHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					accountNamePtr = accountHandle.AddrOfPinnedObject ();
				}

				if (password != null && password.Length > 0) {
					passwordLength = password.Length;
					passwordHandle = GCHandle.Alloc (password, GCHandleType.Pinned);
					passwordPtr = passwordHandle.AddrOfPinnedObject ();
				}

				return SecKeychainAddGenericPassword(
					IntPtr.Zero,
					serviceNameLength,
					serviceNamePtr,
					accountNameLength,
					accountNamePtr,
					passwordLength,
					passwordPtr,
					IntPtr.Zero
					);

			} finally {
				if (serviceHandle.IsAllocated)
					serviceHandle.Free();
				if (accountHandle.IsAllocated)
					accountHandle.Free();
				if (passwordHandle.IsAllocated)
					passwordHandle.Free();
			}
		}

		public static SecStatusCode FindGenericPassword(string serviceName, string accountName, out byte[] password)
		{
			password = null;

			GCHandle serviceHandle = new GCHandle ();
			GCHandle accountHandle = new GCHandle ();
			
			int serviceNameLength = 0;
			IntPtr serviceNamePtr = IntPtr.Zero;
			int accountNameLength = 0;
			IntPtr accountNamePtr = IntPtr.Zero;
			IntPtr passwordPtr = IntPtr.Zero;
			
			try {
				
				if (!String.IsNullOrEmpty(serviceName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (serviceName);
					serviceNameLength = bytes.Length;
					serviceHandle = GCHandle.Alloc (bytes, GCHandleType.Pinned);
					serviceNamePtr = serviceHandle.AddrOfPinnedObject();
				}
				
				if (!String.IsNullOrEmpty(accountName)) {
					var bytes = System.Text.Encoding.UTF8.GetBytes (accountName);
					accountNameLength = bytes.Length;
					accountHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
					accountNamePtr = accountHandle.AddrOfPinnedObject();
				}
				
				int passwordLength = 0;
				
				var code = SecKeychainFindGenericPassword(
					IntPtr.Zero,
					serviceNameLength,
					serviceNamePtr,
					accountNameLength,
					accountNamePtr,
					out passwordLength,
					out passwordPtr,
					IntPtr.Zero
					);
				
				if (code == SecStatusCode.Success && passwordLength > 0){
					password = new byte[passwordLength];
					Marshal.Copy(passwordPtr, password, 0, passwordLength);
				}
				
				return code;
				
			} finally {
				if (serviceHandle.IsAllocated)
					serviceHandle.Free();
				if (accountHandle.IsAllocated)
					accountHandle.Free();
				if (passwordPtr != IntPtr.Zero)
					SecKeychainItemFreeContent(IntPtr.Zero, passwordPtr);
			}
		}
#else
		public static object QueryAsConcreteType (SecRecord query, out SecStatusCode result)
		{
			if (query == null){
				result = SecStatusCode.Param;
				return null;
			}
			
			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)){
				copy.LowlevelSetObject (CFBoolean.True.Handle, SecItem.ReturnRef);
				SetLimit (copy, 1);
				
				IntPtr ptr;
				result = SecItem.SecItemCopyMatching (copy.Handle, out ptr);
				if ((result == SecStatusCode.Success) && (ptr != IntPtr.Zero)) {
					int cfType = CFType.GetTypeID (ptr);
					
					if (cfType == SecCertificate.GetTypeID ())
						return new SecCertificate (ptr, true);
					else if (cfType == SecKey.GetTypeID ())
						return new SecKey (ptr, true);
					else if (cfType == SecIdentity.GetTypeID ())
						return new SecIdentity (ptr, true);
					else
						throw new Exception (String.Format ("Unexpected type: 0x{0:x}", cfType));
				} 
				return null;
			}
		}
#endif
	}
	
	public class SecRecord : IDisposable {
		internal NSMutableDictionary queryDict;

		internal SecRecord (NSMutableDictionary dict)
		{
			queryDict = dict;
		}
		
		public SecRecord (SecKind secKind)
		{
			var kind = SecClass.FromSecKind (secKind);
			if (kind == SecClass.Identity)
				queryDict = new NSMutableDictionary ();
			else
				queryDict = NSMutableDictionary.LowlevelFromObjectAndKey (kind, SecClass.SecClassKey);
		}

		public SecRecord Clone ()
		{
			return new SecRecord (NSMutableDictionary.FromDictionary (queryDict));
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (queryDict != null){
				if (disposing){
					queryDict.Dispose ();
					queryDict = null;
				}
			}
		}

		~SecRecord ()
		{
			Dispose (false);
		}
			
		IntPtr Fetch (IntPtr key)
		{
			return queryDict.LowlevelObjectForKey (key);
		}

		NSObject FetchObject (IntPtr key)
		{
			return Runtime.GetNSObject (queryDict.LowlevelObjectForKey (key));
		}

		string FetchString (IntPtr key)
		{
			return (string) Runtime.GetNSObject<NSString> (queryDict.LowlevelObjectForKey (key));
		}

		NSNumber FetchNumber (IntPtr key)
		{
			return Runtime.GetNSObject<NSNumber> (queryDict.LowlevelObjectForKey (key));
		}

		NSData FetchData (IntPtr key)
		{
			return Runtime.GetNSObject<NSData> (queryDict.LowlevelObjectForKey (key));
		}
		

		void SetValue (NSObject val, IntPtr key)
		{
			queryDict.LowlevelSetObject (val, key);
		}

		void SetValue (IntPtr val, IntPtr key)
		{
			queryDict.LowlevelSetObject (val, key);
		}
		
		//
		// Attributes
		//
		public SecAccessible Accessible {
			get {
				return KeysAccessible.ToSecAccessible (Fetch (SecAttributeKey.AttrAccessible));
			}
			
			set {
				SetValue (KeysAccessible.FromSecAccessible (value), SecAttributeKey.AttrAccessible);
			}
		}

		public NSDate CreationDate {
			get {
				return (NSDate) FetchObject (SecAttributeKey.AttrCreationDate);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecAttributeKey.AttrCreationDate);
			}
		}

		public NSDate ModificationDate {
			get {
				return (NSDate) FetchObject (SecAttributeKey.AttrModificationDate);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecAttributeKey.AttrModificationDate);
			}
		}

		public string Description {
			get {
				return FetchString (SecAttributeKey.AttrDescription);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrDescription);
			}
		}

		public string Comment {
			get {
				return FetchString (SecAttributeKey.AttrComment);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrComment);
			}
		}

		public int Creator {
			get {
				return FetchNumber (SecAttributeKey.AttrCreator).Int32Value;
			}
					
			set {
				SetValue (new NSNumber (value), SecAttributeKey.AttrCreator);
			}
		}

		public int CreatorType {
			get {
				return FetchNumber (SecAttributeKey.AttrType).Int32Value;
			}
					
			set {
				SetValue (new NSNumber (value), SecAttributeKey.AttrType);
			}
		}

		public string Label {
			get {
				return FetchString (SecAttributeKey.AttrLabel);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrLabel);
			}
		}

		public bool Invisible {
			get {
				return Fetch (SecAttributeKey.AttrIsInvisible) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrIsInvisible);
			}
		}

		public bool IsNegative {
			get {
				return Fetch (SecAttributeKey.AttrIsNegative) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrIsNegative);
			}
		}

		public string Account {
			get {
				return FetchString (SecAttributeKey.AttrAccount);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrAccount);
			}
		}

		public string Service {
			get {
				return FetchString (SecAttributeKey.AttrService);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrService);
			}
		}

		public NSData Generic {
			get {
				return FetchData (SecAttributeKey.AttrGeneric);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecAttributeKey.AttrGeneric);
			}
		}

		public string SecurityDomain {
			get {
				return FetchString (SecAttributeKey.AttrSecurityDomain);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrSecurityDomain);
			}
		}

		public string Server {
			get {
				return FetchString (SecAttributeKey.AttrServer);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrServer);
			}
		}

		public SecProtocol Protocol {
			get {
				return SecProtocolKeys.ToSecProtocol (Fetch (SecAttributeKey.AttrProtocol));
			}
			
			set {
				SetValue (SecProtocolKeys.FromSecProtocol (value), SecAttributeKey.AttrProtocol);
			}
		}

		public SecAuthenticationType AuthenticationType {
			get {
				var at = Fetch (SecAttributeKey.AttrAuthenticationType);
				if (at == IntPtr.Zero)
					return SecAuthenticationType.Default;
				return KeysAuthenticationType.ToSecAuthenticationType (at);
			}
			
			set {
				SetValue (KeysAuthenticationType.FromSecAuthenticationType (value),
							     SecAttributeKey.AttrAuthenticationType);
			}
		}

		public int Port {
			get {
				return FetchNumber (SecAttributeKey.AttrPort).Int32Value;
			}
					
			set {
				SetValue (new NSNumber (value), SecAttributeKey.AttrPort);
			}
		}

		public string Path {
			get {
				return FetchString (SecAttributeKey.AttrPath);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrPath);
			}
		}

		// read only
		public string Subject {
			get {
				return FetchString (SecAttributeKey.AttrSubject);
			}
		}

		// read only
		public NSData Issuer {
			get {
				return FetchData (SecAttributeKey.AttrIssuer);
			}
		}

		// read only
		public NSData SerialNumber {
			get {
				return FetchData (SecAttributeKey.AttrSerialNumber);
			}
		}

		// read only
		public NSData SubjectKeyID {
			get {
				return FetchData (SecAttributeKey.AttrSubjectKeyID);
			}
		}

		// read only
		public NSData PublicKeyHash {
			get {
				return FetchData (SecAttributeKey.AttrPublicKeyHash);
			}
		}

		// read only
		public NSNumber CertificateType {
			get {
				return FetchNumber (SecAttributeKey.AttrCertificateType);
			}
		}

		// read only
		public NSNumber CertificateEncoding {
			get {
				return FetchNumber (SecAttributeKey.AttrCertificateEncoding);
			}
		}

		public SecKeyClass KeyClass {
			get {
				var k = Fetch (SecAttributeKey.AttrKeyClass);
				if (k == ClassKeys.Public)
					return SecKeyClass.Public;
				else if (k == ClassKeys.Private)
					return SecKeyClass.Private;
				else if (k == ClassKeys.Symmetric)
					return SecKeyClass.Symmetric;
				throw new Exception ("Unknown value");
			}
			set {
				SetValue (value == SecKeyClass.Public ? ClassKeys.Public : value == SecKeyClass.Private ? ClassKeys.Private : ClassKeys.Symmetric, SecAttributeKey.AttrKeyClass);
			}
		}

		public string ApplicationLabel {
			get {
				return FetchString (SecAttributeKey.AttrApplicationLabel);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrApplicationLabel);
			}
		}

		public bool IsPermanent {
			get {
				return Fetch (SecAttributeKey.AttrIsPermanent) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrIsPermanent);
			}
		}

		public NSData ApplicationTag {
			get {
				return FetchData (SecAttributeKey.AttrApplicationTag);
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecAttributeKey.AttrApplicationTag);
			}
		}

		public SecKeyType KeyType {
			get {
				var k = Fetch (SecAttributeKey.AttrKeyType);
				if (k == KeyTypeKeys.RSA)
					return SecKeyType.RSA;
				else
					return SecKeyType.EC;
			}
			
			set {
				SetValue (value == SecKeyType.RSA ? KeyTypeKeys.RSA : KeyTypeKeys.EC, SecAttributeKey.AttrKeyType);
			}
		}

		public int KeySizeInBits {
			get {
				return FetchNumber (SecAttributeKey.AttrKeySizeInBits).Int32Value;
			}
					
			set {
				SetValue (new NSNumber (value), SecAttributeKey.AttrKeySizeInBits);
			}
		}

		public int EffectiveKeySize {
			get {
				return FetchNumber (SecAttributeKey.AttrEffectiveKeySize).Int32Value;
			}
					
			set {
				SetValue (new NSNumber (value), SecAttributeKey.AttrEffectiveKeySize);
			}
		}

		public bool CanEncrypt {
			get {
				return Fetch (SecAttributeKey.AttrCanEncrypt) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanEncrypt);
			}
		}

		public bool CanDecrypt {
			get {
				return Fetch (SecAttributeKey.AttrCanDecrypt) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanDecrypt);
			}
		}

		public bool CanDerive {
			get {
				return Fetch (SecAttributeKey.AttrCanDerive) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanDerive);
			}
		}

		public bool CanSign {
			get {
				return Fetch (SecAttributeKey.AttrCanSign) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanSign);
			}
		}

		public bool CanVerify {
			get {
				return Fetch (SecAttributeKey.AttrCanVerify) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanVerify);
			}
		}

		public bool CanWrap {
			get {
				return Fetch (SecAttributeKey.AttrCanWrap) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanWrap);
			}
		}

		public bool CanUnwrap {
			get {
				return Fetch (SecAttributeKey.AttrCanUnwrap) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecAttributeKey.AttrCanUnwrap);
			}
		}

		public string AccessGroup {
			get {
				return FetchString (SecAttributeKey.AttrAccessGroup);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecAttributeKey.AttrAccessGroup);
			}
		}

		//
		// Matches
		//

		public SecPolicy MatchPolicy {
			get {
				return new SecPolicy (Fetch (SecItem.MatchPolicy));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value.Handle, SecItem.MatchPolicy);
			}
		}

		public NSArray MatchItemList {
			get {
				return Runtime.GetNSObject<NSArray> (Fetch (SecItem.MatchItemList));
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecItem.MatchItemList);
			}
		}

		public NSData [] MatchIssuers {
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				
				SetValue (NSArray.FromNSObjects (value), SecItem.MatchIssuers);
			}
		}

		public string MatchEmailAddressIfPresent {
			get {
				return FetchString (SecItem.MatchEmailAddressIfPresent);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecItem.MatchEmailAddressIfPresent);
			}
		}

		public string MatchSubjectContains {
			get {
				return FetchString (SecItem.MatchSubjectContains);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (new NSString (value), SecItem.MatchSubjectContains);
			}
		}

		public bool MatchCaseInsensitive {
			get {
				return Fetch (SecItem.MatchCaseInsensitive) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecItem.MatchCaseInsensitive);
			}
		}

		public bool MatchTrustedOnly {
			get {
				return Fetch (SecItem.MatchTrustedOnly) == CFBoolean.True.Handle;
			}
			
			set {
				SetValue (CFBoolean.FromBoolean (value).Handle, SecItem.MatchTrustedOnly);
			}
		}

		public NSDate MatchValidOnDate {
			get {
				return (Runtime.GetNSObject<NSDate> (Fetch (SecItem.MatchValidOnDate)));
			}
			
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecItem.MatchValidOnDate);
			}
		}

		public NSData ValueData {
			get {
				return FetchData (SecItem.ValueData);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecItem.ValueData);
			}
		}
		
		public NSObject ValueRef {
			get {
				return FetchObject (SecItem.ValueRef);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				SetValue (value, SecItem.ValueRef);
			}
		}
	}
	
	internal class SecItem {
		internal static IntPtr securityLibrary = Dlfcn.dlopen (Constants.SecurityLibrary, 0);

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemCopyMatching (IntPtr cfDictRef, out IntPtr result);

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemAdd (IntPtr cfDictRef, IntPtr result);

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemDelete (IntPtr cfDictRef);

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemUpdate (IntPtr cfDictRef, IntPtr attrsToUpdate);

		static IntPtr _MatchPolicy;
		public static IntPtr MatchPolicy {
			get {
				if (_MatchPolicy == IntPtr.Zero)
					_MatchPolicy = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchPolicy");
				return _MatchPolicy;
			}
		}
		
		static IntPtr _MatchItemList;
		public static IntPtr MatchItemList {
			get {
				if (_MatchItemList == IntPtr.Zero)
					_MatchItemList = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchItemList");
				return _MatchItemList;
			}
		}
		
		static IntPtr _MatchSearchList;
		public static IntPtr MatchSearchList {
			get {
				if (_MatchSearchList == IntPtr.Zero)
					_MatchSearchList = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchSearchList");
				return _MatchSearchList;
			}
		}
		
		static IntPtr _MatchIssuers;
		public static IntPtr MatchIssuers {
			get {
				if (_MatchIssuers == IntPtr.Zero)
					_MatchIssuers = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchIssuers");
				return _MatchIssuers;
			}
		}
		
		static IntPtr _MatchEmailAddressIfPresent;
		public static IntPtr MatchEmailAddressIfPresent {
			get {
				if (_MatchEmailAddressIfPresent == IntPtr.Zero)
					_MatchEmailAddressIfPresent = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchEmailAddressIfPresent");
				return _MatchEmailAddressIfPresent;
			}
		}
		
		static IntPtr _MatchSubjectContains;
		public static IntPtr MatchSubjectContains {
			get {
				if (_MatchSubjectContains == IntPtr.Zero)
					_MatchSubjectContains = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchSubjectContains");
				return _MatchSubjectContains;
			}
		}
		
		static IntPtr _MatchCaseInsensitive;
		public static IntPtr MatchCaseInsensitive {
			get {
				if (_MatchCaseInsensitive == IntPtr.Zero)
					_MatchCaseInsensitive = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchCaseInsensitive");
				return _MatchCaseInsensitive;
			}
		}
		
		static IntPtr _MatchTrustedOnly;
		public static IntPtr MatchTrustedOnly {
			get {
				if (_MatchTrustedOnly == IntPtr.Zero)
					_MatchTrustedOnly = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchTrustedOnly");
				return _MatchTrustedOnly;
			}
		}
		
		static IntPtr _MatchValidOnDate;
		public static IntPtr MatchValidOnDate {
			get {
				if (_MatchValidOnDate == IntPtr.Zero)
					_MatchValidOnDate = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchValidOnDate");
				return _MatchValidOnDate;
			}
		}
		
		static IntPtr _MatchLimit;
		public static IntPtr MatchLimit {
			get {
				if (_MatchLimit == IntPtr.Zero)
					_MatchLimit = Dlfcn.GetIntPtr (securityLibrary, "kSecMatchLimit");
				return _MatchLimit;
			}
		}

		static IntPtr _ReturnData;
		public static IntPtr ReturnData {
			get {
				if (_ReturnData == IntPtr.Zero)
					_ReturnData = Dlfcn.GetIntPtr (securityLibrary, "kSecReturnData");
				return _ReturnData;
			}
		}
		
		static IntPtr _ReturnAttributes;
		public static IntPtr ReturnAttributes {
			get {
				if (_ReturnAttributes == IntPtr.Zero)
					_ReturnAttributes = Dlfcn.GetIntPtr (securityLibrary, "kSecReturnAttributes");
				return _ReturnAttributes;
			}
		}
		
		static IntPtr _ReturnRef;
		public static IntPtr ReturnRef {
			get {
				if (_ReturnRef == IntPtr.Zero)
					_ReturnRef = Dlfcn.GetIntPtr (securityLibrary, "kSecReturnRef");
				return _ReturnRef;
			}
		}
		
		static IntPtr _ReturnPersistentRef;
		public static IntPtr ReturnPersistentRef {
			get {
				if (_ReturnPersistentRef == IntPtr.Zero)
					_ReturnPersistentRef = Dlfcn.GetIntPtr (securityLibrary, "kSecReturnPersistentRef");
				return _ReturnPersistentRef;
			}
		}
		
		static IntPtr _ValueData;
		public static IntPtr ValueData {
			get {
				if (_ValueData == IntPtr.Zero)
					_ValueData = Dlfcn.GetIntPtr (securityLibrary, "kSecValueData");
				return _ValueData;
			}
		}
		
		static IntPtr _ValueRef;
		public static IntPtr ValueRef {
			get {
				if (_ValueRef == IntPtr.Zero)
					_ValueRef = Dlfcn.GetIntPtr (securityLibrary, "kSecValueRef");
				return _ValueRef;
			}
		}
		
		static IntPtr _ValuePersistentRef;
		public static IntPtr ValuePersistentRef {
			get {
				if (_ValuePersistentRef == IntPtr.Zero)
					_ValuePersistentRef = Dlfcn.GetIntPtr (securityLibrary, "kSecValuePersistentRef");
				return _ValuePersistentRef;
			}
		}
		
		static IntPtr _UseItemList;
		public static IntPtr UseItemList {
			get {
				if (_UseItemList == IntPtr.Zero)
					_UseItemList = Dlfcn.GetIntPtr (securityLibrary, "kSecUseItemList");
				return _UseItemList;
			}
		}
	}

	internal static class SecClass {
		public static IntPtr SecClassKey;
		public static IntPtr GenericPassword;
		public static IntPtr InternetPassword;
		public static IntPtr Certificate;
		public static IntPtr Key;
		public static IntPtr Identity;

		static SecClass ()
		{
			SecClassKey = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClass");
			GenericPassword = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassGenericPassword");
			InternetPassword = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassInternetPassword");
			Certificate = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassCertificate");
			Key = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassKey");
			Identity = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecClassIdentity");
		}
	
		public static IntPtr FromSecKind (SecKind secKind)
		{
			switch (secKind){
			case SecKind.InternetPassword:
				return InternetPassword;
#if !MONOMAC
			case SecKind.GenericPassword:
				return GenericPassword;
			case SecKind.Certificate:
				return Certificate;
			case SecKind.Key:
				return Key;
			case SecKind.Identity:
				return Identity;
#endif
			default:
				throw new ArgumentException ("secKind");
			}
		}
	}
	
	internal static class SecAttributeKey {
		static IntPtr _AttrAccessible;
		public static IntPtr AttrAccessible {
			get {
				if (_AttrAccessible == IntPtr.Zero)
					_AttrAccessible = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessible");
				return _AttrAccessible;
			}
		}
		
		static IntPtr _AttrAccessGroup;
		public static IntPtr AttrAccessGroup {
			get {
				if (_AttrAccessGroup == IntPtr.Zero)
					_AttrAccessGroup = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessGroup");
				return _AttrAccessGroup;
			}
		}
		
		static IntPtr _AttrCreationDate;
		public static IntPtr AttrCreationDate {
			get {
				if (_AttrCreationDate == IntPtr.Zero)
					_AttrCreationDate = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCreationDate");
				return _AttrCreationDate;
			}
		}
		
		static IntPtr _AttrModificationDate;
		public static IntPtr AttrModificationDate {
			get {
				if (_AttrModificationDate == IntPtr.Zero)
					_AttrModificationDate = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrModificationDate");
				return _AttrModificationDate;
			}
		}
		
		static IntPtr _AttrDescription;
		public static IntPtr AttrDescription {
			get {
				if (_AttrDescription == IntPtr.Zero)
					_AttrDescription = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrDescription");
				return _AttrDescription;
			}
		}
		
		static IntPtr _AttrComment;
		public static IntPtr AttrComment {
			get {
				if (_AttrComment == IntPtr.Zero)
					_AttrComment = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrComment");
				return _AttrComment;
			}
		}
		
		static IntPtr _AttrCreator;
		public static IntPtr AttrCreator {
			get {
				if (_AttrCreator == IntPtr.Zero)
					_AttrCreator = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCreator");
				return _AttrCreator;
			}
		}
		
		static IntPtr _AttrType;
		public static IntPtr AttrType {
			get {
				if (_AttrType == IntPtr.Zero)
					_AttrType = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrType");
				return _AttrType;
			}
		}
		
		static IntPtr _AttrLabel;
		public static IntPtr AttrLabel {
			get {
				if (_AttrLabel == IntPtr.Zero)
					_AttrLabel = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrLabel");
				return _AttrLabel;
			}
		}
		
		static IntPtr _AttrIsInvisible;
		public static IntPtr AttrIsInvisible {
			get {
				if (_AttrIsInvisible == IntPtr.Zero)
					_AttrIsInvisible = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIsInvisible");
				return _AttrIsInvisible;
			}
		}
		
		static IntPtr _AttrIsNegative;
		public static IntPtr AttrIsNegative {
			get {
				if (_AttrIsNegative == IntPtr.Zero)
					_AttrIsNegative = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIsNegative");
				return _AttrIsNegative;
			}
		}
		
		static IntPtr _AttrAccount;
		public static IntPtr AttrAccount {
			get {
				if (_AttrAccount == IntPtr.Zero)
					_AttrAccount = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccount");
				return _AttrAccount;
			}
		}
		
		static IntPtr _AttrService;
		public static IntPtr AttrService {
			get {
				if (_AttrService == IntPtr.Zero)
					_AttrService = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrService");
				return _AttrService;
			}
		}
		
		static IntPtr _AttrGeneric;
		public static IntPtr AttrGeneric {
			get {
				if (_AttrGeneric == IntPtr.Zero)
					_AttrGeneric = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrGeneric");
				return _AttrGeneric;
			}
		}
		
		static IntPtr _AttrSecurityDomain;
		public static IntPtr AttrSecurityDomain {
			get {
				if (_AttrSecurityDomain == IntPtr.Zero)
					_AttrSecurityDomain = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSecurityDomain");
				return _AttrSecurityDomain;
			}
		}
		
		static IntPtr _AttrServer;
		public static IntPtr AttrServer {
			get {
				if (_AttrServer == IntPtr.Zero)
					_AttrServer = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrServer");
				return _AttrServer;
			}
		}
		
		static IntPtr _AttrProtocol;
		public static IntPtr AttrProtocol {
			get {
				if (_AttrProtocol == IntPtr.Zero)
					_AttrProtocol = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocol");
				return _AttrProtocol;
			}
		}
		
		static IntPtr _AttrAuthenticationType;
		public static IntPtr AttrAuthenticationType {
			get {
				if (_AttrAuthenticationType == IntPtr.Zero)
					_AttrAuthenticationType = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationType");
				return _AttrAuthenticationType;
			}
		}
		
		static IntPtr _AttrPort;
		public static IntPtr AttrPort {
			get {
				if (_AttrPort == IntPtr.Zero)
					_AttrPort = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrPort");
				return _AttrPort;
			}
		}
		
		static IntPtr _AttrPath;
		public static IntPtr AttrPath {
			get {
				if (_AttrPath == IntPtr.Zero)
					_AttrPath = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrPath");
				return _AttrPath;
			}
		}
		
		static IntPtr _AttrSubject;
		public static IntPtr AttrSubject {
			get {
				if (_AttrSubject == IntPtr.Zero)
					_AttrSubject = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSubject");
				return _AttrSubject;
			}
		}
		
		static IntPtr _AttrIssuer;
		public static IntPtr AttrIssuer {
			get {
				if (_AttrIssuer == IntPtr.Zero)
					_AttrIssuer = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIssuer");
				return _AttrIssuer;
			}
		}
		
		static IntPtr _AttrSerialNumber;
		public static IntPtr AttrSerialNumber {
			get {
				if (_AttrSerialNumber == IntPtr.Zero)
					_AttrSerialNumber = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSerialNumber");
				return _AttrSerialNumber;
			}
		}
		
		static IntPtr _AttrSubjectKeyID;
		public static IntPtr AttrSubjectKeyID {
			get {
				if (_AttrSubjectKeyID == IntPtr.Zero)
					_AttrSubjectKeyID = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrSubjectKeyID");
				return _AttrSubjectKeyID;
			}
		}
		
		static IntPtr _AttrPublicKeyHash;
		public static IntPtr AttrPublicKeyHash {
			get {
				if (_AttrPublicKeyHash == IntPtr.Zero)
					_AttrPublicKeyHash = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrPublicKeyHash");
				return _AttrPublicKeyHash;
			}
		}
		
		static IntPtr _AttrCertificateType;
		public static IntPtr AttrCertificateType {
			get {
				if (_AttrCertificateType == IntPtr.Zero)
					_AttrCertificateType = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCertificateType");
				return _AttrCertificateType;
			}
		}
		
		static IntPtr _AttrCertificateEncoding;
		public static IntPtr AttrCertificateEncoding {
			get {
				if (_AttrCertificateEncoding == IntPtr.Zero)
					_AttrCertificateEncoding = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCertificateEncoding");
				return _AttrCertificateEncoding;
			}
		}
		
		static IntPtr _AttrKeyClass;
		public static IntPtr AttrKeyClass {
			get {
				if (_AttrKeyClass == IntPtr.Zero)
					_AttrKeyClass = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClass");
				return _AttrKeyClass;
			}
		}
		
		static IntPtr _AttrApplicationLabel;
		public static IntPtr AttrApplicationLabel {
			get {
				if (_AttrApplicationLabel == IntPtr.Zero)
					_AttrApplicationLabel = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrApplicationLabel");
				return _AttrApplicationLabel;
			}
		}
		
		static IntPtr _AttrIsPermanent;
		public static IntPtr AttrIsPermanent {
			get {
				if (_AttrIsPermanent == IntPtr.Zero)
					_AttrIsPermanent = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrIsPermanent");
				return _AttrIsPermanent;
			}
		}
		
		static IntPtr _AttrApplicationTag;
		public static IntPtr AttrApplicationTag {
			get {
				if (_AttrApplicationTag == IntPtr.Zero)
					_AttrApplicationTag = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrApplicationTag");
				return _AttrApplicationTag;
			}
		}
		
		static IntPtr _AttrKeyType;
		public static IntPtr AttrKeyType {
			get {
				if (_AttrKeyType == IntPtr.Zero)
					_AttrKeyType = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyType");
				return _AttrKeyType;
			}
		}
		
		static IntPtr _AttrKeySizeInBits;
		public static IntPtr AttrKeySizeInBits {
			get {
				if (_AttrKeySizeInBits == IntPtr.Zero)
					_AttrKeySizeInBits = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeySizeInBits");
				return _AttrKeySizeInBits;
			}
		}
		
		static IntPtr _AttrEffectiveKeySize;
		public static IntPtr AttrEffectiveKeySize {
			get {
				if (_AttrEffectiveKeySize == IntPtr.Zero)
					_AttrEffectiveKeySize = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrEffectiveKeySize");
				return _AttrEffectiveKeySize;
			}
		}
		
		static IntPtr _AttrCanEncrypt;
		public static IntPtr AttrCanEncrypt {
			get {
				if (_AttrCanEncrypt == IntPtr.Zero)
					_AttrCanEncrypt = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanEncrypt");
				return _AttrCanEncrypt;
			}
		}
		
		static IntPtr _AttrCanDecrypt;
		public static IntPtr AttrCanDecrypt {
			get {
				if (_AttrCanDecrypt == IntPtr.Zero)
					_AttrCanDecrypt = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanDecrypt");
				return _AttrCanDecrypt;
			}
		}
		
		static IntPtr _AttrCanDerive;
		public static IntPtr AttrCanDerive {
			get {
				if (_AttrCanDerive == IntPtr.Zero)
					_AttrCanDerive = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanDerive");
				return _AttrCanDerive;
			}
		}
		
		static IntPtr _AttrCanSign;
		public static IntPtr AttrCanSign {
			get {
				if (_AttrCanSign == IntPtr.Zero)
					_AttrCanSign = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanSign");
				return _AttrCanSign;
			}
		}
		
		static IntPtr _AttrCanVerify;
		public static IntPtr AttrCanVerify {
			get {
				if (_AttrCanVerify == IntPtr.Zero)
					_AttrCanVerify = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanVerify");
				return _AttrCanVerify;
			}
		}
		
		static IntPtr _AttrCanWrap;
		public static IntPtr AttrCanWrap {
			get {
				if (_AttrCanWrap == IntPtr.Zero)
					_AttrCanWrap = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanWrap");
				return _AttrCanWrap;
			}
		}
		
		static IntPtr _AttrCanUnwrap;
		public static IntPtr AttrCanUnwrap {
			get {
				if (_AttrCanUnwrap == IntPtr.Zero)
					_AttrCanUnwrap = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrCanUnwrap");
				return _AttrCanUnwrap;
			}
		}
	}
	
	internal static class KeysAccessible {
		public static IntPtr FromSecAccessible (SecAccessible accessible)
		{
			switch (accessible){
			case SecAccessible.WhenUnlocked:
				return WhenUnlocked;
			case SecAccessible.AfterFirstUnlock:
				return AfterFirstUnlock;
			case SecAccessible.Always:
				return Always;
			case SecAccessible.WhenUnlockedThisDeviceOnly:
				return WhenUnlockedThisDeviceOnly;
			case SecAccessible.AfterFirstUnlockThisDeviceOnly:
				return AfterFirstUnlockThisDeviceOnly;
			case SecAccessible.AlwaysThisDeviceOnly:
				return AlwaysThisDeviceOnly;
			default:
				throw new ArgumentException ("accessible");
			}
		}
			
		static IntPtr _WhenUnlocked;
		public static IntPtr WhenUnlocked {
			get {
				if (_WhenUnlocked == IntPtr.Zero)
					_WhenUnlocked = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleWhenUnlocked");
				return _WhenUnlocked;
			}
		}
		
		static IntPtr _AfterFirstUnlock;
		public static IntPtr AfterFirstUnlock {
			get {
				if (_AfterFirstUnlock == IntPtr.Zero)
					_AfterFirstUnlock = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAfterFirstUnlock");
				return _AfterFirstUnlock;
			}
		}
		
		static IntPtr _Always;
		public static IntPtr Always {
			get {
				if (_Always == IntPtr.Zero)
					_Always = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAlways");
				return _Always;
			}
		}
		
		static IntPtr _WhenUnlockedThisDeviceOnly;
		public static IntPtr WhenUnlockedThisDeviceOnly {
			get {
				if (_WhenUnlockedThisDeviceOnly == IntPtr.Zero)
					_WhenUnlockedThisDeviceOnly = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleWhenUnlockedThisDeviceOnly");
				return _WhenUnlockedThisDeviceOnly;
			}
		}
		
		static IntPtr _AfterFirstUnlockThisDeviceOnly;
		public static IntPtr AfterFirstUnlockThisDeviceOnly {
			get {
				if (_AfterFirstUnlockThisDeviceOnly == IntPtr.Zero)
					_AfterFirstUnlockThisDeviceOnly = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly");
				return _AfterFirstUnlockThisDeviceOnly;
			}
		}
		
		static IntPtr _AlwaysThisDeviceOnly;
		public static IntPtr AlwaysThisDeviceOnly {
			get {
				if (_AlwaysThisDeviceOnly == IntPtr.Zero)
					_AlwaysThisDeviceOnly = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAccessibleAlwaysThisDeviceOnly");
				return _AlwaysThisDeviceOnly;
			}
		}
	
		public static SecAccessible ToSecAccessible (IntPtr handle)
		{
			if (handle == WhenUnlocked)
				return SecAccessible.WhenUnlocked;
			if (handle == AfterFirstUnlock)
				return SecAccessible.AfterFirstUnlock;
			if (handle == Always)
				return SecAccessible.Always;
			if (handle == WhenUnlockedThisDeviceOnly)
				return SecAccessible.WhenUnlockedThisDeviceOnly;
			if (handle == AfterFirstUnlockThisDeviceOnly)
				return SecAccessible.AfterFirstUnlockThisDeviceOnly;
			if (handle == AlwaysThisDeviceOnly)
				return SecAccessible.AlwaysThisDeviceOnly;
			throw new ArgumentException ("obj");
		}
	}
	
	internal static class SecProtocolKeys {
		public static IntPtr FromSecProtocol (SecProtocol protocol)
		{
			switch (protocol){
			case SecProtocol.Ftp: return AttrProtocolFTP;
			case SecProtocol.FtpAccount: return AttrProtocolFTPAccount;
			case SecProtocol.Http: return AttrProtocolHTTP;
			case SecProtocol.Irc: return AttrProtocolIRC;
			case SecProtocol.Nntp: return AttrProtocolNNTP;
			case SecProtocol.Pop3: return AttrProtocolPOP3;
			case SecProtocol.Smtp: return AttrProtocolSMTP;
			case SecProtocol.Socks:return AttrProtocolSOCKS;
			case SecProtocol.Imap:return AttrProtocolIMAP;
			case SecProtocol.Ldap:return AttrProtocolLDAP;
			case SecProtocol.AppleTalk:return AttrProtocolAppleTalk;
			case SecProtocol.Afp:return AttrProtocolAFP;
			case SecProtocol.Telnet:return AttrProtocolTelnet;
			case SecProtocol.Ssh:return AttrProtocolSSH;
			case SecProtocol.Ftps:return AttrProtocolFTPS;
			case SecProtocol.Https:return AttrProtocolHTTPS;
			case SecProtocol.HttpProxy:return AttrProtocolHTTPProxy;
			case SecProtocol.HttpsProxy:return AttrProtocolHTTPSProxy;
			case SecProtocol.FtpProxy:return AttrProtocolFTPProxy;
			case SecProtocol.Smb:return AttrProtocolSMB;
			case SecProtocol.Rtsp:return AttrProtocolRTSP;
			case SecProtocol.RtspProxy:return AttrProtocolRTSPProxy;
			case SecProtocol.Daap:return AttrProtocolDAAP;
			case SecProtocol.Eppc:return AttrProtocolEPPC;
			case SecProtocol.Ipp:return AttrProtocolIPP;
			case SecProtocol.Nntps:return AttrProtocolNNTPS;
			case SecProtocol.Ldaps:return AttrProtocolLDAPS;
			case SecProtocol.Telnets:return AttrProtocolTelnetS;
			case SecProtocol.Imaps:return AttrProtocolIMAPS;
			case SecProtocol.Ircs:return AttrProtocolIRCS;
			case SecProtocol.Pop3s: return AttrProtocolPOP3S;
			}
			throw new ArgumentException ("protocol");
		}

		public static SecProtocol ToSecProtocol (IntPtr handle)
		{
			if (handle == AttrProtocolFTP)
				return SecProtocol.Ftp;
			if (handle == AttrProtocolFTPAccount)
				return SecProtocol.FtpAccount;
			if (handle == AttrProtocolHTTP)
				return SecProtocol.Http;
			if (handle == AttrProtocolIRC)
				return SecProtocol.Irc;
			if (handle == AttrProtocolNNTP)
				return SecProtocol.Nntp;
			if (handle == AttrProtocolPOP3)
				return SecProtocol.Pop3;
			if (handle == AttrProtocolSMTP)
				return SecProtocol.Smtp;
			if (handle == AttrProtocolSOCKS)
				return SecProtocol.Socks;
			if (handle == AttrProtocolIMAP)
				return SecProtocol.Imap;
			if (handle == AttrProtocolLDAP)
				return SecProtocol.Ldap;
			if (handle == AttrProtocolAppleTalk)
				return SecProtocol.AppleTalk;
			if (handle == AttrProtocolAFP)
				return SecProtocol.Afp;
			if (handle == AttrProtocolTelnet)
				return SecProtocol.Telnet;
			if (handle == AttrProtocolSSH)
				return SecProtocol.Ssh;
			if (handle == AttrProtocolFTPS)
				return SecProtocol.Ftps;
			if (handle == AttrProtocolHTTPS)
				return SecProtocol.Https;
			if (handle == AttrProtocolHTTPProxy)
				return SecProtocol.HttpProxy;
			if (handle == AttrProtocolHTTPSProxy)
				return SecProtocol.HttpsProxy;
			if (handle == AttrProtocolFTPProxy)
				return SecProtocol.FtpProxy;
			if (handle == AttrProtocolSMB)
				return SecProtocol.Smb;
			if (handle == AttrProtocolRTSP)
				return SecProtocol.Rtsp;
			if (handle == AttrProtocolRTSPProxy)
				return SecProtocol.RtspProxy;
			if (handle == AttrProtocolDAAP)
				return SecProtocol.Daap;
			if (handle == AttrProtocolEPPC)
				return SecProtocol.Eppc;
			if (handle == AttrProtocolIPP)
				return SecProtocol.Ipp;
			if (handle == AttrProtocolNNTPS)
				return SecProtocol.Nntps;
			if (handle == AttrProtocolLDAPS)
				return SecProtocol.Ldaps;
			if (handle == AttrProtocolTelnetS)
				return SecProtocol.Telnets;
			if (handle == AttrProtocolIMAPS)
				return SecProtocol.Imaps;
			if (handle == AttrProtocolIRCS)
				return SecProtocol.Ircs;
			if (handle == AttrProtocolPOP3S)
				return SecProtocol.Pop3s;
			throw new ArgumentException ("handle");
		}
		
		static IntPtr _AttrProtocolFTP;
		public static IntPtr AttrProtocolFTP {
			get {
				if (_AttrProtocolFTP == IntPtr.Zero)
					_AttrProtocolFTP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTP");
				return _AttrProtocolFTP;
			}
		}
		
		static IntPtr _AttrProtocolFTPAccount;
		public static IntPtr AttrProtocolFTPAccount {
			get {
				if (_AttrProtocolFTPAccount == IntPtr.Zero)
					_AttrProtocolFTPAccount = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTPAccount");
				return _AttrProtocolFTPAccount;
			}
		}
		
		static IntPtr _AttrProtocolHTTP;
		public static IntPtr AttrProtocolHTTP {
			get {
				if (_AttrProtocolHTTP == IntPtr.Zero)
					_AttrProtocolHTTP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTP");
				return _AttrProtocolHTTP;
			}
		}
		
		static IntPtr _AttrProtocolIRC;
		public static IntPtr AttrProtocolIRC {
			get {
				if (_AttrProtocolIRC == IntPtr.Zero)
					_AttrProtocolIRC = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIRC");
				return _AttrProtocolIRC;
			}
		}
		
		static IntPtr _AttrProtocolNNTP;
		public static IntPtr AttrProtocolNNTP {
			get {
				if (_AttrProtocolNNTP == IntPtr.Zero)
					_AttrProtocolNNTP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolNNTP");
				return _AttrProtocolNNTP;
			}
		}
		
		static IntPtr _AttrProtocolPOP3;
		public static IntPtr AttrProtocolPOP3 {
			get {
				if (_AttrProtocolPOP3 == IntPtr.Zero)
					_AttrProtocolPOP3 = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolPOP3");
				return _AttrProtocolPOP3;
			}
		}
		
		static IntPtr _AttrProtocolSMTP;
		public static IntPtr AttrProtocolSMTP {
			get {
				if (_AttrProtocolSMTP == IntPtr.Zero)
					_AttrProtocolSMTP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSMTP");
				return _AttrProtocolSMTP;
			}
		}
		
		static IntPtr _AttrProtocolSOCKS;
		public static IntPtr AttrProtocolSOCKS {
			get {
				if (_AttrProtocolSOCKS == IntPtr.Zero)
					_AttrProtocolSOCKS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSOCKS");
				return _AttrProtocolSOCKS;
			}
		}
		
		static IntPtr _AttrProtocolIMAP;
		public static IntPtr AttrProtocolIMAP {
			get {
				if (_AttrProtocolIMAP == IntPtr.Zero)
					_AttrProtocolIMAP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIMAP");
				return _AttrProtocolIMAP;
			}
		}
		
		static IntPtr _AttrProtocolLDAP;
		public static IntPtr AttrProtocolLDAP {
			get {
				if (_AttrProtocolLDAP == IntPtr.Zero)
					_AttrProtocolLDAP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolLDAP");
				return _AttrProtocolLDAP;
			}
		}
		
		static IntPtr _AttrProtocolAppleTalk;
		public static IntPtr AttrProtocolAppleTalk {
			get {
				if (_AttrProtocolAppleTalk == IntPtr.Zero)
					_AttrProtocolAppleTalk = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolAppleTalk");
				return _AttrProtocolAppleTalk;
			}
		}
		
		static IntPtr _AttrProtocolAFP;
		public static IntPtr AttrProtocolAFP {
			get {
				if (_AttrProtocolAFP == IntPtr.Zero)
					_AttrProtocolAFP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolAFP");
				return _AttrProtocolAFP;
			}
		}
		
		static IntPtr _AttrProtocolTelnet;
		public static IntPtr AttrProtocolTelnet {
			get {
				if (_AttrProtocolTelnet == IntPtr.Zero)
					_AttrProtocolTelnet = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolTelnet");
				return _AttrProtocolTelnet;
			}
		}
		
		static IntPtr _AttrProtocolSSH;
		public static IntPtr AttrProtocolSSH {
			get {
				if (_AttrProtocolSSH == IntPtr.Zero)
					_AttrProtocolSSH = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSSH");
				return _AttrProtocolSSH;
			}
		}
		
		static IntPtr _AttrProtocolFTPS;
		public static IntPtr AttrProtocolFTPS {
			get {
				if (_AttrProtocolFTPS == IntPtr.Zero)
					_AttrProtocolFTPS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTPS");
				return _AttrProtocolFTPS;
			}
		}
		
		static IntPtr _AttrProtocolHTTPS;
		public static IntPtr AttrProtocolHTTPS {
			get {
				if (_AttrProtocolHTTPS == IntPtr.Zero)
					_AttrProtocolHTTPS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTPS");
				return _AttrProtocolHTTPS;
			}
		}
		
		static IntPtr _AttrProtocolHTTPProxy;
		public static IntPtr AttrProtocolHTTPProxy {
			get {
				if (_AttrProtocolHTTPProxy == IntPtr.Zero)
					_AttrProtocolHTTPProxy = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTPProxy");
				return _AttrProtocolHTTPProxy;
			}
		}
		
		static IntPtr _AttrProtocolHTTPSProxy;
		public static IntPtr AttrProtocolHTTPSProxy {
			get {
				if (_AttrProtocolHTTPSProxy == IntPtr.Zero)
					_AttrProtocolHTTPSProxy = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolHTTPSProxy");
				return _AttrProtocolHTTPSProxy;
			}
		}
		
		static IntPtr _AttrProtocolFTPProxy;
		public static IntPtr AttrProtocolFTPProxy {
			get {
				if (_AttrProtocolFTPProxy == IntPtr.Zero)
					_AttrProtocolFTPProxy = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolFTPProxy");
				return _AttrProtocolFTPProxy;
			}
		}
		
		static IntPtr _AttrProtocolSMB;
		public static IntPtr AttrProtocolSMB {
			get {
				if (_AttrProtocolSMB == IntPtr.Zero)
					_AttrProtocolSMB = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolSMB");
				return _AttrProtocolSMB;
			}
		}
		
		static IntPtr _AttrProtocolRTSP;
		public static IntPtr AttrProtocolRTSP {
			get {
				if (_AttrProtocolRTSP == IntPtr.Zero)
					_AttrProtocolRTSP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolRTSP");
				return _AttrProtocolRTSP;
			}
		}
		
		static IntPtr _AttrProtocolRTSPProxy;
		public static IntPtr AttrProtocolRTSPProxy {
			get {
				if (_AttrProtocolRTSPProxy == IntPtr.Zero)
					_AttrProtocolRTSPProxy = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolRTSPProxy");
				return _AttrProtocolRTSPProxy;
			}
		}
		
		static IntPtr _AttrProtocolDAAP;
		public static IntPtr AttrProtocolDAAP {
			get {
				if (_AttrProtocolDAAP == IntPtr.Zero)
					_AttrProtocolDAAP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolDAAP");
				return _AttrProtocolDAAP;
			}
		}
		
		static IntPtr _AttrProtocolEPPC;
		public static IntPtr AttrProtocolEPPC {
			get {
				if (_AttrProtocolEPPC == IntPtr.Zero)
					_AttrProtocolEPPC = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolEPPC");
				return _AttrProtocolEPPC;
			}
		}
		
		static IntPtr _AttrProtocolIPP;
		public static IntPtr AttrProtocolIPP {
			get {
				if (_AttrProtocolIPP == IntPtr.Zero)
					_AttrProtocolIPP = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIPP");
				return _AttrProtocolIPP;
			}
		}
		
		static IntPtr _AttrProtocolNNTPS;
		public static IntPtr AttrProtocolNNTPS {
			get {
				if (_AttrProtocolNNTPS == IntPtr.Zero)
					_AttrProtocolNNTPS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolNNTPS");
				return _AttrProtocolNNTPS;
			}
		}
		
		static IntPtr _AttrProtocolLDAPS;
		public static IntPtr AttrProtocolLDAPS {
			get {
				if (_AttrProtocolLDAPS == IntPtr.Zero)
					_AttrProtocolLDAPS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolLDAPS");
				return _AttrProtocolLDAPS;
			}
		}
		
		static IntPtr _AttrProtocolTelnetS;
		public static IntPtr AttrProtocolTelnetS {
			get {
				if (_AttrProtocolTelnetS == IntPtr.Zero)
					_AttrProtocolTelnetS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolTelnetS");
				return _AttrProtocolTelnetS;
			}
		}
		
		static IntPtr _AttrProtocolIMAPS;
		public static IntPtr AttrProtocolIMAPS {
			get {
				if (_AttrProtocolIMAPS == IntPtr.Zero)
					_AttrProtocolIMAPS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIMAPS");
				return _AttrProtocolIMAPS;
			}
		}
		
		static IntPtr _AttrProtocolIRCS;
		public static IntPtr AttrProtocolIRCS {
			get {
				if (_AttrProtocolIRCS == IntPtr.Zero)
					_AttrProtocolIRCS = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolIRCS");
				return _AttrProtocolIRCS;
			}
		}
		
		static IntPtr _AttrProtocolPOP3S;
		public static IntPtr AttrProtocolPOP3S {
			get {
				if (_AttrProtocolPOP3S == IntPtr.Zero)
					_AttrProtocolPOP3S = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrProtocolPOP3S");
				return _AttrProtocolPOP3S;
			}
		}
	}
	
	internal static class KeysAuthenticationType {
		public static SecAuthenticationType ToSecAuthenticationType (IntPtr handle)
		{
			if (handle == NTLM)
				return SecAuthenticationType.Ntlm;
			if (handle == MSN)
				return SecAuthenticationType.Msn;
			if (handle == DPA)
				return SecAuthenticationType.Dpa;
			if (handle == RPA)
				return SecAuthenticationType.Rpa;
			if (handle == HTTPBasic)
				return SecAuthenticationType.HttpBasic;
			if (handle == HTTPDigest)
				return SecAuthenticationType.HttpDigest;
			if (handle == HTMLForm)
				return SecAuthenticationType.HtmlForm;
			if (handle == Default)
				return SecAuthenticationType.Default;

			throw new ArgumentException ("handle");
		}

		public static IntPtr FromSecAuthenticationType (SecAuthenticationType type)
		{
			switch (type){
			case SecAuthenticationType.Ntlm:
				return NTLM;
			case SecAuthenticationType.Msn:
				return MSN;
			case SecAuthenticationType.Dpa:
				return DPA;
			case SecAuthenticationType.Rpa:
				return RPA;
			case SecAuthenticationType.HttpBasic:
				return HTTPBasic;
			case SecAuthenticationType.HttpDigest:
				return HTTPDigest;
			case SecAuthenticationType.HtmlForm:
				return HTMLForm;
			case SecAuthenticationType.Default:
				return Default;
			default:
				throw new ArgumentException ("type");
			}
		}
	
		static IntPtr _NTLM;
		public static IntPtr NTLM {
			get {
				if (_NTLM == IntPtr.Zero)
					_NTLM = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeNTLM");
				return _NTLM;
			}
		}
		
		static IntPtr _MSN;
		public static IntPtr MSN {
			get {
				if (_MSN == IntPtr.Zero)
					_MSN = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeMSN");
				return _MSN;
			}
		}
		
		static IntPtr _DPA;
		public static IntPtr DPA {
			get {
				if (_DPA == IntPtr.Zero)
					_DPA = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeDPA");
				return _DPA;
			}
		}
		
		static IntPtr _RPA;
		public static IntPtr RPA {
			get {
				if (_RPA == IntPtr.Zero)
					_RPA = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeRPA");
				return _RPA;
			}
		}
		
		static IntPtr _HTTPBasic;
		public static IntPtr HTTPBasic {
			get {
				if (_HTTPBasic == IntPtr.Zero)
					_HTTPBasic = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeHTTPBasic");
				return _HTTPBasic;
			}
		}
		
		static IntPtr _HTTPDigest;
		public static IntPtr HTTPDigest {
			get {
				if (_HTTPDigest == IntPtr.Zero)
					_HTTPDigest = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeHTTPDigest");
				return _HTTPDigest;
			}
		}
		
		static IntPtr _HTMLForm;
		public static IntPtr HTMLForm {
			get {
				if (_HTMLForm == IntPtr.Zero)
					_HTMLForm = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeHTMLForm");
				return _HTMLForm;
			}
		}
		
		static IntPtr _Default;
		public static IntPtr Default {
			get {
				if (_Default == IntPtr.Zero)
					_Default = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrAuthenticationTypeDefault");
				return _Default;
			}
		}
	}
	
	internal static class ClassKeys {
		static IntPtr _Public;
		public static IntPtr Public {
			get {
				if (_Public == IntPtr.Zero)
					_Public = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClassPublic");
				return _Public;
			}
		}
		
		static IntPtr _Private;
		public static IntPtr Private {
			get {
				if (_Private == IntPtr.Zero)
					_Private = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClassPrivate");
				return _Private;
			}
		}
		
		static IntPtr _Symmetric;
		public static IntPtr Symmetric {
			get {
				if (_Symmetric == IntPtr.Zero)
					_Symmetric = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyClassSymmetric");
				return _Symmetric;
			}
		}
	}
	
	internal static class KeyTypeKeys {
		static IntPtr _RSA;
		public static IntPtr RSA {
			get {
				if (_RSA == IntPtr.Zero)
					_RSA = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyTypeRSA");
				return _RSA;
			}
		}
		
		static IntPtr _EC;
		public static IntPtr EC {
			get {
				if (_EC == IntPtr.Zero)
					_EC = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecAttrKeyTypeEC");
				return _EC;
			}
		}
	}
	
	public static class SecMatchLimit {
		
		static IntPtr _MatchLimitOne;
		public static IntPtr MatchLimitOne {
			get {
				if (_MatchLimitOne == IntPtr.Zero)
					_MatchLimitOne = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecMatchLimitOne");
				return _MatchLimitOne;
			}
		}
		
		static IntPtr _MatchLimitAll;
		public static IntPtr MatchLimitAll {
			get {
				if (_MatchLimitAll == IntPtr.Zero)
					_MatchLimitAll = Dlfcn.GetIntPtr (SecItem.securityLibrary, "kSecMatchLimitAll");
				return _MatchLimitAll;
			}
		}
	}

	public class SecurityException : Exception {
		static string ToMessage (SecStatusCode code)
		{
			
			switch (code){
			case SecStatusCode.Success: 
			case SecStatusCode.Unimplemented: 
			case SecStatusCode.Param:
			case SecStatusCode.Allocate:
			case SecStatusCode.NotAvailable:
			case SecStatusCode.DuplicateItem:
			case SecStatusCode.ItemNotFound:
			case SecStatusCode.InteractionNotAllowed:
			case SecStatusCode.Decode:
				return code.ToString ();
			}
			return String.Format ("Unknown error: 0x{0:x}", code);
		}
		
		public SecurityException (SecStatusCode code) : base (ToMessage (code))
		{
		}
	}
	
}
