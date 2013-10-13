using System;
using System.Collections.Generic;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
    public class GLShader : Shader
    {
        private int m_program;

        private int m_vertex;
        private int m_fragment;

        private bool m_isValidFragment = false;
        private bool m_isValidVertex   = false;
        private bool m_isValidProgram  = false;

        public int glname
        {
            get
            {
               
                return m_program;
            }
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
                GL.DeleteProgram( m_program );
            GLHelper.CheckError();

            m_isValidFragment = m_isValidVertex = m_isValidProgram = false;

            m_vertex   = GL.CreateShader( ShaderType.VertexShader   );
            GLHelper.CheckError();
            m_fragment = GL.CreateShader( ShaderType.FragmentShader );
            GLHelper.CheckError();
            string glslVersion = "#version 110\n";
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
                m_program = GL.CreateProgram();
                GL.AttachShader(m_program, m_vertex);
                GL.AttachShader(m_program, m_fragment);
                
                GLHelper.CheckError();
                GL.ValidateProgram(m_program);
                GLHelper.CheckError();
                

                
                m_isCompiled = true;
                m_isValidProgram = m_isCompiled;
                //GL.GetProgram(m_program, ProgramParameter.ValidateStatus, out param);
                //m_isValidProgram &= (param == (int)All.True);
                
                string infoLog = GL.GetProgramInfoLog(m_program);
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
                GL.DeleteProgram( m_program );

            GLHelper.CheckError();

        }

       
        public override void Apply()
        {
            Compile();
            GLHelper.CheckError();
        }

        public override void Free()
        {

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
                
                GL.LinkProgram( m_program );
                GLHelper.CheckError();
                int param;
                GL.GetProgram(m_program, ProgramParameter.LinkStatus, out param);
                m_isValidProgram = (param == (int)All.True);
                //m_isValidProgram = true;
                GLHelper.CheckError();

            }
        }

        public override void Use()
        {
            if ( m_isValidProgram )
            {
                GL.UseProgram( m_program );
                GLHelper.CheckError();
            }
        }
    }
}

