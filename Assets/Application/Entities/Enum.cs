using UnityEngine;
using System.Collections;

namespace TB
{
    public enum PlayerType
    {
        Tetris,
        Digger,
    }

    public enum PhotonEvent
    {
        DestroyBlock = 1,
        DestroyItem = 2,
        DestroyDigger = 3,
    }

    public enum ResultType
    {
        TetrisWin = 1,
        DiggerWin = 2,
    }
}
