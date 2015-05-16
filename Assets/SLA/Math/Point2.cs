using UnityEngine;
using System.Collections;

namespace SLA
{
    [System.Serializable]
    public struct Point2
    {
        public static readonly  Point2[] directions = new Point2[]{new Point2(1, 0) , new Point2(0, 1), new Point2(-1, 0), new Point2(0, -1)};

        public int x;
        public int y;
        public Point2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        static public Point2 operator+(Point2 a, Point2 b)
        {
            return new Point2(a.x + b.x, a.y + b.y);
        }
        
        public static Point2 operator * (Point2 a, int d)
        {
            return new Point2 (a.x * d, a.y * d);
        }
        
        public static Point2 operator * (int d, Point2 a)
        {
            return new Point2 (a.x * d, a.y * d);
        }
        
        public static Point2 operator - (Point2 a, Point2 b)
        {
            return new Point2 (a.x - b.x, a.y - b.y);
        }
        
        public static Point2 operator - (Point2 a)
        {
            return new Point2 (-a.x, -a.y);
        }

        public static bool operator == (Point2 lhs, Point2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator != (Point2 lhs, Point2 rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }

        public override int GetHashCode ()
        {
            return this.x.GetHashCode () ^ this.y.GetHashCode () << 2;
        }
        
        public static int Dot (Point2 lhs, Point2 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }
        
        public int sqrMagnitude
        {
            get
            {
                return this.x * this.x + this.y * this.y;
            }
        }
        
        public static bool OnOneLine(Point2 a, Point2 b, Point2 c)
        {
            Point2 ab = a - b;
            Point2 cb = c - b;
            
            int dot = Dot(ab, cb);
            return ab.sqrMagnitude * cb.sqrMagnitude == dot * dot;
        }

        public static int RectangularDistance(Point2 a, Point2 b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public override bool Equals (object obj)
        {
            if (obj is Point2)
            {
                return this == (Point2)obj;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", new object[]
            {
                this.x,
                this.y
            });
        }
    }
}
