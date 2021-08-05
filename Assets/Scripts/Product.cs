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

        public ProductType Type => _type;
    }
}
