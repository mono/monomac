//
// Copyright 2010, Novell, Inc.
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
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public partial class NSNumber : NSValue {
		public static implicit operator NSNumber (float value)
		{
			return FromFloat (value); 
		}

		public static implicit operator NSNumber (double value)
		{
			return FromDouble (value); 
		}

		public static implicit operator NSNumber (bool value)
		{
			return FromBoolean (value); 
		}
		
		public static implicit operator NSNumber (sbyte value)
		{
			return FromSByte (value);
		}

		public static implicit operator NSNumber (byte value)
		{
			return FromByte (value);
		}

		public static implicit operator NSNumber (short value)
		{
			return FromInt16 (value);
		}

		public static implicit operator NSNumber (ushort value)
		{
			return FromUInt16 (value);
		}

		public static implicit operator NSNumber (int value)
		{
			return FromInt32 (value);
		}

		public static implicit operator NSNumber (uint value)
		{
			return FromUInt32 (value);
		}

		public static implicit operator NSNumber (long value)
		{
			return FromInt64 (value);
		}

		public static implicit operator NSNumber (ulong value)
		{
			return FromUInt64 (value);
		}

		public static explicit operator byte (NSNumber source)
		{
			return source.ByteValue;
		}

		public static explicit operator sbyte (NSNumber source)
		{
			return source.SByteValue;
		}

		public static explicit operator short (NSNumber source)
		{
			return source.Int16Value;
		}

		public static explicit operator ushort (NSNumber source)
		{
			return source.UInt16Value;
		}

		public static explicit operator int (NSNumber source)
		{
			return source.Int32Value;
		}

		public static explicit operator uint (NSNumber source)
		{
			return source.UInt32Value;
		}

		public static explicit operator long (NSNumber source)
		{
			return source.Int64Value;
		}

		public static explicit operator ulong (NSNumber source)
		{
			return source.UInt64Value;
		}

		public static explicit operator float (NSNumber source)
		{
			return source.FloatValue;
		}

		public static explicit operator double (NSNumber source)
		{
			return source.DoubleValue;
		}

		public static explicit operator bool (NSNumber source)
		{
			return source.BoolValue;
		}

		public override string ToString ()
		{
			return StringValue;
		}
	}
}
