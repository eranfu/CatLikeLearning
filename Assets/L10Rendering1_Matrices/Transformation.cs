using UnityEngine;

namespace L10Rendering1
{
    public abstract class Transformation : MonoBehaviour
    {
        public abstract Matrix4x4 Matrix { get; }
    }
}