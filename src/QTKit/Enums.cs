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
