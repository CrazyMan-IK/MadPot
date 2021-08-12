using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;
using DG.Tweening;
using TMPro;

namespace MadPot
{
    public class FoodSpawner : MonoBehaviour
    {
        public event Action LevelWinned = null;
        public event Action LevelFailed = null;
        public event Action LevelRestarted = null;

        [SerializeField] private Transform _productsHolder = null;
        [SerializeField] private Timer _timer = null;
        [SerializeField] private TextMeshProUGUI _levelText = null;

        private readonly List<Product> _spawnedProducts = new List<Product>();
        private Tween _timeScaleTween = null;
        private Coroutine _lastTutorial = null;
        private bool _failed = false;

        public LevelInformation CurrentLevel { get; set; }
        public TutorialHand TutorialHand { get; set; }
        public TutorialViewer TutorialViewer { get; set; }
        public LineRenderer TutorialLine { get; set; }
        public TextMeshProUGUI TutorialText { get; set; }

        private void Start()
        {
            Amplitude.Instance.logEvent("game_start");

            //_timer.Completed += LevelFail;
            RestartGame();
        }

        public void LevelComplete()
        {
            if (_failed)
            {
                return;
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
            }
            else
            {
                Amplitude.Instance.logEvent("level_win");
                LevelWinned?.Invoke();
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
                product.transform.DORotateQuaternion(URandom.rotation, 2.0f).SetEase(Ease.OutQuad).SetUpdate(true);
            }

            if (_timeScaleTween != null)
            {
                _timeScaleTween.Kill(true);
            }

            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, 1.0f).SetEase(Ease.InQuart).SetUpdate(true);

            Amplitude.Instance.logEvent("level_fail");
            LevelFailed?.Invoke();
        }

        public void RestartGame()
        {
            //_levelText.Show("Level " + CurrentLevel.Index);
            var seq = DOTween.Sequence().SetUpdate(true);
            seq.Append(_levelText.rectTransform.DOAnchorPosX(0, 2).From(_levelText.rectTransform.anchoredPosition + Vector2.left * 2000).SetUpdate(true));
            seq.Append(_levelText.rectTransform.DOAnchorPosX(2000, 2).SetUpdate(true));
            _levelText.text = "Level " + CurrentLevel.Index;

            _failed = false;
            for (int i = 0; i < _productsHolder.childCount; i++)
            {
                Destroy(_productsHolder.GetChild(i).gameObject);
            }

            _spawnedProducts.Clear();

            Invoke(nameof(StartGame), 1.5f);

            //_timer.gameObject.SetActive(!CurrentLevel.CurrentCombination.IsTutorial);
            _timer.gameObject.SetActive(false);

            LevelRestarted?.Invoke();
        }

        private void StartGame()
        {
            Amplitude.Instance.logEvent("level_start");

            if (_lastTutorial != null)
            {
                StopCoroutine(_lastTutorial);
            }
            
            _lastTutorial = StartCoroutine(CurrentLevel.CurrentCombination.StartTutorial(CurrentLevel, TutorialHand, TutorialViewer, TutorialText, TutorialLine));

            Spawn();
        }

        private void Spawn()
        {
            if (_failed)
            {
                return;
            }

#if !UNITY_EDITOR || DISABLE_EDITOR_RESTRICTIONS
            if (PlayerPrefs.HasKey("product-" + CurrentLevel.CurrentCombination.TargetProductsType.ToString()))
#endif
            if (CurrentLevel.CurrentCombination.TargetProductsType != ProductType.Edible)
            {
                TutorialText.gameObject.SetActive(true);
                TutorialText.text = "Only " + System.Text.RegularExpressions.Regex.Replace(CurrentLevel.CurrentCombination.TargetProductsType.ToString(), @"([a-z])([A-Z])", "$1 $2").ToLower();

                PlayerPrefs.SetInt("product-" + CurrentLevel.CurrentCombination.TargetProductsType.ToString(), 1);
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
                //product.transform.DORotateQuaternion(targetRotation, 2.5f).SetEase(Ease.OutQuart).SetUpdate(true);
            }

            _timeScaleTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 2.5f).SetEase(Ease.OutQuart).SetUpdate(true);

            _timer.Restart();
            //_timer.gameObject.SetActive(!CurrentLevel.CurrentCombination.IsTutorial);
        }
    }
}
