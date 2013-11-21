#pragma  warning disable 1591

using System;
using System.Collections.Generic;

using System.Drawing;

using OpenTK.Graphics;

namespace pxl
{
	public abstract class Texture : IDisposable
	{
		public enum FilteringMode
		{
			Nearest,
			Bilinear,
			Trilinear
		}

        public enum Format
        {
            RGBA32,
            RGB24,
            Alpha8,

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
        abstract public void Copy(Bitmap bitmap);
        abstract public void Bind(int textureUnitIndex);
        virtual public void Dispose() { }
    }
}
