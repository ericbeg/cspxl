using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace pxl
{
    /// <summary>
    /// 
    /// </summary>
    public static class MeshConvert
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmesh"></param>
        /// <returns></returns>
        public static Mesh ToMesh(this BMesh bmesh)
        {
            return NaiveConvert(bmesh);
        }

        private static Mesh ReduceDuplicatedVerticesConvert(BMesh bmesh)
        {
            // TODO: implement a converter that takes into account shared vertices.

            /*
             * A shared vertex is duplicated when its attribute changes among the faces.
             *
             * 
             * What are the normals of a triangulation of a smooth polygon?
             * 
             */

            /*
             * Strategy.
             * Start by a triangulation.
             *  Keep track 
             * 
             */

            Mesh mesh = new GLMesh();

            // compute the number of triangles and vertices
            int ntris = 0;
            int nverts = 0;
            foreach (var f in bmesh.faces)
            {
                ntris += f.count - 2;
                nverts += f.count;
            }

            // allocate
            if (bmesh.verts.Count > 0)
                mesh.positions = new Vector3[nverts];

            if (bmesh.verts.Count > 0)
                mesh.normals = new Vector3[nverts];

            if (bmesh.uvs.Count > 0)
                mesh.uvs = new Vector2[nverts];

            if (bmesh.colors.Count > 0)
                mesh.colors = new Color4[nverts];

            mesh.triangles = new uint[ntris * 3];

            // triangulate faces
            int voffset = 0;
            int toffset = 0;
            foreach (var f in bmesh.faces)
            {
                Vector3[] poly = new Vector3[f.count];
                Vector3 faceNormal = new Vector3();
                for (int l = 0; l < f.count; ++l)
                {
                    int vi = bmesh.loops[f.loop + l].v;
                    poly[l] = bmesh.verts[vi].ve;
                    faceNormal += bmesh.verts[vi].no;
                }
                faceNormal.Normalize();


                uint[] tris = EarClippingTriangulation.Triangulate(poly, faceNormal);

                // Copy position
                for (int i = 0; i < poly.Length; ++i)
                {
                    mesh.positions[voffset + i] = poly[i];
                }

                if (mesh.hasNormals)
                {
                    if (f.smooth)
                    {
                        for (int i = 0; i < poly.Length; ++i)
                        {
                            int vi = bmesh.loops[f.loop + i].v;

                            mesh.normals[voffset + i] = bmesh.verts[vi].no;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < poly.Length; ++i)
                        {
                            int vi = bmesh.loops[f.loop + i].v;

                            mesh.normals[voffset + i] = faceNormal;
                        }
                    }
                }

                if (mesh.hasUvs)
                    for (int i = 0; i < poly.Length; ++i)
                    {
                        mesh.uvs[voffset + i] = bmesh.uvs[f.loop + i].uv;
                    }

                if (mesh.hasColors)
                    for (int i = 0; i < poly.Length; ++i)
                    {
                        mesh.colors[voffset + i] = bmesh.colors[f.loop + i].color;
                    }

                // Copy triangles
                for (int i = 0; i < tris.Length; ++i)
                {
                    mesh.triangles[toffset + i] = (uint)voffset + tris[i];
                }

                voffset += poly.Length;
                toffset += tris.Length;

            }

            mesh.Apply();

            return mesh;
        }

        private static Mesh NaiveConvert(BMesh bmesh)
        {
            /**
             * This is a non-optimal method to convert a BMesh to a Mesh,
             * since shared vertices are duplicated.
             * 
             * But this implementation can be used as a reference
             * whem implementing other converter.
             */
            Mesh mesh = new GLMesh();

            // compute the number of triangles and vertices
            int ntris = 0;
            int nverts = 0;
            foreach (var f in bmesh.faces)
            {
                ntris += f.count - 2;
                nverts += f.count;
            }

            // allocate
            if (bmesh.verts.Count > 0)
                mesh.positions = new Vector3[nverts];

            if (bmesh.verts.Count > 0)
                mesh.normals = new Vector3[nverts];

            if (bmesh.uvs.Count > 0)
                mesh.uvs = new Vector2[nverts];

            if (bmesh.colors.Count > 0)
                mesh.colors = new Color4[nverts];

            mesh.triangles = new uint[ntris * 3];

            // triangulate faces
            int voffset = 0;
            int toffset = 0;
            foreach (var fa in bmesh.faces)
            {
                Vector3[] poly = new Vector3[fa.count];
                Vector3 faceNormal = new Vector3();
                for (int l = 0; l < fa.count; ++l)
                {
                    int vi = bmesh.loops[fa.loop + l].v;
                    poly[l] = bmesh.verts[vi].ve;
                    faceNormal += bmesh.verts[vi].no;
                }
                faceNormal.Normalize();


                uint[] tris = EarClippingTriangulation.Triangulate(poly, faceNormal);

                // Copy position
                for (int i = 0; i < poly.Length; ++i)
                {
                    mesh.positions[voffset + i] = poly[i];
                }

                if (mesh.hasNormals)
                {
                    if (fa.smooth)
                    {
                        for (int i = 0; i < poly.Length; ++i)
                        {
                            int vi = bmesh.loops[fa.loop + i].v;

                            mesh.normals[voffset + i] = bmesh.verts[vi].no;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < poly.Length; ++i)
                        {
                            int vi = bmesh.loops[fa.loop + i].v;

                            mesh.normals[voffset + i] = faceNormal;
                        }

                    }
                }

                if (mesh.hasUvs)
                    for (int i = 0; i < poly.Length; ++i)
                    {
                        mesh.uvs[voffset + i] = bmesh.uvs[fa.loop + i].uv;
                    }

                if (mesh.hasColors)
                    for (int i = 0; i < poly.Length; ++i)
                    {
                        mesh.colors[voffset + i] = bmesh.colors[fa.loop + i].color;
                    }

                // Copy triangles
                for (int i = 0; i < tris.Length; ++i)
                {
                    mesh.triangles[toffset + i] = (uint)voffset + tris[i];
                }

                voffset += poly.Length;
                toffset += tris.Length;

            }

            mesh.Apply();

            return mesh;
        }
    }
}
