using UnityEngine;
using System.Collections;

namespace SLA
{
    public class Parabora
    {
        public float a;
        public float b;
        public float c;

        public float Evaluate(float x)
        {
            return a * x * x + b * x + c;
        }

        public float Velocity(float x)
        {
            return 2f * a * x + b;
        }

        public float Center{
            get
            {
                return - b / (2f * a);
            }
        }
        static public Parabora Process3Points(Vector2 p, Vector2 q, Vector2 r)
        {
            var matrix = new Matrix4x4();
            matrix[0, 0] = p.x * p.x;
            matrix[0, 1] = p.x;
            matrix[0, 2] = 1f;
            matrix[0, 3] = 0f;
            matrix[1, 0] = q.x * q.x;
            matrix[1, 1] = q.x;
            matrix[1, 2] = 1f;
            matrix[1, 3] = 0f;
            matrix[2, 0] = r.x * r.x;
            matrix[2, 1] = r.x;
            matrix[2, 2] = 1f;
            matrix[2, 3] = 0f;
            matrix[3, 0] = 0f;
            matrix[3, 1] = 0f;
            matrix[3, 2] = 0f;
            matrix[3, 3] = 1f;
            
            Vector4 rightMatrix = new Vector4(p.y, q.y, r.y, 1f);
            var inversedMatrix = matrix.inverse;

            var result = new Parabora();
            result.a = Vector4.Dot(inversedMatrix.GetRow(0), rightMatrix);
            result.b = Vector4.Dot(inversedMatrix.GetRow(1), rightMatrix);
            result.c = Vector4.Dot(inversedMatrix.GetRow(2), rightMatrix);

            return result;
        }

        static public Parabora Process2PointsAndVelocity(Vector2 p, Vector2 q, Vector2 v)
        {
            var matrix = new Matrix4x4();
            matrix[0, 0] = p.x * p.x;
            matrix[0, 1] = p.x;
            matrix[0, 2] = 1f;
            matrix[0, 3] = 0f;
            matrix[1, 0] = q.x * q.x;
            matrix[1, 1] = q.x;
            matrix[1, 2] = 1f;
            matrix[1, 3] = 0f;

            matrix[2, 0] = 2f * v.x;
            matrix[2, 1] = 1f;
            matrix[2, 2] = 0f;
            matrix[2, 3] = 0f;

            matrix[3, 0] = 0f;
            matrix[3, 1] = 0f;
            matrix[3, 2] = 0f;
            matrix[3, 3] = 1f;
            
            Vector4 rightMatrix = new Vector4(p.y, q.y, v.y, 1f);
            var inversedMatrix = matrix.inverse;
            
            var result = new Parabora();
            result.a = Vector4.Dot(inversedMatrix.GetRow(0), rightMatrix);
            result.b = Vector4.Dot(inversedMatrix.GetRow(1), rightMatrix);
            result.c = Vector4.Dot(inversedMatrix.GetRow(2), rightMatrix);
            
            return result;
        }
    }
}
