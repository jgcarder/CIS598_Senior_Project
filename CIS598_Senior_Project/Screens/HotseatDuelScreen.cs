﻿using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.StateManagement;
using CIS598_Senior_Project.MenuObjects;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using CIS598_Senior_Project.FleetObjects;
using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;
using CIS598_Senior_Project.FleetObjects.SquadronObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;

namespace CIS598_Senior_Project.Screens
{
    public class HotseatDuelScreen : GameScreen
    {
        private Game _game;

        private Fleet _player1;
        private Fleet _player2;

        private List<CustButton> _buttons;

        private ContentManager _content;

        private Texture2D _background;
        private Texture2D _texture;
        private Texture2D _label;

        private SpriteFont _descriptor;

        private int _widthIncrement;
        private int _heightIncrement;
        private int _roundNum;

        private bool _player1Turn;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private SoundEffect _button1;
        private SoundEffect _button2;
        private SoundEffect _button3;
        private SoundEffect _button4;

        private List<float> _vol;

        private GameEnum _state;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public HotseatDuelScreen(Game game, List<float> vol, Fleet player1, Fleet player2)
        {
            _game = game;
            _vol = vol;
            _player1 = player1;
            _player2 = player2;

            _state = GameEnum.Setup;

            _roundNum = 0;

            _widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            _buttons = new List<CustButton>();
            //Buttons go here
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

            //Load button textures here
            _buttons[0].Texture = _content.Load<Texture2D>("");
            _buttons[1].Texture = _content.Load<Texture2D>("");
            _buttons[2].Texture = _content.Load<Texture2D>("");
            _buttons[3].Texture = _content.Load<Texture2D>("");

            _label = _content.Load<Texture2D>("");
            _background = _content.Load<Texture2D>("");

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

            switch(_state) {
                case GameEnum.Setup:
                    //Determine who goes first
                        //lowest fleet picks who goes first - 2
                    //placing your ships/squads
                        //take turns placing ships
                        //or 2 squads at a time
                            //if you have an odd number of squads you must finish ships first
                    break;
                case GameEnum.Command_Phase:
                    //Setting your command dials
                        //queue up a dial - 5
                    break;
                case GameEnum.Ship_Phase:
                    //Taking turns activating ships ~
                        //Reveal command dial
                            //spend dial - 1
                                //greater immediate effect
                            //convert to token - 5
                                //use later
                        //Attack
                            //Declare target ~ 
                                //if a squadron they must be within distance 1
                            //Roll dice
                                //if squadrin, use anti squadron dice
                            //Resolve attack effects
                                //modify dice
                                    //reroll
                                    //add
                                    //change ~
                                    //spend
                                    //cancel ~
                                //Spend Accuracy icons - 4
                                //Defense spends defense tokens - 4
                                //Resolve damage
                                    //if a squadron only hits count
                                    //if a ship hits and crits count
                        //Execute manuver
                            //Determine course
                                //consult card and buttons for speed - 10
                            //Move ship
                                //click move button - 1
                    break;
                case GameEnum.Squadron_Phase:
                    //activateing anf either moving or attacking with them
                        //if engaged it cannot move and must attack the other squadron
                        //otherwise move
                        //otherwise attack
                    break;
                case GameEnum.Status_Phase:
                    //reset defense tokens
                    break;
            }
























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
            spriteBatch.Draw(_background, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height + _heightIncrement * 3), Color.White);

            spriteBatch.Draw(_label, new Vector2(_widthIncrement * 34, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 90), Color.White);

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
                    _button4.Play();
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();

                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new PlayMenuScreen(_game, _vol), null);
                    }
                    break;
                case 1:
                    _button1.Play();
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();

                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new FleetLoadScreen(_game, _vol, true), null);
                    }
                    break;
                case 2:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        Thread.Sleep(200);
                        _player1 = null;
                        _player2 = null;
                        _buttons[2].IsActive = false;
                        _buttons[3].IsActive = false;
                    }
                    break;
                case 3:
                    _button3.Play();
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();

                        //ScreenManager.AddScreen(new BackgroundScreen(), null);
                        //ScreenManager.AddScreen(new FleetLoadScreen(_game, _vol, true), null);
                    }
                    break;
            }
        }
    }
}
