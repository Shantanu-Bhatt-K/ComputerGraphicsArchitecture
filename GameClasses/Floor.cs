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
    internal class Floor:GameObject
    {
        BoxCollider collider = new BoxCollider();


        public override void Init(params object[] b)
        {
            base.Init(b);

            //collider.onCollisionEnter += body.OnCollision;
            //collider.onCollisionStay += body.OnCollision;
            transform.scale = new Vector2(20,1);
            collider.Init(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            transform.rotation = -(float)Math.PI/8;
        }

        public override void Update(GameTime gameTime)
        {


            collider.SetCollider(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < collider.collisionPoints.Count; i++)
            {
                Primitives2D.DrawLine(spriteBatch, collider.collisionPoints[i], collider.collisionPoints[(i + 1) % collider.collisionPoints.Count], Color.Green);
            }
           
            base.Draw(ref spriteBatch, gameTime);
        }
    }
}
