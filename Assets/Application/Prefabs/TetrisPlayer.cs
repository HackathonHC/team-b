using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public class TetrisPlayer : MonoBehaviour
    {
        [SerializeField]
        TetrisBlock
            tetrisBlockPrefab;

        Field field;
        TetrisBlock tetrisBlock;
        List<TetrisBlockType> tetrisBlockTypes;
        Point2 tetrisBlockPlace;
        float currentTime;
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
        }

        void CreateTetrisBlock()
        {
            tetrisBlock = Instantiate(tetrisBlockPrefab.gameObject).GetComponent<TetrisBlock>();
            tetrisBlock.Initialize(ChooseTetrisBlockType(), field.BlockUnit);
            tetrisBlockPlace = field.CurrentTopCenterPlace;
            tetrisBlock.transform.localPosition = field.ComputePosition(tetrisBlockPlace);
        }

        TetrisBlockType ChooseTetrisBlockType()
        {
            return (TetrisBlockType)tetrisBlockTypes[UnityEngine.Random.Range(0, tetrisBlockTypes.Count)];
        }

        bool CanGoDown()
        {
            var pivotPlace = tetrisBlock.PivotPlace();
            for(int i = 0; i < TetrisBlock.Size; i++)
            {
                for(int j = 0; j < TetrisBlock.Size; j++)
                {
                    var blockPlace = new Point2(j, i);
                    var fieldPlace = tetrisBlockPlace - pivotPlace + new Point2(j, i + 1);
                    if(tetrisBlock.GetBlockAt(blockPlace) != null && field.GetBlockAt(fieldPlace) != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        void GoDown()
        {
            tetrisBlockPlace += new Point2(0, 1);
            tetrisBlock.transform.localPosition = field.ComputePosition(tetrisBlockPlace);
        }

        void Update()
        {
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
