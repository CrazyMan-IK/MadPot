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

    public class Product : MonoBehaviour
    {
        [SerializeField] private ProductType _type = ProductType.Inedible;
        [SerializeField] private Vector3 _minRotation = new Vector3(-180, -180, -180);
        [SerializeField] private Vector3 _maxRotation = new Vector3(180, 180, 180);

        public ProductType Type => _type;
        public Vector3 MinRotation => _minRotation;
        public Vector3 MaxRotation => _maxRotation;

        private const float _rotationSpeed = 180;
        private const float _accelerationSpeed = 2;
        private const float _rotationTimer = 2;
        private Vector3 _targetRotation = Vector3.zero;
        private Vector3 _rotation = Vector3.zero;
        private float _currentTime = 0;

        private void Awake()
        {
            _targetRotation = URandom.onUnitSphere;
            _rotation = URandom.onUnitSphere;
        }

        private void Update()
        {
            _currentTime += Time.unscaledDeltaTime;
            if (_currentTime >= _rotationTimer)
            {
                _currentTime = 0;
                _targetRotation = URandom.onUnitSphere;
            }

            //transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, _rotationSpeed * Time.deltaTime);
            _rotation = Vector3.Slerp(_rotation, _targetRotation, _accelerationSpeed * Time.unscaledDeltaTime);
            transform.Rotate(_rotation * _rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}
