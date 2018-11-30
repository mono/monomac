// 
// CFStream.cs:
//
// Authors:
//		Martin Baulig <martin.baulig@gmail.com>
//		Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright (C) 2012 Xamarin, Inc.
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
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using MonoMac.CoreServices;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation {

	[Flags]
	public enum CFStreamEventType {
		None = 0,
		OpenCompleted = 1,
		HasBytesAvailable = 2,
		CanAcceptBytes = 4,
		ErrorOccurred = 8,
		EndEncountered = 16
	}
	
	[StructLayout (LayoutKind.Sequential)]
	public struct CFStreamClientContext {
		public int Version;
		public IntPtr Info;
		IntPtr retain;
		IntPtr release;
		IntPtr copyDescription;
		
		public void Retain ()
		{
			if (retain == IntPtr.Zero || Info == IntPtr.Zero)
				return;
			
			CFReadStreamRef_InvokeRetain (retain, Info);
		}
		
		public void Release ()
		{
			if (release == IntPtr.Zero || Info == IntPtr.Zero)
				return;
			
			CFReadStreamRef_InvokeRelease (release, Info);
		}
		
		public override string ToString ()
		{
			if (copyDescription == IntPtr.Zero)
				return base.ToString ();
			
			var ptr = CFReadStreamRef_InvokeCopyDescription (copyDescription, Info);
			return ptr == IntPtr.Zero ? base.ToString () : new NSString (ptr).ToString ();
		}
		
		internal void Invoke (IntPtr callback, IntPtr stream, CFStreamEventType eventType)
		{
			if (callback == IntPtr.Zero)
				return;

			CFReadStreamRef_InvokeCallback (callback, stream, eventType, Info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate IntPtr RetainDelegate (IntPtr info);

		static IntPtr CFReadStreamRef_InvokeRetain (IntPtr retain, IntPtr info)
		{
			return ((RetainDelegate)Marshal.GetDelegateForFunctionPointer (retain, typeof (RetainDelegate))) (info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate void ReleaseDelegate (IntPtr info);

		static void CFReadStreamRef_InvokeRelease (IntPtr release, IntPtr info)
		{
			((ReleaseDelegate)Marshal.GetDelegateForFunctionPointer (release, typeof (ReleaseDelegate))) (info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate IntPtr CopyDescriptionDelegate (IntPtr info);

		static IntPtr CFReadStreamRef_InvokeCopyDescription (IntPtr copyDescription, IntPtr info)
		{
			return ((CopyDescriptionDelegate)Marshal.GetDelegateForFunctionPointer (copyDescription, typeof (CopyDescriptionDelegate))) (info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate void CallbackDelegate (IntPtr stream, CFStreamEventType eventType, IntPtr info);

		static void CFReadStreamRef_InvokeCallback (IntPtr callback, IntPtr stream, CFStreamEventType eventType, IntPtr info)
		{
			((CallbackDelegate)Marshal.GetDelegateForFunctionPointer (callback, typeof (CallbackDelegate))) (stream, eventType, info);
		}
	}

	public enum CFStreamStatus {
		NotOpen = 0,
		Opening,
		Open,
		Reading,
		Writing,
		AtEnd,
		Closed,
		Error
	}

	public abstract class CFStream : CFType, INativeObject, IDisposable {
		IntPtr handle;
		GCHandle gch;
		CFRunLoop loop;
		NSString loopMode;
		bool open, closed;

		#region Stream Constructors

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFStreamCreatePairWithSocket (IntPtr allocator, CFSocketNativeHandle socket,
		                                                 out IntPtr read, out IntPtr write);

#if !COREFX
		public static void CreatePairWithSocket (CFSocket socket, out CFReadStream readStream,
		                                         out CFWriteStream writeStream)
		{
			IntPtr read, write;
			CFStreamCreatePairWithSocket (IntPtr.Zero, socket.GetNative (), out read, out write);
			readStream = new CFReadStream (read);
			writeStream = new CFWriteStream (write);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFStreamCreatePairWithPeerSocketSignature (IntPtr allocator, ref CFSocketSignature sig, out IntPtr read, out IntPtr write);

		public static void CreatePairWithPeerSocketSignature (AddressFamily family, SocketType type,
		                                                      ProtocolType proto, IPEndPoint endpoint,
		                                                      out CFReadStream readStream,
		                                                      out CFWriteStream writeStream)
		{
			using (var address = new CFSocketAddress (endpoint)) {
				var sig = new CFSocketSignature (family, type, proto, address);
				IntPtr read, write;
				CFStreamCreatePairWithPeerSocketSignature (IntPtr.Zero, ref sig, out read, out write);
				readStream = new CFReadStream (read);
				writeStream = new CFWriteStream (write);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFStreamCreatePairWithSocketToCFHost (IntPtr allocator, IntPtr host, int port,
		                                                         out IntPtr read, out IntPtr write);

		public static void CreatePairWithSocketToHost (IPEndPoint endpoint,
		                                               out CFReadStream readStream,
		                                               out CFWriteStream writeStream)
		{
			using (var host = CFHost.Create (endpoint)) {
				IntPtr read, write;
				CFStreamCreatePairWithSocketToCFHost (
				IntPtr.Zero, host.Handle, endpoint.Port, out read, out write);
				readStream = new CFReadStream (read);
				writeStream = new CFWriteStream (write);
			}
		}
#endif
		

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFStreamCreatePairWithSocketToHost (IntPtr allocator, IntPtr host, int port,
		                                                       out IntPtr read, out IntPtr write);

		public static void CreatePairWithSocketToHost (string host, int port,
		                                               out CFReadStream readStream,
		                                               out CFWriteStream writeStream)
		{
			using (var str = new CFString (host)) {
				IntPtr read, write;
				CFStreamCreatePairWithSocketToHost (
					IntPtr.Zero, str.Handle, port, out read, out write);
				readStream = new CFReadStream (read);
				writeStream = new CFWriteStream (write);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFReadStreamCreateForHTTPRequest (IntPtr alloc, IntPtr request);

		public static CFHTTPStream CreateForHTTPRequest (CFHTTPMessage request)
		{
			var handle = CFReadStreamCreateForHTTPRequest (IntPtr.Zero, request.Handle);
			if (handle == IntPtr.Zero)
				return null;

			return new CFHTTPStream (handle);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static IntPtr CFReadStreamCreateForStreamedHTTPRequest (IntPtr alloc, IntPtr request, IntPtr body);

		public static CFHTTPStream CreateForStreamedHTTPRequest (CFHTTPMessage request, CFReadStream body)
		{
			var handle = CFReadStreamCreateForStreamedHTTPRequest (IntPtr.Zero, request.Handle, body.Handle);
			if (handle == IntPtr.Zero)
				return null;

			return new CFHTTPStream (handle);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFStreamCreateBoundPair (IntPtr alloc, out IntPtr readStream, out IntPtr writeStream, CFIndex transferBufferSize);

		public static void CreateBoundPair (out CFReadStream readStream, out CFWriteStream writeStream, int bufferSize)
		{
			IntPtr read, write;
			CFStreamCreateBoundPair (IntPtr.Zero, out read, out write, bufferSize);
			readStream = new CFReadStream (read);
			writeStream = new CFWriteStream (write);
		}

		#endregion

		#region Stream API

		public abstract CFException GetError ();

		protected void CheckError ()
		{
			var exc = GetError ();
			if (exc != null)
				throw exc;
		}

		public void Open ()
		{
			if (open || closed)
				throw new InvalidOperationException ();
			CheckHandle ();
			if (!DoOpen ()) {
				CheckError ();
				throw new InvalidOperationException ();
			}
			open = true;
		}

		protected abstract bool DoOpen ();

		public void Close ()
		{
			if (!open)
				return;
			CheckHandle ();
			if (loop != null) {
				DoSetClient (null, 0, IntPtr.Zero);
				UnscheduleFromRunLoop (loop, loopMode);
				loop = null;
				loopMode = null;
			}
			try {
				DoClose ();
			} finally {
				open = false;
				closed = true;
			}
		}

		protected abstract void DoClose ();

		public CFStreamStatus GetStatus ()
		{
			CheckHandle ();
			return DoGetStatus ();
		}

		protected abstract CFStreamStatus DoGetStatus ();

		internal IntPtr GetProperty (NSString name)
		{
			CheckHandle ();
			return DoGetProperty (name);
		}

		protected abstract IntPtr DoGetProperty (NSString name);

		protected abstract bool DoSetProperty (NSString name, INativeObject value);

		internal void SetProperty (NSString name, INativeObject value)
		{
			CheckHandle ();
			if (DoSetProperty (name, value))
				return;
			throw new InvalidOperationException (string.Format (
				"Cannot set property '{0}' on {1}.", name, GetType ().Name)
			);
		}

		#endregion

		#region Events

		public class StreamEventArgs : EventArgs {
			public CFStreamEventType EventType {
				get;
				private set;
			}

			public StreamEventArgs (CFStreamEventType type)
			{
				this.EventType = type;
			}

			public override string ToString ()
			{
				return string.Format ("[StreamEventArgs: EventType={0}]", EventType);
			}
		}

		public event EventHandler<StreamEventArgs> OpenCompletedEvent;
		public event EventHandler<StreamEventArgs> HasBytesAvailableEvent;
		public event EventHandler<StreamEventArgs> CanAcceptBytesEvent;
		public event EventHandler<StreamEventArgs> ErrorEvent;
		public event EventHandler<StreamEventArgs> ClosedEvent;

		protected virtual void OnOpenCompleted (StreamEventArgs args)
		{
			if (OpenCompletedEvent != null)
				OpenCompletedEvent (this, args);
		}

		protected virtual void OnHasBytesAvailableEvent (StreamEventArgs args)
		{
			if (HasBytesAvailableEvent != null)
				HasBytesAvailableEvent (this, args);
		}

		protected virtual void OnCanAcceptBytesEvent (StreamEventArgs args)
		{
			if (CanAcceptBytesEvent != null)
				CanAcceptBytesEvent (this, args);
		}

		protected virtual void OnErrorEvent (StreamEventArgs args)
		{
			if (ErrorEvent != null)
				ErrorEvent (this, args);
		}

		protected virtual void OnClosedEvent (StreamEventArgs args)
		{
			if (ClosedEvent != null)
				ClosedEvent (this, args);
		}

		#endregion

		protected abstract void ScheduleWithRunLoop (CFRunLoop loop, NSString mode);

		protected abstract void UnscheduleFromRunLoop (CFRunLoop loop, NSString mode);

		protected delegate void CFStreamCallback (IntPtr s, CFStreamEventType type, IntPtr info);

		[MonoPInvokeCallback (typeof(CFStreamCallback))]
		static void OnCallback (IntPtr s, CFStreamEventType type, IntPtr info)
		{
			var stream = GCHandle.FromIntPtr (info).Target as CFStream;
			stream.OnCallback (type);
		}

		protected virtual void OnCallback (CFStreamEventType type)
		{
			var args = new StreamEventArgs (type);
			switch (type) {
			case CFStreamEventType.OpenCompleted:
				OnOpenCompleted (args);
				break;
			case CFStreamEventType.CanAcceptBytes:
				OnCanAcceptBytesEvent (args);
				break;
			case CFStreamEventType.HasBytesAvailable:
				OnHasBytesAvailableEvent (args);
				break;
			case CFStreamEventType.ErrorOccurred:
				OnErrorEvent (args);
				break;
			case CFStreamEventType.EndEncountered:
				OnClosedEvent (args);
				break;
			}
		}

		public void EnableEvents (CFRunLoop runLoop, NSString runLoopMode)
		{
			if (open || closed || (loop != null))
				throw new InvalidOperationException ();
			CheckHandle ();

			loop = runLoop;
			loopMode = runLoopMode;

			var ctx = new CFStreamClientContext ();
			ctx.Info = GCHandle.ToIntPtr (gch);

			var args = CFStreamEventType.OpenCompleted |
				CFStreamEventType.CanAcceptBytes | CFStreamEventType.HasBytesAvailable |
				CFStreamEventType.CanAcceptBytes | CFStreamEventType.ErrorOccurred |
				CFStreamEventType.EndEncountered;

			var ptr = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (CFStreamClientContext)));
			try {
				Marshal.StructureToPtr (ctx, ptr, false);
				if (!DoSetClient (OnCallback, (int)args, ptr))
					throw new InvalidOperationException ("Stream does not support async events.");
			} finally {
				Marshal.FreeHGlobal (ptr);
			}

			ScheduleWithRunLoop (runLoop, runLoopMode);
		}

		protected abstract bool DoSetClient (CFStreamCallback callback, CFIndex eventTypes,
		                                     IntPtr context);

		protected CFStream (IntPtr handle)
		{
			this.handle = handle;
			gch = GCHandle.Alloc (this);
		}

		protected void CheckHandle ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException (GetType ().Name);
		}

		~CFStream ()
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
			if (disposing) {
				Close ();
				if (gch.IsAllocated)
					gch.Free ();
			}
			if (handle != IntPtr.Zero) {
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
	}
}
