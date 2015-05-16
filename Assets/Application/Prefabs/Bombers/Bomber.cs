using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Bomber : MonoBehaviour
    {
        Rigidbody2D _rigidbody2D;
        public Rigidbody2D Rigidbody2D
        {
            get
            {
                return _rigidbody2D ?? (_rigidbody2D = GetComponent<Rigidbody2D>());
            }
        }

        BomberFoot _bomberFoot;
        public BomberFoot BomberFoot
        {
            get
            {
                return _bomberFoot ?? (_bomberFoot = GetComponentInChildren<BomberFoot>());
            }
        }

        const float Velocity = 1f;

        [SerializeField]
        Transform _blaster;

        [SerializeField]
        Transform _bottomBlaster;

        [SerializeField]
        Transform _flipper;

        static Bomber _instance;
        public static Bomber Instance
        {
            get
            {
                return _instance ?? (_instance = FindObjectOfType<Bomber>());
            }
        }

        void Update()
        {
            // controll
            if (GameData.Instance.playerType == PlayerType.Tetris)
            {
                return;
            }

            var horizontalMove = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontalMove) > 0.01f)
            {
                Rigidbody2D.AddForce(new Vector2(Mathf.Sign(horizontalMove), 0f) * 30f * Time.deltaTime, ForceMode2D.Impulse);
            }
            if (horizontalMove > 0.1f)
            {
                _flipper.localScale = new Vector3(-1f, _flipper.localScale.y, _flipper.localScale.z);
            }
            if (horizontalMove < -0.1f)
            {
                _flipper.localScale = new Vector3(1f, _flipper.localScale.y, _flipper.localScale.z);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (BomberFoot.IsLanding)
                {
                    Rigidbody2D.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Transform blaster;

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    blaster = _bottomBlaster;
                }
                else
                {
                    blaster = _blaster;
                }

                var col = Physics2D.OverlapPoint(blaster.position, 1 << Consts.BlockLayer);
                if (col != null)
                {
                    var block = col.GetComponent<Block>();
                    if (block.Type == BlockType.Normal)
                    {
                        Resource.Instance.CreateDestroyBlockEffect(_bottomBlaster.position);
                        Battle.Instance.DestroyBlock(block);
                    }
                }
            }
        }
        void OnCollisionStay2D(Collision2D col)
        {
            var trigger = col.collider.GetComponent<BlockAttackTrigger>();
            if (trigger && trigger.Attacking)
            {
                // death
                Debug.Log("digger died!");
            }
        }
    }
}
