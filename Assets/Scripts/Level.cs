using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace MadPot
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelInformation _level = null;

        [Space]

        [SerializeField] private FoodSpawner _spawner = null;
        [SerializeField] private FigureVisualizer _figureVisualizer = null;
        [SerializeField] private ProductVisualizer _productVisualizer = null;
        [SerializeField] private TutorialHand _hand = null;
        [SerializeField] private TutorialViewer _viewer = null;
        [SerializeField] private LineRenderer _tutorialLine = null;
        [SerializeField] private TextMeshProUGUI _tutorialText = null;

        [Space]

        [SerializeField] private CanvasGroup _winUI = null;
        [SerializeField] private CanvasGroup _failUI = null;

        [SerializeField] private Button _nextLevelButton = null;
        [SerializeField] private Button _restartLevelButton = null;

        [SerializeField] private ParticleSystem _winParticles = null;

        private void Awake()
        {
            OnLevelCombinationChanged();
            _spawner.CurrentLevel = _level;
            _spawner.TutorialHand = _hand;
            _spawner.TutorialViewer = _viewer;
            _spawner.TutorialLine = _tutorialLine;
            _spawner.TutorialText = _tutorialText;

            _spawner.LevelWinned += OnLevelWinned;
            _spawner.LevelFailed += OnLevelFailed;

            _level.CombinationChanged += OnLevelCombinationChanged;
        }

        private void OnDrawGizmos()
        {
            var points = _level.GetTargetPoints(Camera.main).ToList();
            var failPoints = _level.GetFailPoints(Camera.main).ToList();
            var nullPoints = _level.GetNullPoints(Camera.main).ToList();

            Gizmos.color = Color.white;
            for (int i = 0; i < points.Count - 1; i++)
            {
                var currentPoint = points[i];
                var nextPoint = points[i + 1];

                Gizmos.DrawLine(currentPoint, nextPoint);
            }

            Gizmos.color = Color.green;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point, 0.02f);
            }

            Gizmos.color = Color.red;
            foreach (var point in failPoints)
            {
                Gizmos.DrawSphere(point, 0.02f);
            }

            Gizmos.color = Color.gray;
            foreach (var point in nullPoints)
            {
                Gizmos.DrawSphere(point, 0.02f);
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void OnLevelWinned()
        {
            _winParticles.Play();

            _winUI.DOFade(1.0f, 1.0f);
            _nextLevelButton.onClick.AddListener(NextLevel);

            _winUI.interactable = true;
            _winUI.blocksRaycasts = true;
        }

        private void OnLevelFailed()
        {
            _failUI.DOFade(1.0f, 1.0f);
            _restartLevelButton.onClick.AddListener(RestartLevel);

            _failUI.interactable = true;
            _failUI.blocksRaycasts = true;
        }

        private void OnLevelCombinationChanged()
        {
            _figureVisualizer.Type = _level.CurrentCombination.TargetFigureType;
            _productVisualizer.Type = _level.CurrentCombination.TargetProductsType;

            _hand.transform.position = Vector3.down * 512;
            _viewer.transform.position = Vector3.down * 512;

            _hand.gameObject.SetActive(_level.CurrentCombination.IsTutorial);
            _viewer.gameObject.SetActive(_level.CurrentCombination.IsTutorial);
            _tutorialLine.gameObject.SetActive(_level.CurrentCombination.IsTutorial);
            _tutorialText.gameObject.SetActive(false);
        }

        public void NextLevel()
        {
            _level.CombinationChanged -= OnLevelCombinationChanged;

            _level = _level.NextLevel;
            OnLevelCombinationChanged();

            _level.CombinationChanged += OnLevelCombinationChanged;

            _spawner.CurrentLevel = _level;
            _spawner.RestartGame();

            _winUI.DOFade(0.0f, 1.0f);
            _nextLevelButton.onClick.RemoveAllListeners();

            _winUI.interactable = false;
            _winUI.blocksRaycasts = false;
        }

        public void RestartLevel()
        {
            _level.Reset();
            OnLevelCombinationChanged();

            _spawner.RestartGame();

            _failUI.DOFade(0.0f, 1.0f);
            _restartLevelButton.onClick.RemoveAllListeners();

            _failUI.interactable = false;
            _failUI.blocksRaycasts = false;
        }
    }
}
