using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.util
{

    public delegate void KeyEventHandler();
    public delegate void MouseEventHandler();

    public class InputHelper
    {

        private List<Keys> keysToUpdate = new List<Keys>();
        private Dictionary<Keys, bool> previousKeyStates = new Dictionary<Keys, bool>();
        private Dictionary<Keys, KeyEventHandler> onPressHandlers = new Dictionary<Keys, KeyEventHandler>();
        private Dictionary<Keys, KeyEventHandler> onReleaseHandlers = new Dictionary<Keys, KeyEventHandler>();
        private ButtonState previousLeftButtonState;
        private ButtonState previousRightButtonState;
        private MouseEventHandler leftButtonPressHandler;
        private MouseEventHandler rightButtonPressHandler;

        public void SetLeftButtonPressHandler(MouseEventHandler handler)
        {
            leftButtonPressHandler = handler;
        }

        public void SetRightButtonPressHandler(MouseEventHandler handler)
        {
            rightButtonPressHandler = handler;
        }

        public void SetKeyboardPressHandler(Keys key, KeyEventHandler handler)
        {
            onPressHandlers.Add(key, handler);
            keysToUpdate.Add(key);
        }

        public void SetKeyboardReleaseHandler(Keys key, KeyEventHandler handler)
        {
            onReleaseHandlers.Add(key, handler);
            keysToUpdate.Add(key);
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // process mouse values
            if (leftButtonPressHandler != null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && previousLeftButtonState == ButtonState.Released)
                {
                    leftButtonPressHandler();
                }
            }
            if (rightButtonPressHandler != null)
            {
                if (mouseState.RightButton == ButtonState.Pressed && previousRightButtonState == ButtonState.Released)
                {
                    rightButtonPressHandler();
                }
            }

            // process keyboard values
            foreach (Keys key in keysToUpdate)
            {
                bool isDown = keyboardState.IsKeyDown(key);
                bool wasDown = previousKeyStates.ContainsKey(key) ? previousKeyStates[key] : false;

                if (isDown && !wasDown && onPressHandlers.ContainsKey(key))
                {
                    onPressHandlers[key]();
                }
                else if (!isDown && wasDown && onReleaseHandlers.ContainsKey(key))
                {
                    onReleaseHandlers[key]();
                }

                previousKeyStates[key] = isDown;
            }

            // update previous mouse values
            previousLeftButtonState = mouseState.LeftButton;
            previousRightButtonState = mouseState.RightButton;
        }

    }
}
