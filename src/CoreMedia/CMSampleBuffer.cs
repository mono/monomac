// 
// CMSampleBuffer.cs: Implements the managed CMSampleBuffer
//
// Authors: Mono Team
//			Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2010 Novell, Inc
// Copyright 2012-2013 Xamarin Inc
//
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

using OSStatus = System.Int32;

#if !COREBUILD
using MonoMac.AudioToolbox;
using MonoMac.CoreVideo;
#if !MONOMAC
using MonoTouch.UIKit;
#endif
#endif

namespace MonoMac.CoreMedia {

	public enum CMSampleBufferError {
		None							= 0,
		AllocationFailed				= -12730,
		RequiredParameterMissing		= -12731,
		AlreadyHasDataBuffer			= -12732,
		BufferNotReady					= -12733,
		SampleIndexOutOfRange			= -12734,
		BufferHasNoSampleSizes			= -12735,
		BufferHasNoSampleTimingInfo		= -12736,
		ArrayTooSmall					= -12737,
		InvalidEntryCount				= -12738,
		CannotSubdivide					= -12739,
		SampleTimingInfoInvalid			= -12740,
		InvalidMediaTypeForOperation	= -12741,
		InvalidSampleData				= -12742,
		InvalidMediaFormat				= -12743,
		Invalidated						= -12744,
	}

