using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses.StaticClasses
{
    static class CollisionBucket
    {
        static List<Collider> colliders = new List<Collider>();

        public static void AddCollider<T>(T collider) where T : Collider
        {
            colliders.Add(collider as Collider);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var collider in colliders)
            {
                foreach(var collider2 in colliders)
                {
                    if(collider!=collider2)
                    collider.CheckCollision(collider2);
                }
            }
        }
    }
}
