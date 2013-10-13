using System;
using pxl;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace pxl
{
	public class GLMesh : Mesh
	{
		
		uint[] vbo = null;
		uint[] tbo = null;
		
		bool m_isReady = false;
		int m_triscount;
		int m_vertcount;
		
		GLMeshVertexAttributeFormat[] vertexAttributeFormat;
		
        public GLMesh()
            : base()
        {
            vbo = new uint[1];
            tbo = new uint[1];
            m_isReady = false;

        }
		public bool isReady
		{
			get
			{
				return m_isReady;
			}
		}
		
		public override void Apply()
		{
            Mesh mesh = this;

			mesh.SanityCheck ();

            GL.GenBuffers(1, vbo);
            GL.GenBuffers(1, tbo);
            GLHelper.CheckError();

            m_triscount = mesh.triscount;
			m_vertcount = mesh.vertcount;

			// process mesh format
			List<GLMeshVertexAttributeFormat> attrFormat = new List<GLMeshVertexAttributeFormat> ();
			attrFormat.Add ( new GLMeshVertexAttributeFormat("position", VertexAttribPointerType.Float, 3 ));

			if (mesh.hasNormals)
				attrFormat.Add ( new GLMeshVertexAttributeFormat( "normal" , VertexAttribPointerType.Float, 3 ));			

			if (mesh.hasUvs)
				attrFormat.Add ( new GLMeshVertexAttributeFormat( "uv" , VertexAttribPointerType.Float, 2 ));

			if (mesh.hasColors)
				attrFormat.Add ( new GLMeshVertexAttributeFormat( "color" , VertexAttribPointerType.Float, 4 ));

			vertexAttributeFormat = attrFormat.ToArray ();

			// compute vertex atribute buffer size
			int strideSize = 0;
			foreach (GLMeshVertexAttributeFormat f in vertexAttributeFormat) 
			{
				strideSize += f.count * SizeOf (f.type);
			}

			int attrBufferSize = strideSize * mesh.vertcount;

			// pack vertex attribute into buffer
			Byte[] attrBuffer = new byte[attrBufferSize];

			int attrIdx = 0;
			int offset = 0;

			// POSITION
			{
				int attributeSize = vertexAttributeFormat [attrIdx].count * SizeOf (vertexAttributeFormat [attrIdx].type);
				for (int v=0; v < mesh.vertcount; ++v) 
                {
					Vector3 pos = mesh.positions [v];
					Buffer.BlockCopy (pos.GetBytes (), 0, attrBuffer, offset + v * attributeSize, attributeSize);
				}
				++attrIdx;
				offset += attributeSize;
			}

			// NORMAL
			if (mesh.hasNormals)
			{
				int attributeSize = vertexAttributeFormat [attrIdx].count * SizeOf (vertexAttributeFormat [attrIdx].type);
				for( int v=0; v < mesh.vertcount; ++v)
				{
					Vector3 nor = mesh.normals[v];
					Buffer.BlockCopy(  nor.GetBytes(), 0, attrBuffer, offset + v * attributeSize, attributeSize);
				}
				++attrIdx;
				offset += attributeSize;
			}

			// UV
			if (mesh.hasUvs)
			{
				int attributeSize = vertexAttributeFormat [attrIdx].count * SizeOf (vertexAttributeFormat [attrIdx].type);
				for( int v=0; v < mesh.vertcount; ++v)
				{
					Vector2 uv = mesh.uvs[v];
					Buffer.BlockCopy(  uv.GetBytes(), 0, attrBuffer, offset + v * attributeSize, attributeSize);
				}
				++attrIdx;
				offset += attributeSize;
			}

			// COLOR
			if (mesh.hasColors)
			{
				int attributeSize = vertexAttributeFormat [attrIdx].count * SizeOf (vertexAttributeFormat [attrIdx].type);
				for( int v=0; v < mesh.vertcount; ++v)
				{
					Vector4 col = mesh.colors[v];
					Buffer.BlockCopy(  col.GetBytes(), 0, attrBuffer, offset + v * attributeSize, attributeSize);
				}
				++attrIdx;
				offset += attributeSize;
			}


			// Triangles indices transfert
			unsafe 
			{
				fixed( uint* pbuffer = mesh.triangles ) 
				{
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, tbo[0]);
                    GLHelper.CheckError();

					GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)( mesh.triscount*3*sizeof(uint)), (IntPtr) pbuffer, BufferUsageHint.StaticCopy);
                    GLHelper.CheckError();
                }
			}

			// Vertex buffer data transfert
			unsafe
			{
				fixed( Byte* pbuffer = attrBuffer ) 
				{
					GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[0]);
                    GLHelper.CheckError();
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) attrBuffer.Length, (IntPtr) pbuffer, BufferUsageHint.StaticCopy);
                    GLHelper.CheckError();
				}
			}
		}
		
		public void Free()
		{
			GL.DeleteBuffers(1,  vbo  );
			GL.DeleteBuffers(1,  tbo  );
            GLHelper.CheckError();
		}
		
	    public override void Bind()
        {
            GL.BindBuffer( BufferTarget.ArrayBuffer, vbo[0] );
            GLHelper.CheckError();

            int stride = 0;
            // Vertex attribute
            for(int i = 0; i  < vertexAttributeFormat.Length; ++i)
            {
                GLMeshVertexAttributeFormat format = vertexAttributeFormat [i];
                GLHelper.CheckError();
                int size = format.count;
                VertexAttribPointerType type = format.type;
                bool normalized = false;
                IntPtr ptr = (IntPtr) 0;

                GLShader shader = Shader.active as GLShader;


               int attribLocation = GL.GetAttribLocation(shader.glname, format.name);

               GL.VertexAttribPointer(attribLocation, size, type, normalized, stride, ptr);
                GLHelper.CheckError();
                stride += format.count*SizeOf(format.type);
            }

        }

		public override void Draw()
        {            
            GLHelper.CheckError();
            // Triangle indexes
           
            GL.BindBuffer( BufferTarget.ElementArrayBuffer, tbo[0] );
            GLHelper.CheckError();
			GL.DrawElements(BeginMode.Triangles, m_vertcount, DrawElementsType.UnsignedInt, 0 );
            GLHelper.CheckError();
		}
		
		
		int SizeOf( VertexAttribPointerType type )
		{
			int size = 0;
			switch( type )
			{
			case VertexAttribPointerType.Byte: 			size = sizeof( byte )	; break;
			case VertexAttribPointerType.Double: 		size = sizeof( double )	; break;
			case VertexAttribPointerType.Float: 		size = sizeof( float)	; break;
			case VertexAttribPointerType.HalfFloat: 	size = sizeof( float )/2; break;
			case VertexAttribPointerType.Int: 			size = sizeof( int )	; break;
			case VertexAttribPointerType.Short: 		size = sizeof( short )	; break;
			case VertexAttribPointerType.UnsignedByte: 	size = sizeof( byte )	; break;
			case VertexAttribPointerType.UnsignedInt: 	size = sizeof( uint )	; break;
			case VertexAttribPointerType.UnsignedShort: size = sizeof( ushort )	; break;
			}
			return size;
		}
			
		public class GLMeshVertexAttributeFormat
		{
			public GLMeshVertexAttributeFormat( string name, VertexAttribPointerType type, int count)
			{
				this.name  = name;
				this.type  = type;
				this.count = count;
			}

			public string name;
			public VertexAttribPointerType type;
			public int count;
		}
	
		
	}
}

