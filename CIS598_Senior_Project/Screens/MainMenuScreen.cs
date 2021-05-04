﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using CIS598_Senior_Project.MenuObjects;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private Game _game;

        private List<CustButton> _buttons;

        private ContentManager _content;

        private Texture2D _background;
        private Texture2D _texture;
        private Texture2D _label;

        private SpriteFont _descriptor;

        private int _widthIncrement;
        private int _heightIncrement;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private SoundEffect _button1;
        private SoundEffect _button2;
        private SoundEffect _button3;
        private SoundEffect _button4;

        private List<float> _vol;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        /// <summary>
        /// Constructor for the main menu
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="music">part of the volume for the music</param>
        /// <param name="sfx">part of the volume for the sfx</param>
        /// <param name="master">the master volume</param>
        public MainMenuScreen(Game game, List<float> vol)
        {
            _game = game;
            _buttons = new List<CustButton>();

            _vol = vol;

            _widthIncrement = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 100; //game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 100; //game.GraphicsDevice.Viewport.Height / 100;

            int maxH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int maxW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            _buttons.Add(new CustButton(0, new Rectangle(_widthIncrement * 35, maxH - _heightIncrement * 54, _widthIncrement * 30, _heightIncrement * 15), true));
            _buttons.Add(new CustButton(1, new Rectangle(_widthIncrement * 35, maxH - _heightIncrement * 36, _widthIncrement * 30, _heightIncrement * 15), true));
            _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement * 35, maxH - _heightIncrement * 18, _widthIncrement * 30, _heightIncrement * 15), true));
            
            SoundEffect.MasterVolume = vol[0] * vol[2];
            MediaPlayer.Volume = vol[1] * vol[2];
        }

        /// <summary>
        /// For when the screen activates
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _descriptor = _content.Load<SpriteFont>("galbasic");
            _texture = _content.Load<Texture2D>("MetalBackground");

            _buttons[0].Texture = _content.Load<Texture2D>("PlayGame");
            _buttons[1].Texture = _content.Load<Texture2D>("Options");
            _buttons[2].Texture = _content.Load<Texture2D>("QuitGame");

            _label = _content.Load<Texture2D>("MainMenuLabel");

            _button1 = _content.Load<SoundEffect>("Button1");
            _button2 = _content.Load<SoundEffect>("Button2");
            _button3 = _content.Load<SoundEffect>("Button3");
            _button4 = _content.Load<SoundEffect>("Button4");

            foreach (var button in _buttons)
            {
                button.AnAction += ButtonCatcher;
            }
        }

        /// <summary>
        /// For when the screen deactivates
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// To unload the content
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        /// <summary>
        /// Updates the screen
        /// </summary>
        /// <param name="gameTime">The games time</param>
        /// <param name="otherScreenHasFocus">if another screen is the focus of the user</param>
        /// <param name="coveredByOtherScreen">If another screen is on top of this one.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                foreach (var button in _buttons)
                {
                    if (button.IsActive)
                    {
                        if (_currentMouseState.X >= button.Position.X && _currentMouseState.X <= button.Position.X + button.Area.Width
                                            && _currentMouseState.Y >= button.Position.Y && _currentMouseState.Y <= button.Position.Y + button.Area.Height)
                        {
                            if (_currentMouseState.LeftButton == ButtonState.Pressed)// && _previousMouseState.LeftButton == ButtonState.Released)
                            {
                                button.Color = Color.DarkSlateGray;
                                button.AnAction(button, new ButtonClickedEventArgs() { Id = button.Id });
                            }

                            if (_currentMouseState.LeftButton == ButtonState.Pressed)
                            {
                                button.Color = Color.DarkSlateGray;
                            }
                            else
                            {
                                button.Color = Color.White;
                            }
                        }
                        else
                        {
                            button.Color = Color.White;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles some input from the user.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = 0;//(int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];
        }

        /// <summary>
        /// Draws the screen's assets
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(_label, new Vector2(_widthIncrement * 34.2f, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 72), Color.White);

            for (int i = 0; i < _buttons.Count; i++)
            {
                if (_buttons[i].IsActive)
                {
                    if (_buttons[i].Texture != null)
                    {
                        spriteBatch.Draw(_buttons[i].Texture, _buttons[i].Area, _buttons[i].Color);
                    }
                    else
                    {
                        spriteBatch.Draw(_texture, _buttons[i].Area, _buttons[i].Color);
                    }
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// The even handler for the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCatcher(object sender, ButtonClickedEventArgs e)
        {
            CustButton button = (CustButton)sender;

            switch (button.Id)
            {
                case 0:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();
                        _button1.Play();
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new PlayMenuScreen(_game, _vol), null);
                    }
                    break;
                case 1:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();
                        _button2.Play();
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new OptionsMenuScreen(_game, _vol), null);
                    }
                    break;
                case 2:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();
                        _button3.Play();
                        ScreenManager.Game.Exit();

                        //var confirmExitMessageBox = new MessageBoxScreen("Are you sure you want to exit?");
                        //confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
                        //ScreenManager.AddScreen(confirmExitMessageBox, 0);
                    }
                    break;
            }
        }

        /// <summary>
        /// Exits the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

    }
}
