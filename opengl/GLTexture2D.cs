﻿#pragma  warning disable 1591

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
        internal bool m_isValid;

        public GLTexture2D()
            : base()
        {

        }

        public GLTexture2D(int width, int height)
            : base(width, height)
        {
            Apply();
        }

        public GLTexture2D(int width, int height, Texture.Format format)
            : base(width, height, format)
        {
            Apply();
        }

        public GLTexture2D(int width, int height, Texture.Format format, bool mipmap)
            : base(width, height, format, mipmap)
        {
            Apply();
        }

        ~GLTexture2D()
        {
            Free();
        }

        public void Free()
        {
            if (GLHelper.IsValidContext)
            {
                if (glname > 0)
                {
                    GL.DeleteTexture(glname);
                }
            }
            glname = 0; m_isValid = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            Free();
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
            Free();
            
            glname = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, glname);

            SetFilters();

            TextureTarget target = TextureTarget.Texture2D;
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

        private void SetFilters()
        {
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

            TextureTarget target = TextureTarget.Texture2D;
            GL.Hint(HintTarget.PerspectiveCorrectionHint, hintmode);
            GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)minFilter);
            GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)magFilter);

             /*
            if( mipmap )
                GL.TexParameter(target, TextureParameterName.GenerateMipmap, 
            */

        }

        override public void Bind(int textureUnit)
        {
            GL.Enable(EnableCap.Texture2D);
            GLHelper.CheckError();
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0) + textureUnit);
            GLHelper.CheckError();
            GL.BindTexture(TextureTarget.Texture2D, this.glname);
            GLHelper.CheckError();
            SetFilters();
        }

    }
}
