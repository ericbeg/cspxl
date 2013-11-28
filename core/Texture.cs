#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public abstract class Texture: IDisposable
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


        public int width = 256;
        public int height = 256;
        public Format format = Format.RGBA32;
        public FilteringMode filteringMode = FilteringMode.Bilinear;

        public bool mipmap = true;

        public Texture()
            : this(512, 512)
        {
        }

        public Texture(int width, int height)
            : this(width, height, Format.RGBA32)
        {
        }

        public Texture(int width, int height, Format format)
            : this(width, height, format, true)
        {
        }

        public Texture(int width, int height, Format format, bool mipmap)
        {
            this.width = width;
            this.height = height;
            this.format = format;
            this.mipmap = mipmap;
            this.filteringMode = FilteringMode.Bilinear;
            
        }

        virtual public void Dispose() { }
    }
}
