using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SLA;

namespace TB.Battles
{
    public class TetrisPlayer : MonoBehaviour
    {
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
            var pivotPlace = TetrisBlock.PivotPlace();
            for(int i = 0; i < TetrisBlock.Size; i++)
            {
                for(int j = 0; j < TetrisBlock.Size; j++)
                {
                    var blockPlace = new Point2(j, i);
                    var fieldPlace = tetrisBlockPlace - pivotPlace + new Point2(j, i + 1);
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
            tetrisBlockPlace += new Point2(0, 1);
            TetrisBlock.transform.localPosition = field.ComputePosition(tetrisBlockPlace);
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
