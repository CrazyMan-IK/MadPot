using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    [RequireComponent(typeof(RectTransform))]
    public class TutorialViewer : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _minScale = 1.0f;
        [SerializeField] private float _maxScale = 1.2f;

        private RectTransform _rectTransform = null;

        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var current = Mathf.PingPong(_speed * Time.unscaledTime, _maxScale - _minScale) + _minScale;
            transform.localScale = new Vector3(current, current, current);
        }
    }
}
