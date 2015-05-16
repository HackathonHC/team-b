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

        public static GameObject InstantiateWallBlock(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/WallBlock", position, Quaternion.identity, 0);
        }
        
        public static GameObject InstantiateNormalBlock(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/NormalBlock", position, Quaternion.identity, 0);
        }
        
        public static GameObject InstantiateHardBlock(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/HardBlock", position, Quaternion.identity, 0);
        }

        public static GameObject InstantiateUnbreakableBlock(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/UnbreakableBlock", position, Quaternion.identity, 0);
        }
    }

    public enum BlockType
    {
        Normal,
        Wall,
        Unbreakable,
        Hard,
    }
}


