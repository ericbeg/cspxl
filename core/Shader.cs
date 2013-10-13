using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace pxl
{
	public abstract class Shader
	{
        protected bool m_isCompiled = false;
        protected bool m_hasAttemptedCompilation = false;
        private string m_source;
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

        private Dictionary<string, object > m_uniforms = new Dictionary<string, object>();

        public static Shader active = null;

        public void SetUniform( string name, object value)
        {
            m_uniforms[name] = value;
        }

        public abstract void Apply();
        public abstract void Free();
        public abstract void ApplyUniforms();
        public abstract void Link();
        public abstract void Use();
	}
}
