using UnityEngine;
using System.Collections;

namespace SLA
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonViewDestroyer : MonoBehaviour
    {
        public void Execute()
        {
            GetComponent<PhotonView>().RPC("DestroyRPC", PhotonTargets.All);
        }

        [RPC]
        void DestroyRPC()
        {
            Destroy(this.gameObject);
        }
    }
}
