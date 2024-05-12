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
    internal class Powerup: GameObject
    {
        float timer = 0;
        float activeTime = 10;
        public CircleCollider collider=new CircleCollider();
        public Action<Powerup> Collected;
        public Action<Powerup> Destroy;

        public override void Init(params object[] b)
        {
            renderer.colour = Microsoft.Xna.Framework.Color.White;
            base.Init(b);
            transform.scale = new Vector2(0.1f, 0.1f);
            collider.Init(transform.position, renderer.Width / 2 * transform.scale.X);
            collider.tag = "PowerUp";
            collider.onCollisionEnter += OnCollisionEnter;
            collider.isPhysic = false;
        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > activeTime)
            {
                CollisionManager.Remove(this.collider);
                Destroy?.Invoke(this);
            }
            
            collider.SetCollider(transform.position, renderer.Width / 2 * transform.scale.X);

        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
            Primitives2D.DrawCircle(spriteBatch, transform.position, renderer.Width / 2 * transform.scale.X, 50, Color.Red);
            base.Draw(ref spriteBatch, gameTime);
        }
        public void OnCollisionEnter(List<HitPoint> hitpoints)
        {
            foreach (HitPoint hitpoint in hitpoints)
            {
                if(hitpoint.tag=="Player")
                {
                    CollisionManager.Remove(this.collider);
                    Collected?.Invoke(this);
                    break;
                }
            }
        }
    }
}
