using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace MadPot
{
    public enum ProductType
    {
        Inedible,
        Edible,
        FruitsAndVegetables,
        FastFood,
        Meat,
        Bakery
    }

    [RequireComponent(typeof(Outline))]
    public class Product : MonoBehaviour
    {
        private const float RotationSpeed = 180;
        private const float AccelerationSpeed = 2;
        private const float RotationTimer = 2;

        [SerializeField] private ProductType _type = ProductType.Inedible;
        [SerializeField] private Vector3 _minRotation = new Vector3(-180, -180, -180);
        [SerializeField] private Vector3 _maxRotation = new Vector3(180, 180, 180);

        public ProductType Type => _type;
        public Vector3 MinRotation => _minRotation;
        public Vector3 MaxRotation => _maxRotation;

        private Outline _outline = null;
        private Vector3 _targetRotation = Vector3.zero;
        private Vector3 _rotation = Vector3.zero;
        private float _currentTime = 0;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _targetRotation = URandom.onUnitSphere;
            _rotation = URandom.onUnitSphere;

            HideOutline();
        }

        private void Update()
        {
            _currentTime += Time.unscaledDeltaTime;
            if (_currentTime >= RotationTimer)
            {
                _currentTime = 0;
                _targetRotation = URandom.onUnitSphere;
            }

            //transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, _rotationSpeed * Time.deltaTime);
            _rotation = Vector3.Slerp(_rotation, _targetRotation, AccelerationSpeed * Time.unscaledDeltaTime);
            transform.Rotate(_rotation * RotationSpeed * Time.unscaledDeltaTime);
        }

        public void SetOutlineColor(Color color)
        {
            _outline.OutlineColor = color;
        }

        public void ShowOutline()
        {
            _outline.OutlineWidth = 10;
        }

        public void HideOutline()
        {
            _outline.OutlineWidth = 0;
        }
    }
}
