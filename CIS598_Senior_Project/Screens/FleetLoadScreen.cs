/* File: FleetLoadScreen.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using CIS598_Senior_Project.StateManagement;
using CIS598_Senior_Project.MenuObjects;
using CIS598_Senior_Project.FleetObjects;
using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;
using CIS598_Senior_Project.FleetObjects.SquadronObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;

namespace CIS598_Senior_Project.Screens
{
    class FleetLoadScreen : GameScreen
    {
        private int _widthIncrement;
        private int _heightIncrement;

        private string _selectedFleet;
        private string _selectedFleet1;
        private string _selectedFleet2;

        private bool _isEdit;
        private bool _isDelete;
        private bool _duelLoad;

        private ContentManager _content;

        private Game _game;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private SoundEffect _button1;
        private SoundEffect _button2;
        private SoundEffect _button3;
        private SoundEffect _button4;

        private List<string> _fleetsList;
        private List<CustButton> _buttons;

        private SpriteFont _galbasic;
        private SpriteFont _descriptor;

        private Texture2D _background;
        private Texture2D _gradient;
        private Texture2D _texture;

        private List<float> _vol;

        private float _pauseAlpha;
        //private readonly InputAction _pauseAction;

        /// <summary>
        /// Fleet load screenconstructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="vol"></param>
        public FleetLoadScreen(Game game, List<float> vol, bool altLoad)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _vol = vol;

            _buttons = new List<CustButton>();

            _game = game;

            _isDelete = false;
            _isEdit = false;
            _duelLoad = altLoad;

            _widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            _buttons.Add(new CustButton(0, new Rectangle(_widthIncrement, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 16, 10 * _widthIncrement, 15 * _heightIncrement), true));       //Back Button
            _buttons.Add(new CustButton(1, new Rectangle(_widthIncrement, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));                                              //Edit Fleet
            _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));                                              //Delete Fleet
            _buttons.Add(new CustButton(3, new Rectangle(_widthIncrement, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));                                             //Confirm yes
            _buttons.Add(new CustButton(4, new Rectangle(_widthIncrement, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));                                             //Confirm no

            _buttons.Add(new CustButton(5, new Rectangle(_widthIncrement * 13, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(6, new Rectangle(_widthIncrement * 13, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(7, new Rectangle(_widthIncrement * 13, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(8, new Rectangle(_widthIncrement * 13, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(9, new Rectangle(_widthIncrement * 13, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(10, new Rectangle(_widthIncrement * 13, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(11, new Rectangle(_widthIncrement * 37, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(12, new Rectangle(_widthIncrement * 37, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(13, new Rectangle(_widthIncrement * 37, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(14, new Rectangle(_widthIncrement * 37, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(15, new Rectangle(_widthIncrement * 37, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(16, new Rectangle(_widthIncrement * 37, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(17, new Rectangle(_widthIncrement * 61, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(18, new Rectangle(_widthIncrement * 61, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(19, new Rectangle(_widthIncrement * 61, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(20, new Rectangle(_widthIncrement * 61, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(21, new Rectangle(_widthIncrement * 61, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(22, new Rectangle(_widthIncrement * 61, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(23, new Rectangle(_widthIncrement * 85, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(24, new Rectangle(_widthIncrement * 85, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(25, new Rectangle(_widthIncrement * 85, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(26, new Rectangle(_widthIncrement * 85, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(27, new Rectangle(_widthIncrement * 85, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(28, new Rectangle(_widthIncrement * 85, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(29, new Rectangle(_widthIncrement, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));             //set player 1 pleet
            _buttons.Add(new CustButton(30, new Rectangle(_widthIncrement, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));        //set player 2 fleet
            _buttons.Add(new CustButton(31, new Rectangle(_widthIncrement, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));        //clear fleets
            _buttons.Add(new CustButton(32, new Rectangle(_widthIncrement, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));        //make selections
        }
    
        /// <summary>
        /// Activates the screen
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _galbasic = _content.Load<SpriteFont>("galbasic");
            _descriptor = _content.Load<SpriteFont>("descriptor");

            _texture = _content.Load<Texture2D>("MetalBackground");
            _background = _content.Load<Texture2D>("LoadBackground");
            _gradient = _content.Load<Texture2D>("MenuGradient2");

            _fleetsList = FleetLoader.AvailableFleets();

            _buttons[0].Texture = _content.Load<Texture2D>("Back");
            _buttons[1].Texture = _content.Load<Texture2D>("EditFleet");
            _buttons[2].Texture = _content.Load<Texture2D>("DeleteFleet");
            _buttons[3].Texture = _content.Load<Texture2D>("Confirm");
            _buttons[4].Texture = _content.Load<Texture2D>("Cancel");

            _buttons[29].Texture = _content.Load<Texture2D>("SetPlayer1");
            _buttons[30].Texture = _content.Load<Texture2D>("SetPlayer2");
            _buttons[31].Texture = _content.Load<Texture2D>("ClearSelected");
            _buttons[32].Texture = _content.Load<Texture2D>("ConfirmSelection");

            for (int i = 0; i < _fleetsList.Count; i++)
            {
                if(_fleetsList[i] != null)
                {
                    _buttons[i + 5].IsActive = true;
                    string[] line = _fleetsList[i].Split(',');
                    if(line[1].Equals("True"))
                    {
                        _buttons[i + 5].Texture = _content.Load<Texture2D>("RebelFleet");
                    }
                    else if(line[1].Equals("False"))
                    {
                        _buttons[i + 5].Texture = _content.Load<Texture2D>("ImperialFleet");
                    }
                }
            }

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
        /// Deactivates the screen
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unloads the screens content
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        /// <summary>
        /// Updates the screen
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="otherScreenHasFocus">If another screen is the focus</param>
        /// <param name="coveredByOtherScreen">if it's covered by another screen</param>
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


            _fleetsList = FleetLoader.AvailableFleets();
            for (int i = 0; i < _fleetsList.Count; i++)
            {
                if (_fleetsList[i] != null)
                {
                    _buttons[i + 5].IsActive = true;

                    string[] line = _fleetsList[i].Split(',');
                    if (line[1].Equals("True"))
                    {
                        _buttons[i + 5].Texture = _content.Load<Texture2D>("RebelFleet");
                    }
                    else if (line[1].Equals("False"))
                    {
                        _buttons[i + 5].Texture = _content.Load<Texture2D>("ImperialFleet");
                    }
                }
            }


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
        /// Handles some input from the player but not a lot
        /// </summary>
        /// <param name="gameTime">The game</param>
        /// <param name="input">Player input objects</param>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = 0;// (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];
        }

        /// <summary>
        /// Draws the game assets
        /// </summary>
        /// <param name="gameTime">The game</param>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height + 3 * _heightIncrement), Color.White);

            spriteBatch.Draw(_gradient, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 18, 0), Color.White);

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

            for(int i = 0; i < 29; i++)
            {
                if(_buttons[i].IsActive)
                {

                    if (i >= 5)
                    {
                        string s = "";
                        if (_fleetsList.Count > 0)
                        {
                            if (_fleetsList[i - 5] != null)
                            {
                                s = _fleetsList[i - 5].Split(',')[0];
                            }
                        }
                        else s = "";

                        Vector2 v = new Vector2(_buttons[i].Position.X + _widthIncrement * 11, _buttons[i].Position.Y);
                        spriteBatch.DrawString(_descriptor, s, v, _buttons[i].Color);
                    }
                }
            }

            if(_selectedFleet != null)
            {
                Fleet fleet = FleetLoader.LoadFleet(_selectedFleet.Split(',')[0], _content);
                double heightoffset = 1;
                spriteBatch.DrawString(_descriptor, "FLEET: " + fleet.Name + "--" + fleet.TotalPoints + " total points", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (int)(heightoffset * _heightIncrement)), Color.AntiqueWhite);

                heightoffset += 1.5;
                foreach(var ship in fleet.Ships)
                {
                    if(ship != null)
                    {
                        if (ship.ShipTypeA)
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(A): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        else
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(B): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        heightoffset += 1.5;

                        if (ship.Commander != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Commander " + ship.Commander.Name + ": " + ship.Commander.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }

                        if (ship.Title != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Title " + ship.Title.Name + ": " + ship.Title.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }

                        foreach (var upgrade in ship.Upgrades)
                        {
                            if (upgrade != null)
                            {
                                spriteBatch.DrawString(_descriptor, "   -" + upgrade.CardType.ToString() + " " + upgrade.Name + ": " + upgrade.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                                heightoffset += 1.5;
                            }
                        }
                    }
                }

                if (fleet.Squadrons.Count > 0)
                {
                    int[] sq = returnSquads(fleet);
                    if (fleet.IsRebelFleet)
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -A-Wing Squadron(11): x" + sq[0] + " => " + sq[0] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -B-Wing Squadron(14): x" + sq[1] + " => " + sq[1] * 14, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -X-Wing Squadron(13): x" + sq[2] + " => " + sq[2] * 13, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -Y-Wing Squadron(10): x" + sq[3] + " => " + sq[3] * 10, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                    else
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Fighter Squadron(8): x" + sq[0] + " => " + sq[0] * 8, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Advanced Squadron(12): x" + sq[1] + " => " + sq[1] * 12, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Interceptor Squadron(11): x" + sq[2] + " => " + sq[2] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Bomber Squadron(9): x" + sq[3] + " => " + sq[3] * 9, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                }
            }

            if (_selectedFleet1 != null)
            {
                Fleet fleet = FleetLoader.LoadFleet(_selectedFleet1.Split(',')[0], _content);
                double heightoffset = 30;
                spriteBatch.DrawString(_descriptor, "Player 1: " + fleet.Name + "--" + fleet.TotalPoints + " total points", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (int)(heightoffset * _heightIncrement)), Color.AntiqueWhite);

                heightoffset += 1.5;
                foreach (var ship in fleet.Ships)
                {
                    if (ship != null)
                    {
                        if (ship.ShipTypeA)
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(A): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        else
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(B): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        heightoffset += 1.5;

                        if (ship.Commander != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Commander " + ship.Commander.Name + ": " + ship.Commander.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }

                        if (ship.Title != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Title " + ship.Title.Name + ": " + ship.Title.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }

                        foreach (var upgrade in ship.Upgrades)
                        {
                            if (upgrade != null)
                            {
                                spriteBatch.DrawString(_descriptor, "   -" + upgrade.CardType.ToString() + " " + upgrade.Name + ": " + upgrade.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                                heightoffset += 1.5;
                            }
                        }
                    }
                }

                if (fleet.Squadrons.Count > 0)
                {
                    int[] sq = returnSquads(fleet);
                    if (fleet.IsRebelFleet)
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -A-Wing Squadron(11): x" + sq[0] + " => " + sq[0] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -B-Wing Squadron(14): x" + sq[1] + " => " + sq[1] * 14, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -X-Wing Squadron(13): x" + sq[2] + " => " + sq[2] * 13, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -Y-Wing Squadron(10): x" + sq[3] + " => " + sq[3] * 10, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                    else
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Fighter Squadron(8): x" + sq[0] + " => " + sq[0] * 8, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Advanced Squadron(12): x" + sq[1] + " => " + sq[1] * 12, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Interceptor Squadron(11): x" + sq[2] + " => " + sq[2] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Bomber Squadron(9): x" + sq[3] + " => " + sq[3] * 9, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                }
            }

            if (_selectedFleet2 != null)
            {
                Fleet fleet = FleetLoader.LoadFleet(_selectedFleet2.Split(',')[0], _content);
                double heightoffset = 60;
                spriteBatch.DrawString(_descriptor, "Player 2: " + fleet.Name + "--" + fleet.TotalPoints + " total points", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (int)(heightoffset * _heightIncrement)), Color.AntiqueWhite);

                heightoffset += 1.5;
                foreach (var ship in fleet.Ships)
                {
                    if (ship != null)
                    {
                        if (ship.ShipTypeA)
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(A): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        else
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(B): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        heightoffset += 1.5;

                        if (ship.Commander != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Commander " + ship.Commander.Name + ": " + ship.Commander.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }

                        if (ship.Title != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Title " + ship.Title.Name + ": " + ship.Title.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }

                        foreach (var upgrade in ship.Upgrades)
                        {
                            if (upgrade != null)
                            {
                                spriteBatch.DrawString(_descriptor, "   -" + upgrade.CardType.ToString() + " " + upgrade.Name + ": " + upgrade.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                                heightoffset += 1.5;
                            }
                        }
                    }
                }

                if (fleet.Squadrons.Count > 0)
                {
                    int[] sq = returnSquads(fleet);
                    if (fleet.IsRebelFleet)
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -A-Wing Squadron(11): x" + sq[0] + " => " + sq[0] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -B-Wing Squadron(14): x" + sq[1] + " => " + sq[1] * 14, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -X-Wing Squadron(13): x" + sq[2] + " => " + sq[2] * 13, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -Y-Wing Squadron(10): x" + sq[3] + " => " + sq[3] * 10, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                    else
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Fighter Squadron(8): x" + sq[0] + " => " + sq[0] * 8, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Advanced Squadron(12): x" + sq[1] + " => " + sq[1] * 12, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Interceptor Squadron(11): x" + sq[2] + " => " + sq[2] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Bomber Squadron(9): x" + sq[3] + " => " + sq[3] * 9, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                }
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        /// <summary>
        /// The even handler for the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCatcher(object sender, ButtonClickedEventArgs e)
        {
            CustButton button = (CustButton)sender;


            switch(button.Id)
            {
                case 0: //back button
                    Fleet p1;
                    Fleet p2;
                    _button4.Play();
                    if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();
                        if(_duelLoad)
                        {
                            if(_selectedFleet1 != null)
                            {
                                p1 = FleetLoader.LoadFleet(_selectedFleet1.Split(',')[0], _content);
                            }
                            else
                            {
                                p1 = null;
                            }

                            if (_selectedFleet2 != null)
                            {
                                p2 = FleetLoader.LoadFleet(_selectedFleet2.Split(',')[0], _content);
                            }
                            else
                            {
                                p2 = null;
                            }

                            ScreenManager.AddScreen(new BackgroundScreen(), null);
                            ScreenManager.AddScreen(new DuelFleetSelectionScreen(_game, _vol, p1, p2), null);
                        }
                        else
                        {
                            ScreenManager.AddScreen(new BackgroundScreen(), null);
                            ScreenManager.AddScreen(new FleetCustomizationMenuScreen(_game, _vol), null);
                        }
                    }
                    break;
                case 1: //edit fleet
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button2.Play();
                    _isEdit = true;
                    _isDelete = false;
                    _buttons[3].IsActive = true;
                    _buttons[4].IsActive = true;
                    break;
                case 2: //delete fleet
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button2.Play();
                    _isEdit = false;
                    _isDelete = true;
                    _buttons[3].IsActive = true;
                    _buttons[4].IsActive = true;
                    break;
                case 3: //confirm
                    _button3.Play();
                    if (_isDelete) 
                    {
                        FleetLoader.DeleteFleet(_selectedFleet);
                        _fleetsList.Remove(_selectedFleet);
                        _selectedFleet = null;
                        resetButtons();
                    } 
                    else if(_isEdit)
                    {
                        if (_currentMouseState.LeftButton == ButtonState.Pressed)// && _previousMouseState.LeftButton == ButtonState.Pressed)
                        {
                            Fleet fleet = FleetLoader.LoadFleet(_selectedFleet.Split(',')[0], _content);

                            bool result = FleetLoader.DeleteFleet(_selectedFleet);

                            _buttons[_fleetsList.IndexOf(_selectedFleet) + 5].IsActive = false;
                            //_fleetsList.Remove(_selectedFleet);

                            _selectedFleet = null;

                            Thread.Sleep(200);
                            ScreenManager.Game.ResetElapsedTime();

                            ScreenManager.AddScreen(new BackgroundScreen(), null);
                            ScreenManager.AddScreen(new FleetCustomizationScreen(_game, fleet, _vol), null);
                        }
                    }
                    _buttons[3].IsActive = false;
                    _buttons[4].IsActive = false;
                    break;
                case 4: //cancel
                    _button3.Play();
                    _isEdit = false;
                    _isDelete = false;
                    _buttons[3].IsActive = false;
                    _buttons[4].IsActive = false;
                    break;
                case 5:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if(_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 6:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 7:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 8:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 9:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 10:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 11:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 12:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 13:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 14:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 15:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 16:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 17:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 18:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 19:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 20:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 21:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 22:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 23:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 24:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 25:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 26:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 27:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 28:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button1.Play();
                    fleetSelect(button.Id);
                    if (_duelLoad)
                    {
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                    }
                    break;
                case 29: //set player 1
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button2.Play();
                    _selectedFleet1 = _selectedFleet;
                    _selectedFleet = null;
                    _buttons[31].IsActive = true;
                    if(_selectedFleet1 != null && _selectedFleet2 != null)
                    {
                        _buttons[32].IsActive = true;
                    }
                    Thread.Sleep(200);
                    break;
                case 30: //set player 2
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) _button2.Play();
                    _selectedFleet2 = _selectedFleet;
                    _selectedFleet = null;
                    _buttons[31].IsActive = true;
                    if (_selectedFleet1 != null && _selectedFleet2 != null)
                    {
                        _buttons[32].IsActive = true;
                    }
                    Thread.Sleep(200);
                    break;
                case 31: //clear set
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();
                        Thread.Sleep(200);
                        _selectedFleet1 = null;
                        _selectedFleet2 = null;
                        _buttons[31].IsActive = false;
                        _buttons[32].IsActive = false;
                    }
                    break;
                case 32: //confirm selection
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Fleet p11 = FleetLoader.LoadFleet(_selectedFleet1.Split(',')[0], _content);
                        Fleet p21 = FleetLoader.LoadFleet(_selectedFleet2.Split(',')[0], _content);
                        _button4.Play();
                        Thread.Sleep(200);

                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new DuelFleetSelectionScreen(_game, _vol, p11, p21), null);
                    }
                    break;
            }
        }

        /// <summary>
        /// A sweeper method that moves through and switches uneeded buttons to inactive.
        /// </summary>
        /// <param name="index">The index to start the sweep at.</param>
        private void buttonSweeper(int index)
        {
            for (int i = index; i < _buttons.Count; i++)
            {
                _buttons[i].IsActive = false;
            }
        }

        /// <summary>
        /// Selects the fleet to be loaded
        /// </summary>
        /// <param name="id">the button's id</param>
        private void fleetSelect(int id)
        {
            _selectedFleet = _fleetsList[id - 5];
            if(!_duelLoad)
            {
                _buttons[1].IsActive = true;
                _buttons[2].IsActive = true;
                _buttons[3].IsActive = false;
                _buttons[4].IsActive = false;
            }
            else
            {
                _buttons[1].IsActive = false;
                _buttons[2].IsActive = false;
                _buttons[3].IsActive = false;
                _buttons[4].IsActive = false;

                _buttons[29].IsActive = true;
                _buttons[30].IsActive = true;
                //_buttons[31].IsActive = false;
                _buttons[32].IsActive = false;
            }
        }

        /// <summary>
        /// Resets the buttons
        /// </summary>
        private void resetButtons()
        {
            buttonSweeper(5);

            _fleetsList = FleetLoader.AvailableFleets();
            for (int i = 0; i < _fleetsList.Count; i++)
            {
                if (_fleetsList[i] != null)
                {
                    _buttons[i + 5].IsActive = true;

                    string[] line = _fleetsList[i].Split(',');
                    if (line[1].Equals("True"))
                    {
                        _buttons[i + 5].Texture = _content.Load<Texture2D>("RebelFleet");
                    }
                    else if (line[1].Equals("False"))
                    {
                        _buttons[i + 5].Texture = _content.Load<Texture2D>("ImperialFleet");
                    }
                }
            }
        }

        /// <summary>
        /// gets the number of squads of each type
        /// </summary>
        /// <returns>the list of number of squads by type</returns>
        private int[] returnSquads(Fleet fleet)
        {
            int[] result = new int[4];
            if (fleet.IsRebelFleet)
            {
                result[0] += fleet.Squadrons.FindAll(x => x.Name == "A-Wing Squadron").Count;
                result[1] += fleet.Squadrons.FindAll(x => x.Name == "B-Wing Squadron").Count;
                result[2] += fleet.Squadrons.FindAll(x => x.Name == "X-Wing Squadron").Count;
                result[3] += fleet.Squadrons.FindAll(x => x.Name == "Y-Wing Squadron").Count;
            }
            else
            {
                result[0] += fleet.Squadrons.FindAll(x => x.Name == "TIE Fighter Squadron").Count;
                result[1] += fleet.Squadrons.FindAll(x => x.Name == "TIE Advanced Squadron").Count;
                result[2] += fleet.Squadrons.FindAll(x => x.Name == "TIE Interceptor Squadron").Count;
                result[3] += fleet.Squadrons.FindAll(x => x.Name == "TIE Bomber Squadron").Count;
            }

            return result;
        }
    }
}
