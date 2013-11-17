#pragma  warning disable 1591

using System;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace pxl
{
    public abstract class Graphics
    {
        internal static float screenApect = 1.0f;


        internal static void RenderFrame()
        {
            ClearFrameBuffer();

            //GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthTest);

            foreach (Camera cam in Camera.instances)
            {
                cam.Render();
            }
            GLHelper.CheckError();
        }


        private static void ClearFrameBuffer()
        {
            Camera cam = Camera.active;
            if (cam != null)
            {
                switch (cam.clearFlag)
                {
                    case Camera.ClearFlag.BackgroundColor:
                        GL.ClearColor(cam.backgroundColor);
                        GL.Clear(ClearBufferMask.ColorBufferBit);
                        GL.Clear(ClearBufferMask.DepthBufferBit);
                        break;
                    case Camera.ClearFlag.DepthOnly:
                        GL.Clear(ClearBufferMask.DepthBufferBit);
                        break;
                }
            }
            else
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.Clear(ClearBufferMask.DepthBufferBit);
            }
        }


        internal static void RenderActiveCamera()
        {
            Camera cam = Camera.active;

            GameObject[] objects = GameObject.instances;
            Matrix4 viewMatrix = Matrix4.Identity;
            Matrix4 projectionMatrix = Matrix4.Identity;
            Matrix4 viewProjectionMatrix = Matrix4.Identity;
            Matrix4 modelMatrix = Matrix4.Identity;
            Matrix4 modelViewMatrix = Matrix4.Identity;
            Matrix4 modelViewProjectionMatrix = Matrix4.Identity;


            if (cam != null)
            {
                viewMatrix = cam.viewMatrix;
                projectionMatrix = cam.projectionMatrix;
                viewProjectionMatrix = cam.viewProjectionMatrix;

            }

            foreach (var go in objects)
            {
                var rdr = go.GetComponent<Renderer>();
                if (rdr != null)
                {
                    var me = rdr.mesh;
                    var shader = rdr.material.shader;
                    if (shader == null)
                        shader = Shader.fallback;

                    if (me != null)
                    {

                        Shader.active = shader;
                        shader.Link();
                        shader.Use();

                        rdr.material.ApplyShaderUniforms();

                        modelMatrix = go.transform.matrix;

                        modelViewMatrix = viewMatrix * modelMatrix;
                        modelViewProjectionMatrix = projectionMatrix * modelViewMatrix;

                        shader.SetUniform("_Time", Time.t);
                        //shader.SetUniform("modelMatrix", modelMatrix);

                        shader.SetUniform("viewMatrix", viewMatrix);
                        shader.SetUniform("projectionMatrix", projectionMatrix);
                        shader.SetUniform("viewProjectionMatrix", viewProjectionMatrix);
                        shader.SetUniform("modelMatrix", modelMatrix);
                        shader.SetUniform("modelViewMatrix", modelViewMatrix);
                        shader.SetUniform("normalMatrix", modelViewMatrix.sub3);
                        shader.SetUniform("modelViewProjectionMatrix", modelViewProjectionMatrix);

                        /*
                        int uniforms;
                        GL.GetProgram((shader as GLShader).glname, ProgramParameter.ActiveUniforms, out uniforms);
                        for (int i = 0; i < uniforms; ++i)
                        {
                            string name = GL.GetActiveUniform((shader as GLShader).glname, i); 
                        }
                        */
                        me.Draw();
                    }
                }
            }


        }



    }
}

