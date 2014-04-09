#pragma  warning disable 1591

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace pxl
{
	public abstract class Mesh
	{
        public static Mesh active = null;


		public int vertcount{ get{return positions.Length;} }
		public int triscount{ get{return triangles.Length/3;} }
		
		public Vector3[] positions;
		
        public Vector3[] normals;
        public Vector3[] tangents;
        public Vector3[] binormals;

        public Vector2[] uvs;
		public Color4[] colors;

        public const int BONE_PER_VERTEX = 4;
        public byte[] boneIndices;
        public byte[] boneWeights;
		
        public uint[] triangles;
		
		public bool hasPositions 	{ get{return positions 	!= null; } }
		public bool hasNormals 		{ get{return normals 	!= null; } }
		public bool hasUvs 			{ get{return uvs 		!= null; } }
		public bool hasColors 		{ get{return colors 	!= null; } }
        public bool isSkinned       { get{return boneIndices != null && boneWeights != null; } }
		
		public void SanityCheck()
		{
			
			if ( ! hasPositions )
			{
				throw new MeshNoPositionsException();	
			}
			
			if ( triangles != null )
			{
				foreach( uint idx in triangles )
				{
					if ( ! ( 0 <= idx && idx < vertcount) )
					{
						throw  new MeshInvalidTriangleIndexesException();
					}
				}
			}
			else
			{
				throw  new MeshInvalidTriangleIndexesException();
			}

            if (boneIndices != null && boneIndices.Length != vertcount * BONE_PER_VERTEX)
            {
                throw new MeshInvalidSkinningException();
            }

            if (boneWeights != null && boneWeights.Length != vertcount * BONE_PER_VERTEX)
            {
                throw new MeshInvalidSkinningException();
            }

		}


        public abstract void Apply();
        public abstract void Draw();
		
		
		class MeshNoPositionsException : Exception{}
		class MeshInvalidTriangleIndexesException : Exception{}
        class MeshInvalidSkinningException : Exception { }

	}
}
