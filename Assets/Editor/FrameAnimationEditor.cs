using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FrameAnimation))]
public class FrameAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Create Animation"))
        {
            var frameAnimation = (FrameAnimation)target;
            string path = GenerateUniqueAssetPath(frameAnimation, "anim");
            
            var obj = CreateAnimationClip(frameAnimation);
            
            obj.name = frameAnimation.name;
            AssetDatabase.CreateAsset(obj, path);

            EditorUtility.SetDirty(obj);
        }
    }
    
    static public string GenerateUniqueAssetPath(Object selectedObject, string extension)
    {
        string objectName = selectedObject.name;
        string dirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedObject));
        string path = string.Format("{0}/{1}." + extension, dirPath, objectName);
        return AssetDatabase.GenerateUniqueAssetPath(path);
    }
    
    public static AnimationClip CreateAnimationClip(FrameAnimation frameAnimation)
    {
        AnimationClip animClip = new AnimationClip();
        
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";
        
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frameAnimation.frames.Length];
        
        for (int i = 0; i < frameAnimation.frames.Length; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameAnimation.frames[i].time;
            keyFrames[i].value = frameAnimation.frames[i].sprite;
        }

        AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);
        
        return animClip;
    }
}
