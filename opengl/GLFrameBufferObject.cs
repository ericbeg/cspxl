#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace pxl
{
    class GLFrameBufferObject : FrameBufferObject
    {

        private uint glname;
        private uint glname_depth;
        private List<int> glname_textures = new List<int>();

        public GLFrameBufferObject(int width, int height)
            : this(width, height, 24)
        {
        }

        public GLFrameBufferObject(int width, int height, int depthFormat)
            : this(width, height, depthFormat, Texture2D.Format.RGBA32 )
        {
        }

        public GLFrameBufferObject(int width, int height, int depthFormat, Texture2D.Format format)
            : base( width, height, depthFormat, format)
        {
            GL.GenFramebuffers(1, out glname);
            GL.BindFramebuffer( FramebufferTarget.Framebuffer, glname);
            GenDepthBuffer();
        }

        public override void Dispose()
        {
            base.Dispose();
            GL.DeleteFramebuffers(1, ref glname);
            GL.DeleteRenderbuffers(1, ref glname_depth);
            glname = 0;
        }

        void GenDepthBuffer()
        {
            // Solve render buffer format
            RenderbufferStorage format = RenderbufferStorage.DepthComponent24;
            switch (depthFormat)
            {
                case 16: format = RenderbufferStorage.DepthComponent16; break;
                case 24: format = RenderbufferStorage.DepthComponent24; break;
                case 32: format = RenderbufferStorage.DepthComponent32; break;
            }

            // create depth render buffer
            GL.GenRenderbuffers(1, out glname_depth);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, glname_depth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, format, width, height);

            // Attach to depth FBO
            GL.FramebufferRenderbuffer(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthAttachment,
                RenderbufferTarget.Renderbuffer,
                glname_depth);
        }

        void AttachColorTextures()
        {
            glname_textures.Clear();
            foreach (var t in m_textures)
            {
                GLTexture2D glt = t as GLTexture2D;
                if (glt != null)
                {
                    int attachmentIndex = glname_textures.Count;

                    GL.FramebufferTexture2D(
                        FramebufferTarget.Framebuffer,
                        (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0 + attachmentIndex),
                        TextureTarget.Texture2D,
                        glt.glname,
                        0
                        );

                    glname_textures.Add(glt.glname);
                }
            }
        }

        public override bool IsComplete()
        {
            bool itIs = false;

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, glname);
            FramebufferErrorCode code = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            itIs = code == FramebufferErrorCode.FramebufferComplete;
            return itIs;
        }

        public override void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, glname);
            FrameBufferObject.active = this;
        }

        public override void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            FrameBufferObject.active = null;
        }

    }
}
