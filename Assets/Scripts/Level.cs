using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadPot
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelInformation _level = null;
        [SerializeField] private FoodSpawner _spawner = null;
        [SerializeField] private FigureVisualizer _figureVisualizer = null;

        private void Awake()
        {
            OnLevelCombinationChanged();
            _spawner.CurrentLevel = _level;

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

        private void OnLevelCombinationChanged()
        {
            _figureVisualizer.Type = _level.CurrentCombination.TargetFigureType;
        }
    }
}
