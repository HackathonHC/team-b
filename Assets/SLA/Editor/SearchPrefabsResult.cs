using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SLA
{
    public class SearchPrefabsResult : EditorWindow
    {
        public Object[] objects;
        Vector2 _scrollPosition;

        void OnGUI() 
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach(var obj in objects)
            {
                EditorGUILayout.ObjectField(obj, typeof(GameObject), false);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
