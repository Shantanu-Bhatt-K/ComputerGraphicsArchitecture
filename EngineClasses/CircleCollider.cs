using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class CircleCollider : Collider
    {
        public Vector2 position;
        public float radius;


        public override void SetCollider(params object[] b)
        {
            position = (Vector2)b[0];
            radius = (float)b[1];
        }

        public override void CheckCollision(Collider other)
        {
            if (other is CircleCollider circleCollider)
            {
                // Collision detection between two circle colliders
                float distanceSquared = Vector2.DistanceSquared(this.position, circleCollider.position);
                float combinedRadius = this.radius + circleCollider.radius;

                if (distanceSquared <= combinedRadius * combinedRadius)
                {
                    Vector2 normal = this.position - circleCollider.position;
                    
                    normal.Normalize();
                    Vector2 pos = circleCollider.position + circleCollider.radius * normal ;
                    HitPoint hitPoint=new HitPoint(pos,normal);
                   collided?.Invoke(hitPoint);
                    return;

                }
            }
            else if (other is BoxCollider boxCollider)
            {
                if (IsBoxCollidingWithCircle(boxCollider.collisionPoints,position, radius, out HitPoint hitPoint))
                {
                    collided?.Invoke(hitPoint);
                    return;
                }
            }
        }

    }

}
