using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
    class GLTexture : Texture
    {
        public int glname;

        private Color4[] m_pixels;
        private bool m_isValid;

        GLTexture()
            : base()
        {
            m_isValid = false;
        }

        public override Color4[] GetPixels()
        {
            throw new NotImplementedException();
        }

        public override void SetPixels(Color4[] pixels)
        {
            throw new NotImplementedException();
        }

        public override void Apply()
        {
            if (m_isValid)
            {
                GL.DeleteTexture(glname);
            }

            TextureTarget target = TextureTarget.Texture2D;
            glname = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glname);
            //GL.TexParameter(target, TextureParameterName.TextureMagFilter, 
            //GL.TexParameter(target, TextureParameterName.TextureMinFilter, 

            //GL.TexParameter(target, TextureParameterName.GenerateMipmap, 
            

            int level = 0;
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba;
            int width = 256;
            int height = 256;
            int border = 0;
            PixelFormat pixelFormat = PixelFormat.Rgba;
            PixelType pixelType = PixelType.Byte;

            byte[] pixels = null;
           
            GL.TexImage2D<byte>(target, level, internalFormat, width, height, border, pixelFormat, pixelType, pixels);
        }


    }
}
