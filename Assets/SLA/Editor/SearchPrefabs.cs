using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class SearchPrefabs : ScriptableWizard
    {
        public MonoScript searchScript;

        [MenuItem ("SLA/SearchPrefabs")]
        static void FindMenuItem()
        {
            ScriptableWizard.DisplayWizard("Search", typeof(SearchPrefabs), "Search", "");
        }

        void OnWizardCreate()
        {
            var assets = AssetDatabase.FindAssets("t:GameObject");

            var foundItems = new List<Object>();
            foreach (var asset in assets)
            {
                var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof(GameObject));
                if (CheckIfPrefabContainsComponent((GameObject)obj, searchScript.GetClass()))
                {
                    foundItems.Add(obj);
                }
            }

            if (foundItems.Count == 0)
            {
                Debug.Log(  "No active objects were found with attached " +
                          "component \"" + searchScript.GetClass().ToString() + "\"");
            }
            else
            {
                EditorWindow.GetWindow<SearchPrefabsResult>().objects = foundItems.ToArray();
            }
        }

        void OnWizardUpdate()
        {
            if (searchScript == null)
            {
                errorString = "Enter a search and push enter";
                isValid = false;
            }
            else if (searchScript.GetClass() == null || !searchScript.GetClass().IsSubclassOf(typeof(MonoBehaviour)))
            {
                errorString = "script is must be monobehaviour";
                isValid = false;
            }
            else
            {
                errorString = "";
                isValid = true;
            }
        }

        static bool CheckIfPrefabContainsComponent(GameObject prefab, System.Type component)
        {
            if (prefab.GetComponent(component))
            {
                return true;
            }

            // PrefabだとGetComponentInChildrenは使えない。
            foreach(Transform child in prefab.transform)
            {
                if (CheckIfPrefabContainsComponent(child.gameObject, component))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
