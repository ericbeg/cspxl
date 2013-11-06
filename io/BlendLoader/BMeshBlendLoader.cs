#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using OpenTK.Graphics;

namespace pxl
{
    public class BMeshBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
        {
            Mesh me = null;
            BMesh bme = LoadBmesh(bvar) as BMesh;
            if (bme != null)
            {
                me = bme.ToMesh();
            }
            return me;
        }

        private Object LoadBmesh(BlendFile.BlendVar bvar)
        {
            BMesh bm = null;
            if (bvar.type == "Mesh")
            {
                bm = new BMesh();
                BinaryReader reader = bvar.blendFile.binaryReader;

                // Load vertices
                /*
                #0056: struct MVert (20 bytes)
                {
                   float co[3];
                   short no[3];
                   char flag;
                   char bweight;
                };
                */
                BlendFile.FileBlock fbVerts = bvar["mvert"];;

                if ( fbVerts != null && fbVerts.count > 0)
                {

                   long ptr0 = bvar.blendFile.binaryReader.BaseStream.Position;
                   bm.verts.Capacity = fbVerts.count;
                   for (int i = 0; i < fbVerts.count; ++i)
                   {
                      fbVerts.Seek( i );

                      BMesh.BMVert v = new BMesh.BMVert();

                      v.ve = reader.ReadVector3();

                      short[] sno = new short[3];
                      sno[0] = reader.ReadInt16();
                      sno[1] = reader.ReadInt16();
                      sno[2] = reader.ReadInt16();

                      v.no.x = sno[0];
                      v.no.y = sno[1];
                      v.no.z = sno[2];

                      v.no.Normalize();
                      bm.verts.Add( v );
                   }
                }
                // Load edges
                /*
                #0053: struct MEdge (12 bytes)
                {
                   int v1;
                   int v2;
                   char crease;
                   char bweight;
                   short flag;
                };
                */
               
               BlendFile.FileBlock fbMedges = bvar["medge"];
               if (fbMedges != null && fbMedges.count > 0)
               {
                  bm.edges.Capacity = fbMedges.count;
                  for ( int i=0; i < fbMedges.count; ++i )
                  {
                     fbMedges.Seek( i );
                     BMesh.BMEdge e ;
                     
                     e.v1 = reader.ReadInt32();
                     e.v2 = reader.ReadInt32();

                     bm.edges.Add( e );
                  }
               }
                // Load loops
                /*
                #0059: struct MLoop (8 bytes)
                {
                   int v;
                   int e;
                };
                */

                
               BlendFile.FileBlock fbLoops = bvar["mloop"];
               if( fbLoops != null && fbLoops.count > 0 )
               {
                  bm.loops.Capacity = fbLoops.count;
                  for ( int i=0; i < fbLoops.count; ++i )
                  {
                     fbLoops.Seek( i );
                     BMesh.BMLoop l;
                     l.v = reader.ReadInt32();
                     l.e = reader.ReadInt32();
                     bm.loops.Add( l );
                  }
               }

                // Load loops UV
                /*
                #0061: struct MLoopUV (12 bytes)
                {
                   float uv[2];
                   int flag;
                };
                */

               BlendFile.FileBlock fbMloopUVs = bvar["mloopuv"];
               if (fbMloopUVs != null && fbMloopUVs.count > 0)
               {
                   bm.uvs.Capacity = fbMloopUVs.count;
                   for (int i = 0; i < fbMloopUVs.count; ++i)
                   {
                       fbMloopUVs.Seek(i);
                       BMesh.BMLoopUV l;
                       l.uv = reader.ReadVector2();
                       bm.uvs.Add(l);
                   }
               }
                // Load loops Colors
                /*
                #0062: struct MLoopCol (4 bytes)
                {
                   char r;
                   char g;
                   char b;
                   char a;
                };
                */

               BlendFile.FileBlock fbMloopCols = bvar["mloopcols"];
               if (fbMloopCols != null && fbMloopCols.count > 0)
               {
                   bm.uvs.Capacity = fbMloopCols.count;
                   for (int i = 0; i < fbMloopCols.count; ++i)
                   {
                       fbMloopCols.Seek(i);
                       byte[] c = new byte[4];

                       c[0] = reader.ReadByte();
                       c[1] = reader.ReadByte();
                       c[2] = reader.ReadByte();
                       c[3] = reader.ReadByte();

                       BMesh.BMLoopCol l;
                       Color4 col;
                       col.R = (float)c[0] / 255.0f;
                       col.G = (float)c[1] / 255.0f;
                       col.B = (float)c[2] / 255.0f;
                       col.A = (float)c[3] / 255.0f;

                       l.color = col ;
                       bm.colors.Add(l);
                   }
               }

                // Load polygons
                /*
                #0058: struct MPoly (12 bytes)
                {
                   int loopstart;
                   int totloop;
                   short mat_nr;
                   char flag;
                   char pad;
                };
                */
                // from blender/makesdna/DNA_meshdata_types.h +290

               const uint ME_SMOOTH = 1;
               BlendFile.FileBlock fbMpolys = bvar["mpoly"];
               if (fbMpolys != null && fbMpolys.count > 0)
               {
                   bm.faces.Capacity = fbMpolys.count;
                   for (int i = 0; i < fbMpolys.count; ++i)
                   {
                       fbMpolys.Seek(i);
                       BMesh.BMFace f = new BMesh.BMFace();
                       f.loop  = reader.ReadInt32();
                       f.count = reader.ReadInt32();
                       f.matidx = reader.ReadInt16();
                       byte flag = reader.ReadByte();
                       f.smooth = (flag & ME_SMOOTH) == ME_SMOOTH;
                       bm.faces.Add(f);
                   }
               }

            }// if ( bvar.type == "Mesh" )
            return bm;
        }
    }
}

