using UnityEngine;
using System.Collections;

namespace SLA
{
    public static class Physics2DUtil
    {
        // 導出方法は 粘性抵抗があるときの運動 を参照
        // http://spinman.phys.se.tmu.ac.jp/Lecture/Mech/Viscosity/Viscosity.html

        // 速度が小さい時、誤差が大きくなるので注意
        static public float GetMoveLengthExpectation(float velocity, float drag)
        {
            return (velocity - Physics2D.maxLinearCorrection) / drag;
        }

        static public float GetStopTimeExpectation(float velocity, float drag)
        {
            float t0 = 1f / drag;
            return -t0 * Mathf.Log(Physics2D.maxLinearCorrection / velocity);
        }

        static public float GetVelocity(float drag, float moveLength)
        {
            return moveLength * drag + Physics2D.maxLinearCorrection;
        }

        static public float GetDrag(float velocity, float moveLength)
        {
            return (velocity - Physics2D.maxLinearCorrection) / moveLength;
        }


        static public void AddVelocityAngle(Rigidbody2D target, float diffAngle)
        {
            var fromV = target.velocity;
            var toV = new Vector2(
                fromV.x * Mathf.Cos(diffAngle) - fromV.y * Mathf.Sin(diffAngle),
                fromV.x * Mathf.Sin(diffAngle) + fromV.y * Mathf.Cos(diffAngle));
            var f = (toV - fromV) * target.mass;
            target.AddForce(f, ForceMode2D.Impulse);
        }

    }
}
