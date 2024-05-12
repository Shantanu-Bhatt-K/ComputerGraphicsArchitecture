using ComputerGraphicsArchitecture.EngineClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses
{

    internal class PowerupSpawner:GameObject
    {
        float spawnRadius = 400f;
        Vector2 spawnCentre = Vector2.Zero;
        float spawnTimer = 5f;
        float currentTime = 0;
        Player player;
        List<Powerup> powerUps = new List<Powerup>();
        public override void Init(params object[] b)
        {
            spawnCentre = (Vector2)b[0];
            player = (Player)b[1];
            
        }
        public override void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime > spawnTimer)
            {
                currentTime = 0;
                
                SpawnPowerUp();
            }
            
            
            foreach (Powerup p in powerUps.ToList<Powerup>())
            {

                p.Update(gameTime);
            }
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Powerup p in powerUps)
            {
                p.Draw(ref spriteBatch, gameTime);
            }
        }
        void SpawnPowerUp()
        {

            var rand = new Random();
            double randAngle = rand.NextDouble() * 360;
            double randRadius = rand.NextDouble();
            Vector2 spawnDir = spawnCentre + new Vector2((float)Math.Cos(randAngle), (float)Math.Sin(randAngle)) * spawnRadius*(float)randRadius;
            Powerup temp = new Powerup();

            temp.Collected += SetPowerUp;
            temp.Destroy += DestroyPowerUp;
            temp.Init(spawnDir, "PowerUp");

            powerUps.Add(temp);

        }

        void DestroyPowerUp(Powerup powerup)
        {
            powerUps.Remove(powerup);
        }
        void SetPowerUp(Powerup powerup)
        {
            player.SwitchShooting();
            DestroyPowerUp(powerup);
        }



    }
    
}
