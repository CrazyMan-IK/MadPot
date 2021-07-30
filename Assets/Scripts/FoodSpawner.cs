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
        [SerializeField] ProductsList _products = null;
        [SerializeField] Rect _spawnRegion = Rect.zero;

        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1.5f);

        private void Awake()
        {
            StartCoroutine(Spawn());
        }

        private void OnDrawGizmos()
        {
            var mainCamera = Camera.main.transform;

            var leftTop = mainCamera.position + mainCamera.forward + mainCamera.right * _spawnRegion.center.x + mainCamera.up * _spawnRegion.center.y;
            var rightTop = leftTop - mainCamera.right * _spawnRegion.width;
            var leftBottom = leftTop - mainCamera.up * _spawnRegion.height;
            var rightBottom = rightTop - mainCamera.up * _spawnRegion.height;

            Gizmos.DrawLine(leftTop, rightTop);
            Gizmos.DrawLine(rightTop, rightBottom);
            Gizmos.DrawLine(rightBottom, leftBottom);
            Gizmos.DrawLine(leftBottom, leftTop);
        }

        public IEnumerator Spawn()
        {
            while (true)
            {
                var mainCamera = Camera.main.transform;
                var leftTop = mainCamera.position + mainCamera.forward + mainCamera.right * _spawnRegion.center.x + mainCamera.up * _spawnRegion.center.y;
                var rightTop = leftTop - mainCamera.right * _spawnRegion.width;
                var leftBottom = leftTop - mainCamera.up * _spawnRegion.height;
                var xAxis = rightTop - leftTop;
                var zAxis = leftBottom - leftTop;

                var productIndex = URandom.Range(0, _products.Products.Count);
                var product = Instantiate(_products.Products[productIndex], _productsHolder.position, Quaternion.identity, _productsHolder);

                var targetPoint = leftTop + xAxis * URandom.value + zAxis * URandom.value;

                product.DOScale(Vector3.zero, 1.5f).From().SetEase(Ease.OutQuart);
                product.DOMove(targetPoint, 2.0f).SetEase(Ease.OutQuart);
                product.DORotateQuaternion(URandom.rotation, 3.0f).SetEase(Ease.OutQuart);

                yield return _waitForSeconds;
            }
        }
    }
}
