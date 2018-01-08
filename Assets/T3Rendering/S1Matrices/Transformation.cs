using UnityEngine;

namespace T3Rendering.S1Matrices
{
    public abstract class Transformation : MonoBehaviour
    {
        public abstract Matrix4x4 Matrix { get; }
    }
}