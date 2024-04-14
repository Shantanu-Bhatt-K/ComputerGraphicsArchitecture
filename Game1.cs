
using ComputerGraphicsArchitecture.EngineClasses.Collision;
using ComputerGraphicsArchitecture.EngineClasses.InputManagement;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using ComputerGraphicsArchitecture.GameClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ComputerGraphicsArchitecture
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player=new();
        private MeteorSpawner spawner=new MeteorSpawner();
        
        private Texture2D backGround;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            FileReader.Init(Content);
            

            player.Init(new Vector2(100, 300), "PlayerShip");
            spawner.Init(new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/ 2), player.transform);
            
            CommandManager.AddComboBinding(new List<Keys> { Keys.LeftAlt,Keys.LeftControl}, StopGame);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backGround = FileReader.ReadContent<Texture2D>("Background");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
                CollisionManager.Update();
                CommandManager.Update();
                player.Update(gameTime);
                
                spawner.Update(gameTime);
                base.Update(gameTime);
         }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(backGround, Vector2.Zero, Color.White);
            player.Draw(ref _spriteBatch,gameTime);
           
            spawner.Draw(ref _spriteBatch,gameTime);
            _spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }

        void StopGame(eButtonState buttonState,Vector2 amount)
        {
            Exit();
        }
    }
}
