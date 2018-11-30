//
// ExtAudioFile.cs: ExtAudioFile wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
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
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.AudioToolbox;

namespace MonoMac.AudioUnit
{
    public enum ExtAudioFileError
    {
        OK = 0,
        CodecUnavailableInputConsumed    = -66559,
        CodecUnavailableInputNotConsumed = -66560,
        InvalidProperty          = -66561,
        InvalidPropertySize      = -66562,
        NonPCMClientFormat       = -66563,
        InvalidChannelMap        = -66564,
        InvalidOperationOrder    = -66565,
        InvalidDataFormat        = -66566,
        MaxPacketSizeUnknown     = -66567,
        InvalidSeek              = -66568,
        AsyncWriteTooLarge       = -66569,
        AsyncWriteBufferOverflow = -66570,

        // Shared error codes
        NotOpenError                  = -38,
        EndOfFileError                = -39,
        PositionError                 = -40,
        FileNotFoundError             = -43
    }

    public class ExtAudioFile : IDisposable
    {
        IntPtr _extAudioFile;

        public uint? ClientMaxPacketSize {
            get {
                uint size = sizeof (uint);
                uint value;
                if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.ClientMaxPacketSize, ref size, out value) != ExtAudioFileError.OK)
                    return null;

                return value;
            }
        }

        public uint? FileMaxPacketSize {
            get {
                uint size = sizeof (uint);
                uint value;

                if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.FileMaxPacketSize, ref size, out value) != ExtAudioFileError.OK)
                    return null;

                return value;
            }
        }


        public IntPtr? AudioFile {
            get {
                uint size = (uint) Marshal.SizeOf (typeof (IntPtr));
                IntPtr value;
                if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.AudioFile, ref size, out value) != ExtAudioFileError.OK)
                    return null;

                return value;
            }
        }

        public AudioConverter AudioConverter {
            get {
                uint size = sizeof (uint);
                IntPtr value;

                if (ExtAudioFileGetProperty (_extAudioFile, PropertyIDType.AudioConverter, ref size, out value) != ExtAudioFileError.OK)
                    return null;

                return new AudioConverter (value, false);
            }
        }

        public long FileLengthFrames {
            get {
                long length;
                uint size  =  sizeof (long);
                
                var err = ExtAudioFileGetProperty(_extAudioFile, PropertyIDType.FileLengthFrames, ref size, out length);
                if (err != 0)
                {
                    throw new InvalidOperationException(String.Format("Error code:{0}", err));
                }

                return length;
            }
        }

        public AudioStreamBasicDescription FileDataFormat
        {
            get
            {
                AudioStreamBasicDescription dc = new AudioStreamBasicDescription();
                uint size = (uint)Marshal.SizeOf(typeof(AudioStreamBasicDescription));
                int err = ExtAudioFileGetProperty(_extAudioFile, PropertyIDType.FileDataFormat, ref size, ref dc);
                if (err != 0)
                {
                    throw new InvalidOperationException(String.Format("Error code:{0}", err));
                }

                return dc;
            }
        }

        public AudioStreamBasicDescription ClientDataFormat
        {
            set
            {                
                int err = ExtAudioFileSetProperty(_extAudioFile, PropertyIDType.ClientDataFormat,
                    (uint)Marshal.SizeOf(value), ref value);
                if (err != 0)
                {
                    throw new InvalidOperationException(String.Format("Error code:{0}", err));
                }
            }
        }           

        private ExtAudioFile(IntPtr ptr)
        {
            _extAudioFile = ptr;
        }

        ~ExtAudioFile ()
        {
            Dispose (false);
        }

        public static ExtAudioFile OpenUrl (CFUrl url)
        {
            if (url == null)
                throw new ArgumentNullException ("url");

            ExtAudioFileError err;
            IntPtr ptr;
            unsafe {                
                err = ExtAudioFileOpenUrl(url.Handle, (IntPtr)(&ptr));
            }            
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Can not get object instance");
            }
            
            return new ExtAudioFile(ptr);
        }

        public static ExtAudioFile CreateWithUrl (CFUrl url,
            AudioFileType fileType, 
            AudioStreamBasicDescription inStreamDesc, 
            //AudioChannelLayout channelLayout, 
            AudioFileFlags flag)
        {             
            if (url == null)
                throw new ArgumentNullException ("url");

            int err;
            IntPtr ptr = new IntPtr();
            unsafe {                
                err = ExtAudioFileCreateWithUrl(url.Handle, fileType, ref inStreamDesc, IntPtr.Zero, (uint)flag,
                    (IntPtr)(&ptr));
            }            
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Can not get object instance");
            }
            
            return new ExtAudioFile(ptr);         
        }

        public static ExtAudioFileError WrapAudioFileID (IntPtr audioFileID, bool forWriting, out ExtAudioFile outAudioFile)
        {
            IntPtr ptr;
            ExtAudioFileError res;
            unsafe {                
                res = ExtAudioFileWrapAudioFileID (audioFileID, forWriting, (IntPtr)(&ptr));
            }            

            if (res != ExtAudioFileError.OK) {
                outAudioFile = null;
                return res;
            }

            outAudioFile = new ExtAudioFile (ptr);
            return res;
        }

        public void Seek(long frameOffset)
        {
            int err = ExtAudioFileSeek(_extAudioFile, frameOffset);
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
        }
        public long FileTell()
        {
            long frame = 0;
            
            int err = ExtAudioFileTell(_extAudioFile, ref frame);
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
            
            return frame;
        }

        [Obsolete ("Use overload with AudioBuffers")]
        public int Read(int numberFrames, AudioBufferList data)
        {
            if (data == null)
                throw new ArgumentNullException ("data");

            int err = ExtAudioFileRead(_extAudioFile, ref numberFrames, data);
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }

            return numberFrames;
        }

        public uint Read (uint numberFrames, AudioBuffers audioBufferList, out ExtAudioFileError status)
        {
            if (audioBufferList == null)
                throw new ArgumentNullException ("audioBufferList");

            status = ExtAudioFileRead (_extAudioFile, ref numberFrames, (IntPtr) audioBufferList);
            return numberFrames;
        }

        [Obsolete ("Use overload with AudioBuffers")]
        public void WriteAsync(int numberFrames, AudioBufferList data)
        {
            int err = ExtAudioFileWriteAsync(_extAudioFile, numberFrames, data);
            
            if (err != 0) {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }        
        }

        public ExtAudioFileError WriteAsync (uint numberFrames, AudioBuffers audioBufferList)
        {
            if (audioBufferList == null)
                throw new ArgumentNullException ("audioBufferList");

            return ExtAudioFileWriteAsync (_extAudioFile, numberFrames, (IntPtr) audioBufferList);
        }

        public ExtAudioFileError Write (uint numberFrames, AudioBuffers audioBufferList)
        {
            if (audioBufferList == null)
                throw new ArgumentNullException ("audioBufferList");

            return ExtAudioFileWrite (_extAudioFile, numberFrames, (IntPtr) audioBufferList);
        }

        public ExtAudioFileError SynchronizeAudioConverter ()
        {
            IntPtr value = IntPtr.Zero;
            return ExtAudioFileSetProperty (_extAudioFile, PropertyIDType.ConverterConfig,
                Marshal.SizeOf (value), value);
        }

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (_extAudioFile != IntPtr.Zero){
                ExtAudioFileDispose (_extAudioFile);
                _extAudioFile = IntPtr.Zero;
            }
        }

        #region Interop
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileOpenURL")]
        static extern ExtAudioFileError ExtAudioFileOpenUrl(IntPtr inUrl, IntPtr outExtAudioFile); // caution

        [DllImport (MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileWrapAudioFileID (IntPtr inFileID, bool inForWriting, IntPtr outExtAudioFile);    

        [Obsolete]
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileRead")]
        static extern int ExtAudioFileRead(IntPtr  inExtAudioFile, ref int ioNumberFrames, AudioBufferList ioData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileRead (IntPtr inExtAudioFile, ref uint ioNumberFrames, IntPtr ioData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileWrite (IntPtr inExtAudioFile, uint inNumberFrames, IntPtr ioData);                 

        [Obsolete]
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileWriteAsync")]
        static extern int ExtAudioFileWriteAsync(IntPtr inExtAudioFile, int inNumberFrames, AudioBufferList ioData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileWriteAsync(IntPtr inExtAudioFile, uint inNumberFrames, IntPtr ioData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileDispose")]
        static extern int ExtAudioFileDispose(IntPtr inExtAudioFile);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSeek")]
        static extern int ExtAudioFileSeek(IntPtr inExtAudioFile, long inFrameOffset);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileTell")]
        static extern int ExtAudioFileTell(IntPtr inExtAudioFile, ref long outFrameOffset);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileCreateWithURL")]
        static extern int ExtAudioFileCreateWithUrl(IntPtr inURL,
            [MarshalAs(UnmanagedType.U4)] AudioFileType inFileType,
            ref AudioStreamBasicDescription inStreamDesc,
            IntPtr inChannelLayout, //AudioChannelLayout inChannelLayout, AudioChannelLayout results in compilation error (error code 134.)
            UInt32 flags,
            IntPtr outExtAudioFile);            
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
        static extern int ExtAudioFileGetProperty(
            IntPtr inExtAudioFile, 
            PropertyIDType inPropertyID,
            ref uint ioPropertyDataSize,
            IntPtr outPropertyData);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
        static extern int ExtAudioFileGetProperty(
            IntPtr inExtAudioFile,
            PropertyIDType inPropertyID,
            ref uint ioPropertyDataSize,
            ref AudioStreamBasicDescription outPropertyData);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileGetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, ref uint ioPropertyDataSize, out IntPtr outPropertyData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileGetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, ref uint ioPropertyDataSize, out long outPropertyData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileGetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, ref uint ioPropertyDataSize, out uint outPropertyData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
        static extern ExtAudioFileError ExtAudioFileSetProperty (IntPtr inExtAudioFile, PropertyIDType inPropertyID, int ioPropertyDataSize, IntPtr outPropertyData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSetProperty")]
        static extern int ExtAudioFileSetProperty(
            IntPtr inExtAudioFile,
            PropertyIDType inPropertyID,
            uint ioPropertyDataSize,
            ref AudioStreamBasicDescription outPropertyData);
        
        enum PropertyIDType {                 
	        FileDataFormat		   = 0x66666d74,       // 'ffmt'
	        //kExtAudioFileProperty_FileChannelLayout		= 'fclo',   // AudioChannelLayout

            ClientDataFormat = 0x63666d74, //'cfmt',   // AudioStreamBasicDescription
	        //kExtAudioFileProperty_ClientChannelLayout	= 'cclo',   // AudioChannelLayout
	        CodecManufacturer	 	= 0x636d616e,      // 'cman'
	
	        // read-only:
            AudioConverter          = 0x61636e76,      // 'acnv'
	        AudioFile				= 0x6166696c,      // 'afil'
	        FileMaxPacketSize		= 0x666d7073,      // 'fmps'
	        ClientMaxPacketSize    	= 0x636d7073,      // 'cmps'
	        FileLengthFrames		= 0x2366726d,      // '#frm'
	
	        // writable:
	        ConverterConfig         = 0x61636366,      // 'accf'
	        //kExtAudioFileProperty_IOBufferSizeBytes		= 'iobs',	// UInt32
	        //kExtAudioFileProperty_IOBuffer				= 'iobf',	// void *
	        //kExtAudioFileProperty_PacketTable			= 'xpti'	// AudioFilePacketTableInfo             
        };
        #endregion
    }
}
