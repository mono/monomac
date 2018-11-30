//
// MidiServices.cs: Implementation of the MidiObject base class and its derivates
//
// Author:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012 Xamarin Inc
//
// TODO:
//   * Each MidiObject should be added to a static hashtable so we can always
//     obtain objects that have already been created from the handle, and avoid
//     having two managed objects referencing the same unmanaged object.
//
//     Currently a few lookup functions end up creating objects that might have
//     already been surfaced (new MidiEndpoint (handle) for example)
//
// MIDISendSysex
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;

namespace MonoMac.CoreMidi {

	public enum MidiError {
		Ok = 0,
        	InvalidClient = -10830,
        	InvalidPort = -10831,
        	WrongEndpointType = -10832,
        	NoConnection = -10833,
        	UnknownEndpoint = -10834,
        	UnknownProperty = -10835,
        	WrongPropertyType = -10836,
        	NoCurrentSetup = -10837,
        	MessageSendErr = -10838,
        	ServerStartErr = -10839,
        	SetupFormatErr = -10840,
        	WrongThread = -10841,
        	ObjectNotFound = -10842,
        	IDNotUnique = -10843,
		NotPermitted = -10844
	}

	public enum MidiNetworkConnectionPolicy {
		NoOne, HostsInContactsList, Anyone
	}

	[Flags]
	enum MidiObjectType {
		Other = -1,
		Device, Entity, Source, Destination,
		ExternalMask = 0x10,
		ExternalDevice = ExternalMask | Device,
		ExternalEntity = ExternalMask | Entity,
		ExternalSource = ExternalMask | Source,
		ExternalDestination = ExternalMask | Destination,
	}

	public static partial class Midi {
		[DllImport (Constants.CoreMidiLibrary)]
		extern static void MIDIRestart ();

                [DllImport (Constants.SystemLibrary)]
		extern internal static void memcpy (IntPtr target, IntPtr source, int n);

		public static void Restart ()
		{
			MIDIRestart ();
		}
		
		internal static IntPtr EncodePackets (MidiPacket [] packets)
		{
			int size = 4;
			for (int i = packets.Length; i > 0; i--){
				int plen = packets [i - 1].Length;
				plen = (plen + 3)&(~3);
				size += 8 + 4 + plen;
			}
			IntPtr buffer = Marshal.AllocHGlobal (size);
			Marshal.WriteInt32 (buffer, 0, packets.Length);
			int dest = 4;
			for (int i = 0; i < packets.Length; i++){
				Marshal.WriteInt64 (buffer, dest, packets [i].TimeStamp);
				dest += 8;
				Marshal.WriteInt16 (buffer, dest, (short) packets [i].Length);
				dest += 2;
				Midi.memcpy ((IntPtr)((long)buffer + dest), packets [i].Bytes, packets [i].Length);
				dest += (packets [i].Length+3)&(~3);
			}
			return buffer;
		}

		public static int DestinationCount {
			get {
				return MIDIGetNumberOfDestinations ();
			}
		}

		public static int SourceCount {
			get {
				return MIDIGetNumberOfSources ();
			}
		}
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIGetNumberOfDestinations ();
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIGetNumberOfSources ();

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIGetNumberOfExternalDevices ();

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIGetNumberOfDevices ();

		public static int ExternalDeviceCount {
			get {
				return MIDIGetNumberOfExternalDevices ();
			}
		}

		public static int DeviceCount {
			get {
				return MIDIGetNumberOfDevices ();
			}
		}
		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIGetExternalDevice (int item);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIGetDevice (int item);

		public static MidiDevice GetDevice (int deviceIndex)
		{
			var h = MIDIGetDevice (deviceIndex);
			if (h == IntPtr.Zero)
				return null;
			return new MidiDevice (h);
		}

		public static MidiDevice GetExternalDevice (int deviceIndex)
		{
			var h = MIDIGetExternalDevice (deviceIndex);
			if (h == IntPtr.Zero)
				return null;
			return new MidiDevice (h);
		}
	}
	
	public class MidiObject : INativeObject, IDisposable {
		internal IntPtr handle;
		internal bool owns;

#if !COREBUILD
		internal static IntPtr kMIDIPropertyAdvanceScheduleTimeMuSec;
		internal static IntPtr kMIDIPropertyCanRoute;
		internal static IntPtr kMIDIPropertyConnectionUniqueID;
		internal static IntPtr kMIDIPropertyDeviceID;
		internal static IntPtr kMIDIPropertyDisplayName;
		internal static IntPtr kMIDIPropertyDriverDeviceEditorApp;
		internal static IntPtr kMIDIPropertyDriverOwner;
		internal static IntPtr kMIDIPropertyDriverVersion;
		internal static IntPtr kMIDIPropertyImage;
		internal static IntPtr kMIDIPropertyIsBroadcast;
		internal static IntPtr kMIDIPropertyIsDrumMachine;
		internal static IntPtr kMIDIPropertyIsEffectUnit;
		internal static IntPtr kMIDIPropertyIsEmbeddedEntity;
		internal static IntPtr kMIDIPropertyIsMixer;
		internal static IntPtr kMIDIPropertyIsSampler;
		internal static IntPtr kMIDIPropertyManufacturer;
		internal static IntPtr kMIDIPropertyMaxReceiveChannels;
		internal static IntPtr kMIDIPropertyMaxSysExSpeed;
		internal static IntPtr kMIDIPropertyMaxTransmitChannels;
		internal static IntPtr kMIDIPropertyModel;
		internal static IntPtr kMIDIPropertyName;
		internal static IntPtr kMIDIPropertyNameConfiguration;
		internal static IntPtr kMIDIPropertyOffline;
		internal static IntPtr kMIDIPropertyPanDisruptsStereo;
		internal static IntPtr kMIDIPropertyPrivate;
		internal static IntPtr kMIDIPropertyReceiveChannels;
		internal static IntPtr kMIDIPropertyReceivesBankSelectLSB;
		internal static IntPtr kMIDIPropertyReceivesBankSelectMSB;
		internal static IntPtr kMIDIPropertyReceivesClock;
		internal static IntPtr kMIDIPropertyReceivesMTC;
		internal static IntPtr kMIDIPropertyReceivesNotes;
		internal static IntPtr kMIDIPropertyReceivesProgramChanges;
		internal static IntPtr kMIDIPropertySingleRealtimeEntity;
		internal static IntPtr kMIDIPropertySupportsGeneralMIDI;
		internal static IntPtr kMIDIPropertySupportsMMC;
		internal static IntPtr kMIDIPropertySupportsShowControl;
		internal static IntPtr kMIDIPropertyTransmitChannels;
		internal static IntPtr kMIDIPropertyTransmitsBankSelectLSB;
		internal static IntPtr kMIDIPropertyTransmitsBankSelectMSB;
		internal static IntPtr kMIDIPropertyTransmitsClock;
		internal static IntPtr kMIDIPropertyTransmitsMTC;
		internal static IntPtr kMIDIPropertyTransmitsNotes;
		internal static IntPtr kMIDIPropertyTransmitsProgramChanges;
		internal static IntPtr kMIDIPropertyUniqueID;

