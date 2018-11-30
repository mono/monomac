//
// NSData.cs:
// Author:
//   Miguel de Icaza
//
// Copyright 2011, Novell, Inc.
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
//
using MonoMac.ObjCRuntime;
using System;

namespace MonoMac.Foundation {

	public enum NSFileType {
		Directory, Regular, SymbolicLink, Socket, CharacterSpecial, BlockSpecial, Unknown
	}

	public class NSFileAttributes {
		public NSFileAttributes () {}
		
		
		public bool? AppendOnly { get; set; }
		public bool? Busy { get; set; }
		public bool? FileExtensionHidden { get; set; }
		public NSDate CreationDate { get; set; }
		public string OwnerAccountName { get; set; }
		public uint? DeviceIdentifier { get; set; }
		public uint? FileGroupOwnerAccountID { get; set; }

		public bool? Immutable { get; set; }
		public NSDate ModificationDate { get; set; }
		public uint? FileOwnerAccountID { get; set; }
		public uint? HfsTypeCode { get; set; }
		public uint? PosixPermissions { get; set; }
		public uint? FileReferenceCount { get; set; }
		public uint? FileSystemFileNumber { get; set; }
		public ulong? FileSize { get; set; }
		public NSFileType? FileType { get; set; }
		//public bool? ProtectedFile { get; set; }
		
		internal NSDictionary ToDictionary ()
		{
			var dict = new NSMutableDictionary ();
			if (AppendOnly.HasValue)
				dict.SetObject (NSNumber.FromBoolean (AppendOnly.Value), NSFileManager.AppendOnly);
			if (Busy.HasValue)
				dict.SetObject (NSNumber.FromBoolean (Busy.Value), NSFileManager.Busy);
			if (CreationDate != null)
				dict.SetObject (CreationDate, NSFileManager.CreationDate);
			if (ModificationDate != null)
				dict.SetObject (ModificationDate, NSFileManager.ModificationDate);
			if (OwnerAccountName != null)
				dict.SetObject (new NSString (OwnerAccountName), NSFileManager.OwnerAccountName);
			if (DeviceIdentifier.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (DeviceIdentifier.Value), NSFileManager.DeviceIdentifier);
			if (FileExtensionHidden.HasValue)
				dict.SetObject (NSNumber.FromBoolean (FileExtensionHidden.Value), NSFileManager.ExtensionHidden);
			if (FileGroupOwnerAccountID.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (FileGroupOwnerAccountID.Value), NSFileManager.GroupOwnerAccountID);
			if (FileOwnerAccountID.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (FileOwnerAccountID.Value), NSFileManager.OwnerAccountID);
			if (HfsTypeCode.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (HfsTypeCode.Value), NSFileManager.HfsTypeCode);
			if (PosixPermissions.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (PosixPermissions.Value), NSFileManager.PosixPermissions);
			if (FileReferenceCount.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (FileReferenceCount.Value), NSFileManager.ReferenceCount);
			if (FileSystemFileNumber.HasValue)
				dict.SetObject (NSNumber.FromUInt32 (FileSystemFileNumber.Value), NSFileManager.SystemFileNumber);
			if (FileSize.HasValue)
				dict.SetObject (NSNumber.FromUInt64 (FileSize.Value), NSFileManager.Size);
			if (Immutable.HasValue)
				dict.SetObject (NSNumber.FromBoolean (Immutable.Value), NSFileManager.Immutable);
			//if (ProtectedFile.HasValue)
			//dict.SetObject (NSNumber.FromBoolean (ProtectedFile.Value), NSFileManager.ProtectedFile);
			if (FileType.HasValue){
				NSString v = null;
				switch (FileType.Value){
				case NSFileType.Directory:
					v = NSFileManager.TypeDirectory; break;
				case NSFileType.Regular:
					v = NSFileManager.TypeRegular; break;
				case NSFileType.SymbolicLink:
					v = NSFileManager.TypeSymbolicLink; break;
				case NSFileType.Socket:
					v = NSFileManager.TypeSocket; break;
				case NSFileType.CharacterSpecial:
					v = NSFileManager.TypeCharacterSpecial; break;
				case NSFileType.BlockSpecial:
					v = NSFileManager.TypeBlockSpecial; break;
				default:
					v = NSFileManager.TypeUnknown; break;
				}
				dict.SetObject (v, NSFileManager.NSFileType);
			}
			return dict;
		}

