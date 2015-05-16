using UnityEngine;
using System.Collections;

namespace SLA
{
    public class TimeEasingValue
    {
        public enum TimeType
        {
            DefaultTime,
            FixedTime,
            RealTime,
        }

        float _fromValue;
        float _toValue;
        float _startedTime;
        float _duration;
        bool _initialized = false;
        TimeType timeType;

        public System.Func<float, float, float, float> EasingFunction{get; set;}

        public TimeEasingValue(TimeType timeType)
        {
            this.timeType = timeType;
            EasingFunction = EasingUtil.easeInQuad;
        }

        public void Ease(float from, float to, float duration)
        {
            JumpTo(from);
            EaseTo(to, duration);
        }

        public void EaseTo(float to, float duration)
        {
            if (!_initialized)
            {
                _initialized = true;
                _fromValue = to;
            }
            else
            {
                _fromValue = Value;
            }
            _toValue = to;
            _duration = duration;
            _startedTime = Now;
        }

        public void JumpTo(float value)
        {
            _initialized = true;
            _fromValue = value;
            _toValue = value;
            _duration = 0f;
        }

        public float Value
        {
            get
            {
                if (_duration == 0f)
                {
                    return _toValue;
                }
                float progress = Progress;
                if (progress >= 1f)
                {
                    return _toValue;
                }
                if (progress <= 0f)
                {
                    return _fromValue;
                }

                return EasingFunction.Invoke(_fromValue, _toValue, progress);
            }
        }

        public bool Completed
        {
            get
            {
                if (_duration == 0f)
                {
                    return true;
                }
                return (Progress >= 1f);
            }
        }

        float Progress
        {
            get
            {
                return Mathf.Clamp01((Now - _startedTime) / _duration);
            }
        }

        float Now
        {
            get
            {
                switch(timeType)
                {
                case TimeType.DefaultTime:
                    return Time.time;
                case TimeType.FixedTime:
                    return Time.fixedTime;
                case TimeType.RealTime:
                    return Time.realtimeSinceStartup;
                default:
                    Debug.LogError("wrong time type");
                    return 0f;
                }
            }
        }
    }
}
