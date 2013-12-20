#pragma  warning disable 1591

using System;
using OpenTK.Graphics;

namespace pxl
{
	public abstract class Shader
	{
        protected bool m_isCompiled = false;
        protected bool m_hasAttemptedCompilation = false;
        private string m_source;
        private string[] m_includes;
        
        public string source
        {
            get
            {
                return m_source;
            }

            set
            {
                m_source = value;
                m_isCompiled = false;
                m_hasAttemptedCompilation = false;
            }
        }

        public string[] includes
        {
            get
            {
                if (m_includes != null)
                {
                    return m_includes;
                }
                else
                {
                    return new string[0];
                }
            }

            set
            {
                m_includes = value;
            }
        }



        public static Shader active = null;
        public static Shader fallback = null;

        public Texture2D texture;


        public abstract void Apply();
        public abstract void Free();
        public abstract void ApplyUniforms();
        public abstract void Link();
        public abstract void Use();

        public abstract void SetUniform(string name, int uniform);
        public abstract void SetUniform(string name, float uniform);
        public abstract void SetUniform(string name, Vector2 uniform);
        public abstract void SetUniform(string name, Vector3 uniform);
        public abstract void SetUniform(string name, Vector4 uniform);
        public abstract void SetUniform(string name, Color4 uniform);
        public abstract void SetUniform(string name, Matrix3 uniform);
        public abstract void SetUniform(string name, Matrix4 uniform);
    }
}
