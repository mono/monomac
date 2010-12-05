
namespace MonoMac.OpenGL {
	public class CGLContext : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CGLContext (IntPtr handle)
		{
			this.handle = handle;
			CGLRetainContext (this.handle);
		}

		[Preserve (Conditional=true)]
		internal CGLContext (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGLRetainContext (this.handle);
		}
		
		[DllImport (Constants.OpenGLLibrary)]
		extern static void CGLRetainContext (IntPtr handle);

		[DllImport (Constants.OpenGLLibrary)]
		extern static void CGLReleaseContext (IntPtr handle);

		[DllImport (Constants.OpenGLLibrary)]
		extern static CGLStatus CGLCreateContext (IntPtr pixelFormat, IntPtr sharedContext, out IntPtr result);
	}
}