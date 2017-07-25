using L5CurvesAndSplines.Scripts;
using UnityEditor;
using UnityEngine;

namespace L5CurvesAndSplines.Editor
{
    [CustomEditor(typeof(Line))]
    public class LineInspector : UnityEditor.Editor
    {
        private Line line;
        private Transform handleTransform;
        private Quaternion handleRotation;

        private void Awake()
        {
            line = target as Line;
            if (line == null) return;
            handleTransform = line.transform;
        }

        private void OnSceneGUI()
        {
            handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? handleTransform.rotation
                : Quaternion.identity;

            Vector3 p0 = ShowPoint(ref line.p0);
            Vector3 p1 = ShowPoint(ref line.p1);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
        }


        private Vector3 ShowPoint(ref Vector3 p)
        {
            Vector3 point = handleTransform.TransformPoint(p);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (!EditorGUI.EndChangeCheck()) return point;
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            p = handleTransform.InverseTransformPoint(point);
            return point;
        }
    }
}