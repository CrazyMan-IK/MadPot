using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MadPot
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelInformation _level = null;
        [SerializeField] private FoodSpawner _spawner = null;
        [SerializeField] private FigureVisualizer _figureVisualizer = null;
        [SerializeField] private ProductVisualizer _productVisualizer = null;
        [SerializeField] private TutorialHand _hand = null;
        [SerializeField] private TutorialViewer _viewer = null;
        [SerializeField] private LineRenderer _tutorialLine = null;

        [Space]

        [SerializeField] private CanvasGroup _winUI = null;
        [SerializeField] private CanvasGroup _failUI = null;

        [SerializeField] private Button _nextLevelButton = null;
        [SerializeField] private Button _restartLevelButton = null;

        private void Awake()
        {
            OnLevelCombinationChanged();
            _spawner.CurrentLevel = _level;
            _spawner.TutorialHand = _hand;
            _spawner.TutorialViewer = _viewer;
            _spawner.TutorialLine = _tutorialLine;

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

            }
        }

        private void OnLevelWinned()
        {
            _winUI.DOFade(1.0f, 1.0f);
            _nextLevelButton.onClick.AddListener(NextLevel);
        }

        private void OnLevelFailed()
        {
            _failUI.DOFade(1.0f, 1.0f);
            _restartLevelButton.onClick.AddListener(NextLevel);
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
        }

        public void NextLevel()
        {

        }

        public void RestartLevel()
        {

        }
    }
}
