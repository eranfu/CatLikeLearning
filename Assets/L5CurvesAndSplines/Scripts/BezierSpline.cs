using System;
using UnityEngine;

namespace L5CurvesAndSplines.Scripts
{
    public class BezierSpline : MonoBehaviour
    {
        [SerializeField] private Vector3[] points;

        public int CurveCount => (points.Length - 1) / 3;
        public int ControlPointCount => points.Length;

        public void Reset()
        {
            points = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0),
            };
        }

        public Vector3 GetControlPoint(int index)
        {
            return points[index];
        }

        public void SetControlPoint(int index, Vector3 point)
        {
            points[index] = point;
        }

        public Vector3 GetPoint(float t)
        {
            int i;
            if (t >= 1)
            {
                t = 1;
                i = points.Length - 4;
            }
            else
            {
                if (t < 0) t = 0;
                t *= CurveCount;
                i = (int) t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
        }

        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1)
            {
                t = 1;
                i = points.Length - 4;
            }
            else
            {
                if (t < 0) t = 0;
                t *= CurveCount;
                i = (int) t;
                t -= i;
                i *= 3;
            }
            return transform.TransformVector(
                Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t));
        }

        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public void AddCurve()
        {
            Vector3 point = points[points.Length - 1];
            Array.Resize(ref points, points.Length + 3);
            point.x += 1f;
            points[points.Length - 3] = point;
            point.x += 1f;
            points[points.Length - 2] = point;
            point.x += 1f;
            points[points.Length - 1] = point;
        }
    }
}