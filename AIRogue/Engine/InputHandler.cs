using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

namespace AIRogue.Engine
{
    static class InputHandler
    {
        public delegate void OnKeyUp(Keys eventKey);
        public delegate void OnKeyDown(Keys eventKey);

        static public List<OnKeyUp> KeyUpListeners { get; set; } = new List<OnKeyUp>();
        static public List<OnKeyDown> KeyDownListeners { get; set; } = new List<OnKeyDown>();

        static KeyboardState previousState;

        /// <summary>
        /// Handle input events.
        /// </summary>
        static public void HandleInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            HandleKeyDown(keyState);
            HandleKeyUp(keyState);
            previousState = keyState;
        }

        /// <summary>
        /// Check for KeyDown events.
        /// </summary>
        /// <param name="keyState">Current state of the keyboard.</param>
        static void HandleKeyDown(KeyboardState keyState)
        {
            foreach (Keys key in keyState.GetPressedKeys())
            {
                if (previousState.IsKeyDown(key))
                {
                    continue;
                }
                else
                {
                    ProcessKeyDown(key);
                }
            }
        }

        /// <summary>
        /// Check for KeyUp events.
        /// </summary>
        /// <param name="keyState">Current state of the keyboard.</param>
        static void HandleKeyUp(KeyboardState keyState)
        {
            foreach (Keys key in previousState.GetPressedKeys())
            {
                if (keyState.IsKeyDown(key))
                {
                    continue;
                }
                else
                {
                    ProcessKeyUp(key);
                }
            }
        }

        /// <summary>
        /// Call all KeyUp listeners.
        /// </summary>
        /// <param name="key">Key that triggered the event.</param>
        static void ProcessKeyUp(Keys key)
        {
            foreach (OnKeyUp function in KeyUpListeners)
            {
                function(key);
            }
        }

        /// <summary>
        /// Call all KeyDown listeners
        /// </summary>
        /// <param name="key">Key that triggered the event.</param>
        static void ProcessKeyDown(Keys key)
        {
            foreach (OnKeyDown function in KeyDownListeners)
            {
                function(key);
            }
        }
    }
}
