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
            foreach(BaseCollider col in CollisionManager.CollidersList)
            {
                if(col==null) continue;
                if(col==this) continue;
                if(col is CircleCollider collider)
                {
                    
                    if(CollisionManager.Circle_CircleCollision(this,collider,out HitPoint _hp))
                    {
                        hitPoints.Add(_hp);
                        
                        continue;
                    }
                }
                else if(col is BoxCollider boxCol)
                {
                    if (CollisionManager.Box_CircleCollision(boxCol, this, out HitPoint _hp))
                    {
                        hitPoints.Add(_hp);

                        continue;
                    }
                }

            }

            if (hitPoints.Count > 0)
            {
                if(isColliding)
                    onCollisionStay?.Invoke(hitPoints);
                else
                {
                    isColliding= true; 
                    onCollisionEnter?.Invoke(hitPoints);
                }

            }
                
            else
            {
                if(isColliding)
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
            foreach (BaseCollider col in CollisionManager.CollidersList)
            {
                if (col == null) continue;
                if (col == this) continue;
                if (col is CircleCollider circleCol)
                {

                    if (CollisionManager.Box_CircleCollision(this, circleCol, out HitPoint _hp))
                    {
                        hitPoints.Add(_hp);

                        continue;
                    }
                }
                else
                    if(col is BoxCollider boxCol)
                {
                    if(CollisionManager.OBB_OBB_Collision(this,boxCol,out HitPoint _hp))
                    {
                        hitPoints.Add(_hp);
                    }
                }

            }

            if (hitPoints.Count > 0)
            {
                if (isColliding)
                    onCollisionStay?.Invoke(hitPoints);
                else
                {
                    isColliding = true;
                    onCollisionEnter?.Invoke(hitPoints);
                }

            }

            else
            {
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
    }
    static class CollisionManager
    {
        public static readonly List<BaseCollider> CollidersList = new List<BaseCollider>();


        public static void Add<T>(T collider) where T : BaseCollider 
        {
            CollidersList.Add(collider);
        }

        public static void Update()
        {
            foreach(BaseCollider collider in CollidersList)
            {
                collider.CheckCollision();
            }
        }
        public static bool Circle_CircleCollision(CircleCollider c1, CircleCollider c2, out HitPoint hitPoint)
        {
            hitPoint = new HitPoint();
            float penetration = (c1.position - c2.position).Length() - (c1.radius + c2.radius);
            if (penetration > 0)
                return false;
            hitPoint.normal = c2.position - c1.position;
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
            bool returnBool = false;
            hitPoint = new HitPoint();
            Vector2 disp = Vector2.Zero;
            for (int i = 0; i < obbA.collisionPoints.Count; i++)
            {
                Vector2 start1 = obbA.position;
                Vector2 end1 = obbA.collisionPoints[i];
                for (int j = 0; j < obbB.collisionPoints.Count; j++)
                {
                    Vector2 start2 = obbB.collisionPoints[j];
                    Vector2 end2 = obbB.collisionPoints[(j + 1) % obbB.collisionPoints.Count];

                    float h = (end2.X - start2.X) * (start1.Y - end1.Y) - (start1.X - end1.X) * (end2.Y - start2.Y);
                    float t1 = ((start2.Y - end2.Y) * (start1.X - start2.X) + (end2.X - start2.X) * (start1.Y - start2.Y)) / h;
                    float t2 = ((start1.Y - end1.Y) * (start1.X - start2.X) + (end1.X - start1.X) * (start1.Y - start2.Y)) / h;

                    if (t1 >= 0 && t1 < 1 && t2 > 0 && t2 < 1)
                    {
                        disp += (1 - t1) * (end1 - start1);
                        returnBool= true;
                    }
                }
            }
           

            for (int i = 0; i < obbB.collisionPoints.Count; i++)
            {
                Vector2 start1 = obbB.position;
                Vector2 end1 = obbB.collisionPoints[i];
                for (int j = 0; j < obbA.collisionPoints.Count; j++)
                {
                    Vector2 start2 = obbA.collisionPoints[j];
                    Vector2 end2 = obbA.collisionPoints[(j + 1) % obbA.collisionPoints.Count];

                    float h = (end2.X - start2.X) * (start1.Y - end1.Y) - (start1.X - end1.X) * (end2.Y - start2.Y);
                    float t1 = ((start2.Y - end2.Y) * (start1.X - start2.X) + (end2.X - start2.X) * (start1.Y - start2.Y)) / h;
                    float t2 = ((start1.Y - end1.Y) * (start1.X - start2.X) + (end1.X - start1.X) * (start1.Y - start2.Y)) / h;

                    if (t1 >= 0 && t1 < 1 && t2 > 0 && t2 < 1)
                    {
                        disp -= (1 - t1) * (end1 - start1);
                        returnBool = true;
                    }
                }
            }
            hitPoint.penetrationVec = disp;
            disp.Normalize();
            float max = -float.MaxValue;
            Vector2 normal=Vector2.UnitY;
            for(int i=0;i<obbB.collisionPoints.Count;i++)
            {
              Vector2 edge= obbB.collisionPoints[(i+1)%obbB.collisionPoints.Count]-obbB.collisionPoints[i];
                edge.Normalize();
                if (max < (disp.X * edge.Y - disp.Y * edge.X))
                {
                    max = (disp.X * edge.Y - disp.Y * edge.X);
                    normal = edge;  
                }

            }
            hitPoint.normal = new Vector2(-normal.Y,normal.X) ;
            return returnBool;
        }

       
       




        


        




    }
}
