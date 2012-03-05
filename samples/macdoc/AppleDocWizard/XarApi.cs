using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace macdoc
{
	class XarApi
	{
		// A pity but the xar API doesn't seem thread-safe even for read operations like these
		// Otherwise possible algorithm is to setup a TaskScheduler.Default.MaxConcurrencyLevel amount of task
		// which rejoice on a barrier that sequentially extract xar_file_t from the iterator with each task then 
		// xar_extract'ing their own xar_file_t
		public static bool ExtractXar (string filename, string prefix, CancellationToken token, Action<string> pathCallback)
		{
			var cwdSave = Directory.GetCurrentDirectory ();
			Directory.SetCurrentDirectory (prefix);
			IntPtr xart = IntPtr.Zero;
			IntPtr iter = IntPtr.Zero;
			
			try {
				xart = xar_open (filename, 0);
				iter = xar_iter_new ();
			
				IntPtr file = xar_file_first (xart, iter);
				while (file != IntPtr.Zero) {
					if (token.IsCancellationRequested)
						return false;
					if (pathCallback != null)
						pathCallback (xar_get_path (file));
					if (xar_extract (xart, file) < 0)
						return false;
					file = xar_file_next (iter);
				}
			} finally {
				if (iter != IntPtr.Zero)
					xar_iter_free (iter);
				if (xart != IntPtr.Zero)
					xar_close (xart);
				Directory.SetCurrentDirectory (cwdSave);
			}
			
			return true;
		}
		
		[DllImport ("xar")]
		extern static IntPtr xar_open (string filename, int mode);
		
		[DllImport ("xar")]
		extern static IntPtr xar_close (IntPtr xart);
		
		[DllImport ("xar")]
		extern static IntPtr xar_iter_new ();
		
		[DllImport ("xar")]
		extern static void xar_iter_free (IntPtr iter);
		
		[DllImport ("xar")]
		extern static IntPtr xar_file_first (IntPtr xart, IntPtr iter);
		
		[DllImport ("xar")]
		extern static IntPtr xar_file_next (IntPtr iter);
		
		[DllImport ("xar")]
		extern static int xar_extract (IntPtr xart, IntPtr filet);
						
		[DllImport ("xar")]
		extern static string xar_get_path (IntPtr file);
	}
}

