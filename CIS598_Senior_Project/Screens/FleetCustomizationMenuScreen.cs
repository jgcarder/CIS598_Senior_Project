using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public class FleetCustomizationMenuScreen : MenuScreen
    {
        private Game _game;
        public FleetCustomizationMenuScreen(Game game) : base("Fleet Customization")
        {
            _game = game;

            var newFleetMenuEntry = new MenuEntry("New Fleet");
            var editFleetMenuEntry = new MenuEntry("Edit Fleet");
            var backMenuEntry = new MenuEntry("Back");

            newFleetMenuEntry.Selected += NewFleetMenuEntrySelected;
            editFleetMenuEntry.Selected += EditFleetMenuEntrySelected;
            backMenuEntry.Selected += BackMenuEntrySelected;

            MenuEntries.Add(newFleetMenuEntry);
            MenuEntries.Add(editFleetMenuEntry);
            MenuEntries.Add(backMenuEntry);

            foreach (var entry in MenuEntries) Selected.Add(false);
        }

        private void NewFleetMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new FleetCustomizationScreen(_game), e.PlayerIndex);
        }

        private void EditFleetMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

        }

        private void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PlayMenuScreen(_game), e.PlayerIndex);
        }
        
    }
}
