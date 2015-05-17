﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public class Field : MonoBehaviour
    {
        public int Width {get; private set;}
        public int TotalHeight {get; private set;}
        public int TopSpaceHeight {get; private set;}
        public float BlockUnit {get; private set;}
        public Point2 CurrentTopCenterPlace {get; private set;}

        List<Block> blocks;

        public void Initialize(int width, int totalHeight, int topSpaceHeight, float blockUnit)
        {
            Width = width;
            TotalHeight = totalHeight;
            TopSpaceHeight = topSpaceHeight;
            BlockUnit = blockUnit;

            blocks = new List<Block>();
            for(int i = 0; i < (width + 2) * (totalHeight + 2); i++)
            {
                blocks.Add(null);
            }

            for(int i = 0; i < totalHeight + 2; i++)
            {
                // Left
                var leftPlace = new Point2(0, i);
                var rightPlace = new Point2(width + 1, i);
                CreateBlockAt(leftPlace, BlockType.Wall);
                CreateBlockAt(rightPlace, BlockType.Wall);
            }
            for(int i = topSpaceHeight + 1; i < totalHeight + 2; i++)
            {
                var blockExistences = GenerateBlockExisteces(width);
                var unbreakableBlockExistences = GenerateUnbreakableBlockExistences(width, i, blockExistences);
                var selector = GenerateBlockSelector(i);
                for(int j = 1; j <= width; j++)
                {
                    var place = new Point2(j, i);
                    if (blockExistences[j - 1])
                    {
                        if(IsEmpty(place))
                        {
                            if(unbreakableBlockExistences[j - 1])
                            {
                                CreateBlockAt(place, BlockType.Unbreakable);
                            }
                            else
                            {
                                CreateBlockAt(place, selector.Select());
                            }
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

            CurrentTopCenterPlace = new Point2(width / 2 + 1, 0);
        }

        SLA.WeightedRandom<Battles.BlockType> GenerateBlockSelector(int depth)
        {
            int actualDepth = depth - TopSpaceHeight;
            int actualTotalDepth = TotalHeight + 2 - TopSpaceHeight;

            var blockTypeProbMap = new Dictionary<Battles.BlockType, int>();
            blockTypeProbMap.Add(Battles.BlockType.Normal, 100 * (actualTotalDepth - actualDepth) / actualTotalDepth);
            blockTypeProbMap.Add(Battles.BlockType.Hard, 100 * actualDepth / actualTotalDepth);
            return  new SLA.WeightedRandom<Battles.BlockType>(blockTypeProbMap);
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

        List<bool> GenerateUnbreakableBlockExistences(int width, int depth, List<bool> blockExistences)
        {
            var blockExists = new List<bool>();
            for(int i = 0; i < width; i++)
            {
                blockExists.Add(false);
            }
            if(depth < TotalHeight / 2 || depth % 2 == 0)
            {
                return blockExists;
            }

            int leftSpaceIndex = 0;
            int rightSpaceIndex = blockExistences.Count - 1;

            for(int i = 0; i < width; i++)
            {
                if(!blockExistences[i])
                {
                    leftSpaceIndex = i;
                    break;
                }
            }
            for(int i = width - 1; i >= 0; i--)
            {
                if(!blockExistences[i])
                {
                    rightSpaceIndex = i;
                    break;
                }
            }

            if(leftSpaceIndex > width - 1 - rightSpaceIndex)
            {
                int unbreakableCount = Random.Range(0, leftSpaceIndex);
                for(int i = 0; i < unbreakableCount; i++)
                {
                    blockExists[i] = true;
                }
            }
            else
            {
                int unbreakableCount = Random.Range(0, width - 1 - rightSpaceIndex);
                for(int i = 0; i < unbreakableCount; i++)
                {
                    blockExists[width - i - 1] = true;
                }
            }
            return blockExists;
        }

        public Vector3 ComputePosition(Point2 place)
        {
            return new Vector3(place.x - (float)Width / 2 - 0.5f, - (place.y - (float)TotalHeight / 2 - 0.5f), 0) * BlockUnit;
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

