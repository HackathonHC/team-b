using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TB.Battles
{
    public class BlockPhotonView : MonoBehaviour
    {
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
        }

        Vector3 _servedPosition;

        void Start()
        {
            if (!PhotonView.isMine)
            {
                Destroy(GetComponent<Rigidbody2D>());
            }
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.isWriting)
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
                var header = (HeaderFlags)stream.ReceiveNext();
                if ((header & HeaderFlags.Position) != 0)
                {
                    transform.position = (Vector3)stream.ReceiveNext();
                }
            }
        }
    }
}
