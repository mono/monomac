// 
// CMTime.cs: API for creating and manipulating CMTime structs
//
// Authors: Mono Team
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTime {
		[FlagsAttribute]
		public enum Flags {
			Valid = 1,
			HasBeenRounded = 2,
			PositiveInfinity = 4,
			NegativeInfinity = 8,
			Indefinite = 16,
			ImpliedValueFlagsMask = 28
		}

		public static CMTime Invalid = new CMTime (0);
		const Flags kIndefinite = Flags.Valid | Flags.Indefinite;
		public static CMTime Indefinite = new CMTime (kIndefinite);
		const Flags kPositive = Flags.Valid | Flags.PositiveInfinity;
		public static CMTime PositiveInfinity = new CMTime (kPositive);
		const Flags kNegative = Flags.Valid | Flags.NegativeInfinity;
		public static CMTime NegativeInfinity = new CMTime (kNegative);
		public static CMTime Zero = new CMTime (Flags.Valid, 1);
		
		public const int MaxTimeScale = 0x7fffffff;
		public long Value;
		public int TimeScale;
		public Flags TimeFlags;
		public long TimeEpoch;

		CMTime (Flags f)
		{
			Value = 0;
			TimeScale = 0;
			TimeEpoch = 0;
			TimeFlags = f;
		}

		CMTime (Flags f, int timescale)
		{
			Value = 0;
			TimeScale = timescale;
			TimeEpoch = 0;
			TimeFlags = f;
		}
		       
		public CMTime (long value, int timescale)
		{
			Value = value;
			TimeScale = timescale;
			TimeFlags = Flags.Valid;
			TimeEpoch = 0;
		}
		
		public CMTime (long value, int timescale, long epoch)
		{
			Value = value;
			TimeScale = timescale;
			TimeFlags = Flags.Valid;
			TimeEpoch = epoch;
		}

		public bool IsInvalid {
			get {
				return (TimeFlags & Flags.Valid) == 0;
			}
		}

		public bool IsIndefinite {
			get {
				return (TimeFlags & kIndefinite) == kIndefinite;
			}
		}

		public bool IsPositiveInfinity {
			get {
				return (TimeFlags & kPositive) == kPositive;
			}
		}

		public bool IsNegativeInfinity {
			get {
				return (TimeFlags & kNegative) == kNegative;
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeAbsoluteValue (CMTime time);
		
		public CMTime AbsoluteValue {
			get {
				return CMTimeAbsoluteValue (this);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMTimeCompare (CMTime time1, CMTime time2);
		public static int Compare (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2);
		}
		
		public static bool operator == (CMTime time1, CMTime time2)
		{
			return Compare (time1, 	time2) == 0;
		}
		
		public static bool operator != (CMTime time1, CMTime time2)
		{
			return !(time1 == time2);
		}
		
		public override bool Equals (object obj)
		{
			if (!(obj is CMTime))
				return false;
			
			CMTime other = (CMTime) obj;
			return other == this;
		}
		
		public override int GetHashCode ()
		{
			return Value.GetHashCode () ^ TimeScale.GetHashCode () ^ TimeFlags.GetHashCode () ^ TimeEpoch.GetHashCode ();
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeAdd (CMTime addend1, CMTime addend2);
		public static CMTime Add (CMTime time1, CMTime time2)
		{
			return CMTimeAdd (time1, time2);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeSubtract (CMTime minuend, CMTime subtraend);
		public static CMTime Subtract (CMTime minuend, CMTime subtraend)
		{
			return CMTimeSubtract (minuend, subtraend);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiply (CMTime time, int multiplier);
		public static CMTime Multiply (CMTime time, int multiplier)
		{
			return CMTimeMultiply (time, multiplier);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiplyByFloat64 (CMTime time, double multiplier);
		public static CMTime Multiply (CMTime time, double multiplier)
		{
			return CMTimeMultiplyByFloat64 (time, multiplier);
		}
		
		public static CMTime operator + (CMTime time1, CMTime time2)
		{
			return Add (time1, time2);
		}
		
		public static CMTime operator - (CMTime minuend, CMTime subtraend)
		{
			return Subtract (minuend, subtraend);
		}
		
		public static CMTime operator * (CMTime time, int multiplier)
		{
			return Multiply (time, multiplier);
		}
		
		public static CMTime operator * (CMTime time, double multiplier)
		{
			return Multiply (time, multiplier);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeConvertScale (CMTime time, int newScale, CMTimeRoundingMethod method);
		public CMTime ConvertScale (int newScale, CMTimeRoundingMethod method)
		{
			return CMTimeConvertScale (this, newScale, method);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static double CMTimeGetSeconds (CMTime time);
		public double Seconds {
			get {
				return CMTimeGetSeconds (this);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMakeWithSeconds (double seconds, int preferredTimeScale);
		public static CMTime FromSeconds (double seconds, int preferredTimeScale)
		{
			return CMTimeMakeWithSeconds (seconds, preferredTimeScale);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMaximum (CMTime time1, CMTime time2);
		public static CMTime GetMaximum (CMTime time1, CMTime time2)
		{
			return CMTimeMaximum (time1, time2);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMinimum (CMTime time1, CMTime time2);
		public static CMTime GetMinimum (CMTime time1, CMTime time2)
		{
			return CMTimeMinimum (time1, time2);
		}
		
		public readonly static NSString ValueKey;
		public readonly static NSString ScaleKey;
		public readonly static NSString EpochKey;
		public readonly static NSString FlagsKey;
		
		static CMTime ()
		{
			var lib = Dlfcn.dlopen (Constants.CoreMediaLibrary, 0);
			if (lib != IntPtr.Zero) {
				try {
					ValueKey  = Dlfcn.GetStringConstant (lib, "kCMTimeValueKey");
					ScaleKey  = Dlfcn.GetStringConstant (lib, "kCMTimeScaleKey");
					EpochKey  = Dlfcn.GetStringConstant (lib, "kCMTimeEpochKey");
					FlagsKey  = Dlfcn.GetStringConstant (lib, "kCMTimeFlagsKey");
				} finally {
					Dlfcn.dlclose (lib);
				}
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimeCopyAsDictionary (CMTime time, IntPtr allocator);
		public IntPtr AsDictionary {
			get {
				return CMTimeCopyAsDictionary (this, IntPtr.Zero);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimeCopyDescription (IntPtr allocator, CMTime time);
		public string Description {
			get {
				return NSString.FromHandle (CMTimeCopyDescription (IntPtr.Zero, this)).ToString ();
			}
		}
		
		public override string ToString ()
		{
			return Description;
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMakeFromDictionary (IntPtr dict);
		public static CMTime FromDictionary (IntPtr dict)
		{
			return CMTimeMakeFromDictionary (dict);
		}
		
		// Should we also bind CMTimeShow?
	}
}