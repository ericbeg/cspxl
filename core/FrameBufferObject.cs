#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public abstract class FrameBufferObject
    {
        public FrameBufferObject(int width, int height, int depth) { }
        public abstract void AttachColorTexture(Texture tex, int attachment);

    }
}
