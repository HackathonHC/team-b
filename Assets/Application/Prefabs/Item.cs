using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Item : MonoBehaviour
    {
        PhotonView _photonView;
        PhotonView PhotonView
        {
            get
            {
                return _photonView ?? (_photonView = GetComponent<PhotonView>());
            }
        }

        public static GameObject Create(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Item", position, Quaternion.identity, 0);
        }
        
    }
}
