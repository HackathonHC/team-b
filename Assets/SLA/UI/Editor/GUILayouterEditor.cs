using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SLA
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GUILayouter), true)]
    public class GUILayouterEditor : Editor
    {
        public override void OnInspectorGUI ()
        {
            this.DrawDefaultInspector();
            
            EditorGUILayout.Space();
            if(GUILayout.Button("Execute"))
            {
                var guiLayouter = target as GUILayouter;
                guiLayouter.Execute();
            }
        }
    }
}

