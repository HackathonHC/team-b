using UnityEngine;
using System.Collections;

namespace SLA
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Impluse2D : MonoBehaviour
    {
        [SerializeField]
        Vector2 force;

        [SerializeField]
        Vector2 relativeForce;

        [SerializeField]
        float torque;

        void Awake()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(force, ForceMode2D.Impulse);
            rigidbody.AddRelativeForce(relativeForce, ForceMode2D.Impulse);
            rigidbody.AddTorque(torque, ForceMode2D.Impulse);
        }
    }
}
