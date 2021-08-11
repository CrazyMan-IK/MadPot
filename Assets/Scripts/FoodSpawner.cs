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
        public event Action LevelWinned = null;
        public event Action LevelFailed = null;

        [SerializeField] private Transform _productsHolder = null;
        [SerializeField] private Timer _timer = null;

        private readonly List<Product> _spawnedProducts = new List<Product>();
        private Tween _timeScaleTween = null;
        private bool _failed = false;

        public LevelInformation CurrentLevel { get; set; }
        public TutorialHand TutorialHand { get; set; }
        public TutorialViewer TutorialViewer { get; set; }
        public LineRenderer TutorialLine { get; set; }

        private void Start()
        {
            _timer.Completed += LevelFail;
            Invoke(nameof(StartGame), 1.5f);

            _timer.gameObject.SetActive(!CurrentLevel.CurrentCombination.IsTutorial);
        }

        public bool LevelComplete()
        {
            if (_failed)
            {
                return false;
            }

            _timer.Pause();

            foreach (var product in _spawnedProducts)
            {
                if (CurrentLevel.IsProductCorrect(product))
                {
                    product.transform.DOKill();
                    product.transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.OutQuad);
                    product.transform.DOMove(_productsHolder.position, 2.0f).SetEase(Ease.OutQuad).SetUpdate(true);
                    product.transform.DORotateQuaternion(URandom.rotation, 2.0f).SetEase(Ease.OutQuad);
                }
                else
                {
                    var targetPosition = product.transform.position;
                    var direction = (targetPosition - _productsHolder.position);

                    product.transform.DOMove(targetPosition + direction * 4, 2.5f).SetEase(Ease.InOutSine).SetUpdate(true);
                }
            }

            if (_timeScaleTween != null)
            {
                _timeScaleTween.Kill(true);
            }

            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, 1.0f).SetEase(Ease.InQuart).SetUpdate(true);

            _spawnedProducts.Clear();

            CurrentLevel.CompleteCombination();

            if (!CurrentLevel.IsCompleted)
            {
                Invoke(nameof(Spawn), 1.5f);
                return false;
            }
            else
            {
                Amplitude.Instance.logEvent("level_win");
                LevelWinned?.Invoke();
                return true;
            }
        }

        public void LevelFail()
        {
            if (_failed || CurrentLevel.CurrentCombination.IsTutorial)
            {
                return;
            }

            _failed = true;

            _timer.Pause();

            foreach (var product in _spawnedProducts)
            {
                var targetPosition = product.transform.position;
                var direction = (targetPosition - _productsHolder.position);

                product.transform.DOMove(targetPosition + direction * 4, 2.5f).SetEase(Ease.InOutSine).SetUpdate(true);
            }

            if (_timeScaleTween != null)
            {
                _timeScaleTween.Kill(true);
            }

            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, 1.0f).SetEase(Ease.InQuart).SetUpdate(true);

            Amplitude.Instance.logEvent("level_fail");
            LevelFailed?.Invoke();
        }

        private void StartGame()
        {
            Amplitude.Instance.logEvent("level_start");
            StartCoroutine(CurrentLevel.CurrentCombination.StartTutorial(CurrentLevel, TutorialHand, TutorialViewer, TutorialLine));
            Spawn();
        }

        private void Spawn()
        {
            if (_failed)
            {
                return;
            }

            foreach (var levelProduct in CurrentLevel.CurrentCombination.Products)
            {
                var product = Instantiate(levelProduct.Product, _productsHolder.position, Quaternion.identity, _productsHolder);
                _spawnedProducts.Add(product);

                var targetPosition = levelProduct.GetTargetPosition(Camera.main);
                //var direction = (targetPosition - center).normalized;
                var direction = (targetPosition - _productsHolder.position);

                var targetRotation = Camera.main.transform.rotation * Quaternion.Euler(URandom.Range(product.MinRotation.x, product.MaxRotation.x), URandom.Range(product.MinRotation.y, product.MaxRotation.y), URandom.Range(product.MinRotation.z, product.MaxRotation.z));

                product.transform.DOScale(Vector3.zero, 2.0f).From().SetEase(Ease.OutQuart);
                //product.transform.DOMove(targetPosition + direction * 4, 9.0f).SetEase(Ease.OutQuart);
                product.transform.DOMove(targetPosition, 2.5f).SetEase(Ease.OutQuart).SetUpdate(true);
                //product.transform.DORotateQuaternion(URandom.rotation, 3.0f).SetEase(Ease.OutQuart);
                product.transform.DORotateQuaternion(targetRotation, 2.5f).SetEase(Ease.OutQuart).SetUpdate(true);
            }

            _timeScaleTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 2.5f).SetEase(Ease.OutQuart).SetUpdate(true);

            _timer.Restart();
            _timer.gameObject.SetActive(!CurrentLevel.CurrentCombination.IsTutorial);
        }
    }
}
