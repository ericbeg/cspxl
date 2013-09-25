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

        private void Compile()
        {
            if ( m_isValidVertex )
                GL.DeleteShader( m_vertex );

            if ( m_isValidFragment )
                GL.DeleteShader( m_fragment );

            if ( m_isValidProgram )
                GL.DeleteProgram( m_program );

            m_isValidFragment = m_isValidVertex = m_isValidProgram = false;

            m_vertex   = GL.CreateShader( ShaderType.VertexShader   );
            m_fragment = GL.CreateShader( ShaderType.FragmentShader );

            GL.ShaderSource(m_vertex, source);
            GL.ShaderSource(m_fragment, source);

            GL.CompileShader(m_vertex);
            GL.CompileShader(m_fragment);

            int param;
         
            GL.GetShader(m_vertex, ShaderParameter.CompileStatus, out param);
            m_isValidVertex = (param == (int)All.True);
           
            GL.GetShader(m_fragment, ShaderParameter.CompileStatus, out param);
            m_isValidFragment = (param == (int)All.True);

            if ( m_isValidVertex && m_isValidFragment )
            {
                m_program = GL.CreateProgram();
                GL.AttachShader(m_program, m_vertex);
                GL.AttachShader(m_program, m_fragment);
                GL.LinkProgram(m_program);

                GL.GetProgram(m_program, ProgramParameter.LinkStatus, out param);
                m_isValidProgram = (param == (int)All.True);
            }

            if ( ! m_isValidVertex )
                GL.DeleteShader( m_vertex );

            if ( ! m_isValidFragment )
                GL.DeleteShader( m_fragment );

            if ( ! m_isValidProgram )
                GL.DeleteProgram( m_program );


        }

        public override void Apply()
        {
            Compile();
        }

        public override void Free()
        {

        }

        public override void ApplyUniforms()
        {

        }

        public override void Use()
        {
            if ( m_isValidProgram )
            {
                GL.UseProgram(m_program);
            }
        }
    }
}

