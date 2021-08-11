using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    public class TutorialHand : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;

        public Vector3 TargetPoint { get; set; } = Vector3.zero;

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.WorldToScreenPoint(TargetPoint), _speed * Time.unscaledDeltaTime);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = Camera.main.WorldToScreenPoint(position);
        }

        public Vector3 GetPosition()
        {
            return Camera.main.ScreenToWorldPoint(transform.position);
        }
    }
}
