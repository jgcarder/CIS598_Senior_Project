/* File: HotseatDuelScreen.cs
 * Author: Jackson Carder
 */

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
        enum SetupState
        {
            SelectFirst,
            Placement,
            SetupDone
        }

        enum ShipState
        {
            ActivateShip,
            RevealDial,
            Attack,
            ExecuteManuver
        }
            enum AttackState
            {
                DeclareTarget,
                RollDice,
                ModifyDice,
                SpendAccuracies,
                SpendDefenseTokens,
                ResolveDamage
            }
        
        enum SquadronState
        {
            ActivateSquad,
            Move,
            Attack
        }

        enum SquadronCommand
        {
            UseToken,
            UseDial,
            ActivateSquadron,
            Move,
            Attack
        }

        private Game _game;

        private Fleet _player1;
        private Fleet _player2;

        private List<Ship> _shipToPlace1;
        private List<Ship> _shipsPlaced1;
        
        private List<Ship> _shipToPlace2;
        private List<Ship> _shipsPlaced2;

        private List<Squadron> _squadToPlace1;
        private List<Squadron> _squadsPlaced1;

        private List<Squadron> _squadToPlace2;
        private List<Squadron> _squadsPlaced2;

        private int[] _squadTypeAmounts1;
        private int[] _squadTypeAmounts2;

        private Ship _selectedShip;
        private Squadron _selectedSquad;

        private List<CustButton> _buttons;

        private ContentManager _content;

        private Texture2D _background;
        private Texture2D _texture;
        private Texture2D _label;
        private Texture2D _gradient;
        private Texture2D _player1Zone;
        private Texture2D _player2Zone;

        private SpriteFont _descriptor;
        private SpriteFont _galbasic;

        private int _widthIncrement;
        private int _heightIncrement;
        private int _roundNum;
        private int _numToPlace;

        private string _selectingPlayer;

        private bool _player1Turn;
        private bool _player1Start;
        private bool _player1Placing;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private SoundEffect _button1;
        private SoundEffect _button2;
        private SoundEffect _button3;
        private SoundEffect _button4;

        private List<float> _vol;

        private GameEnum _state;
        private SetupState _setupState;
        private ShipState _shipState;
        private AttackState _attackState;
        private SquadronState _squadState;
        private SquadronCommand _squadCommand;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public HotseatDuelScreen(Game game, List<float> vol, Fleet player1, Fleet player2)
        {
            _game = game;
            _vol = vol;
            _player1 = player1;
            _player2 = player2;

            _shipToPlace1 = _player1.Ships;
            _shipsPlaced1 = new List<Ship>();
            _shipToPlace2 = _player2.Ships;
            _shipsPlaced2 = new List<Ship>();
            _squadToPlace1 = _player1.Squadrons;
            _squadsPlaced1 = new List<Squadron>();
            _squadToPlace2 = _player2.Squadrons;
            _squadsPlaced2 = new List<Squadron>();

            _squadTypeAmounts1 = returnSquads(_player1.Squadrons, _player1.IsRebelFleet);
            _squadTypeAmounts2 = returnSquads(_player2.Squadrons, _player2.IsRebelFleet);

            _state = GameEnum.Setup;
            _setupState = SetupState.SelectFirst;
            _shipState = ShipState.ActivateShip;
            _attackState = AttackState.DeclareTarget;
            _squadState = SquadronState.ActivateSquad;
            _squadCommand = SquadronCommand.ActivateSquadron;

            _roundNum = 0;
            _numToPlace = 0;

            _widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            _buttons = new List<CustButton>();
            //Buttons go here
            _buttons.Add(new CustButton(0, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 8, _widthIncrement * 18, _heightIncrement * 7), true));    //Quit game

            _buttons.Add(new CustButton(1, new Rectangle(_widthIncrement * 38, 50 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));         //lowest player wants to go first
            _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement * 52, 50 * _heightIncrement, _widthIncrement * 10, _heightIncrement * 15), true));         //lowest player wants to go second

            _buttons.Add(new CustButton(3, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false)); //-----------------
            _buttons.Add(new CustButton(4, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));
            _buttons.Add(new CustButton(5, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 9 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));
            _buttons.Add(new CustButton(6, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 9 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));
            _buttons.Add(new CustButton(7, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 17 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false)); //Ship placement buttons
            _buttons.Add(new CustButton(8, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 17 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));
            _buttons.Add(new CustButton(9, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 25 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));
            _buttons.Add(new CustButton(10, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 25 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));
            _buttons.Add(new CustButton(11, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 37 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false)); //-----------------

            _buttons.Add(new CustButton(12, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 40 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //-----------------
            _buttons.Add(new CustButton(13, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 51 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false));
            _buttons.Add(new CustButton(14, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 62 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //Squadron placement buttons
            _buttons.Add(new CustButton(15, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 73 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //----------------

        }

        /// <summary>
        /// For when the screen activates
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _descriptor = _content.Load<SpriteFont>("descriptor");
            _galbasic = _content.Load<SpriteFont>("galbasic");

            _texture = _content.Load<Texture2D>("MetalBackground");
            _gradient = _content.Load<Texture2D>("MenuGradient2");

            //Load button textures here
            _buttons[0].Texture = _content.Load<Texture2D>("QuitGame");
            _buttons[1].Texture = _content.Load<Texture2D>("GoFirst");
            _buttons[2].Texture = _content.Load<Texture2D>("GoSecond");
            //_buttons[3].Texture = _content.Load<Texture2D>("");

            //_label = _content.Load<Texture2D>("");
            _background = _content.Load<Texture2D>("SpaceBackground1");
            _player1Zone = _content.Load<Texture2D>("Player1PlacementArea");
            _player2Zone = _content.Load<Texture2D>("Player2PlacementArea");

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

                    switch(_setupState)
                    {
                        case SetupState.SelectFirst:
                            if (_player1.TotalPoints <= _player2.TotalPoints) _selectingPlayer = "Player 1";
                            else _selectingPlayer = "Player 2";
                            break;
                        case SetupState.Placement:
                            if (_shipToPlace1.Count == 0 && _shipToPlace2.Count == 0 && _squadToPlace1.Count == 0 && _squadToPlace2.Count == 0)
                            {
                                _setupState = SetupState.SetupDone;
                            }
                            else
                            {
                                buttonSweeper(3);
                                if (_player1Placing)
                                {
                                    for(int i = 0; i < _shipToPlace1.Count; i++)
                                    {
                                        _buttons[i + 3].IsActive = true;
                                    }
                                    setFleetShipButtons(_shipToPlace1);


                                    int numDiff = 0;
                                    for(int i = 0; i < _squadTypeAmounts1.Length; i++)
                                    {
                                        if (_squadTypeAmounts1[i] > 0) numDiff++;
                                    }

                                    for(int i = 0; i < numDiff; i++)
                                    {
                                        _buttons[i + 12].IsActive = true;
                                    }
                                    setFleetSquadronButtons(_player1.Squadrons, _player1.IsRebelFleet);

                                    //placing ship
                                    if(_selectedShip != null)
                                    {
                                        if (_currentMouseState.X >= _selectedShip.Image.Width / 2 && _currentMouseState.X <= (_game.GraphicsDevice.Viewport.Width - _widthIncrement * 20) - _selectedShip.Image.Width / 2 && _currentMouseState.Y >= (_game.GraphicsDevice.Viewport.Height - _heightIncrement * 25) + _selectedShip.Image.Height / 2 && _currentMouseState.Y <= _game.GraphicsDevice.Viewport.Height - _selectedShip.Image.Height / 2)
                                        {
                                            if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                            {
                                                if(isPlacedTooCLose(true))
                                                {
                                                    _selectedShip.Position = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                                                    _shipToPlace1.Remove(_selectedShip);
                                                    _shipsPlaced1.Add(_selectedShip);
                                                    _selectedShip = null;
                                                    _numToPlace--;

                                                    if (_shipToPlace2.Count > 0 || _squadToPlace2.Count > 0) _player1Placing = false;
                                                    else _player1Placing = true;
                                                }
                                            }
                                        }
                                    }

                                    //placing squad
                                    if(_selectedSquad != null)
                                    {
                                        if(_currentMouseState.X >= 30 && _currentMouseState.X <= (_game.GraphicsDevice.Viewport.Width - _widthIncrement * 20) - 30 && _currentMouseState.Y >= (_game.GraphicsDevice.Viewport.Height - _heightIncrement * 25) + 30 && _currentMouseState.Y <= _game.GraphicsDevice.Viewport.Height - 30)
                                        {
                                            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                            {
                                                if (isPlacedTooCLose(false))
                                                {
                                                    _selectedSquad.Position = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                                                    _selectedSquad.Rotation = MathHelper.Pi;
                                                    Squadron s = _selectedSquad;
                                                    if (_squadToPlace1.Count >= 2) 
                                                    {
                                                        if(_selectedSquad is TIEFighterSquadron || _selectedSquad is AWingSquadron)
                                                        {
                                                            if(_squadTypeAmounts1[0] >= 2) _selectedSquad = _squadToPlace1[_squadToPlace1.IndexOf(_selectedSquad) + 1];
                                                        }
                                                        else if (_selectedSquad is TIEAdvancedSquadron || _selectedSquad is BWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts1[1] >= 2) _selectedSquad = _squadToPlace1[_squadToPlace1.IndexOf(_selectedSquad) + 1];
                                                        }
                                                        else if (_selectedSquad is TIEInterceptorSquadron || _selectedSquad is XWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts1[2] >= 2) _selectedSquad = _squadToPlace1[_squadToPlace1.IndexOf(_selectedSquad) + 1];
                                                        }
                                                        else if (_selectedSquad is TIEBomberSquadron || _selectedSquad is YWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts1[3] >= 2) _selectedSquad = _squadToPlace1[_squadToPlace1.IndexOf(_selectedSquad) + 1];
                                                        }
                                                    }
                                                    

                                                    _squadToPlace1.Remove(s);
                                                    _squadsPlaced1.Add(s);

                                                    if (_selectedSquad is TIEFighterSquadron || _selectedSquad is AWingSquadron) _squadTypeAmounts1[0]--;
                                                    if (_selectedSquad is TIEAdvancedSquadron || _selectedSquad is BWingSquadron) _squadTypeAmounts1[1]--;
                                                    if (_selectedSquad is TIEInterceptorSquadron || _selectedSquad is XWingSquadron) _squadTypeAmounts1[2]--;
                                                    if (_selectedSquad is TIEBomberSquadron || _selectedSquad is YWingSquadron) _squadTypeAmounts1[3]--;

                                                    _numToPlace--;
                                                    if (_numToPlace <= 0) 
                                                    {
                                                        _selectedSquad = null;

                                                        if (_shipToPlace2.Count > 0 || _squadToPlace2.Count > 0) _player1Placing = false;
                                                        else _player1Placing = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else //Player 2 is placing
                                {
                                    for (int i = 0; i < _shipToPlace2.Count; i++)
                                    {
                                        _buttons[i + 3].IsActive = true;
                                        setFleetShipButtons(_shipToPlace2);
                                    }

                                    int numDiff = 0;
                                    for (int i = 0; i < _squadTypeAmounts2.Length; i++)
                                    {
                                        if (_squadTypeAmounts2[i] > 0) numDiff++;
                                    }

                                    for (int i = 0; i < numDiff; i++)
                                    {
                                        _buttons[i + 12].IsActive = true;
                                    }
                                    setFleetSquadronButtons(_player2.Squadrons, _player2.IsRebelFleet);

                                    //placing ship
                                    if (_selectedShip != null)
                                    {
                                        if (_currentMouseState.X >= _selectedShip.Image.Width / 2 && _currentMouseState.X <= (_game.GraphicsDevice.Viewport.Width - _widthIncrement * 20) - _selectedShip.Image.Width / 2 && _currentMouseState.Y >= _selectedShip.Image.Height / 2 && _currentMouseState.Y <= (_game.GraphicsDevice.Viewport.Height / 4) - _selectedShip.Image.Height / 2)
                                        {
                                            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                            {
                                                if (isPlacedTooCLose(true))
                                                {
                                                    _selectedShip.Position = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                                                    _selectedShip.Rotation = MathHelper.Pi;
                                                    _shipToPlace2.Remove(_selectedShip);
                                                    _shipsPlaced2.Add(_selectedShip);
                                                    _selectedShip = null;
                                                    _numToPlace--;

                                                    if (_shipToPlace1.Count > 0 || _squadToPlace1.Count > 0) _player1Placing = true;
                                                    else _player1Placing = false;
                                                }
                                            }
                                        }
                                    }

                                    //placing squad
                                    if (_selectedSquad != null)
                                    {
                                        if (_currentMouseState.X >= 30 && _currentMouseState.X <= (_game.GraphicsDevice.Viewport.Width - _widthIncrement * 20) - 30 && _currentMouseState.Y >= 30 && _currentMouseState.Y <= (_game.GraphicsDevice.Viewport.Height / 4) - 30)
                                        {
                                            if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                            {
                                                if (isPlacedTooCLose(false))
                                                {
                                                    _selectedSquad.Position = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                                                    _selectedSquad.Rotation = MathHelper.Pi;
                                                    Squadron s = _selectedSquad;
                                                    if (_squadToPlace2.Count >= 2)
                                                    {
                                                        if (_selectedSquad is TIEFighterSquadron || _selectedSquad is AWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts2[0] >= 2) _selectedSquad = _squadToPlace2[_squadToPlace2.IndexOf(_selectedSquad) + 1];
                                                        }
                                                        else if (_selectedSquad is TIEAdvancedSquadron || _selectedSquad is BWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts2[1] >= 2) _selectedSquad = _squadToPlace2[_squadToPlace2.IndexOf(_selectedSquad) + 1];
                                                        }
                                                        else if (_selectedSquad is TIEInterceptorSquadron || _selectedSquad is XWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts2[2] >= 2) _selectedSquad = _squadToPlace2[_squadToPlace2.IndexOf(_selectedSquad) + 1];
                                                        }
                                                        else if (_selectedSquad is TIEBomberSquadron || _selectedSquad is YWingSquadron)
                                                        {
                                                            if (_squadTypeAmounts2[3] >= 2) _selectedSquad = _squadToPlace2[_squadToPlace2.IndexOf(_selectedSquad) + 1];
                                                        }
                                                    }
                                                    _squadToPlace2.Remove(s);
                                                    _squadsPlaced2.Add(s);

                                                    if (_selectedSquad is TIEFighterSquadron || _selectedSquad is AWingSquadron) _squadTypeAmounts2[0]--;
                                                    if (_selectedSquad is TIEAdvancedSquadron || _selectedSquad is BWingSquadron) _squadTypeAmounts2[1]--;
                                                    if (_selectedSquad is TIEInterceptorSquadron || _selectedSquad is XWingSquadron) _squadTypeAmounts2[2]--;
                                                    if (_selectedSquad is TIEBomberSquadron || _selectedSquad is YWingSquadron) _squadTypeAmounts2[3]--;

                                                    _numToPlace--;
                                                    if (_numToPlace <= 0)
                                                    {
                                                        _selectedSquad = null;

                                                        if (_shipToPlace1.Count > 0 || _squadToPlace1.Count > 0) _player1Placing = true;
                                                        else _player1Placing = false;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                            break;
                        case SetupState.SetupDone:
                            break;
                    }
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
                    //activateing and either moving or attacking with them
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
            spriteBatch.Draw(_background, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width - _widthIncrement * 20, _game.GraphicsDevice.Viewport.Height + _heightIncrement * 4), Color.White);

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


            if (_state == GameEnum.Setup && _setupState == SetupState.Placement)
            {
                if (_player1Placing) spriteBatch.Draw(_player1Zone, new Rectangle(0, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 25, _game.GraphicsDevice.Viewport.Width - _widthIncrement * 20, _game.GraphicsDevice.Viewport.Height / 4), Color.White);
                else spriteBatch.Draw(_player2Zone, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width - _widthIncrement * 20, _game.GraphicsDevice.Viewport.Height / 4), Color.White);

                foreach (var ship in _shipsPlaced1)
                {
                    if(_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(ship.Image, ship.Position, null, Color.Red, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                    else spriteBatch.Draw(ship.Image, ship.Position, null, Color.White, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                }
                foreach (var ship in _shipsPlaced2)
                {
                    if(_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(ship.Image, ship.Position, null, Color.Blue, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                    else spriteBatch.Draw(ship.Image, ship.Position, null, Color.White, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                }
                foreach (var squad in _squadsPlaced1)
                {
                    if(_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(squad.Image, squad.Position, null, Color.Red, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                    else spriteBatch.Draw(squad.Image, squad.Position, null, Color.White, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                }
                foreach (var squad in _squadsPlaced2)
                {
                    if(_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(squad.Image, squad.Position, null, Color.Blue, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                    else spriteBatch.Draw(squad.Image, squad.Position, null, Color.White, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                }

                if (_player1Placing)
                {
                    int hOffset = 43;
                    for(int i = 0; i < _squadTypeAmounts1.Length; i++)
                    {
                        if(_squadTypeAmounts1[i] > 0)
                        {
                            spriteBatch.DrawString(_galbasic, "X " + _squadTypeAmounts1[i], new Vector2(_widthIncrement * 92, hOffset * _heightIncrement), Color.White);
                            hOffset += 11;
                        }
                    }
                }
                else
                {
                    int hOffset = 43;
                    for (int i = 0; i < _squadTypeAmounts2.Length; i++)
                    {
                        if (_squadTypeAmounts2[i] > 0)
                        {
                            spriteBatch.DrawString(_galbasic, "X " + _squadTypeAmounts2[i], new Vector2(_widthIncrement * 92, hOffset * _heightIncrement), Color.White);
                            hOffset += 11;
                        }
                    }
                }
            }

            //spriteBatch.Draw(_label, new Vector2(_widthIncrement * 34, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 90), Color.White);


            if(_setupState == SetupState.SelectFirst && _state == GameEnum.Setup)
            {
                spriteBatch.DrawString(_galbasic, _selectingPlayer + ": What do you want to do?", new Vector2(_widthIncrement * 34, 40 * _heightIncrement), Color.Gold);
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
                case 1: //selecting player wants to go first
                    _button1.Play();
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_selectingPlayer.Equals("Player 1")) _player1Start = true;
                        else _player1Start = false;

                        _player1Placing = _player1Start;

                        _buttons[1].IsActive = false;
                        _buttons[2].IsActive = false;
                        _selectingPlayer = "";

                        _setupState = SetupState.Placement;
                    }
                    break;
                case 2: //selecting player wants to go second
                    _button2.Play();
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_selectingPlayer.Equals("Player 1")) _player1Start = false;
                        else _player1Start = true;

                        _player1Placing = _player1Start;

                        _buttons[1].IsActive = false;
                        _buttons[2].IsActive = false;
                        _selectingPlayer = "";

                        _setupState = SetupState.Placement;
                    }
                    break;
                case 3:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if(_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[0];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[0];
                        }
                    }
                    break;
                case 4:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[1];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[1];
                        }
                    }
                    break;
                case 5:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[2];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[2];
                        }
                    }
                    break;
                case 6:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[3];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[3];
                        }
                    }
                    break;
                case 7:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[4];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[4];
                        }
                    }
                    break;
                case 8:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[5];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[5];
                        }
                    }
                    break;
                case 9:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[6];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[6];
                        }
                    }
                    break;
                case 10:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[7];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[7];
                        }
                    }
                    break;
                case 11:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _selectedSquad = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace1[8];
                        }
                        else
                        {
                            _numToPlace = 1;
                            _selectedShip = _shipToPlace2[8];
                        }
                    }
                    break;
                case 12: //select squad to place
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released && _numToPlace == 0)
                    {
                        _selectedShip = null;
                        _button3.Play();
                        if(_player1Placing)
                        {
                            if(_squadTypeAmounts1[getNumNonZero(1, _squadTypeAmounts1)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace1[0];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace1[0];
                            }
                        }
                        else
                        {
                            if (_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace2[0];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace2[0];
                            }
                        }
                    }
                    break;
                case 13:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released && _numToPlace == 0)
                    {
                        _selectedShip = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            if (_squadTypeAmounts1[getNumNonZero(2, _squadTypeAmounts1)] >= 2)
                            {
                                _numToPlace = 2;
                                int temp = getNumNonZero(1, _squadTypeAmounts1);
                                _selectedSquad = _squadToPlace1[_squadTypeAmounts1[temp]];
                            }
                            else
                            {
                                _numToPlace = 1;
                                int temp = getNumNonZero(1, _squadTypeAmounts1);
                                _selectedSquad = _squadToPlace1[_squadTypeAmounts1[temp]];
                            }
                        }
                        else
                        {
                            if (_squadTypeAmounts2[getNumNonZero(2, _squadTypeAmounts2)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace2[_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)]];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace2[_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)]];
                            }
                        }
                    }
                    break;
                case 14:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released && _numToPlace == 0)
                    {
                        _selectedShip = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            if (_squadTypeAmounts1[getNumNonZero(3, _squadTypeAmounts1)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace1[_squadTypeAmounts1[getNumNonZero(1, _squadTypeAmounts1)] + _squadTypeAmounts1[getNumNonZero(2, _squadTypeAmounts1)]];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace1[_squadTypeAmounts1[getNumNonZero(1, _squadTypeAmounts1)] + _squadTypeAmounts1[getNumNonZero(2, _squadTypeAmounts1)]];
                            }
                        }
                        else
                        {
                            if (_squadTypeAmounts2[getNumNonZero(3, _squadTypeAmounts2)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace2[_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)] + _squadTypeAmounts2[getNumNonZero(2, _squadTypeAmounts2)]];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace2[_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)] + _squadTypeAmounts2[getNumNonZero(2, _squadTypeAmounts2)]];
                            }
                        }
                    }
                    break;
                case 15:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released && _numToPlace == 0)
                    {
                        _selectedShip = null;
                        _button3.Play();
                        if (_player1Placing)
                        {
                            if (_squadTypeAmounts1[getNumNonZero(4, _squadTypeAmounts1)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace1[_squadTypeAmounts1[getNumNonZero(1, _squadTypeAmounts1)] + _squadTypeAmounts1[getNumNonZero(2, _squadTypeAmounts1)] + _squadTypeAmounts1[getNumNonZero(3, _squadTypeAmounts1)]];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace1[_squadTypeAmounts1[getNumNonZero(1, _squadTypeAmounts1)] + _squadTypeAmounts1[getNumNonZero(2, _squadTypeAmounts1)] + _squadTypeAmounts1[getNumNonZero(3, _squadTypeAmounts1)]];
                            }
                        }
                        else
                        {
                            if (_squadTypeAmounts2[getNumNonZero(4, _squadTypeAmounts2)] >= 2)
                            {
                                _numToPlace = 2;
                                _selectedSquad = _squadToPlace2[_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)] + _squadTypeAmounts2[getNumNonZero(2, _squadTypeAmounts2)] + _squadTypeAmounts2[getNumNonZero(3, _squadTypeAmounts2)]];
                            }
                            else
                            {
                                _numToPlace = 1;
                                _selectedSquad = _squadToPlace2[_squadTypeAmounts2[getNumNonZero(1, _squadTypeAmounts2)] + _squadTypeAmounts2[getNumNonZero(2, _squadTypeAmounts2)] + _squadTypeAmounts2[getNumNonZero(3, _squadTypeAmounts2)]];
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the fleet in a readable manner
        /// </summary>
        /// <param name="fleet">The fleet to be drawn</param>
        /// <param name="sb">The sprite batch used to draw the fleet</param>
        private void drawFleetInfo(Fleet fleet, SpriteBatch sb)
        {

            double heightoffset = 1;//to be set
            sb.DrawString(_descriptor, "FLEET: " + fleet.Name + "--" + fleet.TotalPoints + " total points", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (int)(heightoffset * _heightIncrement)), Color.AntiqueWhite);

            heightoffset += 1.5;
            foreach (var ship in fleet.Ships)
            {
                if (ship != null)
                {
                    if (ship.ShipTypeA)
                    {
                        sb.DrawString(_descriptor, " >" + ship.Name + "(A): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                    }
                    else
                    {
                        sb.DrawString(_descriptor, " >" + ship.Name + "(B): " + ship.PointCost + "---------------", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                    }
                    heightoffset += 1.5;

                    if (ship.Commander != null)
                    {
                        sb.DrawString(_descriptor, "   -" + "Commander " + ship.Commander.Name + ": " + ship.Commander.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }

                    if (ship.Title != null)
                    {
                        sb.DrawString(_descriptor, "   -" + "Title " + ship.Title.Name + ": " + ship.Title.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }

                    foreach (var upgrade in ship.Upgrades)
                    {
                        if (upgrade != null)
                        {
                            sb.DrawString(_descriptor, "   -" + upgrade.CardType.ToString() + " " + upgrade.Name + ": " + upgrade.PointCost, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                            heightoffset += 1.5;
                        }
                    }
                }
            }

            if (fleet.Squadrons.Count > 0)
            {
                int[] sq = returnSquads(fleet.Squadrons, fleet.IsRebelFleet);
                if (fleet.IsRebelFleet)
                {
                    if (sq[0] > 0)
                    {
                        sb.DrawString(_descriptor, "   -A-Wing Squadron(11): x" + sq[0] + " => " + sq[0] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                    if (sq[1] > 0)
                    {
                        sb.DrawString(_descriptor, "   -B-Wing Squadron(14): x" + sq[1] + " => " + sq[1] * 14, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                    if (sq[2] > 0)
                    {
                        sb.DrawString(_descriptor, "   -X-Wing Squadron(13): x" + sq[2] + " => " + sq[2] * 13, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                    if (sq[3] > 0)
                    {
                        sb.DrawString(_descriptor, "   -Y-Wing Squadron(10): x" + sq[3] + " => " + sq[3] * 10, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                }
                else
                {
                    if (sq[0] > 0)
                    {
                        sb.DrawString(_descriptor, "   -TIE Fighter Squadron(8): x" + sq[0] + " => " + sq[0] * 8, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                    if (sq[1] > 0)
                    {
                        sb.DrawString(_descriptor, "   -TIE Advanced Squadron(12): x" + sq[1] + " => " + sq[1] * 12, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                    if (sq[2] > 0)
                    {
                        sb.DrawString(_descriptor, "   -TIE Interceptor Squadron(11): x" + sq[2] + " => " + sq[2] * 11, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                    if (sq[3] > 0)
                    {
                        sb.DrawString(_descriptor, "   -TIE Bomber Squadron(9): x" + sq[3] + " => " + sq[3] * 9, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 17, (float)(heightoffset * _heightIncrement)), Color.AntiqueWhite);
                        heightoffset += 1.5;
                    }
                }
            }
        }

        /// <summary>
        /// gets the number of squads of each type
        /// </summary>
        /// <returns>the list of number of squads by type</returns>
        private int[] returnSquads(List<Squadron> squad, bool isRebelFleet)
        {
            int[] result = new int[4];
            if (isRebelFleet)
            {
                result[0] += squad.FindAll(x => x.Name == "A-Wing Squadron").Count;
                result[1] += squad.FindAll(x => x.Name == "B-Wing Squadron").Count;
                result[2] += squad.FindAll(x => x.Name == "X-Wing Squadron").Count;
                result[3] += squad.FindAll(x => x.Name == "Y-Wing Squadron").Count;
            }
            else
            {
                result[0] += squad.FindAll(x => x.Name == "TIE Fighter Squadron").Count;
                result[1] += squad.FindAll(x => x.Name == "TIE Advanced Squadron").Count;
                result[2] += squad.FindAll(x => x.Name == "TIE Interceptor Squadron").Count;
                result[3] += squad.FindAll(x => x.Name == "TIE Bomber Squadron").Count;
            }

            return result;
        }

        /// <summary>
        /// Sets the images for the fleet remover buttons.
        /// </summary>
        private void setFleetShipButtons(List<Ship> ships)
        {
            for (int i = 0; i < ships.Count; i++)
            {                                               //Adjust button numbers
                if (ships[i] is AssaultFrigateMarkII) _buttons[i + 3].Texture = _content.Load<Texture2D>("AssaultFrigate");
                else if (ships[i] is CR90Corvette) _buttons[i + 3].Texture = _content.Load<Texture2D>("CR90Corvette");
                else if (ships[i] is NebulonBFrigate) _buttons[i + 3].Texture = _content.Load<Texture2D>("NebulonB");
                else if (ships[i] is GladiatorStarDestroyer) _buttons[i + 3].Texture = _content.Load<Texture2D>("GladiatorSD");
                else if (ships[i] is VictoryStarDestroyer) _buttons[i + 3].Texture = _content.Load<Texture2D>("VictorySD");
            }
        }

        /// <summary>
        /// Sets the squadrons in the 
        /// </summary>
        /// <param name="fleet"></param>
        private void setFleetSquadronButtons(List<Squadron> squads, bool isRebelFleet)
        {
            int x = 0;
            int[] s = returnSquads(squads, isRebelFleet);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > 0)
                {                               //update the buttons numbers
                    if (isRebelFleet)
                    {
                        if (i == 0) _buttons[x + 12].Texture = _content.Load<Texture2D>("AWings");
                        if (i == 1) _buttons[x + 12].Texture = _content.Load<Texture2D>("BWings");
                        if (i == 2) _buttons[x + 12].Texture = _content.Load<Texture2D>("XWings");
                        if (i == 3) _buttons[x + 12].Texture = _content.Load<Texture2D>("YWings");
                    }
                    else
                    {
                        if (i == 0) _buttons[x + 12].Texture = _content.Load<Texture2D>("TIEFighters");
                        if (i == 1) _buttons[x + 12].Texture = _content.Load<Texture2D>("TIEAdvanced");
                        if (i == 2) _buttons[x + 12].Texture = _content.Load<Texture2D>("TIEInterceptor");
                        if (i == 3) _buttons[x + 12].Texture = _content.Load<Texture2D>("TIEBombers");
                    }
                    x++;
                }
            }
        }

        /// <summary>
        /// Tests to see if the current placement is too close to another ship or squadron
        /// </summary>
        /// <param name="isShip">If you're placing a ship or a squadron</param>
        /// <returns>false if its too close or true if its good</returns>
        private bool isPlacedTooCLose(bool isShip)
        {
            if(isShip)
            {
                if (_player1Placing)
                {
                    foreach (var squad in _squadsPlaced1)
                    {
                        if (Math.Abs(_currentMouseState.X - squad.Bounds.Center.X) < 110 && Math.Abs(_currentMouseState.Y - squad.Bounds.Center.Y) < 130)
                        {
                            return false;
                        }
                    }

                    foreach (var ship in _shipsPlaced1)
                    {
                        if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < 155 && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < 210)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    foreach (var squad in _squadsPlaced2)
                    {
                        if (Math.Abs(_currentMouseState.X - squad.Bounds.Center.X) < 110 && Math.Abs(_currentMouseState.Y - squad.Bounds.Center.Y) < 130)
                        {
                            return false;
                        }
                    }

                    foreach (var ship in _shipsPlaced2)
                    {
                        if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < 155 && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < 210)
                        {
                            return false;
                        }
                    }
                }
            }
            else //is a squad
            {
                if(_player1Placing)
                {
                    foreach (var squad in _squadsPlaced1)
                    {
                        if(Math.Abs(_currentMouseState.X - squad.Bounds.Center.X) < 51 && Math.Abs(_currentMouseState.Y - squad.Bounds.Center.Y) < 51)
                        {
                            return false;
                        }
                    }

                    foreach(var ship in _shipsPlaced1)
                    {
                        if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < 110 && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < 130)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    foreach (var squad in _squadsPlaced2)
                    {
                        if (Math.Abs(_currentMouseState.X - squad.Bounds.Center.X) < 51 && Math.Abs(_currentMouseState.Y - squad.Bounds.Center.Y) < 51)
                        {
                            return false;
                        }
                    }

                    foreach (var ship in _shipsPlaced2)
                    {
                        if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < 110 && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < 130)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
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
        /// OUtputs the next not zero index on an int array
        /// </summary>
        /// <param name="num">the num'th non zero index you want</param>
        /// <param name="array">the array to search through</param>
        /// <returns>the right index</returns>
        private int getNumNonZero(int num, int[] array)
        {
            int result = 0;
            int output = 0;
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] > 0) result++;
                if (result == num) return i;
            }
            return output;
        }
    }
}
