#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    class GLFrameBufferObject : FrameBufferObject
    {
        public GLFrameBufferObject(int width, int height, int depth)
            : base( width, height, depth)
        {
            throw new NotImplementedException();
        }

        public override void AttachColorTexture(Texture tex, int attachment)
        {
            throw new NotImplementedException();
        }
    }
}
