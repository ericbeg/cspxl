using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using pxl;
namespace System.IO
{
    /// <summary>
    /// Some extensions methods to BinaryReader.
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reader Vector2.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            Vector2 v = new Vector2();
            v.x = reader.ReadSingle();
            v.y = reader.ReadSingle();
            return v;
        }

        /// <summary>
        /// Reader Vector3.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            Vector3 v = new Vector3();
            v.x = reader.ReadSingle();
            v.y = reader.ReadSingle();
            v.z = reader.ReadSingle();
            return v;
        }

        /// <summary>
        /// Reader Vector4.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            Vector4 v = new Vector4();
            v.x = reader.ReadSingle();
            v.y = reader.ReadSingle();
            v.z = reader.ReadSingle();
            v.w = reader.ReadSingle();
            return v;
        }

        /// <summary>
        /// Reader Quaternion.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            Quaternion q = new Quaternion();
            q.x = reader.ReadSingle();
            q.y = reader.ReadSingle();
            q.z = reader.ReadSingle();
            q.w = reader.ReadSingle();
            return q;
        }


        /// <summary>
        /// Reader Matrix4.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Matrix4 ReadMatrix4(this BinaryReader reader)
        {
            Matrix4 m = new Matrix4();

            m.m11 = reader.ReadSingle();
            m.m21 = reader.ReadSingle();
            m.m31 = reader.ReadSingle();
            m.m41 = reader.ReadSingle();

            m.m12 = reader.ReadSingle();
            m.m22 = reader.ReadSingle();
            m.m32 = reader.ReadSingle();
            m.m42 = reader.ReadSingle();

            m.m13 = reader.ReadSingle();
            m.m23 = reader.ReadSingle();
            m.m33 = reader.ReadSingle();
            m.m43 = reader.ReadSingle();

            m.m14 = reader.ReadSingle();
            m.m24 = reader.ReadSingle();
            m.m34 = reader.ReadSingle();
            m.m44 = reader.ReadSingle();

            return m;
        }


    }

}
