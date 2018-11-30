//
// AUGraph.cs: AUGraph wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2010 Novell, Inc.
// Copyright 2012 Xamarin Inc
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
using System.Text;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;
using MonoMac.Foundation;
using System.Collections.Generic;

namespace MonoMac.AudioUnit
{
	public enum AUGraphError
	{
		OK = 0,
		NodeNotFound 				= -10860,
		InvalidConnection 			= -10861,
		OutputNodeError				= -10862,
		CannotDoInCurrentContext	= -10863,
		InvalidAudioUnit			= -10864,

		// Values returned & shared with other error enums
		FormatNotSupported			= -10868,
		InvalidElement				= -10877,		
	}

	public class AUGraph: IDisposable 
	{
		readonly GCHandle gcHandle;
		IntPtr handle;

		internal AUGraph (IntPtr ptr)
		{
			handle = ptr;
			
			int err = AUGraphAddRenderNotify(handle, renderCallback, GCHandle.ToIntPtr(gcHandle));
			if (err != 0)
				throw new ArgumentException(String.Format("Error code: {0}", err));

			gcHandle = GCHandle.Alloc (this);
		}

		public event EventHandler<AudioGraphEventArgs> RenderCallback;

		[Advice ("Use Handle property instead")]
		public IntPtr Handler {
			get {
				return handle;
				}
		}

		public IntPtr Handle {
			get {
				return handle;
			}
		}

		public AUGraph ()
		{
			int err = NewAUGraph (ref handle);
			if (err != 0)
				throw new InvalidOperationException(String.Format("Cannot create new AUGraph. Error code:", err));
		}

		public bool IsInitialized {
			get {
				bool b;
				return AUGraphIsInitialized (handle, out b) == AUGraphError.OK && b;
			}
		}

		public bool IsOpen {
			get {
				bool b;
				return AUGraphIsOpen (handle, out b) == AUGraphError.OK && b;
			}
		}

		public bool IsRunning {
			get {
				bool b;
				return AUGraphIsRunning (handle, out b) == AUGraphError.OK && b;
			}
		}
		
		// callback funtion should be static method and be attatched a MonoPInvokeCallback attribute.        
		[MonoMac.MonoPInvokeCallback(typeof(AudioUnit.AURenderCallback))]
		static int renderCallback(IntPtr inRefCon,
					  ref AudioUnitRenderActionFlags _ioActionFlags,
					  ref AudioTimeStamp _inTimeStamp,
					  int _inBusNumber,
					  int _inNumberFrames,
					  AudioBufferList _ioData)
		{
			// getting audiounit instance
			var handler = GCHandle.FromIntPtr(inRefCon);
			var inst = (AUGraph)handler.Target;
			
			// invoke event handler with an argument
			if (inst.RenderCallback != null){
				var args = new AudioGraphEventArgs(
					_ioActionFlags,
					_inTimeStamp,
					_inBusNumber,
					_inNumberFrames,
					_ioData);
				inst.RenderCallback(inst, args);
			}
			
			return 0; // noerror
		}

		public void Open ()
		{ 
			int err = AUGraphOpen (handle);
			if (err != 0)
				throw new InvalidOperationException(String.Format("Cannot open AUGraph. Error code:", err));
		}

		public int TryOpen ()
		{ 
			int err = AUGraphOpen (handle);
			return err;
		}
		
		public int AddNode (AudioComponentDescription description)
		{
			int node;
			var err = AUGraphAddNode (handle, description, out node);
			if (err != 0)
				throw new ArgumentException(String.Format("Error code:", err));
			
			return node;
		}

		public AUGraphError RemoveNode (int node)
		{
			return AUGraphRemoveNode (handle, node);
		}

		public AUGraphError GetCPULoad (out float averageCPULoad)
		{
			return AUGraphGetCPULoad (handle, out averageCPULoad);
		}

		public AUGraphError GetMaxCPULoad (out float maxCPULoad)
		{
			return AUGraphGetMaxCPULoad (handle, out maxCPULoad);
		}

		public AUGraphError GetNode (uint index, out int node)
		{
			return AUGraphGetIndNode (handle, index, out node);
		}

		public AUGraphError GetNodeCount (out int count)
		{
			return AUGraphGetNodeCount (handle, out count);
		}
		
		public AudioUnit GetNodeInfo (int node)
		{
			IntPtr ptr;
			var err = AUGraphNodeInfo(handle, node, IntPtr.Zero, out ptr);
			if (err != 0)
				throw new ArgumentException(String.Format("Error code:{0}", err));

			if (ptr == IntPtr.Zero)
				throw new InvalidOperationException("Can not get object instance");
			
			return new AudioUnit (ptr);
		}

		public AUGraphError GetNumberOfInteractions (out uint interactionsCount)
		{
			return AUGraphGetNumberOfInteractions (handle, out interactionsCount);
		}

