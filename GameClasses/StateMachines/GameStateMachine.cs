using ComputerGraphicsArchitecture.EngineClasses.Collision;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses.StateMachines
{
    internal class GameStateMachine : Game
    {
        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player = new();
        private MeteorSpawner spawner = new MeteorSpawner();
        public  BaseGameState _currentState;
        private GameStateFactory _states;
        public GameStateMachine()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _states =new GameStateFactory(this);
            
        }

        protected override void Initialize()
        {
            FileReader.Init(Content);
            base.Initialize();
            _currentState = _states.Start();
            _currentState.Enter();
        }
        protected override void Update(GameTime gameTime)
        {
            
            _currentState.Update(gameTime);
            _currentState.CheckSwitchStates();
            CommandManager.Update();
            CollisionManager.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _currentState.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
