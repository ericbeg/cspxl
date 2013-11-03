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
                BlendFile.BlendPointer ptrVerts = (BlendFile.BlendPointer)bvar["mvert"].Read();
                BlendFile.FileBlock fbVerts = bvar.m_bf.GetFileBlockByOldPointer(ptrVerts.address);

                if ( fbVerts.count > 0)
                {
                    reader.BaseStream.Position = fbVerts.dataPosition;

                   long ptr0 = bvar.blendFile.binaryReader.BaseStream.Position;
                   bm.verts.Capacity = fbVerts.count;
                   for (int i = 0; i < fbVerts.count; ++i)
                   {
                      long ptr = ptr0 + i * fbVerts.elementSize;

                      BMesh.BMVert v = new BMesh.BMVert();

                      v.ve.x = reader.ReadSingle();
                      v.ve.y = reader.ReadSingle();
                      v.ve.z = reader.ReadSingle();

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

                /*
               Blendbvar medges = *bvar["*medge"];
               if( medges.isValid() && medges.count() > 0 )
               {
                  Blendbvar edge = medges[0];
                  char* ptr0 = (buffer->ptr + edge.offset);
                  bm->edges.reserve( medges.count() );
                  for ( int i=0; i < medges.count(); ++i )
                  {
                     char* ptr = ptr0+i*edge.size;
                     BMEdge e;

                     memcpyES( &e.v1, ptr + 0            , sizeof(int), 1);
                     memcpyES( &e.v2, ptr + sizeof(int)  , sizeof(int), 1);
                     bm->edges.push_back( e );
                     //printf("edge %d ( %d -> %d )\n", i, e.v1, e.v2);
                  }
               }
               */
                // Load loops
                /*
                #0059: struct MLoop (8 bytes)
                {
                   int v;
                   int e;
                };
                */

                /*
               Blendbvar mloops = *bvar["*mloop"];

               if( mloops.isValid() && mloops.count() > 0 )
               {
                  Blendbvar loop = mloops[0];
                  char* ptr0 = (buffer->ptr + loop.offset);
                  bm->loops.reserve( mloops.count() );
                  for ( int i=0; i < mloops.count(); ++i )
                  {
                     char* ptr = ptr0 + i*loop.size;
                     BMLoop l;
                     memcpyES( &l.v, ptr + 0          , sizeof(int), 1);
                     memcpyES( &l.e, ptr + sizeof(int), sizeof(int), 1);
                     bm->loops.push_back( l );
                     //printf("loop %d e:%d v:%d\n",i ,l.e, l.v);
                  }
               }
               */

                // Load loops UV
                /*
                #0061: struct MLoopUV (12 bytes)
                {
                   float uv[2];
                   int flag;
                };
                */

                /*
               Blendbvar mloopuvs = *bvar["*mloopuv"];
               if ( mloops.isValid() && mloopuvs.count() > 0 )
               {
                  Blendbvar uv = mloopuvs[0];
                  char* ptr0 = (buffer->ptr + uv.offset);
                  bm->uvs.reserve( mloopuvs.count() );
                  for ( int i=0; i < mloopuvs.count(); ++i )
                  {
                     char* ptr = ptr0 + i*uv.size;
                     BMLoopUV l;
                     memcpyES(&l.uv, ptr  + 0, sizeof(float), 2 );
                     bm->uvs.push_back( l );
                     //printf("loop %d e:%d v:%d\n",i ,l.e, l.v);
                  }
               }
               */

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
            return null;
        }
    }
}