		static MidiObject ()
		{
#if MONOMAC
			var midiLibrary = Dlfcn.dlopen (Constants.CoreMidiLibrary, 0);
#else
			var midiLibrary = Libraries.CoreMidi.Handle;
#endif
			kMIDIPropertyAdvanceScheduleTimeMuSec = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyAdvanceScheduleTimeMuSec");
			kMIDIPropertyCanRoute = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyCanRoute");
			kMIDIPropertyConnectionUniqueID = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyConnectionUniqueID");
			kMIDIPropertyDeviceID = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyDeviceID");
			kMIDIPropertyDisplayName = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyDisplayName");
			kMIDIPropertyDriverDeviceEditorApp = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyDriverDeviceEditorApp");
			kMIDIPropertyDriverOwner = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyDriverOwner");
			kMIDIPropertyDriverVersion = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyDriverVersion");
			kMIDIPropertyImage = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyImage");
			kMIDIPropertyIsBroadcast = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyIsBroadcast");
			kMIDIPropertyIsDrumMachine = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyIsDrumMachine");
			kMIDIPropertyIsEffectUnit = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyIsEffectUnit");
			kMIDIPropertyIsEmbeddedEntity = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyIsEmbeddedEntity");
			kMIDIPropertyIsMixer = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyIsMixer");
			kMIDIPropertyIsSampler = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyIsSampler");
			kMIDIPropertyManufacturer = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyManufacturer");
			kMIDIPropertyMaxReceiveChannels = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyMaxReceiveChannels");
			kMIDIPropertyMaxSysExSpeed = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyMaxSysExSpeed");
			kMIDIPropertyMaxTransmitChannels = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyMaxTransmitChannels");
			kMIDIPropertyModel = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyModel");
			kMIDIPropertyName = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyName");
			kMIDIPropertyNameConfiguration = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyNameConfiguration");
			kMIDIPropertyOffline = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyOffline");
			kMIDIPropertyPanDisruptsStereo = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyPanDisruptsStereo");
			kMIDIPropertyPrivate = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyPrivate");
			kMIDIPropertyReceiveChannels = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceiveChannels");
			kMIDIPropertyReceivesBankSelectLSB = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceivesBankSelectLSB");
			kMIDIPropertyReceivesBankSelectMSB = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceivesBankSelectMSB");
			kMIDIPropertyReceivesClock = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceivesClock");
			kMIDIPropertyReceivesMTC = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceivesMTC");
			kMIDIPropertyReceivesNotes = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceivesNotes");
			kMIDIPropertyReceivesProgramChanges = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyReceivesProgramChanges");
			kMIDIPropertySingleRealtimeEntity = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertySingleRealtimeEntity");
			kMIDIPropertySupportsGeneralMIDI = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertySupportsGeneralMIDI");
			kMIDIPropertySupportsMMC = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertySupportsMMC");
			kMIDIPropertySupportsShowControl = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertySupportsShowControl");
			kMIDIPropertyTransmitChannels = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitChannels");
			kMIDIPropertyTransmitsBankSelectLSB = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitsBankSelectLSB");
			kMIDIPropertyTransmitsBankSelectMSB = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitsBankSelectMSB");
			kMIDIPropertyTransmitsClock = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitsClock");
			kMIDIPropertyTransmitsMTC = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitsMTC");
			kMIDIPropertyTransmitsNotes = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitsNotes");
			kMIDIPropertyTransmitsProgramChanges = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyTransmitsProgramChanges");
			kMIDIPropertyUniqueID = Dlfcn.GetIntPtr (midiLibrary, "kMIDIPropertyUniqueID");
		}
#endif
	
		public IntPtr Handle {
			get { return handle; }
		}
		
