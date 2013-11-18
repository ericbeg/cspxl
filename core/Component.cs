#pragma  warning disable 1591

using System;
namespace pxl
{
	public class Component
	{
		internal GameObject m_gameObject = null;
		
		internal Component(){}

        public bool enable = true;
		
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
