using L5CurvesAndSplines.Scripts;
using UnityEditor;
using UnityEngine;

namespace L5CurvesAndSplines.Editor
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveInspector : UnityEditor.Editor
    {
        private BezierCurve curve;
        private Transform handleTransform;
        private Quaternion handleRatation;
        private int lineSteps;
        private float directionScale;
        
        private void Awake()
        {
            curve = target as BezierCurve;
            if (curve == null) return;
            handleTransform = curve.transform;
        }

        private void OnEnable()
        {
            lineSteps = 10;
            directionScale = 0.5f;
        }

        private void OnSceneGUI()
        {
            handleRatation = Tools.pivotRotation == PivotRotation.Local
                ? handleTransform.rotation
                : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);
            Vector4 p3 = ShowPoint(3);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2);
            ShowDirections();
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            for (var i = 0; i < lineSteps; ++i)
            {
                Vector3 point = curve.GetPoint(i / (float) lineSteps);
                Handles.DrawLine(point, point + curve.GetDirection(i / (float) lineSteps) * directionScale);
            }
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(curve.points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRatation);
            if (!EditorGUI.EndChangeCheck()) return point;
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
            return point;
        }
    }
}