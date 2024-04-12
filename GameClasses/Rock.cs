using ComputerGraphicsArchitecture.DebugClasses;
using ComputerGraphicsArchitecture.EngineClasses;
using ComputerGraphicsArchitecture.EngineClasses.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses
{
    internal class Rock:GameObject
    {
       
       
        CircleCollider collider = new CircleCollider();
        Rigidbody body=new Rigidbody();
        public override void Init(params object[] b)
        {
            renderer.colour = Color.White;
            base.Init(b);
            collider.onCollisionEnter += body.OnCollision;
            body.Init(transform);
            collider.Init(transform.position,renderer.Width / 2* transform.scale.X);
        }
        public override void Update(GameTime gameTime)
        {

            body.Update(gameTime);
            collider.SetCollider(transform.position, renderer.Width / 2 * transform.scale.X);
            
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
           
            Primitives2D.DrawCircle(spriteBatch,transform.position, renderer.Width / 2 * transform.scale.X, 10, Color.Blue);
            base.Draw(ref spriteBatch, gameTime);
        }
    }
}
