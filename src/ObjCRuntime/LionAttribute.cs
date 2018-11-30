// 
// LionAttribute.cs: Used to flag classes, methods and properties that
// were introduced in Lion.
// 
// Copyright 2011, Xamarin, Inc.
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

namespace MonoMac.ObjCRuntime {
	public class LionAttribute : IntroducedAttribute {
		public LionAttribute() : base(10, 6) { }
	}
	public class MountainLionAttribute : IntroducedAttribute {
		public MountainLionAttribute() : base(10, 7) { }
	}
	public class MavericksAttribute : IntroducedAttribute {
		public MavericksAttribute() : base(10, 8) { }
	}
	public class YosemiteAttribute : IntroducedAttribute {
		public YosemiteAttribute() : base(10, 9) { }
	}
	public class ElCapitanAttribute : IntroducedAttribute {
		public ElCapitanAttribute() : base(10, 10) { }
	}

	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = true)]
	public abstract class AvailabilityBaseAttribute : Attribute
	{
		public string Message { get; private set; }
		
		public Version Version { get; private set; }
		
		public AvailabilityBaseAttribute(Version version, string message)
		{
			Version = version;
			Message = message;
		}
	}

	public class IntroducedAttribute : AvailabilityBaseAttribute
	{
		public IntroducedAttribute (int majorVersion, int minorVersion, string message = null)
			: base (new Version (majorVersion, minorVersion), message)
		{
		}

		public IntroducedAttribute (int majorVersion, int minorVersion, int subminorVersion, string message = null)
			: base(new Version(majorVersion, minorVersion, subminorVersion), message)
		{
		}
	}
	
	public class DeprecatedAttribute : AvailabilityBaseAttribute
	{
		public DeprecatedAttribute (int majorVersion, int minorVersion, string message = null)
			: base (new Version (majorVersion, minorVersion), message)
		{
		}

		public DeprecatedAttribute (int majorVersion, int minorVersion, int subminorVersion, string message = null)
			: base(new Version(majorVersion, minorVersion, subminorVersion), message)
		{
		}
	}
	
	public class MacAttribute : IntroducedAttribute
	{
		public MacAttribute (int majorVersion, int minorVersion, string message = null)
			: base (majorVersion, minorVersion, message)
		{
		}

		public MacAttribute (int majorVersion, int minorVersion, int subminorVersion, string message = null)
			: base(majorVersion, minorVersion, subminorVersion, message)
		{
		}
	}
}