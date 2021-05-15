/* File: MenuEntry.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public class MenuEntry
    {
        private string _text;
        private float _selectionFade;    // Entries transition out of the selection effect when they are deselected
        private Vector2 _position;    // This is set by the MenuScreen each frame in Update
        private Rectangle _bounds;

        public string Text
        {
            private get => _text;
            set => _text = value;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Rectangle Bounds
        {
            get => _bounds;
            set => _bounds = value;
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;

        /// <summary>
        /// An event handler
        /// </summary>
        /// <param name="playerIndex">The controlling player</param>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
        }

        /// <summary>
        /// Constructor for the menu entry
        /// </summary>
        /// <param name="text">the text to add</param>
        public MenuEntry(string text)
        {
            _text = text;
            _bounds = new Rectangle((int)_position.X - 5, (int)_position.Y - 5, 10 + (10 * _text.Length), 10 + 20);
        }

        /// <summary>
        /// Updates the entry
        /// </summary>
        /// <param name="screen">The screen it's on</param>
        /// <param name="isSelected">if it has been selected</param>
        /// <param name="gameTime">The game's time</param>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }


        /// <summary>
        /// Draws the things
        /// </summary>
        /// <param name="screen">The screen its on</param>
        /// <param name="isSelected">if it is selected</param>
        /// <param name="gameTime">the game's time</param>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * _selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, _text, _position, color, 0,
                origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Gets the height of the window
        /// </summary>
        /// <param name="screen">The screen its on</param>
        /// <returns>the height of the screen</returns>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        /// <summary>
        /// Gets the width of the screen
        /// </summary>
        /// <param name="screen">The screen it is on</param>
        /// <returns>the width</returns>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
