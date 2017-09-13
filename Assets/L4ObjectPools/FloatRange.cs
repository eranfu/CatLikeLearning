using System;
using Random = UnityEngine.Random;

namespace L4ObjectPools
{
    [Serializable]
    public struct FloatRange
    {
        public float Min, Max;
        public float RandomInRange => Random.Range(Min, Max);
    }
}