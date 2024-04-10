using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class HitPoint
    {
        public Vector2 position;
        public Vector2 normal;
        public HitPoint(Vector2 position, Vector2 normal)
        {
            this.position = position;
            this.normal = normal;
        }
    }
    internal class Collider
    {
        public Action<HitPoint> collided;
        public Action<HitPoint> notcollided;
        static public List<Collider> ColliderList = new List<Collider>();
        public virtual void CheckCollision(Collider other)
        {
            if (other == null)
                return;

        }
        public virtual void Init(params object[] b)
        {
            Collider.ColliderList.Add(this);
            SetCollider(b);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var collider in ColliderList)
            {
                foreach (var collider2 in ColliderList)
                {
                    if (collider != collider2)
                        collider.CheckCollision(collider2);
                }
            }
        }
        public virtual void SetCollider(params object[] b)
        {

        }

        protected bool IsBoxCollidingWithCircle(List<Vector2> boxPoints, Vector2 circleCenter, float circleRadius, out HitPoint hitPoint)
        {
            hitPoint = null;

            // Check if the circle's center is inside the box
            if (IsPointInsideBox(circleCenter, boxPoints))
            {
                hitPoint = new HitPoint(circleCenter, Vector2.Zero); // Normal not applicable for center inside box
                return true;
            }

            // Check if the circle intersects with any of the box's edges
            for (int i = 0; i < boxPoints.Count; i++)
            {
                Vector2 p1 = boxPoints[i];
                Vector2 p2 = boxPoints[(i + 1) % boxPoints.Count]; // Next point (loop back to the first point)

                float distance = DistancePointToLine(circleCenter, p1, p2);
                if (distance <= circleRadius)
                {
                    // Calculate hit point along the line segment
                    float t = Math.Max(0, Math.Min(1, Vector2.Dot(circleCenter - p1, p2 - p1) / Vector2.DistanceSquared(p2, p1)));
                    Vector2 hitPosition = p1 + t * (p2 - p1);

                    // Calculate collision normal (perpendicular to the edge)
                    Vector2 edge = p2 - p1;
                    Vector2 normal = new Vector2(-edge.Y, edge.X); // Perpendicular to the edge

                    hitPoint = new HitPoint(hitPosition, normal);
                    return true;
                }
            }

            return false;
        }


        

        protected bool AreLinesIntersecting(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
        {
            intersection = Vector2.Zero;

            // Calculate the intersection point of two lines (p1-p2) and (p3-p4)
            float denominator = ((p4.Y - p3.Y) * (p2.X - p1.X)) - ((p4.X - p3.X) * (p2.Y - p1.Y));

            if (denominator == 0)
            {
                // Lines are parallel
                return false;
            }

            float ua = (((p4.X - p3.X) * (p1.Y - p3.Y)) - ((p4.Y - p3.Y) * (p1.X - p3.X))) / denominator;
            float ub = (((p2.X - p1.X) * (p1.Y - p3.Y)) - ((p2.Y - p1.Y) * (p1.X - p3.X))) / denominator;

            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                // Intersection point is within the line segments
                intersection.X = p1.X + (ua * (p2.X - p1.X));
                intersection.Y = p1.Y + (ua * (p2.Y - p1.Y));
                return true;
            }

            return false;
        }

        protected float DistancePointToLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            float l2 = Vector2.DistanceSquared(lineStart, lineEnd);
            // Calculate the distance between a point and a line defined by two endpoints
            if(l2 ==0.0f) {
                return Vector2.DistanceSquared(lineStart, point);
            }
            float t = Math.Max(0, Math.Min(1, Vector2.Dot(point - lineStart, lineEnd - lineStart) / Vector2.DistanceSquared(lineEnd, lineStart)));
            Vector2 hitPosition = lineStart + t * (lineEnd - lineStart);
            float distance = Vector2.Distance(hitPosition, point);
            return distance;
        }
        protected bool IsPointInsideBox(Vector2 point, List<Vector2> boxPoints)
        {
            float minX = boxPoints.Min(p => p.X);
            float maxX = boxPoints.Max(p => p.X);
            float minY = boxPoints.Min(p => p.Y);
            float maxY = boxPoints.Max(p => p.Y);

            return (point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY);
        }
    }
}
