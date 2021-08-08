using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MadPot
{
    public class CombinationEditorGraphView : GraphView
    {
        const float _pixelsPerUnit = 100f;
        const bool _invertYPosition = true;

        public CombinationEditorGraphView(Combination combination)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("CombinationEditorGraph"));
            this.StretchToParentSize();

            SetupZoom(2.5f, 5, 0.15f, 5f);
            Add(new GridBackground());

            //pan with Alt-LeftMouseButton drag/ MidleMouseButton drag
            this.AddManipulator(new ContentDragger());
            //viewTransform.position = Vector3.down * 405;
            //viewTransform.position = Vector3.down * 10000;
            //var diff = this.ChangeCoordinatesTo(contentViewContainer, Vector2.down * 100);
            //Vector3 s = contentViewContainer.transform.scale;
            //viewTransform.position += Vector3.Scale(diff, s);

            //other things that might interest you
            //this.AddManipulator(new SelectionDragger());
            //this.AddManipulator(new RectangleSelector());
            //this.AddManipulator(new ClickSelector());

            this.AddManipulator(new CombinationManipulator(combination));

            contentViewContainer.BringToFront();
            contentViewContainer.Add(new Label { name = "origin", text = "(0,0)" });

            //set the origin to the center of the window
            schedule.Execute(() =>
            {
                contentViewContainer.transform.position = parent.worldBound.size / 2f;
                viewTransform.position += Vector3.down * 80;
                viewTransform.scale = new Vector3(5, 5, 1);
            });
        }

        public Vector2 WorldToScreenSpace(Vector2 pos)
        {
            var position = pos * _pixelsPerUnit - contentViewContainer.layout.position;
            if (_invertYPosition) position.y = -position.y;
            return contentViewContainer.transform.matrix.MultiplyPoint3x4(position);
        }

        public Vector2 ScreenToWorldSpace(Vector2 pos)
        {
            Vector2 position = contentViewContainer.transform.matrix.inverse.MultiplyPoint3x4(pos);
            if (_invertYPosition) position.y = -position.y;
            return (position + contentViewContainer.layout.position) / _pixelsPerUnit;
        }
    }
}
