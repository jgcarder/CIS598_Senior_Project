/* DuelFleetSelectionScreen.cs
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
using CIS598_Senior_Project.MenuObjects;
using CIS598_Senior_Project.StateManagement;
using CIS598_Senior_Project.FleetObjects;

namespace CIS598_Senior_Project.Screens
{
    public class DuelFleetSelectionScreen : GameScreen
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
        /// The constructor for the beginings of a duel.
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="vol">Volume statictics</param>
        /// <param name="player1">Player1's fleet</param>
        /// <param name="player2">Player2's fleet</param>
        public DuelFleetSelectionScreen(Game game, List<float> vol, Fleet player1, Fleet player2)
        {
            _game = game;
            _buttons = new List<CustButton>();

            _vol = vol;

            _widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            _player1 = player1;
            _player2 = player2;

            //Buttons go here
            _buttons.Add(new CustButton(0, new Rectangle(_widthIncrement, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 16, _widthIncrement * 10, _heightIncrement * 15), true));   //back button
            _buttons.Add(new CustButton(1, new Rectangle(_widthIncrement, _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                               //Select fleets
            if(player1 != null || player2 != null)
            {
                _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement, 18 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                         //clear fleets
            }
            else
            {
                _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement, 18 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), false));                                         //clear fleets
            }

            if (player1 != null && player2 != null)
            {

                _buttons.Add(new CustButton(3, new Rectangle(_widthIncrement, 35 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));                                         //Start
            }
            else
            {
                _buttons.Add(new CustButton(3, new Rectangle(_widthIncrement, 35 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), false));                                         //Start

            }

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
            _buttons[0].Texture = _content.Load<Texture2D>("Back");
            _buttons[1].Texture = _content.Load<Texture2D>("SelectFleets");
            _buttons[2].Texture = _content.Load<Texture2D>("ClearSelected");
            _buttons[3].Texture = _content.Load<Texture2D>("Start");

            _label = _content.Load<Texture2D>("FleetSelectionLabel");
            _background = _content.Load<Texture2D>("DuelFleetSelectionBackground");

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
            spriteBatch.Draw(_background, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(_label, new Vector2(_widthIncrement * 25, -7 * _heightIncrement), Color.White);

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

            if (_player1 != null)
            {
                double heightoffset = 20;
                spriteBatch.DrawString(_descriptor, "FLEET: " + _player1.Name + "--" + _player1.TotalPoints + " total points", new Vector2(_widthIncrement * 15, (int)(heightoffset * _heightIncrement)), Color.AntiqueWhite);

                heightoffset += 3;
                foreach (var ship in _player1.Ships)
                {
                    if (ship != null)
                    {
                        if (ship.ShipTypeA)
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(A): " + ship.PointCost + "---------------", new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        else
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(B): " + ship.PointCost + "---------------", new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        heightoffset += 3;

                        if (ship.Commander != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Commander " + ship.Commander.Name + ": " + ship.Commander.PointCost, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }

                        if (ship.Title != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Title " + ship.Title.Name + ": " + ship.Title.PointCost, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }

                        foreach (var upgrade in ship.Upgrades)
                        {
                            if (upgrade != null)
                            {
                                spriteBatch.DrawString(_descriptor, "   -" + upgrade.CardType.ToString() + " " + upgrade.Name + ": " + upgrade.PointCost, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                                heightoffset += 3;
                            }
                        }
                    }
                }

                if (_player1.Squadrons.Count > 0)
                {
                    int[] sq = returnSquads(_player1);
                    if (_player1.IsRebelFleet)
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -A-Wing Squadron(11): x" + sq[0] + " => " + sq[0] * 11, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -B-Wing Squadron(14): x" + sq[1] + " => " + sq[1] * 14, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -X-Wing Squadron(13): x" + sq[2] + " => " + sq[2] * 13, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -Y-Wing Squadron(10): x" + sq[3] + " => " + sq[3] * 10, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                    }
                    else
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Fighter Squadron(8): x" + sq[0] + " => " + sq[0] * 8, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Advanced Squadron(12): x" + sq[1] + " => " + sq[1] * 12, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Interceptor Squadron(11): x" + sq[2] + " => " + sq[2] * 11, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Bomber Squadron(9): x" + sq[3] + " => " + sq[3] * 9, new Vector2(_widthIncrement * 15, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                    }
                }
            }

            if (_player2 != null)
            {
                double heightoffset = 20;
                spriteBatch.DrawString(_descriptor, "FLEET: " + _player2.Name + "--" + _player2.TotalPoints + " total points", new Vector2(_widthIncrement * 60, (int)(heightoffset * _heightIncrement)), Color.AntiqueWhite);

                heightoffset += 3;
                foreach (var ship in _player2.Ships)
                {
                    if (ship != null)
                    {
                        if (ship.ShipTypeA)
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(A): " + ship.PointCost + "---------------", new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        else
                        {
                            spriteBatch.DrawString(_descriptor, " >" + ship.Name + "(B): " + ship.PointCost + "---------------", new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        }
                        heightoffset += 3;

                        if (ship.Commander != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Commander " + ship.Commander.Name + ": " + ship.Commander.PointCost, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }

                        if (ship.Title != null)
                        {
                            spriteBatch.DrawString(_descriptor, "   -" + "Title " + ship.Title.Name + ": " + ship.Title.PointCost, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }

                        foreach (var upgrade in ship.Upgrades)
                        {
                            if (upgrade != null)
                            {
                                spriteBatch.DrawString(_descriptor, "   -" + upgrade.CardType.ToString() + " " + upgrade.Name + ": " + upgrade.PointCost, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                                heightoffset += 3;
                            }
                        }
                    }
                }

                if (_player2.Squadrons.Count > 0)
                {
                    int[] sq = returnSquads(_player2);
                    if (_player2.IsRebelFleet)
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -A-Wing Squadron(11): x" + sq[0] + " => " + sq[0] * 11, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -B-Wing Squadron(14): x" + sq[1] + " => " + sq[1] * 14, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -X-Wing Squadron(13): x" + sq[2] + " => " + sq[2] * 13, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -Y-Wing Squadron(10): x" + sq[3] + " => " + sq[3] * 10, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                    }
                    else
                    {
                        if (sq[0] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Fighter Squadron(8): x" + sq[0] + " => " + sq[0] * 8, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[1] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Advanced Squadron(12): x" + sq[1] + " => " + sq[1] * 12, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[2] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Interceptor Squadron(11): x" + sq[2] + " => " + sq[2] * 11, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
                        if (sq[3] > 0)
                        {
                            spriteBatch.DrawString(_descriptor, "   -TIE Bomber Squadron(9): x" + sq[3] + " => " + sq[3] * 9, new Vector2(_widthIncrement * 60, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 3;
                        }
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

                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new HotseatDuelScreen(_game, _vol, _player1, _player2), null);
                    }
                    break;
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
