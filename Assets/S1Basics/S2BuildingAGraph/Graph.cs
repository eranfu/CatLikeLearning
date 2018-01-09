using UnityEngine;

namespace S1Basics.S2BuildingAGraph
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private Transform pointPrefab;
        [Range(10, 100)] [SerializeField] private int resolution = 10;

        private Transform[] points;

        private void Awake()
        {
            float step = 2f / resolution;
            Vector3 scale = Vector3.one * step;
            Vector3 position;
            position.z = 0;
            position.y = 0;
            points = new Transform[resolution];
            for (var i = 0; i < resolution; i++)
            {
                Transform point = Instantiate(pointPrefab);
                position.x = (i + 0.5f) * step - 1;
                point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform, false);
                points[i] = point;
            }
        }

        private void Update()
        {
            foreach (Transform point in points)
            {
                Vector3 position = point.localPosition;
                position.y = Mathf.Pow(position.x + 1, 2.2f) * 4.5947938f;
                point.localPosition = position;
            }
        }
    }
}