		internal MidiObject ()
		{
			owns = true;
		}
		
#if !COREBUILD
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectGetIntegerProperty (IntPtr obj, IntPtr str, out int ret);
		internal int GetInt (IntPtr property)
		{
			int val, code;

			code = MIDIObjectGetIntegerProperty (handle, property, out val);
			if (code == 0)
				return val;
			throw new MidiException ((MidiError) code);
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectSetIntegerProperty (IntPtr obj, IntPtr str, int val);
		internal void SetInt (IntPtr property, int value)
		{
			MIDIObjectSetIntegerProperty (handle, property, value);
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectGetDictionaryProperty (IntPtr obj, IntPtr str, out IntPtr dict);
		internal NSDictionary GetDictionary (IntPtr property)
		{
			IntPtr val;
			int code;
			
			code = MIDIObjectGetDictionaryProperty (handle, property, out val);
			if (code == 0)
				return Runtime.GetNSObject<NSDictionary> (val);
			throw new MidiException ((MidiError) code);
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectSetDictionaryProperty (IntPtr obj, IntPtr str, IntPtr dict);
		internal void SetDictionary (IntPtr property, NSDictionary dict)
		{
			MIDIObjectSetDictionaryProperty (handle, property, dict.Handle);
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectGetDataProperty (IntPtr obj, IntPtr str, out IntPtr data);
		
		public NSData GetData (IntPtr property)
		{
			IntPtr val;
			int code;
			
			code = MIDIObjectGetDataProperty (handle, property, out val);
			if (code == 0)
				return Runtime.GetNSObject<NSData> (val);
			throw new MidiException ((MidiError) code);
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectSetDataProperty (IntPtr obj, IntPtr str, IntPtr data);

		public void SetData (IntPtr property, NSData data)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			MIDIObjectSetDataProperty (handle, property, data.Handle);
		}
		
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectGetStringProperty (IntPtr obj, IntPtr str, out IntPtr data);
		
		public string GetString (IntPtr property)
		{
			IntPtr val;
			int code;
			
			code = MIDIObjectGetStringProperty (handle, property, out val);
			if (code == 0){
				var ret = NSString.FromHandle (val);
				CFObject.CFRelease (val);
				return ret;
			}
			return null;
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectSetStringProperty (IntPtr obj, IntPtr str, IntPtr nstr);

		public void SetString (IntPtr property, string value)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			using (var nsval = new NSString (value)){
				MIDIObjectSetDictionaryProperty (handle, property, nsval.Handle);
			}
		}

		
		[DllImport (Constants.CoreMidiLibrary)]
		extern static MidiError MIDIObjectRemoveProperty (IntPtr obj, IntPtr str);
		public MidiError RemoveProperty (string property)
		{
			using (var nsstr = new NSString (property)){
				return MIDIObjectRemoveProperty (handle, nsstr.Handle);
			}			
		}


		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIObjectGetProperties (IntPtr obj, out IntPtr dict, bool deep);
		
		public NSDictionary GetDictionaryProperties (bool deep)
		{
			IntPtr val;
			if (MIDIObjectGetProperties (handle, out val, deep) != 0)
				return null;
			return Runtime.GetNSObject<NSDictionary> (val);
		}
		
#endif
		
                public MidiObject (IntPtr handle)
			: this (handle, true)
                {
                }

		internal MidiObject (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			this.handle = handle;
			this.owns = owns;
		}

                ~MidiObject ()
                {
                        Dispose (false);
                }

		// Default implementation, not all Midi* objects have a native dispose mechanism,
		internal virtual void DisposeHandle ()
		{
			handle = IntPtr.Zero;
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		public virtual void Dispose (bool disposing)
		{
			DisposeHandle ();
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static MidiError MIDIObjectFindByUniqueID (int uniqueId, out IntPtr obj, out MidiObjectType objectType);

		static internal MidiObject MidiObjectFromType (MidiObjectType type, IntPtr handle)
		{
			switch (type & (~MidiObjectType.ExternalMask)){
			case MidiObjectType.Other:
				return new MidiObject (handle);
			case MidiObjectType.Device:
				return new MidiDevice (handle);
			case MidiObjectType.Entity:
				return new MidiEntity (handle);
			case MidiObjectType.Source:
				return new MidiEndpoint (handle, false);
			case MidiObjectType.Destination:
				return new MidiEndpoint (handle, false);
			default:
				throw new Exception ("Unknown MidiObjectType " + (int) type);
			}
		}

		static MidiError FindByUniqueId (int uniqueId, out MidiObject result)
		{
			IntPtr handle;
			MidiObjectType type;
			var code = MIDIObjectFindByUniqueID (uniqueId, out handle, out type);
			result = null;
			if (code != MidiError.Ok)
				return code;

			result = MidiObjectFromType (type, handle);
			return code;
		}
	}

	public class MidiException : Exception {
		internal MidiException (MidiError code) : base (code.ToString ())
		{
			ErrorCode = code;
		}
		
		public MidiError ErrorCode { get; private set; }
	}
	
	delegate void MidiNotifyProc (IntPtr message, IntPtr context);
	
	public class MidiClient : MidiObject {
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIClientCreate (IntPtr str, MidiNotifyProc callback, IntPtr context, out IntPtr handle);
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIClientDispose (IntPtr handle);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDISourceCreate (IntPtr handle, IntPtr name, out IntPtr endpoint);
			
		GCHandle gch;

		internal override void DisposeHandle ()
		{
			if (handle != IntPtr.Zero){
				if (owns)
					MIDIClientDispose (handle);
				handle = IntPtr.Zero;
				gch.Free ();
			}
		}

		public MidiClient (string name) 
		{
			using (var nsstr = new NSString (name)){
				gch = GCHandle.Alloc (this);
				int code = MIDIClientCreate (nsstr.Handle, ClientCallback, GCHandle.ToIntPtr (gch), out handle);
				if (code != 0){
					gch.Free ();
					handle = IntPtr.Zero;
					throw new MidiException ((MidiError) code);
				}
				Name = name;
			}
		}
		public string Name { get; private set; }

		public override string ToString ()
		{
			return Name;
		}

		public MidiEndpoint CreateVirtualSource (string name)
		{
			using (var nsstr = new NSString (name)){
				IntPtr ret;
				var code = MIDISourceCreate (handle, nsstr.Handle, out ret);
				if (code != 0)
					return null;
				return new MidiEndpoint (ret, true);
			}			
		}
		
		public MidiPort CreateInputPort (string name)
		{
			return new MidiPort (this, name, true);
		}

		public MidiPort CreateOutputPort (string name)
		{
			return new MidiPort (this, name, false);
		}

		public event EventHandler SetupChanged;
		public event EventHandler<ObjectAddedOrRemovedEventArgs> ObjectAdded;
		public event EventHandler<ObjectAddedOrRemovedEventArgs> ObjectRemoved;
		public event EventHandler<ObjectPropertyChangedEventArgs> PropertyChanged;
		public event EventHandler ThruConnectionsChanged;
		public event EventHandler SerialPortOwnerChanged;
		public event EventHandler<IOErrorEventArgs> IOError;
		
#if !MONOMAC
		[MonoPInvokeCallback (typeof (MidiNotifyProc))]
#endif
		static void ClientCallback (IntPtr message, IntPtr context)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
			MidiClient client = (MidiClient) gch.Target;

			var id = (MidiNotificationMessageId) Marshal.ReadInt32 (message);
			switch (id){
			case MidiNotificationMessageId.SetupChanged:
				var esc = client.SetupChanged;
				if (esc != null)
					esc (client, EventArgs.Empty);
				break;
			case MidiNotificationMessageId.ObjectAdded:
				var eoa = client.ObjectAdded;
				if (eoa != null){
					var data = (MidiObjectAddRemoveNotification) Marshal.PtrToStructure (message, typeof (MidiObjectAddRemoveNotification));
					eoa (client, new ObjectAddedOrRemovedEventArgs (MidiObjectFromType (data.ParentType, data.Parent),
											MidiObjectFromType (data.ChildType, data.Child)));
				}
				break;
			case MidiNotificationMessageId.ObjectRemoved:
				var eor = client.ObjectRemoved;
				if (eor != null){
					var data = (MidiObjectAddRemoveNotification) Marshal.PtrToStructure (message, typeof (MidiObjectAddRemoveNotification));
					eor (client, new ObjectAddedOrRemovedEventArgs (MidiObjectFromType (data.ParentType, data.Parent),
											MidiObjectFromType (data.ChildType, data.Child)));
				}
				break;
			case MidiNotificationMessageId.PropertyChanged:
				var epc = client.PropertyChanged;
				if (epc != null){
					var data = (MidiObjectPropertyChangeNotification) Marshal.PtrToStructure (message, typeof (MidiObjectPropertyChangeNotification));
					epc (client, new ObjectPropertyChangedEventArgs (
						     MidiObjectFromType (data.ObjectType, data.ObjectHandle), NSString.FromHandle (data.PropertyName)));
				}
				break;
			case MidiNotificationMessageId.ThruConnectionsChanged:
				var e = client.ThruConnectionsChanged;
				if (e != null)
					e (client, EventArgs.Empty);
				break;
			case MidiNotificationMessageId.SerialPortOwnerChanged:
				e = client.SerialPortOwnerChanged;
				if (e != null)
					e (client, EventArgs.Empty);
				break;
			case MidiNotificationMessageId.IOError:
				var eio = client.IOError;
				if (eio != null){
					var data = (MidiIOErrorNotification) Marshal.PtrToStructure (message, typeof (MidiIOErrorNotification));
					eio (client, new IOErrorEventArgs (new MidiDevice (data.DeviceRef), data.ErrorCode));
				}
				break;
			default:
				Debug.WriteLine (String.Format ("Unknown message received: {0}", id));
				break;
			}
		}

		public override void Dispose (bool disposing)
		{
			SetupChanged = null;
			ObjectAdded = null;
			ObjectRemoved = null;
			PropertyChanged = null;
			ThruConnectionsChanged = null;
			SerialPortOwnerChanged = null;
			IOError = null;
			base.Dispose (disposing);
		}
		
		[StructLayout (LayoutKind.Sequential)]
		struct MidiObjectAddRemoveNotification {
			public MidiNotificationMessageId id;
			public int MessageSize;
			public IntPtr Parent;
			public MidiObjectType ParentType;
			public IntPtr Child;
			public MidiObjectType ChildType;
		}
	
		[StructLayout (LayoutKind.Sequential)]
		struct MidiObjectPropertyChangeNotification {
			public MidiNotificationMessageId id;
			public int MessageSize;
			public IntPtr ObjectHandle;
			public MidiObjectType ObjectType;
			public IntPtr PropertyName;
		}
	
		[StructLayout (LayoutKind.Sequential)]
		struct MidiIOErrorNotification {
			public MidiNotificationMessageId id;
			public int MessageSize;
			public IntPtr DeviceRef;
			public int ErrorCode;
		}
	}

	//
	// We do not pack this structure since we do not really actually marshal it,
	// we manually encode it and decode it using Marshal.{Read|Write}
	//
	public class MidiPacket {
		public long  TimeStamp;
		IntPtr byteptr;
		byte [] bytes;
		int    start;
		public ushort Length;

		public MidiPacket (long timestamp, ushort length, IntPtr bytes)
		{
			TimeStamp = timestamp;
			Length = length;
			byteptr = bytes;
		}

		public MidiPacket (long timestamp, byte [] bytes) : this (timestamp, bytes, 0, bytes.Length, false)
		{
		}

		public MidiPacket (long timestamp, byte [] bytes, int start, int len) : this (timestamp, bytes, start, len, true)
		{
		}
		
		MidiPacket (long timestamp, byte [] bytes, int start, int length, bool check)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			if (length > UInt16.MaxValue)
				throw new ArgumentException ("lenght is bigger than 64k");
			
			if (check){
				if (start < 0 || start >= bytes.Length)
					throw new ArgumentException ("range is not within bytes");
				if (start+length > bytes.Length)
					throw new ArgumentException ("range is not within bytes");
			}
			
			TimeStamp = timestamp;
			Length = (ushort) length;
			this.start = start;
			this.bytes = bytes;
		}

		public IntPtr Bytes {
			get {
				if (bytes == null)
					return byteptr;
				unsafe {
					fixed (byte *p = &bytes [start]){
						return (IntPtr) p;
					}
				}
			}
		}
	}

	delegate void MidiReadProc (IntPtr packetList, IntPtr context, IntPtr srcPtr);
	
	public class MidiPort : MidiObject {
		#if !MONOMAC
		static bool isDevice = MonoTouch.ObjCRuntime.Runtime.Arch == MonoTouch.ObjCRuntime.Arch.DEVICE;
		#endif

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIInputPortCreate (IntPtr client, IntPtr portName, MidiReadProc readProc, IntPtr context, out IntPtr midiPort);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIOutputPortCreate (IntPtr client, IntPtr portName, out IntPtr midiPort);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIPortDispose (IntPtr port);
		
		GCHandle gch;
		bool input;
		
		internal MidiPort (MidiClient client, string portName, bool input)
		{
			using (var nsstr = new NSString (portName)){
				GCHandle gch = GCHandle.Alloc (this);
				int code;
				
				if (input)
					code = MIDIInputPortCreate (client.handle, nsstr.Handle, Read, GCHandle.ToIntPtr (gch), out handle);
				else
					code = MIDIOutputPortCreate (client.handle, nsstr.Handle, out handle);
				
				if (code != 0){
					gch.Free ();
					handle = IntPtr.Zero;
					throw new MidiException ((MidiError) code);
				}
				Client = client;
				PortName = portName;
				this.input = input;
			}
		}

		public MidiClient Client { get; private set; }
		public string PortName { get; private set; }
		
		internal override void DisposeHandle ()
		{
			if (handle != IntPtr.Zero){
				if (owns)
					MIDIPortDispose (handle);
				handle = IntPtr.Zero;
				gch.Free ();
			}
		}

		internal static MidiPacket [] ToPackets (IntPtr packetList)
		{
			int npackets = Marshal.ReadInt32 (packetList);
			int p = 4;
			var packets = new MidiPacket [npackets];
			for (int i = 0; i < npackets; i++){
				ushort len = (ushort) Marshal.ReadInt16 (packetList, p+8);
				packets [i] = new MidiPacket (Marshal.ReadInt64 (packetList, p), len, (IntPtr)((long)packetList + p + 10));
#if !MONOMAC
				// MIDIPacket must be 4-byte aligned for ARM
				if (isDevice)
					p += (((10 + len) + 3) & ~3);
				else
#endif
					p += 10 + len;
			}
			return packets;
		}

		public override void Dispose (bool disposing)
		{
			MessageReceived = null;
			base.Dispose (disposing);
		}
		
		public event EventHandler<MidiPacketsEventArgs> MessageReceived;
		
#if !MONOMAC
		[MonoPInvokeCallback (typeof (MidiReadProc))]
#endif
		static void Read (IntPtr packetList, IntPtr context, IntPtr srcPtr)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
			MidiPort port = (MidiPort) gch.Target;

			var e = port.MessageReceived;
			if (e != null)
				e (port, new MidiPacketsEventArgs (packetList));
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIPortConnectSource (IntPtr port, IntPtr endpoint, IntPtr context);
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIPortDisconnectSource (IntPtr port, IntPtr endpoint);

		public MidiError ConnectSource (MidiEndpoint endpoint)
		{
			if (endpoint == null)
				throw new ArgumentNullException ("endpoint");
			return (MidiError) MIDIPortConnectSource (handle, endpoint.handle, GCHandle.ToIntPtr (gch));
		}

		public MidiError Disconnect (MidiEndpoint endpoint)
		{
			if (endpoint == null)
				throw new ArgumentNullException ("endpoint");
			return (MidiError) MIDIPortDisconnectSource (handle, endpoint.handle);
		}
		
		public override string ToString ()
		{
			return (input ? "[input:" : "[output:") + Client + ":" + PortName + "]";
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static MidiError MIDISend (IntPtr port, IntPtr endpoint, IntPtr packets);

		public MidiError Send (MidiEndpoint endpoint, MidiPacket [] packets)
		{
			if (endpoint == null)
				throw new ArgumentNullException ("endpoint");
			if (packets == null)
				throw new ArgumentNullException ("packets");
			var p = Midi.EncodePackets (packets);
			var code = MIDISend (handle, endpoint.handle, p);
			Marshal.FreeHGlobal (p);
			return code;
		}
		
	}

	public class MidiEntity : MidiObject {
		internal MidiEntity (IntPtr handle) : base (handle)
		{
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIEntityGetDestination (IntPtr entity, int idx);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIEntityGetSource (IntPtr entity, int idx);
		
		public MidiEndpoint GetDestination (int idx)
		{
			var dest = MIDIEntityGetDestination (handle, idx);
			if (dest == IntPtr.Zero)
				return null;
			return new MidiEndpoint (handle, false);
		}

		public MidiEndpoint GetSource (int idx)
		{
			var dest = MIDIEntityGetSource (handle, idx);
			if (dest == IntPtr.Zero)
				return null;
			return new MidiEndpoint (handle, false);
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIEntityGetNumberOfDestinations (IntPtr entity);

		public int Destinations {
			get {
				return MIDIEntityGetNumberOfDestinations (handle);
			}
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIEntityGetNumberOfSources (IntPtr entity);

		public int Sources {
			get {
				return MIDIEntityGetNumberOfSources (handle);
			}
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIEntityGetDevice (IntPtr handle, out IntPtr devRef);

		public MidiDevice Device {
			get {
				IntPtr res;
				if (MIDIEntityGetDevice (handle, out res) == 0)
					return new MidiDevice (res);
				return null;
			}
		}

#if !COREBUILD
		public int AdvanceScheduleTimeMuSec {
			get {
				return GetInt (kMIDIPropertyAdvanceScheduleTimeMuSec);
			}
			set {
				SetInt (kMIDIPropertyAdvanceScheduleTimeMuSec, value);
			}
		}

		public bool CanRoute {
			get {
				return GetInt (kMIDIPropertyCanRoute) != 0;
			}
			set {
				SetInt (kMIDIPropertyCanRoute, value ? 1 : 0);
			}
		}

		public int ConnectionUniqueIDInt {
			get {
				return GetInt (kMIDIPropertyConnectionUniqueID);
			}
			set {
				SetInt (kMIDIPropertyConnectionUniqueID, value);
			}
		}

		public NSData ConnectionUniqueIDData {
			get {
				return GetData (kMIDIPropertyConnectionUniqueID);
			}
			set {
				SetData (kMIDIPropertyConnectionUniqueID, value);
			}
		}

		public int DeviceID {
			get {
				return GetInt (kMIDIPropertyDeviceID);
			}
			set {
				SetInt (kMIDIPropertyDeviceID, value);
			}
		}

		public string DisplayName {
			get {
				return GetString (kMIDIPropertyDisplayName);
			}
			set {
				SetString (kMIDIPropertyDisplayName, value);
			}
		}

		public string DriverOwner {
			get {
				return GetString (kMIDIPropertyDriverOwner);
			}
			set {
				SetString (kMIDIPropertyDriverOwner, value);
			}
		}

		public int DriverVersion {
			get {
				return GetInt (kMIDIPropertyDriverVersion);
			}
			set {
				SetInt (kMIDIPropertyDriverVersion, value);
			}
		}

		public int IsBroadcast {
			get {
				return GetInt (kMIDIPropertyIsBroadcast);
			}
			set {
				SetInt (kMIDIPropertyIsBroadcast, value);
			}
		}

		public bool IsDrumMachine {
			get {
				return GetInt (kMIDIPropertyIsDrumMachine) != 0;
			}
		}

		public bool IsEffectUnit {
			get {
				return GetInt (kMIDIPropertyIsEffectUnit) != 0;
			}
		}

		public bool IsEmbeddedEntity {
			get {
				return GetInt (kMIDIPropertyIsEmbeddedEntity) != 0;
			}
		}

		public bool IsMixer {
			get {
				return GetInt (kMIDIPropertyIsMixer) != 0;
			}
		}

		public bool IsSampler {
			get {
				return GetInt (kMIDIPropertyIsSampler) != 0;
			}
		}

		public int MaxReceiveChannels {
			get {
				return GetInt (kMIDIPropertyMaxReceiveChannels);
			}
			//set {
			//SetInt (kMIDIPropertyMaxReceiveChannels, value);
			//}
		}

		public int MaxSysExSpeed {
			get {
				try {
					return GetInt (kMIDIPropertyMaxSysExSpeed);
				} catch {
					// Some endpoints do not support this property
					// return the MIDI 1.0 default in those cases.
					return 3125;
				}
			}
			set {
				SetInt (kMIDIPropertyMaxSysExSpeed, value);
			}
		}

		public int MaxTransmitChannels {
			get {
				return GetInt (kMIDIPropertyMaxTransmitChannels);
			}
			set {
				SetInt (kMIDIPropertyMaxTransmitChannels, value);
			}
		}

		public string Model {
			get {
				return GetString (kMIDIPropertyModel);
			}
			set {
				SetString (kMIDIPropertyModel, value);
			}
		}

		public string Name {
			get {
				return GetString (kMIDIPropertyName);
			}
			set {
				SetString (kMIDIPropertyName, value);
			}
		}

		public NSDictionary NameConfiguration {
			get {
				return GetDictionary (kMIDIPropertyNameConfiguration);
			}
			set {
				SetDictionary (kMIDIPropertyNameConfiguration, value);
			}
		}

		public bool Offline {
			get {
				return GetInt (kMIDIPropertyOffline) != 0;
			}
			set {
				SetInt (kMIDIPropertyOffline, value ? 1 : 0);
			}
		}

		public bool PanDisruptsStereo {
			get {
				return GetInt (kMIDIPropertyPanDisruptsStereo) != 0;
			}
			set {
				SetInt (kMIDIPropertyPanDisruptsStereo, value ? 1 : 0);
			}
		}

		public bool Private {
			get {
				return GetInt (kMIDIPropertyPrivate) != 0;
			}
			set {
				SetInt (kMIDIPropertyPrivate, value ? 1 : 0);
			}
		}

		public bool ReceivesBankSelectLSB {
			get {
				return GetInt (kMIDIPropertyReceivesBankSelectLSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesBankSelectLSB, value ? 1 : 0);
			}
		}

		public bool ReceivesBankSelectMSB {
			get {
				return GetInt (kMIDIPropertyReceivesBankSelectMSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesBankSelectMSB, value ? 1 : 0);
			}
		}

		public bool ReceivesClock {
			get {
				return GetInt (kMIDIPropertyReceivesClock) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesClock, value ? 1 : 0);
			}
		}

		public bool ReceivesMTC {
			get {
				return GetInt (kMIDIPropertyReceivesMTC) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesMTC, value ? 1 : 0);
			}
		}

		public bool ReceivesNotes {
			get {
				return GetInt (kMIDIPropertyReceivesNotes) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesNotes, value ? 1 : 0);
			}
		}

		public bool ReceivesProgramChanges {
			get {
				return GetInt (kMIDIPropertyReceivesProgramChanges) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesProgramChanges, value ? 1 : 0);
			}
		}

		public bool SupportsGeneralMidi {
			get {
				return GetInt (kMIDIPropertySupportsGeneralMIDI) != 0;
			}
			set {
				SetInt (kMIDIPropertySupportsGeneralMIDI, value ? 1 : 0);
			}
		}

		public bool SupportsMMC {
			get {
				return GetInt (kMIDIPropertySupportsMMC) != 0;
			}
			set {
				SetInt (kMIDIPropertySupportsMMC, value ? 1 : 0);
			}
		}

		public bool SupportsShowControl {
			get {
				return GetInt (kMIDIPropertySupportsShowControl) != 0;
			}
			set {
				SetInt (kMIDIPropertySupportsShowControl, value ? 1 : 0);
			}
		}

		public bool TransmitsBankSelectLSB {
			get {
				return GetInt (kMIDIPropertyTransmitsBankSelectLSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsBankSelectLSB, value ? 1 : 0);
			}
		}

		public bool TransmitsBankSelectMSB {
			get {
				return GetInt (kMIDIPropertyTransmitsBankSelectMSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsBankSelectMSB, value ? 1 : 0);
			}
		}

		public bool TransmitsClock {
			get {
				return GetInt (kMIDIPropertyTransmitsClock) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsClock, value ? 1 : 0);
			}
		}

		public bool TransmitsMTC {
			get {
				return GetInt (kMIDIPropertyTransmitsMTC) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsMTC, value ? 1 : 0);
			}
		}

		public bool TransmitsNotes {
			get {
				return GetInt (kMIDIPropertyTransmitsNotes) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsNotes, value ? 1 : 0);
			}
		}

		public bool TransmitsProgramChanges {
			get {
				return GetInt (kMIDIPropertyTransmitsProgramChanges) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsProgramChanges, value ? 1 : 0);
			}
		}
#endif
	}

