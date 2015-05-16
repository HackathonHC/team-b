using UnityEngine;
using System.Collections;

namespace SLA
{
    static public class PhotonPropertyUtil
    {
        public static void SetRoomCustomProperty(object key, object value)
        {
            var table = RoomCustomProperties;
            if (table.ContainsKey(key))
            {
                if (!table[key].Equals(value))
                {
                    table[key] = value;
                    SetRoomCustomProperties(table);
                }
            }
            else
            {
                table.Add(key, value);
                SetRoomCustomProperties(table);
            }
        }
        
        public static void SetRoomCustomProperties(ExitGames.Client.Photon.Hashtable table)
        {
            if (PhotonNetwork.inRoom)
            {
                PhotonNetwork.room.SetCustomProperties(table);
            }
        }
        
        public static void ClearRoomCustomProperties()
        {
            if (PhotonNetwork.inRoom)
            {
                PhotonNetwork.room.customProperties.Clear();
            }
        }
        
        public static ExitGames.Client.Photon.Hashtable RoomCustomProperties
        {
            get
            {
                if (PhotonNetwork.inRoom)
                {
                    return PhotonNetwork.room.customProperties;
                }
                return null;
            }
        }
        
        public static void SetPlayerCustomProperty(object key, object value)
        {
            var table = PlayerCustomProperties;
            if (table.ContainsKey(key))
            {
                if (!table[key].Equals(value))
                {
                    table[key] = value;
                    SetPlayerCustomProperties(table);
                }
            }
            else
            {
                table.Add(key, value);
                SetPlayerCustomProperties(table);
            }
        }
        
        public static void SetPlayerCustomProperties(ExitGames.Client.Photon.Hashtable table)
        {
            PhotonNetwork.player.SetCustomProperties(table);
        }
        
        public static ExitGames.Client.Photon.Hashtable PlayerCustomProperties
        {
            get
            {
                return PhotonNetwork.player.customProperties;
            }
        }

    }
}
