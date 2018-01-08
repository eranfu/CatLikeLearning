using UnityEngine;

namespace T3Rendering.S1Matrices
{
    internal class PositionTransformation : Transformation
    {
        [SerializeField] private Vector3 position = Vector3.zero;

        public override Matrix4x4 Matrix
        {
            get
            {
                var matrix = new Matrix4x4();
                matrix.SetRow(0, new Vector4(1f, 0f, 0f, position.x));
                matrix.SetRow(1, new Vector4(0f, 1f, 0f, position.y));
                matrix.SetRow(2, new Vector4(0f, 0f, 1f, position.z));
                matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                return matrix;
            }
        }
    }
}