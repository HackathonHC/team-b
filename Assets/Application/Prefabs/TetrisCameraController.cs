using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class TetrisCameraController : MonoBehaviour
    {
        void Start()
        {
            if (GameData.Instance.playerType != PlayerType.Tetris)
            {
                enabled = false;
            }
        }
    }
}
