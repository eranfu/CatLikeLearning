using UnityEngine;

namespace L3FramesPerSecond
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private int _frameRange = 60;
        public int AverageFps { get; private set; }
        public int HighestFps { get; private set; }
        public int LowestFps { get; private set; }

        private int[] _fpsBuffer;
        private int _fpsBufferIndex;

        private void Update()
        {
            if (_fpsBuffer == null || _fpsBuffer.Length != _frameRange)
                InitializeBuffer();
            UpdateBuffer();
            CalculateFps();
        }

        private void CalculateFps()
        {
            var sum = 0;
            LowestFps = int.MaxValue;
            HighestFps = 0;
            foreach (int fps in _fpsBuffer)
            {
                sum += fps;
                if (fps < LowestFps)
                    LowestFps = fps;
                else if (fps > HighestFps)
                    HighestFps = fps;
            }
            AverageFps = sum / _fpsBuffer.Length;
        }

        private void UpdateBuffer()
        {
            _fpsBuffer[_fpsBufferIndex] = (int) (1f / Time.unscaledDeltaTime);
            if (++_fpsBufferIndex >= _fpsBuffer.Length)
                _fpsBufferIndex = 0;
        }

        private void InitializeBuffer()
        {
            if (_frameRange < 1)
                _frameRange = 1;
            _fpsBuffer = new int[_frameRange];
            _fpsBufferIndex = 0;
        }
    }
}