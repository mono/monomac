//
// Copyright 2010, Novell, Inc.
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
		static NSApplication () {
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof (NSObject).TypeHandle);
			class_ptr = Class.GetHandle ("NSApplication");
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static void NSApplicationMain (int argc, string [] argv);

		public static void Init ()
		{
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
			SynchronizationContext.SetSynchronizationContext (new AppKitSynchronizationContext ());
			
			NSApplicationMain (args.Length, args);
		}
	}
}
