/* File: ArmadaGame.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CIS598_Senior_Project.Screens;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project
{
    public class ArmadaGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;

        private Song _mainTheme;
        private Song _imperialMarch;
        private Song _cloneWars;
        private Song _cantinaBand;

        /// <summary>
        /// Primary Constructor for the game
        /// </summary>
        public ArmadaGame()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);
            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            _mainTheme = Content.Load<Song>("StarWarsMainTheme");
            _imperialMarch = Content.Load<Song>("StarWarsTheImperialMarch");
            _cloneWars = Content.Load<Song>("StarWarsCloneTheme");
            _cantinaBand = Content.Load<Song>("CantinaBand");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_mainTheme);

            AddInitialScreens();
        }

        /// <summary>
        /// Sets up the initial game screens
        /// </summary>
        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(this, new List<float>() { 0.2f, 0.2f, 0.5f, 0}), null);
        }

        /// <summary>
        /// Initializes the base.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// Loads the content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">the game's time</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
