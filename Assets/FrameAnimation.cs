using UnityEngine;
using System.Collections;

public class FrameAnimation : ScriptableObject
{
    [System.Serializable]
    public class Frame
    {
        public float time;
        public Sprite sprite;
    }

    public Frame[] frames;
}
