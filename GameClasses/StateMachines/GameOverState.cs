using ComputerGraphicsArchitecture.EngineClasses;
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

    internal class GameOverState : BaseGameState
    {

        bool ToSwitch = false;
        Texture2D Background = FileReader.ReadContent<Texture2D>("Background");
        public Button returnButton = new();
        SpriteFont TextFont;
        public GameOverState(GameStateMachine context,GameStateFactory factory) : base(context, factory) { }
        

        public override void CheckSwitchStates()
        {
            if (ToSwitch)
                SwitchState(_factory.Start());
        }

        public override void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.Red);
            returnButton.Draw(ref spriteBatch,gameTime);
            DrawShadowed(spriteBatch, "Game Over", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, _ctx._graphics.PreferredBackBufferHeight / 4));
        }

        public override void Enter()
        {
            TextFont = FileReader.ReadContent<SpriteFont>("Inlanders");
            returnButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, _ctx._graphics.PreferredBackBufferHeight / 2), "ReturnButton", new Vector2(0.05f, 0.05f));
            returnButton.OnClick += EnterPressed;
        }

        public override void Exit()
        {
            ToSwitch = false;
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void EnterPressed()
        {
            ToSwitch = true;
        }

        void DrawShadowed(SpriteBatch _spritebatch, string text, Vector2 position)
        {
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, 3 + (position.Y)), Color.Gray, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, position.Y), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
    }
}
