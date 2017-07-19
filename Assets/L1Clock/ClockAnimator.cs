using System;
using UnityEngine;

namespace L1Clock
{
    public class ClockAnimator : MonoBehaviour
    {
        private const float
            HoursToDegrees = -360f / 12f,
            MinutesToDegrees = -360f / 60f,
            SecondsToDegrees = -360f / 60f;

        public Transform Hours, Minutes, Seconds;
        public bool Analog;

        private void Update()
        {
            if (Analog)
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                Hours.localRotation = Quaternion.Euler(0, 0, (float) time.TotalHours * HoursToDegrees);
                Minutes.localRotation = Quaternion.Euler(0, 0, (float) (time.TotalMinutes * MinutesToDegrees));
                Seconds.localRotation = Quaternion.Euler(0, 0, (float) (time.TotalSeconds * SecondsToDegrees));
            }
            else
            {
                DateTime time = DateTime.Now;
                Hours.localRotation = Quaternion.Euler(0, 0, time.Hour * HoursToDegrees);
                Minutes.localRotation = Quaternion.Euler(0, 0, time.Minute * MinutesToDegrees);
                Seconds.localRotation = Quaternion.Euler(0, 0, time.Second * SecondsToDegrees);
            }
        }
    }
}