		public AUGraphError GetNumberOfInteractions (int node, out uint interactionsCount)
		{
			return AUGraphCountNodeInteractions (handle, node, out interactionsCount);
		}

/*
		// TODO: requires AudioComponentDescription type to be fixed
		public AUGraphError GetNodeInfo (int node, out AudioComponentDescription description, out AudioUnit audioUnit)
		{
			IntPtr au;
			var res = AUGraphNodeInfo (handle, node, out description, out au);
			if (res != AUGraphError.OK) {
				audioUnit = null;
				return res;
			}

			audioUnit = new AudioUnit (au);
			return res;
		}
*/
		public AUGraphError ConnnectNodeInput (int sourceNode, uint sourceOutputNumber, int destNode, uint destInputNumber)
		{
			return AUGraphConnectNodeInput (handle,
							  sourceNode, sourceOutputNumber,                                    
							  destNode, destInputNumber);
		}

		public AUGraphError DisconnectNodeInput (int destNode, uint destInputNumber)
		{
			return AUGraphDisconnectNodeInput (handle, destNode, destInputNumber);
		}

		/*
		// Don't know how to test it yet

		Dictionary<int, RenderDelegate> nodesCallbacks;
		static readonly RenderCallbackShared CreateRenderCallback = RenderCallbackImpl;

		public AUGraphError SetNodeInputCallback (int destNode, uint destInputNumber, RenderDelegate renderDelegate)
		{
			var cb = new AURenderCallbackStruct ();
			cb.Proc = CreateRenderCallback;
			cb.ProcRefCon = GCHandle.ToIntPtr (gcHandle);

			AUGraphError res;
			if (nodesCallbacks == null) {
				nodesCallbacks = new Dictionary<int, RenderDelegate> ();

				res = AUGraphSetNodeInputCallback (handle, destNode, destInputNumber, ref cb);
			} else {
				res = AUGraphError.OK;
			}

			nodesCallbacks [destNode] = renderDelegate;
			return res;
		}

		[MonoPInvokeCallback (typeof (RenderCallbackShared))]
		static AudioUnitStatus RenderCallbackImpl (IntPtr clientData, ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, IntPtr data)
		{
			GCHandle gch = GCHandle.FromIntPtr (clientData);
			var au = (AUGraph) gch.Target;
		}
		*/

		public AUGraphError ClearConnections ()
		{
			return AUGraphClearConnections (handle);
		}

		public AUGraphError Start()
		{
			return AUGraphStart (handle);
		}

		public AUGraphError Stop()
		{
			return AUGraphStop (handle);
		}
		
		public AUGraphError Initialize()
		{
			return AUGraphInitialize (handle);
		}

		public bool Update ()
		{
			bool isUpdated;
			return AUGraphUpdate (handle, out isUpdated) == AUGraphError.OK && isUpdated;
		}

		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				AUGraphUninitialize (handle);
				AUGraphClose (handle);
				DisposeAUGraph (handle);
				
				gcHandle.Free();
				handle = IntPtr.Zero;
			}
		}

		~AUGraph ()
		{
			Dispose (false);
		}
			
		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "NewAUGraph")]
		static extern int NewAUGraph(ref IntPtr outGraph);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphOpen")]
		static extern int AUGraphOpen(IntPtr inGraph);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphAddNode(IntPtr inGraph, AudioComponentDescription inDescription, out int outNode);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphRemoveNode (IntPtr inGraph, int inNode);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphGetNodeCount (IntPtr inGraph, out int outNumberOfNodes);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphGetIndNode (IntPtr inGraph, uint inIndex, out int outNode);
	
		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphNodeInfo(IntPtr inGraph, int inNode, IntPtr outDescription, out IntPtr outAudioUnit);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphNodeInfo (IntPtr inGraph, int inNode, out AudioComponentDescription outDescription, out IntPtr outAudioUnit);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphClearConnections (IntPtr inGraph);
	
		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphConnectNodeInput (IntPtr inGraph, int inSourceNode, uint inSourceOutputNumber, int inDestNode, uint inDestInputNumber);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphDisconnectNodeInput (IntPtr inGraph, int inDestNode, uint inDestInputNumber);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphGetNumberOfInteractions (IntPtr inGraph, out uint outNumInteractions);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphCountNodeInteractions (IntPtr inGraph, int inNode, out uint outNumInteractions);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphInitialize (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphAddRenderNotify")]
	        static extern int AUGraphAddRenderNotify(IntPtr inGraph, AudioUnit.AURenderCallback inCallback, IntPtr inRefCon );
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphStart (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphStop (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphUninitialize (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphClose")]
	        static extern int AUGraphClose(IntPtr inGraph);
	
		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern int DisposeAUGraph(IntPtr inGraph);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphIsOpen (IntPtr inGraph, out bool outIsOpen);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphIsInitialized (IntPtr inGraph, out bool outIsInitialized);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphIsRunning (IntPtr inGraph, out bool outIsRunning);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphGetCPULoad (IntPtr inGraph, out float outAverageCPULoad);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphGetMaxCPULoad (IntPtr inGraph, out float outMaxLoad);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphSetNodeInputCallback (IntPtr inGraph, int inDestNode, uint inDestInputNumber, ref AURenderCallbackStruct inInputCallback);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphUpdate (IntPtr inGraph, out bool outIsUpdated);
	}
}
