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

        [SerializeField]
        GameObject _destroyBlockEffect;

        [SerializeField]
        GameObject _sparksEffectPrefab;

        public void CreateDestroyBlockEffect(Vector3 pos)
        {
            Instantiate(_destroyBlockEffect, pos, Quaternion.identity);
        }
        public void CreateSparksEffect(Vector3 pos)
        {
            Instantiate(_sparksEffectPrefab, pos, Quaternion.identity);
        }
    }
}
