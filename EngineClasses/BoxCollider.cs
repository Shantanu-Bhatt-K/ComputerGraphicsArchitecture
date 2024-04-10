
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class BoxCollider: Collider
    {

        public List<Vector2> collisionPoints=new List<Vector2>();
        public Vector2 position;

        public override void SetCollider(params object[] b)
        {
            position = (Vector2)b[0];
            float rotation = (float)b[1];
            Vector2 size = (Vector2)b[2];
            Vector2 rotVector= new((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
            Vector2 perpVector = new(-rotVector.Y, rotVector.X);
            collisionPoints.Clear();


            collisionPoints.Add(position + (rotVector * size.Y) + (perpVector * size.X));
            collisionPoints.Add(position - (rotVector * size.Y) + (perpVector * size.X));
            collisionPoints.Add(position - (rotVector * size.Y) - (perpVector * size.X));
            collisionPoints.Add(position + (rotVector * size.Y) - (perpVector * size.X));
        }

        public override void CheckCollision(Collider other)
        {
            
            if (other is BoxCollider boxCollider)
            {
                
                // Check for collision between two box colliders
                bool colliding = AreBoxesColliding(this.collisionPoints, boxCollider.collisionPoints,out HitPoint hitPoint);
                if (colliding)
                {
                    collided?.Invoke(hitPoint);
                    return;
                }
            }
            else if (other is CircleCollider circleCollider)
            {
                bool colliding = IsBoxCollidingWithCircle(this.collisionPoints, circleCollider.position, circleCollider.radius, out HitPoint hitPoint);
                if (colliding)
                {
                    collided?.Invoke(hitPoint);
                    return;
                }
            }
            notcollided?.Invoke(default(HitPoint));

        }
        protected bool AreBoxesColliding(List<Vector2> points1, List<Vector2> points2, out HitPoint hitPoint)
        {
            hitPoint = new(Vector2.Zero, Vector2.Zero);
            
            
            // Check if any of the edges of one box intersect with edges of the other box
            for (int i = 0; i < points1.Count; i++)
            {
                Vector2 p1 = points1[i];
                Vector2 p2 = points1[(i + 1) % points1.Count]; // Next point (loop back to the first point)
                
                for (int j = 0; j < points2.Count; j++)
                {
                    Vector2 p3 = points2[j];
                    Vector2 p4 = points2[(j + 1) % points2.Count]; // Next point (loop back to the first point)
                    
                    if (AreLinesIntersecting(p1, p2, p3, p4, out hitPoint.position))
                    {
                        // Calculate collision normal as the normalized vector perpendicular to the edge
                        Vector2 edge = p2 - p1;
                        hitPoint.normal = new Vector2(-edge.Y, edge.X); // Perpendicular to the edge
                        hitPoint.normal.Normalize();

                        return true;
                    }
                }
                if (IsBoxInsideOther(points1, points2) || IsBoxInsideOther(points2, points1))
                {
                    // One polygon is completely inside the other, consider it a collision
                    // You can set hitPoint.normal to any value appropriate for this case
                    // For example, you can set it to the direction from one polygon's center to the other's
                   
                    hitPoint.normal = Vector2.Zero;
                    return true;
                }
            }


            // No edge intersection found
            return false;
        }

        protected bool IsBoxInsideOther(List<Vector2> polygon1, List<Vector2> polygon2)
        {
            foreach (Vector2 point in polygon1)
            {
                if (!IsPointInsideBox(point, polygon2))
                {
                    return false; // At least one point of polygon1 is outside polygon2
                }
            }
            return true; // All points of polygon1 are inside polygon2
        }
    }


}

