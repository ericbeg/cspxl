using System;
namespace pxl
{
	public class Component
	{
		internal GameObject m_gameObject = null;
		
		internal Component(){}
		
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
