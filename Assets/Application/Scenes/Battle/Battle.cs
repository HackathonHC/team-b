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

            if (GameData.Instance.playerType == PlayerType.Digger || Consts.Standalone)
            {
                // FIXME: magic number position
                PhotonNetwork.Instantiate("PhotonViews/Bomber", new Vector3(0.5f, 25.5f, 0f), Quaternion.identity, 0);
            }
            if (GameData.Instance.playerType == PlayerType.Tetris || Consts.Standalone)
            {
                var field = Instantiate(_fieldPrefab).GetComponent<Field>();
                field.Initialize(10, 50, 5, 1);
                var tetrisPlayer = Instantiate(_tetrisPlayerPrefab).GetComponent<TetrisPlayer>();
                tetrisPlayer.Initialize(field, 1);
            }

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
