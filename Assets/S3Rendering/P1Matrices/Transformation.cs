using UnityEngine;

namespace S3Rendering.P1Matrices
{
    public abstract class Transformation : MonoBehaviour
    {
        public abstract Matrix4x4 Matrix { get; }
    }
}