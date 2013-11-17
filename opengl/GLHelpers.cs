#pragma  warning disable 1591

using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
    public static class GLHelper
    {
        public static void CheckError()
        {
           ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                OpenGLErrorException exception = new OpenGLErrorException();
                exception.errorCode = err;
                throw exception ;
            }
        }

        public static string infoString
        {
            get
            {
                string info = "";
                info += "Vendor: " + GL.GetString(StringName.Vendor) + "\n";
                info += "Version: " + GL.GetString(StringName.Version) + "\n";
                //info += "GLSL version: " + GL.GetString(StringName.ShadingLanguageVersion) + "\n";
                info += "Extensions: " + GL.GetString(StringName.Extensions) + "\n";
                
                int i = 0;
                GL.GetInteger(GetPName.MaxVertexAttribs, out i); info += "MaxVertexAttribs=" + i + "\n";
                GL.GetInteger(GetPName.MaxVertexUniformComponents, out i); info += "MaxVertexUniformComponents=" + i + "\n";
                GL.GetInteger(GetPName.MaxFragmentUniformComponents, out i); info += "MaxFragmentUniformComponents=" + i + "\n";
                GL.GetInteger(GetPName.MaxVaryingComponents, out i); info += "MaxVaryingComponents=" + i + "\n";
                GL.GetInteger(GetPName.MaxTextureUnits, out i); info += "MaxTextureUnits=" + i + "\n";
                
                return info;
            }
        }

        public class OpenGLErrorException : Exception
        {
            public ErrorCode errorCode;

            public override string ToString()
            {
                return string.Format("[OpenGLErrorException {0}]", errorCode.ToString() );
            }
        }

    }
}

