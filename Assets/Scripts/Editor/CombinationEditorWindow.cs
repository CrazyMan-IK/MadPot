using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace MadPot
{
    public class CombinationEditorWindow : EditorWindow
    {
        private CombinationEditorGraphView _combinationEditorGraphView;
        private Combination _combination;

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        private static bool Callback(int instanceID, int line)
        {
            var combination = EditorUtility.InstanceIDToObject(instanceID) as Combination;
            if (combination != null)
            {
                OpenWindow(combination);
                return true;
            }
            return false; // we did not handle the open
        }

        private static void OpenWindow(Combination combination)
        {
            var window = GetWindow<CombinationEditorWindow>();
            window.titleContent = new GUIContent("Shape Editor");
            window._combination = combination;
            window.rootVisualElement.Clear();
            window.CreateGraphView();
            window.CreateToolbar();
        }

        private void CreateToolbar()
        {
            var toolbar = new Toolbar();
            var clearBtn = new ToolbarButton(() => _combination.Products.Clear());
            clearBtn.text = "Clear";
            var undoBtn = new ToolbarButton(() => _combination.Products.RemoveAt(_combination.Products.Count - 1));
            undoBtn.text = "Undo";
            toolbar.Add(clearBtn);
            toolbar.Add(new ToolbarSpacer());
            toolbar.Add(undoBtn);
            rootVisualElement.Add(toolbar);
        }

        private void CreateGraphView()
        {
            _combinationEditorGraphView = new CombinationEditorGraphView(_combination);
            _combinationEditorGraphView.name = "Shape Editor Graph";
            rootVisualElement.Add(_combinationEditorGraphView);
        }
    }
}
