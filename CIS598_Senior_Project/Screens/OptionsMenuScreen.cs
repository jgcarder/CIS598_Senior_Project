using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using CIS598_Senior_Project.MenuObjects;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public class OptionsMenuScreen : GameScreen
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

        private double _master;
        private double _music;
        private double _sfx;

        private string _masterDisplay;
        private string _musicDisplay;
        private string _sfxDisplay;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private List<float> _vol;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public OptionsMenuScreen(Game game, List<float> vol)
        {
            _game = game;
            _buttons = new List<CustButton>();

            _vol = vol;

            _masterDisplay = "";
            _musicDisplay = "";
            _sfxDisplay = "";

            _music = (double)_vol[1];
            _master = (double)_vol[2];
            _sfx = (double)_vol[0];

            _widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            _buttons.Add(new CustButton(0, new Rectangle(_widthIncrement, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 16, 10 * _widthIncrement, 15 * _heightIncrement), true)); //back button
            _buttons.Add(new CustButton(1, new Rectangle(_widthIncrement * 25, 15 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                    //decrease master volume
            _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement * 65, 15 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                    //increase master volume

            _buttons.Add(new CustButton(3, new Rectangle(_widthIncrement * 25, 35 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                    //decrease music volume
            _buttons.Add(new CustButton(4, new Rectangle(_widthIncrement * 65, 35 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                    //increase music volume

            _buttons.Add(new CustButton(5, new Rectangle(_widthIncrement * 25, 55 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                    //decrease sfx volume
            _buttons.Add(new CustButton(6, new Rectangle(_widthIncrement * 65, 55 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                    //increase sfx volume

            _buttons.Add(new CustButton(7, new Rectangle(_widthIncrement, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 34, _widthIncrement * 10, _heightIncrement * 15), true));             //reset to defaults
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

            _buttons[0].Texture = _content.Load<Texture2D>("Back");
            _buttons[1].Texture = _content.Load<Texture2D>("Decrease");
            _buttons[2].Texture = _content.Load<Texture2D>("Increase");
            _buttons[3].Texture = _content.Load<Texture2D>("Decrease");
            _buttons[4].Texture = _content.Load<Texture2D>("Increase");
            _buttons[5].Texture = _content.Load<Texture2D>("Decrease");
            _buttons[6].Texture = _content.Load<Texture2D>("Increase");
            _buttons[7].Texture = _content.Load<Texture2D>("Defaults");

            _label = _content.Load<Texture2D>("OptionsMenuLabel");

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

            //_vol[0] = (float)_sfx;
            //_vol[1] = (float)_music;
            //_vol[2] = (float)_master;

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

            SoundEffect.MasterVolume = (float)_music * (float)_master;
            MediaPlayer.Volume = (float)_sfx * (float)_master;

            _masterDisplay = "";
            _musicDisplay = "";
            _sfxDisplay = "";
            for (double i = 0; i < (_master * 100); i += 4.348)
            {
                _masterDisplay += "=";
            }
            for (double i = 0; i < (_sfx * 100); i += 4.348)
            {
                _sfxDisplay += "=";
            }
            for (double i = 0; i < (_music * 100); i += 4.348)
            {
                _musicDisplay += "=";
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

            spriteBatch.Draw(_label, new Vector2(_widthIncrement * 34, _heightIncrement * -7), Color.White);

            spriteBatch.DrawString(_descriptor, "Master Volume", new Vector2(_widthIncrement * 36, 18 * _heightIncrement), Color.White);
            spriteBatch.DrawString(_descriptor, _masterDisplay, new Vector2(_widthIncrement * 36, 23 * _heightIncrement), Color.White);

            spriteBatch.DrawString(_descriptor, "Music Volume", new Vector2(_widthIncrement * 36, 38 * _heightIncrement), Color.White);
            spriteBatch.DrawString(_descriptor, _musicDisplay, new Vector2(_widthIncrement * 36, 43 * _heightIncrement), Color.White);

            spriteBatch.DrawString(_descriptor, "Sound Effects Volume", new Vector2(_widthIncrement * 36, 58 * _heightIncrement), Color.White);
            spriteBatch.DrawString(_descriptor, _sfxDisplay, new Vector2(_widthIncrement * 36, 63 * _heightIncrement), Color.White);

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
                case 0: //back button
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();
                        _vol[0] = (float)_sfx;
                        _vol[1] = (float)_music;
                        _vol[2] = (float)_master;
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new MainMenuScreen(_game, _vol), null);
                    }
                    break;
                case 1: //decrease master
                    if (_master >= 0.02)
                    {
                        _master -= 0.01;
                        _vol[2] -= 0.01f;
                    }
                    else
                    {
                        _master = 0;
                        _vol[2] = 0;
                    }
                    break;
                case 2: //increase master
                    if (_master <= 0.98)
                    {
                        _master += 0.01;
                        _vol[2] += 0.01f;
                    }
                    else
                    {
                        _master = 1;
                        _vol[2] = 1;
                    }
                    break;
                case 3: //decrease music
                    if (_music >= 0.02)
                    {
                        _music -= 0.01;
                        _vol[1] -= 0.01f;
                    }
                    else
                    {
                        _music = 0;
                        _vol[1] = 0;
                    }
                    break;
                case 4: //increase music
                    if (_music <= 0.98)
                    {
                        _music += 0.01;
                        _vol[1] += 0.01f;
                    }
                    else
                    {
                        _music = 1;
                        _vol[1] = 1;
                    }
                    break;
                case 5: //decrease sfx
                    if (_sfx >= 0.02)
                    {
                        _sfx-= 0.01;
                        _vol[0] -= 0.01f;
                    }
                    else
                    {
                        _sfx = 0;
                        _vol[0] = 0;
                    }
                    break;
                case 6: //increase sfx
                    if (_sfx <= 0.98)
                    {
                        _sfx += 0.01;
                        _vol[0] += 0.01f;
                    }
                    else
                    {
                        _sfx = 1;
                        _vol[0] = 1;
                    }
                    break;
                case 7:
                    _master = 0.5;
                    _music = 0.2;
                    _sfx = 0.2;
                    _vol[2] = (float)_master;
                    _vol[1] = (float)_music;
                    _vol[0] = (float)_sfx;
                    break;
            }
        }
    }
}
