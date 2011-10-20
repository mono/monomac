using System;
using MonoMac.OpenGL;
using MonoMac.Foundation;

namespace GLSL15 {
	
	[Serializable] public class Scene  {
		
		RectangleRoundedGLSL iRectangleRoundedGLSL = new RectangleRoundedGLSL();
		
		private bool p_initialized = false;
		public bool initialized {
			get { return p_initialized; }
		}
		
		int programID = 0;
		
		public int initShader(ShaderType shaderTypeToUse, string shaderProgram) {
			int shaderID = 0;

			ErrorCode iErrorCodePrior = GL.GetError();
			if ( iErrorCodePrior != ErrorCode.NoError ) {
				throw new Exception ("Warning: An existing problem with the OpenGL state was found. Error code " + iErrorCodePrior.ToString() + ". This means an invalid call was made and not checked for before this part of the program was run.");
			}
			
			shaderID = GL.CreateShader(shaderTypeToUse);
			ErrorCode iErrorCode = GL.GetError();
			if ( iErrorCode != ErrorCode.NoError ) {
				throw new Exception ("There was a problem creating a shader of type " + shaderTypeToUse.ToString() + ". The error was '" + iErrorCode.ToString() + "'.");
			}
			
			if ( 0 == shaderID ) {
				throw new Exception ("There was a problem creating a shader of type " + shaderTypeToUse.ToString() + ". We were unable to query for the GL Error code.");
			}
			
			if ( string.Empty == shaderProgram ) {
				throw new Exception ("The shaderProgram property of ShaderCreate can not be empty. Please review the configuration");	
			}

			GL.ShaderSource(shaderID, shaderProgram);
			ErrorCode iErrorCode2 = GL.GetError();
			
			if ( iErrorCode2 != ErrorCode.NoError ) {
				switch (iErrorCode2) {
					case ErrorCode.InvalidOperation:
						throw new Exception ("Setting the shader source had a problem. The current shader compiler is not supported OR the shader ID, " + shaderID + ", is not a shader object (the handle ID is to something other than a shader.");
					case ErrorCode.InvalidValue:
						throw new Exception ("Setting the shader source had a problem. The shaderID provide, " + shaderID + ", was not generated by OpenGL or the shader program was empty (count of string length was 0). Please contact the developers as this should never happen.");
					default:
						throw new Exception ("Setting the shader source had a problem. . The error was '" + iErrorCode2.ToString() + "'.");
				}
					
			}
			
			GL.CompileShader(shaderID);
			
			string logInfo = GL.GetShaderInfoLog(shaderID);
			
			if ( string.Empty != logInfo ) {
				throw new Exception ("There was a problem compiling a shading program. Problems as follows '" + logInfo + "'. If the code looks fine, are you using the right shader type? For example, maybe you passed fragment code to a vertex shader?");	
			}
			
			return shaderID;
			
		}
		
		
		public void initProgram() {
			if ( false == initialized ) {
				
				ErrorCode iErrorCodePrior = GL.GetError();
				if ( iErrorCodePrior != ErrorCode.NoError ) {
					throw new Exception ("Warning: An existing problem with the OpenGL state was found. Error code " + iErrorCodePrior.ToString() + ". This means an invalid call was made and not checked for before this part of the program was run.");
				}
				
				programID = GL.CreateProgram();
				ErrorCode iErrorCode = GL.GetError();
				if ( iErrorCode != ErrorCode.NoError ) {
					throw new Exception ("There was a problem creating a shader program. The error was '" + iErrorCode.ToString() + "'.");
				}
				
				if ( 0 == programID ) {
					throw new Exception ("There was a problem creating a shader program. We were unable to query for the GL Error code.");
				}
				
				int vertexShader = initShader(ShaderType.VertexShader, @"
					#version 150
	
					in vec3 VertexPosition;
					in vec3 VertexColor;
					out vec3 Color;
	
					void main() {
						Color = VertexColor;
						gl_Position = vec4(VertexPosition,1.0);
					}
				");
				int fragmentShader = initShader(ShaderType.FragmentShader, @"
					#version 150
	
					in vec3 Color;
					layout (location = 0) out vec4 FragColor;
	
					void main() {
						FragColor = vec4(Color, 1.0);
					}
				");

				GL.AttachShader(programID, vertexShader);
				GL.AttachShader(programID, fragmentShader);
				
				GL.BindAttribLocation(programID, 0, "VertextPosition");
				GL.BindAttribLocation(programID, 1, "VertexColor");				

				GL.LinkProgram(programID);
				ErrorCode iErrorCode3 = GL.GetError();
				if ( iErrorCode3 != ErrorCode.NoError ) {
					throw new Exception ("There was a problem linking the program. The error was '" + iErrorCode3.ToString() + "'.");
				}
				
				iRectangleRoundedGLSL.init();
				
				p_initialized = true;	
			}			
		}
		
		
		/// <summary>
		/// All Setup for OpenGL for this scene goes here
		/// </summary>
		public void init() {
			if (!initialized) {
				initProgram();
				p_initialized = true;
			}
		}
		
		
		/// <summary>
		/// Set causes scene to draw. Set expects a camera
		/// </summary>
		public void show() {
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Clear The Screen And The Depth Buffer
//			scene.withObject = value; // causes all items to draw against this scene
			GL.UseProgram(programID);
			iRectangleRoundedGLSL.show();
		
		}

		
	}
}

