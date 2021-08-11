using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GestureRecognizer;

namespace MadPot
{
    [RequireComponent(typeof(LineRenderer))]
    public class FigureLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
    {
        [SerializeField] private FoodSpawner _spawner = null;

        private readonly List<Vector3> _points = new List<Vector3>();
        private LineRenderer _renderer = null;
        private bool _disabled = true;

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();

            _spawner.LevelWinned += OnLevelWinned;
            _spawner.LevelRestarted += OnLevelRestarted;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_disabled)
            {
                return;
            }

            _points.Clear();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_disabled)
            {
                return;
            }

            var mainCamera = Camera.main;

            //var positionRay = mainCamera.ScreenPointToRay(eventData.position);
            //_points.Add(positionRay.origin + positionRay.direction.normalized);

            Vector3 screenPos = eventData.position;
            screenPos.z = 0.5f;

            var position = mainCamera.ScreenToWorldPoint(screenPos);
            _points.Add(position);// + mainCamera.transform.forward / 2);

            _renderer.positionCount = _points.Count;
            _renderer.SetPositions(_points.ToArray());
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_disabled)
            {
                return;
            }

            Gesture target = new Gesture(_spawner.CurrentLevel.GetTargetPoints(Camera.main).Select(p => new Point(p.x, p.y, 0)).ToArray(), "Target");
            Gesture candidate = new Gesture(_points.Select(p => new Point(p.x, p.y, 0)).ToArray());

            _renderer.positionCount = 0;

            var result = QPointCloudRecognizer.Classify(candidate, new Gesture[] { target });

            if (result.Distance > 7)
            {
                Debug.LogError($"Cannot recognize gesture - {result.Distance}");
                _spawner.LevelFail();
                return;
            }

            Debug.Log($"{result.GestureName} - {result.Distance}");
            
            _spawner.LevelComplete();
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        private void OnLevelWinned()
        {
            _disabled = true;
        }

        private void OnLevelRestarted()
        {
            _disabled = false;
        }
    }
}
