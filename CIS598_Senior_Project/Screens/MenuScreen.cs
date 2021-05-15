/* File: MenuScreen.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public abstract class MenuScreen : GameScreen
    {

        private readonly List<MenuEntry> _menuEntries = new List<MenuEntry>();
        private readonly List<bool> _selected = new List<bool>();
        private int _selectedEntry;
        private readonly string _menuTitle;

        private readonly InputAction _menuUp;
        private readonly InputAction _menuDown;
        private readonly InputAction _menuSelect;
        private readonly InputAction _menuCancel;

        private MouseState _currentMouse;
        private MouseState _priorMouse;

        // Gets the list of menu entries, so derived classes can add or change the menu contents.
        protected IList<MenuEntry> MenuEntries => _menuEntries;

        protected List<bool> Selected => _selected;

        /// <summary>
        /// Protected constructor for the menu screen
        /// </summary>
        /// <param name="menuTitle">The title of the screen</param>
        protected MenuScreen(string menuTitle)
        {
            _menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _menuUp = new InputAction(
                new[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
                new[] { Keys.Up }, true);
            _menuDown = new InputAction(
                new[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
                new[] { Keys.Down }, true);
            _menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);
            _menuCancel = new InputAction(
                new[] { Buttons.B, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        /// <summary>
        /// handles the input for the screen
        /// </summary>
        /// <param name="gameTime">the game's time</param>
        /// <param name="input">the player input</param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // For input tests we pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (_menuUp.Occurred(input, ControllingPlayer, out playerIndex))
            {
                _selectedEntry--;

                if (_selectedEntry < 0)
                    _selectedEntry = _menuEntries.Count - 1;
            }

            if (_menuDown.Occurred(input, ControllingPlayer, out playerIndex))
            {
                _selectedEntry++;

                if (_selectedEntry >= _menuEntries.Count)
                    _selectedEntry = 0;
            }

            if (_menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
                OnSelectEntry(_selectedEntry, playerIndex);
            else if (_menuCancel.Occurred(input, ControllingPlayer, out playerIndex))
                OnCancel(playerIndex);
        }

        /// <summary>
        /// An event handler for a selected entry
        /// </summary>
        /// <param name="entryIndex">The entry selected</param>
        /// <param name="playerIndex">The player who selected it</param>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            _menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// when cancel is pressed
        /// </summary>
        /// <param name="playerIndex">the pressing player</param>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        /// <summary>
        /// cancels the screen on twitter
        /// </summary>
        /// <param name="sender">A karen sending a hate message</param>
        /// <param name="e">Of course E for lord farquad</param>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        /// <summary>
        /// Updates locations of entries
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            var position = new Vector2(0f, 175f);

            // update each menu entry's location in turn
            foreach (var menuEntry in _menuEntries)
            {

                
                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }

        /// <summary>
        /// Updates the screen
        /// </summary>
        /// <param name="gameTime">the game's time</param>
        /// <param name="otherScreenHasFocus">if another screen has focus</param>
        /// <param name="coveredByOtherScreen">if is covered by another screen</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //_priorMouse = _currentMouse;
            //_currentMouse = Mouse.GetState();

            //for(int i = 0; i < _menuEntries.Count; i++)
            //{
            //    if(_currentMouse.X >= _menuEntries[i].Bounds.X && _currentMouse.X <= _menuEntries[i].Bounds.Width && _currentMouse.Y >= _menuEntries[i].Bounds.Y && _currentMouse.Y <= _menuEntries[i].Bounds.Height)
            //    {
            //        _selected[i] = true;
            //    }
            //    else
            //    {
            //        _selected[i] = false;
            //    }
            //}

            // Update each nested MenuEntry object.
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                bool isSelected = IsActive && i == _selectedEntry;
                //bool isSelected = IsActive && _selected[i];
                _menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;
            var font = ScreenManager.Font;

            spriteBatch.Begin();

            for (int i = 0; i < _menuEntries.Count; i++)
            {
                var menuEntry = _menuEntries[i];
                bool isSelected = IsActive && i == _selectedEntry;
                //bool isSelected = IsActive && _selected[i];
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            var titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            var titleOrigin = font.MeasureString(_menuTitle) / 2;
            var titleColor = new Color(192, 192, 192) * TransitionAlpha;
            const float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

    }
}
