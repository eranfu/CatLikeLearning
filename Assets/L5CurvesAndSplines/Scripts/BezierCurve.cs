using UnityEngine;

namespace L5CurvesAndSplines.Scripts
{
    public class BezierCurve : MonoBehaviour
    {
        public Vector3[] points;

        public void Reset()
        {
            points = new[]
            {
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0),
                new Vector3(4, 0, 0),
            };
        }

        public Vector3 GetPoint(float t)
        {
            return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetVelocity(float t)
        {
            return transform.TransformVector(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t));
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }
    }
}