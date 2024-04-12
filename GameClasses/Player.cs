﻿using ComputerGraphicsArchitecture.DebugClasses;
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

       
       Rigidbody body = new Rigidbody();
        public override void Init(params object[] b)
        {
           base.Init(b);
            //transform.rotation = 45;
            collider.onCollisionEnter += body.OnCollision;
            //collider.onCollisionStay += body.OnCollision;
            collider.Init(transform.position, new Vector2(renderer.Width/2,renderer.Height/2)*transform.scale,transform.rotation);
            body.Init(transform);
        }

        public override void Update(GameTime gameTime)
        {

            transform.rotation+=0.02f;
            collider.SetCollider(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            body.Update(gameTime);
        }
        public override void Draw(ref SpriteBatch spriteBatch,GameTime gameTime)
        {
            for (int i = 0; i < collider.collisionPoints.Count; i++)
            {
                Primitives2D.DrawLine(spriteBatch, collider.collisionPoints[i], collider.collisionPoints[(i + 1) % collider.collisionPoints.Count], Color.Green);
            }
            //Primitives2D.DrawCircle(spriteBatch, point, 5, 10, color);
            base.Draw(ref spriteBatch, gameTime);
        }
    }
}
