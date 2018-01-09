using UnityEngine;

namespace S2MeshBasics.S3CubeSphere
{
    public class CircleGizmo : MonoBehaviour
    {
        [SerializeField] private int resolution = 10;

        private void OnDrawGizmosSelected()
        {
            float step = 2f / resolution;
            for (var i = 0; i <= resolution; i++)
            {
                ShowPoint(i * step - 1, -1);
                ShowPoint(i * step - 1, 1);
            }

            for (var i = 1; i < resolution; ++i)
            {
                ShowPoint(-1, i * step - 1);
                ShowPoint(1, i * step - 1);
            }
        }

        private void ShowPoint(float x, float y)
        {
            var square = new Vector2(x, y);
            Vector2 circle;
            circle.x = square.x * Mathf.Sqrt(1 - square.y * square.y * 0.5f);
            circle.y = square.y * Mathf.Sqrt(1 - square.x * square.x * 0.5f);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(square, 0.025f);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(circle, 0.025f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(circle, square);
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(circle, Vector3.zero);
        }
    }
}