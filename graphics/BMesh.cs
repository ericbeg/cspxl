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
        /*
         * This implementation of BMesh is only used to load meshes from blend file.
         * 
         * Information about BMesh can be found here:
         * http://wiki.blender.org/index.php/Dev:2.6/Source/Modeling/BMesh/Design
         * http://wiki.blender.org/index.php/Dev:Ref/Release_Notes/2.63/BMesh
         */

        public List<BMVert> verts = new List<BMVert>();
        public List<BMEdge> edges = new List<BMEdge>();
        public List<BMFace> faces = new List<BMFace>();

        public List<BMLoop> loops = new List<BMLoop>();
        public List<BMLoopUV> uvs = new List<BMLoopUV>();
        public List<BMLoopCol> colors = new List<BMLoopCol>();


        public struct BMVert
        {
            public Vector3 ve; // vertex position
            public Vector3 no; // vertex normal
        }


        public struct BMEdge
        {
            public int v1;
            public int v2;
        }

        public struct BMFace
        {
            public int loop;    // First loop index
            public int count;   // Number of loops
            public int matidx;  // Material index
            public bool smooth; // Flat or smooth shading?
        }

        public struct BMLoop
        {
            public int v;   // First vertex index
            public int e;   // Edge index
            //public int f;   // Face index
        }

        // The loop properties concern the first vertex of the loop ( loop.v ).
        public struct BMLoopUV
        {
            public Vector2 uv;
        }
        
        public struct BMLoopCol
        {
            public Color4 color;
        }
    }
}
