#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public abstract class FrameBufferObject: IDisposable
    {

        protected List<string> m_textureNames = new List<string>();
        protected List<Texture2D> m_textures = new List<Texture2D>();

        public static FrameBufferObject active;

        private int m_width, m_height, m_depthFormat;
        private Texture2D.Format m_format;

        private Texture2D.Format format { get { return m_format; } set { m_format = value; } }

        public int width { get { return m_width; } }
        public int height { get { return m_height; } }
        public int depthFormat { get { return m_depthFormat; } }

        public FrameBufferObject(int width, int height)
            : this(width, height, 24)
        {
        }

        public FrameBufferObject(int width, int height, int depthFormat)
            : this(width, height, depthFormat, Texture2D.Format.RGBA32 )
        {
        }

        public FrameBufferObject(int width, int height, int depthFormat, Texture2D.Format format)
        {
            m_width = width;
            m_height = height;
            m_depthFormat = depthFormat;
            m_format = format;
        }

        public virtual void Dispose()
        {

        }

        public void AttachColorTexture(string name, Texture2D texture)
        {
            if (!m_textureNames.Contains(name))
            {
                m_textureNames.Add(name);
                m_textures.Add(texture);
            }
            else
            {
                for (int i = 0; i < m_textureNames.Count; ++i)
                {
                    if (m_textureNames[i] == name)
                    {
                        m_textures[i] = texture;
                        break;
                    }
                }
            }
        }

        public void DetachColorTexture(string name)
        {
            if (m_textureNames.Contains(name))
            {
                for (int i = 0; i < m_textureNames.Count; ++i)
                {
                    if (m_textureNames[i] == name)
                    {
                        m_textureNames.RemoveAt(i);
                        m_textures.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public abstract bool IsComplete();
        public abstract void Bind();
        public abstract void Unbind();
             

    }
}
