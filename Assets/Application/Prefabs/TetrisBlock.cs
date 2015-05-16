﻿using UnityEngine;
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
        S,
        T,
        Z,
    }

    public class TetrisBlock : MonoBehaviour
    {
        public static int Size = 4;
        static Dictionary<TetrisBlockType, List<int>> blockDefinisions;

        [SerializeField]
        Block
            normalBlockPrefab;

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
            blockDefinisions.Add(TetrisBlockType.S, new List<int>() {
                0,0,0,0,
                0,0,1,1,
                0,1,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.T, new List<int>() {
                0,0,0,0,
                0,1,1,1,
                0,0,1,0,
                0,0,0,0,
            });
            blockDefinisions.Add(TetrisBlockType.Z, new List<int>() {
                0,0,0,0,
                0,1,1,0,
                0,0,1,1,
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
                        var block = Instantiate(normalBlockPrefab).GetComponent<Block>();
                        block.transform.parent = this.transform;
                        block.transform.localPosition = ComputePosition(place);
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
    }
}