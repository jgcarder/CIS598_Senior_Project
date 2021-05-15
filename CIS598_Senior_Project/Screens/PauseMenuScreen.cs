/* File: PauseMenuScreen.cs
 * Author: Jackson Carder
 */


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public class PauseMenuScreen : MenuScreen
    {
        private Game _game;

        /// <summary>
        /// A constructor for a screen I didnt use
        /// </summary>
        /// <param name="game">The game</param>
        public PauseMenuScreen(Game game) : base("Paused")
        {
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        /// <summary>
        /// An event handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">E!!!!</param>
        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        //And is also an event handler, imagine that.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            //LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(_game));
        }

    }
}
