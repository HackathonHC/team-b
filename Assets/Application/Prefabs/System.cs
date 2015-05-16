using UnityEngine;
using System.Collections;

namespace TB
{
    public class System : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
