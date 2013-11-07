﻿#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public class ObjectBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
        {
            Object obj = null;
            if (bvar.type == "Object")
            {
                GameObject go = new GameObject();
                obj = go;

                go.name = bvar["id"]["name"];
                LoadTransformOn( go, bvar );
                BlendFile.BlendVar data = bvar["data"];
                if (data != null)
                {
                    switch( data.type )
                    {
                        case "Mesh":
                            LoadMeshOn(go, bvar);
                            break;
                    }
                }

            }

            return obj;
        }

        private void LoadTransformOn(GameObject go, BlendFile.BlendVar bvar)
        {
            // Load Transform
            float[] loc = bvar["loc"];
            float[] size = bvar["size"];
            float[] quat = bvar["quat"];

            go.transform.localPosition = new Vector3(loc);
            go.transform.localScale = new Vector3(size);
            go.transform.localRotation = new Quaternion(quat[1], quat[2], quat[3], quat[0]);

        }
             
        private void LoadMeshOn( GameObject go, BlendFile.BlendVar bvar )
        {
            BlendFile.BlendVar data = bvar["data"];
            string meshName = data["id"]["name"];
            Mesh me = bvar.blendFile.Load(meshName) as Mesh;
            if (me != null)
            {
                Renderer rdr = go.GetComponent<Renderer>();
                if (rdr == null)
                {
                    rdr = go.AddComponent<Renderer>();
                }
                rdr.mesh = me;
            }
        }
    }
}