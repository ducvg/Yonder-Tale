
using System;
using UnityEngine;

namespace Yonder.Appearance
{
    public class MeshAppearanceStrategy : BaseAppearanceStrategy
    {
        public Mesh mesh;
        public Material material;

        public override void Change(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            skinnedMeshRenderer.sharedMesh = mesh;

        }
    }
}
