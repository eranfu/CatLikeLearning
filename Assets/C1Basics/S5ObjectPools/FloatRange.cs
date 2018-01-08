using System;
using Random = UnityEngine.Random;

namespace C1Basics.S5ObjectPools
{
    [Serializable]
    public struct FloatRange
    {
        public float Min, Max;
        public float RandomInRange => Random.Range(Min, Max);
    }
}