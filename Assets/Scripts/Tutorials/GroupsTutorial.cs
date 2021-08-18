using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MadPot.Interfaces;

namespace MadPot.Tutorials
{
    public class GroupsTutorial : ITutorial
    {
        private readonly WaitForSecondsRealtime _wait = new WaitForSecondsRealtime(1.0f);
        private int _currentPoint = 0;

        public LevelInformation CurrentLevel { get; set; } = null;
        public int CurrentLevelIndex { get; set; } = 0;

        public IEnumerator StartTutorial(TutorialHand hand, TutorialViewer viewer, TextMeshProUGUI text, LineRenderer line)
        {
            Debug.Log("Groups tutorial started");

            hand.gameObject.SetActive(true);
            viewer.gameObject.SetActive(true);

            if (CurrentLevel == null)
            {
                yield break;
            }

            yield return _wait;

            viewer.RectTransform.anchorMin = Vector2.one;
            viewer.RectTransform.anchorMax = Vector2.one;
            viewer.RectTransform.anchoredPosition = new Vector2(-192, -96);

            var points = CurrentLevel.GetTargetPoints(Camera.main).ToArray();
            var startPoint = points.Length - (CurrentLevel.CurrentCombination.TargetFigureType == FigureType.Line ? 1 : 2);
            hand.SetPosition(points[startPoint]);
            hand.TargetPoint = points[startPoint];
            line.positionCount = 2;
            line.SetPosition(0, points[startPoint]);
            line.SetPosition(1, points[startPoint]);

            yield return _wait;

            hand.TargetPoint = points[_currentPoint];

            while (true)
            {
                yield return null;

                while (Vector3.Distance(hand.GetPosition(), points[_currentPoint]) <= 0.001f)
                {
                    Debug.Log("1");

                    line.positionCount++;
                    line.SetPosition(_currentPoint + 1, points[_currentPoint]);
                    _currentPoint++;

                    Debug.Log("2");
                }

                if (_currentPoint == points.Length - 1)
                {
                    Debug.Log("3");
                    line.positionCount--;

                    yield return _wait;
                    _currentPoint = 0;
                    line.positionCount = 2;
                    line.SetPosition(0, hand.GetPosition());
                    line.SetPosition(1, hand.GetPosition());

                    Debug.Log("4");
                }

                if (CurrentLevel.CurrentCombination.TargetFigureType == FigureType.Line && _currentPoint == 0)
                {
                    Debug.Log("5");

                    line.SetPosition(0, points[_currentPoint]);
                    line.SetPosition(1, points[_currentPoint]);
                    hand.SetPosition(points[_currentPoint]);
                    hand.TargetPoint = points[_currentPoint];
                    yield return _wait;

                    Debug.Log("6");
                }

                hand.TargetPoint = points[_currentPoint];
                line.SetPosition(_currentPoint + 1, hand.GetPosition());
            }
        }
    }
}
