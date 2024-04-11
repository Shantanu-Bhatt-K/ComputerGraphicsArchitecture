
using ComputerGraphicsArchitecture.EngineClasses.Collision;
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
        private Rock rock = new();

 
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
           
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            FileReader.Init(Content);
            

            player.Init(new Vector2(100, 200), "PlayerShip");
            rock.Init(new Vector2(300, 100), "Meteor");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            CollisionManager.Update();
            player.Update(gameTime);
            rock.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            //_spriteBatch.Draw(planeTexure, new Vector2(100, 100), null, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
            player.Draw(ref _spriteBatch,gameTime);
            rock.Draw(ref _spriteBatch,gameTime);
            _spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}
