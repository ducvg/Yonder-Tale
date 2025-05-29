using System.Collections.Generic;
using UnityEngine;

namespace Yonder.Appearance
{
    public class AppearanceCustomizer : MonoBehaviour
    {
        public SkinnedMeshRenderer head, leftArm, rightArm, body, leftLeg, rightLeg, mainHand, offHand;
        public List<BaseAppearanceStrategy> appearanceStrategies;
        
    }
}
