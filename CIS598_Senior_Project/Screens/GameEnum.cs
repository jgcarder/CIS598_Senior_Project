/* File: GameEnum.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.Screens
{
    /// <summary>
    /// An enum to track the state of the game rounds
    /// </summary>
    public enum GameEnum
    {
        Setup,
        Command_Phase,
        Ship_Phase,
        Squadron_Phase,
        Status_Phase
    }
}
