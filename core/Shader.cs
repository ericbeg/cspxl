using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace pxl
{
	public abstract class Shader
	{
        public string source;
        private Dictionary<string, object > m_uniforms = new Dictionary<string, object>();

        public static Shader active = null;

        public void SetUniform( string name, object value)
        {
            m_uniforms[name] = value;
        }

        public abstract void Apply();
        public abstract void Free();
        public abstract void ApplyUniforms();
        public abstract void Use();
	}
}
