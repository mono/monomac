//
// AudioUnit.cs: AudioUnit wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;

namespace MonoMac.AudioUnit
{
	public enum AudioUnitStatus {
		NoError = 0,
		OK = NoError,
		ParameterError = -50,
		InvalidProperty = -10879,
		InvalidParameter = -10878,
		InvalidElement = -10877,
		NoConnection = -10876,
		FailedInitialization = -10875,
		TooManyFramesToProcess = -10874,
		InvalidFile = -10871,
		FormatNotSupported = -10868,
		Uninitialized = -10867,
		InvalidScope = -10866,
		PropertyNotWritable = -10865,
		CannotDoInCurrentContext = -10863,
		InvalidPropertyValue = -10851,
		PropertyNotInUse = -10850,
		Initialized = -10849,
		InvalidOfflineRender = -10848,
		Unauthorized = -10847,
	}

	public enum AudioCodecManufacturer : uint
	{
		AppleSoftware	= 0x6170706c,	// 'appl'
		AppleHardware	= 0x61706877,	// 'aphw'
	}
	
	public class AudioUnitException : Exception {
		static string Lookup (int k)
		{
			switch ((AudioUnitStatus)k)
			{
			case AudioUnitStatus.InvalidProperty:
				return "Invalid Property";
				
			case AudioUnitStatus.InvalidParameter :
				return "Invalid Parameter";
				
			case AudioUnitStatus.InvalidElement :
				return "Invalid Element";
				
			case AudioUnitStatus.NoConnection :
				return "No Connection";
				
			case AudioUnitStatus.FailedInitialization :
				return "Failed Initialization";
				
			case AudioUnitStatus.TooManyFramesToProcess :
				return "Too Many Frames To Process";
				
			case AudioUnitStatus.InvalidFile :
				return "Invalid File";
				
			case AudioUnitStatus.FormatNotSupported :
				return "Format Not Supported";
				
			case AudioUnitStatus.Uninitialized :
				return "Uninitialized";
				
			case AudioUnitStatus.InvalidScope :
				return "Invalid Scope";
				
			case AudioUnitStatus.PropertyNotWritable :
				return "Property Not Writable";
				
			case AudioUnitStatus.CannotDoInCurrentContext :
				return "Cannot Do In Current Context";
				
			case AudioUnitStatus.InvalidPropertyValue :
				return "Invalid Property Value";
				
			case AudioUnitStatus.PropertyNotInUse :
				return "Property Not In Use";
				
			case AudioUnitStatus.Initialized :
				return "Initialized";
				
			case AudioUnitStatus.InvalidOfflineRender :
				return "Invalid Offline Render";
				
			case AudioUnitStatus.Unauthorized :
				return "Unauthorized";
				
			}
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}
		
		internal AudioUnitException (int k) : base (Lookup (k))
		{
		}
	}

	public delegate AudioUnitStatus RenderDelegate (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioBuffers data);

	delegate AudioUnitStatus RenderCallbackShared (IntPtr clientData, ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, IntPtr data);

	struct AURenderCallbackStruct
	{
		public RenderCallbackShared Proc;
		public IntPtr ProcRefCon; 
	}
	
	public class AudioUnit : IDisposable, MonoMac.ObjCRuntime.INativeObject
	{
		static readonly RenderCallbackShared CreateRenderCallback = RenderCallbackImpl;

		GCHandle gcHandle;
		IntPtr handle;
		bool _isPlaying;

		RenderDelegate render;

		internal AudioUnit (IntPtr ptr)
		{
			handle = ptr;
			gcHandle = GCHandle.Alloc(this);
		}
		
		public AudioUnit (AudioComponent component)
		{
			if (component == null)
				throw new ArgumentNullException ("component");
			if (component.Handle == IntPtr.Zero)
				throw new ObjectDisposedException ("component");
			
			int err = AudioComponentInstanceNew (component.handle, out handle);
			if (err != 0)
				throw new AudioUnitException (err);
			
			gcHandle = GCHandle.Alloc(this);

#pragma warning disable 612
			BrokenSetRender ();
#pragma warning restore 612
		}

