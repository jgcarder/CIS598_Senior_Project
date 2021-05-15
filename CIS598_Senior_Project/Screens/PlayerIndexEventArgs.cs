/* File: PlayerIndexEventArgs.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.Screens
{
    public class PlayerIndexEventArgs : EventArgs
    {

        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        /// Constructor for this event listener
        /// </summary>
        /// <param name="playerIndex">The player controlling it</param>
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }                                   

    }
}
