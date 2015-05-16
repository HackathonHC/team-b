using UnityEngine;
using System.Collections;

namespace TB.Titles
{
    public class Title : MonoBehaviour
    {
        public void OnClickStartDrillerModeButton()
        {
            MatchingParameter.Instance.playerType = PlayerType.Digger;
            Application.LoadLevel("Matching");
        }

        public void OnClickStartTetrisModeButton()
        {
            MatchingParameter.Instance.playerType = PlayerType.Tetris;
            Application.LoadLevel("Matching");
        }
    }
}
