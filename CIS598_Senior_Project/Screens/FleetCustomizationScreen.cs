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
using CIS598_Senior_Project.FleetObjects;
using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;
using CIS598_Senior_Project.FleetObjects.SquadronObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;

namespace CIS598_Senior_Project.Screens
{
    public class FleetCustomizationScreen : GameScreen
    {
        enum SelectedUpgradeType
        {
            Commander,
            Title,
            Officers,
            SupportTeam,
            WeaponsTeam,
            OffensiveRetrofit,
            DefensiveRetrofit,
            IonCannon,
            Ordinance,
            Turbolasers,
            None
        }

        private ContentManager _content;
        private SpriteFont _gameFont;

        private int _numSquads;
        private int _shipID;
        private int _squadronID;
        private int widthIncrement;
        private int heightIncrement;

        private string _fleetName;
        private string _fleetDeats;
        private string _cardDeats;

        private Texture2D _texture;
        private Texture2D _background;
        private Texture2D _gradient;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private MouseState _previousMouseState;
        private MouseState _currentMouseState;
        private KeyboardState _previousKeyState;
        private KeyboardState _currentKeyState;

        private List<CustButton> _buttons;

        private Fleet _fleet;
        private Ship _selectedShip;
        private UpgradeCard _selectedUpgrade;
        private UpgradeCard _previousUpgrade;
        private Squadron _selectedSquadron;

        private SpriteFont _galbasic;

        private SelectedUpgradeType _selectedUpgradeType;

        private GraphicsDevice _graphics;

        private Game _game;

        public FleetCustomizationScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _numSquads = 0;
            _shipID = 0;
            _squadronID = 0;

            _fleetName = "<Fleet Name>";
            _fleetDeats = "";
            _cardDeats = "";

            _game = game;

            _buttons = new List<CustButton>();

            _selectedUpgradeType = SelectedUpgradeType.None;

            //_graphics = ScreenManager.GraphicsDevice;

            _fleet = new Fleet("");

            widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            //_buttons.Add(new CustButton(0, new Rectangle(), true)); //Fleet ships
            //_buttons.Add(new CustButton(0, new Rectangle(), true)); //Fleet squadrons
            //_buttons.Add(new CustButton(0, new Rectangle(), true)); //Add button whose role changes 
            //_buttons.Add(new CustButton(0, new Rectangle(), true)); //Remove button whose role changes
            //_buttons.Add(new CustButton(0, new Rectangle(), true)); //Select upgrades(Title, Officer, Weapons team, offensive retrofit, ordinance, turbolasers, ion cannon, defensive retrofit, support team)

            _buttons.Add(new CustButton(0, new Rectangle(widthIncrement, heightIncrement, 10 * widthIncrement, 15 * heightIncrement), true));                                               //Save and quit
            _buttons.Add(new CustButton(1, new Rectangle(widthIncrement, 18 * heightIncrement, 10 * widthIncrement, 15 * heightIncrement), true));                                          //Clear fleet
            _buttons.Add(new CustButton(2, new Rectangle(widthIncrement, 35 * heightIncrement, 10 * widthIncrement, 15 * heightIncrement), true));                                          //Select rebel fleet
            _buttons.Add(new CustButton(3, new Rectangle(widthIncrement, 52 * heightIncrement, 10 * widthIncrement, 15 * heightIncrement), true));                                          //Select imperial fleet
            _buttons.Add(new CustButton(4, new Rectangle(widthIncrement, _game.GraphicsDevice.Viewport.Height - heightIncrement * 16, 10 * widthIncrement, 15 * heightIncrement), true));   //Instructions on how this works

