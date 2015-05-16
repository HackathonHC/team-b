using System.IO;
using UnityEditor;
using UnityEngine;

namespace SLA
{
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
            curveBinding.path = frameAnimation.path;
            curveBinding.propertyName = "m_Sprite";
            
            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frameAnimation.frames.Length + 1];

            float time = 0f;
            for (int i = 0; i < frameAnimation.frames.Length; i++)
            {
                keyFrames[i] = new ObjectReferenceKeyframe();
                keyFrames[i].time = time;
                keyFrames[i].value = frameAnimation.frames[i].sprite;

                time += frameAnimation.frames[i].duration;
            }
            keyFrames[keyFrames.Length - 1] = new ObjectReferenceKeyframe();
            keyFrames[keyFrames.Length - 1].time = time;
            keyFrames[keyFrames.Length - 1].value = frameAnimation.frames[frameAnimation.frames.Length - 1].sprite;

            animClip.legacy = (frameAnimation.type == ModelImporterAnimationType.Legacy);
            AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);

            return animClip;
        }
    }
}
