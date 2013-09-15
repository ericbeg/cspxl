using System;
using System.Collections.Generic;
namespace pxl
{
	public class GameObject
	{
		private List<Component> m_components;
		
		
		public GameObject()
		{
			m_components = new List<Component>();
		}
		
		public Component[] components
		{
			get
			{
				return m_components.ToArray();
			}
		}
	
		public void AddComponent<T>()
		{
			//Component component = new T();
			//m_components.Add( component );
		}
		
	}
}
