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
        static readonly int createBlockOffset = 5;

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
        bool isUpdateSuspended = false;

        public void Initialize(Field field, float initialSpeed)
        {
            this.field = field;
            this.speed = initialSpeed;
            tetrisBlockTypes = new List<TetrisBlockType>();
            foreach(var type in Enum.GetValues(typeof(TetrisBlockType)))
            {
                tetrisBlockTypes.Add((TetrisBlockType)type);
            }
            CreateTetrisBlock(true);
            currentTime = 0;
            currentMoveDelay = 0;
        }

        void CreateTetrisBlock(bool firstTime = false)
        {
            TetrisBlock = Instantiate(tetrisBlockPrefab.gameObject).GetComponent<TetrisBlock>();
            TetrisBlock.Initialize(ChooseTetrisBlockType(), Field.BlockUnit);
            if(firstTime)
            {
                tetrisBlockPlace = field.CurrentTopCenterPlace;
            }
            else
            {
                tetrisBlockPlace = new Point2(field.CurrentTopCenterPlace.x, field.FindTopBlockPlace().y - createBlockOffset);
            }
            TetrisBlock.transform.localPosition = Field.ComputePosition(tetrisBlockPlace);
        }

        bool CanCreateTetrisBlock()
        {
            for(int j = 1; j <= Field.Width; j++)
            {
                if(!field.IsEmpty(new Point2(j, 0)))
                {
                    return false;
                }
            }
            return true;
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
            TetrisBlock.transform.localPosition = Field.ComputePosition(tetrisBlockPlace);
        }

        void RotateLeft()
        {
            if(TetrisBlock.CanRotateLeft(field, tetrisBlockPlace))
            {
                TetrisBlock.RotateLeft();
            } else {
                int move = TetrisBlock.CanWallRotateLeft(field, tetrisBlockPlace);
                Debug.Log("RotateLeft move:" + move);
                if ( move != 0) {
                    GoDirection(new Point2(move, 0));
                    TetrisBlock.RotateLeft();
                }
            }
        }

        void RotateRight()
        {
            if(TetrisBlock.CanRotateRight(field, tetrisBlockPlace))
            {
                TetrisBlock.RotateRight();
            } else {
                int move = TetrisBlock.CanWallRotateRight(field, tetrisBlockPlace);
                if ( move != 0) {
                    GoDirection(new Point2(move, 0));
                    TetrisBlock.RotateRight();
                }
            }
        }

        void SettleDown()
        {
            TetrisBlock.SettleDown(field, tetrisBlockPlace);
            Destroy(TetrisBlock);

            var blocks = field.FindFilledRowBlocks();
            if(blocks.Count > 0)
            {
                var removedRowHeights = new List<int>();
                foreach(var block in blocks)
                {
                    if(!removedRowHeights.Contains(block.Place.y))
                    {
                        removedRowHeights.Add(block.Place.y);
                    }
                }

                // remove the same number of rows as fillded rows.
                var extraRemovedRowHeights = field.FindExtraRemovedRowHeights(removedRowHeights.Count);
                extraRemovedRowHeights.RemoveAll((_height) => removedRowHeights.Contains(_height));

                var extraRemovedBlocks = new List<Block>();
                foreach(var height in extraRemovedRowHeights)
                {
                    extraRemovedBlocks.AddRange(field.GetBlocksAtRow(height));
                }
                foreach(var block in extraRemovedBlocks)
                {
                    field.RemoveBlockAt(block.Place);
                }
                foreach(var height in extraRemovedRowHeights)
                {
                    field.RemoveRow(height);
                }

                foreach(var block in blocks)
                {
                    field.RemoveBlockAt(block.Place);
                }
                foreach(var height in removedRowHeights)
                {
                    field.RemoveRow(height);
                }
            }

            isUpdateSuspended = true;
            Invoke("CreateNewBlockOrGameOver", 0.1f);
        }

        void CreateNewBlockOrGameOver()
        {
            if(CanCreateTetrisBlock())
            {
                CreateTetrisBlock();
            }
            else
            {
                Battle.Instance.TryOver(ResultType.DiggerWin);
            }
            isUpdateSuspended = false;
        }

        void Update()
        {
            // controll
            if (GameData.Instance.playerType == PlayerType.Digger)
            {
                return;
            }

            if(Battle.Instance.IsOver)
            {
                return;
            }

            if(isUpdateSuspended)
            {
                return;
            }

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
                else if(Input.GetKey(KeyCode.DownArrow))
                {
                    if(CanGoDown())
                    {
                        GoDown();
                    }
                    else
                    {
                        SettleDown();
                    }
                    currentMoveDelay = moveDelay;
                    currentTime = 0f;
                    return;
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
                    SettleDown();
                }
                currentTime = 0f;
            }
        }
    }
}
