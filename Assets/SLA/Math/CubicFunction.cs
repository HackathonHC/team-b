using UnityEngine;
using System.Collections;

namespace SLA
{
    public class CubicFunction
    {
        public float a;
        public float b;
        public float c;
        public float d;

        public float Evaluate(float x)
        {
            return a * x * x * x+ b * x * x + c * x + d;
        }
        
        public float Velocity(float x)
        {
            return 3f * a * x + 2f * b * x + c;
        }

        static public CubicFunction Process3PointsAndVelocity(Vector2 p, Vector2 q, Vector2 r, Vector2 v)
        {
            var matrix = new Matrix4x4();
            matrix[0, 0] = p.x * p.x * p.x;
            matrix[0, 1] = p.x * p.x;
            matrix[0, 2] = p.x;
            matrix[0, 3] = 1f;
            matrix[1, 0] = q.x * q.x * q.x;
            matrix[1, 1] = q.x * q.x;
            matrix[1, 2] = q.x;
            matrix[1, 3] = 1f;
            matrix[2, 0] = r.x * r.x * r.x;
            matrix[2, 1] = r.x * r.x;
            matrix[2, 2] = r.x;
            matrix[2, 3] = 1f;
            matrix[3, 0] = 3f * v.x * v.x;
            matrix[3, 1] = 2f * v.x;
            matrix[3, 2] = 1f;
            matrix[3, 3] = 0f;
            
            Vector4 rightMatrix = new Vector4(p.y, q.y, r.y, v.y);
            var inversedMatrix = matrix.inverse;
            
            var result = new CubicFunction();
            result.a = Vector4.Dot(inversedMatrix.GetRow(0), rightMatrix);
            result.b = Vector4.Dot(inversedMatrix.GetRow(1), rightMatrix);
            result.c = Vector4.Dot(inversedMatrix.GetRow(2), rightMatrix);
            result.d = Vector4.Dot(inversedMatrix.GetRow(3), rightMatrix);

            return result;
        }


        static public CubicFunction Process2PointsAnd2Velocities(Vector2 p, Vector2 q, Vector2 u, Vector2 v)
        {
            var matrix = new Matrix4x4();
            matrix[0, 0] = p.x * p.x * p.x;
            matrix[0, 1] = p.x * p.x;
            matrix[0, 2] = p.x;
            matrix[0, 3] = 1f;

            matrix[1, 0] = q.x * q.x * q.x;
            matrix[1, 1] = q.x * q.x;
            matrix[1, 2] = q.x;
            matrix[1, 3] = 1f;

            matrix[2, 0] = 3f * u.x * u.x;
            matrix[2, 1] = 2f * u.x;
            matrix[2, 2] = 1f;
            matrix[2, 3] = 0f;

            matrix[3, 0] = 3f * v.x * v.x;
            matrix[3, 1] = 2f * v.x;
            matrix[3, 2] = 1f;
            matrix[3, 3] = 0f;
            
            Vector4 rightMatrix = new Vector4(p.y, q.y, u.y, v.y);
            var inversedMatrix = matrix.inverse;
            
            var result = new CubicFunction();
            result.a = Vector4.Dot(inversedMatrix.GetRow(0), rightMatrix);
            result.b = Vector4.Dot(inversedMatrix.GetRow(1), rightMatrix);
            result.c = Vector4.Dot(inversedMatrix.GetRow(2), rightMatrix);
            result.d = Vector4.Dot(inversedMatrix.GetRow(3), rightMatrix);
            
            return result;
        }
    }
}
