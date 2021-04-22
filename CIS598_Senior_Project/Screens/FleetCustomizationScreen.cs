using System;
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
        private ContentManager _content;
        private SpriteFont _gameFont;

        private Texture2D _texture;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private MouseState _previousMouseState;
        private MouseState _currentMouseState;

        private List<CustButton> _buttons;

        private Fleet _fleet;
        private Ship _selectedShip;
        private UpgradeCard _selectedUpgrade;

        private GraphicsDevice _graphics;

        private Game _game;

        public FleetCustomizationScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _game = game;

            _buttons = new List<CustButton>();

            //_graphics = ScreenManager.GraphicsDevice;

            _fleet = new Fleet("");

            int widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            int heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

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
            _buttons.Add(new CustButton(39, new Rectangle(widthIncrement * 25, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Turbo Laser
            _buttons.Add(new CustButton(40, new Rectangle(widthIncrement * 37, heightIncrement * 86, 10 * widthIncrement, 15 * heightIncrement), false));                                                   //Select Ordinance

            /*
            _buttons.Add(new CustButton(0, new Rectangle(), true));     //Select rebel commander
            _buttons.Add(new CustButton(0, new Rectangle(), true));     //Select imperial commander
            */

        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("bangersMenuFont");
            _texture = _content.Load<Texture2D>("button_test");

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

            /*
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(_game), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                //_playerPosition += movement * 8f;
            }
            */

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

            //spriteBatch.Draw(_texture, source, source, Color.White);

            foreach (var button in _buttons)
            {
                if (button.IsActive)
                {
                    //spriteBatch.Draw(button.texture, button.Area, button.Color);
                    spriteBatch.Draw(_texture, button.Area, button.Color);
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

        private void ButtonCatcher(object sender, ButtonClickedEventArgs e)
        {
            CustButton button = (CustButton)sender;

            switch(button.Id)
            {
                case 0:
                    //Save fleet to .txt file
                    ScreenManager.AddScreen(new BackgroundScreen(), null);
                    ScreenManager.AddScreen(new FleetCustomizationMenuScreen(_game), null);
                    break;
                case 1:
                    buttonSweeper(5);
                    _fleet = new Fleet("");
                    break;
                case 2:
                    buttonSweeper(7);
                    _buttons[5].IsActive = true;
                    _buttons[6].IsActive = true;
                    break;
                case 3:
                    buttonSweeper(5);
                    _buttons[7].IsActive = true;
                    _buttons[8].IsActive = true;
                    break;
                case 4:
                    break;
                case 5:
                    buttonSweeper(7);
                    _buttons[9].IsActive = true;
                    _buttons[10].IsActive = true;
                    _buttons[11].IsActive = true;
                    break;
                case 6:
                    buttonSweeper(7);
                    _buttons[14].IsActive = true;
                    _buttons[15].IsActive = true;
                    _buttons[16].IsActive = true;
                    _buttons[17].IsActive = true;
                    break;
                case 7:
                    buttonSweeper(9);
                    _buttons[12].IsActive = true;
                    _buttons[13].IsActive = true;
                    break;
                case 8:
                    buttonSweeper(9);
                    _buttons[18].IsActive = true;
                    _buttons[19].IsActive = true;
                    _buttons[20].IsActive = true;
                    _buttons[21].IsActive = true;
                    break;
                case 9:
                    buttonSweeper(14);
                    _buttons[22].IsActive = true;
                    _buttons[23].IsActive = true;

                    _selectedShip = new AssaultFrigateMarkII(0, _content);
                    break;
                case 10:
                    buttonSweeper(14);
                    _buttons[24].IsActive = true;
                    _buttons[25].IsActive = true;

                    _selectedShip = new CR90Corvette(0, _content);
                    break;
                case 11:
                    buttonSweeper(14);
                    _buttons[26].IsActive = true;
                    _buttons[27].IsActive = true;
                    break;
                case 12:
                    buttonSweeper(14);
                    _buttons[28].IsActive = true;
                    _buttons[29].IsActive = true;
                    break;
                case 13:
                    buttonSweeper(14);
                    _buttons[30].IsActive = true;
                    _buttons[31].IsActive = true;
                    break;
                case 14:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    break;
                case 19:
                    break;
                case 20:
                    break;
                case 21:
                    break;
                case 22: //Assault mark 2 A
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    upgradeButtonSet();
                    break;
                case 23: //Assault mark 2 B
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    upgradeButtonSet();
                    break;
                case 24: //Corellian corvette A
                    _selectedShip.ShipTypeA = true;
                    buttonSweeper(32);
                    upgradeButtonSet();
                    break;
                case 25: //Corellian corvette B
                    _selectedShip.ShipTypeA = false;
                    buttonSweeper(32);
                    upgradeButtonSet();
                    break;
                case 26:
                    break;
                case 27:
                    break;
                case 28:
                    break;
                case 29:
                    break;
                case 30:
                    break;
                case 31:
                    break;
                case 32:
                    break;
                case 33:
                    break;
                case 34:
                    break;
                case 35:
                    break;
                case 36:
                    break;
                case 37:
                    break;
                case 38:
                    break;
                case 39:
                    break;
                case 40:
                    break;
                case 41:
                    break;
                case 42:
                    break;
                case 43:
                    break;
                case 44:
                    break;
                case 45:
                    break;
                case 46:
                    break;
                case 47:
                    break;
                case 48:
                    break;
                case 49:
                    break;
            }
        }

        /// <summary>
        /// A sweeper method that moves through and switches uneeded buttons to inactive.
        /// </summary>
        /// <param name="index">The index to start the sweep at.</param>
        private void buttonSweeper(int index)
        {
            for(int i = index; i < _buttons.Count; i++)
            {
                _buttons[i].IsActive = false;
            }
        }

        /// <summary>
        /// A helper method that automatically sets the buttons for the possible upgrades.
        /// </summary>
        private void upgradeButtonSet()
        {
            _buttons[32].IsActive = true;
            _buttons[33].IsActive = true;
            if (_selectedShip.UpgradeTypes[1] == 1) _buttons[35].IsActive = true;
            if (_selectedShip.UpgradeTypes[2] == 1) _buttons[34].IsActive = true;
            if (_selectedShip.UpgradeTypes[3] == 1) _buttons[40].IsActive = true;
            if (_selectedShip.UpgradeTypes[4] == 1) _buttons[36].IsActive = true;
            if (_selectedShip.UpgradeTypes[5] == 1) _buttons[39].IsActive = true;
            if (_selectedShip.UpgradeTypes[6] == 1) _buttons[38].IsActive = true;
            if (_selectedShip.UpgradeTypes[7] == 1) _buttons[37].IsActive = true;
        }
    }
}
