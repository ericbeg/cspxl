using System;
using System.Collections.Generic;
namespace pxl
{
	public class GameObject : IDisposable
	{
        private static List<GameObject> m_instances = new List<GameObject>();

        public static GameObject[] instances
        {
            get
            {
                return m_instances.ToArray();
            }
        }

		private List<Component> m_components;
		
		
		public GameObject()
		{
			m_components = new List<Component>();
            Transform t = new Transform(this);
            this.m_components.Add(t);
            m_instances.Add(this);
		}

        public void Dispose()
        {
            Transform tr = GetComponent<Transform>();
            foreach (var t in tr.children)
            {
                t.gameObject.Dispose();
            }
            tr.parent = null;
            m_instances.Remove( this );
        }

		public Component[] components
		{
			get
			{
				return m_components.ToArray();
			}
		}
	
		public T AddComponent<T>()
			where T:  new( )
		{
			T newT  = new T();
			
			Component component = newT as Component;
			if( component != null )
			{
				component.m_gameObject = this;
				m_components.Add( component );
			}

            return newT;
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
