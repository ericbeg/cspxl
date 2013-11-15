#pragma  warning disable 1591

using System;
using System.Collections.Generic;
//using OpenTK;

namespace pxl
{
	public class Transform : Component
	{
        
		private Vector3    m_localPosition;
		private Vector3    m_localScale;
		private Quaternion m_localRotation;
		

		private Vector3    m_position;
		private Vector3    m_lossyScale;
		private Quaternion m_rotation;

		private List<Transform> m_children;
		
		private Transform m_parent;
		
		private Matrix4 m_localMatrix;
		private Matrix4 m_matrix;
		
		private Matrix4 m_localMatrixInv;
		private Matrix4 m_matrixInv;

		private bool m_updateLocalMatrix;
		private bool m_updateMatrix;
		private bool m_updateLocalMatrixInv;
		private bool m_updateMatrixInv;
		

		internal Transform( GameObject gameObject )
			: base( gameObject )
		{
			m_children = new List<Transform>();
			m_parent  = null;
			m_localPosition = new Vector3( 0.0f, 0.0f, 0.0f);
			m_localRotation = Quaternion.Identity;
			m_localScale    = new Vector3( 1.0f, 1.0f, 1.0f);
			
			m_position = new Vector3( 0.0f, 0.0f, 0.0f);
			m_rotation = Quaternion.Identity;
			m_lossyScale    = new Vector3( 1.0f, 1.0f, 1.0f);
			
			m_localMatrix = Matrix4.Identity;
			
			m_updateMatrix         = true;
			m_updateMatrixInv      = true;
			m_updateLocalMatrix    = true;
			m_updateLocalMatrixInv = true;

		}
		
		private void Touch()
		{
			m_updateMatrix         = true;
			m_updateLocalMatrix    = true;
			m_updateMatrixInv      = true;
			m_updateLocalMatrixInv = true;
			
			foreach( Transform child in m_children )
			{
				child.Touch();
			}
		}

        public void LookAt(Vector3 target, Vector3 up)
        {
            Vector3 f = Vector3.Normalized(target - position);

            Vector3 s = Vector3.Normalized(Vector3.Cross(f, Vector3.Normalized(up)));
            Vector3 u = Vector3.Normalized(Vector3.Cross(s, f));

            Matrix4 rot = new Matrix4(
             s.x, u.x, -f.x, 0.0f,
             s.y, u.y, -f.y, 0.0f,
             s.z, u.z, -f.z, 0.0f,
             0.0f, 0.0f, 0.0f, 1.0f);

            rotation = Quaternion.FromMatrix(rot);
        }

		public Transform[] children
        {
            get
            {
                return m_children.ToArray();
            }
        }

		public Vector3 position
		{
			get
			{
				m_position.x  = matrix.m14;
                m_position.y  = matrix.m24;
                m_position.z  = matrix.m34;
				return m_position;
			}
			
			set
			{
				Vector3 newPosition = value;
				localPosition = ancestorsMatrixInverse * newPosition;
                Touch();
			}
		}
		
		public Vector3 lossyScale
		{
			get
			{
				m_lossyScale.x = matrix.m11;
				m_lossyScale.y = matrix.m22;
				m_lossyScale.z = matrix.m33;
				
				return m_lossyScale;
			}
		}
		
		public Quaternion rotation
		{
			get
			{
				return m_rotation;
			}
			
			set
			{
				Quaternion newRotation = value;
				Matrix4 rot =  ancestorsMatrixInverse * Matrix4.Rotate( newRotation ) ;
				localRotation  = Quaternion.FromMatrix( rot );
                Touch();
            }
		}
		
		public Vector3 localPosition
		{
			get
			{
				return m_localPosition;
			}
			
			set
			{
				m_localPosition = value;
				Touch();
			}
		}
		
		public Vector3 localScale
		{
			get
			{
				return m_localScale;
			}
			
			set
			{
				m_localScale = value;
				Touch();
			}
		}
		
		public Quaternion localRotation
		{
			get
			{
				return m_localRotation;
			}
			
			set
			{
				m_localRotation = value;
				Touch();
			}
		}
		
		public Transform parent
		{
			get
			{
				return m_parent;
			}
			
			set
			{
				Transform newParent = value;
				
				if ( ! newParent.HasChild( this ) )
				{
					newParent.m_children.Add( this );
				}
				
				if ( parent != null && parent != newParent )
				{
					parent.m_children.Remove( this );
				}
				
				m_parent = newParent;
                Touch();
            }
		}
		
		private bool HasChild( Transform transform )
		{
			bool has = false;
			foreach( Transform child in m_children )
			{
				if ( child == transform )
				{
					has = true;
					break;
				}
			}
			return has;
		}
		
		public Matrix4 matrix
		{
			get
			{
				if ( m_updateMatrix )
				{
					m_matrix = ancestorsMatrix*localMatrix;
				}
				return m_matrix;
			}
		}
		
		public Matrix4 localMatrix
		{
			get
			{
				if ( m_updateLocalMatrix )
				{

                    Matrix4 trans = Matrix4.Translate(localPosition);
                    Matrix4 rot = Matrix4.Rotate(localRotation);
                    Matrix4 scale = Matrix4.Scale( localScale );
                    m_localMatrix = trans * rot * scale;
				}
				return m_localMatrix;
			}
		}
		
		public Matrix4 matrixInverse
		{
			get
			{
				if ( m_updateMatrixInv )
				{
					m_matrixInv = Matrix4.Inverse( matrix );
				}
				return m_matrixInv;
			}
		}
		
		
		public Matrix4 localMatrixInverse
		{
			get
			{
				if ( m_updateLocalMatrixInv )
				{
					m_localMatrixInv = Matrix4.Inverse( localMatrix );
				}
				return m_localMatrixInv;
			}
		}
	
		public Matrix4 ancestorsMatrix
		{
			get
			{
				if ( parent != null )
				{
					return parent.matrix;
				}
				else
				{
					return Matrix4.Identity;	
				}
			}
		}
		
		public Matrix4 ancestorsMatrixInverse
		{
			get
			{
				if ( parent != null )
				{
					return parent.matrixInverse;
				}
				else
				{
					return Matrix4.Identity;	
				}
			}
		}

		public Vector3 x
		{
			get 
			{
				Vector3 x = matrix.column1.xyz;
				x.Normalize ();
				return x;
			}
		}

		public Vector3 y
		{
			get 
			{
				Vector3 y = matrix.column2.xyz;
				y.Normalize ();
				return y;
			}
		}

		public Vector3 z
		{
			get 
			{
				Vector3 z = matrix.column3.xyz;
				z.Normalize ();
				return z;
			}
		}

	}
}
