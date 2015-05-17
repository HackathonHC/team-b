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
                if (_instance == null)
                {
                    _instance = Resources.Load<Resource>("Resource");
                    _instance.Initialize();
                }
                return _instance;
            }
        }

        [SerializeField]
        GameObject _destroyBlockEffect;

        [SerializeField]
        GameObject _sparksEffectPrefab;

        void Initialize()
        {
            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.CreateSparksEffect] = (values) =>
            {
                var pos = (Vector3)values[0];
                Instantiate(_sparksEffectPrefab, pos, Quaternion.identity);
            };
            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.CreateDestroyBlockEffect] = (values) =>
            {
                var pos = (Vector3)values[0];
                Instantiate(_destroyBlockEffect, pos, Quaternion.identity);
            };
        }

        public void CreateDestroyBlockEffect(Vector3 pos)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.CreateDestroyBlockEffect, pos);
        }

        public void CreateSparksEffect(Vector3 pos)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.CreateSparksEffect, pos);
        }
    }
}
