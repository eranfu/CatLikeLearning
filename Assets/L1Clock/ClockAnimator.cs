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

        public Transform Hours, Minutes, Seconds;
        public bool Analog;

        private void Update()
        {
            if (Analog)
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                Hours.localRotation = Quaternion.Euler(0, 0, (float) time.TotalHours * hoursToDegrees);
                Minutes.localRotation = Quaternion.Euler(0, 0, (float) (time.TotalMinutes * minutesToDegrees));
                Seconds.localRotation = Quaternion.Euler(0, 0, (float) (time.TotalSeconds * secondsToDegrees));
            }
            else
            {
                DateTime time = DateTime.Now;
                Hours.localRotation = Quaternion.Euler(0, 0, time.Hour * hoursToDegrees);
                Minutes.localRotation = Quaternion.Euler(0, 0, time.Minute * minutesToDegrees);
                Seconds.localRotation = Quaternion.Euler(0, 0, time.Second * secondsToDegrees);
            }
        }
    }
}