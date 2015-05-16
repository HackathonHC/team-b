using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
// ScriptableObjectをプレハブとして出力する汎用スクリプト
/// </summary>
// <remarks>
// 指定したScriptableObjectをプレハブに変換する。
// 1.Editorフォルダ下にCreateScriptableObjectPrefub.csを配置
// 2.ScriptableObjectのファイルを選択して右クリック→Create ScriptableObjectを選択
// </remarks>
public class CreateScriptableObjectPrefab
{
    readonly static string[] labels = {"Data", "ScriptableObject"};
    
    [MenuItem("Assets/Create ScriptableObject")]
    static void Create()
    {
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("select some scriptable object");
        }
        foreach (Object selectedObject in Selection.objects)
        {
            string path = GetSavePath(selectedObject);
            var monoScript = selectedObject as MonoScript;
            if (monoScript == null)
            {
                Debug.LogWarning("selected object is not script");
                break;
            }

            var obj = ScriptableObject.CreateInstance(monoScript.GetClass());
            if (obj == null)
            {
                Debug.LogWarning("selected object is not scriptable object");
                break;
            }

            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SetLabels(obj, labels);
            EditorUtility.SetDirty(obj);
        }
    }
    static string GetSavePath (Object selectedObject)
    {
        string objectName = selectedObject.name;
        string dirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedObject));
        string path = string.Format("{0}{1}{2}.asset", dirPath, Path.DirectorySeparatorChar, objectName);
        return AssetDatabase.GenerateUniqueAssetPath(path);
    }
}
