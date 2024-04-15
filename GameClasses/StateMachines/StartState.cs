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
    
    internal class StartState : BaseGameState
    {
        bool ToSwitch=false;
        public StartState(GameStateMachine context, GameStateFactory factory): base(context, factory) { }
        SpriteFont TextFont;
        Texture2D Background ;
        public override void CheckSwitchStates()
        {
            if (ToSwitch)
                SwitchState(_factory.Main());
                
        }

        public override void Draw(SpriteBatch _spritebatch, GameTime gametime)
        {
            _spritebatch.Draw(Background, Vector2.Zero, Color.Blue);

            _spritebatch.DrawString(TextFont, "Press Enter to Start", new Vector2(_ctx._graphics.PreferredBackBufferWidth / 2 - TextFont.MeasureString("Press Enter to Start").Length() / 2, _ctx._graphics.PreferredBackBufferHeight / 2), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);


        }

        public override void Enter()
        {
            ToSwitch = false;
            Background = FileReader.ReadContent<Texture2D>("Background");
            TextFont = FileReader.ReadContent<SpriteFont>("Inlanders");

            CommandManager.AddKeyboardBinding(Keys.Enter, EnterPressed);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void EnterPressed(eButtonState bs,Vector2 input)
        {
            ToSwitch = true;
        }

        public override void Exit()
        {
            ToSwitch = false;
            CommandManager.RemoveKeys(Keys.Enter, EnterPressed);
        }
    }
}
