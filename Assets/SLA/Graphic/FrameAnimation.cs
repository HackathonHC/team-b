using UnityEngine;
using System.Collections;

namespace SLA
{
    public class FrameAnimation : ScriptableObject
    {
        [System.Serializable]
        public class Frame
        {
            public float duration;
            public Sprite sprite;
        }

    #if UNITY_EDITOR
        public UnityEditor.ModelImporterAnimationType type = UnityEditor.ModelImporterAnimationType.Generic;
    #endif

        public string path;
        public Frame[] frames;
    }
}
