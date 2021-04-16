/* File: ButtonClickedEventArgs.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.MenuObjects
{
    /// <summary>
    /// A class for registering a button has been clicked on
    /// </summary>
    public class ButtonClickedEventArgs : EventArgs
    {
        /// <summary>
        /// The button's Id number
        /// </summary>
        public int Id { get; set; }
    }
}
