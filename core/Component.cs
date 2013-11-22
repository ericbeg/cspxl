#pragma  warning disable 1591

using System;
using System.Collections.Generic;

namespace pxl
{
	public class Component : IDisposable
	{
        private static List<Component> m_instances = new List<Component>();

        public static Component[] instances { get { return m_instances.ToArray(); } }
        internal GameObject m_gameObject = null;
		
		public Component()
        {
            m_instances.Add(this);
        }

        virtual public void Dispose()
        {
            m_instances.Remove(this);
        }

        public bool enable = true;
		

        internal virtual void InternalUpdate()
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
