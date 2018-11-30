//
// CMBufferQueue.cs: Implements the CMBufferQueue and CMBuffer managed bindings
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012 Xamarin Inc
//
//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using OSStatus = System.Int32;

// FIXME64: this will change on 64 bit builds
//
using CMItemCount = System.Int32;

namespace MonoMac.CoreMedia {
	
	public delegate CMTime CMBufferGetTime (INativeObject buffer);
	public delegate bool   CMBufferGetBool (INativeObject buffer);
	public delegate int    CMBufferCompare (INativeObject first, INativeObject second);
	
	public class CMBufferQueue : INativeObject, IDisposable {
		GCHandle gch;
		Dictionary<IntPtr, INativeObject> queueObjects;
		internal IntPtr handle;
		CMBufferGetTime getDecodeTimeStamp;
		CMBufferGetTime getPresentationTimeStamp;
		CMBufferGetTime getDuration;
		CMBufferGetBool isDataReady;
		CMBufferCompare compare;
		
		delegate CMTime BufferGetTimeCallback (IntPtr bufferRef, IntPtr refCon);
		delegate bool   BufferGetBooleanCallback (IntPtr bufferRef, IntPtr refCon);
		delegate int    BufferCompareCallback (IntPtr buffer1, IntPtr buffer2, IntPtr refcon);
		
		[StructLayout (LayoutKind.Sequential)]
		struct CMBufferCallbacks {
			 internal uint version;
			 internal IntPtr refcon;
			 internal BufferGetTimeCallback XgetDecodeTimeStamp;
			 internal BufferGetTimeCallback XgetPresentationTimeStamp;
			 internal BufferGetTimeCallback XgetDuration;
			 internal BufferGetBooleanCallback XisDataReady;
			 internal BufferCompareCallback Xcompare;
			 internal IntPtr cfStringPtr_dataBecameReadyNotification;
		}

		// A version with no delegates, just native pointers
		[StructLayout (LayoutKind.Sequential)]
		struct CMBufferCallbacks2 {
			 internal uint version;
			 internal IntPtr refcon;
			 internal IntPtr XgetDecodeTimeStamp;
			 internal IntPtr XgetPresentationTimeStamp;
			 internal IntPtr XgetDuration;
			 internal IntPtr XisDataReady;
			 internal IntPtr Xcompare;
			 internal IntPtr cfStringPtr_dataBecameReadyNotification;
		}

		~CMBufferQueue ()
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
			queueObjects = null;
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
			if (gch.IsAllocated)
				gch.Free ();
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static OSStatus CMBufferQueueCreate (IntPtr allocator, CMItemCount capacity, CMBufferCallbacks cbacks, out IntPtr result);

		[DllImport(Constants.CoreMediaLibrary)]
		extern static OSStatus CMBufferQueueCreate (IntPtr allocator, CMItemCount capacity, IntPtr cbacks, out IntPtr result);

		internal CMBufferQueue (int count)
		{
			queueObjects = new Dictionary<IntPtr,INativeObject> (count);
			gch = GCHandle.Alloc (this);
		}
			
		public static CMBufferQueue FromCallbacks (int count, CMBufferGetTime getDecodeTimeStamp, CMBufferGetTime getPresentationTimeStamp, CMBufferGetTime getDuration,
							   CMBufferGetBool isDataReady, CMBufferCompare compare, NSString dataBecameReadyNotification)
		{
			var bq = new CMBufferQueue (count);
			var cbacks = new CMBufferCallbacks () {
				version = 0,
				refcon = GCHandle.ToIntPtr (bq.gch),
				XgetDecodeTimeStamp = getDecodeTimeStamp == null ? (BufferGetTimeCallback) null : GetDecodeTimeStamp,
				XgetPresentationTimeStamp = getPresentationTimeStamp == null ? (BufferGetTimeCallback) null : GetPresentationTimeStamp,
				XgetDuration = getDuration == null ? (BufferGetTimeCallback) null : GetDuration,
				XisDataReady = isDataReady == null ? (BufferGetBooleanCallback) null : GetDataReady,
				Xcompare = compare == null ? (BufferCompareCallback) null : Compare,
				cfStringPtr_dataBecameReadyNotification = dataBecameReadyNotification == null ? IntPtr.Zero : dataBecameReadyNotification.Handle
			};

			bq.getDecodeTimeStamp = getDecodeTimeStamp;
			bq.getPresentationTimeStamp = getPresentationTimeStamp;
			bq.getDuration = getDuration;
			bq.isDataReady = isDataReady;
			bq.compare = compare;

			if (CMBufferQueueCreate (IntPtr.Zero, count, cbacks, out bq.handle) == 0)
				return bq;

			return null;
		}

