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
        private List<Light> m_instances = new List<Light>();

        new public Light[] instances { get { return m_instances.ToArray(); } }

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
                sh.SetUniform("Lamp.position", gameObject.transform.position );
                sh.SetUniform("Lamp.color", color);
                sh.SetUniform("Lamp.attenuation", attenuation);
                sh.SetUniform("Lamp.cutoff", cutoff);
                sh.SetUniform("Lamp.exponent", exponent);
                sh.SetUniform("Lamp.intensity", intensity);
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
