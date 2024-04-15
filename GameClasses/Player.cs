using ComputerGraphicsArchitecture.DebugClasses;
using ComputerGraphicsArchitecture.EngineClasses;
using ComputerGraphicsArchitecture.EngineClasses.Collision;
using ComputerGraphicsArchitecture.EngineClasses.InputManagement;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using System.Data;

namespace ComputerGraphicsArchitecture.GameClasses
{
    internal class Player: GameObject
    {
        BoxCollider collider = new BoxCollider();
        Vector2 point=new Vector2();
        Vector2 inputVec = Vector2.Zero;
       List<Bullet> bullets = new List<Bullet>();
       Rigidbody body = new Rigidbody();
        public override void Init(params object[] b)
        {
           base.Init(b);
            
            collider.onCollisionEnter += body.OnCollision;
            collider.onCollisionStay += body.OnCollisionStay;
            collider.onCollisionExit += body.OnCollisionExit ;
            collider.Init(transform.position, new Vector2(renderer.Width/2,renderer.Height/2)*transform.scale,transform.rotation);
            collider.tag = "Player";
            body.Init(transform);
            CommandManager.AddKeyboardBinding(Keys.Left, LeftInput);
            CommandManager.AddKeyboardBinding(Keys.Up, UpInput);
            CommandManager.AddKeyboardBinding(Keys.Right, RightInput);
            CommandManager.AddKeyboardBinding(Keys.Down, DownInput);
            CommandManager.AddKeyboardBinding(Keys.Space, Shoot);
        }

        public override void Update(GameTime gameTime)
        {
            collider.SetCollider(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            inputVec=Vector2.Transform(inputVec,Matrix.CreateRotationZ(transform.rotation));
            body.AddForce(inputVec * 1000f);
            inputVec = Vector2.Zero;
            Constrain();
            body.Update(gameTime);
            
            foreach (Bullet bullet in bullets.ToList())
            {
                bullet.Update(gameTime);
            }
        }
        public override void Draw(ref SpriteBatch spriteBatch,GameTime gameTime)
        {
            for (int i = 0; i < collider.collisionPoints.Count; i++)
            {
                Primitives2D.DrawLine(spriteBatch, collider.collisionPoints[i], collider.collisionPoints[(i + 1) % collider.collisionPoints.Count], Color.Green);
            }
            foreach(Bullet bullet in bullets.ToList())
            {
                bullet.Draw(ref spriteBatch, gameTime);
            }
            Primitives2D.DrawCircle(spriteBatch, point, 5, 10, Color.Red);
            base.Draw(ref spriteBatch, gameTime);
        }

        void LeftInput(eButtonState buttonState, Vector2 amount)
        {
            body.prevRot+=0.005f;
        }
        void UpInput(eButtonState buttonState, Vector2 amount)
        {
            inputVec -= Vector2.UnitY;
        }
        void RightInput(eButtonState buttonState, Vector2 amount)
        {
            body.prevRot -= 0.005f;
        }
        void DownInput(eButtonState buttonState, Vector2 amount)
        {
            inputVec += Vector2.UnitY;

        }
        void Shoot(eButtonState state,Vector2 amount)
        {
            if(state==eButtonState.DOWN)
            {
                if(bullets.Count>=1000 )
                {
                    RemoveBullet(bullets[0].transform.position, bullets[0]);
                }
                Vector2 forward= new Vector2((float)Math.Sin(transform.rotation), -(float)Math.Cos(transform.rotation));
                Vector2 bulletPos = transform.position + (renderer.Height * transform.scale.Y) * forward;
                Bullet temp = new Bullet();
                temp.Init(bulletPos, "Bullet",transform.rotation,forward* 10f);
                temp.Destroyed += RemoveBullet;
                 bullets.Add(temp);
                
            }
        }

        void RemoveBullet(Vector2 position, Bullet bullet)
        {
            CollisionManager.Remove(bullet.collider);
            bullets.Remove(bullet);
         }


        void Constrain()
        {
            if(transform.position.X<0)
            {
                body.currentPos.X += 1280;
                body.prevPos.X += 1280;
            }
            if (transform.position.X > 1280)
            {
                body.currentPos.X -= 1280;
                body.prevPos.X -= 1280;
            }
            if (transform.position.Y < 0)
            {
                body.currentPos.Y += 720;
                body.prevPos.Y += 720;
            }
            if (transform.position.Y > 720)
            {
                body.currentPos.Y -=720;
                body.prevPos.Y -=720;
            }

        }








    }
}
