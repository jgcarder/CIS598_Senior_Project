/* File: ScreenFactory.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.StateManagement;

namespace CIS598_Senior_Project
{
    public class ScreenFactory : IScreenFactory
    {
        /// <summary>
        /// Creates a screen of a certain type
        /// </summary>
        /// <param name="screenType">The screens type</param>
        /// <returns>A game screen</returns>
        public GameScreen CreateScreen(Type screenType)
        {
            // All of our screens have empty constructors so we can just use Activator
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
