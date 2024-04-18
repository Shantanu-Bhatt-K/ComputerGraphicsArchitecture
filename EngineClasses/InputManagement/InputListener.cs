using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses.InputManagement
{
        public enum eButtonState
        {
            PRESSED,
            DOWN,
            UP,
            MOVED
        }

     class InputListener
    {
         public event EventHandler<KeyboardEventArgs> OnKeyDown = delegate { };

         public event EventHandler<KeyboardEventArgs> OnKeyPressed = delegate { };

         public event EventHandler<KeyboardEventArgs> OnKeyUp = delegate { };

        public event EventHandler<MultiKeyboardEventsArgs> OnComboDown = delegate { };

        public event EventHandler<MultiKeyboardEventsArgs> OnComboPressed = delegate { };

        public event EventHandler<MultiKeyboardEventsArgs> OnComboUp = delegate { };

        public event EventHandler<MouseEventArgs> onMouseMove = delegate { };

        public event EventHandler<MouseEventArgs>onMouseDown=delegate { };

        public event EventHandler<MouseEventArgs> onMouseUp=delegate { };

        public event EventHandler<MouseEventArgs> onMousePressed = delegate { };



        HashSet<Keys> inputKeys;
        HashSet<List<Keys>> inputCombos;
         KeyboardState currentKeyboardState;
         KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
         GamePadState currentGamePadState;
         GamePadState previousGamePadState;

        //Mouse states used to track Mouse button press
         MouseState currentMouseState;
         MouseState previousMouseState;
         public InputListener()
        {



            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;



            currentMouseState = Mouse.GetState();
            previousMouseState = currentMouseState;
            inputKeys = new HashSet<Keys>();
            inputCombos = new HashSet<List<Keys>>();

        }


        public  void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            FireKeyboardEvents();
            FireMouseEvents();
        }

         void FireKeyboardEvents()
        {
            foreach (Keys key in inputKeys)
            {
                
                if (currentKeyboardState.IsKeyDown(key))
                    OnKeyDown?.Invoke(this, new KeyboardEventArgs(key, currentKeyboardState,
                       previousKeyboardState));
                
                
                if (previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key))
                    OnKeyUp?.Invoke(this, new KeyboardEventArgs(key, currentKeyboardState, previousKeyboardState));

                if(previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key))
                    OnKeyPressed?.Invoke(this, new KeyboardEventArgs(key, currentKeyboardState, previousKeyboardState));
                
            }
            foreach(List<Keys> list in inputCombos)
            {
                bool isDown = true;
                bool isUp = true;
                bool isPressed = true;
                foreach(Keys key in list)
                {
                    if (!currentKeyboardState.IsKeyDown(key))
                        isDown = false;
                    if (!(previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key)))
                        isUp = false;
                    if (!(previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key)))
                        isPressed = false;
                   
                }
                if (isDown)
                    OnComboDown?.Invoke(this, new MultiKeyboardEventsArgs(list, currentKeyboardState,
                   previousKeyboardState));
                if (isUp)
                    OnComboUp?.Invoke(this, new MultiKeyboardEventsArgs(list, currentKeyboardState,
                   previousKeyboardState));
                if (isPressed)
                    OnComboPressed?.Invoke(this, new MultiKeyboardEventsArgs(list, currentKeyboardState,
                   previousKeyboardState));
            }
        }
         void FireMouseEvents()
        {
            if(currentMouseState.Position!=previousMouseState.Position)
            {
                onMouseMove?.Invoke(this,new MouseEventArgs(MouseButton.NONE,currentMouseState,previousMouseState));
            }


            if(currentMouseState.LeftButton==ButtonState.Pressed)
            {
                onMouseDown?.Invoke(this, new MouseEventArgs(MouseButton.LEFT, currentMouseState, previousMouseState));
            }
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton!=ButtonState.Pressed)
            {
                onMousePressed?.Invoke(this, new MouseEventArgs(MouseButton.LEFT, currentMouseState, previousMouseState));
            }
            if (currentMouseState.LeftButton != ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                onMouseUp?.Invoke(this, new MouseEventArgs(MouseButton.LEFT, currentMouseState, previousMouseState));
            }

            
            
            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                onMouseDown?.Invoke(this, new MouseEventArgs(MouseButton.RIGHT, currentMouseState, previousMouseState));
            }
            if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed)
            {
                onMousePressed?.Invoke(this, new MouseEventArgs(MouseButton.RIGHT, currentMouseState, previousMouseState));
            }
            if (currentMouseState.RightButton != ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed)
            {
                onMouseUp?.Invoke(this, new MouseEventArgs(MouseButton.RIGHT, currentMouseState, previousMouseState));
            }

            
            
            if (currentMouseState.MiddleButton == ButtonState.Pressed)
            {
                onMouseDown?.Invoke(this, new MouseEventArgs(MouseButton.MIDDLE, currentMouseState, previousMouseState));
            }
            if (currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton != ButtonState.Pressed)
            {
                onMousePressed?.Invoke(this, new MouseEventArgs(MouseButton.MIDDLE, currentMouseState, previousMouseState));
            }
            if (currentMouseState.MiddleButton != ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Pressed)
            {
                onMouseUp?.Invoke(this, new MouseEventArgs(MouseButton.MIDDLE, currentMouseState, previousMouseState));
            }
        }
        public  void AddKey(Keys _input)
        {
            inputKeys.Add(_input);
        }
        public void AddCombo(List<Keys> _inputCombo)
        {
            inputCombos.Add(_inputCombo);
        }

        public  void RemoveKey(Keys _input)
        {
            inputKeys.Remove(_input);
        }public  void RemoveCombo(List<Keys> _inputCombo)
        {
            inputCombos.Remove(_inputCombo);
        }
    }
}
