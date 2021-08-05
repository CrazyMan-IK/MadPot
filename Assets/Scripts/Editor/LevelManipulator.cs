using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MadPot
{
    public class LevelManipulator : MouseManipulator
    {
        private LevelInformation _level;
        private LevelDraw _levelDraw;

        public LevelManipulator(LevelInformation level)
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            _level = level;
            _levelDraw = new LevelDraw { products = level.Products };
        }
        protected override void RegisterCallbacksOnTarget()
        {
            target.Add(_levelDraw);
            target.Add(new Label { name = "mousePosition", text = "(0,0)" });
            target.RegisterCallback<MouseDownEvent>(MouseDown);
            target.RegisterCallback<MouseMoveEvent>(MouseMove);
            target.RegisterCallback<MouseCaptureOutEvent>(MouseOut);
            target.RegisterCallback<MouseUpEvent>(MouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(MouseDown);
            target.UnregisterCallback<MouseUpEvent>(MouseUp);
            target.UnregisterCallback<MouseMoveEvent>(MouseMove);
            target.UnregisterCallback<MouseCaptureOutEvent>(MouseOut);
        }

        private void MouseOut(MouseCaptureOutEvent evt) => _levelDraw.drawSegment = false;

        private void MouseMove(MouseMoveEvent evt)
        {
            var t = target as LevelEditorGraphView;
            var mouseLabel = target.Q("mousePosition") as Label;
            mouseLabel.transform.position = evt.localMousePosition + Vector2.up * 20;
            mouseLabel.text = t.ScreenToWorldSpace(evt.localMousePosition).ToString();

            //if left mouse is pressed 
            if ((evt.pressedButtons & 1) != 1) return;
            _levelDraw.end = t.ScreenToWorldSpace(evt.localMousePosition);
            _levelDraw.MarkDirtyRepaint();
        }

        private void MouseUp(MouseUpEvent evt)
        {
            if (!CanStopManipulation(evt)) return;
            target.ReleaseMouse();
            if (!_levelDraw.drawSegment) return;

            if (_level.Products.Count == 0) _level.Products.Add(new LevelProduct() { Position = _levelDraw.start });

            var t = target as LevelEditorGraphView;
            _level.Products.Add(new LevelProduct() { Position = t.ScreenToWorldSpace(evt.localMousePosition) });
            _levelDraw.drawSegment = false;

            _levelDraw.MarkDirtyRepaint();
        }

        private void MouseDown(MouseDownEvent evt)
        {
            if (!CanStartManipulation(evt)) return;
            target.CaptureMouse();

            _levelDraw.drawSegment = true;
            var t = target as LevelEditorGraphView;

            if (_level.Products.Count != 0) _levelDraw.start = _level.Products.Last().Position;
            else _levelDraw.start = t.ScreenToWorldSpace(evt.localMousePosition);

            _levelDraw.end = t.ScreenToWorldSpace(evt.localMousePosition);
            _levelDraw.MarkDirtyRepaint();
        }
        private class LevelDraw : ImmediateModeElement
        {
            public List<LevelProduct> products { get; set; } = new List<LevelProduct>();
            public Vector2 start { get; set; }
            public Vector2 end { get; set; }
            public bool drawSegment { get; set; }
            protected override void ImmediateRepaint()
            {
                var corners = new Vector3[4];
                Camera.main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), 1.0f, Camera.MonoOrStereoscopicEye.Mono, corners);

                var lineColor = new Color(1.0f, 0.6f, 0.0f, 1.0f);
                var t = parent as LevelEditorGraphView;

                //Draw screen
                /*float w = Screen.width;
                float h = Screen.width;
                var screenLT = Vector2.left * (w - (w / 2)) + Vector2.up * (h - (h / 2));
                var screenRT = screenLT + Vector2.right * w;
                var screenLB = screenLT + Vector2.down * h;
                var screenRB = screenRT + Vector2.down * h;

                GL.Begin(GL.LINES);
                GL.Color(Color.white);
                GL.Vertex(t.ScreenToWorldSpace(screenLT));
                GL.Vertex(t.ScreenToWorldSpace(screenRT));

                GL.Vertex(t.ScreenToWorldSpace(screenRT));
                GL.Vertex(t.ScreenToWorldSpace(screenRB));

                GL.Vertex(t.ScreenToWorldSpace(screenRB));
                GL.Vertex(t.ScreenToWorldSpace(screenLB));
                
                GL.Vertex(t.ScreenToWorldSpace(screenLB));
                GL.Vertex(t.ScreenToWorldSpace(screenLT));
                GL.End();*/

                GL.Begin(GL.LINES);
                GL.Color(Color.white);
                GL.Vertex(t.WorldToScreenSpace(corners[0]));
                GL.Vertex(t.WorldToScreenSpace(corners[1]));

                GL.Vertex(t.WorldToScreenSpace(corners[1]));
                GL.Vertex(t.WorldToScreenSpace(corners[2]));

                GL.Vertex(t.WorldToScreenSpace(corners[2]));
                GL.Vertex(t.WorldToScreenSpace(corners[3]));

                GL.Vertex(t.WorldToScreenSpace(corners[3]));
                GL.Vertex(t.WorldToScreenSpace(corners[0]));
                GL.End();

                //Draw shape        
                for (int i = 0; i < products.Count - 1; i++)
                {
                    var p1 = t.WorldToScreenSpace(products[i].Position);
                    var p2 = t.WorldToScreenSpace(products[i + 1].Position);
                    GL.Begin(GL.LINES);
                    GL.Color(lineColor);
                    GL.Vertex(p1);
                    GL.Vertex(p2);
                    GL.End();
                }

                if (!drawSegment) return;

                //Draw current segment
                GL.Begin(GL.LINES);
                GL.Color(lineColor);
                GL.Vertex(t.WorldToScreenSpace(start));
                GL.Vertex(t.WorldToScreenSpace(end));
                GL.End();
            }
        }
    }
}
