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
        public GameOverState(GameStateMachine context,GameStateFactory factory) : base(context, factory) { }
        

        public override void CheckSwitchStates()
        {
            if (ToSwitch)
                SwitchState(_factory.Start());
        }

        public override void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.Green);
        }

        public override void Enter()
        {
            CommandManager.AddKeyboardBinding(Keys.Space, EnterPressed);
        }

        public override void Exit()
        {
            ToSwitch = false;
            CommandManager.RemoveKeys(Keys.Space, EnterPressed);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void EnterPressed(eButtonState bs, Vector2 input)
        {
            ToSwitch = true;
        }
    }
}
