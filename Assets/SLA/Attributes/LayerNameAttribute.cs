using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Reflection;
#endif

namespace SLA
{
    public class LayerNameAttribute : PropertyAttribute
    {
        public LayerNameAttribute()
        {
        }
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LayerNameAttribute))]
    public class LayerFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType == typeof(string))
            {
                property.stringValue = LayerMask.LayerToName(EditorGUI.LayerField(position, label, LayerMask.NameToLayer(property.stringValue)));
            }
            else
            {
                property.intValue = EditorGUI.LayerField(position, label, property.intValue);
            }
        }
    }
    #endif
}
