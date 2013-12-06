#pragma  warning disable 1591

using System;
using System.Collections.Generic;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
    public class GLShader : Shader
    {
        private int m_glname;

        private int m_vertex;
        private int m_fragment;

        private bool m_isValidFragment = false;
        private bool m_isValidVertex   = false;
        private bool m_isValidProgram  = false;

        public int glname
        {
            get
            {
               
                return m_glname;
            }
        }

        ~GLShader()
        {
            Free();
        }

        public override void Free()
        {
            if (GLHelper.IsValidContext)
            {

                if (m_vertex > 0)
                    GL.DeleteShader(m_vertex);

                if (m_fragment > 0)
                    GL.DeleteShader(m_fragment);
                GLHelper.CheckError();

                if (m_glname > 0)
                    GL.DeleteProgram(m_glname);
            }
            m_vertex = m_fragment = m_glname = 0;

        }


        private void Compile()
        {
            GLHelper.CheckError();

            if (m_hasAttemptedCompilation)
                return;

            m_hasAttemptedCompilation = true;
            if ( m_isValidVertex )
                GL.DeleteShader( m_vertex );
            GLHelper.CheckError();

            if ( m_isValidFragment )
                GL.DeleteShader( m_fragment );
            GLHelper.CheckError();

            if ( m_isValidProgram )
                GL.DeleteProgram( m_glname );
            GLHelper.CheckError();

            m_isValidFragment = m_isValidVertex = m_isValidProgram = false;

            m_vertex   = GL.CreateShader( ShaderType.VertexShader   );
            GLHelper.CheckError();
            m_fragment = GL.CreateShader( ShaderType.FragmentShader );
            GLHelper.CheckError();
            string glslVersion = "#version 120\n";
            string[] vertexSources = new string[3] { glslVersion, "#define VERTEX_SHADER\n", source };
            string[] fragmentSources = new string[3] { glslVersion, "#define FRAGMENT_SHADER\n", source }; 

            int[] vertexSourceLengths   = new int[vertexSources.Length];
            int[] fragmentSourceLengths = new int[fragmentSources.Length];

            for (int i=0; i < vertexSources.Length; ++i)
                vertexSourceLengths[i] = vertexSources[i].Length;

            for (int i=0; i < fragmentSources.Length; ++i)
                fragmentSourceLengths[i] = fragmentSources[i].Length;

            unsafe
            {
                fixed( int* pLengths = vertexSourceLengths )
                {
                    GL.ShaderSource(m_vertex, vertexSources.Length, vertexSources, pLengths);
                }
                GLHelper.CheckError();

                fixed( int* pLengths = fragmentSourceLengths )
                {
                    GL.ShaderSource(m_fragment, fragmentSources.Length, fragmentSources, pLengths);
                }
                GLHelper.CheckError();

            }

            GL.CompileShader(m_vertex);
            GLHelper.CheckError();

            GL.CompileShader(m_fragment);
            GLHelper.CheckError();

            int param;
         
            GL.GetShader(m_vertex, ShaderParameter.CompileStatus, out param);
            GLHelper.CheckError();
            m_isValidVertex = (param == (int)All.True);
           
            GL.GetShader(m_fragment, ShaderParameter.CompileStatus, out param);
            GLHelper.CheckError();
            m_isValidFragment = (param == (int)All.True);

            if (m_isValidVertex && m_isValidFragment)
            {
                m_glname = GL.CreateProgram();
                GL.AttachShader(m_glname, m_vertex);
                GL.AttachShader(m_glname, m_fragment);
                
                GLHelper.CheckError();
                GL.ValidateProgram(m_glname);
                GLHelper.CheckError();
                
                m_isCompiled = true;
                m_isValidProgram = m_isCompiled;
                //GL.GetProgram(m_program, ProgramParameter.ValidateStatus, out param);
                //m_isValidProgram &= (param == (int)All.True);
                
                string infoLog = GL.GetProgramInfoLog(m_glname);
                GLHelper.CheckError();
                if (infoLog != null && infoLog != "")
                {
                    Console.WriteLine(infoLog);
                }
                
                
            }
            else
            {
                if ( ! m_isValidVertex)
                {
                   string log = GL.GetShaderInfoLog( m_vertex );
                    Console.WriteLine("Vertex Shader log:\n" + log);
                }
            
                if ( ! m_isValidFragment)
                {
                    string log = GL.GetShaderInfoLog( m_fragment );
                    Console.WriteLine("Fragment Shader log:\n" + log);
                }
            }


            m_isValidVertex   &= m_isValidProgram;
            m_isValidFragment &= m_isValidProgram;

            if ( ! m_isValidVertex )
                GL.DeleteShader( m_vertex );

            if ( ! m_isValidFragment )
                GL.DeleteShader( m_fragment );
            GLHelper.CheckError();

            if ( ! m_isValidProgram )
                GL.DeleteProgram( m_glname );

            GLHelper.CheckError();

        }

       
        public override void Apply()
        {
            Compile();
            GLHelper.CheckError();
        }


        public override void ApplyUniforms()
        {

        }

        public override void Link()
        {
            Shader.active = this;

            if ( ! m_hasAttemptedCompilation )
                Compile();

            GLHelper.CheckError();

            if ( m_isCompiled )
            {
                
                GL.LinkProgram( m_glname );
                GLHelper.CheckError();
                int param;
                GL.GetProgram(m_glname, ProgramParameter.LinkStatus, out param);
                m_isValidProgram = (param == (int)All.True);
                //m_isValidProgram = true;
                GLHelper.CheckError();

            }
        }

        public override void Use()
        {
            if ( !m_hasAttemptedCompilation )
                Compile();

            if ( m_isValidProgram )
            {
                GL.UseProgram( m_glname );
                GLHelper.CheckError();
            }
        }

        override public void SetUniform(string name, int uniform)
        {
            int location = GL.GetUniformLocation(glname, name);
            GLHelper.CheckError();

            if (location >= 0)
            {
                GL.Uniform1(location, uniform);
                GLHelper.CheckError();

            }
        }

        public override void SetUniform(string name, float uniform)
        {
            int location = GL.GetUniformLocation(glname, name);
            GLHelper.CheckError();

            if (location >= 0)
            {
                GL.Uniform1( location, uniform);
                GLHelper.CheckError();

            }
        }

        public override void SetUniform(string name, Matrix3 m)
        {
            int location = GL.GetUniformLocation(glname, name);
            GLHelper.CheckError();

            if (location >= 0)
            {
                float[] mf = new float[]{
                    m.m11, m.m21, m.m31,
                    m.m12, m.m22, m.m32,
                    m.m13, m.m23, m.m33
                };

                unsafe
                {
                    fixed (float* ptr = mf)
                    {
                        GL.UniformMatrix3(location, 1, false, ptr);
                    }
                }
                GLHelper.CheckError();
            }
        }



        public override void SetUniform(string name, Matrix4 m)
        {
            int location = GL.GetUniformLocation(glname, name);
            GLHelper.CheckError();

            if (location >= 0)
            {
                OpenTK.Matrix4 om = new OpenTK.Matrix4(
                    m.m11, m.m21, m.m31, m.m41,
                    m.m12, m.m22, m.m32, m.m42,
                    m.m13, m.m23, m.m33, m.m43,
                    m.m14, m.m24, m.m34, m.m44
                    );
                 
                GL.UniformMatrix4(location, false, ref om);
                GLHelper.CheckError();

            }
        }

        public override void SetUniform(string name, Vector2 v)
        {
            int location = GL.GetUniformLocation(glname, name);
            if (location >= 0)
            {
                OpenTK.Vector2 ov = new OpenTK.Vector2(v.x, v.y);
                GL.Uniform2(location, ref ov);
            }
        }

        public override void SetUniform(string name, Vector3 v)
        {
            int location = GL.GetUniformLocation(glname, name);
            if (location >= 0)
            {
                OpenTK.Vector3 ov = new OpenTK.Vector3(v.x, v.y, v.z);
                GL.Uniform3(location, ref ov);
            }
        }

        public override void SetUniform(string name, Vector4 v)
        {
            int location = GL.GetUniformLocation(glname, name);
            if (location >= 0)
            {
                OpenTK.Vector4 ov = new OpenTK.Vector4(v.x, v.y, v.z, v.w);
                GL.Uniform4(location, ref ov);
            }
        }

        public override void SetUniform(string name, Color4 v)
        {
            SetUniform(name, new Vector4(v.R, v.G, v.B, v.A));
        }



    }
}

