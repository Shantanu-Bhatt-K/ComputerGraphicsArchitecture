using ComputerGraphicsArchitecture.EngineClasses;
using ComputerGraphicsArchitecture.EngineClasses.InputManagement;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses.StateMachines
{
    
    internal class StartState : BaseGameState
    {
        bool SwitchMain=false;
        bool SwitchHelp=false;
        bool SwitchScore=false;
        public StartState(GameStateMachine context, GameStateFactory factory): base(context, factory) { }
        public Button startButton = new();
        public Button HelpButton = new();
        public Button ScoreButton= new();
        public Button ExitButton= new();
        SpriteFont TextFont;
        Texture2D Background ;
        public override void CheckSwitchStates()
        {
            if (SwitchMain)
                SwitchState(_factory.Main());
            else if (SwitchHelp)
                SwitchState(_factory.Help());
            else if (SwitchScore)
                SwitchState(_factory.Score());
                
        }

        public override void Draw(SpriteBatch _spritebatch, GameTime gametime)
        {
            _spritebatch.Draw(Background, Vector2.Zero, Color.Blue);
            DrawShadowed(_spritebatch, "Asteroids?", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2, _ctx._graphics.PreferredBackBufferHeight / 4));
         
            startButton.Draw( ref _spritebatch, gametime);
            HelpButton.Draw(ref _spritebatch, gametime);
            ScoreButton.Draw(ref _spritebatch, gametime);
            ExitButton.Draw(ref _spritebatch, gametime);

        }

        public override void Enter()
        {
            SwitchMain = false;
            Background = FileReader.ReadContent<Texture2D>("Background");
            TextFont = FileReader.ReadContent<SpriteFont>("Inlanders");
            AudioManager.AddBGMusic("BGMusic");
            AudioManager.PlayBGMusic("BGMusic",true);
            startButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth/2,_ctx._graphics.PreferredBackBufferHeight/2),"StartButton",new Vector2(0.05f,0.05f));
            HelpButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth/2,3*_ctx._graphics.PreferredBackBufferHeight/4),"HelpButton",new Vector2(0.05f,0.05f));
            ScoreButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth/2,2.5f*_ctx._graphics.PreferredBackBufferHeight/4),"TopScore",new Vector2(0.05f,0.05f));
            ExitButton.Init(new Vector2(_ctx._graphics.PreferredBackBufferWidth/2,3.5f*_ctx._graphics.PreferredBackBufferHeight/4),"ExitButton",new Vector2(0.05f,0.05f));
            startButton.OnClick += SwitchToMain;
            HelpButton.OnClick += SwitchToHelp;
            ExitButton.OnClick += ExitGame;
            ScoreButton.OnClick += SwitchToScore;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void SwitchToMain()
        {
            SwitchMain = true;
        }
        public void SwitchToScore()
        {
            SwitchScore = true;
        }
        public void SwitchToHelp()
        {
            SwitchHelp = true;
        }
        void ExitGame()
        {
            _ctx.Exit();
        }

        public override void Exit()
        {
            SwitchMain = false;
            SwitchScore = false;
            SwitchHelp = false;
            startButton.DestroyButton();
            HelpButton.DestroyButton();
            ScoreButton.DestroyButton();
            ExitButton.DestroyButton();
        }

        void DrawShadowed(SpriteBatch _spritebatch,string text,Vector2 position)
        {
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, 3 + (position.Y)), Color.Gray, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spritebatch.DrawString(TextFont, text, new Vector2(position.X - TextFont.MeasureString(text).Length() / 2, position.Y), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
    }
}
