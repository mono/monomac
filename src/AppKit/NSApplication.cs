//
// Copyright 2010, Novell, Inc.
// Copyright 2012 Xamarin Inc.
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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AppKit {
	public partial class NSApplication : NSResponder {
		public static bool CheckForIllegalCrossThreadCalls = true;
		private static Thread mainThread;

		static NSApplication () {
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof (NSObject).TypeHandle);
			class_ptr = Class.GetHandle ("NSApplication");
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static void NSApplicationMain (int argc, string [] argv);

		static bool initialized;

		public static void Init ()
		{
			if (initialized) {
				throw new InvalidOperationException ("Init has already be be invoked; it can only be invoke once");
			}

			initialized = true;

			var monomac = Assembly.GetExecutingAssembly ();
			Runtime.RegisterAssembly (monomac);

			var name = monomac.GetName ().ToString ();

			foreach (var a in AppDomain.CurrentDomain.GetAssemblies ()) {
				foreach (var r in a.GetReferencedAssemblies ()) {
					//FIXME: Mono's ReferenceMatchesDefinition isn't implemented, so check string values instead
					//if (AssemblyName.ReferenceMatchesDefinition (r, name)) {
					if (name == r.ToString ()) {
						Runtime.RegisterAssembly (a);
						break;
					}
				}
			}

			// Runtime hosts embedding MonoMac may use a different sync context 
			// and call NSApplicationMain externally prior to this Init, so only
			// initialize the context if it hasn't been set externally. Alternatively,
			// AppKitSynchronizationContext could be made public.
			if(SynchronizationContext.Current == null)
				SynchronizationContext.SetSynchronizationContext (new AppKitSynchronizationContext ());

			// Establish the main thread at the time of Init to support hosts
			// that don't call Main.
			NSApplication.mainThread = Thread.CurrentThread;

			// TODO:
			//   Install hook to register dynamically loaded assemblies
		}

		public static void InitDrawingBridge ()
		{
			FieldInfo UseCocoaDrawableField = Type.GetType ("System.Drawing.GDIPlus, System.Drawing").GetField ("UseCocoaDrawable", BindingFlags.Static | BindingFlags.Public);
			FieldInfo UseCarbonDrawableField = Type.GetType ("System.Drawing.GDIPlus, System.Drawing").GetField ("UseCarbonDrawable", BindingFlags.Static | BindingFlags.Public);

			UseCocoaDrawableField.SetValue (null, true);
			UseCarbonDrawableField.SetValue (null, false);
		}

		public static void Main (string [] args)
		{
			// Switch to an AppKitSynchronizationContext if Main is invoked
			if(SynchronizationContext.Current == null || !typeof(AppKitSynchronizationContext).IsAssignableFrom(SynchronizationContext.Current.GetType()))
				SynchronizationContext.SetSynchronizationContext (new AppKitSynchronizationContext ());

			// Init where this is set the first time is generally paired
			// with a call to Main, but this guarantees the right thread.
			NSApplication.mainThread = Thread.CurrentThread;

			NSApplicationMain (args.Length, args);
		}

		public static void EnsureUIThread()
		{
			if (NSApplication.CheckForIllegalCrossThreadCalls && NSApplication.mainThread != Thread.CurrentThread)
				throw new AppKitThreadAccessException();
		}

		// NSEventMask is a superset (64 bits) of the mask that can be used (32 bits)
		// NSEventMaskFromType is often used to convert from NSEventType to the mask
		public NSEvent NextEvent (NSEventMask mask, NSDate expiration, string mode, bool deqFlag)
		{
			return NextEvent ((uint) mask, expiration, mode, deqFlag);
		}

		public void DiscardEvents (NSEventMask mask, NSEvent lastEvent)
		{
			DiscardEvents ((uint) mask, lastEvent);
		}

		// note: if needed override the protected Get|Set methods
		public NSApplicationActivationPolicy ActivationPolicy { 
			get { return GetActivationPolicy (); }
			// ignore return value (bool)
			set { SetActivationPolicy (value); }
		}
	}
}
