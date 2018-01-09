using System;
using Random = UnityEngine.Random;

namespace S1Basics.S5ObjectPools
{
    [Serializable]
    public struct FloatRange
    {
        public float Min, Max;
        public float RandomInRange => Random.Range(Min, Max);
    }
}