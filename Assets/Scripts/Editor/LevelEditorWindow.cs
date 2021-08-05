using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace MadPot
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelEditorGraphView _levelEditorGraphView;
        private LevelInformation _level;

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        private static bool Callback(int instanceID, int line)
        {
            var shape = EditorUtility.InstanceIDToObject(instanceID) as LevelInformation;
            if (shape != null)
            {
                OpenWindow(shape);
                return true;
            }
            return false; // we did not handle the open
        }

        private static void OpenWindow(LevelInformation level)
        {
            var window = GetWindow<LevelEditorWindow>();
            window.titleContent = new GUIContent("Shape Editor");
            window._level = level;
            window.rootVisualElement.Clear();
            window.CreateGraphView();
            window.CreateToolbar();
        }

        private void CreateToolbar()
        {
            var toolbar = new Toolbar();
            var clearBtn = new ToolbarButton(() => _level.Products.Clear());
            clearBtn.text = "Clear";
            var undoBtn = new ToolbarButton(() => _level.Products.RemoveAt(_level.Products.Count - 1));
            undoBtn.text = "Undo";
            toolbar.Add(clearBtn);
            toolbar.Add(new ToolbarSpacer());
            toolbar.Add(undoBtn);
            rootVisualElement.Add(toolbar);
        }

        private void CreateGraphView()
        {
            _levelEditorGraphView = new LevelEditorGraphView(_level);
            _levelEditorGraphView.name = "Shape Editor Graph";
            rootVisualElement.Add(_levelEditorGraphView);
        }
    }
}
