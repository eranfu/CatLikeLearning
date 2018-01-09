using System;
using UnityEngine;

namespace S1Basics.S6CurvesAndSplines.Scripts
{
    public class SplineWalker : MonoBehaviour
    {
        private enum SplineWalkMode
        {
            Once,
            Loop,
            PingPong
        }

        [SerializeField] private BezierSpline spline;
        [SerializeField] private SplineWalkMode walkMode;
        [SerializeField] private float duration;
        [SerializeField] private bool lookForward;

        private float progress;
        private bool goingForward = true;

        private void Update()
        {
            if (goingForward)
            {
                progress += Time.deltaTime / duration;
                if (progress > 1)
                {
                    switch (walkMode)
                    {
                        case SplineWalkMode.Once:
                            progress = 1;
                            break;
                        case SplineWalkMode.Loop:
                            progress -= 1;
                            break;
                        case SplineWalkMode.PingPong:
                            goingForward = false;
                            progress = 2 - progress;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            else
            {
                progress -= Time.deltaTime / duration;
                if (progress < 0)
                {
                    goingForward = true;
                    progress = -progress;
                }
            }
            Vector3 position = spline.GetPoint(progress);
            transform.position = position;
            if (lookForward)
                transform.LookAt(position + (goingForward
                                     ? spline.GetDirection(progress)
                                     : -spline.GetDirection(progress)));
        }
    }
}