		[Obsolete]
		void BrokenSetRender ()
		{
			var callbackStruct = new AURenderCallbackStrct();
			callbackStruct.inputProc = renderCallback; // setting callback function            
			callbackStruct.inputProcRefCon = GCHandle.ToIntPtr(gcHandle); // a pointer that passed to the renderCallback (IntPtr inRefCon) 
			var err = AudioUnitSetProperty(handle,
						   AudioUnitPropertyIDType.SetRenderCallback,
						   AudioUnitScopeType.Input,
						   0, // 0 == speaker                
						   callbackStruct,
						   (uint)Marshal.SizeOf(callbackStruct));
			if (err != 0)
				throw new AudioUnitException (err);
		}

		public AudioComponent Component {
			get {
				return new AudioComponent (AudioComponentInstanceGetComponent (handle));
			}
		}

		public IntPtr Handle {
			get {
				return handle;
			}
		}

		[Obsolete ("Use SetRenderCallback")]
		public event EventHandler<AudioUnitEventArgs> RenderCallback;

		public bool IsPlaying { get { return _isPlaying; } }
		
		[Obsolete]
		// callback funtion should be static method and be attatched a MonoPInvokeCallback attribute.        
		[MonoMac.MonoPInvokeCallback(typeof(AURenderCallback))]
		static int renderCallback(IntPtr inRefCon, ref AudioUnitRenderActionFlags _ioActionFlags,
					  ref AudioTimeStamp _inTimeStamp,
					  int _inBusNumber,
					  int _inNumberFrames,
					  AudioBufferList _ioData)
		{
			// getting audiounit instance
			var handler = GCHandle.FromIntPtr(inRefCon);
			var inst = (AudioUnit)handler.Target;
			
			// evoke event handler with an argument
			if (inst.RenderCallback != null)  { 
				var args = new AudioUnitEventArgs(
					_ioActionFlags,
					_inTimeStamp,
					_inBusNumber,
					_inNumberFrames,
					_ioData);
				inst.RenderCallback(inst, args);
			}
			
			return 0; // noerror
		}

		public void SetAudioFormat(MonoMac.AudioToolbox.AudioStreamBasicDescription audioFormat, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			int err = AudioUnitSetProperty(handle,
						       AudioUnitPropertyIDType.StreamFormat,
						       scope,
						       audioUnitElement, 
						       ref audioFormat,
						       (uint)Marshal.SizeOf(audioFormat));
			if (err != 0)
				throw new AudioUnitException (err);
		}

		public AudioStreamBasicDescription GetAudioFormat(AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			MonoMac.AudioToolbox.AudioStreamBasicDescription audioFormat = new AudioStreamBasicDescription();
			uint size = (uint)Marshal.SizeOf(audioFormat);
			
			int err = AudioUnitGetProperty(handle,
						       AudioUnitPropertyIDType.StreamFormat,
						       scope,
						       audioUnitElement,
						       ref audioFormat,
						       ref size);
			if (err != 0)
				throw new AudioUnitException (err);
			
			return audioFormat;
		}
		