	public class MidiDevice : MidiObject {
		
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIDeviceGetNumberOfEntities (IntPtr handle);
		
		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIDeviceGetEntity (IntPtr handle, int item);

		public MidiEntity GetEntity (int entityIndex)
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException ("handle");
			var h = MIDIDeviceGetEntity (handle, entityIndex);
			if (h == IntPtr.Zero)
				return null;
			return new MidiEntity (h);
		}

		public int EntityCount {
			get {
				return MIDIDeviceGetNumberOfEntities (handle);
			}
		}

#if !COREBUILD
		public string Image {
			get {
				return GetString (kMIDIPropertyImage);
			}
			set {
				SetString (kMIDIPropertyImage, value);
			}
		}

		public string DriverDeviceEditorApp {
			get {
				return GetString (kMIDIPropertyDriverDeviceEditorApp);
			}
			set {
				SetString (kMIDIPropertyDriverDeviceEditorApp, value);
			}
		}

		public int SingleRealtimeEntity {
			get {
				return GetInt (kMIDIPropertySingleRealtimeEntity);
			}
			set {
				SetInt (kMIDIPropertySingleRealtimeEntity, value);
			}
		}

		public int UniqueID {
			get {
				return GetInt (kMIDIPropertyUniqueID);
			}
			set {
				SetInt (kMIDIPropertyUniqueID, value);
			}
		}

