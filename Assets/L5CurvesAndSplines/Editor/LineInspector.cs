using L5CurvesAndSplines.Scripts;
using UnityEditor;
using UnityEngine;

namespace L5CurvesAndSplines.Editor
{
    [CustomEditor(typeof(Line))]
    public class LineInspector : UnityEditor.Editor
    {
        private Line _line;
        private Transform _handleTransform;
        private Quaternion _handleRotation;

        private void Awake()
        {
            _line = target as Line;
            if (_line == null) return;
            _handleTransform = _line.transform;
        }

        private void OnSceneGUI()
        {
            _handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;

            Vector3 p0 = ShowPoint(ref _line.p0);
            Vector3 p1 = ShowPoint(ref _line.p1);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
        }


        private Vector3 ShowPoint(ref Vector3 p)
        {
            Vector3 point = _handleTransform.TransformPoint(p);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (!EditorGUI.EndChangeCheck()) return point;
            Undo.RecordObject(_line, "Move Point");
            EditorUtility.SetDirty(_line);
            p = _handleTransform.InverseTransformPoint(point);
            return point;
        }
    }
}