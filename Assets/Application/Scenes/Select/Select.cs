using UnityEngine;
using System.Collections;

namespace TB.Selects
{
    public class Select : MonoBehaviour
    {
        public void OnClickStartDrillerModeButton()
        {
            GameData.Instance.playerType = PlayerType.Digger;
            Application.LoadLevel("Matching");
        }

        public void OnClickStartTetrisModeButton()
        {
            GameData.Instance.playerType = PlayerType.Tetris;
            Application.LoadLevel("Matching");
        }
    }
}
