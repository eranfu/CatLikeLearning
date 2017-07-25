using System;
using UnityEngine;

namespace L1Clock
{
    public class ClockAnimator : MonoBehaviour
    {
        private const float
            hoursToDegrees = -360f / 12f,
            minutesToDegrees = -360f / 60f,
            secondsToDegrees = -360f / 60f;

        public Transform hours, minutes, seconds;
        public bool analog;

        private void Update()
        {
            if (analog)
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                hours.localRotation = Quaternion.Euler(0, 0, (float) time.TotalHours * hoursToDegrees);
                minutes.localRotation = Quaternion.Euler(0, 0, (float) (time.TotalMinutes * minutesToDegrees));
                seconds.localRotation = Quaternion.Euler(0, 0, (float) (time.TotalSeconds * secondsToDegrees));
            }
            else
            {
                DateTime time = DateTime.Now;
                hours.localRotation = Quaternion.Euler(0, 0, time.Hour * hoursToDegrees);
                minutes.localRotation = Quaternion.Euler(0, 0, time.Minute * minutesToDegrees);
                seconds.localRotation = Quaternion.Euler(0, 0, time.Second * secondsToDegrees);
            }
        }
    }
}