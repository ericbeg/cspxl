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

        /*
         * Render path overview
         * -------------------
         * 
         * 
         *** Forward render path:
         * 
         * For each Camera c
         *  For each light l
         *      for each object o
         *          render o 
         *          accumulate result
         * 
         *** Deferred render path:
         * 
         * For each Camera c
         *  For each object o
         *      render o in GBuffer (Depth, normal & and surface property)
         *  For each light l
         *      render lit fragments
         *      accumulate result
         *      
         * */

        static private Matrix4 viewMatrix = Matrix4.Identity;
        static private Matrix4 projectionMatrix = Matrix4.Identity;
        static private Matrix4 viewProjectionMatrix = Matrix4.Identity;
        static private Matrix4 modelMatrix = Matrix4.Identity;
        static private Matrix4 modelViewMatrix = Matrix4.Identity;
        static private Matrix4 modelViewProjectionMatrix = Matrix4.Identity;

        internal static void RenderFrame()
        {
            ClearFrameBuffer();

            // TODO: Create a class that contains rendering state parameters ( Depth Test, Face Culling, Masks, ...)
            // and apply them here.
            //GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthTest);

            foreach (Camera cam in Camera.instances)
            {
                if (cam.enable)
                {
                    FrameBufferObject fbo = cam.fboTarget;
                    if (fbo != null)
                    {
                        fbo.Bind();
                    }

                    cam.Render();
                }
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


        private static void ApplyCamera()
        {
            Camera cam = Camera.active;

            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.Identity;
            viewProjectionMatrix = Matrix4.Identity;
            modelMatrix = Matrix4.Identity;
            modelViewMatrix = Matrix4.Identity;
            modelViewProjectionMatrix = Matrix4.Identity;


            if (cam != null)
            {
                viewMatrix = cam.viewMatrix;
                projectionMatrix = cam.projectionMatrix;
                viewProjectionMatrix = cam.viewProjectionMatrix;
            }

        }

        internal static void RenderActiveCameraForward()
        {
            ApplyCamera();
            GameObject[] objects = GameObject.instances;
            foreach (var go in objects)
            {
                var rdr = go.GetComponent<MeshRenderer>();
                if (rdr != null)
                {
                    var me = rdr.mesh;
                    var shader = rdr.material.shader;
                    if (shader == null)
                        shader = Shader.fallback;

                    if ( me != null && shader != null )
                    {
                        Shader.active = shader;
                        shader.Link();
                        shader.Use();
                        
                        SetObjectMatrices(go);
                        SetShaderUniforms();
                        rdr.material.SetShaderUniforms();

                        foreach (Light light in Light.instances)
                        {
                            light.SetShaderUniforms();
                            me.Draw();
                        }
                    } 
                }// if ( rdr != null )
            }//foreach (var go in objects)
        }

        private static void SetObjectMatrices(GameObject go)
        {
            modelMatrix = go.transform.matrix;
            modelViewMatrix = viewMatrix * modelMatrix;
            modelViewProjectionMatrix = projectionMatrix * modelViewMatrix;
        }

        private static void SetShaderUniforms()
        {
            Shader shader = Shader.active;
            shader.SetUniform("_Time", Time.t);
            shader.SetUniform("viewMatrix", viewMatrix);
            shader.SetUniform("projectionMatrix", projectionMatrix);
            shader.SetUniform("viewProjectionMatrix", viewProjectionMatrix);
            shader.SetUniform("modelMatrix", modelMatrix);
            shader.SetUniform("modelViewMatrix", modelViewMatrix);
            shader.SetUniform("normalMatrix", modelViewMatrix.sub3);
            shader.SetUniform("modelViewProjectionMatrix", modelViewProjectionMatrix);
        }

        internal static void RenderActiveCameraDeferred()
        {
            ApplyCamera();
            GameObject[] objects = GameObject.instances;
            foreach (var go in objects)
            {
                var rdr = go.GetComponent<MeshRenderer>();
                if (rdr != null)
                {
                    var me = rdr.mesh;
                    var shader = rdr.material.shader;
                    if (shader == null)
                        shader = Shader.fallback;

                    if (me != null && shader != null)
                    {
                        Shader.active = shader;
                        shader.Link();
                        shader.Use();

                        SetObjectMatrices(go);
                        SetShaderUniforms();
                        rdr.material.SetShaderUniforms();

                        foreach (var light in Light.instances)
                        {
                            light.SetShaderUniforms();
                            me.Draw();
                        }
                    }
                }// if ( rdr != null )
            }//foreach (var go in objects)
        }

        public static void Blit(Texture2D texture, FrameBufferObject fbo)
        {
            throw new NotImplementedException();
        }

    }

    public static class Screen
    {
        public static int width;
        public static int height;
    }

    public enum RenderpathType
    {
        Forward,
        Deferred
    };
}

