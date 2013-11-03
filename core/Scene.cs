#pragma  warning disable 1591

using System;
using System.Collections.Generic;

namespace pxl
{
	public class Scene
	{
		private static Scene m_instance = null;
		
		private List<GameObject> m_objects;
		
		private Scene()
		{
			m_objects = new List<GameObject>();
		}
		
		public Scene Instance
		{
			get
			{
				if( m_instance == null )
				{
					m_instance = new Scene();
				}
				
				return m_instance;
			}
		}
		
		public GameObject[] gameObjects
		{
			get
			{
				return m_objects.ToArray();
			}
		}
		
		
		public void Add( GameObject go )
		{
			m_objects.Add( go );
		}
		
		public void Remove( GameObject go )
		{
			m_objects.Remove( go );
		}
		
		public void Clear()
		{
			m_objects.Clear();
		}
	}
}

