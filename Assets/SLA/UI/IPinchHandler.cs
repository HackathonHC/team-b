using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace SLA
{
    public interface IPinchHandler
    {
        void OnPinch(PinchEventData ped);
        void OnDrag(PointerEventData ped);
    }
}
