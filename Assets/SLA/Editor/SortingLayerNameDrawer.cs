using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace SLA
{
    [CustomPropertyDrawer(typeof(SortingLayerNameAttribute))]
    public class SortingLayerNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sortingLayerNames = SortingLayerHelper.sortingLayerNames;
            EditorGUI.BeginProperty(position, label, property);

            // Look up the layer name using the current layer ID
            string oldName;
            if (property.propertyType == SerializedPropertyType.String)
            {
                oldName = property.stringValue;
            }
            else
            {
                oldName = SortingLayerHelper.GetSortingLayerNameFromID(property.intValue);
            }
            
            // Use the name to look up our array index into the names list
            int oldLayerIndex = Array.IndexOf(sortingLayerNames, oldName);
            
            // Show the popup for the names
            int newLayerIndex = EditorGUI.Popup(position, label.text, oldLayerIndex, sortingLayerNames);
            
            // If the index changes, look up the ID for the new index to store as the new ID
            if (newLayerIndex != oldLayerIndex)
            {
                if (property.propertyType == SerializedPropertyType.String)
                {
                    int id = SortingLayerHelper.GetSortingLayerIDForIndex(newLayerIndex);
                    property.stringValue = SortingLayerHelper.GetSortingLayerNameFromID(id);
                }
                else
                {
                    property.intValue = SortingLayerHelper.GetSortingLayerIDForIndex(newLayerIndex);
                }
            }
            
            EditorGUI.EndProperty();
        }
    }

    public static class SortingLayerHelper
    {
        private static Type _utilityType;
        private static PropertyInfo _sortingLayerNamesProperty;
        private static MethodInfo _getSortingLayerUserIdMethod;
        
        static SortingLayerHelper()
        {
            _utilityType = Type.GetType("UnityEditorInternal.InternalEditorUtility, UnityEditor");
            _sortingLayerNamesProperty = _utilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            
            #if UNITY_4
            _getSortingLayerUserIdMethod = _utilityType.GetMethod("GetSortingLayerUserID", BindingFlags.Static | BindingFlags.NonPublic);
            #else
            _getSortingLayerUserIdMethod = _utilityType.GetMethod("GetSortingLayerUniqueID", BindingFlags.Static | BindingFlags.NonPublic);
            #endif
        }
        
        // Gets an array of sorting layer names.
        // Since this uses reflection, callers should check for 'null' which will be returned if the reflection fails.
        public static string[] sortingLayerNames
        {
            get
            {
                if (_sortingLayerNamesProperty == null) {
                    Debug.LogError("not found property");
                    return null;
                }
                
                return _sortingLayerNamesProperty.GetValue(null, null) as string[];
            }
        }
        
        // Given the ID of a sorting layer, returns the sorting layer's name
        public static string GetSortingLayerNameFromID(int id)
        {
            string[] names = sortingLayerNames;
            if (names == null) {
                return null;
            }
            
            for (int i = 0; i < names.Length; i++) {
                if (GetSortingLayerIDForIndex(i) == id) {
                    return names[i];
                }
            }
            
            return null;
        }
        
        // Given the name of a sorting layer, returns the ID.
        public static int GetSortingLayerIDForName(string name)
        {
            string[] names = sortingLayerNames;
            if (names == null) {
                return 0;
            }
            
            return GetSortingLayerIDForIndex(Array.IndexOf(names, name));
        }
        
        // Helper to convert from a sorting layer INDEX to a sorting layer ID. These are not the same thing.
        // IDs are based on the order in which layers were created and do not change when reordering the layers.
        // Thankfully there is a private helper we can call to get the ID for a layer given its index.
        public static int GetSortingLayerIDForIndex(int index)
        {
            if (_getSortingLayerUserIdMethod == null) {
                Debug.LogError("not found method");
                return 0;
            }
            
            return (int)_getSortingLayerUserIdMethod.Invoke(null, new object[] { index });
        }
    }
}
