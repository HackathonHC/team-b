using UnityEngine;
using System.Collections;
using SLA;

namespace TB.Battles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Block : MonoBehaviour
    {
        SpriteRenderer _spriteRdnerer;
        SpriteRenderer SpriteRenderer
        {
            get
            {
                return _spriteRdnerer ?? (_spriteRdnerer = GetComponent<SpriteRenderer>());
            }
        }

        public BlockType Type = BlockType.Normal;
        public Point2 Place {get; set;}

        public int MaxLife; 

        int _damageCount = 0;

        [SerializeField]
        Sprite[] _sprites;

        void Start()
        {
            if (_sprites != null && _sprites.Length > 0)
            {
                SpriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            }
        }

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
        public static GameObject InstantiateBottomBlock(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/BottomBlock", position, Quaternion.identity, 0);
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

        public static GameObject InstantiateBombBlock(Vector3 position)
        {
            return PhotonNetwork.Instantiate("PhotonViews/Blocks/BombBlock", position, Quaternion.identity, 0);
        }
    }

    public enum BlockType
    {
        Normal,
        Wall,
        Unbreakable,
        Hard,
        Bomb,
        Bottom,
    }
}


