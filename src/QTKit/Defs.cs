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
using System.Runtime.InteropServices;

namespace MonoMac.QTKit {
	
	[StructLayout (LayoutKind.Sequential)]
	public partial struct QTTime {
		public static readonly QTTime Zero = new QTTime (0, 1, 0);
		public static readonly QTTime IndefiniteTime = new QTTime (0, 1, TimeFlags.TimeIsIndefinite);
		public long TimeValue;
		public int  TimeScale;
		public TimeFlags Flags;

		public QTTime (long timeValue, int timeScale, TimeFlags flags)
		{
			TimeValue = timeValue;
			TimeScale = timeScale;
			Flags = flags;
		}

		public QTTime (long timeValue, int timeScale)
		{
			TimeValue = timeValue;
			TimeScale = timeScale;
			Flags = 0;
		}

		public override string ToString ()
		{
			if (Flags == 0)
				return String.Format ("[TimeValue={0} scale={1}]", TimeValue, TimeScale);
			else 
				return String.Format ("[TimeValue={0} scale={1} Flags={2}]", TimeValue, TimeScale, Flags);
		}
	}

	public struct QTTimeRange {
		public QTTime Time;
		public QTTime Duration;

		public QTTimeRange (QTTime time, QTTime duration)
		{
			Time = time;
			Duration = duration;
		}

		public override string ToString ()
		{
			return String.Format ("[Time={0} Duration={2}]", Time, Duration);
		}
	}

}
