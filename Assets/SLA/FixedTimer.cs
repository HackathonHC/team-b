using UnityEngine;
using System.Collections;

namespace SLA
{
    public class FixedTimer
    {
        float _willSleepTime = 0f;
        float _enabledTime = 0f;

        public void Enable(float duration)
        {
            _willSleepTime = Mathf.Max(_willSleepTime, Time.fixedTime + duration);
            _enabledTime = Time.fixedTime;
        }

        public bool IsActive()
        {
            return Time.fixedTime < _willSleepTime;
        }

        public float Duration
        {
            get
            {
                return _willSleepTime - _enabledTime;
            }
        }

        public float RemainingTime
        {
            get
            {
                return Mathf.Clamp(_willSleepTime - Time.fixedTime, 0f, Duration);
            }
        }

        public float NormalizedRemainingTime
        {
            get
            {
                if (Duration == 0f)
                {
                    return 0f;
                }
                return Mathf.Clamp01(RemainingTime / Duration);
            }
        }

        public float PassedTime
        {
            get
            {
                return Time.fixedTime - _enabledTime;
            }
        }

        public float NormalizedPassedTime
        {
            get
            {
                if (Duration == 0f)
                {
                    return 1f;
                }
                return Mathf.Clamp01(PassedTime / Duration);
            }
        }
    }
}
