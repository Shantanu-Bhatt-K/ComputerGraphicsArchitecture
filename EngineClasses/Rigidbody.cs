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

        public float prevRot;
        public float currentRot;
        public float rotAccel;
        public float rotBounce;

        public Vector2 bounceDir;

        bool contacted = false;
        List<Vector2> normals=new();
        public void Init(Transform _transform)
        {
            transform= _transform;
            currentPos= transform.position;
            prevPos= transform.position;
            currentRot=transform.rotation;
            prevRot=transform.rotation;
            acceleration =new Vector2(0f,100);
            restitution =0.85f;
            rotAccel = 0.0f;
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
                    //contacted = false;
                }
                else
                {
                    currentPos += vel + acceleration * (float)(dt * dt);
                   
                }
             bounceDir = Vector2.Zero;
                transform.position = currentPos;

            float rotVel;
            if (rotBounce == 0 && !contacted)
                rotVel = currentRot - prevRot;
            
            else
                rotVel = rotBounce;
            prevRot = currentRot;

            
            currentRot += rotVel + rotAccel * (float)(dt * dt);
            rotBounce = 0;
            transform.rotation = currentRot;



        }

        public void OnCollision(List<HitPoint> hitpoints)
        {

           Vector2 vel= (currentPos - prevPos);
            bounceDir = vel;
            float rotVel= currentRot - prevRot;
            rotBounce = rotVel;
            float mag = 0;
            foreach (HitPoint hitpoint in hitpoints)
            {
                hitpoint.normal.Normalize();
                Vector2 hitDir = currentPos - hitpoint.position;
                Vector2 YVec = new Vector2((float)Math.Sin(currentRot), -(float)Math.Cos(currentRot));
                Vector2 XVec = new Vector2(-YVec.Y, YVec.X);
                if (!(Math.Abs(Vector2.Dot(YVec, hitpoint.normal)) >= Math.Cos(0.5f*(float)Math.PI/180) || Math.Abs(Vector2.Dot(XVec, hitpoint.normal)) >= Math.Cos(0.5f * (float)Math.PI / 180)))
                {
                    mag += hitpoint.normal.X * hitDir.Y - hitDir.X * hitpoint.normal.Y;
                }
               
                
                

            


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
            rotBounce = (restitution) * mag * (float)Math.PI / 7200f;



        }

        public void OnCollisionStay(List<HitPoint> hitpoints)
        {

            Vector2 vel = (currentPos - prevPos);
            Vector2 tempVel = vel;
            float rotVel = currentRot - prevRot;
            rotBounce = rotVel;

            float mag = 0;
            foreach (HitPoint hitpoint in hitpoints)
            {
                Vector2 hitDir = currentPos - hitpoint.position;
               
                
                Vector2 YVec = new Vector2((float)Math.Sin(currentRot), -(float)Math.Cos(currentRot));
                Vector2 XVec = new Vector2(-YVec.Y, YVec.X);
                if (!(Math.Abs(Vector2.Dot(YVec, hitpoint.normal)) >= Math.Cos(0.5f * (float)Math.PI / 180) || Math.Abs(Vector2.Dot(XVec, hitpoint.normal)) >= Math.Cos(0.5f * (float)Math.PI / 180)))
                {
                    mag += hitpoint.normal.X * hitDir.Y - hitDir.X * hitpoint.normal.Y;
                }
                else
                {
                    mag = 0;
                }




                hitpoint.normal.Normalize();
                if (Vector2.Dot(tempVel, hitpoint.normal) < 0)
                    tempVel -= (1+ restitution) * Vector2.Dot(vel, hitpoint.normal) * hitpoint.normal;
                else
                    tempVel += Vector2.Dot(hitpoint.normal, vel) * restitution * Vector2.Dot(vel, hitpoint.normal) * hitpoint.normal;
                if (Math.Abs(Vector2.Dot(tempVel, hitpoint.normal)) <= 0.1f)
                {
                    
                    contacted = true;
                    bounceDir = tempVel-Vector2.Dot(tempVel, hitpoint.normal) * hitpoint.normal;
                   
                }
                
            }
            rotBounce = (restitution) * mag * (float)Math.PI / 7200f;


        }
        public void OnCollisionExit()
        {
            contacted = false;
            bounceDir = Vector2.Zero;
        }

        
    }
}