		public AudioUnitStatus SetEnableIO (bool enableIO, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{                                   
			uint flag = enableIO ? (uint)1 : 0;
			return AudioUnitSetProperty (handle, AudioUnitPropertyIDType.EnableIO, scope, audioUnitElement, ref flag, sizeof (uint));
		}

		public AudioUnitStatus SetMaximumFramesPerSlice (uint value, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			return AudioUnitSetProperty (handle, AudioUnitPropertyIDType.MaximumFramesPerSlice, scope, audioUnitElement, ref value, sizeof (uint));
		}

		#region SetRenderCallback

		public AudioUnitStatus SetRenderCallback (RenderDelegate renderDelegate, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			var cb = new AURenderCallbackStruct ();
			cb.Proc = CreateRenderCallback;
			cb.ProcRefCon = GCHandle.ToIntPtr (gcHandle);

			this.render = renderDelegate;

			return AudioUnitSetProperty (handle, AudioUnitPropertyIDType.SetRenderCallback, scope, audioUnitElement, ref cb, Marshal.SizeOf (cb));
		}

		[MonoPInvokeCallback (typeof (RenderCallbackShared))]
		static AudioUnitStatus RenderCallbackImpl (IntPtr clientData, ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, IntPtr data)
		{
			GCHandle gch = GCHandle.FromIntPtr (clientData);
			var au = (AudioUnit) gch.Target;

			return au.render (actionFlags, timeStamp, busNumber, numberFrames, new AudioBuffers (data));
		}

		#endregion

		public int Initialize ()
		{
			return AudioUnitInitialize(handle);
		}

		public int Uninitialize ()
		{
			return AudioUnitUninitialize (handle);
		}

		public void Start()
		{
			if (! _isPlaying) {
				AudioOutputUnitStart(handle);
				_isPlaying = true;
			}
		}
		
		public void Stop()
		{
			if (_isPlaying) {
				AudioOutputUnitStop(handle);
				_isPlaying = false;
			}
		}

		#region Render

		public AudioUnitStatus Render (ref AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioBuffers data)
		{
			return AudioUnitRender (handle, ref actionFlags, ref timeStamp, busNumber, numberFrames, (IntPtr) data);
		}

		[Obsolete]
		public void Render(AudioUnitRenderActionFlags flags,
				   AudioTimeStamp timeStamp,
				   int outputBusnumber,
				   int numberFrames, AudioBufferList data)
		{
			int err = AudioUnitRender(handle,
						  ref flags,
						  ref timeStamp,
						  outputBusnumber,
						  numberFrames,
						  data);
			if (err != 0)
				throw new AudioUnitException (err);
		}

		[Obsolete]
		public AudioUnitStatus TryRender(AudioUnitRenderActionFlags flags,
						AudioTimeStamp timeStamp,
						int outputBusnumber,
						int numberFrames, AudioBufferList data)
		{
			return (AudioUnitStatus) AudioUnitRender(handle,
								ref flags,
								ref timeStamp,
								outputBusnumber,
								numberFrames,
								data);
		}

		#endregion

		public AudioUnitStatus SetParameter (AudioUnitParameterType type, float value, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			return AudioUnitSetParameter (handle, type, scope, audioUnitElement, value, 0);
		}
		
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioComponentInstanceDispose(IntPtr inInstance);

		public void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				Stop ();
				AudioUnitUninitialize (handle);
				AudioComponentInstanceDispose (handle);
				gcHandle.Free();
				handle = IntPtr.Zero;
			}
		}

		[Obsolete]
		internal delegate int AURenderCallback(IntPtr inRefCon,
					      ref AudioUnitRenderActionFlags ioActionFlags,
					      ref AudioTimeStamp inTimeStamp,
					      int inBusNumber,
					      int inNumberFrames,
					      AudioBufferList ioData);
		
		[Obsolete]
		[StructLayout(LayoutKind.Sequential)]
		class AURenderCallbackStrct {
			public AURenderCallback inputProc;
			public IntPtr inputProcRefCon;
			
			public AURenderCallbackStrct() { }
		}    

		[DllImport(MonoMac.Constants.AudioUnitLibrary, EntryPoint = "AudioComponentInstanceNew")]
		static extern int AudioComponentInstanceNew(IntPtr inComponent, out IntPtr inDesc);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern IntPtr AudioComponentInstanceGetComponent (IntPtr inComponent);
		
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitInitialize(IntPtr inUnit);
		
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitUninitialize(IntPtr inUnit);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioOutputUnitStart(IntPtr ci);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioOutputUnitStop(IntPtr ci);

		[Obsolete]
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitRender(IntPtr inUnit,
						  ref AudioUnitRenderActionFlags ioActionFlags,
						  ref AudioTimeStamp inTimeStamp,
						  int inOutputBusNumber,
						  int inNumberFrames,
						  AudioBufferList ioData);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitRender(IntPtr inUnit, ref AudioUnitRenderActionFlags ioActionFlags, ref AudioTimeStamp inTimeStamp,
						  uint inOutputBusNumber, uint inNumberFrames, IntPtr ioData);


