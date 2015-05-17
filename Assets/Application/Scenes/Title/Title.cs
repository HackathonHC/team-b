using UnityEngine;
using System.Collections;

namespace TB.Titles
{
    public class Title : MonoBehaviour
    {
        public void OnClickStartButton()
        {
            Application.LoadLevel("Select");
        }
    }
}
