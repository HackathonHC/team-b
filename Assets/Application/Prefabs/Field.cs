using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public class Field : MonoBehaviour
    {
        public int Width {get; private set;}
        public int Height {get; private set;}
        public float BlockUnit {get; private set;}

        List<Block> blocks;

        public void Initialize(int width, int height, float blockUnit)
        {
            Width = width;
            Height = height;
            BlockUnit = blockUnit;

            blocks = new List<Block>();
            for(int i = 0; i < (width + 2) * (height + 2); i++)
            {
                blocks.Add(null);
            }

            for(int i = 0; i < height + 2; i++)
            {
                // Left
                var leftPlace = new Point2(0, i);
                var rightPlace = new Point2(width + 1, i);
                SetBlockAt(leftPlace, InstantiateWallBlock().GetComponent<Block>());
                SetBlockAt(rightPlace, InstantiateWallBlock().GetComponent<Block>());
            }
            for(int j = 0; j < width + 2; j++)
            {
                // Bottom
                var bottomPlace = new Point2(j, height + 1);
                SetBlockAt(bottomPlace, InstantiateNormalBlock().GetComponent<Block>());
            }
        }

        static GameObject InstantiateWallBlock()
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/WallBlock", Vector3.zero, Quaternion.identity, 0);
        }
        
        static GameObject InstantiateNormalBlock()
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/NormalBlock", Vector3.zero, Quaternion.identity, 0);
        }

        public Vector3 ComputePosition(Point2 place)
        {
            return new Vector3(place.x - (float)Width / 2 - 0.5f, - (place.y - (float)Height / 2 - 0.5f), 0) * BlockUnit;
        }

        public bool IsInField(Point2 place)
        {
            return place.x >= 0 && place.x < Width + 2 && place.y >= 0 && place.y < Height + 2;
        }

        public bool IsBlock(Point2 place)
        {
            return GetBlockAt(place) != null;
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

        public void SetBlockAt(Point2 place, Block block)
        {
            if(IsInField(place))
            {
                block.transform.localPosition = ComputePosition(place);
                block.Place = place;
                blocks[place.y * (Width + 2) + place.x] = block;
            }
        }
    }
}

