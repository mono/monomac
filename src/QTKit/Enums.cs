<<<<<<< HEAD
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

	[Flags]
	public enum TimeFlags {
		TimeIsIndefinite = 1
	}
	
	[Flags]
	public enum  QTMovieFileTypeOptions {
		StillImageTypes = 1 << 0,
		TranslatableTypes = 1 << 1,
		AggressiveTypes = 1 << 2,
		DynamicTypes = 1 << 3,
		CommonTypes = 0,
		AllTypes = 0xffff
	}
} 
=======
using System;

namespace MonoMac.QTKit
{
/* how are structs handled
	public struct QTTime {
        	public long            timeValue;
	        public long            timeScale;
	        public long            flags;
	}

	public struct QTTimeRange{
	        public QTTime          time;
	        public QTTime          duration;
	}
*/	
	public enum  QTMovieFileTypeOptions
	{
	        StillImageTypes                        = 1 << 0,
	        TranslatableTypes                      = 1 << 1,
	        AggressiveTypes                        = 1 << 2,
	        DynamicTypes                           = 1 << 3,
	        CommonTypes                            = 0,
	        AllTypes                               = 0xffff
	}

}
>>>>>>> 1c8be31fbeb1771814e9dd082d906e327fafd34e
