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
using CIS598_Senior_Project.Collisions;

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
            Navigation,
            Engineering,
            Squadron,
            UseToken,
            Attack,
            ExecuteManuver
        }

            enum AttackState
            {
                SelectArc,
                DeclareTarget,
                RollDice,
                ModifyDice,
                SpendAccuracies,
                SpendDefenseTokens,
                ResolveDamage,
                Done
            }
        
        enum SquadronState
        {
            ActivateSquad,
            Move,
            Attack,
            Choose
        }

        enum SquadronCommand
        {
            ActivateSquadron,
            Move,
            Attack, 
            Choose
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
        private Ship _targetedShip;
        private Squadron _selectedSquad;
        private Squadron _targetedSquadron;

        private CommandDialEnum _revealedCommand;
        private CommandDialEnum _selectedDial;

        private List<CustButton> _buttons;

        private ContentManager _content;

        private Texture2D _background;
        private Texture2D _texture;
        private Texture2D _label;
        private Texture2D _gradient;
        private Texture2D _player1Zone;
        private Texture2D _player2Zone;
        private Texture2D _shipRanges;
        private Texture2D _shipIonRange;
        private Texture2D _shipToSquadRange;
        private Texture2D _squadMove1;
        private Texture2D _squadMove2;
        private Texture2D _squadMove3;
        private Texture2D _squadMove4;
        private Texture2D _squadMove5;
        private Texture2D _range;

        private SpriteFont _descriptor;
        private SpriteFont _galbasic;

        private int _widthIncrement;
        private int _heightIncrement;
        private int _roundNum;
        private int _numToPlace;
        private int _speedDiff;
        private int _engineeringPoints;
        private int _numSquadsToActivate;

        private int _selectedArc;
        private int _targetArc;

        private int _sq2SqDamage;
        private int _sq2ShDamage;
        private int _sh2ShDamage;
        private int _sh2SqDamage;
        private int _counterDamage;

        private int _numAttackRemaining;

        private string _selectingPlayer;

        private bool _player1Turn;
        private bool _player1Start;
        private bool _player1Placing;
        private bool _usedToken;

        private bool _squadHasMoved;
        private bool _squadHasAttacked;
        private bool _attackHasCrits;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private SoundEffect _button1;
        private SoundEffect _button2;
        private SoundEffect _button3;
        private SoundEffect _button4;
        private SoundEffect _shoot1;
        private SoundEffect _shoot2;
        private SoundEffect _shoot3;
        private SoundEffect _explosion;

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

            _engineeringPoints = 0;

            _sq2ShDamage = 0;
            _sq2SqDamage = 0;
            _sh2ShDamage = 0;
            _sh2SqDamage = 0;

            _numAttackRemaining = 0;

            _selectedArc = -1;

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
            _attackState = AttackState.SelectArc;
            _squadState = SquadronState.ActivateSquad;
            _squadCommand = SquadronCommand.ActivateSquadron;

            _roundNum = 1;
            _numToPlace = 0;
            _speedDiff = 0;

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

            _buttons.Add(new CustButton(16, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 54 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //Nav command 
            _buttons.Add(new CustButton(17, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 54 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //Eng command 
            _buttons.Add(new CustButton(18, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 65 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //Sqd command 
            _buttons.Add(new CustButton(19, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 65 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false)); //Con command 
            _buttons.Add(new CustButton(20, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 76 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 7), false)); //Set Dial button

            _buttons.Add(new CustButton(21, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 62 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 10), false));     //The activate ship button

            _buttons.Add(new CustButton(22, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 62 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 10), false));      //The revealed command Dial
            _buttons.Add(new CustButton(23, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 62 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 4), false));        //Use dial
            _buttons.Add(new CustButton(24, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 68 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 4), false));        //Save as token for later

            _buttons.Add(new CustButton(25, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 62 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));      //increase speed
            _buttons.Add(new CustButton(26, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 73 * _heightIncrement, _widthIncrement * 8, _heightIncrement * 7), false));      //decrease speed
            _buttons.Add(new CustButton(27, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9,  62* _heightIncrement, _widthIncrement * 8, _heightIncrement * 18), false));      //set new speed

            _buttons.Add(new CustButton(28, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 42 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 7), false));   //restore shields -----no longer used
            _buttons.Add(new CustButton(29, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 35 * _heightIncrement, _widthIncrement * 12, _heightIncrement * 7), false));   //restore Bow shields
            _buttons.Add(new CustButton(30, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 43 * _heightIncrement, _widthIncrement * 5, _heightIncrement * 10), false));   //restore Port shields
            _buttons.Add(new CustButton(31, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 43 * _heightIncrement, _widthIncrement * 5, _heightIncrement * 10), false));   //restore Starboard shields
            _buttons.Add(new CustButton(32, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 54 * _heightIncrement, _widthIncrement * 12, _heightIncrement * 7), false));   //restore Aft shields
            _buttons.Add(new CustButton(33, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 82 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 4), false));   //Back button ------no longer used
            _buttons.Add(new CustButton(34, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 24 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 9), false));   //Restore Hull button
            _buttons.Add(new CustButton(35, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 63 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));   //Done with engineering

            _buttons.Add(new CustButton(36, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 63 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 6), false));  //use engineering token
            _buttons.Add(new CustButton(37, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 70 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 6), false));  //use squadron token
            _buttons.Add(new CustButton(38, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 77 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 6), false));   //Done

            _buttons.Add(new CustButton(39, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 75 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));         //activate squad
            _buttons.Add(new CustButton(40, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 66 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));       //Squad move
            _buttons.Add(new CustButton(41, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 57 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));       //squad attack
            _buttons.Add(new CustButton(42, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 49 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));         //Done with squad

            _buttons.Add(new CustButton(43, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 75 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));          //Attack with squadron

            _buttons.Add(new CustButton(44, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 51 * _heightIncrement, _widthIncrement * 12, _heightIncrement * 7), false));          //selecting bow arc
            _buttons.Add(new CustButton(45, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 59 * _heightIncrement, _widthIncrement * 5, _heightIncrement * 10), false));          //select port arc
            _buttons.Add(new CustButton(46, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 9, 59 * _heightIncrement, _widthIncrement * 5, _heightIncrement * 10), false));           //select starboard arc
            _buttons.Add(new CustButton(47, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 70 * _heightIncrement, _widthIncrement * 12, _heightIncrement * 7), false));          //secelt aft arc
            _buttons.Add(new CustButton(48, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 78 * _heightIncrement, _widthIncrement * 18, _heightIncrement * 8), false));          //confirm selection

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

            _buttons[16].Texture = _content.Load<Texture2D>("NavCommand");
            _buttons[17].Texture = _content.Load<Texture2D>("EngCommand");
            _buttons[18].Texture = _content.Load<Texture2D>("SquadCommand");
            _buttons[19].Texture = _content.Load<Texture2D>("ConcentrateFireCommand");
            _buttons[20].Texture = _content.Load<Texture2D>("SetCommandDial");

            _buttons[21].Texture = _content.Load<Texture2D>("ActivateShip");

            _buttons[23].Texture = _content.Load<Texture2D>("UseDial");
            _buttons[24].Texture = _content.Load<Texture2D>("SaveAsToken");

            _buttons[25].Texture = _content.Load<Texture2D>("Increase");
            _buttons[26].Texture = _content.Load<Texture2D>("Decrease");
            _buttons[27].Texture = _content.Load<Texture2D>("SetSpeed");

            _buttons[28].Texture = _content.Load<Texture2D>("RestoreShields");
            _buttons[29].Texture = _content.Load<Texture2D>("BowShields");
            _buttons[30].Texture = _content.Load<Texture2D>("PortShields");
            _buttons[31].Texture = _content.Load<Texture2D>("StarboardShields");
            _buttons[32].Texture = _content.Load<Texture2D>("AftShields");
            _buttons[33].Texture = _content.Load<Texture2D>("Back");
            _buttons[34].Texture = _content.Load<Texture2D>("RepairHull");
            _buttons[35].Texture = _content.Load<Texture2D>("Done");

            _buttons[36].Texture = _content.Load<Texture2D>("UseEngToken");
            _buttons[37].Texture = _content.Load<Texture2D>("UseSquadToken");
            _buttons[38].Texture = _content.Load<Texture2D>("Done");

            _buttons[39].Texture = _content.Load<Texture2D>("ActivateSquad");
            _buttons[40].Texture = _content.Load<Texture2D>("MoveSquad");
            _buttons[41].Texture = _content.Load<Texture2D>("AttackSquad");
            _buttons[42].Texture = _content.Load<Texture2D>("Done");

            _buttons[43].Texture = _content.Load<Texture2D>("AttackSquad");

            _buttons[44].Texture = _content.Load<Texture2D>("BowArc");
            _buttons[45].Texture = _content.Load<Texture2D>("PortArc");
            _buttons[46].Texture = _content.Load<Texture2D>("StarboardArc");
            _buttons[47].Texture = _content.Load<Texture2D>("AftArc");
            _buttons[48].Texture = _content.Load<Texture2D>("SelectArc");


            //_label = _content.Load<Texture2D>("");
            _background = _content.Load<Texture2D>("SpaceBackground1");
            _player1Zone = _content.Load<Texture2D>("Player1PlacementArea");
            _player2Zone = _content.Load<Texture2D>("Player2PlacementArea");

            _shipRanges = _content.Load<Texture2D>("ShipRangeFinder");
            _shipIonRange = _content.Load<Texture2D>("ShipIonRange");
            _shipToSquadRange = _content.Load<Texture2D>("ShipToSquadRange");
            _squadMove1 = _content.Load<Texture2D>("SquadMovement1");
            _squadMove2 = _content.Load<Texture2D>("SquadMovement2");
            _squadMove3 = _content.Load<Texture2D>("SquadMovement3");
            _squadMove4 = _content.Load<Texture2D>("SquadMovement4");
            _squadMove5 = _content.Load<Texture2D>("SquadMovement5");

            _button1 = _content.Load<SoundEffect>("Button1");
            _button2 = _content.Load<SoundEffect>("Button2");
            _button3 = _content.Load<SoundEffect>("Button3");
            _button4 = _content.Load<SoundEffect>("Button4");
            _shoot1 = _content.Load<SoundEffect>("Shoot1");
            _shoot2 = _content.Load<SoundEffect>("Shoot2");
            _shoot3 = _content.Load<SoundEffect>("Shoot3");
            _explosion = _content.Load<SoundEffect>("Explosion1");


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
                                                    _selectedShip.Rotation = -MathHelper.Pi;
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
                            _state = GameEnum.Command_Phase; //move the state forward out of setup

                            //Re-assign to player 1
                            _player1.Ships = _shipsPlaced1;
                            _player1.Squadrons = _squadsPlaced1;

                            //Re-assign to player 2
                            _player2.Ships = _shipsPlaced2;
                            _player2.Squadrons = _squadsPlaced2;

                            _player1Placing = _player1Start;

                            _selectedShip = null;

                            buttonSweeper(3);
                            _buttons[16].IsActive = true;
                            _buttons[17].IsActive = true;
                            _buttons[18].IsActive = true;
                            _buttons[19].IsActive = true;
                            _buttons[20].IsActive = true;
                            break;
                    }
                    break;
                case GameEnum.Command_Phase:
                    if(numLeftToSet(_player1.Ships) == 0 && numLeftToSet(_player2.Ships) == 0)
                    {
                        _state = GameEnum.Ship_Phase;
                        _selectedShip = null;
                        buttonSweeper(16);
                        _player1Placing = _player1Start;
                    }
                    else
                    {
                        if(_player1Placing)
                        {
                            _selectedShip = nextToPlace(_player1.Ships);
                        }
                        else //player 2 is placing
                        {
                            _selectedShip = nextToPlace(_player2.Ships);
                        }
                    }
                    break;
                case GameEnum.Ship_Phase: //phase for activating ships and stuff
                    switch(_shipState)
                    {
                        case ShipState.ActivateShip:
                            if(shipActivationsRemaining(_player1.Ships) == 0 && shipActivationsRemaining(_player2.Ships) == 0)
                            {
                                _state = GameEnum.Squadron_Phase;
                            }
                            else //if there are ships to activate
                            {
                                foreach(var ship in _player1.Ships)
                                {
                                    if(Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < ship.Bounds.Radius && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < ship.Bounds.Radius)
                                    {
                                        if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                        {
                                            _selectedShip = ship;
                                            if (_player1Placing && !ship.BeenActivated) _buttons[21].IsActive = true;
                                            else _buttons[21].IsActive = false;
                                        }
                                    }
                                }
                                foreach(var ship in _player2.Ships)
                                {
                                    if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < ship.Bounds.Radius && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < ship.Bounds.Radius)
                                    {
                                        if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                        {
                                            _selectedShip = ship;
                                            if (!_player1Placing && !ship.BeenActivated) _buttons[21].IsActive = true;
                                            else _buttons[21].IsActive = false;
                                        }
                                    }
                                }
                            }
                            break;
                        case ShipState.RevealDial:
                            buttonSweeper(16);
                            _buttons[22].IsActive = true;

                            if (_revealedCommand == CommandDialEnum.Navigation) _buttons[22].Texture = _content.Load<Texture2D>("NavCommand");
                            if (_revealedCommand == CommandDialEnum.Engineering) _buttons[22].Texture = _content.Load<Texture2D>("EngCommand");
                            if (_revealedCommand == CommandDialEnum.Squadron) _buttons[22].Texture = _content.Load<Texture2D>("SquadCommand");
                            if (_revealedCommand == CommandDialEnum.ConcentrateFire) _buttons[22].Texture = _content.Load<Texture2D>("ConcentrateFireCommand");

                            _buttons[23].IsActive = true;
                            _buttons[24].IsActive = true;
                            break;
                        case ShipState.Navigation: //They use a nav dial
                            buttonSweeper(20);

                            if (_selectedShip.Movement[_selectedShip.Speed + 1, 0] != -1 && _speedDiff - _selectedShip.Speed < 0) _buttons[25].IsActive = true;
                            if (_selectedShip.Speed > 0 && _selectedShip.Speed - _speedDiff > 0) _buttons[26].IsActive = true;

                            _buttons[27].IsActive = true;
                            break;
                        case ShipState.Engineering: //they use an eng dial
                            buttonSweeper(20);
                            _buttons[29].IsActive = true;
                            _buttons[30].IsActive = true;
                            _buttons[31].IsActive = true;
                            _buttons[32].IsActive = true;
                            _buttons[34].IsActive = true;
                            _buttons[35].IsActive = true;

                            break;
                        case ShipState.Squadron: //they use a squadron dial or token
                            switch(_squadCommand)
                            {
                                case SquadronCommand.ActivateSquadron:
                                    if(Math.Sqrt(Math.Pow(_currentMouseState.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - _selectedShip.Bounds.Center.Y, 2)) <= (_shipToSquadRange.Width) / 2)
                                    {
                                        if(_player1Placing)
                                        {
                                            foreach(var squad in _player1.Squadrons)
                                            {
                                                if(Math.Sqrt(Math.Pow(_currentMouseState.X - squad.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - squad.Bounds.Center.Y, 2)) <= 25)
                                                {
                                                    if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released && !squad.HasBeenActivated)
                                                    {
                                                        buttonSweeper(20);
                                                        _selectedSquad = squad;
                                                        _buttons[39].IsActive = true;
                                                    }
                                                }
                                            }
                                        }
                                        else //is player 2's turn
                                        {
                                            foreach (var squad in _player2.Squadrons)
                                            {
                                                if (Math.Sqrt(Math.Pow(_currentMouseState.X - squad.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - squad.Bounds.Center.Y, 2)) <= 25)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released && !squad.HasBeenActivated)
                                                    {
                                                        buttonSweeper(20);
                                                        _selectedSquad = squad;
                                                        _buttons[39].IsActive = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SquadronCommand.Choose:
                                    _selectedSquad.IsEngaged = isEngaged(_player1Placing);

                                    _buttons[39].IsActive = false;

                                    if (!_squadHasMoved) _buttons[40].IsActive = !_selectedSquad.IsEngaged;
                                    else _buttons[40].IsActive = false;

                                    if (!_squadHasAttacked) _buttons[41].IsActive = isSquadTargetNearby(_player1Placing);
                                    else _buttons[41].IsActive = false;

                                    _buttons[42].IsActive = true;

                                    break;
                                case SquadronCommand.Move: //moving the squad
                                    int radius = 0;
                                    if (_selectedSquad.Speed == 1) radius = _squadMove1.Width / 2;
                                    if (_selectedSquad.Speed == 2) radius = _squadMove2.Width / 2;
                                    if (_selectedSquad.Speed == 3) radius = _squadMove3.Width / 2;
                                    if (_selectedSquad.Speed == 4) radius = _squadMove4.Width / 2;
                                    if (_selectedSquad.Speed == 5) radius = _squadMove5.Width / 2;

                                    if (Math.Sqrt(Math.Pow(_currentMouseState.X - _selectedSquad.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - _selectedSquad.Bounds.Center.Y, 2)) <= radius)
                                    {
                                        if(_currentMouseState.X > 25 && _currentMouseState.X < (_game.GraphicsDevice.Viewport.Width - _widthIncrement * 20) - 25 && _currentMouseState.Y > 25 && _currentMouseState.Y < _game.GraphicsDevice.Viewport.Height - 25)
                                        {
                                            if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                            {
                                                if(!isSquadSpotTaken())
                                                {
                                                    _selectedSquad.Position = new Vector2(_currentMouseState.X, _currentMouseState.Y);
                                                    _squadHasMoved = true;
                                                    _squadCommand = SquadronCommand.Choose;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SquadronCommand.Attack: //attacking with the squad

                                    if(isEscortsNearby()) _targetedSquadron = escortsNearby();

                                    if(_targetedSquadron != null)
                                    {
                                        _sq2SqDamage = squadDamage();
                                        _sq2ShDamage = 0;
                                        _sh2ShDamage = 0;
                                        _sh2SqDamage = 0;
                                        buttonSweeper(40);
                                        _buttons[43].IsActive = true;
                                        //activate fire button
                                    }
                                    else if(_targetedShip != null)
                                    {
                                        _sq2SqDamage = 0;
                                        _sq2ShDamage = shipDamage();
                                        _sh2ShDamage = 0;
                                        _sh2SqDamage = 0;
                                        buttonSweeper(40);
                                        _buttons[43].IsActive = true;
                                        //activate fire button
                                    }

                                    if (Math.Sqrt(Math.Pow(_currentMouseState.X - _selectedSquad.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - _selectedSquad.Bounds.Center.Y, 2)) <= (_squadMove1.Width) / 2)
                                    {
                                        //player 1 and 2 selecting targets
                                        if (_player1Placing)
                                        {
                                            foreach (var squad in _player2.Squadrons)
                                            {
                                                if (Math.Sqrt(Math.Pow(_currentMouseState.X - squad.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - squad.Bounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedShip = null;
                                                        _targetedSquadron = squad;
                                                    }
                                                }
                                            }

                                            foreach(var ship in _player2.Ships)
                                            {
                                                if(Math.Sqrt(Math.Pow(_currentMouseState.X - ship.BowBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.BowBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 0;
                                                    }
                                                }
                                                else if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.PortBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.PortBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 1;
                                                    }
                                                }
                                                else if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.StarboardBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 2;
                                                    }
                                                }
                                                else if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.AftBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.AftBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 3;
                                                    }
                                                }
                                            }
                                            
                                        } 
                                        else //player 2 is placing
                                        {
                                            foreach (var squad in _player1.Squadrons)
                                            {
                                                if (Math.Sqrt(Math.Pow(_currentMouseState.X - squad.Bounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - squad.Bounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedShip = null;
                                                        _targetedSquadron = squad;
                                                    }
                                                }
                                            }

                                            foreach (var ship in _player1.Ships)
                                            {
                                                if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.BowBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.BowBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 0;
                                                    }
                                                }
                                                else if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.PortBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.PortBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 1;
                                                    }
                                                }
                                                else if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.StarboardBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 2;
                                                    }
                                                }
                                                else if (Math.Sqrt(Math.Pow(_currentMouseState.X - ship.AftBounds.Center.X, 2) + Math.Pow(_currentMouseState.Y - ship.AftBounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                                                {
                                                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                                                    {
                                                        _targetedSquadron = null;
                                                        _targetedShip = ship;
                                                        _targetArc = 3;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ShipState.Attack: //Ship attack phase

                            switch(_attackState)
                            {
                                case AttackState.SelectArc:
                                    buttonSweeper(40);

                                    if(!areTargetsNearby(0) && !areTargetsNearby(1) && !areTargetsNearby(2) && !areTargetsNearby(3))
                                    {
                                        _shipState = ShipState.ExecuteManuver;
                                    }
                                    else
                                    {
                                        if (areTargetsNearby(0)) _buttons[44].IsActive = true;
                                        if (areTargetsNearby(1)) _buttons[45].IsActive = true;
                                        if (areTargetsNearby(2)) _buttons[46].IsActive = true;
                                        if (areTargetsNearby(3)) _buttons[47].IsActive = true;
                                        _buttons[48].IsActive = true;
                                    }
                                    break;
                                case AttackState.DeclareTarget:
                                    buttonSweeper(40);
                                    break;
                                case AttackState.RollDice:
                                    break;
                                case AttackState.ModifyDice:
                                    break;
                                case AttackState.SpendAccuracies:
                                    break;
                                case AttackState.SpendDefenseTokens:
                                    break;
                                case AttackState.ResolveDamage:
                                    break;
                                case AttackState.Done:
                                    break;
                            }

                            break;
                        case ShipState.UseToken: //Player uses a token after taking a token, or afterinstead of using a dial
                            buttonSweeper(16);

                            _buttons[36].IsActive = _selectedShip.HasEngineeringToken;
                            _buttons[37].IsActive = _selectedShip.HasSquadronToken;
                            
                            _buttons[38].IsActive = true;
                            
                            break;
                        case ShipState.ExecuteManuver: //ship manuver phase
                            break;
                    }


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
                    foreach(var ship in _player1.Ships)
                    {
                        ship.BeenActivated = false;
                        foreach(var dt in ship.DefenseTokens) if (dt.State == DefenseTokenStateEnum.Exhausted) dt.State = DefenseTokenStateEnum.Ready;
                        if (ship.Title != null && ship.Title.IsExhausted) ship.Title.IsExhausted = false;
                        if (ship.Commander != null && ship.Commander.IsExhausted) ship.Commander.IsExhausted = false;
                        foreach(var upgrade in ship.Upgrades) if (upgrade != null && upgrade.IsExhausted) upgrade.IsExhausted = false;
                    }
                    foreach (var ship in _player2.Ships)
                    {
                        ship.BeenActivated = false;
                        foreach (var dt in ship.DefenseTokens) if (dt.State == DefenseTokenStateEnum.Exhausted) dt.State = DefenseTokenStateEnum.Ready;
                        if (ship.Title != null && ship.Title.IsExhausted) ship.Title.IsExhausted = false;
                        if (ship.Commander != null && ship.Commander.IsExhausted) ship.Commander.IsExhausted = false;
                        foreach (var upgrade in ship.Upgrades) if (upgrade != null && upgrade.IsExhausted) upgrade.IsExhausted = false;
                    }
                    foreach (var squad in _player1.Squadrons) if (squad.HasBeenActivated) squad.HasBeenActivated = false;
                    foreach (var squad in _player2.Squadrons) if (squad.HasBeenActivated) squad.HasBeenActivated = false;

                    _roundNum++;
                    if (_player1Start) _player1Start = false;
                    else _player1Start = true;

                    _state = GameEnum.Command_Phase;
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


            switch(_state)
            {
                case GameEnum.Setup:
                    spriteBatch.DrawString(_galbasic, " <Game Setup>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                    switch (_setupState)
                    {
                        case SetupState.SelectFirst:
                            spriteBatch.DrawString(_galbasic, _selectingPlayer + ": What do you want to do?", new Vector2(_widthIncrement * 34, 40 * _heightIncrement), Color.Gold);
                            break;
                        case SetupState.Placement:

                            if (_player1Placing) spriteBatch.Draw(_player1Zone, new Rectangle(0, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 25, _game.GraphicsDevice.Viewport.Width - _widthIncrement * 20, _game.GraphicsDevice.Viewport.Height / 4), Color.White);
                            else spriteBatch.Draw(_player2Zone, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width - _widthIncrement * 20, _game.GraphicsDevice.Viewport.Height / 4), Color.White);

                            foreach (var ship in _shipsPlaced1)
                            {
                                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(ship.Image, ship.Position, null, Color.Red, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                                else spriteBatch.Draw(ship.Image, ship.Position, null, Color.White, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                            }
                            foreach (var ship in _shipsPlaced2)
                            {
                                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(ship.Image, ship.Position, null, Color.Blue, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                                else spriteBatch.Draw(ship.Image, ship.Position, null, Color.White, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                            }
                            foreach (var squad in _squadsPlaced1)
                            {
                                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(squad.Image, squad.Position, null, Color.Red, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                                else spriteBatch.Draw(squad.Image, squad.Position, null, Color.White, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                            }
                            foreach (var squad in _squadsPlaced2)
                            {
                                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(squad.Image, squad.Position, null, Color.Blue, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                                else spriteBatch.Draw(squad.Image, squad.Position, null, Color.White, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                            }

                            if (_player1Placing)
                            {
                                int hOffset = 43;
                                for (int i = 0; i < _squadTypeAmounts1.Length; i++)
                                {
                                    if (_squadTypeAmounts1[i] > 0)
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

                            break;
                        case SetupState.SetupDone:

                            break;
                    }
                    break;
                case GameEnum.Command_Phase:
                    drawFleets(spriteBatch);
                    if (_selectedShip != null) drawShipInfo(spriteBatch);

                    spriteBatch.DrawString(_galbasic, " <Command Phase>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);
                    spriteBatch.DrawString(_galbasic, " ROUND: " + _roundNum, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 17), Color.Gold);

                    if (_player1Placing) spriteBatch.DrawString(_galbasic, " Player 1 Setting", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 21), Color.Gold);
                    else spriteBatch.DrawString(_galbasic, " Player 2 Setting", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 21), Color.Gold);

                    spriteBatch.DrawString(_galbasic, _selectedDial.ToString(), new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _heightIncrement * 50), Color.Gold);
                    break;
                case GameEnum.Ship_Phase:
                    drawFleets(spriteBatch);

                    spriteBatch.DrawString(_galbasic, " ROUND: " + _roundNum, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 17), Color.Gold);

                    //displays whose turn it is
                    if (_player1Placing) spriteBatch.DrawString(_galbasic, " Player 1's Turn", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 21), Color.Gold);
                    else spriteBatch.DrawString(_galbasic, " Player 2 Turn", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 21), Color.Gold);


                    switch (_shipState)
                    {
                        case ShipState.ActivateShip:
                            spriteBatch.DrawString(_galbasic, " <Select Ship>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                            if (_selectedShip != null) drawShipInfo(spriteBatch);
                            break;
                        case ShipState.RevealDial:
                            spriteBatch.DrawString(_galbasic, " <Reveal Dial>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                            if (_selectedShip != null) drawShipInfo(spriteBatch);
                            if (_revealedCommand == CommandDialEnum.Navigation) spriteBatch.DrawString(_descriptor, "Navigation Command: Allows you to \nincrease or decrease this ship's speed \nby 1.", new Vector2(_game.GraphicsDevice.Viewport.Width - 19 * _widthIncrement, 73 * _heightIncrement), Color.Gold);
                            if (_revealedCommand == CommandDialEnum.Engineering) spriteBatch.DrawString(_descriptor, "Engineering Command: Allows you to recover \nsome shields and health.", new Vector2(_game.GraphicsDevice.Viewport.Width - 19 * _widthIncrement, 73 * _heightIncrement), Color.Gold);
                            if (_revealedCommand == CommandDialEnum.Squadron) spriteBatch.DrawString(_descriptor, "Squadron Command: Allows you to activate \nSome nearby squadrons early", new Vector2(_game.GraphicsDevice.Viewport.Width - 19 * _widthIncrement, 73 * _heightIncrement), Color.Gold);
                            if (_revealedCommand == CommandDialEnum.ConcentrateFire) spriteBatch.DrawString(_descriptor, "Concentrate Fire Command: Allows you to \nre-roll one of your attacks this turn.", new Vector2(_game.GraphicsDevice.Viewport.Width - 19 * _widthIncrement, 73 * _heightIncrement), Color.Gold);
                            break;
                        case ShipState.Navigation:
                            spriteBatch.DrawString(_galbasic, " <Navigation>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                            if (_selectedShip != null) drawShipInfo(spriteBatch);
                            spriteBatch.DrawString(_galbasic, "Speed: " + _speedDiff, new Vector2(_game.GraphicsDevice.Viewport.Width - 19 * _widthIncrement, 69 * _heightIncrement), Color.Gold);
                            break;
                        case ShipState.Engineering:
                            spriteBatch.DrawString(_galbasic, " <Engineering>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                            if (_selectedShip != null) drawReducedShipInfo(spriteBatch);
                            if (_shipState == ShipState.Engineering) spriteBatch.DrawString(_descriptor, "Remaining Engineering Points: " + _engineeringPoints, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 22 * _heightIncrement), Color.Gold);
                            break;
                        case ShipState.Squadron:

                            switch(_squadCommand)
                            {
                                case SquadronCommand.ActivateSquadron:
                                    spriteBatch.DrawString(_galbasic, " <Select Squadron>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    if (_selectedSquad != null) drawSelectedSquadronInfo(spriteBatch);
                                    spriteBatch.Draw(_shipToSquadRange, _selectedShip.Bounds.Center, null, Color.White, 0f, new Vector2(_shipToSquadRange.Width / 2, _shipToSquadRange.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    break;
                                case SquadronCommand.Choose:
                                    spriteBatch.DrawString(_galbasic, " <Chooss Options>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    if (_selectedSquad != null) drawSelectedSquadronInfo(spriteBatch);
                                    spriteBatch.Draw(_shipToSquadRange, _selectedShip.Bounds.Center, null, Color.White, 0f, new Vector2(_shipToSquadRange.Width / 2, _shipToSquadRange.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    break;
                                case SquadronCommand.Attack:
                                    spriteBatch.DrawString(_galbasic, " <Attacking>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    if (_selectedSquad != null) drawSelectedSquadronInfo(spriteBatch);
                                    if (_targetedSquadron != null) drawTargetedSquadronInfo(spriteBatch);
                                    if (_targetedShip != null) drawReducedTargetedShipInfo(spriteBatch);
                                    break;
                                case SquadronCommand.Move:
                                    spriteBatch.DrawString(_galbasic, " <Moving>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    if (_selectedSquad != null) drawSelectedSquadronInfo(spriteBatch);
                                    if (_selectedSquad.Speed == 1) spriteBatch.Draw(_squadMove1, _selectedSquad.Bounds.Center, null, Color.White, 0f, new Vector2(_squadMove1.Width / 2, _squadMove1.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    else if (_selectedSquad.Speed == 2) spriteBatch.Draw(_squadMove2, _selectedSquad.Bounds.Center, null, Color.White, 0f, new Vector2(_squadMove2.Width / 2, _squadMove2.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    else if (_selectedSquad.Speed == 3) spriteBatch.Draw(_squadMove3, _selectedSquad.Bounds.Center, null, Color.White, 0f, new Vector2(_squadMove3.Width / 2, _squadMove3.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    else if (_selectedSquad.Speed == 4) spriteBatch.Draw(_squadMove4, _selectedSquad.Bounds.Center, null, Color.White, 0f, new Vector2(_squadMove4.Width / 2, _squadMove4.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    else if (_selectedSquad.Speed == 5) spriteBatch.Draw(_squadMove5, _selectedSquad.Bounds.Center, null, Color.White, 0f, new Vector2(_squadMove5.Width / 2, _squadMove5.Height / 2), 1f, SpriteEffects.None, 0.5f);
                                    break;
                            }
                            break;
                        case ShipState.UseToken:
                            spriteBatch.DrawString(_galbasic, " <Use Tokens>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);
                            break;
                        case ShipState.Attack:
                            if (_selectedShip != null) drawShipInfo(spriteBatch);

                            switch(_attackState)
                            {
                                case AttackState.SelectArc:
                                    spriteBatch.DrawString(_galbasic, " <Select Arc>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.DeclareTarget:
                                    spriteBatch.DrawString(_galbasic, " <Declare Target>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.RollDice:
                                    spriteBatch.DrawString(_galbasic, " <Modifying Dice>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.ModifyDice:
                                    spriteBatch.DrawString(_galbasic, " <Modifying Dice>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.SpendAccuracies:
                                    spriteBatch.DrawString(_galbasic, " <Spend Accuracies>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.SpendDefenseTokens:
                                    spriteBatch.DrawString(_galbasic, " <Ship Defense>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.ResolveDamage:
                                    spriteBatch.DrawString(_galbasic, " <Resolve Damage>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                                case AttackState.Done:
                                    spriteBatch.DrawString(_galbasic, " <Resolve Damage>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);

                                    break;
                            }

                            if(_selectedArc >= 0)
                            {
                                if (_selectedShip.Arcs[_selectedArc].RedDice.Count > 0)
                                {
                                    spriteBatch.Draw(_shipRanges, _selectedShip.Bounds.Center, null, Color.White, 0f, new Vector2(_shipRanges.Width / 2, _shipRanges.Height / 2), 1f, SpriteEffects.None, 0);
                                    _range = _shipRanges;
                                }
                                else if (_selectedShip.Arcs[_selectedArc].BlueDice.Count > 0)
                                {
                                    spriteBatch.Draw(_shipIonRange, _selectedShip.Bounds.Center, null, Color.White, 0f, new Vector2(_shipIonRange.Width / 2, _shipIonRange.Height / 2), 1f, SpriteEffects.None, 0);
                                    _range = _shipIonRange;
                                }
                                else if (_selectedShip.Arcs[_selectedArc].BlackDice.Count > 0)
                                {
                                    spriteBatch.Draw(_shipToSquadRange, _selectedShip.Bounds.Center, null, Color.White, 0f, new Vector2(_shipToSquadRange.Width / 2, _shipToSquadRange.Height / 2), 1f, SpriteEffects.None, 0);
                                    _range = _shipToSquadRange;
                                }
                                else
                                {
                                    _range = null;
                                }
                            }


                            break;
                        case ShipState.ExecuteManuver:
                            spriteBatch.DrawString(_galbasic, " <Movement Phase>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);
                            if (_selectedShip != null) drawReducedShipInfo(spriteBatch);


                            break;
                    }
                    break;
                case GameEnum.Squadron_Phase:
                    drawFleets(spriteBatch);
                    spriteBatch.DrawString(_galbasic, " <Squad Phase>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);
                    spriteBatch.DrawString(_galbasic, " ROUND: " + _roundNum, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 17), Color.Gold);

                    switch (_squadState)
                    {
                        case SquadronState.ActivateSquad:
                            break;
                        case SquadronState.Choose:
                            break;
                        case SquadronState.Attack:
                            break;
                        case SquadronState.Move:
                            break;
                    }
                    break;
                case GameEnum.Status_Phase:
                    drawFleets(spriteBatch);
                    spriteBatch.DrawString(_galbasic, "  <Squad Phase>", new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 13), Color.Gold);
                    spriteBatch.DrawString(_galbasic, " ROUND: " + _roundNum, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 17), Color.Gold);
                    break;
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
                case 16://selecting the nav command
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedDial = CommandDialEnum.Navigation;
                    }
                    break;
                case 17: //selecting the engineeirng command
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedDial = CommandDialEnum.Engineering;
                    }
                    break;
                case 18: //select the squadron command
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedDial = CommandDialEnum.Squadron;
                    }
                    break;
                case 19: //select the Concentrate fire button
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedDial = CommandDialEnum.ConcentrateFire;
                    }
                    break;
                case 20: //commit to ship
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        _selectedShip.CommandDials.Enqueue(_selectedDial);
                        if(_selectedShip.Command == _selectedShip.CommandDials.Count)
                        {
                            if (_player1Placing)
                            {
                                if(numLeftToSet(_player2.Ships) > 0) _player1Placing = false;
                                _player1.Ships[_player1.Ships.IndexOf(_selectedShip)] = _selectedShip;
                            }
                            else
                            {
                                if(numLeftToSet(_player1.Ships) > 0) _player1Placing = true;
                                _player2.Ships[_player2.Ships.IndexOf(_selectedShip)] = _selectedShip;
                            }
                        }
                        Thread.Sleep(200);
                    }
                    break;
                case 21: //activate ship
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        _selectedShip.BeenActivated = true;
                        _shipState = ShipState.RevealDial;
                        _revealedCommand = _selectedShip.CommandDials.Dequeue();
                        _buttons[21].IsActive = false;
                        Thread.Sleep(200);
                    }
                    break;
                case 22:
                    //Does nothing, its a label that I didn't feel like making an image 
                    break;
                case 23: //use now as Command dial
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        if(_revealedCommand == CommandDialEnum.Navigation)
                        {
                            _shipState = ShipState.Navigation;
                            _buttons[22].IsActive = false;
                            _buttons[23].IsActive = false;
                            _buttons[24].IsActive = false;
                            _speedDiff = _selectedShip.Speed;
                            Thread.Sleep(200);
                        }
                        if (_revealedCommand == CommandDialEnum.Squadron)
                        {
                            if(_player1Placing && _player1.Squadrons.Count > 0)
                            {
                                _shipState = ShipState.Squadron;
                                _numSquadsToActivate = _selectedShip.Squadron;
                                _buttons[22].IsActive = false;
                                _buttons[23].IsActive = false;
                                _buttons[24].IsActive = false;
                                Thread.Sleep(200);
                            }
                            else if(!_player1Placing && _player2.Squadrons.Count > 0)
                            {
                                _shipState = ShipState.Squadron;
                                _numSquadsToActivate = _selectedShip.Squadron;
                                _buttons[22].IsActive = false;
                                _buttons[23].IsActive = false;
                                _buttons[24].IsActive = false;
                                Thread.Sleep(200);
                            }
                            else
                            {
                                _shipState = ShipState.Attack;
                                _buttons[22].IsActive = false;
                                _buttons[23].IsActive = false;
                                _buttons[24].IsActive = false;
                                Thread.Sleep(200);
                            }
                            
                        }
                        if (_revealedCommand == CommandDialEnum.Engineering)
                        {
                            _shipState = ShipState.Engineering;
                            _engineeringPoints = _selectedShip.Engineering;
                            _buttons[22].IsActive = false;
                            _buttons[23].IsActive = false;
                            _buttons[24].IsActive = false;
                            Thread.Sleep(200);
                        }
                    }
                    break;
                case 24: //Save for later as a Token
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();

                        if(_revealedCommand == CommandDialEnum.Navigation && !_selectedShip.HasNavigationToken && _selectedShip.TokenCount < _selectedShip.Command)
                        {
                            _buttons[22].IsActive = false;
                            _buttons[23].IsActive = false;
                            _buttons[24].IsActive = false;
                            _selectedShip.HasNavigationToken = true;
                        }
                        else if (_revealedCommand == CommandDialEnum.Engineering && !_selectedShip.HasEngineeringToken && _selectedShip.TokenCount < _selectedShip.Command)
                        {
                            _buttons[22].IsActive = false;
                            _buttons[23].IsActive = false;
                            _buttons[24].IsActive = false;
                            _selectedShip.HasEngineeringToken = true;
                        }
                        else if (_revealedCommand == CommandDialEnum.Squadron && !_selectedShip.HasSquadronToken && _selectedShip.TokenCount < _selectedShip.Command)
                        {
                            _buttons[22].IsActive = false;
                            _buttons[23].IsActive = false;
                            _buttons[24].IsActive = false;
                            _selectedShip.HasSquadronToken = true;
                        }
                        else if (_revealedCommand == CommandDialEnum.ConcentrateFire && !_selectedShip.HasConcentrateFireToken && _selectedShip.TokenCount < _selectedShip.Command)
                        {
                            _buttons[22].IsActive = false;
                            _buttons[23].IsActive = false;
                            _buttons[24].IsActive = false;
                            _selectedShip.HasConcentrateFireToken = true;
                        }

                        _shipState = ShipState.UseToken;
                        _buttons[38].IsActive = true;
                        Thread.Sleep(200);
                    }
                    break;
                case 25: //increase speed
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        _speedDiff++;
                    }
                    break;
                case 26: //decrease speed
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _speedDiff--;
                    }
                    break;
                case 27: //set speed
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();
                        _selectedShip.Speed = _speedDiff;
                        _shipState = ShipState.Attack;
                    }
                    break;
                case 28:    //restore shields ---no longer used
                    if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        _button1.Play();
                        buttonSweeper(20);
                        _buttons[29].IsActive = true;
                        _buttons[30].IsActive = true;
                        _buttons[31].IsActive = true;
                        _buttons[32].IsActive = true;
                        _buttons[33].IsActive = true;
                        Thread.Sleep(200);
                    }
                    break;
                case 29:    //Bow shields
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        if (_selectedShip.Arcs[0].Shields < _selectedShip.Arcs[0].MaxShields && _engineeringPoints >= 2)
                        {
                            _selectedShip.Arcs[0].Shields++;
                            _engineeringPoints -= 2;
                        }
                    }
                    break;
                case 30:    //Port shields
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        if (_selectedShip.Arcs[1].Shields < _selectedShip.Arcs[1].MaxShields && _engineeringPoints >= 2)
                        {
                            _selectedShip.Arcs[1].Shields++;
                            _engineeringPoints -= 2;
                        }
                    }
                    break;
                case 31:    //starboard shields
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        if (_selectedShip.Arcs[2].Shields < _selectedShip.Arcs[2].MaxShields && _engineeringPoints >= 2)
                        {
                            _selectedShip.Arcs[2].Shields++;
                            _engineeringPoints -= 2;
                        }
                    }
                    break;
                case 32:    //aft shields
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        if (_selectedShip.Arcs[3].Shields < _selectedShip.Arcs[3].MaxShields && _engineeringPoints >= 2)
                        {
                            _selectedShip.Arcs[3].Shields++;
                            _engineeringPoints -= 2;
                        }
                    }
                    break;
                case 33:    //Back button ---no longer used
                    if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();
                        buttonSweeper(20);
                        _buttons[28].IsActive = true;
                        _buttons[34].IsActive = true;
                        _buttons[35].IsActive = true;
                    }
                    break;
                case 34:    //Repair hull
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        if (_selectedShip.Hull < _selectedShip.MaxHull && _engineeringPoints >= 3)
                        {
                            _selectedShip.Hull++;
                            _engineeringPoints -= 3;
                        }
                    }
                    break;
                case 35:    //Done with engineering
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();
                        _engineeringPoints = 0;
                        _shipState = ShipState.Attack;
                        buttonSweeper(20);
                    }
                    break;
                case 36: //use engineering token
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        _engineeringPoints = _selectedShip.Engineering;
                        _shipState = ShipState.Engineering;
                        buttonSweeper(20);
                    }
                    break;
                case 37: //use squadron token
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        
                        _shipState = ShipState.Squadron;
                        buttonSweeper(20);
                    }
                    break;
                case 38: //Decide to not use one
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();
                        _shipState = ShipState.Attack;
                        buttonSweeper(20);
                    }
                    break;
                case 39: //activates squadron
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        _selectedSquad.HasBeenActivated = true;

                        _squadCommand = SquadronCommand.Choose;

                        _squadHasAttacked = false;
                        _squadHasMoved = false;
                    }
                    break;
                case 40: //Moves said squadron
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _squadCommand = SquadronCommand.Move;

                        _squadHasMoved = true;
                        buttonSweeper(40);
                    }
                    break;
                case 41: //Attack with said squadron if a target is available
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button3.Play();
                        _squadCommand = SquadronCommand.Attack;

                        _squadHasAttacked = true;
                        buttonSweeper(40);
                    }
                    break;
                case 42: //Done with this squadron
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button4.Play();
                        _numSquadsToActivate--;
                        if(_numSquadsToActivate <= 0 || numSquadsNearby() <= 0)
                        {
                            _shipState = ShipState.Attack;
                            _squadCommand = SquadronCommand.ActivateSquadron;
                        }
                        else
                        {
                            _squadCommand = SquadronCommand.ActivateSquadron;
                        }

                        _squadHasMoved = false;
                        _squadHasAttacked = false;
                        _selectedSquad = null;
                        _targetedSquadron = null;
                        buttonSweeper(40);
                    }
                    break;
                case 43: //attack target button for squadron
                    if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        Thread.Sleep(200);

                        if (_targetedSquadron != null) //target is a squadron
                        {
                            _targetedSquadron.Hull -= _sq2SqDamage;
                            _targetedSquadron = null;
                            _targetedShip = null;
                            _targetArc = 0;

                            _shoot1.Play();

                            if(_targetedSquadron.HasCounter2) //if the squad can counter(Note: happens even if is destroyed first)
                            {
                                _counterDamage = resolveCounter2();
                                _selectedSquad.Hull -= _counterDamage;
                                
                                _shoot2.Play();
                            }

                            buttonSweeper(40);
                            _squadCommand = SquadronCommand.Choose;
                        }
                        if (_targetedShip != null)
                        {
                            if(_targetedShip.Arcs[_targetArc].Shields < _sq2ShDamage)
                            {
                                _sq2ShDamage -= _targetedShip.Arcs[_targetArc].Shields;
                                _targetedShip.Arcs[_targetArc].Shields = 0;

                                _shoot1.Play();

                                if(_attackHasCrits)
                                {
                                    _targetedShip.Hull -= _sq2ShDamage;
                                    resolveSquadCritical();
                                }
                            }
                            buttonSweeper(40);
                            _squadCommand = SquadronCommand.Choose;
                        }
                    }
                    break;
                case 44:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedArc = 0;
                    }
                    break;
                case 45:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedArc = 1;
                    }
                    break;
                case 46:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedArc = 2;
                    }
                    break;
                case 47:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button2.Play();
                        _selectedArc = 3;
                    }
                    break;
                case 48:
                    if (_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _button1.Play();
                        if(_selectedArc != -1) _attackState = AttackState.DeclareTarget;
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

        /// <summary>
        /// A helper method to draw the fleets on the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch used to draw things</param>
        private void drawFleets(SpriteBatch spriteBatch)
        {
            foreach (var ship in _player1.Ships)
            {
                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(ship.Image, ship.Position, null, Color.Red, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                else spriteBatch.Draw(ship.Image, ship.Position, null, Color.White, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);

                Vector2 test = CollisionHelper.GetNewCoords(ship.Bounds.Center, 74, ship.Rotation + MathHelper.Pi);

                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.Bounds.Center, Color.DarkGoldenrod);

                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.BowBounds.Center, Color.DarkGoldenrod);
                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.PortBounds.Center, Color.DarkGoldenrod);
                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.StarboardBounds.Center, Color.DarkGoldenrod);
                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.AftBounds.Center, Color.DarkGoldenrod);
            }
            foreach (var ship in _player2.Ships)
            {
                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(ship.Image, ship.Position, null, Color.Blue, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);
                else spriteBatch.Draw(ship.Image, ship.Position, null, Color.White, ship.Rotation, ship.Origin, 1, SpriteEffects.None, 1);

                Vector2 test = CollisionHelper.GetNewCoords(ship.Bounds.Center, 74, ship.Rotation + MathHelper.Pi);

                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.Bounds.Center, Color.DarkGoldenrod);

                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.BowBounds.Center, Color.DarkGoldenrod);
                spriteBatch.DrawString(_galbasic, "" + ship.Id, ship.PortBounds.Center, Color.DarkGoldenrod);
            }
            foreach (var squad in _player1.Squadrons)
            {
                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(squad.Image, squad.Position, null, Color.Red, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                else spriteBatch.Draw(squad.Image, squad.Position, null, Color.White, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);

                spriteBatch.DrawString(_galbasic, "" + squad.Id, squad.Bounds.Center, Color.DarkGoldenrod);
            }
            foreach (var squad in _player2.Squadrons)
            {
                if (_player1.IsRebelFleet == _player2.IsRebelFleet) spriteBatch.Draw(squad.Image, squad.Position, null, Color.Blue, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);
                else spriteBatch.Draw(squad.Image, squad.Position, null, Color.White, squad.Rotation, squad.Origin, 1, SpriteEffects.None, 1);

                spriteBatch.DrawString(_galbasic, "" + squad.Id, squad.Bounds.Center, Color.DarkGoldenrod);
            }
        }

        /// <summary>
        /// Returns the number of ships left to assign command dials to
        /// </summary>
        /// <param name="ships">The list of ships to parse through</param>
        /// <returns>The number of ships left to assign command dials to</returns>
        private int numLeftToSet(List<Ship> ships)
        {
            int result = 0;
            foreach (var ship in ships) if (ship.CommandDials.Count != ship.Command) result++;
            return result;
        }

        /// <summary>
        /// Returns the next ship in the fleet that needs to have it's command dials set
        /// </summary>
        /// <param name="ships">List of ships to look through</param>
        /// <returns>The ship to set</returns>
        private Ship nextToPlace(List<Ship> ships)
        {
            Ship result = null;
            foreach (var ship in ships)
            {
                if (ship.CommandDials.Count != ship.Command)
                {
                    result = ship;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Draws the selected ship's information for the user to see
        /// </summary>
        /// <param name="spriteBatch">The Spritebatch used to draw the ship's information</param>
        private void drawShipInfo(SpriteBatch spriteBatch)
        {
            Texture2D image;
            string name = "";
            if(_selectedShip is AssaultFrigateMarkII)
            {
                if (_selectedShip.ShipTypeA) image = _content.Load<Texture2D>("AssaultA");
                else image = _content.Load<Texture2D>("AssaultB");
            }
            else if (_selectedShip is CR90Corvette)
            {
                if (_selectedShip.ShipTypeA) image = _content.Load<Texture2D>("CR90A");
                else image = _content.Load<Texture2D>("CR90B");
            }
            else if (_selectedShip is NebulonBFrigate)
            {
                if (_selectedShip.ShipTypeA) image = _content.Load<Texture2D>("NebulonBEscort");
                else image = _content.Load<Texture2D>("NebulonBSupport");
            }
            else if (_selectedShip is GladiatorStarDestroyer)
            {
                if (_selectedShip.ShipTypeA) image = _content.Load<Texture2D>("GladiatorISD");
                else image = _content.Load<Texture2D>("GladiatorIISD");
            }
            else //if (_selectedShip is VictoryStarDestroyer)
            {
                if (_selectedShip.ShipTypeA) image = _content.Load<Texture2D>("VictoryISD");
                else image = _content.Load<Texture2D>("VictoryIISD");
            }

            if (_selectedShip.HasCommander) name = _selectedShip.Commander.Name;

            spriteBatch.Draw(image, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _heightIncrement, _widthIncrement * 18, _heightIncrement * 30), Color.White);

            spriteBatch.DrawString(_descriptor, "ID: " + _selectedShip.Id + "     Current HP: " + _selectedShip.Hull + "/" + _selectedShip.MaxHull + "    Commands Set: " + _selectedShip.CommandDials.Count, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 32 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Shields:    " + _selectedShip.Arcs[0].Shields + "/" + _selectedShip.Arcs[0].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 34 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "             " + _selectedShip.Arcs[1].Shields + "/" + _selectedShip.Arcs[1].MaxShields + "     " + _selectedShip.Arcs[2].Shields + "/" + _selectedShip.Arcs[2].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 37 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "                  " + _selectedShip.Arcs[3].Shields + "/" + _selectedShip.Arcs[3].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 40 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Nav Token: " + _selectedShip.HasNavigationToken + "   Commander: " + name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 42 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Eng Token: " + _selectedShip.HasEngineeringToken + "   Speed: " + _selectedShip.Speed, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 44 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Squad Token: " + _selectedShip.HasSquadronToken, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 46 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Con. Fire Token: " + _selectedShip.HasConcentrateFireToken, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 48 * _heightIncrement), Color.Gold);
        
            if(_state == GameEnum.Ship_Phase && _shipState != ShipState.Attack)
            {
                int h = 50;
                if(_selectedShip.Title != null)
                {
                    spriteBatch.DrawString(_descriptor, "  Title: " + _selectedShip.Title.Name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, h * _heightIncrement), Color.Gold);
                    h += 2;
                }
                foreach(var upgrade in _selectedShip.Upgrades)
                {
                    if(upgrade != null)
                    {
                        spriteBatch.DrawString(_descriptor, "    " + upgrade.Name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, h * _heightIncrement), Color.Gold);
                        h += 2;
                    }

                }
            }
        }

        /// <summary>
        /// Draws a reduced ship info to save space
        /// </summary>
        /// <param name="spritBatch">The spritBatch used to draw things</param>
        private void drawReducedShipInfo(SpriteBatch spriteBatch)
        {
            string name = "";

            if (_selectedShip.HasCommander) name = _selectedShip.Commander.Name;

            spriteBatch.DrawString(_descriptor, _selectedShip.Name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "ID: " + _selectedShip.Id + "     Current HP: " + _selectedShip.Hull + "/" + _selectedShip.MaxHull + "    Commands Set: " + _selectedShip.CommandDials.Count, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 3 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Shields:    " + _selectedShip.Arcs[0].Shields + "/" + _selectedShip.Arcs[0].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 5 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "             " + _selectedShip.Arcs[1].Shields + "/" + _selectedShip.Arcs[1].MaxShields + "     " + _selectedShip.Arcs[2].Shields + "/" + _selectedShip.Arcs[2].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 8 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "                  " + _selectedShip.Arcs[3].Shields + "/" + _selectedShip.Arcs[3].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 11 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Nav Token: " + _selectedShip.HasNavigationToken + "   Commander: " + name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 13 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Eng Token: " + _selectedShip.HasEngineeringToken + "   Speed: " + _selectedShip.Speed, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 15 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Squad Token: " + _selectedShip.HasSquadronToken, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 17 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Con. Fire Token: " + _selectedShip.HasConcentrateFireToken, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 19 * _heightIncrement), Color.Gold);
        }

        /// <summary>
        /// Draws a reduced ship info to save space
        /// </summary>
        /// <param name="spritBatch">The spritBatch used to draw things</param>
        private void drawReducedTargetedShipInfo(SpriteBatch spriteBatch)
        {
            string name = "";

            if (_selectedShip.HasCommander) name = _selectedShip.Commander.Name;

            spriteBatch.DrawString(_descriptor, _selectedShip.Name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 22 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "ID: " + _selectedShip.Id + "     Current HP: " + _selectedShip.Hull + "/" + _selectedShip.MaxHull + "    Commands Set: " + _selectedShip.CommandDials.Count, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 24 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Shields:    " + _selectedShip.Arcs[0].Shields + "/" + _selectedShip.Arcs[0].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 26 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "             " + _selectedShip.Arcs[1].Shields + "/" + _selectedShip.Arcs[1].MaxShields + "     " + _selectedShip.Arcs[2].Shields + "/" + _selectedShip.Arcs[2].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 29 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "                  " + _selectedShip.Arcs[3].Shields + "/" + _selectedShip.Arcs[3].MaxShields, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 32 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Nav Token: " + _selectedShip.HasNavigationToken + "   Commander: " + name, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 34 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Eng Token: " + _selectedShip.HasEngineeringToken + "   Speed: " + _selectedShip.Speed, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 36 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Squad Token: " + _selectedShip.HasSquadronToken, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 38 * _heightIncrement), Color.Gold);
            spriteBatch.DrawString(_descriptor, "Con. Fire Token: " + _selectedShip.HasConcentrateFireToken, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 19, 40 * _heightIncrement), Color.Gold);
        }

        /// <summary>
        /// Gets the number of ships left to activate
        /// </summary>
        /// <param name="ships">The list of ships to look through</param>
        /// <returns>The number of ships left to activate</returns>
        private int shipActivationsRemaining(List<Ship> ships)
        {
            int result = 0;
            foreach (var ship in ships) if (!ship.BeenActivated) result++;
            return result;
        }

        /// <summary>
        /// Checks to see if there is a squad to target near the selected squad.
        /// </summary>
        /// <param name="player1">A bool to distinguish if player 1 or player 2 is going</param>
        /// <returns>A bool, true if there is a target, false otherwise.</returns>
        private bool isSquadTargetNearby(bool player1)
        {
            if(player1)
            {
                foreach(var sqd in _player2.Squadrons)
                {
                    if(Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - sqd.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - sqd.Bounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (var sqd in _player1.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - sqd.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - sqd.Bounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// A method used in determining if a squadron can move or not
        /// </summary>
        /// <param name="player1"></param>
        /// <returns></returns>
        private bool isEngaged(bool player1)
        {
            if (player1)
            {
                foreach (var sqd in _player2.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - sqd.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - sqd.Bounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                    {
                        if(!sqd.HasHeavy) return true;
                    }
                }
            }
            else
            {
                foreach (var sqd in _player1.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - sqd.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - sqd.Bounds.Center.Y, 2)) <= _squadMove1.Width / 2)
                    {
                        if (!sqd.HasHeavy) return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Draws the selected squadron
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used to draw things</param>
        private void drawSelectedSquadronInfo(SpriteBatch spriteBatch)
        {
            Texture2D image;
            if (_selectedSquad is AWingSquadron) image = _content.Load<Texture2D>("AWingCard");
            else if (_selectedSquad is BWingSquadron) image = _content.Load<Texture2D>("BWingCard");
            else if (_selectedSquad is XWingSquadron) image = _content.Load<Texture2D>("XWingCard");
            else if (_selectedSquad is YWingSquadron) image = _content.Load<Texture2D>("YWingCard");
            else if (_selectedSquad is TIEFighterSquadron) image = _content.Load<Texture2D>("TIEFighterCard");
            else if (_selectedSquad is TIEAdvancedSquadron) image = _content.Load<Texture2D>("TIEAdvancedCard");
            else if (_selectedSquad is TIEInterceptorSquadron) image = _content.Load<Texture2D>("TIEInterceptorCard");
            else image = _content.Load<Texture2D>("TIEBomberCard");

            spriteBatch.Draw(image, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, _heightIncrement, _widthIncrement * 12, _heightIncrement * 18), Color.White);
            spriteBatch.DrawString(_descriptor, "Hull Points: " + _selectedSquad.Hull + "  ID: " + _selectedSquad.Id, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, _heightIncrement * 20), Color.Gold);
        }

        /// <summary>
        /// Draws the Targeted squadron
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used to draw things</param>
        private void drawTargetedSquadronInfo(SpriteBatch spriteBatch)
        {
            Texture2D image;
            if (_targetedSquadron is AWingSquadron) image = _content.Load<Texture2D>("AWingCard");
            else if (_targetedSquadron is BWingSquadron) image = _content.Load<Texture2D>("BWingCard");
            else if (_targetedSquadron is XWingSquadron) image = _content.Load<Texture2D>("XWingCard");
            else if (_targetedSquadron is YWingSquadron) image = _content.Load<Texture2D>("YWingCard");
            else if (_targetedSquadron is TIEFighterSquadron) image = _content.Load<Texture2D>("TIEFighterCard");
            else if (_targetedSquadron is TIEAdvancedSquadron) image = _content.Load<Texture2D>("TIEAdvancedCard");
            else if (_targetedSquadron is TIEInterceptorSquadron) image = _content.Load<Texture2D>("TIEInterceptorCard");
            else image = _content.Load<Texture2D>("TIEBomberCard");

            spriteBatch.Draw(image, new Rectangle(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 16, 22 * _heightIncrement, _widthIncrement * 12, _heightIncrement * 21), Color.White);
            spriteBatch.DrawString(_descriptor, "Target's Hull Points: " + _targetedSquadron.Hull + "  ID: " + _targetedSquadron.Id, new Vector2(_game.GraphicsDevice.Viewport.Width - _widthIncrement * 15, _heightIncrement * 44), Color.Gold);
        }

        /// <summary>
        /// Used for when a squadron is moving
        /// </summary>
        /// <returns></returns>
        private bool isSquadSpotTaken()
        {
            foreach(var ship in _player1.Ships)
            {
                if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < 155 && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < 210)
                {
                    return true;
                }
            }

            foreach(var squad in _player1.Squadrons)
            {
                if (Math.Abs(_currentMouseState.X - squad.Bounds.Center.X) < 51 && Math.Abs(_currentMouseState.Y - squad.Bounds.Center.Y) < 51)
                {
                    return true;
                }
            }

            foreach (var ship in _player2.Ships)
            {
                if (Math.Abs(_currentMouseState.X - ship.Bounds.Center.X) < 155 && Math.Abs(_currentMouseState.Y - ship.Bounds.Center.Y) < 210)
                {
                    return true;
                }
            }

            foreach (var squad in _player2.Squadrons)
            {
                if (Math.Abs(_currentMouseState.X - squad.Bounds.Center.X) < 51 && Math.Abs(_currentMouseState.Y - squad.Bounds.Center.Y) < 51)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// A method for finding and selecting the escort squadron if one exists
        /// </summary>
        /// <returns>The first escort squadron encountered</returns>
        private Squadron escortsNearby()
        {
            if(_player1Placing)
            {
                foreach(var squad in _player2.Squadrons)
                {
                    if(Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - squad.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - squad.Bounds.Center.Y, 2)) < _squadMove1.Width / 2)
                    {
                        if(squad.HasEscort)
                        {
                            return squad;
                        }
                    }
                }
            }
            else //player 2 is going
            {
                foreach (var squad in _player1.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - squad.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - squad.Bounds.Center.Y, 2)) < _squadMove1.Width / 2)
                    {
                        if (squad.HasEscort)
                        {
                            return squad;
                        }
                    }
                }
            }

            return null;
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool targetSquadron()
        {

        }
        */

        /// <summary>
        /// calculates the damage done to capital ships by squadrons
        /// </summary>
        /// <returns>The total damage done to them</returns>
        private int shipDamage()
        {
            int hits = 0;

            List<BlackDieSideEnum> blackRolls = new List<BlackDieSideEnum>();
            List<BlueDieSideEnum> blueRolls = new List<BlueDieSideEnum>();
            List<RedDieSideEnum> redRolls = new List<RedDieSideEnum>();

            foreach(var die in _selectedSquad.AntiShipDice)
            {
                if (die is BlueDie)
                {
                    BlueDie b = new BlueDie(DieTypeEnum.Blue);
                    blueRolls.Add(b.Roll());
                }
                if (die is BlackDie)
                {
                    BlackDie b = new BlackDie(DieTypeEnum.Black);
                    blackRolls.Add(b.Roll());
                }
                if (die is RedDie)
                {
                    RedDie b = new RedDie(DieTypeEnum.Red);
                    redRolls.Add(b.Roll());
                }
            }

            foreach (var roll in blackRolls)
            {
                if(_selectedSquad.HasBomber)
                {
                    if (roll == BlackDieSideEnum.Hit) hits++;
                    if (roll == BlackDieSideEnum.HitCrit) 
                    {
                        hits += 2;
                        _attackHasCrits = true;
                    }
                }
                else
                {
                    if (roll == BlackDieSideEnum.Hit || roll == BlackDieSideEnum.HitCrit) hits++;
                }
            }

            foreach (var roll in blueRolls)
            {
                if (_selectedSquad.HasBomber)
                {
                    if (roll == BlueDieSideEnum.Hit) hits++;
                    if(roll == BlueDieSideEnum.Crit)
                    {
                        hits++;
                        _attackHasCrits = true;
                    }
                }
                else
                {
                    if (roll == BlueDieSideEnum.Hit) hits++;
                }
            }

            foreach (var roll in redRolls)
            {
                if (_selectedSquad.HasBomber)
                {
                    if (roll == RedDieSideEnum.Hit) hits++;
                    if (roll == RedDieSideEnum.DoubleHit) hits += 2;
                    if(roll == RedDieSideEnum.Crit)
                    {
                        hits++;
                        _attackHasCrits = true;
                    }
                }
                else
                {
                    if (roll == RedDieSideEnum.Hit) hits++;
                    if (roll == RedDieSideEnum.DoubleHit) hits += 2;
                }
            }

            return hits;
        }


        /// <summary>
        /// Method for calculating and resolving squadron to squadron attacks
        /// </summary>
        /// <returns>The number of successful hits</returns>
        private int squadDamage()
        {
            int hits = 0;
            int rerolls = 0;

            if (checkForSwarm()) rerolls++;

            List<BlackDieSideEnum> blackRolls = new List<BlackDieSideEnum>();
            List<BlueDieSideEnum> blueRolls = new List<BlueDieSideEnum>();
            List<RedDieSideEnum> redRolls = new List<RedDieSideEnum>();

            foreach (var die in _selectedSquad.AntiSquadronDice)
            {
                if(die is BlueDie)
                {
                    BlueDie b = new BlueDie(DieTypeEnum.Blue);
                    blueRolls.Add(b.Roll());
                }
                if (die is BlackDie)
                {
                    BlackDie b = new BlackDie(DieTypeEnum.Black);
                    blackRolls.Add(b.Roll());
                }
                if (die is RedDie)
                {
                    RedDie b = new RedDie(DieTypeEnum.Red);
                    redRolls.Add(b.Roll());
                }
            }

            foreach(var roll in blackRolls)
            {
                if (roll == BlackDieSideEnum.Hit || roll == BlackDieSideEnum.HitCrit) hits++;
                if(rerolls > 0 && roll == BlackDieSideEnum.Blank)
                {
                    BlackDieSideEnum bs = new BlackDie(DieTypeEnum.Black).Roll();
                    if (bs == BlackDieSideEnum.Hit || bs == BlackDieSideEnum.HitCrit) hits++;

                    rerolls--;
                }
            }

            foreach(var roll in blueRolls)
            {
                if (roll == BlueDieSideEnum.Hit) hits++;
                if (rerolls > 0 && roll == BlueDieSideEnum.Accuracy)
                {
                    BlueDieSideEnum bs = new BlueDie(DieTypeEnum.Blue).Roll();
                    if (bs == BlueDieSideEnum.Hit) hits++;

                    rerolls--;
                }
            }

            foreach (var roll in redRolls)
            {
                if (roll == RedDieSideEnum.Hit) hits++;
                if (roll == RedDieSideEnum.DoubleHit) hits += 2;
                if (rerolls > 0 && (roll == RedDieSideEnum.Blank || roll == RedDieSideEnum.Accuracy || roll == RedDieSideEnum.Crit))
                {
                    RedDieSideEnum bs = new RedDie(DieTypeEnum.Red).Roll();
                    if (bs == RedDieSideEnum.Hit) hits++;
                    if (bs == RedDieSideEnum.DoubleHit) hits += 2;

                    rerolls--;
                }
            }

            return hits;
        }

        /// <summary>
        /// Checks for any other swarm squadrons out there
        /// </summary>
        /// <returns>True if there is another swarm nearby, false otherwise</returns>
        private bool checkForSwarm()
        {
            if(_player1Placing)
            {
                foreach(var sqd in _player1.Squadrons)
                {
                    if(Math.Sqrt(Math.Pow(_targetedSquadron.Bounds.Center.X - sqd.Bounds.Center.X, 2) + Math.Pow(_targetedSquadron.Bounds.Center.Y - sqd.Bounds.Center.Y, 2)) <= (_squadMove1.Width + sqd.Image.Width) / 2)
                    {
                        if (sqd.HasSwarm) return true;
                    }
                }
            }
            else //player 2 is going
            {
                foreach (var sqd in _player2.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_targetedSquadron.Bounds.Center.X - sqd.Bounds.Center.X, 2) + Math.Pow(_targetedSquadron.Bounds.Center.Y - sqd.Bounds.Center.Y, 2)) <= (_squadMove1.Width + sqd.Image.Width) / 2)
                    {
                        if (sqd.HasSwarm) return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// A method for finding the escort squadron if one exists
        /// </summary>
        /// <returns>True if there are squadrons with escort nearby</returns>
        private bool isEscortsNearby()
        {
            if (_player1Placing)
            {
                foreach (var squad in _player2.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - squad.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - squad.Bounds.Center.Y, 2)) < (_squadMove1.Width + squad.Image.Width) / 2)
                    {
                        if (squad.HasEscort)
                        {
                            return true;
                        }
                    }
                }
            }
            else //player 2 is going
            {
                foreach (var squad in _player1.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedSquad.Bounds.Center.X - squad.Bounds.Center.X, 2) + Math.Pow(_selectedSquad.Bounds.Center.Y - squad.Bounds.Center.Y, 2)) < (_squadMove1.Width + squad.Image.Width) / 2)
                    {
                        if (squad.HasEscort)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Resolves critical effects on ships by bomber squadrons
        /// </summary>
        private void resolveSquadCritical()
        {
            Random rand = new Random();
            int outcome = rand.Next(0, 5);
            switch (outcome)
            {
                case 0:  //life support failure 
                    if(_targetedShip.Commander != null && !(_targetedShip.Commander is CommanderGeneralDodonna))
                    {
                        _targetedShip.HasConcentrateFireToken = false;
                        _targetedShip.HasSquadronToken = false;
                        _targetedShip.HasNavigationToken = false;
                        _targetedShip.HasEngineeringToken = false;
                    }
                    break;
                case 1:  //engine failure
                    _targetedShip.Speed = 0;
                    break;
                case 2: //hull breach
                    _targetedShip.Hull--;
                    break;
                case 3: //exhausted defenses
                    foreach(var def in _targetedShip.DefenseTokens)
                    {
                        if (def.State == DefenseTokenStateEnum.Ready) def.State = DefenseTokenStateEnum.Exhausted;
                    }
                    break;
                case 4: //you get lucky
                    break;
            }
        }

        /// <summary>
        /// Resolves the counter ability on squadrons
        /// </summary>
        /// <returns>The number of hits</returns>
        private int resolveCounter2()
        {
            int hits = 0;
            List<BlueDie> bd = new List<BlueDie>();
            bd.Add(new BlueDie(DieTypeEnum.Blue));
            bd.Add(new BlueDie(DieTypeEnum.Blue));

            foreach(var die in bd)
            {
                BlueDieSideEnum bs = die.Roll();
                if (bs == BlueDieSideEnum.Hit) hits++;
            }

            return hits;
        }

        /// <summary>
        /// The number of squadrons in activation distance
        /// </summary>
        /// <returns>returns the number of nearby squads to the ship.</returns>
        private int numSquadsNearby()
        {
            int squads = 0;

            if(_player1Placing)
            {
                foreach(var squad in _player1.Squadrons)
                {
                    if(Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - squad.Bounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - squad.Bounds.Center.Y, 2)) <= (_shipToSquadRange.Width + squad.Image.Width) / 2)
                    {
                        squads++;
                    }
                }
            }
            else
            {
                foreach (var squad in _player2.Squadrons)
                {
                    if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - squad.Bounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - squad.Bounds.Center.Y, 2)) <= (_shipToSquadRange.Width + squad.Image.Width) / 2)
                    {
                        squads++;
                    }
                }
            }
            return squads;
        }

        /// <summary>
        /// Detects if there is a target in the given firing arc.
        /// </summary>
        /// <param name="arc">Arc to look near</param>
        /// <returns>True if there is an available target</returns>
        private bool areTargetsNearby(int arc)
        {
            if(_player1Placing)
            {
                float center = 0;
                float north = 0;
                float east = 0;
                float south = 0;
                float west = 0;
                float arcAngle = (float)_selectedShip.Arcs[arc].ArcRadians;

                foreach (var squad in _player2.Squadrons)
                {
                    center = angleGen(squad.Bounds.Center);
                    north = angleGen(new Vector2(squad.Bounds.Center.X, squad.Bounds.Center.Y - squad.Bounds.Radius));
                    east = angleGen(new Vector2(squad.Bounds.Center.X + squad.Bounds.Radius, squad.Bounds.Center.Y));
                    south = angleGen(new Vector2(squad.Bounds.Center.X, squad.Bounds.Center.Y + squad.Bounds.Radius));
                    west = angleGen(new Vector2(squad.Bounds.Center.X - squad.Bounds.Radius, squad.Bounds.Center.Y));

                    if(arc == 0)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if(arc == 1)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if(arc == 2)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if(arc == 3)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }

                }

                foreach (var ship in _player2.Ships)
                {
                    if (arc == 0)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 1)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 2)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 3)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                }
            }
            else
            {
                float center = 0;
                float north = 0;
                float east = 0;
                float south = 0;
                float west = 0;
                float arcAngle = (float)_selectedShip.Arcs[arc].ArcRadians;

                foreach (var squad in _player1.Squadrons)
                {
                    center = angleGen(squad.Bounds.Center);
                    north = angleGen(new Vector2(squad.Bounds.Center.X, squad.Bounds.Center.Y - squad.Bounds.Radius));
                    east = angleGen(new Vector2(squad.Bounds.Center.X + squad.Bounds.Radius, squad.Bounds.Center.Y));
                    south = angleGen(new Vector2(squad.Bounds.Center.X, squad.Bounds.Center.Y + squad.Bounds.Radius));
                    west = angleGen(new Vector2(squad.Bounds.Center.X - squad.Bounds.Radius, squad.Bounds.Center.Y));

                    if (arc == 0)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 1)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 2)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 3)
                    {
                        if (Math.Sqrt(Math.Pow(squad.Bounds.Center.X - _selectedShip.Bounds.Center.X, 2) + Math.Pow(squad.Bounds.Center.Y - _selectedShip.Bounds.Center.Y, 2)) < (_shipToSquadRange.Width / 2) + 25)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }

                }

                foreach (var ship in _player1.Ships)
                {
                    if (arc == 0)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.PiOver2 - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.PiOver2 + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 1)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.Pi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= MathHelper.Pi + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 2)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= MathHelper.TwoPi - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                    else if (arc == 3)
                    {
                        center = angleGen(ship.BowBounds.Center);
                        north = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y - ship.BowBounds.Radius));
                        east = angleGen(new Vector2(ship.BowBounds.Center.X + ship.BowBounds.Radius, ship.BowBounds.Center.Y));
                        south = angleGen(new Vector2(ship.BowBounds.Center.X, ship.BowBounds.Center.Y + ship.BowBounds.Radius));
                        west = angleGen(new Vector2(ship.BowBounds.Center.X - ship.BowBounds.Radius, ship.BowBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.BowBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.BowBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.PortBounds.Center);
                        north = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y - ship.PortBounds.Radius));
                        east = angleGen(new Vector2(ship.PortBounds.Center.X + ship.PortBounds.Radius, ship.PortBounds.Center.Y));
                        south = angleGen(new Vector2(ship.PortBounds.Center.X, ship.PortBounds.Center.Y + ship.PortBounds.Radius));
                        west = angleGen(new Vector2(ship.PortBounds.Center.X - ship.PortBounds.Radius, ship.PortBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.PortBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.PortBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.StarboardBounds.Center);
                        north = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y - ship.StarboardBounds.Radius));
                        east = angleGen(new Vector2(ship.StarboardBounds.Center.X + ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));
                        south = angleGen(new Vector2(ship.StarboardBounds.Center.X, ship.StarboardBounds.Center.Y + ship.StarboardBounds.Radius));
                        west = angleGen(new Vector2(ship.StarboardBounds.Center.X - ship.StarboardBounds.Radius, ship.StarboardBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.StarboardBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.StarboardBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }

                        center = angleGen(ship.AftBounds.Center);
                        north = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y - ship.AftBounds.Radius));
                        east = angleGen(new Vector2(ship.AftBounds.Center.X + ship.AftBounds.Radius, ship.AftBounds.Center.Y));
                        south = angleGen(new Vector2(ship.AftBounds.Center.X, ship.AftBounds.Center.Y + ship.AftBounds.Radius));
                        west = angleGen(new Vector2(ship.AftBounds.Center.X - ship.AftBounds.Radius, ship.AftBounds.Center.Y));

                        if (Math.Sqrt(Math.Pow(_selectedShip.Bounds.Center.X - ship.AftBounds.Center.X, 2) + Math.Pow(_selectedShip.Bounds.Center.Y - ship.AftBounds.Center.Y, 2)) < _range.Width)
                        {
                            if (center >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && center <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (north >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && north <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (east >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && east <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (south >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && south <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                            if (west >= (3 * MathHelper.PiOver2) - (arcAngle / 2) + Math.Abs(_selectedShip.Rotation) && west <= (3 * MathHelper.PiOver2) + (arcAngle / 2) + Math.Abs(_selectedShip.Rotation)) return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Generates the angle to the nearest points of an arc
        /// </summary>
        /// <param name="point">The point to check against</param>
        /// <returns>The angle in radians</returns>
        private float angleGen(Vector2 point)
        {
            float xDist = point.X - _selectedShip.Bounds.Center.X;
            float yDist = point.Y - _selectedShip.Bounds.Center.Y;
            float hDist = 0;
            float radians = 0;
            bool xNeg = false;
            bool yNeg = false;
            
            if(xDist < 0)
            {
                xDist = Math.Abs(xDist);
                xNeg = true;
            }
            if(yDist < 0)
            {
                yDist = Math.Abs(yDist);
                yNeg = true;
            }

            hDist = (float)Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));

            radians = (float)Math.Acos((Math.Pow(hDist, 2) + Math.Pow(xDist, 2) - Math.Pow(yDist, 2)) / (2 * hDist * xDist));

            if (xNeg && !yNeg) radians += MathHelper.Pi;
            else if (!xNeg && !yNeg) radians = MathHelper.Pi - radians;
            else if (xNeg && yNeg) radians = MathHelper.TwoPi - radians;
            //Else do nothing you're already here

            return radians;
        }
    }
}
