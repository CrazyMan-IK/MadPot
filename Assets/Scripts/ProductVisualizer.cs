using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MadPot
{
    public class ProductVisualizer : MonoBehaviour
    {
        [SerializeField] private Image _productImage = null;
        [SerializeField] private Sprite _vegetableOrFruitSprite = null;
        [SerializeField] private Sprite _fastFoodSprite = null;
        [SerializeField] private Sprite _meatOrFishSprite = null;
        [SerializeField] private Sprite _bakerySprite = null;

        private ProductType _type = ProductType.Inedible;

        public ProductType Type 
        {
            get => _type; 
            set
            {
                if (_type != value)
                {
                    if (value == ProductType.Inedible)
                    {
                        value = ProductType.Edible;
                    }
                    _type = value;

                    gameObject.SetActive(true);
                    switch (_type)
                    {
                        case ProductType.Edible:
                            gameObject.SetActive(false);
                            break;
                        case ProductType.FruitsAndVegetables:
                            _productImage.sprite = _vegetableOrFruitSprite;
                            break;
                        case ProductType.FastFood:
                            _productImage.sprite = _fastFoodSprite;
                            break;
                        case ProductType.Meat:
                            _productImage.sprite = _meatOrFishSprite;
                            break;
                        case ProductType.Bakery:
                            _productImage.sprite = _bakerySprite;
                            break;
                    }
                }
            }
        }
    }
}
