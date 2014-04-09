using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{
    public class Armature : Component
    {

        public Bone[] bones
        {
            get
            {
                return m_bones.ToArray();
            }

            set
            {
                m_bones.Clear();
                m_bones.AddRange(value);
                
                // find symetric syblings
                m_mirrors = new Bone[m_bones.Count];
                for (int i = 0; i < m_bones.Count; ++i)
                {
                    string mirrorName = getMirrorName(m_bones[i].name);
                    m_mirrors[i] = FindBoneWithName( mirrorName );
                }

                m_poseMatrices = null;
                if (m_bones.Count > 0)
                {
                    m_poseMatrices = new Matrix4[m_bones.Count];
                }
            }
        }

        public Matrix4[] poseMatrices
        {
            get
            {
                for (int i = 0; i < m_bones.Count; ++i)
                {
                    m_poseMatrices[i] = m_bones[i].pose;
                }
                return m_poseMatrices;
            }
        }

        public Bone FindBoneWithName(string name)
        {
            Bone bone = null;
            foreach (Bone b in m_bones)
            {
                if (b.name == name)
                {
                    bone = b;
                    break;
                }
            }
            return bone;
        }


        private List<Bone> m_bones = new List<Bone>();
        private Bone[] m_mirrors;
        private Matrix4[] m_poseMatrices;

        private string getMirrorName(string name)
        {
            string mirrorName = null;

            if (name.Length > 1)
            {
                string nameWithoutSide = name.Substring(0, name.Length - 2);
                string side = name.Substring(name.Length - 1 - 2, 2);

                switch (side)
                {
                    case ".R": mirrorName = nameWithoutSide + ".L"; break;
                    case ".L": mirrorName = nameWithoutSide + ".R"; break;
                    case ".r": mirrorName = nameWithoutSide + ".l"; break;
                    case ".l": mirrorName = nameWithoutSide + ".r"; break;
                }

            }
            return mirrorName;
        }

    }
}
