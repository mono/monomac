using System;
using System.Xml.Serialization;
using System.Drawing;
using MonoMac.OpenGL;

namespace GLSL15 {
	public class GlCamera {
		public RectangleF bounds;
		public Scene scene { get; set; }
				
		public void init(RectangleF bounds) {
			scene.init();

			GL.ClearDepth (1.0);					// Depth Buffer setup
			GL.Enable (EnableCap.DepthTest);		// Enables Depth testing
			GL.DepthFunc (DepthFunction.Lequal);	// The type of depth testing to do
			GL.Viewport( 0, 0, (int)bounds.Width, (int)bounds.Height); // Reset The Current Viewport
		}

		public void showScene() {
			scene.show();
		}
	
	}
}
