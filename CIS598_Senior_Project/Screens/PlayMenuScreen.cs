using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project.Screens
{
    public class PlayMenuScreen : MenuScreen
    {
        private Game _game;
        public PlayMenuScreen(Game game) : base("Play Options")
        {
            _game = game;

            var playDuelMenuEntry = new MenuEntry("Hotseat Duel");
            var playCampaignMenuEntry = new MenuEntry("Play Campaign");
            var fleetCreatorMenuEntry = new MenuEntry("Fleet Creator");
            var backMenuEntry = new MenuEntry("Back");

            playDuelMenuEntry.Selected += PlayDuelMenuEntrySelected;
            playCampaignMenuEntry.Selected += PlayCampaignMenuEntrySelected;
            fleetCreatorMenuEntry.Selected += FleetCreatorMenuEntrySelected;
            backMenuEntry.Selected += BackMenuEntrySelected;

            MenuEntries.Add(playDuelMenuEntry);
            MenuEntries.Add(playCampaignMenuEntry);
            MenuEntries.Add(fleetCreatorMenuEntry);
            MenuEntries.Add(backMenuEntry);
            
            foreach(var entry in MenuEntries) Selected.Add(false);
        }

        private void PlayDuelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void PlayCampaignMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void FleetCreatorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new FleetCustomizationMenuScreen(_game), e.PlayerIndex);
        }

        private void BackMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen(_game), e.PlayerIndex);
        }
    }
}
