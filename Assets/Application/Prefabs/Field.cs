using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public class Field : MonoBehaviour
    {
        public const int Width = 10;
        public const int TotalHeight = 50;
        public const int TopSpaceHeight = 10;
        public const float BlockUnit = 1f;
        public Point2 CurrentTopCenterPlace {get; private set;}

        List<Block> blocks;

        public void Initialize()
        {
            blocks = new List<Block>();
            for(int i = 0; i < (Width + 2) * (TotalHeight + 2); i++)
            {
                blocks.Add(null);
            }

            for(int i = 0; i < TotalHeight + 2; i++)
            {
                // Left
                var leftPlace = new Point2(0, i);
                var rightPlace = new Point2(Width + 1, i);
                CreateBlockAt(leftPlace, BlockType.Wall);
                CreateBlockAt(rightPlace, BlockType.Wall);
            }
            for(int i = TopSpaceHeight + 1; i < TotalHeight + 2; i++)
            {
                var blockExistences = GenerateBlockExisteces(Width);
                for(int j = 1; j <= Width; j++)
                {
                    var place = new Point2(j, i);
                    if (blockExistences[j - 1])
                    {
                        if(IsEmpty(place))
                        {
                            CreateBlockAt(place, Consts.BlockSelector.Select());
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 10) == 0)
                        {
                            var position = ComputePosition(place);
                            Item.Create(position);
                        }
                    }
                }
            }

            CurrentTopCenterPlace = new Point2(Width / 2 + 1, 0);
        }

        static List<bool> GenerateBlockExisteces(int width)
        {
            int emptyBlockCount = Random.Range(1, width - 1) * Random.Range(1, width - 1) / (width - 2);
            emptyBlockCount = Mathf.Max(1, emptyBlockCount);
            emptyBlockCount = width - emptyBlockCount;
            var blockExists = new List<bool>();
            for(int k=0 ; k<width ; ++k)
            {
                blockExists.Add(k < emptyBlockCount);
            }
            SLA.RandomUtil.Shuffle<bool>(ref blockExists);
            return blockExists;
        }

        static public Vector3 ComputePosition(Point2 place)
        {
            return new Vector3(place.x - (float)Width / 2 - 0.5f, - (place.y - (float)TotalHeight / 2 - 0.5f), 0) * BlockUnit;
        }
        
        public static Vector3 InitialCenterPlace
        {
            get
            {
                return ComputePosition(new Point2(Width / 2 + 1, 0));
            }
        }

        public static Vector3 SurfacePosition
        {
            get
            {
                return ComputePosition(new Point2(Width / 2 + 1, TopSpaceHeight));
            }
        }

        public bool IsInField(Point2 place)
        {
            return place.x >= 0 && place.x < Width + 2 && place.y >= 0 && place.y < TotalHeight + 2;
        }

        public bool IsEmpty(Point2 place)
        {
            return GetBlockAt(place) == null;
        }
        
        public Block GetBlockAt(Point2 place)
        {
            if(IsInField(place))
            {
                return blocks[place.y * (Width + 2) + place.x];
            }
            else
            {
                return null;
            }
        }

        public void CreateBlockAt(Point2 place, BlockType type)
        {
            var position = ComputePosition(place);
            switch(type)
            {
            case BlockType.Normal:
                SetBlockAt(place, Block.InstantiateNormalBlock(position).GetComponent<Block>());
                break;
            case BlockType.Wall:
                SetBlockAt(place, Block.InstantiateWallBlock(position).GetComponent<Block>());
                break;
            case BlockType.Unbreakable:
                SetBlockAt(place, Block.InstantiateUnbreakableBlock(position).GetComponent<Block>());
                break;
            case BlockType.Hard:
                SetBlockAt(place, Block.InstantiateHardBlock(position).GetComponent<Block>());
                break;
            default:
                Debug.LogWarning("wrong block type!");
                break;
            }
        }

        public void SetBlockAt(Point2 place, Block block)
        {
            if(IsInField(place))
            {
                if(block != null)
                {
                    block.transform.parent = this.transform;
                    block.transform.localPosition = ComputePosition(place);
                    block.transform.localScale = Vector3.one;
                    block.Place = place;
                }
                blocks[place.y * (Width + 2) + place.x] = block;
            }
        }

        public void RemoveBlockAt(Point2 place)
        {
            if(IsInField(place))
            {
                Battle.Instance.DestroyBlock(GetBlockAt(place));
                SetBlockAt(place, null);
            }
        }

        public List<Block> FindFilledRowBlocks()
        {
            var blocks = new List<Block>();
            for(int i = 1; i <= TotalHeight; i++)
            {
                if(IsFilledRow(i))
                {
                    for(int j = 1; j <= Width; j++)
                    {
                        blocks.Add(GetBlockAt(new Point2(j, i)));
                    }
                }
            }
            return blocks;
        }

        bool IsFilledRow(int height)
        {
            for(int j = 1; j <= Width; j++)
            {
                if(IsEmpty(new Point2(j, height)))
                {
                    return false;
                }
            }
            return true;
        }

        public void RemoveRow(int height)
        {
            for(int i = height - 1; i > 0; i--)
            {
                for(int j = 1; j <= Width; j++)
                {
                    SetBlockAt(new Point2(j, i + 1), GetBlockAt(new Point2(j, i)));
                }
            }
        }
    }
}

