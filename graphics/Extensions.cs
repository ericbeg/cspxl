#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using OpenTK;

namespace pxl
{
    public static class Color4Extensions
    {
        public static byte[] ToRGBA32( this Color4 col )
        {
            byte[] b = new byte[4];
            b[0] = Convert.ToByte(255.0 * col.R );
            b[1] = Convert.ToByte(255.0 * col.G);
            b[2] = Convert.ToByte(255.0 * col.B);
            b[3] = Convert.ToByte(255.0 * col.A);
            return b;
        }

        public static byte[] ToRGB24(this Color4 col)
        {
            byte[] b = new byte[3];
            b[0] = Convert.ToByte(255.0 * col.R);
            b[1] = Convert.ToByte(255.0 * col.G);
            b[2] = Convert.ToByte(255.0 * col.B);
            return b;
        }

        public static byte[] ToAlpha8(this Color4 col)
        {
            byte[] b = new byte[3];
            b[0] = Convert.ToByte(255.0 * col.R);
            b[1] = Convert.ToByte(255.0 * col.G);
            b[2] = Convert.ToByte(255.0 * col.B);
            return b;
        }

        public static void CopyRGBA32(this Color4 col, byte[] rgba)
        {
            col.R = Convert.ToSingle(rgba[0]) / 255.0f;
            col.G = Convert.ToSingle(rgba[1]) / 255.0f;
            col.B = Convert.ToSingle(rgba[2]) / 255.0f;
            col.A = Convert.ToSingle(rgba[3]) / 255.0f;
        }

        public static void CopyRGB24(this Color4 col, byte[] rgba)
        {
            col.R = Convert.ToSingle(rgba[0]) / 255.0f;
            col.G = Convert.ToSingle(rgba[1]) / 255.0f;
            col.B = Convert.ToSingle(rgba[2]) / 255.0f;
        }

        public static void CopyAlpha8(this Color4 col, byte[] rgba)
        {
            col.A = Convert.ToSingle(rgba[0]) / 255.0f;
        }

    }
}
