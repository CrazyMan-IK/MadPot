using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    public enum ProductType
    {
        Inedible,
        Edible,
        VegetableOrFruit,
        FastFood,
        MeatOrFish,
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
    }
}
