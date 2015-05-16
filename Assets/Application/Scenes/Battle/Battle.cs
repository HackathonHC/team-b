using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Battle : MonoBehaviour
    {
        static Battle _instance;
        static public Battle Instance{
            get
            {
                return _instance ?? (_instance = FindObjectOfType<Battle>());
            }
        }

        [SerializeField]
        GameObject _tetrisPlayerPrefab;

        [SerializeField]
        GameObject _fieldPrefab;

        void Start()
        {
            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.offlineMode = true;
                PhotonNetwork.CreateRoom("");
            }

            if (GameData.Instance.playerType == PlayerType.Digger)
            {
                PhotonNetwork.Instantiate("PhotonViews/Bomber", Vector3.zero, Quaternion.identity, 0);
            }
            else
            {
                Instantiate(_tetrisPlayerPrefab);
            }

            var fieldObject = Instantiate(_fieldPrefab) as GameObject;
            fieldObject.GetComponent<Field>().Initialize(10, 9, 1);
        }
    }
}
