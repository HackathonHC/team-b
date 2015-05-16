using UnityEngine;
using System.Collections;

namespace SLA
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        Camera _targetCamera;

        void LateUpdate()
        {
            if (!_targetCamera)
            {
                _targetCamera = Camera.main;
            }

            if (!_targetCamera)
            {
                this.transform.rotation = _targetCamera.transform.rotation;
            }
        }
    }
}
