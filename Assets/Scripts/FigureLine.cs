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

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _points.Clear();
        }

        public void OnDrag(PointerEventData eventData)
        {
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
            //return;

            Gesture target = new Gesture(_spawner.CurrentLevel.GetTargetPoints(Camera.main).Select(p => new Point(p.x, p.y, 0)).ToArray(), "Target");
            Gesture candidate = new Gesture(_points.Select(p => new Point(p.x, p.y, 0)).ToArray());

            _renderer.positionCount = 0;

            var result = QPointCloudRecognizer.Classify(candidate, new Gesture[] { target });

            if (result.Distance > 3)
            {
                Debug.LogError($"Cannot recognize gesture - {result.Distance}");
                return;
            }

            Debug.Log($"{result.GestureName} - {result.Distance}");

            _spawner.CompleteLevel();
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
    }
}
