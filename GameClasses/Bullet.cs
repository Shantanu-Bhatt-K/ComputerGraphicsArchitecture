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
    internal class Bullet: GameObject
    {
        public Action<Vector2, Bullet> Destroyed;
        Rigidbody rb=new();
        public BoxCollider collider=new();
        public float maxTime=5f;
        public float life;
        public override void Init(params object[] b)
        {
            
            base.Init(b);
            transform.scale = Vector2.One / 10;
            transform.rotation = (float)b[2];
            
            rb.Init(transform);
            rb.prevPos -= (Vector2)b[3];
            rb.drag = 0;
            collider.Init(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            collider.tag = "Bullet";
           
           
            

        }
        public override void Update(GameTime gameTime)
        {
            rb.Update(gameTime);
           
            life += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (maxTime <= life)
                callDestroy();
            Vector2 vel = rb.currentPos - rb.prevPos;
            transform.rotation = (float)Math.Atan2(vel.X , 1-vel.Y);
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


        public void callDestroy()
        {

            CollisionManager.Remove(this.collider);
            Destroyed?.Invoke(transform.position, this);
            return;


        }
    }
}