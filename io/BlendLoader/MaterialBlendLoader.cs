#pragma  warning disable 1591 // doc
#pragma warning disable 219 // is assigned but never used

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

            // from blender 2.63: blender/makesdna/DNA_material_types.h

            /* texco */
            const uint TEXCO_ORCO = 1;
            const uint TEXCO_REFL = 2;
            const uint TEXCO_NORM = 4;
            const uint TEXCO_GLOB = 8;
            const uint TEXCO_UV = 16;
            const uint TEXCO_OBJECT = 32;
            const uint TEXCO_LAVECTOR = 64;
            const uint TEXCO_VIEW = 128;
            const uint TEXCO_STICKY = 256;
            const uint TEXCO_OSA = 512;
            const uint TEXCO_WINDOW = 1024;
            const uint NEED_UV = 2048;
            const uint TEXCO_TANGENT = 4096;
            /* still stored in vertex->accum, 1 D */
            const uint TEXCO_STRAND = 8192;
            const uint TEXCO_PARTICLE = 8192;/* strand is used for normal materials, particle for halo materials */
            const uint TEXCO_STRESS = 16384;
            const uint TEXCO_SPEED = 32768;

            /* mapto */
            const uint MAP_COL = 1;
            const uint MAP_NORM = 2;
            const uint MAP_COLSPEC = 4;
            const uint MAP_COLMIR = 8;
            const uint MAP_VARS = (0xFFF0);
            const uint MAP_REF = 16;
            const uint MAP_SPEC = 32;
            const uint MAP_EMIT = 64;
            const uint MAP_ALPHA = 128;
            const uint MAP_HAR = 256;
            const uint MAP_RAYMIRR = 512;
            const uint MAP_TRANSLU = 1024;
            const uint MAP_AMB = 2048;
            const uint MAP_DISPLACE = 4096;
            const uint MAP_WARP = 8192;
            const uint MAP_LAYER = 16384;    /* unused */
            // //////////////////////////////////////




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
                
                mat.SetColor("color", col);
                mat.SetColor("specular", spec);

                BlendFile.BlendVar[] mtextures = bvar["mtex"];
                foreach (var mtex in mtextures)
                {
                    Texture2D tex = LoadTexture( mtex );
                    if (tex != null)
                    {
                        string samplerName = "undefined";
                        
                        short mapto = mtex["mapto"];

                        // TODO: ??? The same texture can be mapped to several channels.
                        if ( (mapto & MAP_COL)  > 0 ) samplerName = "mainTex";
                        if ( (mapto & MAP_NORM) > 0 ) samplerName = "normalTex";

                        mat.SetTexture(samplerName, tex); 
                    }
                }

            }

            return mat;
        }

        Texture2D LoadTexture(BlendFile.BlendVar bvar)
        {
            Texture2D texture = null;
            if ( bvar!= null && bvar.type == "MTex" )
            {
                BlendFile.BlendVar tex = bvar["tex"];
                if (tex != null)
                {
                    BlendFile.BlendVar ima = tex["ima"];
                    if (ima != null)
                    {
                        string imname = ima["id"]["name"];

                        Bitmap bitmap = bvar.blendFile.Load(imname) as Bitmap;
                        if (bitmap != null)
                        {
                            GLTexture2D gltexture = new GLTexture2D();
                            gltexture.Copy(bitmap);
                            texture = gltexture;
                        }
                    }
                }
            } // if mtex
            return texture;
        }

    }
}
