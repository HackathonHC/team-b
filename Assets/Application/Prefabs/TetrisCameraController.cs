using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class TetrisCameraController : MonoBehaviour
    {
        const float Offset = 1f;

        void Start()
        {
            if (GameData.Instance.playerType != PlayerType.Tetris)
            {
                enabled = false;
            }
        }

        void LateUpdate()
        {
            var position = TetrisPlayer.Instance.TetrisBlock.transform.position + new Vector3(0, Offset, 0);
            transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
        }
    }
}
