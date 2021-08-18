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
        [SerializeField] private HandPointer _hand = null;

        private readonly List<Vector3> _points = new List<Vector3>();
        private LineRenderer _renderer = null;
        private bool _disabled = true;
        private bool _isDragging = false;

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();

            _spawner.LevelWinned += OnLevelWinned;
            _spawner.LevelRestarted += OnLevelRestarted;

            _hand.MouseDown += OnMouseDown;
            _hand.MouseUp += OnMouseUp;
        }

        private void Update()
        {
            if (!_isDragging)
            {
                return;
            }

            foreach (var point in _points)
            {
                var colliders = Physics.OverlapSphere(point, 0.025f);

                if (colliders.Length < 1)
                {
                    continue;
                }

                var firstProduct = colliders.Select(x => x.GetComponent<Product>()).FirstOrDefault();

                if (firstProduct == null)
                {
                    continue;
                }

                firstProduct.ShowOutline();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnMouseDown(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_disabled || !_isDragging)
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
            OnMouseUp(eventData.position);
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

        private void OnMouseDown(Vector2 position)
        {
            if (_disabled)
            {
                return;
            }

            _isDragging = true;

            _points.Clear();
        }

        private void OnMouseUp(Vector2 position)
        {
            if (_disabled)
            {
                return;
            }

            _isDragging = false;

            Gesture target = new Gesture(_spawner.CurrentLevel.GetTargetPoints(Camera.main).Select(p => new Point(p.x, p.y, 0)).ToArray(), "Target");
            Gesture candidate = new Gesture(_points.Select(p => new Point(p.x, p.y, 0)).ToArray());

            _renderer.positionCount = 0;

            var result = QPointCloudRecognizer.Classify(candidate, new Gesture[] { target });

            _spawner.HideAllOutlines();

            if (result.Distance > _spawner.CurrentLevel.CurrentCombination.MaximumDetectionDistance)
            {
                //Debug.LogError($"Cannot recognize gesture - {result.Distance}");
                _spawner.LevelFail();
                return;
            }

            //Debug.Log($"{result.GestureName} - {result.Distance}");

            _spawner.LevelComplete();
        }
    }
}
