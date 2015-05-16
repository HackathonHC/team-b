using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class Interpolate3
    {
        AnimationCurve mPositionX = new AnimationCurve();
        AnimationCurve mPositionY = new AnimationCurve();
        AnimationCurve mPositionZ = new AnimationCurve();

        public void Add(float time, Vector3 pos)
        {
            mPositionX.AddKey(new Keyframe(time, pos.x));
            mPositionY.AddKey(new Keyframe(time, pos.y));
            mPositionZ.AddKey(new Keyframe(time, pos.z));
        }
        
        public Vector3 Evaluate(float t)
        {
            return new Vector3(
                mPositionX.Evaluate(t),
                mPositionY.Evaluate(t),
                mPositionZ.Evaluate(t));
        }
        
        public float TimeLength()
        {
            return mPositionX.keys[mPositionX.length - 1].time;
        }
    }
}
