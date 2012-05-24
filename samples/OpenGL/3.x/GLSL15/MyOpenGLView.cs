using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreVideo;
using MonoMac.OpenGL;

namespace GLSL15
{
	public partial class MyOpenGLView : MonoMac.AppKit.NSView
	{
		NSOpenGLContext openGLContext;
		NSOpenGLPixelFormat pixelFormat;
		MainWindowController controller;
		CVDisplayLink displayLink;
		NSObject notificationProxy;

		GlCamera iCamera = new GlCamera() {
			scene = new Scene() {
			}
		};

		[Export("initWithFrame:")]
		public MyOpenGLView (RectangleF frame) : this(frame, null)
		{
		}

		public MyOpenGLView (RectangleF frame, NSOpenGLContext context) : base(frame)
		{
//			var attribs = new object [] {
//				NSOpenGLPixelFormatAttribute.Accelerated,
//				NSOpenGLPixelFormatAttribute.NoRecovery,
//				NSOpenGLPixelFormatAttribute.DoubleBuffer,
//				NSOpenGLPixelFormatAttribute.ColorSize, 24,
//				NSOpenGLPixelFormatAttribute.DepthSize, 16 };

			var attribs = new object[] {
				NSOpenGLPixelFormatAttribute.OpenGLProfile,
				NSOpenGLPFAOpenGLProfile.Version3_2Core,
				NSOpenGLPixelFormatAttribute.DoubleBuffer,
				NSOpenGLPixelFormatAttribute.ColorSize, 24,
				NSOpenGLPixelFormatAttribute.DepthSize, 16 };
			
			
			pixelFormat = new NSOpenGLPixelFormat (attribs);
			
			if (pixelFormat == null)
				Console.WriteLine ("No OpenGL pixel format");
			
			// NSOpenGLView does not handle context sharing, so we draw to a custom NSView instead
			openGLContext = new NSOpenGLContext (pixelFormat, context);
			
			openGLContext.MakeCurrentContext ();
			
			// Synchronize buffer swaps with vertical refresh rate
			openGLContext.SwapInterval = true;
			
			// Initialize our newly created view.
			InitGL ();

			SetupDisplayLink();
			
			// Look for changes in view size
			// Note, -reshape will not be called automatically on size changes because NSView does not export it to override 
			notificationProxy = NSNotificationCenter.DefaultCenter.AddObserver (NSView.NSViewGlobalFrameDidChangeNotification, HandleReshape);
			
		}

