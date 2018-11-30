// 
// ImportExport.cs
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright 2011 Xamarin Inc.
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
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;

namespace MonoMac.Security {

	public class SecImportExport {
		
		static NSString passphrase;
		static NSString label;
		static NSString keyid;
		static NSString trust;
		static NSString chain;
		static NSString identity;
		
		static public NSString Passphrase {
			get {
				if (passphrase == null)
					passphrase = new NSString ("passphrase");
				return passphrase;
			}
		}

		static public NSString Label {
			get {
				if (label == null)
					label = new NSString ("label");
				return label;
			}
		}
		
		static public NSString KeyId {
			get {
				if (keyid == null)
					keyid = new NSString ("keyid");
				return keyid;
			}
		}
		
		static public NSString Trust {
			get {
				if (trust == null)
					trust = new NSString ("trust");
				return trust;
			}
		}
		
		static public NSString CertChain {
			get {
				if (chain == null)
					chain = new NSString ("chain");
				return chain;
			}
		}

		static public NSString Identity {
			get {
				if (identity == null)
					identity = new NSString ("identity");
				return identity;
			}
		}

		[DllImport (Constants.SecurityLibrary, EntryPoint="SecPKCS12Import")]
		extern static SecStatusCode SecPKCS12Import (IntPtr pkcs12_data, IntPtr options, out IntPtr items);
		
		static public SecStatusCode ImportPkcs12 (byte[] buffer, NSDictionary options, out NSDictionary[] array)
		{
			using (NSData data = NSData.FromArray (buffer)) {
				return ImportPkcs12 (data, options, out array);
			}
		}
		
		static NSDictionary GetRetainedDictionary (IntPtr handle)
		{
			NSDictionary dict = Runtime.GetNSObject<NSDictionary> (handle);
			dict.Release ();
			return dict;
		}
		
		static public SecStatusCode ImportPkcs12 (NSData data, NSDictionary options, out NSDictionary[] array)
		{
			if (options == null)
				throw new ArgumentNullException ("options");
			
			IntPtr handle;
			SecStatusCode code = SecPKCS12Import (data.Handle, options.Handle, out handle);
			// note: NSArray.ArrayFromHandle will retain the NSDictionary it creates - but the ones 
			// we're receiving already are retained, GetRetainedDictionary drops that extra level
			array = NSArray.ArrayFromHandle <NSDictionary> (handle, GetRetainedDictionary);
			return code;
		}
	}
}