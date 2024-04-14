using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses.InputManagement
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs(Keys key, KeyboardState currentKeyboardState, KeyboardState prevKeyboardState)
        {
            CurrentState = currentKeyboardState;
            PrevState = prevKeyboardState;
            Key = key;
        }

        public readonly KeyboardState CurrentState;
        public readonly KeyboardState PrevState;
        public readonly Keys Key;
    }

    public class MultiKeyboardEventsArgs:EventArgs
    {
        public MultiKeyboardEventsArgs(List<Keys> combo, KeyboardState currentKeyboardState, KeyboardState prevKeyboardState) 
        {
            CurrentState=currentKeyboardState;
            PrevState=prevKeyboardState;
            Combo=combo;
        }
        public readonly KeyboardState CurrentState;
        public readonly KeyboardState PrevState;
        public readonly List<Keys> Combo;
    }
}
