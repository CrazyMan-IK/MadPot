using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Mad Pot/Level", order = 100)]
    public class LevelInformation : ScriptableObject
    {
        [field: SerializeField] public FigureType TargetFigureType { get; set; } = FigureType.None;
        [field: SerializeField] public ProductType TargetProductsType { get; set; } = ProductType.Inedible;
        [field: SerializeField] public List<LevelProduct> Products { get; set; } = new List<LevelProduct>();

        public IEnumerable<Vector3> GetTargetPoints(Camera camera)
        {
            LevelProduct first = null;

            for (int i = 0; i < Products.Count; i++)
            {
                var levelProduct = Products[i];

                if (levelProduct.Product == null)
                {
                    continue;
                }

                if (levelProduct.Product.Type != TargetProductsType)
                {
                    continue;
                }

                if (first == null)
                {
                    first = levelProduct;
                }

                yield return levelProduct.GetTargetPosition(camera);
            }

            if (first != null)
            {
                yield return first.GetTargetPosition(camera);
            }
        }

        public IEnumerable<Vector3> GetFailPoints(Camera camera)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                var levelProduct = Products[i];

                if (levelProduct.Product == null)
                {
                    continue;
                }

                if (levelProduct.Product.Type == TargetProductsType)
                {
                    continue;
                }

                yield return levelProduct.GetTargetPosition(camera);
            }
        }

        public IEnumerable<Vector3> GetNullPoints(Camera camera)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                var levelProduct = Products[i];

                if (levelProduct.Product != null)
                {
                    continue;
                }

                yield return levelProduct.GetTargetPosition(camera);
            }
        }
    }
}