	[Since (4,0)]
	public class CMSampleBuffer : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMSampleBuffer (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMSampleBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMSampleBuffer ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

#if !COREBUILD
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMAudioSampleBufferCreateWithPacketDescriptions (
		   IntPtr allocator,
		   IntPtr dataBuffer,
		   bool dataReady,
		   IntPtr makeDataReadyCallback,
		   IntPtr makeDataReadyRefcon,
		   IntPtr formatDescription,
		   int numSamples,
		   CMTime sbufPTS,
		   AudioStreamPacketDescription[] packetDescriptions,
		   out IntPtr sBufOut);

		public static CMSampleBuffer CreateWithPacketDescriptions (CMBlockBuffer dataBuffer, CMFormatDescription formatDescription, int samplesCount,
			CMTime sampleTimestamp, AudioStreamPacketDescription[] packetDescriptions, out CMSampleBufferError error)
		{
			if (formatDescription == null)
				throw new ArgumentNullException ("formatDescription");
			if (samplesCount <= 0)
				throw new ArgumentOutOfRangeException ("samplesCount");

			IntPtr buffer;
			error = CMAudioSampleBufferCreateWithPacketDescriptions (IntPtr.Zero,
				dataBuffer == null ? IntPtr.Zero : dataBuffer.handle,
				true, IntPtr.Zero, IntPtr.Zero,
				formatDescription.handle,
				samplesCount, sampleTimestamp,
				packetDescriptions,
				out buffer);

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		unsafe static extern OSStatus CMSampleBufferCreateCopyWithNewTiming (
			IntPtr allocator,
			IntPtr originalSBuf,
			int numSampleTimingEntries,
			CMSampleTimingInfo* sampleTimingArray,
			out IntPtr sBufCopyOut
			);

		public static CMSampleBuffer CreateWithNewTiming (CMSampleBuffer original, CMSampleTimingInfo [] timing)
		{
			OSStatus status;
			return CreateWithNewTiming (original, timing, out status);
		}

		public unsafe static CMSampleBuffer CreateWithNewTiming (CMSampleBuffer original, CMSampleTimingInfo [] timing, out OSStatus status)
		{
			IntPtr handle;

			fixed (CMSampleTimingInfo *t = timing)
				if ((status = CMSampleBufferCreateCopyWithNewTiming (IntPtr.Zero, original.Handle, timing.Length, t, out handle)) != 0)
					return null;
			
			return new CMSampleBuffer (handle, true);
		}
/*
		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCallForEachSample (
		   CMSampleBufferRef sbuf,
		   int (*callback)(CMSampleBufferRef sampleBuffer, int index, void *refcon),
		   void *refcon
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCopySampleBufferForRange (
		   CFAllocatorRef allocator,
		   CMSampleBufferRef sbuf,
		   CFRange sampleRange,
		   CMSampleBufferRef *sBufOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCreate (
		   CFAllocatorRef allocator,
		   CMBlockBufferRef dataBuffer,
		   Boolean dataReady,
		   CMSampleBufferMakeDataReadyCallback makeDataReadyCallback,
		   void *makeDataReadyRefcon,
		   CMFormatDescriptionRef formatDescription,
		   int numSamples,
		   int numSampleTimingEntries,
		   const CMSampleTimingInfo *sampleTimingArray,
		   int numSampleSizeEntries,
		   const uint *sampleSizeArray,
		   CMSampleBufferRef *sBufOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCreateCopy (
		   CFAllocatorRef allocator,
		   CMSampleBufferRef sbuf,
		   CMSampleBufferRef *sbufCopyOut
		);
		*/

		[DllImport(Constants.CoreMediaLibrary)]
		static extern CMSampleBufferError CMSampleBufferCreateForImageBuffer (IntPtr allocator,
		   IntPtr imageBuffer, bool dataReady,
		   IntPtr makeDataReadyCallback, IntPtr makeDataReadyRefcon,
		   IntPtr formatDescription,
		   ref CMSampleTimingInfo sampleTiming,
		   out IntPtr bufOut
		);

		public static CMSampleBuffer CreateForImageBuffer (CVImageBuffer imageBuffer, bool dataReady, CMVideoFormatDescription formatDescription, CMSampleTimingInfo sampleTiming, out CMSampleBufferError error)
		{
			if (imageBuffer == null)
				throw new ArgumentNullException ("imageBuffer");
			if (formatDescription == null)
				throw new ArgumentNullException ("formatDescription");

			IntPtr buffer;
			error = CMSampleBufferCreateForImageBuffer (IntPtr.Zero,
				imageBuffer.handle, dataReady,
				IntPtr.Zero, IntPtr.Zero,
				formatDescription.handle,
				ref sampleTiming,
				out buffer);

			if (error != CMSampleBufferError.None)
				return null;

			return new CMSampleBuffer (buffer, true);
		}
#endif
		[DllImport(Constants.CoreMediaLibrary)]
		extern static bool CMSampleBufferDataIsReady (IntPtr handle);
		
		public bool DataIsReady
		{
			get
			{
				return CMSampleBufferDataIsReady (handle);
			}
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioBufferListWithRetainedBlockBuffer (
		   CMSampleBufferRef sbuf,
		   uint *bufferListSizeNeededOut,
		   AudioBufferList *bufferListOut,
		   uint bufferListSize,
		   CFAllocatorRef bbufStructAllocator,
		   CFAllocatorRef bbufMemoryAllocator,
		   uint32_t flags,
		   CMBlockBufferRef *blockBufferOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioStreamPacketDescriptions (
		   CMSampleBufferRef sbuf,
		   uint packetDescriptionsSize,
		   AudioStreamPacketDescription *packetDescriptionsOut,
		   uint *packetDescriptionsSizeNeededOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioStreamPacketDescriptionsPtr (
		   CMSampleBufferRef sbuf,
		   const AudioStreamPacketDescription **packetDescriptionsPtrOut,
		   uint *packetDescriptionsSizeOut
		);*/

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetDataBuffer (IntPtr handle);
		
		public CMBlockBuffer GetDataBuffer ()
		{
			var blockHandle = CMSampleBufferGetDataBuffer (handle);			
			if (blockHandle == IntPtr.Zero)
			{
				return null;
			}
			else
			{
				return new CMBlockBuffer (blockHandle, false);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetDecodeTimeStamp (IntPtr handle);
		
		public CMTime DecodeTimeStamp
		{
			get
			{
				return CMSampleBufferGetDecodeTimeStamp (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetDuration (IntPtr handle);
		
		public CMTime Duration
		{
			get
			{
				return CMSampleBufferGetDuration (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetFormatDescription (IntPtr handle);

		[Advice ("Use GetAudioFormatDescription or GetVideoFormatDescription")]		
		public CMFormatDescription GetFormatDescription ()
		{
			var desc = default(CMFormatDescription);
			var descHandle = CMSampleBufferGetFormatDescription (handle);
			if (descHandle != IntPtr.Zero)
			{
				desc = new CMFormatDescription (descHandle, false);
			}
			return desc;					
		}

		public CMAudioFormatDescription GetAudioFormatDescription ()
		{
			var descHandle = CMSampleBufferGetFormatDescription (handle);
			if (descHandle == IntPtr.Zero)
				return null;

			return new CMAudioFormatDescription (descHandle, false);
		}

		public CMVideoFormatDescription GetVideoFormatDescription ()
		{
			var descHandle = CMSampleBufferGetFormatDescription (handle);
			if (descHandle == IntPtr.Zero)
				return null;

			return new CMVideoFormatDescription (descHandle, false);
		}

#if !COREBUILD

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetImageBuffer (IntPtr handle);

		public CVImageBuffer GetImageBuffer ()
		{
			IntPtr ib = CMSampleBufferGetImageBuffer (handle);
			if (ib == IntPtr.Zero)
				return null;

			var ibt = CFType.GetTypeID (ib);
			if (ibt == CVPixelBuffer.CVImageBufferType)
				return new CVPixelBuffer (ib, false);
			return new CVImageBuffer (ib, false);
		}
		
#endif
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferGetNumSamples (IntPtr handle);
		
		public int NumSamples
		{
			get
			{
				return CMSampleBufferGetNumSamples (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputDecodeTimeStamp (IntPtr handle);
		
		public CMTime OutputDecodeTimeStamp
		{
			get
			{
				return CMSampleBufferGetOutputDecodeTimeStamp (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputDuration (IntPtr handle);
		
		public CMTime OutputDuration
		{
			get
			{
				return CMSampleBufferGetOutputDuration (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputPresentationTimeStamp (IntPtr handle);
		
		public CMTime OutputPresentationTimeStamp
		{
			get
			{
				return CMSampleBufferGetOutputPresentationTimeStamp (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMSampleBufferSetOutputPresentationTimeStamp (IntPtr handle, CMTime outputPresentationTimeStamp);
		
		public int /*CMSampleBufferError*/ SetOutputPresentationTimeStamp (CMTime outputPresentationTimeStamp)
		{
			return (int)CMSampleBufferSetOutputPresentationTimeStamp (handle, outputPresentationTimeStamp);
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetOutputSampleTimingInfoArray (
		   CMSampleBufferRef sbuf,
		   int timingArrayEntries,
		   CMSampleTimingInfo *timingArrayOut,
		   int *timingArrayEntriesNeededOut
		);*/

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetPresentationTimeStamp (IntPtr handle);
		
		public CMTime PresentationTimeStamp
		{
			get
			{
				return CMSampleBufferGetPresentationTimeStamp (handle);
			}
		}
		
#if !COREBUILD

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetSampleAttachmentsArray (IntPtr handle, bool createIfNecessary);
		
		public CMSampleBufferAttachmentSettings [] GetSampleAttachments (bool createIfNecessary)
		{
			var cfArrayRef = CMSampleBufferGetSampleAttachmentsArray (handle, createIfNecessary);
			if (cfArrayRef == IntPtr.Zero)
			{
				return new CMSampleBufferAttachmentSettings [0];
			}
			else
			{
				return NSArray.ArrayFromHandle (cfArrayRef, h => new CMSampleBufferAttachmentSettings (Runtime.GetNSObject<NSMutableDictionary> (h)));
			}
		}
		
#endif

		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMSampleBufferGetSampleSize (IntPtr handle, int sampleIndex);
		
		public uint GetSampleSize (int sampleIndex)
		{
			return CMSampleBufferGetSampleSize (handle, sampleIndex);
		}
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleSizeArray (
		   CMSampleBufferRef sbuf,
		   int sizeArrayEntries,
		   uint *sizeArrayOut,
		   int *sizeArrayEntriesNeededOut
		);
		
		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleTimingInfo (
		   CMSampleBufferRef sbuf,
		   int sampleIndex,
		   CMSampleTimingInfo *timingInfoOut
		);
		*/
		
		[DllImport(Constants.CoreMediaLibrary)]
		unsafe static extern OSStatus CMSampleBufferGetSampleTimingInfoArray (
		   IntPtr sbuf,
		   int timingArrayEntries,
		   CMSampleTimingInfo* timingArrayOut,
		   out int timingArrayEntriesNeededOut
		);

#if !COREBUILD
		public CMSampleTimingInfo [] GetSampleTimingInfo ()
		{
			OSStatus status;
			return GetSampleTimingInfo (out status);
		}

		public unsafe CMSampleTimingInfo [] GetSampleTimingInfo (out OSStatus status) {
			int count;

			status = 0;

			if (handle == IntPtr.Zero)
				return null;

			if ((status = CMSampleBufferGetSampleTimingInfoArray (handle, 0, null, out count)) != 0)
				return null;

			CMSampleTimingInfo [] pInfo = new CMSampleTimingInfo [count];

			if (count == 0)
				return pInfo;

			fixed (CMSampleTimingInfo* info = pInfo)
				if ((status = CMSampleBufferGetSampleTimingInfoArray (handle, count, info, out count)) != 0)
					return null;

			return pInfo;
		}

		static string OSStatusToString (OSStatus status)
		{
			return new NSError (NSError.OsStatusErrorDomain, status).LocalizedDescription;
		}
#endif

		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMSampleBufferGetTotalSampleSize (IntPtr handle);
		
		public uint TotalSampleSize
		{
			get
			{
				return CMSampleBufferGetTotalSampleSize (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferGetTypeID ();
		
		public static int GetTypeID ()
		{
			return CMSampleBufferGetTypeID ();
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMSampleBufferInvalidate (IntPtr handle);
		
		public int /*CMSampleBufferError*/ Invalidate()
		{
			return (int)CMSampleBufferInvalidate (handle);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static bool CMSampleBufferIsValid (IntPtr handle);
		
		public bool IsValid
		{
			get
			{
				return CMSampleBufferIsValid (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMSampleBufferMakeDataReady (IntPtr handle);
		
		public int /*CMSampleBufferError*/ MakeDataReady ()
		{
			return (int)CMSampleBufferMakeDataReady (handle);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMSampleBufferSetDataBuffer (IntPtr handle, IntPtr dataBufferHandle);
		
		public int /*CMSampleBufferError*/ SetDataBuffer (CMBlockBuffer dataBuffer)
		{
			var dataBufferHandle = IntPtr.Zero;
			if (dataBuffer != null)
			{
				dataBufferHandle = dataBuffer.handle;
			}
			return (int)CMSampleBufferSetDataBuffer (handle, dataBufferHandle);
		}
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferSetDataBufferFromAudioBufferList (
		   CMSampleBufferRef sbuf,
		   CFAllocatorRef bbufStructAllocator,
		   CFAllocatorRef bbufMemoryAllocator,
		   uint32_t flags,
		   const AudioBufferList *bufferList
		);*/
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMSampleBufferSetDataReady (IntPtr handle);
		
		public int/*CMSampleBufferError*/ SetDataReady ()
		{
			return (int)CMSampleBufferSetDataReady (handle);
		}
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferSetInvalidateCallback (
		   CMSampleBufferRef sbuf,
		   CMSampleBufferInvalidateCallback invalidateCallback,
		   uint64_t invalidateRefCon
		);*/
				
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSampleBufferError CMSampleBufferTrackDataReadiness (IntPtr handle, IntPtr handleToTrack);
		
		public int/*CMSampleBufferError*/ TrackDataReadiness (CMSampleBuffer bufferToTrack)
		{
			var handleToTrack = IntPtr.Zero;
			if (bufferToTrack != null) {
				handleToTrack = bufferToTrack.handle;
			}
			return (int)CMSampleBufferTrackDataReadiness (handle, handleToTrack);
		}

	}

#if !COREBUILD
	public class CMSampleBufferAttachmentSettings : DictionaryContainer
	{
		static class Selectors
		{
			public static readonly NSString NotSync;
			public static readonly NSString PartialSync;
			public static readonly NSString HasRedundantCoding;
			public static readonly NSString IsDependedOnByOthers;
			public static readonly NSString DependsOnOthers;
			public static readonly NSString EarlierDisplayTimesAllowed;
			public static readonly NSString DisplayImmediately;
			public static readonly NSString DoNotDisplay;
			public static readonly NSString ResetDecoderBeforeDecoding;
			public static readonly NSString DrainAfterDecoding;
			public static readonly NSString PostNotificationWhenConsumed;
			public static readonly NSString ResumeOutput;
			public static readonly NSString TransitionID;
			public static readonly NSString TrimDurationAtStart;
			public static readonly NSString TrimDurationAtEnd;
			public static readonly NSString SpeedMultiplier;
			public static readonly NSString Reverse;
			public static readonly NSString FillDiscontinuitiesWithSilence;
			public static readonly NSString EmptyMedia;
			public static readonly NSString PermanentEmptyMedia;
			public static readonly NSString DisplayEmptyMediaImmediately;
			public static readonly NSString EndsPreviousSampleDuration;
			public static readonly NSString SampleReferenceURL;
			public static readonly NSString SampleReferenceByteOffset;
			public static readonly NSString GradualDecoderRefresh;

#if !MONOMAC
			// Since 6,0
			public static readonly NSString DroppedFrameReason;
#endif

			static Selectors ()
			{
				var handle = Dlfcn.dlopen (Constants.CoreMediaLibrary, 0);
				if (handle == IntPtr.Zero)
					return;
				try {
					NotSync    	= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_NotSync");
					PartialSync = Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_PartialSync");
					HasRedundantCoding    		= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_HasRedundantCoding");
					IsDependedOnByOthers		= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_IsDependedOnByOthers");		
					DependsOnOthers				= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_DependsOnOthers");										
					EarlierDisplayTimesAllowed	= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_EarlierDisplayTimesAllowed");
					DisplayImmediately			= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_DisplayImmediately");
					DoNotDisplay				= Dlfcn.GetStringConstant (handle, "kCMSampleAttachmentKey_DoNotDisplay");
					ResetDecoderBeforeDecoding	= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_ResetDecoderBeforeDecoding");
					DrainAfterDecoding			= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_DrainAfterDecoding");
					PostNotificationWhenConsumed	= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_PostNotificationWhenConsumed");
					ResumeOutput				= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_ResumeOutput");
					TransitionID				= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_TransitionID");
					TrimDurationAtStart			= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_TrimDurationAtStart");
					TrimDurationAtEnd			= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_TrimDurationAtEnd");
					SpeedMultiplier				= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_SpeedMultiplier");
					Reverse						= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_Reverse");
					FillDiscontinuitiesWithSilence	= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_FillDiscontinuitiesWithSilence");
					EmptyMedia					= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_EmptyMedia");
					PermanentEmptyMedia			= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_PermanentEmptyMedia");

					DisplayEmptyMediaImmediately	= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_DisplayEmptyMediaImmediately");
					EndsPreviousSampleDuration	= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_EndsPreviousSampleDuration");
					SampleReferenceURL			= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_SampleReferenceURL");
					SampleReferenceByteOffset	= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_SampleReferenceByteOffset");
					GradualDecoderRefresh		= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_GradualDecoderRefresh");
#if !MONOMAC
					var version = new Version (UIDevice.CurrentDevice.SystemVersion);
					if (version.Major >= 6) {
						DroppedFrameReason		= Dlfcn.GetStringConstant (handle, "kCMSampleBufferAttachmentKey_DroppedFrameReason");
					}
#endif
				} finally {
					Dlfcn.dlclose (handle);
				}
			}
		}

		internal CMSampleBufferAttachmentSettings (NSMutableDictionary dictionary)
			: base (dictionary)
		{
		}

		public bool? NotSync {
			get {
				return GetBoolValue (Selectors.NotSync);
			}
			set {
				SetBooleanValue (Selectors.NotSync, value);
			}
		}

		public bool? PartialSync {
			get {
				return GetBoolValue (Selectors.PartialSync);
			}
			set {
				SetBooleanValue (Selectors.PartialSync, value);
			}
		}

		public bool? RedundantCoding {
			get {
				return GetBoolValue (Selectors.HasRedundantCoding);
			}
			set {
				SetBooleanValue (Selectors.HasRedundantCoding, value);
			}
		}

		public bool? DependedOnByOthers {
			get {
				return GetBoolValue (Selectors.IsDependedOnByOthers);
			}
			set {
				SetBooleanValue (Selectors.IsDependedOnByOthers, value);
			}
		}

		public bool? DependsOnOthers {
			get {
				return GetBoolValue (Selectors.DependsOnOthers);
			}
			set {
				SetBooleanValue (Selectors.DependsOnOthers, value);
			}
		}

		public bool? EarlierDisplayTimesAllowed {
			get {
				return GetBoolValue (Selectors.EarlierDisplayTimesAllowed);
			}
			set {
				SetBooleanValue (Selectors.EarlierDisplayTimesAllowed, value);
			}
		}

		public bool? DisplayImmediately {
			get {
				return GetBoolValue (Selectors.DisplayImmediately);
			}
			set {
				SetBooleanValue (Selectors.DisplayImmediately, value);
			}
		}

		public bool? DoNotDisplay {
			get {
				return GetBoolValue (Selectors.DoNotDisplay);
			}
			set {
				SetBooleanValue (Selectors.DoNotDisplay, value);
			}
		}

		public bool? ResetDecoderBeforeDecoding {
			get {
				return GetBoolValue (Selectors.ResetDecoderBeforeDecoding);
			}
			set {
				SetBooleanValue (Selectors.ResetDecoderBeforeDecoding, value);
			}
		}

		public bool? DrainAfterDecoding {
			get {
				return GetBoolValue (Selectors.DrainAfterDecoding);
			}
			set {
				SetBooleanValue (Selectors.DrainAfterDecoding, value);
			}
		}

		public bool? Reverse {
			get {
				return GetBoolValue (Selectors.Reverse);
			}
			set {
				SetBooleanValue (Selectors.Reverse, value);
			}
		}

		public bool? FillDiscontinuitiesWithSilence {
			get {
				return GetBoolValue (Selectors.FillDiscontinuitiesWithSilence);
			}
			set {
				SetBooleanValue (Selectors.FillDiscontinuitiesWithSilence, value);
			}
		}

		public bool? EmptyMedia {
			get {
				return GetBoolValue (Selectors.EmptyMedia);
			}
			set {
				SetBooleanValue (Selectors.EmptyMedia, value);
			}
		}

		public bool? PermanentEmptyMedia {
			get {
				return GetBoolValue (Selectors.PermanentEmptyMedia);
			}
			set {
				SetBooleanValue (Selectors.PermanentEmptyMedia, value);
			}
		}

		public bool? DisplayEmptyMediaImmediately {
			get {
				return GetBoolValue (Selectors.DisplayEmptyMediaImmediately);
			}
			set {
				SetBooleanValue (Selectors.DisplayEmptyMediaImmediately, value);
			}
		}

		public bool? EndsPreviousSampleDuration {
			get {
				return GetBoolValue (Selectors.EndsPreviousSampleDuration);
			}
			set {
				SetBooleanValue (Selectors.EndsPreviousSampleDuration, value);
			}
		}

#if !MONOMAC
		[Since (6,0)]
		public string DroppedFrameReason {
			get {
				return GetStringValue (Selectors.DroppedFrameReason);
			}
		}
#endif
	
		// TODO: Implement remaining selector properties
		// PostNotificationWhenConsumed
		// ResumeOutput
		// TransitionID
		// TrimDurationAtStart
		// TrimDurationAtEnd
		// SpeedMultiplier
		// SampleReferenceURL
		// SampleReferenceByteOffset
		// GradualDecoderRefresh
	}
#endif
}
