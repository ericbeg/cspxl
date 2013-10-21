using System;
using System.Collections.Generic;

using System.Drawing;

using OpenTK.Graphics;

namespace pxl
{
	public abstract class Texture
	{
		public enum FilteringMode
		{
			Nearest,
			Bilinear,
			Trilinear
		}

        public enum Format
        {
            RGBA,
            RGB,
            Alpha,

            RGBAf,
            RGBf,
            Alphaf
        }


        public int width;
        public int height;
        public Format format;
        public FilteringMode filteringMode;

        public bool mipmap;
        abstract public Color4[] GetPixels();
        abstract public void SetPixels( Color4[] pixels );
        abstract public void Apply();
        abstract public void Copy( Bitmap bitmap );
    }
}