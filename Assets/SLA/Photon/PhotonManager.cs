using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class PhotonManager : Photon.PunBehaviour
    {
        static public PhotonManager Instance{get; private set;}

        const string version = "1";

        System.Action<bool> _joinLobbyCompletion;
        System.Action<bool> _joinRoomCompletion;
        System.Action<bool> _disconnectPhotonCompletion;
        Coroutine _findRoomCoroutine;

        public bool IsProcessing
        {
            get
            {
                if (_findRoomCoroutine != null)
                {
                    return true;
                }

                return (PhotonNetwork.connectionStateDetailed == PeerState.Authenticating)
                    || (PhotonNetwork.connectionStateDetailed == PeerState.ConnectingToGameserver)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.ConnectingToMasterserver)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.ConnectingToNameServer)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.Disconnecting)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.DisconnectingFromGameserver)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.DisconnectingFromMasterserver)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.DisconnectingFromNameServer)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.Joining)
                        || (PhotonNetwork.connectionStateDetailed == PeerState.Leaving);
            }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void Connect(System.Action<bool> completion)
        {
            if (IsProcessing)
            {
                Debug.LogWarning("is processing!");
                if (completion != null)
                {
                    completion(false);
                }
                return;
            }
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (completion != null)
                {
                    completion(false);
                }
                return;
            }

            _joinLobbyCompletion = completion;

            PhotonNetwork.offlineMode = false;

            // 通信データのcrcチェック設定。
            // trueでデータの破損が発生する場合に有効だが、重くなる
            // PhotonNetwork.CrcCheckEnabled = false;

            // TODO: リリース版と開発版でマッチングしないようにする
            PhotonNetwork.ConnectUsingSettings(version);
        }

        public void Disconnect(System.Action<bool> completion)
        {
            if (IsProcessing)
            {
                Debug.LogWarning("is processing!");
                if (completion != null)
                {
                    completion(false);
                }
                return;
            }
            _disconnectPhotonCompletion = completion;
            PhotonNetwork.Disconnect();
        }

        public void JoinRoom(Dictionary<string, object> roomCondition, float findingDuration, System.Action<bool> completion)
        {
            if (IsProcessing)
            {
                Debug.LogWarning("is processing!");
                if (completion != null)
                {
                    completion(false);
                }
                return;
            }
            _joinRoomCompletion = completion;
            _findRoomCoroutine = StartCoroutine(JoinRoomCoroutine(roomCondition, findingDuration));
        }

        IEnumerator JoinRoomCoroutine(Dictionary<string, object> roomCondition, float findingDuration)
        {
            float willTimeOutTime = Time.realtimeSinceStartup + findingDuration;
            
            while(true)
            {
                yield return new WaitForSeconds(1f);

                if (!PhotonNetwork.insideLobby)
                {
                    _findRoomCoroutine = null;
                    if (_joinRoomCompletion != null)
                    {
                        var completion = _joinRoomCompletion;
                        _joinRoomCompletion = null;
                        completion(false);
                    }
                    break;
                }
                RoomInfo suggestRoom = null;
                var roomList = PhotonNetwork.GetRoomList();
                foreach(var room in roomList)
                {
                    if (!ValidateConditions(roomCondition, room.customProperties))
                    {
                        continue;
                    }
                    
                    if (room.playerCount < room.maxPlayers)
                    {
                        if (room.open)
                        {
                            if (suggestRoom == null)
                            {
                                suggestRoom = room;
                            }
                            else
                            {
                                if (room.playerCount > suggestRoom.playerCount)
                                {
                                    suggestRoom = room;
                                }
                            }
                        }
                    }
                }
                
                if (suggestRoom != null)
                {
                    _findRoomCoroutine = null;
                    PhotonNetwork.JoinRoom(suggestRoom.name);
                    break;
                }
                else
                {
                    if (Time.realtimeSinceStartup > willTimeOutTime)
                    {
                        _findRoomCoroutine = null;
                        if (_joinRoomCompletion != null)
                        {
                            var completion = _joinRoomCompletion;
                            _joinRoomCompletion = null;
                            completion(false);
                        }
                        break;
                    }
                }
            }
        }

        public void CreateRoom(Dictionary<string, object> roomCondition, int maxPlayers, System.Action<bool> completion)
        {
            if (IsProcessing)
            {
                Debug.LogWarning("is processing!");
                if (completion != null)
                {
                    completion(false);
                }
                return;
            }

            if (!PhotonNetwork.insideLobby && !PhotonNetwork.offlineMode)
            {
                if (completion != null)
                {
                    completion(false);
                }
                return;
            }
            _joinRoomCompletion = completion;

            var type = new TypedLobby();
            var option = new RoomOptions();
            option.maxPlayers = maxPlayers;
            option.customRoomProperties = new ExitGames.Client.Photon.Hashtable(roomCondition.Count);
            foreach(var it in roomCondition)
            {
                option.customRoomProperties.Add(it.Key, it.Value);
            }
            option.customRoomPropertiesForLobby = new string[roomCondition.Count];
            roomCondition.Keys.CopyTo(option.customRoomPropertiesForLobby, 0);
            
            PhotonNetwork.CreateRoom(null, option, type);
        }

        public override void OnConnectedToPhoton()
        {
            Debug.Log("connectedToPhoton");
            Debug.Log("ping : " + PhotonNetwork.GetPing());
            PhotonNetwork.FetchServerTimestamp();
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            // PhotonServerへの接続処理が失敗した場合にコール
            Debug.Log("failedToConnectToPhoton");
            _joinLobbyCompletion = null;
            if (_joinLobbyCompletion != null)
            {
                var completion = _joinLobbyCompletion;
                _joinLobbyCompletion = null;
                completion(false);
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("joindeLobby");
            if (_joinLobbyCompletion != null)
            {
                var completion = _joinLobbyCompletion;
                _joinLobbyCompletion = null;
                completion(true);
            }
        }
        
        public override void OnLeftLobby()
        {
            Debug.Log("leftLobby");
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.Log("disconnected");
            if (_disconnectPhotonCompletion != null)
            {
                var completion = _disconnectPhotonCompletion;
                _disconnectPhotonCompletion = null;
                completion(true);
            }
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("createRoom");
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("createRoomFailed");
            
            if (_joinRoomCompletion != null)
            {
                var completion = _joinRoomCompletion;
                _joinRoomCompletion = null;
                completion(false);
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("joinedRoom: " + PhotonNetwork.room.name);
            PhotonMessageManager.Instance.TryCreateController();
            if (_joinRoomCompletion != null)
            {
                var completion = _joinRoomCompletion;
                _joinRoomCompletion = null;
                completion(true);
            }
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("joinRoomFailed");
            if (_joinRoomCompletion != null)
            {
                var completion = _joinRoomCompletion;
                _joinRoomCompletion = null;
                completion(false);
            }
        }

        public override void OnLeftRoom()
        {
            Debug.Log("leftRoom");
        }

        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            Debug.Log("masterClientSwitched");
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            //  (接続が確立された後に）何らかの原因で接続が失敗した場合にコール
            Debug.Log("connectionFail");
        }

        public bool RoomIsOpen
        {
            set
            {
                if (PhotonNetwork.room != null)
                {
                    PhotonNetwork.room.open = value;
                }
            }
        }

        static public bool ValidateConditions(Dictionary<string, object> conditions, Dictionary<object, object> properties)
        {
            foreach(var it in conditions)
            {
                object value = null;
                if (properties.TryGetValue(it.Key, out value))
                {
                    if (!value.Equals(it.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
