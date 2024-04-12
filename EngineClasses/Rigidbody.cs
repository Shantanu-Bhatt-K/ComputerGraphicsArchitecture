using ComputerGraphicsArchitecture.EngineClasses.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public float restitution;


        public Vector2 bounceDir;

        bool contacted = false;
        List<Vector2> normals=new();
        public void Init(Transform _transform)
        {
            transform= _transform;
            currentPos= transform.position;
            prevPos= transform.position;
            acceleration =new Vector2(0f,100);
            restitution =0.5f;
        }

        public void Update(GameTime gameTime)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            
                Vector2 vel;
                if (bounceDir.Length() == 0 && !contacted)
                    vel = (currentPos - prevPos);
                else
                    vel = bounceDir;
                prevPos = currentPos;

                if (contacted)
                {
                    currentPos += vel;
                    ;
                    contacted = false;
                }
                else
                {
                    currentPos += vel + acceleration * (float)(dt * dt);
                   
                }
             bounceDir = Vector2.Zero;
                transform.position = currentPos;
            
            

        }

        public void OnCollision(List<HitPoint> hitpoints)
        {

           Vector2 vel= (currentPos - prevPos);
            bounceDir = vel;
           
           
            foreach (HitPoint hitpoint in hitpoints)
            {
                hitpoint.normal.Normalize();

                if (Vector2.Dot(bounceDir, hitpoint.normal) < 0)
                    bounceDir -= (1 + restitution) * Vector2.Dot(vel, hitpoint.normal) * hitpoint.normal;
                else
                    bounceDir += restitution * Vector2.Dot(vel, hitpoint.normal) * hitpoint.normal;

                if (Math.Abs(Vector2.Dot(bounceDir,hitpoint.normal))<=0.001f)
                {
                    contacted = true ;
                    bounceDir-=Vector2.Dot(bounceDir,hitpoint.normal)*hitpoint.normal;
                }
                
            }
            


        }

        public void OnCollisionStay(List<HitPoint> hitpoints)
        {

            Vector2 vel = (currentPos - prevPos);
            Vector2 tempVel = vel;
            foreach (HitPoint hitpoint in hitpoints)
            {
                hitpoint.normal.Normalize();
                if (Vector2.Dot(tempVel, hitpoint.normal) < 0)
                    tempVel -= (1+ restitution) * Vector2.Dot(vel, hitpoint.normal) * hitpoint.normal;
                else
                    tempVel += restitution * Vector2.Dot(vel, hitpoint.normal) * hitpoint.normal;
                if (Math.Abs(Vector2.Dot(tempVel, hitpoint.normal)) <= 0.1f)
                {
                    contacted = true;
                    bounceDir = tempVel-Vector2.Dot(tempVel, hitpoint.normal) * hitpoint.normal;
                }

            }


        }
        public void OnCollisionExit()
        {
            contacted = false;
            bounceDir = Vector2.Zero;
        }

        
    }
}
