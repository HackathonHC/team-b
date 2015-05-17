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
        CreateSparksEffect = 4,
        CreateDestroyBlockEffect = 5,
        PlayBomberAnimation = 6,
        FinishBattle = 7,
    }

    public enum ResultType
    {
        TetrisWin = 1,
        DiggerWin = 2,
    }
}
