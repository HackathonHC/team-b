using UnityEngine;
using System.Collections;

namespace TB.Titles
{
    public class Title : MonoBehaviour
    {
        public void OnClickStartDrillerModeButton()
        {
            MatchingParameter.Instance.playerType = PlayerType.Guest;
            Application.LoadLevel("Matching");
        }

        public void OnClickStartTetrisModeButton()
        {
            MatchingParameter.Instance.playerType = PlayerType.Host;
            Application.LoadLevel("Matching");
        }
    }
}
