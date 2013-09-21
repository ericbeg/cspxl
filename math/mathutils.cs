using System;
using OpenTK;


namespace pxl
{

	public static class Matrix4Extensions
	{
		public static Vector4 Multiply (this Matrix4 mat, Vector4 r)
		{
			Vector4 o = new Vector4 ();
			
			o.X = mat.M11 * r.X + mat.M12 * r.Y + mat.M13 * r.Z + mat.M14 * r.W;
			o.Y = mat.M21 * r.X + mat.M22 * r.Y + mat.M23 * r.Z + mat.M24 * r.W;
			o.Z = mat.M31 * r.X + mat.M32 * r.Y + mat.M33 * r.Z + mat.M34 * r.W;
			o.W = mat.M41 * r.X + mat.M42 * r.Y + mat.M43 * r.Z + mat.M44 * r.W;
			
			return o;
		}

		public static Vector3 Multiply (this Matrix4 mat, Vector3 r)
		{
			Vector4 o = mat.Multiply (new Vector4 (r.X, r.Y, r.Z, 1.0f));
			return (new Vector3 (o.X, o.Y, o.Z)) / o.W;
		}

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
		
		
		
	}
	
}


