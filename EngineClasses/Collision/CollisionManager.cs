using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses.Collision
{
    public abstract class BaseCollider
    {
        public Vector2 position;
        public string tag = "Default";
        public bool isColliding=false;
        public Action<List<HitPoint>> onCollisionEnter;
        public Action onCollisionExit;
        public Action<List<HitPoint>> onCollisionStay;
        public abstract void Init(params object[] b);
        public abstract void SetCollider(params object[] b);

        public abstract void CheckCollision();

       

    }
    public class CircleCollider:BaseCollider
    {
        public float radius;
        List<BaseCollider> prevColliders = new();
        public override void Init(params object[] b)
        {
            
            position = (Vector2)b[0];  
            radius = (float)b[1];
            if (b.Length>=3)
                tag = b[2].ToString();
            CollisionManager.Add(this);

        }

        public override void SetCollider(params object[] b)
        {
            position = (Vector2)b[0];
            radius = (float)b[1];
        }

        public override void CheckCollision()
        {
            
            List<HitPoint> hitPoints = new();
            List<BaseCollider>colliders = new();
            foreach(BaseCollider col in CollisionManager.CollidersList.ToList())
            {
                if(col==null) continue;
                if(col==this) continue;
                if(col is CircleCollider collider)
                {
                    
                    if(CollisionManager.Circle_CircleCollision(this,collider,out HitPoint _hp))
                    {
                        _hp.tag=collider.tag;
                        hitPoints.Add(_hp);
                        colliders.Add(collider);
                       
                    }
                }
                else if(col is BoxCollider boxCol)
                {
                    if (CollisionManager.Circle_BoxCollision(this, boxCol, out HitPoint _hp))
                    {
                        _hp.tag = boxCol.tag;
                        hitPoints.Add(_hp);
                        colliders.Add(boxCol);

                    }
                }

            }
            if(hitPoints.Count>0)
            {
                isColliding = true;
                if (prevColliders.All(colliders.Contains) && colliders.Count == prevColliders.Count)
                    onCollisionStay?.Invoke(hitPoints);
                else
                    onCollisionEnter?.Invoke(hitPoints);
                prevColliders= colliders;
               
            }
            else
            {
                prevColliders.Clear();
                if (isColliding)
                {
                    isColliding = false;
                    onCollisionExit?.Invoke();
                }
            }
               
            
        }
    }

    public class BoxCollider:BaseCollider
    {
        public Vector2 position;
        public Vector2 halfWidths;
        public float rotation;
        public Vector2 YVec = Vector2.UnitY;
        public Vector2 XVec = Vector2.UnitX;
        public List<Vector2> collisionPoints = new List<Vector2>();
        List<BaseCollider> prevColliders = new();


        public override void Init(params object[]b)
        {
            SetCollider(b);
            CollisionManager.Add(this);
        }

        public override void SetCollider(params object[] b)
        {
            position = (Vector2)b[0];
            halfWidths = (Vector2)b[1];
            rotation = (float)b[2];
            YVec = new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation));
            XVec = new Vector2(-YVec.Y, YVec.X);
            YVec.Normalize();
            XVec.Normalize();
            collisionPoints.Clear();


            collisionPoints.Add(position + (YVec * halfWidths.Y) + (XVec * halfWidths.X));
            collisionPoints.Add(position - (YVec * halfWidths.Y) + (XVec * halfWidths.X));
            collisionPoints.Add(position - (YVec * halfWidths.Y) - (XVec * halfWidths.X));
            collisionPoints.Add(position + (YVec * halfWidths.Y) - (XVec * halfWidths.X));
        }
        public override void CheckCollision()
        {
            List<HitPoint> hitPoints = new();
            List<BaseCollider> colliders = new List<BaseCollider>();
            foreach (BaseCollider col in CollisionManager.CollidersList.ToList())
            {
                if (col == null) continue;
                if (col == this) continue;
                if (col is CircleCollider circleCol)
                {

                    if (CollisionManager.Box_CircleCollision(this, circleCol, out HitPoint _hp))
                    {
                        _hp.tag = circleCol.tag;
                        hitPoints.Add(_hp);
                        colliders.Add(circleCol);
                       
                    }
                }
                else
                    if(col is BoxCollider boxCol)
                {
                    if(CollisionManager.OBB_OBB_Collision(this,boxCol,out HitPoint _hp))
                    {
                        _hp.tag = boxCol.tag;
                        hitPoints.Add(_hp);
                        colliders.Add(boxCol);
                    }
                }

            }

            if (hitPoints.Count > 0)
            {
                isColliding = true;
                if (prevColliders.All(colliders.Contains) && colliders.Count == prevColliders.Count)
                    onCollisionStay?.Invoke(hitPoints);
                else
                    onCollisionEnter?.Invoke(hitPoints);
                prevColliders = colliders;

            }
            else
            {
                prevColliders.Clear();
                if (isColliding)
                {
                    isColliding = false;
                    onCollisionExit?.Invoke();
                }
            }


        }

    }


    public class HitPoint
    {
        public Vector2 position;
        public float depth;
        public Vector2 normal;
        public Vector2 penetrationVec;
        public string tag;
    }
    static class CollisionManager
    {
        private static readonly object lockObject = new object();
        public  static readonly List<BaseCollider> CollidersList = new List<BaseCollider>();

        public static void Add(BaseCollider collider)
        {
            lock (lockObject)
            {
                CollidersList.Add(collider);
            }
        }

        public static void Remove(BaseCollider collider)
        {
            lock (lockObject)
            {
                CollidersList.Remove(collider);
            }
        }

        public static void Update()
        {
            Parallel.ForEach(CollidersList.ToList(), collider =>
            {
                collider.CheckCollision();
            });
        }
            public static bool Circle_CircleCollision(CircleCollider c1, CircleCollider c2, out HitPoint hitPoint)
        {
            hitPoint = new HitPoint();
            float penetration = (c1.position - c2.position).Length() - (c1.radius + c2.radius);
            if (penetration > 0)
                return false;
            hitPoint.normal = c1.position - c2.position;
            hitPoint.normal.Normalize();
            hitPoint.penetrationVec = -penetration * hitPoint.normal;
            hitPoint.depth = -penetration;
            hitPoint.position = c1.position + (penetration * hitPoint.normal);
            return true;
        }

        public static bool Box_CircleCollision(BoxCollider b, CircleCollider c, out HitPoint hitPoint)
        {
            hitPoint = new HitPoint();

            hitPoint = new HitPoint();

            // Get the center of the circle
            Vector2 center = c.position;

            // Get the vector from the center of the box to the center of the circle
            Vector2 v = center - b.position;

            // Project the vector onto the axes of the oriented bounding box (OBB)
            float closestX = Vector2.Dot(v, b.XVec) / b.XVec.LengthSquared();
            float closestY = Vector2.Dot(v, b.YVec) / b.YVec.LengthSquared();

            // Clamp the projected point to the extents of the OBB
            closestX = MathHelper.Clamp(closestX, -b.halfWidths.X, b.halfWidths.X);
            closestY = MathHelper.Clamp(closestY, -b.halfWidths.Y, b.halfWidths.Y);

            // Calculate the closest point on the OBB to the circle
            Vector2 closestPoint = b.position + closestX * b.XVec + closestY * b.YVec;

            // Calculate the vector from the closest point on the OBB to the center of the circle
            Vector2 d = center - closestPoint;

            // Calculate the distance between the closest point on the OBB and the center of the circle
            float distance = d.Length() - c.radius;

            // If the distance is less than or equal to zero, there is a collision
            if (distance <= 0)
            {
                // Calculate the penetration vector
                Vector2 normal = -Vector2.Normalize(d);
                Vector2 penetrationVec = -distance * normal;

                // Set hit point data
                hitPoint.normal = normal;
                hitPoint.penetrationVec = penetrationVec;
                hitPoint.position = closestPoint;
                hitPoint.depth = -distance;

                return true;
            }

            return false;
        }

        public static bool Circle_BoxCollision(CircleCollider c, BoxCollider b, out HitPoint hitPoint)
        {
            hitPoint = new HitPoint();

            hitPoint = new HitPoint();

            // Get the center of the circle
            Vector2 center = c.position;

            // Get the vector from the center of the box to the center of the circle
            Vector2 v = center - b.position;

            // Project the vector onto the axes of the oriented bounding box (OBB)
            float closestX = Vector2.Dot(v, b.XVec) / b.XVec.LengthSquared();
            float closestY = Vector2.Dot(v, b.YVec) / b.YVec.LengthSquared();

            // Clamp the projected point to the extents of the OBB
            closestX = MathHelper.Clamp(closestX, -b.halfWidths.X, b.halfWidths.X);
            closestY = MathHelper.Clamp(closestY, -b.halfWidths.Y, b.halfWidths.Y);

            // Calculate the closest point on the OBB to the circle
            Vector2 closestPoint = b.position + closestX * b.XVec + closestY * b.YVec;

            // Calculate the vector from the closest point on the OBB to the center of the circle
            Vector2 d = center - closestPoint;

            // Calculate the distance between the closest point on the OBB and the center of the circle
            float distance = d.Length() - c.radius;

            // If the distance is less than or equal to zero, there is a collision
            if (distance <= 0)
            {
                // Calculate the penetration vector
                Vector2 normal = Vector2.Normalize(d);
                Vector2 penetrationVec = -distance * normal;

                // Set hit point data
                hitPoint.normal = normal;
                hitPoint.penetrationVec = penetrationVec;
                hitPoint.position = closestPoint;
                hitPoint.depth = -distance;

                return true;
            }

            return false;
        }




        public static bool OBB_OBB_Collision(BoxCollider obbA, BoxCollider obbB, out HitPoint hitPoint)
        {
            bool collisionDetected = false;
            hitPoint = new HitPoint();
            Vector2 disp = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Precalculate repeated values for obbB
            Vector2[] normalsB = new Vector2[obbB.collisionPoints.Count];
            for (int i = 0; i < obbB.collisionPoints.Count; i++)
            {
                Vector2 edge = obbB.collisionPoints[(i + 1) % obbB.collisionPoints.Count] - obbB.collisionPoints[i];
                normalsB[i] = new Vector2(-edge.Y, edge.X); // Perpendicular vectors
                normalsB[i].Normalize();
            }

            int numCollisionPointsA = obbA.collisionPoints.Count;
            int numCollisionPointsB = obbB.collisionPoints.Count;

            for (int i = 0; i < numCollisionPointsA && !collisionDetected; i++)
            {
                Vector2 start1 = obbA.position;
                Vector2 end1 = obbA.collisionPoints[i];
                Vector2 edge1 = end1 - start1; // Calculate once

                for (int j = 0; j < numCollisionPointsB && !collisionDetected; j++)
                {
                    Vector2 start2 = obbB.collisionPoints[j];
                    Vector2 end2 = obbB.collisionPoints[(j + 1) % numCollisionPointsB];

                    float h = (end2.X - start2.X) * (start1.Y - end1.Y) - (start1.X - end1.X) * (end2.Y - start2.Y);
                    float t1 = ((start2.Y - end2.Y) * (start1.X - start2.X) + (end2.X - start2.X) * (start1.Y - start2.Y)) / h;
                    float t2 = ((start1.Y - end1.Y) * (start1.X - start2.X) + (end1.X - start1.X) * (start1.Y - start2.Y)) / h;

                    if (t1 >= 0 && t1 < 1 && t2 > 0 && t2 < 1)
                    {
                        disp += (1 - t1) * edge1;
                        position += t1 * edge1;
                        collisionDetected = true;
                    }
                }
            }

            if (collisionDetected)
            {
                // Find the edge of obbB most opposed to the displacement vector
                float max = -float.MaxValue;
                Vector2 normal = Vector2.UnitY;

                for (int i = 0; i < numCollisionPointsB; i++)
                {
                    if (max < (disp.X * normalsB[i].Y - disp.Y * normalsB[i].X))
                    {
                        max = (disp.X * normalsB[i].Y - disp.Y * normalsB[i].X);
                        normal = normalsB[i];
                    }
                }

                hitPoint.penetrationVec = -disp / 2;
                hitPoint.normal = new Vector2(-normal.Y, normal.X);
                hitPoint.position = obbA.position + position;
            }

            return collisionDetected;
        }
















    }
}