		[Obsolete]
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitSetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       AURenderCallbackStrct inData,
						       uint inDataSize);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitSetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref MonoMac.AudioToolbox.AudioStreamBasicDescription inData,
						       uint inDataSize);
        
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
						       ref uint inData, uint inDataSize);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
						       ref AURenderCallbackStruct inData, int inDataSize);

        
		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitGetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref MonoMac.AudioToolbox.AudioStreamBasicDescription outData,
						       ref uint ioDataSize);

		[DllImport(MonoMac.Constants.AudioUnitLibrary)]
		static extern int AudioUnitGetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref uint flag,
						       ref uint ioDataSize
			);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitSetParameter (IntPtr inUnit, AudioUnitParameterType inID, AudioUnitScopeType inScope,
			uint inElement, float inValue, uint inBufferOffsetInFrames);
	}

	public enum AudioUnitPropertyIDType {
		ClassInfo = 0,
		MakeConnection = 1,
		SampleRate = 2,
		ParameterList = 3,
		ParameterInfo = 4,
		CPULoad = 6,
		StreamFormat = 8,
		ElementCount = 11,
		Latency = 12,
		SupportedNumChannels = 13,
		MaximumFramesPerSlice = 14,
		ParameterValueStrings = 16,
		AudioChannelLayout = 19,
		TailTime = 20,
		BypassEffect = 21,
		LastRenderError = 22,
		SetRenderCallback = 23,
		FactoryPresets = 24,
		RenderQuality = 26,
		HostCallbacks = 27,
		InPlaceProcessing = 29,
		ElementName = 30,
		SupportedChannelLayoutTags = 32,
		PresentPreset = 36,
		OfflineRender = 37,
		DependentParameters = 45,
		InputSampleInOutput = 49,
		ShouldAllocateBuffer = 51,
		ParameterHistoryInfo = 53,
		Nickname = 54,
#if MONOMAC
		FastDispatch				= 5,
		SetExternalBuffer			= 15,
		GetUIComponentList			= 18,
		ContextName					= 25,
		CocoaUI						= 31,
		ParameterIDName				= 34,
		ParameterClumpName			= 35,
		ParameterStringFromValue	= 33,
		ParameterValueFromString	= 38,
		IconLocation				= 39,
		PresentationLatency			= 40,
		AUHostIdentifier			= 46,
		MIDIOutputCallbackInfo		= 47,
		MIDIOutputCallback			= 48,
		ClassInfoFromDocument		= 50,
		FrequencyResponse			= 52,
#endif
		// Output Unit
		CurrentDevice				= 2000,
		IsRunning					= 2001,
	    ChannelMap					= 2002, // this will also work with AUConverter
	    EnableIO					= 2003,
	    StartTime					= 2004,
	    SetInputCallback			= 2005,
	    HasIO						= 2006,
	    StartTimestampsAtZero		= 2007,	// this will also work with AUConverter

	    // TODO: Many more are missing but maybe we shold split it by AudioComponentType 
	}

	public enum AudioUnitParameterType
	{
		// Reverb applicable to the 3DMixer
		ReverbFilterFrequency				= 14,
		ReverbFilterBandwidth				= 15,
		ReverbFilterGain					= 16,

		// AUMultiChannelMixer
		MultiChannelMixerVolume				= 0,
		MultiChannelMixerEnable				= 1,
		MultiChannelMixerPan				= 2,

		// AUMatrixMixer unit
		MatrixMixerVolume					= 0,
		MatrixMixerEnable					= 1,
	
		// AudioDeviceOutput, DefaultOutputUnit, and SystemOutputUnit units
		HALOutputVolume 					= 14, 

		// AUTimePitch, AUTimePitch (offline), AUPitch units
		TimePitchRate						= 0,
#if MONOMAC
		TimePitchPitch						= 1,
		TimePitchEffectBlend				= 2,
#endif

		// AUNewTimePitch
		NewTimePitchRate					= 0,
		NewTimePitchPitch					= 1,
		NewTimePitchOverlap					= 4,
		NewTimePitchEnablePeakLocking		= 6,

		// AUSampler unit
		AUSamplerGain						= 900,
		AUSamplerCoarseTuning				= 901,
		AUSamplerFineTuning					= 902,
		AUSamplerPan						= 903,

		// AUBandpass
		BandpassCenterFrequency 			= 0,
		BandpassBandwidth	 				= 1,

		// AUHipass
		HipassCutoffFrequency 				= 0,
		HipassResonance						= 1,

		// AULowpass
		LowPassCutoffFrequency 				= 0,
		LowPassResonance 					= 1,

		// AUHighShelfFilter
		HighShelfCutOffFrequency 			= 0,
		HighShelfGain 						= 1,

		// AULowShelfFilter
		AULowShelfCutoffFrequency			= 0,
		AULowShelfGain						= 1,

		// AUDCFilter
		AUDCFilterDecayTime					= 0,		

		// AUParametricEQ
		ParametricEQCenterFreq				= 0,
		ParametricEQQ						= 1,
		ParametricEQGain					= 2,

		// AUPeakLimiter
		LimiterAttackTime		 			= 0,
		LimiterDecayTime 					= 1,
		LimiterPreGain 						= 2,

		// AUDynamicsProcessor
		DynamicsProcessorThreshold 			= 0,
		DynamicsProcessorHeadRoom	 		= 1,
		DynamicsProcessorExpansionRatio		= 2,
		DynamicsProcessorExpansionThreshold	= 3,
		DynamicsProcessorAttackTime			= 4,
		DynamicsProcessorReleaseTime 		= 5,
		DynamicsProcessorMasterGain			= 6,
		DynamicsProcessorCompressionAmount 	= 1000,
		DynamicsProcessorInputAmplitude		= 2000,
		DynamicsProcessorOutputAmplitude 	= 3000,

		// AUVarispeed
		VarispeedPlaybackRate				= 0,
		VarispeedPlaybackCents				= 1,

		// Distortion unit 
		DistortionDelay						= 0,
		DistortionDecay						= 1,
		DistortionDelayMix					= 2,
		DistortionDecimation				= 3,
		DistortionRounding					= 4,
		DistortionDecimationMix				= 5,
		DistortionLinearTerm				= 6,  
		DistortionSquaredTerm				= 7,	
		DistortionCubicTerm					= 8,  
		DistortionPolynomialMix				= 9,
		DistortionRingModFreq1				= 10,
		DistortionRingModFreq2				= 11,
		DistortionRingModBalance			= 12,
		DistortionRingModMix				= 13,
		DistortionSoftClipGain				= 14,
		DistortionFinalMix					= 15,

		// AUDelay
		DelayWetDryMix 						= 0,
		DelayTime							= 1,
		DelayFeedback 						= 2,
		DelayLopassCutoff	 				= 3,

#if MONOMAC
		// TODO
#else
		// AUNBandEQ
		AUNBandEQGlobalGain					= 0,
		AUNBandEQBypassBand					= 1000,
		AUNBandEQFilterType					= 2000,
		AUNBandEQFrequency					= 3000,
		AUNBandEQGain						= 4000,
		AUNBandEQBandwidth					= 5000,

		// iOS reverb
		Reverb2DryWetMix					= 0,
		Reverb2Gain							= 1,
		Reverb2MinDelayTime					= 2,
		Reverb2MaxDelayTime					= 3,
		Reverb2DecayTimeAt0Hz				= 4,
		Reverb2DecayTimeAtNyquist			= 5,
		Reverb2RandomizeReflections			= 6,
#endif
	}
        
    public enum AudioUnitScopeType {
		Global		= 0,
		Input		= 1,
		Output		= 2,
		Group		= 3,
		Part		= 4,
		Note		= 5,
		Layer		= 6,
		LayerItem	= 7
    }

	[Flags]
    public enum AudioUnitRenderActionFlags {
		PreRender = (1 << 2),
		PostRender = (1 << 3),
		OutputIsSilence = (1 << 4),
		OfflinePreflight = (1 << 5),
		OfflineRender = (1 << 6),
		OfflineComplete = (1 << 7),
		PostRenderError = (1 << 8),
		DoNotCheckRenderArgs = (1 << 9)
    };
}
