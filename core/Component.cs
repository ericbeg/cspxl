#pragma  warning disable 1591

using System;
using System.Collections.Generic;

namespace pxl
{
	public class Component : IDisposable
	{
        private static List<Component> m_instances = new List<Component>();

        internal GameObject m_gameObject = null;
        private bool m_hasStarted = false;

        public static Component[] instances { get { return m_instances.ToArray(); } }
        public Component()
        {
            m_instances.Add(this);
        }

        virtual public void Dispose()
        {
            m_instances.Remove(this);
        }

        public bool enable = true;


        internal virtual void InternalStart()
        {

        }


        internal virtual void InternalUpdate()
        {
            if (!m_hasStarted)
            {
                m_hasStarted = true;
                InternalStart();
            }
        }

        internal virtual void InternalFixedUpdate()
        {

        }

        
        public Component(GameObject gameObject)
		{
			m_gameObject  = gameObject;
		}
		
		public GameObject gameObject
		{
			get
			{
				return m_gameObject;
			}
		}
		
	}
}
