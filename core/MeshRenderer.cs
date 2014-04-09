#pragma  warning disable 1591

using System;
namespace pxl
{
	public class MeshRenderer : Component
	{
        public Material material = null;
        public Mesh mesh = null;

        // Skeletal animation
        public Armature armature;
        public byte[] boneIndices = null;
        public byte[] boneWeights = null;

        public bool isSkinnedMesh
        {
            get
            {
                return armature != null && boneIndices != null && boneWeights != null;
            }
        }
	}
}
