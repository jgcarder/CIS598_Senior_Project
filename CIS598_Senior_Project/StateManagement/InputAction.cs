using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CIS598_Senior_Project.StateManagement
{
    public class InputAction
    {

        private readonly Buttons[] _buttons;
        private readonly Keys[] _keys;
        private readonly bool _firstPressOnly;

        // These delegate types map to the methods on InputState. We use these to simplify the Occurred method
        // by allowing us to map the appropriate delegates and invoke them, rather than having two separate code paths.
        private delegate bool ButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex player);
        private delegate bool KeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex player);

        /// <summary>
        /// Constructs a new InputMapping, binding the suppled triggering input options to the action
        /// </summary>
        /// <param name="triggerButtons">The buttons that trigger this action</param>
        /// <param name="triggerKeys">The keys that trigger this action</param>
        /// <param name="firstPressOnly">If this action only triggers on the initial key/button press</param>
        public InputAction(Buttons[] triggerButtons, Keys[] triggerKeys, bool firstPressOnly)
        {
            // Store the buttons and keys. If the arrays are null, we create a 0 length array so we don't
            // have to do null checks in the Occurred method
            _buttons = triggerButtons != null ? triggerButtons.Clone() as Buttons[] : new Buttons[0];
            _keys = triggerKeys != null ? triggerKeys.Clone() as Keys[] : new Keys[0];
            _firstPressOnly = firstPressOnly;
        }

        /// <summary>
        /// Determines if he action has occured. If playerToTest is null, the player parameter will be the player that performed the action
        /// </summary> 
        /// <param name="stateToTest">The InputState object to test</param>
        /// <param name="playerToTest">If not null, specifies the player (0-3) whose input should be tested</param>
        /// <param name="player">The player (0-3) who triggered the action</param>
        public bool Occurred(InputState stateToTest, PlayerIndex? playerToTest, out PlayerIndex player)
        {
            // Figure out which delegate methods to map from the state which takes care of our "firstPressOnly" logic
            ButtonPress buttonTest;
            KeyPress keyTest;

            if (_firstPressOnly)
            {
                buttonTest = stateToTest.IsNewButtonPress;
                keyTest = stateToTest.IsNewKeyPress;
            }
            else
            {
                buttonTest = stateToTest.IsButtonPressed;
                keyTest = stateToTest.IsKeyPressed;
            }

            // Now we simply need to invoke the appropriate methods for each button and key in our collections
            foreach (var button in _buttons)
            {
                if (buttonTest(button, playerToTest, out player))
                    return true;
            }
            foreach (var key in _keys)
            {
                if (keyTest(key, playerToTest, out player))
                    return true;
            }

            // If we got here, the action is not matched
            player = PlayerIndex.One;
            return false;
        }

    }
}
