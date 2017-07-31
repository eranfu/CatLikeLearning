using L5CurvesAndSplines.Scripts;
using UnityEditor;
using UnityEngine;

namespace L5CurvesAndSplines.Editor
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspctor : UnityEditor.Editor
    {
        private BezierSpline spline;
        private Transform handleTransform;
        private Quaternion handleRatation;
        private int lineSteps;
        private float directionScale;

        private const float handleSize = 0.04f;
        private const float pickSize = 0.06f;
        private int selectedIndex;

        private static Color[] modeColors = new[]
        {
            Color.white,
            Color.yellow,
            Color.cyan
        };

        private void OnEnable()
        {
            spline = target as BezierSpline;
            if (spline == null) return;
            handleTransform = spline.transform;
            lineSteps = 10;
            directionScale = 0.5f;
            selectedIndex = -1;
        }

        private void OnSceneGUI()
        {
            handleRatation = Tools.pivotRotation == PivotRotation.Local
                ? handleTransform.rotation
                : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            for (var i = 1; i < spline.ControlPointCount; i += 3)
            {
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i + 1);
                Vector4 p3 = ShowPoint(i + 2);

                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2);

                p0 = p3;
            }

            ShowDirections();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Toggle Loop");
                spline.Loop = loop;
                EditorUtility.SetDirty(spline);
            }

            ZeroPoint();
            CancelSelected();

            if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }

            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(spline, "Add Curve");
                EditorUtility.SetDirty(spline);
                spline.AddCurve();
            }
        }

        private void CancelSelected()
        {
            if (!GUILayout.Button("Cancel Selected")) return;
            selectedIndex = -1;
            EditorUtility.SetDirty(spline);
        }

        private void ZeroPoint()
        {
            if (!GUILayout.Button("Rise Point At Zero")) return;
            for (var i = 0; i < spline.ControlPointCount; ++i)
            {
                if (spline.GetControlPoint(i) == Vector3.zero)
                {
                    Undo.RecordObject(spline, "Rise Point At Zero");
                    spline.SetControlPoint(i, new Vector3(0, 1, 0));
                    EditorUtility.SetDirty(spline);
                    return;
                }
            }
        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(selectedIndex, point);
            }

            EditorGUI.BeginChangeCheck();
            var mode = (BezierControlPointMode) EditorGUILayout.EnumPopup("Mode",
                spline.GetControlPointMode(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Change Point Mode");
                spline.SetControlPointMode(selectedIndex, mode);
                EditorUtility.SetDirty(spline);
            }
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            int steps = lineSteps * spline.CurveCount;
            for (var i = 0; i < steps; ++i)
            {
                Vector3 point = spline.GetPoint(i / (float) steps);
                Handles.DrawLine(point, point + spline.GetDirection(i / (float) steps) * directionScale);
            }
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
            Handles.color = modeColors[(int) spline.GetControlPointMode(index)];
            float handleScale = HandleUtility.GetHandleSize(point);
            if (index == 0)
                handleScale *= 2;
            if (Handles.Button(point, handleRatation, handleScale * handleSize, handleScale * pickSize,
                Handles.DotHandleCap))
            {
                selectedIndex = index;
                Repaint();
            }

            if (selectedIndex != index)
                return point;
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRatation);
            if (!EditorGUI.EndChangeCheck())
                return point;
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            return point;
        }
    }
}