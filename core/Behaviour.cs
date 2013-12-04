#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{

    public class Behaviour : Component, IDisposable
    {
        static private List<Behaviour> m_instances = new List<Behaviour>();

        static new public List<Behaviour> instances
        {
            get
            {
                return m_instances;
            }
        }

        public Behaviour()
        {
            m_instances.Add( this );
        }

        override public void Dispose()
        {
            m_instances.Remove(this);
        }

        internal override void InternalStart()
        {
            base.InternalStart();
            Start();
        }

        internal override void InternalUpdate()
        {
            base.InternalUpdate();
            Update();
        }

        internal override void InternalFixedUpdate()
        {
            base.InternalFixedUpdate();
            FixedUpdate();
        }

        virtual public void Start()
        {

        }

        virtual public void Update()
        {
        
        }

        virtual public void FixedUpdate()
        {

        }

    }
}
