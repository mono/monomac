//
// AudioConverter.cs: AudioConverter wrapper class
//
// Authors:
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2013 Xamarin Inc.
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AudioToolbox
{
	public enum AudioConverterError
	{
		None = 0,
		FormatNotSupported			= 0x666d743f, // 'fmt?'
		OperationNotSupported		= 0x6f703f3f, // 'op??'
		PropertyNotSupported		= 0x70726f70, // 'prop'
		InvalidInputSize			= 0x696e737a, // 'insz'
		InvalidOutputSize			= 0x6f74737a, // 'otsz'
		UnspecifiedError			= 0x77686174, // 'what'
		BadPropertySizeError		= 0x2173697a, // '!siz'
		RequiresPacketDescriptionsError = 0x21706b64, // '!pkd'
		InputSampleRateOutOfRange	= 0x21697372, // '!isr'
		OutputSampleRateOutOfRange	= 0x216f7372, // '!osr'
		HardwareInUse				= 0x68776975, // 'hwiu'
	  	NoHardwarePermission		= 0x7065726d, // 'perm'
	
		// TODO: Not documented
		// '!dat'
	}

	public enum AudioConverterSampleRateConverterComplexity
	{
		Linear				= 0x6c696e65, // 'line'
		Normal				= 0x6e6f726d, // 'norm'
		Mastering			= 0x62617473, // 'bats'
	}	

	public enum AudioConverterQuality
	{
		Max					= 0x7F,
		High				= 0x60,
		Medium				= 0x40,
		Low					= 0x20,
		Min					= 0
	}

	public enum AudioConverterPrimeMethod
	{
		Pre			= 0,
		Normal		= 1,
		None		= 2
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct AudioConverterPrimeInfo
	{
		public int LeadingFrames;
		public int TrailingFrames;
	}

	public delegate AudioConverterError AudioConverterComplexInputData (ref int numberDataPackets, AudioBuffers data,
		ref AudioStreamPacketDescription[] dataPacketDescription);

	public class AudioConverter : IDisposable
	{
		delegate AudioConverterError AudioConverterComplexInputDataShared (IntPtr inAudioConverter, ref int ioNumberDataPackets, IntPtr ioData,
			IntPtr outDataPacketDescription, IntPtr inUserData);

		IntPtr handle;
		readonly bool owns;
		static readonly AudioConverterComplexInputDataShared ComplexInputDataShared = FillComplexBufferShared;

		public event AudioConverterComplexInputData InputData;

		private AudioConverter (IntPtr handle)
			: this (handle, false)
		{
		}

		internal AudioConverter (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("address");

			this.handle = handle;
			this.owns = owns;
		}

		public uint MinimumInputBufferSize {
			get {
				return GetUIntProperty (AudioConverterPropertyID.MinimumInputBufferSize);
			}
		}

		public uint MinimumOutputBufferSize {
			get {
				return GetUIntProperty (AudioConverterPropertyID.MinimumOutputBufferSize);
			}			
		}

		public uint MaximumInputPacketSize {
			get {
				return GetUIntProperty (AudioConverterPropertyID.MaximumInputPacketSize);
			}
		}

		public uint MaximumOutputPacketSize {
			get {
				return GetUIntProperty (AudioConverterPropertyID.MaximumOutputPacketSize);
			}
		}

		public uint CalculateInputBufferSize {
			get {
				return GetUIntProperty (AudioConverterPropertyID.CalculateInputBufferSize);
			}
		}

		public uint CalculateOutputBufferSize {
			get {
				return GetUIntProperty (AudioConverterPropertyID.CalculateOutputBufferSize);
			}
		}

		public double SampleRateConverterInitialPhase {
			get {
				return GetDoubleProperty (AudioConverterPropertyID.SampleRateConverterInitialPhase);
			}
			set {
				SetProperty (AudioConverterPropertyID.SampleRateConverterInitialPhase, value);
			}
		}

		public AudioConverterSampleRateConverterComplexity SampleRateConverterComplexity {
			get {
				return (AudioConverterSampleRateConverterComplexity) GetUIntProperty (AudioConverterPropertyID.SampleRateConverterComplexity);
			}
		}

		public AudioConverterQuality SampleRateConverterQuality {
			get {
				return (AudioConverterQuality) GetUIntProperty (AudioConverterPropertyID.SampleRateConverterQuality);
			}
		}

		public AudioConverterQuality CodecQuality {
			get {
				return (AudioConverterQuality) GetUIntProperty (AudioConverterPropertyID.CodecQuality);
			}
			set {
				SetProperty (AudioConverterPropertyID.CodecQuality, (uint) value);
			}
		}

		public AudioConverterPrimeMethod PrimeMethod {
			get {
				return (AudioConverterPrimeMethod) GetUIntProperty (AudioConverterPropertyID.PrimeMethod);
			}
			set {
				SetProperty (AudioConverterPropertyID.PrimeMethod, (uint) value);
			}
		}

		public unsafe AudioConverterPrimeInfo PrimeInfo {
			get {
				AudioConverterPrimeInfo value;
				var size = sizeof (AudioConverterPrimeInfo);
				var res = AudioConverterGetProperty (handle, AudioConverterPropertyID.PrimeInfo, ref size, out value);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());

				return value;				
			}
		}

		public int[] ChannelMap {
			get {
				return GetArray<int> (AudioConverterPropertyID.ChannelMap, sizeof (int));
			}
		}

		public byte[] CompressionMagicCookie {
			get {
				int size;
				bool writable;
				if (AudioConverterGetPropertyInfo (handle, AudioConverterPropertyID.CompressionMagicCookie, out size, out writable) != AudioConverterError.None)
					return null;

				var cookie = new byte [size];
				if (AudioConverterGetProperty (handle, AudioConverterPropertyID.CompressionMagicCookie, ref size, cookie) != AudioConverterError.None)
					return null;

				return cookie;
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");

				var res = AudioConverterSetProperty (handle, AudioConverterPropertyID.CompressionMagicCookie, value.Length, value);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());
			}
		}

		public byte[] DecompressionMagicCookie {
			get {
				int size;
				bool writable;
				if (AudioConverterGetPropertyInfo (handle, AudioConverterPropertyID.DecompressionMagicCookie, out size, out writable) != AudioConverterError.None)
					return null;

				var cookie = new byte [size];
				if (AudioConverterGetProperty (handle, AudioConverterPropertyID.DecompressionMagicCookie, ref size, cookie) != AudioConverterError.None)
					return null;

				return cookie;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");

				var res = AudioConverterSetProperty (handle, AudioConverterPropertyID.DecompressionMagicCookie, value.Length, value);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());
			}
		}

		public uint EncodeBitRate {
			get {
				return GetUIntProperty (AudioConverterPropertyID.EncodeBitRate);
			}
			set {
				SetProperty (AudioConverterPropertyID.EncodeBitRate, value);
			}
		}

		public double EncodeAdjustableSampleRate {
			get {
				return GetDoubleProperty (AudioConverterPropertyID.EncodeAdjustableSampleRate);
			}
			set {
				SetProperty (AudioConverterPropertyID.EncodeAdjustableSampleRate, value);
			}
		}

		public AudioChannelLayout InputChannelLayout {
			get {
				int size;
				bool writable;
				if (AudioConverterGetPropertyInfo (handle, AudioConverterPropertyID.InputChannelLayout, out size, out writable) != AudioConverterError.None)
					return null;

				IntPtr ptr = Marshal.AllocHGlobal (size);
				var res = AudioConverterGetProperty (handle, AudioConverterPropertyID.InputChannelLayout, ref size, ptr);
				var layout = res == AudioConverterError.None ? new AudioChannelLayout (ptr) : null;
				Marshal.FreeHGlobal (ptr);
				return layout;
			}
		}

		public AudioChannelLayout OutputChannelLayout {
			get {
				int size;
				bool writable;
				if (AudioConverterGetPropertyInfo (handle, AudioConverterPropertyID.OutputChannelLayout, out size, out writable) != AudioConverterError.None)
					return null;

				IntPtr ptr = Marshal.AllocHGlobal (size);
				var res = AudioConverterGetProperty (handle, AudioConverterPropertyID.OutputChannelLayout, ref size, ptr);
				var layout = res == AudioConverterError.None ? new AudioChannelLayout (ptr) : null;
				Marshal.FreeHGlobal (ptr);
				return layout;
			}
		}

		public AudioValueRange[] ApplicableEncodeBitRates {
			get {
				return GetAudioValueRange (AudioConverterPropertyID.ApplicableEncodeBitRates);
			}
		}

		public AudioValueRange[] AvailableEncodeBitRates {
			get {
				return GetAudioValueRange (AudioConverterPropertyID.AvailableEncodeBitRates);
			}
		}

		public AudioValueRange[] ApplicableEncodeSampleRates {
			get {
				return GetAudioValueRange (AudioConverterPropertyID.ApplicableEncodeSampleRates);
			}
		}

		public AudioValueRange[] AvailableEncodeSampleRates {
			get {
				return GetAudioValueRange (AudioConverterPropertyID.AvailableEncodeSampleRates);
			}
		}

		public AudioChannelLayoutTag[] AvailableEncodeChannelLayoutTags {
			get {
				return GetArray<AudioChannelLayoutTag> (AudioConverterPropertyID.AvailableEncodeChannelLayoutTags, sizeof (AudioChannelLayoutTag));
			}
		}

		public unsafe AudioStreamBasicDescription CurrentOutputStreamDescription {
			get {
				int size;
				bool writable;
				var res = AudioConverterGetPropertyInfo (handle, AudioConverterPropertyID.CurrentOutputStreamDescription, out size, out writable);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());

				IntPtr ptr = Marshal.AllocHGlobal (size);
				res = AudioConverterGetProperty (handle, AudioConverterPropertyID.CurrentOutputStreamDescription, ref size, ptr);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());

				var asbd = *(AudioStreamBasicDescription *) ptr;
				Marshal.FreeHGlobal (ptr);
				return asbd;
			}
		}

		public unsafe AudioStreamBasicDescription CurrentInputStreamDescription {
			get {
				int size;
				bool writable;
				var res = AudioConverterGetPropertyInfo (handle, AudioConverterPropertyID.CurrentInputStreamDescription, out size, out writable);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());

				IntPtr ptr = Marshal.AllocHGlobal (size);
				res = AudioConverterGetProperty (handle, AudioConverterPropertyID.CurrentInputStreamDescription, ref size, ptr);
				if (res != AudioConverterError.None)
					throw new ArgumentException (res.ToString ());

				var asbd = *(AudioStreamBasicDescription*) ptr;
				Marshal.FreeHGlobal (ptr);
				return asbd;
			}
		}

		public int BitDepthHint {
			get {
				return (int) GetUIntProperty (AudioConverterPropertyID.PropertyBitDepthHint);
			}
			set {
				SetProperty (AudioConverterPropertyID.PropertyBitDepthHint, value);
			}
		}

		public unsafe AudioFormat[] FormatList {
			get {
				return GetArray<AudioFormat> (AudioConverterPropertyID.PropertyFormatList, sizeof (AudioFormat));
			}
		}

