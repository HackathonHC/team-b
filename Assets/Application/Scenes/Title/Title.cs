using UnityEngine;
using System.Collections;

namespace TB.Titles
{
    public class Title : MonoBehaviour
    {
        public void OnClickStartDrillerModeButton()
        {
            Application.LoadLevel("Matching");
        }

        public void OnClickStartTetrisModeButton()
        {
            Application.LoadLevel("Matching");
        }
    }
}