		// All Setup For OpenGL Goes Here
		public bool InitGL ()
		{
			ErrorCode iErrorCodePrior = GL.GetError();
			if ( iErrorCodePrior != ErrorCode.NoError ) {
				throw new Exception ("Warning: An existing problem with the OpenGL state was found. Error code " + iErrorCodePrior.ToString() + ". This means an invalid call was made and not checked for before this part of the program was run.");
			}

			// Enable Texture Mapping
			// GL.Enable (EnableCap.Texture2D); OpenGL 3.2 deprecated http://www.opengl.org/discussion_boards/ubbthreads.php?ubb=showflat&Number=279600
			
			// Enables Smooth Shading  
			// GL.ShadeModel (ShadingModel.Smooth);
			
			
			// Set background color to black     
			GL.ClearColor (0, 0, 0, 0.5f);
			
			ErrorCode iErrorCode4 = GL.GetError();
			if ( iErrorCode4 != ErrorCode.NoError ) {
				throw new Exception ("Error code " + iErrorCode4.ToString() + ".");
			}
			
			
			// Setup Depth Testing

			// Depth Buffer setup
			GL.ClearDepth (1.0);
			
			ErrorCode iErrorCode5 = GL.GetError();
			if ( iErrorCode5 != ErrorCode.NoError ) {
				throw new Exception ("Error code " + iErrorCode5.ToString() + ".");
			}
			

			// Really Nice Perspective Calculations
			// GL.Hint (HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			
			GL.BlendFunc (BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			
			ErrorCode iErrorCode7 = GL.GetError();
			if ( iErrorCode7 != ErrorCode.NoError ) {
				throw new Exception ("Error code " + iErrorCode7.ToString() + ".");
			}
			
			GL.Enable (EnableCap.Blend);

			ErrorCode iErrorCode8 = GL.GetError();
			if ( iErrorCode8 != ErrorCode.NoError ) {
				throw new Exception ("Error code " + iErrorCode8.ToString() + ".");
			}
			
			return true;
		}
		
		private void DrawView ()
		{
			// This method will be called on both the main thread (through -drawRect:) and a secondary thread (through the display link rendering loop)
			// Also, when resizing the view, -reshape is called on the main thread, but we may be drawing on a secondary thread
			// Add a mutex around to avoid the threads accessing the context simultaneously 
			openGLContext.CGLContext.Lock ();
			
			// Make sure we draw to the right context
			openGLContext.MakeCurrentContext ();
			
			// Delegate to the scene object for rendering
			iCamera.showScene();

			openGLContext.FlushBuffer ();
			
			openGLContext.CGLContext.Unlock ();
		}		
		
		public override void DrawRect (RectangleF dirtyRect)
		{
			// Ignore if the display link is still running
			if (!displayLink.IsRunning && controller != null)
				DrawView ();
		}

		public override bool AcceptsFirstResponder ()
		{
			// We want this view to be able to receive key events
			return true;
		}

		public override void LockFocus ()
		{
			base.LockFocus ();
			if (openGLContext.View != this)
				openGLContext.View = this;
		}

		public override void KeyDown (NSEvent theEvent)
		{
			controller.KeyDown (theEvent);
		}

		public override void MouseDown (NSEvent theEvent)
		{
			controller.MouseDown (theEvent);
		}



		private void SetupDisplayLink ()
		{
			// Create a display link capable of being used with all active displays
			displayLink = new CVDisplayLink ();
			
			// Set the renderer output callback function
			displayLink.SetOutputCallback (MyDisplayLinkOutputCallback);
			
			// Set the display link for the current renderer
			CGLContext cglContext = openGLContext.CGLContext;
			CGLPixelFormat cglPixelFormat = PixelFormat.CGLPixelFormat;
			displayLink.SetCurrentDisplay (cglContext, cglPixelFormat);
			
		}

		public CVReturn MyDisplayLinkOutputCallback (CVDisplayLink displayLink, ref CVTimeStamp inNow, ref CVTimeStamp inOutputTime, CVOptionFlags flagsIn, ref CVOptionFlags flagsOut)
		{
			CVReturn result = GetFrameForTime (inOutputTime);
			
			return result;
		}


		private CVReturn GetFrameForTime (CVTimeStamp outputTime)
		{
			// There is no autorelease pool when this method is called because it will be called from a background thread
			// It's important to create one or you will leak objects
			using (NSAutoreleasePool pool = new NSAutoreleasePool ()) {
				
				// Update the animation
//				double current = DateTime.Now.TimeOfDay.TotalMilliseconds;
				
//				controller.Scene.advanceTimeBy ((float)(current - controller.RenderTime));
//				controller.RenderTime = (float)current;
				
				DrawView ();
			}
			
			return CVReturn.Success;
			
		}

		public NSOpenGLContext OpenGLContext {
			get { return openGLContext; }
		}

		public NSOpenGLPixelFormat PixelFormat {
			get { return pixelFormat; }
		}

		public MainWindowController MainController {
			set { controller = value; }
		}
		
		public void updateViewPort(RectangleF bounds)
		{
			iCamera.bounds = bounds;
			iCamera.init(bounds);
		}

		public void UpdateView ()
		{
			// This method will be called on the main thread when resizing, but we may be drawing on a secondary thread through the display link
			// Add a mutex around to avoid the threads accessing the context simultaneously
			openGLContext.CGLContext.Lock ();
			
			// Delegate to the scene object to update for a change in the view size
//			controller.Scene.setViewportRect (Bounds);
			updateViewPort(Bounds);
			
			
			openGLContext.Update ();
			
			openGLContext.CGLContext.Unlock ();
		}

		private void HandleReshape (NSNotification note)
		{
			UpdateView ();
		}

		public void StartAnimation ()
		{
			if (displayLink != null && !displayLink.IsRunning)
				displayLink.Start ();
		}

		public void StopAnimation ()
		{
			if (displayLink != null && displayLink.IsRunning)
				displayLink.Stop ();
		}
		
		// Clean up the notifications
		public void DeAllocate()
		{
			displayLink.Stop();
			displayLink.SetOutputCallback(null);
			
			NSNotificationCenter.DefaultCenter.RemoveObserver(notificationProxy); 
		}
	}
}

