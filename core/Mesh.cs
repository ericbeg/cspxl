using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
	public abstract class Mesh
	{
		public int vertcount;
		public int triscount;
		
		public Vector3[] positions;
		public Vector3[] normals;
		public Vector2[] uvs;
		public Vector4[] colors;
		
		public int[] triangles;
		
		
		public bool hasPositions 	{ get{return positions 	!= null; } }
		public bool hasNormals 		{ get{return normals 	!= null; } }
		public bool hasUvs 			{ get{return uvs 		!= null; } }
		public bool hasColors 		{ get{return colors 	!= null; } }
		
		
		public void SanityCheck()
		{
			
			if ( ! hasPositions )
			{
				throw new MeshNoPositionsException();	
			}
			
			if ( triangles != null )
			{
				foreach( int idx in triangles )
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
		}


        public abstract void Apply();
        public abstract void Draw();
		
		
		class MeshNoPositionsException : Exception{}
		class MeshInvalidTriangleIndexesException : Exception{}

	}
}
