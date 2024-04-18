using ComputerGraphicsArchitecture.EngineClasses;
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
    internal class HelpState : BaseGameState
    {
        public HelpState(GameStateMachine context, GameStateFactory factory) : base(context, factory) { }
        public Button returnButton = new();
        public Texture2D backgroundTexture;
        SpriteFont TextFont;
        bool goBack = false;
        public override void CheckSwitchStates()
        {
            if (goBack)
                SwitchState(_factory.Start());
        }

        public override void Draw(SpriteBatch _spritebatch, GameTime gametime)
        {
            _spritebatch.Draw(backgroundTexture, Vector2.Zero, Color.Blue);
            DrawShadowed(_spritebatch, "Help", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, _ctx._graphics.PreferredBackBufferHeight / 10),1,Color.Yellow);
            DrawShadowed(_spritebatch, "Press Up to move Forward", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2,2* _ctx._graphics.PreferredBackBufferHeight / 10),0.5f,Color.White);
            DrawShadowed(_spritebatch, "Press Down to move Backward", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 3*_ctx._graphics.PreferredBackBufferHeight / 10), 0.5f, Color.White);
            DrawShadowed(_spritebatch, "Press Left to rotate Clockwise", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 4*_ctx._graphics.PreferredBackBufferHeight / 10), 0.5f, Color.White);
            DrawShadowed(_spritebatch, "Press Right to rotate Counter-Clockwise", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 5*_ctx._graphics.PreferredBackBufferHeight / 10), 0.5f, Color.White);
            DrawShadowed(_spritebatch, "Press Space to Shoot", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 6*_ctx._graphics.PreferredBackBufferHeight / 10), 0.5f, Color.White);
            DrawShadowed(_spritebatch, "Press Enter to Switch Modes", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 7*_ctx._graphics.PreferredBackBufferHeight / 10), 0.5f, Color.White);

            returnButton.Draw(ref _spritebatch, gametime);
        }

        public override void Enter()
        {
            backgroundTexture = FileReader.ReadContent<Texture2D>("Background");
            TextFont = FileReader.ReadContent<SpriteFont>("Inlanders");
            returnButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 9*_ctx._graphics.PreferredBackBufferHeight / 10), "ReturnButton", new Vector2(0.05f, 0.05f));
            returnButton.OnClick += Return;
        }

        public override void Exit()
        {
            goBack = false;
            returnButton.DestroyButton();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        void Return()
        {
            goBack= true;
        }
        void DrawShadowed(SpriteBatch _spritebatch, string text, Vector2 position)
        {
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, 3 + (position.Y)), Color.Gray, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, position.Y), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
        void DrawShadowed(SpriteBatch _spritebatch, string text, Vector2 position,float scale,Color color)
        {
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - (TextFont.MeasureString(text).Length()*scale )/ 2, 3 + (position.Y)), Color.Gray, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - (TextFont.MeasureString(text).Length() * scale)/2, position.Y), color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }
    }
}
