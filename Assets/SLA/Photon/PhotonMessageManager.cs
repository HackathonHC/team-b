using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class PhotonMessageManager
    {
        static PhotonMessageManager _instance;
        public static PhotonMessageManager Instance
        {
            get
            {
                return _instance ?? (_instance = new PhotonMessageManager());
            }
        }

        Dictionary<int, object[]> _receivedProperties = new Dictionary<int, object[]>();
        Dictionary<int, Queue<object[]>> _receivedQueues = new Dictionary<int, Queue<object[]>>();

        public Dictionary<int, System.Action<object[]>> OnReceivedEvents{get; protected set;}

        PhotonRPCMessanger _controller;

        PhotonMessageManager()
        {
            OnReceivedEvents = new Dictionary<int, System.Action<object[]>>();
        }

        public object[] PopQueue(int key)
        {
            Queue<object[]> queue;
            if (_receivedQueues.TryGetValue(key, out queue))
            {
                if (queue.Count > 0)
                {
                    return queue.Dequeue();
                }
            }
            return null;
        }
        
        public object[] GetProperty(int key)
        {
            object[] result = null;
            if (_receivedProperties.TryGetValue(key, out result))
            {
                return result;
            }
            return null;
        }
        
        public object[] PopProperty(int key)
        {
            object[] result = null;
            if (_receivedProperties.TryGetValue(key, out result))
            {
                _receivedProperties.Remove(key);
                return result;
            }
            return null;
        }

        public void ServeQueue(int key, params object[] values)
        {
            TryCreateController();
            if (_controller)
            {
                _controller.RPC("SendQueue", PhotonTargets.Others, key, values);
            }
        }

        public void ServeQueueTo(PhotonTargets targets, int key, params object[] values)
        {
            TryCreateController();
            if (_controller)
            {
                _controller.RPC("SendQueue", targets, key, values);
            }
        }

        public void ServeQueueTo(PhotonPlayer target, int key, params object[] values)
        {
            TryCreateController();
            if (_controller)
            {
                _controller.RPC("SendQueue", target, key, values);
            }
        }

        public void ServeProperty(int key, params object[] values)
        {
            TryCreateController();
            if (_controller)
            {
                _controller.RPC("SendProperty", PhotonTargets.Others, key, values);
            }
        }

        public void ServePropertyTo(PhotonTargets targets, int key, params object[] values)
        {
            TryCreateController();
            if (_controller)
            {
                _controller.RPC("SendProperty", targets, key, values);
            }
        }

        public void ServePropertyTo(PhotonPlayer target, int key, params object[] values)
        {
            TryCreateController();
            if (_controller)
            {
                _controller.RPC("SendProperty", target, key, values);
            }
        }

        public void OnReceivedProperty(int key, object[] values)
        {
            _receivedProperties[key] = values;

            System.Action<object[]> e = null;
            if (OnReceivedEvents.TryGetValue(key, out e))
            {
                e.Invoke(values);
            }
        }

        public void OnReceivedQueue(int key, object[] values)
        {
            System.Action<object[]> e = null;
            if (OnReceivedEvents.TryGetValue(key, out e))
            {
                e.Invoke(values);
                return;
            }

            Queue<object[]> queue;
            if (!_receivedQueues.TryGetValue(key, out queue))
            {
                queue = new Queue<object[]>();
                _receivedQueues.Add(key, queue);
            }
            queue.Enqueue(values);
        }
        
        public void TryCreateController()
        {
            if (PhotonNetwork.connected || PhotonNetwork.offlineMode)
            {
                if (!_controller)
                {
                    var obj = (GameObject)PhotonNetwork.Instantiate(typeof(PhotonRPCMessanger).Name, Vector3.zero, Quaternion.identity, 0);
                    _controller = obj.GetComponent<PhotonRPCMessanger>();
                }
            }
            else
            {
                if (_controller)
                {
                    Object.Destroy(_controller.gameObject);
                    _controller = null;
                }
            }
        }
    }
}
