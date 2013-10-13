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

