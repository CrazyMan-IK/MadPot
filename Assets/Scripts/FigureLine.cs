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
        private readonly List<Vector3> _points = new List<Vector3>();
        private LineRenderer _renderer = null;

        private Gesture[] _testingGestures;

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();

            _testingGestures = new Gesture[3];

            _testingGestures[0] = new Gesture(new Point[] { new Point(0, 0, 0), new Point(1, 0, 0), new Point(1, 1, 0), new Point(0, 1, 0), new Point(0, 0, 0) }, "Rectangle");
            _testingGestures[1] = new Gesture(new Point[] { new Point(0, 0, 0), new Point(0.5f, 1, 0), new Point(1, 0, 0), new Point(0, 0, 0) }, "Triangle 1");
            _testingGestures[2] = new Gesture(new Point[] { new Point(0, 1, 0), new Point(0.5f, 0, 0), new Point(1, 1, 0), new Point(0, 0, 0) }, "Triangle 2");
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(16, 16, 64, 32), "SaveFigure"))
            {

            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _points.Clear();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mainCamera = Camera.main;

            var positionRay = mainCamera.ScreenPointToRay(eventData.position);
            _points.Add(positionRay.origin + positionRay.direction.normalized);

            _renderer.positionCount = _points.Count;
            _renderer.SetPositions(_points.ToArray());
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Gesture candidate = new Gesture(_points.Select(p => new Point(p.x, p.y, 0)).ToArray());

            var result = QPointCloudRecognizer.Classify(candidate, _testingGestures);

            if (result.Distance > 40)
            {
                Debug.Log($"Cannot recognize gesture - {result.Distance}");
                return;
            }

            Debug.Log($"{result.GestureName} - {result.Distance}");
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
    }
}
