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

        public class BMVert
        {
            public Vector3 ve = new Vector3();
            public Vector3 no = new Vector3();
        }


        public class BMEdge
        {
            public int v1;
            public int v2;
        }


        public class BMLoop
        {
            public int v;
            public int e;
        }


        public class BMLoopUV
        {
            public Vector2 uv = new Vector2();
        }


        public class BMLoopCol
        {
            public Color4 col;
        }

        public class BMFace
        {
            public int loop;
            public int count;
            public int matidx;
            public bool smooth;
        }
    }
}
