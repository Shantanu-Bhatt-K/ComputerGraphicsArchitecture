using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

using System.Text;
using System.Threading.Tasks;
using ComputerGraphicsArchitecture.EngineClasses;
using Microsoft.Xna.Framework.Graphics;
using ComputerGraphicsArchitecture.EngineClasses.Collision;

namespace ComputerGraphicsArchitecture.GameClasses
{
    internal class MeteorSpawner:GameObject
    {
        List<Rock> Meteors=new List<Rock>();
        List<Rock> types = new List<Rock>();
        float spawnRadius = 800f;
        Vector2 spawnCentre=Vector2.Zero;
        float spawnTimer=3f;
        float difficultyMeter = 0.9f;
        float currentTime = 0;
        Transform playerTransform;
        public override void Init(params object[] b)
        {
            spawnCentre = (Vector2)b[0];
            playerTransform = (Transform)b[1];
            Rock bigRock = new Rock();
            Rock mediumRock = new Rock();
            Rock smallRock = new Rock();
            bigRock.Init(Vector2.Zero, "Meteor",Vector2.Zero);
            bigRock.transform.scale = new Vector2(1, 1);
            CollisionManager.Remove(bigRock.collider);
            bigRock.type = 0;
            mediumRock.Init(Vector2.Zero, "Meteor", Vector2.Zero);
            mediumRock.transform.scale = new Vector2(0.75f, 0.75f);
            CollisionManager.Remove(mediumRock.collider);
            mediumRock.type = 1;
            smallRock.Init(Vector2.Zero, "Meteor", Vector2.Zero);
            smallRock.transform.scale = new Vector2(0.5f, 0.5f);
            CollisionManager.Remove(smallRock.collider);
            smallRock.type = 2;

            types.Add(bigRock);
            types.Add(mediumRock);
            types.Add(smallRock);


        }

        public override void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (currentTime > spawnTimer )
            {
                currentTime = 0;
                spawnTimer *= difficultyMeter;
                SpawnRock();
            }
            spawnTimer = Math.Max(spawnTimer, 0.5f);
            for (int i=0;i<Meteors.Count;i++)
            {
                if ((Meteors[i].transform.position - spawnCentre).Length() > 1000)
                {
                    RemoveRock(Meteors[i]);
                }
               
            }
            foreach (Rock r in Meteors)
            {
                
                r.Update(gameTime);
            }
        }

        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Rock r in Meteors)
            {
                r.Draw(ref spriteBatch, gameTime);
            }
        }


        void SpawnRock()
        {

            var rand = new Random();
            double randAngle=rand.NextDouble()*360;
            Vector2 spawnDir = spawnCentre+new Vector2((float)Math.Cos(randAngle), (float)Math.Sin(randAngle)) * spawnRadius;
            Rock tempRock = new(types[rand.Next(3)]);
            tempRock.Destroyed += RemoveRock;
            Vector2 speed=-Vector2.Normalize(playerTransform.position-spawnDir)*5*(float)rand.NextDouble();
            tempRock.Init(spawnDir, "Meteor", speed);
           
            Meteors.Add(tempRock);

        }

        void SpawnRock(Vector2 position,int type)
        {
            var rand = new Random();
            double randAngle = rand.NextDouble() * 360;
            Vector2 dir = new Vector2((float)Math.Cos(randAngle), (float)Math.Sin(randAngle)) ;
            Rock tempRock = new(types[type+1]);
            tempRock.Destroyed += RemoveRock;
            Vector2 speed = dir* 2 ;
            tempRock.Init(position, "Meteor", speed);
            Meteors.Add(tempRock);
            speed = -speed;
            tempRock = new(types[type+1]);
            tempRock.Destroyed += RemoveRock;
           
            tempRock.Init(position, "Meteor", speed);
            Meteors.Add(tempRock);

        }

        void RemoveRock(Vector2 position,Rock rock)
        {
            Meteors.Remove(rock);
            if(rock.type<2)
            {
                SpawnRock(position, rock.type);
            }
            
        }
        void RemoveRock( Rock rock)
        {
            CollisionManager.Remove(rock.collider);
            Meteors.Remove(rock);
            

        }




    }
}
