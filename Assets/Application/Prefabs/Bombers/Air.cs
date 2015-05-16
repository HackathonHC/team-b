using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Air
    {
        float _timestamp;
        float _value;

        public float Value
        {
            get
            {
                return _value - (Time.time - _timestamp);
            }
            set
            {
                _value = value;
                _timestamp = Time.time;
            }
        }
    }
}
