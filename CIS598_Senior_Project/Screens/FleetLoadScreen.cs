using System;
using System.Collections.Generic;
using System.Text;
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

        private ContentManager _content;

        private Game _game;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private List<CustButton> _buttons;

        private Texture2D _background;
        private Texture2D _gradient;

        public FleetLoadScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _buttons = new List<CustButton>();

            _game = game;

            _widthIncrement = _game.GraphicsDevice.Viewport.Width / 100;
            _heightIncrement = _game.GraphicsDevice.Viewport.Height / 100;

            _buttons.Add(new CustButton(0, new Rectangle(_widthIncrement, _game.GraphicsDevice.Viewport.Height - _heightIncrement * 16, 10 * _widthIncrement, 15 * _heightIncrement), true));       //Back Button
            _buttons.Add(new CustButton(1, new Rectangle(_widthIncrement, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));                                              //Edit Fleet
            _buttons.Add(new CustButton(2, new Rectangle(_widthIncrement, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));                                              //Delete Fleet
            _buttons.Add(new CustButton(3, new Rectangle(_widthIncrement * 13, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), true));

            _buttons.Add(new CustButton(4, new Rectangle(_widthIncrement * 13, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(5, new Rectangle(_widthIncrement * 13, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(6, new Rectangle(_widthIncrement * 13, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(7, new Rectangle(_widthIncrement * 13, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(8, new Rectangle(_widthIncrement * 13, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(9, new Rectangle(_widthIncrement * 13, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(10, new Rectangle(_widthIncrement * 37, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(11, new Rectangle(_widthIncrement * 37, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(12, new Rectangle(_widthIncrement * 37, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(13, new Rectangle(_widthIncrement * 37, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(14, new Rectangle(_widthIncrement * 37, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(15, new Rectangle(_widthIncrement * 37, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(16, new Rectangle(_widthIncrement * 61, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(17, new Rectangle(_widthIncrement * 61, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(18, new Rectangle(_widthIncrement * 61, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(19, new Rectangle(_widthIncrement * 61, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(20, new Rectangle(_widthIncrement * 61, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(21, new Rectangle(_widthIncrement * 61, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));

            _buttons.Add(new CustButton(22, new Rectangle(_widthIncrement * 85, _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(23, new Rectangle(_widthIncrement * 85, 18 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(24, new Rectangle(_widthIncrement * 85, 35 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(25, new Rectangle(_widthIncrement * 85, 52 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(26, new Rectangle(_widthIncrement * 85, 69 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
            _buttons.Add(new CustButton(27, new Rectangle(_widthIncrement * 85, 86 * _heightIncrement, 10 * _widthIncrement, 15 * _heightIncrement), false));
        }
    

        public override void Activate()
        {



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
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void ButtonCatcher(object sender, ButtonClickedEventArgs e)
        {

        }
    }
}
