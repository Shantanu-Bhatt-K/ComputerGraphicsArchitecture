using ComputerGraphicsArchitecture.EngineClasses.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class Rigidbody
    {

        public Vector2 prevPos;
        public Vector2 currentPos;
        public Vector2 acceleration;
        public Transform transform;
        
        public void Init(Transform _transform)
        {
            transform= _transform;
            currentPos= transform.position;
            prevPos= transform.position;
            acceleration =new Vector2(0f,100);

        }

        public void Update(GameTime gameTime)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            
                Vector2 vel = currentPos - prevPos;
                prevPos = currentPos;

                currentPos += vel + acceleration *(float) (dt * dt);
                transform.position = currentPos;
            
            

        }

        public void OnCollision(List<HitPoint> hitpoints)
        {
            Vector2 vel = currentPos - prevPos;
            Vector2 pen = Vector2.Dot(hitpoints[0].penetrationVec,hitpoints[0].normal)*hitpoints[0].normal;
            currentPos -= pen;
            prevPos +=(Vector2.Dot(currentPos-prevPos, Vector2.Normalize(hitpoints[0].normal)) +  Vector2.Dot(vel, Vector2.Normalize(hitpoints[0].normal))) * Vector2.Normalize(hitpoints[0].normal);
            
           
        }
        public void OnCollisionStay(List<HitPoint> hitpoints)
        {
            currentPos -= hitpoints[0].penetrationVec;
        }
        
    }
}
