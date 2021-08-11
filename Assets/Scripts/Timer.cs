using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MadPot
{
    public class Timer : MonoBehaviour
    {
        public event Action Completed = null;

        [SerializeField] private float _targetTime = 1;
        [SerializeField] private TextMeshProUGUI _previewText = null;

        private bool _isCompleted = false;
        private bool _isPaused = false;
        private float _currentTime = 0;

        private void Awake()
        {
            _isCompleted = true;
            _currentTime = _targetTime;
        }

        private void Update()
        {
            if (_isCompleted || _isPaused)
            {
                return;
            }

            _currentTime += Time.unscaledDeltaTime;

            _previewText.text = TimeSpan.FromSeconds(_targetTime - _currentTime).ToString("ss\\:ff");

            if (_currentTime >= _targetTime)
            {
                _isCompleted = true;

                Completed?.Invoke();
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Restart()
        {
            _isPaused = false;
            _isCompleted = false;

            _currentTime = 0;
        }
    }
}
