using UnityEngine;

namespace C1Basics.S6CurvesAndSplines.Scripts
{
    public class BezierCurve : MonoBehaviour
    {
        public Vector3[] Points;

        public void Reset()
        {
            Points = new[]
            {
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0),
                new Vector3(4, 0, 0),
            };
        }

        public Vector3 GetPoint(float t)
        {
            return transform.TransformPoint(Bezier.GetPoint(Points[0], Points[1], Points[2], Points[3], t));
        }

        public Vector3 GetVelocity(float t)
        {
            return transform.TransformVector(Bezier.GetFirstDerivative(Points[0], Points[1], Points[2], Points[3], t));
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }
    }
}