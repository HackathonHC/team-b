using UnityEngine;
using System.Collections;

namespace SLA
{
    public struct Math
    {
        static public int DevideScore(int score, int MaxHP, int fromHP, int toHP)
        {
            int fromHPScore = (MaxHP - fromHP) * score / MaxHP;
            int toHPScore = (MaxHP - toHP) * score / MaxHP;
            
            return toHPScore - fromHPScore;
        }

        static public float Ease(float current, float target, float dt, float movePerSecond)
        {
            float t = Mathf.Pow(1f - movePerSecond, dt);
            return Mathf.Lerp(current, target, 1f - t);
        }

        static public float Distance(Ray ray, Vector3 pos)
        {
            Vector3 a = pos - ray.origin;
            return Vector3.Cross(a, ray.direction).magnitude / ray.direction.magnitude;
        }

        static public Vector3 NearestPoint(Ray ray, Vector3 pos)
        {
            float d = Distance(ray, pos);
            Vector3 a = pos - ray.origin;
            var b = Vector3.Cross(ray.direction, a);
            var c = Vector3.Cross(ray.direction, b);

            if (c.sqrMagnitude == 0f)
            {
                return pos;
            }
            else
            {
                return pos + c.normalized * d;
            }
        }

        static public Quaternion BillboardLookAt(Camera camera, Vector3 direction)
        {
            var cameraAngle = camera.transform.rotation * Vector3.forward;
            var right = Vector3.Cross(direction, cameraAngle);
            var shadowDirection = Vector3.Cross(cameraAngle, right);
            return Quaternion.LookRotation(shadowDirection, cameraAngle);
        }
    }
}

