using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; 
using System.Collections;

namespace SLA
{
    public class DownButton : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent onDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown.Invoke();
        }
    }
}
