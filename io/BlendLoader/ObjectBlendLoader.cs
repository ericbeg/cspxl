#pragma  warning disable 1591

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

                LoadMaterialsOn(go, bvar);

            } // if bvar is object

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


        private void LoadMaterialsOn(GameObject go, BlendFile.BlendVar bvar)
        {
            BlendFile.BlendVar data = bvar["data"];
            BlendFile.BlendVar[] mats = null;
            mats = bvar["mat"];

            if ( (mats == null || mats.Length == 0 ) &&  data.type == "Mesh")
            {
                mats = data["mat"];
            }

            if (mats != null)
            {
                List<Material> materials = new List<Material>();
                foreach (var mat in mats)
                {
                    if (mat != null)
                    {
                        string matname = mat["id"]["name"];
                        Material ma = bvar.blendFile.Load(matname) as Material;
                        if (ma != null)
                        {
                            materials.Add(ma);
                        }
                    }
                }

                if (materials.Count > 0)
                {
                    Renderer rdr = go.GetComponent<Renderer>();
                    if (rdr == null)
                        rdr = go.AddComponent<Renderer>();
                    rdr.material = materials[0];
                }
            } // if mats not null
        }
    }
}
