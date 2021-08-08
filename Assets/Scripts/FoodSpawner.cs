using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;
using DG.Tweening;

namespace MadPot
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] Transform _productsHolder = null;

        private readonly List<Product> _spawnedProducts = new List<Product>();

        public LevelInformation CurrentLevel { get; set; }

        private void Start()
        {
            Spawn();
        }

        public void CompleteLevel()
        {
            foreach (var product in _spawnedProducts)
            {
                if (CurrentLevel.IsProductCorrect(product))
                {
                    product.transform.DOScale(Vector3.zero, 3.0f).SetEase(Ease.OutQuart);
                    product.transform.DOMove(_productsHolder.position, 2.0f).SetEase(Ease.OutQuart);
                    product.transform.DORotateQuaternion(URandom.rotation, 1.5f).SetEase(Ease.OutQuart);

                    DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, 0.1f).SetEase(Ease.Linear);
                }
            }

            _spawnedProducts.Clear();

            CurrentLevel.CompleteCombination();

            if (!CurrentLevel.IsCompleted)
            {
                Invoke(nameof(Spawn), 1.5f);
            }
        }

        private void Spawn()
        {
            //var center = Camera.main.transform.position + Camera.main.transform.forward;

            foreach (var levelProduct in CurrentLevel.CurrentCombination.Products)
            {
                var product = Instantiate(levelProduct.Product, _productsHolder.position, Quaternion.identity, _productsHolder);
                _spawnedProducts.Add(product);

                var targetPosition = levelProduct.GetTargetPosition(Camera.main);
                //var direction = (targetPosition - center).normalized;
                var direction = (targetPosition - _productsHolder.position);

                product.transform.DOScale(Vector3.zero, 2.0f).From().SetEase(Ease.OutQuart);
                product.transform.DOMove(targetPosition + direction * 4, 9.0f).SetEase(Ease.OutQuart);
                product.transform.DORotateQuaternion(URandom.rotation, 3.0f).SetEase(Ease.OutQuart);

                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.001f, 0.5f).SetEase(Ease.Linear);
            }
        }
    }
}