		internal static bool fetch (NSDictionary dict, NSString key, ref bool b)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k == null)
				return false;
			b = k.BoolValue;
			return true;
		}

		internal static bool fetch (NSDictionary dict, NSString key, ref uint b)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k == null)
				return false;
			b = k.UInt32Value;
			return true;
		}

		internal static bool fetch (NSDictionary dict, NSString key, ref ulong b)
		{
			var k = dict.ObjectForKey (key) as NSNumber;
			if (k == null)
				return false;
			b = k.UInt64Value;
			return true;
		}
		
		public static NSFileAttributes FromDict (NSDictionary dict)
		{
			if (dict == null)
				return null;
			var ret = new NSFileAttributes ();

			bool b = false;
			if (fetch (dict, NSFileManager.AppendOnly, ref b))
				ret.AppendOnly = b;
			if (fetch (dict, NSFileManager.Busy, ref b))
				ret.Busy = b;
			if (fetch (dict, NSFileManager.Immutable, ref b))
				ret.Immutable = b;
			//if (fetch (dict, NSFileManager.ProtectedFile, ref b))
			//ret.ProtectedFile = b;
			if (fetch (dict, NSFileManager.ExtensionHidden, ref b))
				ret.FileExtensionHidden = b;
			var date = dict.ObjectForKey (NSFileManager.CreationDate) as NSDate;
			if (date != null)
				ret.CreationDate = date;
			date = dict.ObjectForKey (NSFileManager.ModificationDate) as NSDate;
			if (date != null)
				ret.ModificationDate = date;
			var name = dict.ObjectForKey (NSFileManager.OwnerAccountName) as NSString;
			if (name != null)
				ret.OwnerAccountName = name.ToString ();
			uint u = 0;
			if (fetch (dict, NSFileManager.DeviceIdentifier, ref u))
				ret.DeviceIdentifier = u;
			if (fetch (dict, NSFileManager.GroupOwnerAccountID, ref u))
				ret.FileGroupOwnerAccountID = u;
			if (fetch (dict, NSFileManager.OwnerAccountID, ref u))
				ret.FileOwnerAccountID = u;
			if (fetch (dict, NSFileManager.HfsTypeCode, ref u))
				ret.HfsTypeCode = u;
			if (fetch (dict, NSFileManager.PosixPermissions, ref u))
				ret.PosixPermissions = u;
			if (fetch (dict, NSFileManager.ReferenceCount, ref u))
				ret.FileReferenceCount = u;
			if (fetch (dict, NSFileManager.SystemFileNumber, ref u))
				ret.FileSystemFileNumber = u;
			ulong l = 0;
			if (fetch (dict, NSFileManager.Size, ref l))
				ret.FileSize = l;
			return ret;
		}
	}

	public class NSFileSystemAttributes {
		NSDictionary dict;
		
		internal NSFileSystemAttributes (NSDictionary dict)
		{
			this.dict = dict;
		}

		public ulong Size { get; internal set; }
		public ulong FreeSize { get; internal set; }
		public long Nodes { get; internal set; }
		public long FreeNodes { get; internal set; }
		public uint Number { get; internal set; }

		internal static NSFileSystemAttributes FromDict (NSDictionary dict)
		{
			if (dict == null)
				return null;
			var ret = new NSFileSystemAttributes (dict);
			ulong l = 0;
			uint i = 0;
			ret.Size     = NSFileAttributes.fetch (dict, NSFileManager.SystemSize, ref l) ? l : 0;
			ret.FreeSize = NSFileAttributes.fetch (dict, NSFileManager.SystemFreeSize, ref l) ? l : 0;
			ret.Nodes    = NSFileAttributes.fetch (dict, NSFileManager.SystemNodes, ref l) ? (long) l : 0;
			ret.FreeNodes= NSFileAttributes.fetch (dict, NSFileManager.SystemFreeNodes, ref l) ? (long) l : 0;
			ret.Number   = NSFileAttributes.fetch (dict, NSFileManager.SystemFreeNodes, ref i) ? i : 0;

			return ret;
		}

		// For source code compatibility with users that had done manual NSDictionary lookups before
		public static implicit operator NSDictionary (NSFileSystemAttributes attr)
		{
			return attr.dict;
		}
		
	}		
	
	public partial class NSFileManager {
		public bool SetAttributes (NSFileAttributes attributes, string path, out NSError error)
		{
			if (attributes == null)
				throw new ArgumentNullException ("attributes");
			return SetAttributes (attributes.ToDictionary (), path, out error);
		}

		public bool SetAttributes (NSFileAttributes attributes, string path)
		{
			NSError ignore;
			if (attributes == null)
				throw new ArgumentNullException ("attributes");

			return SetAttributes (attributes.ToDictionary (), path, out ignore);
		}

		public bool CreateDirectory (string path, bool createIntermediates, NSFileAttributes attributes, out NSError error)
		{
			var dict = attributes == null ? null : attributes.ToDictionary ();
			return CreateDirectory (path, createIntermediates, dict, out error);
		}

		public bool CreateDirectory (string path, bool createIntermediates, NSFileAttributes attributes)
		{
			NSError error;
			var dict = attributes == null ? null : attributes.ToDictionary ();
			return CreateDirectory (path, createIntermediates, dict, out error);
		}

		public bool CreateFile (string path, NSData data, NSFileAttributes attributes)
		{
			var dict = attributes == null ? null : attributes.ToDictionary ();
			return CreateFile (path, data, dict);
		}
		
		public NSFileAttributes GetAttributes (string path, out NSError error)
		{
			return NSFileAttributes.FromDict (_GetAttributes (path, out error));
		}

		public NSFileAttributes GetAttributes (string path)
		{
			NSError error;
			return NSFileAttributes.FromDict (_GetAttributes (path, out error));
		}

		public NSFileSystemAttributes GetFileSystemAttributes (string path)
		{
			NSError error;
			return NSFileSystemAttributes.FromDict (_GetFileSystemAttributes (path, out error));
		}

		public NSFileSystemAttributes GetFileSystemAttributes (string path, out NSError error)
		{
			return NSFileSystemAttributes.FromDict (_GetFileSystemAttributes (path, out error));
		}

		public string CurrentDirectory {
			get { return GetCurrentDirectory (); }
			// ignore boolean return value
			set { ChangeCurrentDirectory (value); }
		}
	}
}