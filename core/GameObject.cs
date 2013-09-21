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
			where T:  new( )
		{
			T newT  = new T();
			
			Component component = newT as Component;
			if( component != null )
			{
				component.m_gameObject = this;
				m_components.Add( component );
			}
		}
		
		public T GetComponent<T>()
			where T: class
		{
			T comp = null;
			
			foreach( Component c in components )
			{
				var candidat = c as T;
				if( candidat != null )
				{
					comp = candidat;
					break;
				}
			}
			
			return comp;
		}
	
		public T[] GetComponents<T>()
			where T: class
		{
			List<T> comps = new List<T>();
			
			foreach( Component c in components )
			{
				var candidat = c as T;
				if( candidat != null )
				{
					comps.Add( candidat );
				}
			}
			
			return comps.ToArray();
		}
	
	}
}
