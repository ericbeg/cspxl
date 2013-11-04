#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace pxl
{
    public class BMeshBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
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
                       fbLoops.Seek(i);
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

                /*
                Blendbvar mloopcols = *bvar["*mloopcol"];
                if ( mloopcols.isValid() && mloopcols.count() > 0)
                {
                   Blendbvar col = mloopcols[0];
                   char* ptr0 = (buffer->ptr + col.offset);
                   bm->cols.reserve( mloopcols.count() );
                   for ( int i=0; i < mloopcols.count(); ++i )
                   {
                      char* ptr = ptr0+i*col.size;
                      BMLoopCol l;
                      l.col.r = *(char*)(ptr + 0*sizeof(char));
                      l.col.g = *(char*)(ptr + 1*sizeof(char));
                      l.col.b = *(char*)(ptr + 2*sizeof(char));
                      l.col.a = *(char*)(ptr + 3*sizeof(char));
                      bm->cols.push_back( l );
                      //printf("loop %d e:%d v:%d\n",i ,l.e, l.v);
                   }
                }
                */

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

                /*
                #ifndef ME_SMOOTH
                #define ME_SMOOTH     1
                #endif
                Blendbvar mpolys = *bvar["*mpoly"];
                if( mpolys.isValid() && mpolys.count() > 0 )
                {
                   Blendbvar  poly = mpolys[0];
                   char* ptr0 = (buffer->ptr + poly.offset);
                   bm->faces.reserve( mpolys.count() );
                   for ( int i=0; i < mpolys.count(); ++i )
                   {
                      char* ptr = ptr0+i*poly.size;
                      BMFace f;
                      short mat_nr;
                      char flag;
                      memcpyES(&f.loop  , ptr +      0                      , sizeof(int), 1);
                      memcpyES(&f.count , ptr +   sizeof(int)               , sizeof(int), 1);
                      memcpyES(&mat_nr  , ptr + 2*sizeof(int)               , sizeof(short), 1);
                      memcpyES(&flag    , ptr + 2*sizeof(int)+sizeof(short) , sizeof(char), 1);
                      f.matidx = mat_nr;
                      f.smooth = flag & ME_SMOOTH;
                      bm->faces.push_back( f );
                      //printf("face %d loop:%d count:%d mat:%d \n", i, f.loop, f.count, f.matidx);
                   }
                }
              */
            }// if ( bvar.type == "Mesh" )
            return bm;
        }
    }
}

