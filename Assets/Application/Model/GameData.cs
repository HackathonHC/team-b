using UnityEngine;
using System.Collections;

namespace TB
{
    public class GameData
    {
        static GameData _instance;
        public static GameData Instance
        {
            get
            {
                return _instance ?? (_instance = new GameData());
            }
        }

        public PlayerType playerType;

        GameData()
        {
        }
    }
}
