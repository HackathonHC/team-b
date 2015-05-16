using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; 
using System.Collections;

namespace SLA
{
    public class LongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent onLongPress;
        public float interval = 0.2f;

        private float _pointerDownTime;
        private bool _waitingPressed;
        public Vector2 PressedPosition{get; protected set;}

        void Awake()
        {
            _waitingPressed = false;
        }

        void Update()
        {
            if (_waitingPressed && Time.time - _pointerDownTime > interval)
            {
                onLongPress.Invoke();
                _waitingPressed = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _waitingPressed = true;
            _pointerDownTime = Time.time;
            PressedPosition = eventData.position;
        }

        // ドラッグするとOnPointerUpが呼ばれるので、わざわざ移動量を測定する必要はない
        public void OnPointerUp(PointerEventData eventData)
        {
            _waitingPressed = false;
        } 
    }
}
