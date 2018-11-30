//
// AudioComponent.cs: AudioComponent wrapper class
//
// Author:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
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
using MonoMac.ObjCRuntime;
using MonoMac.AudioToolbox;
using MonoMac.CoreFoundation;

namespace MonoMac.AudioUnit
{
	public class AudioComponent : INativeObject {
		internal IntPtr handle;

		public IntPtr Handle { get { return handle; } }

		internal AudioComponent(IntPtr handle)
		{ 
			this.handle = handle;
		}

		public AudioUnit CreateAudioUnit ()
		{
			return new AudioUnit (this);
		}
		
		public static AudioComponent FindNextComponent (AudioComponent cmp, AudioComponentDescription cd)
		{
			// Getting component hanlder
			IntPtr handle;
			if (cmp == null)
				handle = AudioComponentFindNext(IntPtr.Zero, cd);
			else
				handle = AudioComponentFindNext(cmp.Handle, cd);
			
			// creating an instance
			if (handle != IntPtr.Zero)
				return new AudioComponent (handle);
			else
				return null;
		}

		public static AudioComponent FindComponent (AudioComponentDescription cd)
		{
			return FindNextComponent(null, cd);
		}

		public static AudioComponent FindComponent (AudioTypeOutput output)
		{
			return FindComponent (AudioComponentDescription.CreateOutput (output));
		}

		public static AudioComponent FindComponent (AudioTypeMusicDevice musicDevice )
		{
			return FindComponent (AudioComponentDescription.CreateMusicDevice (musicDevice));
		}
		
		public static AudioComponent FindComponent (AudioTypeConverter conveter)
		{
			return FindComponent (AudioComponentDescription.CreateConverter (conveter));
		}
		
		public static AudioComponent FindComponent (AudioTypeEffect effect)
		{
			return FindComponent (AudioComponentDescription.CreateEffect (effect));
		}
		
		public static AudioComponent FindComponent (AudioTypeMixer mixer)
		{
			return FindComponent (AudioComponentDescription.CreateMixer (mixer));
		}
		
		public static AudioComponent FindComponent (AudioTypePanner panner)
		{
			return FindComponent (AudioComponentDescription.CreatePanner (panner));
		}
		
		public static AudioComponent FindComponent (AudioTypeGenerator generator)
		{
			return FindComponent (AudioComponentDescription.CreateGenerator (generator));
		}

		[DllImport(MonoMac.Constants.AudioUnitLibrary, EntryPoint = "AudioComponentFindNext")]
		static extern IntPtr AudioComponentFindNext(IntPtr inComponent, AudioComponentDescription inDesc);

		[DllImport(MonoMac.Constants.AudioUnitLibrary, EntryPoint = "AudioComponentCopyName")]
		static extern int AudioComponentCopyName (IntPtr component, out IntPtr cfstr);
		
		public string Name {
			get {
				IntPtr r;
				if (AudioComponentCopyName (handle, out r) == 0)
					return CFString.FetchString (r);
				return null;
			}
		}

		[DllImport(MonoMac.Constants.AudioUnitLibrary, EntryPoint = "AudioComponentGetDescription")]
		static extern int AudioComponentGetDescription (IntPtr component, out AudioComponentDescription desc);
		public AudioComponentDescription Description {
			get {
				AudioComponentDescription desc;

				if (AudioComponentGetDescription (handle, out desc) == 0)
					return desc;
				return null;
			}
		}

		[DllImport(MonoMac.Constants.AudioUnitLibrary, EntryPoint = "AudioComponentCount")]
		static extern int AudioComponentCount (AudioComponentDescription desc);
		static int CountMatches (AudioComponentDescription desc)
		{
			if (desc == null)
				throw new ArgumentNullException ("desc");
			return AudioComponentCount (desc);
		}

		[DllImport(MonoMac.Constants.AudioUnitLibrary, EntryPoint = "AudioComponentGetVersion")]
		static extern int AudioComponentGetVersion (IntPtr component, out int version);

		public Version Version {
			get {
				int ret;
				if (AudioComponentGetVersion (handle, out ret) == 0)
					return new Version (ret >> 16, (ret >> 8) & 0xff, ret & 0xff);

				return null;
			}
		}
    }
}
