using UnityEngine;
using System.Collections;

namespace TB.Matchings
{
    public class Matching : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text _log;

        void Start()
        {
            if (GameData.Instance.playerType == PlayerType.Tetris)
            {
                ConnectAsHost();
            }
            else
            {
                ConnectAsGuest();
            }
        }

        void ConnectAsHost()
        {
            _log.text += "connecting photon as host...\n";
            SLA.PhotonManager.Instance.Connect((successConnecting) => {
                if (!successConnecting)
                {
                    Application.LoadLevel("Title");
                }
                _log.text += "connected\n";
                _log.text += "creating room...\n";
                SLA.PhotonManager.Instance.CreateRoom(null, 2, success => {
                    if (success)
                    {
                        _log.text += "waiting for guest player...\n";
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

        void ConnectAsGuest()
        {
            _log.text += "connecting photon as guest...\n";
            SLA.PhotonManager.Instance.Connect((successConnecting) => {
                if (!successConnecting)
                {
                    Application.LoadLevel("Title");
                }
                _log.text += "connected\n";
                _log.text += "joining room...\n";
                SLA.PhotonManager.Instance.JoinRoom(null, 30f, success => {
                    if (success)
                    {
                        _log.text += "wait...\n";
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
