using UnityEngine;

namespace S1Basics.S6CurvesAndSplines.Scripts
{
    public class SplineDecorator : MonoBehaviour
    {
        [SerializeField] private BezierSpline spline;
        [SerializeField] private int frequency;
        [SerializeField] private bool lookForward;
        [SerializeField] private Transform[] items;

        private void Awake()
        {
            if (frequency <= 0 || items == null || items.Length == 0)
                return;
            float stepSize = frequency * items.Length;
            stepSize = spline.Loop ? 1 / stepSize : 1 / (stepSize - 1);
            for (int p = 0, f = 0; f < frequency; f++)
            {
                for (var i = 0; i < items.Length; i++, p++)
                {
                    Transform item = Instantiate(items[i]);
                    Vector3 position = spline.GetPoint(p * stepSize);
                    item.position = position;
                    if (lookForward)
                    {
                        item.LookAt(position + spline.GetDirection(p * stepSize));
                    }
                    item.parent = transform;
                }
            }
        }
    }
}