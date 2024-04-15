using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.GameClasses.StateMachines
{
    internal abstract class BaseGameState
    {
        public BaseGameState(GameStateMachine context, GameStateFactory factory)
        {
            _ctx = context;
            _factory = factory;
        }
        public abstract void Enter();
        public abstract void Update(GameTime gametime);
        public abstract void Exit();
        public abstract void Draw(SpriteBatch _spriteBatch,GameTime gametime);
        public abstract void CheckSwitchStates();
        protected GameStateMachine _ctx;
        protected GameStateFactory _factory;

        protected void UpdateState()
        {

        }
        protected void SwitchState(BaseGameState newState)
        {
            Exit();
            newState.Enter();
            _ctx._currentState= newState;
        }
    }
}
