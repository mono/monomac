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
using System;

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

namespace MonoMac.CoreData {

        public enum NSEntityMappingType : nuint {
                Undefined = 0x00,
                Custom = 0x01,
                Add = 0x02,
		Remove = 0x03,
		Copy = 0x05,
		Transform = 0x06
        }

	public enum NSAttributeType : nuint {
		Undefined = 0,
		Integer16 = 100,
		Integer32 = 200,
		Integer64 = 300,
		Decimal = 400,
		Double = 500,
		Float = 600,
		String = 700,
		Boolean = 800,
		Date = 900,
		Binary = 1000,
		Transformable = 1800    
	}

	[Flags]
	public enum NSFetchRequestResultType : nuint {
		ManagedObject = 0x00,
		ManagedObjectID = 0x01,
		DictionaryResultType = 0x02,
		NSCountResultType = 0x04
	}

	public enum NSKeyValueSetMutationKind : nuint {
		Union = 1,
		Minus = 2,
		Intersect = 3,
		NSKeyValueSet = 4
	}

	public enum NSDeleteRule : nuint {
		NoAction,
		Nullify,
		Cascade,
		Deny
	}

	public enum NSPersistentStoreRequestType : nuint {
		Fetch = 1,
		Save,
#if MONOMAC
		BatchUpdate = 6
#endif
	}

	public enum NSManagedObjectContextConcurrencyType : nuint {
		Confinement, PrivateQueue, MainQueue
	}

	public enum NSMergePolicyType : nuint {
		Error, PropertyStoreTrump, PropertyObjectTrump, Overwrite, RollbackMerge
	}

#if !MONOMAC
	public enum NSFetchedResultsChangeType {
		Insert = 1,
		Delete = 2,
		Move = 3,
		Update = 4
	}
#endif
}