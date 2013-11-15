#pragma  warning disable 1591

using System;
using System.Collections.Generic;

using OpenTK.Graphics;

namespace pxl
{
	public class Material
	{

        public float[] floats { get { return m_floats.ToArray(); } }
        public Vector2[] vectors2 { get { return m_vectors2.ToArray(); } }
        public Vector3[] vectors3 { get { return m_vectors3.ToArray(); } }
        public Vector4[] vectors4 { get { return m_vectors4.ToArray(); } }
        public Color4[] colors { get { return m_colors.ToArray(); } }
        public Matrix4[] matrices { get { return m_matrices.ToArray(); } }
        public Texture[] textures { get { return m_textures.ToArray(); } }

        public string[] floatNames { get { return m_floatNames.ToArray(); } }
        public string[] vector2Names { get { return m_vector2Names.ToArray(); } }
        public string[] vectorNames3 { get { return m_vector3Names.ToArray(); } }
        public string[] vectorNames4 { get { return m_vector4Names.ToArray(); } }
        public string[] colorNames { get { return m_colorNames.ToArray(); } }
        public string[] matrixNames { get { return m_matrixNames.ToArray(); } }
        public string[] textureNames { get { return m_textureNames.ToArray(); } }



        public Shader shader;

        private List<Texture> m_textures = new List<Texture>();

        private List<float> m_floats = new List<float>();
        
        private List<Vector2> m_vectors2 = new List<Vector2>();
        private List<Vector3> m_vectors3 = new List<Vector3>();
        private List<Vector4> m_vectors4 = new List<Vector4>();

        private List<Color4> m_colors = new List<Color4>();

        private List<Matrix4> m_matrices = new List<Matrix4>();

       // names
        private List<string> m_textureNames = new List<string>();

        private List<string> m_floatNames = new List<string>();

        private List<string> m_vector2Names = new List<string>();
        private List<string> m_vector3Names = new List<string>();
        private List<string> m_vector4Names = new List<string>();

        private List<string> m_colorNames = new List<string>();


        private List<string> m_matrixNames = new List<string>();



        public void SetTexture(string name, Texture value)
        {
            int i = m_textureNames.IndexOf(name);
            if (i < 0)
            {
                m_textureNames.Add(name);
                m_textures.Add(value);
            }
            else
            {
                m_textures[i] = value;
            }
        }

        public void SetFloat(string name, float value)
        {
            int i = m_floatNames.IndexOf(name);
            if (i < 0)
            {
                m_floatNames.Add(name);
                m_floats.Add(value);
            }
            else
            {
                m_floats[i] = value;
            }
        }

        public void SetVector2(string name, Vector2 value)
        {
            int i = m_vector2Names.IndexOf(name);
            if (i < 0)
            {
                m_vector2Names.Add(name);
                m_vectors2.Add(value);
            }
            else
            {
                m_vectors2[i] = value;
            }
        }

        public void SetVector3(string name, Vector3 value)
        {
            int i = m_vector3Names.IndexOf(name);
            if (i < 0)
            {
                m_vector3Names.Add(name);
                m_vectors3.Add(value);
            }
            else
            {
                m_vectors3[i] = value;
            }
        }

        public void SetVector4(string name, Vector4 value)
        {
            int i = m_vector4Names.IndexOf(name);
            if (i < 0)
            {
                m_vector4Names.Add(name);
                m_vectors4.Add(value);
            }
            else
            {
                m_vectors4[i] = value;
            }
        }

        public void SetColor(string name, Color4 value)
        {
            int i = m_colorNames.IndexOf(name);
            if (i < 0)
            {
                m_colorNames.Add(name);
                m_colors.Add(value);
            }
            else
            {
                m_colors[i] = value;
            }
        }


        public void SetMatrix4(string name, Matrix4 value)
        {
            int i = m_matrixNames.IndexOf(name);
            if (i < 0)
            {
                m_matrixNames.Add(name);
                m_matrices.Add(value);
            }
            else
            {
                m_matrices[i] = value;
            }
        }

        public float GetFloat(string name)
        {
            int i = m_floatNames.IndexOf( name );
            if( i < 0 ) throw new MaterialHasNoPropertyException(name);
            return m_floats[i];
        }

        public Vector2 GetVector2(string name)
        {
            int i = m_vector2Names.IndexOf(name);
            if (i < 0) throw new MaterialHasNoPropertyException(name);
            return m_vectors2[i];
        }

        public Vector3 GetVector3(string name)
        {
            int i = m_vector3Names.IndexOf(name);
            if (i < 0) throw new MaterialHasNoPropertyException(name);
            return m_vectors3[i];
        }

        public Vector4 GetVector4(string name)
        {
            int i = m_vector4Names.IndexOf(name);
            if (i < 0) throw new MaterialHasNoPropertyException(name);
            return m_vectors4[i];
        }

        public Color4 GetColor4(string name)
        {
            int i = m_colorNames.IndexOf(name);
            if (i < 0) throw new MaterialHasNoPropertyException(name);
            return m_colors[i];
        }

        public Matrix4 GetMatrix4(string name)
        {
            int i = m_matrixNames.IndexOf(name);
            if (i < 0) throw new MaterialHasNoPropertyException(name);
            return m_matrices[i];
        }

        public class MaterialHasNoPropertyException : Exception
        {
            string m_propertyName = "Unknown";
            public MaterialHasNoPropertyException(string propertyName){this.m_propertyName = propertyName;}
            public override string ToString()
            {
                return string.Format("Material has no property named `{0}'.", m_propertyName);
            }
        }

        internal void ApplyShaderUniforms()
        {
            // Bind textures
            for (int i = 0; i < textures.Length; ++i)
            {
                GLTexture texture = textures[i] as GLTexture; // TODO: Abstraction leak here. 
                string name = textureNames[i];
                if (texture != null)
                {
                    texture.Bind(i);
                    Shader.active.SetUniform(name, i);
                }
            }

        }

	}
}
