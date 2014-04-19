#pragma  warning disable 1591

using System;
using System.Collections.Generic;
namespace pxl
{
	public class GameObject : IDisposable
	{
        private static List<GameObject> m_instances = new List<GameObject>();
        private List<Component> m_components;
        private Transform m_transform;

        public static void ClearAll()
        {
            GameObject[] obs = instances;
            foreach (GameObject ob in obs)
            {
                ob.Dispose();
            }
            m_instances.Clear();
        }

        public string name = "GameObject";

        public static GameObject[] instances
        {
            get
            {
                return m_instances.ToArray();
            }
        }

		public GameObject()
		{
			m_components = new List<Component>();
            Transform t = new Transform(this);
            this.m_components.Add(t);
            m_instances.Add(this);
            m_transform = t;
		}

        ~GameObject()
        {
            Dispose();
        }

        public void Dispose()
        {
            Console.WriteLine(string.Format("Dispose {0}", this.name));
            if (transform != null)
            {
                foreach (var t in transform.children)
                {
                    t.gameObject.Dispose();
                }
                transform.parent = null;
            }

            Component[] comps = components;
            foreach (Component co in comps)
                co.Dispose();

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
            AddComponent(this, component);
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

        public Transform transform
        {
            get
            {
                return m_transform;
            }
        }

        public static GameObject Find(string name)
        {
            GameObject go = null;
            foreach (var g in m_instances)
            {
                if (g.name == name)
                {
                    go = g;
                    break;
                }
            }
            return go;
        }

        public static GameObject[] FindAll(string name)
        {
            List<GameObject> gos = new List<GameObject>(); 
            foreach (var g in m_instances)
            {
                if (g.name == name)
                {
                    gos.Add(g);
                }
            }
            return gos.ToArray();
        }

        public static GameObject[] FindRoots()
        {
            List<GameObject> gos = new List<GameObject>(); 
            foreach (var g in m_instances)
            {
                if (g.transform.parent == null)
                {
                    gos.Add(g);
                }
            }
            return gos.ToArray();
        }

        public static GameObject FindObjectOfType<T>()
            where T : Component
        {
            GameObject go = null;
            foreach (var g in m_instances)
            {
                T c = g.GetComponent<T>();
                if ( c != null )
                {
                    go = g;
                    break;
                }
            }
            return go;
        }

        public static GameObject[] FindObjectsOfType<T>()
            where T : Component
        {
            List<GameObject> gos = new List<GameObject>();
            foreach (var g in m_instances)
            {
                T c = g.GetComponent<T>();
                if (c != null)
                {
                    gos.Add(g);
                }
            }
            return gos.ToArray();
        }

        public override string ToString()
        {
            return string.Format( "{0} \"{1}\"", base.ToString(), name );
        }

        internal static void AddComponent(GameObject gameObject, Component component)
        {
            if (component != null && !gameObject.m_components.Contains(component))
            {
                component.m_gameObject = gameObject;
                gameObject.m_components.Add(component);
            }
            else
            {
                component.m_gameObject = null;
            }
        }


    }
}
