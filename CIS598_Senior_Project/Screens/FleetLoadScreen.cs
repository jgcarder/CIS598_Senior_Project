﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private bool _isEdit;
        private bool _isDelete;

        private ContentManager _content;

        private Game _game;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private List<string> _fleetsList;
        private List<CustButton> _buttons;

        private SpriteFont _galbasic;
        private SpriteFont _descriptor;

        private Texture2D _background;
        private Texture2D _gradient;
        private Texture2D _texture;

        private List<float> _vol;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public FleetLoadScreen(Game game, List<float> vol)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _vol = vol;

            _buttons = new List<CustButton>();

            _game = game;

            _isDelete = false;
            _isEdit = false;

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

        }
    

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

            for(int i = 0; i < _fleetsList.Count; i++)
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

            foreach (var button in _buttons)
            {
                button.AnAction += ButtonCatcher;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

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

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(), Color.White);

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

                    if(i >= 5)
                    {
                        string s;
                        if (_fleetsList.Count > 0) s = _fleetsList[i - 5].Split(',')[0];
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
                    if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(200);
                        ScreenManager.Game.ResetElapsedTime();

                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new FleetCustomizationMenuScreen(_game, _vol), null);
                    }
                    break;
                case 1: //edit fleet
                    _isEdit = true;
                    _isDelete = false;
                    _buttons[3].IsActive = true;
                    _buttons[4].IsActive = true;
                    break;
                case 2: //delete fleet
                    _isEdit = false;
                    _isDelete = true;
                    _buttons[3].IsActive = true;
                    _buttons[4].IsActive = true;
                    break;
                case 3: //confirm
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
                    _isEdit = false;
                    _isDelete = false;
                    _buttons[3].IsActive = false;
                    _buttons[4].IsActive = false;
                    break;
                case 5:
                    fleetSelect(button.Id);
                    break;
                case 6:
                    fleetSelect(button.Id);
                    break;
                case 7:
                    fleetSelect(button.Id);
                    break;
                case 8:
                    fleetSelect(button.Id);
                    break;
                case 9:
                    fleetSelect(button.Id);
                    break;
                case 10:
                    fleetSelect(button.Id);
                    break;
                case 11:
                    fleetSelect(button.Id);
                    break;
                case 12:
                    fleetSelect(button.Id);
                    break;
                case 13:
                    fleetSelect(button.Id);
                    break;
                case 14:
                    fleetSelect(button.Id);
                    break;
                case 15:
                    fleetSelect(button.Id);
                    break;
                case 16:
                    fleetSelect(button.Id);
                    break;
                case 17:
                    fleetSelect(button.Id);
                    break;
                case 18:
                    fleetSelect(button.Id);
                    break;
                case 19:
                    fleetSelect(button.Id);
                    break;
                case 20:
                    fleetSelect(button.Id);
                    break;
                case 21:
                    fleetSelect(button.Id);
                    break;
                case 22:
                    fleetSelect(button.Id);
                    break;
                case 23:
                    fleetSelect(button.Id);
                    break;
                case 24:
                    fleetSelect(button.Id);
                    break;
                case 25:
                    fleetSelect(button.Id);
                    break;
                case 26:
                    fleetSelect(button.Id);
                    break;
                case 27:
                    fleetSelect(button.Id);
                    break;
                case 28:
                    fleetSelect(button.Id);
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
            _buttons[1].IsActive = true;
            _buttons[2].IsActive = true;
            _buttons[3].IsActive = false;
            _buttons[4].IsActive = false;

            //buttonSweeper(5);
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
