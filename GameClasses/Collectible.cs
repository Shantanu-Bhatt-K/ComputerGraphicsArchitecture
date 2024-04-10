using ComputerGraphicsArchitecture.DebugClasses;
using ComputerGraphicsArchitecture.EngineClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses
{
    internal class Collectible: GameObject
    {
        CircleCollider collider;

        public override void Init(params object[] b)
        {
            base.Init(b);

            collider = new CircleCollider();
            //collider.Init(transform.position,transform.scale.X*renderer.Width/2);
            collider.Init(transform.position, transform.rotation, new Vector2(renderer.texture.Width / 2, renderer.texture.Height / 2) * transform.scale);
        }

        public override void Update(GameTime gameTime)
        {
            transform.position += new Vector2(0, -1);
            collider.SetCollider(transform.position, transform.scale.X * renderer.Width / 2);
            
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {

            Primitives2D.DrawCircle(spriteBatch, transform.position, collider.radius,15,Color.Green);
            
            base.Draw(ref spriteBatch, gameTime);
        }
    }
}
