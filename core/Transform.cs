using System;
using System.Collections.Generic;
using OpenTK;

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
		
		Transform( GameObject gameObject )
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
		
		public Vector3 position
		{
			get
			{
				Vector4 pos = matrix.Column3;
				m_position.X  = pos.X;
				m_position.Y  = pos.Y;
				m_position.Z  = pos.Z;
				return m_position;
			}
			
			set
			{
				Vector3 newPosition = value;
				localPosition = ancestorsMatrixInverse.Multiply( newPosition );
			}
		}
		
		public Vector3 lossyScale
		{
			get
			{
				m_lossyScale.X = matrix.M11;
				m_lossyScale.Y = matrix.M22;
				m_lossyScale.Z = matrix.M33;
				
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
				Matrix4 rot =  ancestorsMatrixInverse*Matrix4.Rotate( newRotation );
				localRotation  = rot.ToQuaternion();
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
				
				parent = newParent;
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
					m_matrix = ancestorsMatrix*m_localMatrix;
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
					
					m_matrix = 
							Matrix4.CreateTranslation( m_localPosition )
						*	Matrix4.Rotate( m_localRotation )
						*   Matrix4.Scale( m_localScale );	
						;
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
					m_matrixInv = matrix;
					m_matrixInv.Invert();
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
					m_localMatrixInv = localMatrix;
					m_localMatrixInv.Invert();
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

		
	}
}
