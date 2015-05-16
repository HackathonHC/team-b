using UnityEngine;
using System.Collections;

namespace TB
{
    public class SystemLauncher : MonoBehaviour
    {
        static bool _launched = false;

        void Start()
        {
            if (!_launched)
            {
                _launched = true;
                Instantiate<GameObject>(Resources.Load<GameObject>("System"));
            }
            Destroy(this.gameObject);
        }
    }
}
