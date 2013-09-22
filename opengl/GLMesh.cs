using System;
using pxl;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace cspxl
{
	
	
	public class GLMesh
	{
		
		uint[] vbo = new uint[1];
		uint[] tbo = new uint[1];
		
		bool m_isInitialized = false;
		
		GLMeshVertexAttributeFormat[] vertexAttributeFormat;
		
		bool isInitialized
		{
			get
			{
				return m_isInitialized;
			}
		}
		
		public GLMesh ()
		{
			m_isInitialized = false;
		}
		
		public void Create( Mesh mesh )
		{
			GL.GenBuffers(1, vbo);
			GL.GenBuffers(1, tbo);
			
			Byte[] buffer = BitConverter.GetBytes( 1.00 );
			
			unsafe
			{
				fixed( Byte* pbuffer = buffer ) 
				{
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) buffer.Length, (IntPtr) pbuffer, BufferUsageHint.StaticCopy);
				}
			}
		}
		
		public void Free()
		{
			GL.DeleteBuffers(1,  vbo  );
			GL.DeleteBuffers(1,  tbo  );
		}
		
		
		public void Draw()
		{
			GL.BindBuffer( BufferTarget.ArrayBuffer, vbo[0] );
			
			int stride = 0;
			foreach( GLMeshVertexAttributeFormat format in vertexAttributeFormat )
			{
				// Vertex attribute
				uint index = 0;
				int size = 0;
				VertexAttribPointerType type = format.type;
				bool normalized = false;
				IntPtr ptr = (IntPtr) 0;
				
				GL.VertexAttribPointer( index, size, type,  normalized, stride, ptr );
				stride += format.count*SizeOf(format.type);
			}
				
			// Triangle indexes
			GL.BindBuffer( BufferTarget.ArrayBuffer, tbo[0] );
			
			//GL.DrawElements();
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
			public VertexAttribPointerType type;
			public int count;
		}
	
		
	}
}

