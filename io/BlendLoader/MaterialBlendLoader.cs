using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace pxl
{
    public class MaterialBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
        {
            Material mat = null;
            if (bvar != null && bvar.type == "Material")
            {
                mat = new Material();
                Color4 col   = new Color4();
                Color4 spec = new Color4();
                short hardness = 0;
 
                col.R = bvar["r"];
                col.G = bvar["g"];
                col.B = bvar["b"];
                col.A = bvar["alpha"];

                spec.R = bvar["r"];
                spec.G = bvar["g"];
                spec.B = bvar["b"];
                hardness = bvar["har"];

                spec.A = (float)hardness;
                
                mat.SetColor("Color", col);
                mat.SetColor("Specular", spec);
            }

            return mat;

        }

    }
}