#if !MONOMAC
		public bool CanResumeFromInterruption {
			get {
				return GetUIntProperty (AudioConverterPropertyID.CanResumeFromInterruption) != 0;
			}
		}
#endif

		public static AudioConverter Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat)
		{
			AudioConverterError res;
			return Create (sourceFormat, destinationFormat, out res);
		}

		public static AudioConverter Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat, out AudioConverterError error)
		{
			IntPtr ptr = new IntPtr ();
			error = AudioConverterNew (ref sourceFormat, ref destinationFormat, ref ptr);
			if (error != AudioConverterError.None)
				return null;

			return new AudioConverter (ptr, true);
		}

		public static AudioConverter Create (AudioStreamBasicDescription sourceFormat, AudioStreamBasicDescription destinationFormat, AudioClassDescription[] descriptions)
		{
			if (descriptions == null)
				throw new ArgumentNullException ("descriptions");

			IntPtr ptr = new IntPtr ();
			var res = AudioConverterNewSpecific (ref sourceFormat, ref destinationFormat, descriptions.Length, ref descriptions, ref ptr);
			if (res != AudioConverterError.None)
				return null;

			return new AudioConverter (ptr, true);
		}

		public static AudioFormatType[] DecodeFormats {
			get {
				return GetFormats (AudioFormatProperty.DecodeFormatIDs); 
			}
		}

		public static AudioFormatType[] EncodeFormats {
			get {
				return GetFormats (AudioFormatProperty.EncodeFormatIDs); 
			}
		}

		~AudioConverter ()
		{
			Dispose (false);
			GC.SuppressFinalize (this);
		}

		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero) {
				if (owns)
					AudioConverterDispose (handle);

				handle = IntPtr.Zero;
			}
		}

		public AudioConverterError ConvertBuffer (byte[] input, byte[] output)
		{
			if (input == null)
				throw new ArgumentNullException ("input");
			if (output == null)
				throw new ArgumentNullException ("output");

			int outSize = output.Length;
			return AudioConverterConvertBuffer (handle, input.Length, input, ref outSize, output);
		}

		[Since (5,0)]
		public AudioConverterError ConvertComplexBuffer (int numberPCMFrames, AudioBuffers inputData, AudioBuffers outputData)
		{
			if (inputData == null)
				throw new ArgumentNullException ("inputData");
			if (outputData == null)
				throw new ArgumentNullException ("outputData");

			return AudioConverterConvertComplexBuffer (handle, numberPCMFrames, (IntPtr) inputData, (IntPtr) outputData);
		}

		public AudioConverterError FillComplexBuffer (ref int outputDataPacketSize,
					AudioBuffers outputData, AudioStreamPacketDescription[] packetDescription)
		{
			AudioConverterError res;

			var this_handle = GCHandle.Alloc (this);
			var this_ptr = GCHandle.ToIntPtr (this_handle);

			if (packetDescription == null) {
				res = AudioConverterFillComplexBuffer (handle, ComplexInputDataShared, this_ptr, ref outputDataPacketSize, (IntPtr) outputData, IntPtr.Zero);
			} else {
				unsafe {
					fixed (AudioStreamPacketDescription* pdesc = &packetDescription [0]) {
						res = AudioConverterFillComplexBuffer (handle, ComplexInputDataShared, this_ptr, ref outputDataPacketSize, (IntPtr) outputData, (IntPtr) pdesc);
					}
				}
			}

			return res;
		}

		[MonoPInvokeCallback (typeof (AudioConverterComplexInputDataShared))]
		static AudioConverterError FillComplexBufferShared (IntPtr inAudioConverter, ref int ioNumberDataPackets, IntPtr ioData,
			IntPtr outDataPacketDescription, IntPtr inUserData)
		{
			AudioStreamPacketDescription[] data;
			if (outDataPacketDescription == IntPtr.Zero) {
				data = null;
			} else {
				// TODO: AudioStreamPacketDescription
				data = null;
			}

            var handler = GCHandle.FromIntPtr (inUserData);
            var inst = (AudioConverter) handler.Target;

            // evoke event handler with an argument
            if (inst.InputData == null)
				throw new ArgumentNullException ("InputData");

			var res = inst.InputData (ref ioNumberDataPackets, new AudioBuffers (ioData), ref data);
			if (data == null) {
				outDataPacketDescription = IntPtr.Zero;
			} else {
				// TODO: AudioStreamPacketDescription
				throw new NotImplementedException ("outDataPacketDescription is not null");
			}
			return res;
		}

		public AudioConverterError Reset ()
		{
			return AudioConverterReset (handle);
		}

		unsafe static AudioFormatType[] GetFormats (AudioFormatProperty prop)
		{
			int size;
			if (AudioFormatPropertyNative.AudioFormatGetPropertyInfo (prop, 0, IntPtr.Zero, out size) != 0)
				return null;

			var elementSize = sizeof (AudioFormatType);
			var data = new AudioFormatType[size / elementSize];
			fixed (AudioFormatType* ptr = data) {
				var res = AudioFormatPropertyNative.AudioFormatGetProperty (prop, 0, IntPtr.Zero, ref size, (IntPtr)ptr);
				if (res != 0)
					return null;

				Array.Resize (ref data, elementSize);
				return data;
			}
		}

		uint GetUIntProperty (AudioConverterPropertyID propertyID)
		{
			uint value;
			var size = sizeof (uint);
			var res = AudioConverterGetProperty (handle, propertyID, ref size, out value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());

			return value;
		}

		double GetDoubleProperty (AudioConverterPropertyID propertyID)
		{
			double value;
			var size = sizeof (double);
			var res = AudioConverterGetProperty (handle, propertyID, ref size, out value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());

			return value;
		}

		unsafe AudioValueRange[] GetAudioValueRange (AudioConverterPropertyID prop)
		{
			return GetArray<AudioValueRange> (prop, sizeof (AudioValueRange));
		}

		unsafe T[] GetArray<T> (AudioConverterPropertyID prop, int elementSize)
		{
			int size;
			bool writable;
			if (AudioConverterGetPropertyInfo (handle, prop, out size, out writable) != AudioConverterError.None)
				return null;

			var data = new T [size / elementSize];
			var array_handle = GCHandle.Alloc (data, GCHandleType.Pinned);

			try {
				var ptr = array_handle.AddrOfPinnedObject ();
				var res = AudioConverterGetProperty (handle, prop, ref size, ptr);
				if (res != 0)
					return null;

				Array.Resize (ref data, size / elementSize);
				return data;
			} finally {
				array_handle.Free ();
			}
		}		

		void SetProperty (AudioConverterPropertyID propertyID, uint value)
		{
			var res = AudioConverterSetProperty (handle, propertyID, sizeof (uint), ref value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());
		}

		void SetProperty (AudioConverterPropertyID propertyID, int value)
		{
			var res = AudioConverterSetProperty (handle, propertyID, sizeof (int), ref value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());
		}

		void SetProperty (AudioConverterPropertyID propertyID, double value)
		{
			var res = AudioConverterSetProperty (handle, propertyID, sizeof (double), ref value);
			if (res != AudioConverterError.None)
				throw new ArgumentException (res.ToString ());
		}

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
        static extern AudioConverterError AudioConverterNew (ref AudioStreamBasicDescription inSourceFormat, ref AudioStreamBasicDescription inDestinationFormat, ref IntPtr outAudioConverter);		

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]	
		static extern AudioConverterError AudioConverterNewSpecific (ref AudioStreamBasicDescription inSourceFormat, ref AudioStreamBasicDescription inDestinationFormat,
			int inNumberClassDescriptions, ref AudioClassDescription[] inClassDescriptions, ref IntPtr outAudioConverter);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterDispose (IntPtr inAudioConverter);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]	
		static extern AudioConverterError AudioConverterReset (IntPtr inAudioConverter);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterConvertComplexBuffer (IntPtr inAudioConverter, int inNumberPCMFrames,
			IntPtr inInputData, IntPtr outOutputData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			ref int ioPropertyDataSize, out uint outPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			ref int ioPropertyDataSize, out int outPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			ref int ioPropertyDataSize, out double outPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			ref int ioPropertyDataSize, byte[] outPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			ref int ioPropertyDataSize, out AudioConverterPrimeInfo outPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			ref int ioPropertyDataSize, IntPtr outPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterGetPropertyInfo (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			out int outSize, out bool outWritable);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterSetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			int inPropertyDataSize, ref uint inPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterSetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			int inPropertyDataSize, ref int inPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterSetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			int inPropertyDataSize, ref double inPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterSetProperty (IntPtr inAudioConverter, AudioConverterPropertyID inPropertyID,
			int inPropertyDataSize, byte[] inPropertyData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterConvertBuffer (IntPtr inAudioConverter, int inInputDataSize, byte[] inInputData,
			ref int ioOutputDataSize, byte[] outOutputData);

		[DllImport (MonoMac.Constants.AudioToolboxLibrary)]
		static extern AudioConverterError AudioConverterFillComplexBuffer (IntPtr inAudioConverter,
			AudioConverterComplexInputDataShared inInputDataProc, IntPtr inInputDataProcUserData,
			ref int ioOutputDataPacketSize, IntPtr outOutputData,
			IntPtr outPacketDescription);
	}

	enum AudioConverterPropertyID
	{
		MinimumInputBufferSize			= 0x6d696273, // 'mibs'
		MinimumOutputBufferSize			= 0x6d6f6273, // 'mobs'
		// Deprecated
		// MaximumInputBufferSize		= 0x78696273, // 'xibs'
		MaximumInputPacketSize			= 0x78697073, // 'xips'
		MaximumOutputPacketSize			= 0x786f7073, // 'xops'
		CalculateInputBufferSize		= 0x63696273, // 'cibs'
		CalculateOutputBufferSize		= 0x636f6273, // 'cobs'
		
		// TODO: Format specific
		// InputCodecParameters         = 'icdp'
		// OutputCodecParameters        = 'ocdp'

		// Deprecated
    	// SampleRateConverterAlgorithm = 'srci'
		SampleRateConverterComplexity	= 0x73726361, // 'srca'
		SampleRateConverterQuality		= 0x73726371, // 'srcq'
		SampleRateConverterInitialPhase = 0x73726370, // 'srcp'
		CodecQuality					= 0x63647175, // 'cdqu'
		PrimeMethod						= 0x70726d6d, // 'prmm'
		PrimeInfo						= 0x7072696d, // 'prim'
		ChannelMap						= 0x63686d70, // 'chmp'
		DecompressionMagicCookie		= 0x646d6763, // 'dmgc'
		CompressionMagicCookie			= 0x636d6763, // 'cmgc'
		EncodeBitRate					= 0x62726174, // 'brat'
		EncodeAdjustableSampleRate		= 0x616a7372, // 'ajsr'
		InputChannelLayout				= 0x69636c20, // 'icl '
		OutputChannelLayout				= 0x6f636c20, // 'ocl '
		ApplicableEncodeBitRates		= 0x61656272, // 'aebr'
		AvailableEncodeBitRates			= 0x76656272, // 'vebr'
		ApplicableEncodeSampleRates		= 0x61657372, // 'aesr'
		AvailableEncodeSampleRates		= 0x76657372, // 'vesr'
		AvailableEncodeChannelLayoutTags	= 0x6165636c, // 'aecl'
		CurrentOutputStreamDescription	= 0x61636f64, // 'acod'
		CurrentInputStreamDescription	= 0x61636964, // 'acid'
		PropertySettings				= 0x61637073, // 'acps'	// TODO
		PropertyBitDepthHint			= 0x61636264, // 'acbd'
		PropertyFormatList				= 0x666c7374, // 'flst'
		CanResumeFromInterruption		= 0x63726669, // 'crfi'
	}
}
