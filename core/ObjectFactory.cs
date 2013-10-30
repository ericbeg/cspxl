﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public class ObjectFactory
    {

        public static Mesh BuildMeshQuad()
        {
            Mesh me = new GLMesh();
            me.vertcount = 4;

            me.positions = new Vector3[]
		    {
			    new Vector3(-1, -1, 0),
			    new Vector3(1, -1, 0),
			    new Vector3(1, 1, 0),
			    new Vector3(-1, 1, 0)
		    };

            me.normals = new Vector3[]
		    {
			    new Vector3(0.0f, 0.0f, 1.0f),
			    new Vector3(0.0f, 0.0f, 1.0f),
			    new Vector3(0.0f, 0.0f, 1.0f),
			    new Vector3(0.0f, 0.0f, 1.0f)
		    };

            me.uvs = new Vector2[]
		    {
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f)
		    };

            me.triscount = 2;
            me.triangles = new uint[]
		    {
			    0, 1, 2,
			    0, 2, 3
		    };


            GLMesh glme = me as GLMesh;
            glme.Apply();

            return me;
        }

        public static Mesh BuildMeshCube()
        {
            Mesh me = new GLMesh();
            me.vertcount = 24;
            float s = 0.5f;
            me.positions = new Vector3[]
		    {
                // front
			    new Vector3(-s, -s, s),
			    new Vector3( s, -s, s),
			    new Vector3( s,  s, s),
			    new Vector3(-s,  s, s),

                // back
			    new Vector3(-s, -s, -s),
			    new Vector3( s, -s, -s),
			    new Vector3( s,  s, -s),
			    new Vector3(-s,  s, -s),

                // left
			    new Vector3(-s, -s, -s),
			    new Vector3(-s, -s,  s),
			    new Vector3(-s,  s,  s),
			    new Vector3(-s,  s, -s),

                // right
			    new Vector3( s, -s, -s),
			    new Vector3( s, -s,  s),
			    new Vector3( s,  s,  s),
			    new Vector3( s,  s, -s),

                // top
			    new Vector3(-s, s, -s),
			    new Vector3(-s, s,  s),
			    new Vector3( s, s,  s),
			    new Vector3( s, s, -s),

                // bottom
			    new Vector3(-s, -s, -s),
			    new Vector3(-s, -s,  s),
			    new Vector3( s, -s,  s),
			    new Vector3( s, -s, -s),



		    };

            me.normals = new Vector3[]
		    {
                // front
			    Vector3.zAxis,
			    Vector3.zAxis,
			    Vector3.zAxis,
			    Vector3.zAxis,

                // back
			    -Vector3.zAxis,
			    -Vector3.zAxis,
			    -Vector3.zAxis,
			    -Vector3.zAxis,

                 // left
			    -Vector3.xAxis,
			    -Vector3.xAxis,
			    -Vector3.xAxis,
			    -Vector3.xAxis,

                // right
			    Vector3.xAxis,
			    Vector3.xAxis,
			    Vector3.xAxis,
			    Vector3.xAxis,

                // bottom
			    -Vector3.yAxis,
			    -Vector3.yAxis,
			    -Vector3.yAxis,
			    -Vector3.yAxis,

                // top
			    Vector3.yAxis,
			    Vector3.yAxis,
			    Vector3.yAxis,
			    Vector3.yAxis

		    };

            me.uvs = new Vector2[]
		    {
                // front
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f),

                // back
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f),

                // left
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f),

                // right
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f),

                // bottom
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f),

                // top
			    new Vector2(0.0f,0.0f),
			    new Vector2(1.0f, 0.0f),
			    new Vector2(1.0f, 1.0f),
			    new Vector2(0.0f, 1.0f),

		    };


            me.triscount = 12;
            me.triangles = new uint[]
		    {
                // front
			    0, 1, 2,
			    0, 2, 3,

                // back
			    4, 5, 6,
			    4, 6, 7,

                // left
			    8, 9,  10,
			    8, 10, 11,

                // right
			    12, 13, 14,
			    12, 14, 15,

                // bottom
			    16, 17, 18,
			    16, 18, 19,

                // top
			    20, 21, 22,
			    20, 22, 23

		    };


            GLMesh glme = me as GLMesh;
            glme.Apply();

            return me;
        }


        public GameObject BuildMeshObject()
        {
            return null;
        }
    }
}
