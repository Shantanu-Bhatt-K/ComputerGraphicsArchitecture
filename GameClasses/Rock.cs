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
        Vector2 point = Vector2.Zero;
        void Pointupdate(List<HitPoint> hitpoints)
        {
            point = hitpoints.First().position;
        }
        void ChangeColor(List<HitPoint> hitPoints)
        {
            renderer.colour = Color.Red;
        }void RevertColor()
        {
            point = Vector2.Zero;
        }
        BoxCollider collider = new BoxCollider();
        public override void Init(params object[] b)
        {
            renderer.colour = Color.Transparent;
            base.Init(b);
            transform.scale = new Vector2(1, 4);
            //collider.onCollisionEnter += ChangeColor;
            collider.onCollisionExit += RevertColor;
            collider.onCollisionStay += Pointupdate;
            collider.Init(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
        }
        public override void Update(GameTime gameTime)
        {


            collider.Init(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            transform.rotation += 0.01f;
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < collider.collisionPoints.Count; i++)
            {
                Primitives2D.DrawLine(spriteBatch, collider.collisionPoints[i], collider.collisionPoints[(i + 1) % collider.collisionPoints.Count], Color.Green);
            }
            Primitives2D.DrawCircle(spriteBatch,point, 5, 10, Color.Blue);
            base.Draw(ref spriteBatch, gameTime);
        }
    }
}