		[DllImport(Constants.CoreMediaLibrary)]
		unsafe extern static CMBufferCallbacks2 *CMBufferQueueGetCallbacksForUnsortedSampleBuffers ();
		
		public static CMBufferQueue CreateUnsorted (int count)
		{
			var bq = new CMBufferQueue (count);
			unsafe {
				var copy = *CMBufferQueueGetCallbacksForUnsortedSampleBuffers ();
				var pcopy = &copy;
				copy.refcon = GCHandle.ToIntPtr (bq.gch);
			
				if (CMBufferQueueCreate (IntPtr.Zero, count, (IntPtr) pcopy, out bq.handle) == 0)
					return bq;
			}
			return null;
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static OSStatus CMBufferQueueEnqueue (IntPtr queueHandle, IntPtr cfObjectHandle);
		
		//
		// It really should be ICFType, and we should pepper various classes with ICFType
		//
		public void Enqueue (INativeObject cftypeBuffer)
		{
			if (cftypeBuffer == null)
				throw new ArgumentNullException ("cftypeBuffer");
			lock (queueObjects){
				var cfh = cftypeBuffer.Handle;
				CMBufferQueueEnqueue (handle, cfh);
				if (!queueObjects.ContainsKey (cfh))
					queueObjects [cfh] = cftypeBuffer;
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMBufferQueueDequeueAndRetain (IntPtr queueHandle);
		
		public INativeObject Dequeue ()
		{
			//
			// Our managed objects already take a reference on the object,
			// and by keeping the objects alive in the `queueObjects'
			// dictionary, we kept the reference alive.   So we need to
			// release the newly acquired reference
			//
			var oHandle = CMBufferQueueDequeueAndRetain (handle);
			if (oHandle == IntPtr.Zero)
				return null;

			CFObject.CFRelease (oHandle);
			lock (queueObjects){
				var managed = queueObjects [oHandle];
				queueObjects.Remove (oHandle);
				return managed;
			}
		}

		// Surfaces the given buffer pointer to a managed object
		INativeObject Surface (IntPtr v)
		{
			return queueObjects [v];
		}

#if !MONOMAC
		[MonoPInvokeCallback (typeof (BufferGetTimeCallback))]
#endif
		static CMTime GetDecodeTimeStamp (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue) GCHandle.FromIntPtr (refcon).Target;
			return queue.getDecodeTimeStamp (queue.Surface (buffer));
		}

#if !MONOMAC
		[MonoPInvokeCallback (typeof (BufferGetTimeCallback))]
#endif
		static CMTime GetPresentationTimeStamp (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue) GCHandle.FromIntPtr (refcon).Target;
			return queue.getPresentationTimeStamp (queue.Surface (buffer));
		}
#if !MONOMAC
		[MonoPInvokeCallback (typeof (BufferGetTimeCallback))]
#endif
		static CMTime GetDuration (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue) GCHandle.FromIntPtr (refcon).Target;
			return queue.getDuration (queue.Surface (buffer));
		}
#if !MONOMAC
		[MonoPInvokeCallback (typeof (BufferGetBooleanCallback))]
#endif
		static bool GetDataReady (IntPtr buffer, IntPtr refcon)
		{
			var queue = (CMBufferQueue) GCHandle.FromIntPtr (refcon).Target;
			return queue.isDataReady (queue.Surface (buffer));
		}

#if !MONOMAC
	[MonoPInvokeCallback (typeof (BufferCompareCallback))]
#endif
		static int Compare (IntPtr buffer1, IntPtr buffer2, IntPtr refcon)
		{
			var queue = (CMBufferQueue) GCHandle.FromIntPtr (refcon).Target;
			return queue.compare (queue.Surface (buffer1), queue.Surface (buffer2));
		}

	}
}