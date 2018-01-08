using UnityEngine;

namespace C1Basics.S6CurvesAndSplines.Scripts
{
    public static class Bezier
    {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1 - t;
            return oneMinusT * oneMinusT * p0 + 2 * oneMinusT * t * p1 + t * t * p2;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
        }

        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1 - t;
            return oneMinusT * oneMinusT * oneMinusT * p0 +
                   3 * oneMinusT * oneMinusT * t * p1 +
                   3 * oneMinusT * t * t * p2 +
                   t * t * t * p3;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float oneMinusT = 1 - t;
            return 3 * oneMinusT * oneMinusT * (p1 - p0) +
                   6 * oneMinusT * t * (p2 - p1) +
                   3 * t * t * (p3 - p2);
        }
    }
}