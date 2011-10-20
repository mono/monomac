////=====================================================================
//// Code based on code in book OpenGL 4.0 Shading Language Cookbook by
//// David Wolff.
//// 
//// Implemented here by Eric Hosick just to show MonoMac using OpenGL 3.2
//// I'm not an OpenGL expert so I am sure there is a lot of hacking
//// going on here. Hopefully someone will come around and clean it up.
////
////
//// Permission is hereby granted, free of charge, to any person obtaining
//// a copy of this software and associated documentation files (the
//// "Software"), to deal in the Software without restriction, including
//// without limitation the rights to use, copy, modify, merge, publish,
//// distribute, sublicense, and/or sell copies of the Software, and to
//// permit persons to whom the Software is furnished to do so, subject to
//// the following conditions:
//// 
//// The above copyright notice and this permission notice shall be
//// included in all copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//// 
//using System;
//using System.Drawing;
//
//using MonoMac.Foundation;
//using MonoMac.AppKit;
//using MonoMac.OpenGL;
//
//namespace GLSL15
//{
//	public class GLSLExample
//	{
//		
//		public GLSLExample () : base()
//		{
//            // Other state
//            GL.Enable(EnableCap.DepthTest);
//            GL.ClearColor(System.Drawing.Color.MidnightBlue);
//		}
//		
//		public RectangleF p_bounds;
//		
//		
//		// Resize And Initialize The GL Window 
//		//      - See also the method in the MyOpenGLView Constructor about the NSView.NSViewGlobalFrameDidChangeNotification
//		public void ResizeGLScene (RectangleF bounds)
//		{
//			p_bounds = bounds;
//		}
//
//		// This creates a symmetric frustum.
//		// It converts to 6 params (l, r, b, t, n, f) for glFrustum()
//		// from given 4 params (fovy, aspect, near, far)
//		public static void Perspective (double fovY, double aspectRatio, double front, double back)
//		{
//			const
//			double DEG2RAD = Math.PI / 180 ; 
//
//			// tangent of half fovY
//			double tangent = Math.Tan (fovY / 2 * DEG2RAD);
//
//			// half height of near plane
//			double height = front * tangent;
//
//			// half width of near plane
//			double width = height * aspectRatio;
//
//			// params: left, right, bottom, top, near, far
//			GL.Frustum (-width, width, -height, height, front, back);
//			
//			
//		}
//		
//		int shaderCreate(ShaderType shaderType, string shaderProgram) {
//
//			ErrorCode iErrorCodePrior = GL.GetError();
//			if ( iErrorCodePrior != ErrorCode.NoError ) {
//				throw new Exception ("Warning: An existing problem with the OpenGL state was found. Error code " + iErrorCodePrior.ToString() + ". This means an invalid call was made and not checked for before this part of the program was run.");
//			}
//			
//			int shaderID = GL.CreateShader(shaderType);
//
//			ErrorCode iErrorCode = GL.GetError();
//			if ( iErrorCode != ErrorCode.NoError ) {
//				throw new Exception ("There was a problem creating a shader of type " + shaderType.ToString() + ". The error was '" + iErrorCode.ToString() + "'.");
//			}			
//			
//			if ( 0 == shaderID ) {
//				throw new Exception ("There was a problem creating a shader of type " + shaderType.ToString() + ". We were unable to query for the GL Error code.");
//			}
//			
//			if ( string.Empty == shaderProgram ) {
//				throw new Exception ("The shaderProgram property of ShaderCreate can not be empty. Please review the configuration");	
//			}
//
//			GL.ShaderSource(shaderID, shaderProgram);
//			ErrorCode iErrorCode2 = GL.GetError();					
//			if ( iErrorCode2 != ErrorCode.NoError ) {
//				throw new Exception ("There was a problem with the following shader to the program: '" + shaderProgram + "'. The error was '" + iErrorCode2.ToString() + "'.");
//			}
//			
//			GL.CompileShader(shaderID);
//			
//			string logInfo = GL.GetShaderInfoLog(shaderID);
//			
//			if ( string.Empty != logInfo ) {
//				throw new Exception ("There was a problem compiling a shading program. Problems as follows '" + logInfo + "'. If the code looks fine, are you using the right shader type? For example, maybe you passed fragment code to a vertex shader?");	
//			}			
//			
//			return shaderID;
//		}
//		
//		protected int shaderVertex;
//		protected int shaderFragment;
//
//		
//		void setupGLSL() {
//			
//			string vertexShaderGLSL = @"
//				#version 150
//	
//				in vec3 VertexPosition;
//				in vec3 VertexColor;
//				out vec3 Color;
//	
//				void main() {
//					Color = VertexColor;
//					gl_Position = vec4(VertexPosition,1.0);
//				}
//			";
//	
//			string fragmentShaderGLSL = @"
//				#version 150
//	
//				in vec3 Color;
//				layout (location = 0) out vec4 FragColor;
//	
//				void main() {
//					FragColor = vec4(Color, 1.0);
//				}
//			";			
//
//			shaderVertex = shaderCreate(ShaderType.VertexShader, vertexShaderGLSL);
//			shaderFragment = shaderCreate(ShaderType.VertexShader, fragmentShaderGLSL);			
//			
//		}
//		
//		protected int programID;
//		
//		void setupProgram() {
//			
//			programID = GL.CreateProgram();
//			
//			if ( 0 == programID ) {
//			
//				ErrorCode iErrorCode = GL.GetError();
//				if ( iErrorCode != ErrorCode.NoError ) {
//					throw new Exception ("There was a problem creating a shader program. The error was '" + iErrorCode.ToString() + "'.");
//				} else {
//					throw new Exception ("There was a problem creating a shader program. We were unable to query for the GL Error code.");
//				}
//			}
//			
//			GL.AttachShader(programID, shaderVertex);
//			GL.AttachShader(programID, shaderFragment);			
//			
//			GL.BindAttribLocation(programID, 0, "VertextPosition");
//			GL.BindAttribLocation(programID, 1, "VertexColor");			
//			
//
//
//			GL.LinkProgram(programID);
//			ErrorCode iErrorCode3 = GL.GetError();
//			if ( iErrorCode3 != ErrorCode.NoError ) {
//				throw new Exception ("There was a problem linking the program. The error was '" + iErrorCode3.ToString() + "'.");
//			}			
//			
//		}
//		
//		int[] vboHandles = new int[2]; 
//		int[] vaoHandles = new int[1];
//		
//		void setupTriangle() {
//			
//			float[] positionData = new float[9] {
//				-0.8f, -0.8f, 0.0f,
//				0.8f, -0.8f, 0.0f,
//				0.0f,  0.8f, 0.0f };				
//
//			float[] colorData = new float[9] {
//			      1.0f, 0.0f, 0.0f,
//			      0.0f, 1.0f, 0.0f,
//			      0.0f, 0.0f, 1.0f };
//			
//			GL.GenBuffers(2, vboHandles);
//			
//			int positionBufferHandle = vboHandles[0];
//			int colorBufferHandle = vboHandles[1];
//
//			GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
//			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(positionData.Length * sizeof(float)), positionData, BufferUsageHint.StaticDraw);
//			
//			GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
//			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * sizeof(float)), colorData, BufferUsageHint.StaticDraw);
//			
//			GL.GenVertexArrays(1, vaoHandles);
//			GL.BindVertexArray(vaoHandles[0]);
//			
//			GL.EnableVertexAttribArray(0);
//			GL.EnableVertexAttribArray(1);
//			
//			GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
//			GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false, 0, 0);
//			
//			GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
//			GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float,false, 0, 0);					
//		}
//		
//		public float aspectRatio {
//			get { return p_bounds.Width / p_bounds.Height; }
//		}		
//		
//		bool initialized = false;
//
//		void initialize() {
//			if ( false == initialized ) {
//				
//				setupGLSL();
//				setupProgram();
//				setupTriangle();
//				
//				GL.ClearDepth (1.0);					// Depth Buffer setup
//				GL.Enable (EnableCap.DepthTest);		// Enables Depth testing
//				GL.DepthFunc (DepthFunction.Lequal);	// The type of depth testing to do
//				
//				GL.Viewport( 0, 0, (int)p_bounds.Width, (int)p_bounds.Height); // Reset The Current Viewport
//				ErrorCode iErrorCodePrior = GL.GetError();
//
//				if ( iErrorCodePrior != ErrorCode.NoError ) {
//					throw new Exception ("Warning: An existing problem with the OpenGL state was found. Error code " + iErrorCodePrior.ToString() + ". This means an invalid call was made and not checked for before this part of the program was run.");
//				}
//				
//				initialized = true;
//			}
//			
//		}
//		
//		
//		// This method renders our scene and where all of your drawing code will go.
//		// The main thing to note is that we've factored the drawing code out of the NSView subclass so that
//		// the full-screen and non-fullscreen views share the same states for rendering 
//		public bool DrawGLScene (RectangleF bounds)
//		{
//			p_bounds = bounds;
//			initialize();
//			
//			
//			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Clear The Screen And The Depth Buffer
//			
//			GL.UseProgram(programID);
//
//			return true;
//		}
//
//	}
//}
//
//
//
//
