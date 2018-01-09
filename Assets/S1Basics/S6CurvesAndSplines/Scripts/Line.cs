using UnityEngine;

namespace S1Basics.S6CurvesAndSplines.Scripts
{
    public class Line : MonoBehaviour
    {
        public Vector3 P0, P1;

        private void Reset()
        {
            P0 = new Vector3(1, 1, 1);
            P1 = new Vector3(2, 2, 2);
        }
    }
}