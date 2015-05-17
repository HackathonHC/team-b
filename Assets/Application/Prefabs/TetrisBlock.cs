using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public enum TetrisBlockType
    {
        I,
        J,
        L,
        O,
//        S,
        T,
//        Z,
        One,
        Two,
        Three,
        Bomb,
    }

    public class TetrisBlock : MonoBehaviour
    {
        public static int Size = 4;
        static Dictionary<TetrisBlockType, List<int>> blockDefinisions;

        void Awake()
        {
            blockDefinisions = new Dictionary<TetrisBlockType, List<int>>();
            blockDefinisions.Add(TetrisBlockType.I, new List<int>() {
                0,0,1,0,
                0,0,1,0,
                0,0,1,0,
                0,0,1,0,
            });
            blockDefinisions.Add(TetrisBlockType.J, new List<int>() {
                0,0,1,0,
                0,0,1,0,
                0,1,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.L, new List<int>() {
                0,1,0,0,
                0,1,0,0,
                0,1,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.O, new List<int>() {
                0,0,0,0,
                0,1,1,0,
                0,1,1,0,
                0,0,0,0,
            });
//            blockDefinisions.Add(TetrisBlockType.S, new List<int>() {
//                0,0,0,0,
//                0,0,1,1,
//                0,1,1,0,
//                0,0,0,0,
//            });
            blockDefinisions.Add(TetrisBlockType.T, new List<int>() {
                0,0,0,0,
                0,1,1,1,
                0,0,1,0,
                0,0,0,0,
            });
//            blockDefinisions.Add(TetrisBlockType.Z, new List<int>() {
//                0,0,0,0,
//                0,1,1,0,
//                0,0,1,1,
//                0,0,0,0,
//            });
            blockDefinisions.Add(TetrisBlockType.One, new List<int>() {
                0,0,0,0,
                0,0,0,0,
                0,0,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.Two, new List<int>() {
                0,0,0,0,
                0,0,1,0,
                0,0,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.Three, new List<int>() {
                0,0,0,0,
                0,0,1,0,
                0,1,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.Bomb, new List<int>() {
                0,0,0,0,
                0,0,0,0,
                0,0,1,0,
                0,0,0,0,
            });
        }
        
        public List<Block> Blocks { get; private set; }
        float blockUnit;

        public void Initialize(TetrisBlockType type, float blockUnit)
        {
            Blocks = new List<Block>();
            this.blockUnit = blockUnit;

            var definistion = blockDefinisions[type];
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    if(definistion[i * Size + j] == 1)
                    {
                        var place = new Point2(j, i);
                        var position = ComputePosition(place);
                        var block = InstantiateTetrisBlock(position, type).GetComponent<Block>();
                        block.transform.parent = this.transform;
                        block.transform.localPosition = position;
                        block.transform.localScale = Vector3.one;
                        block.Place = place;
                        Blocks.Add(block);
                    }
                    else
                    {
                        Blocks.Add(null);
                    }
                }
            }
        }

        static GameObject InstantiateTetrisBlock(Vector3 position, TetrisBlockType type)
        {
            if(type == TetrisBlockType.Bomb)
            {
                return PhotonNetwork.Instantiate("PhotonViews/Blocks/BombTetrisBlock", position, Quaternion.identity, 0);
            }
            else
            {
                return PhotonNetwork.Instantiate("PhotonViews/Blocks/TetrisBlock", position, Quaternion.identity, 0);
            }
        }

        public Point2 PivotPlace()
        {
            return new Point2(Size / 2, Size - 1);
        }

        public Vector3 ComputePosition(Point2 place)
        {
            var pivotPlace = PivotPlace();
            return new Vector3(place.x - pivotPlace.x, - (place.y - pivotPlace.y), 0) * blockUnit;
        }

        public Point2 BottomBlockPlace()
        {
            for(int i = Blocks.Count - 1; i > 0; i--)
            {
                if(Blocks[i] != null)
                {
                    return new Point2(i % Size, i / Size);
                }
            }
            return default(Point2);
        }

        public Point2 TopBlockPlace()
        {
            for(int i = 0; i < Blocks.Count; i++)
            {
                if(Blocks[i] != null)
                {
                    return new Point2(i % Size, i / Size);
                }
            }
            return default(Point2);
        }

        public Point2 LeftBlockPlace()
        {
            for(int i = 0; i < Blocks.Count; i++)
            {
                var x = i / Size;
                var y = i % Size;
                if(Blocks[y * Size + x] != null)
                {
                    return new Point2(x, y);
                }
            }
            return default(Point2);
        }

        public Point2 RightBlockPlace()
        {
            for(int i = 0; i < Blocks.Count; i++)
            {
                var x = Size - i / Size - 1;
                var y = i % Size;
                if(Blocks[y * Size + x] != null)
                {
                    return new Point2(x, y);
                }
            }
            return default(Point2);
        }

        public Block GetBlockAt(Point2 place)
        {
            return Blocks[place.y * Size + place.x];
        }

        public bool CanRotateLeft(Field field, Point2 tetrisBlockPlace)
        {
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    var x = Size - i - 1;
                    var y = j;
                    var place = tetrisBlockPlace - PivotPlace() + new Point2(x, y);
                    if(!field.IsEmpty(place))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CanRotateRight(Field field, Point2 tetrisBlockPlace)
        {
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    var x = i;
                    var y = Size - j - 1;
                    var place = tetrisBlockPlace - PivotPlace() + new Point2(x, y);
                    if(!field.IsEmpty(place))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // 壁際チェック右回転
        public int CanWallRotateRight(Field field, Point2 tetrisBlockPlace)
        {
            int move = 0;
            // ブロック3個分までチェック
            for (int k = 0; k < 3; k++) {
                if (tetrisBlockPlace.x <= 5) {
                    move++;
                } else {
                    move--;
                }

                bool hitFlg = false;
                for(int i = 0; i < Size; i++)
                {
                    for(int j = 0; j < Size; j++)
                    {
                        var x = i;
                        var y = Size - j - 1;
                        x += move;
                        var place = tetrisBlockPlace - PivotPlace() + new Point2(x, y);
                        if(!field.IsEmpty(place))
                        {
                            hitFlg = true;
                            break;
                        }
                    }
                }
                if (!hitFlg) {
                    return move;
                }
            }
            return 0;
        }

        // 壁際チェック左回転
        public int CanWallRotateLeft(Field field, Point2 tetrisBlockPlace)
        {
            int move = 0;
            // ブロック3個分までチェック
            for (int k = 0; k < 3; k++) {
                if (tetrisBlockPlace.x <= 5) {
                    move++;
                } else {
                    move--;
                }

                bool hitFlg = false;
                for(int i = 0; i < Size; i++)
                {
                    for(int j = 0; j < Size; j++)
                    {
                        var x = Size - i - 1;
                        var y = j;
                        x += move;
                        var place = tetrisBlockPlace - PivotPlace() + new Point2(x, y);
                        if(!field.IsEmpty(place))
                        {
                            hitFlg = true;
                            break;
                        }
                    }
                }
                if (!hitFlg) {
                    return move;
                }
            }
            return 0;
        }

        public void RotateLeft()
        {
            var tempBlocks = new List<Block>();
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    var x = Size - i - 1;
                    var y = j;
                    tempBlocks.Add(Blocks[y * Size + x]);
                }
            }

            UpdateBlocks(tempBlocks);
        }

        public void RotateRight()
        {
            var tempBlocks = new List<Block>();
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    var x = i;
                    var y = Size - j - 1;
                    tempBlocks.Add(Blocks[y * Size + x]);
                }
            }

            UpdateBlocks(tempBlocks);
        }

        void UpdateBlocks(List<Block> newBlocks)
        {
            for(int i = 0; i < newBlocks.Count; i++)
            {
                Blocks[i] = newBlocks[i];
                if(Blocks[i] != null)
                {
                    var place = new Point2(i % Size, i / Size);
                    Blocks[i].Place = place;
                    Blocks[i].transform.localPosition = ComputePosition(place);
                }
            }
        }

        public void SettleDown(Field field, Point2 tetrisBlockPlace)
        {
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    var block = Blocks[i * Size + j];
                    if(block != null)
                    {
                        var place = tetrisBlockPlace - PivotPlace() + new Point2(j, i);
                        field.SetBlockAt(place, block);
                    }
                }
            }
        }
    }
}
