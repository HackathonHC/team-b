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
                var field = Instantiate(_fieldPrefab).GetComponent<Field>();
                field.Initialize(10, 9, 1);
                var tetrisPlayer = Instantiate(_tetrisPlayerPrefab).GetComponent<TetrisPlayer>();
                tetrisPlayer.Initialize(field, 5);
                Instantiate(_tetrisPlayerPrefab);
            }

            var fieldObject = Instantiate(_fieldPrefab) as GameObject;
            fieldObject.GetComponent<Field>().Initialize(10, 9, 1);

            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.DestroyBlock] = (values) => 
            {
                int viewID = global::System.Convert.ToInt32(values[0]);
                var view = PhotonView.Find(viewID);
                if (view)
                {
                    Destroy(view.gameObject);
                }
            };
        }

        public void DestroyBlock(Block block)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.DestroyBlock,
                                                           block.GetComponent<PhotonView>().viewID);
        }
    }
}
