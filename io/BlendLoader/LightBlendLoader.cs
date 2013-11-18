#pragma  warning disable 1591 // doc
#pragma warning disable 219 // is assigned but never used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace pxl
{
    public class LightBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
        {
            Light light = null;
            // from blender/makesdna/DNA_lamp_types.h
            /* type */
            const int LA_LOCAL = 0;
            const int LA_SUN = 1;
            const int LA_SPOT = 2;
            const int LA_HEMI = 3;
            const int LA_AREA = 4;


            /* mode */
            const int LA_SHAD_BUF = 1;
            const int LA_HALO = 2;
            const int LA_LAYER = 4;
            const int LA_QUAD = 8; /* no longer used */
            const int LA_NEG = 16;
            const int LA_ONLYSHADOW = 32;
            const int LA_SPHERE = 64;
            const int LA_SQUARE = 128;
            const int LA_TEXTURE = 256;
            const int LA_OSATEX = 512;
            /* const int LA_DEEP_SHADOW 1024 */
            /* not used anywhere */
            const int LA_NO_DIFF = 2048;
            const int LA_NO_SPEC = 4096;
            const int LA_SHAD_RAY = 8192;
            /* yafray: lamp shadowbuffer flag, softlight */
            /* Since it is used with LOCAL lamp, can't use LA_SHAD */
            /* const int LA_YF_SOFT   16384 */
            /* no longer used */
            const int LA_LAYER_SHADOW = 32768;
            const int LA_SHAD_TEX = (1 << 16);
            const int LA_SHOW_CONE = (1 << 17);
            // //////////////////////////////////////

            if (bvar != null && bvar.type == "Lamp")
            {
                light = new Light();
                Color4 col;

                col.R = bvar["r"];
                col.G = bvar["g"];
                col.B = bvar["b"];
                col.A = 1.0f;

                light.color = col;


                float E = bvar["energy"];
                float L = bvar["att1"];
                float Q = bvar["att2"];
                float D = bvar["dist"];

                //printf("E=%f  L=%f Q=%f D=%f \n", E, L, Q, D );

                /*
                From Blender 2.4 documention http://wiki.blender.org/index.php/Doc:2.4/Manual/Lighting/Lights/Light_Attenuation
                 Mixing “Linear” and “Quad”

                If both the Linear and Quad slider fields have values greater than 0.0, then the formula used to calculate the light attenuation profile changes to this:

                I = E × (D / (D + L × r)) × (D2 / (D2 + Q × r2))

                Where

                    I is the calculated Intensity of light.
                    E is the current Energy slider setting.
                    D is the current setting of the Dist field.
                    L is the current setting of the Linear slider.
                    Q is the current setting of the Quad slider.
                    r is the distance from the lamp where the light intensity gets measured.
    
                Rewriting the equation yields:
                   I = (D^3*E)/(
                     D^3
                   + r*D^2*L
                   + r^2*D*Q
                   + r^3*L*Q 
                   )
 
                 */
                Vector4 attenuation;
                attenuation.x = D * D * D; // constant
                attenuation.y = D * D * L; // linear
                attenuation.z = D * Q;   // quadratic
                attenuation.w = L * Q;   // cubic


                // The shading results are different from the shading in Blender,
                // a correction is aplied. The correction coefs has been obtained with trial and error.
                // TODO: make those coefficients accessible at runtime. 
                attenuation.y *= 1.0f;
                attenuation.z *= 0.4f;
                attenuation.w *= 0.6f;

                // bring the numerator to 1.0
                float ec = E * attenuation.x;
                attenuation = attenuation/ec;

                light.attenuation = attenuation;
                light.intensity = E;
                short type = bvar["type"];
                int mode = bvar["mode"];

                //cam->setNear(bvar["clipsta"]);
                //cam->setFar(bvar["clipend"]);
                light.cutoff = (((float)bvar["spotsize"]) / 2.0f) * ((float)Math.PI / 180.0f);
                //light->setBlend( bvar["spotblend"] );
                //light->setShadowBufferBias( bvar["bias"] );
                //light->setShadowBufferSize( (short)bvar["bufsize"]);


                Color4 shadowColor;
                shadowColor.R = bvar["shdwr"];
                shadowColor.G = bvar["shdwg"];
                shadowColor.B = bvar["shdwb"];
                shadowColor.A = 1.0f;

                //light->setShadowColor( shadowColor );

                if (type == LA_LOCAL) light.type = Light.Type.Point;
                if (type == LA_SUN) light.type = Light.Type.Sun;
                if (type == LA_SPOT) light.type = Light.Type.Spot;
                if (type == LA_HEMI) light.type = Light.Type.Spot;
                if (type == LA_AREA) light.type = Light.Type.Spot;

                if (light.type == Light.Type.Sun)
                {
                    //light->setShadowBufferEnable( mode & LA_SHAD_RAY  );
                }
                else if (light.type == Light.Type.Spot)
                {
                    //light->setShadowBufferEnable( mode & LA_SHAD_BUF  );
                }
                else
                {
                    //light->setShadowBufferEnable( false  );
                }

            }

            return light;
        }
    }
}
