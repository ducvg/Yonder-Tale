using UnityEngine;

namespace Yonder.Appearance
{
    public class ObjectAppearanceStrategy : BaseAppearanceStrategy
    {
        public GameObject equipmentPrefab;

        public override void Change(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            Instantiate(equipmentPrefab, Vector3.zero, Quaternion.identity, skinnedMeshRenderer.gameObject.transform);
        }
    }
}
