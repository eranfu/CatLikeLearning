using UnityEngine;

namespace L5CurvesAndSplines.Scripts
{
    public class Line : MonoBehaviour
    {
        public Vector3 p0, p1;

        private void Reset()
        {
            p0 = new Vector3(1, 1, 1);
            p1 = new Vector3(2, 2, 2);
        }
    }
}