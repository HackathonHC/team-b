using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public class TetrisPlayer : MonoBehaviour
    {
        static readonly float moveDelay = 0.1f;

        static TetrisPlayer _instance;
        public static TetrisPlayer Instance
        {
            get
            {
                return _instance ?? (_instance = FindObjectOfType<TetrisPlayer>());
            }
        }

        [SerializeField]
        TetrisBlock
            tetrisBlockPrefab;

        public TetrisBlock TetrisBlock {get; private set;}

        Field field;
        List<TetrisBlockType> tetrisBlockTypes;
        Point2 tetrisBlockPlace;
        float currentTime;
        float currentMoveDelay;
        float speed;

        public void Initialize(Field field, float initialSpeed)
        {
            this.field = field;
            this.speed = initialSpeed;
            tetrisBlockTypes = new List<TetrisBlockType>();
            foreach(var type in Enum.GetValues(typeof(TetrisBlockType)))
            {
                tetrisBlockTypes.Add((TetrisBlockType)type);
            }
            CreateTetrisBlock();
            currentTime = 0;
            currentMoveDelay = 0;
        }

        void CreateTetrisBlock()
        {
            TetrisBlock = Instantiate(tetrisBlockPrefab.gameObject).GetComponent<TetrisBlock>();
            TetrisBlock.Initialize(ChooseTetrisBlockType(), field.BlockUnit);
            tetrisBlockPlace = field.CurrentTopCenterPlace;
            TetrisBlock.transform.localPosition = field.ComputePosition(tetrisBlockPlace);
        }

        TetrisBlockType ChooseTetrisBlockType()
        {
            return (TetrisBlockType)tetrisBlockTypes[UnityEngine.Random.Range(0, tetrisBlockTypes.Count)];
        }

        bool CanGoDown()
        {
            return CanGoDireciton(new Point2(0, 1));
        }

        bool CanGoRight()
        {
            return CanGoDireciton(new Point2(1, 0));
        }

        bool CanGoLeft()
        {
            return CanGoDireciton(new Point2(-1, 0));
        }

        bool CanGoDireciton(Point2 direciton)
        {
            var pivotPlace = TetrisBlock.PivotPlace();
            for(int i = 0; i < TetrisBlock.Size; i++)
            {
                for(int j = 0; j < TetrisBlock.Size; j++)
                {
                    var blockPlace = new Point2(j, i);
                    var fieldPlace = tetrisBlockPlace - pivotPlace + new Point2(j, i) + direciton;
                    if(TetrisBlock.GetBlockAt(blockPlace) != null && field.GetBlockAt(fieldPlace) != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        void GoDown()
        {
            GoDirection(new Point2(0, 1));
        }

        void GoRight()
        {
            GoDirection(new Point2(1, 0));
        }

        void GoLeft()
        {
            GoDirection(new Point2(-1, 0));
        }

        void GoDirection(Point2 direction)
        {
            tetrisBlockPlace += direction;
            TetrisBlock.transform.localPosition = field.ComputePosition(tetrisBlockPlace);
        }

        void RotateLeft()
        {
            if(TetrisBlock.CanRotateLeft(field, tetrisBlockPlace))
            {
                TetrisBlock.RotateLeft();
            }
        }

        void RotateRight()
        {
            if(TetrisBlock.CanRotateRight(field, tetrisBlockPlace))
            {
                TetrisBlock.RotateRight();
            }
        }

        void Update()
        {
            if(currentMoveDelay > 0)
            {
                currentMoveDelay -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if(CanGoLeft())
                    {
                        GoLeft();
                        currentMoveDelay = moveDelay;
                    }
                }
                else if(Input.GetKey(KeyCode.RightArrow))
                {
                    if(CanGoRight())
                    {
                        GoRight();
                        currentMoveDelay = moveDelay;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                RotateLeft();
            }
            else if(Input.GetKeyDown(KeyCode.X))
            {
                RotateRight();
            }

            currentTime += Time.deltaTime;

            if(currentTime > 1 / speed)
            {
                if(CanGoDown())
                {
                    GoDown();
                }
                else
                {

                }
                currentTime = 0f;
            }
        }
    }
}
