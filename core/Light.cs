#pragma  warning disable 1591 // doc

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
namespace pxl
{
    public class Light : Component, IDisposable
    {
        static private List<Light> m_instances = new List<Light>();

        new static public Light[] instances { get { return m_instances.ToArray(); } }

        public Light()
        {
            m_instances.Add(this);
        }

        override public void Dispose()
        {
            base.Dispose();
            m_instances.Remove(this);
        }

        internal void SetShaderUniforms()
        {
            Shader sh = Shader.active;
            if (sh != null)
            {
                Vector4 position = new Vector4();
                
                position.xyz = gameObject.transform.position;
                position.w = (this.type == Type.Sun) ? 0.0f : 1.0f;
                
                sh.SetUniform("Light.position", position );
                sh.SetUniform("Light.color", color);
                sh.SetUniform("Light.attenuation", attenuation);
                sh.SetUniform("Light.cutoff", cutoff);
                sh.SetUniform("Light.exponent", exponent);
                sh.SetUniform("Light.intensity", intensity);
            }

        }

        public Type type;
        public Color4 color;
        public float intensity;
        public float cutoff;
        public float exponent;
        public Vector4 attenuation;

        public enum Type
        {
            Point,
            Spot,
            Sun
        };
    }
}
