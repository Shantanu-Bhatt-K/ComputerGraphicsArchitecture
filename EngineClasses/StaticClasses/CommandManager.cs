using ComputerGraphicsArchitecture.EngineClasses.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses.StaticClasses
{
    public delegate void GameAction(eButtonState buttonState, Vector2 amount);
    public static class CommandManager
    {
        static private InputListener m_Input;
        static  Dictionary<Keys,List<GameAction>> m_KeyBindings = new Dictionary<Keys, List<GameAction>>();
        static  Dictionary<List<Keys>,List<GameAction>> m_ComboBindings = new Dictionary<List<Keys>, List<GameAction>>();
        static CommandManager()
        {
            m_Input = new InputListener();
            // Register events with the input listener
            m_Input.OnKeyDown += OnKeyDown;
            m_Input.OnKeyPressed += OnKeyPressed;
            m_Input.OnKeyUp += OnKeyUp;
            m_Input.OnComboDown += OnComboDown;
            m_Input.OnComboUp += OnComboUp;
            m_Input.OnComboPressed += OnComboPressed;

        }
        static public void Update()
        {
            // Update polling input listener, everything else is handled by events
            m_Input.Update();
        }
        static public void OnKeyDown(object sender, KeyboardEventArgs e)
        {

            List<GameAction> actions = m_KeyBindings[e.Key];
            foreach (GameAction action in actions)
            {
                if (action != null)
                {
                    action(eButtonState.DOWN, new Vector2(1.0f));
                }
            }
           
        }
        static public void OnKeyUp(object sender, KeyboardEventArgs e)
        {
            List<GameAction> actions = m_KeyBindings[e.Key];
            foreach (GameAction action in actions)
            {
                if (action != null)
                {
                    action(eButtonState.UP, new Vector2(1.0f));
                }
            }
        }
        static public void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            List<GameAction> actions = m_KeyBindings[e.Key];
            foreach (GameAction action in actions)
            {
                if (action != null)
                {
                    action(eButtonState.PRESSED, new Vector2(1.0f));
                }
            }
        }

        static public void OnComboDown(object sender, MultiKeyboardEventsArgs e)
        {

            List<GameAction> actions = m_ComboBindings[e.Combo];
            foreach (GameAction action in actions)
            {
                if (action != null)
                {
                    action(eButtonState.DOWN, new Vector2(1.0f));
                }
            }

        }
        static public void OnComboUp(object sender, MultiKeyboardEventsArgs e)
        {
            List<GameAction> actions = m_ComboBindings[e.Combo];
            foreach (GameAction action in actions)
            {
                if (action != null)
                {
                    action(eButtonState.UP, new Vector2(1.0f));
                }
            }
        }
        static public void OnComboPressed(object sender, MultiKeyboardEventsArgs e)
        {
            List<GameAction> actions = m_ComboBindings[e.Combo];
            foreach (GameAction action in actions)
            {
                if (action != null)
                {
                    action(eButtonState.PRESSED, new Vector2(1.0f));
                }
            }
        }

        static public void AddKeyboardBinding(Keys key, GameAction action)
        {
            // Add key to listen for when polling
            m_Input.AddKey(key);
            // Add the binding to the command map
            if(m_KeyBindings.ContainsKey(key))
            {
                m_KeyBindings[key].Add(action);
            }
            else
            {
                m_KeyBindings.Add(key,new List<GameAction> { action});
            }
            
        }

        static public void AddComboBinding(List<Keys> keys, GameAction action)
        {
            // Add key to listen for when polling
            m_Input.AddCombo(keys);
            // Add the binding to the command map
            if (m_ComboBindings.ContainsKey(keys))
            {
                m_ComboBindings[keys].Add(action);
            }
            else
            {
                m_ComboBindings.Add(keys, new List<GameAction> { action });
            }

        }
    }

}
