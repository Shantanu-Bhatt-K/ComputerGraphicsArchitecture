using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses.StateMachines
{
    internal class GameStateFactory
    {
        GameStateMachine _ctx;

        public GameStateFactory(GameStateMachine ctx)
        {
            this._ctx = ctx;
        }

        public BaseGameState Start()
        {
            return new StartState(_ctx,this);
        }
        public BaseGameState Main()

        {
            return new MainState(_ctx,this);
        }

        public BaseGameState Pause()
        {
            return new PauseState(_ctx,this);
        }
        public BaseGameState GameOver()
        {
            return new GameOverState(_ctx, this);
        }
    }
}