		public int AdvanceScheduleTimeMuSec {
			get {
				return GetInt (kMIDIPropertyAdvanceScheduleTimeMuSec);
			}
			set {
				SetInt (kMIDIPropertyAdvanceScheduleTimeMuSec, value);
			}
		}

		public bool CanRoute {
			get {
				return GetInt (kMIDIPropertyCanRoute) != 0;
			}
			set {
				SetInt (kMIDIPropertyCanRoute, value ? 1 : 0);
			}
		}

		public int ConnectionUniqueIDInt {
			get {
				return GetInt (kMIDIPropertyConnectionUniqueID);
			}
			set {
				SetInt (kMIDIPropertyConnectionUniqueID, value);
			}
		}

		public NSData ConnectionUniqueIDData {
			get {
				return GetData (kMIDIPropertyConnectionUniqueID);
			}
			set {
				SetData (kMIDIPropertyConnectionUniqueID, value);
			}
		}

		public int DeviceID {
			get {
				return GetInt (kMIDIPropertyDeviceID);
			}
			set {
				SetInt (kMIDIPropertyDeviceID, value);
			}
		}

		public string DisplayName {
			get {
				return GetString (kMIDIPropertyDisplayName);
			}
			set {
				SetString (kMIDIPropertyDisplayName, value);
			}
		}

