using ComputerGraphicsArchitecture.DebugClasses;
using ComputerGraphicsArchitecture.EngineClasses;
using ComputerGraphicsArchitecture.EngineClasses.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses
{
    internal class Player: GameObject
    {
        BoxCollider collider = new BoxCollider();
        Vector2 point=new Vector2();
        
       
       Rigidbody body = new Rigidbody();
        public override void Init(params object[] b)
        {
           base.Init(b);
            transform.rotation = -10*(float)Math.PI/180;
            collider.onCollisionEnter += body.OnCollision;
            collider.onCollisionStay += body.OnCollisionStay;
            collider.onCollisionExit += body.OnCollisionExit ;
            collider.Init(transform.position, new Vector2(renderer.Width/2,renderer.Height/2)*transform.scale,transform.rotation);
            body.Init(transform);
            body.prevPos = transform.position + new Vector2(0, 0);
            //body.restitution = 0;
        }

        public override void Update(GameTime gameTime)
        {

            
            collider.SetCollider(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            body.Update(gameTime);
        }
        public override void Draw(ref SpriteBatch spriteBatch,GameTime gameTime)
        {
            for (int i = 0; i < collider.collisionPoints.Count; i++)
            {
                Primitives2D.DrawLine(spriteBatch, collider.collisionPoints[i], collider.collisionPoints[(i + 1) % collider.collisionPoints.Count], Color.Green);
            }
            Primitives2D.DrawCircle(spriteBatch, point, 5, 10, Color.Red);
            base.Draw(ref spriteBatch, gameTime);
        }
    }
}
