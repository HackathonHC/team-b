using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TB.Battles
{
    public class BomberPhotonView : MonoBehaviour
    {
        Bomber _bomber;
        Bomber Bomber
        {
            get
            {
                return _bomber ?? (_bomber = GetComponent<Bomber>());
            }
        }

        PhotonView _photonView;
        PhotonView PhotonView
        {
            get
            {
                return _photonView ?? (_photonView = GetComponent<PhotonView>());
            }
        }

        [global::System.Flags]
        public enum HeaderFlags
        {
            Position = 1,
            Scale = 1 << 1,
        }

        Vector3 _servedPosition;
        Vector3 _positionWhenReceived;
        Vector3 _receivedPosition;
        float _receivedTime = 0f;
        double _servedTime = 0d;
        double _serveInterval;

        void Start()
        {
            if (!PhotonView.isMine)
            {
                Destroy(GetComponent<Rigidbody2D>());
            }
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if( stream.isWriting)
            {
                HeaderFlags header = 0;
                var serveParameters = new List<object>();
                if (_servedPosition != transform.position)
                {
                    _servedPosition = this.transform.position;
                    header |= HeaderFlags.Position;
                    serveParameters.Add(_servedPosition);
                }

                if (header != 0)
                {
                    stream.SendNext(header);
                    foreach(var it in serveParameters)
                    {
                        stream.SendNext(it);
                    }
                }
            }
            else
            {
                _receivedTime = Time.fixedTime;
                
                if (_servedTime == 0d)
                {
                    _serveInterval = 1d / PhotonNetwork.sendRateOnSerialize;
                }
                else
                {
                    _serveInterval = info.timestamp - _servedTime;
                }
                _servedTime = info.timestamp;
                _positionWhenReceived = transform.position;
                var header = (HeaderFlags)stream.ReceiveNext();
                if ((header & HeaderFlags.Position) != 0)
                {
                    _receivedPosition = (Vector2)stream.ReceiveNext();
                }
            }
        }
        void FixedUpdate()
        {
            if (!PhotonView.isMine && _receivedTime != 0f)
            {
                var elapsedTime = Time.fixedTime - _receivedTime;
                var fraction = elapsedTime / (float)_serveInterval;
                
                transform.position = Vector3.Lerp(_positionWhenReceived, _receivedPosition, fraction);
            }
        }
    }
}
