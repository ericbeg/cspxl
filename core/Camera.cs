#pragma  warning disable 1591

using System;
using OpenTK;
using OpenTK.Graphics;

namespace pxl
{
	public class Camera : Component
	{
		private float m_fovy;
		private float m_aspect;
		private float m_near;
		private float m_far;
		private float m_scale;
		private bool  m_perspective;


		private bool m_updateView;
		private bool m_updateProjection;
		private bool m_updateViewProjection;

        private Color4 m_backgroundColor;

        public enum ClearFlag
        {

            BackgroundColor,
            DepthOnly
        }

        public Camera()
        {
            fovy = (float)Math.PI / 2.0f;
            aspect = 1.0f;
            perspective = true;
            scale = 1.0f;
            near = 0.1f;
            far = 100.0f;
            backgroundColor = Color4.CornflowerBlue;
            
            m_updateView = true;
            m_updateProjection = true;
            m_updateViewProjection = true;
        }

        public ClearFlag clearFlag = ClearFlag.BackgroundColor;
        public Color4 backgroundColor
        {
            get
            {
                return m_backgroundColor;
            }

            set
            {
                m_backgroundColor = value;
            }
        }

		private void UpdateView()
		{
			if (m_updateView || true) 
			{
				Transform t    = gameObject.transform;
				Vector3 eye    = t.position;
				Vector3 target = eye - t.z;
				Vector3 up     = t.y;

				m_viewMatrix = Matrix4.ViewLookAt (eye, target, up);
				m_updateView = false;
				m_updateViewProjection = true;
			}
		}

		private void UpdateProjection()
		{
            if (m_updateProjection || true) 
			{
				if( m_perspective )
				{
					m_projectionMatrix = Matrix4.Perspective(fovy, aspect*Graphics.screenApect, near, far); 
				}
				else
				{
					float w = scale;
					float h = scale;
                    float r = w / 2.0f;
                    float l = -r;
                    float t = h / 2.0f;
                    float b = -t;

					m_projectionMatrix = Matrix4.OrthographicProjection(l, r, b, t, near, far);
				}
				m_updateProjection = false;
				m_updateViewProjection = true;
			}

		}

		private void UpdateViewProjection()
		{
            if (m_updateViewProjection || true )
			{
				m_viewProjectionMatrix = projectionMatrix * viewMatrix;
				m_updateViewProjection = false;
			}
		}


		private Matrix4 m_viewMatrix;
		private Matrix4 m_projectionMatrix;
		private Matrix4 m_viewProjectionMatrix;

		static public Camera active = null;
		public float fovy
		{
			get 
			{
				return m_fovy;
			}

			set 
			{
				m_fovy = value;
				m_updateProjection = true;
			}
		}
		
		public float aspect
		{
			get
			{
				return m_aspect;
			}

			set
			{
				m_aspect = value;
				m_updateProjection = true;
			}
		}

		public float near
		{
			get
			{
				return m_near;
			}
			set
			{
				m_near = value;
				m_updateProjection = true;
			}
		}

		public float far
		{
			get
			{
				return m_far;
			}
			set
			{
				m_far = value;
				m_updateProjection = true;
			}
		}

		public float scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_scale = value;
				m_updateProjection = true;
			}
		}

		public bool perspective
		{
			get
			{
				return m_perspective;
			}

			set
			{
				m_perspective = value;
				m_updateProjection = true;
			}
		}


		public Matrix4 viewMatrix{get{ UpdateView(); return m_viewMatrix;}	}
		public Matrix4 projectionMatrix{get{ UpdateProjection(); return m_projectionMatrix;}	}
		public Matrix4 viewProjectionMatrix{get{ UpdateViewProjection(); return m_viewProjectionMatrix;}	}

		
	}
}
