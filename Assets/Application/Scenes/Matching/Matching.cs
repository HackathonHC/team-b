using UnityEngine;
using System.Collections;

namespace TB.Matchings
{
    public class Matching : MonoBehaviour
    {
        void Start()
        {
            ConnectAsHost();
        }

        void ConnectAsHost()
        {
            SLA.PhotonManager.Instance.Connect((successConnecting) => {
                if (!successConnecting)
                {
                    Application.LoadLevel("Title");
                }
                SLA.PhotonManager.Instance.CreateRoom(null, 2, success => {
                    if (success)
                    {
                        StartCoroutine(WaitForMatching());
                    }
                    else
                    {
                        SLA.PhotonManager.Instance.Disconnect(null);
                        Application.LoadLevel("Title");
                    }
                });
            });
        }

        void ConnectAsClient()
        {
            SLA.PhotonManager.Instance.Connect((successConnecting) => {
                if (!successConnecting)
                {
                    Application.LoadLevel("Title");
                }
                SLA.PhotonManager.Instance.JoinRoom(null, 30f, success => {
                    if (success)
                    {
                        StartCoroutine(WaitForMatching());
                    }
                    else
                    {
                        Application.LoadLevel("Title");
                    }
                });
            });
        }

        IEnumerator WaitForMatching()
        {
            while(true)
            {
                yield return null;
                if (PhotonNetwork.playerList.Length == 2)
                {
                    Application.LoadLevel("Battle");
                }
            }
        }
    }
}
