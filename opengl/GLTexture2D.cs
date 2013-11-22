#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
    public class GLTexture2D : Texture2D
    {
        public int glname;
        private bool m_isValid;

        public GLTexture2D()
            : base()
        {
            m_isValid = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (m_isValid)
            {
                GL.DeleteTexture(glname);
                glname = 0;
            }
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

            // Solve texture filtering
            TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear;
            TextureMagFilter magFilter = TextureMagFilter.Linear;
            HintMode hintmode = HintMode.DontCare;

            switch (filteringMode)
            {
                case FilteringMode.Nearest:
                    minFilter = TextureMinFilter.Nearest;
                    magFilter = TextureMagFilter.Nearest;
                    hintmode = HintMode.Fastest;
                    break;
                case FilteringMode.Bilinear:
                    minFilter = TextureMinFilter.Linear;
                    magFilter = TextureMagFilter.Linear;
                    hintmode = HintMode.Fastest;
                    break;
                case FilteringMode.Trilinear:
                    minFilter = TextureMinFilter.LinearMipmapLinear;
                    magFilter = TextureMagFilter.Linear;
                    hintmode = HintMode.Fastest;
                    break;
            }
            
            GL.Hint(HintTarget.PerspectiveCorrectionHint, hintmode);
            GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)minFilter );
            GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)magFilter);

            /*
            if( mipmap )
                GL.TexParameter(target, TextureParameterName.GenerateMipmap, 
             */
            

            int level = 0;
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba;
            int width = 256;
            int height = 256;
            int border = 0;
            OpenTK.Graphics.OpenGL.PixelFormat pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgba;
            PixelType pixelType = PixelType.Byte;

            byte[] pixels = null;
           
            GL.TexImage2D<byte>(target, level, internalFormat, width, height, border, pixelFormat, pixelType, pixels);
        }

        public override void Copy(Bitmap bitmap)
        {
            if (m_isValid)
            {
                GL.DeleteTexture(glname);
            }

            GL.GenTextures(1, out glname);
            GL.BindTexture(TextureTarget.Texture2D, glname);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // resolve texture internal format
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba32f;
            switch (format)
            {
                case Format.Alpha8: internalFormat = PixelInternalFormat.Alpha; break;
                case Format.RGB24:  internalFormat = PixelInternalFormat.Rgb; break;
                case Format.RGBA32: internalFormat = PixelInternalFormat.Rgba; break;
                /*
                case Format.Alphaf: internalFormat = PixelInternalFormat.Luminance16; break;
                case Format.RGBf: internalFormat = PixelInternalFormat.rgba; break;
                case Format.RGBAf: internalFormat = PixelInternalFormat.Rgba32f; break;
                 */

            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            
            width  = bitmap.Width;
            height = bitmap.Height;

            m_isValid = true;
        }

        override public void Bind(int textureUnit)
        {
            GL.Enable(EnableCap.Texture2D);
            GLHelper.CheckError();
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0) + textureUnit);
            GLHelper.CheckError();
            GL.BindTexture(TextureTarget.Texture2D, this.glname);
            GLHelper.CheckError();

        }

    }
}
