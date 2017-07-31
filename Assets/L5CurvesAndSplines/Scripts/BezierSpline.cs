using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace L5CurvesAndSplines.Scripts
{
    public class BezierSpline : MonoBehaviour
    {
        [SerializeField] private Vector3[] points;
        [SerializeField] private BezierControlPointMode[] modes;
        [FormerlySerializedAs("Loop")] [SerializeField] private bool loop;

        public int CurveCount => (points.Length - 1) / 3;
        public int ControlPointCount => points.Length;

        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;
                if (!value) return;
                modes[modes.Length - 1] = modes[0];
                points[points.Length - 1] = points[0];
                EnforceMode(0);
            }
        }

        public void Reset()
        {
            points = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0),
            };

            modes = new[]
            {
                BezierControlPointMode.Free,
                BezierControlPointMode.Free
            };
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            return modes[(index + 1) / 3];
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            int modeIndex = (index + 1) / 3;
            modes[modeIndex] = mode;
            if (loop)
            {
                if (modeIndex == 0)
                    modes[modes.Length - 1] = mode;
                else if (modeIndex == modes.Length - 1)
                    modes[0] = mode;
            }
            EnforceMode(index);
        }

        public Vector3 GetControlPoint(int index)
        {
            return points[index];
        }

        public void SetControlPoint(int index, Vector3 point)
        {
            if (index % 3 == 0)
            {
                Vector3 delta = point - points[index];
                if (loop)
                {
                    if (index == 0)
                    {
                        points[1] += delta;
                        points[points.Length - 2] += delta;
                        points[points.Length - 1] = point;
                    }
                    else if (index == points.Length - 1)
                    {
                        points[1] += delta;
                        points[points.Length - 2] += delta;
                        points[0] = point;
                    }
                    else
                    {
                        points[index - 1] += delta;
                        points[index + 1] += delta;
                    }
                }
                else
                {
                    if (index > 0)
                        points[index - 1] += delta;
                    if (index + 1 < points.Length)
                        points[index + 1] += delta;
                }
                points[index] = point;
                return;
            }
            points[index] = point;
            EnforceMode(index);
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

            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
            EnforceMode(points.Length - 4);
            if (loop)
            {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                EnforceMode(0);
            }
        }

        private void EnforceMode(int index)
        {
            int modeIndex = (index + 1) / 3;
            if (!loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) return;
            BezierControlPointMode mode = modes[modeIndex];
            if (mode == BezierControlPointMode.Free) return;

            int middleIndex = modeIndex * 3;
            int fixedIndex, enforcedIndex;
            if (index <= middleIndex)
            {
                fixedIndex = middleIndex - 1;
                if (fixedIndex < 0)
                    fixedIndex = points.Length - 2;
                enforcedIndex = middleIndex + 1;
                if (enforcedIndex >= points.Length)
                    enforcedIndex = 1;
            }
            else
            {
                fixedIndex = middleIndex + 1;
                if (fixedIndex >= points.Length)
                    fixedIndex = 1;
                enforcedIndex = middleIndex - 1;
                if (enforcedIndex < 0)
                    enforcedIndex = points.Length - 2;
            }

            Vector3 middle = points[middleIndex];
            Vector3 enforcedTangent = middle - points[fixedIndex];
            if (mode == BezierControlPointMode.Aligned)
                points[enforcedIndex] = middle + enforcedTangent.normalized *
                                        Vector3.Distance(middle, points[enforcedIndex]);
            else
                points[enforcedIndex] = middle + enforcedTangent;
        }
    }
}