		public string DriverOwner {
			get {
				return GetString (kMIDIPropertyDriverOwner);
			}
			set {
				SetString (kMIDIPropertyDriverOwner, value);
			}
		}

		public int DriverVersion {
			get {
				return GetInt (kMIDIPropertyDriverVersion);
			}
			set {
				SetInt (kMIDIPropertyDriverVersion, value);
			}
		}

		public bool IsDrumMachine {
			get {
				return GetInt (kMIDIPropertyIsDrumMachine) != 0;
			}
		}

		public bool IsEffectUnit {
			get {
				return GetInt (kMIDIPropertyIsEffectUnit) != 0;
			}
		}

		public bool IsEmbeddedEntity {
			get {
				return GetInt (kMIDIPropertyIsEmbeddedEntity) != 0;
			}
		}

		public bool IsMixer {
			get {
				return GetInt (kMIDIPropertyIsMixer) != 0;
			}
		}

		public bool IsSampler {
			get {
				return GetInt (kMIDIPropertyIsSampler) != 0;
			}
		}

		public string Manufacturer {
			get {
				return GetString (kMIDIPropertyManufacturer);
			}
			set {
				SetString (kMIDIPropertyManufacturer, value);
			}
		}

		public int MaxReceiveChannels {
			get {
				return GetInt (kMIDIPropertyMaxReceiveChannels);
			}
			//set {
			//SetInt (kMIDIPropertyMaxReceiveChannels, value);
			//}
		}

