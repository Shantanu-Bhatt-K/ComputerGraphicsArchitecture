using ComputerGraphicsArchitecture.DebugClasses;
using ComputerGraphicsArchitecture. EngineClasses;
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
        BoxCollider collider;
        public void ChangeColor(HitPoint hp)
        {
            renderer.colour = Color.Red;
        }
        public void revertColor(HitPoint hp)
        { renderer.colour = Color.White; }
        
        public override void Init(params object[] b)
        {
           base.Init(b);
            
            collider = new BoxCollider();
            collider.Init(transform.position, transform.rotation, new Vector2(renderer.texture.Width/2, renderer.texture.Height/2) * transform.scale);
            collider.collided += ChangeColor;
            collider.notcollided += revertColor;
        }

        public override void Update(GameTime gameTime)
        {
            
            transform.position += new Vector2(1, 0);
            
            collider.SetCollider(transform.position, transform.rotation, new Vector2(renderer.texture.Width / 2, renderer.texture.Height / 2) * transform.scale);
            transform.rotation += 0.01f;
        }
        public override void Draw(ref SpriteBatch spriteBatch,GameTime gameTime)
        {
            for(int i=0;i<collider.collisionPoints.Count;i++)
            {
                Primitives2D.DrawLine(spriteBatch, collider.collisionPoints[i], collider.collisionPoints[(i + 1) % collider.collisionPoints.Count], Color.Green);
            }
           base.Draw(ref spriteBatch, gameTime);
        }
    }
}
