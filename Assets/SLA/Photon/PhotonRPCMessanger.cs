using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class PhotonRPCMessanger : PhotonView
    {
        [RPC]
        void SendQueue(int key, object[] values)
        {
            PhotonMessageManager.Instance.OnReceivedQueue(key, values);
        }

        [RPC]
        void SendProperty(int key, object[] values)
        {
            PhotonMessageManager.Instance.OnReceivedProperty(key, values);
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (PhotonNetwork.connected)
                {
                    PhotonNetwork.Disconnect();
                }
            }
        }
    }
}
