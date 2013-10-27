using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using Matrix4 = OpenTK.Matrix4;

namespace pxl
{
	public class Application :  GameWindow
	{
		// Constructor 
		public Application() : this( 800, 600 ){ }
			
		public Application(int width, int height) : base(width, height, new GraphicsMode(32, 24) ){}
		
	    protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }
		
		
        /// <summary>
        /// Prepares the next frame for rendering.
        /// </summary>
        /// <remarks>
        /// Place your control logic here. This is the place to respond to user input,
        /// update object positions etc.
        /// </remarks>
        protected override void OnUpdateFrame( FrameEventArgs e )
        {
            base.OnUpdateFrame(e);

            // Update time data
            Time._currentFrameDate = DateTime.Now;
            Time._Update();
            Time._previousFrameDate = Time._currentFrameDate;

            // Update behaviours
            var behaviours = Behaviour.instances;
            foreach (var behaviour in behaviours)
            {
                behaviour.Update();
            }

            if (Keyboard[OpenTK.Input.Key.Escape])
            {
                this.Exit();
                return;
            }
        }



		/// <summary>
        /// Place your rendering code here.
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            ClearFrameBuffer();

            GL.Disable(EnableCap.DepthTest);


            GameObject[] objects = GameObject.instances;
            Matrix4 viewMatrix = Matrix4.Identity;
            Matrix4 projectionMatrix = Matrix4.Identity;
            Matrix4 viewProjectionMatrix = Matrix4.Identity;
            Matrix4 modelMatrix = Matrix4.Identity;
            Matrix4 modelViewMatrix = Matrix4.Identity;
            Matrix4 modelViewProjectionMatrix = Matrix4.Identity;

            Camera cam = Camera.active;
            if (cam != null)
            {
                viewMatrix = cam.viewMatrix;
                projectionMatrix = cam.projectionMatrix;
                viewProjectionMatrix = viewMatrix*projectionMatrix;
                
            }
            foreach (var go in objects)
            {
                var rdr = go.GetComponent<Renderer>();
                if ( rdr != null )
                {
                    var me = rdr.mesh;
                    var shader = rdr.material.shader;
                    if (me != null)
                    {

                        shader.Link();
                        shader.Use();

                        // Bind textures
                        Texture[] textures    = rdr.material.textures;
                        string[] samplerNames = rdr.material.shader.samplerNames;
                        for (int i = 0; i < textures.Length; ++i)
                        {

                            GLTexture texture = textures[i] as GLTexture;
                            string name = samplerNames[i];
                            if (texture != null)
                            {
                                texture.Bind(i);
                                shader.SetUniform(name, i);
                            }
                        }

                        modelMatrix = go.transform.matrix;

                        modelViewMatrix = modelMatrix*viewMatrix;
                        modelViewProjectionMatrix =  modelMatrix*modelViewMatrix;

                        shader.SetUniform("_Time", Time.t);
                        shader.SetUniform("modelMatrix", modelMatrix);

                        

                        shader.SetUniform("viewMatrix", viewMatrix);
                        shader.SetUniform("projectionMatrix", projectionMatrix);
                        shader.SetUniform("viewProjectionMatrix", viewProjectionMatrix);
                        shader.SetUniform("modelViewMatrix", modelViewMatrix);
                        shader.SetUniform("modelViewProjectionMatrix", modelViewProjectionMatrix);

	                    me.Draw();
                    }
                }
            }

            this.SwapBuffers();
            GLHelper.CheckError();
            //Thread.Sleep(100);
        }

        public void Loop()
        {
            Loop(70.0f);
        }
		public void Loop(float updateRate)
		{
            Time._startingDate = DateTime.Now;
            Time._previousFrameDate = Time._startingDate;
            using (this)
			{
                this.Run(updateRate);
			}
		}
		
		public void Quit()
		{
			
		}

        private void ClearFrameBuffer()
        {
            Camera cam = Camera.active;
            if (cam != null)
            {
                switch (cam.clearFlag)
                {
                    case Camera.ClearFlag.BackgroundColor:
                        GL.ClearColor(cam.backgroundColor);
                        GL.Clear(ClearBufferMask.ColorBufferBit);
                        break;
                    case Camera.ClearFlag.DepthOnly:
                        GL.Clear(ClearBufferMask.ColorBufferBit);
                        break;
                }
            }
        }
			
	}
}
