using UnityEngine;

namespace S3Rendering.P1Matrices
{
    internal class RotationTransformation : Transformation
    {
        [SerializeField] private Vector3 rotation = Vector3.zero;

        public override Matrix4x4 Matrix
        {
            get
            {
                float radX = rotation.x * Mathf.Deg2Rad;
                float radY = rotation.y * Mathf.Deg2Rad;
                float radZ = rotation.z * Mathf.Deg2Rad;
                float sinX = Mathf.Sin(radX);
                float cosX = Mathf.Cos(radX);
                float sinY = Mathf.Sin(radY);
                float cosY = Mathf.Cos(radY);
                float sinZ = Mathf.Sin(radZ);
                float cosZ = Mathf.Cos(radZ);

                var m = new Matrix4x4();
                m.SetColumn(0, new Vector4(
                    cosY * cosZ,
                    cosX * sinZ + sinX * sinY * cosZ,
                    sinX * sinZ - cosX * sinY * cosZ,
                    0f
                ));
                m.SetColumn(1, new Vector4(
                    -cosY * sinZ,
                    cosX * cosZ - sinX * sinY * sinZ,
                    sinX * cosZ + cosX * sinY * sinZ,
                    0f
                ));
                m.SetColumn(2, new Vector4(
                    sinY,
                    -sinX * cosY,
                    cosX * cosY,
                    0f
                ));
                m.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
                return m;
            }
        }
    }
}