		public int MaxSysExSpeed {
			get {
				try {
					return GetInt (kMIDIPropertyMaxSysExSpeed);
				} catch {
					// Some endpoints do not support this property
					// return the MIDI 1.0 default in those cases.
					return 3125;
				}
			}
			set {
				SetInt (kMIDIPropertyMaxSysExSpeed, value);
			}
		}

		public int MaxTransmitChannels {
			get {
				return GetInt (kMIDIPropertyMaxTransmitChannels);
			}
			set {
				SetInt (kMIDIPropertyMaxTransmitChannels, value);
			}
		}

		public string Model {
			get {
				return GetString (kMIDIPropertyModel);
			}
			set {
				SetString (kMIDIPropertyModel, value);
			}
		}

		public string Name {
			get {
				return GetString (kMIDIPropertyName);
			}
			set {
				SetString (kMIDIPropertyName, value);
			}
		}

		public NSDictionary NameConfiguration {
			get {
				return GetDictionary (kMIDIPropertyNameConfiguration);
			}
			set {
				SetDictionary (kMIDIPropertyNameConfiguration, value);
			}
		}

		public bool Offline {
			get {
				return GetInt (kMIDIPropertyOffline) != 0;
			}
			set {
				SetInt (kMIDIPropertyOffline, value ? 1 : 0);
			}
		}

		public bool PanDisruptsStereo {
			get {
				return GetInt (kMIDIPropertyPanDisruptsStereo) != 0;
			}
			set {
				SetInt (kMIDIPropertyPanDisruptsStereo, value ? 1 : 0);
			}
		}

		public bool Private {
			get {
				return GetInt (kMIDIPropertyPrivate) != 0;
			}
			set {
				SetInt (kMIDIPropertyPrivate, value ? 1 : 0);
			}
		}

		public bool ReceivesBankSelectLSB {
			get {
				return GetInt (kMIDIPropertyReceivesBankSelectLSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesBankSelectLSB, value ? 1 : 0);
			}
		}

		public bool ReceivesBankSelectMSB {
			get {
				return GetInt (kMIDIPropertyReceivesBankSelectMSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesBankSelectMSB, value ? 1 : 0);
			}
		}

		public bool ReceivesClock {
			get {
				return GetInt (kMIDIPropertyReceivesClock) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesClock, value ? 1 : 0);
			}
		}

		public bool ReceivesMTC {
			get {
				return GetInt (kMIDIPropertyReceivesMTC) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesMTC, value ? 1 : 0);
			}
		}

		public bool ReceivesNotes {
			get {
				return GetInt (kMIDIPropertyReceivesNotes) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesNotes, value ? 1 : 0);
			}
		}

		public bool ReceivesProgramChanges {
			get {
				return GetInt (kMIDIPropertyReceivesProgramChanges) != 0;
			}
			set {
				SetInt (kMIDIPropertyReceivesProgramChanges, value ? 1 : 0);
			}
		}

		public bool SupportsGeneralMidi {
			get {
				return GetInt (kMIDIPropertySupportsGeneralMIDI) != 0;
			}
			set {
				SetInt (kMIDIPropertySupportsGeneralMIDI, value ? 1 : 0);
			}
		}

		public bool SupportsMMC {
			get {
				return GetInt (kMIDIPropertySupportsMMC) != 0;
			}
			set {
				SetInt (kMIDIPropertySupportsMMC, value ? 1 : 0);
			}
		}

		public bool SupportsShowControl {
			get {
				return GetInt (kMIDIPropertySupportsShowControl) != 0;
			}
			set {
				SetInt (kMIDIPropertySupportsShowControl, value ? 1 : 0);
			}
		}

		public bool TransmitsBankSelectLSB {
			get {
				return GetInt (kMIDIPropertyTransmitsBankSelectLSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsBankSelectLSB, value ? 1 : 0);
			}
		}

		public bool TransmitsBankSelectMSB {
			get {
				return GetInt (kMIDIPropertyTransmitsBankSelectMSB) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsBankSelectMSB, value ? 1 : 0);
			}
		}

		public bool TransmitsClock {
			get {
				return GetInt (kMIDIPropertyTransmitsClock) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsClock, value ? 1 : 0);
			}
		}

		public bool TransmitsMTC {
			get {
				return GetInt (kMIDIPropertyTransmitsMTC) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsMTC, value ? 1 : 0);
			}
		}

		public bool TransmitsNotes {
			get {
				return GetInt (kMIDIPropertyTransmitsNotes) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsNotes, value ? 1 : 0);
			}
		}

		public bool TransmitsProgramChanges {
			get {
				return GetInt (kMIDIPropertyTransmitsProgramChanges) != 0;
			}
			set {
				SetInt (kMIDIPropertyTransmitsProgramChanges, value ? 1 : 0);
			}
		}
#endif
	
		internal MidiDevice (IntPtr handle) : base (handle)
		{
		}
	}
	
	public class MidiEndpoint : MidiObject {
		GCHandle gch;

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIEndpointDispose (IntPtr handle);
		
		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIDestinationCreate (IntPtr client, IntPtr name, MidiReadProc readProc, IntPtr context, out IntPtr midiEndpoint);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIFlushOutput (IntPtr handle);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static MidiError MIDIReceived (IntPtr handle, IntPtr packetList);
		
		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIGetSource (int sourceIndex);

		[DllImport (Constants.CoreMidiLibrary)]
		extern static IntPtr MIDIGetDestination (int destinationIndex);

		internal override void DisposeHandle ()
		{
			if (handle != IntPtr.Zero){
				if (owns)
					MIDIEndpointDispose (handle);
				handle = IntPtr.Zero;
				gch.Free ();
			}
		}

		public string EndpointName { get; private set; }

		internal MidiEndpoint (IntPtr handle) : base (handle, false)
		{
			EndpointName = "Endpoint from Lookup";
		}

		internal MidiEndpoint (IntPtr handle, bool owns) : base (handle, owns)
		{
			EndpointName = "Endpoint from Lookup";
		}

		internal MidiEndpoint (IntPtr handle, string endpointName, bool owns) : base (handle, owns)
		{
			EndpointName = endpointName;
		}
		

		public static MidiEndpoint GetSource (int sourceIndex)
		{
			var h = MIDIGetSource (sourceIndex);
			if (h == IntPtr.Zero)
				return null;
			return new MidiEndpoint (h, "Source" + sourceIndex, false);
		}

		public static MidiEndpoint GetDestination (int destinationIndex)
		{
			var h = MIDIGetDestination (destinationIndex);
			if (h == IntPtr.Zero)
				return null;
			return new MidiEndpoint (h, "Destination" + destinationIndex, false);
		}

		internal MidiEndpoint (MidiClient client, string name)
		{
			using (var nsstr = new NSString (name)){
				GCHandle gch = GCHandle.Alloc (this);
				int code;
				
				code = MIDIDestinationCreate (client.handle, nsstr.Handle, Read, GCHandle.ToIntPtr (gch), out handle);
				
				if (code != 0){
					gch.Free ();
					handle = IntPtr.Zero;
					throw new MidiException ((MidiError) code);
				}
				EndpointName = name;
			}
		}

		public override void Dispose (bool disposing)
		{
			MessageReceived = null;
			base.Dispose (disposing);
		}
		
		public event EventHandler<MidiPacketsEventArgs> MessageReceived;

#if !MONOMAC
		[MonoPInvokeCallback (typeof (MidiReadProc))]
#endif
		static void Read (IntPtr packetList, IntPtr context, IntPtr srcPtr)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
			MidiEndpoint port = (MidiEndpoint) gch.Target;

			var e = port.MessageReceived;
			if (e != null)
				e (port, new MidiPacketsEventArgs (packetList));
		}

		public void FlushOutput ()
		{
			MIDIFlushOutput (handle);
		}

		public MidiError Received (MidiPacket [] packets)
		{
			if (packets == null)
				throw new ArgumentNullException ("packets");

			var block = Midi.EncodePackets (packets);
			var code = MIDIReceived (handle, block);
			Marshal.FreeHGlobal (block);
			return code;
		}

		[DllImport (Constants.CoreMidiLibrary)]
		extern static int MIDIEndpointGetEntity (IntPtr endpoint, out IntPtr entity);
		
		public MidiEntity Entity {
			get {
				IntPtr entity;
				var code = MIDIEndpointGetEntity (handle, out entity);
				if (code == 0)
					return new MidiEntity (entity);
				return null;
			}
		}

