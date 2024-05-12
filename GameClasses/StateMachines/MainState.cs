using ComputerGraphicsArchitecture.EngineClasses.Collision;
using ComputerGraphicsArchitecture.EngineClasses.InputManagement;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses.StateMachines
{
    
    internal class MainState : BaseGameState
    {
        public MainState(GameStateMachine context, GameStateFactory factory) : base(context, factory) { }
        
        private Player player = new();
        private MeteorSpawner spawner = new MeteorSpawner();
        private PowerupSpawner powerupspawner = new PowerupSpawner();
        private Texture2D backGround;
        private SpriteFont HudFont;
        public override void CheckSwitchStates()
        {
            if(player.Health<=0)
            {
                SwitchState(_factory.GameOver());
            }
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime gameTime)
        {
            _spriteBatch.Draw(backGround, Vector2.Zero, Color.White);
           
            _spriteBatch.DrawString(HudFont, "Health - " + player.Health.ToString(), new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(HudFont, "Score - " + player.Score.ToString(), new Vector2(10, 60), Color.White);
            player.Draw(ref _spriteBatch,gameTime);

            spawner.Draw(ref _spriteBatch,gameTime);
            powerupspawner.Draw(ref _spriteBatch,gameTime);
        }

        public override void Enter()
        {
            player.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, _ctx._graphics.PreferredBackBufferHeight / 2), "PlayerShip");
            spawner.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth/2, _ctx._graphics.PreferredBackBufferHeight/ 2), player);
            powerupspawner.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth/2, _ctx._graphics.PreferredBackBufferHeight/ 2), player);
            backGround = FileReader.ReadContent<Texture2D>("Background");
            HudFont = FileReader.ReadContent<SpriteFont>("Inlanders");
            CommandManager.AddComboBinding(new List<Keys> { Keys.LeftAlt,Keys.LeftControl}, StopGame);
        }

        public override void Exit()
        {
            List<int> Scores = FileReader.ReadXML<List<int>>("scores");
            if(Scores==null)
                Scores = new List<int>();
            Scores.Add(player.Score);
            Scores.Sort();
            Scores.Reverse();
            if (Scores.Count > 8)
            {
                Scores.RemoveRange(8, Scores.Count - 8);
            }
            FileReader.WriteXML<List<int>>("scores",Scores);
            CollisionManager.ClearList(); 
            player.RemoveBindings();

        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            spawner.Update(gameTime);
            powerupspawner.Update(gameTime);
        }

        public void StopGame(eButtonState bs,Vector2 amount)
        {
            _ctx.Exit();
        }
    }
}
