using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class BlockAttackTrigger : MonoBehaviour
    {
        Vector3 _previousPosition;
        SLA.FixedTimer _attacking = new SLA.FixedTimer();
        const float Duration = 0.03f;

        void FixedUpdate()
        {
            if (_previousPosition != transform.position)
            {
                _previousPosition = transform.position;
                _attacking.Enable(Duration);
            }
        }

        public bool Attacking
        {
            get
            {
                return _attacking.IsActive();
            }
        }
    }
}
