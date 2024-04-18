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
    internal class ScoreState : BaseGameState
    {
        public Button returnButton = new();
        public Texture2D backgroundTexture;
        SpriteFont TextFont;
        List<int> Scores= new List<int>();
        bool goBack = false;
        public ScoreState(GameStateMachine context, GameStateFactory factory) : base(context, factory)
        {
        }

        public override void CheckSwitchStates()
        {
            if (goBack)
                SwitchState(_factory.Start());
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime gametime)
        {
            DrawShadowed(_spriteBatch, "High Scores", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2,0), 1, Color.Yellow);
            for (int i = 0;i<Scores.Count;i++)
            {
                DrawShadowed(_spriteBatch, "Score "+(i+1).ToString()+" #" + Scores[i].ToString(), new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, (i+1)*_ctx._graphics.PreferredBackBufferHeight / 10),0.5f, Color.White);
            }
            returnButton.Draw(ref _spriteBatch, gametime);
        }

        public override void Enter()
        {
            backgroundTexture = FileReader.ReadContent<Texture2D>("Background");
            Scores = FileReader.ReadXML<List<int>>("scores");
            TextFont = FileReader.ReadContent<SpriteFont>("Inlanders");
            returnButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, 9 * _ctx._graphics.PreferredBackBufferHeight / 10), "ReturnButton", new Vector2(0.05f, 0.05f));
            returnButton.OnClick += Return;
        }

        public override void Exit()
        {
            goBack = false;
            returnButton.DestroyButton();
        }

        public override void Update(GameTime gametime)
        {
            
        }
        void Return()
        {
            goBack = true;
        }

        void DrawShadowed(SpriteBatch _spritebatch, string text, Vector2 position)
        {
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, 3 + (position.Y)), Color.Gray, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, position.Y), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
        void DrawShadowed(SpriteBatch _spritebatch, string text, Vector2 position, float scale, Color color)
        {
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - (TextFont.MeasureString(text).Length() * scale) / 2, 3 + (position.Y)), Color.Gray, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - (TextFont.MeasureString(text).Length() * scale) / 2, position.Y), color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }
    }
}
