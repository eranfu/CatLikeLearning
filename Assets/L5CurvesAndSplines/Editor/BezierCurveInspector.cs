using L5CurvesAndSplines.Scripts;
using UnityEditor;
using UnityEngine;

namespace L5CurvesAndSplines.Editor
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveInspector : UnityEditor.Editor
    {
        private BezierCurve _curve;
        private Transform _handleTransform;
        private Quaternion _handleRatation;
        private int _lineSteps;
        private float _directionScale;

        private void OnSceneGUI()
        {
            _handleRatation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);
            Vector4 p3 = ShowPoint(3);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2);
            ShowDirections();
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            for (var i = 0; i < _lineSteps; ++i)
            {
                Vector3 point = _curve.GetPoint(i / (float) _lineSteps);
                Handles.DrawLine(point, point + _curve.GetDirection(i / (float) _lineSteps) * _directionScale);
            }
        }

        private void Awake()
        {
            _curve = target as BezierCurve;
            if (_curve == null) return;
            _handleTransform = _curve.transform;
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = _handleTransform.TransformPoint(_curve.points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRatation);
            if (!EditorGUI.EndChangeCheck()) return point;
            Undo.RecordObject(_curve, "Move Point");
            EditorUtility.SetDirty(_curve);
            _curve.points[index] = _handleTransform.InverseTransformPoint(point);
            return point;
        }

        private void OnEnable()
        {
            _lineSteps = 10;
            _directionScale = 0.5f;
        }
    }
}