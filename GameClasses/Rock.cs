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
        Vector2 speed = Vector2.One;
        float immuneTime = 0.5f;
        float timer = 0;
        bool isImmune = true;
        public CircleCollider collider = new CircleCollider();
        Rigidbody body=new Rigidbody();
        public int type;
        public Action<Rock> Destroyed;
        public Action<Vector2,Rock> Spawn;
        public Rock(Rock rock)
        {
           this.renderer.texture=rock.renderer.texture;
            this.renderer.colour=rock.renderer.colour;
            this.type=rock.type;
            this.transform.scale = rock.transform.scale;


        }
       

        public Rock()
        {
        }

        public override void Init(params object[] b)
        {
            renderer.colour = Color.White;
            base.Init(b);
            
            body.Init(transform);
            body.prevPos = transform.position+(Vector2)b[2];
            body.drag = 0;
            collider.Init(transform.position,renderer.Width / 2* transform.scale.X);
            collider.tag = "Meteor";
        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(timer>immuneTime && isImmune)
                {
                    collider.onCollisionEnter += body.OnCollision;
                    collider.onCollisionEnter += callDestroy;
                    collider.onCollisionStay += body.OnCollisionStay;
                    collider.onCollisionExit += body.OnCollisionExit;
                    isImmune = false;
                }
            body.Update(gameTime);
            collider.SetCollider(transform.position, renderer.Width / 2 * transform.scale.X);
            
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
           
            Primitives2D.DrawCircle(spriteBatch,transform.position, renderer.Width / 2 * transform.scale.X, 50, Color.Blue);
            base.Draw(ref spriteBatch, gameTime);
        }


        public void callDestroy(List<HitPoint>hitp)
        {
            foreach (HitPoint hitpoint in hitp)
            {
                if (hitpoint.tag == "Player")
                {
                    CollisionManager.Remove(this.collider);
                    Destroyed?.Invoke( this);
                   
                }
                if (hitpoint.tag == "Bullet" )
                {
                    CollisionManager.Remove(this.collider);
                    Spawn?.Invoke(transform.position, this);
                    
                }
                
            }
                

        }
    }
}
