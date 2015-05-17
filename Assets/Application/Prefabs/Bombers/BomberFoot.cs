using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class BomberFoot : MonoBehaviour
    {
        public bool IsLanding{get; protected set;}

        bool nextState = false;

        void LateUpdate()
        {
            IsLanding = nextState;
            nextState = false;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            nextState = true;

            if (col.tag == "BottomBlock")
            {
                Battle.Instance.TryOver(ResultType.DiggerWin);
            }
        }

        void OnTriggerStay2D(Collider2D col)
        {
            nextState = true;
        }
    }
}

