using UnityEngine;
using System;
using System.Collections;

namespace SLA
{
    [RequireComponent(typeof(Renderer))]
    public class RendererSort : MonoBehaviour
    {
        [SortingLayerName]
        [SerializeField]
        int _sortingLayer;
        
        [SerializeField]
        int _orderInLayer;
        
        void Awake()
        {
            var renderer = GetComponent<Renderer>();
            renderer.sortingOrder = _orderInLayer;
            renderer.sortingLayerID = _sortingLayer;
        }
    }
}