            _buttons.Add(new CustButton(5, new Rectangle(widthIncrement * 13, heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                             //Select rebel ships
            _buttons.Add(new CustButton(6, new Rectangle(widthIncrement * 13, heightIncrement * 18, 10 * widthIncrement, 15 * heightIncrement), false));                                        //Select rebel squadrons
            _buttons.Add(new CustButton(7, new Rectangle(widthIncrement * 13, heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                             //Select imperial ships
            _buttons.Add(new CustButton(8, new Rectangle(widthIncrement * 13, heightIncrement * 18, 10 * widthIncrement, 15 * heightIncrement), false));                                        //Select imperial squadrons
            
            _buttons.Add(new CustButton(9, new Rectangle(widthIncrement * 13, heightIncrement * 35, 10 * widthIncrement, 15 * heightIncrement), false));                                            //Select Assault Frigate Mark II
            _buttons.Add(new CustButton(10, new Rectangle(widthIncrement * 13, heightIncrement * 52, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select CR90 Corellian Corvette
            _buttons.Add(new CustButton(11, new Rectangle(widthIncrement * 13, heightIncrement * 69, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select Nebulon B Frigate
            _buttons.Add(new CustButton(12, new Rectangle(widthIncrement * 13, heightIncrement * 35, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select Gladiator SD
            _buttons.Add(new CustButton(13, new Rectangle(widthIncrement * 13, heightIncrement * 52, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select Victory SD

            _buttons.Add(new CustButton(14, new Rectangle(widthIncrement * 13, heightIncrement * 35, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select A-wing squadron
            _buttons.Add(new CustButton(15, new Rectangle(widthIncrement * 13, heightIncrement * 52, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select B-wing squadron
            _buttons.Add(new CustButton(16, new Rectangle(widthIncrement * 13, heightIncrement * 69, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select X-wing squadron
            _buttons.Add(new CustButton(17, new Rectangle(widthIncrement * 13, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select Y-wing squadron
            _buttons.Add(new CustButton(18, new Rectangle(widthIncrement * 13, heightIncrement * 35, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select tie fighter squadron
            _buttons.Add(new CustButton(19, new Rectangle(widthIncrement * 13, heightIncrement * 52, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select tie advanced squadron
            _buttons.Add(new CustButton(20, new Rectangle(widthIncrement * 13, heightIncrement * 69, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select tie interceptor squadron
            _buttons.Add(new CustButton(21, new Rectangle(widthIncrement * 13, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                           //Select tie bomber squadron

            _buttons.Add(new CustButton(22, new Rectangle(widthIncrement * 25, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select Mark II A
            _buttons.Add(new CustButton(23, new Rectangle(widthIncrement * 37, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select Mark II B
            _buttons.Add(new CustButton(24, new Rectangle(widthIncrement * 25, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select version A
            _buttons.Add(new CustButton(25, new Rectangle(widthIncrement * 37, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select version B
            _buttons.Add(new CustButton(26, new Rectangle(widthIncrement * 25, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select escort refit
            _buttons.Add(new CustButton(27, new Rectangle(widthIncrement * 37, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select support refit
            _buttons.Add(new CustButton(28, new Rectangle(widthIncrement * 25, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select gladiator class I
            _buttons.Add(new CustButton(29, new Rectangle(widthIncrement * 37, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select gladiator class II
            _buttons.Add(new CustButton(30, new Rectangle(widthIncrement * 25, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select victory class I
            _buttons.Add(new CustButton(31, new Rectangle(widthIncrement * 37, heightIncrement, 10 * widthIncrement, 32 * heightIncrement), false));                                                    //Select victory class II

            _buttons.Add(new CustButton(32, new Rectangle(widthIncrement * 25, heightIncrement * 35, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Title
            _buttons.Add(new CustButton(33, new Rectangle(widthIncrement * 37, heightIncrement * 35, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Officer
            _buttons.Add(new CustButton(34, new Rectangle(widthIncrement * 25, heightIncrement * 52, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Weapons Team
            _buttons.Add(new CustButton(35, new Rectangle(widthIncrement * 37, heightIncrement * 52, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Support Team
            _buttons.Add(new CustButton(36, new Rectangle(widthIncrement * 25, heightIncrement * 69, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Offencive Retrofit
            _buttons.Add(new CustButton(37, new Rectangle(widthIncrement * 37, heightIncrement * 69, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Defencive Retrofit
            _buttons.Add(new CustButton(38, new Rectangle(widthIncrement * 25, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Ion Cannon
            _buttons.Add(new CustButton(39, new Rectangle(widthIncrement * 37, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Turbo Laser
            _buttons.Add(new CustButton(40, new Rectangle(widthIncrement * 49, heightIncrement * 69, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Ordinance
            _buttons.Add(new CustButton(41, new Rectangle(widthIncrement * 49, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Commander

            _buttons.Add(new CustButton(42, new Rectangle(widthIncrement * 61, heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));                                                             //Upgrade select
            _buttons.Add(new CustButton(43, new Rectangle(widthIncrement * 61, 15 * heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));
            _buttons.Add(new CustButton(44, new Rectangle(widthIncrement * 61, 29 * heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));
            _buttons.Add(new CustButton(45, new Rectangle(widthIncrement * 61, 43 * heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));
            _buttons.Add(new CustButton(46, new Rectangle(widthIncrement * 61, 57 * heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));
            _buttons.Add(new CustButton(47, new Rectangle(widthIncrement * 61, 71 * heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));
            _buttons.Add(new CustButton(48, new Rectangle(widthIncrement * 61, 85 * heightIncrement, 6 * widthIncrement, 12 * heightIncrement), false));

            _buttons.Add(new CustButton(49, new Rectangle(widthIncrement * 49, heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                            //Add ship to fleet button
            _buttons.Add(new CustButton(50, new Rectangle(widthIncrement * 49, 18 * heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                       //clear ship upgrades button

            _buttons.Add(new CustButton(51, new Rectangle(widthIncrement * 25, heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                            //Increase squadrons to add
            _buttons.Add(new CustButton(52, new Rectangle(widthIncrement * 25, 35 * heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                       //Decrease squadrons to add
            _buttons.Add(new CustButton(53, new Rectangle(widthIncrement * 25, 52 * heightIncrement, 10 * widthIncrement, 15 * heightIncrement), false));                                       //Add squadrons to fleet

            _buttons.Add(new CustButton(54, new Rectangle(_game.GraphicsDevice.Viewport.Width - widthIncrement * 14, 8 * heightIncrement, 10 * widthIncrement, 7 * heightIncrement), true));    //Button to start editing the fleet name
            _buttons.Add(new CustButton(55, new Rectangle(_game.GraphicsDevice.Viewport.Width - widthIncrement * 14, 16 * heightIncrement, 10 * widthIncrement, 7 * heightIncrement), false));  //button to set the fleet name


        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("bangersMenuFont");
            _texture = _content.Load<Texture2D>("MetalBackground");
            _background = _content.Load<Texture2D>("FleetEditBackground");
            _gradient = _content.Load<Texture2D>("MenuGradient2");
            _galbasic = _content.Load<SpriteFont>("galbasic");

            foreach(var button in _buttons)
            {
                button.AnAction += ButtonCatcher;
            }

            //_button = new Button(1, new Vector2(100, 100));
            //_button.TouchArea = new Rectangle(100, 100, 50, 50);
            //_button.AnAction += ButtonCatcher;

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                foreach (var button in _buttons) 
                {
                    if(button.IsActive)
                    {
                        if (_currentMouseState.X >= button.Position.X && _currentMouseState.X <= button.Position.X + button.Area.Width
                                            && _currentMouseState.Y >= button.Position.Y && _currentMouseState.Y <= button.Position.Y + button.Area.Height)
                        {
                            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                            {
                                button.Color = Color.DarkSlateGray;
                                button.AnAction(button, new ButtonClickedEventArgs() { Id = button.Id });
                                if (button.Id > 21 && button.Id < 51) _selectedShip.refreshShip();
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
                    }
                }
            }

            if(_buttons[55].IsActive)
            {
                if(_currentKeyState.IsKeyDown(Keys.LeftShift) || _currentKeyState.IsKeyDown(Keys.RightShift))
                {
                    if (_currentKeyState.IsKeyDown(Keys.A) && _previousKeyState.IsKeyUp(Keys.A)) _fleetName += "A";
                    if (_currentKeyState.IsKeyDown(Keys.B) && _previousKeyState.IsKeyUp(Keys.B)) _fleetName += "B";
                    if (_currentKeyState.IsKeyDown(Keys.C) && _previousKeyState.IsKeyUp(Keys.C)) _fleetName += "C";
                    if (_currentKeyState.IsKeyDown(Keys.D) && _previousKeyState.IsKeyUp(Keys.D)) _fleetName += "D";
                    if (_currentKeyState.IsKeyDown(Keys.E) && _previousKeyState.IsKeyUp(Keys.E)) _fleetName += "E";
                    if (_currentKeyState.IsKeyDown(Keys.F) && _previousKeyState.IsKeyUp(Keys.F)) _fleetName += "F";
                    if (_currentKeyState.IsKeyDown(Keys.G) && _previousKeyState.IsKeyUp(Keys.G)) _fleetName += "G";
                    if (_currentKeyState.IsKeyDown(Keys.H) && _previousKeyState.IsKeyUp(Keys.H)) _fleetName += "H";
                    if (_currentKeyState.IsKeyDown(Keys.I) && _previousKeyState.IsKeyUp(Keys.I)) _fleetName += "I";
                    if (_currentKeyState.IsKeyDown(Keys.J) && _previousKeyState.IsKeyUp(Keys.J)) _fleetName += "J";
                    if (_currentKeyState.IsKeyDown(Keys.K) && _previousKeyState.IsKeyUp(Keys.K)) _fleetName += "K";
                    if (_currentKeyState.IsKeyDown(Keys.L) && _previousKeyState.IsKeyUp(Keys.L)) _fleetName += "L";
                    if (_currentKeyState.IsKeyDown(Keys.M) && _previousKeyState.IsKeyUp(Keys.M)) _fleetName += "M";
                    if (_currentKeyState.IsKeyDown(Keys.N) && _previousKeyState.IsKeyUp(Keys.N)) _fleetName += "N";
                    if (_currentKeyState.IsKeyDown(Keys.O) && _previousKeyState.IsKeyUp(Keys.O)) _fleetName += "O";
                    if (_currentKeyState.IsKeyDown(Keys.P) && _previousKeyState.IsKeyUp(Keys.P)) _fleetName += "P";
                    if (_currentKeyState.IsKeyDown(Keys.Q) && _previousKeyState.IsKeyUp(Keys.Q)) _fleetName += "Q";
                    if (_currentKeyState.IsKeyDown(Keys.R) && _previousKeyState.IsKeyUp(Keys.R)) _fleetName += "R";
                    if (_currentKeyState.IsKeyDown(Keys.S) && _previousKeyState.IsKeyUp(Keys.S)) _fleetName += "S";
                    if (_currentKeyState.IsKeyDown(Keys.T) && _previousKeyState.IsKeyUp(Keys.T)) _fleetName += "T";
                    if (_currentKeyState.IsKeyDown(Keys.U) && _previousKeyState.IsKeyUp(Keys.U)) _fleetName += "U";
                    if (_currentKeyState.IsKeyDown(Keys.V) && _previousKeyState.IsKeyUp(Keys.V)) _fleetName += "V";
                    if (_currentKeyState.IsKeyDown(Keys.W) && _previousKeyState.IsKeyUp(Keys.W)) _fleetName += "W";
                    if (_currentKeyState.IsKeyDown(Keys.X) && _previousKeyState.IsKeyUp(Keys.X)) _fleetName += "X";
                    if (_currentKeyState.IsKeyDown(Keys.Y) && _previousKeyState.IsKeyUp(Keys.Y)) _fleetName += "Y";
                    if (_currentKeyState.IsKeyDown(Keys.Z) && _previousKeyState.IsKeyUp(Keys.Z)) _fleetName += "Z";
                }
                else
                {
                    if (_currentKeyState.IsKeyDown(Keys.A) && _previousKeyState.IsKeyUp(Keys.A)) _fleetName += "a";
                    if (_currentKeyState.IsKeyDown(Keys.B) && _previousKeyState.IsKeyUp(Keys.B)) _fleetName += "b";
                    if (_currentKeyState.IsKeyDown(Keys.C) && _previousKeyState.IsKeyUp(Keys.C)) _fleetName += "c";
                    if (_currentKeyState.IsKeyDown(Keys.D) && _previousKeyState.IsKeyUp(Keys.D)) _fleetName += "d";
                    if (_currentKeyState.IsKeyDown(Keys.E) && _previousKeyState.IsKeyUp(Keys.E)) _fleetName += "e";
                    if (_currentKeyState.IsKeyDown(Keys.F) && _previousKeyState.IsKeyUp(Keys.F)) _fleetName += "f";
                    if (_currentKeyState.IsKeyDown(Keys.G) && _previousKeyState.IsKeyUp(Keys.G)) _fleetName += "g";
                    if (_currentKeyState.IsKeyDown(Keys.H) && _previousKeyState.IsKeyUp(Keys.H)) _fleetName += "h";
                    if (_currentKeyState.IsKeyDown(Keys.I) && _previousKeyState.IsKeyUp(Keys.I)) _fleetName += "i";
                    if (_currentKeyState.IsKeyDown(Keys.J) && _previousKeyState.IsKeyUp(Keys.J)) _fleetName += "j";
                    if (_currentKeyState.IsKeyDown(Keys.K) && _previousKeyState.IsKeyUp(Keys.K)) _fleetName += "k";
                    if (_currentKeyState.IsKeyDown(Keys.L) && _previousKeyState.IsKeyUp(Keys.L)) _fleetName += "l";
                    if (_currentKeyState.IsKeyDown(Keys.M) && _previousKeyState.IsKeyUp(Keys.M)) _fleetName += "m";
                    if (_currentKeyState.IsKeyDown(Keys.N) && _previousKeyState.IsKeyUp(Keys.N)) _fleetName += "n";
                    if (_currentKeyState.IsKeyDown(Keys.O) && _previousKeyState.IsKeyUp(Keys.O)) _fleetName += "o";
                    if (_currentKeyState.IsKeyDown(Keys.P) && _previousKeyState.IsKeyUp(Keys.P)) _fleetName += "p";
                    if (_currentKeyState.IsKeyDown(Keys.Q) && _previousKeyState.IsKeyUp(Keys.Q)) _fleetName += "q";
                    if (_currentKeyState.IsKeyDown(Keys.R) && _previousKeyState.IsKeyUp(Keys.R)) _fleetName += "r";
                    if (_currentKeyState.IsKeyDown(Keys.S) && _previousKeyState.IsKeyUp(Keys.S)) _fleetName += "s";
                    if (_currentKeyState.IsKeyDown(Keys.T) && _previousKeyState.IsKeyUp(Keys.T)) _fleetName += "t";
                    if (_currentKeyState.IsKeyDown(Keys.U) && _previousKeyState.IsKeyUp(Keys.U)) _fleetName += "u";
                    if (_currentKeyState.IsKeyDown(Keys.V) && _previousKeyState.IsKeyUp(Keys.V)) _fleetName += "v";
                    if (_currentKeyState.IsKeyDown(Keys.W) && _previousKeyState.IsKeyUp(Keys.W)) _fleetName += "w";
                    if (_currentKeyState.IsKeyDown(Keys.X) && _previousKeyState.IsKeyUp(Keys.X)) _fleetName += "x";
                    if (_currentKeyState.IsKeyDown(Keys.Y) && _previousKeyState.IsKeyUp(Keys.Y)) _fleetName += "y";
                    if (_currentKeyState.IsKeyDown(Keys.Z) && _previousKeyState.IsKeyUp(Keys.Z)) _fleetName += "z";
                }
                if (_currentKeyState.IsKeyDown(Keys.Space) && _previousKeyState.IsKeyUp(Keys.Space)) _fleetName += " ";
                if (_currentKeyState.IsKeyDown(Keys.NumPad0) && _previousKeyState.IsKeyUp(Keys.NumPad0)) _fleetName += "0";
                if (_currentKeyState.IsKeyDown(Keys.NumPad1) && _previousKeyState.IsKeyUp(Keys.NumPad1)) _fleetName += "1";
                if (_currentKeyState.IsKeyDown(Keys.NumPad2) && _previousKeyState.IsKeyUp(Keys.NumPad2)) _fleetName += "2";
                if (_currentKeyState.IsKeyDown(Keys.NumPad3) && _previousKeyState.IsKeyUp(Keys.NumPad3)) _fleetName += "3";
                if (_currentKeyState.IsKeyDown(Keys.NumPad4) && _previousKeyState.IsKeyUp(Keys.NumPad4)) _fleetName += "4";
                if (_currentKeyState.IsKeyDown(Keys.NumPad5) && _previousKeyState.IsKeyUp(Keys.NumPad5)) _fleetName += "5";
                if (_currentKeyState.IsKeyDown(Keys.NumPad6) && _previousKeyState.IsKeyUp(Keys.NumPad6)) _fleetName += "6";
                if (_currentKeyState.IsKeyDown(Keys.NumPad7) && _previousKeyState.IsKeyUp(Keys.NumPad7)) _fleetName += "7";
                if (_currentKeyState.IsKeyDown(Keys.NumPad8) && _previousKeyState.IsKeyUp(Keys.NumPad8)) _fleetName += "8";
                if (_currentKeyState.IsKeyDown(Keys.NumPad9) && _previousKeyState.IsKeyUp(Keys.NumPad9)) _fleetName += "9";
                if (_currentKeyState.IsKeyDown(Keys.Back) && _previousKeyState.IsKeyUp(Keys.Back) && _fleetName.Length > 0) _fleetName = _fleetName.Remove(_fleetName.Length - 1);
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

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
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var source = new Rectangle(100, 100, 50, 50);
            //_button.TouchArea = source;

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            spriteBatch.Draw(_gradient, new Vector2(_game.GraphicsDevice.Viewport.Width - widthIncrement * 18, 0), Color.White);

            spriteBatch.DrawString(_galbasic, _fleetName, new Vector2(_game.GraphicsDevice.Viewport.Width - widthIncrement * 16, 3 * heightIncrement), Color.AntiqueWhite);

            foreach (var button in _buttons)
            {
                if (button.IsActive)
                {
                    //spriteBatch.Draw(button.texture, button.Area, button.Color);
                    spriteBatch.Draw(_texture, button.Area, button.Color);
                }
            }

            if(_selectedSquadron != null)
            {
                spriteBatch.DrawString(_galbasic, "" + _numSquads, new Vector2(widthIncrement * 29, 23 * heightIncrement), Color.AntiqueWhite);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void ButtonCatcher(object sender, ButtonClickedEventArgs e)
        {
            CustButton button = (CustButton)sender;

            switch(button.Id)
            {
                case 0: //save and exit
                    //Save fleet to .txt file
                    ScreenManager.AddScreen(new BackgroundScreen(), null);
                    ScreenManager.AddScreen(new FleetCustomizationMenuScreen(_game), null);
                    break;
                case 1: //clear fleet
                    buttonSweeper(5);
                    _fleet = new Fleet("");
                    _shipID = 0;
                    _numSquads = 0;
                    _fleetDeats = "";
                    _fleetName = "<Fleet Name>";
                    _cardDeats = "";
                    _selectedShip = null;
                    _selectedUpgrade = null;
                    _previousUpgrade = null;
                    _selectedSquadron = null;
                    _selectedUpgradeType = SelectedUpgradeType.None;

                    _buttons[2].IsActive = true;
                    _buttons[3].IsActive = true;
                    break;
                case 2: //select rebel fleet
                    buttonSweeper(5);
                    _fleet.IsRebelFleet = true;
                    _selectedSquadron = null;
                    _numSquads = 0;
                    _buttons[3].IsActive = false;
                    _buttons[5].IsActive = true;
                    _buttons[6].IsActive = true;
                    break;
                case 3: //select imp fleet
                    buttonSweeper(5);
                    _fleet.IsRebelFleet = false;
                    _selectedSquadron = null;
                    _numSquads = 0;
                    _buttons[2].IsActive = false;
                    _buttons[7].IsActive = true;
                    _buttons[8].IsActive = true;
                    break;
                case 4: //instructions
                    break;
                case 5: //rebel ships
                    buttonSweeper(7);
                    _selectedSquadron = null;
                    _numSquads = 0;
                    _buttons[9].IsActive = true;
                    _buttons[10].IsActive = true;
                    _buttons[11].IsActive = true;
                    break;
                case 6: //rebel squads
                    buttonSweeper(7);
                    _selectedSquadron = null;
                    _numSquads = 0;
                    _buttons[14].IsActive = true;
                    _buttons[15].IsActive = true;
                    _buttons[16].IsActive = true;
                    _buttons[17].IsActive = true;
                    break;
                case 7: //imp ships
                    buttonSweeper(9);
                    _selectedSquadron = null;
                    _numSquads = 0;
                    _buttons[12].IsActive = true;
                    _buttons[13].IsActive = true;
                    break;
                case 8: //imp squads
                    buttonSweeper(9);
                    _selectedSquadron = null;
                    _numSquads = 0;
                    _buttons[18].IsActive = true;
                    _buttons[19].IsActive = true;
                    _buttons[20].IsActive = true;
                    _buttons[21].IsActive = true;
                    break;
                case 9: //select Assault
                    buttonSweeper(14);
                    _buttons[22].IsActive = true;
                    _buttons[23].IsActive = true;

                    _selectedShip = new AssaultFrigateMarkII(_shipID, _content);
                    break;
                case 10: //select cr90
                    buttonSweeper(14);
                    _buttons[24].IsActive = true;
                    _buttons[25].IsActive = true;

                    _selectedShip = new CR90Corvette(_shipID, _content);
                    break;
                case 11: //select nebulon
                    buttonSweeper(14);
                    _buttons[26].IsActive = true;
                    _buttons[27].IsActive = true;

                    _selectedShip = new NebulonBFrigate(_shipID, _content);
                    break;
                case 12: //select gladiator
                    buttonSweeper(14);
                    _buttons[28].IsActive = true;
                    _buttons[29].IsActive = true;

                    _selectedShip = new GladiatorStarDestroyer(_shipID, _content);
                    break;
                case 13: //select victory
                    buttonSweeper(14);
                    _buttons[30].IsActive = true;
                    _buttons[31].IsActive = true;

                    _selectedShip = new VictoryStarDestroyer(_shipID, _content);
                    break;
                case 14: //a-wing
                    _selectedSquadron = new AWingSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 15: //b-wing
                    _selectedSquadron = new BWingSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 16: //x-wing
                    _selectedSquadron = new XWingSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 17: //y-wing
                    _selectedSquadron = new YWingSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 18: //tie fighter
                    _selectedSquadron = new TIEFighterSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 19: //tie advanced
                    _selectedSquadron = new TIEAdvancedSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 20: //tie interceptor
                    _selectedSquadron = new TIEInterceptorSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 21: //tie bomber
                    _selectedSquadron = new TIEBomberSquadron(_squadronID, _content);
                    _numSquads = 0;
                    buttonSweeper(22);
                    _buttons[51].IsActive = true;
                    _buttons[52].IsActive = true;
                    _buttons[53].IsActive = true;
                    break;
                case 22: //Assault mark 2 A
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 23: //Assault mark 2 B
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 24: //Corellian corvette A
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 25: //Corellian corvette B
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 26: //Nebulon-B Escort Frigate
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 27: //Nebulon-B Support Frigate
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 28: //Gladiator I class SD
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 29: //Gladiator II class SD
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 30: //Victory I class SD
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 31: //Victory II class SD
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    upgradeButtonSet();
                    break;
                case 32: //selecting title
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.Title;
                    individualUpgradeSet();
                    break;
                case 33: //selecting officers
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.Officers;
                    individualUpgradeSet();
                    break;
                case 34: //selecting weapons teams
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.WeaponsTeam;
                    individualUpgradeSet();
                    break;
                case 35: //selecting support team
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.SupportTeam;
                    individualUpgradeSet();
                    break;
                case 36: //selecting offensive retrofit
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.OffensiveRetrofit;
                    individualUpgradeSet();
                    break;
                case 37: //selecting defensive retrofit
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.DefensiveRetrofit;
                    individualUpgradeSet();
                    break;
                case 38: //selecting ion cannons
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.IonCannon;
                    individualUpgradeSet();
                    break;
                case 39: //selecting turbolasers
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.Turbolasers;
                    individualUpgradeSet();
                    break;
                case 40: //selecting ordinance
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.Ordinance;
                    individualUpgradeSet();
                    break;
                case 41: //selecting commaders
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    _selectedUpgradeType = SelectedUpgradeType.Commander;
                    individualUpgradeSet();
                    break;
                case 42:
                    selectingUpgrade(42);
                    upgradeShip();
                    break;
                case 43:
                    selectingUpgrade(43);
                    upgradeShip();
                    break;
                case 44:
                    selectingUpgrade(44);
                    upgradeShip();
                    break;
                case 45:
                    selectingUpgrade(45);
                    upgradeShip();
                    break;
                case 46:
                    selectingUpgrade(46);
                    upgradeShip();
                    break;
                case 47:
                    selectingUpgrade(47);
                    upgradeShip();
                    break;
                case 48:
                    selectingUpgrade(48);
                    upgradeShip();
                    break;
                case 49: //add ship to fleet
                    _fleet.Ships.Add(_selectedShip);
                    _shipID++;
                    buttonSweeper(9);
                    break;
                case 50: //clear ship upgrades
                    _selectedShip.Commander = null;
                    _selectedShip.Title = null;
                    _selectedShip.Upgrades = new UpgradeCard[8];
                    buttonSweeper(42);
                    _buttons[49].IsActive = true;
                    _buttons[50].IsActive = true;
                    break;
                case 51:
                    int x = _numSquads;
                    if(_selectedSquadron.PointCost * (x + 1) <= 400/3)
                    {
                        _numSquads++;
                    }
                    break;
                case 52:
                    _numSquads--;
                    if(_numSquads < 0)
                    {
                        _numSquads = 0;
                    }
                    break;
                case 53:
                    addSquads();
                    _selectedSquadron = null;
                    buttonSweeper(9);
                    break;
                case 54: //edit fleet name
                    _buttons[54].IsActive = false;
                    _buttons[55].IsActive = true;
                    _fleetName = "";
                    break;
                case 55: //set fleet name
                    _fleet.Name = _fleetName;
                    if (_fleetName.Length == 0) _fleetName = "<Fleet Name>";
                    _buttons[54].IsActive = true;
                    _buttons[55].IsActive = false;
                    break;
                case 56:
                    break;
                case 57:
                    break;
                case 58:
                    break;
                case 59:
                    break;
                case 60:
                    break;
            }
        }

        private void addSquads()
        {
            for(int i = 0; i < _numSquads; i++)
            {
                if (_selectedSquadron is AWingSquadron) _fleet.Squadrons.Add(new AWingSquadron(_squadronID, _content));
                else if (_selectedSquadron is BWingSquadron) _fleet.Squadrons.Add(new BWingSquadron(_squadronID, _content));
                else if (_selectedSquadron is XWingSquadron) _fleet.Squadrons.Add(new XWingSquadron(_squadronID, _content));
                else if (_selectedSquadron is YWingSquadron) _fleet.Squadrons.Add(new YWingSquadron(_squadronID, _content));
                else if (_selectedSquadron is TIEFighterSquadron) _fleet.Squadrons.Add(new TIEFighterSquadron(_squadronID, _content));
                else if (_selectedSquadron is TIEAdvancedSquadron) _fleet.Squadrons.Add(new TIEAdvancedSquadron(_squadronID, _content));
                else if (_selectedSquadron is TIEInterceptorSquadron) _fleet.Squadrons.Add(new TIEInterceptorSquadron(_squadronID, _content));
                else if (_selectedSquadron is TIEBomberSquadron) _fleet.Squadrons.Add(new TIEBomberSquadron(_squadronID, _content));
                _squadronID++;
            }
            _numSquads = 0;
        }

        private void selectingUpgrade(int button)
        {
            string ship = getSelectedShip();

            switch(_selectedUpgradeType)
            {
                case SelectedUpgradeType.Title:
                    switch(ship)
                    {
                        case "Assault":
                            if (button == 42) _selectedUpgrade = new TitleGallantHaven(_content);
                            if (button == 43) _selectedUpgrade = new TitleParagon(_content);
                            break;
                        case "CR90":
                            if (button == 42) _selectedUpgrade = new TitleDodonnasPride(_content);
                            if (button == 43) _selectedUpgrade = new TitleJainasLight(_content);
                            if (button == 44) _selectedUpgrade = new TitleTantiveIV(_content);
                            break;
                        case "Nebulon":
                            if (button == 42) _selectedUpgrade = new TitleRedemption(_content);
                            if (button == 43) _selectedUpgrade = new TitleSalvation(_content);
                            if (button == 44) _selectedUpgrade = new TitleYavaris(_content);
                            break;
                        case "Gladiator":
                            if (button == 42) _selectedUpgrade = new TitleDominator(_content);
                            if (button == 43) _selectedUpgrade = new TitleInsidious(_content);
                            break;
                        case "Victory":
                            if (button == 42) _selectedUpgrade = new TitleCorruptor(_content);
                            if (button == 43) _selectedUpgrade = new TitleDominator(_content);
                            if (button == 44) _selectedUpgrade = new TitleWarlord(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.WeaponsTeam:
                    switch (ship)
                    {
                        case "Assault":
                            if (button == 42) _selectedUpgrade = new FlightControllers(_content);
                            if (button == 43) _selectedUpgrade = new GunneryTeam(_content);
                            if (button == 44) _selectedUpgrade = new SensorTeam(_content);
                            break;
                        case "Gladiator":
                            if (button == 42) _selectedUpgrade = new FlightControllers(_content);
                            if (button == 43) _selectedUpgrade = new GunneryTeam(_content);
                            if (button == 44) _selectedUpgrade = new SensorTeam(_content);
                            break;
                        case "Victory":
                            if (button == 42) _selectedUpgrade = new FlightControllers(_content);
                            if (button == 43) _selectedUpgrade = new GunneryTeam(_content);
                            if (button == 44) _selectedUpgrade = new SensorTeam(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.SupportTeam:
                    switch (ship)
                    {
                        case "CR90":
                            if (button == 42) _selectedUpgrade = new EngineTechs(_content);
                            if (button == 43) _selectedUpgrade = new EngineeringTeam(_content);
                            if (button == 44) _selectedUpgrade = new NavTeam(_content);
                            break;
                        case "Nebulon":
                            if (button == 42) _selectedUpgrade = new EngineTechs(_content);
                            if (button == 43) _selectedUpgrade = new EngineeringTeam(_content);
                            if (button == 44) _selectedUpgrade = new NavTeam(_content);
                            break;
                        case "Gladiator":
                            if (button == 42) _selectedUpgrade = new EngineTechs(_content);
                            if (button == 43) _selectedUpgrade = new EngineeringTeam(_content);
                            if (button == 44) _selectedUpgrade = new NavTeam(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.OffensiveRetrofit:
                    switch (ship)
                    {
                        case "Assault":
                            if (button == 42) _selectedUpgrade = new ExpandedHangarBay(_content);
                            if (button == 43) _selectedUpgrade = new PointDefenseReroute(_content);
                            break;
                        case "Victory":
                            if (button == 42) _selectedUpgrade = new ExpandedHangarBay(_content);
                            if (button == 43) _selectedUpgrade = new PointDefenseReroute(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.DefensiveRetrofit:
                    switch (ship)
                    {
                        case "Assault":
                            if (button == 42) _selectedUpgrade = new AdvancedProjectors(_content);
                            if (button == 43) _selectedUpgrade = new ElectronicCountermeasures(_content);
                            break;
                        case "CR90":
                            if (button == 42) _selectedUpgrade = new AdvancedProjectors(_content);
                            if (button == 43) _selectedUpgrade = new ElectronicCountermeasures(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.Officers:
                    switch (ship)
                    {
                        case "Assault":
                            if (button == 42) _selectedUpgrade = new OfficerAdarTallon(_content);
                            if (button == 43) _selectedUpgrade = new OfficerLeiaOrgana(_content);
                            if (button == 44) _selectedUpgrade = new OfficerRaymusAntilles(_content);
                            if (button == 45) _selectedUpgrade = new OfficerDefenseLiaison(_content);
                            if (button == 46) _selectedUpgrade = new OfficerIntelOfficer(_content);
                            if (button == 47) _selectedUpgrade = new OfficerVeteranCaptain(_content);
                            if (button == 48) _selectedUpgrade = new OfficerWeaponsLiaison(_content);
                            break;
                        case "CR90":
                            if (button == 42) _selectedUpgrade = new OfficerAdarTallon(_content);
                            if (button == 43) _selectedUpgrade = new OfficerLeiaOrgana(_content);
                            if (button == 44) _selectedUpgrade = new OfficerRaymusAntilles(_content);
                            if (button == 45) _selectedUpgrade = new OfficerDefenseLiaison(_content);
                            if (button == 46) _selectedUpgrade = new OfficerIntelOfficer(_content);
                            if (button == 47) _selectedUpgrade = new OfficerVeteranCaptain(_content);
                            if (button == 48) _selectedUpgrade = new OfficerWeaponsLiaison(_content);
                            break;
                        case "Nebulon":
                            if (button == 42) _selectedUpgrade = new OfficerAdarTallon(_content);
                            if (button == 43) _selectedUpgrade = new OfficerLeiaOrgana(_content);
                            if (button == 44) _selectedUpgrade = new OfficerRaymusAntilles(_content);
                            if (button == 45) _selectedUpgrade = new OfficerDefenseLiaison(_content);
                            if (button == 46) _selectedUpgrade = new OfficerIntelOfficer(_content);
                            if (button == 47) _selectedUpgrade = new OfficerVeteranCaptain(_content);
                            if (button == 48) _selectedUpgrade = new OfficerWeaponsLiaison(_content);
                            break;
                        case "Gladiator":
                            if (button == 42) _selectedUpgrade = new OfficerAdmiralChiraneau(_content);
                            if (button == 43) _selectedUpgrade = new OfficerDirectorIsard(_content);
                            if (button == 44) _selectedUpgrade = new OfficerWullfYularen(_content);
                            if (button == 45) _selectedUpgrade = new OfficerDefenseLiaison(_content);
                            if (button == 46) _selectedUpgrade = new OfficerIntelOfficer(_content);
                            if (button == 47) _selectedUpgrade = new OfficerVeteranCaptain(_content);
                            if (button == 48) _selectedUpgrade = new OfficerWeaponsLiaison(_content);
                            break;
                        case "Victory":
                            if (button == 42) _selectedUpgrade = new OfficerAdarTallon(_content);
                            if (button == 43) _selectedUpgrade = new OfficerLeiaOrgana(_content);
                            if (button == 44) _selectedUpgrade = new OfficerRaymusAntilles(_content);
                            if (button == 45) _selectedUpgrade = new OfficerDefenseLiaison(_content);
                            if (button == 46) _selectedUpgrade = new OfficerIntelOfficer(_content);
                            if (button == 47) _selectedUpgrade = new OfficerVeteranCaptain(_content);
                            if (button == 48) _selectedUpgrade = new OfficerWeaponsLiaison(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.Turbolasers:
                    switch (ship)
                    {
                        case "Assault":
                            if (button == 42) _selectedUpgrade = new EnhancedArmament(_content);
                            if (button == 43) _selectedUpgrade = new H9Turbolasers(_content);
                            if (button == 44) _selectedUpgrade = new XI7Turbolasers(_content);
                            if (button == 45) _selectedUpgrade = new XX9Turbolasers(_content);
                            break;
                        case "CR90":
                            if(_selectedShip.ShipTypeA)
                            {
                                if (button == 42) _selectedUpgrade = new EnhancedArmament(_content);
                                if (button == 43) _selectedUpgrade = new H9Turbolasers(_content);
                                if (button == 44) _selectedUpgrade = new XI7Turbolasers(_content);
                                if (button == 45) _selectedUpgrade = new XX9Turbolasers(_content);
                            }
                            break;
                        case "Nebulon":
                            if (button == 42) _selectedUpgrade = new EnhancedArmament(_content);
                            if (button == 43) _selectedUpgrade = new H9Turbolasers(_content);
                            if (button == 44) _selectedUpgrade = new XI7Turbolasers(_content);
                            if (button == 45) _selectedUpgrade = new XX9Turbolasers(_content);
                            break;
                        case "Victory":
                            if (!_selectedShip.ShipTypeA)
                            {
                                if (button == 42) _selectedUpgrade = new EnhancedArmament(_content);
                                if (button == 43) _selectedUpgrade = new H9Turbolasers(_content);
                                if (button == 44) _selectedUpgrade = new XI7Turbolasers(_content);
                                if (button == 45) _selectedUpgrade = new XX9Turbolasers(_content);
                            }
                            break;
                    }
                    break;
                case SelectedUpgradeType.IonCannon:
                    switch (ship)
                    {
                        case "CR90":
                            if (!_selectedShip.ShipTypeA)
                            {
                                if (button == 42) _selectedUpgrade = new IonCannonBatteries(_content);
                                if (button == 43) _selectedUpgrade = new LeadingShots(_content);
                                if (button == 44) _selectedUpgrade = new OverloadPulse(_content);
                            }
                            break;
                        case "Victory":
                            if (button == 42) _selectedUpgrade = new IonCannonBatteries(_content);
                            if (button == 43) _selectedUpgrade = new LeadingShots(_content);
                            if (button == 44) _selectedUpgrade = new OverloadPulse(_content);
                            break;
                    }
                    break;
                case SelectedUpgradeType.Ordinance:
                    switch (ship)
                    {
                        case "Gladiator":
                            if (button == 42) _selectedUpgrade = new AssaultConcussionMissiles(_content);
                            if (button == 43) _selectedUpgrade = new ExpandedLaunchers(_content);
                            break;
                        case "Victory":
                            if (_selectedShip.ShipTypeA)
                            {
                                if (button == 42) _selectedUpgrade = new AssaultConcussionMissiles(_content);
                                if (button == 43) _selectedUpgrade = new ExpandedLaunchers(_content);
                            }
                            break;
                    }
                    break;
                case SelectedUpgradeType.Commander:
                    if(_fleet.IsRebelFleet)
                    {
                        if (button == 42) _selectedUpgrade = new CommanderGarmBelIblis(_content);
                        if (button == 43) _selectedUpgrade = new CommanderGeneralDodonna(_content);
                        if (button == 44) _selectedUpgrade = new CommanderMonMothma(_content);
                    }
                    else
                    {
                        if (button == 42) _selectedUpgrade = new CommanderAdmiralMotti(_content);
                        if (button == 43) _selectedUpgrade = new CommanderAdmiralScreed(_content);
                        if (button == 44) _selectedUpgrade = new CommanderGrandMoffTarkin(_content);
                    }
                    break;
            }
        }

        /// <summary>
        /// Applys the current upgrade to the current ship.
        /// </summary>
        private void upgradeShip()
        {
            if (_selectedUpgradeType == SelectedUpgradeType.Title) _selectedShip.Title = _selectedUpgrade;
            else if(_selectedUpgradeType == SelectedUpgradeType.Commander) _selectedShip.Commander = _selectedUpgrade;

            if (_selectedShip.UpgradeTypes[0] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.Officers) _selectedShip.Upgrades[0] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[1] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.SupportTeam) _selectedShip.Upgrades[1] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[2] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.WeaponsTeam) _selectedShip.Upgrades[2] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[3] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.Ordinance) _selectedShip.Upgrades[3] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[4] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.OffensiveRetrofit) _selectedShip.Upgrades[4] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[5] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.Turbolasers) _selectedShip.Upgrades[5] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[6] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.IonCannon) _selectedShip.Upgrades[6] = _selectedUpgrade;
            if (_selectedShip.UpgradeTypes[7] == 1 && _selectedUpgrade.CardType == UpgradeTypeEnum.DefensiveRetrofit) _selectedShip.Upgrades[7] = _selectedUpgrade;
        }

        /// <summary>
        /// A sweeper method that moves through and switches uneeded buttons to inactive.
        /// </summary>
        /// <param name="index">The index to start the sweep at.</param>
        private void buttonSweeper(int index)
        {
            for(int i = index; i < _buttons.Count; i++)
            {
                if(_buttons[i].Id != 54 && _buttons[i].Id != 55)
                {
                    _buttons[i].IsActive = false;
                }
            }
        }

        /// <summary>
        /// A helper method that automatically sets the buttons for the possible upgrades.
        /// </summary>
        private void upgradeButtonSet()
        {
            _buttons[32].IsActive = true;
            _buttons[33].IsActive = true;
            _buttons[41].IsActive = true;

            foreach (var ship in _fleet.Ships)
            {
                if(ship.HasCommander)
                {
                    _buttons[41].IsActive = false;
                    break;
                }
            }

            if (_selectedShip.UpgradeTypes[1] == 1) _buttons[35].IsActive = true;
            if (_selectedShip.UpgradeTypes[2] == 1) _buttons[34].IsActive = true;
            if (_selectedShip.UpgradeTypes[3] == 1) _buttons[40].IsActive = true;
            if (_selectedShip.UpgradeTypes[4] == 1) _buttons[36].IsActive = true;
            if (_selectedShip.UpgradeTypes[5] == 1) _buttons[39].IsActive = true;
            if (_selectedShip.UpgradeTypes[6] == 1) _buttons[38].IsActive = true;
            if (_selectedShip.UpgradeTypes[7] == 1) _buttons[37].IsActive = true;
        }

        private void individualUpgradeSet()
        {
            string shipType = getSelectedShip();
            if(!shipType.Equals("null"))
            {
                switch (_selectedUpgradeType)
                {
                    case SelectedUpgradeType.Title:
                        switch (shipType)
                        {
                            case "Assault":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                            case "CR90":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                            case "Nebulon":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                            case "Gladiator":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                            case "Victory":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                        }
                        break;
                    case SelectedUpgradeType.Officers:
                        _buttons[42].IsActive = true;
                        _buttons[43].IsActive = true;
                        _buttons[44].IsActive = true;
                        _buttons[45].IsActive = true;
                        _buttons[46].IsActive = true;
                        _buttons[47].IsActive = true;
                        _buttons[48].IsActive = true;
                        break;
                    case SelectedUpgradeType.OffensiveRetrofit:
                        switch (shipType)
                        {
                            case "Assault":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                            case "Victory":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                        }
                        break;
                    case SelectedUpgradeType.DefensiveRetrofit:
                        switch (shipType)
                        {
                            case "Assault":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                            case "CR90":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                        }
                        break;
                    case SelectedUpgradeType.SupportTeam:
                        switch (shipType)
                        {
                            case "CR90":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                            case "Nebulon":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                            case "Gladiator":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                        }
                        break;
                    case SelectedUpgradeType.WeaponsTeam:
                        switch (shipType)
                        {
                            case "Assault":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                            case "Gladiator":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                            case "Victory":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                        }
                        break;
                    case SelectedUpgradeType.Turbolasers:
                        switch (shipType)
                        {
                            case "Assault":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                _buttons[45].IsActive = true;
                                break;
                            case "CR90":
                                if(_selectedShip.ShipTypeA)
                                {
                                    _buttons[42].IsActive = true;
                                    _buttons[43].IsActive = true;
                                    _buttons[44].IsActive = true;
                                    _buttons[45].IsActive = true;
                                }
                                break;
                            case "Nebulon":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                _buttons[45].IsActive = true;
                                break;
                            case "Victory":
                                if (!_selectedShip.ShipTypeA)
                                {
                                    _buttons[42].IsActive = true;
                                    _buttons[43].IsActive = true;
                                    _buttons[44].IsActive = true;
                                    _buttons[45].IsActive = true;
                                }
                                break;
                        }
                        break;
                    case SelectedUpgradeType.IonCannon:
                        switch (shipType)
                        {
                            case "CR90":
                                if (!_selectedShip.ShipTypeA)
                                {
                                    _buttons[42].IsActive = true;
                                    _buttons[43].IsActive = true;
                                    _buttons[44].IsActive = true;
                                }
                                break;
                            case "Victory":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                _buttons[44].IsActive = true;
                                break;
                        }
                        break;
                    case SelectedUpgradeType.Ordinance:
                        switch (shipType)
                        {
                            case "Gladiator":
                                _buttons[42].IsActive = true;
                                _buttons[43].IsActive = true;
                                break;
                            case "Victory":
                                if (_selectedShip.ShipTypeA)
                                {
                                    _buttons[42].IsActive = true;
                                    _buttons[43].IsActive = true;
                                }
                                break;
                        }
                        break;
                    case SelectedUpgradeType.Commander:
                        _buttons[42].IsActive = true;
                        _buttons[43].IsActive = true;
                        _buttons[44].IsActive = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Return what the selected ship's type is
        /// </summary>
        /// <returns>The type as a string</returns>
        private string getSelectedShip()
        {
            if (_selectedShip is AssaultFrigateMarkII) return "Assault";
            else if (_selectedShip is CR90Corvette) return "CR90";
            else if (_selectedShip is NebulonBFrigate) return "Nebulon";
            else if (_selectedShip is GladiatorStarDestroyer) return "Gladiator";
            else if (_selectedShip is VictoryStarDestroyer) return "Victory";
            else return "null";
        }
    }
}
