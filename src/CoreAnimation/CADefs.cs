//
// A few complementary classes for CoreAnimation
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
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
//
using System;
using MonoMac.Foundation;
using System.Runtime.InteropServices;
using MonoMac.CoreGraphics;

namespace MonoMac.CoreAnimation {

	partial class CAAnimation {
		const string Linear = "linear";
		const string Discrete = "discrete";
		const string Paced = "paced";
		const string Cubic = "cubic";

		const string RotateAuto = "auto";
		const string RotateAutoReverse = "autoReverse";

		[DllImport(Constants.QuartzLibrary, EntryPoint="CACurrentMediaTime")]
		public extern static double CurrentMediaTime ();
	}

	public partial class CAFillMode {
		public const string Removed = "removed";
		public const string Forwards = "forwards";
		public const string Backwards = "backwards";
		public const string Both = "both";
		public const string Frozen = "frozen";
	}

	partial class CATransition {
		const string Fade = "fade";
		const string MoveIn = "moveIn";
		const string Push = "push";
		const string Reveal = "reveal";
		
		const string FromRight = "fromRight";
		const string FromLeft = "fromLeft";
		const string FromTop = "fromTop";
		const string FromBottom = "fromBottom";
	}

	public partial class CAGradientLayer {
		public CGColor CreateColor (IntPtr p)
		{
			return new CGColor (p);
		}
		
		public CGColor [] Colors {
			get {
				return NSArray.ArrayFromHandle<CGColor> (_Colors, CreateColor);
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				
				IntPtr [] ptrs = new IntPtr [value.Length];
				for (int i = 0; i < ptrs.Length; i++)
					ptrs [i] = value [i].Handle;
				
				using (NSArray array = NSArray.FromIntPtrs (ptrs)){
					_Colors = array.Handle;
				}
			}
		}
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct CABarBeatTime {
		public int Bar;
		public ushort Beat;
		public ushort Subbeat;
		public ushort SubbeatDivisor;
		public ushort Reserved;
	}

	public partial class CAKeyFrameAnimation {

		[Obsolete ("This method in the future will return a CAKeyFrameAnimation, update your source, or use GetFromKeyPath to avoid this warning for now")]
		public static CAPropertyAnimation FromKeyPath (string path)
		{
			return GetFromKeyPath (path);
		}
	}
	
}
