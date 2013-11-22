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


        public int width;
        public int height;
        public Format format;
        public FilteringMode filteringMode;

        public bool mipmap;

        virtual public void Dispose() { }
    }
}
