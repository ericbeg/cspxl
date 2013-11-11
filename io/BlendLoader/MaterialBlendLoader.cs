using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Drawing.Imaging;
using System.Drawing;

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

                BlendFile.BlendVar[] mtextures = bvar["mtex"];
                foreach (var mtex in mtextures)
                {
                    Texture tex = LoadTexture( mtex );
                    if (tex != null)
                    {
                        mat.SetTexture("mainTexture", tex); // TODO: define texture name
                    }
                }

            }

            return mat;
        }

        Texture LoadTexture(BlendFile.BlendVar bvar)
        {
            Texture texture = null;
            if ( bvar!= null && bvar.type == "MTex" )
            {
                BlendFile.BlendVar tex = bvar["tex"];
                if (tex != null)
                {
                    BlendFile.BlendVar ima = tex["ima"];
                    if (ima != null)
                    {
                        string imname = ima["id"]["name"];
                        BlendFile.BlendVar packedFile = ima["packedfile"];
                        if (packedFile != null)
                        {
                            int size = packedFile["size"];
                            int seek = packedFile["seek"];
                            BlendFile.BlendVar data = packedFile["data"];
                            data.Seek();
                            byte[] bytes = data.blendFile.binaryReader.ReadBytes(size);
                            MemoryStream ms = new MemoryStream(bytes);
                            Bitmap bitmap = new Bitmap(ms);
                            GLTexture gltexture = new GLTexture();
                            gltexture.Copy(bitmap);
                            texture = gltexture;
                        }
                    }

                }
            }
            return texture;
        }

    }
}
