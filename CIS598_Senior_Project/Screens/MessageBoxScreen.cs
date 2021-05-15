/* File: MessageBoxScreen.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CIS598_Senior_Project.StateManagement;
using CIS598_Senior_Project.FleetObjects;

namespace CIS598_Senior_Project.Screens
{
    /// <summary>
    /// A class for message box screens.
    /// </summary>
    public class MessageBoxScreen : GameScreen
    {

        private readonly string _message;
        private Texture2D _gradientTexture;
        private readonly InputAction _menuSelect;
        private readonly InputAction _menuCancel;

        private Fleet _fleet;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="includeUsageText">bool to include text</param>
        public MessageBoxScreen(string message, bool includeUsageText = true)
        {
            const string usageText = "\nSpace, Enter = ok" +
                                     "\nBackspace = cancel";

            if (includeUsageText)
                _message = message + usageText;
            else
                _message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            _menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);
            _menuCancel = new InputAction(
                new[] { Buttons.B, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        /// <summary>
        /// another constructor but this one's packing heat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fleet"></param>
        /// <param name="includeUsageText"></param>
        public MessageBoxScreen(string message, Fleet fleet, bool includeUsageText = true)
        {
            const string usageText = "\nSpace, Enter = ok" +
                                     "\nBackspace = cancel";

            if (includeUsageText)
                _message = message + usageText;
            else
                _message = message;

            IsPopup = true;

            _fleet = fleet;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            _menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);
            _menuCancel = new InputAction(
                new[] { Buttons.B, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Loads graphics content for this screen. This uses the shared ContentManager
        // provided by the Game class, so the content will remain loaded forever.
        // Whenever a subsequent MessageBoxScreen tries to load this same content,
        // it will just get back another reference to the already loaded data.
        public override void Activate()
        {
            var content = ScreenManager.Game.Content;
            _gradientTexture = content.Load<Texture2D>("gradient");
        }

        /// <summary>
        /// Handles input
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="input">The player's input</param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (_menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
            {
                //if(_fleet != null) //save it
                Accepted?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
            else if (_menuCancel.Occurred(input, ControllingPlayer, out playerIndex))
            {
                Cancelled?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
            }
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">gametime duh?</param>
        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var textSize = font.MeasureString(_message);
            var textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            var backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                (int)textPosition.Y - vPad, (int)textSize.X + hPad * 2, (int)textSize.Y + vPad * 2);

            var color = Color.White * TransitionAlpha;    // Fade the popup alpha during transitions

            spriteBatch.Begin();

            spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);
            spriteBatch.DrawString(font, _message, textPosition, color);

            spriteBatch.End();
        }

    }
}
