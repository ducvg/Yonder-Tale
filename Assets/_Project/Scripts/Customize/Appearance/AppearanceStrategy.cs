
using System;
using UnityEngine;

namespace Yonder.Appearance
{
    public abstract class BaseAppearanceStrategy : ScriptableObject
    {
        public AppearanceType type;
        public abstract void Change(SkinnedMeshRenderer skinnedMeshRenderer);
    }

    [Serializable]
    public struct AppearanceData
    {
        public AppearanceType type;

    }

    [Serializable]
    public enum AppearanceType
    {
        Hat,        //object
        Head,       //mesh
        LeftArm,    //mesh
        RightArm,   //mesh
        Cape,       //object
        Body,       //mesh
        LeftLeg,    //mesh
        RightLeg,   //mesh

        Mainhand,   //object
        OffHand     //object
    }
}
