#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public class CameraBlendLoader : BlendFile.IBlendLoader
    {

        public Object Load(BlendFile.BlendVar bvar)
        {
            // From blender/makesdna/DNA_camera_types.h
            /* type */

            const byte CAM_PERSP = 0;
            const byte CAM_ORTHO = 1;

            // from /gameengine/Rasterizer/RAS_FramingManager.h +190
            /* Should match CAMERA_SENSOR_FIT... from DNA_camera_types.h */
            const byte RAS_SENSORFIT_AUTO = 0;
            const byte RAS_SENSORFIT_HOR  = 1;
            const byte RAS_SENSORFIT_VERT = 2;


            Camera cam = null;
            if (bvar != null && bvar.type == "Camera")
            {
                cam = new Camera();
                cam.near  = bvar["clipsta"];
                cam.far   = bvar["clipend"];
                cam.scale = bvar["ortho_scale"];
                cam.perspective  = ((byte)bvar["type"]) == CAM_PERSP;

                // Compute fovy
                float sensor_x = bvar["sensor_x"];
                float sensor_y = bvar["sensor_y"];
                float lens     = bvar["lens"];
                
                float fovx  = (float)Math.Atan( sensor_x / (lens*1e-3f) );
                
                // Compute vertical field of view
                float near   = cam.near;
                float aspect = sensor_x/sensor_y;

                float xmax = near*(float)Math.Atan(fovx/2.0f);
                float ymax = xmax/aspect;
                float fovy = 2.0f*(float)Math.Atan(  ymax/near );

                cam.fovy = fovy;
            }

            return cam;
        }
    }
}
