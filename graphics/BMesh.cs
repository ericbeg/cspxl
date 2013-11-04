#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace pxl
{
    public class BMesh
    {
        public List<BMFace> faces = new List<BMFace>();
        public List<BMLoop> loops = new List<BMLoop>();
        public List<BMLoopUV> uvs = new List<BMLoopUV>();
        public List<BMLoopCol> cols = new List<BMLoopCol>();

        public List<BMEdge> edges = new List<BMEdge>();
        public List<BMVert> verts = new List<BMVert>();

        public struct BMVert
        {
            public Vector3 ve;
            public Vector3 no;
        }


        public struct BMEdge
        {
            public int v1;
            public int v2;
        }


        public struct BMLoop
        {
            public int v;
            public int e;
        }


        public struct BMLoopUV
        {
            public Vector2 uv;
        }


        public struct BMLoopCol
        {
            public Color4 col;
        }

        public struct BMFace
        {
            public int loop;
            public int count;
            public int matidx;
            public bool smooth;
        }
    }
}
