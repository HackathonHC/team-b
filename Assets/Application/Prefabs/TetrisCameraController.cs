using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class TetrisCameraController : MonoBehaviour
    {
        const float Offset = -2f;

        void Start()
        {
            if (GameData.Instance.playerType != PlayerType.Tetris)
            {
                enabled = false;
            }
        }

        void LateUpdate()
        {
            var player = TetrisPlayer.Instance;
            if(player && player.TetrisBlock != null)
            {
                var position = TetrisPlayer.Instance.TetrisBlock.transform.position + new Vector3(0, Offset, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, position.y, transform.localPosition.z);
            }
        }
    }
}
