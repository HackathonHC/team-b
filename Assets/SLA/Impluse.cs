using UnityEngine;
using System.Collections;

namespace SLA
{
    [RequireComponent(typeof(Rigidbody))]
    public class Impluse : MonoBehaviour
    {
        [SerializeField]
        Vector3 force;

        [SerializeField]
        Vector3 relativeForce;

        [SerializeField]
        Vector3 torque;

        [SerializeField]
        Vector3 relativeTorque;

        void Awake()
        {
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(force, ForceMode.Impulse);
            rigidbody.AddRelativeForce(relativeForce, ForceMode.Impulse);
            rigidbody.AddTorque(torque, ForceMode.Impulse);
            rigidbody.AddRelativeTorque(relativeTorque, ForceMode.Impulse);
        }
    }
}
