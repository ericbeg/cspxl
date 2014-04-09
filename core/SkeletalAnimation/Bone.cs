using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    /**
     * In Skeletal Animation, a bone is an element of an armature.
     *   
     * Matrices definition:
     * 
     *  - Pose:         Transforms from [rest model space] to [pose model space].
     *  - PoseLocal:    Transforms from [rest bone space] to [pose bone space].
     *  
     *  - Rest:         Transform from [rest bone space] to [rest model space].
     *  - RestLocal:    [rest bone base]
     */
    public class Bone : Component
    {

        private bool m_isDirtyPose;
        private bool m_isDirtyPoseLocal;

        private bool m_isDirtyRest;
        private bool m_isDirtyRestInv;
        private bool m_isDirtyRestLocal;


        public string name = "Bone";
        public Matrix4 m_pose;
        public Matrix4 m_poseLocal;

        public Matrix4 m_rest;
        public Matrix4 m_restInv;
        public Matrix4 m_restLocal;


        public bool isConnected{get; set;}

        public Matrix4 pose
        {
            get
            {
                if (m_isDirtyPose)
                {
                    m_pose = gameObject.transform.matrix * restInv;
                    m_isDirtyPose = false;
                }
                return m_pose;
            }
        }

        public Matrix4 poseLocal
        {
            get
            {
                if (m_isDirtyPoseLocal)
                {
                    m_poseLocal = gameObject.transform.localMatrix * restInv;
                    m_isDirtyPoseLocal = false;
                }
                return m_poseLocal;
            }
        }

        public Matrix4 rest
        {
            get
            {
                return m_rest;
            }
        }

        public Matrix4 restInv
        {
            get
            {
                if (m_isDirtyRestInv)
                {
                    m_restInv = Matrix4.Inverse(rest);
                    m_isDirtyRestInv = false;
                }
                return m_restInv;
            }

        }

        public Matrix4 restLocal
        {
            get
            {
                return m_restLocal;
            }
        }

        /// <summary>
        /// Connect this bone to its parent if isConnnected is true.
        /// </summary>
        public void Connect()
        {
            /*
             * If the bone is connected, only rotation affects the bone.
             * ;the translation relative to its parent does not change.
             */
            Transform transform = gameObject.transform;
            if (isConnected && transform.parent != null)
            {
                Vector3 localPos;
                Matrix4 r = restLocal;
                
                localPos.x = r.m14;
                localPos.y = r.m24;
                localPos.z = r.m34;

                transform.localPosition = localPos;
            }
        }

        public void Bind()
        {
            Transform transform = gameObject.transform;
            m_rest = transform.matrix;
            m_restLocal = transform.localMatrix;

            m_isDirtyRest = m_isDirtyRestInv
            = m_isDirtyRestLocal
            = m_isDirtyPose = m_isDirtyPoseLocal
            = true ;
        }

    }
}
