#pragma  warning disable 1591

using System;
using System.Collections.Generic;

using System.Drawing;

using OpenTK.Graphics;

namespace pxl
{
	public abstract class Texture2D : Texture
	{

        public Texture2D()
            : base()
        {
        }

        public Texture2D(int width, int height)
            : base(width, height)
        {
        }

        public Texture2D(int width, int height, Texture.Format format)
            : base(width, height, format)
        {
        }

        public Texture2D(int width, int height, Texture.Format format, bool mipmap)
            : base(width, height, format, mipmap)
        {
        }

        abstract public Color4[] GetPixels();
        abstract public void SetPixels( Color4[] pixels );
        abstract public void Apply();
        abstract public void Copy(Bitmap bitmap);
        abstract public void Bind(int textureUnitIndex);
    }
}
