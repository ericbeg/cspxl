﻿#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public struct Vector2
    {
        public float x, y;

        public Vector2(float[] vec ) : this( vec[0], vec[1] )  {}

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float Norm2()
        {
            return Norm2(this);
        }

        public float Norm()
        {
            return Norm(this);
        }

        public void Normalize()
        {
            float n = Norm();
            x /= n;
            y /= n;
        }

        public static float Norm2(Vector2 vec)
        {
            return
            vec.x * vec.x +
            vec.y * vec.y;
        }

        public static float Norm(Vector2 vec)
        {
            return
                (float)Math.Sqrt(
            vec.x * vec.x +
            vec.y * vec.y)
            ;
        }

        public Vector2 Normalized(Vector2 vec)
        {
            float n = vec.Norm();
            return new Vector2(vec.x / n, vec.y / n);
        }


        public static float Dot(Vector2 left, Vector2 right)
        {
            return
                left.x * right.x +
                left.y * right.y;
        }

        public static Vector3 Cross(Vector2 left, Vector2 right)
        {
            return
                Vector3.Cross(
                    new Vector3(left.x, left.y, 0.0f),
                    new Vector3(right.x, right.y, 0.0f)
                );
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.x + right.x,
                left.y + right.y
                );
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            Vector2 r = (1.0f - t) * a + t * b;
            return r;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.x - right.x,
                left.y - right.y
                );
        }

        public static Vector2 operator *(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.x * right.x,
                left.y * right.y
                );
        }

        public static Vector2 operator *(Vector2 left, float right)
        {
            return new Vector2(
                left.x * right,
                left.y * right
                );
        }

        public static Vector2 operator *(float left, Vector2 right)
        {
            return new Vector2(
                left * right.x,
                left * right.y
                );
        }

        public static Vector2 operator /(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.x / right.x,
                left.y / right.y
                );
        }

        public static readonly Vector2 xAxis = new Vector2(1.0f, 0.0f);
        public static readonly Vector2 yAxis = new Vector2(0.0f, 1.0f);

        public static readonly Vector2 zero = new Vector2(0.0f, 0.0f);
        public static readonly Vector2 one = new Vector2(1.0f, 1.0f);


    }

    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float[] vec ) : this( vec[0], vec[1], vec[2] )  {}

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float Norm2()
        {
            return Norm2(this);
        }

        public float Norm()
        {
            return Norm(this);
        }

        public void Normalize()
        {
            float n = Norm();
            x /= n;
            y /= n;
            z /= n;
        }

        public static float Norm2(Vector3 vec)
        {
            return
            vec.x * vec.x +
            vec.y * vec.y +
            vec.z * vec.z;
        }

        public static float Norm(Vector3 vec)
        {
            return
                (float)Math.Sqrt(
            vec.x * vec.x +
            vec.y * vec.y +
            vec.z * vec.z)
            ;
        }

        public static Vector3 Normalized(Vector3 vec)
        {
            float n = vec.Norm();
            return new Vector3(vec.x / n, vec.y / n, vec.z / n);
        }


        public static float Dot(Vector3 left, Vector3 right)
        {
            return
                left.x * right.x +
                left.y * right.y +
                left.z * right.z;
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            return
                new Vector3(
                left.y * right.z - left.z * right.y,
                left.z * right.x - left.x * right.z,
                left.x * right.y - left.y * right.x
                );

        }

        public static Vector3 Lerp( Vector3 a, Vector3 b, float t)
        {
            Vector3 r = (1.0f-t)*a + t*b;
            return r;
        }

        public static float Angle(Vector3 a, Vector3 b)
        {
            float div = Vector3.Norm(a) * Vector3.Norm(b);

            if (div == 0.0f)
                return 0.0f;

            float dot_ab = Vector3.Dot(a, b);

            if (!(dot_ab < div))
            {
                return 0.0f;
            }

            return (float)Math.Acos(dot_ab / div);
        }


        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.x + right.x,
                left.y + right.y,
                left.z + right.z
                );
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.x - right.x,
                left.y - right.y,
                left.z - right.z
                );
        }

        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.x * right.x,
                left.y * right.y,
                left.z * right.z
                );
        }

        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.x / right.x,
                left.y / right.y,
                left.z / right.z
                );
        }

        public static Vector3 operator +(Vector3 left, float right)
        {
            return new Vector3(
                left.x + right,
                left.y + right,
                left.z + right
                );
        }

        public static Vector3 operator +(float left, Vector3 right)
        {
            return new Vector3(
                right.x + left,
                right.y + left,
                right.z + left
                );
        }


        public static Vector3 operator -(Vector3 left, float right)
        {
            return new Vector3(
                left.x - right,
                left.y - right,
                left.z - right
                );
        }

        public static Vector3 operator *(Vector3 r, float left)
        {
            return new Vector3(
                r.x * left,
                r.y * left,
                r.z * left
                );
        }

        public static Vector3 operator *(float left, Vector3 right)
        {
            return new Vector3(
                right.x * left,
                right.y * left,
                right.z * left
                );
        }

        public static Vector3 operator /(Vector3 left, float right)
        {
            return new Vector3(
                left.x / right,
                left.y / right,
                left.z / right
                );
        }


        
        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static readonly Vector3 xAxis = new Vector3(1.0f, 0.0f, 0.0f);
        public static readonly Vector3 yAxis = new Vector3(0.0f, 1.0f, 0.0f);
        public static readonly Vector3 zAxis = new Vector3(0.0f, 0.0f, 1.0f);

        public static readonly Vector3 one  = new Vector3(1.0f, 1.0f, 1.0f);
        public static readonly Vector3 zero = new Vector3(0.0f, 0.0f, 0.0f);

    }

    public struct Vector4
    {
        public float x, y, z, w;

        public Vector3 xyz
        {
            get
            {
                return new Vector3(x, y, z);
            }

            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }

        }

        public Vector4(float[] vec ) : this( vec[0], vec[1], vec[2], vec[3] )  {}

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float Norm2()
        {
            return Norm2(this);
        }

        public float Norm()
        {
            return Norm(this);
        }

        public static float Norm2(Vector4 vec)
        {
            return
            vec.x * vec.x +
            vec.y * vec.y +
            vec.z * vec.z +
            vec.w * vec.w;
        }

        public static float Norm(Vector4 vec)
        {
            return
                (float)Math.Sqrt(
            vec.x * vec.x +
            vec.y * vec.y +
            vec.z * vec.z +
            vec.w * vec.w)
            ;
        }


        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            return new Vector4(
                left.x + right.x,
                left.y + right.y,
                left.z + right.z,
                left.w + right.w
                );
        }

        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            return new Vector4(
                left.x - right.x,
                left.y - right.y,
                left.z - right.z,
                left.w - right.w
                );
        }

        public static Vector4 operator *(Vector4 left, Vector4 right)
        {
            return new Vector4(
                left.x * right.x,
                left.y * right.y,
                left.z * right.z,
                left.w * right.w
                );
        }

        public static Vector4 operator /(Vector4 left, Vector4 right)
        {
            return new Vector4(
                left.x / right.x,
                left.y / right.y,
                left.z / right.z,
                left.w / right.w
                );
        }


        public static Vector4 operator +(Vector4 left, float right)
        {
            return new Vector4(
                left.x + right,
                left.y + right,
                left.z + right,
                left.w + right
                );
        }

        public static Vector4 operator -(Vector4 left, float right)
        {
            return new Vector4(
                left.x - right,
                left.y - right,
                left.z - right,
                left.w - right
                );
        }

        public static Vector4 operator *(Vector4 left, float right)
        {
            return new Vector4(
                left.x * right,
                left.y * right,
                left.z * right,
                left.w * right
                );
        }

        public static Vector4 operator /(Vector4 left, float right)
        {
            return new Vector4(
                left.x / right,
                left.y / right,
                left.z / right,
                left.w / right
                );
        }

        public static readonly Vector4 xAxis = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
        public static readonly Vector4 yAxis = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
        public static readonly Vector4 zAxis = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
        public static readonly Vector4 wAxis = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        public static readonly Vector4 zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        public static readonly Vector4 one  = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);


    }

    public class Matrix3
    {
        public float m11, m12, m13;
        public float m21, m22, m23;
        public float m31, m32, m33;
        public Matrix3(float[] m)
        :this(
            m[0], m[3], m[6],  
            m[1], m[4], m[7],   
            m[2], m[5], m[8]
            ){}

        public Matrix3(
            float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33
            )
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }



    }

    public struct Matrix4
    {
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;

        public Vector4 column1
        {
            get
            {
                return new Vector4(m11, m21, m31, m41);
            }

            set
            {
                Vector4 v = value;
                m11 = v.x;
                m21 = v.y;
                m31 = v.z;
                m41 = v.w;
            }
        }

        public Vector4 column2
        {
            get
            {
                return new Vector4(m12, m22, m32, m42);
            }

            set
            {
                Vector4 v = value;
                m12 = v.x;
                m22 = v.y;
                m32 = v.z;
                m42 = v.w;
            }
        }

        public Vector4 column3
        {
            get
            {
                return new Vector4(m13, m23, m33, m43);
            }

            set
            {
                Vector4 v = value;
                m13 = v.x;
                m23 = v.y;
                m33 = v.z;
                m43 = v.w;
            }
        }

        public Vector4 column4
        {
            get
            {
                return new Vector4(m14, m24, m34, m44);
            }

            set
            {
                Vector4 v = value;
                m14 = v.x;
                m24 = v.y;
                m34 = v.z;
                m44 = v.w;
            }
        }

        public Matrix3 sub3
        {
            get
            {
                return new Matrix3(
                    m11, m12, m13,
                    m21, m22, m23,
                    m31, m32, m33
                    );
            }

            set
            {
                m11 = value.m11; m12 = value.m12; m13 = value.m13;
                m21 = value.m21; m22 = value.m22; m23 = value.m23;
                m31 = value.m31; m32 = value.m32; m33 = value.m33;
            }
        }


        public Matrix4(float[] m)
        :this(
            m[0], m[4], m[8], m[12],  
            m[1], m[5], m[9], m[13],  
            m[2], m[6], m[10], m[14],  
            m[3], m[7], m[11], m[15] 
            ){}

        public Matrix4(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44
            )
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13; this.m14 = m14;
            this.m21 = m21; this.m22 = m22; this.m23 = m23; this.m24 = m24;
            this.m31 = m31; this.m32 = m32; this.m33 = m33; this.m34 = m34;
            this.m41 = m41; this.m42 = m42; this.m43 = m43; this.m44 = m44;
        }



        public static Matrix4 operator +(Matrix4 left, Matrix4 right)
        {
            return new Matrix4(
                left.m11 + right.m11, left.m12 + right.m12, left.m13 + right.m13, left.m14 + right.m14,
                left.m21 + right.m21, left.m22 + right.m22, left.m23 + right.m23, left.m24 + right.m24,
                left.m31 + right.m31, left.m32 + right.m32, left.m33 + right.m33, left.m34 + right.m34,
                left.m41 + right.m41, left.m42 + right.m42, left.m43 + right.m43, left.m44 + right.m44
                );
        }

        public static Matrix4 operator -(Matrix4 left, Matrix4 right)
        {
            return new Matrix4(
                left.m11 - right.m11, left.m12 - right.m12, left.m13 - right.m13, left.m14 - right.m14,
                left.m21 - right.m21, left.m22 - right.m22, left.m23 - right.m23, left.m24 - right.m24,
                left.m31 - right.m31, left.m32 - right.m32, left.m33 - right.m33, left.m34 - right.m34,
                left.m41 - right.m41, left.m42 - right.m42, left.m43 - right.m43, left.m44 - right.m44
                );
        }

        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return new Matrix4(
                left.m11 * right.m11 + left.m12 * right.m21 + left.m13 * right.m31 + left.m14 * right.m41,
                left.m11 * right.m12 + left.m12 * right.m22 + left.m13 * right.m32 + left.m14 * right.m42,
                left.m11 * right.m13 + left.m12 * right.m23 + left.m13 * right.m33 + left.m14 * right.m43,
                left.m11 * right.m14 + left.m12 * right.m24 + left.m13 * right.m34 + left.m14 * right.m44,

                left.m21 * right.m11 + left.m22 * right.m21 + left.m23 * right.m31 + left.m24 * right.m41,
                left.m21 * right.m12 + left.m22 * right.m22 + left.m23 * right.m32 + left.m24 * right.m42,
                left.m21 * right.m13 + left.m22 * right.m23 + left.m23 * right.m33 + left.m24 * right.m43,
                left.m21 * right.m14 + left.m22 * right.m24 + left.m23 * right.m34 + left.m24 * right.m44,

                left.m31 * right.m11 + left.m32 * right.m21 + left.m33 * right.m31 + left.m34 * right.m41,
                left.m31 * right.m12 + left.m32 * right.m22 + left.m33 * right.m32 + left.m34 * right.m42,
                left.m31 * right.m13 + left.m32 * right.m23 + left.m33 * right.m33 + left.m34 * right.m43,
                left.m31 * right.m14 + left.m32 * right.m24 + left.m33 * right.m34 + left.m34 * right.m44,

                left.m41 * right.m11 + left.m42 * right.m21 + left.m43 * right.m31 + left.m44 * right.m41,
                left.m41 * right.m12 + left.m42 * right.m22 + left.m43 * right.m32 + left.m44 * right.m42,
                left.m41 * right.m13 + left.m42 * right.m23 + left.m43 * right.m33 + left.m44 * right.m43,
                left.m41 * right.m14 + left.m42 * right.m24 + left.m43 * right.m34 + left.m44 * right.m44);
        }

        public static Matrix4 operator /(Matrix4 left, float right)
        {
            return new Matrix4(
                left.m11 / right, left.m12 / right, left.m13 / right, left.m14 / right,
                left.m21 / right, left.m22 / right, left.m23 / right, left.m24 / right,
                left.m31 / right, left.m32 / right, left.m33 / right, left.m34 / right,
                left.m41 / right, left.m42 / right, left.m43 / right, left.m44 / right
                );
        }

        public static Vector3 operator *(Matrix4 mat, Vector3 r)
        {
            Vector3 o = new Vector3();

            o.x = mat.m11 * r.x + mat.m12 * r.y + mat.m13 * r.z + mat.m14 * 1.0f;
            o.y = mat.m21 * r.x + mat.m22 * r.y + mat.m23 * r.z + mat.m24 * 1.0f;
            o.z = mat.m31 * r.x + mat.m32 * r.y + mat.m33 * r.z + mat.m34 * 1.0f;
            //o.w = mat.m41 * r.x + mat.m42 * r.y + mat.m43 * r.z + mat.m44 * 1.0f;

            return o;
        }

        public static Vector4 operator *(Matrix4 mat, Vector4 r)
        {
            Vector4 o = new Vector4();

            o.x = mat.m11 * r.x + mat.m12 * r.y + mat.m13 * r.z + mat.m14 * r.w;
            o.y = mat.m21 * r.x + mat.m22 * r.y + mat.m23 * r.z + mat.m24 * r.w;
            o.z = mat.m31 * r.x + mat.m32 * r.y + mat.m33 * r.z + mat.m34 * r.w;
            o.w = mat.m41 * r.x + mat.m42 * r.y + mat.m43 * r.z + mat.m44 * r.w;

            return o;
        }


        public static Matrix4 Inverse(Matrix4 m)
        {

            float kplo = m.m33 * m.m44 - m.m34 * m.m43; float jpln = m.m32 * m.m44 - m.m34 * m.m42; float jokn = m.m32 * m.m43 - m.m33 * m.m42;
            float gpho = m.m23 * m.m44 - m.m24 * m.m43; float fphn = m.m22 * m.m44 - m.m24 * m.m42; float fogn = m.m22 * m.m43 - m.m23 * m.m42;
            float glhk = m.m23 * m.m34 - m.m24 * m.m33; float flhj = m.m22 * m.m34 - m.m24 * m.m32; float fkgj = m.m22 * m.m33 - m.m23 * m.m32;
            float iplm = m.m31 * m.m44 - m.m34 * m.m41; float iokm = m.m31 * m.m43 - m.m33 * m.m41; float ephm = m.m21 * m.m44 - m.m24 * m.m41;
            float injm = m.m31 * m.m42 - m.m32 * m.m41; float eogm = m.m21 * m.m43 - m.m23 * m.m41; float elhi = m.m21 * m.m34 - m.m24 * m.m31;
            float ekgi = m.m21 * m.m33 - m.m23 * m.m31; float enfm = m.m21 * m.m42 - m.m22 * m.m41; float ejfi = m.m21 * m.m32 - m.m22 * m.m31;


            float det = m.m11 * (m.m22 * (kplo) - m.m23 * (jpln) + m.m24 * (jokn))
                   - m.m12 * (m.m21 * (kplo) - m.m23 * (iplm) + m.m24 * (iokm))
                   + m.m13 * (m.m21 * (jpln) - m.m22 * (iplm) + m.m24 * (injm))
                   - m.m14 * (m.m21 * (jokn) - m.m22 * (iokm) + m.m23 * (injm));

            if (det == 0.0f)
            {
                /*
                return new Matrix4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
                */
                throw new Matrix4NotInvertibleException();
            }

            return new Matrix4(
            (m.m22 * (kplo) - m.m23 * (jpln) + m.m24 * (jokn)),
            (-m.m12 * (kplo) + m.m13 * (jpln) - m.m14 * (jokn)),
            (m.m12 * (gpho) - m.m13 * (fphn) + m.m14 * (fogn)),
            (-m.m12 * (glhk) + m.m13 * (flhj) - m.m14 * (fkgj)),

            (-m.m21 * (kplo) + m.m23 * (iplm) - m.m24 * (iokm)),
            (m.m11 * (kplo) - m.m13 * (iplm) + m.m14 * (iokm)),
            (-m.m11 * (gpho) + m.m13 * (ephm) - m.m14 * (eogm)),
            (m.m11 * (glhk) - m.m13 * (elhi) + m.m14 * (ekgi)),

            (m.m21 * (jpln) - m.m22 * (iplm) + m.m24 * (injm)),
            (-m.m11 * (jpln) + m.m12 * (iplm) - m.m14 * (injm)),
            (m.m11 * (fphn) - m.m12 * (ephm) + m.m14 * (enfm)),
            (-m.m11 * (flhj) + m.m12 * (elhi) - m.m14 * (ejfi)),

            (-m.m21 * (jokn) + m.m22 * (iokm) - m.m23 * (injm)),
            (m.m11 * (jokn) - m.m12 * (iokm) + m.m13 * (injm)),
            (-m.m11 * (fogn) + m.m12 * (eogm) - m.m13 * (enfm)),
            (m.m11 * (fkgj) - m.m12 * (ekgi) + m.m13 * (ejfi))
            ) / det
            ;
        }


        public static readonly Matrix4 Identity = new Matrix4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
                );

        public static Matrix4 Translate(Vector3 displacement)
        {
            return new Matrix4(
                1.0f, 0.0f, 0.0f, displacement.x,
                0.0f, 1.0f, 0.0f, displacement.y,
                0.0f, 0.0f, 1.0f, displacement.z,
                0.0f, 0.0f, 0.0f, 1.0f
                );
        }


        public static Matrix4 Transpose(Matrix4 m)
        {
            return new Matrix4(
                m.m11, m.m21, m.m31, m.m41,
                m.m12, m.m22, m.m32, m.m42,
                m.m13, m.m23, m.m33, m.m43,
                m.m14, m.m24, m.m34, m.m44
                );
        }

        public static Matrix4 Scale(Vector3 scale)
        {
            return new Matrix4(
                scale.x, 0.0f, 0.0f, 0.0f,
                0.0f, scale.y, 0.0f, 0.0f,
                0.0f, 0.0f, scale.z, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
                );
        }

        public static Matrix4 Rotate(Quaternion rotation)
        {
            // ripped from from Blender2.5
            float q0, q1, q2, q3, qda, qdb, qdc, qaa, qab, qac, qbb, qbc, qcc;

            float M_SQRT2 = (float)Math.Sqrt(2.0);
            q0 = M_SQRT2 * rotation.w;
            q1 = M_SQRT2 * rotation.x;
            q2 = M_SQRT2 * rotation.y;
            q3 = M_SQRT2 * rotation.z;

            qda = q0 * q1;
            qdb = q0 * q2;
            qdc = q0 * q3;
            qaa = q1 * q1;
            qab = q1 * q2;
            qac = q1 * q3;
            qbb = q2 * q2;
            qbc = q2 * q3;
            qcc = q3 * q3;

            Matrix4 m = new Matrix4();
            m.m11 = (1.0f - qbb - qcc);
            m.m12 = (qdc + qab);
            m.m13 = (-qdb + qac);
            m.m14 = 0.0f;

            m.m21 = (-qdc + qab);
            m.m22 = (1.0f - qaa - qcc);
            m.m23 = (qda + qbc);
            m.m24 = 0.0f;

            m.m31 = (qdb + qac);
            m.m32 = (-qda + qbc);
            m.m33 = (1.0f - qaa - qbb);
            m.m34 = 0.0f;

            m.m41 = m.m42 = m.m43 = 0.0f;
            m.m44 = 1.0f;

            return m;

        }

        public static Matrix4 OrthographicProjection(
         float left,
         float right,
         float bottom,
         float top,
         float zNear,
         float zFar
            )
        {
            float tx = -(right + left) / (right - left);
            float ty = -(top + bottom) / (top - bottom);
            float tz = -(zFar + zNear) / (zFar - zNear);

            return new Matrix4(
            2.0f / (right - left), 0.0f, 0.0f, tx,
            0.0f, 2.0f / (top - bottom), 0.0f, ty,
            0.0f, 0.0f, -2.0f / (zFar - zNear), tz,
            0.0f, 0.0f, 0.0f, 1.0f);
        }

        public static Matrix4 Frustum(
         float left,
         float right,
         float bottom,
         float top,
         float zNear,
         float zFar)
        {

            float A = (right + left) / (right - left);
            float B = (top + bottom) / (top - bottom);
            float C = -(zFar + zNear) / (zFar - zNear);
            float D = -(2.0f * zFar * zNear) / (zFar - zNear);

            float U = (2.0f * zNear) / (right - left);
            float V = (2.0f * zNear) / (top - bottom);
            float o = 0.0f;
            float m1 = -1.0f;
            return new Matrix4(
            U, o, A, o,
            o, V, B, o,
            o, o, C, D,
            o, o, m1, o);
        }

        public static Matrix4 Perspective(
         float fovy,
         float aspect,
         float zNear,
         float zFar)
        {
            float ymin, ymax, xmin, xmax;
            ymax = zNear * (float)Math.Tan(fovy / 2.0f);
            ymin = -ymax;
            xmin = ymin * aspect;
            xmax = ymax * aspect;
            return Matrix4.Frustum(xmin, xmax, ymin, ymax, zNear, zFar);
        }

        public static Matrix4 ViewLookAt(
           Vector3 eye,
           Vector3 at,
           Vector3 up
        )
        {

            Vector3 f = Vector3.Normalized(at - eye);

            Vector3 s = Vector3.Normalized(Vector3.Cross(f, Vector3.Normalized(up)));
            Vector3 u = Vector3.Normalized(Vector3.Cross(s, f));

            Matrix4 Rot = new Matrix4(
             s.x, s.y, s.z, 0.0f,
             u.x, u.y, u.z, 0.0f,
            -f.x, -f.y, -f.z, 0.0f,
             0.0f, 0.0f, 0.0f, 1.0f);

            Matrix4 Trans = Matrix4.Translate(-eye);

            return Rot * Trans;
        }


        override public string ToString()
        {
            return string.Format(
 "{0}\t{1}\t{2}\t{3}\n{4}\t{5}\t{6}\t{7}\n{8}\t{9}\t{10}\t{11}\n{12}\t{13}\t{14}\t{15}\n",
 m11, m12, m13, m14,
 m21, m22, m23, m24,
 m31, m32, m33, m34,
 m41, m42, m43, m44
 );
        }

        public class Matrix4NotInvertibleException : Exception
        {
        }

    }

    public struct Quaternion
    {
        public float x, y, z, w;

        public Quaternion(float[] qua) : this(qua[0], qua[1], qua[2], qua[3]) { }


        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            /*
            from : http://www.cprogramming.com/tutorial/3d/quaternions.html
             (Q1 * Q2).x = (w1x2 + x1w2 + y1z2 - z1y2)
             (Q1 * Q2).y = (w1y2 - x1z2 + y1w2 + z1x2)
             (Q1 * Q2).z = (w1z2 + x1y2 - y1x2 + z1w2
             (Q1 * Q2).w = (w1w2 - x1x2 - y1y2 - z1z2)
            */
            return new Quaternion(
            left.w * right.x + left.x * right.w + left.y * right.z - left.z * right.y,
            left.w * right.y - left.x * right.z + left.y * right.w + left.z * right.x,
            left.w * right.z + left.x * right.y - left.y * right.x + left.z * right.w,
            left.w * right.w - left.x * right.x - left.y * right.y - left.z * right.z
            );
        }


        public static float Norm2(Quaternion quat)
        {
            return
                quat.x * quat.x +
                quat.y * quat.y +
                quat.z * quat.z +
                quat.w * quat.w;
        }

        public static float Norm(Quaternion quat)
        {
            return
                (float)Math.Sqrt(
                quat.x * quat.x +
                quat.y * quat.y +
                quat.z * quat.z +
                quat.w * quat.w);
        }

        public static Quaternion Normalized(Quaternion quat)
        {
            Quaternion res = new Quaternion();

            float l = Norm(quat);

            if (l > 0.0f)
            {
                res = new Quaternion(quat.x /= l, quat.y /= l, quat.z /= l, quat.w /= l);
            }
            return res;
        }

        public static Quaternion FromAngleAxis(float angle, Vector3 axis)
        {
            float a2 = angle / 2.0f;
            float sin_a2 = (float)Math.Sin(a2);

            return new Quaternion(
               axis.x * sin_a2,
               axis.y * sin_a2,
               axis.z * sin_a2,
               (float)Math.Cos(a2)
             );

        }

        public static Quaternion FromMatrix(Matrix4 r)
        {
            // ripped from blender 2.5
            float[] q = new float[4];
            float tr, s;
            float[][] mat = new float[][] { new float[4], new float[4], new float[4], new float[4] };

            mat[0][0] = r.m11; mat[1][0] = r.m12; mat[2][0] = r.m13;
            mat[0][1] = r.m21; mat[1][1] = r.m22; mat[2][1] = r.m23;
            mat[0][2] = r.m31; mat[1][2] = r.m32; mat[2][2] = r.m33;

            tr = 0.25f * (1.0f + mat[0][0] + mat[1][1] + mat[2][2]);

            if (tr > 0.0f)
            {
                s = (float)Math.Sqrt(tr);
                q[0] = s;
                s = 1.0f / (4.0f * s);
                q[1] = ((mat[1][2] - mat[2][1]) * s);
                q[2] = ((mat[2][0] - mat[0][2]) * s);
                q[3] = ((mat[0][1] - mat[1][0]) * s);
            }
            else
            {
                if (mat[0][0] > mat[1][1] && mat[0][0] > mat[2][2])
                {
                    s = 2.0f * (float)Math.Sqrt(1.0f + mat[0][0] - mat[1][1] - mat[2][2]);
                    q[1] = 0.25f * s;

                    s = 1.0f / s;
                    q[0] = ((mat[2][1] - mat[1][2]) * s);
                    q[2] = ((mat[1][0] + mat[0][1]) * s);
                    q[3] = ((mat[2][0] + mat[0][2]) * s);
                }
                else if (mat[1][1] > mat[2][2])
                {
                    s = 2.0f * (float)Math.Sqrt(1.0f + mat[1][1] - mat[0][0] - mat[2][2]);
                    q[2] = 0.25f * s;

                    s = 1.0f / s;
                    q[0] = (float)((mat[2][0] - mat[0][2]) * s);
                    q[1] = (float)((mat[1][0] + mat[0][1]) * s);
                    q[3] = (float)((mat[2][1] + mat[1][2]) * s);
                }
                else
                {
                    s = 2.0f * (float)Math.Sqrt(1.0f + mat[2][2] - mat[0][0] - mat[1][1]);
                    q[3] = 0.25f * s;

                    s = 1.0f / s;
                    q[0] = ((mat[1][0] - mat[0][1]) * s);
                    q[1] = ((mat[2][0] + mat[0][2]) * s);
                    q[2] = ((mat[2][1] + mat[1][2]) * s);
                }
            }

            Quaternion qua = new Quaternion(q[1], q[2], q[3], q[0]);
            qua = Quaternion.Normalized(qua);

            return qua;

        }

        static Quaternion Lerp(Quaternion start, Quaternion end, float t)
        {
            Quaternion r = new Quaternion();
            r.x = (1.0f - t) * start.x - t * end.x;
            r.y = (1.0f - t) * start.y - t * end.y;
            r.z = (1.0f - t) * start.z - t * end.z;
            r.w = (1.0f - t) * start.w - t * end.w;
            return r;
        }
    }


}
