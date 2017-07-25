using System;
using Random = UnityEngine.Random;

namespace L4ObjectPools
{
    [Serializable]
    public struct FloatRange
    {
        public float min, max;
        public float RandomInRange => Random.Range(min, max);
    }
}