﻿using UnityEngine;
using System.Collections;
using SLA;

namespace TB.Battles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        public BlockType Type = BlockType.Normal;
        public Point2 Place {get; set;}
    }

    public enum BlockType
    {
        Normal,
        Wall,
    }
}