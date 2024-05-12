using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

using System.Text;
using System.Threading.Tasks;
using ComputerGraphicsArchitecture.EngineClasses;
using Microsoft.Xna.Framework.Graphics;
using ComputerGraphicsArchitecture.EngineClasses.Collision;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;

namespace ComputerGraphicsArchitecture.GameClasses
{
    public class MeteorData
    {
        public Vector2 scale = Vector2.Zero;
        public int type;

    }
    internal class MeteorSpawner:GameObject
    {
        List<Rock> Meteors=new List<Rock>();
        List<Rock> types = new List<Rock>();

        float spawnRadius = 800f;
        Vector2 spawnCentre=Vector2.Zero;
        float spawnTimer=3f;
        float difficultyMeter = 0.9f;
        float currentTime = 0;
        Player player;
        public override void Init(params object[] b)
        {
            List<MeteorData> list = new List<MeteorData>();
           
            spawnCentre = (Vector2)b[0];
            player= (Player)b[1];
            list = FileReader.ReadXML<List<MeteorData>>("MeteorTypes");
            foreach (MeteorData mete in list)
            {
                Rock temp=new Rock();
                temp.Init(Vector2.Zero, "Meteor", Vector2.Zero);
                temp.transform.scale = mete.scale;
                temp.type=mete.type;
                CollisionManager.Remove(temp.collider);
                types.Add(temp);
            }
            

            AudioManager.AddSFX("Explosion");
            

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
            tempRock.Spawn += RemoveRock;
            tempRock.Destroyed += reducePlayerhealth;
            Vector2 speed=-Vector2.Normalize(player.transform.position-spawnDir)*5*(float)rand.NextDouble();
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
            tempRock.Spawn += RemoveRock;
            tempRock.Destroyed += reducePlayerhealth;
            Vector2 speed = dir* 2 ;
            tempRock.Init(position, "Meteor", speed);
            Meteors.Add(tempRock);
            speed = -speed;
            tempRock = new(types[type+1]);
            tempRock.Destroyed += RemoveRock;
            tempRock.Spawn += RemoveRock;
            tempRock.Destroyed += reducePlayerhealth;
            tempRock.Init(position, "Meteor", speed);
            Meteors.Add(tempRock);

        }

        void RemoveRock(Vector2 position,Rock rock)
        {
            player.Score += 5*(3-rock.type);
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
        void reducePlayerhealth(Rock rock)
        {
            player.Health -= 10;
        }




    }
}
