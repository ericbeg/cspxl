#pragma  warning disable 1591

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

        public Application(int width, int height)
            : base(width, height, new GraphicsMode(32, 24))
        {
            RegisterBlendLoaders();

            this.Mouse.Move += (object sender, MouseMoveEventArgs args) =>
            {
                Input.mouse_x = args.X;
                Input.mouse_y = args.Y;
            };

            Graphics.screenApect = (float)width / (float)height;
            Screen.width = width;
            Screen.height = height;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            
            int width = Width;
            int height = Height;

            Graphics.screenApect = (float)width / (float)height;
            Screen.width = width;
            Screen.height = height;
        }

        private void RegisterBlendLoaders()
        {
            BlendFile.Register(new CameraBlendLoader(), "CA");
            BlendFile.Register(new ImageBlendLoader(), "IM");
            BlendFile.Register(new LightBlendLoader(), "LA");
            BlendFile.Register(new MaterialBlendLoader(), "MA");
            BlendFile.Register(new BMeshBlendLoader(), "ME");
            BlendFile.Register(new ObjectBlendLoader(), "OB");
            BlendFile.Register(new SceneBlendLoader(), "SC");
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
            
            // Update components

            var components = Component.instances;
            foreach (var c in components)
            {
                c.InternalUpdate();
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
            Graphics.RenderFrame();
            this.SwapBuffers();
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

			
	}
}
