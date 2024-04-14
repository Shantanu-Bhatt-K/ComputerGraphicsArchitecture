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
        public Vector2 impulse = Vector2.Zero;
        public Transform transform;
        public float restitution;

        public float prevRot;
        public float currentRot;
        public float rotAccel;
        public float rotBounce;
        public float rotDrag;
        public float drag;
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
            acceleration =new Vector2(0,0);
            restitution =1f;
            rotAccel = 0.0f;
            drag = 200f;
            rotDrag = 400f;
            
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
                    contacted = false;
                }
                else
                {
                    currentPos += vel + (impulse+acceleration-(drag*vel)) * (float)(dt * dt);
                   
                }
             bounceDir = Vector2.Zero;
                transform.position = currentPos;

            float rotVel;
            if (rotBounce == 0 && !contacted)
                rotVel = currentRot - prevRot;
            
            else
                rotVel = rotBounce;
            prevRot = currentRot;

            
            currentRot += rotVel + (rotAccel-rotDrag*rotVel) * (float)(dt * dt);
            rotBounce = 0;
            transform.rotation = currentRot;

            impulse = Vector2.Zero; 


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
                if (Single.IsNaN(hitpoint.normal.X))
                    continue;
                Vector2 hitDir = currentPos - hitpoint.position;
                Vector2 YVec = new Vector2((float)Math.Sin(currentRot), -(float)Math.Cos(currentRot));
                Vector2 XVec = new Vector2(-YVec.Y, YVec.X);
                if (!(Math.Abs(Vector2.Dot(YVec, hitpoint.normal)) >= Math.Cos(0.5f*(float)Math.PI/180) || Math.Abs(Vector2.Dot(XVec, hitpoint.normal)) >= Math.Cos(0.5f * (float)Math.PI / 180)))
                {
                    if (!Single.IsNaN(hitpoint.normal.X))
                        mag += hitpoint.normal.X * hitDir.Y - hitDir.X * hitpoint.normal.Y;
                }
                prevPos += Vector2.Dot(currentPos - prevPos, hitpoint.normal) * hitpoint.normal;
                currentPos += hitpoint.penetrationVec;
                

               
                
            }
            rotBounce = (restitution) * mag * (float)Math.PI / 900f;



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
                if(Single.IsNaN(hitpoint.normal.X))
                    continue;
                Vector2 hitDir = currentPos - hitpoint.position;


                Vector2 YVec = new Vector2((float)Math.Sin(currentRot), -(float)Math.Cos(currentRot));
                Vector2 XVec = new Vector2(-YVec.Y, YVec.X);
                if (!(Math.Abs(Vector2.Dot(YVec, hitpoint.normal)) >= Math.Cos(0.5f * (float)Math.PI / 180) || Math.Abs(Vector2.Dot(XVec, hitpoint.normal)) >= Math.Cos(0.5f * (float)Math.PI / 180)))
                {
                    if (!Single.IsNaN(hitpoint.normal.X))
                        mag += hitpoint.normal.X * hitDir.Y - hitDir.X * hitpoint.normal.Y;
                }
                else
                {
                    mag = 0;
                }

                currentPos += hitpoint.penetrationVec;
             

            }
            rotBounce = (restitution) * mag * (float)Math.PI / 900f;


        }
        public void OnCollisionExit()
        {
            contacted = false;
            bounceDir = Vector2.Zero;
        }


        public void AddForce(Vector2 force)
        {
            impulse = force;
        }

        
    }
}
