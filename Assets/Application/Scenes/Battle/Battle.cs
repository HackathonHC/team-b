﻿using UnityEngine;
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

        [SerializeField]
        Gauge _airGauge;

        [SerializeField]
        GameObject _cameraController;

        [SerializeField]
        Result _result;

        [SerializeField]
        Transform _background;

        [SerializeField]
        Transform _backgroundBottom;

        void Start()
        {
            Resource.Instance.Initialize();

            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.offlineMode = true;
                PhotonNetwork.CreateRoom("");
            }

            if (GameData.Instance.playerType == PlayerType.Digger || (Consts.Standalone && PhotonNetwork.offlineMode))
            {
                PhotonNetwork.Instantiate("PhotonViews/Bomber", Field.SurfacePosition + new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
            }
            if (GameData.Instance.playerType == PlayerType.Tetris || (Consts.Standalone && PhotonNetwork.offlineMode))
            {
                Field field = null;;
                field = Instantiate(_fieldPrefab).GetComponent<Field>();
                field.Initialize();
                var tetrisPlayer = Instantiate(_tetrisPlayerPrefab).GetComponent<TetrisPlayer>();
                tetrisPlayer.Initialize(field, 1);
            }

            _background.transform.position = Field.SurfacePosition;

            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.DestroyBlock] = (values) => 
            {
                int viewID = global::System.Convert.ToInt32(values[0]);
                var view = PhotonView.Find(viewID);
                if (view)
                {
                    Destroy(view.gameObject);
                }
            };
            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.DestroyItem] = (values) => 
            {
                int viewID = global::System.Convert.ToInt32(values[0]);
                var view = PhotonView.Find(viewID);
                if (view)
                {
                    Destroy(view.gameObject);
                }
            };
            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.DestroyDigger] = (values) => 
            {
                int viewID = global::System.Convert.ToInt32(values[0]);
                var view = PhotonView.Find(viewID);
                if (view)
                {
                    Destroy(view.gameObject);
                }
            };
            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.FinishBattle] = (values) => 
            {
                var result = (ResultType)values[0];
                _result.TrySet(result);
            };
        }

        public void DestroyBlock(Block block)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.DestroyBlock,
                                                           block.GetComponent<PhotonView>().viewID);
        }

        public void DestroyItem(Item item)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.DestroyItem,
                                                           item.GetComponent<PhotonView>().viewID);
        }

        public void DestroyDigger(Bomber digger)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.DestroyDigger,
                                                           digger.GetComponent<PhotonView>().viewID);
        }

        public void SetAirGaugeValue(float v)
        {
            _airGauge.SetValue(v);
        }

        public void ShakeCamera(float amount, float duration)
        {
            iTween.ShakePosition(_cameraController, Vector3.one * amount, duration);
        }

        public void TryOver(ResultType result)
        {
            SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.FinishBattle, (int)result);
        }

        public bool IsOver
        {
            get
            {
                return _result.IsOver;
            }
        }

        public float BackgroundBottom
        {
            get
            {
                return _backgroundBottom.position.y;
            }
        }
    }
}
