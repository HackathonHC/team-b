using UnityEngine;
using System.Collections;

namespace SLA
{
    public class AudioListener2D : MonoBehaviour
    {
        static public AudioListener2D CurrentInstance{get; private set;}

        public float range;

        void OnEnable()
        {
            CurrentInstance = this;
        }

        void OnDisable()
        {
            if (CurrentInstance == this)
            {
                CurrentInstance = null;
            }
        }
    }
}
