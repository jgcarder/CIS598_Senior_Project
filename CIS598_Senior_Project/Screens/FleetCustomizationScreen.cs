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

        private List<Button> _buttons;

        private Fleet _fleet;

        public FleetCustomizationScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _fleet = new Fleet("");

            _buttons.Add(); //Save and quit
            _buttons.Add(); //Clear fleet

            _buttons.Add(); //Add button whose role changes 
            _buttons.Add(); //Remove button whose role changes

            _buttons.Add(); //Select upgrades(Officer, Weapons team, offensive retrofit, ordinance, turbolasers, ion cannon, defensive retrofit, support team)

            _buttons.Add(); //Select rebel fleet
            _buttons.Add();     //Select rebel commander
            _buttons.Add();     //Select rebel ships
            _buttons.Add();         //Select Assault Frigate Mark II
            _buttons.Add();             //Select Mark II A
            _buttons.Add();             //Select Mark II B
            _buttons.Add();         //Select CR90 Corellian Corvette
            _buttons.Add();             //Select version A
            _buttons.Add();             //Select version B
            _buttons.Add();         //Select Nebulon B Frigate
            _buttons.Add();             //Select escort refit
            _buttons.Add();             //Select support refit
            _buttons.Add();     //Select rebel squadrons
            _buttons.Add();         //Select A-wing squadron
            _buttons.Add();         //Select B-wing squadron
            _buttons.Add();         //Select X-wing squadron
            _buttons.Add();         //Select Y-wing squadron
            _buttons.Add(); //Select imperial fleet
            _buttons.Add();     //Select imperial commander
            _buttons.Add();     //Select imperial ships
            _buttons.Add();         //Select Gladiator SD
            _buttons.Add();             //Select class I
            _buttons.Add();             //Select class II
            _buttons.Add();         //Select Victory SD
            _buttons.Add();             //Select class I
            _buttons.Add();             //Select class II
            _buttons.Add();     //Select imperial squadrons
            _buttons.Add();         //Select tie fighter squadron
            _buttons.Add();         //Select tie advanced squadron
            _buttons.Add();         //Select tie interceptor squadron
            _buttons.Add();         //Select tie bomber squadron
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("bangersMenuFont");
            _texture = _content.Load<Texture2D>("colored_packed");

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
                        if (_currentMouseState.X >= button.Position.X && _currentMouseState.X <= button.Position.X + button.TouchArea.Width
                                            && _currentMouseState.Y >= button.Position.Y && _currentMouseState.Y <= button.Position.Y + button.TouchArea.Height)
                        {
                            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                            {
                                button.AnAction(button, new ButtonClickedEventArgs() { Id = button.Id });
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

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
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
                    spriteBatch.Draw(button.texture, button.TouchArea, button.Color);
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
    }
}
