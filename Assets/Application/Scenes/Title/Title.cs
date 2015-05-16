using UnityEngine;
using System.Collections;

namespace TB.Titles
{
    public class Title : MonoBehaviour
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
