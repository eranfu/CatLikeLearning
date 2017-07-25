using UnityEngine;

namespace L3FramesPerSecond
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private int frameRange = 60;
        public int AverageFps { get; private set; }
        public int HighestFps { get; private set; }
        public int LowestFps { get; private set; }

        private int[] fpsBuffer;
        private int fpsBufferIndex;

        private void Update()
        {
            if (fpsBuffer == null || fpsBuffer.Length != frameRange)
                InitializeBuffer();
            UpdateBuffer();
            CalculateFps();
        }

        private void CalculateFps()
        {
            var sum = 0;
            LowestFps = int.MaxValue;
            HighestFps = 0;
            foreach (int fps in fpsBuffer)
            {
                sum += fps;
                if (fps < LowestFps)
                    LowestFps = fps;
                else if (fps > HighestFps)
                    HighestFps = fps;
            }
            AverageFps = sum / fpsBuffer.Length;
        }

        private void UpdateBuffer()
        {
            fpsBuffer[fpsBufferIndex] = (int) (1f / Time.unscaledDeltaTime);
            if (++fpsBufferIndex >= fpsBuffer.Length)
                fpsBufferIndex = 0;
        }

        private void InitializeBuffer()
        {
            if (frameRange < 1)
                frameRange = 1;
            fpsBuffer = new int[frameRange];
            fpsBufferIndex = 0;
        }
    }
}