using UnityEngine;
using System.Collections;
using SLA;

namespace TB.Battles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        public BlockType Type = BlockType.Normal;
        public Point2 Place {get; set;}

        public int MaxLife; 

        int _damageCount = 0;

        public int Life
        {
            get
            {
                return MaxLife - _damageCount;
            }
            set
            {
                _damageCount = MaxLife - value;
            }
        }
    }

    public enum BlockType
    {
        Normal,
        Wall,
    }
}


