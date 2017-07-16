using UnityEngine;

namespace L3FramesPerSecond
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private int _frameRange = 60;
        public int AverageFPS { get; private set; }
        public int HighestFPS { get; private set; }
        public int LowestFPS { get; private set; }

        private int[] _fpsBuffer;
        private int _fpsBufferIndex;

        private void Update()
        {
            if (_fpsBuffer == null || _fpsBuffer.Length != _frameRange)
                InitializeBuffer();
            UpdateBuffer();
            CalculateFPS();
        }

        private void CalculateFPS()
        {
            var sum = 0;
            LowestFPS = int.MaxValue;
            HighestFPS = 0;
            foreach (int fps in _fpsBuffer)
            {
                sum += fps;
                if (fps < LowestFPS)
                    LowestFPS = fps;
                else if (fps > HighestFPS)
                    HighestFPS = fps;
            }
            AverageFPS = sum / _fpsBuffer.Length;
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