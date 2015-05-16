using UnityEngine;
using System.Collections;
using UniLinq;

namespace SLA
{
    public static class PhotonUtil
    {
        public static bool FindPlayerID(int id)
        {
            foreach(var it in PhotonNetwork.playerList)
            {
                if (it.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        static int generateIDPosition = 0;
        
        public static string GenerateID()
        {
            var result = PhotonNetwork.player.ID.ToString("x") + "_" + generateIDPosition.ToString("x");
            ++generateIDPosition;
            return result;
        }

        public static int PlayerIndex
        {
            get
            {
                return SortedPhotonPlayerList
                    .Select(_ => _.ID)
                    .ToList()
                    .IndexOf(PhotonNetwork.player.ID);
            }
        }

        public static PhotonPlayer[] SortedPhotonPlayerList
        {
            get
            {
                return PhotonNetwork.playerList.OrderBy(_ => _.ID).ToArray();
            }
        }
    }
}
