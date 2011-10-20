using System;
using System.Runtime.InteropServices; // AllocHGlobal
using MonoMac.OpenGL;
using MonoMac.Foundation; // NSBundle

namespace GLSL15 {
    [Serializable] public class RectangleRoundedGLSL {
		
		protected bool p_initialized = false;
		public bool initialized {
			get {
				return p_initialized;
			}
		}

		int[] vboHandles = new int[2]; 
		int[] vaoHandles = new int[1];
		
		public void init() {
			if ( !initialized ) {

				float[] positionData = new float[9] {
					-0.8f, -0.8f, 0.0f,
					0.8f, -0.8f, 0.0f,
					0.0f,  0.8f, 0.0f };				

				float[] colorData = new float[9] {
				      1.0f, 0.0f, 0.0f,
				      0.0f, 1.0f, 0.0f,
				      0.0f, 0.0f, 1.0f };
				
				GL.GenBuffers(2, vboHandles);
				
				int positionBufferHandle = vboHandles[0];
				int colorBufferHandle = vboHandles[1];

				GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(positionData.Length * sizeof(float)), positionData, BufferUsageHint.StaticDraw);
				
				GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * sizeof(float)), colorData, BufferUsageHint.StaticDraw);
				
				GL.GenVertexArrays(1, vaoHandles);
				GL.BindVertexArray(vaoHandles[0]);
				
				GL.EnableVertexAttribArray(0);
				GL.EnableVertexAttribArray(1);
				
				GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
				GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false, 0, 0);
				
				GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
				GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float,false, 0, 0);					
				
				p_initialized = true;
			}
		}
		
		public void show() {
			GL.BindVertexArray(vaoHandles[0]);
			GL.DrawArrays(BeginMode.Triangles, 0, 3);
		}
		
	}
}
