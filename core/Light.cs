﻿#pragma  warning disable 1591 // doc

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
namespace pxl
{
    public class Light : Component
    {
        public Type type;
        public Color4 color;
        public float intensity;
        public float cutoff;
        public float exponent;
        public Vector4 attenuation;

        public enum Type
        {
            Point,
            Spot,
            Sun
        };
    }
}