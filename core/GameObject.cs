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
			where T:  new()
		{
			T newT  = new T();
			Component component = newT as Component;
			if( component != null )
			{
				m_components.Add( component );
			}
		}
		
	}
}
