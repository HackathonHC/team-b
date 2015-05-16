using UnityEngine;
using System.Collections;

namespace SLA
{
    public class RandomRotation : MonoBehaviour
    {
        void Awake()
        {
            transform.rotation = Random.rotation;
        }
    }
}
