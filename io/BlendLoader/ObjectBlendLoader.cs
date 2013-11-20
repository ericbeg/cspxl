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
                LoadTransformOn(go, bvar);
                BlendFile.BlendVar data = bvar["data"];
                if (data != null)
                {
                    switch (data.type)
                    {
                        case "Camera":
                            LoadCameraOn(go, bvar);
                            break;
                        case "Lamp":
                            LoadLightOn(go, bvar);
                            break;
                        case "Mesh":
                            LoadMeshOn(go, bvar);
                            break;
                    }
                }

                LoadMaterialsOn(go, bvar);


                // load the children
                if (go != null)
                {
                    //string obname = "OB" + ob->getName();
                    string obname = go.name;
                    string[] childrennames = bvar.blendFile.GetChildrenNames( obname );
                    if (childrennames != null)
                    {
                        for (uint i = 0; i < childrennames.Length; ++i)
                        {
                            BlendFile.BlendVar childvar = bvar.blendFile[ childrennames[i] ];
                            GameObject child = bvar.blendFile.Load( childvar["id"]["name"] ) as GameObject;
                            
                            // Attach child to parent and keep world matrix
                            Matrix4 m = child.transform.matrix;
                            child.transform.parent = go.transform;
                            child.transform.matrix = m;
                        }
                    }
                }



            } // if bvar is object

            return obj;
        }

        private void LoadCameraOn(GameObject go, BlendFile.BlendVar bvar)
        {
            BlendFile.BlendVar data = bvar["data"];
            string camname = data["id"]["name"];
            Camera cam = bvar.blendFile.Load(camname) as Camera;
            if (cam != null)
            {
                GameObject.AddComponent(go, cam);
            }

        }

        private void LoadLightOn(GameObject go, BlendFile.BlendVar bvar)
        {
            BlendFile.BlendVar data = bvar["data"];
            string camname = data["id"]["name"];
            Light light = bvar.blendFile.Load(camname) as Light;
            if (light != null)
            {
                GameObject.AddComponent(go, light);
            }

        }


        private void LoadTransformOn(GameObject go, BlendFile.BlendVar bvar)
        {
            // Load Transform
            //float[] loc = bvar["loc"];
            float[] size = bvar["size"];
            //float[] quat  = bvar["quat"];
            float[] obmat = bvar["obmat"];
            Matrix4 mat = new Matrix4(obmat);

            go.transform.matrix = mat;
            //go.transform.localPosition = new Vector3(loc);
            go.transform.localScale = new Vector3(size);
            //go.transform.localRotation = new Quaternion(quat[1], quat[2], quat[3], quat[0]); // Apparently, this is not updated when the blend file is saved.
            //go.transform.localRotation = Quaternion.FromMatrix(mat);

            //Quaternion q = go.transform.localRotation;
            //go.transform.localRotation = new Quaternion(q.x, q.y, q.z, -q.w); // ???: Convention?
            //Matrix4 m = Matrix4.Rotate(go.transform.localRotation);

            //Matrix4 d = m - mat;

        }

        private void LoadMeshOn(GameObject go, BlendFile.BlendVar bvar)
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

            if ((mats == null || mats.Length == 0) && data.type == "Mesh")
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
