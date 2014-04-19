#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public class SceneBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
        {
            GameObject scene = null;
            if (bvar != null && bvar.type == "Scene")
            {
                scene = new GameObject();
                scene.name = bvar["id"]["name"];
                //std::string idname = bvar["id"]["name[66]"].toString();
                //scene->setName( idname.substr(2, idname.size() - 2) );
                //scene->setURI( bvar.getURI() );

                BlendFile.BlendVar world = bvar["world"];

                if (world != null)
                {
                    BlendFile.worldAmbientColor.R = world["ambr"];
                    BlendFile.worldAmbientColor.G = world["ambg"];
                    BlendFile.worldAmbientColor.B = world["ambb"];
                }

                BlendFile.BlendVar[] objects = bvar["base"];
                foreach (BlendFile.BlendVar vbase in objects)
                {
                    BlendFile.BlendVar vob = vbase["object"];
                    BlendFile.BlendVar parent = vob["parent"];
                    bool isRoot = (parent == null);

                    if (isRoot)
                    {
                        GameObject child = bvar.blendFile.Load(vob["id"]["name"]) as GameObject;
                        child.transform.parent = scene.transform;
                    }
                }
            }


            return scene;
        }
    }
}
