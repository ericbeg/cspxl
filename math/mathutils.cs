#pragma  warning disable 1591

using System;
using OpenTK;


namespace pxl
{

	public static class Matrix4Extensions
	{
        /*
		public static Quaternion ToQuaternion (this Matrix4 m)
		{
			Quaternion q;
			
			float trace = 1 + m.M11 + m.M22 + m.M33;
			float S = 0;
			float X = 0;
			float Y = 0;
			float Z = 0;
			float W = 0;
			
			if (trace > 0.0000001) 
			{
				S = (float)Math.Sqrt (trace) * 2.0f;
				X = (m.M23 - m.M32) / S;
				Y = (m.M31 - m.M13) / S;
				Z = (m.M12 - m.M21) / S;
				W = 0.25f * S;
			}else 
			{
				if (m.M11 > m.M22 && m.M11 > m.M33)
				{
					// Column 0: 
					S = (float)Math.Sqrt (1.0 + m.M11 - m.M22 - m.M33) * 2.0f;
					X = 0.25f * S;
					Y = (m.M12 + m.M21) / S;
					Z = (m.M31 + m.M13) / S;
					W = (m.M23 - m.M32) / S;
				}
				else if (m.M22 > m.M33) 
				{
					// Column 1: 
					S = (float)Math.Sqrt (1.0 + m.M22 - m.M11 - m.M33) * 2.0f;
					X = (m.M12 + m.M21) / S;
					Y = 0.25f * S;
					Z = (m.M23 + m.M32) / S;
					W = (m.M31 - m.M13) / S;
				}
				else
				{
					// Column 2:
					S = (float)Math.Sqrt (1.0 + m.M33 - m.M11 - m.M22) * 2.0f;
					X = (m.M31 + m.M13) / S;
					Y = (m.M23 + m.M32) / S;
					Z = 0.25f * S;
					W = (m.M12 - m.M21) / S;
				}
			}
			q = new Quaternion (X, Y, Z, W);
			return q;
		}
        */

		public static Byte[] GetBytes( this Matrix4 mat)
		{

			Byte[] buffer = new byte[ 4*4*sizeof(float) ];

			Buffer.BlockCopy (BitConverter.GetBytes (mat.m11), 0, buffer,  0*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m21), 0, buffer,  1*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m31), 0, buffer,  2*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m41), 0, buffer,  3*sizeof(float), sizeof(float));

			Buffer.BlockCopy (BitConverter.GetBytes (mat.m12), 0, buffer,  4*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m22), 0, buffer,  5*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m32), 0, buffer,  6*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m42), 0, buffer,  7*sizeof(float), sizeof(float));

			Buffer.BlockCopy (BitConverter.GetBytes (mat.m13), 0, buffer,  8*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m23), 0, buffer,  9*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m33), 0, buffer,  10*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m43), 0, buffer,  11*sizeof(float), sizeof(float));
		
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m14), 0, buffer,  12*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m24), 0, buffer,  13*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m34), 0, buffer,  14*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (mat.m44), 0, buffer,  15*sizeof(float), sizeof(float));

			return buffer;
		}
	}

	public static class Vector2Extensions
	{
		public static Byte[] GetBytes(this Vector2 vec2)
		{
			Byte[] buffer = new byte[3*sizeof(float)];
			Buffer.BlockCopy (BitConverter.GetBytes (vec2.x), 0, buffer,  0*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (vec2.y), 0, buffer,  1*sizeof(float), sizeof(float));
			return buffer;
		}
	}

	public static class Vector3Extensions
	{
		public static Byte[] GetBytes(this Vector3 vec3)
		{
			Byte[] buffer = new byte[3*sizeof(float)];
			Buffer.BlockCopy (BitConverter.GetBytes (vec3.x), 0, buffer,  0*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (vec3.y), 0, buffer,  1*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (vec3.z), 0, buffer,  2*sizeof(float), sizeof(float));
			return buffer;
		}
	}

	public static class Vector4Extensions
	{
		public static Byte[] GetBytes(this Vector4 vec4)
		{
			Byte[] buffer = new byte[4*sizeof(float)];
			Buffer.BlockCopy (BitConverter.GetBytes (vec4.x), 0, buffer,  0*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (vec4.y), 0, buffer,  1*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (vec4.z), 0, buffer,  2*sizeof(float), sizeof(float));
			Buffer.BlockCopy (BitConverter.GetBytes (vec4.w), 0, buffer,  3*sizeof(float), sizeof(float));
			return buffer;
		}
	}

}


