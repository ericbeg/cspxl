using System;
using System.Collections.Generic;

namespace pxl
{
	public enum FilteringMode
	{
		Nearest,
		Bilinear,
		Trilinear
	}
	public class Texture
	{
		private FilteringMode m_filteringMode;
		
		public FilteringMode filteringMode
		{
			get
			{
				return m_filteringMode;
			}
			
			set
			{
				m_filteringMode = value;
			}
		}
	}
}