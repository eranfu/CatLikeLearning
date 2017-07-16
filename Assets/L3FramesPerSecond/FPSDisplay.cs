using System;
using UnityEngine;
using UnityEngine.UI;

namespace L3FramesPerSecond
{
    [RequireComponent(typeof(FPSCounter))]
    public class FPSDisplay : MonoBehaviour
    {
        [Serializable]
        private struct FPSColor
        {
            public Color color;
            public int minimumFPS;
        }

        [SerializeField] private Text _highestFPSLabel, _averageFPSLabel, _lowestFPSLabel;
        [SerializeField] private FPSColor[] _coloring;
        private FPSCounter _fpsCounter;

        private static readonly string[] stringsFrom0To99 =
        {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "02", "03", "04", "05", "06", "07", "08", "09",
            "20", "21", "02", "03", "04", "05", "06", "07", "08", "09",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
        };

        private void Awake()
        {
            _fpsCounter = GetComponent<FPSCounter>();
        }

        private void Update()
        {
            Display(_highestFPSLabel, _fpsCounter.HighestFPS);
            Display(_averageFPSLabel, _fpsCounter.AverageFPS);
            Display(_lowestFPSLabel, _fpsCounter.LowestFPS);
        }

        private void Display(Text label, int fps)
        {
            label.text = stringsFrom0To99[Mathf.Clamp(fps, 0, 99)];
            for (var i = 0; i < _coloring.Length; i++)
            {
                if (_coloring[i].minimumFPS > fps) continue;
                label.color = _coloring[i].color;
                break;
            }
        }
    }
}