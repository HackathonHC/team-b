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

        void FixedUpdate()
        {
            var a = Input.GetAxis("Horizontal");
            Rigidbody2D.AddForce(new Vector2(a, 0f), ForceMode2D.Impulse);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (BomberFoot.IsLanding)
                {
                    Rigidbody2D.AddForce(new Vector2(0f, 8f), ForceMode2D.Impulse);
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                // destroy block
            }
        }
    }
}
