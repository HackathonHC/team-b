using UnityEngine;
using System.Collections;

namespace TB
{
    public class Resource : ScriptableObject
    {
        static Resource _instance;
        static public Resource Instance
        {
            get
            {
                return _instance ?? (_instance = Resources.Load<Resource>("Resource"));
            }
        }
    }
}
