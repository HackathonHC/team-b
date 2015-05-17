﻿using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Bomber : MonoBehaviour
    {
        public enum StateType
        {
            Active = 1,
            Digging = 2,
        }

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
        
        Animator _animator;
        Animator Animator
        {
            get
            {
                return _animator ?? (_animator = GetComponent<Animator>());
            }
        }

        const float Velocity = 1f;
        const float DestroyBlockDelay = 1f;

        [SerializeField]
        Transform _blaster;

        [SerializeField]
        Transform _bottomBlaster;

        [SerializeField]
        Transform _flipper;

        public StateType State {get; protected set;}

        const float AirMax = 30f;

        string _animatonName;

        Air _remainingAir = new Air();

        bool _died = false;

        static Bomber _instance;
        public static Bomber Instance
        {
            get
            {
                return _instance ?? (_instance = FindObjectOfType<Bomber>());
            }
        }

        void Start()
        {
            State = StateType.Active;
            _remainingAir.Value = AirMax;

            SLA.PhotonMessageManager.Instance.OnReceivedEvents[(int)PhotonEvent.PlayBomberAnimation] = (values) =>
            {
                var name = global::System.Convert.ToString(values[0]);
                _animatonName = name;
                Animator.Play(_animatonName, 0, 0f);
            };
        }

        void Update()
        {
            // controll
            if (GameData.Instance.playerType == PlayerType.Tetris)
            {
                return;
            }

            Battle.Instance.SetAirGaugeValue(_remainingAir.Value / AirMax);

            if (State == StateType.Digging)
            {
                return;
            }

            if (_died)
            {
                return;
            }

            var horizontalMove = Input.GetAxis("Horizontal");
            float moveAngle = 0f;
            if (Mathf.Abs(horizontalMove) > 0.01f)
            {
                moveAngle = Mathf.Sign(horizontalMove);
                float force = (_remainingAir.Value > 0f) ? 30f : 15f;
                Rigidbody2D.AddForce(new Vector2(moveAngle, 0f) * force * Time.deltaTime, ForceMode2D.Impulse);
            }
            else
            {
                PlayMotion("Idle");
            }

            if (moveAngle > 0.1f)
            {
                PlayMotion("WalkRight");
                _flipper.localScale = new Vector3(-1f, _flipper.localScale.y, _flipper.localScale.z);
            }
            if (moveAngle < -0.1f)
            {
                PlayMotion("WalkLeft");
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
                if (BomberFoot.IsLanding)
                {
                    Transform blaster;
                    string animationName;

                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        blaster = _bottomBlaster;
                        animationName = "Dig";
                    }
                    else
                    {
                        blaster = _blaster;
                        animationName = (_blaster.position.x > this.transform.position.x) ? "DigRight" : "DigLeft";
                    }

                    var col = Physics2D.OverlapPoint(blaster.position, 1 << Consts.BlockLayer);
                    if (col != null)
                    {
                        var block = col.GetComponent<Block>();
                        if (block.Type != BlockType.Wall)
                        {
                            PlayMotion(animationName);
                            StartCoroutine(DestroyBlockCoroutine(blaster.position, block));
                        }
                    }
                }
            }
        }

        IEnumerator DestroyBlockCoroutine(Vector3 effectPosition, Block target)
        {
            State = StateType.Digging;
            Resource.Instance.CreateSparksEffect(effectPosition);
            Battle.Instance.ShakeCamera(0.1f, DestroyBlockDelay);
            yield return new WaitForSeconds(DestroyBlockDelay);

            State = StateType.Active;

            PlayMotion("Idle");
            if (target.Type == BlockType.Wall)
            {
                yield break;
            }
            if (target.Type == BlockType.Unbreakable)
            {
                yield break;
            }

            Resource.Instance.CreateDestroyBlockEffect(effectPosition);
            target.Life -= 1;
            if (target.Life <= 0)
            {
                Battle.Instance.DestroyBlock(target);
                Battle.Instance.ShakeCamera(0.25f, 0.5f);
            }
        }

        void OnCollisionStay2D(Collision2D col)
        {
            var trigger = col.collider.GetComponent<BlockAttackTrigger>();
            if (trigger && trigger.Attacking)
            {
                Battle.Instance.TryOver(ResultType.TetrisWin);
                PlayMotion("Dead");
                _died = true;
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == Consts.ItemLayer)
            {
                Battle.Instance.DestroyItem(col.gameObject.GetComponent<Item>());
                _remainingAir.Value = AirMax;
            }
        }

        public void PlayMotion(string name)
        {
            if (_animatonName != name)
            {
                SLA.PhotonMessageManager.Instance.ServeQueueTo(PhotonTargets.All, (int)PhotonEvent.PlayBomberAnimation, name);
            }
        }
    }
}
