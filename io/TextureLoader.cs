using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System.IO;
namespace pxl
{
    class TextureLoader
    {
        public static Texture2D Load(string filepath)
        {
            var stream = new FileStream(filepath, System.IO.FileMode.Open);
            Texture2D tex = Load(stream);
            stream.Close();
            return tex;
        }

        public static  Texture2D Load(Stream stream)
        {
            Bitmap img = new Bitmap(stream);
            GLTexture2D gltexture = new GLTexture2D();
            Copy(gltexture, img);
            return gltexture;
        }

        internal static void Copy(GLTexture2D tex, Bitmap bitmap)
        {
            tex.Free();

            GL.GenTextures(1, out tex.glname);
            GL.BindTexture(TextureTarget.Texture2D, tex.glname);

            // resolve texture internal format
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba32f;
            switch (tex.format)
            {
                case Texture.Format.Alpha8: internalFormat = PixelInternalFormat.Alpha; break;
                case Texture.Format.RGB24: internalFormat = PixelInternalFormat.Rgb; break;
                case Texture.Format.RGBA32: internalFormat = PixelInternalFormat.Rgba; break;
                /*
                case Format.Alphaf: internalFormat = PixelInternalFormat.Luminance16; break;
                case Format.RGBf: internalFormat = PixelInternalFormat.rgba; break;
                case Format.RGBAf: internalFormat = PixelInternalFormat.Rgba32f; break;
                 */

            }


            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            tex.width = bitmap.Width;
            tex.height = bitmap.Height;

            tex.m_isValid = true;
        }



    }
}
