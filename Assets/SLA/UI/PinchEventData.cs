using UnityEngine;
using System.Collections;

namespace SLA
{
    public class PinchEventData
    {
        readonly Pinch _handler;
        public PinchEventData(Pinch handler, float delta)
        {
            _handler = handler;
            DeltaFingerDistance = delta;
        }
        
        public float DeltaFingerDistance{get; protected set;}
        public int FingerCount{
            get
            {
                return _handler.FingersCount;
            }
        }
    }
}