#if !COREBUILD
		public bool IsNetworkSession {
			get {
				using (var dict =  GetDictionaryProperties (true)){
					if (dict == null)
						return false;
					
					using (var key = new NSString ("apple.midirtp.session"))
						return dict.ContainsKey (key);
				}
			}
		}

		public int AdvanceScheduleTimeMuSec {
			get {
				return GetInt (kMIDIPropertyAdvanceScheduleTimeMuSec);
			}
			set {
				SetInt (kMIDIPropertyAdvanceScheduleTimeMuSec, value);
			}
		}

		public int ConnectionUniqueIDInt {
			get {
				return GetInt (kMIDIPropertyConnectionUniqueID);
			}
			set {
				SetInt (kMIDIPropertyConnectionUniqueID, value);
			}
		}

		public NSData ConnectionUniqueIDData {
			get {
				return GetData (kMIDIPropertyConnectionUniqueID);
			}
			set {
				SetData (kMIDIPropertyConnectionUniqueID, value);
			}
		}

		public string DisplayName {
			get {
				return GetString (kMIDIPropertyDisplayName);
			}
			set {
				SetString (kMIDIPropertyDisplayName, value);
			}
		}

		public string DriverOwner {
			get {
				return GetString (kMIDIPropertyDriverOwner);
			}
			set {
				SetString (kMIDIPropertyDriverOwner, value);
			}
		}

		public int DriverVersion {
			get {
				return GetInt (kMIDIPropertyDriverVersion);
			}
			set {
				SetInt (kMIDIPropertyDriverVersion, value);
			}
		}

		public int IsBroadcast {
			get {
				return GetInt (kMIDIPropertyIsBroadcast);
			}
			set {
				SetInt (kMIDIPropertyIsBroadcast, value);
			}
		}

		public string Manufacturer {
			get {
				return GetString (kMIDIPropertyManufacturer);
			}
			set {
				SetString (kMIDIPropertyManufacturer, value);
			}
		}

		public int MaxSysExSpeed {
			get {
				try {
					return GetInt (kMIDIPropertyMaxSysExSpeed);
				} catch {
					// Some endpoints do not support this property
					// return the MIDI 1.0 default in those cases.
					return 3125;
				}
			}
			set {
				SetInt (kMIDIPropertyMaxSysExSpeed, value);
			}
		}

		public string Name {
			get {
				return GetString (kMIDIPropertyName);
			}
			set {
				SetString (kMIDIPropertyName, value);
			}
		}

		public NSDictionary NameConfiguration {
			get {
				return GetDictionary (kMIDIPropertyNameConfiguration);
			}
			set {
				SetDictionary (kMIDIPropertyNameConfiguration, value);
			}
		}

		public bool Offline {
			get {
				return GetInt (kMIDIPropertyOffline) != 0;
			}
			set {
				SetInt (kMIDIPropertyOffline, value ? 1 : 0);
			}
		}

		public bool Private {
			get {
				return GetInt (kMIDIPropertyPrivate) != 0;
			}
			set {
				SetInt (kMIDIPropertyPrivate, value ? 1 : 0);
			}
		}

		public int ReceiveChannels {
			get {
				return GetInt (kMIDIPropertyReceiveChannels);
			}
			set {
				SetInt (kMIDIPropertyReceiveChannels, value);
			}
		}

		public int TransmitChannels {
			get {
				return GetInt (kMIDIPropertyTransmitChannels);
			}
			set {
				SetInt (kMIDIPropertyTransmitChannels, value);
			}
		}

// MidiEndpoint 
#endif
	}

	enum MidiNotificationMessageId {
		SetupChanged = 1,
		ObjectAdded,
		ObjectRemoved,
		PropertyChanged,
		ThruConnectionsChanged,
		SerialPortOwnerChanged,
		IOError,
	}

	//
	// The notification EventArgs
	//
	public class ObjectAddedOrRemovedEventArgs : EventArgs {
		public ObjectAddedOrRemovedEventArgs (MidiObject parent, MidiObject child)
		{
			Parent = parent;
			Child = child;
		}
		public MidiObject Parent { get; private set; }
		public MidiObject Child { get; private set; }
	}

	public class ObjectPropertyChangedEventArgs : EventArgs {
		public ObjectPropertyChangedEventArgs (MidiObject midiObject, string propertyName)
		{
			MidiObject = midiObject;
			PropertyName = propertyName;
		}
		public MidiObject MidiObject { get; private set; }
		public string PropertyName { get; private set; }
	}

	public class IOErrorEventArgs : EventArgs {
		public IOErrorEventArgs (MidiDevice device, int errorCode)
		{
			Device = device;
			ErrorCode = errorCode;
		}
		public MidiDevice Device { get; set; }
		public int ErrorCode { get; set; }
	}

	public class MidiPacketsEventArgs : EventArgs {
		IntPtr packetList;
		
		internal MidiPacketsEventArgs (IntPtr packetList)
		{
			this.packetList = packetList;
		}

		public IntPtr PacketListRaw {
			get {
				return packetList;
			}
		}

		public MidiPacket [] Packets {
			get {
				return MidiPort.ToPackets (packetList);
			}
		}
	}
}
