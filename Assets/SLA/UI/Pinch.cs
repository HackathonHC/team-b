using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace SLA
{
    [RequireComponent(typeof(IPinchHandler))]
    public class Pinch : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
    {
        Dictionary<int, Vector2> _fingerPositions = new Dictionary<int, Vector2>();
        
        IPinchHandler _iPinchHandler;
        IPinchHandler IPinchHandler
        {
            get
            {
                return _iPinchHandler ?? (_iPinchHandler = GetComponent<IPinchHandler>());
            }
        }
        
        #if UNITY_EDITOR
        void Update()
        {
            float delta = Input.mouseScrollDelta.y;
            if (delta != 0f)
            {
                IPinchHandler.OnPinch(new PinchEventData(this, delta * 10f));
            }
        }
        #endif
        
        public void OnPointerDown(PointerEventData ped)
        {
            _fingerPositions[ped.pointerId] = ped.position;
        }
        
        public void OnDrag(PointerEventData ped)
        {
            _fingerPositions[ped.pointerId] = ped.position;
            
            if (_fingerPositions.Count == 2)
            {
                foreach(var pair in _fingerPositions)
                {
                    if (pair.Key != ped.pointerId)
                    {
                        var prev = Vector2.Distance(pair.Value, ped.position - ped.delta);
                        var current = Vector2.Distance(pair.Value, ped.position);
                        var delta = current - prev;
                        IPinchHandler.OnPinch(new PinchEventData(this, delta));
                        return;
                    }
                }
            }
            IPinchHandler.OnDrag(ped);
        }
        
        public void OnPointerExit(PointerEventData ped)
        {
            OnEndDrag(ped);
        }
        
        public void OnEndDrag(PointerEventData ped)
        {
            _fingerPositions.Remove(ped.pointerId);
        }
        
        public int FingersCount
        {
            get
            {
                return _fingerPositions.Count;
            }
        }
    }
}
