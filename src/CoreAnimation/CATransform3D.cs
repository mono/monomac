// 
// CATransform3D.cs: Implements the managed CATransform3D
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
//
using System;
using System.Runtime.InteropServices;
using MonoMac.CoreGraphics;

namespace MonoMac.CoreAnimation {

	[StructLayout(LayoutKind.Sequential)]
	public struct CATransform3D {
		public float m11, m12, m13, m14;
		public float m21, m22, m23, m24;
		public float m31, m32, m33, m34;
		public float m41, m42, m43, m44;

		static public readonly CATransform3D Identity;

		static CATransform3D ()
		{
			Identity = new CATransform3D ();
			Identity.m11 = 1f;
			Identity.m22 = 1f;
			Identity.m33 = 1f;
			Identity.m44 = 1f;
		}
		
		//[DllImport(Constants.QuartzLibrary)]

		[DllImport(Constants.QuartzLibrary)]
		extern static int CATransform3DIsIdentity (CATransform3D t);
		public bool IsIdentity {
			get {
				return CATransform3DIsIdentity (this) != 0;
			}
		}

		[DllImport(Constants.QuartzLibrary)]
		extern static int CATransform3DEqualToTransform (CATransform3D a, CATransform3D b);
		public bool Equals (CATransform3D other)
		{
			return CATransform3DEqualToTransform (this, other) != 0;
		}

		public override bool Equals (object other)
		{
			if (!(other is CATransform3D))
				return false;
			return CATransform3DEqualToTransform (this, (CATransform3D) other) != 0;
		}

		public override int GetHashCode ()
		{
			unsafe {
				int code = (int) m11;
				fixed (float *fp = &m11){
					int *ip = (int *) fp;
					for (int i = 1; i < 16; i++){
						code = code ^ ip [i];
					}
				}
				return code;
			}
		}

		// Transform matrix =  [1 0 0 0; 0 1 0 0; 0 0 1 0; tx ty tz 1]
		//[DllImport(Constants.QuartzLibrary)]
		//extern static CATransform3D CATransform3DMakeTranslation (float tx, float ty, float tz);
		public static CATransform3D MakeTranslation (float tx, float ty, float tz)
		{
			//return CATransform3DMakeTranslation (tx, ty, tz);
			CATransform3D r = Identity;
			r.m41 = tx;
			r.m42 = ty;
			r.m43 = tz;

			return r;
		}

		// Scales matrix = [sx 0 0 0; 0 sy 0 0; 0 0 sz 0; 0 0 0 1]
		//[DllImport(Constants.QuartzLibrary)]
		//extern static CATransform3D CATransform3DMakeScale (float sx, float sy, float sz);
		public static CATransform3D MakeScale (float sx, float sy, float sz)
		{
			CATransform3D r = Identity;
			r.m11 = sx;
			r.m22 = sy;
			r.m33 = sz;

			return r;
		}
		
		[DllImport(Constants.QuartzLibrary, EntryPoint="CATransform3DMakeRotation")]
		public extern static CATransform3D MakeRotation (float angle, float x, float y, float z);

		[DllImport(Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DTranslate (CATransform3D t, float tx, float ty, float tz);
		public CATransform3D Translate (float tx, float ty, float tz)
		{
			return CATransform3DTranslate (this, tx, ty, tz);
		}
		
		[DllImport(Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DScale (CATransform3D t, float sx, float sy, float sz);
		public CATransform3D Scale (float sx, float sy, float sz)
		{
			return CATransform3DScale (this, sx, sy, sz);
		}
		public CATransform3D Scale (float s)
		{
			return CATransform3DScale (this, s, s, s);
		}
		
		[DllImport(Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DRotate (CATransform3D t, float angle, float x, float y, float z);
		public CATransform3D Rotate (float angle, float x, float y, float z)
		{
			return CATransform3DRotate (this, angle, x, y, z);
		}
		
		[DllImport(Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DConcat (CATransform3D a, CATransform3D b);
		public CATransform3D Concat (CATransform3D b)
		{
			return CATransform3DConcat (this, b);
		}
		
		[DllImport(Constants.QuartzLibrary)]
		extern static CATransform3D CATransform3DInvert (CATransform3D t);
		public CATransform3D Invert (CATransform3D t)
		{
			return CATransform3DInvert (this);
		}
		
		[DllImport(Constants.QuartzLibrary, EntryPoint="CATransform3DMakeAffineTransform")]
		public extern static CATransform3D MakeFromAffine (CGAffineTransform m);
		

		[DllImport(Constants.QuartzLibrary)]
		extern static bool CATransform3DIsAffine (CATransform3D t);
		public bool IsAffine {
			get {
				return CATransform3DIsAffine (this);
			}
		}

		[DllImport(Constants.QuartzLibrary, EntryPoint="CATransform3DGetAffineTransform")]
		public extern static CGAffineTransform GetAffine (CATransform3D t);

		public override string ToString ()
		{
			return String.Format ("[{0} {1} {2} {3}; {4} {5} {6} {7}; {8} {9} {10} {11}; {12} {13} {14} {15}]",
					      m11, m12, m13, m14,
					      m21, m22, m23, m24,
					      m31, m32, m33, m34,
					      m41, m42, m43, m44);
		}
	}
}
