using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TB
{
    static public class Consts
    {
        static public readonly int BlockLayer = LayerMask.NameToLayer("Block");
        static public readonly bool Standalone = true;

        static public readonly SLA.WeightedRandom<Battles.BlockType> BlockSelector = new SLA.WeightedRandom<Battles.BlockType>(new Dictionary<Battles.BlockType, int>(){
            {Battles.BlockType.Normal, 1},
            {Battles.BlockType.Hard, 1},
            {Battles.BlockType.Unbreakable, 1},
        });
    }
}
