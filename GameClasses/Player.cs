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

        Vector2 point = Vector2.Zero;
        void ChangeColor(List<HitPoint> hitPoints)
        {
            renderer.colour = Color.Red;
        }
        void RevertColor()
        {
            point=Vector2.Zero;
        }
        BoxCollider collider = new BoxCollider();
       
        void Pointupdate(List<HitPoint>hitpoints)
        {
            point=hitpoints.First().position;
        }
        public override void Init(params object[] b)
        {
           base.Init(b);

            //collider.onCollisionEnter += ChangeColor;
            collider.onCollisionExit += RevertColor;
            collider.onCollisionStay += Pointupdate;
            collider.Init(transform.position, new Vector2(renderer.Width/2,renderer.Height/2)*transform.scale,transform.rotation);
            renderer.colour= Color.Transparent;
        }

        public override void Update(GameTime gameTime)
        {

            transform.position += new Vector2(0.5f, 0);
            collider.SetCollider(transform.position, new Vector2(renderer.Width / 2, renderer.Height / 2) * transform.scale, transform.rotation);
            transform.rotation += 0.005f;
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
