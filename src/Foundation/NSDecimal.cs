//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
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
using System;
using System.Runtime.InteropServices;
using MonoMac;

namespace MonoMac.Foundation {
	[StructLayout (LayoutKind.Sequential)]
	public struct NSDecimal {
		public int fields;
		public short m1, m2, m3, m4, m5, m6, m7, m8;

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalCompare")]
		public static extern NSComparisonResult Compare (ref NSDecimal left, ref NSDecimal right);

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalRound")]
		public static extern void Round (out NSDecimal result, ref NSDecimal number, int scale, NSRoundingMode mode);
		
		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalNormalize")]
		public static extern NSCalculationError Normalize (ref NSDecimal number1, ref NSDecimal number2);
		
		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalAdd")]
		public static extern NSCalculationError Add (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode);

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalSubtract")]
		public static extern NSCalculationError Subtract (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode);

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalMultiply")]
		public static extern NSCalculationError Multiply (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode);

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalDivide")]
		public static extern NSCalculationError Divide (out NSDecimal result, ref NSDecimal left, ref NSDecimal right, NSRoundingMode mode);
		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalPower")]
		public static extern NSComparisonResult Power (out NSDecimal result, ref NSDecimal number, int power, NSRoundingMode mode);

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalMultiplyByPowerOf10")]
		public static extern NSComparisonResult MultiplyByPowerOf10 (out NSDecimal result, ref NSDecimal number, short power10, NSRoundingMode mode);

		[DllImport (Constants.FoundationLibrary, EntryPoint="NSDecimalMultiplyByPowerOf10")]
		static extern IntPtr NSDecimalString (ref NSDecimal value, IntPtr locale);

#if !COREBUILD
		public override string ToString ()
		{
			//return new NSString (NSDecimalString (ref this, NSLocale.CurrentLocale.Handle));
			return String.Format ("{0}:{1}{2}{3}{4}{5}{6}{7}{8}", fields, m1, m2, m3, m4, m5, m6, m7, m8);
		}
#endif
		public static NSDecimal operator + (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Add (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		public static NSDecimal operator - (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Subtract (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		public static NSDecimal operator * (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Multiply (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		public static NSDecimal operator / (NSDecimal left, NSDecimal right)
		{
			NSDecimal result;

			Divide (out result, ref left, ref right, NSRoundingMode.Plain);
			return result;
		}

		public static bool operator == (NSDecimal left, NSDecimal right)
		{
			return Compare (ref left, ref right) == NSComparisonResult.Same;
		}

		public static bool operator != (NSDecimal left, NSDecimal right)
		{
			return Compare (ref left, ref right) != NSComparisonResult.Same;
		}

#if !COREBUILD
		public static implicit operator NSDecimal (int value)
		{
			return new NSNumber (value).NSDecimalValue;
		}

		public static explicit operator int (NSDecimal value)
		{
			return new NSDecimalNumber (value).Int32Value;
		}
#endif
		
		public override bool Equals (object obj)
		{
			if (!(obj is NSDecimal))
				return false;
			
			return this == (NSDecimal) obj;
		}
		
		public override int GetHashCode ()
		{
			return fields ^ m1 ^ m2 ^ m3 ^ m4 ^ m5 ^ m6 ^ m7 ^ m8; 
		}
	}
}