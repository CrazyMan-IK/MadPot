using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    [CreateAssetMenu(fileName = "New Products List", menuName = "Mad Pot/Products List", order = 100)]
    public class ProductsList : ScriptableObject
    {
        [field: SerializeField] public List<Transform> Products { get; set; } = new List<Transform>();
    }
}
